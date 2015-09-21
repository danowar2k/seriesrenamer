using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using Renamer.Classes;
using Renamer.Classes.Util;
using Renamer.Classes.Configuration;
using Renamer.Classes.UI.Dialogs;
using System.Windows.Forms;
using BrightIdeasSoftware;
using System.ComponentModel;

namespace Renamer
{
    class MediaFileManager : IEnumerable
    {
        protected static MediaFileManager instance;
        private static object m_lock = new object();
        private BackgroundWorker currentWorker = null;
        private DoWorkEventArgs currentRenameEventArgs = null;
        private double ProgressAtCopyStart=0;
        private double ProgressAtCopyEnd = 0;
        public static MediaFileManager Instance {
            get {
                if (instance == null) {
                    lock (m_lock) { if (instance == null) instance = new MediaFileManager(); }
                }
                return instance;
            }
        }


        /// <summary>
        /// List of media files and associated renaming information
        /// </summary>
        protected List<MediaFile> mediaFiles = new List<MediaFile>();

        public void clear() {
            this.mediaFiles.Clear();
        }

        public MediaFile this[int index] {
            get {
                return this.mediaFiles[index];
            }
        }

        public int CountMediaFiles {
            get {
                return this.mediaFiles.Count;
            }
        }

		public void remove(List<MediaFile> oldMediaFiles) {
			foreach (MediaFile oldMediaFile in oldMediaFiles) {
				remove(oldMediaFile);
			}
		}

        public void remove(MediaFile oldMediaFile) {
            this.mediaFiles.Remove(oldMediaFile);
        }

        public int indexOf(MediaFile someMediaFile)
        {
            for (int i = 0; i < mediaFiles.Count; i++)
            {
                if (mediaFiles[i] == someMediaFile)
                {
                    return i;
                }
            }
            return -1;
        }

        public void removeMissingFileEntries() {
			List<MediaFile> invalidMediaFiles = new List<MediaFile>();
            foreach (MediaFile someMediaFile in mediaFiles) {
				if (!validMediaFile(someMediaFile)) {
					invalidMediaFiles.Add(someMediaFile);
				}
            }
			this.remove(invalidMediaFiles);
        }

		private bool validMediaFile(MediaFile someMediaFile) {
			string mediaFilepath = someMediaFile.FilePath.Path + Path.DirectorySeparatorChar + someMediaFile.EpisodeTitle;
			if (!File.Exists(mediaFilepath)) {
				return false;
			}
			return true;
		}

        /// <summary>
        /// creates names for all entries using season, episode and name and the target pattern
        /// </summary>
        public void createNewNames() {
            foreach (MediaFile someMediaFile in mediaFiles) {
                someMediaFile.createTargetName();
            }
        }

        /// <summary>
        /// Gets video files matching season and episode number
        /// </summary>
		/// <param name="showname">showname to search for</param>
		/// <param name="seasonNr">season to search for</param>
        /// <param name="episodeNr">episode to search for</param>
        /// <returns>List of all matching Candidates, never null, but may be empty</returns>
        public List<MediaFile> getMatchingTvShowVideos(string showname, int seasonNr, int episodeNr) {
            List<MediaFile> matchingTvShowVideos = new List<MediaFile>();
            foreach (MediaFile mediaFile in this.mediaFiles) {
				if (mediaFile.IsVideofile) {
					if (mediaFile.Showname == showname 
							&& mediaFile.SeasonNr == seasonNr 
							&& mediaFile.EpisodeNr == episodeNr) {
						matchingTvShowVideos.Add(mediaFile);
					}
				}
            }
            return matchingTvShowVideos;
        }

        /// <summary>
        /// Gets video files matching showname
        /// </summary>
		/// <param name="showname">showname to search for</param>
		/// <returns>List of all matching media files, never null, but may be empty</returns>
        public List<MediaFile> GetMediaFiles(string showname) {
            List<MediaFile> showMediaFiles = new List<MediaFile>();
            foreach (MediaFile mediaFile in this.mediaFiles) {
                if (mediaFile.Showname == showname) {
                    showMediaFiles.Add(mediaFile);
                }
            }
            return showMediaFiles;
        }

        /// <summary>
        /// Gets subtitle files matching season and episode number
        /// </summary>
		/// <param name="seasonNr">season to search for</param>
		/// <param name="episodeNr">episode to search for</param>
        /// <returns>List of all matching InfoEntries, never null, but may be empty</returns>
        public List<MediaFile> GetMatchingEpisodeSubtitles(int seasonNr, int episodeNr) {
            List<MediaFile> matchingSubtitles = new List<MediaFile>();
            foreach (MediaFile mediaFile in this.mediaFiles) {
				if (mediaFile.IsSubtitle) {
					if (mediaFile.SeasonNr == seasonNr && mediaFile.EpisodeNr == episodeNr) {
						matchingSubtitles.Add(mediaFile);
					}
				}
            }
            return matchingSubtitles;
        }

        /// <summary>
        /// Gets video file matching to subtitle
        /// </summary>
		/// <param name="subtitleFile">subtitle file to find matching video file for</param>
        /// <returns>Matching video file</returns>
        public MediaFile GetVideo(MediaFile subtitleFile) {
            foreach (MediaFile mediaFile in this.mediaFiles) {
				if (mediaFile.IsVideofile) {
					if (Path.GetFileNameWithoutExtension(subtitleFile.Filename) == Path.GetFileNameWithoutExtension(mediaFile.Filename)) {
						return mediaFile;
					}
				}
            }
            return null;
        }



        /// <summary>
        /// Gets subtitle file matching to video file
        /// </summary>
        /// <param name="videoFile">Video file to find matching subtitle file for</param>
        /// <returns>Matching subtitle file</returns>
        public MediaFile GetSubtitleFile(MediaFile videoFile) {
            foreach (MediaFile mediaFile in this.mediaFiles) {
				if (mediaFile.IsSubtitle) {
					if (Path.GetFileNameWithoutExtension(videoFile.Filename) == Path.GetFileNameWithoutExtension(mediaFile.Filename)) {
						return mediaFile;
					}
				}
            }
            return null;
        }

		// FIXME: This method is stupid
        /// <summary>
        /// Get a video file matching season and episode number
        /// 
        /// </summary>
		/// <param name="seasonNr">season nr to search for</param>
		/// <param name="episodeNr">episode nr to search for</param>
        /// <returns>Matching video file, null if not found or more than one found</returns>
        public MediaFile GetMatchingVideo(string showname, int seasonNr, int episodeNr) {
            List<MediaFile> matchingVideoFiles = getMatchingTvShowVideos(showname, seasonNr, episodeNr);
            MediaFile onlyMatch = null;
			if (matchingVideoFiles.Count == 1) {
				onlyMatch = matchingVideoFiles[0];
			}
            return onlyMatch;
        }

        #region IEnumerable Member

        public IEnumerator GetEnumerator() {
            return this.mediaFiles.GetEnumerator();
        }

        #endregion

        internal void Add(MediaFile newMediaFile) {
            this.mediaFiles.Add(newMediaFile);
        }

        public MediaFile GetCollidingMediaFile(MediaFile mediaFileToCheck){
            foreach(MediaFile otherMediaFile in this){
                if(IsSameTarget(mediaFileToCheck,otherMediaFile)){
                    return otherMediaFile;
                }
            }
            return null;
        }
        /// <summary>
        /// figures out if 2 media file target path+filename collide
        /// </summary>
        /// <param name="otherMediaFile"></param>
        /// <param name="someMediaFile"></param>
        /// <returns></returns>
		public bool IsSameTarget(MediaFile someMediaFile, MediaFile otherMediaFile)
        {
			if (someMediaFile == otherMediaFile) {
				return false;
			}
            string someFilePath;
            if (someMediaFile.DestinationPath == "") {
                someFilePath = someMediaFile.FilePath.Path;
            } else {
                someFilePath = someMediaFile.DestinationPath;
				if (!someMediaFile.ProcessingRequested) {
					return false;
				}
            }

			string otherFilePath;
			if (otherMediaFile.DestinationPath == "") {
                otherFilePath = otherMediaFile.FilePath.Path;
            } else {
                otherFilePath = otherMediaFile.DestinationPath;
				if (!otherMediaFile.ProcessingRequested) {
					return false;
				}
            }

			string someFilename;
			if (someMediaFile.NewFilename == "") {
                someFilename = someMediaFile.Filename;
            } else {
                someFilename = someMediaFile.NewFilename;
				if (!someMediaFile.ProcessingRequested) {
					return false;
				}
            }

			string otherFilename;
			if (otherMediaFile.NewFilename == "") {
                otherFilename = otherMediaFile.Filename;
            } else {
                otherFilename = otherMediaFile.NewFilename;
				if (!otherMediaFile.ProcessingRequested) {
					return false;
				}
            }

            return (someFilename == otherFilename && someFilePath == otherFilePath);
        }

		/// <summary>
		/// This checks if the file is will be copied by checking if drive letters change.
		/// (This is the first letter that is checked)
		/// </summary>
		/// <param name="someMediaFile"></param>
		/// <returns></returns>
		private bool mediaFileChangesPartition(MediaFile someMediaFile) {
			if (someMediaFile.ProcessingRequested && someMediaFile.DestinationPath != "") {
				char sourcePartitionLetter = someMediaFile.FilePath.Fullfilename.ToLower()[0];
				char targetPartitionLetter = someMediaFile.DestinationPath.ToLower()[0];
				if (sourcePartitionLetter != targetPartitionLetter) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// This counts how many file bytes are copied during renaming.
		/// </summary>
		/// <returns></returns>
		private double countCopiedFileBytes() {
			double fileBytesSum = 0;
			foreach (MediaFile mediaFile in mediaFiles) {
				if (mediaFileChangesPartition(mediaFile)) {
					FileInfo mediaFileInfo = new FileInfo(mediaFile.FilePath.Fullfilename);
					fileBytesSum += mediaFileInfo.Length;
				}
			}
			return fileBytesSum;
		}

		private bool renamingCancelled(BackgroundWorker someWorker, DoWorkEventArgs renameEventArgs) {
			if (someWorker.CancellationPending) {
				renameEventArgs.Cancel = true;
				Logger.Instance.LogMessage("Renaming Cancelled.", LogLevel.INFO);
				return true;
			}
			return false;
		}

		private void deleteMediaFiles(List<MediaFile> filesToDelete) {
			foreach (MediaFile fileToDelete in filesToDelete) {
				deleteMediaFiles(filesToDelete);
			}
		}

		private void deleteMediaFile(MediaFile fileToDelete) {
			try {
				File.Delete(fileToDelete.FilePath.Fullfilename);
				mediaFiles.Remove(fileToDelete);
			} catch (Exception ex) {
				Logger.Instance.LogMessage("Couldn't delete " + fileToDelete.FilePath.Fullfilename + ": " + ex.Message, LogLevel.ERROR);
			}
		}

		private bool fileWillBeDeleted(MediaFile someFile) {
			if (someFile.MarkedForDeletion && someFile.ProcessingRequested) {
				return true;
			}
			return false;
		}

		private void setProgress(double copiedBytes, double copiedFileBytesSum, MediaFile mediaFile, bool willBeCopied) {
			if (copiedFileBytesSum > 0) {
				ProgressAtCopyStart = (copiedBytes / copiedFileBytesSum * 100);
				if (willBeCopied) {
					double fileSize = (double) getFilesize(mediaFile);
					ProgressAtCopyEnd = Math.Min(ProgressAtCopyStart + (fileSize / copiedFileBytesSum * 100), 100);
				} else {
					ProgressAtCopyEnd = ProgressAtCopyStart;
				}
			} else {
				ProgressAtCopyStart = mediaFiles.IndexOf(mediaFile);
				ProgressAtCopyEnd = mediaFiles.IndexOf(mediaFile);
			}
		}

		private bool notVisibleAfterRenaming(MediaFile mediaFile) {
			int destinationDirDepth = Filepath.GetSubdirectoryLevel(Helper.ReadProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY), mediaFile.DestinationPath);
			if (destinationDirDepth == -1) {
				return true;
			} else {
				int maxDirDepth = Helper.ReadInt(ConfigKeyConstants.MAX_SEARCH_DEPTH_KEY);
				return destinationDirDepth > maxDirDepth;
			}
		}

		private long getFilesize(MediaFile mediaFile) {
			return new FileInfo(mediaFile.FilePath.Fullfilename).Length;
		}

		/// <summary>
        /// Main Rename function
        /// </summary>
        public void Rename(BackgroundWorker someWorker, DoWorkEventArgs someRenameEventArgs) {
            this.currentWorker = someWorker;
            this.currentRenameEventArgs = someRenameEventArgs;

			double copiedFileBytesSum = countCopiedFileBytes();

			double copiedBytes = 0;
			//Go through all files and do stuff
//            for (int i = 0; i < this.mediaFiles.Count; i++) {
			List<MediaFile> filesToCheck = new List<MediaFile>(mediaFiles);
			foreach (MediaFile mediaFile in filesToCheck) {
				if (renamingCancelled(someWorker, someRenameEventArgs)) {
					return;
				}

				if (fileWillBeDeleted(mediaFile)) {
					deleteMediaFile(mediaFile);
				}

                //need to set before ie.Rename because it resets some conditions
                bool willBeCopied = mediaFileChangesPartition(mediaFile);
				
				//if there are files which actually need to be copied since they're on a different drive, only count those for the progress bar status since they take much longer
				setProgress(copiedBytes, copiedFileBytesSum, mediaFile, willBeCopied);
                
                someWorker.ReportProgress((int)ProgressAtCopyStart);

				bool removeFromSet = notVisibleAfterRenaming(mediaFile);
                //this call will also report progress more detailed by calling ReportSingleFileProgress()
                mediaFile.Rename(someWorker, someRenameEventArgs);
				if (removeFromSet) {
					mediaFiles.Remove(mediaFile);
                }
                //after file is (really) copied, add its size
                if (willBeCopied) {
                    copiedBytes += getFilesize(mediaFile);
                }
            }

            if (Helper.ReadBool(ConfigKeyConstants.DELETE_EMPTIED_FOLDERS_KEY)) {
                //Delete all empty folders code
                Helper.DeleteAllEmptyFolders(Helper.ReadProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY), new List<string>(Helper.ReadProperties(ConfigKeyConstants.EMPTY_FOLDER_CHECK_IGNORED_FILETYPES_KEY)),someWorker,someRenameEventArgs);
            }
			logEndOfRenaming(someRenameEventArgs.Cancel);
        }

		private void logEndOfRenaming(bool renamingCancelled) {
			if (!renamingCancelled) {
				Logger.Instance.LogMessage("Renaming (and possibly moving and deleting) finished!", LogLevel.INFO);
			} else {
				Logger.Instance.LogMessage("Stopped Renaming.", LogLevel.INFO);
			}
		}

        public CopyFileCallbackAction ReportSingleFileProgress(
			FileInfo source, FileInfo destination, 
			object state, long totalFileSize, long totalBytesTransferred) {
            currentWorker.ReportProgress((int)(ProgressAtCopyStart + (double)totalBytesTransferred / (double)totalFileSize * (ProgressAtCopyEnd - ProgressAtCopyStart)));
            if (currentWorker.CancellationPending)
            {
                currentRenameEventArgs.Cancel = true;
                return CopyFileCallbackAction.Cancel;
            }
            return CopyFileCallbackAction.Continue;
        }

        public void renameShow(string from, string to) {
            from = from.ToLower();
            foreach (MediaFile mediaFile in mediaFiles) {
                if (mediaFile.Showname.ToLower() == from) {
                    mediaFile.Showname = to;
                }
            }
        }

        public bool containsShow(string showname) {
            showname = showname.ToLower();
            foreach (MediaFile mediaFile in mediaFiles) {
                if (mediaFile.Showname.ToLower() == showname) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Decides which files should be marked for processing
        /// </summary>
		/// <param name="showname">user entered showname</param>
        public void SelectSimilarFilesForProcessing(string showname) {
            List<MediaFile> matches = FindSimilarByName(showname);

            foreach (MediaFile mediaFile in this.mediaFiles) {
                if (matches.Contains(mediaFile)) {
                    mediaFile.ProcessingRequested = true;
                    mediaFile.IsMovie = false;
                }
                else {
                    mediaFile.ProcessingRequested = false;
                }
            }
        }

        /// <summary>
        /// Sets new episode to some files and takes care of storing it properly (last [TITLE_HISTORY_SIZE_KEY] Titles are stored)
        /// </summary>
		/// <param name="someMediaFiles">files to which this episode should be set to</param>
		/// <param name="newShowname">name to be set</param>
        public void setNewShowname(List<MediaFile> someMediaFiles, string newShowname) {
            foreach (MediaFile mediaFile in someMediaFiles) {
                if (mediaFile.Showname != newShowname) {
                    mediaFile.Showname = newShowname;
                }
            }

			List<string> lastSearchedShownames = new List<string>(Helper.ReadProperties(ConfigKeyConstants.LAST_SEARCHED_SHOWNAMES_KEY));
			//check if list of shownames contains new showname
            int Index = -1;
            for (int i = 0; i < lastSearchedShownames.Count; i++) {
                string searchedShowname = lastSearchedShownames[i];
                if (searchedShowname == newShowname) {
                    Index = i;
                    break;
                }
            }

            //if the episode is new
            if (Index == -1) {
				lastSearchedShownames.Insert(0, newShowname);
            }
            //if the episode is in the list already, bring it to the front
            else {
				lastSearchedShownames.RemoveAt(Index);
				lastSearchedShownames.Insert(0, newShowname);
            }
			int shownameHistorySize = Helper.ReadInt(ConfigKeyConstants.SHOWNAME_HISTORY_SIZE_KEY);
			Helper.WriteProperties(ConfigKeyConstants.LAST_SEARCHED_SHOWNAMES_KEY,
				lastSearchedShownames.GetRange(0, Math.Min(lastSearchedShownames.Count, shownameHistorySize)).ToArray());
		}

        /// <summary>
        /// Sets a new path for the list view
        /// </summary>
		/// <param name="newPath">Path to be set</param>
        public void setPath(ref string newPath) {
            if (newPath == null || newPath == "" || !Directory.Exists(newPath))
                return;

            if (newPath.Length == 2) {
                if (char.IsLetter(newPath[0]) && newPath[1] == ':') {
                    newPath = newPath + Path.DirectorySeparatorChar;
                }
            }

			//Skip this for now on networks
            
            if (!isNetworkPath(newPath)) {
                //fix casing of the path if user entered it
				newPath = fixPathCase(newPath);
            }
            //Same path, ignore
            if (Helper.ReadProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY).ToLower() == newPath.ToLower()) {
                return;
            }
            else {
                Helper.WriteProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY, newPath);
                Environment.CurrentDirectory = newPath;
            }
        }

		private bool isNetworkPath(string somePath) {
			return somePath.StartsWith("\\\\");
		}

		private string fixPathCase(string somePath) {
			DirectoryInfo currentPath = new DirectoryInfo(somePath);
			string fixedPath = "";
			while (currentPath.Parent != null) {
				fixedPath = currentPath.Parent.GetDirectories(currentPath.Name)[0].Name + Path.DirectorySeparatorChar + fixedPath;
				currentPath = currentPath.Parent;
			}
			fixedPath = currentPath.Name.ToUpper() + fixedPath;
			fixedPath = fixedPath.TrimEnd(new char[] { Path.DirectorySeparatorChar });
			if (fixedPath.Length == 2) {
				if (char.IsLetter(fixedPath[0]) && fixedPath[1] == ':') {
					fixedPath = fixedPath + Path.DirectorySeparatorChar;
				}
			}
			return fixedPath;
		}

        /// <summary>
        /// Finds similar files by looking at the file path and comparing it to a showname
        /// </summary>
        /// <param name="Basepath">basepath of the show</param>
		/// <param name="showname">name of the show to filter</param>
        /// <param name="source">source files</param>
        /// <returns>a list of matches</returns>
        public List<MediaFile> FindSimilarByName(string showname) {
            List<MediaFile> matches = new List<MediaFile>();
            showname = showname.ToLower();
            //whatever, just check path and file path if it contains the showname
            foreach (MediaFile mediaFile in this.mediaFiles) {

                string lowerCaseFilename = mediaFile.Filename.ToLower();

                //try to extract the name from a shortcut, i.e. sga for Stargate Atlantis
                string noWordCharPattern = "[^\\w]";
                Match findWhitespaces = Regex.Match(lowerCaseFilename, noWordCharPattern, RegexOptions.IgnoreCase);
                if (findWhitespaces != null && findWhitespaces.Success) {
                    string firstWord = lowerCaseFilename.Substring(0, findWhitespaces.Index);
                    if (firstWord.Length > 0 && Helper.ContainsLetters(firstWord, showname)) {
						//FIXME: this should probably be removed, because I can think of a thousand times where this will break
                        matches.Add(mediaFile);
                        continue;
                    }
                }

                //now check if whole showname is in the file path
                string CleanupRegex = Helper.ReadProperty(ConfigKeyConstants.REGEX_SHOWNAME_CLEANUP_KEY);
                lowerCaseFilename = Regex.Replace(lowerCaseFilename, CleanupRegex, " ");
                if (lowerCaseFilename.Contains(showname)) {
                    matches.Add(mediaFile);
                    continue;
                }

				string[] parentFolders = Helper.getParentFolderNames(mediaFile.FilePath.Path);
				//or in some top folder
                foreach (string str in parentFolders) {
                    lowerCaseFilename = str.ToLower();
                    lowerCaseFilename = Regex.Replace(lowerCaseFilename, CleanupRegex, " ");
                    if (lowerCaseFilename.Contains(showname)) {
                        matches.Add(mediaFile);
                        break;
                    }
                }
            }
            return matches;
        }

        public void clearEpisodeNamesOfShow(string showname) {
            foreach (MediaFile mediaFile in mediaFiles) {
                if (mediaFile.Showname == showname) {
                    mediaFile.EpisodeTitle = "";
                }
            }
        }
        public void clearEpisodeNames() {
            foreach (MediaFile mediaFile in mediaFiles) {
                mediaFile.EpisodeTitle = "";
            }
        }
    }
}
