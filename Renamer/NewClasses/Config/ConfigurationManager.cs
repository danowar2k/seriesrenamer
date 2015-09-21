using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using Renamer.NewClasses.Config.Application;
using Renamer.NewClasses.Util;

namespace Renamer.NewClasses.Config {
	/// <summary>
	/// Manages the creation and storage of Configurations
	/// </summary>
	public class ConfigurationManager {

		/// <summary>
		/// Quotes to look for and remove in Strings
		/// </summary>
		public const string QUOTES = "\"";

		public const string APP_CONFIG_FILENAME = "Renamer.cfg";

		public const string APPLICATION_CONFIG_FOLDER = "Application";

		public const string CONFIGURATION_DEFAULTS_FILENAME = "Defaults.cfg";

		public static readonly string DEFAULTS_PATH =
			Path.DirectorySeparatorChar + APPLICATION_CONFIG_FOLDER +
			Path.DirectorySeparatorChar + CONFIGURATION_DEFAULTS_FILENAME;

		public static readonly string APP_CONFIG_PATH =
			AppConstants.APP_BASE_DIR + Path.DirectorySeparatorChar + APP_CONFIG_FILENAME;

		private static readonly ILog log = LogManager.GetLogger
	(MethodBase.GetCurrentMethod().DeclaringType);

		public static AppConfigurationWrapper readAppConfiguration() {
			return new AppConfigurationWrapper(readConfiguration(APP_CONFIG_PATH));
		}

		public static void writeAppConfiguration(AppConfigurationWrapper theConfiguration) {
			writeConfiguration(theConfiguration.getWrappedConfiguration(), APP_CONFIG_PATH);
		}

		/// <summary>
		/// Writes the <see cref="someConfiguration"/> to the given path.
		/// </summary>
		/// <param name="someConfiguration">the configuration to save to disk</param>
		/// <param name="filepath">Path where config file should be stored to.</param>
		public static void writeConfiguration(ConfigurationWrapper someConfiguration, string filepath) {
			if (someConfiguration == null) {
				return;
			}
			StreamWriter fileWriter = null;
			string tempfilepath = filepath + AppConstants.TEMPFILE_EXTENSION;
			try {
				FileStream writeStream = File.Open(tempfilepath, FileMode.Create, FileAccess.ReadWrite);
				fileWriter = new StreamWriter(writeStream);
				fileWriter.AutoFlush = true;
				writePropertyComment(fileWriter, AppPropertyComments.INITIAL_COMMENT_KEY);
				foreach (string propertyName in someConfiguration.getPropertyNames()) {
					if (someConfiguration.isSingleValueProperty(propertyName)) {
						string value = someConfiguration.getSingleStringProperty(propertyName);
						writeProperty(fileWriter, propertyName, value);
						continue;

					}
					if (someConfiguration.isMultiValueProperty(propertyName)) {
						List<string> values = someConfiguration.getMultiStringProperty(propertyName);
						writeMultiValueProperty(fileWriter, propertyName, values);
						continue;
					}
					log.Warn("Configuration property " + propertyName + " not written - non-valid value");
				}
				if (File.Exists(filepath)) {
					File.Delete(filepath);
				}
				fileWriter.Close();
				File.Move(tempfilepath, filepath);
			} catch (Exception ex) {
				log.Error("Couldn't write configuration to " + filepath + "\nError:\n" + ex.Message);
			} finally {
				if (fileWriter != null) {
					fileWriter.Close();
					File.Delete(tempfilepath);
				}
			}
		}

		/// <summary>
		/// Parse the given configuration file and stores the data to the config
		/// </summary>
		/// <param name="configFilepath">the path to the config file to read</param>
		public static ConfigurationWrapper readConfiguration(string configFilepath) {

			FileStream configFileStream = null;
			StreamReader configFileReader = null;
			Configuration someConfiguration = new Configuration();
			try {
				ParserMode mode = ParserMode.Normal;
				string currentLine;
				int lineCounter = 0;
				string currentPropertyName = null;
				List<string> multipleValues = new List<string>();

				configFileStream = File.Open(configFilepath, FileMode.OpenOrCreate, FileAccess.Read);
				configFileReader = new StreamReader(configFileStream);

				while ((currentLine = configFileReader.ReadLine()) != null) {
					lineCounter++;

					currentLine = cleanupLine(currentLine);

					if (String.IsNullOrEmpty(currentLine) || isComment(currentLine)) {
						continue;
					}

					switch (mode) {
						case ParserMode.MultiValueProperty:
							if (currentLine.Equals(Configuration.END_MULTI_VALUE_FIELD)) {
								someConfiguration[currentPropertyName] = multipleValues;
								mode = ParserMode.Normal;
							} else {
								multipleValues.Add(currentLine);
							}
							continue;
						case ParserMode.Normal:
							int keyValueSeparatorIndex = 
								currentLine.IndexOf(Configuration.KEY_VALUE_SEPARATOR, StringComparison.Ordinal);
							if (keyValueSeparatorIndex <= 0) {
								logCorruptLine(lineCounter);
								continue;
							}
							currentPropertyName = currentLine.Substring(0, keyValueSeparatorIndex).Trim();
							string valuePart = currentLine.Substring(keyValueSeparatorIndex + 1).Trim();

							if (String.IsNullOrEmpty(valuePart)) {
								log.Debug("Property " + currentPropertyName + " is not set");
								continue;
							}
							if (valuePart.Equals(Configuration.BEGIN_MULTI_VALUE_PROPERTY)) {
								mode = ParserMode.MultiValueProperty;
								multipleValues.Clear();
							} else {
								someConfiguration[currentPropertyName] = valuePart;
							}
							break;
						default:
							continue;
					}

				}

			} catch (Exception ex) {
				AppConfigurationWrapper appConfiguration = AppDefaults.getDefaultConfiguration();
				writeConfiguration(appConfiguration, configFilepath);
				log.Error("Couldn't process config file " + configFilepath + ":" + ex.Message);
				log.Warn("Generating configuration file using default values.");
			} finally {
				configFileStream?.Close();
				configFileReader?.Close();
			}
			return new ConfigurationWrapper(someConfiguration);
		}

		/// <summary>
		/// Writes variable with the <see cref="propertyName"/> to the <see cref="fileWriter"/>
		/// </summary>
		private static void writeProperty(TextWriter fileWriter, string propertyName, string propertyValue) {
			writePropertyComment(fileWriter, propertyName);
			string line = propertyName + Configuration.KEY_VALUE_SEPARATOR + propertyValue;
			fileWriter.WriteLine(line);
			fileWriter.WriteLine("");
		}

		private static void writePropertyComment(TextWriter fileWriter, string propertyName) {
			List<string> comments = AppPropertyComments.getComments(propertyName);
			foreach (string comment in comments) {
				fileWriter.WriteLine(comment);
			}
		}

		private static void writeMultiValueProperty(TextWriter fileWriter, string propertyName, IEnumerable<string> values) {
			writePropertyComment(fileWriter, propertyName);
			string firstLine = 
				propertyName + Configuration.KEY_VALUE_SEPARATOR + Configuration.BEGIN_MULTI_VALUE_PROPERTY;
			fileWriter.WriteLine(firstLine);
			foreach (string valueLine in values.Select(quote)) {
				fileWriter.WriteLine(valueLine);
			}
			fileWriter.WriteLine(Configuration.END_MULTI_VALUE_FIELD);
			fileWriter.WriteLine("");
		}

		private static string quote(string someString) {
			if (someString.Contains(" ")) {
				someString = QUOTES + someString + QUOTES;
			}
			return someString;
		}

		/// <summary>
		/// Removes trailing whitespace and enclosing quotes from a string
		/// </summary>
		/// <param name="someLine">the line to clean up</param>
		/// <returns>the clean line</returns>
		private static string cleanupLine(string someLine) {
			someLine = someLine.Trim();

			if (someLine.StartsWith(QUOTES) && someLine.EndsWith(QUOTES)) {
				someLine = someLine.Substring(1, someLine.Length - 2);
			}
			return someLine;
		}

		/// <summary>
		/// Check if the current line is a comment.
		/// </summary>
		/// <param name="someLine">the line to check</param>
		/// <returns>true if the line is a comment</returns>
		private static bool isComment(string someLine) {
			if (someLine.StartsWith(Configuration.COMMENT)) {
				return true;
			}
			return false;
		}

		/// <summary>
		/// Logs when a line inside the read config file makes no sense.
		/// </summary>
		/// <param name="lineNumber">the line number of the corrupt line</param>
		private static void logCorruptLine(int lineNumber) {
			log.Warn("The configfile seems to be corrupt (at line " + lineNumber + "), I'll try to ignore this");		
		}

	}
}