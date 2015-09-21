namespace Renamer.NewClasses.Config.Providers {
	public class ParseHTMLStrategyProperties {
		/// <summary>
		/// Regex used to filter out unwanted search results
		/// </summary>
		public const string REGEX_SEARCH_RESULTS_BLACKLIST_KEY = "SearchResultsBlacklist";

		/// <summary>
		/// Search URL, %T is a placeholder for the search episode
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

		// Source:TitleProviderConstants

        /// <summary>
        /// Additionally, if the search engine redirects to the single result directly, we might need a string to attach to the results page to get to the episodes page
        /// </summary>
        public const string TV_SHOW_EPISODES_URL_SUFFIX_KEY = "EpisodesURL";

        /// <summary>
        /// Link to the page containing seasonEpisodeNr infos. %L is used as placeholder for the link corresponding to the show the user selected
        /// </summary>
        public const string TV_SHOW_URL_KEY = "RelationsPage";

        /// <summary>
        /// Regular expression to extract season/number/seasonEpisodeNr name relationship from the page containing this info
        /// This needs to contain:
        /// (?&ltSeason&gtRegExpToExtractSeason) - to get the season number
        /// (?&ltEpisode&gtRegExpToExtractEpisode) - to get the seasonEpisodeNr number
        /// (?&ltTitle&gtRegExpToExtractTitle) - to get the episode belonging to that season/seasonEpisodeNr
        ///If Relationspage uses %S placeholder, there is no need to include (?<SEASON_NR_MARKER>RegExpToExtractSeason) here
        /// </summary>
        public const string EPISODE_INFO_REGEX_KEY = "RelationsRegExp";

        /// <summary>
        /// string to search for for cropping off some source from the relations page start
        /// </summary>
        public const string INFO_START_INDICATOR_KEY = "RelationsStart";

        /// <summary>
        /// string to search for for cropping off some source from the relations page end
        /// </summary>
        public const string INFO_END_INDICATOR_KEY = "RelationsEnd";

        /// <summary>
        /// start regex for relations pages from end of file
        /// </summary>
        public const string DO_REVERSE_PARSE_KEY = "RelationsRightToLeft";

        /// <summary>
        /// Last selected results
        /// </summary>
        public const string QUERY_CHOICE_PAIRS_KEY = "SelectedResults";
	}
}