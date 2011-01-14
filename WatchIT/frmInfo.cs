using System;
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

			this.lvChanges.ColumnClick += this.lvChanges_OnColumnClick;
			// initial sort
			this.lvChanges.Sorting = this.Project.SortOrder;
			this.lvChanges.ListViewItemSorter = new ListViewItemComparer(this.Project.SortColumn, this.Project.SortOrder);
			this.lvChanges.Sort();
		}

		private void lvChanges_OnColumnClick (object s, ColumnClickEventArgs e) {
			this.Project.SortOrder = (this.Project.SortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
			this.Project.SortColumn = e.Column;
			this.lvChanges.Sorting = this.Project.SortOrder;
			this.lvChanges.ListViewItemSorter = new ListViewItemComparer(this.Project.SortColumn, this.Project.SortOrder);
			this.lvChanges.Sort();
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
							files.Add(pc.Fullpath);
						}

					}

					// no need to create dragdrop with no files
					if (files.Count > 0) {
						files.ForEach(f => Console.WriteLine(f));
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
				// happens when main window does a Clear on a project
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

		private void clearToolStripMenuItem_Click (object sender, EventArgs e) {

			foreach (ListViewItem lvi in this.lvChanges.SelectedItems) {

				Project.Change pc = lvi.Tag as Project.Change;
				this.Project.RemoveChange(pc);
				this.lvChanges.Items.Remove(lvi);

			}

		}

		private void lvChanges_KeyDown (object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.A && e.Control) {
				foreach (ListViewItem lvi in this.lvChanges.Items) {
					lvi.Selected = true;
				}
				return;
			}

			if (e.KeyCode == Keys.D && e.Control) {
				foreach (ListViewItem lvi in this.lvChanges.Items) {
					lvi.Selected = false;
				}
				return;
			}
		}

	}

} // Namespace
