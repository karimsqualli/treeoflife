using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeOfLife
{
    public static class Loggers
    {
        static List<Logger> _List = new List<Logger>();
        static public List<Logger> List{ get { return _List; } }

        static public void Register(Logger _log)
        {
            if (_List.Contains(_log)) return;
            _List.Add(_log);
        }

        static public void Unregister(Logger _log)
        {
            _List.Remove(_log);
        }

        static public void Write(MessageLevelEnum _level, string _tag, string _message)
        {
            foreach(Logger log in _List)
                log.Write(_level, _tag, _message);
        }

        static public void WriteError(string _tag, string _message) { Write(MessageLevelEnum.Error, _tag, _message); }
        static public void WriteError(string _message) { Write(MessageLevelEnum.Error, LogTags.Program, _message); }

        static public void WriteWarning(string _tag, string _message) { Write(MessageLevelEnum.Warning, _tag, _message); }
        static public void WriteWarning(string _message) { Write(MessageLevelEnum.Warning, LogTags.Program, _message); }

        static public void WriteInformation(string _tag, string _message) { Write(MessageLevelEnum.Information, _tag, _message); }
        static public void WriteInformation(string _message) { Write(MessageLevelEnum.Information, LogTags.Program, _message); }

    }
}
