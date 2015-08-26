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

namespace Renamer.Classes.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.scMainLogSplit = new System.Windows.Forms.SplitContainer();
			this.lblFileListingProgress = new System.Windows.Forms.Label();
			this.btnCancelBackgroundTask = new System.Windows.Forms.Button();
			this.lstCandidateFiles = new BrightIdeasSoftware.FastObjectListView();
			this.ColumnSource = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.ColumnFilepath = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.ColumnShowname = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.ColumnSeason = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.ColumnEpisode = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.ColumnEpisodeName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.ColumnDestination = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.ColumnNewFilename = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.contextFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editSubtitleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.markAsMovieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.markAsTVSeriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lookUpOnIMDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.renamingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.createDirectoryStructureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.createDirectoryStructureToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.dontCreateDirectoryStructureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.useUmlautsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.useUmlautsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.dontUseUmlautsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.useProvidedNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.caseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.largeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.smallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.igNorEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cAPSLOCKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.replaceInPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.invertSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectByKeywordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectSimilarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.uncheckAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.invertCheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.setShownameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setSeasonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setEpisodesFromtoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setDestinationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.originalNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pathOrigNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.titleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newFileNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.destinationNewFileNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.operationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.regexTesterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutDialogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.taskProgressBar = new System.Windows.Forms.ProgressBar();
			this.btnExploreCurrentFolder = new System.Windows.Forms.Button();
			this.btnRenameSelectedCandidates = new System.Windows.Forms.Button();
			this.txtTargetFilenamePattern = new System.Windows.Forms.TextBox();
			this.cbSubtitleProviders = new System.Windows.Forms.ComboBox();
			this.lblSubFrom = new System.Windows.Forms.Label();
			this.lblTargetFilename = new System.Windows.Forms.Label();
			this.btnSearchForSubtitles = new System.Windows.Forms.Button();
			this.btnAboutApplication = new System.Windows.Forms.Button();
			this.cbProviders = new System.Windows.Forms.ComboBox();
			this.lblTitlesFrom = new System.Windows.Forms.Label();
			this.btnSearchForTitles = new System.Windows.Forms.Button();
			this.btnOpenConfiguration = new System.Windows.Forms.Button();
			this.btnChangeCurrentFolder = new System.Windows.Forms.Button();
			this.txtCurrentFolderPath = new System.Windows.Forms.TextBox();
			this.lblCurrentFolderPath = new System.Windows.Forms.Label();
			this.txtBasicLog = new System.Windows.Forms.TextBox();
			this.rtbEnhancedLog = new System.Windows.Forms.RichTextBox();
			this.fbdChangeCurrentFolder = new System.Windows.Forms.FolderBrowserDialog();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.backgroundTaskWorker = new System.ComponentModel.BackgroundWorker();
			((System.ComponentModel.ISupportInitialize)(this.scMainLogSplit)).BeginInit();
			this.scMainLogSplit.Panel1.SuspendLayout();
			this.scMainLogSplit.Panel2.SuspendLayout();
			this.scMainLogSplit.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.lstCandidateFiles)).BeginInit();
			this.contextFiles.SuspendLayout();
			this.SuspendLayout();
			// 
			// scMainLogSplit
			// 
			this.scMainLogSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.scMainLogSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.scMainLogSplit.Location = new System.Drawing.Point(0, 0);
			this.scMainLogSplit.Name = "scMainAreaContainer";
			this.scMainLogSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// scMainLogSplit.Panel1 (Main Area)
			// 
			this.scMainLogSplit.Panel1.Controls.Add(this.lblFileListingProgress);
			this.scMainLogSplit.Panel1.Controls.Add(this.btnCancelBackgroundTask);
			this.scMainLogSplit.Panel1.Controls.Add(this.lstCandidateFiles);
			this.scMainLogSplit.Panel1.Controls.Add(this.taskProgressBar);
			this.scMainLogSplit.Panel1.Controls.Add(this.btnExploreCurrentFolder);
			this.scMainLogSplit.Panel1.Controls.Add(this.btnRenameSelectedCandidates);
			this.scMainLogSplit.Panel1.Controls.Add(this.txtTargetFilenamePattern);
			this.scMainLogSplit.Panel1.Controls.Add(this.cbSubtitleProviders);
			this.scMainLogSplit.Panel1.Controls.Add(this.lblSubFrom);
			this.scMainLogSplit.Panel1.Controls.Add(this.lblTargetFilename);
			this.scMainLogSplit.Panel1.Controls.Add(this.btnSearchForSubtitles);
			this.scMainLogSplit.Panel1.Controls.Add(this.btnAboutApplication);
			this.scMainLogSplit.Panel1.Controls.Add(this.cbProviders);
			this.scMainLogSplit.Panel1.Controls.Add(this.lblTitlesFrom);
			this.scMainLogSplit.Panel1.Controls.Add(this.btnSearchForTitles);
			this.scMainLogSplit.Panel1.Controls.Add(this.btnOpenConfiguration);
			this.scMainLogSplit.Panel1.Controls.Add(this.btnChangeCurrentFolder);
			this.scMainLogSplit.Panel1.Controls.Add(this.txtCurrentFolderPath);
			this.scMainLogSplit.Panel1.Controls.Add(this.lblCurrentFolderPath);
			this.scMainLogSplit.Panel1MinSize = 300;
			// 
			// scMainLogSplit.Panel2 (Log Area)
			// 
			this.scMainLogSplit.Panel2.Controls.Add(this.txtBasicLog);
			this.scMainLogSplit.Panel2.Controls.Add(this.rtbEnhancedLog);
			this.scMainLogSplit.Panel2MinSize = 100;
			this.scMainLogSplit.Size = new System.Drawing.Size(1016, 571);
			this.scMainLogSplit.SplitterDistance = 463;
			this.scMainLogSplit.TabIndex = 0;
			this.scMainLogSplit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scMainLogSplit_MouseDown);
			this.scMainLogSplit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.scMainLogSplit_MouseUp);
			// 
			// lblFileListingProgress
			// 
			this.lblFileListingProgress.AutoSize = true;
			this.lblFileListingProgress.Location = new System.Drawing.Point(12, 9);
			this.lblFileListingProgress.Name = "lblFileListingProgress";
			this.lblFileListingProgress.Size = new System.Drawing.Size(0, 13);
			this.lblFileListingProgress.TabIndex = 19;
			this.lblFileListingProgress.Visible = false;
			// 
			// btnCancelBackgroundTask
			// 
			this.btnCancelBackgroundTask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancelBackgroundTask.Enabled = false;
			this.btnCancelBackgroundTask.Location = new System.Drawing.Point(447, 4);
			this.btnCancelBackgroundTask.Name = "btnCancelBackgroundTask";
			this.btnCancelBackgroundTask.Size = new System.Drawing.Size(85, 23);
			this.btnCancelBackgroundTask.TabIndex = 18;
			this.btnCancelBackgroundTask.Text = "Cancel";
			this.btnCancelBackgroundTask.UseVisualStyleBackColor = true;
			this.btnCancelBackgroundTask.Visible = false;
			this.btnCancelBackgroundTask.Click += new System.EventHandler(this.btnCancelBackgroundTask_Click);
			// 
			// lstCandidateFiles
			// 
			this.lstCandidateFiles.AllColumns.Add(this.ColumnSource);
			this.lstCandidateFiles.AllColumns.Add(this.ColumnFilepath);
			this.lstCandidateFiles.AllColumns.Add(this.ColumnShowname);
			this.lstCandidateFiles.AllColumns.Add(this.ColumnSeason);
			this.lstCandidateFiles.AllColumns.Add(this.ColumnEpisode);
			this.lstCandidateFiles.AllColumns.Add(this.ColumnEpisodeName);
			this.lstCandidateFiles.AllColumns.Add(this.ColumnDestination);
			this.lstCandidateFiles.AllColumns.Add(this.ColumnNewFilename);
			this.lstCandidateFiles.AllowColumnReorder = true;
			this.lstCandidateFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstCandidateFiles.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
			this.lstCandidateFiles.CheckBoxes = true;
			this.lstCandidateFiles.CheckedAspectName = "";
			this.lstCandidateFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnSource,
            this.ColumnFilepath,
            this.ColumnShowname,
            this.ColumnSeason,
            this.ColumnEpisode,
            this.ColumnEpisodeName,
            this.ColumnDestination,
            this.ColumnNewFilename});
			this.lstCandidateFiles.ContextMenuStrip = this.contextFiles;
			this.lstCandidateFiles.EmptyListMsg = "No matching files in this folder";
			this.lstCandidateFiles.EmptyListMsgFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstCandidateFiles.FullRowSelect = true;
			this.lstCandidateFiles.Location = new System.Drawing.Point(12, 33);
			this.lstCandidateFiles.Name = "lstCandidateFiles";
			this.lstCandidateFiles.ShowGroups = false;
			this.lstCandidateFiles.ShowImagesOnSubItems = true;
			this.lstCandidateFiles.ShowItemToolTips = true;
			this.lstCandidateFiles.Size = new System.Drawing.Size(996, 427);
			this.lstCandidateFiles.TabIndex = 17;
			this.lstCandidateFiles.UseCompatibleStateImageBehavior = false;
			this.lstCandidateFiles.View = System.Windows.Forms.View.Details;
			this.lstCandidateFiles.VirtualMode = true;
			this.lstCandidateFiles.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.lstCandidateFiles_CellEditFinishing);
			this.lstCandidateFiles.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.lstCandidateFiles_CellEditStarting);
			this.lstCandidateFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstCandidateFiles_DragDrop);
			this.lstCandidateFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstCandidateFiles_DragEnter);
			this.lstCandidateFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstCandidateFiles_KeyDown);
			this.lstCandidateFiles.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstCandidateFiles_KeyUp);
			this.lstCandidateFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstCandidateFiles_MouseDoubleClick);
			// 
			// ColumnSource
			// 
			this.ColumnSource.AspectName = "Filename";
			this.ColumnSource.IsEditable = false;
			this.ColumnSource.Text = "Old Filename";
			// 
			// ColumnFilepath
			// 
			this.ColumnFilepath.AspectName = "FilePath.Path";
			this.ColumnFilepath.IsEditable = false;
			this.ColumnFilepath.Text = "Filepath";
			// 
			// ColumnShowname
			// 
			this.ColumnShowname.AspectName = "ShowName";
			this.ColumnShowname.Text = "Showname";
			// 
			// ColumnSeason
			// 
			this.ColumnSeason.AspectName = "Season";
			this.ColumnSeason.Text = "Season";
			// 
			// ColumnEpisode
			// 
			this.ColumnEpisode.AspectName = "Episode";
			this.ColumnEpisode.Text = "Episode";
			// 
			// ColumnEpisodeName
			// 
			this.ColumnEpisodeName.Text = "Episode Title";
			// 
			// ColumnDestination
			// 
			this.ColumnDestination.AspectName = "Destination";
			this.ColumnDestination.Text = "Destination";
			this.ColumnDestination.Width = 295;
			// 
			// ColumnNewFilename
			// 
			this.ColumnNewFilename.AspectName = "NewFilename";
			this.ColumnNewFilename.Text = "New Filename";
			this.ColumnNewFilename.Width = 150;
			// 
			// contextFiles
			// 
			this.contextFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.editSubtitleToolStripMenuItem,
            this.markAsMovieToolStripMenuItem,
            this.markAsTVSeriesToolStripMenuItem,
            this.lookUpOnIMDBToolStripMenuItem,
            this.toolStripSeparator4,
            this.renamingToolStripMenuItem,
            this.selectToolStripMenuItem,
            this.checkToolStripMenuItem,
            this.toolStripSeparator2,
            this.setShownameToolStripMenuItem,
            this.setSeasonToolStripMenuItem,
            this.setEpisodesFromtoToolStripMenuItem,
            this.setDestinationToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.toolStripSeparator3,
            this.copyToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.regexTesterToolStripMenuItem,
            this.aboutDialogToolStripMenuItem});
			this.contextFiles.Name = "contextFiles";
			this.contextFiles.ShowImageMargin = false;
			this.contextFiles.Size = new System.Drawing.Size(186, 440);
			this.contextFiles.Opening += new System.ComponentModel.CancelEventHandler(this.contextFiles_Opening);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.viewToolStripMenuItem.Text = "Open";
			this.viewToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
			// 
			// editSubtitleToolStripMenuItem
			// 
			this.editSubtitleToolStripMenuItem.Name = "editSubtitleToolStripMenuItem";
			this.editSubtitleToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.editSubtitleToolStripMenuItem.Text = "Edit Subtitle";
			this.editSubtitleToolStripMenuItem.Click += new System.EventHandler(this.editSubtitleToolStripMenuItem_Click);
			// 
			// markAsMovieToolStripMenuItem
			// 
			this.markAsMovieToolStripMenuItem.Name = "markAsMovieToolStripMenuItem";
			this.markAsMovieToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.markAsMovieToolStripMenuItem.Text = "Mark as Movie";
			this.markAsMovieToolStripMenuItem.Click += new System.EventHandler(this.markAsMovieToolStripMenuItem_Click);
			// 
			// markAsTVSeriesToolStripMenuItem
			// 
			this.markAsTVSeriesToolStripMenuItem.Name = "markAsTVSeriesToolStripMenuItem";
			this.markAsTVSeriesToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.markAsTVSeriesToolStripMenuItem.Text = "Mark as TV series";
			this.markAsTVSeriesToolStripMenuItem.Click += new System.EventHandler(this.markAsTVSeriesToolStripMenuItem_Click);
			// 
			// lookUpOnIMDBToolStripMenuItem
			// 
			this.lookUpOnIMDBToolStripMenuItem.Name = "lookUpOnIMDBToolStripMenuItem";
			this.lookUpOnIMDBToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.lookUpOnIMDBToolStripMenuItem.Text = "Look up on IMDB";
			this.lookUpOnIMDBToolStripMenuItem.Click += new System.EventHandler(this.lookUpOnIMDBToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(182, 6);
			// 
			// renamingToolStripMenuItem
			// 
			this.renamingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createDirectoryStructureToolStripMenuItem,
            this.useUmlautsToolStripMenuItem,
            this.caseToolStripMenuItem,
            this.replaceInPathToolStripMenuItem});
			this.renamingToolStripMenuItem.Name = "renamingToolStripMenuItem";
			this.renamingToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.renamingToolStripMenuItem.Text = "Renaming";
			// 
			// createDirectoryStructureToolStripMenuItem
			// 
			this.createDirectoryStructureToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createDirectoryStructureToolStripMenuItem1,
            this.dontCreateDirectoryStructureToolStripMenuItem});
			this.createDirectoryStructureToolStripMenuItem.Name = "createDirectoryStructureToolStripMenuItem";
			this.createDirectoryStructureToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.createDirectoryStructureToolStripMenuItem.Text = "Create DirectoryStructure";
			// 
			// createDirectoryStructureToolStripMenuItem1
			// 
			this.createDirectoryStructureToolStripMenuItem1.Name = "createDirectoryStructureToolStripMenuItem1";
			this.createDirectoryStructureToolStripMenuItem1.Size = new System.Drawing.Size(238, 22);
			this.createDirectoryStructureToolStripMenuItem1.Text = "Create directory structure";
			this.createDirectoryStructureToolStripMenuItem1.Click += new System.EventHandler(this.createDirectoryStructureToolStripMenuItem1_Click);
			// 
			// dontCreateDirectoryStructureToolStripMenuItem
			// 
			this.dontCreateDirectoryStructureToolStripMenuItem.Name = "dontCreateDirectoryStructureToolStripMenuItem";
			this.dontCreateDirectoryStructureToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
			this.dontCreateDirectoryStructureToolStripMenuItem.Text = "Don\'t create directory structure";
			this.dontCreateDirectoryStructureToolStripMenuItem.Click += new System.EventHandler(this.dontCreateDirectoryStructureToolStripMenuItem_Click);
			// 
			// useUmlautsToolStripMenuItem
			// 
			this.useUmlautsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.useUmlautsToolStripMenuItem1,
            this.dontUseUmlautsToolStripMenuItem,
            this.useProvidedNamesToolStripMenuItem});
			this.useUmlautsToolStripMenuItem.Name = "useUmlautsToolStripMenuItem";
			this.useUmlautsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.useUmlautsToolStripMenuItem.Text = "Use umlauts";
			// 
			// useUmlautsToolStripMenuItem1
			// 
			this.useUmlautsToolStripMenuItem1.Name = "useUmlautsToolStripMenuItem1";
			this.useUmlautsToolStripMenuItem1.Size = new System.Drawing.Size(181, 22);
			this.useUmlautsToolStripMenuItem1.Text = "Use umlauts";
			this.useUmlautsToolStripMenuItem1.Click += new System.EventHandler(this.useUmlautsToolStripMenuItem1_Click);
			// 
			// dontUseUmlautsToolStripMenuItem
			// 
			this.dontUseUmlautsToolStripMenuItem.Name = "dontUseUmlautsToolStripMenuItem";
			this.dontUseUmlautsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.dontUseUmlautsToolStripMenuItem.Text = "Don\'t use umlauts";
			this.dontUseUmlautsToolStripMenuItem.Click += new System.EventHandler(this.dontUseUmlautsToolStripMenuItem_Click);
			// 
			// useProvidedNamesToolStripMenuItem
			// 
			this.useProvidedNamesToolStripMenuItem.Name = "useProvidedNamesToolStripMenuItem";
			this.useProvidedNamesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.useProvidedNamesToolStripMenuItem.Text = "Use provided names";
			this.useProvidedNamesToolStripMenuItem.Click += new System.EventHandler(this.useProvidedNamesToolStripMenuItem_Click);
			// 
			// caseToolStripMenuItem
			// 
			this.caseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.largeToolStripMenuItem,
            this.smallToolStripMenuItem,
            this.igNorEToolStripMenuItem,
            this.cAPSLOCKToolStripMenuItem});
			this.caseToolStripMenuItem.Name = "caseToolStripMenuItem";
			this.caseToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.caseToolStripMenuItem.Text = "Case";
			// 
			// largeToolStripMenuItem
			// 
			this.largeToolStripMenuItem.Name = "largeToolStripMenuItem";
			this.largeToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
			this.largeToolStripMenuItem.Text = "Large";
			this.largeToolStripMenuItem.Click += new System.EventHandler(this.largeToolStripMenuItem_Click);
			// 
			// smallToolStripMenuItem
			// 
			this.smallToolStripMenuItem.Name = "smallToolStripMenuItem";
			this.smallToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
			this.smallToolStripMenuItem.Text = "small";
			this.smallToolStripMenuItem.Click += new System.EventHandler(this.smallToolStripMenuItem_Click);
			// 
			// igNorEToolStripMenuItem
			// 
			this.igNorEToolStripMenuItem.Name = "igNorEToolStripMenuItem";
			this.igNorEToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
			this.igNorEToolStripMenuItem.Text = "IgNorE";
			this.igNorEToolStripMenuItem.Click += new System.EventHandler(this.igNorEToolStripMenuItem_Click);
			// 
			// cAPSLOCKToolStripMenuItem
			// 
			this.cAPSLOCKToolStripMenuItem.Name = "cAPSLOCKToolStripMenuItem";
			this.cAPSLOCKToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
			this.cAPSLOCKToolStripMenuItem.Text = "CAPSLOCK";
			this.cAPSLOCKToolStripMenuItem.Click += new System.EventHandler(this.cAPSLOCKToolStripMenuItem_Click);
			// 
			// replaceInPathToolStripMenuItem
			// 
			this.replaceInPathToolStripMenuItem.Name = "replaceInPathToolStripMenuItem";
			this.replaceInPathToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
			this.replaceInPathToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.replaceInPathToolStripMenuItem.Text = "Replace";
			this.replaceInPathToolStripMenuItem.Click += new System.EventHandler(this.replaceInPathToolStripMenuItem_Click);
			// 
			// selectToolStripMenuItem
			// 
			this.selectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.invertSelectionToolStripMenuItem,
            this.selectAllToolStripMenuItem,
            this.selectByKeywordToolStripMenuItem,
            this.selectSimilarToolStripMenuItem});
			this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
			this.selectToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.selectToolStripMenuItem.Text = "Select";
			// 
			// invertSelectionToolStripMenuItem
			// 
			this.invertSelectionToolStripMenuItem.Name = "invertSelectionToolStripMenuItem";
			this.invertSelectionToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
			this.invertSelectionToolStripMenuItem.Text = "Invert selection";
			this.invertSelectionToolStripMenuItem.Click += new System.EventHandler(this.invertSelectionToolStripMenuItem_Click);
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
			this.selectAllToolStripMenuItem.Text = "Select all";
			this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
			// 
			// selectByKeywordToolStripMenuItem
			// 
			this.selectByKeywordToolStripMenuItem.Name = "selectByKeywordToolStripMenuItem";
			this.selectByKeywordToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.selectByKeywordToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
			this.selectByKeywordToolStripMenuItem.Text = "Select by keyword";
			this.selectByKeywordToolStripMenuItem.Click += new System.EventHandler(this.selectByKeywordToolStripMenuItem_Click);
			// 
			// selectSimilarToolStripMenuItem
			// 
			this.selectSimilarToolStripMenuItem.Name = "selectSimilarToolStripMenuItem";
			this.selectSimilarToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
			this.selectSimilarToolStripMenuItem.Text = "Select similar by name";
			this.selectSimilarToolStripMenuItem.Click += new System.EventHandler(this.byNameToolStripMenuItem_Click);
			// 
			// checkToolStripMenuItem
			// 
			this.checkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkAllToolStripMenuItem,
            this.uncheckAllToolStripMenuItem,
            this.invertCheckToolStripMenuItem,
            this.toggleSelectedToolStripMenuItem});
			this.checkToolStripMenuItem.Name = "checkToolStripMenuItem";
			this.checkToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.checkToolStripMenuItem.Text = "Check";
			// 
			// checkAllToolStripMenuItem
			// 
			this.checkAllToolStripMenuItem.Name = "checkAllToolStripMenuItem";
			this.checkAllToolStripMenuItem.ShortcutKeyDisplayString = "Space";
			this.checkAllToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.checkAllToolStripMenuItem.Text = "Check all";
			this.checkAllToolStripMenuItem.Click += new System.EventHandler(this.checkAllToolStripMenuItem_Click);
			// 
			// uncheckAllToolStripMenuItem
			// 
			this.uncheckAllToolStripMenuItem.Name = "uncheckAllToolStripMenuItem";
			this.uncheckAllToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.uncheckAllToolStripMenuItem.Text = "Uncheck all";
			this.uncheckAllToolStripMenuItem.Click += new System.EventHandler(this.uncheckAllToolStripMenuItem_Click);
			// 
			// invertCheckToolStripMenuItem
			// 
			this.invertCheckToolStripMenuItem.Name = "invertCheckToolStripMenuItem";
			this.invertCheckToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.invertCheckToolStripMenuItem.Text = "Invert Check";
			this.invertCheckToolStripMenuItem.Click += new System.EventHandler(this.invertCheckToolStripMenuItem_Click);
			// 
			// toggleSelectedToolStripMenuItem
			// 
			this.toggleSelectedToolStripMenuItem.Name = "toggleSelectedToolStripMenuItem";
			this.toggleSelectedToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.toggleSelectedToolStripMenuItem.Text = "Toggle Selected";
			this.toggleSelectedToolStripMenuItem.Click += new System.EventHandler(this.toggleSelectedToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(182, 6);
			// 
			// setShownameToolStripMenuItem
			// 
			this.setShownameToolStripMenuItem.Name = "setShownameToolStripMenuItem";
			this.setShownameToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.setShownameToolStripMenuItem.Text = "Set Showname";
			this.setShownameToolStripMenuItem.Click += new System.EventHandler(this.setShownameToolStripMenuItem_Click);
			// 
			// setSeasonToolStripMenuItem
			// 
			this.setSeasonToolStripMenuItem.Name = "setSeasonToolStripMenuItem";
			this.setSeasonToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.setSeasonToolStripMenuItem.Text = "Set Season";
			this.setSeasonToolStripMenuItem.Click += new System.EventHandler(this.setSeasonToolStripMenuItem_Click);
			// 
			// setEpisodesFromtoToolStripMenuItem
			// 
			this.setEpisodesFromtoToolStripMenuItem.Name = "setEpisodesFromtoToolStripMenuItem";
			this.setEpisodesFromtoToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.setEpisodesFromtoToolStripMenuItem.Text = "Set Episodes from...to...";
			this.setEpisodesFromtoToolStripMenuItem.Click += new System.EventHandler(this.setEpisodesFromtoToolStripMenuItem_Click);
			// 
			// setDestinationToolStripMenuItem
			// 
			this.setDestinationToolStripMenuItem.Name = "setDestinationToolStripMenuItem";
			this.setDestinationToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.setDestinationToolStripMenuItem.Text = "Set Destination";
			this.setDestinationToolStripMenuItem.Click += new System.EventHandler(this.setDestinationToolStripMenuItem_Click);
			// 
			// renameToolStripMenuItem
			// 
			this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
			this.renameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.renameToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.renameToolStripMenuItem.Text = "Set Filename";
			this.renameToolStripMenuItem.Visible = false;
			this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(182, 6);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.originalNameToolStripMenuItem,
            this.pathOrigNameToolStripMenuItem,
            this.titleToolStripMenuItem,
            this.newFileNameToolStripMenuItem,
            this.destinationNewFileNameToolStripMenuItem,
            this.operationToolStripMenuItem});
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.copyToolStripMenuItem.Text = "Copy...";
			// 
			// originalNameToolStripMenuItem
			// 
			this.originalNameToolStripMenuItem.Name = "originalNameToolStripMenuItem";
			this.originalNameToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.originalNameToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
			this.originalNameToolStripMenuItem.Text = "Original Name";
			this.originalNameToolStripMenuItem.Click += new System.EventHandler(this.originalNameToolStripMenuItem_Click);
			// 
			// pathOrigNameToolStripMenuItem
			// 
			this.pathOrigNameToolStripMenuItem.Name = "pathOrigNameToolStripMenuItem";
			this.pathOrigNameToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
			this.pathOrigNameToolStripMenuItem.Text = "Path + Orig. Name";
			this.pathOrigNameToolStripMenuItem.Click += new System.EventHandler(this.pathOrigNameToolStripMenuItem_Click);
			// 
			// titleToolStripMenuItem
			// 
			this.titleToolStripMenuItem.Name = "titleToolStripMenuItem";
			this.titleToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
			this.titleToolStripMenuItem.Text = "Title";
			this.titleToolStripMenuItem.Click += new System.EventHandler(this.titleToolStripMenuItem_Click);
			// 
			// newFileNameToolStripMenuItem
			// 
			this.newFileNameToolStripMenuItem.Name = "newFileNameToolStripMenuItem";
			this.newFileNameToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
			this.newFileNameToolStripMenuItem.Text = "New Filename";
			this.newFileNameToolStripMenuItem.Click += new System.EventHandler(this.newFileNameToolStripMenuItem_Click);
			// 
			// destinationNewFileNameToolStripMenuItem
			// 
			this.destinationNewFileNameToolStripMenuItem.Name = "destinationNewFileNameToolStripMenuItem";
			this.destinationNewFileNameToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
			this.destinationNewFileNameToolStripMenuItem.Text = "Destination + New Filename";
			this.destinationNewFileNameToolStripMenuItem.Click += new System.EventHandler(this.destinationNewFileNameToolStripMenuItem_Click);
			// 
			// operationToolStripMenuItem
			// 
			this.operationToolStripMenuItem.Name = "operationToolStripMenuItem";
			this.operationToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
			this.operationToolStripMenuItem.Text = "Operation";
			this.operationToolStripMenuItem.Click += new System.EventHandler(this.operationToolStripMenuItem_Click);
			// 
			// refreshToolStripMenuItem
			// 
			this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
			this.refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.refreshToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.refreshToolStripMenuItem.Text = "Refresh";
			this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
			// 
			// removeToolStripMenuItem
			// 
			this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
			this.removeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.removeToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.removeToolStripMenuItem.Text = "Remove from list";
			this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
			// 
			// regexTesterToolStripMenuItem
			// 
			this.regexTesterToolStripMenuItem.Name = "regexTesterToolStripMenuItem";
			this.regexTesterToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
			this.regexTesterToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.regexTesterToolStripMenuItem.Text = "RegexTester";
			this.regexTesterToolStripMenuItem.Visible = false;
			this.regexTesterToolStripMenuItem.Click += new System.EventHandler(this.regexTesterToolStripMenuItem_Click);
			// 
			// aboutDialogToolStripMenuItem
			// 
			this.aboutDialogToolStripMenuItem.Name = "aboutDialogToolStripMenuItem";
			this.aboutDialogToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.aboutDialogToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.aboutDialogToolStripMenuItem.Text = "AboutDialog";
			this.aboutDialogToolStripMenuItem.Visible = false;
			this.aboutDialogToolStripMenuItem.Click += new System.EventHandler(this.aboutDialogToolStripMenuItem_Click);
			// 
			// taskProgressBar
			// 
			this.taskProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.taskProgressBar.BackColor = System.Drawing.SystemColors.Control;
			this.taskProgressBar.Location = new System.Drawing.Point(12, 4);
			this.taskProgressBar.Name = "taskProgressBar";
			this.taskProgressBar.Size = new System.Drawing.Size(429, 23);
			this.taskProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.taskProgressBar.TabIndex = 16;
			this.taskProgressBar.Visible = false;
			// 
			// btnExploreCurrentFolder
			// 
			this.btnExploreCurrentFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExploreCurrentFolder.Image = global::Renamer.Properties.Resources.Browse;
			this.btnExploreCurrentFolder.Location = new System.Drawing.Point(413, 4);
			this.btnExploreCurrentFolder.Name = "btnExploreCurrentFolder";
			this.btnExploreCurrentFolder.Size = new System.Drawing.Size(28, 23);
			this.btnExploreCurrentFolder.TabIndex = 15;
			this.btnExploreCurrentFolder.UseVisualStyleBackColor = true;
			this.btnExploreCurrentFolder.Click += new System.EventHandler(this.btnExploreCurrentFolder_Click);
			// 
			// btnRenameSelectedCandidates
			// 
			this.btnRenameSelectedCandidates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRenameSelectedCandidates.AutoSize = true;
			this.btnRenameSelectedCandidates.Location = new System.Drawing.Point(741, 4);
			this.btnRenameSelectedCandidates.Name = "btnRenameSelectedCandidates";
			this.btnRenameSelectedCandidates.Size = new System.Drawing.Size(85, 23);
			this.btnRenameSelectedCandidates.TabIndex = 11;
			this.btnRenameSelectedCandidates.Text = "Rename !";
			this.btnRenameSelectedCandidates.UseVisualStyleBackColor = true;
			this.btnRenameSelectedCandidates.Click += new System.EventHandler(this.btnRenameSelectedCandidates_Click);
			// 
			// txtTargetFilenamePattern
			// 
			this.txtTargetFilenamePattern.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTargetFilenamePattern.Location = new System.Drawing.Point(629, 6);
			this.txtTargetFilenamePattern.MinimumSize = new System.Drawing.Size(100, 20);
			this.txtTargetFilenamePattern.Name = "txtTargetFilenamePattern";
			this.txtTargetFilenamePattern.Size = new System.Drawing.Size(106, 20);
			this.txtTargetFilenamePattern.TabIndex = 9;
			this.txtTargetFilenamePattern.Text = "S%sE%E - %N";
			this.txtTargetFilenamePattern.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTarget_KeyDown);
			this.txtTargetFilenamePattern.Leave += new System.EventHandler(this.txtTarget_Leave);
			// 
			// cbSubtitleProviders
			// 
			this.cbSubtitleProviders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSubtitleProviders.FormattingEnabled = true;
			this.cbSubtitleProviders.Location = new System.Drawing.Point(447, 244);
			this.cbSubtitleProviders.Name = "cbSubtitleProviders";
			this.cbSubtitleProviders.Size = new System.Drawing.Size(196, 21);
			this.cbSubtitleProviders.TabIndex = 13;
			this.cbSubtitleProviders.Visible = false;
			this.cbSubtitleProviders.SelectedIndexChanged += new System.EventHandler(this.cbSubs_SelectedIndexChanged);
			// 
			// lblSubFrom
			// 
			this.lblSubFrom.AutoSize = true;
			this.lblSubFrom.Location = new System.Drawing.Point(353, 249);
			this.lblSubFrom.Name = "lblSubFrom";
			this.lblSubFrom.Size = new System.Drawing.Size(88, 13);
			this.lblSubFrom.TabIndex = 12;
			this.lblSubFrom.Text = "Get subtitles from";
			this.lblSubFrom.Visible = false;
			// 
			// lblTargetFilename
			// 
			this.lblTargetFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTargetFilename.AutoSize = true;
			this.lblTargetFilename.Location = new System.Drawing.Point(543, 9);
			this.lblTargetFilename.Name = "lblTargetFilename";
			this.lblTargetFilename.Size = new System.Drawing.Size(80, 13);
			this.lblTargetFilename.TabIndex = 8;
			this.lblTargetFilename.Text = "Target filename";
			// 
			// btnSearchForSubtitles
			// 
			this.btnSearchForSubtitles.Location = new System.Drawing.Point(680, 244);
			this.btnSearchForSubtitles.Name = "btnSearchForSubtitles";
			this.btnSearchForSubtitles.Size = new System.Drawing.Size(85, 23);
			this.btnSearchForSubtitles.TabIndex = 11;
			this.btnSearchForSubtitles.Text = "Get Subtitles !";
			this.btnSearchForSubtitles.UseVisualStyleBackColor = true;
			this.btnSearchForSubtitles.Visible = false;
			this.btnSearchForSubtitles.Click += new System.EventHandler(this.btnSearchForSubtitles_Click);
			// 
			// btnAboutApplication
			// 
			this.btnAboutApplication.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAboutApplication.Location = new System.Drawing.Point(923, 4);
			this.btnAboutApplication.Name = "btnAboutApplication";
			this.btnAboutApplication.Size = new System.Drawing.Size(85, 23);
			this.btnAboutApplication.TabIndex = 10;
			this.btnAboutApplication.Text = "About...";
			this.btnAboutApplication.UseVisualStyleBackColor = true;
			this.btnAboutApplication.Click += new System.EventHandler(this.btnAboutApplication_Click);
			// 
			// cbProviders
			// 
			this.cbProviders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbProviders.FormattingEnabled = true;
			this.cbProviders.Location = new System.Drawing.Point(180, 244);
			this.cbProviders.Name = "cbProviders";
			this.cbProviders.Size = new System.Drawing.Size(167, 21);
			this.cbProviders.TabIndex = 9;
			this.cbProviders.Visible = false;
			this.cbProviders.SelectedIndexChanged += new System.EventHandler(this.cbProviders_SelectedIndexChanged);
			// 
			// lblTitlesFrom
			// 
			this.lblTitlesFrom.AutoSize = true;
			this.lblTitlesFrom.Location = new System.Drawing.Point(86, 247);
			this.lblTitlesFrom.Name = "lblTitlesFrom";
			this.lblTitlesFrom.Size = new System.Drawing.Size(71, 13);
			this.lblTitlesFrom.TabIndex = 8;
			this.lblTitlesFrom.Text = "Get titles from";
			this.lblTitlesFrom.Visible = false;
			// 
			// btnSearchForTitles
			// 
			this.btnSearchForTitles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearchForTitles.Location = new System.Drawing.Point(447, 4);
			this.btnSearchForTitles.Name = "btnSearchForTitles";
			this.btnSearchForTitles.Size = new System.Drawing.Size(85, 23);
			this.btnSearchForTitles.TabIndex = 5;
			this.btnSearchForTitles.Text = "Get Titles !";
			this.btnSearchForTitles.UseVisualStyleBackColor = true;
			this.btnSearchForTitles.Click += new System.EventHandler(this.btnSearchForTitles_Click);
			// 
			// btnOpenConfiguration
			// 
			this.btnOpenConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpenConfiguration.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnOpenConfiguration.Location = new System.Drawing.Point(832, 4);
			this.btnOpenConfiguration.Name = "btnOpenConfiguration";
			this.btnOpenConfiguration.Size = new System.Drawing.Size(85, 23);
			this.btnOpenConfiguration.TabIndex = 3;
			this.btnOpenConfiguration.Text = "Configuration";
			this.btnOpenConfiguration.UseVisualStyleBackColor = true;
			this.btnOpenConfiguration.Click += new System.EventHandler(this.btnOpenConfiguration_Click);
			// 
			// btnChangeCurrentFolder
			// 
			this.btnChangeCurrentFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnChangeCurrentFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnChangeCurrentFolder.Location = new System.Drawing.Point(379, 4);
			this.btnChangeCurrentFolder.Name = "btnChangeCurrentFolder";
			this.btnChangeCurrentFolder.Size = new System.Drawing.Size(28, 23);
			this.btnChangeCurrentFolder.TabIndex = 2;
			this.btnChangeCurrentFolder.Text = "...";
			this.btnChangeCurrentFolder.UseVisualStyleBackColor = true;
			this.btnChangeCurrentFolder.Click += new System.EventHandler(this.btnChangeCurrentFolder_Click);
			// 
			// txtCurrentFolderPath
			// 
			this.txtCurrentFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtCurrentFolderPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txtCurrentFolderPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
			this.txtCurrentFolderPath.Location = new System.Drawing.Point(54, 6);
			this.txtCurrentFolderPath.Name = "txtCurrentFolderPath";
			this.txtCurrentFolderPath.Size = new System.Drawing.Size(319, 20);
			this.txtCurrentFolderPath.TabIndex = 1;
			this.txtCurrentFolderPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCurrentFolderPath_KeyDown);
			this.txtCurrentFolderPath.Leave += new System.EventHandler(this.txtCurrentFolderPath_Leave);
			// 
			// lblCurrentFolderPath
			// 
			this.lblCurrentFolderPath.AutoSize = true;
			this.lblCurrentFolderPath.Location = new System.Drawing.Point(12, 9);
			this.lblCurrentFolderPath.Name = "lblCurrentFolderPath";
			this.lblCurrentFolderPath.Size = new System.Drawing.Size(36, 13);
			this.lblCurrentFolderPath.TabIndex = 0;
			this.lblCurrentFolderPath.Text = "Folder";
			// 
			// txtBasicLog
			// 
			this.txtBasicLog.Location = new System.Drawing.Point(12, 3);
			this.txtBasicLog.Multiline = true;
			this.txtBasicLog.Name = "txtLog";
			this.txtBasicLog.ReadOnly = true;
			this.txtBasicLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtBasicLog.Size = new System.Drawing.Size(996, 91);
			this.txtBasicLog.TabIndex = 1;
			this.toolTip.SetToolTip(this.txtBasicLog, "Double Click to open Logfile");
			this.txtBasicLog.Visible = false;
			this.txtBasicLog.DoubleClick += new System.EventHandler(this.LogElement_DoubleClick);
			// 
			// rtbEnhancedLog
			// 
			this.rtbEnhancedLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rtbEnhancedLog.Location = new System.Drawing.Point(12, 3);
			this.rtbEnhancedLog.Name = "rtbLog";
			this.rtbEnhancedLog.ReadOnly = true;
			this.rtbEnhancedLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.rtbEnhancedLog.Size = new System.Drawing.Size(996, 91);
			this.rtbEnhancedLog.TabIndex = 0;
			this.rtbEnhancedLog.Text = "";
			this.toolTip.SetToolTip(this.rtbEnhancedLog, "Double Click to open Logfile");
			this.rtbEnhancedLog.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbLog_LinkClicked);
			this.rtbEnhancedLog.DoubleClick += new System.EventHandler(this.LogElement_DoubleClick);
			// 
			// fbdChangeCurrentFolder
			// 
			this.fbdChangeCurrentFolder.Description = "Browse for folder containing series.";
			this.fbdChangeCurrentFolder.ShowNewFolderButton = false;
			// 
			// backgroundTaskWorker
			// 
			this.backgroundTaskWorker.WorkerReportsProgress = true;
			this.backgroundTaskWorker.WorkerSupportsCancellation = true;
			this.backgroundTaskWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundTaskWorker_DoWork);
			this.backgroundTaskWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundTaskWorker_ProgressChanged);
			this.backgroundTaskWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundTaskWorker_RunWorkerCompleted);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1016, 573);
			this.Controls.Add(this.scMainLogSplit);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(944, 506);
			this.Name = "MainForm";
			this.Text = "Series Renamer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
			this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.scMainLogSplit.Panel1.ResumeLayout(false);
			this.scMainLogSplit.Panel1.PerformLayout();
			this.scMainLogSplit.Panel2.ResumeLayout(false);
			this.scMainLogSplit.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.scMainLogSplit)).EndInit();
			this.scMainLogSplit.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.lstCandidateFiles)).EndInit();
			this.contextFiles.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer scMainLogSplit;
        private System.Windows.Forms.Label lblCurrentFolderPath;
        private System.Windows.Forms.TextBox txtCurrentFolderPath;
        private System.Windows.Forms.Button btnChangeCurrentFolder;
        private System.Windows.Forms.Button btnOpenConfiguration;
        private System.Windows.Forms.FolderBrowserDialog fbdChangeCurrentFolder;
        private System.Windows.Forms.Button btnSearchForTitles;
        private System.Windows.Forms.Label lblTargetFilename;
        private System.Windows.Forms.TextBox txtTargetFilenamePattern;
        private System.Windows.Forms.Button btnRenameSelectedCandidates;
        private System.Windows.Forms.ComboBox cbProviders;
        private System.Windows.Forms.Label lblTitlesFrom;
        private System.Windows.Forms.Button btnAboutApplication;
        private System.Windows.Forms.ContextMenuStrip contextFiles;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem setSeasonToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.Label lblSubFrom;
        private System.Windows.Forms.Button btnSearchForSubtitles;
        private System.Windows.Forms.ComboBox cbSubtitleProviders;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem setEpisodesFromtoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editSubtitleToolStripMenuItem;
        private System.Windows.Forms.RichTextBox rtbEnhancedLog;
        private System.Windows.Forms.ToolStripMenuItem setDestinationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem originalNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pathOrigNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem titleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFileNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem destinationNewFileNameToolStripMenuItem;
        private System.Windows.Forms.Button btnExploreCurrentFolder;
        private System.Windows.Forms.TextBox txtBasicLog;
        private System.Windows.Forms.ToolStripMenuItem operationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renamingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createDirectoryStructureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceInPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invertSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectByKeywordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectSimilarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncheckAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invertCheckToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createDirectoryStructureToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem dontCreateDirectoryStructureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useUmlautsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useUmlautsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem dontUseUmlautsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useProvidedNamesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem caseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem largeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem igNorEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cAPSLOCKToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setShownameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem regexTesterToolStripMenuItem;
        public System.Windows.Forms.ProgressBar taskProgressBar;
        private BrightIdeasSoftware.FastObjectListView lstCandidateFiles;
        private BrightIdeasSoftware.OLVColumn ColumnSource;
        private BrightIdeasSoftware.OLVColumn ColumnFilepath;
        private BrightIdeasSoftware.OLVColumn ColumnShowname;
        private BrightIdeasSoftware.OLVColumn ColumnSeason;
        private BrightIdeasSoftware.OLVColumn ColumnEpisode;
        private BrightIdeasSoftware.OLVColumn ColumnEpisodeName;
        private BrightIdeasSoftware.OLVColumn ColumnDestination;
        private BrightIdeasSoftware.OLVColumn ColumnNewFilename;
        private System.Windows.Forms.ToolStripMenuItem markAsMovieToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markAsTVSeriesToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundTaskWorker;
        private System.Windows.Forms.Button btnCancelBackgroundTask;
        public System.Windows.Forms.Label lblFileListingProgress;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutDialogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lookUpOnIMDBToolStripMenuItem;
    }
}

