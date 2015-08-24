#region SVN Info
/***************************************************************
 * $Author$
 * $Revision$
 * $Date$
 * $LastChangedBy$
 * $LastChangedDate$
 * $URL$
 * 
 * License: GPLv3
 * 
****************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Diagnostics;
using Renamer.Logging;
using System.Text.RegularExpressions;

namespace Renamer.Classes.Configuration
{
    /// <summary>
    /// ConfigKeyConstants file used as cache
    /// </summary>
    class ConfigFile : IEnumerable
    {
        /// <summary>
        /// File extension of config files
        /// </summary>
        public static string configFileExtensionPattern = "*.cfg";

        public static string delimiter = "^";

        /// <summary>
        /// Path of the configuration file, is empty for internal defaults
        /// </summary>
        private string configFilePath = "";

        /// <summary>
        /// Hashtable containing configuration properties accessible by their names<br/>
        /// Its values are either a String or a list of Strings
        /// </summary>
        private Hashtable configurationProperties = new Hashtable();

        /// <summary>
        /// If set, cache has changed and file needs to be saved
        /// </summary>
        private bool needsFlush = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="someConfigFilePath">Path of the file, for manual file creation, leave empty here</param>
        public ConfigFile(string someConfigFilePath) {
            configFilePath = someConfigFilePath;
            if (someConfigFilePath == null || someConfigFilePath == "") {
                return;
            }
            ConfigFileParser parser = new ConfigFileParser(this);
            parser.readConfigFile(someConfigFilePath);
        }

        /// <summary>
        /// Saves the file
        /// </summary>
        public void Flush() {
            if (needsFlush) {
                this.cleanupConfigurationProperties();
                ConfigFileWriter configWriter = new ConfigFileWriter(this);
                configWriter.writeConfigFile(configFilePath);
                needsFlush = false;
            }
        }

        /// <summary>
        /// Check whether a property is set in this configuration
        /// </summary>
        /// <param name="propertyName">Name of the property the config should be checked for.</param>
        /// <returns>True if property has been set, false otherwise.</returns>
        public bool containsProperty(string propertyName) {
            return this.configurationProperties.ContainsKey(propertyName);
        }

        /// <summary>
        /// Adds a property to the configuration.
        /// <b>It's preferred to use the index operator to add variables</b>
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">The value of the property</param>
        private void addProperty(string propertyName, object value) {
            this.configurationProperties.Add(propertyName, value);
            this.needsFlush = true;
        }

        /// <summary>
        /// Assigns a new value to an already existing configuration and 
        /// check if a flush will be needed after performing this operation
        /// </summary>
        /// <param name="configKey"></param>
        /// <param name="newValue"></param>
        private void changeConfiguration(string configKey, object newValue) {
            if (this.configurationProperties[configKey] != newValue) {
                this.configurationProperties[configKey] = newValue;
                this.needsFlush = true;
            }
        }

        /// <summary>
        /// IndexOperator for accessing configuration property directly with their names
        /// </summary>
        /// <param name="propertyName">Key of the variable.</param>
        /// <returns>The variable</returns>
        public object this[string propertyName] {
            get {
                Settings settings = Settings.Instance;
                if (this.containsProperty(propertyName)) {
                    return this.configurationProperties[propertyName];
                }
                if (settings.Defaults.containsProperty(propertyName)) {
                    this[propertyName] = settings.Defaults[propertyName];
                    return this[propertyName];
                }
                else {
                    Logger.Instance.LogMessage("Couldn't find property " + propertyName + " in " + configFilePath, LogLevel.ERROR);
                    return null;
                }
            }
            set {

                if (value is List<string>) {
                    value = ((List<string>)value).ToArray();
                }

                if (this.containsProperty(propertyName)) {
                    this.changeConfiguration(propertyName, value);
                }
                else {

                    this.addProperty(propertyName, value);
                }
            }
        }

        /// <summary>
        /// Loads default settings to this config file
        /// </summary>
        public void LoadDefaults() {
            Settings settings = Settings.Instance;
            this.configurationProperties = (Hashtable)settings.Defaults.configurationProperties.Clone();
        }

        /// <summary>
        /// Converts lists of strings to arrays to get consistent types
        /// </summary>
        private void cleanupConfigurationProperties() {
            object[] keys = new object[this.configurationProperties.Keys.Count];
            this.configurationProperties.Keys.CopyTo(keys, 0);
            foreach (object key in keys) {
                if (this.configurationProperties[key] is List<string>) {                   
                    Debug.WriteLine(this.configurationProperties);
                    this.configurationProperties[key] = ((List<string>)this.configurationProperties[key]).ToArray();
                }
            }
        }

        /// <summary>
        /// Number of configured properties
        /// </summary>
        public int countProperties {
            get {
                return this.configurationProperties.Count;
            }
        }

        #region IEnumerable Member

        public IEnumerator GetEnumerator() {
            return this.configurationProperties.Keys.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Parser for configurationfiles
        /// </summary>
        private class ConfigFileParser
        {
            private enum ParserState : int
            {
                Error,
                Comment,
                MultiValueField,
                NormalLine
            };

            private ParserState prevState;
            private Settings settings;
            private string currentLine;
            private int lineCounter;
            private string currentPropertyName;
            private List<string> currentValues;
            private ConfigFile config;

            /// <summary>
            /// Creates a new ConfigFileParser object for a given config
            /// </summary>
            /// <param name="config">ConfigFile the parser should store the data of the config file.</param>
            public ConfigFileParser(ConfigFile config)
            {
                this.settings = Settings.Instance;
                currentLine = null;
                lineCounter = 0;
                this.config = config;
            }

            /// <summary>
            /// Parse the given configuration file and stores the data to the config
            /// </summary>
            /// <param name="filepath"></param>
            public void readConfigFile(string filepath)
            {
                prevState = ParserState.NormalLine;
                FileStream s = null;
                StreamReader r = null;
                try
                {
                    s = File.Open(filepath, FileMode.OpenOrCreate, FileAccess.Read);
                    r = new StreamReader(s);
                    while ((currentLine = r.ReadLine()) != null)
                    {
                        //Remove leading and trailing whitespaces
                        currentLine = currentLine.Trim();

                        //space escape sequence
                        if (currentLine.StartsWith("\"") && currentLine.EndsWith("\""))
                        {
                            currentLine = currentLine.Substring(1, currentLine.Length - 2);
                        }

                        lineCounter++;
                        // Skip empty lines
                        if (String.IsNullOrEmpty(currentLine))
                        {
                            continue;
                        }
                        // ignoring comments in MultiValueFields
                        if (prevState != ParserState.MultiValueField && currentLine.StartsWith(Settings.COMMENT))
                        {
                            prevState = ParserState.Comment;
                            continue;
                        }

                        // remove comments at the end of a line
                        if (prevState != ParserState.MultiValueField)
                        {
                            int keyValueSplitIndex = currentLine.IndexOf("=");
                            if (keyValueSplitIndex > 0)
                            {
                                // extract key from line
                                currentPropertyName = currentLine.Substring(0, keyValueSplitIndex).Trim();

                                string valuePart = currentLine.Substring(keyValueSplitIndex + 1).Trim();
                                if (String.IsNullOrEmpty(valuePart))
                                {
                                    //Logger.Instance.LogMessage("Option " + currentPropertyName + " is empty", LogLevel.DEBUG);
                                }
                                // continue if a multivalue field is found
                                if (valuePart == Settings.BEGIN_MULTI_VALUE_FIELD)
                                {
                                    this.prevState = ParserState.MultiValueField;
                                    this.currentValues = new List<string>();
                                    continue;
                                }

                                config[currentPropertyName] = valuePart;
                                this.prevState = ParserState.NormalLine;
                            }
                            else
                            {
                                //Logger.Instance.LogMessage("The configfile seems to be corrupt (at line " + lineCounter + "), I'll try to ignore this", LogLevel.WARNING);
                            }
                        }
                        else /* Multi Value Field */
                        {
                            if (currentLine == Settings.END_MULTI_VALUE_FIELD)
                            {
                                config[currentPropertyName] = currentValues;
                                this.prevState = ParserState.NormalLine;
                                continue;
                            }
                            currentValues.Add(currentLine);
                        }

                    }
                }
                catch (Exception ex)
                {
                    config.LoadDefaults();
                    config.Flush();
                    Logger.Instance.LogMessage("Couldn't process config file " + filepath + ":" + ex.Message, LogLevel.ERROR);
                    Logger.Instance.LogMessage("Using default values", LogLevel.WARNING);
                }
                finally
                {
                    if (s != null)
                    {
                        s.Close();
                    }
                    if (r != null)
                    {
                        r.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Stores a ConfigFile in a File
        /// </summary>
        private class ConfigFileWriter
        {

            private enum ParserState : int
            {
                Error,
                Comment,
                MultiValueField,
                NormalLine
            };

            private ParserState prevState;
            private Settings settings;
            private string currentLine;
            private int lineCounter;
            private string currentPropertyName;
            private ConfigFile config;
            private List<string> writtenProperties;
            StreamWriter fileWriter;

            /// <summary>
            /// Creates a new ConfigFileWriter object for a given config
            /// </summary>
            /// <param name="config">ConfigFile the writer should read the data from.</param>
            public ConfigFileWriter(ConfigFile config)
            {
                this.settings = Settings.Instance;
                this.currentLine = null;
                this.lineCounter = 0;
                this.config = config;
                this.writtenProperties = new List<string>(config.countProperties);
                this.fileWriter = null;
            }


            /// <summary>
            /// Writes the <see cref="config"/> to the given path.
            /// </summary>
            /// <param name="filepath">Path where config file should be stored to.</param>
            public void writeConfigFile(string filepath)
            {
                prevState = ParserState.NormalLine;
                FileStream writeStream = null;
                try
                {
                    FileStream fileStream = File.Open(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    StreamReader fileReader = new StreamReader(fileStream);
                    writeStream = File.Open(filepath + ".tmp", FileMode.Create, FileAccess.ReadWrite);
                    fileWriter = new StreamWriter(writeStream);
                    fileWriter.AutoFlush = true;
                    while ((currentLine = fileReader.ReadLine()) != null)
                    {
                        //Remove leading and trailing whitespaces
                        currentLine = currentLine.Trim();
                        lineCounter++;
                        // Skip empty lines
                        if (String.IsNullOrEmpty(currentLine))
                        {
                            continue;
                        }
                        // redirect comments
                        if (prevState != ParserState.MultiValueField && currentLine.StartsWith(Settings.COMMENT))
                        {
                            fileWriter.WriteLine(currentLine);
                            prevState = ParserState.Comment;
                            continue;
                        }

                        currentLine = this.removeTrailingComment(currentLine);

                        if (prevState != ParserState.MultiValueField)
                        {
                            int keyValueSplitIndex = currentLine.IndexOf("=");
                            if (keyValueSplitIndex > 0)
                            {
                                // extract key from line
                                currentPropertyName = currentLine.Substring(0, keyValueSplitIndex).Trim();
                                writeProperty();
                            }
                        }
                        else /* Multi Value Field */
                        {
                            if (currentLine == Settings.END_MULTI_VALUE_FIELD)
                            {
                                this.prevState = ParserState.NormalLine;
                            }
                        }
                    }
                    // check for unwritten properties, added to the config
                    // TODO: maybe replace with non linear search for better performance
                    if (this.writtenProperties.Count < this.config.countProperties)
                    {
                        foreach (string key in this.config)
                        {
                            if (!this.writtenProperties.Contains(key))
                            {
                                currentPropertyName = key;
                                writeProperty();
                            }
                        }
                    }

                    fileStream.Close();
                    File.Delete(filepath);
                    writeStream.Close();
                    File.Move(filepath + ".tmp", filepath);
                }
                catch (Exception ex)
                {
                    Logger.Instance.LogMessage("Couldn't write config file " + filepath + "\nFehler:\n" + ex.Message, LogLevel.ERROR);
                }
                finally
                {
                    if (writeStream != null)
                    {
                        writeStream.Close();
                        File.Delete(filepath + ".tmp");
                    }
                }
            }

            /// <summary>
            /// Writes variable with the <see cref="currentPropertyName"/> to the <see cref="fileWriter"/>
            /// </summary>
            private void writeProperty()
            {
                currentLine = currentPropertyName + "=";
                if (config.containsProperty(currentPropertyName))
                {

                    this.writtenProperties.Add(currentPropertyName);

                    if (config[currentPropertyName] is string[])
                    {
                        this.prevState = ParserState.MultiValueField;
                        currentLine += Settings.BEGIN_MULTI_VALUE_FIELD;
                        fileWriter.WriteLine(currentLine);

                        //recoverEasyRegex((string[])config[currentKey]));
                        foreach (string val in ((string[])config[currentPropertyName]))
                        {
                            fileWriter.WriteLine("\t" + Escape(val));
                        }
                        currentLine = Settings.END_MULTI_VALUE_FIELD;
                    }
                    else
                    {
                        currentLine += Escape((string)config[currentPropertyName]);
                        this.prevState = ParserState.NormalLine;
                    }
                }

                fileWriter.WriteLine(currentLine);
                fileWriter.WriteLine();
            }

            /// <summary>
            /// removes trailing comments on a line
            /// </summary>
            /// <returns>line without comments</returns>
            private string removeTrailingComment(string someLine)
            {
                int commentStart = currentLine.IndexOf(Settings.COMMENT);
                if (commentStart == -1)
                {
                    return currentLine;
                }
                return currentLine.Substring(0, currentLine.IndexOf(Settings.COMMENT));
            }

            private string Escape(string v)
            {
                if (v.StartsWith(" ") || v.EndsWith(" "))
                {
                    v = "\"" + v + "\"";
                }
                return v;
            }
        }

    }

}
