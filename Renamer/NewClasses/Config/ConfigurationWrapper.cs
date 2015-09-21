using System;
using System.Collections.Generic;

namespace Renamer.NewClasses.Config {
	public class ConfigurationWrapper {

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger
	(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private readonly Configuration thisConfiguration;

		private readonly List<string> singleBoolProperties = new List<string>();

		private readonly List<string> singleNumberProperties = new List<string>();

		private readonly List<string> singleStringProperties = new List<string>();

		private readonly List<string> multiBoolProperties = new List<string>();

		private readonly List<string> multiNumberProperties = new List<string>();

		private readonly List<string> multiStringProperties = new List<string>();

		public ConfigurationWrapper(Configuration someConfiguration) {
			if (someConfiguration != null) {
				thisConfiguration = someConfiguration;
			} else {
				throw new ArgumentException("Can't initialize wrapper without configuration.");
			}
			init();
		}

		private void init() {
			foreach (string propertyName in thisConfiguration.getPropertyNames()) {
				if (thisConfiguration.isSingleValued(propertyName)) {
					string value = thisConfiguration.getSingleValueProperty(propertyName);
					checkPropertyType(propertyName, value, true);
					continue;
				}
				if (thisConfiguration.isMultiValued(propertyName)) {
					string value = thisConfiguration.getMultiValueProperty(propertyName)[0];
					checkPropertyType(propertyName, value, false);
				}
			}
		}

		private void checkPropertyType(string propertyName, string value, bool isSingleValued) {
			bool tryBoolValue;
			if (bool.TryParse(value, out tryBoolValue)) {
				if (isSingleValued) {
					singleBoolProperties.Add(propertyName);
				} else {
					multiBoolProperties.Add(propertyName);				
				}
				
			}
			double tryDoubleValue;
			if (double.TryParse(value, out tryDoubleValue)) {
				if (isSingleValued) {
					singleNumberProperties.Add(propertyName);
				} else {
					multiNumberProperties.Add(propertyName);
				}
			}
			if (isSingleValued) {
				singleStringProperties.Add(propertyName);
			} else {
				multiStringProperties.Add(propertyName);
			}
		}

		public bool getSingleBoolProperty(string propertyName) {
			if (!singleBoolProperties.Contains(propertyName)) {
				throw new ArgumentException("Asked for a single boolean property value, but none there: " + propertyName);
			}
			return bool.Parse(thisConfiguration.getSingleValueProperty(propertyName));
		}
		public List<bool> getMultiBoolProperty(string propertyName) {
			if (!multiBoolProperties.Contains(propertyName)) {
				throw new ArgumentException("Asked for a multi boolean property value, but none there: " + propertyName);
			}
			List<string> values = thisConfiguration.getMultiValueProperty(propertyName);
			return values.ConvertAll(new Converter<string, bool>(bool.Parse));
		}

		public double getSingleNumberProperty(string propertyName) {
			if (!singleNumberProperties.Contains(propertyName)) {
				throw new ArgumentException("Asked for a single number property value, but none there: " + propertyName);
			}
			double output;
			double.TryParse(thisConfiguration.getSingleValueProperty(propertyName), out output);
			return output;
		}
		public List<double> getMultiNumberProperty(string propertyName) {
			if (!multiNumberProperties.Contains(propertyName)) {
				throw new ArgumentException("Asked for a multi number property value, but none there: " + propertyName);
			}
			List<string> values = thisConfiguration.getMultiValueProperty(propertyName);
			return values.ConvertAll(convertToDouble);
		}

		public string getSingleStringProperty(string propertyName) {
			if (!singleStringProperties.Contains(propertyName)) {
				throw new ArgumentException("Asked for a single string property value, but none there: " + propertyName);
			}
			return thisConfiguration.getSingleValueProperty(propertyName);
		}
		public List<string> getMultiStringProperty(string propertyName) {
			if (!multiStringProperties.Contains(propertyName)) {
				throw new ArgumentException("Asked for a multi string property value, but none there: " + propertyName);
			}
			return thisConfiguration.getMultiValueProperty(propertyName);
		}

		public void set(string propertyName, object newValue) {
			thisConfiguration[propertyName] = newValue;
		}

		/// <summary>
		/// Read a enum value from the Configuration
		/// </summary>
		/// <typeparam name="T">Type of expected enum value</typeparam>
		/// <param name="propertyName">Identifier the enum value is stored</param>
		/// <returns></returns>
		public T getEnumProperty<T>(string propertyName) {
			string result = null;
			try {
				result = getSingleStringProperty(propertyName);
				return (T)Enum.Parse(typeof(T), result);
			} catch {
				log.Error(
					"Couldn't parse property to Enum<" + typeof(T) + "> " 
					+ propertyName + " = " + result);
				return default(T);
			}
		}

		public List<string> getPropertyNames() {
			return thisConfiguration.getPropertyNames();
		}

		public bool isSingleValueProperty(string propertyName) {
			return thisConfiguration.isSingleValued(propertyName);
		}

		public bool isMultiValueProperty(string propertyName) {
			return thisConfiguration.isMultiValued(propertyName);
		}

		private static double convertToDouble(string input) {
			double output;
			if (double.TryParse(input, out output)) {
				return output;
			}
			return double.NaN;
		}
	}
}
