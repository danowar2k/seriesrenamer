using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Renamer.NewClasses.Config;
using Renamer.NewClasses.Config.Application;
using Renamer.NewClasses.Config.Providers;
using Renamer.NewClasses.Util;

namespace Renamer.NewClasses.Providers {
	internal class TvShowInfoProviderManager {

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger
			(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private static readonly string TVSHOWINFOPROVIDER_CONFIGURATION_FOLDER = 
			Path.DirectorySeparatorChar + "Databases" + Path.DirectorySeparatorChar + "TvShows";

		/// <summary>
		/// The list of available tv show info providers.
		/// </summary>
		private static List<TvShowInfoProvider> tvShowInfoProviders = new List<TvShowInfoProvider>();

		/// <summary>
		/// Constructor that prepares all tv show info providers.
		/// </summary>
		public TvShowInfoProviderManager() {
			init();
		}

		private static void init() {
			if (tvShowInfoProviders.Count == 0) {
				loadAllValidTvShowInfoProviders();
			}
		}

		public List<TvShowInfoProvider> getTvShowInfoProviders() {
			return tvShowInfoProviders;
		}

		/// <summary>
		/// Checks the app subfolder for tv show provider configuration files and populates the list with valid configurations
		/// </summary>
		public static void loadAllValidTvShowInfoProviders() {
			StringBuilder status = new StringBuilder();
			status.AppendLine("Providers found:");
			foreach (string configFileName in getConfigurationFileNames(TVSHOWINFOPROVIDER_CONFIGURATION_FOLDER)) {
				log.Debug("Provider: " + configFileName);
				try {
					TvShowInfoProvider newTvShowInfoProvider = new TvShowInfoProvider(configFileName);
					if (!isValid(newTvShowInfoProvider)) {
						log.Error("Invalid provider file: " + configFileName);
						delete(newTvShowInfoProvider);
						continue;
					}
					status.AppendLine(newTvShowInfoProvider.getName());
				} catch (Exception ex) {
					log.Error("Invalid provider file: " + configFileName + " Error: " + ex.Message);
				}
			}
			log.Info(status.ToString());
		}

		public static void saveQueryResults() {
			foreach (TvShowInfoProvider tvShowInfoProvider in tvShowInfoProviders) {
				List<string> queryResults = new List<string>();
				foreach (KeyValuePair<string, string> kvp in tvShowInfoProvider.getQueryChoicePairs()) {
					string shownameQuery = kvp.Key;
					string chosenResult = kvp.Value;
					queryResults.Add(shownameQuery + "," + chosenResult);
				}
				tvShowInfoProvider.getProviderConfiguration().set(
					ParseHTMLStrategyProperties.QUERY_CHOICE_PAIRS_KEY, queryResults);
			}
		}

		/// <summary>
		/// Gets currently selected tv show provider
		/// </summary>
		/// <returns>Last used tv show provider, or null if error</returns>
		public static TvShowInfoProvider getCurrentProvider() {
			return
				getProviderByName(
					Settings.Instance.getAppConfiguration().getSingleStringProperty(AppProperties.LAST_USED_TV_SHOW_PROVIDER_KEY));
		}

		/// <summary>
		/// Gets a tv show info provider by its name
		/// </summary>
		/// <param name="tvShowInfoProviderName">name of the titleProvider</param>
		/// <returns>titleProvider matching the name, or null if not found</returns>
		public static TvShowInfoProvider getProviderByName(string tvShowInfoProviderName) {
			foreach (TvShowInfoProvider tvShowInfoProvider in tvShowInfoProviders) {
				if (tvShowInfoProvider.getName().Equals(tvShowInfoProviderName)) {
					return tvShowInfoProvider;
				}
			}
			return null;
		}

		public static List<string> getTvShowInfoProviderNames() {
			return tvShowInfoProviders.Select(
				tvShowProvider => tvShowProvider.getName()).ToList();
		}

		public static void delete(TvShowInfoProvider oldInfoProvider) {
			tvShowInfoProviders.Remove(oldInfoProvider);
		}

		/// <summary>
		/// Returns an array of all filenames in the given subfolder of the application
		/// </summary>
		protected static string[] getConfigurationFileNames(string appSubFolder) {
			return Directory.GetFiles(AppConstants.APP_BASE_DIR + appSubFolder, Configuration.CONFIG_FILE_EXTENSION_PATTERN);
		}

		private static bool isValid(TvShowInfoProvider someProvider) {
			if (string.IsNullOrEmpty(someProvider.getName())) {
				return false;
			}
			return true;
		}
	}
}