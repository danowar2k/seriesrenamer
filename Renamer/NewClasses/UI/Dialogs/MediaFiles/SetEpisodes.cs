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
using System.Windows.Forms;

namespace Renamer.NewClasses.UI.Dialogs.MediaFiles
{
    /// <summary>
    /// Dialog to set episode values ascending or descending for multiple files at once
    /// </summary>
    public partial class SetEpisodes : Form
    {
        /// <summary>
        /// number of files
        /// </summary>
        int nrOfSelectedFiles;

        /// <summary>
        /// first selected episode nr
        /// </summary>
        int firstEpisodeNr;


        /// <summary>
        /// start index
        /// </summary>
        public int fromNr = 1;
        
        /// <summary>
        /// end index
        /// </summary>
        public int toNr;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="theNrOfSelectedFiles">Number of files</param>
        public SetEpisodes(int theNrOfSelectedFiles)
        {
            nrOfSelectedFiles = theNrOfSelectedFiles;
            firstEpisodeNr = 1;
            InitializeComponent();
        }

        public SetEpisodes(int theNrOfSelectedFiles, int newFirstEpisodeNr)
        {
            nrOfSelectedFiles = theNrOfSelectedFiles;
            firstEpisodeNr = newFirstEpisodeNr;
            InitializeComponent();
        }

        private void nudFromNr_ValueChanged(object sender, EventArgs e)
        {
            fromNr = (int)nudFromNr.Value;
	        int targetTo = fromNr + nrOfSelectedFiles - 1;
            if (nudToNr.Value != targetTo) {
                nudToNr.Value = targetTo;
            }
        }

        private void nudToNr_ValueChanged(object sender, EventArgs e)
        {
            toNr = (int)nudToNr.Value;
	        int targetFrom = toNr - nrOfSelectedFiles + 1;
            if (nudFromNr.Value != targetFrom) {
                nudFromNr.Value = targetFrom;
            }
        }

        private void SetEpisodes_Load(object sender, EventArgs e)
        {
            nudFromNr.Value = firstEpisodeNr;
            nudToNr.Value = nudFromNr.Value + nrOfSelectedFiles - 1;
			// FIXME: Is this stupid?
            nudToNr.Minimum = nudToNr.Value;
            nudToNr.Maximum = nudFromNr.Maximum + nrOfSelectedFiles - 1;
            nudFromNr.Select();
        }
    }
}
