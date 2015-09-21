using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renamer.NewClasses.Util {
	class ListUtils {
		public static List<string> update(List<string> items, string newItem, bool ignoreCase) {
			StringComparer compareType = ignoreCase ? StringComparer.CurrentCultureIgnoreCase : StringComparer.CurrentCulture;
			if (items.Contains(newItem, compareType)) {
				items.Remove(newItem);
			}
			items.Insert(0, newItem);
			return items;
		}
		public static List<string> updateStartsWith(List<string> items, string newItem) {
			string matchedLanguageEntry = "";
			foreach (string item in items) {
				if (item.StartsWith(newItem)) {
					matchedLanguageEntry = item;
					break;
				}
			}
			if (!String.IsNullOrEmpty(matchedLanguageEntry)) {
				items.Remove(matchedLanguageEntry);
				items.Insert(0, matchedLanguageEntry);
			} else {
				items.Insert(0, newItem);
			}
			return items;
		}
	}
}
