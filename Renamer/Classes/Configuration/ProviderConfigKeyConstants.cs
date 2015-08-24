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

namespace Renamer.Classes.Configuration
{
    /// <summary>
    /// Helper class containing property names used in subtitle titleProvider files
    /// </summary>
    public class ProviderConfigKeyConstants
    {
        /// <summary>
        /// Name of the titleProvider
        /// </summary>
        public const string PROVIDER_NAME_KEY = "Name";

        /// <summary>
        /// Regex used to filter out unwanted search results
        /// </summary>
        public const string REGEX_SEARCH_RESULTS_BLACKLIST_KEY = "SearchResultsBlacklist";

        /// <summary>
        /// Search URL, %T is a placeholder for the search title
        /// </summary>
        public const string SEARCH_URL_KEY = "SearchURL";

        /// <summary>
        /// substring of the search results page URL
        /// </summary>
        public const string SEARCH_RESULTS_URL_KEY = "SearchResultsURL";

        /// <summary>
        /// substring of the series page URL
        /// </summary>
        public const string SERIES_URL_KEY = "SeriesURL";

        /// <summary>
        /// Regular expression for parsing search results
        /// </summary>
        public const string REGEX_PARSE_SEARCH_RESULTS_KEY = "SearchRegExp";

        /// <summary>
        /// string to search for for cropping off some source from the search page start
        /// </summary>
        public const string SEARCH_START_KEY = "SearchStart";

        /// <summary>
        /// string to search for for cropping off some source from the search page end
        /// </summary>
        public const string SEARCH_END_KEY = "SearchEnd";

        /// <summary>
        /// some regular expressions to remove from search results name
        /// </summary>
        public const string SEARCH_REMOVE_KEY = "SearchRemove";

        /// <summary>
        /// start regex for search pages from end of file
        /// </summary>
        public const string SEARCH_RIGHT_TO_LEFT_KEY = "SearchRightToLeft";

        /// <summary>
        /// If some page forwards to this URL, it is assumed the link is invalid
        /// </summary>
        public const string NOT_FOUND_URL_KEY = "NotFoundURL";

        /// <summary>
        /// encoding of the page, leave empty for automatic
        /// </summary>
        public const string ENCODING_KEY = "Encoding";

        /// <summary>
        /// language may be set in config file, mostly used for treating umlauts right now
        /// </summary>
        public const string LANGUAGE_KEY = "Language";

        public const string RELATIONS_REMOVE_KEY = "RelationsRemove";

        /// <summary>
        /// Helper class containing property names used in subtitle titleProvider files
        /// </summary>
        public static class SubtitlesKeyConstants
        {
            /// <summary>
            /// Is the download link directly on the search results page?
            /// </summary>
            public const string DIRECT_LINK_KEY = "DirectLink";

            /// <summary>
            /// Additionally, if the search engine redirects to the single result directly, we might need a string to attach to the results page to get to the episodes page
            /// </summary>
            public const string SUBTITLES_URL_KEY = "SubtitlesURL";

            /// <summary>
            /// Link to the page containing subtitle links. %L is used as placeholder for the link corresponding to the show the user selected
            /// For multiple pages of subtitle downloads, use %P
            /// </summary>
            public const string SUBTITLES_PAGE_KEY = "SubtitlesPage";

            /// <summary>
            /// Regular expression to extract subtitle links (along with names) from downloads page
            /// This needs to contain: 
            /// (?&ltSeason&gtRegExpToExtractSeason) - to get the season number
            /// (?&ltEpisode&gtRegExpToExtractEpisode) - to get the seasonEpisodeNr number
            /// (?&ltLink&gtRegExpToExtractLink) - to get the download link for one seasonEpisodeNr
            /// If Package is set to 1, only download link is required
            /// </summary>
            public const string REGEX_SUBTITLE_KEY = "SubtitleRegExp";

            /// <summary>
            /// string to search for for cropping off some source from the search page start
            /// </summary>
            public const string SUBTITLES_START_KEY = "SubtitlesStart";

            /// <summary>
            /// string to search for for cropping off some source from the search page end
            /// </summary>
            public const string SUBTITLES_END_KEY = "SubtitlesEnd";

            /// <summary>
            /// If the download link(s) can be constructed directly from the search results page, use this variable.
            /// %L gets replaced with the value aquired from Search results page "link" property, 
            /// %P will allow to iterate over pages/seasons etc
            /// </summary>
            public const string CONSTRUCT_LINK_KEY = "ConstructLink";
        }

        /// <summary>
        /// Helper class containing property names used in season/seasonEpisodeNr&lt;-&gt;titleProvider files
        /// </summary>
        public static class TitleProviderKeyConstants
        {
            /// <summary>
            /// Additionally, if the search engine redirects to the single result directly, we might need a string to attach to the results page to get to the episodes page
            /// </summary>
            public const string EPISODES_URL_KEY = "EpisodesURL";

            /// <summary>
            /// Link to the page containing seasonEpisodeNr infos. %L is used as placeholder for the link corresponding to the show the user selected
            /// </summary>
            public const string RELATIONS_PAGE_KEY = "RelationsPage";

            /// <summary>
            /// Regular expression to extract season/number/seasonEpisodeNr name relationship from the page containing this info
            /// This needs to contain:
            /// (?&ltSeason&gtRegExpToExtractSeason) - to get the season number
            /// (?&ltEpisode&gtRegExpToExtractEpisode) - to get the seasonEpisodeNr number
            /// (?&ltTitle&gtRegExpToExtractTitle) - to get the title belonging to that season/seasonEpisodeNr
            ///If Relationspage uses %S placeholder, there is no need to include (?<SEASON_NR>RegExpToExtractSeason) here
            /// </summary>
            public const string REGEX_RELATIONS_KEY = "RelationsRegExp";

            /// <summary>
            /// string to search for for cropping off some source from the relations page start
            /// </summary>
            public const string RELATIONS_START_KEY = "RelationsStart";

            /// <summary>
            /// string to search for for cropping off some source from the relations page end
            /// </summary>
            public const string RELATIONS_END_KEY = "RelationsEnd";

            /// <summary>
            /// start regex for relations pages from end of file
            /// </summary>
            public const string RELATIONS_RIGHT_TO_LEFT_KEY = "RelationsRightToLeft";

            /// <summary>
            /// Last selected results
            /// </summary>
            public const string SELECTED_SHOW_ENTRIES_KEY = "SelectedResults";
        }

    }
}
