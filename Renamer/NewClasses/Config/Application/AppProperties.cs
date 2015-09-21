namespace Renamer.NewClasses.Config.Application
{
    /// <summary>
    /// Helper class containing property names used in config file
    /// </summary>
    static class AppProperties
    {
		/// <summary>
		/// Automatically resize columns to fit window size
		/// </summary>
		public const string AUTO_RESIZE_COLUMNS_KEY = "ResizeColumns";
		/// <summary>
		/// timeout for network connections
		/// </summary>
		public const string CONNECTION_TIMEOUT_KEY = "Timeout";
		/// <summary>
		/// If files are to be moved in a sorted directory structure
		/// </summary>
		public const string CREATE_DIRECTORY_STRUCTURE_KEY = "CreateDirectoryStructure";
		/// <summary>
		/// Indicates if season subdirectory should be used, or if all files will be placed in show dirArgument
		/// </summary>
		public const string CREATE_SEASON_SUBFOLDERS_KEY = "UseSeasonSubDir";
		/// <summary>
		/// Before renaming the file, the episode episode will be changed according to the rules defined with this property
		/// Mostly this is done to pretty print the episode episode and to avoid having multiple invalid characters in the episode leading to ugly filenames
		/// </summary>
		public const string CUSTOM_REPLACE_REGEX_STRINGS_KEY = "Replace";
		/// <summary>
		/// How diacritics in episode titles are handled when renaming files
		/// </summary>
		public const string DIACRITIC_STRATEGY_KEY = "Umlaute";
		// FIXME: Never used
		/// <summary>
		/// If other empty folders should be deleted too
		/// </summary>
		public const string DELETE_ALL_EMPTY_FOLDERS_KEY = "deleteAllEmptyFolders";
		/// <summary>
		/// If folders emptied by moving files out of them should be deleted
		/// </summary>
		public const string DELETE_EMPTIED_FOLDERS_KEY = "DeleteEmptyFolders";
		/// <summary>
		/// If Sample files should be deleted on processing
		/// </summary>
		public const string DELETE_SAMPLE_FILES_KEY = "DeleteSampleFiles";
		/// <summary>
        /// DELIMITER character used in config file
        /// </summary>
        public const string DELIMITER_KEY = "Delimiter";
		/// <summary>
		/// Directory in which files are moved (as basedir)
		/// </summary>
		public const string DESTINATION_DIRECTORY_KEY = "DestinationDirectory";
		/// <summary>
		/// when checking if a folder is empty, files of these filetypes are ignored
		/// this means that even a folder containing only one or more of these files is deleted
		/// </summary>
		public const string EMPTY_FOLDER_CHECK_IGNORED_FILETYPES_KEY = "IgnoreFiles";
		/// <summary>
		/// TV show episodes downloaded from the internet may indicate information about the season and episode number of the episode
		/// in various ways. This property holds all patterns that the tool tries to find in episode filenames to extract the information
		/// </summary>
		public const string EPISODE_IDENTIFIER_PATTERNS_KEY = "EpIdentifier";
		/// <summary>
		/// Last selected directory
		/// </summary>
		public const string LAST_DIRECTORY_KEY = "LastDirectory";
		/// <summary>
		/// Last entered series titles
		/// </summary>
		public const string LAST_SEARCHED_SHOWNAMES_KEY = "LastTitles";
		/// <summary>
		/// Last selected subtitle titleProvider
		/// </summary>
		public const string LAST_SELECTED_SUBTITLE_PROVIDER_KEY = "LastSubProvider";
		/// <summary>
		/// Last selected titleProvider
		/// </summary>
		public const string LAST_USED_TV_SHOW_PROVIDER_KEY = "LastProvider";
		/// <summary>
		/// How letter case in episode titles is handled when renaming files
		/// </summary>
		public const string LETTER_CASE_STRATEGY_KEY = "Case";
		/// <summary>
		/// Loglevel for logfile
		/// </summary>
		public const string LOGFILE_LOGLEVEL_KEY = "LogFileLevel";
		/// <summary>
		/// Logfile name
		/// </summary>
		public const string LOGFILE_NAME_KEY = "LogName";
		/// <summary>
		/// Order in which the columns are displayed, positions in this array equal to absolute ordering of the columns, values are display order
		/// </summary>
		public const string MAIN_SCREEN_COLUMN_ORDER_KEY = "ColumnOrder";
		/// <summary>
		/// Widths of the columns
		/// </summary>
		public const string MAIN_SCREEN_COLUMN_WIDTH_KEY = "ColumnWidths";
		/// <summary>
		/// Maximal subdirectory search depth
		/// </summary>
		public const string MAX_SEARCH_DEPTH_KEY = "MaxDepth";
		/// <summary>
		/// Loglevel for messageboxes
		/// </summary>
		public const string MESSAGEBOX_LOGLEVEL_KEY = "LogMessageBoxLevel";
		/// <summary>
		/// List used as a test to see if a file could be a movie
		/// </summary>
		public const string MOVIE_INDICATOR_STRINGS_KEY = "MovieIndicator";
		/// <summary>
		/// tags to remove from movies
		/// </summary>
		public const string MOVIES_TAGS_TO_REMOVE_LIST_KEY = "Tags";
		/// <summary>
		/// PREFERRED_RESULT_LANGUAGES_KEY used for preselecting search results
		/// </summary>
		public const string PREFERRED_RESULT_LANGUAGES_KEY = "Languages";
		/// <summary>
		/// Regex used to extract showname from configurationFilePath/foldername
		/// </summary>
		public const string REGEX_EXTRACT_SHOWNAME_FROM_FOLDER_KEY = "ShownameExtractionRegex";
		/// <summary>
		/// Regex used to remove non-characters and other crap from names
		/// </summary>
		public const string REGEX_SHOWNAME_CLEANUP_KEY = "CleanupRegex";
		/// <summary>
		/// Some characters in episode titles can't be used in filenames, e.g. ?
		/// When renaming files, these invalid characters are replaced with the value of this property
		/// </summary>
		public const string REPLACE_INVALID_CHARS_WITH_KEY = "InvalidCharReplace";
		/// <summary>
		/// If Log messages about missing episodes should be shown
		/// </summary>
		public const string REPORT_MISSING_EPISODES_KEY = "FindMissingEpisodes";
		/// <summary>
		/// The tool can extract information about the season number of the episode using the folder the file is in
		/// This property stores all patterns that the tool looks for when trying to extract this information
		/// </summary>
		public const string SEASON_NR_EXTRACTION_PATTERNS_KEY = "Extract";
		/// <summary>
		/// strings used to determine if a configurationFilePath may not be used for shownames (i.e. "AVSEQ01")
		/// </summary>
		public const string SHOWNAME_FILENAME_BLACKLIST_KEY = "FilenameBlacklist";
		/// <summary>
		/// Size of the episode history array
		/// </summary>
		public const string SHOWNAME_HISTORY_SIZE_KEY = "TitleHistorySize";
		/// <summary>
		/// strings used to determine if a recognized showname may be invalid (i.e. "SEASON_NR_MARKER")
		/// </summary>
		public const string SHOWNAME_PATH_WORD_BLACKLIST_KEY = "PathBlacklist";
		/// <summary>
		/// subtitle file extensions
		/// </summary>
		public const string SUBTITLE_FILE_EXTENSIONS_KEY = "SubtitleExtensions";
		/// <summary>
		/// Target filename pattern
		/// </summary>
		public const string TARGET_FILENAME_PATTERN_KEY = "TargetPattern";
		/// <summary>
		/// Loglevel for TextBox
		/// </summary>
		public const string TEXTBOX_LOGLEVEL_KEY = "LogTextBoxLevel";
		/// <summary>
        /// video file extensions
        /// </summary>
        public const string VIDEO_FILE_EXTENSIONS_KEY = "Extensions";
		/// <summary>
		/// Visible columns
		/// </summary>
		public const string VISIBLE_COLUMNS_KEY = "Visible Columns";
		/// <summary>
		/// Size of the main window
		/// </summary>
		public const string WINDOW_SIZE_KEY = "WindowSize";

    }
}
