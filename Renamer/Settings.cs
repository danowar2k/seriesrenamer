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
using System.IO;
using Renamer.Classes;
using Renamer.Classes.Configuration;
using System.Collections;
using Renamer.Logging;

namespace Renamer
{
    /// <summary>
    /// Class used to store settings which can't be stored in config file
    /// 
    /// This class is a Singleton, only one instance is available
    /// </summary>
    class Settings : IEnumerable
    {

        private static Settings instance;

        /// <summary>
        /// configurationFilePath of the main config file
        /// </summary>
        public const String MAIN_CONFIG_FILENAME = "Renamer.cfg";

        /// <summary>
        /// Comment string used in config file
        /// </summary>
        public const String COMMENT = "//";

        public const String BEGIN_MULTI_VALUE_FIELD = "{";

        public const String END_MULTI_VALUE_FIELD = "}";

        /// <summary>
        /// List of cached config files
        /// </summary>
        private Hashtable cachedConfigFiles;

        /// <summary>
        /// Cache config file containing default values for main config file
        /// </summary>
        private ConfigFile defaultConfigFile;

        /// <summary>
        /// Mono compatibility mode
        /// </summary>
        private bool monoCompatibilityMode;

        private string currentlyReadingFilePath;

        /// <summary>
        /// Get the instance of Settings, create the settings, if required
        /// </summary>
        /// <returns>actual Settings</returns>
        public static Settings Instance {
            get {
                if (instance == null) {
                    instance = new Settings();
                }
                return instance;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        protected Settings() {
            monoCompatibilityMode = inMonoCompatibilityMode();

            this.currentlyReadingFilePath = "";
            cachedConfigFiles = new Hashtable();
            SetupDefaults();
        }

        public ConfigFile this[string name] {
            get {
                if (!this.fileCached(name)) {
                    // Load a file because it's not available yet
                    if (this.IsMonoCompatibilityMode) {
                        Logger.Instance.LogMessage(name + " not loaded yet", LogLevel.LOG);
                    }
                    return this.addConfigFile(name);
                }
                return (ConfigFile)this.cachedConfigFiles[name];
            }
            set {
                if (this.cachedConfigFiles.ContainsKey(name)) {
                    this.cachedConfigFiles[name] = value;
                }
                else {
                    this.cachedConfigFiles.Add(name, value);
                }
            }
        }

        public ConfigFile this[int index] {
            get {
                return (ConfigFile)this.cachedConfigFiles[index];
            }
            set {
                this.cachedConfigFiles[index] = value;
            }
        }

        public ConfigFile Defaults {
            get {
                return defaultConfigFile;
            }
        }

        public bool IsMonoCompatibilityMode {
            get {
                return this.monoCompatibilityMode;
            }
        }


        // TODO: Instead of generating default values here, create a default config file inside the program and copy it
        // when necessary.
        /// <summary>
        /// Adds default values to default config file
        /// </summary>
        private void SetupDefaults() {
            defaultConfigFile = new ConfigFile("");
            defaultConfigFile[ConfigKeyConstants.DELIMITER_KEY] = "^";
            defaultConfigFile[ConfigKeyConstants.VIDEO_FILE_EXTENSIONS_KEY] = new List<string>(new string[] { "avi", "mpg", "mpeg", "mp4", "divx", "mkv", "wmv" });
            defaultConfigFile[ConfigKeyConstants.SUBTITLE_FILE_EXTENSIONS_KEY] = new List<string>(new string[] { "srt", "sub" });
            defaultConfigFile[ConfigKeyConstants.LOGFILE_LOGLEVEL_KEY] = "DEBUG";
            defaultConfigFile[ConfigKeyConstants.TEXTBOX_LOGLEVEL_KEY] = "LOG";
            defaultConfigFile[ConfigKeyConstants.MESSAGEBOX_LOGLEVEL_KEY] = "CRITICAL";
            defaultConfigFile[ConfigKeyConstants.LOGFILE_NAME_KEY] = "Renamer.log";
            defaultConfigFile[ConfigKeyConstants.MAX_SEARCH_DEPTH_KEY] = "2";
            defaultConfigFile[ConfigKeyConstants.REPLACE_INVALID_CHARS_WITH_KEY] = "-";
            defaultConfigFile[ConfigKeyConstants.EPISODE_IDENTIFIER_PATTERNS_KEY] = new List<string>(new string[] { "S%SE%E", "%Sx%E", "S%S.E%E", "- %S%E -", "-E%E-", ".%S%E.", "%S.%E", "%S%E" });
            defaultConfigFile[ConfigKeyConstants.TARGET_FILENAME_PATTERN_KEY] = "S%SE%E - %N";
            defaultConfigFile[ConfigKeyConstants.LAST_SEARCHED_SERIES_TITLES_KEY] = new List<string>();
            defaultConfigFile[ConfigKeyConstants.LAST_SELECTED_TITLE_PROVIDER_KEY] = "www.thetvdb.com";
            defaultConfigFile[ConfigKeyConstants.LAST_DIRECTORY_KEY] = "C:\\";
            defaultConfigFile[ConfigKeyConstants.LAST_SELECTED_SUBTITLE_PROVIDER_KEY] = "www.subtitles.de";
            defaultConfigFile[ConfigKeyConstants.CONNECTION_TIMEOUT_KEY] = "10000";
            defaultConfigFile[ConfigKeyConstants.DIACRITIC_STRATEGY_KEY] = Renamer.Classes.Candidate.UmlautAction.Use.ToString();
            defaultConfigFile[ConfigKeyConstants.LETTER_CASE_STRATEGY_KEY] = Renamer.Classes.Candidate.Case.Ignore.ToString();
            defaultConfigFile[ConfigKeyConstants.SEASON_NR_EXTRACTION_PATTERNS_KEY] = new List<string>(new string[] { "Season %S", "Season_%S", "Season%S", "Staffel %S", "Staffel_%S", "Staffel%S", "S%S" });
            defaultConfigFile[ConfigKeyConstants.CREATE_DIRECTORY_STRUCTURE_KEY] = "1";
            defaultConfigFile[ConfigKeyConstants.DELETE_EMPTIED_FOLDERS_KEY] = "1";
            defaultConfigFile[ConfigKeyConstants.DELETE_ALL_EMPTY_FOLDERS_KEY] = "1";
            defaultConfigFile[ConfigKeyConstants.EMPTY_FOLDER_CHECK_IGNORED_FILETYPES_KEY] = new List<string>(new string[] { "nfo", "diz" });
            defaultConfigFile[ConfigKeyConstants.MAIN_SCREEN_COLUMN_ORDER_KEY] = new List<string>(new string[] { "0", "1", "2", "3", "4", "5", "6", "7" });
            defaultConfigFile[ConfigKeyConstants.MAIN_SCREEN_COLUMN_WIDTH_KEY] = new List<string>(new string[] { "222", "203", "120", "51", "53", "122", "176", "163" });
            defaultConfigFile[ConfigKeyConstants.WINDOW_SIZE_KEY] = new List<string>(new string[] { "1024", "600" });
            defaultConfigFile[ConfigKeyConstants.CREATE_SEASON_SUBFOLDERS_KEY] = "1";
            defaultConfigFile[ConfigKeyConstants.TITLE_HISTORY_SIZE_KEY] = "100";
            defaultConfigFile[ConfigKeyConstants.REGEX_EXTRACT_SHOWNAME_FROM_FOLDER_KEY] = new List<string>(new string[] { "(?<pos>\\.Ep\\d+.)", "^(?<pos>[Ss]\\d+[Ee]\\d+[\\.-])", "((?<pos>\\.[Ss]\\d+[Ee]\\d+([\\.-]|\\w))|(?<pos>\\.\\d+\\.)|(?<pos>[\\. _-]\\d{3,})|(?<pos>\\d+x\\d+)|(?<pos>[Ss]\\d+x\\d+))", "(?<pos>\\d{2,4}.\\d{2,4}.\\d{2,4})" });
            defaultConfigFile[ConfigKeyConstants.REGEX_SHOWNAME_CLEANUP_KEY] = "[.-_;,!='+]";
            defaultConfigFile[ConfigKeyConstants.CUSTOM_REPLACE_REGEX_STRINGS_KEY] = new List<string>(new string[] { "//Comments are indicated by //", "//Format used here is \"From->To\",", "//where \"From\" is a c# regular expression, see", "//http://www.radsoftware.com.au/articles/regexlearnsyntax.aspx for details", "//example: \"\\s->.\" replaces whitespaces with dots", "cd->CD", " i\\.-> I.", " ii\\.-> II.", " iii\\.-> III.", " iv\\.-> IV.", " v\\.-> V.", " vi\\.-> VI.", " vii\\.-> VII.", " i -> I ", " ii -> II ", " iii -> III ", " iv -> IV ", " v -> V ", " vi -> VI ", " vii -> VII ", "\\[\\d+\\]->", " +-> " });
            defaultConfigFile[ConfigKeyConstants.MOVIES_TAGS_TO_REMOVE_LIST_KEY] = new List<string>(new string[] { "Xvid", "DivX", "R5", "R3", "GERMAN", "DVD", "INTERNAL", "PROPER", "TELECINE", "LINE", "LD", "MD", "AC3", "SVCD", "XSVCD", "VCD", "Dubbed", "HD", "720P", "720", "SCREENER", "RSVCD", "\\d{4}", "TS", "GER", "by \\w+", "xmas[^A-Za-z0-9]+special" });
            defaultConfigFile[ConfigKeyConstants.AUTO_RESIZE_COLUMNS_KEY] = "1";
            defaultConfigFile[ConfigKeyConstants.SHOWNAME_PATH_WORD_BLACKLIST_KEY] = new List<string>(new string[] {"Season", "Staffel", "Disk", "(D)isk", "(S)taffel", "(S)eason", "DVD", "Special", "Downloads", "Download", "Serien", "Series", "Serie", "Videos", "Movies", "Moviez", "Filme"});
            defaultConfigFile[ConfigKeyConstants.REPORT_MISSING_EPISODES_KEY] = "1";
            defaultConfigFile[ConfigKeyConstants.DELETE_SAMPLE_FILES_KEY] = "1";
            defaultConfigFile[ConfigKeyConstants.DESTINATION_DIRECTORY_KEY] = "";
            defaultConfigFile[ConfigKeyConstants.SHOWNAME_FILENAME_BLACKLIST_KEY] = new List<string>(new string[] { "AVSEQ01" });
            defaultConfigFile[ConfigKeyConstants.MOVIE_INDICATOR_STRINGS_KEY] = new List<string>(new string[] { "Movies", "Moviez", "Films", "Filme", "Film", "Movie" });
            defaultConfigFile[ConfigKeyConstants.DESTINATION_DIRECTORY_KEY] = "";
            defaultConfigFile[ConfigKeyConstants.PREFERRED_RESULT_LANGUAGES_KEY]=new List<string>(new string[]{"English|en","German|Deutsch|ger|de","Italian|Italiano|it", "French|Francais|Français|fr","Spanish|Espanol|Hispanol|sp"});
        }

        public bool fileCached(string filePath) {
            return this.cachedConfigFiles.ContainsKey(filePath);
        }

        public ConfigFile addConfigFile(string filePath) {
            if (filePath == currentlyReadingFilePath) {
                return null;
            }
            currentlyReadingFilePath = filePath;
            this[filePath] = new ConfigFile(filePath);
            currentlyReadingFilePath = "";
            return this[filePath];
        }

        private Boolean inMonoCompatibilityMode() {
            if (Type.GetType("Mono.Runtime") != null) {
                return true;
            } else {
                return false;
            }
        }

        #region IEnumerable Member

        public IEnumerator GetEnumerator() {
            return this.cachedConfigFiles.GetEnumerator();
        }

        #endregion
    }
}