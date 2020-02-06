using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinceSoundPlayer
{
    public class PlayerBase
    {
        public event EventHandler OnFileChanging = null;
        public event EventHandler OnFileChanged = null;
        string _File = null;
        public string File
        {
            get { return _File; }
            set
            {
                if (_File == value) return;
                OnFileChanging?.Invoke(this, new EventArgs());
                _File = value;
                OnFileChanged?.Invoke(this, new EventArgs());
            }
        }
    }

    public interface IPlayerUserControl
    {
        void OnOpen();
        void OnPlay();
        void OnPause();
        void OnStop();
        void OnClose();
    }
}
