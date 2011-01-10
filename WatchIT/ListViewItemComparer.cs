using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace WatchIT {

	class ListViewItemComparer : IComparer {
		private int Column = 0;
		private SortOrder Order = SortOrder.Ascending;

		public ListViewItemComparer () {
		}

		public ListViewItemComparer (int column, SortOrder order) {
			this.Column = column;
			this.Order = order;
		}

		public int Compare (object x, object y) {
			int rv = -1;
			rv = string.Compare((x as ListViewItem).SubItems[this.Column].Text, (y as ListViewItem).SubItems[this.Column].Text);
			if (this.Order == SortOrder.Descending) {
				rv *= -1;
			}
			return (rv);
		}

	}

} // namespace
