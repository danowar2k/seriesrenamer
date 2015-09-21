using System.ComponentModel;
using System.IO;
using Renamer.NewClasses.Providers.Strategies;
using Renamer.NewClasses.Config;
using Renamer.NewClasses.Config.Properties;

namespace Renamer.NewClasses.Providers
{
    /// <summary>
    /// An abstract provider of something
    /// </summary>
    public abstract class AbstractProvider
    {
        /// <summary>
        /// <see cref="name"/>
        /// </summary>
        private string name;
		/// <summary>
		/// The path fo the configuration used for this provider.
		/// </summary>
        public string configurationFilePath;
		/// <summary>
		/// Representation of the current provider configuration
		/// </summary>
	    protected ConfigurationWrapper providerConfiguration;
		/// <summary>
		/// The strategy used to search for the things provided by the provider.
		/// </summary>
		private SearchStrategy searchStrategy;

		protected AbstractProvider(string someConfigurationFilePath) {
			setConfiguration(someConfigurationFilePath);
			setSearchStrategy(getDefaultStrategy());
		}

        protected AbstractProvider(string someConfigurationFilePath, SearchStrategy someSearchStrategy) {
			setConfiguration(someConfigurationFilePath);
	        setSearchStrategy(someSearchStrategy ?? getDefaultStrategy());
        }

		public void setConfiguration(string newConfigurationPath) {
			if (string.IsNullOrEmpty(newConfigurationPath)) {
				return;
			}
			configurationFilePath = newConfigurationPath;
			providerConfiguration = ConfigurationManager.readConfiguration(configurationFilePath);
			setName(providerConfiguration.getSingleStringProperty(ProviderProperties.PROVIDER_NAME_KEY));
		}
		public void setSearchStrategy(SearchStrategy newStrategy) {
			if (newStrategy != null) {
				searchStrategy = newStrategy;
			}
		}

		private SearchStrategy getDefaultStrategy() {
			return new ParseHTMLStrategy(this, providerConfiguration);
		}

		/// <summary>
		/// get the name of the provider
		/// </summary>
		public string getName() {
			return name;
		}

		/// <summary>
		/// Set the name of the provider
		/// </summary>
		/// <param name="newName">the new name</param>
		public void setName(string newName) {
			if (!string.IsNullOrEmpty(newName)) {
				name = newName;
			}
		}

		public ConfigurationWrapper getProviderConfiguration() {
			return providerConfiguration;
		}
    }
}
