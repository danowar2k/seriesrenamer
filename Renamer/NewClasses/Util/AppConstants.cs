using System.IO;

namespace Renamer.NewClasses.Util {

	/// <summary>
	/// Various constants used in the application.
	/// </summary>
	class AppConstants {
		/// <summary>
		/// The directory where the application is.
		/// </summary>
		public static readonly string APP_BASE_DIR = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
		/// <summary>
		/// Extension for temporary files
		/// </summary>
		public const string TEMPFILE_EXTENSION = ".tmp";
		/// <summary>
		/// The default name for the application log file.
		/// </summary>
		public const string APP_LOG_FILENAME = "Renamer.log";
	}
}
