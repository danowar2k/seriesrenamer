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

namespace Renamer.Dialogs
{
	partial class AboutBox
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
            this.btnShowDetails = new System.Windows.Forms.Button();
            this.lblApplicationBuildDate = new System.Windows.Forms.Label();
            this.btnSystemInfo = new System.Windows.Forms.Button();
            this.lblApplicationCopyright = new System.Windows.Forms.Label();
            this.lblApplicationVersion = new System.Windows.Forms.Label();
            this.lblApplicationDescription = new System.Windows.Forms.Label();
            this.grpSomeBox = new System.Windows.Forms.GroupBox();
            this.lblApplicationTitle = new System.Windows.Forms.Label();
            this.btnOkAndClose = new System.Windows.Forms.Button();
            this.txtAdditionalDescription = new System.Windows.Forms.RichTextBox();
            this.tabPanelDetails = new System.Windows.Forms.TabControl();
            this.tabPageApplication = new System.Windows.Forms.TabPage();
            this.lstAppInfo = new System.Windows.Forms.ListView();
            this.colApplicationKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colApplicationValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPageAssemblies = new System.Windows.Forms.TabPage();
            this.lstAssemblyInfo = new System.Windows.Forms.ListView();
            this.colAssemblyName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAssemblyVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAssemblyBuilt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAssemblyCodeBase = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPageAssemblyDetails = new System.Windows.Forms.TabPage();
            this.lstAssemblyDetails = new System.Windows.Forms.ListView();
            this.colAssemblyKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAssemblyValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbAssemblyNames = new System.Windows.Forms.ComboBox();
            this.picApplicationIcon = new System.Windows.Forms.PictureBox();
            this.tabPanelDetails.SuspendLayout();
            this.tabPageApplication.SuspendLayout();
            this.tabPageAssemblies.SuspendLayout();
            this.tabPageAssemblyDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picApplicationIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnShowDetails
            // 
            this.btnShowDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowDetails.Location = new System.Drawing.Point(228, 245);
            this.btnShowDetails.Name = "btnShowDetails";
            this.btnShowDetails.Size = new System.Drawing.Size(76, 23);
            this.btnShowDetails.TabIndex = 25;
            this.btnShowDetails.Text = "&Details >>";
            this.btnShowDetails.Click += new System.EventHandler(this.btnShowDetails_Click);
            // 
            // lblApplicationBuildDate
            // 
            this.lblApplicationBuildDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblApplicationBuildDate.Location = new System.Drawing.Point(6, 79);
            this.lblApplicationBuildDate.Name = "lblApplicationBuildDate";
            this.lblApplicationBuildDate.Size = new System.Drawing.Size(380, 16);
            this.lblApplicationBuildDate.TabIndex = 23;
            this.lblApplicationBuildDate.Text = "Built on %builddate%";
            // 
            // btnSystemInfo
            // 
            this.btnSystemInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSystemInfo.Location = new System.Drawing.Point(212, 245);
            this.btnSystemInfo.Name = "btnSystemInfo";
            this.btnSystemInfo.Size = new System.Drawing.Size(92, 23);
            this.btnSystemInfo.TabIndex = 22;
            this.btnSystemInfo.Text = "&System Info...";
            this.btnSystemInfo.Visible = false;
            this.btnSystemInfo.Click += new System.EventHandler(this.btnSystemInfo_Click);
            // 
            // lblApplicationCopyright
            // 
            this.lblApplicationCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblApplicationCopyright.Location = new System.Drawing.Point(6, 99);
            this.lblApplicationCopyright.Name = "lblApplicationCopyright";
            this.lblApplicationCopyright.Size = new System.Drawing.Size(380, 16);
            this.lblApplicationCopyright.TabIndex = 21;
            this.lblApplicationCopyright.Text = "Copyright © %year%, %company%";
            // 
            // lblApplicationVersion
            // 
            this.lblApplicationVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblApplicationVersion.Location = new System.Drawing.Point(6, 59);
            this.lblApplicationVersion.Name = "lblApplicationVersion";
            this.lblApplicationVersion.Size = new System.Drawing.Size(380, 16);
            this.lblApplicationVersion.TabIndex = 20;
            this.lblApplicationVersion.Text = "Version %version%";
            // 
            // lblApplicationDescription
            // 
            this.lblApplicationDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblApplicationDescription.Location = new System.Drawing.Point(58, 27);
            this.lblApplicationDescription.Name = "lblApplicationDescription";
            this.lblApplicationDescription.Size = new System.Drawing.Size(328, 16);
            this.lblApplicationDescription.TabIndex = 19;
            this.lblApplicationDescription.Text = "%description%";
            // 
            // grpSomeBox
            // 
            this.grpSomeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSomeBox.Location = new System.Drawing.Point(6, 47);
            this.grpSomeBox.Name = "grpSomeBox";
            this.grpSomeBox.Size = new System.Drawing.Size(380, 2);
            this.grpSomeBox.TabIndex = 18;
            this.grpSomeBox.TabStop = false;
            this.grpSomeBox.Text = "grpSomeBox";
            // 
            // lblApplicationTitle
            // 
            this.lblApplicationTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblApplicationTitle.Location = new System.Drawing.Point(58, 7);
            this.lblApplicationTitle.Name = "lblApplicationTitle";
            this.lblApplicationTitle.Size = new System.Drawing.Size(328, 16);
            this.lblApplicationTitle.TabIndex = 17;
            this.lblApplicationTitle.Text = "%title%";
            // 
            // btnOkAndClose
            // 
            this.btnOkAndClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkAndClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOkAndClose.Location = new System.Drawing.Point(312, 245);
            this.btnOkAndClose.Name = "btnOkAndClose";
            this.btnOkAndClose.Size = new System.Drawing.Size(76, 23);
            this.btnOkAndClose.TabIndex = 16;
            this.btnOkAndClose.Text = "OK";
            // 
            // txtAdditionalDescription
            // 
            this.txtAdditionalDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAdditionalDescription.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtAdditionalDescription.Location = new System.Drawing.Point(6, 123);
            this.txtAdditionalDescription.Name = "txtAdditionalDescription";
            this.txtAdditionalDescription.ReadOnly = true;
            this.txtAdditionalDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtAdditionalDescription.Size = new System.Drawing.Size(380, 114);
            this.txtAdditionalDescription.TabIndex = 26;
            this.txtAdditionalDescription.Text = "%product% is %copyright%, %trademark%";
            this.txtAdditionalDescription.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.txtAdditionalDescription_LinkClicked);
            // 
            // tabPanelDetails
            // 
            this.tabPanelDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabPanelDetails.Controls.Add(this.tabPageApplication);
            this.tabPanelDetails.Controls.Add(this.tabPageAssemblies);
            this.tabPanelDetails.Controls.Add(this.tabPageAssemblyDetails);
            this.tabPanelDetails.Location = new System.Drawing.Point(6, 123);
            this.tabPanelDetails.Name = "tabPanelDetails";
            this.tabPanelDetails.SelectedIndex = 0;
            this.tabPanelDetails.Size = new System.Drawing.Size(378, 114);
            this.tabPanelDetails.TabIndex = 27;
            this.tabPanelDetails.Visible = false;
            this.tabPanelDetails.SelectedIndexChanged += new System.EventHandler(this.tabPanelDetails_SelectedIndexChanged);
            // 
            // tabPageApplication
            // 
            this.tabPageApplication.Controls.Add(this.lstAppInfo);
            this.tabPageApplication.Location = new System.Drawing.Point(4, 22);
            this.tabPageApplication.Name = "tabPageApplication";
            this.tabPageApplication.Size = new System.Drawing.Size(370, 88);
            this.tabPageApplication.TabIndex = 0;
            this.tabPageApplication.Text = "Application";
            // 
            // lstAppInfo
            // 
            this.lstAppInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colApplicationKey,
            this.colApplicationValue});
            this.lstAppInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAppInfo.FullRowSelect = true;
            this.lstAppInfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstAppInfo.Location = new System.Drawing.Point(0, 0);
            this.lstAppInfo.Name = "lstAppInfo";
            this.lstAppInfo.Size = new System.Drawing.Size(370, 88);
            this.lstAppInfo.TabIndex = 16;
            this.lstAppInfo.UseCompatibleStateImageBehavior = false;
            this.lstAppInfo.View = System.Windows.Forms.View.Details;
            // 
            // colApplicationKey
            // 
            this.colApplicationKey.Text = "Application Key";
            this.colApplicationKey.Width = 120;
            // 
            // colApplicationValue
            // 
            this.colApplicationValue.Text = "Value";
            this.colApplicationValue.Width = 700;
            // 
            // tabPageAssemblies
            // 
            this.tabPageAssemblies.Controls.Add(this.lstAssemblyInfo);
            this.tabPageAssemblies.Location = new System.Drawing.Point(4, 22);
            this.tabPageAssemblies.Name = "tabPageAssemblies";
            this.tabPageAssemblies.Size = new System.Drawing.Size(370, 88);
            this.tabPageAssemblies.TabIndex = 1;
            this.tabPageAssemblies.Text = "Assemblies";
            // 
            // lstAssemblyInfo
            // 
            this.lstAssemblyInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAssemblyName,
            this.colAssemblyVersion,
            this.colAssemblyBuilt,
            this.colAssemblyCodeBase});
            this.lstAssemblyInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAssemblyInfo.FullRowSelect = true;
            this.lstAssemblyInfo.Location = new System.Drawing.Point(0, 0);
            this.lstAssemblyInfo.MultiSelect = false;
            this.lstAssemblyInfo.Name = "lstAssemblyInfo";
            this.lstAssemblyInfo.Size = new System.Drawing.Size(370, 88);
            this.lstAssemblyInfo.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstAssemblyInfo.TabIndex = 13;
            this.lstAssemblyInfo.UseCompatibleStateImageBehavior = false;
            this.lstAssemblyInfo.View = System.Windows.Forms.View.Details;
            this.lstAssemblyInfo.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstAssemblyInfo_ColumnClick);
            this.lstAssemblyInfo.DoubleClick += new System.EventHandler(this.lstAssemblyInfo_DoubleClick);
            // 
            // colAssemblyName
            // 
            this.colAssemblyName.Text = "Assembly";
            this.colAssemblyName.Width = 123;
            // 
            // colAssemblyVersion
            // 
            this.colAssemblyVersion.Text = "Version";
            this.colAssemblyVersion.Width = 100;
            // 
            // colAssemblyBuilt
            // 
            this.colAssemblyBuilt.Text = "Built";
            this.colAssemblyBuilt.Width = 130;
            // 
            // colAssemblyCodeBase
            // 
            this.colAssemblyCodeBase.Text = "CodeBase";
            this.colAssemblyCodeBase.Width = 750;
            // 
            // tabPageAssemblyDetails
            // 
            this.tabPageAssemblyDetails.Controls.Add(this.lstAssemblyDetails);
            this.tabPageAssemblyDetails.Controls.Add(this.cbAssemblyNames);
            this.tabPageAssemblyDetails.Location = new System.Drawing.Point(4, 22);
            this.tabPageAssemblyDetails.Name = "tabPageAssemblyDetails";
            this.tabPageAssemblyDetails.Size = new System.Drawing.Size(370, 88);
            this.tabPageAssemblyDetails.TabIndex = 2;
            this.tabPageAssemblyDetails.Text = "Assembly Details";
            // 
            // lstAssemblyDetails
            // 
            this.lstAssemblyDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAssemblyKey,
            this.colAssemblyValue});
            this.lstAssemblyDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAssemblyDetails.FullRowSelect = true;
            this.lstAssemblyDetails.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstAssemblyDetails.Location = new System.Drawing.Point(0, 21);
            this.lstAssemblyDetails.Name = "lstAssemblyDetails";
            this.lstAssemblyDetails.Size = new System.Drawing.Size(370, 67);
            this.lstAssemblyDetails.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstAssemblyDetails.TabIndex = 19;
            this.lstAssemblyDetails.UseCompatibleStateImageBehavior = false;
            this.lstAssemblyDetails.View = System.Windows.Forms.View.Details;
            // 
            // colAssemblyKey
            // 
            this.colAssemblyKey.Text = "Assembly Key";
            this.colAssemblyKey.Width = 120;
            // 
            // colAssemblyValue
            // 
            this.colAssemblyValue.Text = "Value";
            this.colAssemblyValue.Width = 700;
            // 
            // cbAssemblyNames
            // 
            this.cbAssemblyNames.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbAssemblyNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAssemblyNames.Location = new System.Drawing.Point(0, 0);
            this.cbAssemblyNames.Name = "cbAssemblyNames";
            this.cbAssemblyNames.Size = new System.Drawing.Size(370, 21);
            this.cbAssemblyNames.Sorted = true;
            this.cbAssemblyNames.TabIndex = 18;
            this.cbAssemblyNames.SelectedIndexChanged += new System.EventHandler(this.cbAssemblyNames_SelectedIndexChanged);
            // 
            // picApplicationIcon
            // 
            this.picApplicationIcon.Location = new System.Drawing.Point(14, 7);
            this.picApplicationIcon.Name = "applicationIcon";
            this.picApplicationIcon.Size = new System.Drawing.Size(32, 32);
            this.picApplicationIcon.TabIndex = 24;
            this.picApplicationIcon.TabStop = false;
            // 
            // AboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOkAndClose;
            this.ClientSize = new System.Drawing.Size(394, 275);
            this.Controls.Add(this.btnShowDetails);
            this.Controls.Add(this.picApplicationIcon);
            this.Controls.Add(this.lblApplicationBuildDate);
            this.Controls.Add(this.btnSystemInfo);
            this.Controls.Add(this.lblApplicationCopyright);
            this.Controls.Add(this.lblApplicationVersion);
            this.Controls.Add(this.lblApplicationDescription);
            this.Controls.Add(this.grpSomeBox);
            this.Controls.Add(this.lblApplicationTitle);
            this.Controls.Add(this.btnOkAndClose);
            this.Controls.Add(this.txtAdditionalDescription);
            this.Controls.Add(this.tabPanelDetails);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About %title%";
            this.Load += new System.EventHandler(this.AboutBox_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AboutBox_Paint);
            this.tabPanelDetails.ResumeLayout(false);
            this.tabPageApplication.ResumeLayout(false);
            this.tabPageAssemblies.ResumeLayout(false);
            this.tabPageAssemblyDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picApplicationIcon)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnShowDetails;
		private System.Windows.Forms.PictureBox picApplicationIcon;
		private System.Windows.Forms.Label lblApplicationBuildDate;
		private System.Windows.Forms.Button btnSystemInfo;
		private System.Windows.Forms.Label lblApplicationCopyright;
		private System.Windows.Forms.Label lblApplicationVersion;
		private System.Windows.Forms.Label lblApplicationDescription;
		private System.Windows.Forms.GroupBox grpSomeBox;
		private System.Windows.Forms.Label lblApplicationTitle;
		private System.Windows.Forms.Button btnOkAndClose;
		internal System.Windows.Forms.RichTextBox txtAdditionalDescription;
		internal System.Windows.Forms.TabControl tabPanelDetails;
		internal System.Windows.Forms.TabPage tabPageApplication;
		internal System.Windows.Forms.ListView lstAppInfo;
		internal System.Windows.Forms.ColumnHeader colApplicationKey;
		internal System.Windows.Forms.ColumnHeader colApplicationValue;
		internal System.Windows.Forms.TabPage tabPageAssemblies;
		internal System.Windows.Forms.ListView lstAssemblyInfo;
		internal System.Windows.Forms.ColumnHeader colAssemblyName;
		internal System.Windows.Forms.ColumnHeader colAssemblyVersion;
		internal System.Windows.Forms.ColumnHeader colAssemblyBuilt;
		internal System.Windows.Forms.ColumnHeader colAssemblyCodeBase;
		internal System.Windows.Forms.TabPage tabPageAssemblyDetails;
		internal System.Windows.Forms.ListView lstAssemblyDetails;
		internal System.Windows.Forms.ColumnHeader colAssemblyKey;
		internal System.Windows.Forms.ColumnHeader colAssemblyValue;
		internal System.Windows.Forms.ComboBox cbAssemblyNames;
	}
}