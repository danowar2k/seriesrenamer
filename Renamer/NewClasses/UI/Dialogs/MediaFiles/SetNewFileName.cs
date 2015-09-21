using System;
using System.Windows.Forms;

namespace Renamer.NewClasses.UI.Dialogs.MediaFiles
{
    public partial class SetNewFileName : Form
    {
        public string result = "";
        public SetNewFileName(string oldFilename)
        {
            InitializeComponent();
            txtNewFilename.Text = oldFilename;
        }

        private void btnSetNewFilename_Click(object sender, EventArgs e)
        {
            result = txtNewFilename.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

    }
}
