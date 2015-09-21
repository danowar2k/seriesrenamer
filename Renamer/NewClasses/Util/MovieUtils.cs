using System.Collections.Generic;
using Renamer.Classes;

namespace Renamer.NewClasses.Util {
	/// <summary>
	/// Utility functions used when working with movies (not really though)
	/// </summary>
	public class MovieUtils {
		/// <summary>
		/// Given a set of MediaFiles, returns those that are in the given path.
		/// </summary>
		/// <param name="source">files to check</param>
		/// <param name="Basepath">path that the files should be in</param>
		/// <returns>list of media files on the path</returns>
		public static List<MediaFile> findSimilarByPath(List<MediaFile> source, string Basepath) {
			List<MediaFile> matches = new List<MediaFile>();
			foreach (MediaFile ie in source) {
				if (ie.FilePath.Path.StartsWith(Basepath)) {
					matches.Add(ie);
				}
			}
			return matches;
		}
	}
}
