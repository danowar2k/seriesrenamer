using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renamer.NewClasses.Util {
	/// <summary>
	/// Constants used in renaming patterns to replace with certain information.
	/// </summary>
	public class RenamingConstants {
		/// <summary>
		/// The title of the episode
		/// </summary>
		public const string EPISODE_TITLE_MARKER = "%N";
		/// <summary>
		/// The name of the show
		/// </summary>
		public const string SHOWNAME_MARKER = "%T";
		/// <summary>
		/// The season number of an episode (with zero padding)
		/// </summary>
		public const string SEASON_NR_MARKER = "%S";
		/// <summary>
		/// The season number of an episode (no zero padding)
		/// </summary>
		public const string SEASON_NR_MARKER_NO_PAD = "%s";
		/// <summary>
		/// The in-season episode number of an episode (with zero padding)
		/// </summary>
		public const string EPISODE_NR_MARKER = "%E";
		/// <summary>
		/// The in-season episode number of an episode (no zero padding)
		/// </summary>
		public const string EPISODE_NR_MARKER_NO_PAD = "%e";
		/// <summary>
		/// To replace something in a URL with a link
		/// </summary>
		public const string SHOW_LINK_MARKER = "%L";
	}
}
