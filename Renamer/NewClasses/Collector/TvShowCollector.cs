using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Renamer.NewClasses.Config.Application;
using Renamer.NewClasses.Providers;
using Renamer.NewClasses.Network;

// Datensammler verwendet Datenbereithalter
// Datenbereithalter kann auf mehrere Arten verwendet werden
// Die Standard-Verwendung ist über die URLs und das Parsen des HTML
// TVDB kann auch Verwendung über API
namespace Renamer.NewClasses.Collector
{
    public class TvShowCollector
    {
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger
	(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


		public static void getAllRelations(List<ParsedSearch> searchResults, BackgroundWorker worker, DoWorkEventArgs e) {
			int progress = 0;
			foreach (ParsedSearch ps in searchResults) {
				if (ps.Results != null && ps.Results.Count > 0) {
					if (worker.CancellationPending) {
						e.Cancel = true;
						break;
					}
					GetRelations((string)ps.Results[ps.SelectedResult], ps.Showname, ps.provider, worker, e);
				}
				progress++;
				worker.ReportProgress((int)((double)progress / ((double)searchResults.Count) * 100));
			}
		}

		public static void SetProxy(HttpWebRequest client, string url) {
			// Comment out foreach statement to use normal System.Net proxy detection 
			foreach (
					Uri address
					in WinHttpSafeNativeMethods.getProxiesForUrl(new Uri(url))) {
				client.Proxy = new WebProxy(address);
				break;
			}
		}



		//parse page(s) containing relations
		/// <summary>
		/// Parses page containing the relation data
		/// </summary>
		/// <param name="url">URL of the page to parse</param>
		/// <param name="Showname">Showname</param>
		public static void GetRelations(string url, string Showname, TvShowInfoProvider provider, BackgroundWorker worker, DoWorkEventArgs dwea) {
			if (provider == null) {
				log.Error("GetRelations: No relation provider found/selected");
				return;
			}
			if (String.IsNullOrEmpty(url)) {
				log.Error("GetRelations: URL is null or empty");
				return;
			}
			if (String.IsNullOrEmpty(Showname)) {
				log.Error("GetRelations: Showname is null or empty");
				return;
			}
			log.Debug("Trying to get relations from " + url);
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
				log.Debug("Trying to get relations for season " + season + " from " + url);
				HttpWebRequest requestHtml = null;
				try {
					requestHtml = (HttpWebRequest)(HttpWebRequest.Create(url));
				} catch (Exception ex) {
					log.Error(ex.Message);
					requestHtml.Abort();
					return;
				}
				requestHtml.Timeout = Helper.ReadInt(AppProperties.CONNECTION_TIMEOUT_KEY);
				// get response
				HttpWebResponse responseHtml = null;
				try {
					responseHtml = (HttpWebResponse)(requestHtml.GetResponse());
				} catch (WebException ex) {
					//Serienjunkies returns "(300) Mehrdeutige Umleitung" when an inexistant season is requested
					if (ex.Message.Contains("(300)"))
						break;
					log.Error(ex.Message);
					if (responseHtml != null) {
						responseHtml.Close();
					}
					return;
				}
				log.Debug("Response URL: " + responseHtml.ResponseUri.AbsoluteUri);
				//if we get redirected, lets assume this page does not exist
				if (responseHtml.ResponseUri.AbsoluteUri != url) {
					log.Debug("Response URL doesn't match request URL, page doesn't seem to exist");
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
					} catch (Exception ex) {
						log.Error("Invalid encoding in config file: " + ex.Message);
						enc = Encoding.GetEncoding(responseHtml.CharacterSet);
					}
				} else {
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
				if (!Settings.Instance.isMonoCompatibilityMode) {
					log.Debug("Trying to match source from " + responseHtml.ResponseUri.AbsoluteUri + " with " + pattern);
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
					if (result != "") {
						//parse season and episode numbers
						int s, e;

						Int32.TryParse(m.Groups["Season"].Value, out s);
						Int32.TryParse(m.Groups["Episode"].Value, out e);
						//if we are iterating through season pages, take season from page url directly
						if (url != url2) {
							rc.AddEpisode(new ShowEpisode(season, e, result));
							log.Debug("Found Relation: " + "S" + s.ToString() + "E" + e.ToString() + " - " + result);
						} else {
							rc.AddEpisode(new ShowEpisode(s, e, result));
							log.Debug("Found Relation: " + "S" + s.ToString() + "E" + e.ToString() + " - " + result);
						}
					}
				}
				TvShowManager.Instance.addTvShow(rc);

				// THOU SHALL NOT FORGET THE BREAK
				if (!url2.Contains("%S"))
					break;
				season++;
			}
			log.Debug("" + (season - 1) + " Seasons, " + rc.CountEpisodes + " relations found");
		}

		/// <summary>
		/// Updatess list view and do lots of other connected stuff with it
		/// </summary>
		/// <param name="clear">if true, list is cleared first and unconnected subtitle files are scheduled to be renamed</param>
		public static void UpdateList(bool clear, BackgroundWorker worker, DoWorkEventArgs e) {
			MediaFileManager infoManager = MediaFileManager.Instance;

			// Clear list if desired, remove deleted files otherwise
			if (clear) {
				infoManager.clear();
			} else {
				infoManager.removeMissingFileEntries();
			}

			// read path from config && remove tailing slashes
			string path = Helper.ReadProperty(AppProperties.LAST_DIRECTORY_KEY);
			path = path.TrimEnd(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });
			log.Debug("Opening folder " + path);
			bool CreateDirectoryStructure = Helper.ReadBool(AppProperties.CREATE_DIRECTORY_STRUCTURE_KEY);
			bool UseSeasonSubdirs = Helper.ReadBool(AppProperties.CREATE_SEASON_SUBFOLDERS_KEY);

			if (Directory.Exists(path)) {
				//scan for new files
				List<string> extensions = new List<string>(Helper.ReadProperties(AppProperties.VIDEO_FILE_EXTENSIONS_KEY));
				extensions.AddRange(Helper.ReadProperties(AppProperties.SUBTITLE_FILE_EXTENSIONS_KEY));

				//convert all extensions to lowercase
				for (int i = 0; i < extensions.Count; i++) {
					extensions[i] = extensions[i].ToLower();
				}
				//read all files with matching extension
				List<FileSystemInfo> Files = new List<FileSystemInfo>();
				int count = 0;
				foreach (string ex in extensions) {
					Files.AddRange(Helper.getAllFilesRecursively(path, "*." + ex, ref count, worker));
				}


				if (worker.CancellationPending) {
					e.Cancel = true;
					log.Info("Cancelled opening folder.");
					return;
				}

				if (MainForm.Instance.InvokeRequired) {
					MainForm.Instance.Invoke(new EventHandler(delegate {
						MainForm.Instance.lblFileListingProgress.Visible = false;
						MainForm.Instance.taskProgressBar.Visible = true;
					}));
				} else {
					MainForm.Instance.lblFileListingProgress.Visible = false;
					MainForm.Instance.taskProgressBar.Visible = true;
				}

				//some declarations already for speed
				string[] patterns = Helper.ReadProperties(AppProperties.EPISODE_IDENTIFIER_PATTERNS_KEY);
				for (int i = 0; i < patterns.Length; i++) {
					patterns[i] = RegexConverter.toRegex(patterns[i]);
				}
				int DirectorySeason = -1;
				MediaFile ie = null;
				bool contains = false;
				DateTime dt;
				string currentpath = "";
				string MovieIndicator = String.Join("|", Helper.ReadProperties(AppProperties.MOVIE_INDICATOR_STRINGS_KEY));
				//Form1.Instance.taskProgressBar.Maximum = Files.Count;
				for (int f = 0; f < Files.Count; f++) {
					if (worker.CancellationPending) {
						e.Cancel = true;
						log.Info("Cancelled opening folder.");
						return;
					}
					//Form1.Instance.taskProgressBar.Value=f;
					worker.ReportProgress((int)((double)f / ((double)Files.Count) * 100));
					FileSystemInfo file = Files[f];
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
					if (Regex.Match(currentpath, MovieIndicator).Success) {
						ie.isMovie = true;
					}
					//Set basic values, by setting those values destination directory and configurationFilePath will be generated automagically                    
					ie.FilePath.Path = currentpath;
					ie.Filename = file.Name;
					ie.Extension = Path.GetExtension(file.FullName).ToLower().Replace(".", "");


					if (!ie.isMovie) {
						//Get season number and showname from directory
						DirectorySeason = ExtractSeasonFromDirectory(Path.GetDirectoryName(file.FullName));
						dt = DateTime.Now;

						//try to recognize season and episode from configurationFilePath
						///////////////////////////////////////////////////
						ExtractSeasonAndEpisode(ie, patterns);
						///////////////////////////////////////////////////

						//Need to do this for filenames which only contain episode numbers (But might be moved in a season dirArgument already)
						//if season number couldn't be extracted, try to get it from folder
						if (ie.SeasonNr <= 0 && DirectorySeason != -1) {
							ie.SeasonNr = DirectorySeason;
						}

						//if season recognized from directory name doesn't match season recognized from configurationFilePath, the file might be located in a wrong directory
						if (DirectorySeason != -1 && ie.SeasonNr != DirectorySeason) {
							log.Warn("File seems to be located inconsistently: " + ie.Filename + " was recognized as season " + ie.SeasonNr + ", but folder name indicates that it should be season " + DirectorySeason.ToString());
						}


					}

					//if nothing could be recognized, assume that this is a movie
					if ((ie.SeasonNr < 1 && ie.EpisodeNr < 1) || ie.isMovie) {
						ie.RemoveVideoTags();
					}

					//if not added yet, add it
					if (!contains) {
						MediaFileManager.Instance.Add(ie);
					}
				}
				//SelectSimilarFilesForProcessing(path,Helper.ReadProperties(AppProperties.LAST_SEARCHED_SERIES_TITLES_KEY)[0]);
				SelectRecognizedFilesForProcessing();
			}

			//Recreate subtitle names so they can adjust to the video files they belong to
			foreach (MediaFile ie in MediaFileManager.Instance) {
				if (ie.IsSubtitle) {
					ie.createTargetName();
				}
			}

			log.Info("Found " + MediaFileManager.Instance.CountMediaFiles + " Files");
			if (Helper.ReadBool(AppProperties.REPORT_MISSING_EPISODES_KEY)) {
				FindMissingEpisodes();
			}
		}
	}
}