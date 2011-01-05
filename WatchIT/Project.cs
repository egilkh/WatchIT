using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatchIT {

	public class Project {

		public string Path { get; set; }

		public List<Change> Changes = new List<Change>();
		
		public bool ShowingWindow = false;

		public event EventHandler OnChange;

		public System.Drawing.Point Location = Properties.Settings.Default.frmInfo_Position;
		public System.Drawing.Size Size = Properties.Settings.Default.frmInfo_Size;

		public int ColumnWidthTime = 60;
		public int ColumnWidthPath = 230;
		public int ColumnWidthChange = 60;

		public class Change {
			public DateTime Time { get; set; }
			public string Fullpath { get; set; }
			public System.IO.WatcherChangeTypes ChangeType { get; set; }
		}

		public void AddChange (Change c) {
			if (!this.Changes.Contains(c)) {
				this.Changes.Add(c);
				if (this.OnChange != null) {
					this.OnChange(this, null);
				}
			}
		}

		public Project () {
		}

		public Project (string path) {
			this.Path = path;
		}

	}

} // namespace
