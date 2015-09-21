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
using System.Linq;
using Renamer.NewClasses.Enums;

namespace Renamer.NewClasses.TvShows
{
	/// <summary>
	/// A tv show with seasons.
	/// </summary>
    public class TvShow {
		/// <summary>
		/// The original language of the show
		/// </summary>
		private Language originalLanguage = Language.NONE;

		/// <summary>
		/// The localized shownames
		/// </summary>
		private Dictionary<Language, string> localizations;

		/// <summary>
		/// The seasons of this show
		/// </summary>
		private SortedSet<Season> seasons;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="someOriginalLanguage">the original language of this show</param>
		/// <param name="someOriginalShowname">the original showname</param>
        public TvShow(Language someOriginalLanguage, string someOriginalShowname) {
			init();
			setOriginalLanguage(someOriginalLanguage);
			setOriginalShowname(someOriginalShowname);
        }
		/// <summary>
		/// Initialize the tv show.
		/// </summary>
		private void init() {
			localizations = new Dictionary<Language, string>();
			seasons = new SortedSet<Season>();
		}
		/// <summary>
		/// Gets the original language
		/// </summary>
		/// <returns></returns>
		public Language getOriginalLanguage() {
			return originalLanguage;
		}
		/// <summary>
		/// Sets the original language.
		/// </summary>
		/// <param name="newOriginalLanguage">the new original language</param>
		public void setOriginalLanguage(Language newOriginalLanguage) {
			if (originalLanguage != newOriginalLanguage && newOriginalLanguage != Language.NONE) {
				originalLanguage = newOriginalLanguage;
			}
		}
		/// <summary>
		/// Checks if the original language has been set
		/// </summary>
		/// <returns>true if the original language has been set</returns>
		public bool hasOriginalLanguage() {
			if (getOriginalLanguage() != Language.NONE) {
				return true;
			}
			return false;
		}
		/// <summary>
		/// Gets the localized shownames.
		/// </summary>
		/// <returns>the localizations</returns>
		public Dictionary<Language, string> getLocalizations() {
			return localizations;
		}
		/// <summary>
		/// Replaces the localizations.
		/// </summary>
		/// <param name="newLocalizations">the new localizations</param>
		public void setLocalizations(Dictionary<Language, string> newLocalizations) {
			if (newLocalizations == null) {
				return;
			}
			clearLocalizations();
			addLocalizations(newLocalizations);
		}
		/// <summary>
		/// Removes all localizations.
		/// </summary>
		public void clearLocalizations() {
			removeLocalizations(localizations.Keys);
		}
		/// <summary>
		/// Removes some localizations by language
		/// </summary>
		/// <param name="languages">the localizations to remove</param>
		public void removeLocalizations(Dictionary<Language, string>.KeyCollection languages) {
			if (languages == null) {
				return;
			}
			foreach (Language language in languages) {
				removeLocalization(language);
			}
		}
		/// <summary>
		/// Removes a localization by language
		/// </summary>
		/// <param name="language">the localization to remove</param>
		public void removeLocalization(Language language) {
			localizations.Remove(language);
		}
		/// <summary>
		/// Adds localizations.
		/// </summary>
		/// <param name="newLocalizations">new localizations</param>
		public void addLocalizations(Dictionary<Language, string> newLocalizations) {
			if (newLocalizations == null) {
				return;
			}
			foreach (KeyValuePair<Language, string> newLocalization in newLocalizations) {
				addLocalization(newLocalization.Key, newLocalization.Value);
			}
		}
		/// <summary>
		/// Adds a new localization
		/// </summary>
		/// <param name="newLanguage">the language</param>
		/// <param name="newLocalizedShowname">the localized showname</param>
		public void addLocalization(Language newLanguage, string newLocalizedShowname) {
			if (String.IsNullOrEmpty(newLocalizedShowname) || newLanguage == Language.NONE) {
				return;
			}
			localizations.Add(newLanguage, newLocalizedShowname);
		}
		/// <summary>
		/// Gets the showname in the original language
		/// </summary>
		/// <returns>the original showname</returns>
		public string getOriginalShowname() {
			string originalShowname;
			if (getLocalizations().TryGetValue(getOriginalLanguage(), out originalShowname)) {
				return originalShowname;
			}
			return "";
		}
		/// <summary>
		/// Sets the original showname
		/// </summary>
		/// <param name="newOriginalShowname">the new original showname</param>
		public void setOriginalShowname(string newOriginalShowname) {
			if (string.IsNullOrEmpty(newOriginalShowname)) {
				return;
			}
			removeLocalization(getOriginalLanguage());
			addLocalization(getOriginalLanguage(), newOriginalShowname);
		}
		/// <summary>
		/// Gets the seasons of this tv show.
		/// </summary>
		/// <returns>the seasons</returns>
		public SortedSet<Season> getSeasons() {
			return seasons;
		}

		/// <summary>
		/// Replaces all seasons of this show with new ones.
		/// </summary>
		/// <param name="newSeasons">the new seasons</param>
		public void setSeasons(List<Season> newSeasons) {
			if (newSeasons == null) {
				return;
			}
			clearSeasons();
			addSeasons(newSeasons);
		}
		/// <summary>
		/// Removes all seasons from this show
		/// </summary>
		public void clearSeasons() {
			removeSeasons(seasons.ToList());
		}
		/// <summary>
		/// Removes some seasons from this show
		/// </summary>
		/// <param name="oldSeasons">the old seasons</param>
		public void removeSeasons(IList<Season> oldSeasons) {
			if (oldSeasons == null) {
				return;
			}
			foreach (Season oldSeason in oldSeasons) {
				removeSeason(oldSeason);
			}
		}
		/// <summary>
		/// Removes a single season from this show
		/// </summary>
		/// <param name="oldSeason">the season to remove</param>
		public void removeSeason(Season oldSeason) {
			if (oldSeason == null) {
				return;
			}
			if (seasons.Contains(oldSeason)) {
				seasons.Remove(oldSeason);
				oldSeason.setShow(null);
			}
		}
		/// <summary>
		/// Adds some seasons to the show
		/// </summary>
		/// <param name="newSeasons">the new seasons</param>
		public void addSeasons(IList<Season> newSeasons) {
			if (newSeasons == null) {
				return;
			}
			foreach (Season newSeason in newSeasons) {
				addSeason(newSeason);
			}
		}
		/// <summary>
		/// Adds a season to this show
		/// </summary>
		/// <param name="newSeason">the season</param>
		public void addSeason(Season newSeason) {
			if (newSeason == null) {
				return;
			}
			if (!seasons.Contains(newSeason)) {
				seasons.Add(newSeason);
				newSeason.setShow(this);
			}
		}
		/// <summary>
		/// Counts the seasons in this show
		/// </summary>
		/// <returns>the number of seasons</returns>
		public int countSeasons() {
			return seasons.Count;
		}
		/// <summary>
		/// Checks if this show has any seasons
		/// </summary>
		/// <returns>true if the show has seasons</returns>
		public bool hasSeasons() {
			if (countSeasons() > 0) {
				return true;
			}
			return false;
		}
		/// <summary>
		/// Checks if this show contains a season
		/// </summary>
		/// <param name="someSeason">season to look for</param>
		/// <returns>true if the season is in this show</returns>
		public bool contains(Season someSeason) {
			if (someSeason == null || !hasSeasons()) {
				return false;
			}
			if (seasons.Contains(someSeason)) {
				return true;
			}
			return false;
		}
		/// <summary>
		/// Checks if this show contains a certain episode
		/// </summary>
		/// <param name="someEpisode">episode to look for</param>
		/// <returns>true if the episode is in a season of this show</returns>
		public bool contains(Episode someEpisode) {
			if (someEpisode == null || !hasSeasons()) {
				return false;
			}
			Season episodeSeason = someEpisode.getSeason();
			if (episodeSeason == null) {
				return false;
			}
			if (contains(episodeSeason)) {
				return true;
			}
			return false;
		}
		/// <summary>
		/// Counts all episodes of this show
		/// </summary>
		/// <returns>the number of episodes</returns>
		public int countEpisodes() {
			int episodeCount = 0;
			foreach (Season season in seasons) {
				episodeCount += season.countEpisodes();
			}
			return episodeCount;
		}

		/// <summary>
		/// Finds lowest season nr stored in this tv show information
		/// </summary>
		/// <returns>the lowest season nr stored in this tv show</returns>
		public int findMinSeason() {
			if (hasSeasons()) {
				return seasons.Min.getSeasonNr();
			}
			return -1;
		}

		/// <summary>
		/// Finds highest season in relations
		/// </summary>
		/// <returns>index of the season, or -1 ;)</returns>
		public int findMaxSeason() {
			if (hasSeasons()) {
				return seasons.Max.getSeasonNr();
			}
			return -1;
		}
		/// <summary>
		/// Calculates the total episode nr of an episode within this show, regardless of season
		/// </summary>
		/// <param name="someEpisode">the episode to check</param>
		/// <returns>-1 if the episode isn't in this season</returns>
		/// <returns>Example: 2nd episode of 2nd season, 1st season has 20 episodes: 22</returns>
		public int getTotalEpisodeNr(Episode someEpisode) {
			if (!hasSeasons() || !contains(someEpisode)) {
				return -1;
			}
			int totalEpisodeNr = 0;
			foreach (Season season in seasons) {
				if (!season.contains(someEpisode)) {
					totalEpisodeNr += season.countEpisodes();
				} else {
					totalEpisodeNr += someEpisode.getEpisodeNr();
				}
			}
			return totalEpisodeNr;
		}

		/// <summary>
		/// Finds lowest episode nr of a season in relations
		/// </summary>
		/// <param name="seasonNr">season to find lowest episode relation in</param>
		/// <returns>index of the episode, or 10000 ;)</returns>
		//public int findMinEpisodeNr(int seasonNr) {
		//	List<Episode> seasonEpisodes;
		//	if (seasons.TryGetValue(seasonNr, out seasonEpisodes)) {
		//		return seasonEpisodes[0].getSeasonEpisodeNr();
		//	}
		//	return -1;
		//}

		/// <summary>
		/// Finds highest episode of a season in relations
		/// </summary>
		/// <param name="seasonNr">season to find highest episode relation in</param>
		/// <returns>index of the episode, or -1 ;)</returns>
		//public int findMaxEpisodeNr(int seasonNr) {
		//	List<Episode> seasonEpisodes;
		//	if (seasons.TryGetValue(seasonNr, out seasonEpisodes)) {
		//		return seasonEpisodes.Count + 1;
		//	}
		//	return -1;
		//}

		//public void AddEpisode(ShowEpisode episode) {
		//	this.Episodes.Add(episode);
		//}

		/// <summary>
		/// A string representation of this show, complete with name, number of seasons and episodes
		/// </summary>
		/// <returns>SHOWNAME (x seasons, y episodes)</returns>
		public string toString() {
			return getOriginalShowname() + " (" + countSeasons() + " seasons, " + countEpisodes() + " episodes)";
		}

    }

}
