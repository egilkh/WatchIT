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
			this.Location = global::WatchIT.Properties.Settings.Default.frmMain_Location;
			this.Size = global::WatchIT.Properties.Settings.Default.frmMain_Size;

			//
			this.columnPath.Width = global::WatchIT.Properties.Settings.Default.columnPath_Width;
			this.columnChanges.Width = global::WatchIT.Properties.Settings.Default.columnChanges_Width;

			//
			//this.ckbBasename.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::WatchIT.Properties.Settings.Default, "ckbBasename_Checked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.ckbBasename.Checked = global::WatchIT.Properties.Settings.Default.ckbBasename_Checked;

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
					// TODO: HOOK UP IF NOT EXISTS AND GO!
					if (!System.IO.Directory.Exists(file)) {
						return;
					}
					this.Projects.Add(file);
				}
			};

		}

		private void MainForm_FormClosing (object sender, FormClosingEventArgs e) {
			this.SaveSettings();
		}

		private void SaveSettings () {
			// save projects
			Serialization ser = new Serialization();
			Properties.Settings.Default.Projects = ser.SerializeObject(this.Projects);

			// save sizes for column
			global::WatchIT.Properties.Settings.Default.columnPath_Width = columnPath.Width;
			global::WatchIT.Properties.Settings.Default.columnChanges_Width = columnChanges.Width;

			// form
			global::WatchIT.Properties.Settings.Default.frmMain_Location = this.Location;
			global::WatchIT.Properties.Settings.Default.frmMain_Size = this.Size;

			// checkbox
			global::WatchIT.Properties.Settings.Default.ckbBasename_Checked = this.ckbBasename.Checked;

			//
			Properties.Settings.Default.Save();
		}
		
		private void UpdateListBox () {
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

	}

} // namespace
