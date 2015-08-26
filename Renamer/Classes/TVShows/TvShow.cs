#region SVN Info
/***************************************************************
 * $Author$
 * $Revision$
 * $Date$
 * $LastChangedBy$
 * $LastChangedDate$
 * $URL$
 * 
 * License: GPLv3
 * 
****************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Renamer.Classes.TVShows
{
    public class TvShow : IEnumerable
    {
        protected List<ShowEpisode> Episodes = new List<ShowEpisode>();
        public string Name = "";

        public TvShow(string showname)
        {
            this.Name = showname;
        }
        /// <summary>
        /// Finds lowest season nr stored in this tv show information
        /// </summary>
        /// <returns>index of the season, or 10000 ;)</returns>
        public int FindMinSeason()
        {
            int lowestSeasonNr = 10000;
            foreach (ShowEpisode episode in Episodes)
            {
				int seasonNr = Convert.ToInt32(episode.SeasonNr);
				if (seasonNr < lowestSeasonNr) {
					lowestSeasonNr = seasonNr;
				}
            }
            return lowestSeasonNr;
        }

        /// <summary>
        /// Finds highest season in relations
        /// </summary>
        /// <returns>index of the season, or -1 ;)</returns>
        public int FindMaxSeason()
        {
            int highestSeasonNr = -1;
            foreach (ShowEpisode episode in Episodes)
            {
				int seasonNr = Convert.ToInt32(episode.SeasonNr);
				if (seasonNr > highestSeasonNr) {
					highestSeasonNr = seasonNr;
				}
            }
            return highestSeasonNr;
        }

        /// <summary>
        /// Finds lowest episode nr of a season in relations
        /// </summary>
        /// <param name="season">season to find lowest episode relation in</param>
        /// <returns>index of the episode, or 10000 ;)</returns>
        public int FindMinEpisodeNr(int seasonNr)
        {
            int lowestEpisodeNrInSeason = 10000;
            foreach (ShowEpisode episode in Episodes)
            {
				if (Convert.ToInt32(episode.SeasonNr) == seasonNr)
                {
					int episodeSeasonNr = Convert.ToInt32(episode.SeasonEpisodeNr);
					if (episodeSeasonNr < lowestEpisodeNrInSeason) {
						lowestEpisodeNrInSeason = episodeSeasonNr;
					}
                }
            }
            return lowestEpisodeNrInSeason;
        }

        /// <summary>
        /// Finds highest episode of a season in relations
        /// </summary>
        /// <param name="season">season to find highest episode relation in</param>
        /// <returns>index of the episode, or -1 ;)</returns>
        public int FindMaxEpisodeNr(int seasonNr)
        {
            int highestEpisodeNrInSeason = -1;
            foreach (ShowEpisode episode in Episodes)
            {
                if (Convert.ToInt32(episode.SeasonNr) == seasonNr)
                {
					int episodeSeasonNr = Convert.ToInt32(episode.SeasonEpisodeNr);
					if (episodeSeasonNr > highestEpisodeNrInSeason) {
						highestEpisodeNrInSeason = episodeSeasonNr;
					}
                }
            }
            return highestEpisodeNrInSeason;
        }

        public void AddEpisode(ShowEpisode episode) {
            this.Episodes.Add(episode);
        }

        public int CountEpisodes {
            get {
                return this.Episodes.Count;
            }
        }

        public ShowEpisode this[int id]{
            get {
                return this.Episodes[id];
            }
        }
        public string ToString()
        {
            return Name;
        }
        #region IEnumerable Member

        public IEnumerator GetEnumerator() {
            return this.Episodes.GetEnumerator();
        }

        #endregion
    }

}
