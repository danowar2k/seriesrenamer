using System.Collections.Generic;
using Renamer.NewClasses.Util;

namespace Renamer.NewClasses.Config.Application {
	internal class AppPropertyComments {

		public const string INITIAL_COMMENT_KEY = "CONFIG_BEGIN";

		private static readonly string[] initialComments = {
			Configuration.COMMENT + " Series Renamer configuration file. Comments should start with " + Configuration.COMMENT +
			" and may not be put in the same line with a variable"
		};

		private static readonly string[] videoFileExtensions = {
			Configuration.COMMENT + " File extensions included in the renaming process"
		};

		private static readonly string[] subtitleFileExtensions = {
			Configuration.COMMENT + " Subtitle file extensions, also included in renaming"
		};

		private static readonly string[] loggingLevels = {
			Configuration.COMMENT + " What Verbosity Level the different Logging destinations have:",
			Configuration.COMMENT + " NONE, CRITICAL, ERROR, WARNING, INFO, LOG, DEBUG, VERBOSE"
		};

		private static readonly string[] logFilename = {
			Configuration.COMMENT + " Log file name"
		};

		private static readonly string[] maxSearchDepth = {
			Configuration.COMMENT +
			" Maximum search depth for subdirectories. 0 will scan only the current directory without subdirectories."
		};

		private static readonly string[] replaceInvalidCharsWith = {
			Configuration.COMMENT + " All invalid file name chars will be replaced by this string automatically if set."
		};

		private static readonly string[] episodeIdentifierPatterns = {
			Configuration.COMMENT + " Filters used to extract season and episode number from filename",
			Configuration.COMMENT + " Those filters are processed in the below order. Placeholders:",
			Configuration.COMMENT + " " + RenamingConstants.SEASON_NR_MARKER + " - Season",
			Configuration.COMMENT + " " + RenamingConstants.EPISODE_NR_MARKER + " - Episode"
		};

		private static readonly string[] targetFilenamePattern = {
			Configuration.COMMENT + " Pattern of the target filename. Placeholders:",
			Configuration.COMMENT + " " + RenamingConstants.SEASON_NR_MARKER + " - Season (2 digits minimum)",
			Configuration.COMMENT + " " + RenamingConstants.SEASON_NR_MARKER_NO_PAD + " - Season (1 digit minimum)",
			Configuration.COMMENT + " " + RenamingConstants.EPISODE_NR_MARKER + " - Episode(2 digits minimum)",
			Configuration.COMMENT + " " + RenamingConstants.EPISODE_NR_MARKER_NO_PAD + " - Episode(1 digit minimum)",
			Configuration.COMMENT + " " + RenamingConstants.EPISODE_TITLE_MARKER + " - Name",
			Configuration.COMMENT + " " + RenamingConstants.SHOWNAME_MARKER + " - Title"
		};

		private static readonly string[] lastUsedTvShowProvider = {
			Configuration.COMMENT + " Last selected tv show provider"
		};

		private static readonly string[] lastSearchedShownames = {
			Configuration.COMMENT + " Last queried shownames"
		};

		private static readonly string[] lastDirectory = {
			Configuration.COMMENT + " Last directory"
		};

		private static readonly string[] lastSelectedSubtitleProvider = {
			Configuration.COMMENT + " Last subtitle provider"
		};

		private static readonly string[] connectionTimeout = {
			Configuration.COMMENT + " Timeout for internet access in miliseconds"
		};

		private static readonly string[] invalidCharAction = {
			Configuration.COMMENT + " action to take on invalid filename characters"
		};

		private static readonly string[] diacriticStrategy = {
			Configuration.COMMENT + " What to do with diacritics"
		};

		private static readonly string[] letterCaseStrategy = {
			Configuration.COMMENT + " How to treat the letter casing of words"
		};

		private static readonly string[] createDirectoryStructure = {
			Configuration.COMMENT + " Move files to proper directory structures"
		};

		private static readonly string[] seasonNrExtractionPatterns = {
			Configuration.COMMENT + " Name of season dir, for extracting Season(" + RenamingConstants.SEASON_NR_MARKER +
			") and possibly name (" + RenamingConstants.SHOWNAME_MARKER + ")"
		};

		private static readonly string[] deleteEmptiedFolders = {
			Configuration.COMMENT + " Delete directories which got empty because of file moving"
		};

		private static readonly string[] deleteSampleFiles = {
			Configuration.COMMENT + " Delete Sample files"
		};

		private static readonly string[] reportMissingEpisodes = {
			Configuration.COMMENT + " Find Missing episodes"
		};

		private static readonly string[] emptyFolderCheckIgnoredFiletypes = {
			Configuration.COMMENT + " Also when they contain any of these filetypes"
		};

		private static readonly string[] deleteAllEmptyFolders = {
			Configuration.COMMENT + " Delete all empty subfolders"
		};

		private static readonly string[] autoResizeColumns = {
			Configuration.COMMENT + " Sets if columns are resized to maintain aspect ratio"
		};

		private static readonly string[] moviesTagsToRemoveList = {
			Configuration.COMMENT + " Tags which are recognized on movie files to rip them from filename"
		};

		private static readonly string[] customReplaceRegexStrings = {
			Configuration.COMMENT + " What to replace for final filenames"
		};

		private static readonly string[] regexExtractShownameFromFolder = {
			Configuration.COMMENT + " Regular expression for showname extraction from filenames"
		};

		private static readonly string[] shownamePathWordBlacklist = {
			Configuration.COMMENT + " Some blacklist used to ignore path names that can't be used for showname extraction"
		};

		private static readonly string[] shownameFilenameBlacklist = {
			Configuration.COMMENT + " Blacklist to determine filenames which can't be used to extract things from"
		};

		private static readonly string[] movieIndicatorStrings = {
			Configuration.COMMENT + " List used as a test to see if a file could be a movie"
		};

		private static readonly string[] regexShownameCleanup = {
			Configuration.COMMENT + " some signs which are removed (on movies?)"
		};

		private static readonly string[] shownameHistorySize = {
			Configuration.COMMENT + " Number of history items to store"
		};

		private static readonly string[] createSeasonSubfolders = {
			Configuration.COMMENT + " If season subdir is to be used"
		};

		private static readonly string[] windowSize = {
			Configuration.COMMENT + " size of the main window"
		};

		private static readonly string[] mainScreenColumnOrder = {
			Configuration.COMMENT + " Order of the file window columns"
		};

		private static readonly string[] mainScreenColumnWidth = {
			Configuration.COMMENT + " Width of the file window columns"
		};

		private static readonly string[] destinationDirectory = {
			Configuration.COMMENT + " Default destination directory"
		};

		private static readonly string[] delimiter = {
			Configuration.COMMENT +
			" Internal delimiter for config file entries which are a list of values. Don't change if not absolutely neccessary."
		};

		private static readonly string[] preferredResultLanguages = {
			Configuration.COMMENT + " Languages for preselecting search results"
		};

		public static List<string> getComments(string propertyName) {
			switch (propertyName) {
				case INITIAL_COMMENT_KEY:
					return new List<string>(initialComments);
				case AppProperties.AUTO_RESIZE_COLUMNS_KEY:
					return new List<string>(autoResizeColumns);
				case AppProperties.CONNECTION_TIMEOUT_KEY:
					return new List<string>(connectionTimeout);
				case AppProperties.CREATE_DIRECTORY_STRUCTURE_KEY:
					return new List<string>(createDirectoryStructure);
				case AppProperties.CREATE_SEASON_SUBFOLDERS_KEY:
					return new List<string>(createSeasonSubfolders);
				case AppProperties.CUSTOM_REPLACE_REGEX_STRINGS_KEY:
					return new List<string>(customReplaceRegexStrings);
				case AppProperties.DELETE_ALL_EMPTY_FOLDERS_KEY:
					return new List<string>(deleteAllEmptyFolders);
				case AppProperties.DELETE_EMPTIED_FOLDERS_KEY:
					return new List<string>(deleteEmptiedFolders);
				case AppProperties.DELETE_SAMPLE_FILES_KEY:
					return new List<string>(deleteSampleFiles);
				case AppProperties.DELIMITER_KEY:
					return new List<string>(delimiter);
				case AppProperties.DESTINATION_DIRECTORY_KEY:
					return new List<string>(destinationDirectory);
				case AppProperties.DIACRITIC_STRATEGY_KEY:
					return new List<string>(diacriticStrategy);
				case AppProperties.EMPTY_FOLDER_CHECK_IGNORED_FILETYPES_KEY:
					return new List<string>(emptyFolderCheckIgnoredFiletypes);
				case AppProperties.EPISODE_IDENTIFIER_PATTERNS_KEY:
					return new List<string>(episodeIdentifierPatterns);
				case AppProperties.LAST_DIRECTORY_KEY:
					return new List<string>(lastDirectory);
				case AppProperties.LAST_SEARCHED_SHOWNAMES_KEY:
					return new List<string>(lastSearchedShownames);
				case AppProperties.LAST_SELECTED_SUBTITLE_PROVIDER_KEY:
					return new List<string>(lastSelectedSubtitleProvider);
				case AppProperties.LAST_USED_TV_SHOW_PROVIDER_KEY:
					return new List<string>(lastUsedTvShowProvider);
				case AppProperties.LETTER_CASE_STRATEGY_KEY:
					return new List<string>(letterCaseStrategy);
				case AppProperties.LOGFILE_LOGLEVEL_KEY:
					return new List<string>(loggingLevels);
				case AppProperties.LOGFILE_NAME_KEY:
					return new List<string>(logFilename);
				case AppProperties.MAIN_SCREEN_COLUMN_ORDER_KEY:
					return new List<string>(mainScreenColumnOrder);
				case AppProperties.MAIN_SCREEN_COLUMN_WIDTH_KEY:
					return new List<string>(mainScreenColumnWidth);
				case AppProperties.MAX_SEARCH_DEPTH_KEY:
					return new List<string>(maxSearchDepth);
				case AppProperties.MESSAGEBOX_LOGLEVEL_KEY:
					return new List<string>();
				case AppProperties.MOVIES_TAGS_TO_REMOVE_LIST_KEY:
					return new List<string>(moviesTagsToRemoveList);
				case AppProperties.MOVIE_INDICATOR_STRINGS_KEY:
					return new List<string>(movieIndicatorStrings);
				case AppProperties.PREFERRED_RESULT_LANGUAGES_KEY:
					return new List<string>(preferredResultLanguages);
				case AppProperties.REGEX_EXTRACT_SHOWNAME_FROM_FOLDER_KEY:
					return new List<string>(regexExtractShownameFromFolder);
				case AppProperties.REGEX_SHOWNAME_CLEANUP_KEY:
					return new List<string>(regexShownameCleanup);
				case AppProperties.REPLACE_INVALID_CHARS_WITH_KEY:
					return new List<string>(replaceInvalidCharsWith);
				case AppProperties.REPORT_MISSING_EPISODES_KEY:
					return new List<string>(reportMissingEpisodes);
				case AppProperties.SEASON_NR_EXTRACTION_PATTERNS_KEY:
					return new List<string>(seasonNrExtractionPatterns);
				case AppProperties.SHOWNAME_FILENAME_BLACKLIST_KEY:
					return new List<string>(shownameFilenameBlacklist);
				case AppProperties.SHOWNAME_HISTORY_SIZE_KEY:
					return new List<string>(shownameHistorySize);
				case AppProperties.SHOWNAME_PATH_WORD_BLACKLIST_KEY:
					return new List<string>(shownamePathWordBlacklist);
				case AppProperties.SUBTITLE_FILE_EXTENSIONS_KEY:
					return new List<string>(subtitleFileExtensions);
				case AppProperties.TARGET_FILENAME_PATTERN_KEY:
					return new List<string>(targetFilenamePattern);
				case AppProperties.TEXTBOX_LOGLEVEL_KEY:
					return new List<string>();
				case AppProperties.VIDEO_FILE_EXTENSIONS_KEY:
					return new List<string>(videoFileExtensions);
				case AppProperties.VISIBLE_COLUMNS_KEY:
					return null; // FIXME;
				case AppProperties.WINDOW_SIZE_KEY:
					return new List<string>(windowSize);
				default:
					return new List<string>();
			}
		}
	}
}