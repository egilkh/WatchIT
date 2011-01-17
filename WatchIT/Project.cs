using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatchIT {

	[Serializable]
	public class Project {

		[NonSerialized]
		private System.IO.FileSystemWatcher FSW = new System.IO.FileSystemWatcher();

		[NonSerialized]
		private System.Windows.Forms.Form window = null;

		public class Change {
			public DateTime First { get; set; }
			public DateTime Last { get; set; }
			public string Fullpath { get; set; }
			public System.IO.WatcherChangeTypes ChangeType { get; set; }
		}

		// Events
		public delegate void EventHandler (object sender, Change c);
		public event EventHandler OnChange;

		// These should get serialized
		#region Serializeable Properties
		public string Path { get; set; }

		public List<Change> Changes = new List<Change>();
		
		public bool ShowingWindow = false;

		public System.Drawing.Point Location = Properties.Settings.Default.frmInfo_Location;
		public System.Drawing.Size Size = Properties.Settings.Default.frmInfo_Size;

		public int ColumnWidthTime = 60;
		public int ColumnWidthPath = 230;
		public int ColumnWidthChange = 60;

		public int SortColumn = 0;
		public System.Windows.Forms.SortOrder SortOrder = System.Windows.Forms.SortOrder.Ascending;

		public string Filter = "";
		#endregion

		public System.Windows.Forms.Form Window () {
			return (this.window);
		}

		public System.Windows.Forms.Form Window (System.Windows.Forms.Form f) {
			this.window = f;
			f.FormClosed += delegate {
				this.window = null;
			};
			return this.window;
		}

		public void AddChange (Change c) {
			if (!this.Changes.Contains(c)) {
				this.Changes.Add(c);
				if (this.OnChange != null) { this.OnChange(this, c); }
			}
		}

		public void RemoveChange (Change c) {
			if (this.Changes.Contains(c)) {
				this.Changes.Remove(c);
				if (this.OnChange != null) { this.OnChange(this, null); }
			}
		}

		public void ClearChanges () {
			this.Changes.Clear();
			if (this.OnChange != null) { this.OnChange(this, null); }
		}

		public Project () {
		}

		public Project (string path) {
			this.Path = path;
		}

		~Project () {
			this.FSW.EnableRaisingEvents = false;
			this.FSW.Dispose();
			if (this.window != null) {
				this.Window().Close();
			}
		}

		public bool Setup (System.Windows.Forms.Form form) {
			this.FSW.Path = this.Path;
			this.FSW.Changed += this.FSW_Action;
			this.FSW.Created += this.FSW_Action;
			this.FSW.Deleted += this.FSW_Action;
			this.FSW.Renamed += this.FSW_Renamed;
			this.FSW.IncludeSubdirectories = true;
			this.FSW.EnableRaisingEvents = true;
			this.FSW.SynchronizingObject = form;
			return (true);
		}

		// Proxy for all events
		private void FSW_Action (object sender, System.IO.FileSystemEventArgs e) {
			this.FSW_AddEvent(e.FullPath, e.ChangeType);
		}

		private void FSW_Renamed (object sender, System.IO.RenamedEventArgs e) {
			this.FSW_AddEvent(e.OldFullPath, System.IO.WatcherChangeTypes.Deleted);
			this.FSW_AddEvent(e.FullPath, System.IO.WatcherChangeTypes.Renamed);
		}

		private void FSW_AddEvent (string fullPath, System.IO.WatcherChangeTypes changeType) {
			Project.Change pc = this.Changes.SingleOrDefault(pjc => pjc.Fullpath.Equals(fullPath) && pjc.ChangeType == changeType);
			if (pc == null) {
				Project.Change npc = new Project.Change() {
					First = DateTime.Now,
					Last = DateTime.Now,
					ChangeType = changeType,
					Fullpath = fullPath
				};
				this.AddChange(npc);
			} else {
				pc.Last = DateTime.Now;
			}
		}

	}

} // namespace
