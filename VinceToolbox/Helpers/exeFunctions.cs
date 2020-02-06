using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows;

namespace VinceToolbox
{
    //=================================================================================================================
    //
    // some exe function helper
    //      exe path, file path in exe directory
    //      open explorer, open file 
    //
    //=================================================================================================================
    public class exeFunctions
    {
        //-------------------------------------------------------------------------------------------------------------
        static public string exePath()
        {
            string path = Assembly.GetEntryAssembly().Location;
            return Path.GetDirectoryName( path );
        }

        //-------------------------------------------------------------------------------------------------------------
        static public string exeSubPath( string _subPath )
        {
            return exePath() + Path.DirectorySeparatorChar + _subPath;
        }

        //-------------------------------------------------------------------------------------------------------------
        static public string exeFile(string _subPath, string _file)
        {
            return exePath() + Path.DirectorySeparatorChar + _subPath + Path.DirectorySeparatorChar + _file;
        }

        //-------------------------------------------------------------------------------------------------------------
        static public void gotoFile( string _file )
        {
            string path = _file.Replace('/', '\\');
            string arguments = "/select, " + @path;
            System.Diagnostics.Process.Start("explorer.exe", arguments);
        }

        //-------------------------------------------------------------------------------------------------------------
        static public void openFile(string _file, string _editor = null)
        {
            string path = _file.Replace('/', '\\');
            if (_editor != null && File.Exists(_editor))
            {
                System.Diagnostics.Process.Start(_editor, path);
            }
            else
            {
                string arguments = "/open, " + @path;
                System.Diagnostics.Process.Start("explorer.exe", arguments);
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        static public void execFile( string _command, string _args, string _dir = null, bool _restoreDir = false)
        {
            string path = _command.Replace('/', '\\');
            string oldDir = "";

            if (_dir != null && Directory.Exists(_dir))
            {
                if (_restoreDir) oldDir = Directory.GetCurrentDirectory();
                Environment.CurrentDirectory = _dir;
            }

            System.Diagnostics.Process.Start(_command, _args);
            
            if (oldDir != "")
                Environment.CurrentDirectory = oldDir;
        }
    }
}
