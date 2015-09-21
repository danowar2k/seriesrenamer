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
using Renamer.NewClasses.Enums;
using Renamer.NewClasses.Util;

namespace Renamer.NewClasses.TvShows
{
    
    /// <summary>
    /// An episode of a TV show
    /// </summary>
    public class Episode : IComparable<Episode>
    {

		/// <summary>
		/// Default format for printing episode numbers
		/// </summary>
		public const string DEFAULT_EPISODE_NR_FORMAT = "D2";

		/// <summary>
		/// This is set when no episode nr is set.
		/// </summary>
		public const int UNSET_EPISODE_NR = -1;

		/// <summary>
		/// The string that is displayed when the episode data is not complete
		/// </summary>
		public const string INCOMPLETE_EPISODE_STRING = "SxxExx - EPISODE INCOMPLETE";

		/// <summary>
		/// The season this episode is in
		/// </summary>
		private Season season = null;

		/// <summary>
		/// The episode nr relative to the season it is in
		/// </summary>
		private int episodeNr = UNSET_EPISODE_NR;

		/// <summary>
		/// The title of this episode localized in several languages.
		/// </summary>
		private Dictionary<Language, string> localizations = null;

        /// <summary>
        /// Constructor
        /// </summary>
		/// <param name="someSeason">The season this episode is in</param>
		/// <param name="someEpisodeNr">Episode nr relative to the season</param>
        /// <param name="someOriginalTitle">the title of the episode when it was first released in its original language</param>
        public Episode(Season someSeason, int someEpisodeNr, string someOriginalTitle) {
			init();
			setEpisodeNr(someEpisodeNr);
			setSeason(someSeason);
			setOriginalTitle(someOriginalTitle);
        }

		private void init() {
			localizations = new Dictionary<Language, string>();
		}

		/// <summary>
		/// Returns the season this episode is from
		/// </summary>
		/// <returns>the season this episode is from</returns>
		public Season getSeason() {
			return season;
		}

		/// <summary>
		/// Sets the season this episode is from (only possible when episode nr is set)
		/// </summary>
		/// <param name="newSeason">The new season to set</param>
		public void setSeason(Season newSeason) {
			if (!hasEpisodeNumber()) {
				return;
			}
			if (season != null) {
				if (newSeason != season) {
					season.removeEpisode(this);
				}
			}
			season = newSeason;
			if (season != null) {
				season.addEpisode(this);
			}
		}

		/// <summary>
		/// Returns the nr this episode is in this season
		/// </summary>
		/// <returns>the episode nr</returns>
		public int getEpisodeNr() {
			return episodeNr;
		}
		/// <summary>
		/// Sets the nr this episode is in this season
		/// </summary>
		/// <param name="newEpisodeNr">the new episode nr</param>
		public void setEpisodeNr(int newEpisodeNr) {
			if (newEpisodeNr == UNSET_EPISODE_NR) {
				return;
			}
			episodeNr = newEpisodeNr;
		}

		/// <summary>
		/// Check if the episode number has been set yet
		/// </summary>
		/// <returns>true if the episode has a number</returns>
		public bool hasEpisodeNumber() {
			if (getEpisodeNr() == UNSET_EPISODE_NR) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns the localizations
		/// </summary>
		/// <returns>the localizations</returns>
		public Dictionary<Language, string> getLocalizations() {
			return localizations;
		}
		/// <summary>
		/// Replaces all localizations with new ones
		/// </summary>
		/// <param name="newLocalizations">the new localizations</param>
		public void setLocalizedTitles(Dictionary<Language, string> newLocalizations) {
			if (newLocalizations == null) {
				return;
			}
			clearLocalizations();
			addLocalizations(newLocalizations);
		}

		/// <summary>
		/// Clears the localizations
		/// </summary>
		public void clearLocalizations() {
			removeLocalizations(localizations.Keys);
		}

		/// <summary>
		/// Removes the localizations in these languages
		/// </summary>
		/// <param name="languages">the localizations to remove</param>
		public void removeLocalizations(Dictionary<Language, string>.KeyCollection languages) {
			if (languages == null) {
				return;
			}
			foreach (Language language in languages) {
				removeLocalizations(language);
			}
		}
		/// <summary>
		/// Removes the localization in the language
		/// </summary>
		/// <param name="language">the localization to remove</param>
		public void removeLocalizations(Language language) {
			localizations.Remove(language);
		}

		/// <summary>
		/// Adds new localizations for the episode title
		/// </summary>
		/// <param name="newLocalizations">the new localizations, pairs of language and title</param>
		public void addLocalizations(Dictionary<Language, string> newLocalizations) {
			if (newLocalizations == null) {
				return;
			}
			foreach (KeyValuePair<Language,string> newLocalization in newLocalizations) {
				addLocalizations(newLocalization.Key, newLocalization.Value);
			}
		}
		/// <summary>
		/// Adds a new localization of the title
		/// </summary>
		/// <param name="newLanguage">the localization language</param>
		/// <param name="newLocalizedTitle">the localized title</param>
		public void addLocalizations(Language newLanguage, string newLocalizedTitle) {
			if (string.IsNullOrEmpty(newLocalizedTitle) || newLanguage == Language.NONE) {
				return;
			}
			localizations.Add(newLanguage, newLocalizedTitle);
		}

		/// <summary>
		/// Returns the show this episode is from.
		/// </summary>
		/// <returns>the show this episode is from</returns>
		public TvShow getShow() {
			if (season == null) {
				return null;
			}
			return season.getShow();
		}

		/// <summary>
		/// Returns the total episode number of this episode inside the show
		/// </summary>
		/// <returns>the total episode nr of the episode</returns>
		public int getTotalEpisodeNr() {
			TvShow theShow = getShow();
			if (theShow == null) {
				return UNSET_EPISODE_NR;
			}
			return theShow.getTotalEpisodeNr(this);
		}
		/// <summary>
		/// Gets the original language of the episode which is the same as the show's original language.
		/// </summary>
		/// <returns>the original language</returns>
		public Language getOriginalLanguage() {
			TvShow theShow = getShow();
			if (theShow == null) {
				return Language.NONE;
			}
			return theShow.getOriginalLanguage();
		}

		/// <summary>
		/// Returns the unlocalized original title of this episode
		/// </summary>
		/// <returns>the episode title</returns>
		public string getOriginalTitle() {
			string originalTitle;
			if (localizations.TryGetValue(getOriginalLanguage(), out originalTitle)) {
				return originalTitle;
			}
			return "";
		}

		/// <summary>
		/// Sets the new unlocalized original title of this episode
		/// </summary>
		/// <param name="newOriginalTitle">the new original title</param>
		public void setOriginalTitle(string newOriginalTitle) {
			if (string.IsNullOrEmpty(newOriginalTitle)) {
				return;
			}
			removeLocalizations(getOriginalLanguage());
			addLocalizations(getOriginalLanguage(), newOriginalTitle);
		}
		/// <summary>
		/// Checks if the episode data is complete to build the string representation used for filenames.
		/// </summary>
		/// <returns>true if the data is complete</returns>
		public bool complete() {
			if (season != null && season.complete() && episodeNr != UNSET_EPISODE_NR && (!string.IsNullOrEmpty(getOriginalTitle()))) {
				return true;
			}
			return false;
		}

		/// <summary>
		/// Returns a String representation for this episode in the format:
		/// SxxEyy - zz
		/// </summary>
		public string toString() {
			if (complete()) {
				return getSeason().toSeasonString()
					+ "E" + episodeNr.ToString(DEFAULT_EPISODE_NR_FORMAT)
					+ " - " + getOriginalTitle();
			}
			return INCOMPLETE_EPISODE_STRING;
		}

		/// <summary>
		/// Converts the episode to a string, matching the given pattern
		/// </summary>
		/// <param name="template">a template containing markers to be replaced with the episode data, see <see cref=""/> for details</param>
		/// <param name="titleLanguage">the language the title should be in</param>
		/// <returns>a filename for the episode in the wanted language, if available. Else it is in the original language.</returns>
		public string toString(string template, Language titleLanguage) {
			template = template.Replace(RenamingConstants.SEASON_NR_MARKER, getSeason().getSeasonNr().ToString(Season.DEFAULT_SEASON_NR_FORMAT));
			template = template.Replace(RenamingConstants.SEASON_NR_MARKER_NO_PAD, getSeason().getSeasonNr().ToString());
			template = template.Replace(RenamingConstants.EPISODE_NR_MARKER, getEpisodeNr().ToString(DEFAULT_EPISODE_NR_FORMAT));
			template = template.Replace(RenamingConstants.EPISODE_NR_MARKER_NO_PAD, getEpisodeNr().ToString());
			template = template.Replace(RenamingConstants.EPISODE_TITLE_MARKER, 
				localizations.ContainsKey(titleLanguage) ? localizations[titleLanguage] : getOriginalTitle());
			return template;
		}
		/// <summary>
		/// Compares two episodes based on their episode nr.
		/// </summary>
		/// <param name="otherEpisode">the other episode</param>
		/// <returns>1 if this episode is "bigger", -1 if this episode is "smaller", 0 if both are equal</returns>
		public int CompareTo(Episode otherEpisode) {
			if (otherEpisode == null) {
				return 1;
			}
			return episodeNr.CompareTo(otherEpisode.getEpisodeNr());
		}
	}
}
