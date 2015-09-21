using System;
using System.Collections.Generic;
using Renamer.NewClasses.Enums;
using Renamer.NewClasses.Util;

namespace Renamer.NewClasses.Config.Application {
	public class AppConfigurationWrapper {

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger
	(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private readonly ConfigurationWrapper wrappedConfiguration;

		public AppConfigurationWrapper(ConfigurationWrapper someWrappedConfiguration) {
			wrappedConfiguration = someWrappedConfiguration;
		}

		public ConfigurationWrapper getWrappedConfiguration() {
			return wrappedConfiguration;
		}

		public string getInvalidCharReplaceWith() {
			return wrappedConfiguration.getSingleStringProperty(AppProperties.REPLACE_INVALID_CHARS_WITH_KEY);
		}
		public void setInvalidCharReplaceWith(string newValue) {
			wrappedConfiguration.set(AppProperties.REPLACE_INVALID_CHARS_WITH_KEY, newValue);
		}

		public string getDestinationDirectory() {
			return wrappedConfiguration.getSingleStringProperty(AppProperties.DESTINATION_DIRECTORY_KEY);
		}
		public void setDestinationDirectory(string newValue) {
			wrappedConfiguration.set(AppProperties.DESTINATION_DIRECTORY_KEY, newValue);
		}

		public string getLastUsedTvShowProvider() {
			return wrappedConfiguration.getSingleStringProperty(AppProperties.LAST_USED_TV_SHOW_PROVIDER_KEY);
		}
		public void setLastUsedTvShowProvider(string newValue) {
			wrappedConfiguration.set(AppProperties.LAST_USED_TV_SHOW_PROVIDER_KEY, newValue);
		}

		public List<string> getMovieTagsToRemove() {
			return wrappedConfiguration.getMultiStringProperty(AppProperties.MOVIES_TAGS_TO_REMOVE_LIST_KEY);
		}
		public void setMovieTagsToRemove(List<string> newValues) {
			wrappedConfiguration.set(AppProperties.MOVIES_TAGS_TO_REMOVE_LIST_KEY, newValues);
		}

		public List<string> getSeasonNrExtractionPatterns() {
			return wrappedConfiguration.getMultiStringProperty(AppProperties.SEASON_NR_EXTRACTION_PATTERNS_KEY);
		}
		public void setSeasonNrExtractionPatterns(List<string> newValues) {
			wrappedConfiguration.set(AppProperties.SEASON_NR_EXTRACTION_PATTERNS_KEY, newValues);
		}

		public List<string> getEpisodeIdentifierPatterns() {
			return wrappedConfiguration.getMultiStringProperty(AppProperties.EPISODE_IDENTIFIER_PATTERNS_KEY);
		}
		public void setEpisodeIdentifierPatterns(List<string> newValues) {
			wrappedConfiguration.set(AppProperties.EPISODE_IDENTIFIER_PATTERNS_KEY, newValues);
		}

		public List<string> getCustomReplaceRegexes() {
			return wrappedConfiguration.getMultiStringProperty(AppProperties.CUSTOM_REPLACE_REGEX_STRINGS_KEY);
		}
		public void setCustomReplaceRegexes(List<string> newValues) {
			wrappedConfiguration.set(AppProperties.CUSTOM_REPLACE_REGEX_STRINGS_KEY, newValues);
		}

		public List<string> getVideoFileExtensions() {
			return wrappedConfiguration.getMultiStringProperty(AppProperties.VIDEO_FILE_EXTENSIONS_KEY);
		}
		public void setVideoFileExtensions(List<string> newValues) {
			wrappedConfiguration.set(AppProperties.VIDEO_FILE_EXTENSIONS_KEY, newValues);
		}

		public List<string> getSubtitleFileExtensions() {
			return wrappedConfiguration.getMultiStringProperty(AppProperties.SUBTITLE_FILE_EXTENSIONS_KEY);
		}
		public void setSubtitleFileExtensions(List<string> newValues) {
			wrappedConfiguration.set(AppProperties.SUBTITLE_FILE_EXTENSIONS_KEY, newValues);
		}

		public List<string> getEmptyFolderIgnoredFiletypes() {
			return wrappedConfiguration.getMultiStringProperty(AppProperties.EMPTY_FOLDER_CHECK_IGNORED_FILETYPES_KEY);
		}
		public void setEmptyFolderIgnoredFiletypes(List<string> newValues) {
			wrappedConfiguration.set(AppProperties.EMPTY_FOLDER_CHECK_IGNORED_FILETYPES_KEY, newValues);
		}

		public List<string> getPreferredLanguages() {
			return wrappedConfiguration.getMultiStringProperty(AppProperties.PREFERRED_RESULT_LANGUAGES_KEY);
		}
		public void setPreferredLanguages(List<string> newValues) {
			wrappedConfiguration.set(AppProperties.PREFERRED_RESULT_LANGUAGES_KEY, newValues);
		}

		public DiacriticStrategy getDiacriticStrategy() {
			return wrappedConfiguration.getEnumProperty<DiacriticStrategy>(AppProperties.DIACRITIC_STRATEGY_KEY);
		}

		public void setDiacriticStrategy(string newValue) {
			DiacriticStrategy newStrategy;
			if (Enum.TryParse(newValue, out newStrategy)) {
				setDiacriticStrategy(newStrategy);
			}
		}
		public void setDiacriticStrategy(DiacriticStrategy newValue) {
			wrappedConfiguration.set(AppProperties.DIACRITIC_STRATEGY_KEY, newValue);
		}

		public LetterCaseStrategy getLetterCaseStrategy() {
			return wrappedConfiguration.getEnumProperty<LetterCaseStrategy>(AppProperties.LETTER_CASE_STRATEGY_KEY);
		}
		public void setLetterCaseStrategy(string newValue) {
			LetterCaseStrategy newStrategy;
			if (Enum.TryParse(newValue, out newStrategy)) {
				setLetterCaseStrategy(newStrategy);
			}
		}
		public void setLetterCaseStrategy(LetterCaseStrategy newValue) {
			wrappedConfiguration.set(AppProperties.LETTER_CASE_STRATEGY_KEY, newValue);
		}
		public int getMaxSearchDepth() {
			return (int) wrappedConfiguration.getSingleNumberProperty(AppProperties.MAX_SEARCH_DEPTH_KEY);
		}
		public void setMaxSearchDepth(int newValue) {
			wrappedConfiguration.set(AppProperties.MAX_SEARCH_DEPTH_KEY, newValue);
		}

		public int getConnectionTimeout() {
			return (int) wrappedConfiguration.getSingleNumberProperty(AppProperties.CONNECTION_TIMEOUT_KEY);
		}
		public void setConnectionTimeout(int newValue) {
			wrappedConfiguration.set(AppProperties.CONNECTION_TIMEOUT_KEY, newValue);
		}

		public bool getCreateDirectoryStructure() {
			return wrappedConfiguration.getSingleBoolProperty(AppProperties.CREATE_DIRECTORY_STRUCTURE_KEY);
		}
		public void setCreateDirectoryStructure(bool newValue) {
			wrappedConfiguration.set(AppProperties.CREATE_DIRECTORY_STRUCTURE_KEY, newValue);
		}
		public bool getDeleteEmptiedFolders() {
			return wrappedConfiguration.getSingleBoolProperty(AppProperties.DELETE_EMPTIED_FOLDERS_KEY);
		}
		public void setDeleteEmptiedFolders(bool newValue) {
			wrappedConfiguration.set(AppProperties.DELETE_EMPTIED_FOLDERS_KEY, newValue);
		}
		public bool getDeleteAllEmptyFolders() {
			return wrappedConfiguration.getSingleBoolProperty(AppProperties.DELETE_ALL_EMPTY_FOLDERS_KEY);
		}
		public void setDeleteAllEmptyFolders(bool newValue) {
			wrappedConfiguration.set(AppProperties.DELETE_ALL_EMPTY_FOLDERS_KEY, newValue);
		}
		public bool getCreateSeasonSubfolders() {
			return wrappedConfiguration.getSingleBoolProperty(AppProperties.CREATE_SEASON_SUBFOLDERS_KEY);
		}
		public void setCreateSeasonSubfolders(bool newValue) {
			wrappedConfiguration.set(AppProperties.CREATE_SEASON_SUBFOLDERS_KEY, newValue);
		}
		public bool getAutoResizeColumns() {
			return wrappedConfiguration.getSingleBoolProperty(AppProperties.AUTO_RESIZE_COLUMNS_KEY);
		}
		public void setAutoResizeColumns(bool newValue) {
			wrappedConfiguration.set(AppProperties.AUTO_RESIZE_COLUMNS_KEY, newValue);
		}
		public bool getReportMissingEpisodes() {
			return wrappedConfiguration.getSingleBoolProperty(AppProperties.REPORT_MISSING_EPISODES_KEY);
		}
		public void setReportMissingEpisodes(bool newValue) {
			wrappedConfiguration.set(AppProperties.REPORT_MISSING_EPISODES_KEY, newValue);
		}
		public bool getDeleteSampleFiles() {
			return wrappedConfiguration.getSingleBoolProperty(AppProperties.DELETE_SAMPLE_FILES_KEY);
		}
		public void setDeleteSampleFiles(bool newValue) {
			wrappedConfiguration.set(AppProperties.DELETE_SAMPLE_FILES_KEY, newValue);
		}

		public void updateLanguagePriorities(Language firstLanguage) {
			
		}
		public void updateLastShownames(string someShowname) {
			List<string> lastTitles = wrappedConfiguration.getMultiStringProperty(AppProperties.LAST_SEARCHED_SHOWNAMES_KEY);
			lastTitles = ListUtils.update(lastTitles, someShowname, true);
			wrappedConfiguration.set(AppProperties.LAST_SEARCHED_SHOWNAMES_KEY, lastTitles);
		}
	}
}
