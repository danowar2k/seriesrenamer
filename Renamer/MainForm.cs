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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections;
using System.Diagnostics;
using Schematrix;
using ICSharpCode.SharpZipLib.Zip;
using Renamer.Classes;
using Renamer.Dialogs;
using Renamer.Classes.Configuration;
using System.Runtime.InteropServices;
using Renamer.Logging;
using Renamer.Classes.Provider;
using System.Threading;
using BrightIdeasSoftware;
namespace Renamer {
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
        private Control focused = null;

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
        #region PROVIDER_NAME_KEY Creation




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
                Candidate ie = (Candidate)((OLVListItem)lvi).RowObject;
                //Go through all selected files and remove tags and clean them up
                ie.Showname = "";
                ie.Destination = "";
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
                Candidate ie = (Candidate)((OLVListItem)lvi).RowObject;
                //Go through all selected files and remove tags and clean them up
                ie.Movie = false;
                ie.Showname = SeriesNameExtractor.Instance.ExtractSeriesName(ie);
                ie.Destination = "";
                ie.NewFilename = "";
                DataGenerator.ExtractSeasonAndEpisode(ie);
            }
            updateGUI();
        }
        #endregion

        /// <summary>
        /// Main Rename function
        /// </summary>
        private void Rename() {
            //Treat colliding files
            foreach (Candidate ie in CandidateManager.Instance) {
                if (ie.ProcessingRequested) {
                    Candidate ieColliding = CandidateManager.Instance.GetCollidingInfoEntry(ie);
                    while (ieColliding != null) {
                        CollidingFiles cf = new CollidingFiles(ie, ieColliding);
                        cf.ShowDialog();
                        ieColliding = CandidateManager.Instance.GetCollidingInfoEntry(ie);
                    }
                }
            }
            SetBusyGUI();
            WorkerArguments wa = new WorkerArguments();
            wa.scheduledTask = MainTask.Rename;
            backgroundTaskWorker.RunWorkerAsync(wa);
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
        private void lstFiles_ColumnClick(object sender, ColumnClickEventArgs e) {
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
            this.DragEnter += new DragEventHandler(Form_DragEnter);
            this.DragDrop += new DragEventHandler(Form_DragDrop);
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

        // mouse enter with a dragged item
        private void Form_DragEnter(object sender, DragEventArgs e) {
            // accept only files
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;

        }

        // dropping an item on the form
        private void Form_DragDrop(object sender, DragEventArgs e) {
            // path list
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (FileList.Length == 0)
                return;

            string path;

            if (FileList.Length == 1 && Directory.Exists(FileList[0])) {
                // one directory selected - set this directory as path
                path = FileList[0];
            } else {
                // more files selected: try to get the parent directory
                if (FileList[0].LastIndexOf('\\') > 0)
                    path = FileList[0].Substring(0, FileList[0].LastIndexOf('\\'));
                else
                    path = FileList[0];
            }

            CandidateManager.Instance.SetPath(ref path);
            txtCurrentFolderPath.Text = path;
            updateCandidateFiles(true);
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
        private void scContainer_MouseDown(object sender, MouseEventArgs e) {
            // Get the focused control before the splitter is focused
            focused = getFocused(this.Controls);
        }

        //restore last focussed control so splitter doesn't keep focus
        private void scContainer_MouseUp(object sender, MouseEventArgs e) {
            // If a previous control had focus
            if (focused != null) {
                // Return focus and clear the temp variable for
                // garbage collection (is this needed?)
                focused.Focus();
                focused = null;
            }
        }

        //Update Current Provider setting
        private void cbProviders_SelectedIndexChanged(object sender, EventArgs e) {
            Helper.WriteProperty(ConfigKeyConstants.LAST_SELECTED_TITLE_PROVIDER_KEY, cbProviders.SelectedItem.ToString());
        }

        //Update Current Subtitle Provider setting
        private void cbSubs_SelectedIndexChanged(object sender, EventArgs e) {
            Helper.WriteProperty(ConfigKeyConstants.LAST_SELECTED_SUBTITLE_PROVIDER_KEY, cbSubtitleProviders.SelectedItem.ToString());
        }

        //Show About box dialog
        private void btnAboutApplication_Click(object sender, EventArgs e) {
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
            try {
                Process myProc = Process.Start(e.LinkText);
            } catch (Exception ex) {
                Logger.Instance.LogMessage("Couldn't open " + e.LinkText + ":" + ex.Message, LogLevel.ERROR);
            }
        }

        /*//Update Destination paths when title of show is changed
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
            //weird mono hackfix
            if (settings.IsMonoCompatibilityMode) {
                fbdChangeCurrentFolder.SelectedPath = Environment.CurrentDirectory;
            } else {
            }

            DialogResult dr = fbdChangeCurrentFolder.ShowDialog();
            if (dr == DialogResult.OK) {
                string path = fbdChangeCurrentFolder.SelectedPath;
                CandidateManager.Instance.SetPath(ref path);
                txtCurrentFolderPath.Text = path;
                updateCandidateFiles(true);
            }
        }

        public void initMyLoggers() {
            Logger logger = Logger.Instance;
            logger.removeAllLoggers();
            logger.addLogger(new FileLogger(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "Renamer.log", true, Helper.ReadEnum<LogLevel>(ConfigKeyConstants.LOGFILE_LOGLEVEL_KEY)));
            logger.addLogger(new MessageBoxLogger(Helper.ReadEnum<LogLevel>(ConfigKeyConstants.MESSAGEBOX_LOGLEVEL_KEY)));

            //mono compatibility fixes
            if (Type.GetType("Mono.Runtime") != null) {
                logger.addLogger(new TextBoxLogger(txtLog, Helper.ReadEnum<LogLevel>(ConfigKeyConstants.TEXTBOX_LOGLEVEL_KEY)));
                rtbLog.Visible = false;
                txtLog.Visible = true;
                Logger.Instance.LogMessage("Running on Mono", LogLevel.INFO);
            } else {
                rtbLog.Text = "";
                logger.addLogger(new RichTextBoxLogger(rtbLog, Helper.ReadEnum<LogLevel>(ConfigKeyConstants.TEXTBOX_LOGLEVEL_KEY)));
            }
        }

        //Show configuration dialog
        private void btnOpenConfiguration_Click(object sender, EventArgs e) {
            ConfigurationDialog cfgDialog = new ConfigurationDialog();
            if (cfgDialog.ShowDialog() == DialogResult.OK) {
                updateCandidateFiles(true);
            }
        }

        //Fetch all title information etc yada yada yada blalblabla
        private void btnSearchForTitles_Click(object sender, EventArgs e) {
            WorkerArguments wa = new WorkerArguments();
            wa.scheduledTask = MainTask.DownloadData;
            SetBusyGUI();
            backgroundTaskWorker.RunWorkerAsync(wa);
        }

        /*//Enter = Click "Get Titles" button
        private void cbTitle_KeyDown(object sender, KeyEventArgs e)
        {
                if (e.KeyCode == Keys.Enter)
                {
                        SetNewTitle(cbTitle.Text);
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
                checkAndChangeCurrentPath();
                //need to update the displayed path because it might be changed (tailing backlashes are removed, and added for drives ("C:\" instead of "C:"))
            } else if (e.KeyCode == Keys.Escape) {
                resetCurrentPath();
            }
        }

        private void checkAndChangeCurrentPath() {
            string enteredPath = txtCurrentFolderPath.Text;
            if (Directory.Exists(enteredPath)) {
                CandidateManager.Instance.SetPath(ref enteredPath);
                txtCurrentFolderPath.Text = enteredPath;
                txtCurrentFolderPath.SelectionStart = txtCurrentFolderPath.Text.Length;
                updateCandidateFiles(true);
            } else {
                resetCurrentPath();
            }
        }

        private void resetCurrentPath() {
            txtCurrentFolderPath.Text = Helper.ReadProperty(ConfigKeyConstants.LAST_DIRECTORY_KEY);
            txtCurrentFolderPath.SelectionStart = txtCurrentFolderPath.Text.Length;
        }

        //Focus lost = change current directory
        private void txtCurrentFolderPath_Leave(object sender, EventArgs e) {
            checkAndChangeCurrentPath();
        }

        private void updateGUI() {
            //also update some gui elements for the sake of it
            bool foundCandidates = (CandidateManager.Instance.Count > 0);

            bool searchStringFound = false;
            bool candidateIsShow = false;
            if (foundCandidates) {

                foreach (Candidate ie in CandidateManager.Instance) {
                    if (!string.IsNullOrEmpty(ie.Showname)) {
                        searchStringFound = true;
                        if (!ie.Movie) {
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
                foreach (Candidate ie in CandidateManager.Instance) {
                    if (ie.IsVideofile && !string.IsNullOrEmpty(ie.Showname)) {
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
            Rename();
        }

        //Focus lost = store desired pattern and update names
        private void txtTarget_Leave(object sender, EventArgs e) {
            if (txtTargetFilenamePattern.Text != Helper.ReadProperty(ConfigKeyConstants.TARGET_FILENAME_PATTERN_KEY)) {
                Helper.WriteProperty(ConfigKeyConstants.TARGET_FILENAME_PATTERN_KEY, txtTargetFilenamePattern.Text);
                CandidateManager.Instance.CreateNewNames();
                updateCandidateFiles();
            }
        }

        //Enter = store desired pattern and update names
        private void txtTarget_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                if (txtTargetFilenamePattern.Text != Helper.ReadProperty(ConfigKeyConstants.TARGET_FILENAME_PATTERN_KEY)) {
                    Helper.WriteProperty(ConfigKeyConstants.TARGET_FILENAME_PATTERN_KEY, txtTargetFilenamePattern.Text);
                    CandidateManager.Instance.CreateNewNames();
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
        //Set which context menu options should be avaiable
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
                Candidate ie = (Candidate)lvi.RowObject;
                if (subext.Contains(ie.Extension.ToLower())) {
                    editSubtitleToolStripMenuItem.Visible = true;
                }

                //if selected file is a video and there is a matching subtitle
                if (CandidateManager.Instance.GetSubtitle(ie) != null) {
                    editSubtitleToolStripMenuItem.Visible = true;
                }

                //if there is a matching video
                if (CandidateManager.Instance.GetVideo(ie) != null) {
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
                Candidate.DirectoryStructure CreateDirectoryStructure = Candidate.DirectoryStructure.Unset;
                Candidate.Case Case = Candidate.Case.Unset;
                Candidate.UmlautAction Umlaute = Candidate.UmlautAction.Unset;
                for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                    OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                    Candidate ie = (Candidate)lvi.RowObject;
                    if (ie.Movie) TVShowsOnly = false;
                    else MoviesOnly = false;
                    if (i == 0) {
                        CreateDirectoryStructure = ie.CreateDirectoryStructure;
                        Case = ie.Casing;
                        Umlaute = ie.UmlautUsage;
                    } else {
                        if (CreateDirectoryStructure != ie.CreateDirectoryStructure) {
                            CreateDirectoryStructure = Candidate.DirectoryStructure.Unset;
                        }
                        if (Case != ie.Casing) {
                            Case = Candidate.Case.Unset;
                        }
                        if (Umlaute != ie.UmlautUsage) {
                            Umlaute = Candidate.UmlautAction.Unset;
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
                if (CreateDirectoryStructure == Candidate.DirectoryStructure.CreateDirectoryStructure) {
                    createDirectoryStructureToolStripMenuItem1.Checked = true;
                } else if (CreateDirectoryStructure == Candidate.DirectoryStructure.NoDirectoryStructure) {
                    dontCreateDirectoryStructureToolStripMenuItem.Checked = true;
                }
                if (Case == Candidate.Case.UpperFirst) {
                    largeToolStripMenuItem.Checked = true;
                } else if (Case == Candidate.Case.small) {
                    smallToolStripMenuItem.Checked = true;
                } else if (Case == Candidate.Case.Ignore) {
                    igNorEToolStripMenuItem.Checked = true;
                } else if (Case == Candidate.Case.CAPSLOCK) {
                    cAPSLOCKToolStripMenuItem.Checked = true;
                }
                if (Umlaute == Candidate.UmlautAction.Use) {
                    useUmlautsToolStripMenuItem.Checked = true;
                } else if (Umlaute == Candidate.UmlautAction.Dont_Use) {
                    dontUseUmlautsToolStripMenuItem.Checked = true;
                } else if (Umlaute == Candidate.UmlautAction.Ignore) {
                    useProvidedNamesToolStripMenuItem.Checked = true;
                }
                for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                    OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                    Candidate ie = (Candidate)lvi.RowObject;
                    if (ie.Filename != "")
                        OldFilename = true;
                    if (ie.FilePath.Path != "")
                        OldPath = true;
                    if (ie.Name != "")
                        Name = true;
                    if (ie.Destination != "")
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
            foreach (Candidate ie in CandidateManager.Instance) {
                ie.ProcessingRequested = true;
            }
            lstCandidateFiles.Refresh();
        }

        //Uncheck all list boxes
        private void uncheckAllToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (Candidate ie in CandidateManager.Instance) {
                ie.ProcessingRequested = false;
            }
            lstCandidateFiles.Refresh();
        }

        //Invert check status of Selected list boxes
        private void invertCheckToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (Candidate ie in CandidateManager.Instance) {
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
                    Candidate ie = (Candidate)lvi.RowObject;
                    if (ie.Season != -1) {
                        sum += ie.Season;
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
                        Candidate ie = (Candidate)lvi.RowObject;
                        ie.Season = es.season;
                        if (ie.Destination != "") {
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
                Candidate firstEpisodeInfo = (Candidate)firstEpisodeEntry.RowObject;
                SetEpisodes se = new SetEpisodes(lstCandidateFiles.SelectedIndices.Count, firstEpisodeInfo.Episode);

                if (se.ShowDialog() == DialogResult.OK) {
                    for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                        OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                        Candidate ie = (Candidate)lvi.RowObject;
                        ie.Episode = (i + se.From);
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
            List<Candidate> lie = new List<Candidate>();
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                Candidate ie = (Candidate)lvi.RowObject;
                lie.Add(ie);
            }
            foreach (Candidate ie in lie) {
                CandidateManager.Instance.Remove(ie);
                lstCandidateFiles.RemoveObject(ie);
            }
            updateGUI();
        }

        //Delete file
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Delete selected files from the file system? This action cannot be undone!", "Delete selected files?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            List<Candidate> lie = new List<Candidate>();
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                Candidate ie = (Candidate)lvi.RowObject;
                lie.Add(ie);
            }
            foreach (Candidate ie in lie) {
                try {
                    File.Delete(ie.FilePath.Path + Path.DirectorySeparatorChar + ie.Filename);
                    CandidateManager.Instance.Remove(ie);
                    lstCandidateFiles.RemoveObject(ie);
                } catch (Exception ex) {
                    Logger.Instance.LogMessage("Error deleting file: " + ex.Message, LogLevel.ERROR);
                }
            }
            updateGUI();
        }

        //Open file
        private void viewToolStripMenuItem_Click(object sender, EventArgs e) {
            Candidate ie = CandidateManager.Instance.GetVideo((Candidate)((OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[0]]).RowObject);
            string VideoPath = ie.FilePath.Path + Path.DirectorySeparatorChar + ie.Filename;
            try {
                Process myProc = Process.Start(VideoPath);
            } catch (Exception ex) {
                Logger.Instance.LogMessage("Couldn't open " + VideoPath + ":" + ex.Message, LogLevel.ERROR);
            }
        }

        //Edit subtitle
        private void editSubtitleToolStripMenuItem_Click(object sender, EventArgs e) {
            Candidate sub = CandidateManager.Instance.GetSubtitle((Candidate)((OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[0]]).RowObject);
            Candidate video = CandidateManager.Instance.GetVideo((Candidate)((OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[0]]).RowObject);
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
                        Candidate ie = (Candidate)lvi.RowObject;
                        string destination = ib.input;
                        ie.Destination = destination;
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
            lstCandidateFiles.VirtualListSize = CandidateManager.Instance.Count;
            lstCandidateFiles.SetObjects(CandidateManager.Instance);
            lstCandidateFiles.Sort();
            lstCandidateFiles.Refresh();
        }





        private void initCandidates() {
            lstCandidateFiles.RowFormatter = delegate(OLVListItem olvi) {
                //reset colors to make sure they are set properly
                olvi.BackColor = Color.White;
                olvi.ForeColor = Color.Black;
                Candidate rowCandidate = (Candidate)olvi.RowObject;
                if ((rowCandidate.NewFilename == "" && (rowCandidate.Destination == "" || rowCandidate.Destination == rowCandidate.FilePath.Path))
                    || !rowCandidate.ProcessingRequested) {
                    olvi.ForeColor = Color.Gray;
                }
                if (!rowCandidate.MarkedForDeletion) {
                    foreach (Candidate someCandidate in CandidateManager.Instance) {
                        if (rowCandidate != someCandidate) {
                            if (CandidateManager.Instance.IsSameTarget(rowCandidate, someCandidate)) {
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
                return ((Candidate)x).ProcessingRequested;
            };
            this.lstCandidateFiles.BooleanCheckStatePutter = delegate(object x, bool newValue) {
                ((Candidate)x).ProcessingRequested = newValue;
                bool shouldBeEnabled = newValue;
                if (!shouldBeEnabled) {
                    foreach (Candidate someCandidate in CandidateManager.Instance) {
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
                return ((Candidate)x).Filename;
            };

            //Source path
            this.ColumnFilepath.AspectGetter = delegate(object x) {
                return ((Candidate)x).FilePath.Path;
            };

            //Showname
            this.ColumnShowname.AspectGetter = delegate(object x) {
                return ((Candidate)x).Showname;
            };
            this.ColumnShowname.AspectPutter = delegate(object x, object newValue) {
                ((Candidate)x).Showname = (string)newValue;
                updateGUI();
            };

            //SEASON_NR
            this.ColumnSeason.AspectGetter = delegate(object x) {
                return ((Candidate)x).Season;
            };
            this.ColumnSeason.AspectPutter = delegate(object x, object newValue) {
                ((Candidate)x).Season = (int)newValue;
                updateGUI();
            };

            //EPISODE_NR
            this.ColumnEpisode.AspectGetter = delegate(object x) {
                return ((Candidate)x).Episode;
            };
            this.ColumnEpisode.AspectPutter = delegate(object x, object newValue) {
                ((Candidate)x).Episode = (int)newValue;
                updateGUI();
            };

            //EPISODE_TITLE
            this.ColumnEpisodeName.AspectGetter = delegate(object x) {
                return ((Candidate)x).Name;
            };
            this.ColumnEpisodeName.AspectPutter = delegate(object x, object newValue) {
                ((Candidate)x).Name = (string)newValue;
                //backtrack to see if entered text matches a season/episode
                TitleCollection titles = TitleManager.Instance.GetTitleCollection(((Candidate)x).Showname);
                if (titles != null) {
                    foreach (EpisodeData title in titles) { 
                        if ((string)newValue == title.EpisodeTitle) {
                            ((Candidate)x).Season = title.SeasonNr;
                            ((Candidate)x).Episode = title.SeasonEpisodeNr;
                            break;
                        }
                    }
                }
                updateGUI();
            };

            //Destination
            this.ColumnDestination.AspectGetter = delegate(object x) {
                return ((Candidate)x).Destination;
            };
            this.ColumnDestination.AspectPutter = delegate(object x, object newValue) {
                ((Candidate)x).Destination = (string)newValue;
                updateGUI();
            };

            //Filename
            this.ColumnNewFilename.AspectGetter = delegate(object x) {
                return ((Candidate)x).NewFilename;
            };
            this.ColumnNewFilename.AspectPutter = delegate(object x, object newValue) {
                ((Candidate)x).NewFilename = (string)newValue;
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
                SetNewTitle(cbTitle.Text);
            }
        }*/

        private void originalNameToolStripMenuItem_Click(object sender, EventArgs e) {
            string clipboard = "";
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                Candidate ie = (Candidate)lvi.RowObject;
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
                Candidate ie = (Candidate)lvi.RowObject;
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
                Candidate ie = (Candidate)lvi.RowObject;
                clipboard += ie.Name + Environment.NewLine;
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
                Candidate ie = (Candidate)lvi.RowObject;
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
                Candidate ie = (Candidate)lvi.RowObject;
                if (ie.Destination != "" && ie.NewFilename != "") {
                    clipboard += ie.Destination + Path.DirectorySeparatorChar + ie.NewFilename + Environment.NewLine;
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
                Candidate ie = (Candidate)lvi.RowObject;
                bool DestinationDifferent = ie.Destination != "" && ie.Destination != ie.FilePath.Path;
                bool FilenameDifferent = ie.NewFilename != "" && ie.NewFilename != ie.Filename;
                if (DestinationDifferent && FilenameDifferent) {
                    clipboard += ie.Filename + " --> " + ie.Destination + Path.DirectorySeparatorChar + ie.NewFilename + Environment.NewLine;
                } else if (DestinationDifferent) {
                    clipboard += ie.Filename + " --> " + ie.Destination + Environment.NewLine;
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
            if (Directory.Exists(txtCurrentFolderPath.Text)) {
                Process explorerProcess = Process.Start(txtCurrentFolderPath.Text);
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
                Candidate ie = (Candidate)lvi.RowObject;
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
                        source = ie.Destination;
                        break;
                }
                //Insert parameter values
                LocalSearchString = LocalSearchString.Replace("%OF", ie.Filename);
                LocalSearchString = LocalSearchString.Replace("%DF", ie.NewFilename);
                LocalSearchString = LocalSearchString.Replace("%OP", ie.FilePath.Path);
                LocalSearchString = LocalSearchString.Replace("%DP", ie.Destination);
                LocalSearchString = LocalSearchString.Replace("%T", title);
                LocalSearchString = LocalSearchString.Replace("%N", ie.Name);
                LocalSearchString = LocalSearchString.Replace("%E", ie.Episode.ToString());
                LocalSearchString = LocalSearchString.Replace("%s", ie.Season.ToString());
                LocalSearchString = LocalSearchString.Replace("%S", ie.Season.ToString("00"));
                LocalReplaceString = LocalReplaceString.Replace("%OF", ie.Filename);
                LocalReplaceString = LocalReplaceString.Replace("%DF", ie.NewFilename);
                LocalReplaceString = LocalReplaceString.Replace("%OP", ie.FilePath.Path);
                LocalReplaceString = LocalReplaceString.Replace("%DP", ie.Destination);
                LocalReplaceString = LocalReplaceString.Replace("%T", title);
                LocalReplaceString = LocalReplaceString.Replace("%N", ie.Name);
                LocalReplaceString = LocalReplaceString.Replace("%E", ie.Episode.ToString());
                LocalReplaceString = LocalReplaceString.Replace("%s", ie.Season.ToString());
                LocalReplaceString = LocalReplaceString.Replace("%S", ie.Season.ToString("00"));

                //see if replace will be done for count var
                if (source.Contains(SearchString))
                    count++;
                //do the replace
                source = source.Replace(LocalSearchString, LocalReplaceString);
                if (destination == "Filename") {
                    ie.NewFilename = source;
                } else if (destination == "Path") {
                    ie.Destination = source;
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
                Candidate ie = (Candidate)lvi.RowObject;
                string Showname = ie.Showname;
                if (!names.Contains(Showname)) {
                    names.Add(Showname);
                }
            }
            List<Candidate> similar = new List<Candidate>();
            foreach (string str in names) {
                similar.AddRange(CandidateManager.Instance.FindSimilarByName(str));
            }
            for (int i = 0; i < lstCandidateFiles.Items.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[i];
                Candidate ie = (Candidate)lvi.RowObject;
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
                Candidate ie = (Candidate)lvi.RowObject;
                string path = ie.FilePath.Path;
                if (!paths.Contains(path)) {
                    paths.Add(path);
                }
            }
            List<Candidate> similar = new List<Candidate>();
            foreach (string str in paths) {
                similar.AddRange(CandidateManager.Instance.FindSimilarByName(str));
            }
            for (int i = 0; i < lstCandidateFiles.Items.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[i];
                Candidate ie = (Candidate)lvi.RowObject;
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
                Candidate ie = (Candidate)lvi.RowObject;
                ie.CreateDirectoryStructure = Candidate.DirectoryStructure.CreateDirectoryStructure;
            }
            ((ToolStripMenuItem)sender).Checked = true;
            dontCreateDirectoryStructureToolStripMenuItem.Checked = false;
            updateGUI();
        }

        private void dontCreateDirectoryStructureToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                Candidate ie = (Candidate)lvi.RowObject;
                ie.CreateDirectoryStructure = Candidate.DirectoryStructure.NoDirectoryStructure;
            }
            ((ToolStripMenuItem)sender).Checked = true;
            createDirectoryStructureToolStripMenuItem.Checked = false;
            updateGUI();
        }

        private void useUmlautsToolStripMenuItem1_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                Candidate ie = (Candidate)lvi.RowObject;
                ie.UmlautUsage = Candidate.UmlautAction.Use;
            }
            ((ToolStripMenuItem)sender).Checked = true;
            dontUseUmlautsToolStripMenuItem.Checked = false;
            useProvidedNamesToolStripMenuItem.Checked = false;
            updateGUI();
        }

        private void dontUseUmlautsToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                Candidate ie = (Candidate)lvi.RowObject;
                ie.UmlautUsage = Candidate.UmlautAction.Dont_Use;
            }
            ((ToolStripMenuItem)sender).Checked = true;
            useUmlautsToolStripMenuItem.Checked = false;
            useProvidedNamesToolStripMenuItem.Checked = false;
            updateGUI();
        }

        private void useProvidedNamesToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                Candidate ie = (Candidate)lvi.RowObject;
                ie.UmlautUsage = Candidate.UmlautAction.Ignore;
            }
            ((ToolStripMenuItem)sender).Checked = true;
            useUmlautsToolStripMenuItem.Checked = false;
            dontUseUmlautsToolStripMenuItem.Checked = false;
            updateGUI();
        }

        private void largeToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                Candidate ie = (Candidate)lvi.RowObject;
                ie.Casing = Candidate.Case.UpperFirst;
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
                Candidate ie = (Candidate)lvi.RowObject;
                ie.Casing = Candidate.Case.small;
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
                Candidate ie = (Candidate)lvi.RowObject;
                ie.Casing = Candidate.Case.Ignore;
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
                Candidate ie = (Candidate)lvi.RowObject;
                ie.Casing = Candidate.Case.CAPSLOCK;
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
                    Candidate ie = (Candidate)lvi.RowObject;
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
                        ((Candidate)lvi.RowObject).Showname = es.SelectedName;
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
            SetBusyGUI();
            taskProgressBar.Visible = false;
            lblFileListingProgress.Visible = true;

            WorkerArguments openDirectoryTask = new WorkerArguments();
            openDirectoryTask.scheduledTask = MainTask.OpenDirectory;
            openDirectoryTask.taskArgs = new object[] { clear };
            backgroundTaskWorker.RunWorkerAsync(openDirectoryTask);


        }
        #endregion

        private void lstEntries_CellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) {
            removeToolStripMenuItem.ShortcutKeys = Keys.None;
            Candidate ie = (Candidate)e.RowObject;
            if (e.Column == ColumnEpisode) {
                ((NumericUpDown)e.Control).Minimum = 1;
                TitleCollection rc = TitleManager.Instance.GetTitleCollection(ie.Showname);
                if (rc != null) {
                    ((NumericUpDown)e.Control).Maximum = rc.FindMaxEpisode(ie.Season);
                }
                ((NumericUpDown)e.Control).Select(0, e.Control.Text.Length);
            } else if (e.Column == ColumnSeason) {
                ((NumericUpDown)e.Control).Minimum = 1;
                TitleCollection rc = TitleManager.Instance.GetTitleCollection(ie.Showname);
                if (rc != null) {
                    ((NumericUpDown)e.Control).Maximum = rc.FindMaxSeason();
                }
                ((NumericUpDown)e.Control).Select(0, e.Control.Text.Length);
            } else if (e.Column == ColumnEpisodeName) {
                TitleCollection rc = TitleManager.Instance.GetTitleCollection(ie.Showname);
                if (rc != null) {
                    ComboBox cb = new ComboBox();
                    cb.Bounds = e.CellBounds;
                    cb.Font = ((FastObjectListView)sender).Font;
                    cb.DropDownStyle = ComboBoxStyle.DropDown;
                    foreach (EpisodeData r in rc) {
                        if (r.SeasonNr == ie.Season) {
                            cb.Items.Add(r.EpisodeTitle);
                        }
                        if (ie.Name == r.EpisodeTitle) {
                            cb.SelectedItem = r.EpisodeTitle;
                        }
                    }
                    if (cb.SelectedIndex < 0 && cb.Items.Count > 0) {
                        cb.SelectedIndex = 0;
                    }
                    cb.SelectedIndexChanged += new EventHandler(cb_SelectedIndexChanged);
                    cb.Tag = e.RowObject; // remember which person we are editing
                    e.Control = cb;
                }
            } else if (e.Column == ColumnNewFilename) {
                TextBox tb = (TextBox)e.Control;
                int pos = tb.Text.LastIndexOf('.');
                if (pos > 0) {
                    tb.SelectionStart = 0;
                    tb.SelectionLength = pos;
                }
            }
        }

        private void cb_SelectedIndexChanged(object sender, EventArgs e) {
            if (((ComboBox)sender).Tag.GetType() == typeof(Candidate)) {
                Candidate ie = (Candidate)((ComboBox)sender).Tag;
                ie.Name = ((ComboBox)sender).Text;
            }
        }

        private void lstEntries_CellEditFinishing(object sender, CellEditEventArgs e) {
            removeToolStripMenuItem.ShortcutKeys = Keys.Delete;
            Candidate ie = (Candidate)e.RowObject;
            if (e.Control.GetType() == typeof(ComboBox)) {
                ComboBox cb = (ComboBox)e.Control;
            } else if (e.Column == ColumnSeason) {
                int newValue = (int)((NumericUpDown)e.Control).Value;
                TitleCollection rc = TitleManager.Instance.GetTitleCollection(((Candidate)e.RowObject).Showname);
                if (rc != null) {
                    if (ie.Episode > rc.FindMaxEpisode(newValue)) {
                        ie.Episode = rc.FindMaxEpisode(newValue);
                    }
                }

            }
            updateGUI();
        }

        private void lstEntries_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                    OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                    Candidate ie = (Candidate)lvi.RowObject;
                    if (ie.IsSubtitle) ie = CandidateManager.Instance.GetVideo(ie);
                    Process myProc = Process.Start(ie.FilePath.Path + Path.DirectorySeparatorChar + ie.Filename);
                }
            } else if (e.KeyCode == Keys.Space) {
                spacedown = true;
            }
        }
        private void lstEntries_DragDrop(object sender, DragEventArgs e) {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (s.Length == 1 && Directory.Exists(s[0])) {
                CandidateManager.Instance.SetPath(s[0]);
            }
        }

        private void lstEntries_DragEnter(object sender, DragEventArgs e) {
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
                CandidateManager.Instance.Rename(worker, e);
            }
        }

        private void backgroundTaskWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            this.taskProgressBar.Value = this.taskProgressBar.Maximum * e.ProgressPercentage / 100;
        }

        private void backgroundTaskWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            SetIdleGUI();
            if (lastScheduledTask == MainTask.OpenDirectory) {
                if (!e.Cancelled) {
                    updateCandidateFiles();
                } else {
                    CandidateManager.Instance.Clear();
                }
                updateGUI();
            } else if (lastScheduledTask == MainTask.DownloadData && !e.Cancelled) {
                ShownameSearch ss = new ShownameSearch(DataGenerator.Results);
                if (ss.ShowDialog(MainForm.Instance) == DialogResult.OK) {
                    DataGenerator.Results = ss.Results;
                    foreach (DataGenerator.ParsedSearch ps in DataGenerator.Results) {
                        if (ps.SearchString != ps.Showname) {
                            if (MessageBox.Show("Rename " + ps.Showname + " to " + ps.SearchString + "?", "Apply new Showname", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                                CandidateManager.Instance.RenameShow(ps.Showname, ps.SearchString);
                            }
                        }
                        if (ps.Results != null && ps.Results.Count > 0) {
                            //get rid of old relations
                            TitleManager.Instance.RemoveRelationCollection(ps.Showname);
                            foreach (Candidate ie in CandidateManager.Instance) {
                                if (ie.Showname == ps.Showname && ie.ProcessingRequested) {
                                    ie.Name = "";
                                    ie.NewFilename = "";
                                    ie.Language = ps.provider.Language;
                                }
                            }
                        }
                    }
                    WorkerArguments createRelationsTask = new WorkerArguments();
                    createRelationsTask.scheduledTask = MainTask.CreateRelations;
                    SetBusyGUI();
                    backgroundTaskWorker.RunWorkerAsync(createRelationsTask);
                }
            } else if (lastScheduledTask == MainTask.CreateRelations && e.Cancelled) {
                TitleManager.Instance.Clear();
            }
            updateCandidateFiles();
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
        private void SetBusyGUI() {
            setGUIState(true);
        }
        /// <summary>
        /// Called at points where the gui was set busy and now should resume normal function.
        /// </summary>
        public void SetIdleGUI() {
            setGUIState(false);
        }

        private void btnCancelBackgroundTask_Click(object sender, EventArgs e) {
            backgroundTaskWorker.CancelAsync();
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstCandidateFiles.SelectedIndices.Count == 1) {
                lstCandidateFiles.EditSubItem((OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[0]], 7);
            }
        }

        private void aboutDialogToolStripMenuItem_Click(object sender, EventArgs e) {
            btnAboutApplication.PerformClick();
        }

        private void lstEntries_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lstCandidateFiles.SelectedIndices.Count == 1 && spacedown) {
                Candidate ie = (Candidate)((OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[0]]).RowObject;
                if (Helper.ReadProperties(ConfigKeyConstants.LAST_DIRECTORY_KEY)[0] != ie.FilePath.Path) {
                    CandidateManager.Instance.SetPath(ie.FilePath.Path);
                    txtCurrentFolderPath.Text = ie.FilePath.Path;
                    updateCandidateFiles(true);
                }
            }
        }

        private void lstEntries_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Space) {
                spacedown = false;
                //Hotkey in context menu isn't set because this would make the control not receive the space key, which is also used as a modifier
                for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                    OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                    Candidate ie = (Candidate)lvi.RowObject;
                    ie.ProcessingRequested = !ie.ProcessingRequested;
                    lstCandidateFiles.RefreshObject(ie);
                }
            }
        }

        private void toggleSelectedToolStripMenuItem_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstCandidateFiles.SelectedIndices.Count; i++) {
                OLVListItem lvi = (OLVListItem)lstCandidateFiles.Items[lstCandidateFiles.SelectedIndices[i]];
                Candidate ie = (Candidate)lvi.RowObject;
                ie.ProcessingRequested = !ie.ProcessingRequested;
                lstCandidateFiles.RefreshObject(ie);
            }
        }

        private void lookUpOnIMDBToolStripMenuItem_Click(object sender, EventArgs e) {

            if (lstCandidateFiles.SelectedIndices.Count == 1) {
                Candidate ie = GetInfoEntryFromOLVI(lstCandidateFiles.SelectedIndices[0]);
                string SearchString = string.Format("http://us.imdb.com/find?s=tt&q={0};s=tt;site=aka", ie.Showname);
                Process.Start(SearchString);
            }

        }
        private Candidate GetInfoEntryFromOLVI(int index) {
            if (lstCandidateFiles.GetItemCount() <= index) throw new IndexOutOfRangeException("count=" + lstCandidateFiles.GetItemCount() + " index=" + index);
            return (Candidate)((OLVListItem)lstCandidateFiles.Items[index]).RowObject;
        }
        private void Log_DoubleClick(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("Would you like to open the Logfile?", "Open Logfile", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) {
                Process.Start(Helper.GetLogfileDataPath());
            }

        }
    }
}

