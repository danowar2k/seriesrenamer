﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Renamer.Classes;
using Renamer.Classes.Logging;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Renamer.Classes.UI.Dialogs;
using System.Runtime.InteropServices;
using Renamer.Classes.Configuration;
using Renamer.Classes.TVShows;
using Renamer.Classes.Provider;
using Renamer.Classes.Subtitles;
using Renamer.Classes.UI;
using Renamer.Classes.Util;

using System.ComponentModel;

namespace Renamer
{
    public class DataGenerator
    {
        public static List<ParsedSearch> Results;
        public static void GetAllTitles(BackgroundWorker worker, DoWorkEventArgs e) {
            //make a list of shownames
            List<string> shownames = new List<string>();
            foreach (MediaFile ie in MediaFileManager.Instance) {
                if (ie.ProcessingRequested && !ie.Movie && ie.Showname!="" && !ie.Sample && !shownames.Contains(ie.Showname) ) {
                    shownames.Add(ie.Showname);
                }
            }
            List<ParsedSearch> SearchResults = new List<ParsedSearch>();
            int progress=0;
            // get titles for the entire list generated before
            foreach (string showname in shownames) {
                if (worker.CancellationPending)
                {
                    Results = null;
                    e.Cancel = true;
                    Logger.Instance.LogMessage("Cancelled searching for titles.", LogLevel.INFO);
                    return;
                }
                SearchResults.Add(Search(TitleProvider.GetCurrentProvider(), showname, showname));
                progress++;
                worker.ReportProgress((int)((double)progress / ((double)shownames.Count) * 100));
            }
            Results = SearchResults;
        }
            
        public static void GetAllRelations(BackgroundWorker worker, DoWorkEventArgs e){            
            int progress = 0;
            foreach (ParsedSearch ps in Results)
            {
                if (ps.Results != null && ps.Results.Count > 0)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    GetRelations((string)ps.Results[ps.SelectedResult], ps.Showname, ps.provider,worker,e);                    
                }
                progress++;
                worker.ReportProgress((int)((double)progress / ((double)Results.Count) * 100));
            }
        }

        public class ParsedSearch{
            public string SearchString;
            public string Showname;
            public TitleProvider provider;
            public Hashtable Results;
            public string SelectedResult = "";
            public string ToString()
            {
                return SelectedResult;
            }
        }
        public static ParsedSearch Search(TitleProvider provider, string SearchString, string Showname){
            ParsedSearch ps = new ParsedSearch();
            ps.provider = provider;
            ps.SearchString = SearchString;
            ps.Showname = Showname;
            //once
            for (int a = 0; a < 1; a++) {
                // request
                if (provider == null) {
                    Logger.Instance.LogMessage("No relation provider found/selected", LogLevel.ERROR);
                    return ps;
                }
                /*
                //get rid of old relations
                RelationManager.Instance.RemoveRelationCollection(Showname);
                foreach (Candidate ie in InfoEntryManager.Instance) {
                    if (ie.Showname == Showname && ie.ProcessingRequested) {
                        ie.PROVIDER_NAME_KEY = "";
                        ie.NewFileName = "";
                        ie.LANGUAGE_KEY = titleProvider.LANGUAGE_KEY;
                    }
                }*/
                string url = provider.SearchUrl;
                Logger.Instance.LogMessage("Search URL: " + url, LogLevel.DEBUG);
                if (url == null || url == "") {
                    Logger.Instance.LogMessage("Can't search because no search URL is specified for this provider", LogLevel.ERROR);
                    break;
                }
                url = url.Replace("%T", SearchString);
                url = System.Web.HttpUtility.UrlPathEncode(url);
                Logger.Instance.LogMessage("Encoded Search URL: " + url, LogLevel.DEBUG);
                HttpWebRequest requestHtml = null;
                try {
                    requestHtml = (HttpWebRequest)(HttpWebRequest.Create(url));
                }
                catch (Exception ex) {
                    Logger.Instance.LogMessage(ex.Message, LogLevel.ERROR);
                    if (requestHtml != null)
                        requestHtml.Abort();
                    break;
                }
                //SetProxy(requestHtml, url);
                Logger.Instance.LogMessage("Searching at " + url.Replace(" ", "%20"), LogLevel.INFO);
                requestHtml.Timeout = Convert.ToInt32(Helper.ReadProperty(ConfigKeyConstants.CONNECTION_TIMEOUT_KEY));
                // get response
                HttpWebResponse responseHtml = null;
                try {
                    responseHtml = (HttpWebResponse)(requestHtml.GetResponse());
                }
                catch (Exception ex) {
                    Logger.Instance.LogMessage(ex.Message, LogLevel.ERROR);
                    if (responseHtml != null)
                        responseHtml.Close();
                    if (requestHtml != null)
                        requestHtml.Abort();
                    break;
                }
                Logger.Instance.LogMessage("Search Results URL: " + responseHtml.ResponseUri.AbsoluteUri, LogLevel.DEBUG);
                //if search engine directs us straight to the result page, skip parsing search results
                string seriesURL = provider.SeriesUrl;
                if (responseHtml.ResponseUri.AbsoluteUri.Contains(seriesURL)) {
                    Logger.Instance.LogMessage("Search Results URL contains Series URL: " + seriesURL, LogLevel.DEBUG);
                    ps.Results = new Hashtable();
                    string CleanedName=CleanSearchResultName(Showname,provider);
                    if(provider.SearchResultsBlacklist=="" || !Regex.Match(CleanedName,provider.SearchResultsBlacklist).Success){
                        ps.Results.Add(CleanedName, responseHtml.ResponseUri.AbsoluteUri + provider.EpisodesUrl);
                        Logger.Instance.LogMessage("Search engine forwarded directly to single result: " + responseHtml.ResponseUri.AbsoluteUri.Replace(" ", "%20") + provider.EpisodesUrl.Replace(" ", "%20"), LogLevel.INFO);
                    }
                    return ps;
                }
                else {
                    Logger.Instance.LogMessage("Search Results URL doesn't contain Series URL: " + seriesURL + ", this is a proper search results page", LogLevel.DEBUG);
                    // and download
                    StreamReader r = null;
                    try {
                        r = new StreamReader(responseHtml.GetResponseStream());
                    }
                    catch (Exception ex) {
                        if (r != null)
                            r.Close();
                        if (responseHtml != null) {
                            responseHtml.Close();
                        }
                        if (requestHtml != null) {
                            requestHtml.Abort();
                        }
                        Logger.Instance.LogMessage(ex.Message, LogLevel.ERROR);
                        break;
                    }
                    string source = r.ReadToEnd();
                    r.Close();

                    //Source cropping
                    source = source.Substring(Math.Max(source.IndexOf(provider.SearchStart), 0));
                    source = source.Substring(0, Math.Max(source.LastIndexOf(provider.SearchEnd), source.Length - 1));
                    ps = ParseSearch(ref source, responseHtml.ResponseUri.AbsoluteUri, Showname, SearchString, provider);
                }

                responseHtml.Close();
            }
            return ps;
        }

        public static string CleanSearchResultName(string SearchResult, TitleProvider provider) {
            foreach (string pattern in provider.SearchRemove)
            {
                SearchResult = Regex.Replace(SearchResult, pattern, "");
            }
            return SearchResult.Trim();
        }
        /*
        /// <summary>
        /// gets titles, by using database search feature and parsing results, after that, show them in gui
        /// </summary>
        public static void GetTitles(string Showname) {
            //once
            for (int a = 0; a < 1; a++) {
                // request
                TitleProvider titleProvider = TitleProvider.GetCurrentProvider();
                if (titleProvider == null) {
                    Logger.Instance.LogMessage("No relation titleProvider found/selected", LogLevel.ERROR);
                    return;
                }
                //get rid of old relations
                RelationManager.Instance.RemoveRelationCollection(Showname);
                foreach (Candidate ie in InfoEntryManager.Instance) {
                    if (ie.Showname == Showname && ie.ProcessingRequested) {
                        ie.PROVIDER_NAME_KEY = "";
                        ie.NewFileName = "";
                        ie.LANGUAGE_KEY = titleProvider.LANGUAGE_KEY;
                    }
                }
                string url = titleProvider.SearchUrl;
                Logger.Instance.LogMessage("Search URL: " + url, LogLevel.DEBUG);
                if (url == null || url == "") {
                    Logger.Instance.LogMessage("Can't search because no search URL is specified for this titleProvider", LogLevel.ERROR);
                    break;
                }
                string[] LAST_SEARCHED_SERIES_TITLES_KEY = Helper.ReadProperties(ConfigKeyConstants.LAST_SEARCHED_SERIES_TITLES_KEY);
                url = url.CUSTOM_REPLACE_REGEX_STRINGS_KEY("%T", Showname);
                url = System.Web.HttpUtility.UrlPathEncode(url);
                Logger.Instance.LogMessage("Encoded Search URL: " + url, LogLevel.DEBUG);
                HttpWebRequest requestHtml = null;
                try {
                    requestHtml = (HttpWebRequest)(HttpWebRequest.Create(url));
                }
                catch (Exception ex) {
                    Logger.Instance.LogMessage(ex.Message, LogLevel.ERROR);
                    if (requestHtml != null)
                        requestHtml.Abort();
                    break;
                }
                //SetProxy(requestHtml, url);
                Logger.Instance.LogMessage("Searching at " + url.CUSTOM_REPLACE_REGEX_STRINGS_KEY(" ", "%20"), LogLevel.INFO);
                requestHtml.CONNECTION_TIMEOUT_KEY = Convert.ToInt32(Helper.ReadProperty(ConfigKeyConstants.CONNECTION_TIMEOUT_KEY));
                // get response
                HttpWebResponse responseHtml = null;
                try {
                    responseHtml = (HttpWebResponse)(requestHtml.GetResponse());
                }
                catch (Exception ex) {
                    Logger.Instance.LogMessage(ex.Message, LogLevel.ERROR);
                    if (responseHtml != null)
                        responseHtml.Close();
                    if (requestHtml != null)
                        requestHtml.Abort();
                    break;
                }
                Logger.Instance.LogMessage("Search Results URL: " + responseHtml.ResponseUri.AbsoluteUri, LogLevel.DEBUG);
                //if search engine directs us straight to the result page, skip parsing search results
                string seriesURL = titleProvider.SeriesUrl;
                if (responseHtml.ResponseUri.AbsoluteUri.Contains(seriesURL)) {
                    Logger.Instance.LogMessage("Search Results URL contains Series URL: " + seriesURL, LogLevel.DEBUG);
                    Logger.Instance.LogMessage("Search engine forwarded directly to single result: " + responseHtml.ResponseUri.AbsoluteUri.CUSTOM_REPLACE_REGEX_STRINGS_KEY(" ", "%20") + titleProvider.EpisodesUrl.CUSTOM_REPLACE_REGEX_STRINGS_KEY(" ", "%20"), LogLevel.INFO);
                    GetRelations(responseHtml.ResponseUri.AbsoluteUri + titleProvider.EpisodesUrl, Showname);
                }
                else {
                    Logger.Instance.LogMessage("Search Results URL doesn't contain Series URL: " + seriesURL + ", this is a proper search results page", LogLevel.DEBUG);
                    // and download
                    StreamReader r = null;
                    try {
                        r = new StreamReader(responseHtml.GetResponseStream());
                    }
                    catch (Exception ex) {
                        if (r != null)
                            r.Close();
                        if (responseHtml != null) {
                            responseHtml.Close();
                        }
                        if (requestHtml != null) {
                            requestHtml.Abort();
                        }
                        Logger.Instance.LogMessage(ex.Message, LogLevel.ERROR);
                        break;
                    }
                    string source = r.ReadToEnd();
                    r.Close();

                    //Source cropping
                    source = source.Substring(Math.Max(source.IndexOf(titleProvider.SEARCH_START_KEY), 0));
                    source = source.Substring(0, Math.Max(source.LastIndexOf(titleProvider.SEARCH_END_KEY), source.Length - 1));
                    ParseSearch(ref source, responseHtml.ResponseUri.AbsoluteUri, Showname, titleProvider);
                }

                responseHtml.Close();
            }
        }
        */
        public static void SetProxy(HttpWebRequest client, string url) {
            // Comment out foreach statement to use normal System.Net proxy detection 
            foreach (
                    Uri address
                    in WinHttpSafeNativeMethods.GetProxiesForUrl(new Uri(url))) {
                client.Proxy = new WebProxy(address);
                break;
            }
        }

        
        /// <summary>
        /// Parses search results from a series search
        /// </summary>
        /// <param name="source">Source code of the search results page</param>
        /// <param name="Showname">Showname</param>
        /// <param name="SourceURL">URL of the page source</param>
        public static ParsedSearch ParseSearch(ref string source, string SourceURL, string Showname, string SearchString, TitleProvider provider) {
            ParsedSearch ps = new ParsedSearch();
            ps.Showname = Showname;
            ps.SearchString = SearchString;
            ps.provider = provider;
            if (String.IsNullOrEmpty(source)){
                return ps;
            }
            
            if (provider == null) {
                Logger.Instance.LogMessage("No relation provider found/selected", LogLevel.ERROR);
                return ps;
            }

            Logger.Instance.LogMessage("Trying to match source at " + SourceURL + " with " + provider.SearchRegExp, LogLevel.DEBUG);

            RegexOptions ro = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            if (provider.SearchRightToLeft){
                ro |= RegexOptions.RightToLeft;
            }
            MatchCollection mc = Regex.Matches(source, provider.SearchRegExp, ro);

            if (mc.Count == 0) {
                Logger.Instance.LogMessage("No results found", LogLevel.INFO);
                return ps;
            }
            /*else if (mc.Count == 1) {
                string url = titleProvider.RELATIONS_PAGE_KEY;
                Logger.Instance.LogMessage("One result found on search page, going to " + url.CUSTOM_REPLACE_REGEX_STRINGS_KEY(" ", "%20") + " with %L=" + mc[0].Groups["link"].Value, LogLevel.DEBUG);
                url = url.CUSTOM_REPLACE_REGEX_STRINGS_KEY("%L", mc[0].Groups["link"].Value);
                url = System.Web.HttpUtility.HtmlDecode(url);
                ps.Results = new Hashtable();
                string CleanedName=CleanSearchResultName(Showname,titleProvider);
                if(!Regex.Match(CleanedName, titleProvider.REGEX_SEARCH_RESULTS_BLACKLIST_KEY).Success){
                    ps.Results.Add(CleanedName, url);
                    Logger.Instance.LogMessage("Search engine found one result: " + url.CUSTOM_REPLACE_REGEX_STRINGS_KEY(" ", "%20"), LogLevel.INFO);
                }
                return ps;
                //GetRelations(url, Showname);
            }*/
            else {
                Logger.Instance.LogMessage("Search engine found multiple results at " + SourceURL.Replace(" ", "%20"), LogLevel.INFO);
                ps.Results = new Hashtable();
                foreach (Match m in mc)
                {
                    string url = provider.RelationsPage;
                    url = url.Replace("%L", m.Groups["link"].Value);
                    url = System.Web.HttpUtility.HtmlDecode(url);
                    string name=System.Web.HttpUtility.HtmlDecode(m.Groups["name"].Value + " " + m.Groups["year"].Value);
                    //temporary fix, this should be externalized in the titleProvider configs
                    if(name.ToLower().Contains("poster")) continue;
                    string CleanedName = CleanSearchResultName(name, provider);
                    try
                    {
                        ps.Results.Add(CleanedName, url);
                    }
                    catch (Exception)
                    {
                        Logger.Instance.LogMessage("Can't add " + CleanedName + " to search results because an entry of same name already exists", LogLevel.ERROR);
                    }
                }
                return ps;
                /*
                SelectResult sr = new SelectResult(mc, titleProvider, false);
                if (sr.ShowDialog() == DialogResult.Cancel || sr.url == "")
                    return;
                //Apply language of selected result to matching episodes
                if (titleProvider.LANGUAGE_KEY == Helper.PREFERRED_RESULT_LANGUAGES_KEY.None) {
                    foreach (Candidate ie in InfoEntryManager.Instance) {
                        if (ie.Showname == Showname && ie.ProcessingRequested) {
                            ie.LANGUAGE_KEY = sr.LANGUAGE_KEY;
                        }
                    }
                }
                
                string url = titleProvider.RELATIONS_PAGE_KEY;
                Logger.Instance.LogMessage("User selected " + titleProvider.RELATIONS_PAGE_KEY + "with %L=" + sr.url, LogLevel.DEBUG);
                url = url.CUSTOM_REPLACE_REGEX_STRINGS_KEY("%L", sr.url);
                url = System.Web.HttpUtility.HtmlDecode(url);
                GetRelations(url, Showname);*/
            }
        }

        //parse page(s) containing relations
        /// <summary>
        /// Parses page containing the relation data
        /// </summary>
        /// <param name="url">URL of the page to parse</param>
        /// <param name="Showname">Showname</param>
        public static void GetRelations(string url, string Showname, TitleProvider provider,BackgroundWorker worker, DoWorkEventArgs dwea) {
            if (provider == null) {
                Logger.Instance.LogMessage("GetRelations: No relation provider found/selected", LogLevel.ERROR);
                return;
            }
            if (String.IsNullOrEmpty(url))
            {
                Logger.Instance.LogMessage("GetRelations: URL is null or empty", LogLevel.ERROR);
                return;
            }
            if (String.IsNullOrEmpty(Showname))
            {
                Logger.Instance.LogMessage("GetRelations: Showname is null or empty", LogLevel.ERROR);
                return;
            }
            Logger.Instance.LogMessage("Trying to get relations from " + url, LogLevel.DEBUG);
            //if episode infos are stored on a new page for each season, this should be marked with %S in url, so we can iterate through all those pages
            int season = 1;
            string url2 = url;
            //Create new RelationCollection
            TvShow rc = new TvShow(Showname);
            while (true) {
                if (url2.Contains("%S")) {
                    url = url2.Replace("%S", season.ToString());
                }

                if (url == null || url == "")
                    return;
                // request
                url = System.Web.HttpUtility.UrlPathEncode(url);
                Logger.Instance.LogMessage("Trying to get relations for season " + season + " from " + url, LogLevel.DEBUG);
                HttpWebRequest requestHtml = null;
                try {
                    requestHtml = (HttpWebRequest)(HttpWebRequest.Create(url));
                }
                    
                catch (Exception ex) {
                    Logger.Instance.LogMessage(ex.Message, LogLevel.ERROR);
                    requestHtml.Abort();
                    return;
                }
                requestHtml.Timeout = Helper.ReadInt(ConfigKeyConstants.CONNECTION_TIMEOUT_KEY);
                // get response
                HttpWebResponse responseHtml = null;
                try {
                    responseHtml = (HttpWebResponse)(requestHtml.GetResponse());
                }
                catch (WebException ex) {
                    //Serienjunkies returns "(300) Mehrdeutige Umleitung" when an inexistant season is requested
                    if (ex.Message.Contains("(300)")) break;
                    Logger.Instance.LogMessage(ex.Message, LogLevel.ERROR);
                    if (responseHtml != null) {
                        responseHtml.Close();
                    }
                    return;
                }
                Logger.Instance.LogMessage("Response URL: " + responseHtml.ResponseUri.AbsoluteUri, LogLevel.DEBUG);
                //if we get redirected, lets assume this page does not exist
                if (responseHtml.ResponseUri.AbsoluteUri != url) {
                    Logger.Instance.LogMessage("Response URL doesn't match request URL, page doesn't seem to exist", LogLevel.DEBUG);
                    responseHtml.Close();
                    requestHtml.Abort();
                    break;
                }
                // and download
                //Logger.Instance.LogMessage("charset=" + responseHtml.CharacterSet, LogLevel.INFO);
                Encoding enc;
                if (provider.Encoding != null && provider.Encoding != "") {
                    try {
                        enc = Encoding.GetEncoding(provider.Encoding);
                    }
                    catch (Exception ex) {
                        Logger.Instance.LogMessage("Invalid encoding in config file: " + ex.Message, LogLevel.ERROR);
                        enc = Encoding.GetEncoding(responseHtml.CharacterSet);
                    }
                }
                else {
                    enc = Encoding.GetEncoding(responseHtml.CharacterSet);
                }
                StreamReader r = new StreamReader(responseHtml.GetResponseStream(), enc);
                string source = r.ReadToEnd();
                r.Close();
                responseHtml.Close();


                //Source cropping
                source = source.Substring(Math.Max(source.IndexOf(provider.RelationsStart), 0));
                source = source.Substring(0, Math.Max(source.LastIndexOf(provider.RelationsEnd), 0));

                string pattern = provider.RelationsRegExp;

                //for some reason, responseHtml.ResponseUri is null for some providers when running on mono
                if(!Settings.Instance.IsMonoCompatibilityMode){
                    Logger.Instance.LogMessage("Trying to match source from " + responseHtml.ResponseUri.AbsoluteUri + " with " + pattern, LogLevel.DEBUG);
                }
                RegexOptions ro = RegexOptions.IgnoreCase | RegexOptions.Singleline;
                if (provider.RelationsRightToLeft)
                    ro |= RegexOptions.RightToLeft;
                MatchCollection mc = Regex.Matches(source, pattern, ro);

                string CleanupRegex = provider.RelationsRemove;
                for (int i = 0; i < mc.Count; i++) {
                    Match m = mc[i];
                    string result = Regex.Replace(System.Web.HttpUtility.HtmlDecode(m.Groups["Title"].Value), CleanupRegex, "");
                    //RELATIONS_REMOVE_KEY may be used to filter out results completely, for example if they are a html tag
                    if (result != "")
                    {
                        //parse season and episode numbers
                        int s, e;

                        Int32.TryParse(m.Groups["Season"].Value, out s);
                        Int32.TryParse(m.Groups["Episode"].Value, out e);
                        //if we are iterating through season pages, take season from page url directly
                        if (url != url2)
                        {
                            rc.AddEpisode(new ShowEpisode(season, e, result));
                            Logger.Instance.LogMessage("Found Relation: " + "S" + s.ToString() + "E" + e.ToString() + " - " + result, LogLevel.DEBUG);
                        }
                        else
                        {
                            rc.AddEpisode(new ShowEpisode(s, e, result));
                            Logger.Instance.LogMessage("Found Relation: " + "S" + s.ToString() + "E" + e.ToString() + " - " + result, LogLevel.DEBUG);
                        }
                    }
                }
                TvShowManager.Instance.AddTvShow(rc);

                // THOU SHALL NOT FORGET THE BREAK
                if (!url2.Contains("%S"))
                    break;
                season++;
            }
            Logger.Instance.LogMessage("" + (season - 1) + " Seasons, " + rc.CountEpisodes + " relations found", LogLevel.DEBUG);
        }        

        /// <summary>
        /// Updatess list view and do lots of other connected stuff with it
        /// </summary>
        /// <param name="clear">if true, list is cleared first and unconnected subtitle files are scheduled to be renamed</param>
        /// <param name="KeepShowName">if set, show name isn't altered</param>
        public static void UpdateList(bool clear, BackgroundWorker worker, DoWorkEventArgs e) {
            MediaFileManager infoManager = MediaFileManager.Instance;
           
            // Clear list if desired, remove deleted files otherwise
            if (clear) {
                infoManager.clear();
            }
            else {
                infoManager.removeMissingFileEntries();
            }

            // read path from config && remove tailing slashes
            string path = Helper.ReadProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY);
            path = path.TrimEnd(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });
            Logger.Instance.LogMessage("Opening folder " + path, LogLevel.DEBUG);
            bool CreateDirectoryStructure = Helper.ReadBool(ConfigKeyConstants.CREATE_DIRECTORY_STRUCTURE_KEY);
            bool UseSeasonSubdirs = Helper.ReadBool(ConfigKeyConstants.CREATE_SEASON_SUBFOLDERS_KEY);

            if (Directory.Exists(path)) {
                //scan for new files
                List<string> extensions = new List<string>(Helper.ReadProperties(ConfigKeyConstants.VIDEO_FILE_EXTENSIONS_KEY));
                extensions.AddRange(Helper.ReadProperties(ConfigKeyConstants.SUBTITLE_FILE_EXTENSIONS_KEY));
                if (extensions == null) {
                    Logger.Instance.LogMessage("No File Extensions found!", LogLevel.WARNING);
                    return;
                }
                //convert all extensions to lowercase
                for (int i = 0; i < extensions.Count; i++) {
                    extensions[i] = extensions[i].ToLower();
                }
                //read all files with matching extension
                List<FileSystemInfo> Files = new List<FileSystemInfo>();
                int count = 0;
                foreach (string ex in extensions) {
                    Files.AddRange(Helper.GetAllFilesRecursively(path, "*." + ex, ref count,worker));
                }


                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    Logger.Instance.LogMessage("Cancelled opening folder.", LogLevel.INFO);
                    return;
                }

                if (MainForm.Instance.InvokeRequired)
                {
                    MainForm.Instance.Invoke(new EventHandler(delegate
                    {
                        MainForm.Instance.lblFileListingProgress.Visible = false;
                        MainForm.Instance.taskProgressBar.Visible = true;
                    }));
                }
                else
                {
                    MainForm.Instance.lblFileListingProgress.Visible = false;
                    MainForm.Instance.taskProgressBar.Visible = true;
                }

                //some declarations already for speed
                string[] patterns = Helper.ReadProperties(ConfigKeyConstants.EPISODE_IDENTIFIER_PATTERNS_KEY);
                for (int i = 0; i < patterns.Length; i++)
                {
                    patterns[i] = RegexConverter.toRegex(patterns[i]);
                }
                int DirectorySeason = -1;
                MediaFile ie = null;
                bool contains = false;
                DateTime dt;
                string currentpath = "";
                string MovieIndicator = String.Join("|", Helper.ReadProperties(ConfigKeyConstants.MOVIE_INDICATOR_STRINGS_KEY));
                //Form1.Instance.taskProgressBar.Maximum = Files.Count;
                for(int f=0;f<Files.Count;f++){
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        Logger.Instance.LogMessage("Cancelled opening folder.", LogLevel.INFO);
                        return;
                    }
                    //Form1.Instance.taskProgressBar.Value=f;
                    worker.ReportProgress((int) ((double)f/((double)Files.Count)*100));
                    FileSystemInfo file=Files[f];
                    //showname and season recognized from path
                    DirectorySeason = -1;
                    //Check if there is already an entry on this file, and if not, create one
                    ie = null;
                    currentpath = Path.GetDirectoryName(file.FullName);
                    foreach (MediaFile i in MediaFileManager.Instance) {
                        if (i.Filename == file.Name && i.FilePath.Path == currentpath) {
                            ie = i;
                            break;
                        }
                    }

                    if (ie == null) {
                        ie = new MediaFile();
                    }

                    //test for movie path so we can skip all series recognition code
                    if (Regex.Match(currentpath, MovieIndicator).Success)
                    {
                        ie.Movie = true;
                    }
                    //Set basic values, by setting those values destination directory and configurationFilePath will be generated automagically                    
                    ie.FilePath.Path = currentpath;
                    ie.Filename = file.Name;
                    ie.Extension = Path.GetExtension(file.FullName).ToLower().Replace(".", "");

                    
                    if(!ie.Movie)
                    {
                        //Get season number and showname from directory
                        DirectorySeason = ExtractSeasonFromDirectory(Path.GetDirectoryName(file.FullName));
                        dt = DateTime.Now;

                        //try to recognize season and episode from configurationFilePath
                        ///////////////////////////////////////////////////
                        ExtractSeasonAndEpisode(ie, patterns);
                        ///////////////////////////////////////////////////

                        //Need to do this for filenames which only contain episode numbers (But might be moved in a season dirArgument already)
                        //if season number couldn't be extracted, try to get it from folder
                        if (ie.SeasonNr <= 0 && DirectorySeason != -1)
                        {
                            ie.SeasonNr = DirectorySeason;
                        }

                        //if season recognized from directory name doesn't match season recognized from configurationFilePath, the file might be located in a wrong directory
                        if (DirectorySeason != -1 && ie.SeasonNr != DirectorySeason)
                        {
                            Logger.Instance.LogMessage("File seems to be located inconsistently: " + ie.Filename + " was recognized as season " + ie.SeasonNr + ", but folder name indicates that it should be season " + DirectorySeason.ToString(), LogLevel.WARNING);
                        }

                        
                    }

                    //if nothing could be recognized, assume that this is a movie
                    if ((ie.SeasonNr < 1 && ie.EpisodeNr < 1) || ie.Movie) {
                        ie.RemoveVideoTags();
                    }

                    //if not added yet, add it
                    if (!contains) {
                        MediaFileManager.Instance.Add(ie);
                    }
                }
                //SelectSimilarFilesForProcessing(path,Helper.ReadProperties(ConfigKeyConstants.LAST_SEARCHED_SERIES_TITLES_KEY)[0]);
                SelectRecognizedFilesForProcessing();
            }

            //Recreate subtitle names so they can adjust to the video files they belong to
            foreach (MediaFile ie in MediaFileManager.Instance)
            {
                if (ie.IsSubtitle)
                {
                    ie.createTargetName();
                }
            }

            Logger.Instance.LogMessage("Found " + MediaFileManager.Instance.CountMediaFiles + " Files", LogLevel.INFO);
            if (Helper.ReadBool(ConfigKeyConstants.REPORT_MISSING_EPISODES_KEY))
            {
                FindMissingEpisodes();
            }
        }

        /// <summary>
        /// Extracts season and episode number by using some regexes from config file
        /// </summary>
        /// <param name="ie">Candidate which should be processed</param>
        public static void ExtractSeasonAndEpisode(MediaFile ie)
        {
            string[] patterns = Helper.ReadProperties(ConfigKeyConstants.EPISODE_IDENTIFIER_PATTERNS_KEY);
            for (int i = 0; i < patterns.Length; i++)
            {
                patterns[i] = RegexConverter.toRegex(patterns[i]);
            }
            ExtractSeasonAndEpisode(ie, patterns);
        }

        /// <summary>
        /// Extracts season and episode number by using some regexes specified in patterns
        /// </summary>
        /// <param name="ie">Candidate which should be processed</param>
        /// <param name="patterns">Patterns to be used for recognition, supply these for speed reasons?</param>
        public static void ExtractSeasonAndEpisode(MediaFile ie, string[] patterns)
        {            
            string strSeason = "";
            string strEpisode = "";
            Match m = null;
            int episode = -1;
            int season = -1;
            Logger.Instance.LogMessage("Extracting season and episode from " + ie.FilePath + Path.DirectorySeparatorChar+ie.Filename, LogLevel.DEBUG);
            //[MOVIES_TAGS_TO_REMOVE_LIST_KEY] and (CRCXXXXX) are removed for recognition, since they might produce false positives
            string cleanedname = Regex.Replace(ie.Filename, "\\[.*?\\]|\\(.{8}\\)", "");
            foreach (string pattern in patterns)
            {
                //Try to match. If it works, get the season and the episode from the match
                m = Regex.Match(cleanedname, pattern, RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
                if (m.Success)
                {
                    Logger.Instance.LogMessage("This pattern produced a match: " + pattern, LogLevel.DEBUG);
                    strSeason = "";
                    strEpisode = "";
                    //ignore numbers like 2063, chance is higher they're a year than a valid season/ep combination
                    if (Int32.Parse(m.Groups["Season"].Value + m.Groups["Episode"].Value) > 2000 && (m.Groups["Season"].Value + m.Groups["Episode"].Value).StartsWith("20"))
                    {
                        continue;
                    }
                    try
                    {
                        strSeason = Int32.Parse(m.Groups["Season"].Value).ToString();
                    }
                    catch (FormatException)
                    {
                    }
                    try
                    {
                        strEpisode = Int32.Parse(m.Groups["Episode"].Value).ToString();
                    }
                    catch (FormatException)
                    {
                    }
                    //Fix for .0216. notation for example, 4 numbers should always be recognized as %S%S%E%E (IF THEY ARENT A YEAR!)
                    if (strEpisode.Length == 3 && strSeason.Length == 1)
                    {
                        strSeason += strEpisode[0];
                        strEpisode = strEpisode.Substring(1);
                        if (strSeason[0] == '0')
                        {
                            strSeason = strSeason.Substring(1);
                        }
                    }
                    
                    try
                    {
                        episode = Int32.Parse(strEpisode);
                    }
                    catch
                    {
                        Logger.Instance.LogMessage("Cannot parse found episode: " + strEpisode, LogLevel.DEBUG);
                    }
                    try
                    {
                        season = Int32.Parse(strSeason);
                    }
                    catch
                    {
                        Logger.Instance.LogMessage("Cannot parse found season: " + strSeason, LogLevel.DEBUG);
                    }

                    
                    if ((ie.Filename.ToLower().Contains("720p") && season == 7 && episode == 20) | (ie.Filename.ToLower().Contains("1080p") && season == 10 && episode == 80))
                    {
                        season = -1;
                        episode = -1;
                        continue;
                    }
                    ie.SeasonNr = season;
                    ie.EpisodeNr = episode;
                    return;
                }
            }
        }
        class EpisodeCollection
        {
            public int maxEpisode = 0;
            public int minEpisode = Int32.MaxValue;
            public List<MediaFile> entries = new List<MediaFile>();
        }

        private static void FindMissingEpisodes()
        {            
            Hashtable paths = new Hashtable();

            foreach (MediaFile ie in MediaFileManager.Instance)
            {
                if (!string.IsNullOrEmpty(ie.Showname))
                {
                    if (paths.ContainsKey(ie.Showname + ie.SeasonNr))
                    {
                        if (((EpisodeCollection)paths[ie.Showname + ie.SeasonNr]).maxEpisode < ie.EpisodeNr)
                        {
                            ((EpisodeCollection)paths[ie.Showname + ie.SeasonNr]).maxEpisode = ie.EpisodeNr;
                        }
                        if (((EpisodeCollection)paths[ie.Showname + ie.SeasonNr]).minEpisode > ie.EpisodeNr)
                        {
                            ((EpisodeCollection)paths[ie.Showname + ie.SeasonNr]).minEpisode = ie.EpisodeNr;
                        }
                        ((EpisodeCollection)paths[ie.Showname + ie.SeasonNr]).entries.Add(ie);
                    }
                    else
                    {
                        EpisodeCollection ec = new EpisodeCollection();
                        ec.maxEpisode = ie.EpisodeNr;
                        ec.minEpisode = ie.EpisodeNr;
                        ec.entries.Add(ie);
                        paths.Add(ie.Showname + ie.SeasonNr, ec);
                    }
                }
            }
            foreach (string key in paths.Keys)
            {
                int missing = 0;
                string message = "Missing episodes in " + ((EpisodeCollection)paths[key]).entries[0].Showname +" Season "+((EpisodeCollection)paths[key]).entries[0].SeasonNr+": ";
                string premessage = "";
                string postmessage = "";
                for (int i = 1; i <= ((EpisodeCollection)paths[key]).maxEpisode; i++)
                {                    
                    bool found=false;
                    foreach (MediaFile ie in ((EpisodeCollection)paths[key]).entries)
                    {
                        if (ie.EpisodeNr == i)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        if(i<((EpisodeCollection)paths[key]).minEpisode){
                            premessage+=String.Format("E{0:00}, ",i);
                        }else{
                            postmessage+=String.Format("E{0:00}, ",i);
                        }
                        missing += 1;
                    }
                }
                
                if (missing > 0)
                {
                    //if not too many missing episodes were found, add the lower missing episodes too
                    if (missing < ((EpisodeCollection)paths[key]).entries.Count)
                    {
                        message += premessage;
                    }
                    message += postmessage;

                    //if something is to be reported
                    if (postmessage != "" || missing < ((EpisodeCollection)paths[key]).entries.Count)
                    {
                        message = message.Substring(0, message.Length - 2);
                        Logger.Instance.LogMessage(message, LogLevel.INFO);
                    }
                }
            }
        }
        private static void SelectRecognizedFilesForProcessing() {
            foreach (MediaFile ie in MediaFileManager.Instance) {
                if (ie.Sample&&!ie.MarkedForDeletion) {
                    ie.ProcessingRequested = false;
                    ie.Movie = false;
                }
                else {
                    ie.ProcessingRequested = (ie.SeasonNr != -1 && ie.EpisodeNr != -1)||(ie.Movie && ((ie.Destination!="" && ie.Destination!=ie.FilePath.Path)||(ie.NewFilename!=""&&ie.NewFilename!=ie.Filename)));
                }
            }
        }

        /// <summary>
        /// Extracts season from directory name
        /// </summary>
        /// <param name="path">path from which to extract the data (NO FILEPATH, JUST FOLDER)</param>
        /// <returns>recognized season, -1 if not recognized</returns>
        public static int ExtractSeasonFromDirectory(string path) {
            string[] patterns = Helper.ReadProperties(ConfigKeyConstants.SEASON_NR_EXTRACTION_PATTERNS_KEY);
            string[] folders = Helper.splitFilePath(path);
            for (int i = 0; i < patterns.Length; i++) {
                string pattern = patterns[i];
                pattern = pattern.Replace(ConfigKeyConstants.RegexMarker.EPISODE_TITLE, "*.?");
                //need \\d+ here instead of just \\d, to allow for seasons > 9
                pattern = pattern.Replace(ConfigKeyConstants.RegexMarker.SEASON_NR, "(?<Season>\\d+)");
                Match m = Regex.Match(folders[folders.Length - 1], pattern, RegexOptions.IgnoreCase);

                if (!m.Success) {
                    continue;
                }
                try {
                    return Int32.Parse(m.Groups["Season"].Value);
                }
                catch {
                    return -1;
                }
            }
            return -1;
        }


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
                }
                catch (Exception ex) {
                    Logger.Instance.LogMessage(ex.Message, LogLevel.ERROR);
                    return;
                }
                requestHtml.Timeout = Convert.ToInt32(Helper.ReadProperty(ConfigKeyConstants.CONNECTION_TIMEOUT_KEY));
                // get response
                HttpWebResponse responseHtml = null;
                try {
                    responseHtml = (HttpWebResponse)(requestHtml.GetResponse());
                }
                catch (Exception ex) {
                    Logger.Instance.LogMessage(ex.Message, LogLevel.ERROR);
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
            if (Settings.Instance.IsMonoCompatibilityMode) {
                Logger.Instance.LogMessage("Subtitle downloading is not supported in Mono, since additional dlls for unpacking are required which won't work here :(", LogLevel.WARNING);
                return;
            }
            SubtitleFileManager.Instance.ClearLinks();
            // request
            SubtitleProvider subprovider = SubtitleProvider.GetCurrentProvider();
            if (subprovider == null) {
                Logger.Instance.LogMessage("No subtitle provider found/selected", LogLevel.ERROR);
                return;
            }
            string url = subprovider.SearchUrl;
            if (url == null || url == "") {
                Logger.Instance.LogMessage("Can't search because no search URL is specified for this subtitle provider", LogLevel.ERROR);
                return;
            }
            url = url.Replace("%T", Helper.ReadProperties(ConfigKeyConstants.LAST_SEARCHED_SERIES_TITLES_KEY)[0]);
            url = System.Web.HttpUtility.UrlPathEncode(url);
            HttpWebRequest requestHtml;
            try {
                requestHtml = (HttpWebRequest)(HttpWebRequest.Create(url));
            }
            catch (Exception ex) {
                Logger.Instance.LogMessage(ex.Message, LogLevel.ERROR);
                return;
            }
            Logger.Instance.LogMessage("Searching at " + url.Replace(" ", "%20"), LogLevel.INFO);
            requestHtml.Timeout = Convert.ToInt32(Helper.ReadProperty(ConfigKeyConstants.CONNECTION_TIMEOUT_KEY));
            // get response
            HttpWebResponse responseHtml = null;
            try {
                responseHtml = (HttpWebResponse)(requestHtml.GetResponse());
            }
            catch (Exception ex) {
                Logger.Instance.LogMessage(ex.Message, LogLevel.ERROR);
                if (responseHtml != null)
                    responseHtml.Close();
                return;
            }
            //if search engine directs us straight to the result page, skip parsing search results
            string seriesURL = subprovider.SeriesUrl;
            if (responseHtml.ResponseUri.AbsoluteUri.Contains(seriesURL)) {
                Logger.Instance.LogMessage("Search engine forwarded directly to single result: " + responseHtml.ResponseUri.AbsoluteUri.Replace(" ", "%20") + subprovider.SubtitlesURL.Replace(" ", "%20"), LogLevel.INFO);
                //GetSubtitleFromSeriesPage(responseHtml.ResponseUri.AbsoluteUri + subprovider.SUBTITLES_URL_KEY);
            }
            else {

                // and download
                StreamReader r = null;
                try {
                    r = new StreamReader(responseHtml.GetResponseStream());
                }
                catch (Exception ex) {
                    if (r != null)
                        r.Close();
                    Logger.Instance.LogMessage(ex.Message, LogLevel.ERROR);
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
            //TODO: here!!!
            /*
            if (info.SubtitleLinks.Count > 0) {
                i = DownloadSubtitles();
                ProcessSubtitles(i);
                updateCandidateFiles();
                string folder = Helper.ReadProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY) + "TEMP" + i.ToString();
            }
             */

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
            /*foreach(Match m in mc){
                    MessageBox.Show("Match: "+m.Value+"\r\nPROVIDER_NAME_KEY: "+m.Groups["name"].Value+"\r\nyear: "+m.Groups["year"].Value+"\r\nlink: "+m.Groups["link"].Value);
            }*/
            if (mc.Count == 0) {
                Logger.Instance.LogMessage("No results found", LogLevel.INFO);
            }
            else if (mc.Count == 1) {
                string url = subprovider.SubtitlesPage;
                url = url.Replace("%L", mc[0].Groups["link"].Value);
                if (subprovider.ConstructLink != "") {
                    ConstructLinks(mc[0].Groups["link"].Value);
                }
                else {
                    //GetSubtitleFromSeriesPage(url);
                }
            }
            else {
                Logger.Instance.LogMessage("Search engine found multiple results at " + SourceURL.Replace(" ", "%20"), LogLevel.INFO);
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
                    }
                    else {
                        //GetSubtitleFromSeriesPage(url);
                    }
                }
            }
        }
    }
}
