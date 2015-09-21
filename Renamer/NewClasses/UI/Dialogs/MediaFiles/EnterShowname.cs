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
using System.Windows.Forms;
using Renamer.NewClasses.Config;

namespace Renamer.NewClasses.UI.Dialogs.MediaFiles
{
    /// <summary>
    /// Dialog to set a showname
    /// </summary>
    public partial class EnterShowname : Form
    {
        /// <summary>
        /// Selected showname
        /// </summary>
        public string SelectedName = "";

	    private List<string> shownames = new List<string>(); 

	    /// <summary>
	    /// Constructor (TODO: Give Configuration's last shownames)
	    /// </summary>
	    /// <param name="files">List of files to select</param>
	    public EnterShowname(string currentShowname, List<string> knownShownames) {
            InitializeComponent();
			if (knownShownames != null) {
				shownames.AddRange(knownShownames);
			}
			cbShownames.Items.AddRange(shownames.ToArray());
		    if (cbShownames.Items.Count > 0) {
                cbShownames.SelectedIndex = 0;
            }
            for (int i = 0; i < cbShownames.Items.Count; i++) {
                string storedShowname = (string)cbShownames.Items[i];
                if (storedShowname.Equals(currentShowname,StringComparison.CurrentCultureIgnoreCase)) {
                    cbShownames.SelectedIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        /// Set selected index and DialogResult.OK and close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetShowname_Click(object sender, EventArgs e)
        {
            SelectedName = cbShownames.Text;
            DialogResult = DialogResult.OK;
			Settings.Instance.getAppConfiguration().updateLastShownames(SelectedName);
            Close();
        }

        /// <summary>
        /// Set DialogResult.Cancel and close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

    }
}
