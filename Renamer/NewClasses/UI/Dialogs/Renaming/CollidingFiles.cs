using System;
using System.Windows.Forms;
using System.IO;

namespace Renamer.NewClasses.UI.Dialogs.Renaming
{
    public partial class CollidingFiles : Form
    {
        MediaFile ie1, ie2;
        public CollidingFiles(MediaFile ie1, MediaFile ie2)
        {
            InitializeComponent();
            this.ie1 = ie1;
            this.ie2 = ie2;
            label2.Text = ie1.FilePath.Path + Path.DirectorySeparatorChar + ie1.Filename;
            label4.Text = ie1.GetFormattedFullDestination();
            if (!ie2.ProcessingRequested)
            {
                btnSkipSecond.Enabled = false;
            }
        }

        private void btnSkipFirst_Click(object sender, EventArgs e)
        {
            ie1.ProcessingRequested = false;
            Close();
        }

        private void btnRenameFirst_Click(object sender, EventArgs e)
        {
            
            SetNewFileName snfn;
            if (ie1.NewFilename != "")
            {
                snfn = new SetNewFileName(ie1.NewFilename);
            }
            else
            {
                snfn = new SetNewFileName(ie1.Filename);
            }
            if (snfn.ShowDialog() == DialogResult.OK)
            {
                ie1.NewFilename = snfn.result;
                ie1.ProcessingRequested = true;
                Close();
            }
        }

        private void btnMoveFirst_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (ie1.DestinationPath != "")
            {
                fbd.SelectedPath = ie1.DestinationPath;
            }
            else
            {
                fbd.SelectedPath = ie1.FilePath.Path;
            }
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ie1.DestinationPath = fbd.SelectedPath;
                ie1.ProcessingRequested = true;
                Close();
            }
        }

        private void btnSkipSecond_Click(object sender, EventArgs e)
        {
            ie2.ProcessingRequested = false;
            Close();
        }

        private void btnRenameSecond_Click(object sender, EventArgs e)
        {
            SetNewFileName snfn;
            if (ie2.NewFilename != "")
            {
                snfn = new SetNewFileName(ie2.NewFilename);
            }
            else
            {
                snfn = new SetNewFileName(ie2.Filename);
            }
            if (snfn.ShowDialog() == DialogResult.OK)
            {
                ie2.NewFilename = snfn.result;
                ie2.ProcessingRequested = true;
                Close();
            }
        }

        private void btnMoveSecond_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (ie2.DestinationPath != "")
            {
                fbd.SelectedPath = ie2.DestinationPath;
            }
            else
            {
                fbd.SelectedPath = ie2.FilePath.Path;
            }
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ie2.ProcessingRequested = true;
                ie2.DestinationPath = fbd.SelectedPath;
                Close();
            }
        }
    }
}
