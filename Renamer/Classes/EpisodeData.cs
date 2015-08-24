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

namespace Renamer.Classes
{
    
    /// <summary>
    /// A class wrapping season number, episode number and episode title
    /// </summary>
    public class EpisodeData
    {

        #region Members
        private int seasonNr = -1;
        private int seasonEpisodeNr = -1;
        private string episodeTitle = "";
        #endregion

        #region Properties
        /// <summary>
        /// season nr for this episode
        /// </summary>
        public int SeasonNr {
            get { return seasonNr; }
            set { seasonNr = value; }
        }

        /// <ISODE_NRsummary>
        /// episode number in season
        /// </summary>
        public int SeasonEpisodeNr {
            get { return seasonEpisodeNr; }
            set { seasonEpisodeNr = value; }
        }

        /// <summary>
        /// episode title
        /// </summary>
        public string EpisodeTitle {
            get { return episodeTitle; }
            set { episodeTitle = value; }
        }
        #endregion
        #region Public methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="someSeasonNr">seasonNr</param>
        /// <param name="someSeasonEpisodeNr">seasonEpisodeNr</param>
        /// <param name="someEpisodeTitle">episodeTitle</param>
        public EpisodeData(int someSeasonNr, int someSeasonEpisodeNr, string someEpisodeTitle)
        {
            SeasonNr = seasonNr;
            SeasonEpisodeNr = someSeasonEpisodeNr;
            EpisodeTitle = someEpisodeTitle;
        }

        /// <summary>
        /// Returns a String representation for this episode in the format:
        /// SxEy - ZZ
        /// </summary>
        public override string ToString()
        {
            return "S" + SeasonNr + "E" + SeasonEpisodeNr + " - " + EpisodeTitle;
        }

        /// <summary>
        /// Converts the relation to a string, matching the given pattern
        /// </summary>
        /// <param name="template">a template containing markers to be replaced with the episode data, see <see cref=""/> for details</param>
        /// <returns>episode string</returns>
        public string ToString(string template) {
            template = template.Replace("%S", this.seasonNr.ToString("00"));
            template = template.Replace("%s", this.seasonNr.ToString());
            template = template.Replace("%E", this.seasonEpisodeNr.ToString("00"));
            template = template.Replace("%e", this.seasonEpisodeNr.ToString());
            template = template.Replace("%N", this.episodeTitle);
            return template;
        }
        #endregion
    }
    
}
