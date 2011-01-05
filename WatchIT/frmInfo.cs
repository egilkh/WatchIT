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

		public frmInfo (Project p) {

			this.Project = p;
			this.Project.ShowingWindow = true;

			InitializeComponent();
			this.UpdateComponents();

			this.timerInfo.Interval = 1000;
		}

		private void frmInfo_Load (object sender, EventArgs e) {
			// databinds
			this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::WatchIT.Properties.Settings.Default, "frmInfo_Position", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.DataBindings.Add(new System.Windows.Forms.Binding("Size", global::WatchIT.Properties.Settings.Default, "frmInfo_Size", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));

			// set the databind stuff
			this.Location = global::WatchIT.Properties.Settings.Default.frmInfo_Position;
			this.Size = global::WatchIT.Properties.Settings.Default.frmInfo_Size;

			this.lvChanges.FullRowSelect = true;
			this.lvChanges.MultiSelect = true;


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
		}

		private void UpdateComponents () {
			this.Text = "Path Info: " + this.Project.Path;

			foreach (Project.Change pc in this.Project.Changes) {
				ListViewItem l = new ListViewItem();
				l.Tag = pc;
				l.Text = pc.Time.ToLongTimeString();
				l.SubItems.Add(new ListViewItem.ListViewSubItem(l, pc.Fullpath.Remove(0, this.Project.Path.Length + 1)));
				l.SubItems.Add(new ListViewItem.ListViewSubItem(l, pc.ChangeType.ToString()));
				this.lvChanges.Items.Add(l);
			}

		}

	}

} // Namespace
