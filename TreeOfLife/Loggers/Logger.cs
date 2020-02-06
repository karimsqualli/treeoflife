using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeOfLife
{
    public abstract class Logger
    {
        public List<bool> ShowPerLevel;

        public Logger(VerboseModeEnum _mode = VerboseModeEnum.Full )
        {
            SetVerboseMode(_mode);
        }

        public void SetVerboseMode(VerboseModeEnum _mode)
        {
            if (ShowPerLevel == null)
            {
                ShowPerLevel = new List<bool>();
                foreach (MessageLevelEnum level in Enum.GetValues(typeof(MessageLevelEnum)))
                    ShowPerLevel.Add(false);
            }
            else
            {
                foreach (MessageLevelEnum level in Enum.GetValues(typeof(MessageLevelEnum)))
                    ShowPerLevel[(int)level] = false;
            }
            if (_mode == VerboseModeEnum.NothingAtAll) return;
            ShowPerLevel[(int)MessageLevelEnum.FatalError] = true;
            ShowPerLevel[(int)MessageLevelEnum.Error] = true;
            if (_mode == VerboseModeEnum.OnlyErrors) return;
            ShowPerLevel[(int)MessageLevelEnum.HighLevelWarning] = true;
            ShowPerLevel[(int)MessageLevelEnum.Warning] = true;
            ShowPerLevel[(int)MessageLevelEnum.LowLevelWarning] = true;
            if (_mode == VerboseModeEnum.ErrorsAndWarnings) return;
            ShowPerLevel[(int)MessageLevelEnum.Information] = true;
        }

        public void Write(MessageLevelEnum _level, string _tag, string _message )
        {
            if (!ShowPerLevel[(int)_level]) return;
            InternalWrite(_level, _tag, _message);
        }
        protected abstract void InternalWrite(MessageLevelEnum _level, string _tag, string _message);

        public void AddError(string _tag, string _message) { Write(MessageLevelEnum.Error, _tag, _message ); }
        public void AddWarning(string _tag, string _message) { Write(MessageLevelEnum.Warning, _tag, _message); }
        public void AddInformation(string _tag, string _message) { Write(MessageLevelEnum.Information, _tag, _message ); }
    }
}
