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
using System.Text;
using Renamer.Classes.Configuration;
using Renamer.Classes.TVShows;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;
using System.Timers;
using Renamer.NewClasses.Logging;
using Renamer.NewClasses.Movies;
using Renamer.NewClasses.Util;
using Renamer.NewClasses.UI.Dialogs;
using System.Windows.Forms;
using System.ComponentModel;
namespace Renamer.Classes
{
    /// <summary>
    /// Contains all information about a single video or subtitle file, including scheduled renaming info
	/// //FIXME: Split Media file information and renaming information, maybe make a third class referencing the two things
    /// </summary>
    public class MediaFile
    {

        #region Enumerations
        /// <summary>
        /// what to do with Umlauts
        /// </summary>
        public enum UmlautAction : int { Unset, Ignore, Use, Dont_Use };

        /// <summary>
        /// what to do with casing of filenames
        /// </summary>
        public enum CaseAction : int { Unset, Ignore, small, UpperFirst, CAPSLOCK };

        /// <summary>
        /// if file should be moved
        /// </summary>
        public enum DirectoryStructureAction : int { Unset, CreateDirectoryStructure, NoDirectoryStructure };
        #endregion


        #region members
        private Filepath originalPath;
        private Filepath destinationPath;

        private string showname;
        private string episodeTitle;

        private int seasonNr;
        private int episodeNr;

        private bool isVideoFile;
        private bool isSubtitleFile;
        private bool processingRequested;
        private bool isMovie;
        private bool isSampleFile;
        private bool markedForDeletion;

        private UmlautAction umlautAction;
        private CaseAction caseAction;
        private DirectoryStructureAction directoryStructureAction;
        private Helper.Languages language;
        #endregion

        #region Static Members
        private static string[] toBeReplaced = { "ä", "Ä", "ö", "Ö", "ü", "Ü", "ß", "É", "È", "Ê", "Ë", "Á", "À", "Â", "Ã", "Å", "Í", "Ì", "Î", "Ï", "Ú", "Ù", "Û", "Ó", "Ò", "Ô", "Ý", "Ç", "é", "è", "ê", "ë", "á", "à", "â", "ã", "å", "í", "ì", "î", "ï", "ú", "ù", "û", "ó", "ò", "ô", "ý", "ÿ", "ç" };
        private static string[] toBeReplacedWith = { "ae", "Ae", "oe", "Oe", "ue", "Ue", "ss", "E", "E", "E", "E", "A", "A", "A", "A", "A", "I", "I", "I", "I", "U", "U", "U", "O", "O", "O", "Y", "C", "e", "e", "e", "e", "a", "a", "a", "a", "a", "i", "i", "i", "i", "u", "u", "u", "o", "o", "o", "y", "y", "c" };
        public static string NotRecognized = "Not recognized, please enter manually";
        #endregion

        #region Properties

        /// <summary>
        /// destination directory
        /// </summary>
        public string DestinationPath {
            get {
                if (MarkedForDeletion) {
                    return "To be deleted";
                } else {                    
                    return destinationPath.Path;
                }
            }
            set { destinationPath.Path = value; }
        }
        /// <summary>
        /// new file name with extension
        /// </summary>
        public string NewFilename {
            get {
                if (MarkedForDeletion) {
                    return "To be deleted";
                } else {
                    if (String.IsNullOrEmpty(Showname)) {
                        return "";
                    } else {
                        return this.destinationPath.Filename;
                    }
                }
            }
            set {
                this.destinationPath.Filename = value; 
            }
        }

        /// <summary>
        /// Old file name with extension
        /// </summary>
        public string Filename {
            get { return originalPath.Filename; }
            set {
                if (originalPath.Filename != value) {
                    originalPath.Filename = value;
                    if (originalPath.Filename != "") {
                        checkExtension();
                        if (originalPath.Path != "")
                        {
                            extractName();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Extension of the file without dot, i.e. "avi" or "srt"
        /// </summary>
        public string Extension {
            get { return originalPath.Extension; }
            set {
                if (originalPath.Extension != value) {
                    originalPath.Extension = value;
                    destinationPath.Extension = value;
                    checkExtension();
                    extractName();
                }
            }
        }
        
        /// <summary>
        /// Path of the file
        /// </summary>
        public Filepath FilePath {
            get { return originalPath; }
            set {
                originalPath = value;
            }
        }
        /// <summary>
        /// Name of the show this file belongs to.
        /// </summary>
        public string Showname {
            get { return showname; }
            set {
                if (value == NotRecognized) value = "";
                if (showname != value) {
					if (value == null) {
						value = "";
					}
                    if (value == "Sample" && Helper.ReadBool(ConfigKeyConstants.DELETE_SAMPLE_FILES_KEY)) {
                        MarkedForDeletion = true;
                    }
                    showname = value;
                    //need to find showname again incase there is different data for new showname
                    if (!IsMovie)
                    {
                        SetupRelation();
                    }
                    //file path might contain showname, so it needs to be updated anyway
                    createTargetName();
                    //directory structure also contains showname and should be updated
                    SetPath();
                }
            }
        }
        
        /// <summary>
        /// number of the season
        /// </summary>
        public int SeasonNr {
            get { return seasonNr; }
            set {
                if (seasonNr != value) {
                    seasonNr = value;
                    createTargetName();
                    SetPath();
                    SetupRelation();
                }
            }
        }
        /// <summary>
        /// number of the episode
        /// </summary>
        public int EpisodeNr {
            get { return episodeNr; }
            set {
                if (episodeNr != value) {
                    episodeNr = value;
                    SetupRelation();
                    createTargetName();
                }
            }
        }
        /// <summary>
        /// name of the episode
        /// </summary>
        public string EpisodeTitle {
            get { return episodeTitle; }
            set {
                if (episodeTitle != value) {
                    episodeTitle = value;
                    SetPath();
                    createTargetName();
                }
            }
        }

        public bool IsVideofile {
            get {
                return this.isVideoFile;
            }
        }
        public bool IsSubtitle {
            get {
                return this.isSubtitleFile;
            }
        }

        /// <summary>
        /// If file is a movie.
        /// </summary>
        public bool IsMovie {
            get { return isMovie; }
            set
            {
                if (isMovie != value)
                {
                    isMovie = value;
                    if (Showname != "")
                    {
                        createTargetName();
                        SetPath();
                    }
                }
            }
        }
        /// <summary>
        /// If file is to be processed
        /// </summary>
        public bool ProcessingRequested {
            get { return processingRequested; }
            set { processingRequested = value; }
        }

        public bool IsSample
        {
            get { return isSampleFile; }
            set { isSampleFile = value; }
        }
        /// <summary>
        /// If file is to be deleted
        /// </summary>
        public bool MarkedForDeletion
        {
            get { return markedForDeletion; }
            set { markedForDeletion = value; }
        }

        /// <summary>
        /// from which level in the directory structure the name was extracted,
		/// 0=configurationFilePath, 1=name file is in, 2= parent of 1, etc
        /// </summary>
        public int ExtractedNameLevel
        {
            get;
            set;
        }
        /// <summary>
        /// Option indicates the using of umlaute
        /// </summary>
        public UmlautAction UmlautUsage {
            get { return umlautAction; }
            set {
                if (umlautAction != value) {
                    umlautAction = value;
                    createTargetName();
                    SetPath();
                }
            }
        }
        /// <summary>
        /// Option indicates the use of UPPER and lowercase
        /// </summary>
        public CaseAction Casing {
            get { return caseAction; }
            set {
                if (caseAction != value) {
                    caseAction = value;
                    createTargetName();
                    SetPath();
                }
            }
        }

        public DirectoryStructureAction CreateDirectoryStructure {
            get { return directoryStructureAction; }
            set {
                if (directoryStructureAction != value) {
                    directoryStructureAction = value;
                    SetPath();
                }
            }
        }
        public Helper.Languages Language {
            get { return language; }
            set {
                if (language != value) {
                    language = value;
                    if (originalPath.Name != "") SetPath();
                    if (NewFilename != "") createTargetName();
                }
            }
        }
        /// <summary>
        /// True if the file is located in a season folder, false if uninitialized or otherwise
        /// </summary>
        public bool inSeasonFolder() {
			if (string.IsNullOrEmpty(FilePath.Path)) {
				return false;
			}
            string[] extractionPatterns = Helper.ReadProperties(ConfigKeyConstants.SEASON_NR_EXTRACTION_PATTERNS_KEY);
            foreach(string extractionPattern in extractionPatterns){
                if(Regex.IsMatch(FilePath.Path,extractionPattern))
                    return true;
            }
            return false;
		}

		public bool IsMultiFileMovie
        {
            get;
            set;
        }

        #endregion

        #region Private properties

        
        #endregion

        #region private Methods
        private void initMembers() {
            originalPath = new Filepath();
            destinationPath = new Filepath();

            showname = "";
            episodeTitle = "";

            seasonNr = -1;
            episodeNr = -1;

            isVideoFile = false;
            isSubtitleFile = false;
            processingRequested = true;
            isMovie = false;

            umlautAction = UmlautAction.Unset;
            caseAction = CaseAction.Unset;
            directoryStructureAction = DirectoryStructureAction.Unset;
            language = Helper.Languages.None;
            ExtractedNameLevel = 0;
        }


        /// <summary>
        /// Extracts the showname from the original file path
        /// </summary>
        private void extractName() {
            if (!originalPath.isEmpty) {
                if (IsMovie) {
					// FIXME: Hier sieht man, dass die Klasse mehrfach verwendet wird
                    Showname = MovieNameExtractor.Instance.ExtractMovieName(this);
                } else {
                    Showname = SeriesNameExtractor.Instance.ExtractSeriesName(this);
                }
                if (Showname == "Sample") {
                    IsSample = true;
                }
            }
        }

        /// <summary>
        /// set Properties depending on the Extension of the file
        /// </summary>
        private void checkExtension() {
            this.isVideoFile = (new List<string>(Helper.ReadProperties(ConfigKeyConstants.VIDEO_FILE_EXTENSIONS_KEY, true))).Contains(this.originalPath.Extension);
            this.isSubtitleFile = (new List<string>(Helper.ReadProperties(ConfigKeyConstants.SUBTITLE_FILE_EXTENSIONS_KEY, true))).Contains(this.originalPath.Extension);
        }

        private void SetMoviesPath() {
            getCreateDirectory();
            if (directoryStructureAction != DirectoryStructureAction.CreateDirectoryStructure) {
                DestinationPath = "";
                return;
            }

            DestinationPath = Helper.ReadProperty(ConfigKeyConstants.DESTINATION_DIRECTORY_KEY);

            if (!Directory.Exists(DestinationPath))
            {
                DestinationPath = Filepath.goUpwards(FilePath.Path,ExtractedNameLevel);
            }
            if (IsMultiFileMovie)
            {
                DestinationPath = getMoviesDestinationPath(DestinationPath);
            }
            if (DestinationPath != FilePath.Path)
            {
                DestinationPath = DestinationPath;
            }
            else
            {
                DestinationPath = "";
            }

        }

        private string getMoviesDestinationPath(string currentDestination) {
            string[] folders = Helper.getParentFolderNames(currentDestination);
            if (folders.Length > 0)
            {
                if (Helper.InitialsMatch(folders[folders.Length - 1], MovieNameWithoutPart()))
                {
                    return currentDestination;
                }
            }
            return Filepath.goIntoFolder(currentDestination, MovieNameWithoutPart());
        }

        private string MovieNameWithoutPart()
        {
            return Showname.Substring(0, Showname.Length - 3);
        }

        // TODO: function is still tooooooo large
        private void SetSeriesPath() {
            if (SeasonNr == -1||Showname=="")
            {
                DestinationPath = "";
                return;
            }
            //string basepath = Helper.ReadProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY);
            DestinationPath = Helper.ReadProperty(ConfigKeyConstants.DESTINATION_DIRECTORY_KEY);
           
            if (!Directory.Exists(DestinationPath))
            {
                DestinationPath = FilePath.Path;
            }
            bool DifferentDestinationPath = FilePath.Path!=DestinationPath;
            //for placing files in directory structure, figure out if selected directory is show name, otherwise create one
            bool isNetwork = FilePath.Path.StartsWith(""+Path.DirectorySeparatorChar + Path.DirectorySeparatorChar);
            string[] dirs = this.originalPath.Folders;
            bool InSeriesDir = false;
            bool InSeasonDir = false;
            //any (other) season dirArgument
            bool InASeasonDir = false;
            bool UseSeasonSubDirs = Helper.ReadBool(ConfigKeyConstants.CREATE_SEASON_SUBFOLDERS_KEY);
            //figure out if we are in a season dirArgument
            string[] seasondirs = Helper.ReadProperties(ConfigKeyConstants.SEASON_NR_EXTRACTION_PATTERNS_KEY);
            string aSeasondir = "";
            int showdirlevel = 0;

            //figure out if we are in an extraction dirArgument, if we are, we need to go upwards one level
            if (dirs.Length > 0 &&Filepath.IsExtractionDirectory(dirs[dirs.Length-1]))
            {
                DestinationPath = Filepath.goUpwards(DestinationPath, 1);
                List<string> blah=new List<string>(dirs);
                blah.RemoveAt(dirs.Length - 1);
                dirs = blah.ToArray();
            }

            //check if we are in a season and/or series directory
            //loop backwards so first season entry is used if nothing is recognized and folder has to be created
            for (int i = seasondirs.Length - 1; i >= 0; i--) {
                aSeasondir = RegexConverter.replaceSeriesname(seasondirs[i], showname);
                bool InSomething = false;
                if (dirs.Length > 1)
                {
                    Match m = Regex.Match(dirs[dirs.Length - 1], aSeasondir);
                    int parsedSeason;
                    Int32.TryParse(m.Groups["Season"].Value, out parsedSeason);
                    if (m.Success)
                    {
                        if (parsedSeason == seasonNr)
                        {
                            InSeasonDir = true;
                        }
                        InASeasonDir = true;
                        InSomething = true;
                    }
                }

                //remove dots to avoid problems with series like "N.C.I.S." or "Dr. House"
                /*if (dirs.Length > 0 && dirs[dirs.Length - 1].CUSTOM_REPLACE_REGEX_STRINGS_KEY(".","").StartsWith(nameOfSeries.CUSTOM_REPLACE_REGEX_STRINGS_KEY(".",""))){
                    InSeriesDir=true;
                }*/
                if (dirs.Length > 0 && Helper.InitialsMatch(dirs[dirs.Length - 1], showname))
                {
                    InSeriesDir = true;
                }
                /*else if (dirs.Length > 1 && dirs[dirs.Length - 2].CUSTOM_REPLACE_REGEX_STRINGS_KEY(".", "").StartsWith(nameOfSeries.CUSTOM_REPLACE_REGEX_STRINGS_KEY(".", "")))*/
                else if (dirs.Length > 1 && Helper.InitialsMatch(dirs[dirs.Length - 2], showname))
                {
                    InSeriesDir = true;
                    showdirlevel = 1;
                }
                if (InSomething) break;
            }
            
            getCreateDirectory();
            if (directoryStructureAction != DirectoryStructureAction.CreateDirectoryStructure || !isSeasonValid()) {
                DestinationPath = "";
                return;
            }
            //if files aren't meant to be moved somewhere else
            if (!DifferentDestinationPath)
            {                
                //somewhere else, create new series dirArgument
                if (!InSeriesDir && !InSeasonDir &&!InASeasonDir)
                {
                    DestinationPath = addSeriesDir(DestinationPath);
                    DestinationPath = addSeasonsDirIfDesired(DestinationPath);
                }
                //in series dirArgument, create seasons dirArgument
                else if (InSeriesDir&&!InASeasonDir)
                {
                    DestinationPath = addSeasonsDirIfDesired(DestinationPath);
                }
                //wrong season dirArgument, add real seasons dirArgument
                else if (InSeriesDir && InASeasonDir &&!InSeasonDir)
                {
                    DestinationPath = Filepath.goUpwards(DestinationPath, 1);
                    if (showdirlevel == 0)
                    {
                        DestinationPath = addSeriesDir(DestinationPath);
                    }
                    DestinationPath = addSeasonsDirIfDesired(DestinationPath);
                }
                //wrong show dirArgument, go back two levels and add proper dirArgument structure
                else if (!InSeriesDir && InASeasonDir)
                {
                    DestinationPath = addSeriesDir(Filepath.goUpwards(DestinationPath, 2));
                    DestinationPath = addSeasonsDirIfDesired(DestinationPath);
                }
            }
            //if they should be moved
            else
            {
                DestinationPath = addSeriesDir(DestinationPath);
                DestinationPath = addSeasonsDirIfDesired(DestinationPath);
            }
            if (DestinationPath != FilePath.Path)
            {
                DestinationPath = DestinationPath;
            }
            else
            {
                DestinationPath = "";
            }
        }

        private void getCreateDirectory() {
            if (directoryStructureAction != DirectoryStructureAction.Unset)
                return;
            loadSettingCreateDirectory();
        }
        private void loadSettingCreateDirectory() {
            directoryStructureAction = (Helper.ReadBool(ConfigKeyConstants.CREATE_DIRECTORY_STRUCTURE_KEY)) ? DirectoryStructureAction.CreateDirectoryStructure : DirectoryStructureAction.NoDirectoryStructure;
        }
     
        private bool isSeasonValid() {
            return this.seasonNr >= 0;
        }

        

        private string addSeriesDir(string path){
            return path + System.IO.Path.DirectorySeparatorChar + showname;
        }
        
        private string addSeasonsDirIfDesired(string path) {
            if (useSeasonSubDirs())
            {
                string[] dirs = path.Split(new char[] { Path.DirectorySeparatorChar });
                //figure out if we are in a season dirArgument
                string[] seasondirs = Helper.ReadProperties(ConfigKeyConstants.SEASON_NR_EXTRACTION_PATTERNS_KEY);
                string aSeasondir = "";
                
                if (Directory.Exists(path))
                {
                    string[] Directories = Directory.GetDirectories(path);                
                    for (int i = 0; i < seasondirs.Length; i++)
                    {
                        foreach (string dir in Directories)
                        {
                            aSeasondir = RegexConverter.replaceSeriesnameAndSeason(seasondirs[i], showname, seasonNr);
                            if (dirs.Length > 1)
                            {
                                Match m = Regex.Match(dir, aSeasondir);

                                if (m.Success)
                                {
                                    return path + System.IO.Path.DirectorySeparatorChar + Path.GetFileName(dir);
                                }
                            }
                        }
                    }
                }
            }
            return path + ((useSeasonSubDirs())?seasonsSubDir():"");
        }
 
        private bool useSeasonSubDirs() {
            return Helper.ReadBool(ConfigKeyConstants.CREATE_SEASON_SUBFOLDERS_KEY);
        }

        private string seasonsSubDir() {
            string seasondir = RegexConverter.replaceSeriesnameAndSeason(Helper.ReadProperties(ConfigKeyConstants.SEASON_NR_EXTRACTION_PATTERNS_KEY)[0], showname, seasonNr);
            return System.IO.Path.DirectorySeparatorChar + seasondir;
        }


        #endregion
        #region public Methods
        public MediaFile() {
            initMembers();
        }

        /// <summary>
        /// Generates the file and directory name the file should be stored
        /// </summary>
        public void SetPath() {
            if (String.IsNullOrEmpty(showname)) {
                DestinationPath = "";
            }
            if (!isMovie) {
                SetSeriesPath();
            }
            else {
                SetMoviesPath();
            }
        }

        /// <summary>
        /// This function tries to find an episode name which matches the showname, episode and season number by looking at previously downloaded relations
        /// </summary>
        public void SetupRelation() {
            findEpisodeName(TvShowManager.Instance.GetTvShow(this.Showname));
        }

        public void findEpisodeName(TvShow rc) {
            resetName();
            if (rc == null)
                return;

            for (int i = 0; i < rc.CountEpisodes; i++) {
                if (isValidRelation(rc[i])) {
                    EpisodeTitle = rc[i].Title;
                    break;
                }
                if (isInValidSeason(rc[i]) || isInValidEpisode(rc[i]))
                    EpisodeTitle = rc[i].Title;
            }
        }

        private bool isValidRelation(ShowEpisode relation) {
            return relation.SeasonNr == seasonNr && relation.SeasonEpisodeNr == episodeNr;
        }

        private bool isInValidEpisode(ShowEpisode relation) {
            return relation.SeasonNr == seasonNr && episodeNr == -1;
        }

        private bool isInValidSeason(ShowEpisode relation) {
            return relation.SeasonEpisodeNr == episodeNr && seasonNr == -1;
        }

        private void resetName() {
            this.EpisodeTitle = "";
        }

        public void adjustSpelling(ref string input, bool extension) {
            input = adjustUmlauts(input);
            input = adjustCasing(input, extension);
        }

        private string adjustUmlauts(string input) {
            UmlautAction ua = readUmlautUsage();
            if (ua == UmlautAction.Use && language == Helper.Languages.German) {
                input = transformDoubleLetterToUmalauts(input);
            }
            else if (ua == UmlautAction.Dont_Use) {
                input = replaceUmlautsAndSpecialChars(input);
            }
            return input;
        }

        private UmlautAction readUmlautUsage() {
            UmlautAction ua = umlautAction;
            if (ua == UmlautAction.Unset) {
                ua = Helper.ReadEnum<UmlautAction>(ConfigKeyConstants.DIACRITIC_STRATEGY_KEY);
                if (ua == UmlautAction.Unset)
                    ua = UmlautAction.Ignore;
            }
            return ua;
        }

        private string adjustCasing(string input, bool extension) {
            if (caseAction == CaseAction.Unset) {
                caseAction = Helper.ReadEnum<CaseAction>(ConfigKeyConstants.LETTER_CASE_STRATEGY_KEY);
                if (caseAction == CaseAction.Unset)
                    caseAction = CaseAction.Ignore;
            }
            if (caseAction == CaseAction.small) {
                input = input.ToLower();
            }
            else if (caseAction == CaseAction.UpperFirst) {
                if (!extension) {
                    Regex r = new Regex(@"\b(\w)(\w+)?\b", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    input = Helper.convertToCamelCase(input);
                }
                else {
                    input = input.ToLower();
                }
            }
            else if (caseAction == CaseAction.CAPSLOCK) {
                input = input.ToUpper();
            }
            return input;
        }

        private string replaceUmlautsAndSpecialChars(string input) {
            for (int index = 0; index < toBeReplaced.Length; index++) {
                input = input.Replace(toBeReplaced[index], toBeReplacedWith[index]);
            }
            return input;
        }
        private string transformDoubleLetterToUmalauts(string input) {
            input = input.Replace("ae", "ä");
            input = input.Replace("Ae", "Ä");
            input = input.Replace("oe", "ö");
            input = input.Replace("Oe", "Ö");
            input = input.Replace("ue", "ü");
            input = input.Replace("Ue", "Ü");
            input = input.Replace("eü", "eue");
            return input;
        }
     
        
        /// <summary>
        /// This function generates a new configurationFilePath from the Target Pattern, episode, season, episode, showname,... values
        /// </summary>
        public void createTargetName() {
            if ((IsMovie && Showname =="") || (!IsMovie && !isSubtitleFile && episodeTitle == "")) {
                NewFilename = "";
                return;
            }
            else {
                //Note: Subtitle destination path is set here too
                if (isSubtitleFile)
                {
                    if (episodeTitle == "" && SeasonNr > -1 && EpisodeNr > -1)
                    {
                        MediaFile videoEntry = MediaFileManager.Instance.GetMatchingVideo(Showname, SeasonNr, EpisodeNr);
                        if (videoEntry != null)
                        {
                            string nfn, dst;
                            if (videoEntry.NewFilename == "")
                            {
                                nfn = Path.GetFileNameWithoutExtension(videoEntry.Filename);
                            }
                            else
                            {
                                nfn = Path.GetFileNameWithoutExtension(videoEntry.NewFilename);
                            }
                            nfn += "." + Extension;

                            //Move to Video file
                            dst = videoEntry.DestinationPath;

                            //Don't do this if name fits already
                            if (nfn == Filename)
                            {
                                nfn = "";
                            }
                            //Don't do this if path fits already
                            if (dst == FilePath.Path)
                            {
                                dst = "";
                            }
                            NewFilename = nfn;
                            DestinationPath = dst;
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                //Target Filename format
                string tmpname;

                //Those 3 strings need case/Umlaut processing
                string epname = this.episodeTitle;
                string seriesname = showname;
                string extension = Extension;

                if (!isMovie) {
                    tmpname = Helper.ReadProperty(ConfigKeyConstants.TARGET_FILENAME_PATTERN_KEY);
                    tmpname = tmpname.Replace("%e", episodeNr.ToString());
                    tmpname = tmpname.Replace("%E", episodeNr.ToString("00"));
                    tmpname = tmpname.Replace("%s", seasonNr.ToString());
                    tmpname = tmpname.Replace("%S", seasonNr.ToString("00"));
                    adjustSpelling(ref epname, false);
                }
                else {
                    tmpname = "%T";
                }

                
                adjustSpelling(ref seriesname, false);
                adjustSpelling(ref extension, true);

                //Now that series episode, episode episode and extension are properly processed, add them to the configurationFilePath

                //Remove extension from target configurationFilePath (if existant) and add properly cased one
                tmpname = Regex.Replace(tmpname, "\\." + extension, "", RegexOptions.IgnoreCase);
                tmpname += "." + extension;

                tmpname = tmpname.Replace("%T", seriesname);
                tmpname = tmpname.Replace("%N", epname);

                //string replace function
                List<string> replace = new List<string>(Helper.ReadProperties(ConfigKeyConstants.CUSTOM_REPLACE_REGEX_STRINGS_KEY));
                List<string> from = new List<string>();
                List<string> to = new List<string>();
                foreach (string s in replace) {
                    if (!s.StartsWith(Settings.COMMENT)) {
                        string[] replacement = s.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                        if (replacement != null && replacement.Length == 2) {
                            tmpname = Regex.Replace(tmpname, replacement[0], replacement[1], RegexOptions.IgnoreCase);
                        }
                    }
                }

                //set new configurationFilePath if renaming process is required
                if (Filename == tmpname) {
                    NewFilename = "";
                }
                else {
                    NewFilename = tmpname;
                }
            }
        }

        public void RemoveVideoTags()
        {
            this.ProcessingRequested = false;
            //Go through all selected files and remove tags and clean them up
            this.DestinationPath = "";
            EpisodeNr = -1;
            SeasonNr = -1;
            this.IsMovie = true;
            this.Showname=MovieNameExtractor.Instance.ExtractMovieName(this);
                        
            if (this.NewFilename != "" || this.DestinationPath != "")
            {
                this.ProcessingRequested = true;
            }
        }
        public void MarkAsTVShow()
        {
            this.ProcessingRequested = false;
            //Go through all selected files and remove tags and clean them up
            this.DestinationPath = "";
            this.IsMovie = false;
            this.Showname = SeriesNameExtractor.Instance.ExtractSeriesName(this);

            if (this.NewFilename != "" || this.DestinationPath != "")
            {
                this.ProcessingRequested = true;
            }
        }
        public string GetFormattedFullDestination()
        {
            string dest = "";
            if (DestinationPath != "")
            {
                dest = DestinationPath;
            }
            else
            {
                dest = FilePath.Path;
            }
            if (NewFilename != "")
            {
                dest += Path.DirectorySeparatorChar + NewFilename;
            }
            else
            {
                dest += Path.DirectorySeparatorChar + Filename;
            }
            return dest;
        }
        public void Rename(BackgroundWorker worker, DoWorkEventArgs e) {
            if (this.ProcessingRequested
                && ((this.Filename != this.NewFilename && this.NewFilename != "")
                    || (this.DestinationPath != this.FilePath.Path && this.DestinationPath != ""))) {
                try {
                    //create directory if needed
                    if (this.DestinationPath != "" && !Directory.Exists(this.DestinationPath)) {
                        Directory.CreateDirectory(this.DestinationPath);
                    }
                    //Move to desired destination      
                    string src = this.FilePath.Path + Path.DirectorySeparatorChar + this.Filename;
                    string target="";
                    if (this.DestinationPath != "") {
                        if (this.NewFilename != "") {
                            target = this.DestinationPath + Path.DirectorySeparatorChar + this.NewFilename;
                        }
                        else {
                            target = this.DestinationPath + Path.DirectorySeparatorChar + this.Filename;
                        }
                    }
                    else {
                        if (this.NewFilename != "") {
                            target = this.FilePath.Path + Path.DirectorySeparatorChar + this.NewFilename;
                        }
                    }

                    if (target != "")
                    {
                        if (target.ToLower()[0] == src.ToLower()[0])
                        {
                            File.Move(src, target);
                        }
                        else
                        {                            
                            FileRoutines.CopyFile(new FileInfo(src), new FileInfo(target), CopyFileOptions.AllowDecryptedDestination | CopyFileOptions.FailIfDestinationExists, new CopyFileCallback(MediaFileManager.Instance.ReportSingleFileProgress));
                            if (worker.CancellationPending)
                            {
                                return;
                            }
                            File.Delete(src);
                        }
                    }

                    //Refresh values
                    if (this.NewFilename != "") {
                        this.Filename = this.NewFilename;
                    }
                    if (this.DestinationPath != "") {
                        this.FilePath.Path = this.DestinationPath;
                    }
                    this.DestinationPath = "";
                    this.NewFilename = "";
                    //This will always be false, but might be needed elsewhere sometime
                    ShouldBeProcessed();
                }
                catch (Exception ex) {
                    //if the user didn't want to cancel, this is an actualy error message and needs to be displayed
                    if (!worker.CancellationPending)
                    {
                        Logger.Instance.LogMessage("Couldn't move " + this.FilePath.Path + Path.DirectorySeparatorChar + this.Filename + " -> " + this.DestinationPath + Path.DirectorySeparatorChar + this.NewFilename + ": " + ex.Message, LogLevel.ERROR);
                    }
                }
            }
        }
        public void ShouldBeProcessed()
        {
            ProcessingRequested = DestinationPath != "" || NewFilename != "";
        }
        public string ToString() {
            return Filename;
        }
        #endregion
    }
}
