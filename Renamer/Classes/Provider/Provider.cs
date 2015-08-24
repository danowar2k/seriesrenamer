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
using Renamer.Classes.Configuration;

namespace Renamer.Classes.Provider
{
    /// <summary>
    /// An abstract titleProvider of tv show episode titles
    /// </summary>
    public abstract class Provider
    {
        private static string appBaseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// <see cref="name"/>
        /// </summary>
        private string name = "";
        public string configurationFilePath = "";
        /// <summary>
        /// <see cref="SearchUrl"/>
        /// </summary>
        private string searchUrl = "";
        private string searchResultsUrl = "";
        private string seriesUrl = "";
        private string searchRegExp = "";
        private string[] searchRemove;
        private bool searchRightToLeft = false;
        private string encoding = "";
        private string searchStart = "";
        private string searchEnd = "";
        private string notFoundUrl = "";
        private Helper.Languages language; //Helper.languagePriorities.None
        private string searchResultsBlacklist = "";
        private string relationsRemove = "";

        public Provider() {

        }

        public Provider(string someConfigurationFilePath) {

            this.configurationFilePath = someConfigurationFilePath;
            this.Name = Helper.ReadProperty(ProviderConfigKeyConstants.PROVIDER_NAME_KEY, someConfigurationFilePath);
            this.SearchRegExp = Helper.ReadProperty(ProviderConfigKeyConstants.REGEX_PARSE_SEARCH_RESULTS_KEY, someConfigurationFilePath);
            this.SearchResultsUrl = Helper.ReadProperty(ProviderConfigKeyConstants.SEARCH_RESULTS_URL_KEY, someConfigurationFilePath);
            this.SearchUrl = Helper.ReadProperty(ProviderConfigKeyConstants.SEARCH_URL_KEY, someConfigurationFilePath);
            this.SeriesUrl = Helper.ReadProperty(ProviderConfigKeyConstants.SERIES_URL_KEY, someConfigurationFilePath);
            this.SearchRemove = Helper.ReadProperties(ProviderConfigKeyConstants.SEARCH_REMOVE_KEY, someConfigurationFilePath);
            this.SearchStart = Helper.ReadProperty(ProviderConfigKeyConstants.SEARCH_START_KEY, someConfigurationFilePath);
            this.SearchEnd = Helper.ReadProperty(ProviderConfigKeyConstants.SEARCH_END_KEY, someConfigurationFilePath);
            this.NotFoundUrl = Helper.ReadProperty(ProviderConfigKeyConstants.NOT_FOUND_URL_KEY, someConfigurationFilePath);
            this.Encoding = Helper.ReadProperty(ProviderConfigKeyConstants.ENCODING_KEY, someConfigurationFilePath);
            this.Language = Helper.ReadEnum<Helper.Languages>(ProviderConfigKeyConstants.LANGUAGE_KEY, someConfigurationFilePath);
            this.SearchRightToLeft = Helper.ReadBool(ProviderConfigKeyConstants.SEARCH_RIGHT_TO_LEFT_KEY, someConfigurationFilePath);
            this.SearchResultsBlacklist = Helper.ReadProperty(ProviderConfigKeyConstants.REGEX_SEARCH_RESULTS_BLACKLIST_KEY, someConfigurationFilePath);
            this.RelationsRemove = Helper.ReadProperty(ProviderConfigKeyConstants.RELATIONS_REMOVE_KEY, someConfigurationFilePath);
        }


        /// <summary>
        /// Returns an array of all filenames in the given subfolder of the application
        /// </summary>
        protected static string[] getConfigurationFileNames(string appSubFolder)
        {
            return Directory.GetFiles(appBaseDir + appSubFolder, ConfigFile.configFileExtensionPattern);
        }


        

        /// <summary>
        /// name of the titleProvider
        /// </summary>
        public string Name {
            get { return name; }
            set { name = value; }
        }

        //Blacklist used to filter out unwanted search results
        public string SearchResultsBlacklist
        {
            get { return searchResultsBlacklist; }
            set { searchResultsBlacklist = value; }
        }

        /// <summary>
        /// Search URL, %T is a placeholder for the search title
        /// </summary>
        public string SearchUrl {
            get { return searchUrl; }
            set { searchUrl = value; }
        }

        /// <summary>
        /// substring of the search results page URL
        /// </summary>
        public string SearchResultsUrl {
            get { return searchResultsUrl; }
            set { searchResultsUrl = value; }
        }

        /// <summary>
        /// substring of the series page URL
        /// </summary>
        public string SeriesUrl {
            get { return seriesUrl; }
            set { seriesUrl = value; }
        }

        /// <summary>
        /// Regular expression for parsing search results
        /// </summary>
        public string SearchRegExp {
            get { return searchRegExp; }
            set { searchRegExp = value; }
        }

        /// <summary>
        /// some regular expressions to remove from search results name
        /// </summary>
        public string[] SearchRemove {
            get { return searchRemove; }
            set { searchRemove = value; }
        }

        public string RelationsRemove
        {
            get { return relationsRemove; }
            set { relationsRemove = value; }
        }
        /// <summary>
        /// start regex for search pages from end of file
        /// </summary>
        public bool SearchRightToLeft {
            get { return searchRightToLeft; }
            set { searchRightToLeft = value; }
        }

        /// <summary>
        /// Page encoding, leave empty for automatic
        /// </summary>
        public string Encoding {
            get { return encoding; }
            set { encoding = value; }
        }

        public string SearchStart {
            get { return searchStart; }
            set { searchStart = value; }
        }

        public string SearchEnd {
            get { return searchEnd; }
            set { searchEnd = value; }
        }

        /// <summary>
        /// If some page forwards to this URL, it is assumed the link is invalid
        /// </summary>
        public string NotFoundUrl {
            get { return notFoundUrl; }
            set { notFoundUrl = value; }
        }

        /// <summary>
        /// LANGUAGE_KEY can be specified, but doesn't have to be
        /// </summary>
        public Helper.Languages Language {
            get { return language; }
            set { language = value; }
        }
    }
}
