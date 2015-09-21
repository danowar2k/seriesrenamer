using System.Collections.Generic;
using System.IO;
using Renamer.NewClasses.Enums;
using Renamer.NewClasses.Util;

namespace Renamer.NewClasses.Config.Application {
	/// <summary>
	/// This is a wrapper for default values for the configuration.
	/// Provides the default values and can generate a default configuration.
	/// </summary>
	public class AppDefaults {

		/// <summary>
		/// File extensions included in the renaming process
		/// </summary>
		private static readonly string[] videoFileExtensions = {
			"avi",
			"mpg",
			"mpeg", 
			"mp4", 
			"divx", 
			"mkv", 
			"wmv"
		};
		/// <summary>
		/// Subtitle file extensions, also included in renaming
		/// </summary>
		private static readonly string[] subtitleFileExtensions = {
			"srt", 
			"sub"
		};
		/// <summary>
		/// Filters used to extract season and episode number from filename
		/// Those filters are processed in the below order. Placeholders:
		/// %S - Season
		/// %E - Episode
		/// </summary>
		private static readonly string[] episodeIdentifierPatterns = {
			"S" + RenamingConstants.SEASON_NR_MARKER + "E" + RenamingConstants.EPISODE_NR_MARKER,
			RenamingConstants.SEASON_NR_MARKER + "x" + RenamingConstants.EPISODE_NR_MARKER,
			"S" + RenamingConstants.SEASON_NR_MARKER + ".E" + RenamingConstants.EPISODE_NR_MARKER, 
			"- " + RenamingConstants.SEASON_NR_MARKER+ RenamingConstants.EPISODE_NR_MARKER + " -", 
			"-E" + RenamingConstants.EPISODE_NR_MARKER + "-", 
			"." + RenamingConstants.SEASON_NR_MARKER + RenamingConstants.EPISODE_NR_MARKER + ".", 
			RenamingConstants.SEASON_NR_MARKER + "." + RenamingConstants.EPISODE_NR_MARKER, 
			RenamingConstants.SEASON_NR_MARKER + RenamingConstants.EPISODE_NR_MARKER
		};
		/// <summary>
		/// Last queried shownames
		/// </summary>
		private static readonly string[] lastSearchedShownames = {
			
		};

		/// <summary>
		/// Name of season dir, for extracting Season(%S) and possibly name (%T)
		/// </summary>
		private static readonly string[] seasonNrExtractionPatterns = {
			"Season " + RenamingConstants.SEASON_NR_MARKER,
			"Season_" + RenamingConstants.SEASON_NR_MARKER, 
			"Season" + RenamingConstants.SEASON_NR_MARKER, 
			"Staffel " + RenamingConstants.SEASON_NR_MARKER, 
			"Staffel_" + RenamingConstants.SEASON_NR_MARKER, 
			"Staffel" + RenamingConstants.SEASON_NR_MARKER, 
			"S" + RenamingConstants.SEASON_NR_MARKER
		};
		/// <summary>
		/// When checking if a folder should be deleted because it is empty,
		/// still delete when the folder contains only files of these filetypes
		/// </summary>
		private static readonly string[] emptyFolderCheckIgnoredFiletypes = {
			"nfo",
			"diz"
		};
		/// <summary>
		/// Order of the file window columns
		/// </summary>
		private static readonly string[] mainScreenColumnOrder = {
			"0", "1", "2", "3", "4", "5", "6", "7"
		};
		/// <summary>
		/// Width of the file window columns
		/// </summary>
		private static readonly string[] mainScreenColumnWidth = {
			"222",
			"203",
			"120",
			"51", 
			"53",
			"122",
			"176",
			"163"
		};
		/// <summary>
		/// size of the main window
		/// </summary>
		private static readonly string[] windowSize = {
			"1024",
			"600"
		};
		/// <summary>
		/// Regular expression for showname extraction from filenames
		/// </summary>
		private static readonly string[] regexExtractShownameFromFolder = {
			"(?<pos>\\.Ep\\d+.)",
			"^(?<pos>[Ss]\\d+[Ee]\\d+[\\.-])",
			"((?<pos>\\.[Ss]\\d+[Ee]\\d+([\\.-]|\\w))|(?<pos>\\.\\d+\\.)|(?<pos>[\\. _-]\\d{3,})|(?<pos>\\d+x\\d+)|(?<pos>[Ss]\\d+x\\d+))",
			"(?<pos>\\d{2,4}.\\d{2,4}.\\d{2,4})"
		};
		/// <summary>
		/// What to replace for final filenames
		/// </summary>
		private static readonly string[] customReplaceRegexStrings = {
			Configuration.COMMENT + " Comments are indicated by " + Configuration.COMMENT,
			Configuration.COMMENT + " Format used here is \"From->To\",",
			Configuration.COMMENT + " where \"From\" is a c# regular expression, see",
			Configuration.COMMENT + " http://www.radsoftware.com.au/articles/regexlearnsyntax.aspx for details",
			Configuration.COMMENT + " example: \"\\s->.\" replaces whitespaces with dots",
			"cd->CD", 
			" i\\.-> I.", 
			" ii\\.-> II.", 
			" iii\\.-> III.", 
			" iv\\.-> IV.", 
			" v\\.-> V.", 
			" vi\\.-> VI.", 
			" vii\\.-> VII.", 
			" i -> I ", 
			" ii -> II ",
			" iii -> III ",
			" iv -> IV ", 
			" v -> V ", 
			" vi -> VI ",
			" vii -> VII ",
			"\\[\\d+\\]->",
			" +-> "
		};
		/// <summary>
		/// Tags which are recognized on movie files to rip them from filename
		/// </summary>
		private static readonly string[] moviesTagsToRemoveList = {
			"Xvid",
			"DivX", 
			"R5", 
			"R3", 
			"GERMAN", 
			"DVD", 
			"INTERNAL", 
			"PROPER", 
			"TELECINE", 
			"LINE", 
			"LD",
			"MD",
			"AC3",
			"SVCD",
			"XSVCD", 
			"VCD", 
			"Dubbed",
			"HD", 
			"720P",
			"720",
			"SCREENER", 
			"RSVCD", 
			"\\d{4}", 
			"TS", 
			"GER", 
			"by \\w+",
			"xmas[^A-Za-z0-9]+special"
		};
		/// <summary>
		/// Some blacklist used to ignore path names that can't be used for showname extraction
		/// </summary>
		private static readonly string[] shownamePathWordBlacklist = {
			"Season", 
			"Staffel", 
			"Disk", 
			"(D)isk",
			"(S)taffel",
			"(S)eason", 
			"DVD", 
			"Special",
			"Downloads",
			"Download", 
			"Serien", 
			"Series", 
			"Serie", 
			"Videos",
			"Movies",
			"Moviez",
			"Filme"
		};
		/// <summary>
		/// Blacklist to determine filenames which can't be used to extract things from
		/// </summary>
		private static readonly string[] shownameFilenameBlacklist = {
			"AVSEQ01"
		};
		/// <summary>
		/// List used as a test to see if a file could be a movie
		/// </summary>
		private static readonly string[] movieIndicatorStrings = {
			"Movies",
			"Moviez",
			"Films",
			"Filme",
			"Film", 
			"Movie"
		};
		/// <summary>
		/// Languages for preselecting search results
		/// </summary>
		private static readonly string[] preferredResultLanguages = {
			"English|en",
			"German|Deutsch|ger|de",
			"Italian|Italiano|it",
			"French|Francais|Français|fr",
			"Spanish|Espanol|Hispanol|sp"
		};

		/// <summary>
		/// Sets if columns are resized to maintain aspect ratio
		/// </summary>
		public const string AUTO_RESIZE_COLUMNS = "1";
		/// <summary>
		/// Timeout for internet access in miliseconds
		/// </summary>
		public const string CONNECTION_TIMEOUT = "10000";
		/// <summary>
		/// Move files to proper directory structures
		/// </summary>
		public const string CREATE_DIRECTORY_STRUCTURE = "1";
		/// <summary>
		/// If season subdir is to be used
		/// </summary>
		public const string CREATE_SEASON_SUBFOLDERS = "1";
		/// <summary>
		/// Delete all empty subfolders
		/// </summary>
		public const string DELETE_ALL_EMPTY_FOLDERS = "1";
		/// <summary>
		/// Delete directories which were emptied because of file moving
		/// </summary>
		public const string DELETE_EMPTIED_FOLDERS = "1";
		/// <summary>
		/// Delete Sample files
		/// </summary>
		public const string DELETE_SAMPLE_FILES = "1";
		/// <summary>
		/// Internal delimiter for config file entries which are a list of values.
		/// Don't change if not absolutely neccessary.
		/// </summary>
		public const string DELIMITER = "^";
		/// <summary>
		/// Default destination directory
		/// </summary>
		public const string DESTINATION_DIRECTORY = "";
		/// <summary>
		/// What to do with diacritics
		/// </summary>
		public const DiacriticStrategy DIACRITIC_STRATEGY = DiacriticStrategy.Use;
		/// <summary>
		/// action to take on invalid filename characters
		/// </summary>
		public const string INVALID_CHAR_ACTION = "Ask";
		/// <summary>
		/// Last directory
		/// </summary>
		public const string LAST_DIRECTORY = "C:\\";
		/// <summary>
		/// Last subtitle provider
		/// </summary>
		public const string LAST_SELECTED_SUBTITLE_PROVIDER = "www.subtitles.de";
		/// <summary>
		/// Last selected tv show provider
		/// </summary>
		public const string LAST_USED_TV_SHOW_PROVIDER = "www.thetvdb.com";
		/// <summary>
		/// How to treat the letter casing of words
		/// </summary>
		public const LetterCaseStrategy LETTER_CASE_STRATEGY = LetterCaseStrategy.Ignore;
		/// <summary>
		/// What Verbosity Level the log file has
		/// NONE, CRITICAL, ERROR, WARNING, INFO, LOG, DEBUG, VERBOSE
		/// </summary>
		public const string LOGFILE_LOGLEVEL = "DEBUG";
		/// <summary>
		/// Log file name
		/// </summary>
		public const string LOGFILE_NAME = "Renamer.log";
		/// <summary>
		/// Maximum search depth for subdirectories. 
		/// 0 will scan only the current directory without subdirectories.
		/// </summary>
		public const string MAX_SEARCH_DEPTH = "2";
		/// <summary>
		/// What Verbosity Level the messagebox has
		/// NONE, CRITICAL, ERROR, WARNING, INFO, LOG, DEBUG, VERBOSE
		/// </summary>
		public const string MESSAGEBOX_LOGLEVEL = "CRITICAL";
		/// <summary>
		/// some signs which are removed (on movies?)
		/// </summary>
		public const string REGEX_SHOWNAME_CLEANUP = "[.-_;,!='+]";
		/// <summary>
		/// All invalid file name chars will be replaced by this string automatically if set.
		/// </summary>
		public const string REPLACE_INVALID_CHARS_WITH = "-";
		/// <summary>
		/// Find Missing episodes
		/// </summary>
		public const string REPORT_MISSING_EPISODES = "1";
		/// <summary>
		/// Number of history items to store
		/// </summary>
		public const string SHOWNAME_HISTORY_SIZE = "100";
		/// <summary>
		/// Pattern of the target filename. Placeholders:
		/// %S - Season (2 digits minimum)
		/// %s - Season (1 digit minimum)
		/// %E - Episode(2 digits minimum)
		/// %e - Episode(1 digit minimum)
		/// %N - Name
		/// %T - Title
		/// </summary>
		public const string TARGET_FILENAME_PATTERN = "S" + RenamingConstants.SEASON_NR_MARKER + "E" + RenamingConstants.EPISODE_NR_MARKER + " - " + RenamingConstants.EPISODE_TITLE_MARKER;
		/// <summary>
		/// What Verbosity Level the textbox has
		/// NONE, CRITICAL, ERROR, WARNING, INFO, LOG, DEBUG, VERBOSE
		/// </summary>
		public const string TEXTBOX_LOGLEVEL = "LOG";

		public static List<string> getVideoFileExtensions() {
			return new List<string>(videoFileExtensions);
		}

		public static List<string> getSubtitleFileExtensions() {
			return new List<string>(subtitleFileExtensions);
		}

		public static List<string> getEpisodeIdentifierPatterns() {
			return new List<string>(episodeIdentifierPatterns);
		}

		public static List<string> getLastSearchedShownames() {
			return new List<string>(lastSearchedShownames);
		}

		public static List<string> getSeasonNrExtractionPatterns() {
			return new List<string>(seasonNrExtractionPatterns);
		}
		public static List<string> getEmptyFolderCheckIgnoredFiletypes() {
			return new List<string>(emptyFolderCheckIgnoredFiletypes);
		}
		public static List<string> getMainScreenColumnOrder() {
			return new List<string>(mainScreenColumnOrder);
		}
		public static List<string> getMainScreenColumnWidth() {
			return new List<string>(mainScreenColumnWidth);
		}
		public static List<string> getWindowSize() {
			return new List<string>(windowSize);
		}
		public static List<string> getRegexExtractShownameFromFolder() {
			return new List<string>(regexExtractShownameFromFolder);
		}
		public static List<string> getCustomReplaceRegexStrings() {
			return new List<string>(customReplaceRegexStrings);
		}
		public static List<string> getMoviesTagsToRemoveList() {
			return new List<string>(moviesTagsToRemoveList);
		}
		public static List<string> getShownamePathWordBlacklist() {
			return new List<string>(shownamePathWordBlacklist);
		}
		public static List<string> getShownameFilenameBlacklist() {
			return new List<string>(shownameFilenameBlacklist);
		}
		public static List<string> getMovieIndicatorStrings() {
			return new List<string>(movieIndicatorStrings);
		}
		public static List<string> getPreferredResultLanguages() {
			return new List<string>(preferredResultLanguages);
		}

		public static AppConfigurationWrapper getDefaultConfiguration() {
			Configuration defaultConfiguration = new Configuration();
			defaultConfiguration[AppProperties.DELIMITER_KEY] = DELIMITER;
			defaultConfiguration[AppProperties.VIDEO_FILE_EXTENSIONS_KEY] = getVideoFileExtensions();
			defaultConfiguration[AppProperties.SUBTITLE_FILE_EXTENSIONS_KEY] = getSubtitleFileExtensions();
			defaultConfiguration[AppProperties.LOGFILE_LOGLEVEL_KEY] = LOGFILE_LOGLEVEL;
			defaultConfiguration[AppProperties.TEXTBOX_LOGLEVEL_KEY] = TEXTBOX_LOGLEVEL;
			defaultConfiguration[AppProperties.MESSAGEBOX_LOGLEVEL_KEY] = MESSAGEBOX_LOGLEVEL;
			defaultConfiguration[AppProperties.LOGFILE_NAME_KEY] = LOGFILE_NAME;
			defaultConfiguration[AppProperties.MAX_SEARCH_DEPTH_KEY] = MAX_SEARCH_DEPTH;
			defaultConfiguration[AppProperties.REPLACE_INVALID_CHARS_WITH_KEY] = REPLACE_INVALID_CHARS_WITH;
			defaultConfiguration[AppProperties.EPISODE_IDENTIFIER_PATTERNS_KEY] = getEpisodeIdentifierPatterns();
			defaultConfiguration[AppProperties.TARGET_FILENAME_PATTERN_KEY] = TARGET_FILENAME_PATTERN;
			defaultConfiguration[AppProperties.LAST_SEARCHED_SHOWNAMES_KEY] = getLastSearchedShownames();
			defaultConfiguration[AppProperties.LAST_USED_TV_SHOW_PROVIDER_KEY] = LAST_USED_TV_SHOW_PROVIDER;
			defaultConfiguration[AppProperties.LAST_DIRECTORY_KEY] = LAST_DIRECTORY;
			defaultConfiguration[AppProperties.LAST_SELECTED_SUBTITLE_PROVIDER_KEY] = LAST_SELECTED_SUBTITLE_PROVIDER;
			defaultConfiguration[AppProperties.CONNECTION_TIMEOUT_KEY] = CONNECTION_TIMEOUT;
			defaultConfiguration[AppProperties.DIACRITIC_STRATEGY_KEY] = DIACRITIC_STRATEGY.ToString();
			defaultConfiguration[AppProperties.LETTER_CASE_STRATEGY_KEY] = LETTER_CASE_STRATEGY.ToString();
			defaultConfiguration[AppProperties.SEASON_NR_EXTRACTION_PATTERNS_KEY] = getSeasonNrExtractionPatterns();
			defaultConfiguration[AppProperties.CREATE_DIRECTORY_STRUCTURE_KEY] = CREATE_DIRECTORY_STRUCTURE;
			defaultConfiguration[AppProperties.DELETE_EMPTIED_FOLDERS_KEY] = DELETE_EMPTIED_FOLDERS;
			defaultConfiguration[AppProperties.DELETE_ALL_EMPTY_FOLDERS_KEY] = DELETE_ALL_EMPTY_FOLDERS;
			defaultConfiguration[AppProperties.EMPTY_FOLDER_CHECK_IGNORED_FILETYPES_KEY] = getEmptyFolderCheckIgnoredFiletypes();
			defaultConfiguration[AppProperties.MAIN_SCREEN_COLUMN_ORDER_KEY] = getMainScreenColumnOrder();
			defaultConfiguration[AppProperties.MAIN_SCREEN_COLUMN_WIDTH_KEY] = getMainScreenColumnWidth();
			defaultConfiguration[AppProperties.WINDOW_SIZE_KEY] = getWindowSize();
			defaultConfiguration[AppProperties.CREATE_SEASON_SUBFOLDERS_KEY] = CREATE_SEASON_SUBFOLDERS;
			defaultConfiguration[AppProperties.SHOWNAME_HISTORY_SIZE_KEY] = SHOWNAME_HISTORY_SIZE;
			defaultConfiguration[AppProperties.REGEX_EXTRACT_SHOWNAME_FROM_FOLDER_KEY] = getRegexExtractShownameFromFolder();
			defaultConfiguration[AppProperties.REGEX_SHOWNAME_CLEANUP_KEY] = REGEX_SHOWNAME_CLEANUP;
			defaultConfiguration[AppProperties.CUSTOM_REPLACE_REGEX_STRINGS_KEY] = getCustomReplaceRegexStrings();
			defaultConfiguration[AppProperties.MOVIES_TAGS_TO_REMOVE_LIST_KEY] = getMoviesTagsToRemoveList();
			defaultConfiguration[AppProperties.AUTO_RESIZE_COLUMNS_KEY] = AUTO_RESIZE_COLUMNS;
			defaultConfiguration[AppProperties.SHOWNAME_PATH_WORD_BLACKLIST_KEY] = getShownamePathWordBlacklist();
			defaultConfiguration[AppProperties.REPORT_MISSING_EPISODES_KEY] = REPORT_MISSING_EPISODES;
			defaultConfiguration[AppProperties.DELETE_SAMPLE_FILES_KEY] = DELETE_SAMPLE_FILES;
			defaultConfiguration[AppProperties.DESTINATION_DIRECTORY_KEY] = DESTINATION_DIRECTORY;
			defaultConfiguration[AppProperties.SHOWNAME_FILENAME_BLACKLIST_KEY] = getShownameFilenameBlacklist();
			defaultConfiguration[AppProperties.MOVIE_INDICATOR_STRINGS_KEY] = getMovieIndicatorStrings();
			defaultConfiguration[AppProperties.DESTINATION_DIRECTORY_KEY] = DESTINATION_DIRECTORY;
			defaultConfiguration[AppProperties.PREFERRED_RESULT_LANGUAGES_KEY] = getPreferredResultLanguages();

			return new AppConfigurationWrapper(new ConfigurationWrapper(defaultConfiguration));
		}
		/// <summary>
		/// Returns the default value for a given property, if it is an already defined property
		/// </summary>
		/// <param name="propertyName">the name of the property</param>
		/// <returns></returns>
		public static object getDefaultValue(string propertyName) {
			switch (propertyName) {
				case AppProperties.AUTO_RESIZE_COLUMNS_KEY:
					return AUTO_RESIZE_COLUMNS;
				case AppProperties.CONNECTION_TIMEOUT_KEY:
					return CONNECTION_TIMEOUT;
				case AppProperties.CREATE_DIRECTORY_STRUCTURE_KEY:
					return CREATE_DIRECTORY_STRUCTURE;
				case AppProperties.CREATE_SEASON_SUBFOLDERS_KEY:
					return CREATE_SEASON_SUBFOLDERS;
				case AppProperties.CUSTOM_REPLACE_REGEX_STRINGS_KEY:
					return getCustomReplaceRegexStrings();
				case AppProperties.DELETE_ALL_EMPTY_FOLDERS_KEY:
					return DELETE_ALL_EMPTY_FOLDERS;
				case AppProperties.DELETE_EMPTIED_FOLDERS_KEY:
					return DELETE_EMPTIED_FOLDERS;
				case AppProperties.DELETE_SAMPLE_FILES_KEY:
					return DELETE_SAMPLE_FILES;
				case AppProperties.DELIMITER_KEY:
					return DELIMITER;
				case AppProperties.DESTINATION_DIRECTORY_KEY:
					return DESTINATION_DIRECTORY;
				case AppProperties.DIACRITIC_STRATEGY_KEY:
					return DIACRITIC_STRATEGY.ToString();
				case AppProperties.EMPTY_FOLDER_CHECK_IGNORED_FILETYPES_KEY:
					return getEmptyFolderCheckIgnoredFiletypes();
				case AppProperties.EPISODE_IDENTIFIER_PATTERNS_KEY:
					return getEpisodeIdentifierPatterns();
				case AppProperties.LAST_DIRECTORY_KEY:
					return LAST_DIRECTORY;
				case AppProperties.LAST_SEARCHED_SHOWNAMES_KEY:
					return getLastSearchedShownames();
				case AppProperties.LAST_SELECTED_SUBTITLE_PROVIDER_KEY:
					return LAST_SELECTED_SUBTITLE_PROVIDER;
				case AppProperties.LAST_USED_TV_SHOW_PROVIDER_KEY:
					return LAST_USED_TV_SHOW_PROVIDER;
				case AppProperties.LETTER_CASE_STRATEGY_KEY:
					return LETTER_CASE_STRATEGY.ToString();
				case AppProperties.LOGFILE_LOGLEVEL_KEY:
					return LOGFILE_LOGLEVEL;
				case AppProperties.LOGFILE_NAME_KEY:
					return LOGFILE_NAME;
				case AppProperties.MAIN_SCREEN_COLUMN_ORDER_KEY:
					return getMainScreenColumnOrder();
				case AppProperties.MAIN_SCREEN_COLUMN_WIDTH_KEY:
					return getMainScreenColumnWidth();
				case AppProperties.MAX_SEARCH_DEPTH_KEY:
					return MAX_SEARCH_DEPTH;
				case AppProperties.MESSAGEBOX_LOGLEVEL_KEY:
					return MESSAGEBOX_LOGLEVEL;
				case AppProperties.MOVIES_TAGS_TO_REMOVE_LIST_KEY:
					return getMoviesTagsToRemoveList();
				case AppProperties.MOVIE_INDICATOR_STRINGS_KEY:
					return getMovieIndicatorStrings();
				case AppProperties.PREFERRED_RESULT_LANGUAGES_KEY:
					return getPreferredResultLanguages();
				case AppProperties.REGEX_EXTRACT_SHOWNAME_FROM_FOLDER_KEY:
					return getRegexExtractShownameFromFolder();
				case AppProperties.REGEX_SHOWNAME_CLEANUP_KEY:
					return REGEX_SHOWNAME_CLEANUP;
				case AppProperties.REPLACE_INVALID_CHARS_WITH_KEY:
					return REPLACE_INVALID_CHARS_WITH;
				case AppProperties.REPORT_MISSING_EPISODES_KEY:
					return REPORT_MISSING_EPISODES;
				case AppProperties.SEASON_NR_EXTRACTION_PATTERNS_KEY:
					return getSeasonNrExtractionPatterns();
				case AppProperties.SHOWNAME_FILENAME_BLACKLIST_KEY:
					return getShownameFilenameBlacklist();
				case AppProperties.SHOWNAME_HISTORY_SIZE_KEY:
					return SHOWNAME_HISTORY_SIZE;
				case AppProperties.SHOWNAME_PATH_WORD_BLACKLIST_KEY:
					return getShownamePathWordBlacklist();
				case AppProperties.SUBTITLE_FILE_EXTENSIONS_KEY:
					return getSubtitleFileExtensions();
				case AppProperties.TARGET_FILENAME_PATTERN_KEY:
					return TARGET_FILENAME_PATTERN;
				case AppProperties.TEXTBOX_LOGLEVEL_KEY:
					return TEXTBOX_LOGLEVEL;
				case AppProperties.VIDEO_FILE_EXTENSIONS_KEY:
					return getVideoFileExtensions();
				case AppProperties.VISIBLE_COLUMNS_KEY:
					return null; // FIXME;
				case AppProperties.WINDOW_SIZE_KEY:
					return getWindowSize();
				default:
					return null;
			}
		}

		public static string getLogfilePath() {
			return AppConstants.APP_BASE_DIR + Path.DirectorySeparatorChar + AppConstants.APP_LOG_FILENAME;
		}

		/// <summary>
		/// generates the default filepath for the configuration file
		/// </summary>
		/// <returns>path to the configuration file</returns>
		public static string getDefaultConfigFilePath() {
			return AppConstants.APP_BASE_DIR + Path.DirectorySeparatorChar + ConfigurationManager.CONFIGURATION_DEFAULTS_FILENAME;
		}

	}
}
