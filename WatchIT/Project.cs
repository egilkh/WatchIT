using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatchIT {

	[Serializable]
	public class Project {

		public class Change {
			public DateTime Time { get; set; }
			public string Fullpath { get; set; }
			public System.IO.WatcherChangeTypes ChangeType { get; set; }
		}

		#region Properties
		public string Path { get; set; }

		public List<Change> Changes = new List<Change>();
		
		public bool ShowingWindow = false;

		public delegate void EventHandler (object sender, Change c);
		public event EventHandler OnChange;

		public System.Drawing.Point Location = Properties.Settings.Default.frmInfo_Location;
		public System.Drawing.Size Size = Properties.Settings.Default.frmInfo_Size;

		public int ColumnWidthTime = 60;
		public int ColumnWidthPath = 230;
		public int ColumnWidthChange = 60;
		#endregion

		[NonSerialized]
		System.IO.FileSystemWatcher FSW = new System.IO.FileSystemWatcher();

		public void AddChange (Change c) {
			if (!this.Changes.Contains(c)) {
				this.Changes.Add(c);
				if (this.OnChange != null) {
					this.OnChange(this, c);
				}
			}
		}

		public Project () {
		}

		public Project (string path) {
			this.Path = path;
		}

		~Project () {
			this.FSW.EnableRaisingEvents = false;
			this.FSW.Dispose();
			Console.WriteLine("~Project()");
		}

		public bool Setup (System.Windows.Forms.Form form) {
			Console.WriteLine("Project.Setup()");
			this.FSW.Path = this.Path;
			this.FSW.Changed += this.FSWAction;
			this.FSW.Created += this.FSWAction;
			this.FSW.Deleted += this.FSWAction;
			this.FSW.Renamed += this.FSWRenamed;
			this.FSW.IncludeSubdirectories = true;
			this.FSW.EnableRaisingEvents = true;
			this.FSW.SynchronizingObject = form;
			return (true);
		}

		// ze renamed action
		private void FSWRenamed (object sender, System.IO.RenamedEventArgs e) {
			this.FSWAddEvent(e.OldFullPath, System.IO.WatcherChangeTypes.Deleted);
			this.FSWAddEvent(e.FullPath, System.IO.WatcherChangeTypes.Renamed);
		}

		// proxy for actions except Renamed
		private void FSWAction (object sender, System.IO.FileSystemEventArgs e) {
			this.FSWAddEvent(e.FullPath, e.ChangeType);
		}

		private void FSWAddEvent (string fullPath, System.IO.WatcherChangeTypes changeType) {
			Project.Change pc = this.Changes.SingleOrDefault(pjc => pjc.Fullpath.Equals(fullPath) && pjc.ChangeType == changeType);
			if (pc == null) {
				Project.Change npc = new Project.Change() {
					Time = DateTime.Now,
					ChangeType = changeType,
					Fullpath = fullPath
				};
				this.AddChange(npc);
			}
		}

	}

	[Serializable]
	public class Projects : IEnumerable<Project> {

		public delegate void EventHandler (object sender, Project p);

		// For every change that happens to any project
		public event EventHandler OnChange;
		// Project Added
		public event EventHandler OnAdd;
		// Project Removed
		public event EventHandler OnRemove;
		// When list is cleared
		public event EventHandler OnClear;

		// internal list
		List<Project> projects = new List<Project>();

		public System.Windows.Forms.Form form = null;

		public Projects () {
		}

		public void Setup (System.Windows.Forms.Form form) {
			this.form = form;
			foreach (Project p in this.projects) {
				p.Setup(this.form);
				if (this.OnAdd != null) { this.OnAdd(this, p); }
			}
		}

		public bool Add (string path) {
			Project pp = this.projects.SingleOrDefault(ppp => ppp.Path.Equals(path));
			if (pp == null) {
				this.Add(new Project(path));
				return (true);
			}
			return (false);
		}

		// gets run by the deserializer . must beware of late initialization
		public bool Add (Project p) {
			if (!this.projects.Contains(p)) {

				if (this.form != null) {
					p.Setup(this.form); // Ensure stuff for late execution
				}
				p.OnChange += this.ProjectChange;
				this.projects.Add(p);

				if (this.OnAdd != null) { this.OnAdd(this, p); }

				return (true);
			}
			return (false);
		}

		public void ProjectChange (object sender, Project.Change c) {
			if (this.OnChange != null) {
				this.OnChange(this, (sender as Project));
			}
		}

		public bool Remove (Project p) {
			if (this.projects.Contains(p)) {
				this.projects.Remove(p);

				if (this.OnRemove != null) { this.OnRemove(this, p); }

				return (true);
			}
			return (false);
		}

		public bool Clear () {

			this.projects.Clear();
			// assume if list.count == 0 the list got cleared

			if (this.projects.Count() == 0) {
				if (this.OnClear != null) { this.OnClear(this, null); }
			}

			return (false);
		}

		public int Count () {
			return (this.projects.Count());
		}

		public IEnumerator<Project> GetEnumerator () {
			foreach (Project p in this.projects) {
				yield return p;
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
			return (this.GetEnumerator());
		}
	}

} // namespace
