using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using Renamer.Classes.Logging;
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
        private BackgroundWorker worker = null;
        private DoWorkEventArgs dwea = null;
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
			string mediaFilepath = someMediaFile.FilePath.Path + Path.DirectorySeparatorChar + someMediaFile.Name;
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
        /// <param name="season">season to search for</param>
        /// <param name="episode">episode to search for</param>
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
            if (someMediaFile.Destination == "") {
                someFilePath = someMediaFile.FilePath.Path;
            } else {
                someFilePath = someMediaFile.Destination;
				if (!someMediaFile.ProcessingRequested) {
					return false;
				}
            }

			string otherFilePath;
			if (otherMediaFile.Destination == "") {
                otherFilePath = otherMediaFile.FilePath.Path;
            } else {
                otherFilePath = otherMediaFile.Destination;
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
        /// Main Rename function
        /// </summary>
        public void Rename(BackgroundWorker worker, DoWorkEventArgs e) {
            this.worker = worker;
            this.dwea = e;

            long TotalBytes = 0;
            long Bytes = 0;
            foreach (MediaFile ie in mediaFiles)
            {
                if (ie.ProcessingRequested && ie.Destination != "" && ie.FilePath.Fullfilename.ToLower()[0] != ie.Destination.ToLower()[0])
                {
                    FileInfo info = new FileInfo(ie.FilePath.Fullfilename);
                    TotalBytes += info.Length;
                }
            }
            //Go through all files and do stuff
            for (int i = 0; i < this.mediaFiles.Count; i++) {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    Logger.Instance.LogMessage("Renaming Cancelled.", LogLevel.INFO);
                    return;
                }
                
                MediaFile ie = MediaFileManager.Instance[i];
                if (ie.MarkedForDeletion&&ie.ProcessingRequested)
                {
                    try
                    {
                        File.Delete(ie.FilePath.Fullfilename);
                        mediaFiles.Remove(ie);
                        //Go back so no entry is skipped after removal of current entry
                        i--;
                        continue;
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.LogMessage("Couldn't delete " + ie.FilePath.Fullfilename + ": " + ex.Message, LogLevel.ERROR);
                    }
                }

                //need to set before ie.Rename because it resets some conditions
                bool copyfile = ie.ProcessingRequested && ie.Destination != "" && ie.FilePath.Fullfilename.ToLower()[0] != ie.Destination.ToLower()[0];

                //if there are files which actually need to be copied since they're on a different drive, only count those for the progress bar status since they take much longer
                if (TotalBytes > 0)
                {
                    ProgressAtCopyStart = ((double)Bytes / (double)TotalBytes * 100);
                    if (copyfile)
                    {
                        ProgressAtCopyEnd = Math.Min(ProgressAtCopyStart + (long)(((double)new FileInfo(ie.FilePath.Fullfilename).Length) / (double)TotalBytes * 100), 100);
                    }
                    else
                    {
                        ProgressAtCopyEnd = ProgressAtCopyStart;
                    }
                }
                else
                {
                    ProgressAtCopyStart = i;
                    ProgressAtCopyEnd = i;
                }
                
                worker.ReportProgress((int)ProgressAtCopyStart);

                //if a Candidate is moved to a different destination directory that isn't visible in the current basedir, remove it
                int subdirlevel=Filepath.GetSubdirectoryLevel(Helper.ReadProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY), ie.Destination);
                bool remove =  subdirlevel > Helper.ReadInt(ConfigKeyConstants.MAX_SEARCH_DEPTH_KEY)||subdirlevel==-1;
                //this call will also report progress more detailed by calling ReportSingleFileProgress()
                ie.Rename(worker, e);
                if (remove)
                {
                    mediaFiles.Remove(ie);
                    //Go back so no entry is skipped after removal of current entry
                    i--;
                }
                //after file is (really) copied, add its size
                if (copyfile)
                {
                    Bytes += new FileInfo(ie.FilePath.Fullfilename).Length;
                }
            }
            if (Helper.ReadBool(ConfigKeyConstants.DELETE_EMPTIED_FOLDERS_KEY)) {
                //Delete all empty folders code
                Helper.DeleteAllEmptyFolders(Helper.ReadProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY), new List<string>(Helper.ReadProperties(ConfigKeyConstants.EMPTY_FOLDER_CHECK_IGNORED_FILETYPES_KEY)),worker,e);
            }
            if (!e.Cancel)
            {
                Logger.Instance.LogMessage("Renaming (and possibly moving and deleting) finished!", LogLevel.INFO);
            }
            else
            {
                Logger.Instance.LogMessage("Renaming Cancelled.", LogLevel.INFO);
            }
        }

        public CopyFileCallbackAction ReportSingleFileProgress(FileInfo source, FileInfo destination, object state,
        long totalFileSize, long totalBytesTransferred)
        {
            worker.ReportProgress((int)(ProgressAtCopyStart + (double)totalBytesTransferred / (double)totalFileSize * (ProgressAtCopyEnd - ProgressAtCopyStart)));
            if (worker.CancellationPending)
            {
                dwea.Cancel = true;
                return CopyFileCallbackAction.Cancel;
            }
            return CopyFileCallbackAction.Continue;
        }
        public void RenameShow(string from, string to)
        {
            from = from.ToLower();
            foreach (MediaFile ie in mediaFiles)
            {
                if (ie.Showname.ToLower() == from)
                {
                    ie.Showname = to;
                }
            }
        }
        public bool ContainsShow(string showname)
        {
            showname = showname.ToLower();
            foreach (MediaFile ie in mediaFiles)
            {
                if (ie.Showname.ToLower() == showname)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Decides which files should be marked for processing
        /// </summary>
        /// <param name="Basepath">basepath, as always</param>
        /// <param name="Showname">user entered showname</param>
        public void SelectSimilarFilesForProcessing(string Basepath, string Showname) {
            List<MediaFile> matches = FindSimilarByName(Showname);
            foreach (MediaFile ie in this.mediaFiles) {
                if (matches.Contains(ie)) {
                    ie.ProcessingRequested = true;
                    ie.Movie = false;
                }
                else {
                    ie.ProcessingRequested = false;
                }
            }
            return;
        }

        /// <summary>
        /// Sets new episode to some files and takes care of storing it properly (last [TITLE_HISTORY_SIZE_KEY] Titles are stored)
        /// </summary>
        /// <param name="files">files to which this episode should be set to</param>
        /// <param name="episode">name to be set</param>
        public void SetNewTitle(List<MediaFile> files, string title) {
            string[] LastTitlesOld = Helper.ReadProperties(ConfigKeyConstants.LAST_SEARCHED_SERIES_TITLES_KEY);
            foreach (MediaFile ie in files) {
                if (ie.Showname != title) {
                    ie.Showname = title;
                }
            }

            //check if list of titles contains new episode
            int Index = -1;
            for (int i = 0; i < LastTitlesOld.Length; i++) {
                string str = LastTitlesOld[i];
                if (str == title) {
                    Index = i;
                    break;
                }
            }

            //if the episode is new
            if (Index == -1) {
                List<string> LastTitlesNew = new List<string>();
                LastTitlesNew.Add(title);
                foreach (string s in LastTitlesOld) {
                    LastTitlesNew.Add(s);
                }
                int size = Helper.ReadInt(ConfigKeyConstants.TITLE_HISTORY_SIZE_KEY);
                Helper.WriteProperties(ConfigKeyConstants.LAST_SEARCHED_SERIES_TITLES_KEY, LastTitlesNew.GetRange(0, Math.Min(LastTitlesNew.Count, size)).ToArray());
            }
            //if the episode is in the list already, bring it to the front
            else {
                List<string> items = new List<string>(LastTitlesOld);
                items.RemoveAt(Index);
                items.Insert(0, title);
                Helper.WriteProperties(ConfigKeyConstants.LAST_SEARCHED_SERIES_TITLES_KEY, items.ToArray());
            }
        }
        public void SetPath(string path) {
            string refPath = path;
            SetPath(ref refPath);
        }
        /// <summary>
        /// Sets a new path for the list view
        /// </summary>
        /// <param name="path">Path to be set</param>
        public void SetPath(ref string path) {
            if (path == null || path == "" || !Directory.Exists(path))
                return;

            if (path.Length == 2) {
                if (char.IsLetter(path[0]) && path[1] == ':') {
                    path = path + Path.DirectorySeparatorChar;
                }
            }
            DirectoryInfo currentpath = new DirectoryInfo(path);

            //Skip this for now on networks
            
            if(!path.StartsWith("\\\\")){
                //fix casing of the path if user entered it
                string fixedpath = "";
                while (currentpath.Parent != null) {
                    fixedpath = currentpath.Parent.GetDirectories(currentpath.Name)[0].Name + Path.DirectorySeparatorChar + fixedpath;
                    currentpath = currentpath.Parent;
                }
                fixedpath = currentpath.Name.ToUpper() + fixedpath;
                fixedpath = fixedpath.TrimEnd(new char[] { Path.DirectorySeparatorChar });
                if (fixedpath.Length == 2) {
                    if (char.IsLetter(fixedpath[0]) && fixedpath[1] == ':') {
                        fixedpath = fixedpath + Path.DirectorySeparatorChar;
                    }
                }
                path = fixedpath;
            }
            //Same path, ignore
            if (Helper.ReadProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY).ToLower() == path.ToLower()) {
                return;
            }
            else {
                Helper.WriteProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY, path);
                Environment.CurrentDirectory = path;
            }
        }

        /// <summary>
        /// Finds similar files by looking at the configurationFilePath and comparing it to a showname
        /// </summary>
        /// <param name="Basepath">basepath of the show</param>
        /// <param name="Showname">name of the show to filter</param>
        /// <param name="source">source files</param>
        /// <returns>a list of matches</returns>
        public List<MediaFile> FindSimilarByName(string Showname) {
            List<MediaFile> matches = new List<MediaFile>();
            Showname = Showname.ToLower();
            //whatever, just check path and configurationFilePath if it contains the showname
            foreach (MediaFile ie in this.mediaFiles) {
                string[] folders = Helper.splitFilePath(ie.FilePath.Path);
                string processed = ie.Filename.ToLower();

                //try to extract the name from a shortcut, i.e. sga for Stargate Atlantis
                string pattern = "[^\\w]";
                Match m = Regex.Match(processed, pattern, RegexOptions.IgnoreCase);
                if (m != null && m.Success) {
                    string abbreviation = processed.Substring(0, m.Index);
                    if (abbreviation.Length > 0 && Helper.ContainsLetters(abbreviation, Showname)) {
                        matches.Add(ie);
                        continue;
                    }
                }

                //now check if whole showname is in the configurationFilePath
                string CleanupRegex = Helper.ReadProperty(ConfigKeyConstants.REGEX_SHOWNAME_CLEANUP_KEY);
                processed = Regex.Replace(processed, CleanupRegex, " ");
                if (processed.Contains(Showname)) {
                    matches.Add(ie);
                    continue;
                }

                //or in some top folder
                foreach (string str in folders) {
                    processed = str.ToLower();
                    processed = Regex.Replace(processed, CleanupRegex, " ");
                    if (processed.Contains(Showname)) {
                        matches.Add(ie);
                        break;
                    }
                }
            }
            return matches;
        }


        #region Unsused for now!
        public static int GetNumberOfVideoFilesInFolder(string path) {
            List<string> vidext = new List<string>(Helper.ReadProperties(ConfigKeyConstants.VIDEO_FILE_EXTENSIONS_KEY));
            int count = 0;
            foreach (string file in Directory.GetFiles(path)) {
                if (vidext.Contains(Path.GetFileNameWithoutExtension(file))) {
                    count++;
                }
            }
            return count;
        }
        #endregion

        public void ClearRelation(string showname)
        {
            foreach (MediaFile ie in mediaFiles)
            {
                if (ie.Showname == showname)
                {
                    ie.Name = "";
                }
            }
        }
        public void ClearRelations()
        {
            foreach (MediaFile ie in mediaFiles)
            {
                ie.Name = "";
            }
        }
    }
}
