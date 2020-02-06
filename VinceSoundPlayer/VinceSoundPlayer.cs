using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VinceSoundPlayer
{
    class Manager
    {
        //List<MediaControl.AudioPlayer> MultiAudioPlayers = null;
        
        private Manager()
        {
        }

        private static Manager _Instance = new Manager();
        public static Manager Instance { get { return _Instance; } }

        //=========================================================================================
        // File
        //
        static private string _SupportedExtension = "*.mp3;*.ram;*.rm;*.wav;*.wma;*.mid";

        //-----------------------------------------------------------------------------------------
        public bool SupportedFile(string _filename)
        {
            string extension = "*." + System.IO.Path.GetExtension(_filename);
            int index = _SupportedExtension.IndexOf(extension);
            if (index == -1) return false;
            index += 5;
            if (_SupportedExtension[index] == ';') return true;
            if (_SupportedExtension[index] == 0) return true;
            return false;
        }

        //-----------------------------------------------------------------------------------------
        public string OpenDialogFilter()
        {
            string filter = "Music Formats|";
            filter += _SupportedExtension;

            string[] extensions = _SupportedExtension.Split( new char[] {';'});
            foreach (string ext in extensions)
            {
                if (!ext.StartsWith("*.")) continue;
                filter += "|" + ext.Substring(2) + " (" + ext + ")|" + ext;
            }
            return filter;
        }

        //-----------------------------------------------------------------------------------------
        public string BrowseMusicFile()
        {
            DialogResult ResultDialog;
            try
            {
                OpenFileDialog OpenDialog = new OpenFileDialog();
                OpenDialog.Filter = OpenDialogFilter();
                OpenDialog.RestoreDirectory = true;
                OpenDialog.FileName = "";
                OpenDialog.Title = "Choose Music File:";
                OpenDialog.CheckFileExists = true;
                OpenDialog.CheckPathExists = true;
                OpenDialog.Multiselect = false; //choose one file
                ResultDialog = OpenDialog.ShowDialog();
                if (ResultDialog == DialogResult.OK)
                    return OpenDialog.FileName;
            }
            catch {}
            return null;
        }

        //=========================================================================================
        // Exclusive play
        //
        IPlayerUserControl _ExclusiveOwner = null;
        string _ExclusiveFile = null;
        bool _ExclusiveLock = false;
        MediaControl.AudioPlayer _ExclusiveAudioPlayer = new MediaControl.AudioPlayer();

        public bool canOpen(object _owner, string _file)
        {
            if (_owner == null || _file == null) return false;
            if (_ExclusiveLock) return (_owner == _ExclusiveOwner);
            return true;
        }

        public void Open(IPlayerUserControl _owner, string _file, bool _autoPlay = true)
        {
            if (_ExclusiveLock)
                return;
            
            Close();
            _ExclusiveOwner = _owner;
            _ExclusiveFile = _file;

            _ExclusiveAudioPlayer.Open(_file);
            if (_ExclusiveOwner != null) _ExclusiveOwner.OnOpen();
            if (_autoPlay && _ExclusiveAudioPlayer.bFileIsOpen)
            {
                _ExclusiveAudioPlayer.Play();
                if (_ExclusiveOwner != null) _ExclusiveOwner.OnPlay();
            }
        }

        public bool IsOpened()
        {
            return (_ExclusiveAudioPlayer.bFileIsOpen);
        }

        public object OpenedBy()
        {
            return _ExclusiveOwner;
        }

        public bool Close(IPlayerUserControl _owner = null)
        {
            if (_owner != null && _ExclusiveOwner != _owner) return false;
            if (IsPlaying()) _ExclusiveAudioPlayer.Stop();
            if (_ExclusiveOwner != null) _ExclusiveOwner.OnStop();
            if (IsOpened()) _ExclusiveAudioPlayer.Close();
            IPlayerUserControl oldOwner = _ExclusiveOwner;
            _ExclusiveFile = null;
            _ExclusiveOwner = null;
            if (oldOwner != null) oldOwner.OnClose();
            return true;
        }

        public bool IsPlaying()
        {
            return (_ExclusiveAudioPlayer.Status() == "playing");
        }

        public void Play()
        {
            if (_ExclusiveAudioPlayer.bFileIsOpen)
            {
                if (_ExclusiveAudioPlayer.Pause)
                {
                    _ExclusiveAudioPlayer.Pause = false;
                    if (_ExclusiveOwner != null) _ExclusiveOwner.OnPlay();
                }
                else
                {
                    _ExclusiveAudioPlayer.Play();
                    if (_ExclusiveOwner != null) _ExclusiveOwner.OnPause();
                }
                //_ExclusiveAudioPlayer.Pause = false;
            }
        }

        public void Pause()
        {
            if (_ExclusiveAudioPlayer.bFileIsOpen)
            {
                _ExclusiveAudioPlayer.Pause = true;
                if (_ExclusiveOwner != null) _ExclusiveOwner.OnPause();
            }
        }

        
    }
}
