using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Renamer.NewClasses.Util {
	/// <summary>
	/// Utility functions for working with the file system.
	/// </summary>
	public class FileSystemUtils {

		protected static readonly char[] DIRECTORY_SEPARATORS = { System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar };

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger
	(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		/// <summary>
		/// Get the names of all folders in the path
		/// </summary>
		/// <param name="somePath">the path to get the folder names from</param>
		/// <returns>names of all folders contained in the path</returns>
		public static List<string> getFolderNames(string somePath) {
			List<string> folderNames = new List<string>();
			if (string.IsNullOrEmpty(somePath)) {
				return folderNames;
			}
			DirectoryInfo dir = null;
			if (File.Exists(somePath)) {
				FileInfo fi = new FileInfo(somePath);
				dir = fi.Directory;
			}
			if (dir == null) {
				if (Directory.Exists(somePath)) {
					dir = new DirectoryInfo(somePath);
				}
			}
			if (dir == null) {
				return folderNames;
			}
			folderNames.Add(dir.Name);
			while (dir.Parent != null) {
				dir = dir.Parent;
				folderNames.Add(dir.Name);
			}
			return folderNames;
		}

		/// <summary>
		/// Gets all files recursively from subdirectories.
		/// Collects files until it reaches a certain maximum depth.
		/// </summary>
		/// <param name="startDirectory">root folder</param>
		/// <param name="fileFilterPattern">Pattern for file matching, "*" for all</param>
		/// <param name="maxSearchDepth">maximum directory depth</param>
		/// <param name="worker">optional background worker to cancel asynchronous actions</param>
		/// <returns>List of FileInfo classes from all matched files</returns>
		public static List<FileInfo> getAllFilesRecursively(
			string startDirectory, string fileFilterPattern, int maxSearchDepth, BackgroundWorker worker) {
			return getAllFilesRecursively(startDirectory, fileFilterPattern, 0, maxSearchDepth, worker);
		}

		private static List<FileInfo> getAllFilesRecursively(
			string startDir, string fileFilterPattern,
			int currentSearchDepth, int maxSearchDepth, BackgroundWorker worker) {
			List<FileInfo> fileInfos = new List<FileInfo>();
			if (currentSearchDepth > maxSearchDepth) {
				return fileInfos;
			}
			if (worker != null && worker.CancellationPending) {
				return fileInfos;
			}
			List<string> fileEntries =
				new List<string>(Directory.GetFiles(
					startDir, fileFilterPattern, SearchOption.TopDirectoryOnly));

			fileInfos.AddRange(fileEntries.Select(fileEntry => new FileInfo(fileEntry)).ToList());

			List<string> directoryEntries =
				new List<string>(Directory.GetDirectories(
					startDir, fileFilterPattern, SearchOption.TopDirectoryOnly));
			foreach (string directoryEntry in directoryEntries) {
				fileInfos.AddRange(getAllFilesRecursively(
					directoryEntry, fileFilterPattern, currentSearchDepth + 1, maxSearchDepth, worker));
			}
			return fileInfos;
		} 

		/// <summary>
		/// Given a dirPath, deletes all empty folders in this folder and its subfolders.
		/// Folders are seen as empty if only the ignored filetypes are present in a folder.
		/// </summary>
		/// <param name="dirPath">start folder to delete and search</param>
		/// <param name="ignoredFiletypes">filetypes (not containing ".") which are ignored when detecting if a folder is empty</param>
		public static void deleteAllEmptyFolders(
			string dirPath, List<string> ignoredFiletypes, BackgroundWorker worker, DoWorkEventArgs e) {
			bool delete = true;
			List<string> folders = new List<string>(Directory.GetDirectories(dirPath));
			if (!folders.Any()) {
				return;
			}
			foreach (string folder in folders) {
				if (worker.CancellationPending) {
					e.Cancel = true;
					return;
				}
				deleteAllEmptyFolders(folder, ignoredFiletypes, worker, e);
				if (worker.CancellationPending) {
					e.Cancel = true;
					return;
				}
			}
			folders.Clear();
			folders.AddRange(Directory.GetDirectories(dirPath));
			if (!folders.Any()) {
				return;
			}
			List<string> fileEntries = new List<string>(Directory.GetFiles(dirPath));
			List<FileInfo> fileInfos = new List<FileInfo>(
				fileEntries.Select(fileEntry => new FileInfo(fileEntry)).ToList());
			foreach (FileInfo fileInfo in fileInfos) {
				string noDotExtension = fileInfo.Extension.Replace(".","");
				if (string.IsNullOrEmpty(noDotExtension) || !ignoredFiletypes.Contains(noDotExtension)) {
					delete = false;
					break;				
				}
			}
			if (delete) {
				try {
					Directory.Delete(dirPath, true);
				} catch (Exception ex) {
					log.Error("Couldn't delete " + dirPath + ": " + ex.Message);
				}
			}
		}

		/// <summary>
		/// Goes to the xth parent of the directory, x determined by the value of howOften
		/// </summary>
		/// <param name="directory">The directory to start from</param>
		/// <param name="howOften">how often to go to the parent directory</param>
		/// <returns>The path for the chosen parent dir</returns>
		public static string goUpwards(string directory, int howOften) {
			while (howOften > 0) {
				directory = Directory.GetParent(directory).FullName;
				howOften--;
			}
			return directory;
		}

		public static string goIntoFolder(string directory, string folder) {
			directory = directory.Trim(DIRECTORY_SEPARATORS);
			folder = folder.Trim(DIRECTORY_SEPARATORS);
			return directory + Path.DirectorySeparatorChar + folder;
		}

		/// <summary>
		/// figures out if a folder is a subdir of another folder, and how deep it is nested
		/// </summary>
		/// <param name="basepath">the base path</param>
		/// <param name="possibleSubdir">the path which is to be checked for subfolderity</param>
		/// <returns>-1 if not a subfolder, >=0 otherwise</returns>
		public static int getSubdirectoryLevel(string basepath, string possibleSubdir) {
			if (string.IsNullOrEmpty(basepath) || string.IsNullOrEmpty(possibleSubdir)) {
				return -1;
			}
			string[] basefolders = basepath.Split(DIRECTORY_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
			string[] possiblesubfolders = possibleSubdir.Split(DIRECTORY_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
			if (basefolders.Length > possiblesubfolders.Length)
				return -1;
			bool match = true;
			for (int i = 0; i < basefolders.Length; i++) {
				if (!basefolders[i].Equals(possiblesubfolders[i], StringComparison.CurrentCultureIgnoreCase)) {
					match = false;
					break;
				}
			}
			if (match) {
				return possiblesubfolders.Length - basefolders.Length;
			}
			return -1;
		}
	}
}
