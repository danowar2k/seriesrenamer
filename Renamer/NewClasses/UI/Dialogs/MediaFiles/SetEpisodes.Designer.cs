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

namespace Renamer.NewClasses.UI.Dialogs.MediaFiles
{
    partial class SetEpisodes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSetEpisodeNrs = new System.Windows.Forms.Button();
			this.lblFromNr = new System.Windows.Forms.Label();
			this.lblToNr = new System.Windows.Forms.Label();
			this.nudToNr = new System.Windows.Forms.NumericUpDown();
			this.nudFromNr = new System.Windows.Forms.NumericUpDown();
			this.lblSetEpisodeNumbers = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nudToNr)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudFromNr)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(114, 81);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnSetEpisodeNrs
			// 
			this.btnSetEpisodeNrs.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSetEpisodeNrs.Location = new System.Drawing.Point(33, 81);
			this.btnSetEpisodeNrs.Name = "btnSetEpisodeNrs";
			this.btnSetEpisodeNrs.Size = new System.Drawing.Size(75, 23);
			this.btnSetEpisodeNrs.TabIndex = 1;
			this.btnSetEpisodeNrs.Text = "OK";
			this.btnSetEpisodeNrs.UseVisualStyleBackColor = true;
			// 
			// lblFromNr
			// 
			this.lblFromNr.AutoSize = true;
			this.lblFromNr.Location = new System.Drawing.Point(12, 31);
			this.lblFromNr.Name = "lblFromNr";
			this.lblFromNr.Size = new System.Drawing.Size(27, 13);
			this.lblFromNr.TabIndex = 2;
			this.lblFromNr.Text = "from";
			// 
			// lblToNr
			// 
			this.lblToNr.AutoSize = true;
			this.lblToNr.Location = new System.Drawing.Point(12, 57);
			this.lblToNr.Name = "lblToNr";
			this.lblToNr.Size = new System.Drawing.Size(16, 13);
			this.lblToNr.TabIndex = 3;
			this.lblToNr.Text = "to";
			// 
			// nudToNr
			// 
			this.nudToNr.Location = new System.Drawing.Point(69, 55);
			this.nudToNr.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.nudToNr.Name = "nudToNr";
			this.nudToNr.Size = new System.Drawing.Size(120, 20);
			this.nudToNr.TabIndex = 4;
			this.nudToNr.ValueChanged += new System.EventHandler(this.nudToNr_ValueChanged);
			// 
			// nudFromNr
			// 
			this.nudFromNr.Location = new System.Drawing.Point(69, 29);
			this.nudFromNr.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.nudFromNr.Name = "nudFromNr";
			this.nudFromNr.Size = new System.Drawing.Size(120, 20);
			this.nudFromNr.TabIndex = 5;
			this.nudFromNr.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudFromNr.ValueChanged += new System.EventHandler(this.nudFromNr_ValueChanged);
			// 
			// lblSetEpisodeNumbers
			// 
			this.lblSetEpisodeNumbers.AutoSize = true;
			this.lblSetEpisodeNumbers.Location = new System.Drawing.Point(12, 9);
			this.lblSetEpisodeNumbers.Name = "lblSetEpisodeNumbers";
			this.lblSetEpisodeNumbers.Size = new System.Drawing.Size(182, 13);
			this.lblSetEpisodeNumbers.TabIndex = 6;
			this.lblSetEpisodeNumbers.Text = "Set episode numbers of selected files";
			// 
			// SetEpisodes
			// 
			this.AcceptButton = this.btnSetEpisodeNrs;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(196, 116);
			this.Controls.Add(this.lblSetEpisodeNumbers);
			this.Controls.Add(this.nudFromNr);
			this.Controls.Add(this.nudToNr);
			this.Controls.Add(this.lblToNr);
			this.Controls.Add(this.lblFromNr);
			this.Controls.Add(this.btnSetEpisodeNrs);
			this.Controls.Add(this.btnCancel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SetEpisodes";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Set Episodes";
			this.Load += new System.EventHandler(this.SetEpisodes_Load);
			((System.ComponentModel.ISupportInitialize)(this.nudToNr)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudFromNr)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSetEpisodeNrs;
        private System.Windows.Forms.Label lblFromNr;
        private System.Windows.Forms.Label lblToNr;
        private System.Windows.Forms.NumericUpDown nudToNr;
        private System.Windows.Forms.NumericUpDown nudFromNr;
        private System.Windows.Forms.Label lblSetEpisodeNumbers;
    }
}