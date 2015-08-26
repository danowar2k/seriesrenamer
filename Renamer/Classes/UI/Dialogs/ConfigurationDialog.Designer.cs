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

namespace Renamer.Classes.UI.Dialogs
{
    partial class ConfigurationDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationDialog));
            this.lblConnectionTimeout = new System.Windows.Forms.Label();
            this.lblReplaceInvalidFilenameCharsWith = new System.Windows.Forms.Label();
            this.lblSearchDepth = new System.Windows.Forms.Label();
            this.lblVideoFileExtensions = new System.Windows.Forms.Label();
            this.lblEpisodeIdentifierPatterns = new System.Windows.Forms.Label();
            this.nudConnectionTimeout = new System.Windows.Forms.NumericUpDown();
            this.txtReplaceInvalidFilenameCharsWith = new System.Windows.Forms.TextBox();
            this.nudSearchDepth = new System.Windows.Forms.NumericUpDown();
            this.txtVideoFileExtensions = new System.Windows.Forms.TextBox();
            this.txtEpisodeIdentifierPatterns = new System.Windows.Forms.TextBox();
            this.btnCancelAndClose = new System.Windows.Forms.Button();
            this.btnSaveAndClose = new System.Windows.Forms.Button();
            this.lblMinLevelLogToFile = new System.Windows.Forms.Label();
            this.lblMinLevelLogToWindow = new System.Windows.Forms.Label();
            this.lblMinLevelLogToMessageBox = new System.Windows.Forms.Label();
            this.cbMinLevelLogToFile = new System.Windows.Forms.ComboBox();
            this.cbMinLevelLogToWindow = new System.Windows.Forms.ComboBox();
            this.cbMinLevelLogToMessageBox = new System.Windows.Forms.ComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.txtSubtitleFileExtensions = new System.Windows.Forms.TextBox();
            this.lblSubtitleFileExtensions = new System.Windows.Forms.Label();
            this.cbLetterCaseStrategy = new System.Windows.Forms.ComboBox();
            this.lblLetterCaseStrategy = new System.Windows.Forms.Label();
            this.cbDiacriticStrategy = new System.Windows.Forms.ComboBox();
            this.lblDiacriticStrategy = new System.Windows.Forms.Label();
            this.lblSeasonNrExtractionPatterns = new System.Windows.Forms.Label();
            this.txtSeasonNrExtractionPatterns = new System.Windows.Forms.TextBox();
            this.chkCreateDirectoryStructure = new System.Windows.Forms.CheckBox();
            this.lblCreateDirectoryStructure = new System.Windows.Forms.Label();
            this.generalTab = new System.Windows.Forms.TabPage();
            this.lblPreferredLanguage = new System.Windows.Forms.Label();
            this.cbPreferredLanguage = new System.Windows.Forms.ComboBox();
            this.cbDefaultTitleProvider = new System.Windows.Forms.ComboBox();
            this.lblDefaultTitleProvider = new System.Windows.Forms.Label();
            this.renamingTab = new System.Windows.Forms.TabPage();
            this.txtRemovedMovieTags = new System.Windows.Forms.TextBox();
            this.lblRemovedMovieTags = new System.Windows.Forms.Label();
            this.lblReplacementsWhenRenaming = new System.Windows.Forms.Label();
            this.txtReplacementsWhenRenaming = new System.Windows.Forms.TextBox();
            this.loggingTab = new System.Windows.Forms.TabPage();
            this.grpLogLevels = new System.Windows.Forms.GroupBox();
            this.chkAutoResizeColumns = new System.Windows.Forms.CheckBox();
            this.lblAutoResizeColumns = new System.Windows.Forms.Label();
            this.folderStructureTab = new System.Windows.Forms.TabPage();
            this.btnChangeDefaultDestinationDir = new System.Windows.Forms.Button();
            this.lblDefaultDestinationDir = new System.Windows.Forms.Label();
            this.txtDefaultDestinationDir = new System.Windows.Forms.TextBox();
            this.lblDeleteSampleFiles = new System.Windows.Forms.Label();
            this.chkDeleteSampleFiles = new System.Windows.Forms.CheckBox();
            this.chkReportMissingEpisodes = new System.Windows.Forms.CheckBox();
            this.lblReportMissingEpisodes = new System.Windows.Forms.Label();
            this.chkCreateSeasonSubdir = new System.Windows.Forms.CheckBox();
            this.lblCreateSeasonSubdir = new System.Windows.Forms.Label();
            this.lblDeleteAllEmptyFolders = new System.Windows.Forms.Label();
            this.chkDeleteAllEmptyFolders = new System.Windows.Forms.CheckBox();
            this.lblEmptyFolderCheckIgnoredFiletypes = new System.Windows.Forms.Label();
            this.txtEmptyFolderCheckIgnoredFiletypes = new System.Windows.Forms.TextBox();
            this.lblDeleteEmptiedFolders = new System.Windows.Forms.Label();
            this.chkDeleteEmptiedFolders = new System.Windows.Forms.CheckBox();
            this.configurationTabs = new System.Windows.Forms.TabControl();
            this.btnResetToDefaults = new System.Windows.Forms.Button();
            this.changeDestionationDirDialog = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.nudConnectionTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSearchDepth)).BeginInit();
            this.generalTab.SuspendLayout();
            this.renamingTab.SuspendLayout();
            this.loggingTab.SuspendLayout();
            this.grpLogLevels.SuspendLayout();
            this.folderStructureTab.SuspendLayout();
            this.configurationTabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblConnectionTimeout
            // 
            this.lblConnectionTimeout.AutoSize = true;
            this.lblConnectionTimeout.Location = new System.Drawing.Point(12, 13);
            this.lblConnectionTimeout.Name = "lblConnectionTimeout";
            this.lblConnectionTimeout.Size = new System.Drawing.Size(67, 13);
            this.lblConnectionTimeout.TabIndex = 0;
            this.lblConnectionTimeout.Text = "Timeout (ms)";
            this.toolTip.SetToolTip(this.lblConnectionTimeout, "Timeout setting for all web related connections");
            // 
            // lblReplaceInvalidFilenameCharsWith
            // 
            this.lblReplaceInvalidFilenameCharsWith.AutoSize = true;
            this.lblReplaceInvalidFilenameCharsWith.Location = new System.Drawing.Point(15, 41);
            this.lblReplaceInvalidFilenameCharsWith.Name = "lblReplaceInvalidFilenameCharsWith";
            this.lblReplaceInvalidFilenameCharsWith.Size = new System.Drawing.Size(234, 13);
            this.lblReplaceInvalidFilenameCharsWith.TabIndex = 2;
            this.lblReplaceInvalidFilenameCharsWith.Text = "String to replace invalid filename characters with";
            // 
            // lblSearchDepth
            // 
            this.lblSearchDepth.AutoSize = true;
            this.lblSearchDepth.Location = new System.Drawing.Point(12, 40);
            this.lblSearchDepth.Name = "lblSearchDepth";
            this.lblSearchDepth.Size = new System.Drawing.Size(131, 13);
            this.lblSearchDepth.TabIndex = 3;
            this.lblSearchDepth.Text = "Subdirectory search depth";
            this.toolTip.SetToolTip(this.lblSearchDepth, "Search depth for scanning subdirectories for files");
            // 
            // lblVideoFileExtensions
            // 
            this.lblVideoFileExtensions.AutoSize = true;
            this.lblVideoFileExtensions.Location = new System.Drawing.Point(12, 123);
            this.lblVideoFileExtensions.Name = "lblVideoFileExtensions";
            this.lblVideoFileExtensions.Size = new System.Drawing.Size(103, 13);
            this.lblVideoFileExtensions.TabIndex = 4;
            this.lblVideoFileExtensions.Text = "Video file extensions";
            this.toolTip.SetToolTip(this.lblVideoFileExtensions, "File extensions to include in renaming process");
            // 
            // lblEpisodeIdentifierPatterns
            // 
            this.lblEpisodeIdentifierPatterns.AutoSize = true;
            this.lblEpisodeIdentifierPatterns.Location = new System.Drawing.Point(350, 123);
            this.lblEpisodeIdentifierPatterns.Name = "lblEpisodeIdentifierPatterns";
            this.lblEpisodeIdentifierPatterns.Size = new System.Drawing.Size(147, 13);
            this.lblEpisodeIdentifierPatterns.TabIndex = 5;
            this.lblEpisodeIdentifierPatterns.Text = "Filename identification pattern";
            this.toolTip.SetToolTip(this.lblEpisodeIdentifierPatterns, resources.GetString("lblEpisodeIdentifierPatterns.ToolTip"));
            // 
            // nudConnectionTimeout
            // 
            this.nudConnectionTimeout.AccessibleDescription = "";
            this.nudConnectionTimeout.AccessibleName = "";
            this.nudConnectionTimeout.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudConnectionTimeout.Location = new System.Drawing.Point(457, 11);
            this.nudConnectionTimeout.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudConnectionTimeout.Name = "nudConnectionTimeout";
            this.nudConnectionTimeout.Size = new System.Drawing.Size(97, 20);
            this.nudConnectionTimeout.TabIndex = 6;
            this.toolTip.SetToolTip(this.nudConnectionTimeout, "Timeout setting for all web related connections");
            // 
            // txtReplaceInvalidFilenameCharsWith
            // 
            this.txtReplaceInvalidFilenameCharsWith.Location = new System.Drawing.Point(368, 38);
            this.txtReplaceInvalidFilenameCharsWith.Name = "txtReplaceInvalidFilenameCharsWith";
            this.txtReplaceInvalidFilenameCharsWith.Size = new System.Drawing.Size(186, 20);
            this.txtReplaceInvalidFilenameCharsWith.TabIndex = 8;
            this.toolTip.SetToolTip(this.txtReplaceInvalidFilenameCharsWith, "String to replace invalid filename characters with");
            // 
            // nudSearchDepth
            // 
            this.nudSearchDepth.Location = new System.Drawing.Point(457, 38);
            this.nudSearchDepth.Name = "nudSearchDepth";
            this.nudSearchDepth.Size = new System.Drawing.Size(97, 20);
            this.nudSearchDepth.TabIndex = 9;
            this.toolTip.SetToolTip(this.nudSearchDepth, "Search depth for scanning subdirectories for files");
            // 
            // txtVideoFileExtensions
            // 
            this.txtVideoFileExtensions.Location = new System.Drawing.Point(15, 139);
            this.txtVideoFileExtensions.Multiline = true;
            this.txtVideoFileExtensions.Name = "txtVideoFileExtensions";
            this.txtVideoFileExtensions.Size = new System.Drawing.Size(163, 108);
            this.txtVideoFileExtensions.TabIndex = 10;
            this.toolTip.SetToolTip(this.txtVideoFileExtensions, "File extensions to include in renaming process");
            // 
            // txtEpisodeIdentifierPatterns
            // 
            this.txtEpisodeIdentifierPatterns.Location = new System.Drawing.Point(353, 139);
            this.txtEpisodeIdentifierPatterns.Multiline = true;
            this.txtEpisodeIdentifierPatterns.Name = "txtEpisodeIdentifierPatterns";
            this.txtEpisodeIdentifierPatterns.Size = new System.Drawing.Size(201, 108);
            this.txtEpisodeIdentifierPatterns.TabIndex = 11;
            this.toolTip.SetToolTip(this.txtEpisodeIdentifierPatterns, resources.GetString("txtEpisodeIdentifierPatterns.ToolTip"));
            // 
            // btnCancelAndClose
            // 
            this.btnCancelAndClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelAndClose.Location = new System.Drawing.Point(508, 299);
            this.btnCancelAndClose.Name = "btnCancelAndClose";
            this.btnCancelAndClose.Size = new System.Drawing.Size(75, 23);
            this.btnCancelAndClose.TabIndex = 12;
            this.btnCancelAndClose.Text = "Cancel";
            this.btnCancelAndClose.UseVisualStyleBackColor = true;
            this.btnCancelAndClose.Click += new System.EventHandler(this.btnCancelAndClose_Click);
            // 
            // btnSaveAndClose
            // 
            this.btnSaveAndClose.Location = new System.Drawing.Point(420, 299);
            this.btnSaveAndClose.Name = "btnSaveAndClose";
            this.btnSaveAndClose.Size = new System.Drawing.Size(82, 23);
            this.btnSaveAndClose.TabIndex = 13;
            this.btnSaveAndClose.Text = "OK";
            this.btnSaveAndClose.UseVisualStyleBackColor = true;
            this.btnSaveAndClose.Click += new System.EventHandler(this.btnSaveAndClose_Click);
            // 
            // lblMinLevelLogToFile
            // 
            this.lblMinLevelLogToFile.AutoSize = true;
            this.lblMinLevelLogToFile.Location = new System.Drawing.Point(12, 16);
            this.lblMinLevelLogToFile.Name = "lblMinLevelLogToFile";
            this.lblMinLevelLogToFile.Size = new System.Drawing.Size(121, 13);
            this.lblMinLevelLogToFile.TabIndex = 14;
            this.lblMinLevelLogToFile.Text = "Minimum level for log file";
            this.toolTip.SetToolTip(this.lblMinLevelLogToFile, "How error messages are processed");
            // 
            // lblMinLevelLogToWindow
            // 
            this.lblMinLevelLogToWindow.AutoSize = true;
            this.lblMinLevelLogToWindow.Location = new System.Drawing.Point(12, 43);
            this.lblMinLevelLogToWindow.Name = "lblMinLevelLogToWindow";
            this.lblMinLevelLogToWindow.Size = new System.Drawing.Size(144, 13);
            this.lblMinLevelLogToWindow.TabIndex = 15;
            this.lblMinLevelLogToWindow.Text = "Minimum level for log window";
            this.toolTip.SetToolTip(this.lblMinLevelLogToWindow, "How warning messages are processed");
            // 
            // lblMinLevelLogToMessageBox
            // 
            this.lblMinLevelLogToMessageBox.AutoSize = true;
            this.lblMinLevelLogToMessageBox.Location = new System.Drawing.Point(12, 70);
            this.lblMinLevelLogToMessageBox.Name = "lblMinLevelLogToMessageBox";
            this.lblMinLevelLogToMessageBox.Size = new System.Drawing.Size(164, 13);
            this.lblMinLevelLogToMessageBox.TabIndex = 16;
            this.lblMinLevelLogToMessageBox.Text = "Minimum level for message boxes";
            this.toolTip.SetToolTip(this.lblMinLevelLogToMessageBox, "How status messages are processed");
            // 
            // cbMinLevelLogToFile
            // 
            this.cbMinLevelLogToFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMinLevelLogToFile.FormattingEnabled = true;
            this.cbMinLevelLogToFile.Items.AddRange(new object[] {
            "VERBOSE",
            "DEBUG",
            "LOG",
            "INFO",
            "WARNING",
            "ERROR",
            "CRITICAL",
            "NONE"});
            this.cbMinLevelLogToFile.Location = new System.Drawing.Point(386, 13);
            this.cbMinLevelLogToFile.Name = "cbMinLevelLogToFile";
            this.cbMinLevelLogToFile.Size = new System.Drawing.Size(163, 21);
            this.cbMinLevelLogToFile.TabIndex = 18;
            this.toolTip.SetToolTip(this.cbMinLevelLogToFile, "Logging messages with this and higher level wrotten into the logfile");
            // 
            // cbMinLevelLogToWindow
            // 
            this.cbMinLevelLogToWindow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMinLevelLogToWindow.FormattingEnabled = true;
            this.cbMinLevelLogToWindow.Items.AddRange(new object[] {
            "VERBOSE",
            "DEBUG",
            "LOG",
            "INFO",
            "WARNING",
            "ERROR",
            "CRITICAL",
            "NONE"});
            this.cbMinLevelLogToWindow.Location = new System.Drawing.Point(386, 40);
            this.cbMinLevelLogToWindow.Name = "cbMinLevelLogToWindow";
            this.cbMinLevelLogToWindow.Size = new System.Drawing.Size(163, 21);
            this.cbMinLevelLogToWindow.TabIndex = 19;
            this.toolTip.SetToolTip(this.cbMinLevelLogToWindow, "Logging messages with this and higher level shown in the logfile");
            // 
            // cbMinLevelLogToMessageBox
            // 
            this.cbMinLevelLogToMessageBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMinLevelLogToMessageBox.FormattingEnabled = true;
            this.cbMinLevelLogToMessageBox.Items.AddRange(new object[] {
            "VERBOSE",
            "DEBUG",
            "LOG",
            "INFO",
            "WARNING",
            "ERROR",
            "CRITICAL",
            "NONE"});
            this.cbMinLevelLogToMessageBox.Location = new System.Drawing.Point(386, 67);
            this.cbMinLevelLogToMessageBox.Name = "cbMinLevelLogToMessageBox";
            this.cbMinLevelLogToMessageBox.Size = new System.Drawing.Size(163, 21);
            this.cbMinLevelLogToMessageBox.TabIndex = 20;
            this.toolTip.SetToolTip(this.cbMinLevelLogToMessageBox, "Logging messages with this and higher level will popup a MessageBox");
            // 
            // txtSubtitleFileExtensions
            // 
            this.txtSubtitleFileExtensions.Location = new System.Drawing.Point(184, 139);
            this.txtSubtitleFileExtensions.Multiline = true;
            this.txtSubtitleFileExtensions.Name = "txtSubtitleFileExtensions";
            this.txtSubtitleFileExtensions.Size = new System.Drawing.Size(163, 108);
            this.txtSubtitleFileExtensions.TabIndex = 23;
            this.toolTip.SetToolTip(this.txtSubtitleFileExtensions, "File extensions used for subtitles");
            // 
            // lblSubtitleFileExtensions
            // 
            this.lblSubtitleFileExtensions.AutoSize = true;
            this.lblSubtitleFileExtensions.Location = new System.Drawing.Point(181, 123);
            this.lblSubtitleFileExtensions.Name = "lblSubtitleFileExtensions";
            this.lblSubtitleFileExtensions.Size = new System.Drawing.Size(111, 13);
            this.lblSubtitleFileExtensions.TabIndex = 24;
            this.lblSubtitleFileExtensions.Text = "Subtitle file extensions";
            this.toolTip.SetToolTip(this.lblSubtitleFileExtensions, "File extensions used for subtitles");
            // 
            // cbLetterCaseStrategy
            // 
            this.cbLetterCaseStrategy.FormattingEnabled = true;
            this.cbLetterCaseStrategy.Items.AddRange(new object[] {
            "IgNOrE",
            "small",
            "Large",
            "CAPSLOCK"});
            this.cbLetterCaseStrategy.Location = new System.Drawing.Point(368, 91);
            this.cbLetterCaseStrategy.Name = "cbLetterCaseStrategy";
            this.cbLetterCaseStrategy.Size = new System.Drawing.Size(186, 21);
            this.cbLetterCaseStrategy.TabIndex = 26;
            this.toolTip.SetToolTip(this.cbLetterCaseStrategy, "Select the desired case of the words in the filenames");
            // 
            // lblLetterCaseStrategy
            // 
            this.lblLetterCaseStrategy.AutoSize = true;
            this.lblLetterCaseStrategy.Location = new System.Drawing.Point(15, 93);
            this.lblLetterCaseStrategy.Name = "lblLetterCaseStrategy";
            this.lblLetterCaseStrategy.Size = new System.Drawing.Size(31, 13);
            this.lblLetterCaseStrategy.TabIndex = 25;
            this.lblLetterCaseStrategy.Text = "Case";
            this.toolTip.SetToolTip(this.lblLetterCaseStrategy, "Select the desired case of the words in the filenames");
            // 
            // cbDiacriticStrategy
            // 
            this.cbDiacriticStrategy.FormattingEnabled = true;
            this.cbDiacriticStrategy.Items.AddRange(new object[] {
            "Ignore",
            "Use",
            "Don\'t Use"});
            this.cbDiacriticStrategy.Location = new System.Drawing.Point(368, 64);
            this.cbDiacriticStrategy.Name = "cbDiacriticStrategy";
            this.cbDiacriticStrategy.Size = new System.Drawing.Size(186, 21);
            this.cbDiacriticStrategy.TabIndex = 24;
            this.toolTip.SetToolTip(this.cbDiacriticStrategy, "Select if Umlaute in names should be ignored, enforced or removed");
            // 
            // lblDiacriticStrategy
            // 
            this.lblDiacriticStrategy.AutoSize = true;
            this.lblDiacriticStrategy.Location = new System.Drawing.Point(15, 67);
            this.lblDiacriticStrategy.Name = "lblDiacriticStrategy";
            this.lblDiacriticStrategy.Size = new System.Drawing.Size(343, 13);
            this.lblDiacriticStrategy.TabIndex = 23;
            this.lblDiacriticStrategy.Text = "Filter out weird dots/lines/etc above characters (Umlauts, Accents, etc)";
            this.toolTip.SetToolTip(this.lblDiacriticStrategy, "Select if Umlaute in names should be ignored, enforced or removed");
            // 
            // lblSeasonNrExtractionPatterns
            // 
            this.lblSeasonNrExtractionPatterns.AutoSize = true;
            this.lblSeasonNrExtractionPatterns.Location = new System.Drawing.Point(15, 29);
            this.lblSeasonNrExtractionPatterns.Name = "lblSeasonNrExtractionPatterns";
            this.lblSeasonNrExtractionPatterns.Size = new System.Drawing.Size(347, 13);
            this.lblSeasonNrExtractionPatterns.TabIndex = 27;
            this.lblSeasonNrExtractionPatterns.Text = "--> Season subdirectory name (needs %S and optionally %T placeholder)";
            this.toolTip.SetToolTip(this.lblSeasonNrExtractionPatterns, "Setting this allows the program to get the show name and the seasons from folder " +
        "names\r\n%S - Season\r\n%T - Series Title");
            // 
            // txtSeasonNrExtractionPatterns
            // 
            this.txtSeasonNrExtractionPatterns.Location = new System.Drawing.Point(368, 26);
            this.txtSeasonNrExtractionPatterns.Multiline = true;
            this.txtSeasonNrExtractionPatterns.Name = "txtSeasonNrExtractionPatterns";
            this.txtSeasonNrExtractionPatterns.Size = new System.Drawing.Size(186, 86);
            this.txtSeasonNrExtractionPatterns.TabIndex = 28;
            this.toolTip.SetToolTip(this.txtSeasonNrExtractionPatterns, "Setting this allows the program to get the show name and the seasons from folder " +
        "names\r\n%S - Season\r\n%T - Series Title");
            // 
            // chkCreateDirectoryStructure
            // 
            this.chkCreateDirectoryStructure.AutoSize = true;
            this.chkCreateDirectoryStructure.Location = new System.Drawing.Point(140, 7);
            this.chkCreateDirectoryStructure.Name = "chkCreateDirectoryStructure";
            this.chkCreateDirectoryStructure.Size = new System.Drawing.Size(15, 14);
            this.chkCreateDirectoryStructure.TabIndex = 29;
            this.toolTip.SetToolTip(this.chkCreateDirectoryStructure, "If checked, files will be moved to ShowName\\Season x\\");
            this.chkCreateDirectoryStructure.UseVisualStyleBackColor = true;
            // 
            // lblCreateDirectoryStructure
            // 
            this.lblCreateDirectoryStructure.AutoSize = true;
            this.lblCreateDirectoryStructure.Location = new System.Drawing.Point(15, 7);
            this.lblCreateDirectoryStructure.Name = "lblCreateDirectoryStructure";
            this.lblCreateDirectoryStructure.Size = new System.Drawing.Size(125, 13);
            this.lblCreateDirectoryStructure.TabIndex = 30;
            this.lblCreateDirectoryStructure.Text = "Create directory structure";
            this.toolTip.SetToolTip(this.lblCreateDirectoryStructure, "If checked, files will be moved to ShowName\\Season x\\");
            // 
            // generalTab
            // 
            this.generalTab.Controls.Add(this.lblPreferredLanguage);
            this.generalTab.Controls.Add(this.cbPreferredLanguage);
            this.generalTab.Controls.Add(this.cbDefaultTitleProvider);
            this.generalTab.Controls.Add(this.lblDefaultTitleProvider);
            this.generalTab.Controls.Add(this.lblConnectionTimeout);
            this.generalTab.Controls.Add(this.lblSearchDepth);
            this.generalTab.Controls.Add(this.lblVideoFileExtensions);
            this.generalTab.Controls.Add(this.lblEpisodeIdentifierPatterns);
            this.generalTab.Controls.Add(this.lblSubtitleFileExtensions);
            this.generalTab.Controls.Add(this.txtVideoFileExtensions);
            this.generalTab.Controls.Add(this.txtEpisodeIdentifierPatterns);
            this.generalTab.Controls.Add(this.txtSubtitleFileExtensions);
            this.generalTab.Controls.Add(this.nudSearchDepth);
            this.generalTab.Controls.Add(this.nudConnectionTimeout);
            this.generalTab.Location = new System.Drawing.Point(4, 22);
            this.generalTab.Name = "generalTab";
            this.generalTab.Padding = new System.Windows.Forms.Padding(3);
            this.generalTab.Size = new System.Drawing.Size(560, 253);
            this.generalTab.TabIndex = 0;
            this.generalTab.Text = "General";
            this.toolTip.SetToolTip(this.generalTab, "General settings which don\'t fit anywhere else");
            this.generalTab.UseVisualStyleBackColor = true;
            // 
            // lblPreferredLanguage
            // 
            this.lblPreferredLanguage.AutoSize = true;
            this.lblPreferredLanguage.Location = new System.Drawing.Point(12, 94);
            this.lblPreferredLanguage.Name = "lblPreferredLanguage";
            this.lblPreferredLanguage.Size = new System.Drawing.Size(116, 13);
            this.lblPreferredLanguage.TabIndex = 36;
            this.lblPreferredLanguage.Text = "Preferred title language";
            // 
            // cbPreferredLanguage
            // 
            this.cbPreferredLanguage.FormattingEnabled = true;
            this.cbPreferredLanguage.Location = new System.Drawing.Point(368, 91);
            this.cbPreferredLanguage.Name = "cbPreferredLanguage";
            this.cbPreferredLanguage.Size = new System.Drawing.Size(186, 21);
            this.cbPreferredLanguage.TabIndex = 35;
            // 
            // cbDefaultTitleProvider
            // 
            this.cbDefaultTitleProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefaultTitleProvider.FormattingEnabled = true;
            this.cbDefaultTitleProvider.Location = new System.Drawing.Point(368, 64);
            this.cbDefaultTitleProvider.Name = "cbDefaultTitleProvider";
            this.cbDefaultTitleProvider.Size = new System.Drawing.Size(186, 21);
            this.cbDefaultTitleProvider.TabIndex = 34;
            // 
            // lblDefaultTitleProvider
            // 
            this.lblDefaultTitleProvider.AutoSize = true;
            this.lblDefaultTitleProvider.Location = new System.Drawing.Point(12, 67);
            this.lblDefaultTitleProvider.Name = "lblDefaultTitleProvider";
            this.lblDefaultTitleProvider.Size = new System.Drawing.Size(130, 13);
            this.lblDefaultTitleProvider.TabIndex = 33;
            this.lblDefaultTitleProvider.Text = "Default title search source";
            // 
            // renamingTab
            // 
            this.renamingTab.Controls.Add(this.txtRemovedMovieTags);
            this.renamingTab.Controls.Add(this.lblRemovedMovieTags);
            this.renamingTab.Controls.Add(this.lblReplacementsWhenRenaming);
            this.renamingTab.Controls.Add(this.txtReplacementsWhenRenaming);
            this.renamingTab.Controls.Add(this.cbLetterCaseStrategy);
            this.renamingTab.Controls.Add(this.lblLetterCaseStrategy);
            this.renamingTab.Controls.Add(this.cbDiacriticStrategy);
            this.renamingTab.Controls.Add(this.lblDiacriticStrategy);
            this.renamingTab.Controls.Add(this.lblReplaceInvalidFilenameCharsWith);
            this.renamingTab.Controls.Add(this.txtReplaceInvalidFilenameCharsWith);
            this.renamingTab.Location = new System.Drawing.Point(4, 22);
            this.renamingTab.Name = "renamingTab";
            this.renamingTab.Padding = new System.Windows.Forms.Padding(3);
            this.renamingTab.Size = new System.Drawing.Size(560, 253);
            this.renamingTab.TabIndex = 1;
            this.renamingTab.Text = "Renaming";
            this.toolTip.SetToolTip(this.renamingTab, "Settings related to the renaming itsself");
            this.renamingTab.UseVisualStyleBackColor = true;
            // 
            // txtRemovedMovieTags
            // 
            this.txtRemovedMovieTags.Location = new System.Drawing.Point(391, 133);
            this.txtRemovedMovieTags.Multiline = true;
            this.txtRemovedMovieTags.Name = "txtRemovedMovieTags";
            this.txtRemovedMovieTags.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemovedMovieTags.Size = new System.Drawing.Size(163, 114);
            this.txtRemovedMovieTags.TabIndex = 30;
            // 
            // lblRemovedMovieTags
            // 
            this.lblRemovedMovieTags.AutoSize = true;
            this.lblRemovedMovieTags.Location = new System.Drawing.Point(388, 117);
            this.lblRemovedMovieTags.Name = "lblRemovedMovieTags";
            this.lblRemovedMovieTags.Size = new System.Drawing.Size(141, 13);
            this.lblRemovedMovieTags.TabIndex = 29;
            this.lblRemovedMovieTags.Text = "Tags to remove from Movies";
            // 
            // lblReplacementsWhenRenaming
            // 
            this.lblReplacementsWhenRenaming.AutoSize = true;
            this.lblReplacementsWhenRenaming.Location = new System.Drawing.Point(15, 117);
            this.lblReplacementsWhenRenaming.Name = "lblReplacementsWhenRenaming";
            this.lblReplacementsWhenRenaming.Size = new System.Drawing.Size(105, 13);
            this.lblReplacementsWhenRenaming.TabIndex = 28;
            this.lblReplacementsWhenRenaming.Text = "Replace in filenames";
            // 
            // txtReplacementsWhenRenaming
            // 
            this.txtReplacementsWhenRenaming.Location = new System.Drawing.Point(18, 133);
            this.txtReplacementsWhenRenaming.Multiline = true;
            this.txtReplacementsWhenRenaming.Name = "txtReplacementsWhenRenaming";
            this.txtReplacementsWhenRenaming.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReplacementsWhenRenaming.Size = new System.Drawing.Size(367, 114);
            this.txtReplacementsWhenRenaming.TabIndex = 27;
            // 
            // loggingTab
            // 
            this.loggingTab.Controls.Add(this.grpLogLevels);
            this.loggingTab.Controls.Add(this.chkAutoResizeColumns);
            this.loggingTab.Controls.Add(this.lblAutoResizeColumns);
            this.loggingTab.Location = new System.Drawing.Point(4, 22);
            this.loggingTab.Name = "loggingTab";
            this.loggingTab.Size = new System.Drawing.Size(560, 253);
            this.loggingTab.TabIndex = 2;
            this.loggingTab.Text = "Logging/GUI";
            this.toolTip.SetToolTip(this.loggingTab, "Settings related to the logging functions");
            this.loggingTab.UseVisualStyleBackColor = true;
            // 
            // grpLogLevels
            // 
            this.grpLogLevels.Controls.Add(this.lblMinLevelLogToFile);
            this.grpLogLevels.Controls.Add(this.lblMinLevelLogToWindow);
            this.grpLogLevels.Controls.Add(this.lblMinLevelLogToMessageBox);
            this.grpLogLevels.Controls.Add(this.cbMinLevelLogToFile);
            this.grpLogLevels.Controls.Add(this.cbMinLevelLogToMessageBox);
            this.grpLogLevels.Controls.Add(this.cbMinLevelLogToWindow);
            this.grpLogLevels.Location = new System.Drawing.Point(3, 3);
            this.grpLogLevels.Name = "grpLogLevels";
            this.grpLogLevels.Size = new System.Drawing.Size(555, 104);
            this.grpLogLevels.TabIndex = 26;
            this.grpLogLevels.TabStop = false;
            this.grpLogLevels.Text = "Log levels";
            // 
            // chkAutoResizeColumns
            // 
            this.chkAutoResizeColumns.AutoSize = true;
            this.chkAutoResizeColumns.Location = new System.Drawing.Point(542, 149);
            this.chkAutoResizeColumns.Name = "chkAutoResizeColumns";
            this.chkAutoResizeColumns.Size = new System.Drawing.Size(15, 14);
            this.chkAutoResizeColumns.TabIndex = 25;
            this.chkAutoResizeColumns.UseVisualStyleBackColor = true;
            // 
            // lblAutoResizeColumns
            // 
            this.lblAutoResizeColumns.AutoSize = true;
            this.lblAutoResizeColumns.Location = new System.Drawing.Point(12, 149);
            this.lblAutoResizeColumns.Name = "lblAutoResizeColumns";
            this.lblAutoResizeColumns.Size = new System.Drawing.Size(224, 13);
            this.lblAutoResizeColumns.TabIndex = 24;
            this.lblAutoResizeColumns.Text = "Automatically resize columns to fit window size";
            // 
            // folderStructureTab
            // 
            this.folderStructureTab.Controls.Add(this.btnChangeDefaultDestinationDir);
            this.folderStructureTab.Controls.Add(this.lblDefaultDestinationDir);
            this.folderStructureTab.Controls.Add(this.txtDefaultDestinationDir);
            this.folderStructureTab.Controls.Add(this.lblDeleteSampleFiles);
            this.folderStructureTab.Controls.Add(this.chkDeleteSampleFiles);
            this.folderStructureTab.Controls.Add(this.chkReportMissingEpisodes);
            this.folderStructureTab.Controls.Add(this.lblReportMissingEpisodes);
            this.folderStructureTab.Controls.Add(this.chkCreateSeasonSubdir);
            this.folderStructureTab.Controls.Add(this.lblCreateSeasonSubdir);
            this.folderStructureTab.Controls.Add(this.lblDeleteAllEmptyFolders);
            this.folderStructureTab.Controls.Add(this.chkDeleteAllEmptyFolders);
            this.folderStructureTab.Controls.Add(this.lblEmptyFolderCheckIgnoredFiletypes);
            this.folderStructureTab.Controls.Add(this.txtEmptyFolderCheckIgnoredFiletypes);
            this.folderStructureTab.Controls.Add(this.lblDeleteEmptiedFolders);
            this.folderStructureTab.Controls.Add(this.chkDeleteEmptiedFolders);
            this.folderStructureTab.Controls.Add(this.lblCreateDirectoryStructure);
            this.folderStructureTab.Controls.Add(this.chkCreateDirectoryStructure);
            this.folderStructureTab.Controls.Add(this.txtSeasonNrExtractionPatterns);
            this.folderStructureTab.Controls.Add(this.lblSeasonNrExtractionPatterns);
            this.folderStructureTab.Location = new System.Drawing.Point(4, 22);
            this.folderStructureTab.Name = "folderStructureTab";
            this.folderStructureTab.Padding = new System.Windows.Forms.Padding(3);
            this.folderStructureTab.Size = new System.Drawing.Size(560, 253);
            this.folderStructureTab.TabIndex = 3;
            this.folderStructureTab.Text = "Folder Structure";
            this.toolTip.SetToolTip(this.folderStructureTab, "Settings related to moving the files, deleting empty folders, ...");
            this.folderStructureTab.UseVisualStyleBackColor = true;
            // 
            // btnChangeDefaultDestinationDir
            // 
            this.btnChangeDefaultDestinationDir.Location = new System.Drawing.Point(528, 116);
            this.btnChangeDefaultDestinationDir.Name = "btnChangeDefaultDestinationDir";
            this.btnChangeDefaultDestinationDir.Size = new System.Drawing.Size(26, 23);
            this.btnChangeDefaultDestinationDir.TabIndex = 45;
            this.btnChangeDefaultDestinationDir.Text = "...";
            this.btnChangeDefaultDestinationDir.UseVisualStyleBackColor = true;
            this.btnChangeDefaultDestinationDir.Click += new System.EventHandler(this.btnChangeDefaultDestinationDir_Click);
            // 
            // lblDefaultDestinationDir
            // 
            this.lblDefaultDestinationDir.AutoSize = true;
            this.lblDefaultDestinationDir.Location = new System.Drawing.Point(15, 121);
            this.lblDefaultDestinationDir.Name = "lblDefaultDestinationDir";
            this.lblDefaultDestinationDir.Size = new System.Drawing.Size(142, 13);
            this.lblDefaultDestinationDir.TabIndex = 44;
            this.lblDefaultDestinationDir.Text = "Default Destination Directory";
            // 
            // txtDefaultDestinationDir
            // 
            this.txtDefaultDestinationDir.Location = new System.Drawing.Point(368, 118);
            this.txtDefaultDestinationDir.Name = "txtDefaultDestinationDir";
            this.txtDefaultDestinationDir.Size = new System.Drawing.Size(154, 20);
            this.txtDefaultDestinationDir.TabIndex = 43;
            // 
            // lblDeleteSampleFiles
            // 
            this.lblDeleteSampleFiles.AutoSize = true;
            this.lblDeleteSampleFiles.Location = new System.Drawing.Point(15, 164);
            this.lblDeleteSampleFiles.Name = "lblDeleteSampleFiles";
            this.lblDeleteSampleFiles.Size = new System.Drawing.Size(100, 13);
            this.lblDeleteSampleFiles.TabIndex = 42;
            this.lblDeleteSampleFiles.Text = "Delete Sample Files";
            this.toolTip.SetToolTip(this.lblDeleteSampleFiles, "If set, files recognized as samples will be deleted in the renaming process");
            // 
            // chkDeleteSampleFiles
            // 
            this.chkDeleteSampleFiles.AutoSize = true;
            this.chkDeleteSampleFiles.Location = new System.Drawing.Point(539, 164);
            this.chkDeleteSampleFiles.Name = "chkDeleteSampleFiles";
            this.chkDeleteSampleFiles.Size = new System.Drawing.Size(15, 14);
            this.chkDeleteSampleFiles.TabIndex = 41;
            this.chkDeleteSampleFiles.UseVisualStyleBackColor = true;
            // 
            // chkReportMissingEpisodes
            // 
            this.chkReportMissingEpisodes.AutoSize = true;
            this.chkReportMissingEpisodes.Location = new System.Drawing.Point(539, 144);
            this.chkReportMissingEpisodes.Name = "chkReportMissingEpisodes";
            this.chkReportMissingEpisodes.Size = new System.Drawing.Size(15, 14);
            this.chkReportMissingEpisodes.TabIndex = 40;
            this.chkReportMissingEpisodes.UseVisualStyleBackColor = true;
            // 
            // lblReportMissingEpisodes
            // 
            this.lblReportMissingEpisodes.AutoSize = true;
            this.lblReportMissingEpisodes.Location = new System.Drawing.Point(15, 144);
            this.lblReportMissingEpisodes.Name = "lblReportMissingEpisodes";
            this.lblReportMissingEpisodes.Size = new System.Drawing.Size(109, 13);
            this.lblReportMissingEpisodes.TabIndex = 39;
            this.lblReportMissingEpisodes.Text = "Find missing episodes";
            this.toolTip.SetToolTip(this.lblReportMissingEpisodes, "If set, Log messages which indicate missing episodes will be produced");
            // 
            // chkCreateSeasonSubdir
            // 
            this.chkCreateSeasonSubdir.AutoSize = true;
            this.chkCreateSeasonSubdir.Location = new System.Drawing.Point(307, 7);
            this.chkCreateSeasonSubdir.Name = "chkCreateSeasonSubdir";
            this.chkCreateSeasonSubdir.Size = new System.Drawing.Size(15, 14);
            this.chkCreateSeasonSubdir.TabIndex = 38;
            this.chkCreateSeasonSubdir.UseVisualStyleBackColor = true;
            // 
            // lblCreateSeasonSubdir
            // 
            this.lblCreateSeasonSubdir.AutoSize = true;
            this.lblCreateSeasonSubdir.Location = new System.Drawing.Point(161, 7);
            this.lblCreateSeasonSubdir.Name = "lblCreateSeasonSubdir";
            this.lblCreateSeasonSubdir.Size = new System.Drawing.Size(140, 13);
            this.lblCreateSeasonSubdir.TabIndex = 37;
            this.lblCreateSeasonSubdir.Text = "--> Use Season subdirectory";
            this.toolTip.SetToolTip(this.lblCreateSeasonSubdir, "If checked, files are put into showname\\season x\\ directories, if unchecked, only" +
        " showname\\ directory is set");
            // 
            // lblDeleteAllEmptyFolders
            // 
            this.lblDeleteAllEmptyFolders.AutoSize = true;
            this.lblDeleteAllEmptyFolders.Location = new System.Drawing.Point(15, 207);
            this.lblDeleteAllEmptyFolders.Name = "lblDeleteAllEmptyFolders";
            this.lblDeleteAllEmptyFolders.Size = new System.Drawing.Size(131, 13);
            this.lblDeleteAllEmptyFolders.TabIndex = 36;
            this.lblDeleteAllEmptyFolders.Text = "--> Delete all empty folders";
            this.toolTip.SetToolTip(this.lblDeleteAllEmptyFolders, "All empty subfolders will be deleted");
            // 
            // chkDeleteAllEmptyFolders
            // 
            this.chkDeleteAllEmptyFolders.AutoSize = true;
            this.chkDeleteAllEmptyFolders.Location = new System.Drawing.Point(539, 207);
            this.chkDeleteAllEmptyFolders.Name = "chkDeleteAllEmptyFolders";
            this.chkDeleteAllEmptyFolders.Size = new System.Drawing.Size(15, 14);
            this.chkDeleteAllEmptyFolders.TabIndex = 35;
            this.toolTip.SetToolTip(this.chkDeleteAllEmptyFolders, "All empty subfolders will be deleted");
            this.chkDeleteAllEmptyFolders.UseVisualStyleBackColor = true;
            // 
            // lblEmptyFolderCheckIgnoredFiletypes
            // 
            this.lblEmptyFolderCheckIgnoredFiletypes.AutoSize = true;
            this.lblEmptyFolderCheckIgnoredFiletypes.Location = new System.Drawing.Point(15, 230);
            this.lblEmptyFolderCheckIgnoredFiletypes.Name = "lblEmptyFolderCheckIgnoredFiletypes";
            this.lblEmptyFolderCheckIgnoredFiletypes.Size = new System.Drawing.Size(289, 13);
            this.lblEmptyFolderCheckIgnoredFiletypes.TabIndex = 34;
            this.lblEmptyFolderCheckIgnoredFiletypes.Text = "--> Also delete folders if they contain only filetypes on this list";
            this.toolTip.SetToolTip(this.lblEmptyFolderCheckIgnoredFiletypes, resources.GetString("lblEmptyFolderCheckIgnoredFiletypes.ToolTip"));
            // 
            // txtEmptyFolderCheckIgnoredFiletypes
            // 
            this.txtEmptyFolderCheckIgnoredFiletypes.Location = new System.Drawing.Point(368, 227);
            this.txtEmptyFolderCheckIgnoredFiletypes.Name = "txtEmptyFolderCheckIgnoredFiletypes";
            this.txtEmptyFolderCheckIgnoredFiletypes.Size = new System.Drawing.Size(186, 20);
            this.txtEmptyFolderCheckIgnoredFiletypes.TabIndex = 33;
            this.toolTip.SetToolTip(this.txtEmptyFolderCheckIgnoredFiletypes, resources.GetString("txtEmptyFolderCheckIgnoredFiletypes.ToolTip"));
            // 
            // lblDeleteEmptiedFolders
            // 
            this.lblDeleteEmptiedFolders.AutoSize = true;
            this.lblDeleteEmptiedFolders.Location = new System.Drawing.Point(15, 184);
            this.lblDeleteEmptiedFolders.Name = "lblDeleteEmptiedFolders";
            this.lblDeleteEmptiedFolders.Size = new System.Drawing.Size(215, 13);
            this.lblDeleteEmptiedFolders.TabIndex = 32;
            this.lblDeleteEmptiedFolders.Text = "Delete empty folders emptied by moving files";
            this.toolTip.SetToolTip(this.lblDeleteEmptiedFolders, "If files are moved out of a folder and the folder is then empty, it will be delet" +
        "ed");
            // 
            // chkDeleteEmptiedFolders
            // 
            this.chkDeleteEmptiedFolders.AutoSize = true;
            this.chkDeleteEmptiedFolders.Location = new System.Drawing.Point(539, 184);
            this.chkDeleteEmptiedFolders.Name = "chkDeleteEmptiedFolders";
            this.chkDeleteEmptiedFolders.Size = new System.Drawing.Size(15, 14);
            this.chkDeleteEmptiedFolders.TabIndex = 31;
            this.toolTip.SetToolTip(this.chkDeleteEmptiedFolders, "If files are moved out of a folder and the folder is then empty, it will be delet" +
        "ed");
            this.chkDeleteEmptiedFolders.UseVisualStyleBackColor = true;
            this.chkDeleteEmptiedFolders.CheckedChanged += new System.EventHandler(this.chkDeleteEmptiedFolders_CheckedChanged);
            // 
            // configurationTabs
            // 
            this.configurationTabs.Controls.Add(this.generalTab);
            this.configurationTabs.Controls.Add(this.renamingTab);
            this.configurationTabs.Controls.Add(this.folderStructureTab);
            this.configurationTabs.Controls.Add(this.loggingTab);
            this.configurationTabs.Location = new System.Drawing.Point(15, 14);
            this.configurationTabs.Name = "configurationTabs";
            this.configurationTabs.SelectedIndex = 0;
            this.configurationTabs.Size = new System.Drawing.Size(568, 279);
            this.configurationTabs.TabIndex = 25;
            // 
            // btnResetToDefaults
            // 
            this.btnResetToDefaults.Location = new System.Drawing.Point(15, 299);
            this.btnResetToDefaults.Name = "btnResetToDefaults";
            this.btnResetToDefaults.Size = new System.Drawing.Size(75, 23);
            this.btnResetToDefaults.TabIndex = 26;
            this.btnResetToDefaults.Text = "Defaults";
            this.btnResetToDefaults.UseVisualStyleBackColor = true;
            this.btnResetToDefaults.Click += new System.EventHandler(this.btnResetToDefaults_Click);
            // 
            // ConfigurationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelAndClose;
            this.ClientSize = new System.Drawing.Size(599, 338);
            this.Controls.Add(this.btnResetToDefaults);
            this.Controls.Add(this.btnSaveAndClose);
            this.Controls.Add(this.btnCancelAndClose);
            this.Controls.Add(this.configurationTabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration";
            this.Load += new System.EventHandler(this.ConfigurationDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudConnectionTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSearchDepth)).EndInit();
            this.generalTab.ResumeLayout(false);
            this.generalTab.PerformLayout();
            this.renamingTab.ResumeLayout(false);
            this.renamingTab.PerformLayout();
            this.loggingTab.ResumeLayout(false);
            this.loggingTab.PerformLayout();
            this.grpLogLevels.ResumeLayout(false);
            this.grpLogLevels.PerformLayout();
            this.folderStructureTab.ResumeLayout(false);
            this.folderStructureTab.PerformLayout();
            this.configurationTabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblConnectionTimeout;
        private System.Windows.Forms.Label lblReplaceInvalidFilenameCharsWith;
        private System.Windows.Forms.Label lblSearchDepth;
        private System.Windows.Forms.Label lblVideoFileExtensions;
        private System.Windows.Forms.Label lblEpisodeIdentifierPatterns;
        private System.Windows.Forms.NumericUpDown nudConnectionTimeout;
        private System.Windows.Forms.TextBox txtReplaceInvalidFilenameCharsWith;
        private System.Windows.Forms.NumericUpDown nudSearchDepth;
        private System.Windows.Forms.TextBox txtVideoFileExtensions;
        private System.Windows.Forms.TextBox txtEpisodeIdentifierPatterns;
        private System.Windows.Forms.Button btnCancelAndClose;
        private System.Windows.Forms.Button btnSaveAndClose;
        private System.Windows.Forms.Label lblMinLevelLogToFile;
        private System.Windows.Forms.Label lblMinLevelLogToWindow;
        private System.Windows.Forms.Label lblMinLevelLogToMessageBox;
        private System.Windows.Forms.ComboBox cbMinLevelLogToFile;
        private System.Windows.Forms.ComboBox cbMinLevelLogToWindow;
        private System.Windows.Forms.ComboBox cbMinLevelLogToMessageBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TextBox txtSubtitleFileExtensions;
        private System.Windows.Forms.Label lblSubtitleFileExtensions;
        private System.Windows.Forms.TabControl configurationTabs;
        private System.Windows.Forms.TabPage generalTab;
        private System.Windows.Forms.TabPage renamingTab;
        private System.Windows.Forms.TabPage loggingTab;
        private System.Windows.Forms.ComboBox cbDiacriticStrategy;
        private System.Windows.Forms.Label lblDiacriticStrategy;
        private System.Windows.Forms.ComboBox cbLetterCaseStrategy;
        private System.Windows.Forms.Label lblLetterCaseStrategy;
        private System.Windows.Forms.Button btnResetToDefaults;
        private System.Windows.Forms.TextBox txtSeasonNrExtractionPatterns;
        private System.Windows.Forms.Label lblSeasonNrExtractionPatterns;
        private System.Windows.Forms.CheckBox chkCreateDirectoryStructure;
        private System.Windows.Forms.Label lblCreateDirectoryStructure;
        private System.Windows.Forms.TabPage folderStructureTab;
        private System.Windows.Forms.Label lblDeleteEmptiedFolders;
        private System.Windows.Forms.CheckBox chkDeleteEmptiedFolders;
        private System.Windows.Forms.Label lblEmptyFolderCheckIgnoredFiletypes;
        private System.Windows.Forms.TextBox txtEmptyFolderCheckIgnoredFiletypes;
        private System.Windows.Forms.CheckBox chkDeleteAllEmptyFolders;
        private System.Windows.Forms.Label lblDeleteAllEmptyFolders;
        private System.Windows.Forms.Label lblCreateSeasonSubdir;
        private System.Windows.Forms.CheckBox chkCreateSeasonSubdir;
        private System.Windows.Forms.Label lblReplacementsWhenRenaming;
        private System.Windows.Forms.TextBox txtReplacementsWhenRenaming;
        private System.Windows.Forms.TextBox txtRemovedMovieTags;
        private System.Windows.Forms.Label lblRemovedMovieTags;
        private System.Windows.Forms.CheckBox chkAutoResizeColumns;
        private System.Windows.Forms.Label lblAutoResizeColumns;
        private System.Windows.Forms.GroupBox grpLogLevels;
        private System.Windows.Forms.CheckBox chkReportMissingEpisodes;
        private System.Windows.Forms.Label lblReportMissingEpisodes;
        private System.Windows.Forms.Label lblDeleteSampleFiles;
        private System.Windows.Forms.CheckBox chkDeleteSampleFiles;
        private System.Windows.Forms.TextBox txtDefaultDestinationDir;
        private System.Windows.Forms.Button btnChangeDefaultDestinationDir;
        private System.Windows.Forms.Label lblDefaultDestinationDir;
        private System.Windows.Forms.FolderBrowserDialog changeDestionationDirDialog;
        private System.Windows.Forms.ComboBox cbDefaultTitleProvider;
        private System.Windows.Forms.Label lblDefaultTitleProvider;
        private System.Windows.Forms.Label lblPreferredLanguage;
        private System.Windows.Forms.ComboBox cbPreferredLanguage;
    }
}