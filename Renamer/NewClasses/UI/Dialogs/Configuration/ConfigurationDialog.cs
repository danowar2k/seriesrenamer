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
using System.Globalization;
using System.Windows.Forms;
using Renamer.NewClasses.Config;
using Renamer.NewClasses.Config.Application;
using Renamer.NewClasses.Providers;
using Renamer.NewClasses.Util;

namespace Renamer.NewClasses.UI.Dialogs.Configuration
{
    /// <summary>
    /// Configuration dialog
    /// </summary>
    public partial class ConfigurationDialog : Form {

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger
	(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

	    private AppConfigurationWrapper theConfiguration;
        /// <summary>
        /// standard constructor
        /// </summary>
        public ConfigurationDialog(AppConfigurationWrapper appConfiguration) {
	        theConfiguration = appConfiguration;
			InitializeComponent();
        }

        /// <summary>
        /// Initialization, sets all values from configuration file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigurationDialog_Load(object sender, EventArgs e) {
	        readConfiguration(theConfiguration);
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
	        theConfiguration.setInvalidCharReplaceWith(txtReplaceInvalidFilenameCharsWith.Text);
			theConfiguration.setDestinationDirectory(txtDefaultDestinationDir.Text);

			theConfiguration.setEmptyFolderIgnoredFiletypes(StringUtils.multilineTextToList(txtEmptyFolderCheckIgnoredFiletypes.Text));
			theConfiguration.setVideoFileExtensions(StringUtils.multilineTextToList(txtVideoFileExtensions.Text, true));
			theConfiguration.setSubtitleFileExtensions(StringUtils.multilineTextToList(txtSubtitleFileExtensions.Text, true));
			theConfiguration.setEpisodeIdentifierPatterns(StringUtils.multilineTextToList(txtEpisodeIdentifierPatterns.Text));
			theConfiguration.setCustomReplaceRegexes(StringUtils.multilineTextToList(txtReplacementsWhenRenaming.Text));
			theConfiguration.setMovieTagsToRemove(StringUtils.multilineTextToList(txtRemovedMovieTags.Text));
			theConfiguration.setSeasonNrExtractionPatterns(StringUtils.multilineTextToList(txtSeasonNrExtractionPatterns.Text));

			theConfiguration.setDiacriticStrategy(cbDiacriticStrategy.Text);
			theConfiguration.setLetterCaseStrategy(cbLetterCaseStrategy.Text);

			theConfiguration.setMaxSearchDepth((int) nudSearchDepth.Value);
			theConfiguration.setConnectionTimeout((int) nudConnectionTimeout.Value);

			theConfiguration.setCreateDirectoryStructure(chkCreateDirectoryStructure.Checked);
			theConfiguration.setDeleteEmptiedFolders(chkDeleteEmptiedFolders.Checked);
			theConfiguration.setDeleteAllEmptyFolders(chkDeleteAllEmptyFolders.Checked);
			theConfiguration.setCreateSeasonSubfolders(chkCreateSeasonSubdir.Checked);
			theConfiguration.setAutoResizeColumns(chkAutoResizeColumns.Checked);
			theConfiguration.setReportMissingEpisodes(chkReportMissingEpisodes.Checked);
			theConfiguration.setDeleteSampleFiles(chkDeleteSampleFiles.Checked);

            List<string> languagePriorities = theConfiguration.getPreferredLanguages();
            languagePriorities = ListUtils.updateStartsWith(languagePriorities, cbPreferredLanguage.Text);
			theConfiguration.setPreferredLanguages(languagePriorities);

			theConfiguration.setLastUsedTvShowProvider(cbDefaultTitleProvider.SelectedItem.ToString());

            DialogResult = DialogResult.OK;
			ConfigurationManager.writeAppConfiguration(theConfiguration);
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
            }
        }

        private void resetToDefaults() {
			readConfiguration(AppDefaults.getDefaultConfiguration());
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

		private void readConfiguration(AppConfigurationWrapper someConfiguration) {
			txtReplaceInvalidFilenameCharsWith.Text = someConfiguration.getInvalidCharReplaceWith();
			txtDefaultDestinationDir.Text = someConfiguration.getDestinationDirectory();

			txtEmptyFolderCheckIgnoredFiletypes.Text = StringUtils.buildMultilineText(someConfiguration.getEmptyFolderIgnoredFiletypes());
			txtVideoFileExtensions.Text = StringUtils.buildMultilineText(someConfiguration.getVideoFileExtensions());
			txtSubtitleFileExtensions.Text = StringUtils.buildMultilineText(someConfiguration.getSubtitleFileExtensions(), true);
			txtEpisodeIdentifierPatterns.Text = StringUtils.buildMultilineText(someConfiguration.getEpisodeIdentifierPatterns());
			txtReplacementsWhenRenaming.Text = StringUtils.buildMultilineText(someConfiguration.getCustomReplaceRegexes());
			txtRemovedMovieTags.Text = StringUtils.buildMultilineText(someConfiguration.getMovieTagsToRemove());
			txtSeasonNrExtractionPatterns.Text = StringUtils.buildMultilineText(someConfiguration.getSeasonNrExtractionPatterns());

			cbDiacriticStrategy.SelectedIndex = (int)someConfiguration.getDiacriticStrategy() - 1;
			cbLetterCaseStrategy.SelectedIndex = (int)someConfiguration.getLetterCaseStrategy() - 1;

			// FIXME: wohl eher int
			nudSearchDepth.Text = someConfiguration.getMaxSearchDepth().ToString(CultureInfo.InvariantCulture);
			nudConnectionTimeout.Value = (decimal)someConfiguration.getConnectionTimeout();

			chkCreateDirectoryStructure.Checked = someConfiguration.getCreateDirectoryStructure();
			chkDeleteEmptiedFolders.Checked = someConfiguration.getDeleteEmptiedFolders();
			chkDeleteAllEmptyFolders.Checked = someConfiguration.getDeleteAllEmptyFolders();
			chkCreateSeasonSubdir.Checked = someConfiguration.getCreateSeasonSubfolders();
			chkAutoResizeColumns.Checked = someConfiguration.getAutoResizeColumns();
			chkReportMissingEpisodes.Checked = someConfiguration.getReportMissingEpisodes();
			chkDeleteSampleFiles.Checked = someConfiguration.getDeleteSampleFiles();


			List<string> prioritizedLanguages = someConfiguration.getPreferredLanguages();
			foreach (string prioritizedLanguage in prioritizedLanguages) {
				string englishLanguageName =
					prioritizedLanguage.Substring(0, prioritizedLanguage.IndexOf("|", StringComparison.Ordinal));
				cbPreferredLanguage.Items.Add(englishLanguageName);
			}
			if (prioritizedLanguages.Count > 0) {
				cbPreferredLanguage.SelectedIndex = 0;
			}

			string lastProvider = someConfiguration.getLastUsedTvShowProvider() ?? "";
			cbDefaultTitleProvider.Items.AddRange(TvShowInfoProviderManager.getTvShowInfoProviderNames().ToArray());
			cbDefaultTitleProvider.SelectedIndex = Math.Max(0, cbDefaultTitleProvider.Items.IndexOf(lastProvider));		
		}

		private void lblDefaultDestinationDir_Click(object sender, EventArgs e) {

		}

		private void chkDeleteSampleFiles_CheckedChanged(object sender, EventArgs e) {

		}
    }

}
