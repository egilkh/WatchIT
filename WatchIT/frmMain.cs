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
		private void d (string s)
		{
		}
#endif

		public frmMain () {
			InitializeComponent();
		}

		private void MainForm_Load (object sender, EventArgs e) {
			// tooltipz
			this.toolTip.SetToolTip(this.ckbBasename, "Only show the name of the last directory in listing.");

			// databinds
			//this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::WatchIT.Properties.Settings.Default, "frmMain_Position", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			//this.DataBindings.Add(new System.Windows.Forms.Binding("Size", global::WatchIT.Properties.Settings.Default, "frmMain_Size", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));

			this.Location = global::WatchIT.Properties.Settings.Default.frmMain_Location;
			this.Size = global::WatchIT.Properties.Settings.Default.frmMain_Size;

			//
			this.ckbBasename.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::WatchIT.Properties.Settings.Default, "ckbBasename_Checked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.ckbBasename.Checked = global::WatchIT.Properties.Settings.Default.ckbBasename_Checked;

			//
			this.columnPath.Width = global::WatchIT.Properties.Settings.Default.columnPath_Width;
			this.columnChanges.Width = global::WatchIT.Properties.Settings.Default.columnChanges_Width;


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

			// save projects
			Serialization ser = new Serialization();
			Properties.Settings.Default.Projects = ser.SerializeObject(this.Projects);

			// save sizes for column
			global::WatchIT.Properties.Settings.Default.columnPath_Width = columnPath.Width;
			global::WatchIT.Properties.Settings.Default.columnChanges_Width = columnChanges.Width;

			global::WatchIT.Properties.Settings.Default.frmMain_Location = this.Location;
			global::WatchIT.Properties.Settings.Default.frmMain_Size = this.Size;


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
					throw new Exception("Holy tits Batman! This is bad stuffs.");
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
			if (this.lvPaths.SelectedItems.Count < 1) {
				return;
			}

			foreach (ListViewItem lvi in this.lvPaths.SelectedItems) {
				(this.lvPaths.SelectedItems[0].Tag as Project).Changes.Clear();
			}

		}

		private void lvPaths_DoubleClick (object sender, EventArgs e) {
			if (this.lvPaths.SelectedItems.Count < 1) {
				return;
			}

			Project p = this.lvPaths.SelectedItems[0].Tag as Project;
			if (!p.ShowingWindow) {
				frmInfo fi = new frmInfo(p);
				fi.Show();
			}
		}



		private void ckbBasename_CheckedChanged (object sender, EventArgs e) {
			this.UpdateListBox();
		}

	}

} // namespace
