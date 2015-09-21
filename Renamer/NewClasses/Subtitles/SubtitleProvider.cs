﻿#region SVN Info
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
using Renamer.Classes.Logging;
using Renamer.Classes.Util;
using Renamer.Classes.Configuration;

namespace Renamer.Classes.Provider
{
    /// <summary>
    /// A subtitle file titleProvider
    /// </summary>
    class SubtitleProvider : AbstractProvider
    {

        private const string location = "/Databases/Subtitles";

        private static List<SubtitleProvider> list;

        private static List<SubtitleProvider> List {
            get {
                if (list == null) {
                    list = new List<SubtitleProvider>();
                    LoadAll();
                }
                return list;
            }
        }

        /*
         * is there a way to move this to the superclass ...
         * */
        public static void LoadAll() {
            string[] providers = getConfigurationFileNames(location);
            string status = "Providers found:";
            foreach (string file in providers) {
                Logger.Instance.LogMessage("Provider: " + file, LogLevel.DEBUG);
                try {
                    SubtitleProvider rel = new SubtitleProvider(file);

                    if (String.IsNullOrEmpty(rel.Name)) {
                        Logger.Instance.LogMessage("Invalid provider file: " + file, LogLevel.ERROR);
                        rel.delete();
                        continue;
                    }
                    status += " " + rel.Name + ",";
                }
                catch (Exception ex) {
                    Logger.Instance.LogMessage("Invalid provider file: " + file + " Error: " + ex.Message, LogLevel.ERROR);
                }
            }
            status = status.TrimEnd(new char[] { ',' });
            //Logger.Instance.LogMessage(status, LogLevel.INFO);
        }

        public SubtitleProvider() {
            List.Add(this);
        }
        public SubtitleProvider(string filename)
            : base(filename) {
            this.SubtitlesPage = Helper.ReadProperty(ProviderConfigKeyConstants.SubtitlesKeyConstants.SUBTITLES_PAGE_KEY, filename);
            this.SubtitleRegExp = Helper.ReadProperty(ProviderConfigKeyConstants.SubtitlesKeyConstants.REGEX_SUBTITLE_KEY, filename);
            this.SubtitlesURL = Helper.ReadProperty(ProviderConfigKeyConstants.SubtitlesKeyConstants.SUBTITLES_URL_KEY, filename);
            this.SubtitlesStart = Helper.ReadProperty(ProviderConfigKeyConstants.SubtitlesKeyConstants.SUBTITLES_START_KEY, filename);
            this.SubtitlesEnd = Helper.ReadProperty(ProviderConfigKeyConstants.SubtitlesKeyConstants.SUBTITLES_END_KEY, filename);
            this.ConstructLink = Helper.ReadProperty(ProviderConfigKeyConstants.SubtitlesKeyConstants.CONSTRUCT_LINK_KEY, filename);
            list.Add(this);
        }


        public void delete() {
            List.Remove(this);
        }

        public static string[] ProviderNames {
            get {
                string[] ret = new string[List.Count];
                int i = 0;
                foreach (AbstractProvider rel in List) {
                    ret[i++] = rel.Name;
                }
                return (string[])ret.Clone();
            }
        }



        /// <summary>
        /// Gets currently selected subtitle titleProvider
        /// </summary>
        /// <returns>Currently selected subtitle titleProvider, or null if error</returns>
        public static SubtitleProvider GetCurrentProvider() {
            return GetProviderByName(Helper.ReadProperty(ConfigKeyConstants.LAST_SELECTED_SUBTITLE_PROVIDER_KEY));
        }

        /// <summary>
        /// Gets a subtitle titleProvider by its name
        /// </summary>
        /// <param name="name">name of the subtitle titleProvider</param>
        /// <returns>subtitle titleProvider matching the name, or null if not found</returns>
        public static SubtitleProvider GetProviderByName(string name) {
            foreach (SubtitleProvider sp in List) {
                if (sp.Name == name) {
                    return sp;
                }
            }
            return null;
        }



        private bool directLink = false;
        private string constructLink = "";
        private string subtitlesPage = "";
        private string subtitleRegExp = "";
        private string subtitlesURL = "";
        private string subtitlesStart = "";
        private string subtitlesEnd = "";

        /// <summary>
        /// Is the download link directly on the search results page? (Instead of a page related to that show in between)
        /// </summary>
        public bool DirectLink {
            get { return directLink; }
            set { directLink = value; }
        }

        /// <summary>
        /// Link to the page containing subtitle links. %L is used as placeholder for the link corresponding to the show the user selected
        /// For multiple pages of subtitle downloads, use %P
        /// </summary>
        public string SubtitlesPage {
            get { return subtitlesPage; }
            set { subtitlesPage = value; }
        }

        /// <summary>
        /// Regular expression to extract subtitle links (along with names) from downloads page
        /// This needs to contain: 
        /// (?&ltSeason&gtRegExpToExtractSeason) - to get the season number
        /// (?&ltEpisode&gtRegExpToExtractEpisode) - to get the episode number
        /// (?&ltLink&gtRegExpToExtractLink) - to get the download link for one episode
        /// If Package is set to 1, only download link is required
        /// </summary>
        public string SubtitleRegExp {
            get { return subtitleRegExp; }
            set { subtitleRegExp = value; }
        }

        /// <summary>
        /// Additionally, if the search engine redirects to the single result directly, we might need a string to attach to the results page to get to the episodes page
        /// </summary>
        public string SubtitlesURL {
            get { return subtitlesURL; }
            set { subtitlesURL = value; }
        }


        public string SubtitlesStart {
            get { return subtitlesStart; }
            set { subtitlesStart = value; }
        }

        public string SubtitlesEnd {
            get { return subtitlesEnd; }
            set { subtitlesEnd = value; }
        }


        /// <summary>
        /// If the download link(s) can be constructed directly from the search results page, use this variable.
        /// %L gets replaced with the value aquired from Search results page "link" property, 
        /// %P will allow to iterate over pages/seasons etc
        /// </summary>
        public string ConstructLink {
            get { return constructLink; }
            set { constructLink = value; }
        }

    }
}
