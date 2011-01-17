namespace WatchIT
{
	partial class frmInfo
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInfo));
			this.lvChanges = new System.Windows.Forms.ListView();
			this.columnTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnChange = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lvChangesMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.txtFilter = new System.Windows.Forms.TextBox();
			this.lblFilter = new System.Windows.Forms.Label();
			this.lvChangesMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// lvChanges
			// 
			this.lvChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvChanges.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnTime,
            this.columnPath,
            this.columnChange});
			this.lvChanges.ContextMenuStrip = this.lvChangesMenuStrip;
			this.lvChanges.Location = new System.Drawing.Point(12, 38);
			this.lvChanges.Name = "lvChanges";
			this.lvChanges.Size = new System.Drawing.Size(360, 412);
			this.lvChanges.TabIndex = 0;
			this.lvChanges.UseCompatibleStateImageBehavior = false;
			this.lvChanges.View = System.Windows.Forms.View.Details;
			this.lvChanges.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvChanges_KeyDown);
			// 
			// columnTime
			// 
			this.columnTime.Text = "Time";
			// 
			// columnPath
			// 
			this.columnPath.Text = "File";
			this.columnPath.Width = 236;
			// 
			// columnChange
			// 
			this.columnChange.Text = "Change";
			// 
			// lvChangesMenuStrip
			// 
			this.lvChangesMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
			this.lvChangesMenuStrip.Name = "lvChangesMenuStrip";
			this.lvChangesMenuStrip.Size = new System.Drawing.Size(153, 48);
			// 
			// clearToolStripMenuItem
			// 
			this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
			this.clearToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.clearToolStripMenuItem.Text = "Clear";
			this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
			// 
			// txtFilter
			// 
			this.txtFilter.Location = new System.Drawing.Point(47, 12);
			this.txtFilter.Name = "txtFilter";
			this.txtFilter.Size = new System.Drawing.Size(325, 20);
			this.txtFilter.TabIndex = 1;
			this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
			// 
			// lblFilter
			// 
			this.lblFilter.AutoSize = true;
			this.lblFilter.Location = new System.Drawing.Point(9, 15);
			this.lblFilter.Name = "lblFilter";
			this.lblFilter.Size = new System.Drawing.Size(32, 13);
			this.lblFilter.TabIndex = 2;
			this.lblFilter.Text = "Filter:";
			// 
			// frmInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 462);
			this.Controls.Add(this.lblFilter);
			this.Controls.Add(this.txtFilter);
			this.Controls.Add(this.lvChanges);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmInfo";
			this.ShowInTaskbar = false;
			this.Text = "Path Info";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmInfo_FormClosing);
			this.Load += new System.EventHandler(this.frmInfo_Load);
			this.lvChangesMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView lvChanges;
		private System.Windows.Forms.ColumnHeader columnTime;
		private System.Windows.Forms.ColumnHeader columnPath;
		private System.Windows.Forms.ColumnHeader columnChange;
		private System.Windows.Forms.ContextMenuStrip lvChangesMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
		private System.Windows.Forms.TextBox txtFilter;
		private System.Windows.Forms.Label lblFilter;
	}
}