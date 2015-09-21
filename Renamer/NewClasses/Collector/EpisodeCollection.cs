using System;
using System.Collections.Generic;

namespace Renamer.NewClasses.Collector {
	public class EpisodeCollection {
		public int maxEpisode = 0;
		public int minEpisode = Int32.MaxValue;
		public List<MediaFile> entries = new List<MediaFile>();
	}
}
