namespace Renamer.NewClasses.UI.Dialogs.MediaFiles
{
    partial class SetNewFileName
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
			this.btnSetNewFilename = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtNewFilename = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnSetNewFilename
			// 
			this.btnSetNewFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSetNewFilename.Location = new System.Drawing.Point(124, 38);
			this.btnSetNewFilename.Name = "btnSetNewFilename";
			this.btnSetNewFilename.Size = new System.Drawing.Size(75, 23);
			this.btnSetNewFilename.TabIndex = 0;
			this.btnSetNewFilename.Text = "&OK";
			this.btnSetNewFilename.UseVisualStyleBackColor = true;
			this.btnSetNewFilename.Click += new System.EventHandler(this.btnSetNewFilename_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(205, 38);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// txtNewFilename
			// 
			this.txtNewFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtNewFilename.Location = new System.Drawing.Point(12, 12);
			this.txtNewFilename.Name = "txtNewFilename";
			this.txtNewFilename.Size = new System.Drawing.Size(268, 20);
			this.txtNewFilename.TabIndex = 2;
			// 
			// SetNewFileName
			// 
			this.AcceptButton = this.btnSetNewFilename;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(292, 69);
			this.Controls.Add(this.txtNewFilename);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSetNewFilename);
			this.Name = "SetNewFileName";
			this.Text = "Enter new filename";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSetNewFilename;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtNewFilename;
    }
}