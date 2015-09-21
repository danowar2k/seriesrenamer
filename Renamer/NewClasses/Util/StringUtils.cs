using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Renamer.NewClasses.Util {
	/// <summary>
	/// Utility functions for working with strings
	/// </summary>
	public class StringUtils {
		/// <summary>
		/// Regular expression for roman numerals (incomplete)
		/// </summary>
		public const string ROMAN_NUMERALS_REGEX = " [iI]+[ $]";

		public const string REGEX_LOWERCASE_CHAR = "\\p{Ll}";

		public const string REGEX_UPPERCASE_CHAR = "\\p{Lu}";

		/// <summary>
		/// Checks if a string contains a series of letters, like hlo in hello
		/// </summary>
		/// <param name="letters">string which contains the letters which will be checked for in the other string</param>
		/// <param name="container">string in which the letters should be contained</param>
		/// <returns>true if container contains those letters, false otherwise</returns>
		public static bool containsEveryLetter(string letters, string container) {
			return letters.All(letter => container.Contains(letter.ToString()));
		}

		/// <summary>
		/// Make the first letter of every word UPPERCASE, words must be sepperated by space
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string convertToCamelCase(string str) {
			TextInfo ti = new CultureInfo("en-US", false).TextInfo;
			return ti.ToTitleCase(str);
		}


		public static bool containsLowercaseCharacters(string str) {
			return Regex.IsMatch(str, REGEX_LOWERCASE_CHAR);
		}
		public static bool containsUppercaseCharacters(string str) {
			return Regex.IsMatch(str, REGEX_UPPERCASE_CHAR);
		}
		public static string buildMultilineText(IEnumerable<string> values, bool toLowerCase = false) {
			StringBuilder b = new StringBuilder();
			foreach (string value in values) {
				b.AppendLine(toLowerCase ? value.ToLower() : value);
			}
			if (b.Length > 0) {
				b.Remove(b.Length - 2, Environment.NewLine.Length);
			}
			return b.ToString();
		}

		public static List<string> multilineTextToList(string multilineText, bool toLowerCase = false) {
			StringReader r = new StringReader(multilineText);
			List<string> lines = new List<string>();
			string line;
			while ((line = r.ReadLine()) != null) {
				lines.Add(toLowerCase ? line.ToLower() : line);
			}
			return lines;
		}
	}
}
