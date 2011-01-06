using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatchIT {

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
