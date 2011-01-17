using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WatchIT {

	public partial class frmMain : Form {

		private Projects Projects = null; // allocated in constructor
		private ToolTip toolTip = new ToolTip();

		private int SortColumn = 0;

#if DEBUG
		private void d (string s) {
			Console.WriteLine(s);
		}
#else
		private void d (string s) {
		}
#endif

		public frmMain () {
			InitializeComponent();
		}

		private void MainForm_Load (object sender, EventArgs e) {

			this.toolTip.SetToolTip(this.ckbBasename, "Only show the name of the last directory in listing.");

			// 
			this.Location = Properties.Settings.Default.frmMain_Location;
			this.Size = Properties.Settings.Default.frmMain_Size;

			//
			this.columnPath.Width = Properties.Settings.Default.columnPath_Width;
			this.columnChanges.Width = Properties.Settings.Default.columnChanges_Width;

			//
			this.ckbBasename.Checked = Properties.Settings.Default.ckbBasename_Checked;

			// deserialize 
			Serialization ser = new Serialization();
			this.Projects = ser.DeserializeObject<Projects>(Properties.Settings.Default.Projects);

			if (this.Projects == null) {
				this.Projects = new Projects();
			}

			this.Projects.OnAdd += delegate(object s, Project p) {
				ListViewItem l = new ListViewItem();
				l.Tag = p;
				if (!this.ckbBasename.Checked) {
					l.Text = p.Path;
				} else {
					l.Text = p.Path.Substring(p.Path.LastIndexOf('\\') + 1);
				}

				l.SubItems.Add(new ListViewItem.ListViewSubItem() {
					Text = p.Changes.Count.ToString()
				});
				this.lvPaths.Items.Add(l);

				if (p.ShowingWindow == true) {
					p.Window(new frmInfo(p));
					p.Window().Show();
				}

			};

			this.Projects.OnChange += delegate(object s, Project p) {
				foreach (ListViewItem lvi in lvPaths.Items) {
					if ((lvi.Tag as Project) == p) {
						lvi.SubItems[1].Text = p.Changes.Count.ToString();
						return;
					}
				}
			};

			this.Projects.Setup(this); // Ensure stuff for late execution

			// drag and drop
			this.lvPaths.AllowDrop = true;

			this.lvPaths.DragEnter += delegate(object ss, DragEventArgs se) {
				if (se.Data.GetDataPresent(DataFormats.FileDrop)) {
					se.Effect = DragDropEffects.Move;
				}
			};

			this.lvPaths.DragDrop += delegate(object ss, DragEventArgs se) {

				string[] files = (string[])se.Data.GetData(DataFormats.FileDrop);

				foreach (string file in files) {
					if (!System.IO.Directory.Exists(file)) {
						return;
					}
					this.Projects.Add(file);
				}
			};

			// Sortingz
			this.lvPaths.AutoArrange = true; // keep itams sorted
			this.lvPaths.ColumnClick += this.lvPaths_OnColumnClick;
			this.lvPaths.Sorting = (SortOrder)(Properties.Settings.Default.lvPaths_Sorting);
			this.SortColumn = Properties.Settings.Default.lvPaths_SortColumn;
			this.lvPaths.ListViewItemSorter = new ListViewItemComparer(this.SortColumn, this.lvPaths.Sorting);
			this.lvPaths.Sort();

		}

		private void MainForm_FormClosing (object s, FormClosingEventArgs e) {
			this.SaveSettings();
		}

		private void lvPaths_OnColumnClick (object s, ColumnClickEventArgs e) {
			this.SortColumn = e.Column;
			this.lvPaths.Sorting = this.lvPaths.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
			this.lvPaths.ListViewItemSorter = new ListViewItemComparer(e.Column, this.lvPaths.Sorting);
			this.lvPaths.Sort();
		}

		private void SaveSettings () {
			// save projects
			Serialization ser = new Serialization();
			Properties.Settings.Default.Projects = ser.SerializeObject(this.Projects);

			// save sizes for column
			Properties.Settings.Default.columnPath_Width = columnPath.Width;
			Properties.Settings.Default.columnChanges_Width = columnChanges.Width;

			// form
			Properties.Settings.Default.frmMain_Location = this.Location;
			Properties.Settings.Default.frmMain_Size = this.Size;

			// checkbox
			Properties.Settings.Default.ckbBasename_Checked = this.ckbBasename.Checked;

			// Sorting
			Properties.Settings.Default.lvPaths_Sorting = (int)this.lvPaths.Sorting;
			Properties.Settings.Default.lvPaths_SortColumn = this.SortColumn;

			//
			Properties.Settings.Default.Save();
		}
		
		private void UpdateListBox () {
			// force a refresh of the entire listbox
			// TODO: Could there be a hidden gem somewhere that let's us do this better ?
			foreach (ListViewItem lvi in this.lvPaths.Items) {
				Project p = lvi.Tag as Project;
				lvi.Text = (this.ckbBasename.Checked) ? p.Path.Substring(p.Path.LastIndexOf('\\') + 1) : p.Path;
				lvi.SubItems[1].Text = p.Changes.Count.ToString();
			}
		}

		private void addToolStripMenuItem_Click (object sender, EventArgs e) {

			FolderBrowserDialog fbd = new FolderBrowserDialog();
			if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				if (!System.IO.Directory.Exists(fbd.SelectedPath)) {
					throw new Exception("User provided a bad folder path to add as a project.");
				}

				this.Projects.Add(fbd.SelectedPath);
			}

		}

		private void removeToolStripMenuItem_Click (object sender, EventArgs e) {

			foreach (ListViewItem lvi in this.lvPaths.SelectedItems) {
				Project p = lvi.Tag as Project;
				this.lvPaths.Items.RemoveAt(lvi.Index);
				this.Projects.Remove(p);
			}

		}

		private void clearToolStripMenuItem_Click (object sender, EventArgs e) {

			foreach (ListViewItem lvi in this.lvPaths.SelectedItems) {
				(this.lvPaths.SelectedItems[0].Tag as Project).ClearChanges();
			}

		}

		private void lvPaths_DoubleClick (object sender, EventArgs e) {

			foreach (ListViewItem lvi in this.lvPaths.SelectedItems) {

				Project p = lvi.Tag as Project;
				if (!p.ShowingWindow) {
					if (p.Window() == null) {
						p.Window(new frmInfo(p));
					}
				}

				p.Window().Show();
				p.Window().WindowState = FormWindowState.Normal;
				p.Window().BringToFront();

			}

		}

		private void ckbBasename_CheckedChanged (object sender, EventArgs e) {
			this.UpdateListBox();
		}

		private void lvPaths_KeyDown (object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.A && e.Control) {
				foreach (ListViewItem lvi in this.lvPaths.Items) {
					lvi.Selected = true;
				}
				return;
			}

			if (e.KeyCode == Keys.D && e.Control) {
				foreach (ListViewItem lvi in this.lvPaths.Items) {
					lvi.Selected = false;
				}
				return;
			}
		}

	}

} // namespace
