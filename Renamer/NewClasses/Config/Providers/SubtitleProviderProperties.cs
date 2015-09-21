
namespace Renamer.NewClasses.Config.Properties {
	class SubtitleProviderProperties {
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
}
