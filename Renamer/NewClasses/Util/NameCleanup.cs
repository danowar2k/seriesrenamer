using System.Text.RegularExpressions;

namespace Renamer.NewClasses.Util
{
    public class NameCleanup {
	    public const string REGEX_RELEASE_GROUP_SUFFIX = "-\\w{2,4}$";

		public const string REGEX_RELEASE_GROUP_PREFIX_FIRST = "([^\\p{Lu}]\\p{Ll}{1,3})";

		public const string REGEX_RELEASE_GROUP_PREFIX_SECOND = "(\\p{Lu}{2,2}\\w)";

	    public const string REGEX_RELEASE_GROUP_PREFIX =
			"^(" + REGEX_RELEASE_GROUP_PREFIX_FIRST + "|" + REGEX_RELEASE_GROUP_PREFIX_SECOND + ")-";

		/// <summary>
		/// Does the following things to a string:
		/// - puts spaces between chars (don't know if this is stupid)
		/// - removes [X], (X) etc.
		/// - trims spaces, brackets, and other special chars
		/// - replaces . and _ with spaces
		/// - removes [] or [TEXT] from string
		/// - removes double spaces
		/// - converts the string to Camel Case
		/// - trims the String again
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
        public static string Postprocessing(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }
            name = Regex.Replace(name, "(?<pos1>[\\p{Ll}\\d])(?<pos2>[\\p{Lu}\\d][\\p{Ll}\\d])|(?<pos1>[^\\d])(?<pos2>\\d)", new MatchEvaluator(insertSpaces));
            name = Regex.Replace(name, "\\[.+\\]|\\(.+\\)", "");
            name = name.Trim('-', '_', '.', ' ', '(', ')', '[', ']');
            name = Regex.Replace(name, "[\\._]", " ");
            name = Regex.Replace(name, "\\[.*\\]", "");
            name = name.Replace("  ", " ");
            if (!StringUtils.containsLowercaseCharacters(name) || !StringUtils.containsUppercaseCharacters(name))
            {
                name = StringUtils.convertToCamelCase(name);
            }
            name = name.Trim();
            return name;
        }
		/// <summary>
		/// For every match, this checks if there are word chars in matches.
		/// If true, puts a space between the groups
		/// </summary>
		/// <param name="m">the regex match</param>
		/// <returns>possibly altered string</returns>
        private static string insertSpaces(Match m)
        {
            //if we only found numbers, we want to skip this tag (i.e. 007)
            foreach (char c in m.Groups["pos1"].Value + m.Groups["pos2"].Value)
            {
                if (!char.IsDigit(c))
                {
                    return m.Groups["pos1"].Value + " " + m.Groups["pos2"].Value;
                }
            }
            return m.Groups["pos1"].Value + m.Groups["pos2"].Value;
        }
		/// <summary>
		/// Remove release group tags from filenames
		/// This is a stupidly naive function
		/// </summary>
		/// <param name="filename">the file name to clean</param>
		/// <returns>the filename without the release group tag</returns>
        public static string removeReleaseGroupTag(string filename)
        {
            // normally 3 to 6 characters at the beginning of the filename seperated by a '-'
            // if filename too short, it might be a part of the real name, so skip it
			if (filename.Length <= 5) {
				return filename;
			}

			filename = Regex.Replace(filename, REGEX_RELEASE_GROUP_PREFIX, "");
			filename = Regex.Replace(filename, REGEX_RELEASE_GROUP_SUFFIX, "");
			return filename;
		}   
    }
}
