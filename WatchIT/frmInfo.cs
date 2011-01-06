﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WatchIT {
	public partial class frmInfo : Form {
		public Project Project = null;

		private Timer timerInfo = new Timer();

		private ToolTip ToolTip = new ToolTip();

		public frmInfo (Project p) {

			this.Project = p;
			this.Project.ShowingWindow = true;

			InitializeComponent();
			this.UpdateComponents();

			this.Project.OnChange += this.UpdateChanges;
		}

		private void frmInfo_Load (object sender, EventArgs e) {

			this.Move += delegate (object s, EventArgs ee) {
				this.Project.Location = this.Location;
			};

			this.Resize += delegate {
				this.Project.Size = this.Size;
			};
			
			this.Location = this.Project.Location;
			this.Size = this.Project.Size;

			this.lvChanges.FullRowSelect = true;
			this.lvChanges.MultiSelect = true;
			this.lvChanges.ShowItemToolTips = true;

			this.columnTime.Width = this.Project.ColumnWidthTime;
			this.columnPath.Width = this.Project.ColumnWidthPath;
			this.columnChange.Width = this.Project.ColumnWidthChange;

			this.lvChanges.MouseMove += delegate(object so, MouseEventArgs se) {

				if (se.Button != System.Windows.Forms.MouseButtons.Left) {
					return;
				}

				if (this.lvChanges.SelectedItems.Count > 0) {

					List<string> files = new List<string>();

					foreach (ListViewItem lvi in this.lvChanges.SelectedItems) {

						Project.Change pc = lvi.Tag as Project.Change;

						if (pc.ChangeType != System.IO.WatcherChangeTypes.Deleted) {
							Console.WriteLine(pc.Fullpath);
							files.Add(pc.Fullpath);
						}

					}

					// no need to create dragdrop with no files
					if (files.Count > 0) {
						this.DoDragDrop(new DataObject(DataFormats.FileDrop, files.ToArray()), DragDropEffects.Copy);
					}
				}

			};
		}

		private void frmInfo_FormClosing (object sender, FormClosingEventArgs e) {
			this.Project.ShowingWindow = false;

			this.Project.Location = this.Location;
			this.Project.Size = this.Size;

			this.Project.ColumnWidthTime = this.columnTime.Width;
			this.Project.ColumnWidthPath = this.columnPath.Width;
			this.Project.ColumnWidthChange = this.columnChange.Width;
		}

		private void UpdateComponents () {
			this.Text = "Path Info: " + this.Project.Path;

			foreach (Project.Change c in this.Project.Changes) {
				this.AddChangeToList(c);
			}
		}

		private void UpdateChanges (object sender, Project.Change c) {

			if (c == null && this.Project.Changes.Count == 0) {
				this.lvChanges.Items.Clear();
			}

			foreach (ListViewItem lvi in this.lvChanges.Items) {
				Project.Change cc = lvi.Tag as Project.Change;
				if (cc == c) {
					return;
				}
			}

			this.AddChangeToList(c);
		}

		private void AddChangeToList (Project.Change c) {

			if (c == null) {
				return;
			}

			ListViewItem l = new ListViewItem();
			l.Tag = c;
			l.Text = c.Time.ToLongTimeString();
			l.SubItems.Add(new ListViewItem.ListViewSubItem(l, c.Fullpath.Remove(0, this.Project.Path.Length + 1)));
			l.SubItems.Add(new ListViewItem.ListViewSubItem(l, c.ChangeType.ToString()));
			l.ToolTipText = "Added and such!";
			this.lvChanges.Items.Add(l);

			this.lvChanges.Update();
		}

	}

} // Namespace
