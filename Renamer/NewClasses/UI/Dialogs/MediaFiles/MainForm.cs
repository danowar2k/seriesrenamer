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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Diagnostics;
using Renamer.Classes.Provider;
using BrightIdeasSoftware;
using Renamer.NewClasses.Config;
namespace Renamer.NewClasses.UI.Dialogs.MediaFiles {
    /// <summary>
    /// Main Form
    /// </summary>
    public partial class MainForm : Form {
        /// <summary>
        /// Settings class contains some stuff which can't be stored in config file for one reason or another
        /// </summary>
        private Settings settings;

        /// <summary>
        /// Custom sorting class to sort list view for 2 columns
        /// </summary>
        private ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();

        /// <summary>
        /// Column width ratios needed to keep them during resize
        /// </summary>
        private List<float> columnWidths = new List<float>();

        /// <summary>
        /// Temp variable to store a previously focused control
        /// </summary>
        private Control lastFocusedControl = null;

        /// <summary>
        /// bool for storing space key status
        /// </summary>
        private bool spacedown = false;

        /// <summary>
        /// Program arguments
        /// </summary>
        private List<string> args;

        protected static MainForm instance;
        private static object m_lock = new object();

        public static MainForm Instance {
            get {
                if (instance == null) {
                    lock (m_lock) { if (instance == null) instance = new MainForm(null); }
                }
                return instance;
            }
            set {
                instance = value;
            }
        }


        /// <summary>
        /// The last task that has been scheduled for background work
        /// </summary>
        private MainTask lastScheduledTask;

        public struct WorkerArguments {
            public MainTask scheduledTask;
            public object[] taskArgs;
        }
        /// <summary>
        /// GUI constructor
        /// </summary>
        /// <param name="args">program arguments</param>
        public MainForm(string[] args) {
            this.args = new List<string>(args);
            InitializeComponent();
        }

        #region processing
        #region Provider Name Creation




        /// <summary>
        /// Sets an Candidate as movie and generates name
        /// </summary>
        private void MarkAsMovie() {
            //MOVIES_TAGS_TO_REMOVE_LIST_KEY should be preceeded by . or _ or ( or [ or - or ]
            string[] tags = Helper.ReadProperties(ConfigKeyConstants.MOVIES_TAGS_TO_REMOVE_LIST_KEY);
            List<string> regexes = new List<string>();
            foreach (string s in tags) {
                regexes.Add("[\\._\\(\\[-]" + s);
            }
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)((OLVListItem)lvi).RowObject;
                //Go through all selected files and remove tags and clean them up
                ie.Showname = "";
                ie.DestinationPath = "";
                ie.NewFilename = "";
                ie.RemoveVideoTags();
            }
            updateGUI();
        }

        /// <summary>
        /// Sets an Candidate as TV Show and generates name
        /// </summary>
        private void MarkAsTVShow() {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)((OLVListItem)lvi).RowObject;
                //Go through all selected files and remove tags and clean them up
                ie.IsMovie = false;
                ie.Showname = SeriesNameExtractor.Instance.ExtractSeriesName(ie);
                ie.DestinationPath = "";
                ie.NewFilename = "";
                DataGenerator.ExtractSeasonAndEpisode(ie);
            }
            updateGUI();
        }
        #endregion

        /// <summary>
        /// Checks for colliding candidates (duplicate filenames)
		/// and resolves the collisions.
		/// Concludes by renaming selected candidates.
        /// </summary>
        private void renameSelectedCandidates() {
            foreach (MediaFile ie in MediaFileManager.Instance) {
                if (ie.ProcessingRequested) {
                    MediaFile ieColliding = MediaFileManager.Instance.GetCollidingMediaFile(ie);
                    while (ieColliding != null) {
                        CollidingFiles cf = new CollidingFiles(ie, ieColliding);
                        cf.ShowDialog();
                        ieColliding = MediaFileManager.Instance.GetCollidingMediaFile(ie);
                    }
                }
            }
            setBusyGUI();
			startRenameTask();
        }

		private void startRenameTask() {
			WorkerArguments renameTask = new WorkerArguments();
			renameTask.scheduledTask = MainTask.Rename;
			backgroundTaskWorker.RunWorkerAsync(renameTask);
		}

        #endregion
        #region SubtitlesKeyConstants
        #region Parsing





        /// <summary>
        /// This function is needed if Subtitle links are located on a series page, not implemented yet
        /// </summary>
        /// <param name="url">URL of the page to get subtitle links from</param>
        private void GetSubtitleFromSeriesPage(string url) {
            //Don't forget the cropping
            //
            //Source cropping
            //source = source.Substring(Math.Max(source.IndexOf(titleProvider.SEARCH_START_KEY),0));
            //source = source.Substring(0, Math.Max(source.LastIndexOf(titleProvider.SEARCH_END_KEY),0));
            /*
            //if episode infos are stored on a new page for each season, this should be marked with %S in url, so we can iterate through all those pages
            int season = 1;
            string url2 = url;
            while (true)
            {
                    if (url2.Contains("%S"))
                    {
                            url = url2.CUSTOM_REPLACE_REGEX_STRINGS_KEY("%S", season.ToString());
                    }
                    if (url == null || url == "") return;
                    // request
                    url = System.Web.HttpUtility.UrlPathEncode(url);
                    HttpWebRequest requestHtml = null;
                    try
                    {
                            requestHtml = (HttpWebRequest)(HttpWebRequest.Create(url));
                    }
                    catch (Exception ex)
                    {
                            Logger.Instance.LogMessage(ex.Message, LogLevel.ERROR);
                            return;
                    }
                    requestHtml.CONNECTION_TIMEOUT_KEY = 5000;
                    // get response
                    HttpWebResponse responseHtml;
                    try
                    {
                            responseHtml = (HttpWebResponse)(requestHtml.GetResponse());
                    }
                    catch (WebException e)
                    {
                            Logger.Instance.LogMessage(e.Message, LogLevel.ERROR);
                            return;
                    }
                    //if we get redirected, lets assume this page does not exist
                    if (responseHtml.ResponseUri.AbsoluteUri != url)
                    {
                            break;
                    }
                    // and download
                    //Logger.Instance.LogMessage("charset=" + responseHtml.CharacterSet, LogLevel.INFO);

                    StreamReader r = new StreamReader(responseHtml.GetResponseStream(), ENCODING_KEY.GetEncoding(responseHtml.CharacterSet));
                    string source = r.ReadToEnd();
                    r.Close();
                    responseHtml.Close();
                    string pattern = Settings.subprovider.REGEX_RELATIONS_KEY;
                    MatchCollection mc = Regex.Matches(source, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    foreach (Match m in mc)
                    {
                            //if we are iterating through season pages, take season from page url directly
                            if (url != url2)
                            {
                                    Info.AddRelationCollection(new EpisodeData(season.ToString(), m.Groups["EPISODE_NR"].Value, System.Web.HttpUtility.HtmlDecode(m.Groups["EPISODE_TITLE"].Value)));
                                    //Logger.Instance.LogMessage("Found EpisodeData: " + "S" + season.ToString() + "E" + m.Groups["EPISODE_NR"].Value + " - " + System.Web.HttpUtility.HtmlDecode(m.Groups["EPISODE_TITLE"].Value), LogLevel.INFO);
                            }
                            else
                            {
                                    Info.AddRelationCollection(new EpisodeData(m.Groups["SEASON_NR"].Value, m.Groups["EPISODE_NR"].Value, System.Web.HttpUtility.HtmlDecode(m.Groups["EPISODE_TITLE"].Value)));
                                    //Logger.Instance.LogMessage("Found EpisodeData: " + "S" + m.Groups["SEASON_NR"].Value + "E" + m.Groups["EPISODE_NR"].Value + " - " + System.Web.HttpUtility.HtmlDecode(m.Groups["EPISODE_TITLE"].Value), LogLevel.INFO);
                            }
                    }
                    // THOU SHALL NOT FORGET THE BREAK
                    if (!url2.Contains("%S")) break;
                    season++;
            }*/
        }
        #endregion




        #endregion

        //Since sorting after the last two selected columns is supported, we need some event handling here
        private void lstCandidateFiles_ColumnClick(object sender, ColumnClickEventArgs e) {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn) {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending) {
                    lvwColumnSorter.Order = SortOrder.Descending;
                } else {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            } else {
                // Set the column number that is to be sorted; default to ascending.

                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            //this.lstFiles.Sort();
        }

        //End editing with combo box data types
        /*private void cbEdit_SelectedIndexChanged(object sender, EventArgs e) {
            lstFiles.EndEditing(true);
        }

        //Start editing single values
        private void lstFiles_SubItemClicked(object sender, ListViewEx.SubItemEventArgs e) {
            if (e.SubItem != 0 && e.SubItem != 1) {
                if (settings.IsMonoCompatibilityMode) {
                    Logger.Instance.LogMessage("Editing Entries dynamically is not supported in Mono unfortunately :(", LogLevel.WARNING);
                    return;
                }
                RelationCollection rc = RelationManager.Instance.GetRelationCollection(InfoEntryManager.Instance[(int)e.Item.Tag].Showname);
                if (e.SubItem == 4) {
                    //if season is valid and there are relations at all, show combobox. Otherwise, just show edit box
                    if (rc != null && RelationManager.Instance.Count > 0 && Convert.ToInt32(e.Item.SubItems[2].Text) >= rc.FindMinSeason() && Convert.ToInt32(e.Item.SubItems[2].Text) <= rc.FindMaxSeason()) {
                        comEdit.Items.Clear();
                        foreach (EpisodeData rel in rc) {
                            if (rel.SEASON_NR == InfoEntryManager.Instance[(int)e.Item.Tag].SEASON_NR) {
                                comEdit.Items.Add(rel.PROVIDER_NAME_KEY);
                            }
                        }
                        lstFiles.StartEditing(comEdit, e.Item, e.SubItem);
                    }
                    else {
                        lstFiles.StartEditing(txtEdit, e.Item, e.SubItem);
                    }
                }
                else if (e.SubItem == 5 || e.SubItem == 6 || e.SubItem == 7) {
                    lstFiles.StartEditing(txtEdit, e.Item, e.SubItem);
                }
                else {
                    //clamp season and episode values to allowed values
                    if (rc != null && rc.Count > 0) {
                        if (e.SubItem == 2) {
                            numEdit.Minimum = rc.FindMinSeason();
                            numEdit.Maximum = rc.FindMaxSeason();
                        }
                        else if (e.SubItem == 3) {
                            numEdit.Minimum = rc.FindMinEpisode(Convert.ToInt32(InfoEntryManager.Instance[(int)e.Item.Tag].SEASON_NR));
                            numEdit.Maximum = rc.FindMaxEpisode(Convert.ToInt32(InfoEntryManager.Instance[(int)e.Item.Tag].SEASON_NR));
                        }
                    }
                    else {
                        numEdit.Minimum = 0;
                        numEdit.Maximum = 10000;
                    }
                    lstFiles.StartEditing(numEdit, e.Item, e.SubItem);
                }
            }
        }

        //End editing a value, apply possible changes and process them
        private void lstFiles_SubItemEndEditing(object sender, ListViewEx.SubItemEndEditingEventArgs e) {
            string dirArgument = Helper.ReadProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY);
            bool CREATE_DIRECTORY_STRUCTURE_KEY = Helper.ReadInt(ConfigKeyConstants.CREATE_DIRECTORY_STRUCTURE_KEY) == 1;
            bool UseSeasonSubdirs = Helper.ReadInt(ConfigKeyConstants.CREATE_SEASON_SUBFOLDERS_KEY) == 1;
            int tmp = -1;
            Candidate ie = InfoEntryManager.Instance.GetByListViewItem(e.Item);
            //add lots of stuff here
            switch (e.SubItem) {
                //season
                case 2:
                    try{
                        tmp = Int32.Parse(e.DisplayText);
                    }
                    catch (Exception ex){
                        Logger.Instance.LogMessage("Cannot parse '" + e.DisplayText + "' to an integer", LogLevel.WARNING);
                    }
                    ie.SEASON_NR = tmp;
                    if (e.DisplayText == "") {
                        ie.Movie = true;
                    }
                    else {
                        ie.Movie = false;
                    }
                    //SetupRelation((int)e.Item.Tag);
                    //foreach (Candidate ie in InfoEntryManager.Episodes)
                    //{
                    //    SetDestinationPath(ie, dirArgument, CREATE_DIRECTORY_STRUCTURE_KEY, UseSeasonSubdirs);
                    //}
                    break;
                //EPISODE_NR
                case 3:
                    try {
                        tmp = Int32.Parse(e.DisplayText);
                    }
                    catch (Exception ex) {
                        Logger.Instance.LogMessage("Cannot parse '" + e.DisplayText + "' to an integer", LogLevel.WARNING);
                    }
                    ie.EPISODE_NR = tmp;
                    if (e.DisplayText == "") {
                        ie.Movie = true;
                    }
                    else {
                        ie.Movie = false;
                    }
                    //SetupRelation((int)e.Item.Tag);                    
                    //SetDestinationPath(ie, dirArgument, CREATE_DIRECTORY_STRUCTURE_KEY, UseSeasonSubdirs);
                    break;
                //name
                case 4:
                    //backtrack to see if entered text matches a season/episode
                    RelationCollection rc = RelationManager.Instance.GetRelationCollection(ie.Showname);
                    if (rc != null) {
                        foreach (EpisodeData rel in rc) {
                            //if found, set season and episode in gui and sync back to data
                            if (e.DisplayText == rel.PROVIDER_NAME_KEY) {
                                e.Item.SubItems[2].Text = rel.SEASON_NR.ToString();
                                e.Item.SubItems[3].Text = rel.EPISODE_NR.ToString();
                            }
                        }
                    }
                    ie.PROVIDER_NAME_KEY = e.DisplayText;
                    break;
                //Filename
                case 5:
                    ie.NewFileName = e.DisplayText;
                    if (ie.NewFileName != e.DisplayText)
                    {
                        e.Cancel = true;
                        Logger.Instance.LogMessage("Changed entered Text from " + e.DisplayText + " to " + ie.NewFileName + " because of invalid configurationFilePath characters.", LogLevel.INFO);
                    }
                    break;
                //Destination
                case 6:
                    try {
                        Path.GetDirectoryName(e.DisplayText);
                        ie.Destination = e.DisplayText;
                        if (ie.Destination != e.DisplayText)
                        {
                            e.Cancel = true;
                            Logger.Instance.LogMessage("Changed entered Text from " + e.DisplayText + " to " + ie.Destination + " because of invalid path characters.", LogLevel.INFO);
                        }
                    }
                    catch (Exception) {
                        e.Cancel = true;
                    }
                    break;
                case 7:                   
                    ie.Showname = e.DisplayText;
                    //Cancel editing since we want to set the subitem manually in SyncItem to a different value
                    if (ie.Showname == "")
                    {
                        e.Cancel = true;
                    }
                    break;
                default:
                    throw new Exception("Unreachable code");
            }
            SyncItem((int)e.Item.Tag, false);
        }

        //Double click = Invert process flag
        private void lstFiles_DoubleClick(object sender, EventArgs e) {
            if (lstFiles.SelectedIndices.Count > 0)
            {
                lstFiles.Items[lstFiles.SelectedIndices[0]].Checked = !lstFiles.Items[lstFiles.SelectedIndices[0]].Checked;
            }
        }*/

        #region GUI-Events
        //Main Initialization
        private void MainForm_Load(object sender, EventArgs e) {
            settings = Settings.Instance;
            this.initMyLoggers();

            // Init logging here:

            enableDragAndDrop();

            //and read a value to make sure it is loaded into memory
            Helper.ReadProperty(ConfigKeyConstants.LETTER_CASE_STRATEGY_KEY);

            txtTargetFilenamePattern.Text = Helper.ReadProperty(ConfigKeyConstants.TARGET_FILENAME_PATTERN_KEY);

            initSubtitleProviders();

            string lastUsedDirectory = Helper.ReadProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY);
            string updatedDirectory = updateLastDirectory();
            if (!String.IsNullOrEmpty(updatedDirectory)) {
                lastUsedDirectory = updatedDirectory;
            }

            initCandidates();

            if (lastUsedDirectory != null && lastUsedDirectory != "" && Directory.Exists(lastUsedDirectory)) {
                txtCurrentFolderPath.Text = lastUsedDirectory;
                Environment.CurrentDirectory = lastUsedDirectory;
                updateCandidateFiles(true);
            }

            restoreVisibleColumns();
            restoreColumns();
            restoreWindowSize();

        }

        private void enableDragAndDrop() {
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(MainForm_DragEnter);
            this.DragDrop += new DragEventHandler(MainForm_DragDrop);
        }

        private void initSubtitleProviders() {
            cbSubtitleProviders.Items.AddRange(SubtitleProvider.ProviderNames);
            string LastSubProvider = Helper.ReadProperty(ConfigKeyConstants.LAST_SELECTED_SUBTITLE_PROVIDER_KEY);
            if (LastSubProvider == null)
                LastSubProvider = "";
            cbSubtitleProviders.SelectedIndex = Math.Max(-1, cbSubtitleProviders.Items.IndexOf(LastSubProvider));
            Helper.WriteProperty(ConfigKeyConstants.LAST_SELECTED_SUBTITLE_PROVIDER_KEY, cbSubtitleProviders.Text);
        }

        private string updateLastDirectory() {
            //First argument=folder
            if (args.Count > 0) {
                string dirArgument = args[0].Replace("\"", "");
                if (Directory.Exists(args[0])) {
                    Helper.WriteProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY, dirArgument);
                    return dirArgument;
                }
            }
            return "";
        }

        private void restoreVisibleColumns() {
            string[] visibleColumns = Helper.ReadProperties(ConfigKeyConstants.VISIBLE_COLUMNS_KEY);
            for (int i = 0; i < visibleColumns.Length; i++) {
                string str = visibleColumns[i];
                try {
                    int visible = 1;
                    Int32.TryParse(str, out visible);
                    if (visible == 0) {
                        ((OLVColumn)lstCandidateFiles.AllColumns[i]).IsVisible = false;
                    }
                } catch (Exception) { };
            }
            lstCandidateFiles.RebuildColumns();
        }

        private void restoreColumns() {
            string[] ColumnWidths = Helper.ReadProperties(ConfigKeyConstants.MAIN_SCREEN_COLUMN_WIDTH_KEY);
            string[] ColumnOrder = Helper.ReadProperties(ConfigKeyConstants.MAIN_SCREEN_COLUMN_ORDER_KEY);
            for (int i = 0; i < lstCandidateFiles.Columns.Count; i++) {
                try {
                    int width = lstCandidateFiles.Columns[i].Width;
                    Int32.TryParse(ColumnWidths[i], out width);
                    lstCandidateFiles.Columns[i].Width = width;
                } catch (Exception) {
                    Logger.Instance.LogMessage("Invalid Value for ColumnWidths[" + i + "]", LogLevel.ERROR);
                }
                try {
                    int order = lstCandidateFiles.Columns[i].DisplayIndex;
                    Int32.TryParse(ColumnOrder[i], out order);
                    lstCandidateFiles.Columns[i].DisplayIndex = order;
                } catch (Exception) {
                    Logger.Instance.LogMessage("Invalid Value for ColumnOrder[" + i + "]", LogLevel.ERROR);
                }
            }
        }

        private void restoreWindowSize() {
            string[] Windowsize = Helper.ReadProperties(ConfigKeyConstants.WINDOW_SIZE_KEY);
            if (Windowsize.GetLength(0) >= 2) {
                try {
                    int w, h;
                    Int32.TryParse(Windowsize[0], out w);
                    Int32.TryParse(Windowsize[1], out h);
                    this.Width = w;
                    this.Height = h;
                } catch (Exception) {
                    Logger.Instance.LogMessage("Couldn't process WindowSize property: " + Helper.ReadProperty(ConfigKeyConstants.WINDOW_SIZE_KEY), LogLevel.ERROR);
                }
                //focus fix
                txtCurrentFolderPath.Focus();
                txtCurrentFolderPath.Select(txtCurrentFolderPath.Text.Length, 0);
            }
        }

		private void onlyAcceptDraggingFiles(DragEventArgs e) {
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
			} else {
				e.Effect = DragDropEffects.None;
			}
		}

        private void MainForm_DragEnter(object sender, DragEventArgs e) {
			onlyAcceptDraggingFiles(e);
        }

		private string extractNewCurrentDirectory(List<string> filePaths) {
			if (filePaths.Count == 0)
				return "";

			if (filePaths.Count == 1 && Directory.Exists(filePaths[0])) {
				return filePaths[0];
			} else {
				return firstParentDirectory(filePaths);
			}
		}

		private string firstParentDirectory(List<string> filePaths) {
			foreach (string filePath in filePaths) {
				if (filePath.LastIndexOf('\\') > 0) {
					return filePath.Substring(0, filePaths[0].LastIndexOf('\\'));
				} else {
					return filePath;
				}
			}
			return "";
		}

        // dropping an item on the form
        private void MainForm_DragDrop(object sender, DragEventArgs e) {
			changeCurrentFolderToDraggedItem(e);
        }

		private void changeCurrentFolderToDraggedItem(DragEventArgs e) {
			string[] draggedData = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			List<string> draggedFilePaths = new List<string>(draggedData);
			string newCurrentPath = extractNewCurrentDirectory(draggedFilePaths);
			if (!String.IsNullOrEmpty(newCurrentPath)) {
				checkAndChangeCurrentPath(newCurrentPath);
			}
		}

        //Auto column resize by storing column width ratios at resize start
        private void MainForm_ResizeBegin(object sender, EventArgs e) {
            if (Helper.ReadBool(ConfigKeyConstants.AUTO_RESIZE_COLUMNS_KEY)) {
                StoreColumnRatios();
            }
        }

        private void StoreColumnRatios() {
            columnWidths.Clear();
            float columnWidthSum = 0;
            foreach (OLVColumn olvc in lstCandidateFiles.Columns) {
                float widthRatio = (float)(olvc.Width) / (float)(lstCandidateFiles.ClientRectangle.Width);
                columnWidths.Add(widthRatio);
                columnWidthSum += widthRatio;
            }
            //some numeric correction to make ratios:
            for (int i = 0; i < lstCandidateFiles.Columns.Count; i++) {
                columnWidths[i] *= (float)1 / columnWidthSum;
            }
        }

        //Auto column resize, restore Column width ratios during resize
        private void MainForm_Resize(object sender, EventArgs e) {
            if (this.Visible) {
                if (Helper.ReadBool(ConfigKeyConstants.AUTO_RESIZE_COLUMNS_KEY)) {
                    autoResizeColumns();
                }
            }
        }

        private void autoResizeColumns() {
            if (lstCandidateFiles != null && lstCandidateFiles.Columns.Count > 0 && columnWidths != null) {
                for (int i = 0; i < lstCandidateFiles.Columns.Count; i++) {
                    lstCandidateFiles.Columns[i].Width = (int)(columnWidths[i] * (float)(lstCandidateFiles.ClientRectangle.Width));
                }
            }
        }

        //Auto column resize, restore Column width ratios at resize end (to make sure!)
        private void MainForm_ResizeEnd(object sender, EventArgs e) {
            if (Helper.ReadBool(ConfigKeyConstants.AUTO_RESIZE_COLUMNS_KEY)) {
                autoResizeColumns();
            }
        }

        //Save last focussed control so it can be restored after splitter between file and log window is moved
        private void scMainLogSplit_MouseDown(object sender, MouseEventArgs e) {
			storeLastFocusedControl();
        }

		private void storeLastFocusedControl() {
			lastFocusedControl = getFocused(this.Controls);
		}

		private void restoreFocusToLastFocusedControl() {
			if (lastFocusedControl != null) {
				lastFocusedControl.Focus();
				lastFocusedControl = null;
			}
		}
        //restore last focussed control so splitter doesn't keep focus
        private void scMainLogSplit_MouseUp(object sender, MouseEventArgs e) {
			restoreFocusToLastFocusedControl();
        }

        private void cbProviders_SelectedIndexChanged(object sender, EventArgs e) {
			string selectedTitleProvider = cbProviders.SelectedItem.ToString();
			updateTitleProviderSettings(selectedTitleProvider);
        }

		private void updateTitleProviderSettings(string selectedTitleProvider) {
			Helper.WriteProperty(ConfigKeyConstants.LAST_USED_TV_SHOW_PROVIDER_KEY, selectedTitleProvider);
		}

        private void cbSubs_SelectedIndexChanged(object sender, EventArgs e) {
			string selectedSubtitleProvider = cbSubtitleProviders.SelectedItem.ToString();
			updateSubtitleProviderSettings(selectedSubtitleProvider);
            
        }

		private void updateSubtitleProviderSettings(string selectedSubtitleProvider) {
			Helper.WriteProperty(ConfigKeyConstants.LAST_SELECTED_SUBTITLE_PROVIDER_KEY, selectedSubtitleProvider);
		}

        private void btnAboutApplication_Click(object sender, EventArgs e) {
			showAboutBox();
        }

		private void showAboutBox() {
			AboutBox ab = new AboutBox();
			ab.AppMoreInfo += Environment.NewLine;
			ab.AppMoreInfo += "Using:" + Environment.NewLine;
			ab.AppMoreInfo += "ObjectListView " + "http://www.codeproject.com/KB/list/ObjectListView.aspx" + Environment.NewLine;
			ab.AppMoreInfo += "About Box " + "http://www.codeproject.com/KB/vb/aboutbox.aspx" + Environment.NewLine;
			ab.AppMoreInfo += "Unrar.dll " + "http://www.rarlab.com" + Environment.NewLine;
			ab.AppMoreInfo += "SharpZipLib " + "http://www.icsharpcode.net/OpenSource/SharpZipLib/" + Environment.NewLine;
			ab.ShowDialog(this);
		}
        //Open link in browser when clicked in log
        private void rtbLog_LinkClicked(object sender, LinkClickedEventArgs e) {
			openLinkInBrowser(e.LinkText);
        }

		private void openLinkInBrowser(string url) {
			try {
				Process urlOpened = Process.Start(url);
			} catch (Exception ex) {
				Logger.Instance.LogMessage("Couldn't open " + url + ":" + ex.Message, LogLevel.ERROR);
			}			
		}

        /*//Update Destination paths when episode of show is changed
        private void cbTitle_TextChanged(object sender, EventArgs e)
        {
                //when nothing is selected, assume there is no autocompletion taking place right now
                if (cbTitle.SelectedText == "" && cbTitle.Text != "")
                {
                        for (int i = 0; i < cbTitle.Items.Count; i++)
                        {
                                if (((string)cbTitle.Items[i]).ToLower() == cbTitle.Text.ToLower())
                                {
                                        bool found = false;
                                        foreach (object o in cbTitle.Items)
                                        {
                                                if ((string)o == cbTitle.Text)
                                                {
                                                        found = true;
                                                        break;
                                                }
                                        }
                                        if (!found)
                                        {
                                                cbTitle.Items.Add(cbTitle.Text);
                                        }
                                        break;
                                }
                        }
                }
        }*/


        //Show File Open dialog and update file list
        private void btnChangeCurrentFolder_Click(object sender, EventArgs e) {
			string selectedFolderPath = selectFolderPath(fbdChangeCurrentFolder);
			checkAndChangeCurrentPath(selectedFolderPath);
        }

		private string selectFolderPath(FolderBrowserDialog someFolderBrowser) {
			if (settings.IsMonoCompatibilityMode) {
				someFolderBrowser.SelectedPath = Environment.CurrentDirectory;
			}
			DialogResult result = someFolderBrowser.ShowDialog();
			if (result == DialogResult.OK) {
				return someFolderBrowser.SelectedPath;
			} else {
				return "";
			}
		}

        public void initMyLoggers() {
            Logger logger = Logger.Instance;
            logger.removeAllLoggers();
            logger.addLogger(new FileLogger(Helper.GetLogfilePath(), true, Helper.ReadEnum<LogLevel>(ConfigKeyConstants.LOGFILE_LOGLEVEL_KEY)));
            logger.addLogger(new MessageBoxLogger(Helper.ReadEnum<LogLevel>(ConfigKeyConstants.MESSAGEBOX_LOGLEVEL_KEY)));

            //mono compatibility fixes
            if (Type.GetType("Mono.Runtime") != null) {
                logger.addLogger(new TextBoxLogger(txtBasicLog, Helper.ReadEnum<LogLevel>(ConfigKeyConstants.TEXTBOX_LOGLEVEL_KEY)));
                rtbEnhancedLog.Visible = false;
                txtBasicLog.Visible = true;
                Logger.Instance.LogMessage("Running on Mono", LogLevel.INFO);
            } else {
                rtbEnhancedLog.Text = "";
                logger.addLogger(new RichTextBoxLogger(rtbEnhancedLog, Helper.ReadEnum<LogLevel>(ConfigKeyConstants.TEXTBOX_LOGLEVEL_KEY)));
            }
        }

        //Show configuration dialog
        private void btnOpenConfiguration_Click(object sender, EventArgs e) {
			showConfigurationDialog();
        }

		private void showConfigurationDialog() {
			ConfigurationDialog cfgDialog = new ConfigurationDialog();
			if (cfgDialog.ShowDialog() == DialogResult.OK) {
				updateCandidateFiles(true);
			}
		}
        //Fetch all episode information etc yada yada yada blalblabla
        private void btnSearchForTitles_Click(object sender, EventArgs e) {
			startDownloadTitlesTask();
        }

		private void startDownloadTitlesTask() {
			WorkerArguments downloadTitlesTask = new WorkerArguments();
			downloadTitlesTask.scheduledTask = MainTask.DownloadData;
			setBusyGUI();
			backgroundTaskWorker.RunWorkerAsync(downloadTitlesTask);
		}

        /*//Enter = Click "Get Titles" button
        private void cbTitle_KeyDown(object sender, KeyEventArgs e)
        {
                if (e.KeyCode == Keys.Enter)
                {
                        setNewTitle(cbTitle.Text);
                        btnSearchForTitles.PerformClick();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                        cbTitle.Text = Helper.ReadProperties(ConfigKeyConstants.LAST_SEARCHED_SERIES_TITLES_KEY)[0];
                        cbTitle.SelectionStart = cbTitle.Text.Length;
                }
        }*/

        //Enter = Change current directory
        private void txtCurrentFolderPath_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
				checkAndChangeCurrentPath(txtCurrentFolderPath.Text);
                //need to update the displayed path because it might be changed (tailing backlashes are removed, and added for drives ("C:\" instead of "C:"))
            } else if (e.KeyCode == Keys.Escape) {
                resetToLastValidPath();
            }
        }

        private void checkAndChangeCurrentPath(string newPath) {
			if (Directory.Exists(newPath)) {
				MediaFileManager.Instance.setPath(ref newPath);
				txtCurrentFolderPath.Text = newPath;
                txtCurrentFolderPath.SelectionStart = txtCurrentFolderPath.Text.Length;
                updateCandidateFiles(true);
            } else {
                resetToLastValidPath();
            }
        }

        private void resetToLastValidPath() {
            txtCurrentFolderPath.Text = Helper.ReadProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY);
            txtCurrentFolderPath.SelectionStart = txtCurrentFolderPath.Text.Length;
        }

        private void txtCurrentFolderPath_Leave(object sender, EventArgs e) {
			checkAndChangeCurrentPath(txtCurrentFolderPath.Text);
        }

        private void updateGUI() {
            //also update some gui elements for the sake of it
            bool foundCandidates = (MediaFileManager.Instance.CountMediaFiles > 0);

            bool searchStringFound = false;
            bool candidateIsShow = false;
            if (foundCandidates) {

                foreach (MediaFile ie in MediaFileManager.Instance) {
                    if (!string.IsNullOrEmpty(ie.Showname)) {
                        searchStringFound = true;
                        if (!ie.IsMovie) {
                            candidateIsShow = true;
                            break;
                        }
                    }
                }
            }
            btnSearchForTitles.Enabled = searchStringFound;
            btnRenameSelectedCandidates.Enabled = candidateIsShow;


            bool hasVideoCandidates = false;
            if (foundCandidates) {
                foreach (MediaFile someCandidate in MediaFileManager.Instance) {
                    if (someCandidate.IsVideofile && !string.IsNullOrEmpty(someCandidate.Showname)) {
                        hasVideoCandidates = true;
                        break;
                    }
                }
            }
            btnSearchForSubtitles.Enabled = hasVideoCandidates;

            lstCandidateFiles.Refresh();
        }

        //Start renaming
        private void btnRenameSelectedCandidates_Click(object sender, EventArgs e) {
            renameSelectedCandidates();
        }

        //Focus lost = store desired pattern and update names
        private void txtTarget_Leave(object sender, EventArgs e) {
            if (txtTargetFilenamePattern.Text != Helper.ReadProperty(ConfigKeyConstants.TARGET_FILENAME_PATTERN_KEY)) {
                Helper.WriteProperty(ConfigKeyConstants.TARGET_FILENAME_PATTERN_KEY, txtTargetFilenamePattern.Text);
                MediaFileManager.Instance.createNewNames();
                updateCandidateFiles();
            }
        }

        //Enter = store desired pattern and update names
        private void txtTarget_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                if (txtTargetFilenamePattern.Text != Helper.ReadProperty(ConfigKeyConstants.TARGET_FILENAME_PATTERN_KEY)) {
                    Helper.WriteProperty(ConfigKeyConstants.TARGET_FILENAME_PATTERN_KEY, txtTargetFilenamePattern.Text);
                    MediaFileManager.Instance.createNewNames();
                    updateCandidateFiles();
                }
            } else if (e.KeyCode == Keys.Escape) {
                txtTargetFilenamePattern.Text = Helper.ReadProperty(ConfigKeyConstants.TARGET_FILENAME_PATTERN_KEY);
                txtTargetFilenamePattern.SelectionStart = txtTargetFilenamePattern.Text.Length;
            } else if ((e.KeyCode == Keys.Back) && e.Control) {
                e.SuppressKeyPress = true;
                int selStart = txtTargetFilenamePattern.SelectionStart;
                while (selStart > 0 && txtTargetFilenamePattern.Text.Substring(selStart - 1, 1) == " ") {
                    selStart--;
                }
                int prevSpacePos = -1;
                if (selStart != 0) {
                    prevSpacePos = txtTargetFilenamePattern.Text.LastIndexOf(' ', selStart - 1);
                }
                txtTargetFilenamePattern.Select(prevSpacePos + 1, txtTargetFilenamePattern.SelectionStart - prevSpacePos - 1);
                txtTargetFilenamePattern.SelectedText = "";
            }
        }

        //start fetching subtitles
        private void btnSearchForSubtitles_Click(object sender, EventArgs e) {
            DataGenerator.GetSubtitles();
        }

        private void saveColumnSettings() {
            string[] columnWidths = new string[lstCandidateFiles.Columns.Count];
            string[] columnOrder = new string[lstCandidateFiles.Columns.Count];
            for (int i = 0; i < lstCandidateFiles.Columns.Count; i++) {
                columnOrder[i] = lstCandidateFiles.Columns[i].DisplayIndex.ToString();
                columnWidths[i] = lstCandidateFiles.Columns[i].Width.ToString();
            }
            Helper.WriteProperties(ConfigKeyConstants.MAIN_SCREEN_COLUMN_ORDER_KEY, columnOrder);
            Helper.WriteProperties(ConfigKeyConstants.MAIN_SCREEN_COLUMN_WIDTH_KEY, columnWidths);
        }

        private void saveWindowSize() {
            string[] windowSize = new string[2];
            if (this.WindowState == FormWindowState.Normal) {
                windowSize[0] = this.Size.Width.ToString();
                windowSize[1] = this.Size.Height.ToString();
            } else {
                windowSize[0] = this.RestoreBounds.Width.ToString();
                windowSize[1] = this.RestoreBounds.Height.ToString();
            }
            Helper.WriteProperties(ConfigKeyConstants.WINDOW_SIZE_KEY, windowSize);
        }

        private void saveVisibleColumns() {
            List<string> VisibleColumns = new List<string>();
            for (int i = 0; i < lstCandidateFiles.AllColumns.Count; i++) {
                OLVColumn olvc = (OLVColumn)lstCandidateFiles.AllColumns[i];
                if (olvc.IsVisible) {
                    VisibleColumns.Add("1");
                } else {
                    VisibleColumns.Add("0");
                }
            }
            Helper.WriteProperties(ConfigKeyConstants.VISIBLE_COLUMNS_KEY, VisibleColumns.ToArray());
        }

        private void saveConfigurationFiles() {
            foreach (DictionaryEntry dict in settings) {
                ((ConfigFile)dict.Value).Flush();
            }
        }

        //Cleanup, save some stuff etc
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            Helper.WriteProperty(ConfigKeyConstants.LAST_SELECTED_SUBTITLE_PROVIDER_KEY, cbSubtitleProviders.SelectedItem.ToString());

            saveColumnSettings();
            saveWindowSize();
            saveVisibleColumns();

            //Also flush values stored directly in TitleProvider classd
            TitleProvider.saveQueryResults();

            saveConfigurationFiles();
        }
        #endregion
        #region Contextmenu
        //Set which context menu options should be available
        private void contextFiles_Opening(object sender, CancelEventArgs e) {
            editSubtitleToolStripMenuItem.Visible = false;
            viewToolStripMenuItem.Visible = false;
            renamingToolStripMenuItem.Visible = false;
            markAsMovieToolStripMenuItem.Visible = true;
            markAsTVSeriesToolStripMenuItem.Visible = true;
            selectToolStripMenuItem.Visible = true;
            checkToolStripMenuItem.Visible = true;
            renamingToolStripMenuItem.Visible = true;
            setDestinationToolStripMenuItem.Visible = true;
            setEpisodesFromtoToolStripMenuItem.Visible = true;
            setSeasonToolStripMenuItem.Visible = true;
            setShownameToolStripMenuItem.Visible = true;
            removeToolStripMenuItem.Visible = true;
            deleteToolStripMenuItem.Visible = true;
            toolStripSeparator2.Visible = true;
            toolStripSeparator3.Visible = true;
            toolStripSeparator4.Visible = true;
            lookUpOnIMDBToolStripMenuItem.Visible = false;
            if (lstCandidateFiles.SelectedIndices.Count == 0) {
                markAsMovieToolStripMenuItem.Visible = false;
                markAsTVSeriesToolStripMenuItem.Visible = false;
                selectToolStripMenuItem.Visible = false;
                checkToolStripMenuItem.Visible = false;
                renamingToolStripMenuItem.Visible = false;
                setDestinationToolStripMenuItem.Visible = false;
                setEpisodesFromtoToolStripMenuItem.Visible = false;
                setSeasonToolStripMenuItem.Visible = false;
                setShownameToolStripMenuItem.Visible = false;
                removeToolStripMenuItem.Visible = false;
                deleteToolStripMenuItem.Visible = false;
                toolStripSeparator2.Visible = false;
                toolStripSeparator3.Visible = false;
                toolStripSeparator4.Visible = false;
            }
            if (lstCandidateFiles.SelectedIndices.Count == 1) {
                lookUpOnIMDBToolStripMenuItem.Visible = true;
                //if selected file is a subtitle
                List<string> subext = new List<string>(Helper.ReadProperties(ConfigKeyConstants.SUBTITLE_FILE_EXTENSIONS_KEY));
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[0]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                if (subext.Contains(ie.Extension.ToLower())) {
                    editSubtitleToolStripMenuItem.Visible = true;
                }

                //if selected file is a video and there is a matching subtitle
                if (MediaFileManager.Instance.GetSubtitleFile(ie) != null) {
                    editSubtitleToolStripMenuItem.Visible = true;
                }

                //if there is a matching video
                if (MediaFileManager.Instance.GetVideo(ie) != null) {
                    viewToolStripMenuItem.Visible = true;
                }
            }
            if (lstCandidateFiles.SelectedIndices.Count > 0) {
                renamingToolStripMenuItem.Visible = true;
                createDirectoryStructureToolStripMenuItem1.Checked = false;
                dontCreateDirectoryStructureToolStripMenuItem.Checked = false;
                largeToolStripMenuItem.Checked = false;
                smallToolStripMenuItem.Checked = false;
                igNorEToolStripMenuItem.Checked = false;
                cAPSLOCKToolStripMenuItem.Checked = false;
                useUmlautsToolStripMenuItem.Checked = false;
                dontUseUmlautsToolStripMenuItem.Checked = false;
                useProvidedNamesToolStripMenuItem.Checked = false;
                copyToolStripMenuItem.Visible = true;
                bool OldPath = false;
                bool OldFilename = false;
                bool Name = false;
                bool Destination = false;
                bool NewFilename = false;
                bool MoviesOnly = true;
                bool TVShowsOnly = true;
                MediaFile.DirectoryStructureAction CreateDirectoryStructure = MediaFile.DirectoryStructureAction.Unset;
                MediaFile.CaseAction Case = MediaFile.CaseAction.Unset;
                MediaFile.UmlautAction Umlaute = MediaFile.UmlautAction.Unset;
                for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                    OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                    MediaFile ie = (MediaFile)lvi.RowObject;
                    if (ie.IsMovie) TVShowsOnly = false;
                    else MoviesOnly = false;
                    if (i == 0) {
                        CreateDirectoryStructure = ie.CreateDirectoryStructure;
                        Case = ie.Casing;
                        Umlaute = ie.UmlautUsage;
                    } else {
                        if (CreateDirectoryStructure != ie.CreateDirectoryStructure) {
                            CreateDirectoryStructure = MediaFile.DirectoryStructureAction.Unset;
                        }
                        if (Case != ie.Casing) {
                            Case = MediaFile.CaseAction.Unset;
                        }
                        if (Umlaute != ie.UmlautUsage) {
                            Umlaute = MediaFile.UmlautAction.Unset;
                        }
                    }
                }
                if (TVShowsOnly) {
                    markAsMovieToolStripMenuItem.Visible = true;
                    markAsTVSeriesToolStripMenuItem.Visible = false;
                } else if (MoviesOnly) {
                    markAsTVSeriesToolStripMenuItem.Visible = true;
                    markAsMovieToolStripMenuItem.Visible = false;
                } else {
                    markAsMovieToolStripMenuItem.Visible = true;
                    markAsTVSeriesToolStripMenuItem.Visible = true;
                }
                if (CreateDirectoryStructure == MediaFile.DirectoryStructureAction.CreateDirectoryStructure) {
                    createDirectoryStructureToolStripMenuItem1.Checked = true;
                } else if (CreateDirectoryStructure == MediaFile.DirectoryStructureAction.NoDirectoryStructure) {
                    dontCreateDirectoryStructureToolStripMenuItem.Checked = true;
                }
                if (Case == MediaFile.CaseAction.UpperFirst) {
                    largeToolStripMenuItem.Checked = true;
                } else if (Case == MediaFile.CaseAction.small) {
                    smallToolStripMenuItem.Checked = true;
                } else if (Case == MediaFile.CaseAction.Ignore) {
                    igNorEToolStripMenuItem.Checked = true;
                } else if (Case == MediaFile.CaseAction.CAPSLOCK) {
                    cAPSLOCKToolStripMenuItem.Checked = true;
                }
                if (Umlaute == MediaFile.UmlautAction.Use) {
                    useUmlautsToolStripMenuItem.Checked = true;
                } else if (Umlaute == MediaFile.UmlautAction.Dont_Use) {
                    dontUseUmlautsToolStripMenuItem.Checked = true;
                } else if (Umlaute == MediaFile.UmlautAction.Ignore) {
                    useProvidedNamesToolStripMenuItem.Checked = true;
                }
                for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                    OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                    MediaFile ie = (MediaFile)lvi.RowObject;
                    if (ie.Filename != "")
                        OldFilename = true;
                    if (ie.FilePath.Path != "")
                        OldPath = true;
                    if (ie.EpisodeTitle != "")
                        Name = true;
                    if (ie.DestinationPath != "")
                        Destination = true;
                    if (ie.NewFilename != "")
                        NewFilename = true;
                }
                originalNameToolStripMenuItem.Visible = OldFilename;
                pathOrigNameToolStripMenuItem.Visible = OldPath && OldFilename;
                titleToolStripMenuItem.Visible = Name;
                newFileNameToolStripMenuItem.Visible = NewFilename;
                destinationNewFileNameToolStripMenuItem.Visible = Destination && NewFilename;
            } else {
                copyToolStripMenuItem.Visible = false;
            }
        }

        //Select all list items
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            lstCandidateFiles.SelectedIndices.Clear();
            for (int i = 0; i < lstCandidateFiles.Items.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[i];
                lstCandidateFiles.SelectedIndices.Add(lvi.Index);
            }
        }

        //Invert file list selection
        private void invertSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.Items.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[i];
                if (lstCandidateFiles.SelectedIndices.Contains(lvi.Index)) {
                    lstCandidateFiles.SelectedIndices.Remove(lvi.Index);
                } else {
                    lstCandidateFiles.SelectedIndices.Add(lvi.Index);
                }
            }
        }

        //Check all list boxes
        private void checkAllToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (MediaFile ie in MediaFileManager.Instance) {
                ie.ProcessingRequested = true;
            }
            lstCandidateFiles.Refresh();
        }

        //Uncheck all list boxes
        private void uncheckAllToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (MediaFile ie in MediaFileManager.Instance) {
                ie.ProcessingRequested = false;
            }
            lstCandidateFiles.Refresh();
        }

        //Invert check status of Selected list boxes
        private void invertCheckToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (MediaFile ie in MediaFileManager.Instance) {
                ie.ProcessingRequested = !ie.ProcessingRequested;
            }
            lstCandidateFiles.Refresh();
        }

        //Filter function to select files by keyword
        private void selectByKeywordToolStripMenuItem_Click(object sender, EventArgs e) {
            Filter f = new Filter("");
            if (f.ShowDialog() == DialogResult.OK) {
                lstCandidateFiles.SelectedIndices.Clear();
                for (int i = 0; i < lstCandidateFiles.Items.Count; i++) {
                    OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[i];
                    if (lvi.Text.ToLower().Contains(f.result.ToLower())) {
                        lstCandidateFiles.SelectedIndices.Add(lvi.Index);
                    }
                }
            }

        }

        //Set season property for selected items
        private void setSeasonToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstCandidateFiles.SelectedIndices.Count == 1) {
                lstCandidateFiles.EditSubItem((OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[0]], 3);
            } else if (lstCandidateFiles.SelectedIndices.Count > 1) {
                //yes we are smart and guess the season from existing ones
                int sum = 0;
                int count = 0;
                for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                    OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                    MediaFile ie = (MediaFile)lvi.RowObject;
                    if (ie.SeasonNr != -1) {
                        sum += ie.SeasonNr;
                        count++;
                    }
                }
                int EstimatedSeason = 1;
                if (count > 0) {
                    EstimatedSeason = (int)Math.Round(((float)sum / (float)count));
                }
                EnterSeason es = new EnterSeason(EstimatedSeason);
                if (es.ShowDialog() == DialogResult.OK) {
                    string basepath = Helper.ReadProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY);
                    bool createdirectorystructure = (Helper.ReadInt(ConfigKeyConstants.CREATE_DIRECTORY_STRUCTURE_KEY) > 0);
                    bool UseSeasonDir = (Helper.ReadInt(ConfigKeyConstants.CREATE_SEASON_SUBFOLDERS_KEY) > 0);
                    for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                        OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                        MediaFile ie = (MediaFile)lvi.RowObject;
                        ie.SeasonNr = es.season;
                        if (ie.DestinationPath != "") {
                            ie.ProcessingRequested = true;
                        }
                    }
                    updateGUI();
                }
            }
        }

        //Set episodes for selected items to a range
        private void setEpisodesFromtoToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstCandidateFiles.SelectedIndices.Count == 1) {
                lstCandidateFiles.EditSubItem((OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[0]], 4);
            } else if (lstCandidateFiles.SelectedIndices.Count > 1) {
                // Here the episode nr of the first of the selected items is used to preset the dialog
                // TODO: It would be better if there was a preference setting where you could decide for yourself
                // which method to use (first item, always 1, lowest episode nr,...)
                OLVListItem firstEpisodeEntry = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[0]];
                MediaFile firstEpisodeInfo = (MediaFile)firstEpisodeEntry.RowObject;
                SetEpisodes se = new SetEpisodes(lstCandidateFiles.SelectedIndices.Count, firstEpisodeInfo.EpisodeNr);

                if (se.ShowDialog() == DialogResult.OK) {
                    for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                        OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                        MediaFile ie = (MediaFile)lvi.RowObject;
                        ie.EpisodeNr = (i + se.From);
                    }
                    updateGUI();
                }
            }
        }

        //Refresh file list
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e) {
            updateCandidateFiles(true);
        }

        //Remove from list
        private void removeToolStripMenuItem_Click(object sender, EventArgs e) {
            List<MediaFile> lie = new List<MediaFile>();
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                lie.Add(ie);
            }
            foreach (MediaFile ie in lie) {
                MediaFileManager.Instance.remove(ie);
                lstCandidateFiles.RemoveObject(ie);
            }
            updateGUI();
        }

        //Delete file
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Delete selected files from the file system? This action cannot be undone!", "Delete selected files?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            List<MediaFile> lie = new List<MediaFile>();
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                lie.Add(ie);
            }
            foreach (MediaFile ie in lie) {
                try {
                    File.Delete(ie.FilePath.Path + Path.DirectorySeparatorChar + ie.Filename);
                    MediaFileManager.Instance.remove(ie);
                    lstCandidateFiles.RemoveObject(ie);
                } catch (Exception ex) {
                    Logger.Instance.LogMessage("Error deleting file: " + ex.Message, LogLevel.ERROR);
                }
            }
            updateGUI();
        }

        //Open file
        private void viewToolStripMenuItem_Click(object sender, EventArgs e) {
            MediaFile ie = MediaFileManager.Instance.GetVideo((MediaFile)((OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[0]]).RowObject);
            string VideoPath = ie.FilePath.Path + Path.DirectorySeparatorChar + ie.Filename;
            try {
                Process myProc = Process.Start(VideoPath);
            } catch (Exception ex) {
                Logger.Instance.LogMessage("Couldn't open " + VideoPath + ":" + ex.Message, LogLevel.ERROR);
            }
        }

        //Edit subtitle
        private void editSubtitleToolStripMenuItem_Click(object sender, EventArgs e) {
            MediaFile sub = MediaFileManager.Instance.GetSubtitleFile((MediaFile)((OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[0]]).RowObject);
            MediaFile video = MediaFileManager.Instance.GetVideo((MediaFile)((OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[0]]).RowObject);
            if (sub != null) {
                string path = sub.FilePath.Path + Path.DirectorySeparatorChar + sub.Filename;
                string videopath = "";
                if (video != null) {
                    videopath = video.FilePath.Path + Path.DirectorySeparatorChar + video.Filename;
                }
                EditSubtitles es = new EditSubtitles(path, videopath);
                es.ShowDialog();
            }
        }

        //Set Destination
        private void setDestinationToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstCandidateFiles.SelectedIndices.Count == 1) {
                lstCandidateFiles.EditSubItem((OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[0]], 6);
            } else if (lstCandidateFiles.SelectedIndices.Count > 1) {
                InputBox ib = new InputBox("Set Destination", "Set Destination directory for selected files", Helper.ReadProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY), InputBox.BrowseType.Folder, true);
                if (ib.ShowDialog(this) == DialogResult.OK) {
                    for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                        OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                        MediaFile ie = (MediaFile)lvi.RowObject;
                        string destination = ib.input;
                        ie.DestinationPath = destination;
                    }
                    updateGUI();
                }
            }
        }
        #endregion
        #region misc

        /// <summary>
        /// Fills list view control with info data
        /// </summary>
        private void updateCandidateFiles() {
            // TODO: show at least a progressbar while adding items, user can't see anything but processor utilization will be very high
            lstCandidateFiles.Items.Clear();
            lstCandidateFiles.SetObjects(null);
            lstCandidateFiles.VirtualListSize = MediaFileManager.Instance.CountMediaFiles;
            lstCandidateFiles.SetObjects(MediaFileManager.Instance);
            lstCandidateFiles.Sort();
            lstCandidateFiles.Refresh();
        }





        private void initCandidates() {
            lstCandidateFiles.RowFormatter = delegate(OLVListItem olvi) {
                //reset colors to make sure they are set properly
                olvi.BackColor = Color.White;
                olvi.ForeColor = Color.Black;
                MediaFile rowCandidate = (MediaFile)olvi.RowObject;
                if ((rowCandidate.NewFilename == "" && (rowCandidate.DestinationPath == "" || rowCandidate.DestinationPath == rowCandidate.FilePath.Path))
                    || !rowCandidate.ProcessingRequested) {
                    olvi.ForeColor = Color.Gray;
                }
                if (!rowCandidate.MarkedForDeletion) {
                    foreach (MediaFile someCandidate in MediaFileManager.Instance) {
                        if (rowCandidate != someCandidate) {
                            if (MediaFileManager.Instance.IsSameTarget(rowCandidate, someCandidate)) {
                                olvi.BackColor = Color.IndianRed;
                                break;
                                // FIXME: Was bedeutet "gelber Hintergrund"?
                            } else if (olvi.BackColor != Color.Yellow) {
                                olvi.BackColor = Color.White;
                            }
                        }
                    }
                }
            };
            //Processing
            this.lstCandidateFiles.BooleanCheckStateGetter = delegate(object x) {
                return ((MediaFile)x).ProcessingRequested;
            };
            this.lstCandidateFiles.BooleanCheckStatePutter = delegate(object x, bool newValue) {
                ((MediaFile)x).ProcessingRequested = newValue;
                bool shouldBeEnabled = newValue;
                if (!shouldBeEnabled) {
                    foreach (MediaFile someCandidate in MediaFileManager.Instance) {
                        if (someCandidate.ProcessingRequested) {
                            shouldBeEnabled = true;
                            break;
                        }
                    }
                }
                btnSearchForTitles.Enabled = shouldBeEnabled;
                btnSearchForSubtitles.Enabled = shouldBeEnabled;
                lstCandidateFiles.Refresh();
                return newValue;
            };

            //source configurationFilePath
            this.ColumnSource.AspectGetter = delegate(object x) {
                return ((MediaFile)x).Filename;
            };

            //Source path
            this.ColumnFilepath.AspectGetter = delegate(object x) {
                return ((MediaFile)x).FilePath.Path;
            };

            //Showname
            this.ColumnShowname.AspectGetter = delegate(object x) {
                return ((MediaFile)x).Showname;
            };
            this.ColumnShowname.AspectPutter = delegate(object x, object newValue) {
                ((MediaFile)x).Showname = (string)newValue;
                updateGUI();
            };

            //SEASON_NR
            this.ColumnSeason.AspectGetter = delegate(object x) {
                return ((MediaFile)x).SeasonNr;
            };
            this.ColumnSeason.AspectPutter = delegate(object x, object newValue) {
                ((MediaFile)x).SeasonNr = (int)newValue;
                updateGUI();
            };

            //EPISODE_NR
            this.ColumnEpisode.AspectGetter = delegate(object x) {
                return ((MediaFile)x).EpisodeNr;
            };
            this.ColumnEpisode.AspectPutter = delegate(object x, object newValue) {
                ((MediaFile)x).EpisodeNr = (int)newValue;
                updateGUI();
            };

            //EPISODE_TITLE
            this.ColumnEpisodeName.AspectGetter = delegate(object x) {
                return ((MediaFile)x).EpisodeTitle;
            };
            this.ColumnEpisodeName.AspectPutter = delegate(object x, object newValue) {
                ((MediaFile)x).EpisodeTitle = (string)newValue;
                //backtrack to see if entered text matches a season/episode
				TvShow titles = TvShowManager.Instance.GetTvShow(((MediaFile)x).Showname);
                if (titles != null) {
                    foreach (ShowEpisode title in titles) { 
                        if ((string)newValue == title.Title) {
                            ((MediaFile)x).SeasonNr = title.SeasonNr;
                            ((MediaFile)x).EpisodeNr = title.SeasonEpisodeNr;
                            break;
                        }
                    }
                }
                updateGUI();
            };

            //Destination
            this.ColumnDestination.AspectGetter = delegate(object x) {
                return ((MediaFile)x).DestinationPath;
            };
            this.ColumnDestination.AspectPutter = delegate(object x, object newValue) {
                ((MediaFile)x).DestinationPath = (string)newValue;
                updateGUI();
            };

            //Filename
            this.ColumnNewFilename.AspectGetter = delegate(object x) {
                return ((MediaFile)x).NewFilename;
            };
            this.ColumnNewFilename.AspectPutter = delegate(object x, object newValue) {
                ((MediaFile)x).NewFilename = (string)newValue;
                updateGUI();
            };
        }


        /// <summary>
        /// Gets focussed control
        /// </summary>
        /// <param name="controls">Array of controls in which to search for</param>
        /// <returns>focussed control or null</returns>
        private Control getFocused(Control.ControlCollection controls) {
            foreach (Control c in controls) {
                if (c.Focused) {
                    // Return the focused control
                    return c;
                } else if (c.ContainsFocus) {
                    // If the focus is contained inside a control's children
                    // return the child
                    return getFocused(c.Controls);
                }
            }
            // No control on the form has focus
            return null;
        }
        #endregion


        /*bool UsedDropDown = false;
        private void cbTitle_DropDownClosed(object sender, EventArgs e)
        {
            UsedDropDown = true;
        }

        private void cbTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UsedDropDown)
            {
                UsedDropDown = !UsedDropDown;
                setNewTitle(cbTitle.Text);
            }
        }*/

        private void originalNameToolStripMenuItem_Click(object sender, EventArgs e) {
            string clipboard = "";
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                clipboard += ie.Filename + Environment.NewLine;
            }
            clipboard = clipboard.Substring(0, Math.Max(clipboard.Length - Environment.NewLine.Length, 0));
            clipboard = clipboard.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            Clipboard.SetText(clipboard);
        }

        private void pathOrigNameToolStripMenuItem_Click(object sender, EventArgs e) {
            string clipboard = "";
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                clipboard += ie.FilePath.Path + Path.DirectorySeparatorChar + ie.Filename + Environment.NewLine;
            }
            clipboard = clipboard.Substring(0, Math.Max(clipboard.Length - Environment.NewLine.Length, 0));
            clipboard = clipboard.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            Clipboard.SetText(clipboard);
        }

        private void titleToolStripMenuItem_Click(object sender, EventArgs e) {
            string clipboard = "";
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                clipboard += ie.EpisodeTitle + Environment.NewLine;
            }
            clipboard = clipboard.Substring(0, Math.Max(clipboard.Length - Environment.NewLine.Length, 0));
            clipboard = clipboard.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            if (clipboard != "") {
                Clipboard.SetText(clipboard);
            }
        }

        private void newFileNameToolStripMenuItem_Click(object sender, EventArgs e) {
            string clipboard = "";
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                clipboard += ie.NewFilename + Environment.NewLine;
            }
            clipboard = clipboard.Substring(0, Math.Max(clipboard.Length - Environment.NewLine.Length, 0));
            clipboard = clipboard.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            Clipboard.SetText(clipboard);
        }

        private void destinationNewFileNameToolStripMenuItem_Click(object sender, EventArgs e) {
            string clipboard = "";
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                if (ie.DestinationPath != "" && ie.NewFilename != "") {
                    clipboard += ie.DestinationPath + Path.DirectorySeparatorChar + ie.NewFilename + Environment.NewLine;
                }
            }
            clipboard = clipboard.Substring(0, Math.Max(clipboard.Length - Environment.NewLine.Length, 0));
            clipboard = clipboard.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            Clipboard.SetText(clipboard);
        }

        private void operationToolStripMenuItem_Click(object sender, EventArgs e) {
            string clipboard = "";
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                bool DestinationDifferent = ie.DestinationPath != "" && ie.DestinationPath != ie.FilePath.Path;
                bool FilenameDifferent = ie.NewFilename != "" && ie.NewFilename != ie.Filename;
                if (DestinationDifferent && FilenameDifferent) {
                    clipboard += ie.Filename + " --> " + ie.DestinationPath + Path.DirectorySeparatorChar + ie.NewFilename + Environment.NewLine;
                } else if (DestinationDifferent) {
                    clipboard += ie.Filename + " --> " + ie.DestinationPath + Environment.NewLine;
                } else if (FilenameDifferent) {
                    clipboard += ie.Filename + " --> " + ie.NewFilename + Environment.NewLine;
                }
            }
            clipboard = clipboard.Substring(0, Math.Max(clipboard.Length - Environment.NewLine.Length, 0));
            clipboard = clipboard.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            if (!string.IsNullOrEmpty(clipboard)) {
                Clipboard.SetText(clipboard);
            }
        }

        private void btnExploreCurrentFolder_Click(object sender, EventArgs e) {
			openExplorer(txtCurrentFolderPath.Text);
        }

		private void openExplorer(string somePath) {
			if (Directory.Exists(somePath)) {
				Process explorerProcess = Process.Start(somePath);
			}
		}

        private void replaceInPathToolStripMenuItem_Click(object sender, EventArgs e) {
            ReplaceWindow rw = new ReplaceWindow(this);
            rw.Show();
            rw.TopMost = true;
        }

        /// <summary>
        /// Replaces strings from various fields in selected files
        /// </summary>
        /// <param name="SearchString">String to look for, may contain parameters</param>
        /// <param name="ReplaceString">CUSTOM_REPLACE_REGEX_STRINGS_KEY string, may contain parameters</param>
        /// <param name="Source">Field from which the source string is taken</param>
        public void Replace(string SearchString, string ReplaceString, string Source) {
            int count = 0;

            string destination = "Filename";
            if (Source.Contains("Path"))
                destination = "Path";
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                string title = ie.Showname;
                string source = "";

                string LocalSearchString = SearchString;
                string LocalReplaceString = ReplaceString;
                //aquire source string
                switch (Source) {
                    case "Original Filename":
                        source = ie.Filename;
                        break;
                    case "Original Path":
                        source = ie.FilePath.Path;
                        break;
                    case "Destination Filename":
                        source = ie.NewFilename;
                        break;
                    case "Destination Path":
                        source = ie.DestinationPath;
                        break;
                }
                //Insert parameter values
                LocalSearchString = LocalSearchString.Replace("%OF", ie.Filename);
                LocalSearchString = LocalSearchString.Replace("%DF", ie.NewFilename);
                LocalSearchString = LocalSearchString.Replace("%OP", ie.FilePath.Path);
                LocalSearchString = LocalSearchString.Replace("%DP", ie.DestinationPath);
                LocalSearchString = LocalSearchString.Replace("%T", title);
                LocalSearchString = LocalSearchString.Replace("%N", ie.EpisodeTitle);
                LocalSearchString = LocalSearchString.Replace("%E", ie.EpisodeNr.ToString());
                LocalSearchString = LocalSearchString.Replace("%s", ie.SeasonNr.ToString());
                LocalSearchString = LocalSearchString.Replace("%S", ie.SeasonNr.ToString("00"));
                LocalReplaceString = LocalReplaceString.Replace("%OF", ie.Filename);
                LocalReplaceString = LocalReplaceString.Replace("%DF", ie.NewFilename);
                LocalReplaceString = LocalReplaceString.Replace("%OP", ie.FilePath.Path);
                LocalReplaceString = LocalReplaceString.Replace("%DP", ie.DestinationPath);
                LocalReplaceString = LocalReplaceString.Replace("%T", title);
                LocalReplaceString = LocalReplaceString.Replace("%N", ie.EpisodeTitle);
                LocalReplaceString = LocalReplaceString.Replace("%E", ie.EpisodeNr.ToString());
                LocalReplaceString = LocalReplaceString.Replace("%s", ie.SeasonNr.ToString());
                LocalReplaceString = LocalReplaceString.Replace("%S", ie.SeasonNr.ToString("00"));

                //see if replace will be done for count var
                if (source.Contains(SearchString))
                    count++;
                //do the replace
                source = source.Replace(LocalSearchString, LocalReplaceString);
                if (destination == "Filename") {
                    ie.NewFilename = source;
                } else if (destination == "Path") {
                    ie.DestinationPath = source;
                }

                //mark files for processing
                ie.ProcessingRequested = true;
                updateGUI();
            }
            if (count > 0) {
                Logger.Instance.LogMessage(SearchString + " was replaced with " + ReplaceString + " in " + count + " fields.", LogLevel.INFO);
            } else {
                Logger.Instance.LogMessage(SearchString + " was not found in any of the selected files.", LogLevel.INFO);
            }
        }

        private void byNameToolStripMenuItem_Click(object sender, EventArgs e) {
            List<string> names = new List<string>();
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                string Showname = ie.Showname;
                if (!names.Contains(Showname)) {
                    names.Add(Showname);
                }
            }
            List<MediaFile> similar = new List<MediaFile>();
            foreach (string str in names) {
                similar.AddRange(MediaFileManager.Instance.FindSimilarByName(str));
            }
            for (int i = 0; i < lstCandidateFiles.Items.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[i];
                MediaFile ie = (MediaFile)lvi.RowObject;
                if (similar.Contains(ie)) {
                    lvi.Selected = true;
                } else {
                    lvi.Selected = false;
                }
            }
            lstCandidateFiles.Refresh();
        }

        private void byPathToolStripMenuItem_Click(object sender, EventArgs e) {
            List<string> paths = new List<string>();
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                string path = ie.FilePath.Path;
                if (!paths.Contains(path)) {
                    paths.Add(path);
                }
            }
            List<MediaFile> similar = new List<MediaFile>();
            foreach (string str in paths) {
                similar.AddRange(MediaFileManager.Instance.FindSimilarByName(str));
            }
            for (int i = 0; i < lstCandidateFiles.Items.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[i];
                MediaFile ie = (MediaFile)lvi.RowObject;
                if (similar.Contains(ie)) {
                    lvi.Selected = true;
                } else {
                    lvi.Selected = false;
                }
            }
            lstCandidateFiles.Refresh();
        }

        private void createDirectoryStructureToolStripMenuItem1_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                ie.CreateDirectoryStructure = MediaFile.DirectoryStructureAction.CreateDirectoryStructure;
            }
            ((ToolStripMenuItem)sender).Checked = true;
            dontCreateDirectoryStructureToolStripMenuItem.Checked = false;
            updateGUI();
        }

        private void dontCreateDirectoryStructureToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                ie.CreateDirectoryStructure = MediaFile.DirectoryStructureAction.NoDirectoryStructure;
            }
            ((ToolStripMenuItem)sender).Checked = true;
            createDirectoryStructureToolStripMenuItem.Checked = false;
            updateGUI();
        }

        private void useUmlautsToolStripMenuItem1_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                ie.UmlautUsage = MediaFile.UmlautAction.Use;
            }
            ((ToolStripMenuItem)sender).Checked = true;
            dontUseUmlautsToolStripMenuItem.Checked = false;
            useProvidedNamesToolStripMenuItem.Checked = false;
            updateGUI();
        }

        private void dontUseUmlautsToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                ie.UmlautUsage = MediaFile.UmlautAction.Dont_Use;
            }
            ((ToolStripMenuItem)sender).Checked = true;
            useUmlautsToolStripMenuItem.Checked = false;
            useProvidedNamesToolStripMenuItem.Checked = false;
            updateGUI();
        }

        private void useProvidedNamesToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                ie.UmlautUsage = MediaFile.UmlautAction.Ignore;
            }
            ((ToolStripMenuItem)sender).Checked = true;
            useUmlautsToolStripMenuItem.Checked = false;
            dontUseUmlautsToolStripMenuItem.Checked = false;
            updateGUI();
        }

        private void largeToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                ie.Casing = MediaFile.CaseAction.UpperFirst;
            }
            ((ToolStripMenuItem)sender).Checked = true;
            smallToolStripMenuItem.Checked = false;
            igNorEToolStripMenuItem.Checked = false;
            cAPSLOCKToolStripMenuItem.Checked = false;
            updateGUI();
        }

        private void smallToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                ie.Casing = MediaFile.CaseAction.small;
            }
            ((ToolStripMenuItem)sender).Checked = true;
            largeToolStripMenuItem.Checked = false;
            igNorEToolStripMenuItem.Checked = false;
            cAPSLOCKToolStripMenuItem.Checked = false;
            updateGUI();
        }

        private void igNorEToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                ie.Casing = MediaFile.CaseAction.Ignore;
            }
            ((ToolStripMenuItem)sender).Checked = true;
            smallToolStripMenuItem.Checked = false;
            largeToolStripMenuItem.Checked = false;
            cAPSLOCKToolStripMenuItem.Checked = false;
            updateGUI();
        }

        private void cAPSLOCKToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                MediaFile ie = (MediaFile)lvi.RowObject;
                ie.Casing = MediaFile.CaseAction.CAPSLOCK;
            }
            ((ToolStripMenuItem)sender).Checked = true;
            smallToolStripMenuItem.Checked = false;
            igNorEToolStripMenuItem.Checked = false;
            largeToolStripMenuItem.Checked = false;
            updateGUI();
        }

        private void setShownameToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstCandidateFiles.SelectedIndices.Count == 1) {
                lstCandidateFiles.EditSubItem((OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[0]], 2);
            } else if (lstCandidateFiles.SelectedIndices.Count > 1) {
                Dictionary<string, int> ht = new Dictionary<string, int>();
                for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                    OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                    MediaFile ie = (MediaFile)lvi.RowObject;
                    if (!ht.ContainsKey(ie.Showname)) {
                        ht.Add(ie.Showname, 1);
                    } else {
                        ht[ie.Showname] += 1;
                    }
                }
                int max = 0;
                string Showname = "";
                foreach (KeyValuePair<string, int> pair in ht) {
                    if (pair.Value > max) {
                        Showname = pair.Key;
                    }
                }
                EnterShowname es = new EnterShowname(Showname);
                if (es.ShowDialog() == DialogResult.OK) {
                    for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                        OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                        ((MediaFile)lvi.RowObject).Showname = es.SelectedName;
                    }
                    updateGUI();
                }
            }
        }

        private void regexTesterToolStripMenuItem_Click(object sender, EventArgs e) {
            RegexTester rt = new RegexTester();
            rt.Show();
        }



        #region Functions remaining in MainForm
        private void updateCandidateFiles(bool clear) {
            lstCandidateFiles.ClearObjects();
            setBusyGUI();
            taskProgressBar.Visible = false;
            lblFileListingProgress.Visible = true;

			startOpenDirectoryTask(clear);


        }

		private void startOpenDirectoryTask(bool clear) {
			WorkerArguments openDirectoryTask = new WorkerArguments();
			openDirectoryTask.scheduledTask = MainTask.OpenDirectory;
			openDirectoryTask.taskArgs = new object[] { clear };
			backgroundTaskWorker.RunWorkerAsync(openDirectoryTask);
		}

        #endregion

        private void prepareEpisodeNrControl(MediaFile someCandidate, NumericUpDown episodeNrControl) {
            // FIXME: Can't you set this just once?
            episodeNrControl.Minimum = 1;
			TvShow titleCollection = TvShowManager.Instance.GetTvShow(someCandidate.Showname);
            if (titleCollection != null) {
                episodeNrControl.Maximum = titleCollection.FindMaxEpisodeNr(someCandidate.SeasonNr);
            }
            episodeNrControl.Select(0, episodeNrControl.Text.Length);
        }

        private void prepareSeasonNrControl(MediaFile someCandidate, NumericUpDown seasonNrControl) {
            // FIXME: Can't you set this just once?
            seasonNrControl.Minimum = 1;
			TvShow titleCollection = TvShowManager.Instance.GetTvShow(someCandidate.Showname);
            if (titleCollection != null) {
                seasonNrControl.Maximum = titleCollection.FindMaxSeason();
            }
            seasonNrControl.Select(0, seasonNrControl.Text.Length);
        }

        private void prepareEpisodeTitleControl(MediaFile someCandidate, object sender, CellEditEventArgs e) {
			TvShow titleCollection = TvShowManager.Instance.GetTvShow(someCandidate.Showname);
            if (titleCollection != null) {
                ComboBox cbEpisodeTitle = new ComboBox();
                cbEpisodeTitle.Bounds = e.CellBounds;
                cbEpisodeTitle.Font = ((FastObjectListView)sender).Font;
                cbEpisodeTitle.DropDownStyle = ComboBoxStyle.DropDown;
                Boolean titleMatched = false;
                foreach (ShowEpisode episode in titleCollection) {
                    if (episode.SeasonNr == someCandidate.SeasonNr) {
                        cbEpisodeTitle.Items.Add(episode.Title);
                    }
                    if (!titleMatched && someCandidate.EpisodeTitle == episode.Title) {
                        cbEpisodeTitle.SelectedItem = episode.Title;
                        titleMatched = true;
                    }
                }
                if (cbEpisodeTitle.SelectedIndex < 0 && cbEpisodeTitle.Items.Count > 0) {
                    cbEpisodeTitle.SelectedIndex = 0;
                }
                cbEpisodeTitle.SelectedIndexChanged += new EventHandler(cbEpisodeTitle_SelectedIndexChanged);
                cbEpisodeTitle.Tag = e.RowObject; // remember which candidate we are editing
                e.Control = cbEpisodeTitle;
            }
        }

		private void prepareNewFilenameControl(TextBox newFilenameControl) {
			int lastDotIndex = newFilenameControl.Text.LastIndexOf('.');
			if (lastDotIndex > 0) {
				newFilenameControl.SelectionStart = 0;
				newFilenameControl.SelectionLength = lastDotIndex;
			}
		}

        private void lstCandidateFiles_CellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) {
            removeToolStripMenuItem.ShortcutKeys = Keys.None;
            MediaFile candidate = (MediaFile)e.RowObject;
            if (e.Column == ColumnEpisode) {
                prepareEpisodeNrControl(candidate, (NumericUpDown)e.Control);
            } else if (e.Column == ColumnSeason) {
                prepareSeasonNrControl(candidate, (NumericUpDown)e.Control);
            } else if (e.Column == ColumnEpisodeName) {
                prepareEpisodeTitleControl(candidate, sender, e);
            } else if (e.Column == ColumnNewFilename) {
                prepareNewFilenameControl((TextBox) e.Control);
            }
        }

        private void cbEpisodeTitle_SelectedIndexChanged(object sender, EventArgs e) {
			ComboBox cbEpisodeTitle = (ComboBox)sender;

            if (cbEpisodeTitle.Tag.GetType() == typeof(MediaFile)) {
				MediaFile candidate = (MediaFile)cbEpisodeTitle.Tag;
				candidate.EpisodeTitle = cbEpisodeTitle.Text;
            }
        }

		private void changeSeasonNr(MediaFile someCandidate, NumericUpDown seasonNrControl) {
			int newSeasonNr = (int)seasonNrControl.Value;
			TvShow tvShow = TvShowManager.Instance.GetTvShow(someCandidate.Showname);
			if (tvShow != null) {
				if (someCandidate.EpisodeNr > tvShow.FindMaxEpisodeNr(newSeasonNr)) {
					someCandidate.EpisodeNr = tvShow.FindMaxEpisodeNr(newSeasonNr);
				}
			}			
		}

        private void lstCandidateFiles_CellEditFinishing(object sender, CellEditEventArgs e) {
            removeToolStripMenuItem.ShortcutKeys = Keys.Delete;
            MediaFile someCandidate = (MediaFile)e.RowObject;
			if (e.Column == ColumnSeason) {
				changeSeasonNr(someCandidate, (NumericUpDown) e.Control);
            }
            updateGUI();
        }

		/// <summary>
		/// Every selected entry in the list is started with the default program for it.
		/// Example: Starts a VLC instance for each selected video file.
		/// </summary>
		private void executeAllSelectedCandidates() {
			// FIXME: Test if this works
			foreach (MediaFile someCandidate in lstCandidateFiles.SelectedObjects) {
				MediaFile fileToStart = someCandidate;
				if (someCandidate.IsSubtitle) {
					fileToStart = MediaFileManager.Instance.GetVideo(someCandidate);
				}
				Process defaultAssociatedProgramProcess = Process.Start(fileToStart.FilePath.Path + Path.DirectorySeparatorChar + fileToStart.Filename);
			}
		}

        private void lstCandidateFiles_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
				executeAllSelectedCandidates();
            } else if (e.KeyCode == Keys.Space) {
                spacedown = true; // see KeyUp
            }
        }

		private void toggleProcessingRequestedForSelectedCandidates() {
			foreach (MediaFile someCandidate in lstCandidateFiles.SelectedObjects) {
				someCandidate.ProcessingRequested = !someCandidate.ProcessingRequested;
			}
			lstCandidateFiles.RefreshObjects(lstCandidateFiles.SelectedObjects);
		}

		private void lstCandidateFiles_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Space) {
				spacedown = false;
				//Hotkey in context menu isn't set because this would make the control not receive the space key, which is also used as a modifier
				toggleProcessingRequestedForSelectedCandidates();
			}
		}

		/// <summary>
		/// If some folder is dragged into the list view, change to the folder path
		/// If some file is dragged into the list view, do nothing
		/// //FIXME: It seems this code is never reached because the form is already intercepting the drag & drop
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lstCandidateFiles_DragDrop(object sender, DragEventArgs e) {
            string[] dragArgs = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (dragArgs.Length == 1) {
				string possiblePath = dragArgs[0];
				if (Directory.Exists(possiblePath)) {
					checkAndChangeCurrentPath(possiblePath);
				}
            }
        }
		/// <summary>
		/// When a dragged item enters the list view...
		/// //FIXME: It seems this code is never reached because the form is already intercepting the drag & drop
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void lstCandidateFiles_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                if (s.Length == 1 && Directory.Exists(s[0])) {
                    e.Effect = DragDropEffects.Move;
                }
            }
        }

        private void markAsMovieToolStripMenuItem_Click(object sender, EventArgs e) {
            MarkAsMovie();
        }

        private void markAsTVSeriesToolStripMenuItem_Click(object sender, EventArgs e) {
            MarkAsTVShow();
        }

        private void backgroundTaskWorker_DoWork(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;
            WorkerArguments taskArguments = (WorkerArguments)e.Argument;
            lastScheduledTask = taskArguments.scheduledTask;
            if (taskArguments.scheduledTask == MainTask.OpenDirectory) {
                bool clear = (bool) taskArguments.taskArgs[0];
                DataGenerator.UpdateList(clear, worker, e);
            } else if (taskArguments.scheduledTask == MainTask.DownloadData) {
                DataGenerator.GetAllTitles(worker, e);
            } else if (taskArguments.scheduledTask == MainTask.CreateRelations) {
                DataGenerator.GetAllRelations(worker, e);
            } else if (taskArguments.scheduledTask == MainTask.Rename) {
                MediaFileManager.Instance.Rename(worker, e);
            }
        }

        private void backgroundTaskWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			updateProgressBar(taskProgressBar, e.ProgressPercentage);
        }

		private void updateProgressBar(ProgressBar someProgressBar, int percentage) {
			someProgressBar.Value = someProgressBar.Maximum * percentage / 100;
		}

        private void backgroundTaskWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            SetIdleGUI();
            if (lastScheduledTask == MainTask.OpenDirectory) {
                if (!e.Cancelled) {
                    updateCandidateFiles();
                } else {
                    MediaFileManager.Instance.clear();
                }
                updateGUI();
            } else if (lastScheduledTask == MainTask.DownloadData && !e.Cancelled) {
                ShownameSearch ss = new ShownameSearch(DataGenerator.Results);
                if (ss.ShowDialog(MainForm.Instance) == DialogResult.OK) {
                    DataGenerator.Results = ss.Results;
                    foreach (DataGenerator.ParsedSearch ps in DataGenerator.Results) {
                        if (ps.SearchString != ps.Showname) {
                            if (MessageBox.Show("Rename " + ps.Showname + " to " + ps.SearchString + "?", "Apply new Showname", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                                MediaFileManager.Instance.renameShow(ps.Showname, ps.SearchString);
                            }
                        }
                        if (ps.Results != null && ps.Results.Count > 0) {
                            //get rid of old relations
                            TvShowManager.Instance.RemoveTvShow(ps.Showname);
                            foreach (MediaFile ie in MediaFileManager.Instance) {
                                if (ie.Showname == ps.Showname && ie.ProcessingRequested) {
                                    ie.EpisodeTitle = "";
                                    ie.NewFilename = "";
                                    ie.Language = ps.provider.Language;
                                }
                            }
                        }
                    }

					startCreateTitlesTask();
                }
            } else if (lastScheduledTask == MainTask.CreateRelations && e.Cancelled) {
                TvShowManager.Instance.removeAllTvShows();
            }
            updateCandidateFiles();
        }

		private void startCreateTitlesTask() {
			WorkerArguments createRelationsTask = new WorkerArguments();
			createRelationsTask.scheduledTask = MainTask.CreateRelations;
			setBusyGUI();
			backgroundTaskWorker.RunWorkerAsync(createRelationsTask);
		}

        // FIXME: There should be some sign as to what action is currently blocking the UI.

        private void setGUIState(bool busy) {
            bool allowCancelBackgroundTask;
            bool showProgress;
            bool allowInteraction;
            if (busy) {
                allowCancelBackgroundTask = true;
                showProgress = true;
                allowInteraction = false;
                taskProgressBar.Value = 0;
            } else {
                allowCancelBackgroundTask = false;
                showProgress = false;
                allowInteraction = true;
            }
            taskProgressBar.Visible = showProgress;
            lblCurrentFolderPath.Visible = allowInteraction;
            txtCurrentFolderPath.Visible = allowInteraction;
            btnCancelBackgroundTask.Visible = allowCancelBackgroundTask;
            btnCancelBackgroundTask.Enabled = allowCancelBackgroundTask;
            btnChangeCurrentFolder.Enabled = allowInteraction;
            btnChangeCurrentFolder.Visible = allowInteraction;
            btnExploreCurrentFolder.Enabled = allowInteraction;
            btnExploreCurrentFolder.Visible = allowInteraction;
            btnOpenConfiguration.Enabled = allowInteraction;
            btnRenameSelectedCandidates.Enabled = allowInteraction;
            txtTargetFilenamePattern.Enabled = allowInteraction;
            lstCandidateFiles.Enabled = allowInteraction;
            if (!busy) {
                lblFileListingProgress.Visible = false;
                lstCandidateFiles.Focus();
            }
        }

        /// <summary>
        /// Called at points where the program should not allow any direct interaction with the GUI other than canceling the current action.
        /// </summary>
        private void setBusyGUI() {
            setGUIState(true);
        }
        /// <summary>
        /// Called at points where the gui was set busy and now should resume normal function.
        /// </summary>
        public void SetIdleGUI() {
            setGUIState(false);
        }

        private void btnCancelBackgroundTask_Click(object sender, EventArgs e) {
			cancelBackgroundTask();
        }

		private void cancelBackgroundTask() {
			backgroundTaskWorker.CancelAsync();
		}

        private void renameToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstCandidateFiles.SelectedIndices.Count == 1) {
                lstCandidateFiles.EditSubItem((OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[0]], 7);
            }
        }

        private void aboutDialogToolStripMenuItem_Click(object sender, EventArgs e) {
			showAboutBox();
        }

		/// <summary>
		/// When doubleclicking an item in the list and the space key is pressed (Why?),
		/// change the current path to the clicked candidate
		/// Can happen when the current folder has many subfolders and there are candidates in every folder
		/// Double click then jumps to the subfolder
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void lstCandidateFiles_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lstCandidateFiles.SelectedIndices.Count == 1 && spacedown) {
                MediaFile someCandidate = (MediaFile) lstCandidateFiles.SelectedObject;
                if (Helper.ReadProperties(ConfigKeyConstants.LAST_DIRECTORY_KEY)[0] != someCandidate.FilePath.Path) {
					checkAndChangeCurrentPath(someCandidate.FilePath.Path);
                }
            }
        }

        private void toggleSelectedToolStripMenuItem_Click(object sender, EventArgs e) {
			toggleProcessingRequestedForSelectedCandidates();
        }

        private void lookUpOnIMDBToolStripMenuItem_Click(object sender, EventArgs e) {

            if (lstCandidateFiles.SelectedIndices.Count == 1) {
                MediaFile ie = GetInfoEntryFromOLVI(lstCandidateFiles.SelectedIndices[0]);
                string SearchString = string.Format("http://us.imdb.com/find?s=tt&q={0};s=tt;site=aka", ie.Showname);
                Process.Start(SearchString);
            }

        }
        private MediaFile GetInfoEntryFromOLVI(int index) {
            if (lstCandidateFiles.GetItemCount() <= index) throw new IndexOutOfRangeException("count=" + lstCandidateFiles.GetItemCount() + " index=" + index);
            return (MediaFile)((OLVListItem)lstCandidateFiles.Items[index]).RowObject;
        }

        private void LogElement_DoubleClick(object sender, EventArgs e) {
            if (askToOpenLogFile()) {
				openLogFile(Helper.GetLogfilePath());
            }
        }

		private Boolean askToOpenLogFile() {
			DialogResult result = MessageBox.Show("Would you like to open the Logfile?", "Open Logfile", MessageBoxButtons.YesNo);
			if (result == DialogResult.Yes) {
				return true;
			} else {
				return false;
			}
		}
		private void openLogFile(string logFilePath) {
			Process.Start(logFilePath);
		}
    }
}