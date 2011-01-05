using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatchIT {

	public class Project {

		public string Path { get; set; }

		public List<Change> Changes = new List<Change>();

		public bool ShowingWindow = false;

		public class Change {
			public DateTime Time { get; set; }
			public string Fullpath { get; set; }
			public System.IO.WatcherChangeTypes ChangeType { get; set; }
		}

		public Project () {
		}

		public Project (string path) {
			this.Path = path;
		}

	}

} // namespace
