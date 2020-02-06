using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Threading;
using System.Runtime.InteropServices;

namespace TreeOfLife
{
    //=============================================================================================
    // Tip form
    //
    public partial class TipForm : Form
    {
        //-----------------------------------------------------------------------------------------
        public TipForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            InitializeComponent();
        }

        public bool DisplaySynonyms
        {
            get { return true; }
        }

        public bool DisplaySpeciesCount
        {
            get { return true; }
        }


        //-----------------------------------------------------------------------------------------
        TaxonTreeNode _Taxon = null;
        public TaxonTreeNode Taxon
        {
            get { return _Taxon; }
            set
            {
                _Taxon = value;
                _ImageDesc = null;
                _CollectionName = null;
                Adapt();
            }
        }

        //-----------------------------------------------------------------------------------------
        TaxonImageDesc _ImageDesc = null;
        string _CollectionName = null;
        public TaxonImageDesc ImageDesc
        {
            get { return _ImageDesc; }
            set
            {
                _ImageDesc = value;
                _CollectionName = _ImageDesc == null ? null : TaxonImages.Manager.CollectionName(_ImageDesc.CollectionId);
                Adapt();
            }
        }

        string _OtherText = null;
        public string OtherText
        {
            get => _OtherText;
            set
            {
                _OtherText = value;
                AdaptToOtherText();
            }
        }

        //-----------------------------------------------------------------------------------------
        // adapt size to displayed taxon
        //
        int _LineCount;
        string[] _Lines = new string[20];
        float[] _LinesY = new float[20];
        float[] _LinesH = new float[20];
        Brush[] _LinesBrush = new Brush[20];
        object[] _LinesObject = new object[20];

        private void AddLine( Graphics g, string _line, Brush _brush, ref SizeF _totalSize )
        {
            SizeF size = g.MeasureString(_line, Font);

            _Lines[_LineCount] = _line;
            _LinesBrush[_LineCount] = _brush;
            _LinesY[_LineCount] = _totalSize.Height;
            _LinesH[_LineCount] = size.Height;
            _LinesObject[_LineCount] = null;
            _LineCount++;

            _totalSize.Height += size.Height + 2;
            _totalSize.Width = Math.Max(_totalSize.Width, size.Width);
        }

        private void AddLineRedListCategory( Graphics g, RedListCategoryEnum _value, Brush _brush, ref SizeF _totalSize )
        {
            SizeF size = g.MeasureString(RedListCategoryExt.GetName(_value), Font);

            _Lines[_LineCount] = null;
            _LinesBrush[_LineCount] = _brush;
            _LinesY[_LineCount] = _totalSize.Height;
            _LinesH[_LineCount] = size.Height;
            _LinesObject[_LineCount] = _value;
            _LineCount++;

            _totalSize.Height += size.Height + 2;
            _totalSize.Width = Math.Max(36 + _totalSize.Width, size.Width);
        }
        
        private void Adapt()
        {
            if (_Taxon == null) return;
            Graphics g = CreateGraphics();

            _LineCount = 0;
            SizeF size = new SizeF(0.0f, 2.0f);
            AddLine(g, _Taxon.Desc.RefMainName, Brushes.White, ref size);

            AddLineRedListCategory(g, _Taxon.Desc.RedListCategory, Brushes.White, ref size);

            if (DisplaySynonyms && _Taxon.Desc.RefMultiName.HasSynonym)
            {
                int countSyn = 0;
                string[] synonyms = _Taxon.Desc.RefMultiName.GetSynonymsArray();
                foreach (string s in synonyms)
                {
                    AddLine(g, "    " + s, Brushes.LightGray, ref size);
                    if (++countSyn == 8) break;
                }
            }

            if (_Taxon.Desc.HasFrenchName)
            {
                AddLine(g, _Taxon.Desc.FrenchMultiName.Main, Brushes.White, ref size); 
                if (DisplaySynonyms && _Taxon.Desc.FrenchMultiName.HasSynonym)
                {
                    int countSyn = 0;
                    string[] synonyms = _Taxon.Desc.FrenchMultiName.GetSynonymsArray();
                    foreach (string s in synonyms)
                    {
                        AddLine(g, "    " + s, Brushes.LightGray, ref size);
                        if (++countSyn == 8) break;
                    }
                }
            }

            if (DisplaySpeciesCount && _Taxon.Desc.ClassicRank != ClassicRankEnum.SousEspece)
            {
                string line;
                if (_Taxon.Desc.ClassicRank == ClassicRankEnum.Espece)
                {
                    int count = 0;
                    _Taxon.ParseNodeDesc((n) => { if (n.ClassicRank == ClassicRankEnum.SousEspece) count++; });
                    line = count.ToString() + " sub species";
                }
                else
                {
                    int count = 0;
                    _Taxon.ParseNodeDesc((n) => { if (n.ClassicRank == ClassicRankEnum.Espece) count++; });
                    line = count.ToString() + " species";
                }
                AddLine(g, line, Brushes.WhiteSmoke, ref size);
            }

            if (!String.IsNullOrEmpty(_CollectionName))
                AddLine(g, "( " + _CollectionName + " )", Brushes.WhiteSmoke, ref size);

            Width = (int) size.Width + 4;
            Height = (int) size.Height;

            Invalidate();
        }

        void AdaptToOtherText()
        {
            if (_OtherText == null) return;
            Graphics g = CreateGraphics();

            string[] lines = _OtherText.Split('\n');
            _LineCount = 0;
            SizeF size = new SizeF(0.0f, 2.0f);

            foreach (string line in lines)
            {
                AddLine(g, line, Brushes.White, ref size);
            }

            Width = (int)size.Width + 4;
            Height = (int)size.Height;
            Invalidate();
        }

        //-----------------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_Taxon == null) return;
            for (int i = 0; i < _LineCount; i++)
            {
                if (_Lines[i] != null) 
                    e.Graphics.DrawString(_Lines[i], Font, _LinesBrush[i], 2, _LinesY[i]);
                else if (_LinesObject[i] is RedListCategoryEnum redlist)
                {
                    Rectangle R = Rectangle.FromLTRB(2, (int)_LinesY[i], 34, (int)(_LinesY[i] + _LinesH[i]));
                    RedListCategoryExt.PaintTag(redlist, e.Graphics, R, Font);
                    e.Graphics.DrawString(RedListCategoryExt.GetName(redlist), Font, _LinesBrush[i], 34, _LinesY[i]);
                }
            }
        }

        //-----------------------------------------------------------------------------------------
        // do not accept any focus, it's useless and prevents mouse wheel to work on graph.
        protected override void OnGotFocus(EventArgs e)
        {
 	        base.OnGotFocus(e);
            if (TaxonUtils.MainWindow != null)
                TaxonUtils.MainWindow.Focus();
        }
        
    }

    //=============================================================================================
    // Tip manager
    //
    public static class TipManager
    {
        //-----------------------------------------------------------------------------------------
        private static TipForm _Form = null;
        private static Thread _Thread = null;
        private static bool _Exit = false;
        private static bool _Pause = false;

        //-----------------------------------------------------------------------------------------
        private static TaxonTreeNode _TaxonAsked = null;
        private static TaxonImageDesc _ImageDescAsked = null;
        private static string _TextAsked = null;

        private static TaxonTreeNode _TaxonDisplayed = null;
        private static TaxonImageDesc _ImageDescDisplayed = null;
        private static string _TextDisplayed = null;

        private static Point _PositionMarker;
        private static DateTime _TimeMarker;

        //-----------------------------------------------------------------------------------------
        static public void Start() 
        {
            _Exit = false;

            _Form = new TipForm();
            _Form.Show();
            _Form.Visible = false;
            _Form.TopMost = true;

            ThreadStart start = new ThreadStart(TipManagerLoop);
            _Thread = new Thread(start);
            _Thread.Start();
        }

        //-----------------------------------------------------------------------------------------
        static public void Pause(bool _pause)
        {
            _Pause = _pause;
        }

        //-----------------------------------------------------------------------------------------
        static public void Stop()
        {
            _Exit = true;
        }

        //-----------------------------------------------------------------------------------------
        static public void SetTaxon(TaxonTreeNode _taxon, TaxonImageDesc _imageDesc )
        {
            if (_taxon == _TaxonAsked && _imageDesc == _ImageDescAsked ) return;
            _TaxonAsked = _taxon;
            _ImageDescAsked = _imageDesc;
        }

        //-----------------------------------------------------------------------------------------
        static public void SetText( string _text )
        {
            if (_text == _TextAsked) return;
            _TaxonAsked = null;
            _ImageDescAsked = null;
            _TextAsked = _text;
        }

        //-----------------------------------------------------------------------------------------
        static private bool CheckPosition()
        {
            if (Math.Abs(Cursor.Position.X - _PositionMarker.X) < 5 && Math.Abs(Cursor.Position.Y - _PositionMarker.Y) < 5)
                return true;
            _TimeMarker = DateTime.Now;
            _PositionMarker = Cursor.Position;
            return false;
        }

        //-----------------------------------------------------------------------------------------
        static private bool CheckTime()
        {
            TimeSpan ts = DateTime.Now - _TimeMarker;
            return (ts.TotalMilliseconds > 500);
        }

        //-----------------------------------------------------------------------------------------
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        static private bool CheckForegroundApplication()
        {
            // Get process ID of foreground window
            IntPtr handle = GetForegroundWindow();
            uint foregroundProcessID = 0;
            uint result = GetWindowThreadProcessId(handle, out foregroundProcessID);

            // Get my process Id
            System.Diagnostics.Process myProcess = System.Diagnostics.Process.GetCurrentProcess();
            uint myProcessID = (uint)myProcess.Id;

            // display tip only if foreground window is owned by TOL process
            return myProcessID == foregroundProcessID;
        }

        //-----------------------------------------------------------------------------------------
        static private void Hide()
        {
            if (_Form.Visible)
                _Form.Visible = false;
        }

        //-----------------------------------------------------------------------------------------
        static private void Show()
        {
            if (!_Form.Visible)
                _Form.Visible = true;
            _Form.Enabled = false;
            //_Form.Location = new Point(Cursor.Position.X + 10, Cursor.Position.Y + 10);

            Point location = new Point(Cursor.Position.X + 10, Cursor.Position.Y + 10);

            System.Windows.Forms.Screen[] screens = System.Windows.Forms.Screen.AllScreens;
            foreach (System.Windows.Forms.Screen screen in screens)
            {
                Rectangle rScreen = screen.Bounds;
                if (rScreen.Contains(Cursor.Position))
                {
                    if (location.X + _Form.Width > rScreen.Right)
                        location.X = rScreen.Right - _Form.Width;
                    if (location.Y + _Form.Height > rScreen.Bottom)
                        location.Y = rScreen.Bottom - _Form.Height;
                    break;
                }
            }

            _Form.Location = location;
        }

        //-----------------------------------------------------------------------------------------
        static private void DisplayTaxon()
        {
            _Form.Taxon = _TaxonDisplayed;
            _Form.ImageDesc = _ImageDescDisplayed;
        }

        //-----------------------------------------------------------------------------------------
        static private void DisplayText()
        {
            _Form.OtherText = _TextDisplayed;
        }

        //-----------------------------------------------------------------------------------------
        static void TipManagerLoop()
        {
            while (!_Exit)
            {
                bool hide = _Pause;
                if (_TextAsked != null)
                    hide |= _TextAsked.Length == 0;
                else
                {
                    hide |= (_TaxonAsked == null);
                    hide |= (_TaxonDisplayed != null && _TaxonDisplayed != _TaxonAsked);
                    hide |= (_ImageDescDisplayed != null && _ImageDescDisplayed != _ImageDescAsked);
                }

                if (hide)
                {
                    _Form.BeginInvoke(new Action(() => Hide()));
                    _TaxonDisplayed = null;
                    _ImageDescDisplayed = null;
                    _TextDisplayed = null;
                    _TimeMarker = DateTime.Now;
                    _PositionMarker = Cursor.Position;
                }
                else if (_TextDisplayed == null && _TextAsked != null)
                {
                    if (CheckPosition() && CheckTime() && CheckForegroundApplication())
                    {
                        _TextDisplayed = _TextAsked;
                        _Form.BeginInvoke(new Action(() => DisplayText()));
                    }
                }
                else if (_TaxonDisplayed == null && _TaxonAsked != null)
                {
                    if (CheckPosition() && CheckTime() && CheckForegroundApplication())
                    {
                        _TaxonDisplayed = _TaxonAsked;
                        _ImageDescDisplayed = _ImageDescAsked;
                        _Form.BeginInvoke(new Action(() => DisplayTaxon()));
                    }
                }
                else 
                {
                    _Form.BeginInvoke(new Action(() => Show()));
                }
                Thread.Sleep(50);
            }
        }
    }
}
