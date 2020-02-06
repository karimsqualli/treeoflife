using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Text.RegularExpressions;

namespace VinceToolbox
{
    //=================================================================================================================
    //
    // some file function helper
    //      format path ( \ => /, lower )
    //      relative path
    //      save text file
    //
    //=================================================================================================================
    public class fileFunctions
    {
        public const int c_format_slash = 1;
        public const int c_format_lower = 2;

        //-------------------------------------------------------------------------------------------------------------
        public static string formatPath(string _path, int _format)
        {
            bool slash = (_format & c_format_slash) == c_format_slash;
            bool lower = (_format & c_format_lower) == c_format_lower;
            if (slash & lower) return _path.Replace('\\', '/').ToLower();
            if (slash) return _path.Replace('\\', '/');
            if (lower) return _path.ToLower();
            return _path;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static string relativePath(string _root, string _path)
        {
            string root = formatPath(_root, c_format_slash);
            string path = formatPath(_path, c_format_slash);
            if (path.Length >= root.Length && path.Substring(0, root.Length).ToLower() == root.ToLower())
                return path.Substring(root.Length);
            return path;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool isInRoot(string _root, string _path)
        {
            string path = formatPath(_path, c_format_slash | c_format_lower);
            string rootPath = formatPath(_root, c_format_slash | c_format_lower);
            return (path.StartsWith(rootPath));
        }

        //-------------------------------------------------------------------------------------------------------------
        public static string absolutePath(string _root, string _path)
        {
            string relPath = relativePath(_root, _path);
            string absPath = formatPath(_root, c_format_slash);
            if (absPath[absPath.Length - 1] != '/' ) absPath += "/";
            return absPath + _path;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool validFilename(string _filename )
        {
            string expression = "^[^" + string.Join("", Array.ConvertAll(Path.GetInvalidFileNameChars(), x => Regex.Escape(x.ToString()))) + "]+$";
            Regex validator = new Regex(expression, RegexOptions.Compiled);
            return validator.IsMatch(_filename);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static string cleanFilename(string _filename)
        {
            string expression = "[" + string.Join("", Array.ConvertAll(Path.GetInvalidFileNameChars(), x => Regex.Escape(x.ToString()))) + "]";
            Regex cleaner = new Regex(expression, RegexOptions.Compiled);
            return cleaner.Replace(_filename, "");
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool validDirectory(string _path)
        {
            string expression = "^[^" + string.Join("", Array.ConvertAll(Path.GetInvalidPathChars(), x => Regex.Escape(x.ToString()))) + "]+$";
            Regex validator = new Regex(expression, RegexOptions.Compiled);
            return validator.IsMatch(_path);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static string cleanDirectory(string _path)
        {
            string expression = "[" + string.Join("", Array.ConvertAll(Path.GetInvalidPathChars(), x => Regex.Escape(x.ToString()))) + "]";
            Regex cleaner = new Regex(expression, RegexOptions.Compiled);
            return cleaner.Replace(_path, "");
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool validPath(string _path)
        {
            try
            {
                string file = Path.GetFileName(_path);
                string directory = Path.GetDirectoryName(_path);
                return validFilename(file) && validDirectory(directory);
            }
            catch
            {
                return false;
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        public static string cleanPath(string _path)
        {
            try
            {
                string file = cleanFilename(Path.GetFileName(_path));
                string directory = cleanPath(Path.GetDirectoryName(_path));
                return directory + "\\" + file;
            }
            catch
            {
                return _path;
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool saveTextFile(string _filename, string _content)
        {
            try
            {
                using (StreamWriter outfile = new StreamWriter(_filename))
                {
                    outfile.Write(_content);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool readTextFile(string _filename, ref string _content )
        {
            _content = "";
            try
            {
                _content = File.ReadAllText(_filename);
                return true;
            }
            catch 
            {
                _content = "";
                return false; 
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        public static FileInfo findInPath(DirectoryInfo _dir, string _filename)
        {
            try
            {
                FileInfo[] files = _dir.GetFiles();
                if (files == null) return null;

                // search for first occurrence
                FileInfo fi = null;
                fi = files.FirstOrDefault(f => f.Name == _filename);
                if (fi != null) return fi;

                foreach (DirectoryInfo d in _dir.GetDirectories())
                {
                    fi = findInPath(d, _filename);
                    if (fi != null) return fi;
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // error accessing files or dir
                System.Diagnostics.Debug.Write(ex.Message);
            }

            return null;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static FileInfo findInPath(string _path, string _filename)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(_path);
                if (!dir.Exists) return null;
                return findInPath(dir, _filename);
            }
            catch (UnauthorizedAccessException ex)
            {
                // error accessing files or dir
                System.Diagnostics.Debug.Write(ex.Message);
                return null;
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        static public bool renameFile(string _oldName, string _newName, out string _error )
        {
            if (!File.Exists( _oldName ))
            {
                _error = string.Format("File doesn't exist !");
                return false;
            }

            if (!validPath(_newName))
            {
                _error = string.Format("File name is invalid !");
                return false;
            }

            try
            {
                if (File.Exists(_newName))
                    File.Delete(_newName);
                File.Move(_oldName, _newName);
            }
            catch (System.Exception ex)
            {
                _error = string.Format("Error while trying to rename file: \n{0}", ex.Message);
                return false;
            }

            _error = "";
            return true;
        }
    }
}
