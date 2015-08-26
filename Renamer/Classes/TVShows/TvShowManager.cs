using System;
using System.Collections.Generic;
using System.Text;
using Renamer.Classes;

namespace Renamer.Classes.TVShows
{
    class TvShowManager
    {
        #region Singleton
        protected static TvShowManager instance;
        private static object m_lock = new object();

        public static TvShowManager Instance {
            get {
                if (instance == null) {
                    lock (m_lock) { if (instance == null) instance = new TvShowManager(); }
                }
                return instance;
            }
        }
        #endregion

        #region Members

        /// <summary>
        /// List of season/episode<->name relations
        /// </summary>
        protected List<TvShow> tvShows = new List<TvShow>();
        #endregion

        #region Constructor(s)
        protected TvShowManager() {
        }
        #endregion

        #region Properties
        public int CountShows {
            get { return this.tvShows.Count; }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a tv show with a given Showname
        /// </summary>
        /// <param name="Showname">The Showname</param>
        /// <returns>The tv show, or null if not found</returns>
        public TvShow GetTvShow(string showname) {
            foreach (TvShow tvShow in tvShows) {
                if (tvShow.Name == showname) {
                    return tvShow;
                }
            }
            return null;
        }
        /// <summary>
        /// Removes a tv show with a given Showname
        /// </summary>
        /// <param name="showname">The showname</param>
        public void RemoveTvShow(string showname) {
            this.tvShows.Remove(this.GetTvShow(showname));
            MediaFileManager.Instance.ClearRelation(showname);
        }

        public void AddTvShow(TvShow newShow) {
            this.tvShows.Add(newShow);
            MediaFileManager manager = MediaFileManager.Instance;
            List<MediaFile> candidates = manager.GetMediaFiles(newShow.Name);
            foreach (MediaFile someCandidate in candidates) {
                someCandidate.findEpisodeName(newShow);
            }
        }
        public void removeAllTvShows() {
            this.tvShows.Clear();
            MediaFileManager.Instance.ClearRelations();
        }
        #endregion

    }
}
