namespace WatchIT
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
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
		private void InitializeComponent ()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.lvPaths = new System.Windows.Forms.ListView();
			this.columnPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnChanges = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lvPathsMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ckbBasename = new System.Windows.Forms.CheckBox();
			this.lblBottom = new System.Windows.Forms.Label();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lvPathsMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// lvPaths
			// 
			this.lvPaths.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvPaths.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnPath,
            this.columnChanges});
			this.lvPaths.ContextMenuStrip = this.lvPathsMenuStrip;
			this.lvPaths.FullRowSelect = true;
			this.lvPaths.Location = new System.Drawing.Point(12, 12);
			this.lvPaths.Name = "lvPaths";
			this.lvPaths.Size = new System.Drawing.Size(360, 222);
			this.lvPaths.TabIndex = 0;
			this.lvPaths.UseCompatibleStateImageBehavior = false;
			this.lvPaths.View = System.Windows.Forms.View.Details;
			this.lvPaths.DoubleClick += new System.EventHandler(this.lvPaths_DoubleClick);
			this.lvPaths.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvPaths_KeyDown);
			// 
			// columnPath
			// 
			this.columnPath.Text = "Path";
			// 
			// columnChanges
			// 
			this.columnChanges.Text = "Changes";
			this.columnChanges.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lvPathsMenuStrip
			// 
			this.lvPathsMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.toolStripSeparator1,
            this.openFolderToolStripMenuItem});
			this.lvPathsMenuStrip.Name = "contextMenuStrip1";
			this.lvPathsMenuStrip.Size = new System.Drawing.Size(153, 120);
			// 
			// addToolStripMenuItem
			// 
			this.addToolStripMenuItem.Name = "addToolStripMenuItem";
			this.addToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.addToolStripMenuItem.Text = "Add";
			this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
			// 
			// clearToolStripMenuItem
			// 
			this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
			this.clearToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.clearToolStripMenuItem.Text = "Clear";
			this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
			// 
			// removeToolStripMenuItem
			// 
			this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
			this.removeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.removeToolStripMenuItem.Text = "Remove";
			this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
			// 
			// ckbBasename
			// 
			this.ckbBasename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ckbBasename.AutoSize = true;
			this.ckbBasename.Location = new System.Drawing.Point(290, 240);
			this.ckbBasename.Name = "ckbBasename";
			this.ckbBasename.Size = new System.Drawing.Size(82, 17);
			this.ckbBasename.TabIndex = 1;
			this.ckbBasename.Text = "Short Name";
			this.ckbBasename.UseVisualStyleBackColor = true;
			this.ckbBasename.CheckedChanged += new System.EventHandler(this.ckbBasename_CheckedChanged);
			// 
			// lblBottom
			// 
			this.lblBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblBottom.Location = new System.Drawing.Point(12, 237);
			this.lblBottom.Name = "lblBottom";
			this.lblBottom.Size = new System.Drawing.Size(272, 20);
			this.lblBottom.TabIndex = 2;
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
			// 
			// openFolderToolStripMenuItem
			// 
			this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
			this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.openFolderToolStripMenuItem.Text = "Open Folder";
			this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 269);
			this.Controls.Add(this.lblBottom);
			this.Controls.Add(this.ckbBasename);
			this.Controls.Add(this.lvPaths);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmMain";
			this.Text = "WatchIT";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.lvPathsMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView lvPaths;
		private System.Windows.Forms.ColumnHeader columnPath;
		private System.Windows.Forms.ColumnHeader columnChanges;
		private System.Windows.Forms.ContextMenuStrip lvPathsMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
		private System.Windows.Forms.CheckBox ckbBasename;
		private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
		private System.Windows.Forms.Label lblBottom;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;

	}
}

