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
using System.IO;
using Renamer.Logging;
using Renamer.Classes.Configuration;
using System.Collections;
using System.Collections.ObjectModel;
namespace Renamer.Classes.Provider
{
    /// <summary>
    /// A titleProvider for episode titles
    /// This class keeps track of all available title providers
    /// </summary>
    public class TitleProvider : Provider
    {
        /// <summary>
        /// The separator used in the search result history separating the search string and the chosen result 
        /// </summary>
        private const string SHOW_ENTRY_SEPARATOR = ",";
        /// <summary>
        /// The folder where configuration files for the episode data providers can be found
        /// </summary>
        private const string APP_SUBFOLDER = "/Databases/Titles";

        private static List<TitleProvider> titleProviders;

        private string relationsPage = "";
        private string relationsRegExp = "";
        private bool relationsRightToLeft = false;
        private string episodesUrl = "";
        private string relationsStart = "";
        private string relationsEnd = "";

        public Dictionary<string, string> SelectedResults = new Dictionary<string, string>();

        public TitleProvider()
        {
            TitleProviders.Add(this);
        }

        public TitleProvider(string configurationFileName)
            : base(configurationFileName)
        {
            this.RelationsPage = Helper.ReadProperty(ProviderConfigKeyConstants.TitleProviderKeyConstants.RELATIONS_PAGE_KEY, configurationFileName);
            this.RelationsRegExp = Helper.ReadProperty(ProviderConfigKeyConstants.TitleProviderKeyConstants.REGEX_RELATIONS_KEY, configurationFileName);
            this.EpisodesUrl = Helper.ReadProperty(ProviderConfigKeyConstants.TitleProviderKeyConstants.EPISODES_URL_KEY, configurationFileName);
            this.RelationsStart = Helper.ReadProperty(ProviderConfigKeyConstants.TitleProviderKeyConstants.RELATIONS_START_KEY, configurationFileName);
            this.RelationsEnd = Helper.ReadProperty(ProviderConfigKeyConstants.TitleProviderKeyConstants.RELATIONS_END_KEY, configurationFileName);
            this.RelationsRightToLeft = Helper.ReadBool(ProviderConfigKeyConstants.TitleProviderKeyConstants.RELATIONS_RIGHT_TO_LEFT_KEY, configurationFileName);
            List<string> selectedShowEntries = new List<string>(Helper.ReadProperties(ProviderConfigKeyConstants.TitleProviderKeyConstants.SELECTED_SHOW_ENTRIES_KEY, configurationFileName));

            string[] separators = { SHOW_ENTRY_SEPARATOR };
            foreach (string showEntry in selectedShowEntries)
            {
                string[] entryParts = showEntry.Split(separators, 2, StringSplitOptions.RemoveEmptyEntries);
                if (entryParts.Length == 2)
                {
                    string searchString = entryParts[0];
                    string chosenResult = entryParts[1];
                    SelectedResults.Add(searchString, chosenResult);
                }
            }
            TitleProviders.Add(this);
        }

        private static List<TitleProvider> TitleProviders
        {
            get {
                if (titleProviders == null) {
                    titleProviders = new List<TitleProvider>();
                    LoadAllValidTitleProviders();
                }
                return titleProviders;
            }
        }

        /// <summary>
        /// Checks the app subfolder for title titleProvider configuration files and populates the list with valid titleProvider configurations
        /// </summary>
        public static void LoadAllValidTitleProviders()
        {
            StringBuilder status = new StringBuilder();
            status.AppendLine("Providers found:");
            foreach (string configFileName in getConfigurationFileNames(APP_SUBFOLDER))
            {
                Logger.Instance.LogMessage("Provider: " + configFileName, LogLevel.DEBUG);
                try {
                    TitleProvider newTitleProvider = new TitleProvider(configFileName);

                    // TODO: So the titleProvider name is required (!)
                    if (String.IsNullOrEmpty(newTitleProvider.Name)) {
                        Logger.Instance.LogMessage("Invalid provider file: " + configFileName, LogLevel.ERROR);
                        newTitleProvider.delete();
                        continue;
                    }
                    status.AppendLine(newTitleProvider.Name);
                }
                catch (Exception ex) {
                    Logger.Instance.LogMessage("Invalid provider file: " + configFileName + " Error: " + ex.Message, LogLevel.ERROR);
                }
            }
            Logger.Instance.LogMessage(status.ToString(), LogLevel.INFO);
        }

        public static void saveQueryResults()
        {
            foreach (TitleProvider titleProvider in TitleProviders)
            {
                List<string> queryResults = new List<string>();
                foreach (KeyValuePair<string, string> kvp in titleProvider.SelectedResults)
                {
                    string titleQuery = kvp.Key;
                    string chosenResult = kvp.Value;
                    queryResults.Add(titleQuery + "," + chosenResult);
                }
                Helper.WriteProperties(ProviderConfigKeyConstants.TitleProviderKeyConstants.SELECTED_SHOW_ENTRIES_KEY, queryResults.ToArray(), titleProvider.configurationFilePath);
            }
        }
        

        /// <summary>
        /// Gets currently selected titleProvider
        /// </summary>
        /// <returns>Currently selected titleProvider, or null if error</returns>
        public static TitleProvider GetCurrentProvider() {
            return GetProviderByName(Helper.ReadProperty(ConfigKeyConstants.LAST_SELECTED_TITLE_PROVIDER_KEY));
        }

        /// <summary>
        /// Gets a titleProvider by its name
        /// </summary>
        /// <param name="name">name of the titleProvider</param>
        /// <returns>titleProvider matching the name, or null if not found</returns>
        public static TitleProvider GetProviderByName(string providerName) {
            foreach (TitleProvider titleProvider in TitleProviders)
            {
                if (titleProvider.Name == providerName) {
                    return titleProvider;
                }
            }
            return null;
        }


        public static List<string> getProviderNames() {
            List<string> providerNames = new List<string>();
            foreach (TitleProvider titleProvider in TitleProviders) {
                providerNames.Add(titleProvider.Name);
            }
            return providerNames;
        }

        public string toString()
        {
            return Name;
        }


        public void delete() {
            TitleProviders.Remove(this);
        }


        /// <summary>
        /// Link to the page containing episode infos. %L is used as placeholder for the link corresponding to the show the user selected
        /// </summary>
        public string RelationsPage {
            get { return relationsPage; }
            set { relationsPage = value; }
        }

        /// <summary>
        /// Regular expression to extract season/number/episode name relationship from the page containing this info
        /// This needs to contain:
        /// (?&ltSeason&gtRegExpToExtractSeason) - to get the season number
        /// (?&ltEpisode&gtRegExpToExtractEpisode) - to get the episode number
        /// (?&ltTitle&gtRegExpToExtractTitle) - to get the title belonging to that season/episode
        ///If Relationspage uses %S placeholder, there is no need to include (?<SEASON_NR>RegExpToExtractSeason) here
        /// </summary>
        public string RelationsRegExp {
            get { return relationsRegExp; }
            set { relationsRegExp = value; }
        }

        /// <summary>
        /// start regex for relations pages from end of file
        /// </summary>
        public bool RelationsRightToLeft {
            get { return relationsRightToLeft; }
            set { relationsRightToLeft = value; }
        }

        /// <summary>
        /// Additionally, if the search engine redirects to the single result directly, we might need a string to attach to the results page to get to the episodes page
        /// </summary>

        public string EpisodesUrl {
            get { return episodesUrl; }
            set { episodesUrl = value; }
        }


        public string RelationsStart {
            get { return relationsStart; }
            set { relationsStart = value; }
        }

        public string RelationsEnd {
            get { return relationsEnd; }
            set { relationsEnd = value; }
        }
    }
}
