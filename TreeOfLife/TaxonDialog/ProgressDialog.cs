using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.TaxonDialog
{
    public partial class ProgressDialog : Localization.Form
    {
        public ProgressDialog()
        {
            InitializeComponent();
            TopMost = true;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Opaque, true);

            _FontTitle = listBox.Font;
            _FontInfo = new Font(_FontTitle.FontFamily, 7, FontStyle.Italic);
        }

        private void ProgressDialog_Load(object sender, EventArgs e)
        {
            _RefreshTimer = new Timer();
            _RefreshTimer.Interval = 100;
            _RefreshTimer.Tick += RefreshTimer_Tick;
            _RefreshTimer.Start();
            Application.DoEvents();
        }

        Timer _RefreshTimer = null;
        bool NeedRefresh = false;
        bool HasEndings = false;

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            if (HasEndings)
            {
                HasEndings = false;
                ProgressItem piToRemove = null;
                foreach (ProgressItem pi in listBox.Items.OfType<ProgressItem>())
                {
                    if (pi.IsEnding)
                    {
                        HasEndings = true;
                        NeedRefresh = true;
                        if (pi.GetEndRatio() >= 1)
                        {
                            piToRemove = pi;
                            break;
                        }

                    }
                }
                if (piToRemove != null)
                    listBox.Items.Remove(piToRemove);

                if (HasEndings)
                {
                    listBox.DrawMode = DrawMode.Normal;
                    listBox.DrawMode = DrawMode.OwnerDrawVariable;
                    Application.DoEvents();
                }
            }

            if (NeedRefresh)
            {
                NeedRefresh = false;
                listBox.Invalidate();
                Application.DoEvents();
            }
        }

        public ProgressItem Add( string _title, string _info, float _min, float _max)
        {
            ProgressItem pi = new ProgressItem() { Title = _title, Info = _info, Min = _min, Max = _max };
            pi.OnUpdate += OnProgressItemUpdate;
            pi.OnEnded += OnProgressItemEnded;
            listBox.Items.Add(pi);
            NeedRefresh = true;
            Application.DoEvents();
            return pi;
        }

        protected void OnProgressItemUpdate(object sender, EventArgs e)
        {
            NeedRefresh = true;
            Application.DoEvents();
        }

        protected void OnProgressItemEnded(object sender, EventArgs e)
        {
            HasEndings = true;
            NeedRefresh = true;
            Application.DoEvents();
        }

        Font _FontTitle;
        Font _FontInfo;

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= listBox.Items.Count) return;
            ProgressItem pi = listBox.Items[e.Index] as ProgressItem;
            if (pi == null) return;
            e.Graphics.FillRectangle(Brushes.DimGray, e.Bounds);
            e.Graphics.Clip = new Region(e.Bounds);

            int height = 10;
            if (pi.Title != null)
            {
                height += 16;
                PointF pos = new PointF(e.Bounds.Left + 4, e.Bounds.Top + 2);
                e.Graphics.DrawString(pi.Title, _FontTitle, Brushes.WhiteSmoke, pos);
            }

            if (pi.Info != null)
            {
                height += 14;
                PointF pos = new PointF(e.Bounds.Left + 20, e.Bounds.Top + 16);
                e.Graphics.DrawString(pi.Info, _FontInfo, Brushes.Gainsboro, pos);
            }

            Rectangle barRect = e.Bounds;
            barRect.X += 4;
            barRect.Width -= 8;
            barRect.Y += height - 8;
            barRect.Height = 5;
            e.Graphics.FillRectangle(Brushes.Black, barRect);
            barRect.Width = (int)(pi.Ratio * barRect.Width);
            e.Graphics.FillRectangle(Brushes.Green, barRect);

            e.Graphics.DrawLine(Pens.Silver, new Point(e.Bounds.Left, e.Bounds.Bottom - 1), new Point(e.Bounds.Right, e.Bounds.Bottom - 1));
        }

        private void listBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= listBox.Items.Count) return;
            ProgressItem pi = listBox.Items[e.Index] as ProgressItem;
            if (pi == null) return;
            e.ItemHeight = 10;
            if (pi.Title != null) e.ItemHeight += 16;
            if (pi.Info != null) e.ItemHeight += 14;
            if (pi.IsEnding)
                e.ItemHeight = (int)(e.ItemHeight * (1 - pi.GetEndRatio()));
        }

        
    }

    public class ProgressItem
    {
        public string Title = null;
        public string Info = null;

        public float Min;
        public float Max;

        private float _Current = 0;
        public float Current { get { return _Current; } }

        public event EventHandler OnUpdate;
        public event EventHandler OnEnded;

        private float _Ratio = 0;
        public float Ratio
        {
            get { return _Ratio; }
            private set
            {
                if (_Ratio == value) return;
                _Ratio = value;
                if (OnUpdate != null)
                    OnUpdate(this, EventArgs.Empty);
            }
        }

        public void Inc(string _info = null, float step = 1) { Update(_Current + step, _info); }

        public void Update( float value, string _info = null, string _title = null)
        {
            if (_info != null) Info = _info;
            if (_title != null) Title = _title;
            if (value <= Min)
            {
                _Current = Min;
                Ratio = 0;
            }
            else if (value >= Max)
            {
                _Current = Max;
                Ratio = 1;
            }
            else
            {
                _Current = value;
                Ratio = (Current - Min) / (Max - Min);
            }
        }

        public bool IsEnding = false;
        DateTime _EndTime;
        float _EndLength;

        public void End()
        {
            _EndTime = DateTime.Now;
            _EndLength = 1000;
            IsEnding = true;
            if (OnEnded != null)
                OnEnded(this, EventArgs.Empty);
        }

        public float GetEndRatio()
        {
            float ms = (float) (DateTime.Now - _EndTime).TotalMilliseconds;
            if (ms > _EndLength) return 1;
            return (ms / _EndLength);
        }
    }

    /* to test progress 
        public void Activate()
        {
            using (ProgressDialog progressDlg = new TaxonDialog.ProgressDialog())
            {
                progressDlg.StartPosition = FormStartPosition.CenterScreen;
                progressDlg.Show();

                ProgressItem aaa = progressDlg.Add("aaa 200 end", null, 0, 199);
                for (int i = 0; i < 200; i++)
                {
                    aaa.Update(i);
                    System.Threading.Thread.Sleep(10);
                }
                aaa.End();

                ProgressItem bbb = progressDlg.Add("bbb 500 end", "", 0, 499);
                for (int i = 0; i < 500; i++)
                {
                    bbb.Update(i, "informations " + i.ToString());
                    System.Threading.Thread.Sleep(10);
                }
                bbb.End();

                ProgressItem ccc = progressDlg.Add("ccc 400 not end", "", 0, 399);
                for (int i = 0; i < 400; i++)
                {
                    ccc.Update(i, "informations " + i.ToString());
                    System.Threading.Thread.Sleep(10);
                }

                ProgressItem ddd = progressDlg.Add("ddd 400 End", null, 0, 399);
                for (int i = 0; i < 400; i++)
                {
                    ddd.Update(i);
                    System.Threading.Thread.Sleep(10);
                }
                ddd.End();

                ProgressItem eee = progressDlg.Add("eee 1000", "pouet", 0, 1000 - 1);
                for (int i = 0; i < 1000; i++)
                {
                    eee.Update(i);
                    System.Threading.Thread.Sleep(10);
                }

            }
        }*/
}
