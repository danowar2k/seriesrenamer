using System;
using Renamer.NewClasses.Config.Application;

namespace Renamer.NewClasses.Config
{
    /// <summary>
    /// Wrapping user configuration and auto-detected system-specific settings
    /// This class is a Singleton, only one instance is available
    /// </summary>
    class Settings
    {

        private static Settings instance;

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger
	(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

	    private AppConfigurationWrapper appConfiguration;

        /// <summary>
        /// Mono compatibility mode
        /// </summary>
        private readonly bool monoCompatibilityMode;

        /// <summary>
        /// Get the instance of Settings, create the settings, if required
        /// </summary>
        /// <returns>actual Settings</returns>
        public static Settings Instance {
            get { return instance ?? (instance = new Settings()); }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        protected Settings() {
            monoCompatibilityMode = inMonoCompatibilityMode();
	        appConfiguration = ConfigurationManager.readAppConfiguration();
        }

		public AppConfigurationWrapper getAppConfiguration() {
			return appConfiguration;
		}

        public bool isMonoCompatibilityMode() {
            return monoCompatibilityMode;
        }

        private static bool inMonoCompatibilityMode() {
	        return Type.GetType("Mono.Runtime") != null;
        }

    }
}