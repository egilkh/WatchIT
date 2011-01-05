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
			this.lvChanges = new System.Windows.Forms.ListView();
			this.columnTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnChange = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
			this.lvChanges.Location = new System.Drawing.Point(12, 12);
			this.lvChanges.Name = "lvChanges";
			this.lvChanges.Size = new System.Drawing.Size(360, 438);
			this.lvChanges.TabIndex = 0;
			this.lvChanges.UseCompatibleStateImageBehavior = false;
			this.lvChanges.View = System.Windows.Forms.View.Details;
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
			// frmInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 462);
			this.Controls.Add(this.lvChanges);
			this.Name = "frmInfo";
			this.ShowInTaskbar = false;
			this.Text = "Path Info";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmInfo_FormClosing);
			this.Load += new System.EventHandler(this.frmInfo_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lvChanges;
		private System.Windows.Forms.ColumnHeader columnTime;
		private System.Windows.Forms.ColumnHeader columnPath;
		private System.Windows.Forms.ColumnHeader columnChange;
	}
}