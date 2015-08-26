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
using Renamer.Classes.UI;

namespace Renamer.Classes.UI.Dialogs
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
