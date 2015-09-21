namespace Renamer.NewClasses.Util
{
	/// <summary>
	/// Converts a string containing the renaming markers to
	/// a regular expression.
	/// </summary>
    public class RegexConverter
    {
        public static string toRegex(string markedString) {
	        string regexPattern = markedString;
			regexPattern = regexPattern.Replace(RenamingConstants.SHOWNAME_MARKER, "*.?");
			if (regexPattern.Contains(RenamingConstants.SEASON_NR_MARKER + RenamingConstants.EPISODE_NR_MARKER)) {
				regexPattern = regexPattern.Replace(RenamingConstants.SEASON_NR_MARKER, "(?<Season>\\d)");
				regexPattern = regexPattern.Replace(RenamingConstants.EPISODE_NR_MARKER, "(?<Episode>\\d\\d+)");
            }
            else {
				regexPattern = regexPattern.Replace(RenamingConstants.SEASON_NR_MARKER, "(?<Season>\\d+)");
				regexPattern = regexPattern.Replace(RenamingConstants.EPISODE_NR_MARKER, "(?<Episode>\\d+)");
            }
			return regexPattern;
        }

        public static string replaceSeriesname(string markedString, string showname) {
            return markedString.Replace(
				RenamingConstants.SEASON_NR_MARKER, "(?<Season>\\d+)").Replace(
				RenamingConstants.SHOWNAME_MARKER, showname);
        }
        public static string insertSeriesnameAndSeason(string markedString, string showname, int seasonNr)
        {
            return markedString.Replace(
				RenamingConstants.SEASON_NR_MARKER, seasonNr.ToString()).Replace(
				RenamingConstants.SHOWNAME_MARKER, showname);
        }
        public static string insertSeasonNr(string markedString, string season) {
            return markedString.Replace(RenamingConstants.SEASON_NR_MARKER, season);
        }
    }
}
