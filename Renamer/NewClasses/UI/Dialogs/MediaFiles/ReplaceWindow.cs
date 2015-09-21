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
    public partial class ReplaceWindow : Form
    {
        MainForm MainWindow = null;
        public ReplaceWindow(MainForm parent)
        {
            MainWindow = parent;
            InitializeComponent();
            cbReplaceIn.SelectedIndex = 0;
            txtSearch.Focus();
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            MainWindow.Replace(txtSearch.Text, txtReplace.Text, cbReplaceIn.SelectedItem.ToString());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
