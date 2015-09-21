using System;
using System.Windows.Forms;

namespace Renamer.NewClasses.UI.Dialogs.MediaFiles
{
    /// <summary>
    /// Enter season dialog for setting season value on multiple files at once
    /// </summary>
    public partial class EnterSeason : Form
    {
        /// <summary>
        /// Selected season
        /// </summary>
        public int seasonNr = 1;

        /// <summary>
        /// standard constructor
        /// </summary>
        public EnterSeason(int someSeasonNr)
        {
            seasonNr = someSeasonNr;
            InitializeComponent();
            nudSeasonNr.Value = seasonNr;
        }

        /// <summary>
        /// Sets selected season value and DialogResult.OK and closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetSeasonNr_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            seasonNr = (int)nudSeasonNr.Value;
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

        private void EnterSeason_Load(object sender, EventArgs e)
        {
            nudSeasonNr.Select();
        }
    }
}
