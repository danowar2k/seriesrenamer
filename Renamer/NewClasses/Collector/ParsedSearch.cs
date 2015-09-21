using System.Collections;
using Renamer.NewClasses.Providers;

namespace Renamer.NewClasses.Collector {
	public class ParsedSearch {
		public string SearchString;

		public string Showname;

		public AbstractProvider provider;

		public Hashtable Results;

		public string SelectedResult = "";

		public string ToString() {
			return SelectedResult;
		}
	}
}
