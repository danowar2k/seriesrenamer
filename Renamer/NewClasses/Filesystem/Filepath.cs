using System;
using System.Text.RegularExpressions;
using System.IO;
using Renamer.NewClasses.Config;
using Renamer.NewClasses.Config.Application;

namespace Renamer.NewClasses.Filesystem
{
    public class Filepath
    {
        public static Filepath fromFileLocation(string filename) {
            return new Filepath(filename);
        }

        public static Filepath fromFileNameAndPath(string filename, string path) {
            return new Filepath(filename, path);
        }

        protected static char[] DIRECTORY_SEPERATORS = { System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar };
        protected static string EXTENSION_SEPERATOR = ".";

        protected string path;
        protected string filename;
        protected string extension;

        public Filepath() {
            this.Filename = "";
            this.Path = "";
        }
        public Filepath(string filename) {
            this.Filename = filename;
            extractPath();
        }
        public Filepath(string filename, string path) {
            if(isValidFilename(filename)){
                throw new Exception("Invalid filename. The path must not be in the filename, use th");
            }
            this.Path = path;
            this.Filename = filename;
        }

        #region private methods
        private void extractPath() {
            if (hasPath()) {
                this.Path = getPath();
                removePath();
            }
        }

        private bool hasPath() {
            return (getPathEndPosition() != -1);
        }
        private string getPath() {
            return filename.Substring(0, getPathEndPosition());
        }
        private void removePath() {
            if (hasPath())
                filename = filename.Substring(getPathEndPosition());
        }
        private int getPathEndPosition() {
            return filename.LastIndexOfAny(DIRECTORY_SEPERATORS);
        }

        private void extractAndRemoveExtension() {
            if (hasExtension()) {
                Extension = getExtension();
                removeExtension();
            }
        }

        private bool hasExtension() {
            return (getExtensionStartPosition() != -1);
        }
        private string getExtension() {
            return filename.Substring(getExtensionStartPosition());
        }
        private void removeExtension() {
            filename = filename.Substring(0, Math.Max(0, getExtensionStartPosition()-1));
        }
        private int getExtensionStartPosition() {
            return filename.LastIndexOf(EXTENSION_SEPERATOR)+1;
        }


        private bool isValidFilename(string filename) {
            if (filename.IndexOfAny(DIRECTORY_SEPERATORS) != -1) {
                return false;
            }
            return true;
        }

        private static string handleNullValue(string value) {
            return (string.IsNullOrEmpty(value) ? "" : value);
        }

        private void cleanup() {
            makePathUniform();
            replaceInvalidCharsInFilename();
        }

        private void makePathUniform(){
            if (string.IsNullOrEmpty(path))
                return;
            
            replaceAltDirectorySeperator();
            path = path.TrimEnd(DIRECTORY_SEPERATORS);
            if (path.Length == 2)
            {
                trailingSlashCheck();
            }
            replaceDoubleSlashes();
        }
        private void replaceDoubleSlashes()
        {
            while (path.IndexOf(System.IO.Path.DirectorySeparatorChar + System.IO.Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) >0)
            {
                path = path.Replace(System.IO.Path.DirectorySeparatorChar + System.IO.Path.DirectorySeparatorChar.ToString(), System.IO.Path.DirectorySeparatorChar.ToString());
            }
        }
        private void replaceInvalidCharsInPath() {
            path = replaceInvalidChars(path);
        }
        private void replaceInvalidCharsInFilename() {
            filename = replaceInvalidChars(filename);
        }
        private void replaceInvalidCharsInExtension() {
            filename = replaceInvalidChars(filename);
        }
        private string replaceInvalidChars(string str) {
            return Regex.Replace(
				str, invalidFileCharsPattern(), 
				Settings.Instance.getAppConfiguration().getSingleStringProperty(
				AppProperties.REPLACE_INVALID_CHARS_WITH_KEY));
        }

        private string invalidFileCharsPattern() {
            return "[" + Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())) + "]";
        }

        private void replaceAltDirectorySeperator() {
            path = path.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
        }

        private void trailingSlashCheck() {
            if(!pathHasTrailingSlash())
                addTrailingSlash();
        }

        private bool pathHasTrailingSlash() {
            return path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString());
        }

        private void addTrailingSlash() {
            path = path + System.IO.Path.DirectorySeparatorChar;
        }

        #endregion
        #region Properties
        public string Path {
            get {
                return path;
            }
            set {
                value = handleNullValue(value);
                path = value;
                makePathUniform();
            }
        }

        public string Name {
            get {
                return filename;
            }
            set {
                value = handleNullValue(value);
                filename = value;
                replaceInvalidCharsInFilename();
            }
        }

        public string Extension {
            get {
                return extension;
            }
            set {
                value = handleNullValue(value);
                extension = value.ToLower();
                replaceInvalidCharsInExtension();
            }
        }

        public string Fullfilename {
            get {
                return Path + System.IO.Path.DirectorySeparatorChar + Filename;
            }
        }

        public string Filename {
            get {
	            if (filename != "" && extension != "") {
                    return filename + EXTENSION_SEPERATOR + extension;
                }
	            if (filename != "" || extension != "") {
		            return "";
	            }
	            return "";
            }
	        set {
                value = handleNullValue(value);
                filename = value;
                extractAndRemoveExtension();
                replaceInvalidCharsInFilename();
                replaceInvalidCharsInExtension();
            }
        }

        public string[] Folders {
            get {
                return path.Split(DIRECTORY_SEPERATORS);
            }
        }

        public bool isEmpty {
            get {
                return Fullfilename == "";
            }
        }

#endregion

    }
}
