using System.Collections.Generic;

// FIXME: Observer - https://msdn.microsoft.com/en-us/library/ee850490%28v=vs.110%29.aspx
// This should be an observable object
namespace Renamer.NewClasses.TvShows
{
	/// <summary>
	/// This manages the TV shows currently in memory
	/// </summary>
    public class TvShowCollection
    {
		/// <summary>
		/// Singleton instance
		/// </summary>
        protected static TvShowCollection instance;
		/// <summary>
		/// Lock when creating instance
		/// </summary>
        private static object m_lock = new object();

		/// <summary>
		/// TV shows managed by this collection
		/// </summary>
		protected List<TvShow> tvShows;

		/// <summary>
		/// Returns the single instance.
		/// </summary>
		/// <returns>the TvShowManager</returns>
		public static TvShowCollection getInstance() {
            if (instance == null) {
                lock (m_lock) { if (instance == null) instance = new TvShowCollection(); }
            }
            return instance;
        }
		/// <summary>
		/// Constructor.
		/// </summary>
        protected TvShowCollection() {
			init();
		}

		/// <summary>
		/// Initialize the structures
		/// </summary>
		private void init() {
			tvShows = new List<TvShow>();
		}

		/// <summary>
		/// Count the number of shows.
		/// </summary>
		/// <returns></returns>
        public int countShows() {
            return tvShows.Count;
        }

        /// <summary>
        /// Returns a tv show with a given showname
        /// </summary>
		/// <param name="showname">The showname</param>
        /// <returns>The tv show, or null if not found</returns>
        public TvShow getTvShow(string showname) {
            foreach (TvShow tvShow in tvShows) {
                if (tvShow.getOriginalShowname() == showname) {
                    return tvShow;
                }
            }
            return null;
        }
        /// <summary>
        /// Removes a tv show with a given showname
        /// </summary>
        /// <param name="showname">The showname</param>
        public void removeTvShow(string showname) {
            tvShows.Remove(getTvShow(showname));
// FIXME: Should be media file manager be in here or just listening and updating accordingly?
			//MediaFileManager.Instance.clearEpisodeNamesOfShow(showname);
        }

		/// <summary>
		/// Adds a new tv show
		/// </summary>
		/// <param name="newShow">the show to add</param>
        public void addTvShow(TvShow newShow) {
            tvShows.Add(newShow);
// FIXME: Should be media file manager be in here or just listening and updating accordingly?
			//MediaFileManager manager = MediaFileManager.Instance;
			//List<MediaFile> candidates = manager.GetMediaFiles(newShow.getOriginalShowname());
			//foreach (MediaFile someCandidate in candidates) {
			//	someCandidate.findEpisodeName(newShow);
			//}
        }
		/// <summary>
		/// Remove all tv shows from this collection
		/// </summary>
        public void removeAllTvShows() {
			tvShows.Clear();
            MediaFileManager.Instance.clearEpisodeNames();
        }

    }
}
