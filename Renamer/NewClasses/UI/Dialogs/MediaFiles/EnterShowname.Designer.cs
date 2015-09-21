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
    partial class EnterShowname
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSetShowname = new System.Windows.Forms.Button();
			this.cbShownames = new System.Windows.Forms.ComboBox();
			this.lblShowname = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(205, 52);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSetShowname
			// 
			this.btnSetShowname.Location = new System.Drawing.Point(124, 52);
			this.btnSetShowname.Name = "btnSetShowname";
			this.btnSetShowname.Size = new System.Drawing.Size(75, 23);
			this.btnSetShowname.TabIndex = 1;
			this.btnSetShowname.Text = "OK";
			this.btnSetShowname.UseVisualStyleBackColor = true;
			this.btnSetShowname.Click += new System.EventHandler(this.btnSetShowname_Click);
			// 
			// cbShownames
			// 
			this.cbShownames.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cbShownames.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cbShownames.FormattingEnabled = true;
			this.cbShownames.Location = new System.Drawing.Point(15, 25);
			this.cbShownames.Name = "cbShownames";
			this.cbShownames.Size = new System.Drawing.Size(265, 21);
			this.cbShownames.TabIndex = 0;
			// 
			// lblShowname
			// 
			this.lblShowname.AutoSize = true;
			this.lblShowname.Location = new System.Drawing.Point(12, 9);
			this.lblShowname.Name = "lblShowname";
			this.lblShowname.Size = new System.Drawing.Size(156, 13);
			this.lblShowname.TabIndex = 3;
			this.lblShowname.Text = "Set showname for selected files";
			// 
			// EnterShowname
			// 
			this.AcceptButton = this.btnSetShowname;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(292, 85);
			this.Controls.Add(this.lblShowname);
			this.Controls.Add(this.cbShownames);
			this.Controls.Add(this.btnSetShowname);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EnterShowname";
			this.Text = "Set Showname";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSetShowname;
        private System.Windows.Forms.ComboBox cbShownames;
        private System.Windows.Forms.Label lblShowname;
    }
}