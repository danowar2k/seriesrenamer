using System;
using System.Collections.Generic;
using System.Linq;

namespace Renamer.NewClasses.TvShows {
	/// <summary>
	/// A season of a tv show.
	/// </summary>
	public class Season : IComparable<Season> {

		/// <summary>
		/// Default format for printing season numbers
		/// </summary>
		public const string DEFAULT_SEASON_NR_FORMAT = "D2";

		/// <summary>
		/// The value when no season nr is set yet.
		/// </summary>
		public const int UNSET_SEASON_NR = -1;

		/// <summary>
		/// The string that is displayed when the episode data is not complete
		/// </summary>
		public const string INCOMPLETE_SEASON_STRING = "xxxx - Season yy - SEASON INCOMPLETE";

		/// <summary>
		/// The show this season is from.
		/// </summary>
		private TvShow show = null;

		/// <summary>
		/// The season nr within the show.
		/// </summary>
		private int seasonNr = UNSET_SEASON_NR;

		/// <summary>
		/// The episodes in this season, sorted by episode nr.
		/// </summary>
		private SortedSet<Episode> episodes = null;

		/// <summary>
		/// Constructor for a season.
		/// </summary>
		/// <param name="someShow">The show this season is in</param>
		/// <param name="someSeasonNr">The season nr</param>
		public Season(TvShow someShow, int someSeasonNr) {
			init();
			setSeasonNr(someSeasonNr);
			setShow(someShow);
		}

		private void init() {
			episodes = new SortedSet<Episode>();
		}

		/// <summary>
		/// Gets the show this season is from.
		/// </summary>
		/// <returns>the show</returns>
		public TvShow getShow() {
			return show;
		}
		/// <summary>
		/// Sets the show this season is from (only possible when season nr is set)
		/// </summary>
		/// <param name="newShow">the show this season should be in</param>
		public void setShow(TvShow newShow) {
			if (!hasSeasonNr()) {
				return;
			}
			if (show != null) {
				if (newShow != show) {
					show.removeSeason(this);
				}
			}
			show = newShow;
			if (show != null) {
				show.addSeason(this);
			}
		}

		/// <summary>
		/// Gets the season nr
		/// </summary>
		/// <returns>the season nr</returns>
		public int getSeasonNr() {
			return seasonNr;
		}
		/// <summary>
		/// Sets the season nr
		/// </summary>
		/// <param name="newSeasonNr">the new season nr</param>
		public void setSeasonNr(int newSeasonNr) {
			if (newSeasonNr == UNSET_SEASON_NR) {
				return;
			}
			seasonNr = newSeasonNr;
		}

		/// <summary>
		/// Checks if the season has a number set yet
		/// </summary>
		/// <returns>true if the season number has been set</returns>
		public bool hasSeasonNr() {
			if (getSeasonNr() == UNSET_SEASON_NR) {
				return false;
			}
			return true;
		}
		/// <summary>
		/// Gets the episodes of this season.
		/// </summary>
		/// <returns>the episodes</returns>
		public SortedSet<Episode> getEpisodes() {
			return episodes;
		}
		/// <summary>
		/// Replaces the episodes of this season with new ones
		/// </summary>
		/// <param name="newEpisodes">the new episodes</param>
		public void setEpisodes(List<Episode> newEpisodes) {
			if (newEpisodes == null) {
				return;
			}
			clearEpisodes();
			addEpisodes(newEpisodes);
		}

		/// <summary>
		/// Removes all episodes from this season
		/// </summary>
		public void clearEpisodes() {
			removeEpisodes(episodes.ToList<Episode>());
		}

		/// <summary>
		/// Removes the old episodes from this season
		/// </summary>
		/// <param name="oldEpisodes">episodes to remove</param>
		public void removeEpisodes(IList<Episode> oldEpisodes) {
			if (oldEpisodes == null) {
				return;
			}
			foreach (Episode oldEpisode in oldEpisodes) {
				removeEpisode(oldEpisode);
			}
		}
		/// <summary>
		/// Removes an old episode from this season
		/// </summary>
		/// <param name="oldEpisode">episode to remove</param>
		public void removeEpisode(Episode oldEpisode) {
			if (oldEpisode == null) {
				return;
			}
			if (episodes.Contains(oldEpisode)) {
				episodes.Remove(oldEpisode);
				oldEpisode.setSeason(null);
			}
		}

		/// <summary>
		/// Adds new episodes to the season
		/// </summary>
		/// <param name="newEpisodes">the new episodes</param>
		public void addEpisodes(List<Episode> newEpisodes) {
			if (newEpisodes== null) {
				return;
			}
			foreach (Episode newEpisode in newEpisodes) {
				addEpisode(newEpisode);
			}
		}
		/// <summary>
		/// Adds an episode to the season
		/// </summary>
		/// <param name="newEpisode">the new episode</param>
		public void addEpisode(Episode newEpisode) {
			if (newEpisode == null) {
				return;
			}
			if (!episodes.Contains(newEpisode) && newEpisode.hasEpisodeNumber()) {
				//FIXME: When the comparison says equal, do I need to remove the episode or will it be overwritten?
				removeEpisode(getEpisode(newEpisode.getEpisodeNr()));
				episodes.Add(newEpisode);
				newEpisode.setSeason(this);
			}
		}
		/// <summary>
		/// Checks if this season contains exactly this episode
		/// </summary>
		/// <param name="someEpisode"></param>
		/// <returns>true if the episode is in this season</returns>
		public bool contains(Episode someEpisode) {
			return episodes.Contains(someEpisode);
		}

		/// <summary>
		/// Checks if the season has an episode with a certain number
		/// </summary>
		/// <param name="episodeNr">the episode nr to check for</param>
		/// <returns>true if the season contains an episode with this nr</returns>
		public bool hasEpisode(int episodeNr) {
			return episodes.Any<Episode>(
				episode => 
					episode.getEpisodeNr() == episodeNr);
		}
		/// <summary>
		/// Gets the episode of the season with the given number
		/// </summary>
		/// <param name="episodeNr">the number of the episode to look for</param>
		/// <returns>the episode with the number or null</returns>
		public Episode getEpisode(int episodeNr) {
			return episodes.FirstOrDefault<Episode>(
				episode => 
					episode.getEpisodeNr() == episodeNr);
		}

		/// <summary>
		/// Counts the episodes in the season.
		/// </summary>
		/// <returns>the number of episodes</returns>
		public int countEpisodes() {
			return episodes.Count;
		}

		/// <summary>
		/// Compares a season with another by comparing the season numbers
		/// </summary>
		/// <param name="otherSeason">the season to compare to</param>
		/// <returns>1 if this season is "bigger", -1 if this season is "smaller", 0 if both are equal</returns>
		public int CompareTo(Season otherSeason) {
			if (otherSeason == null) {
				return 1;
			}
			return seasonNr.CompareTo(otherSeason.getSeasonNr());
		}
		/// <summary>
		/// Checks if the season is complete, i.e. has a show attached and a season number set.
		/// </summary>
		/// <returns>true if complete</returns>
		public bool complete() {
			if (show != null && hasSeasonNr()) {
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// Returns a string representation of this season containing the associated show, the season nr and the episode count
		/// </summary>
		/// <returns>Formatted string SHOWNAME - Season xx (y episodes)</returns>
		public string toString() {
			if (complete()) {
				return getShow().getOriginalShowname()
					+ " - Season " + getSeasonNr().ToString(DEFAULT_SEASON_NR_FORMAT)
 					+ " (" + getEpisodes().Count + " episodes)";
			}
			return INCOMPLETE_SEASON_STRING;
		}

		/// <summary>
		/// Returns a short string for filename generation.
		/// </summary>
		/// <returns>Formatted string: Sxx</returns>
		public string toSeasonString() {
			if (complete()) {
				return "S" + getSeasonNr().ToString(DEFAULT_SEASON_NR_FORMAT);
			}
			return "";
		}
	}
}
