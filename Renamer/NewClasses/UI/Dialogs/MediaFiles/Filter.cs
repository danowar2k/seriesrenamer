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
    /// File selection filter dialog
    /// </summary>
    public partial class Filter : Form
    {
        /// <summary>
        /// entered filter string
        /// </summary>
        public string result;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="str">Initial value to show in textbox</param>
        public Filter(string str)
        {
            InitializeComponent();
            txtFilter.Text = str;
        }

        /// <summary>
        /// Sets entered filter text and DialogResult.OK and closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult=DialogResult.OK;
            result=txtFilter.Text;
            Close();
        }

        /// <summary>
        /// Sets DialogResult.Cancel and closes
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
