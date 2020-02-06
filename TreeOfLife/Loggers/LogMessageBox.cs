using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife
{
    class LogMessageBox : Logger
    {
        protected override void InternalWrite(MessageLevelEnum _level, string _tag, string _message)
        {
            string title = _level.ToString();
            if (!String.IsNullOrEmpty(_tag)) title += ", " + _tag;
            MessageBoxIcon icon = MessageBoxIcon.Warning;
            if (_level == MessageLevelEnum.Error || _level == MessageLevelEnum.FatalError)
                icon = MessageBoxIcon.Error;
            else if (_level == MessageLevelEnum.Information)
                icon = MessageBoxIcon.Information;
            MessageBox.Show(_message, title, MessageBoxButtons.OK, icon);
        }
    }
}
