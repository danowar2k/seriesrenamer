using System.Collections.Generic;
using Renamer.NewClasses.Config.Application;

namespace Renamer.NewClasses.Config
{
    /// <summary>
    /// Configuration file used as cache
    /// </summary>
    public class Configuration
    {
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger
	(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
        /// File extension of config files
        /// </summary>
        public const string CONFIG_FILE_EXTENSION_PATTERN = "*.cfg";

        public const string DELIMITER = "^";
		/// <summary>
		/// Comment string used in config file
		/// </summary>
		public const string COMMENT = "#";

		public const string BEGIN_MULTI_VALUE_PROPERTY = "{";

		public const string END_MULTI_VALUE_FIELD = "}";

		public const string KEY_VALUE_SEPARATOR = "=";

        /// <summary>
        /// Path of the configuration file, is empty for internal defaults
        /// </summary>
        private string configFilePath = "";

        /// <summary>
        /// Hashtable containing configuration properties accessible by their names<br/>
        /// Its values are either a String or a list of Strings
        /// </summary>
		private readonly Dictionary<string, string> singleValueProperties = new Dictionary<string, string>();
		private readonly Dictionary<string, List<string>> multiValueProperties = new Dictionary<string, List<string>>();

		/// <summary>
        /// If true, the configuration has changed and needs to be saved to disk
        /// </summary>
        private bool configurationChanged;

        /// <summary>
        /// Check whether a property is set in this configuration
        /// </summary>
        /// <param name="propertyName">Name of the property the config should be checked for.</param>
        /// <returns>True if property has been set, false otherwise.</returns>
        public bool hasProperty(string propertyName) {
			if (singleValueProperties.ContainsKey(propertyName)) {
				return true;
			}
			return multiValueProperties.ContainsKey(propertyName);
        }

		public bool isSingleValued(string propertyName) {
			return singleValueProperties.ContainsKey(propertyName);
		}

	    public bool isMultiValued(string propertyName) {
		    return multiValueProperties.ContainsKey(propertyName);
	    }

	    public string getSingleValueProperty(string propertyName) {
		    return isSingleValued(propertyName) ? singleValueProperties[propertyName] : string.Empty;
	    }

	    public List<string> getMultiValueProperty(string propertyName) {
			List<string> values = new List<string>();
			if (!isMultiValued(propertyName)) {
				return values;
			}
			values.AddRange(multiValueProperties[propertyName]);
			return values;
		}

        /// <summary>
        /// Adds a property to the configuration.
        /// <b>It's preferred to use the index operator to add variables</b>
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">The value of the property</param>
        private void addProperty(string propertyName, string value) {
			singleValueProperties.Add(propertyName, value);
            configurationChanged = true;
        }

		private void addProperty(string propertyName, List<string> values) {
			List<string> configurationValues = new List<string>(values);
			multiValueProperties.Add(propertyName, configurationValues);
			configurationChanged = true;
		}

        /// <summary>
        /// Assigns a new value to property and 
        /// mark this configuration for flushing
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
        private void changeProperty(string propertyName, string newValue) {
	        if (singleValueProperties[propertyName].Equals(newValue)) {
		        return;
	        }
	        singleValueProperties[propertyName] = newValue;
            configurationChanged = true;
        }

		private void changeProperty(string propertyName, List<string> newValues) {
			List<string> oldValues = multiValueProperties[propertyName];
			if (oldValues.Equals(newValues)) {
				return;
			}
			oldValues.Clear();
			oldValues.AddRange(newValues);
			configurationChanged = true;
		}
        /// <summary>
        /// IndexOperator for accessing configuration property directly with their names
        /// </summary>
        /// <param name="propertyName">Key of the variable.</param>
        /// <returns>The variable</returns>
        public object this[string propertyName] {
            get {
                if (hasProperty(propertyName)) {
					if (singleValueProperties.ContainsKey(propertyName)) {
						return singleValueProperties[propertyName];
					}
					if (multiValueProperties.ContainsKey(propertyName)) {
						return multiValueProperties[propertyName];
					}
                }
	            object defaultValue = AppDefaults.getDefaultValue(propertyName);
	            if (defaultValue != null) {
		            this[propertyName] = AppDefaults.getDefaultValue(propertyName);
		            return this[propertyName];
	            }
	            log.Error("Couldn't find property " + propertyName + " in " + configFilePath);
		        return null;
            }
            set {
                if (hasProperty(propertyName)) {
	                var s = value as string;
	                if (s != null) {
						changeProperty(propertyName, s);
					} else if (value is List<string>) {
						changeProperty(propertyName, (List<string>) value);
					}
                } else {
	                var s = value as string;
	                if (s != null) {
						addProperty(propertyName, s);
					} else if (value is List<string>) {
						addProperty(propertyName, (List<string>)value);
					}
                }
            }
        }

	    /// <summary>
	    /// Number of configured properties
	    /// </summary>
	    public int countProperties() {
		    return singleValueProperties.Count + multiValueProperties.Count;
	    }


		/// <summary>
		/// Returns a list of all property names in this configuration.
		/// </summary>
		/// <returns>the list of names of configured properties</returns>
		public List<string> getPropertyNames() {
			List<string> propertyNames = new List<string>();
			propertyNames.AddRange(singleValueProperties.Keys);
			propertyNames.AddRange(multiValueProperties.Keys);
			return propertyNames;
		} 
    }

}
