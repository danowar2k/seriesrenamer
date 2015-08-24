using System;
using System.Collections.Generic;
using System.Text;
using Renamer.Classes;

namespace Renamer
{
    class TitleManager
    {
        #region Singleton
        protected static TitleManager instance;
        private static object m_lock = new object();

        public static TitleManager Instance {
            get {
                if (instance == null) {
                    lock (m_lock) { if (instance == null) instance = new TitleManager(); }
                }
                return instance;
            }
        }
        #endregion

        #region Members

        /// <summary>
        /// List of season/episode<->name relations
        /// </summary>
        protected List<TitleCollection> relations = new List<TitleCollection>();
        #endregion

        #region Constructor(s)
        protected TitleManager() {
        }
        #endregion

        #region Properties
        public int Count {
            get { return this.relations.Count; }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a RelationCollection with a given Showname
        /// </summary>
        /// <param name="Showname">The Showname</param>
        /// <returns>The RelationCollection, or null if not found</returns>
        public TitleCollection GetTitleCollection(string Showname) {
            foreach (TitleCollection rc in relations) {
                if (rc.Showname == Showname) {
                    return rc;
                }
            }
            return null;
        }
        /// <summary>
        /// Removes a RelationCollection with a given Showname
        /// </summary>
        /// <param name="Showname">The Showname</param>
        /// <returns>The RelationCollection, or null if not found</returns>
        public void RemoveRelationCollection(string showname) {
            this.relations.Remove(this.GetTitleCollection(showname));
            CandidateManager.Instance.ClearRelation(showname);
        }

        public void AddRelationCollection(TitleCollection rc) {
            this.relations.Add(rc);
            CandidateManager manager = CandidateManager.Instance;
            List<Candidate> infoEntryList = manager.GetVideos(rc.Showname);
            foreach (Candidate ie in infoEntryList) {
                ie.findEpisodeName(rc);
            }
        }
        public void Clear() {
            this.relations.Clear();
            CandidateManager.Instance.ClearRelations();
        }
        #endregion

    }
}
