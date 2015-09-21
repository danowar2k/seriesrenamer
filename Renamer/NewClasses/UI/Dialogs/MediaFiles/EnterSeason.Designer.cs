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
    partial class EnterSeason
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
			this.btnSetSeasonNr = new System.Windows.Forms.Button();
			this.lblSeasonNr = new System.Windows.Forms.Label();
			this.nudSeasonNr = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.nudSeasonNr)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(93, 51);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSetSeasonNr
			// 
			this.btnSetSeasonNr.Location = new System.Drawing.Point(12, 51);
			this.btnSetSeasonNr.Name = "btnSetSeasonNr";
			this.btnSetSeasonNr.Size = new System.Drawing.Size(75, 23);
			this.btnSetSeasonNr.TabIndex = 1;
			this.btnSetSeasonNr.Text = "OK";
			this.btnSetSeasonNr.UseVisualStyleBackColor = true;
			this.btnSetSeasonNr.Click += new System.EventHandler(this.btnSetSeasonNr_Click);
			// 
			// lblSeasonNr
			// 
			this.lblSeasonNr.AutoSize = true;
			this.lblSeasonNr.Location = new System.Drawing.Point(12, 9);
			this.lblSeasonNr.Name = "lblSeasonNr";
			this.lblSeasonNr.Size = new System.Drawing.Size(81, 13);
			this.lblSeasonNr.TabIndex = 2;
			this.lblSeasonNr.Text = "Enter Season #";
			// 
			// nudSeasonNr
			// 
			this.nudSeasonNr.Location = new System.Drawing.Point(12, 25);
			this.nudSeasonNr.Name = "nudSeasonNr";
			this.nudSeasonNr.Size = new System.Drawing.Size(156, 20);
			this.nudSeasonNr.TabIndex = 3;
			this.nudSeasonNr.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// EnterSeason
			// 
			this.AcceptButton = this.btnSetSeasonNr;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(181, 87);
			this.ControlBox = false;
			this.Controls.Add(this.nudSeasonNr);
			this.Controls.Add(this.lblSeasonNr);
			this.Controls.Add(this.btnSetSeasonNr);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "EnterSeason";
			this.Text = "Enter Season #";
			this.Load += new System.EventHandler(this.EnterSeason_Load);
			((System.ComponentModel.ISupportInitialize)(this.nudSeasonNr)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSetSeasonNr;
        private System.Windows.Forms.Label lblSeasonNr;
        private System.Windows.Forms.NumericUpDown nudSeasonNr;
    }
}