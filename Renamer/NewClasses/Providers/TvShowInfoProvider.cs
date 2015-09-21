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
using Renamer.NewClasses.Config.Providers;
using Renamer.NewClasses.Providers.Strategies;

namespace Renamer.NewClasses.Providers {
	/// <summary>
	/// A provider for information about tv shows
	/// Any tv show provider keeps track about all tv show providers
	/// </summary>
	public class TvShowInfoProvider : AbstractProvider {

		/// <summary>
		/// This is used to separate the showname query and the result chosen to be the corresponding show
		/// </summary>
		public static readonly string[] QUERY_CHOICE_SEPARATORS = {","};

		/// <summary>
		/// The URL to use when searching for tv shows.
		/// </summary>
		private string tvShowUrl = "";
		/// <summary>
		/// The regular expression to use when looking for information about individual episodes.
		/// </summary>
		private string episodeInfoRegEx = "";
		/// <summary>
		/// If true, the page containing the search results is parsed from bottom to top.
		/// </summary>
		private bool doReverseParse;
		/// <summary>
		/// The suffix to append to the URL for tv shows to search information about individual episodes.
		/// </summary>
		private string tvShowEpisodesUrlSuffix = "";
		/// <summary>
		/// When parsing the results page, this string indicates the point 
		/// where to start to look for information about an episode.
		/// </summary>
		private string infoStartIndicator = "";
		/// <summary>
		/// When parsing the results page, this string indicates the point 
		/// where to end looking for information about an episode.
		/// </summary>
		private string infoEndIndicator = "";

		/// <summary>
		/// When using a tv show provider to search for information about tv show episode,
		/// the user will at first try to find the correct tv show.
		/// The user enters a query string (probably the name of a show).
		/// As the string may be ambiguous, the results can provide multiple shows.
		/// The user chooses which show was the one he was looking for.
		/// This is what is stored here:
		/// - The original showname query string
		/// - The show that the user chose was the correct one
		/// </summary>
		private Dictionary<string, string> queryChoicePairs = new Dictionary<string, string>();

		public TvShowInfoProvider(string configurationFileName)
			: base(configurationFileName, new ParseHTMLStrategy(this, configurationFileName)) {
				setTvShowUrl(providerConfiguration.getSingleStringProperty(ParseHTMLStrategyProperties.TV_SHOW_URL_KEY));
				setEpisodeInfoRegEx(providerConfiguration.getSingleStringProperty(ParseHTMLStrategyProperties.EPISODE_INFO_REGEX_KEY));
				setTvShowEpisodesUrlSuffix(providerConfiguration.getSingleStringProperty(ParseHTMLStrategyProperties.TV_SHOW_EPISODES_URL_SUFFIX_KEY));
				setInfoStartIndicator(providerConfiguration.getSingleStringProperty(ParseHTMLStrategyProperties.INFO_START_INDICATOR_KEY));
				setInfoEndIndicator(providerConfiguration.getSingleStringProperty(ParseHTMLStrategyProperties.INFO_END_INDICATOR_KEY));
				setReverseParse(providerConfiguration.getSingleBoolProperty(ParseHTMLStrategyProperties.DO_REVERSE_PARSE_KEY));
				setQueryChoicePairs(providerConfiguration.getMultiStringProperty(ParseHTMLStrategyProperties.QUERY_CHOICE_PAIRS_KEY));
		}

		/// <summary>
		/// URL of the page containing information about a tv show. 
		/// %L is used as placeholder for the link corresponding to the show the user selected
		/// </summary>
		public string getTvShowUrl() {
			return tvShowUrl;
		}
		/// <summary>
		/// Set the tv show URL.
		/// </summary>
		/// <param name="newTvShowUrl">new tv show URL</param>
		public void setTvShowUrl(string newTvShowUrl) {
			tvShowUrl = newTvShowUrl;
		}

		/// <summary>
		/// Regular expression to extract episode information from the page containing this info
		/// This needs to contain:
		/// (?&ltSeason&gtRegExpToExtractSeason) - to get the season number
		/// (?&ltEpisode&gtRegExpToExtractEpisode) - to get the episode number
		/// (?&ltTitle&gtRegExpToExtractTitle) - to get the title of the episode
		///If the tv show URL uses %S placeholder, there is no need to include (?&ltSEASON_NR_MARKER&gtRegExpToExtractSeason) here
		/// </summary>
		public string getEpisodeInfoRegEx() {
			return episodeInfoRegEx;
		}
		/// <summary>
		/// Sets the regular expression to extract episode info from a page
		/// </summary>
		/// <param name="newEpisodeInfoRegEx">new regular expression</param>
		public void setEpisodeInfoRegEx(string newEpisodeInfoRegEx) {
			episodeInfoRegEx = newEpisodeInfoRegEx;
		}


		/// <summary>
		/// Check if we parse the html source of a result page from the bottom up
		/// </summary>
		public bool isReverseParsed() {
			return doReverseParse;
		}
		/// <summary>
		/// Set if we parse the html source of a result page from the bottom up
		/// </summary>
		/// <param name="newDoReverseParse">new value for reverse parsing</param>
		public void setReverseParse(bool newDoReverseParse) {
			doReverseParse = newDoReverseParse;
		}

		/// <summary>
		/// Additionally, if the search engine redirects to the single result directly, 
		/// we might need a string to attach to the results url to request the episodes page
		/// </summary>
		public string getTvShowEpisodesUrlSuffix() {
			return tvShowEpisodesUrlSuffix;
		}
		/// <summary>
		/// Set the suffix for the episode info url.
		/// </summary>
		/// <param name="newSuffix">new suffix</param>
		public void setTvShowEpisodesUrlSuffix(string newSuffix) {
			tvShowEpisodesUrlSuffix = newSuffix;
		}

		/// <summary>
		/// Returns the indicator when episode info starts on a page.
		/// </summary>
		/// <returns></returns>
		public string getInfoStartIndicator() {
			return infoStartIndicator;
		}
		/// <summary>
		/// Sets the indicator where episode information starts.
		/// </summary>
		/// <param name="newStartIndicator">new start indicator</param>
		public void setInfoStartIndicator(string newStartIndicator) {
			infoEndIndicator = newStartIndicator;
		}

		/// <summary>
		/// Returns the indicator when episode info ends on a page.
		/// </summary>
		/// <returns></returns>
		public string getInfoEndIndicator() {
			return infoEndIndicator;
		}
		/// <summary>
		/// Sets the indicator where episode information ends.
		/// </summary>
		/// <param name="newEndIndicator">new end indicator</param>
		public void setInfoEndIndicator(string newEndIndicator) {
			infoEndIndicator = newEndIndicator;
		}

		public Dictionary<string, string> getQueryChoicePairs() {
			return queryChoicePairs;
		}

		public void setQueryChoicePairs(List<string> pairs) {
			queryChoicePairs.Clear();

			foreach (string queryChoice in pairs) {
				string[] parts = queryChoice.Split(QUERY_CHOICE_SEPARATORS, 2, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length == 2) {
					string query = parts[0];
					string choice = parts[1];
					queryChoicePairs.Add(query, choice);
				}
			}		
		}

		/// <summary>
		/// String representation of this tv show provider.
		/// </summary>
		/// <returns>the name of the provider</returns>
		public string toString() {
			return getName();
		}

	}
}