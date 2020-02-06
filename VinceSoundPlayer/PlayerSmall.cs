using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace VinceSoundPlayer
{
    public partial class PlayerSmall : UserControl, IPlayerUserControl
    {
        PlayerBase _PlayerBase = new PlayerBase();
        //PlayerSmallData _Data = new PlayerSmallData();

        //---------------------------------------------------------------------------------------
        public PlayerSmall(DisplayModeEnum _displayMode = DisplayModeEnum.PlayPause)
        {
            InitializeComponent();

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            this.BackColor = Color.Transparent;

            DisplayMode = _displayMode;
            UpdateUI();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | 0x20;
                return cp;
            }
        }

        //---------------------------------------------------------------------------------------
        public bool AllowOpen = true;

        public string File
        {
            get { return _PlayerBase.File; }
            set
            {
                Manager.Instance.Close(this);
                _PlayerBase.File = value;
                UpdateUI();
            }
        }

        public void Close()
        {
            Manager.Instance.Close(this);
        }


        //=========================================================================================
        // PlayerUserControl interface
        //

        //---------------------------------------------------------------------------------------
        public void OnOpen() { UpdateUI(); }
        public void OnStop() { UpdateUI(); }
        public void OnPlay() { UpdateUI(); }
        public void OnClose() { UpdateUI(); }
        public void OnPause() { UpdateUI(); }

        public bool IsPlaying() { return Manager.Instance.IsPlaying(); }

        //=========================================================================================
        // Button visual
        //

        static public int IconSize { get => 12; }

        static public Bitmap PlayIcon { get { return Properties.Resources.Sound_Play; } }

        static public Bitmap PauseIcon { get { return Properties.Resources.Sound_pause; } }

        static public Bitmap StopIcon { get { return Properties.Resources.Sound_stop; } }

        //=========================================================================================
        // Interface
        //

        //---------------------------------------------------------------------------------------
        public enum DisplayModeEnum
        {
            Nothing = 0,
            PlayPause = 1,
            Stop = 2,
            PlayPauseStop = 3
        }

        private DisplayModeEnum _DisplayMode = DisplayModeEnum.Nothing;
        public DisplayModeEnum DisplayMode
        {
            get { return _DisplayMode; }
            set
            {
                if (_DisplayMode == value) return;
                _DisplayMode = value;

                if ((_DisplayMode & DisplayModeEnum.PlayPause) == DisplayModeEnum.PlayPause)
                    _ButtonPlayPause = new MyButton(this, PlayIcon);
                else
                    _ButtonPlayPause = null;

                if ((_DisplayMode & DisplayModeEnum.Stop) == DisplayModeEnum.Stop)
                    _ButtonStop = new MyButton(this, StopIcon);
                else
                    _ButtonStop = null;

                UpdateUI();
            }
        }

        private MyButton _ButtonPlayPause = null;
        private MyButton _ButtonStop = null;

        //---------------------------------------------------------------------------------------
        private void UpdateUI()
        {
            if (Manager.Instance.OpenedBy() != this)
            {
                if (_ButtonPlayPause != null)
                {
                    _ButtonPlayPause.Image = PlayIcon;
                    _ButtonPlayPause.Enabled = Manager.Instance.canOpen(this, _PlayerBase.File);
                    _ButtonPlayPause.Visible = true;
                }
                if (_ButtonStop != null)
                {
                    _ButtonStop.Enabled = false;
                    _ButtonStop.Visible = false;
                }
            }
            else
            {
                if (_ButtonPlayPause != null)
                {
                    _ButtonPlayPause.Visible = true;
                    if (Manager.Instance.IsPlaying())
                        _ButtonPlayPause.Image = PauseIcon;
                    else
                        _ButtonPlayPause.Image = PlayIcon;
                }
                if (_ButtonStop != null)
                {
                    _ButtonStop.Enabled = true;
                    _ButtonStop.Visible = true;
                }
            }
            InvalidateEx();
        }

        static public void StaticUpdateUI(PlayerSmallData _data)
        {
            if (Manager.Instance.OpenedBy() != _data)
            {
                if (_data.ButtonPlayPause != null)
                {
                    _data.ButtonPlayPause.Image = PlayIcon;
                    _data.ButtonPlayPause.Enabled = Manager.Instance.canOpen(_data, _data.File);
                    _data.ButtonPlayPause.Visible = true;
                }
                if (_data.ButtonStop != null)
                {
                    _data.ButtonStop.Enabled = false;
                    _data.ButtonStop.Visible = false;
                }
            }
            else
            {
                if (_data.ButtonPlayPause != null)
                {
                    _data.ButtonPlayPause.Visible = true;
                    if (Manager.Instance.IsPlaying())
                        _data.ButtonPlayPause.Image = PauseIcon;
                    else
                        _data.ButtonPlayPause.Image = PlayIcon;
                }
                if (_data.ButtonStop != null)
                {
                    _data.ButtonStop.Enabled = true;
                    _data.ButtonStop.Visible = true;
                }
            }
            //InvalidateEx();
        }

        //---------------------------------------------------------------------------------------
        protected void InvalidateEx()
        {
            if (Parent == null) return;
            Rectangle rc = new Rectangle(Location, Size);
            Parent.Invalidate(rc, true);
        }

        //---------------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            Rectangle R = ClientRectangle;

            if (_ButtonPlayPause != null)
                _ButtonPlayPause.Paint(e.Graphics, ref R);
            if (_ButtonStop != null)
                _ButtonStop.Paint(e.Graphics, ref R);
        }

        //---------------------------------------------------------------------------------------
        public static void StaticPaint(Graphics _graphics, Rectangle R, Point _mousePos, PlayerSmallData _data, PlayerSmallDisplayParams _params = null )
        {
            if (_data == null) return;
            SmoothingMode saveMode = _graphics.SmoothingMode;
            _graphics.SmoothingMode = SmoothingMode.HighQuality;

            if (_data.ButtonPlayPause != null)
            {
                if (_data.ButtonPlayPause.MouseHover && !_data.ButtonPlayPause.Rect.Contains(_mousePos))
                    _data.ButtonPlayPause.MouseHover = false;
                _data.ButtonPlayPause.Paint(_graphics, ref R, _params);
            }
            if (_data.ButtonStop != null)
            {
                if (_data.ButtonStop.MouseHover && !_data.ButtonStop.Rect.Contains(_mousePos))
                    _data.ButtonStop.MouseHover = false;
                _data.ButtonStop.Paint(_graphics, ref R, _params);
            }
            _graphics.SmoothingMode = saveMode;
        }

        //---------------------------------------------------------------------------------------
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_ButtonPlayPause != null)
                _ButtonPlayPause.MouseHover = _ButtonPlayPause.Rect.Contains(e.Location);
            if (_ButtonStop != null)
                _ButtonStop.MouseHover = _ButtonStop.Rect.Contains(e.Location);
        }

        //---------------------------------------------------------------------------------------
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (_ButtonPlayPause != null && _ButtonPlayPause.MouseHover)
            {
                ButtonPlayPause_Click(_PlayerBase, this, AllowOpen);
                UpdateUI();
                return;
            }
            if (_ButtonStop != null && _ButtonStop.MouseHover)
            {
                ButtonStop_Click();
                UpdateUI();
                return;
            }
            base.OnMouseClick(e);
        }

        //=========================================================================================
        // Events
        //

        //---------------------------------------------------------------------------------------
        public static bool ButtonPlayPause_Click(PlayerBase _player, IPlayerUserControl _itf, bool _allowOpen)
        {
            if (_player.File == null)
            {
                if (!_allowOpen) return false;
                _player.File = Manager.Instance.BrowseMusicFile();
            }
            if (_player.File == null) return false;

            if (Manager.Instance.OpenedBy() != _itf)
            {
                if (!Manager.Instance.canOpen(_itf, _player.File))
                    return false;
                Manager.Instance.Open(_itf, _player.File, false);
            }

            if (Manager.Instance.IsPlaying())
                Manager.Instance.Pause();
            else
                Manager.Instance.Play();
            return true;
        }

        //---------------------------------------------------------------------------------------
        public static void ButtonStop_Click()
        {
            Manager.Instance.Close();
        }

        //=========================================================================================
        // My Buttons
        //

        //---------------------------------------------------------------------------------------
        public class MyButton
        {
            public MyButton(PlayerSmall _owner, Image _image)
            {
                _Owner = _owner;
                Image = _image;
            }

            private PlayerSmall _Owner;

            public Image Image;
            public Rectangle Rect;

            private bool _MouseHover = false;
            public bool MouseHover
            {
                get { return _MouseHover; }
                set
                {
                    if (_MouseHover == value) return;
                    _MouseHover = value;
                    if (_Enabled && _Owner != null)
                        _Owner.InvalidateEx();
                }
            }

            private bool _Enabled = false;
            public bool Enabled
            {
                get { return _Enabled; }
                set
                {
                    if (_Enabled == value) return;
                    _Enabled = value;
                    if (_Owner != null)
                        _Owner.InvalidateEx();
                }
            }

            private bool _Visible = false;
            public bool Visible
            {
                get { return _Visible; }
                set
                {
                    if (_Visible == value) return;
                    _Visible = value;
                    if (_Owner != null)
                        _Owner.InvalidateEx();
                }
            }

            public void Paint(Graphics _graphics, ref Rectangle _rectangle, PlayerSmallDisplayParams _params = null)
            {
                if (_rectangle.Width < IconSize || _rectangle.Height < IconSize) return;
                if (!_Visible) return;
                _params = _params ?? PlayerSmallDisplayParams.Default;

                Rect = _rectangle;
                Rect.Width = IconSize;
                Rect.Y += (Rect.Height - IconSize) / 2;
                Rect.Height = IconSize;

                _rectangle.X += IconSize;
                _rectangle.Width -= IconSize;

                float color = Enabled ? (MouseHover ? _params.ColorHovered : _params.ColorEnabled) : _params.ColorDisabled;
                float[][] colorMatrixElements = {
                       new float[] {color, 0, 0, 0, 0},
                       new float[] {0, color, 0, 0, 0},
                       new float[] {0, 0, color, 0, 0},
                       new float[] {0, 0, 0, 1, 0},     // alpha scaling factor of 1 
                       new float[] {0, 0, 0, 0, 1}};

                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
                ImageAttributes imageAttributes = new ImageAttributes();
                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                _graphics.DrawImage(Image, Rect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes);
            }
        }
    }

    //---------------------------------------------------------------------------------------
    public class PlayerSmallData : IPlayerUserControl
    {
        public PlayerSmallData(PlayerSmall.DisplayModeEnum _displayMode = PlayerSmall.DisplayModeEnum.PlayPause)
        {
            DisplayMode = _displayMode;
            PlayerSmall.StaticUpdateUI(this);
        }

        public PlayerSmall.MyButton ButtonPlayPause = null;
        public PlayerSmall.MyButton ButtonStop = null;

        private PlayerSmall.DisplayModeEnum _DisplayMode = PlayerSmall.DisplayModeEnum.Nothing;
        public PlayerSmall.DisplayModeEnum DisplayMode
        {
            get { return _DisplayMode; }
            set
            {
                if (_DisplayMode == value) return;
                _DisplayMode = value;

                if ((_DisplayMode & PlayerSmall.DisplayModeEnum.PlayPause) == PlayerSmall.DisplayModeEnum.PlayPause)
                    ButtonPlayPause = new PlayerSmall.MyButton(null, PlayerSmall.PlayIcon);
                else
                    ButtonPlayPause = null;

                if ((_DisplayMode & PlayerSmall.DisplayModeEnum.Stop) == PlayerSmall.DisplayModeEnum.Stop)
                    ButtonStop = new PlayerSmall.MyButton(null, PlayerSmall.StopIcon);
                else
                    ButtonStop = null;

                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler OnChanged;

        PlayerBase _PlayerBase = new PlayerBase();

        //---------------------------------------------------------------------------------------
        public bool AllowOpen = true;

        public string File
        {
            get { return _PlayerBase.File; }
            set
            {
                Manager.Instance.Close(this);
                _PlayerBase.File = value;
                PlayerSmall.StaticUpdateUI(this);
            }
        }

        public void Close()
        {
            Manager.Instance.Close(this);
        }

        //=========================================================================================
        // PlayerUserControl interface
        //

        //---------------------------------------------------------------------------------------
        public void OnOpen() { PlayerSmall.StaticUpdateUI(this); }
        public void OnStop() { PlayerSmall.StaticUpdateUI(this); }
        public void OnPlay() { PlayerSmall.StaticUpdateUI(this); }
        public void OnClose() { PlayerSmall.StaticUpdateUI(this); }
        public void OnPause() { PlayerSmall.StaticUpdateUI(this); }

        public bool IsPlaying() { return Manager.Instance.IsPlaying(); }

        //---------------------------------------------------------------------------------------
        public void OnMouseMove(MouseEventArgs e)
        {
            if (ButtonPlayPause != null)
            {
                bool oldhover = ButtonPlayPause.MouseHover;
                ButtonPlayPause.MouseHover = ButtonPlayPause.Rect.Contains(e.Location);
                if (oldhover != ButtonPlayPause.MouseHover)
                    OnChanged?.Invoke(this, EventArgs.Empty);
            }
            if (ButtonStop != null)
            {
                bool oldhover = ButtonStop.MouseHover;
                ButtonStop.MouseHover = ButtonStop.Rect.Contains(e.Location);
                if (oldhover != ButtonStop.MouseHover)
                    OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        //---------------------------------------------------------------------------------------
        public bool OnMouseClick(MouseEventArgs e)
        {
            if (ButtonPlayPause != null && ButtonPlayPause.MouseHover)
            {
                if (e.Button == MouseButtons.Left && PlayerSmall.ButtonPlayPause_Click(_PlayerBase, this, AllowOpen))
                {
                    PlayerSmall.StaticUpdateUI(this);
                    OnChanged?.Invoke(this, EventArgs.Empty);
                }
                return true;
            }
            if (ButtonStop != null && ButtonStop.MouseHover)
            {
                if (e.Button == MouseButtons.Left)
                {
                    PlayerSmall.ButtonStop_Click();
                    PlayerSmall.StaticUpdateUI(this);
                    OnChanged?.Invoke(this, EventArgs.Empty);
                }
                return true;
            }
            return false;
        }

        //---------------------------------------------------------------------------------------
        public void OnMouseLeave()
        {
            if (ButtonPlayPause != null && ButtonPlayPause.MouseHover)
            {
                ButtonPlayPause.MouseHover = false;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
            if (ButtonStop != null && ButtonStop.MouseHover)
            {
                ButtonStop.MouseHover = false;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public class PlayerSmallDisplayParams
    {
        public float ColorDisabled { get; set; } = 0.8f;
        public float ColorEnabled { get; set; } = 0.5f;
        public float ColorHovered { get; set; } = 0f;

        public static PlayerSmallDisplayParams Default { get; private set; } = new PlayerSmallDisplayParams();
    }

}
