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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Renamer.Classes.Configuration;
using Renamer.Classes;
using Renamer.Classes.Provider;
using Renamer.Logging;

namespace Renamer.Dialogs
{
    /// <summary>
    /// Configuration dialog
    /// </summary>
    public partial class ConfigurationDialog : Form
    {
        /// <summary>
        /// standard constructor
        /// </summary>
        public ConfigurationDialog() {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization, sets all values from configuration file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigurationDialog_Load(object sender, EventArgs e) {
            txtReplaceInvalidFilenameCharsWith.Text = Helper.ReadProperty(ConfigKeyConstants.REPLACE_INVALID_CHARS_WITH_KEY);
            txtEmptyFolderCheckIgnoredFiletypes.Text = Helper.ReadProperty(ConfigKeyConstants.EMPTY_FOLDER_CHECK_IGNORED_FILETYPES_KEY);
            txtDefaultDestinationDir.Text = Helper.ReadProperty(ConfigKeyConstants.DESTINATION_DIRECTORY_KEY);

            readMultilineProperty(txtVideoFileExtensions, ConfigKeyConstants.VIDEO_FILE_EXTENSIONS_KEY, true);
            readMultilineProperty(txtSubtitleFileExtensions, ConfigKeyConstants.SUBTITLE_FILE_EXTENSIONS_KEY, true);
            readMultilineProperty(txtEpisodeIdentifierPatterns, ConfigKeyConstants.EPISODE_IDENTIFIER_PATTERNS_KEY);
            readMultilineProperty(txtReplacementsWhenRenaming, ConfigKeyConstants.CUSTOM_REPLACE_REGEX_STRINGS_KEY);
            readMultilineProperty(txtRemovedMovieTags, ConfigKeyConstants.MOVIES_TAGS_TO_REMOVE_LIST_KEY);
            readMultilineProperty(txtSeasonNrExtractionPatterns, ConfigKeyConstants.SEASON_NR_EXTRACTION_PATTERNS_KEY);

            cbMinLevelLogToFile.SelectedIndex = (int)(Helper.ReadEnum<Logging.LogLevel>(ConfigKeyConstants.LOGFILE_LOGLEVEL_KEY));
            cbMinLevelLogToWindow.SelectedIndex = (int)(Helper.ReadEnum<Logging.LogLevel>(ConfigKeyConstants.TEXTBOX_LOGLEVEL_KEY));
            cbMinLevelLogToMessageBox.SelectedIndex = (int)(Helper.ReadEnum<Logging.LogLevel>(ConfigKeyConstants.MESSAGEBOX_LOGLEVEL_KEY));

            cbDiacriticStrategy.SelectedIndex = (int)Helper.ReadEnum<Candidate.UmlautAction>(ConfigKeyConstants.DIACRITIC_STRATEGY_KEY) - 1;
            cbLetterCaseStrategy.SelectedIndex = (int)Helper.ReadEnum<Candidate.Case>(ConfigKeyConstants.LETTER_CASE_STRATEGY_KEY) - 1;

            // FIXME: wohl eher int
            nudSearchDepth.Text = Helper.ReadProperty(ConfigKeyConstants.MAX_SEARCH_DEPTH_KEY);
            nudConnectionTimeout.Value = Helper.ReadInt(ConfigKeyConstants.CONNECTION_TIMEOUT_KEY);

            chkCreateDirectoryStructure.Checked = Helper.ReadBool(ConfigKeyConstants.CREATE_DIRECTORY_STRUCTURE_KEY);
            chkDeleteEmptiedFolders.Checked = Helper.ReadBool(ConfigKeyConstants.DELETE_EMPTIED_FOLDERS_KEY);
            chkDeleteAllEmptyFolders.Checked = Helper.ReadBool(ConfigKeyConstants.DELETE_ALL_EMPTY_FOLDERS_KEY);
            chkCreateSeasonSubdir.Checked = Helper.ReadBool(ConfigKeyConstants.CREATE_SEASON_SUBFOLDERS_KEY);
            chkAutoResizeColumns.Checked = Helper.ReadBool(ConfigKeyConstants.AUTO_RESIZE_COLUMNS_KEY);
            chkReportMissingEpisodes.Checked = Helper.ReadBool(ConfigKeyConstants.REPORT_MISSING_EPISODES_KEY);
            chkDeleteSampleFiles.Checked = Helper.ReadBool(ConfigKeyConstants.DELETE_SAMPLE_FILES_KEY);         
   
            cbDefaultTitleProvider.Items.AddRange(TitleProvider.getProviderNames().ToArray());

            List<string> prioritizedLanguages = new List<string>(Helper.ReadProperties(ConfigKeyConstants.PREFERRED_RESULT_LANGUAGES_KEY));
            foreach (string prioritizedLanguage in prioritizedLanguages)
            {
                string englishLanguageName = prioritizedLanguage.Substring(0, prioritizedLanguage.IndexOf("|"));
                cbPreferredLanguage.Items.Add(englishLanguageName);
            }
            if (prioritizedLanguages.Count > 0)
            {
                cbPreferredLanguage.SelectedIndex = 0;
            }

            string LastProvider = Helper.ReadProperty(ConfigKeyConstants.LAST_SELECTED_TITLE_PROVIDER_KEY);
            if (LastProvider == null)
                LastProvider = "";
            cbDefaultTitleProvider.SelectedIndex = Math.Max(0, cbDefaultTitleProvider.Items.IndexOf(LastProvider));
            TitleProvider provider = TitleProvider.GetCurrentProvider();
            // FIXME: Warum nicht früher abbrechen?
            if (provider == null)
            {
                Logger.Instance.LogMessage("No title provider found/selected", LogLevel.ERROR);
                return;
            }
            Helper.WriteProperty(ConfigKeyConstants.LAST_SELECTED_TITLE_PROVIDER_KEY, cbDefaultTitleProvider.Text);
        }

        private void readMultilineProperty(TextBox targetBox, string sourcePropertyName, bool toLowerCase = false) {
            targetBox.Text = "";
            foreach (string s in Helper.ReadProperties(sourcePropertyName)) {
                string value;
                if (toLowerCase) {
                    value = s.ToLower();
                } else {
                    value = s;
                }
                targetBox.Text += value + Environment.NewLine;
            }
            if (targetBox.Text.Length > 0)
                targetBox.Text = targetBox.Text.Substring(0, targetBox.Text.Length - Environment.NewLine.Length);

        }
        
        private void writeMultilineProperty(TextBox sourceBox, string targetPropertyName, bool toLowerCase = false) {
            string[] multiLineString = sourceBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (toLowerCase) {
                for (int i = 0; i < multiLineString.GetLength(0); i++) {
                    multiLineString[i] = multiLineString[i].ToLower();
                }
            }
            Helper.WriteProperties(targetPropertyName, multiLineString);
        }

        private Boolean anyLogPropertyChanged() {
            if (Helper.ReadProperty(ConfigKeyConstants.LOGFILE_LOGLEVEL_KEY) != cbMinLevelLogToFile.SelectedIndex.ToString()
                || Helper.ReadProperty(ConfigKeyConstants.TEXTBOX_LOGLEVEL_KEY) != cbMinLevelLogToWindow.SelectedIndex.ToString()
                || Helper.ReadProperty(ConfigKeyConstants.MESSAGEBOX_LOGLEVEL_KEY) != cbMinLevelLogToMessageBox.SelectedIndex.ToString()) {
                return true;
            }
            return false;
        }

        private List<string> changeLanguagePriorities(List<string> languagePriorities, string newPreferredLanguage) {
            string matchedLanguageEntry = "";
            foreach (string someLanguage in languagePriorities) {
                if (someLanguage.StartsWith(newPreferredLanguage)) {
                    matchedLanguageEntry = someLanguage;
                    break;
                }
            }
            if (!String.IsNullOrEmpty(matchedLanguageEntry)) {
                languagePriorities.Remove(matchedLanguageEntry);
                languagePriorities.Insert(0, matchedLanguageEntry);
            } else {
                languagePriorities.Insert(0, newPreferredLanguage);
            }
            return languagePriorities;
        }

        /// <summary>
        /// Discards all changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelAndClose_Click(object sender, EventArgs e) {
            Close();
        }

        /// <summary>
        /// Saves all settings to config file cache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAndClose_Click(object sender, EventArgs e) {
            bool clearLog = false;
            if (anyLogPropertyChanged()) {
                clearLog = true;
            }

            Helper.WriteProperty(ConfigKeyConstants.REPLACE_INVALID_CHARS_WITH_KEY, txtReplaceInvalidFilenameCharsWith.Text);
            Helper.WriteProperties(ConfigKeyConstants.EMPTY_FOLDER_CHECK_IGNORED_FILETYPES_KEY, txtEmptyFolderCheckIgnoredFiletypes.Text);
            Helper.WriteProperty(ConfigKeyConstants.DESTINATION_DIRECTORY_KEY, txtDefaultDestinationDir.Text);

            writeMultilineProperty(txtVideoFileExtensions, ConfigKeyConstants.VIDEO_FILE_EXTENSIONS_KEY, true);
            writeMultilineProperty(txtSubtitleFileExtensions, ConfigKeyConstants.SUBTITLE_FILE_EXTENSIONS_KEY, true);
            writeMultilineProperty(txtEpisodeIdentifierPatterns, ConfigKeyConstants.EPISODE_IDENTIFIER_PATTERNS_KEY);
            writeMultilineProperty(txtReplacementsWhenRenaming, ConfigKeyConstants.CUSTOM_REPLACE_REGEX_STRINGS_KEY);
            writeMultilineProperty(txtRemovedMovieTags, ConfigKeyConstants.MOVIES_TAGS_TO_REMOVE_LIST_KEY);
            writeMultilineProperty(txtSeasonNrExtractionPatterns, ConfigKeyConstants.SEASON_NR_EXTRACTION_PATTERNS_KEY);

            Helper.WriteProperty(ConfigKeyConstants.LOGFILE_LOGLEVEL_KEY, cbMinLevelLogToFile.SelectedIndex.ToString());
            Helper.WriteProperty(ConfigKeyConstants.TEXTBOX_LOGLEVEL_KEY, cbMinLevelLogToWindow.SelectedIndex.ToString());
            Helper.WriteProperty(ConfigKeyConstants.MESSAGEBOX_LOGLEVEL_KEY, cbMinLevelLogToMessageBox.SelectedIndex.ToString());

            Helper.WriteProperty(ConfigKeyConstants.DIACRITIC_STRATEGY_KEY, Enum.GetName(typeof(Candidate.UmlautAction), cbDiacriticStrategy.SelectedIndex + 1));
            Helper.WriteProperty(ConfigKeyConstants.LETTER_CASE_STRATEGY_KEY, Enum.GetName(typeof(Candidate.Case), cbLetterCaseStrategy.SelectedIndex + 1));

            Helper.WriteProperty(ConfigKeyConstants.MAX_SEARCH_DEPTH_KEY, nudSearchDepth.Value.ToString());
            Helper.WriteProperty(ConfigKeyConstants.CONNECTION_TIMEOUT_KEY, nudConnectionTimeout.Value.ToString());

           Helper.WriteBool(ConfigKeyConstants.CREATE_DIRECTORY_STRUCTURE_KEY, chkCreateDirectoryStructure.Checked);
            Helper.WriteBool(ConfigKeyConstants.DELETE_EMPTIED_FOLDERS_KEY, chkDeleteEmptiedFolders.Checked);
            Helper.WriteBool(ConfigKeyConstants.DELETE_ALL_EMPTY_FOLDERS_KEY, chkDeleteAllEmptyFolders.Checked);
            Helper.WriteBool(ConfigKeyConstants.CREATE_SEASON_SUBFOLDERS_KEY, chkCreateSeasonSubdir.Checked);
            Helper.WriteBool(ConfigKeyConstants.AUTO_RESIZE_COLUMNS_KEY, chkAutoResizeColumns.Checked);
            Helper.WriteBool(ConfigKeyConstants.REPORT_MISSING_EPISODES_KEY, chkReportMissingEpisodes.Checked);
            Helper.WriteBool(ConfigKeyConstants.DELETE_SAMPLE_FILES_KEY, chkDeleteSampleFiles.Checked);

            List<string> languagePriorities = new List<string>(Helper.ReadProperties(ConfigKeyConstants.PREFERRED_RESULT_LANGUAGES_KEY));
            languagePriorities = changeLanguagePriorities(languagePriorities, cbPreferredLanguage.Text);
            Helper.WriteProperties(ConfigKeyConstants.PREFERRED_RESULT_LANGUAGES_KEY, languagePriorities.ToArray());

            Helper.WriteProperty(ConfigKeyConstants.LAST_SELECTED_TITLE_PROVIDER_KEY, cbDefaultTitleProvider.SelectedItem.ToString());

            if (clearLog)
            {
                MainForm.Instance.initMyLoggers();
                Logger.Instance.LogMessage("Cleared log window because logging settings changed.", LogLevel.INFO);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Resets config file to default values stored in Settings class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetToDefaults_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Reset to defaults?", "Reset", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                resetToDefaults();
                ConfigurationDialog_Load(null, null);
            }
        }

        private void resetToDefaults() {
            Settings settings = Settings.Instance;
            //find config file, delete in memory, overwrite physical file and reload it. Then, refresh this dialog
            string defaultConfigFilePath = Helper.DefaultConfigFile();
            settings[defaultConfigFilePath].LoadDefaults();
            settings[defaultConfigFilePath].Flush();
        }

        private void chkDeleteEmptiedFolders_CheckedChanged(object sender, EventArgs e) {
            lblEmptyFolderCheckIgnoredFiletypes.Enabled = chkDeleteEmptiedFolders.Checked;
            lblDeleteAllEmptyFolders.Enabled = chkDeleteEmptiedFolders.Checked;
            chkDeleteAllEmptyFolders.Enabled = chkDeleteEmptiedFolders.Checked;
            txtEmptyFolderCheckIgnoredFiletypes.Enabled = chkDeleteEmptiedFolders.Checked;
        }

        private void btnChangeDefaultDestinationDir_Click(object sender, EventArgs e)
        {
            changeDestionationDirDialog.SelectedPath = txtDefaultDestinationDir.Text;
            if (changeDestionationDirDialog.ShowDialog() == DialogResult.OK)
            {
                txtDefaultDestinationDir.Text = changeDestionationDirDialog.SelectedPath;
            }
        }

    }

}
