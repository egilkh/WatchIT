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

		private List<Project> Projects = null; // allocated in constructor
		private List<System.IO.FileSystemWatcher> Watchers = new System.Collections.Generic.List<System.IO.FileSystemWatcher>();

		private Timer FormTimer = new Timer();

		private volatile bool ShouldRefresh = false;

		private ToolTip toolTip = new ToolTip();

		private volatile List<frmInfo> InfoWindows = new List<frmInfo>();

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
			this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::WatchIT.Properties.Settings.Default, "frmMain_Position", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.DataBindings.Add(new System.Windows.Forms.Binding("Size", global::WatchIT.Properties.Settings.Default, "frmMain_Size", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));

			this.Location = global::WatchIT.Properties.Settings.Default.frmMain_Position;
			this.Size = global::WatchIT.Properties.Settings.Default.frmMain_Size;

			//
			this.ckbBasename.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::WatchIT.Properties.Settings.Default, "ckbBasename_Checked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.ckbBasename.Checked = global::WatchIT.Properties.Settings.Default.ckbBasename_Checked;

			//
			this.columnPath.Width = global::WatchIT.Properties.Settings.Default.columnPath_Width;
			this.columnChanges.Width = global::WatchIT.Properties.Settings.Default.columnChanges_Width;


			// deserialize 
			Serialization ser = new Serialization();
			this.Projects = ser.DeserializeObject<List<Project>>(Properties.Settings.Default.Projects);

			if (this.Projects == null) {
				this.Projects = new List<Project>();
			}

			foreach (Project p in Projects) {
				this.AddProjectToList(p);
				this.CreateWatcher(p);
			}

			// force update
			//this.UpdateListBox();

			this.FormTimer.Interval = 100;
			this.FormTimer.Start();

			this.FormTimer.Tick += delegate {
				d("FormTimer.Tick - Start");
				if (this.ShouldRefresh) {
					this.UpdateListBox();
					this.ShouldRefresh = false;
				}
				Console.WriteLine(this.Projects[0].ShowingWindow.ToString());
				d("FormTimer.Tick - End");
			};

			this.AllowDrop = true;
			this.lvPaths.AllowDrop = true;
			this.lvPaths.DragDrop += delegate(object ss, DragEventArgs se) {

				string[] files = (string[])se.Data.GetData(DataFormats.FileDrop);

				foreach (string file in files) {
					// TODO: HOOK UP IF NOT EXISTS AND GO!
					this.AddProject(file);
				}
			};

			this.lvPaths.DragEnter += delegate(object ss, DragEventArgs se) {
				if (se.Data.GetDataPresent(DataFormats.FileDrop)) {
					se.Effect = DragDropEffects.Move;
				}
			};

		}

		private void MainForm_FormClosing (object sender, FormClosingEventArgs e) {

			// save projects and changes
			Serialization ser = new Serialization();
			Properties.Settings.Default.Projects = ser.SerializeObject(this.Projects);

			// save sizes for column
			global::WatchIT.Properties.Settings.Default.columnPath_Width = columnPath.Width;
			global::WatchIT.Properties.Settings.Default.columnChanges_Width = columnChanges.Width;

			Properties.Settings.Default.Save();

		}
		
		private bool CreateWatcher (Project p) {
			if (!System.IO.Directory.Exists(p.Path)) {
				return(false);
			}

			System.IO.FileSystemWatcher f = new System.IO.FileSystemWatcher();
			f.Path = p.Path;
			f.Changed += this.fsw_Action;
			f.Created += this.fsw_Action;
			f.Renamed += this.fsw_Renamed;
			f.Deleted += this.fsw_Action;
			f.IncludeSubdirectories = true;
			f.NotifyFilter = System.IO.NotifyFilters.LastWrite | System.IO.NotifyFilters.FileName;
			f.EnableRaisingEvents = true;
			this.Watchers.Add(f);

			return(true);
		}

		private bool RemoveWatcher (Project p) {

			System.IO.FileSystemWatcher sfw = this.Watchers.SingleOrDefault(w => w.Path.Equals(p.Path));
			sfw.EnableRaisingEvents = false;
			this.Watchers.Remove(sfw);

			return(true);
		}

		private void UpdateListBox () {
			foreach (ListViewItem lvi in this.lvPaths.Items) {
				Project p = lvi.Tag as Project;
				lvi.Text = (this.ckbBasename.Checked) ? p.Path.Substring(p.Path.LastIndexOf('\\') + 1) : p.Path;
				lvi.SubItems[1].Text = p.Changes.Count.ToString();
			}

			return;
		}

		// registers Deleted and Renamed for a file
		private void fsw_Renamed (object sender, System.IO.RenamedEventArgs e) {
			d(e.FullPath + " " + e.ChangeType + " " + e.GetHashCode() + " ");
			fsw_AddEvent(e.OldFullPath, System.IO.WatcherChangeTypes.Deleted);
			fsw_AddEvent(e.FullPath, System.IO.WatcherChangeTypes.Renamed);
		}

		// proxy for actions except Renamed
		private void fsw_Action (object sender, System.IO.FileSystemEventArgs e) {
			d(e.FullPath + " " + e.ChangeType + " " + e.GetHashCode() + " ");
			fsw_AddEvent(e.FullPath, e.ChangeType);
		}

		// adds an event (if needed to a project)
		private void fsw_AddEvent (string fullPath, System.IO.WatcherChangeTypes changeType) {

			d("FSW_AddEvent - Start - [F:" + fullPath + " - T:" + changeType.ToString() + "]");
			Project p = null;
			try {
				p = this.Projects.SingleOrDefault(pj => this.ComparePaths(pj.Path, fullPath));
				//TODO: Pass på at det kun er 1 element i sekvensen, bedre tekst søk
			} catch (Exception e) {
				d("E:" + e.Message);
			}
			
			if (p == null) {
				throw new Exception("Holy tits Batman, this should never happen!");
			}

			d("FSW_AddEvent - I found ze project: " + p.Path);

			// always register 1 of this action for a file
			Project.Change pc = p.Changes.SingleOrDefault(pjc => pjc.Fullpath.Equals(fullPath) && pjc.ChangeType == changeType);
			if (pc == null) {
				d("FSW_AddEvent - New change detected: " + fullPath);
				Project.Change npc = new Project.Change() {
					Time = DateTime.Now,
					ChangeType = changeType,
					Fullpath = fullPath
				};
				p.Changes.Add(npc);

				this.ShouldRefresh = true;

			}

			d("FSW_AddEvent - End");
		}

		private bool ComparePaths (string p1, string p2) {

			// use length of first path's 'seperator' to determine if this is a good match
			string[] parts1 = p1.Split(System.IO.Path.DirectorySeparatorChar);
			string[] parts2 = p2.Split(System.IO.Path.DirectorySeparatorChar);

			if (parts1.Count() > parts2.Count()) { return(false); }

			bool MatchFound = true;

			// inefficient with large/deep paths
			for (int i = 0; i < parts1.Count(); i++) {
				if (!parts1[i].Equals(parts2[i])) {
					MatchFound = false;
					break;
				}
			}

			return(MatchFound);
		}

		private bool AddProject (string path) {

			if (!System.IO.Directory.Exists(path)) {
				return (false);
			}

			Project p = this.Projects.SingleOrDefault(pj => pj.Path.Equals(path));
			if (p == null) {
				Project np = new Project(path);
				this.Projects.Add(np);
				this.CreateWatcher(np);

				this.AddProjectToList(np);

				this.ShouldRefresh = true;

				return(true);
			}
			return (false);
		}

		private void AddProjectToList (Project p) {
			ListViewItem l = new ListViewItem();
			l.Tag = p;
			if (!this.ckbBasename.Checked) {
				l.Text = p.Path;
			} else {
				l.Text = p.Path.Substring(p.Path.LastIndexOf('\\') + 1);
			}

			l.SubItems.Add(new ListViewItem.ListViewSubItem() {
				Text = (l.Tag as Project).Changes.Count.ToString()
			});
			this.lvPaths.Items.Add(l);
		}

		#region Actions for lvPaths

		private void addToolStripMenuItem_Click (object sender, EventArgs e) {

			FolderBrowserDialog fbd = new FolderBrowserDialog();
			if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				if (!System.IO.Directory.Exists(fbd.SelectedPath)) {
					throw new Exception("Holy tits Batman! This is bad stuffs.");
				}

				this.AddProject(fbd.SelectedPath);
			}

		}

		private void removeToolStripMenuItem_Click (object sender, EventArgs e) {

			if (this.lvPaths.SelectedItems.Count < 1) {
				return;
			}

			Project p = this.lvPaths.SelectedItems[0].Tag as Project;
			this.lvPaths.Items.RemoveAt(this.lvPaths.SelectedItems[0].Index);
			this.RemoveWatcher(p);
			this.Projects.Remove(p);
			

			this.ShouldRefresh = true;

		}

		private void clearToolStripMenuItem_Click (object sender, EventArgs e) {
			if (this.lvPaths.SelectedItems.Count < 1) {
				return;
			}

			Project p = this.lvPaths.SelectedItems[0].Tag as Project;
			p.Changes.Clear();
			this.ShouldRefresh = true;
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

		#endregion

		private void ckbBasename_CheckedChanged (object sender, EventArgs e) {
			this.ShouldRefresh = true;
		}

	}

} // namespace
