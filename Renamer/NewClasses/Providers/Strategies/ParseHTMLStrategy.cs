using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Renamer.NewClasses.Collector;
using Renamer.NewClasses.Config;
using Renamer.NewClasses.Config.Application;
using Renamer.NewClasses.Config.Providers;
using Renamer.NewClasses.Enums;
using Renamer.NewClasses.Util;

namespace Renamer.NewClasses.Providers.Strategies {
	public class ParseHTMLStrategy : SearchStrategy {
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger
	(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private AbstractProvider theProvider;
		private ConfigurationWrapper theConfiguration;
		/// <summary>
		/// <see cref="SearchUrl"/>
		/// </summary>
		private string searchUrl;
		private string searchResultsUrl;
		private string seriesUrl;
		private string searchRegExp;
		private List<string> searchRemove = new List<string>();
		private bool searchRightToLeft;
		private string encoding;
		private string searchStart;
		private string searchEnd;
		private string notFoundUrl;
		private Language language; //Helper.languagePriorities.None
		private string searchResultsBlacklist;
		private string relationsRemove;


		/// <summary>
		//Blacklist used to filter out unwanted search results
		/// </summary>
		public string getSearchResultsBlacklist() {
			return searchResultsBlacklist;
		}
		public void setSearchResultsBlacklist(string newValue) {
			searchResultsBlacklist = newValue;
		}

		/// <summary>
		/// Search URL, %T is a placeholder for the search episode
		/// </summary>
		public string getSearchUrl() {
			return searchUrl;
		}
		public void setSearchUrl(string newValue) {
			searchUrl = newValue;
		}

		/// <summary>
		/// substring of the search results page URL
		/// </summary>
		public string getSearchResultsUrl() {
			return searchResultsUrl;
		}
		public void setSearchResultsUrl(string newValue) {
			searchResultsUrl = newValue;
		}

		/// <summary>
		/// substring of the series page URL
		/// </summary>
		public string getSeriesUrl() {
			return seriesUrl;
		}
		public void setSeriesUrl(string newValue) {
			seriesUrl = newValue;
		}

		/// <summary>
		/// Regular expression for parsing search results
		/// </summary>
		public string getSearchRegExp() {
			return searchRegExp;
		}
		public void setSearchRegExp(string newValue) {
			searchRegExp = newValue;
		}

		/// <summary>
		/// some regular expressions to remove from search results name
		/// </summary> 
		public List<string> getSearchRemove() {
			return searchRemove;
		}
		public void setSearchRemove(List<string> newValue) {
			if (newValue != null) {
				searchRemove.Clear();
				searchRemove.AddRange(newValue);
			}
		}

		public string getRelationsRemove() {
			return relationsRemove;
		}
		public void setRelationsRemove(string newValue) {
			relationsRemove = newValue;
		}

		/// <summary>
		/// start regex for search pages from end of file
		/// </summary>
		public bool getSearchRightToLeft() {
			return searchRightToLeft;
		}
		public void setSearchRightToLeft(bool newValue) {
			searchRightToLeft = newValue;
		}

		/// <summary>
		/// Page encoding, leave empty for automatic
		/// </summary>
		public string getEncoding() {
			return encoding;
		}
		public void setEncoding(string newValue) {
			encoding = newValue;
		}

		public string getSearchStart() {
			return searchStart;
		}
		public void setSearchStart(string newValue) {
			searchStart = newValue;
		}

		public string getSearchEnd() {
			return searchEnd;
		}
		public void setSearchEnd(string newValue) {
			searchEnd = newValue;
		}

		/// <summary>
		/// If some page forwards to this URL, it is assumed the link is invalid
		/// </summary>
		public string getNotFoundUrl() {
			return notFoundUrl;
		}
		public void setNotFoundUrl(string newValue) {
			notFoundUrl = newValue;
		}

		/// <summary>
		/// language can be specified, but doesn't have to be
		/// </summary>
		public Language getLanguage() {
			return language;
		}
		public void setLanguage(Language newValue) {
			language = newValue;
		}

		public ParseHTMLStrategy(AbstractProvider provider, ConfigurationWrapper someConfiguration) {
			theProvider = provider;
			theConfiguration = someConfiguration;
			setSearchRegExp(theConfiguration.getSingleStringProperty(ParseHTMLStrategyProperties.REGEX_PARSE_SEARCH_RESULTS_KEY));
			setSearchResultsUrl(theConfiguration.getSingleStringProperty(ParseHTMLStrategyProperties.SEARCH_RESULTS_URL_KEY));
			setSearchUrl(theConfiguration.getSingleStringProperty(ParseHTMLStrategyProperties.SEARCH_URL_KEY));
			setSeriesUrl(theConfiguration.getSingleStringProperty(ParseHTMLStrategyProperties.SERIES_URL_KEY));
			setSearchRemove(theConfiguration.getMultiStringProperty(ParseHTMLStrategyProperties.SEARCH_REMOVE_KEY));
			setSearchStart(theConfiguration.getSingleStringProperty(ParseHTMLStrategyProperties.SEARCH_START_KEY));
			setSearchEnd(theConfiguration.getSingleStringProperty(ParseHTMLStrategyProperties.SEARCH_END_KEY));
			setNotFoundUrl(theConfiguration.getSingleStringProperty(ParseHTMLStrategyProperties.NOT_FOUND_URL_KEY));
			setEncoding(theConfiguration.getSingleStringProperty(ParseHTMLStrategyProperties.ENCODING_KEY));
			setLanguage(theConfiguration.getEnumProperty<Language>(ParseHTMLStrategyProperties.LANGUAGE_KEY));
			setSearchRightToLeft(theConfiguration.getSingleBoolProperty(ParseHTMLStrategyProperties.SEARCH_RIGHT_TO_LEFT_KEY));
			setSearchResultsBlacklist(theConfiguration.getSingleStringProperty(ParseHTMLStrategyProperties.REGEX_SEARCH_RESULTS_BLACKLIST_KEY));
			setRelationsRemove(theConfiguration.getSingleStringProperty(ParseHTMLStrategyProperties.RELATIONS_REMOVE_KEY));
		}

		public List<ParsedSearch> searchForShows(List<string> shownames, BackgroundWorker worker, DoWorkEventArgs e) {
			List<ParsedSearch> searchResults = new List<ParsedSearch>();
			int searchProgress = 0;
			foreach (string showname in shownames) {
				if (worker.CancellationPending) {
					searchResults.Clear();
					e.Cancel = true;
					log.Info("Cancelled searching for titles.");
					return searchResults;
				}
				searchResults.Add(search(showname, showname));
				searchProgress++;
				worker.ReportProgress((int)(searchProgress / ((double)shownames.Count * 100)));
			}
			return searchResults;
		}

		public ParsedSearch search(string showname, string searchString) {
            ParsedSearch ps = new ParsedSearch();
			ps.provider = theProvider;
            ps.SearchString = searchString;
            ps.Showname = showname;
            // request
			if (theProvider == null) {
                log.Error("No relation provider found/selected");
                return ps;
            }

			string url = getSearchUrl();
            log.Debug("Search URL: " + url);
            if (string.IsNullOrEmpty(url)) {
                log.Error("Can't search because no search URL is specified for this provider");
                return ps;
            }
            url = url.Replace(RenamingConstants.SHOWNAME_MARKER, searchString);
            url = System.Web.HttpUtility.UrlPathEncode(url);
            log.Debug("Encoded Search URL: " + url);
            WebRequest requestHtml = null;
			try {
                requestHtml = WebRequest.Create(url);
            }
            catch (Exception ex) {
                log.Error(ex.Message);
	            requestHtml?.Abort();
	            return ps;
            }
            //SetProxy(requestHtml, url);
            log.Info("Searching at " + url.Replace(" ", "%20"));
            requestHtml.Timeout = Convert.ToInt32(
				Settings.Instance.getAppConfiguration().getSingleNumberProperty(AppProperties.CONNECTION_TIMEOUT_KEY));
            // get response
            WebResponse responseHtml = null;
            try {
                responseHtml = requestHtml.GetResponse();
            }
            catch (Exception ex) {
                log.Error(ex.Message);
	            responseHtml?.Close();
                requestHtml.Abort();
                return ps;
            }
            log.Debug("Search Results URL: " + responseHtml.ResponseUri.AbsoluteUri);
            //if search engine directs us straight to the result page, skip parsing search results
            string seriesURL = getSeriesUrl();
            if (responseHtml.ResponseUri.AbsoluteUri.Contains(seriesURL)) {
                log.Debug("Search Results URL contains Series URL: " + seriesURL);
                ps.Results = new Hashtable();
                string cleanedName = cleanSearchResultName(showname, getSearchRemove());
                if(getSearchResultsBlacklist() == "" || !Regex.Match(cleanedName,getSearchResultsBlacklist()).Success){
                    ps.Results.Add(cleanedName, responseHtml.ResponseUri.AbsoluteUri + getEpisodesUrl());
                    log.Info("Search engine forwarded directly to single result: " + responseHtml.ResponseUri.AbsoluteUri.Replace(" ", "%20") + getEpisodesUrl().Replace(" ", "%20"));
                }
                return ps;
            }
            log.Debug("Search Results URL doesn't contain Series URL: " + seriesURL + ", this is a proper search results page");
            // and download
            StreamReader r = null;
            try {
                r = new StreamReader(responseHtml.GetResponseStream());
            }
            catch (Exception ex) {
	            r?.Close();
	            responseHtml.Close();
                requestHtml.Abort();
                log.Error(ex.Message);
                return ps;
            }
            string source = r.ReadToEnd();
            r.Close();

            //Source cropping
            source = source.Substring(Math.Max(source.IndexOf(getSearchStart(), StringComparison.Ordinal), 0));
            source = source.Substring(0, Math.Max(source.LastIndexOf(getSearchEnd(), StringComparison.Ordinal), source.Length - 1));
            ps = parseSearch(ref source, responseHtml.ResponseUri.AbsoluteUri, showname, searchString);
            responseHtml.Close();
            return ps;
		}

		public static string cleanSearchResultName(string searchResult, List<string> searchRemove) {
			foreach (string pattern in searchRemove) {
				searchResult = Regex.Replace(searchResult, pattern, "");
			}
			return searchResult.Trim();
		}

		/// <summary>
		/// Parses search results from a series search
		/// </summary>
		/// <param name="source">Source code of the search results page</param>
		/// <param name="showname">Showname</param>
		/// <param name="sourceUrl">URL of the page source</param>
		public ParsedSearch parseSearch(ref string source, string sourceUrl, string showname, string searchString) {
			ParsedSearch ps = new ParsedSearch();
			ps.Showname = showname;
			ps.SearchString = searchString;
			ps.provider = theProvider;
			if (String.IsNullOrEmpty(source)) {
				return ps;
			}

			if (theProvider == null) {
				log.Error("No relation provider found/selected");
				return ps;
			}

			log.Debug("Trying to match source at " + sourceUrl + " with " + getSearchRegExp());

			RegexOptions ro = RegexOptions.IgnoreCase | RegexOptions.Singleline;
			if (getSearchRightToLeft()) {
				ro |= RegexOptions.RightToLeft;
			}
			MatchCollection mc = Regex.Matches(source, getSearchRegExp(), ro);

			if (mc.Count == 0) {
				log.Info("No results found");
				return ps;
			} else {
				log.Info("Search engine found multiple results at " + sourceUrl.Replace(" ", "%20"));
				ps.Results = new Hashtable();
				foreach (Match m in mc) {
					string url = getSeriesUrl();
					url = url.Replace(RenamingConstants.SHOW_LINK_MARKER, m.Groups["link"].Value);
					url = System.Web.HttpUtility.HtmlDecode(url);
					string name = System.Web.HttpUtility.HtmlDecode(m.Groups["name"].Value + " " + m.Groups["year"].Value);
					//temporary fix, this should be externalized in the titleProvider configs
					if (name.ToLower().Contains("poster"))
						continue;
					string CleanedName = cleanSearchResultName(name, getSearchRemove());
					try {
						ps.Results.Add(CleanedName, url);
					} catch (Exception) {
						log.Error("Can't add " + CleanedName + " to search results because an entry of same name already exists");
					}
				}
				return ps;
			}
		}
	}
}
