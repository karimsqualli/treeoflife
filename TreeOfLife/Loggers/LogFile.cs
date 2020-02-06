using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TreeOfLife
{
    class LogFile : Logger
    {
        public string Filename;

        DateTime _StartTime;
        public bool _WriteEllapsedTime;

        public LogFile(string _filename, bool _addEllapsedTime = false)
        {
            Filename = _filename;
            _WriteEllapsedTime = _addEllapsedTime;
            _StartTime = DateTime.Now;

            if (Filename == null)
                Filename = "TreeofLife.log";

            if (!Path.IsPathRooted(Filename))
                Filename = VinceToolbox.exeFunctions.exePath() + Path.DirectorySeparatorChar + Filename;

            string path = Path.GetDirectoryName(Filename);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (File.Exists(Filename))
                File.Delete(Filename);
        }

        protected override void InternalWrite(MessageLevelEnum _level, string _tag, string _message)
        {
            using (StreamWriter writer = new StreamWriter(Filename, true))
            {
                WriteHeader(writer, _level, _tag);
                writer.WriteLine(_message);
            }
        }

        void WriteHeader( StreamWriter _writer, MessageLevelEnum _level, string _tag)
        {
            string header = "[ " + _level.ToString();
            if (!String.IsNullOrEmpty(_tag)) header += ", " + _tag;
            if (_WriteEllapsedTime)
            {
                TimeSpan ts = DateTime.Now - _StartTime;
                header += ", " + ts.ToString(@"hh\:mm\:ss\.ff");
            }
            _writer.WriteLine( header + " ]");
        }
    }
}
