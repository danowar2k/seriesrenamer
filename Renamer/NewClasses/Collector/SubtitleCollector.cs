using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Renamer.NewClasses.Config;
using Renamer.NewClasses.Config.Application;

namespace Renamer.NewClasses.Collector {
	class SubtitleCollector {

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger
	(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Used if the download link(s) can be constructed directly from the search results page
		/// %L gets replaced with the value aquired from Search results page "link" property, 
		/// %P will allow to iterate over pages/seasons etc
		/// </summary>
		/// <param name="extracted">Extracted value from search results which is inserted into "CONSTRUCT_LINK_KEY" url</param>
		private static void ConstructLinks(string extracted) {
			SubtitleProvider subprovider = SubtitleProvider.GetCurrentProvider();
			string link = subprovider.ConstructLink;
			link = link.Replace("%L", extracted);
			int loop = 1;
			if (link.Contains("%P")) {
				loop = 20;
			}
			//TODO: Make 20 setable somewhere or find better cancel condition
			for (int i = 1; i < loop + 1; i++) {
				string anotherlink = link.Replace("%P", i.ToString());
				anotherlink = System.Web.HttpUtility.UrlPathEncode(anotherlink);
				HttpWebRequest requestHtml;
				try {
					requestHtml = (HttpWebRequest)(HttpWebRequest.Create(anotherlink));
				} catch (Exception ex) {
					log.Error(ex.Message);
					return;
				}
				requestHtml.Timeout = Convert.ToInt32(Helper.ReadProperty(AppProperties.CONNECTION_TIMEOUT_KEY));
				// get response
				HttpWebResponse responseHtml = null;
				try {
					responseHtml = (HttpWebResponse)(requestHtml.GetResponse());
				} catch (Exception ex) {
					log.Error(ex.Message);
					if (responseHtml != null)
						responseHtml.Close();
					return;
				}

				responseHtml.Close();
				if (subprovider.NotFoundUrl == "" || responseHtml.ResponseUri.ToString() != subprovider.NotFoundUrl) {
					SubtitleFileManager.Instance.AddSubtitleLink(responseHtml.ResponseUri.ToString());
				}
			}

		}
		/// <summary>
		/// Main subtitle acquisition function
		/// </summary>
		public static void GetSubtitles() {
			if (Settings.Instance.isMonoCompatibilityMode()) {
				log.Warn("Subtitle downloading is not supported in Mono, since additional dlls for unpacking are required which won't work here :(");
				return;
			}
			SubtitleFileManager.Instance.ClearLinks();
			// request
			SubtitleProvider subprovider = SubtitleProvider.GetCurrentProvider();
			if (subprovider == null) {
				log.Error("No subtitle provider found/selected");
				return;
			}
			string url = subprovider.SearchUrl;
			if (url == null || url == "") {
				log.Error("Can't search because no search URL is specified for this subtitle provider");
				return;
			}
			url = url.Replace("%T", Helper.ReadProperties(AppProperties.LAST_SEARCHED_SHOWNAMES_KEY)[0]);
			url = System.Web.HttpUtility.UrlPathEncode(url);
			HttpWebRequest requestHtml;
			try {
				requestHtml = (HttpWebRequest)(HttpWebRequest.Create(url));
			} catch (Exception ex) {
				log.Error(ex.Message);
				return;
			}
			log.Info("Searching at " + url.Replace(" ", "%20"));
			requestHtml.Timeout = Convert.ToInt32(Helper.ReadProperty(AppProperties.CONNECTION_TIMEOUT_KEY));
			// get response
			HttpWebResponse responseHtml = null;
			try {
				responseHtml = (HttpWebResponse)(requestHtml.GetResponse());
			} catch (Exception ex) {
				log.Error(ex.Message);
				if (responseHtml != null)
					responseHtml.Close();
				return;
			}
			//if search engine directs us straight to the result page, skip parsing search results
			string seriesURL = subprovider.SeriesUrl;
			if (responseHtml.ResponseUri.AbsoluteUri.Contains(seriesURL)) {
				log.Info("Search engine forwarded directly to single result: " + responseHtml.ResponseUri.AbsoluteUri.Replace(" ", "%20") + subprovider.SubtitlesURL.Replace(" ", "%20"));
				//GetSubtitleFromSeriesPage(responseHtml.ResponseUri.AbsoluteUri + subprovider.SUBTITLES_URL_KEY);
			} else {

				// and download
				StreamReader r = null;
				try {
					r = new StreamReader(responseHtml.GetResponseStream());
				} catch (Exception ex) {
					if (r != null)
						r.Close();
					log.Error(ex.Message);
					return;
				}
				string source = r.ReadToEnd();
				r.Close();


				//Source cropping
				source = source.Substring(Math.Max(source.IndexOf(subprovider.SearchStart), 0));
				source = source.Substring(0, Math.Max(source.LastIndexOf(subprovider.SearchEnd), 0));

				ParseSubtitleSearch(ref source, responseHtml.ResponseUri.AbsoluteUri);
			}
			int i;

			responseHtml.Close();
		}

		/// <summary>
		/// Subtitle Search Result Parsing function.
		/// Extracts search results (i.e. Show names) and gets links to them.
		/// If more than one show is found, user gets to select one, otherwise he will be directly forwarded
		/// For now only pages where search links directly to subtitles work
		/// </summary>
		/// <param name="source">HTML Source of the search results page</param>
		/// <param name="SourceURL">URL of the source</param>
		private static void ParseSubtitleSearch(ref string source, string SourceURL) {
			if (source == "")
				return;
			SubtitleProvider subprovider = SubtitleProvider.GetCurrentProvider();
			string pattern = subprovider.SearchRegExp;
			RegexOptions ro = RegexOptions.IgnoreCase | RegexOptions.Singleline;
			if (subprovider.SearchRightToLeft)
				ro |= RegexOptions.RightToLeft;
			MatchCollection mc = Regex.Matches(source, pattern, ro);
			if (mc.Count == 0) {
				log.Info("No results found");
			} else if (mc.Count == 1) {
				string url = subprovider.SubtitlesPage;
				url = url.Replace("%L", mc[0].Groups["link"].Value);
				if (subprovider.ConstructLink != "") {
					ConstructLinks(mc[0].Groups["link"].Value);
				} else {
					//GetSubtitleFromSeriesPage(url);
				}
			} else {
				log.Info("Search engine found multiple results at " + SourceURL.Replace(" ", "%20"));
				SelectResult sr = new SelectResult(mc, subprovider, true);
				if (sr.ShowDialog() == DialogResult.Cancel)
					return;
				if (sr.urls.Count == 0)
					return;
				foreach (string str in sr.urls) {
					string url = subprovider.SubtitlesPage;
					url = url.Replace("%L", str);
					if (subprovider.ConstructLink != "") {
						ConstructLinks(str);
					} else {
						//GetSubtitleFromSeriesPage(url);
					}
				}
			}
		}
	}
}
