using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using Renamer.Classes;
using Renamer.Classes.Util;
using Renamer.NewClasses.Config.Application;

namespace Renamer.NewClasses.Collector {
	public class LocalCollector {
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger
	(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static void GetAllTitles(MediaFileManager manager, BackgroundWorker worker, DoWorkEventArgs e) {
			GetAllTitles(manager.GetMediaFiles("*"), worker, e);
		}

		public static void GetAllTitles(List<MediaFile> mediaFiles, BackgroundWorker worker, DoWorkEventArgs e) {
			List<string> shownames = new List<string>();
			foreach (MediaFile ie in mediaFiles) {
				if (ie.ProcessingRequested && !ie.isMovie && ie.Showname != "" && !ie.IsSample && !shownames.Contains(ie.Showname)) {
					shownames.Add(ie.Showname);
				}
			}
			TvShowCollector.searchForShows(worker, e);
		}
		/// <summary>
		/// Extracts season and episode number by using some regexes from config file
		/// </summary>
		/// <param name="ie">Candidate which should be processed</param>
		public static void ExtractSeasonAndEpisode(MediaFile ie) {
			string[] patterns = Helper.ReadProperties(AppProperties.EPISODE_IDENTIFIER_PATTERNS_KEY);
			for (int i = 0; i < patterns.Length; i++) {
				patterns[i] = RegexConverter.toRegex(patterns[i]);
			}
			ExtractSeasonAndEpisode(ie, patterns);
		}

		/// <summary>
		/// Extracts season and episode number by using some regexes specified in patterns
		/// </summary>
		/// <param name="ie">Candidate which should be processed</param>
		/// <param name="patterns">Patterns to be used for recognition, supply these for speed reasons?</param>
		public static void ExtractSeasonAndEpisode(MediaFile ie, string[] patterns) {
			string strSeason = "";
			string strEpisode = "";
			Match m = null;
			int episode = -1;
			int season = -1;
			log.Debug("Extracting season and episode from " + ie.FilePath + Path.DirectorySeparatorChar + ie.Filename);
			//[MOVIES_TAGS_TO_REMOVE_LIST_KEY] and (CRCXXXXX) are removed for recognition, since they might produce false positives
			string cleanedname = Regex.Replace(ie.Filename, "\\[.*?\\]|\\(.{8}\\)", "");
			foreach (string pattern in patterns) {
				//Try to match. If it works, get the season and the episode from the match
				m = Regex.Match(cleanedname, pattern, RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
				if (m.Success) {
					log.Debug("This pattern produced a match: " + pattern);
					strSeason = "";
					strEpisode = "";
					//ignore numbers like 2063, chance is higher they're a year than a valid season/ep combination
					if (Int32.Parse(m.Groups["Season"].Value + m.Groups["Episode"].Value) > 2000 && (m.Groups["Season"].Value + m.Groups["Episode"].Value).StartsWith("20")) {
						continue;
					}
					try {
						strSeason = Int32.Parse(m.Groups["Season"].Value).ToString();
					} catch (FormatException) {
					}
					try {
						strEpisode = Int32.Parse(m.Groups["Episode"].Value).ToString();
					} catch (FormatException) {
					}
					//Fix for .0216. notation for example, 4 numbers should always be recognized as %S%S%E%E (IF THEY ARENT A YEAR!)
					if (strEpisode.Length == 3 && strSeason.Length == 1) {
						strSeason += strEpisode[0];
						strEpisode = strEpisode.Substring(1);
						if (strSeason[0] == '0') {
							strSeason = strSeason.Substring(1);
						}
					}

					try {
						episode = Int32.Parse(strEpisode);
					} catch {
						log.Debug("Cannot parse found episode: " + strEpisode);
					}
					try {
						season = Int32.Parse(strSeason);
					} catch {
						log.Debug("Cannot parse found season: " + strSeason);
					}


					if ((ie.Filename.ToLower().Contains("720p") && season == 7 && episode == 20) | (ie.Filename.ToLower().Contains("1080p") && season == 10 && episode == 80)) {
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
		private static void FindMissingEpisodes() {
			Hashtable paths = new Hashtable();

			foreach (MediaFile ie in MediaFileManager.Instance) {
				if (!string.IsNullOrEmpty(ie.Showname)) {
					if (paths.ContainsKey(ie.Showname + ie.SeasonNr)) {
						if (((EpisodeCollection)paths[ie.Showname + ie.SeasonNr]).maxEpisode < ie.EpisodeNr) {
							((EpisodeCollection)paths[ie.Showname + ie.SeasonNr]).maxEpisode = ie.EpisodeNr;
						}
						if (((EpisodeCollection)paths[ie.Showname + ie.SeasonNr]).minEpisode > ie.EpisodeNr) {
							((EpisodeCollection)paths[ie.Showname + ie.SeasonNr]).minEpisode = ie.EpisodeNr;
						}
						((EpisodeCollection)paths[ie.Showname + ie.SeasonNr]).entries.Add(ie);
					} else {
						EpisodeCollection ec = new EpisodeCollection();
						ec.maxEpisode = ie.EpisodeNr;
						ec.minEpisode = ie.EpisodeNr;
						ec.entries.Add(ie);
						paths.Add(ie.Showname + ie.SeasonNr, ec);
					}
				}
			}
			foreach (string key in paths.Keys) {
				int missing = 0;
				string message = "Missing episodes in " + ((EpisodeCollection)paths[key]).entries[0].Showname + " Season " + ((EpisodeCollection)paths[key]).entries[0].SeasonNr + ": ";
				string premessage = "";
				string postmessage = "";
				for (int i = 1; i <= ((EpisodeCollection)paths[key]).maxEpisode; i++) {
					bool found = false;
					foreach (MediaFile ie in ((EpisodeCollection)paths[key]).entries) {
						if (ie.EpisodeNr == i) {
							found = true;
							break;
						}
					}
					if (!found) {
						if (i < ((EpisodeCollection)paths[key]).minEpisode) {
							premessage += String.Format("E{0:00}, ", i);
						} else {
							postmessage += String.Format("E{0:00}, ", i);
						}
						missing += 1;
					}
				}

				if (missing > 0) {
					//if not too many missing episodes were found, add the lower missing episodes too
					if (missing < ((EpisodeCollection)paths[key]).entries.Count) {
						message += premessage;
					}
					message += postmessage;

					//if something is to be reported
					if (postmessage != "" || missing < ((EpisodeCollection)paths[key]).entries.Count) {
						message = message.Substring(0, message.Length - 2);
						log.Info(message);
					}
				}
			}
		}
		private static void SelectRecognizedFilesForProcessing() {
			foreach (MediaFile ie in MediaFileManager.Instance) {
				if (ie.IsSample && !ie.MarkedForDeletion) {
					ie.ProcessingRequested = false;
					ie.isMovie = false;
				} else {
					ie.ProcessingRequested = (ie.SeasonNr != -1 && ie.EpisodeNr != -1) || (ie.isMovie && ((ie.DestinationPath != "" && ie.DestinationPath != ie.FilePath.Path) || (ie.NewFilename != "" && ie.NewFilename != ie.Filename)));
				}
			}
		}

		/// <summary>
		/// Extracts season from directory name
		/// </summary>
		/// <param name="path">path from which to extract the data (NO FILEPATH, JUST FOLDER)</param>
		/// <returns>recognized season, -1 if not recognized</returns>
		public static int ExtractSeasonFromDirectory(string path) {
			string[] patterns = Helper.ReadProperties(AppProperties.SEASON_NR_EXTRACTION_PATTERNS_KEY);
			string[] folders = Helper.getFolderNames(path);
			for (int i = 0; i < patterns.Length; i++) {
				string pattern = patterns[i];
				pattern = pattern.Replace(AppProperties.RegexMarker.EPISODE_TITLE_MARKER, "*.?");
				//need \\d+ here instead of just \\d, to allow for seasons > 9
				pattern = pattern.Replace(AppProperties.RegexMarker.SEASON_NR_MARKER, "(?<Season>\\d+)");
				Match m = Regex.Match(folders[folders.Length - 1], pattern, RegexOptions.IgnoreCase);

				if (!m.Success) {
					continue;
				}
				try {
					return Int32.Parse(m.Groups["Season"].Value);
				} catch {
					return -1;
				}
			}
			return -1;
		}
	}
}
