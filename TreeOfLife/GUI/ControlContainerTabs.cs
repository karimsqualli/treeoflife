using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TreeOfLife.GUI;
using TreeOfLife.Controls;

namespace TreeOfLife.GUI
{
    public partial class ControlContainerTabs : UserControl, ControlContainerInterface
    {
        //-------------------------------------------------------------------
        public ControlContainerTabs()
        {
            InitializeComponent();

            BorderStyle = BorderStyle.FixedSingle;
            Dock = DockStyle.Fill; 

            ControlContainerInterfaceList.register(this);
        }
        public List<GuiControlInterface> Children { get; } = new List<GuiControlInterface>();

        //-------------------------------------------------------------------
        public Control GetControl() { return this; }

        //-------------------------------------------------------------------
        public void SetFocus(GuiControlInterface _control )
        {
            if (Children.Contains(_control))
                Current = _control as GuiControlInterface;
        }

        //-------------------------------------------------------------------
        public void Add(GuiControlInterface _itf, bool _visible = true)
        {
            if (Children.IndexOf(_itf) != -1) return;
            if (_itf.Control == null) return;
            if (_itf.Control.Parent != null) return;

            _itf.Control.Parent = this;
            _itf.Control.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _itf.Control.Left = 0;
            _itf.Control.Width = Width;
            _itf.Control.Top = Theme.Current.TabHeader_Height;
            _itf.Control.Height = Height - Theme.Current.TabHeader_Height;
            _itf.Control.Visible = false;
            _itf.OwnerContainer = this;

            Children.Add(_itf);

            if (_visible || _Current == null)
                Current = _itf;
            Invalidate();
        }

        //-------------------------------------------------------------------
        public void Remove(GuiControlInterface _itf, bool _closeIfEmpty)
        {
            int index = Children.IndexOf(_itf);
            if (index == -1) return;
            if (_itf.Control == null) return;

            if (!_itf.Control.Visible)
                index = -1;
            else
                Current = null;

            Children.Remove(_itf);
            _itf.OwnerContainer = null;

            if (_closeIfEmpty)
                CloseIfEmpty();

            if (index >= Children.Count) index--;
            if (index >= 0)
                Current = Children[index];
            Invalidate();
        }

        //-------------------------------------------------------------------
        void CloseIfEmpty()
        {
            if (Children.Count == 0 && Parent is SplitterPanel && Parent.Parent is ControlContainerSplitter && Parent.Parent.Parent != null)
            {
                ControlContainerSplitter split = Parent.Parent as ControlContainerSplitter;
                Control parent = split.Parent;

                parent.SuspendLayout();
                ControlCollection toRemove = null;
                if (split.Panel1.Controls.Contains(this))
                    toRemove = split.Panel2.Controls;
                else
                    toRemove = split.Panel1.Controls;

                List<Control> toKeep = new List<Control>();

                foreach (Control c in toRemove) toKeep.Add(c);
                toRemove.Clear();
                parent.Controls.Remove(split);
                split.Dispose();

                foreach (Control c in toKeep)
                    parent.Controls.Add(c);
                parent.ResumeLayout();
                parent.Invalidate();
            }
        }

        //-------------------------------------------------------------------
        public ControlContainerInterface Split(DockStyle _dock)
        {
            if (_dock == DockStyle.None || _dock == DockStyle.Fill) return null;
            if (Parent == null) return null;

            ControlContainerSplitter split = new ControlContainerSplitter();
            Orientation orientation = Orientation.Horizontal;
            int distance = ClientRectangle.Height / 2;
            if ((_dock == DockStyle.Left) || (_dock == DockStyle.Right))
            {
                orientation = Orientation.Vertical;
                distance = ClientRectangle.Width / 2;
            }

            ShowHeaderWhenOnlyOne = true;
            ControlContainerTabs newTabs = new ControlContainerTabs();

            split.Orientation = orientation;
            split.Dock = DockStyle.Fill;
            split.Location = new Point(0, 0);

            Control parent = Parent;
            parent.Controls.Remove(this);
            if ((_dock == DockStyle.Left) || (_dock == DockStyle.Top))
            {
                split.Panel1.Controls.Add(newTabs);
                split.Panel2.Controls.Add(this);
            }
            else
            {
                split.Panel1.Controls.Add(this);
                split.Panel2.Controls.Add(newTabs);
            }
            parent.Controls.Add(split);

            split.SplitterDistance = distance;

            return newTabs;
        }

        //-------------------------------------------------------------------
        public void GetAll(List<GuiControlInterface> _list)
        {
            _list.AddRange(Children);
        }

        //-------------------------------------------------------------------
        public void DetachAll()
        {
            foreach (GuiControlInterface tc in Children)
                tc.OwnerContainer = null;
            Children.Clear();
            Current = null;
        }

        //-------------------------------------------------------------------
        public event EventHandler OnCurrentTabChanging;
        public event EventHandler OnCurrentTabChanged;

        //-------------------------------------------------------------------
        GuiControlInterface _Current = null;
        public GuiControlInterface Current
        {
            get { return _Current; }
            set
            {
                if (_Current == value) return;

                OnCurrentTabChanging?.Invoke(this, new EventArgs());

                if (_Current != null)
                {
                    Controls.Remove(_Current.Control);
                    _Current.Control.Visible = false;
                }
                _Current = value;
                if (_Current != null)
                {
                    Controls.Add(_Current.Control);
                    _Current.Control.Visible = true;
                    UpdateCurrentSize();
                }

                OnCurrentTabChanged?.Invoke(this, new EventArgs());
                Invalidate();
            }
        }

        //-------------------------------------------------------------------
        public void UpdateCurrentSize()
        {
            if (_Current == null) return;
            if (_Current.Control == null) return;
            int y = 0;
            if (Children.Count > 1 || _ShowHeaderWhenOnlyOne)
                y = Theme.Current.TabHeader_Height;
            _Current.Control.Left = 0;
            _Current.Control.Width = Width;
            _Current.Control.Top = y;
            _Current.Control.Height = Height - y;
        }

        //-------------------------------------------------------------------
        bool _ShowHeaderWhenOnlyOne = true;
        public bool ShowHeaderWhenOnlyOne
        {
            get { return _ShowHeaderWhenOnlyOne; }
            set
            {
                if (_ShowHeaderWhenOnlyOne == value) return;
                _ShowHeaderWhenOnlyOne = value;
                UpdateCurrentSize();
                Invalidate();
            }
        }

        //-------------------------------------------------------------------
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateCurrentSize();
            Invalidate();
        }

        //-------------------------------------------------------------------
        Point capturePoint;
        GuiControlInterface _MovedTaxonControl = null;

        //-------------------------------------------------------------------
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (Children.Count == 0 || _ChildrenRect == null || _ChildrenRect.Count != Children.Count)
                return;

            if (!_HeaderVisible || e.Location.Y > Theme.Current.TabHeader_Height) 
            {
                base.OnMouseDown(e);
                return;
            }

            if (_PrevButtonVisible && _PrevButtonRect.Contains(e.Location))
            {
                int indexCurrent = Children.IndexOf(Current);
                if (indexCurrent >= 1)
                    Current = Children[indexCurrent - 1];
                return;
            }

            if (_NextButtonVisible && _NextButtonRect.Contains(e.Location))
            {
                int indexCurrent = Children.IndexOf(Current);
                if (indexCurrent < Children.Count - 1)
                    Current = Children[indexCurrent + 1];
                return;
            }

            for (int i = 0; i < Children.Count; i++)
            {
                if (_ChildrenRect[i].Contains(e.Location))
                {
                    Current = Children[i];
                    if (Current.CanBeMoved)
                        _MovedTaxonControl = Current;
                    Capture = true;
                    capturePoint = e.Location;
                    return;
                }
            }
        }

        //-------------------------------------------------------------------
        #region a teeny weeny tiny bit of API functions used
        private const int WM_NCLBUTTONDBLCLK = 0x00A3;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MOVE = 0xF010;

        // NOTE: I don't like using API's in .Net... so I try to avoid them if possible.
        // this time there was no way around it.

        // this function is used to be able to send some very specific (uncommon) messages
        // to the floaty forms. It is used particularly to switch between start dragging a docked panel
        // to dragging a floaty form.
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);
        #endregion private members

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (Capture && _MovedTaxonControl != null)
            {
                Rectangle R = _HeaderRect;
                R.Inflate(5, 5);

                if (!R.Contains(e.Location))
                {
                    FormContainer form = FormContainer.GetFormContainer(this);
                    
                    GuiControlInterface tuc = _MovedTaxonControl;
                    Remove(_MovedTaxonControl, false);

                    FormContainer moveForm = new FormContainer(tuc);
                    moveForm.Show();

                    Point global = Cursor.Position;
                    moveForm.Left = global.X - 20;
                    moveForm.Top = global.Y - 8;

                    FormContainer tempForm = moveForm;
                    SendMessage(this.Handle.ToInt32(), WM_LBUTTONUP, 0, 0);
                    SendMessage(tempForm.Handle.ToInt32(), WM_SYSCOMMAND, SC_MOVE | 0x02, 0);

                    CloseIfEmpty();

                    if (form != null)
                        form.UpdateName();
                }
                else
                {
                    for (int i = 0; i < Children.Count; i++)
                    {
                        if (Children[i] == _MovedTaxonControl) 
                            continue;

                        if (_ChildrenRect[i].Left < e.Location.X && _ChildrenRect[i].Right > e.Location.X)
                        {
                            Children.Remove(_MovedTaxonControl);
                            Children.Insert(i, _MovedTaxonControl);
                            Invalidate();
                            break;
                        }
                    }
                }
            }
        }

        //-------------------------------------------------------------------
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _MovedTaxonControl = null;
        }

        //-------------------------------------------------------------------
        List<Rectangle> _ChildrenRect = new List<Rectangle>();

        bool _HeaderVisible = false;
        Rectangle _HeaderRect;

        bool _NextButtonVisible = false;
        Rectangle _NextButtonRect;

        bool _PrevButtonVisible = false;
        Rectangle _PrevButtonRect;

        //-------------------------------------------------------------------
        void PaintTabs(Graphics G)
        {
            // Paint Header
            if (Children.Count == 0) return;
            if (Children.Count == 1 && !ShowHeaderWhenOnlyOne) return;

            _HeaderVisible = true;

            int indexCurrent = Children.IndexOf(Current);
            if (indexCurrent == -1) return;

            int[] wChild = new int[Children.Count];
            int wTotal = 0;
            
            for (int i = 0; i < Children.Count; i++)
            {
                TaxonControlInfo info = FactoryOfTaxonControl.TaxonControlInfoFromType(Children[i].GetType());
                info.CurrentTaxonControl = Children[i];
                
                int w = (int) G.MeasureString(info.DisplayName, Font).Width + 1;
                w += 2 * Theme.Current.TabHeader_Margin;
                w += Theme.Current.TabHeader_IconSize + Theme.Current.TabHeader_GapBetweenIconAndText;

                wChild[ i ] = w;
                wTotal += w;
            }

            int[] X= new int[Children.Count + 1];
            int x = 0;

            if (wTotal < Width)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    X[i] = x;
                    x += wChild[i];
                }
                X[Children.Count] = x;
            }
            else if (Children.Count == 1)
            {
                X[1] = Width;
            }
            else
            {
                int[] newW = new int[Children.Count];
                newW[indexCurrent] = wChild[indexCurrent];
                int wLeft = Width - wChild[indexCurrent];
                int wLeftTab = (Children.Count - 1);
                int wLeftPerTab = wLeft / wLeftTab;

                if (wLeftPerTab < 24)
                {
                    if (indexCurrent > 0)
                    {
                        _PrevButtonVisible = true;
                        _PrevButtonRect = new Rectangle(0, (Theme.Current.TabHeader_Height - 16) / 2, 16, 16);
                        x = 20;
                    }
                    newW[indexCurrent] = Width - x;
                    if (indexCurrent < Children.Count - 1)
                    {
                        _NextButtonVisible = true;
                        _NextButtonRect = new Rectangle(Width - 16, (Theme.Current.TabHeader_Height - 16) / 2, 16, 16);
                        newW[indexCurrent] -= 20;
                    }
                }
                else
                {
                    bool ended = false;
                    while (!ended)
                    {
                        ended = true;
                        for (int i = 0; i < Children.Count; i++)
                        {
                            if (newW[i] == 0 && wChild[i] < wLeftPerTab)
                            {
                                newW[i] = wChild[i];
                                wLeft -= wChild[i];
                                wLeftTab--;
                                wLeftPerTab = wLeft / wLeftTab;
                                ended = false;
                            }
                        }
                    }

                    for (int i = 0; i < Children.Count; i++)
                    {
                        if (newW[i] == 0) newW[i] = wLeftPerTab;
                    }
                }

                for (int i = 0; i < Children.Count; i++)
                {
                    X[i] = x;
                    x += newW[i];
                }
                X[Children.Count] = x;

            }

            _ChildrenRect.Clear();

            for (int i = 0; i < Children.Count; i++)
            {
                Rectangle R = Rectangle.FromLTRB( X[i], 0, X[i+1], Theme.Current.TabHeader_Height);
                _ChildrenRect.Add(R);
                if (R.Width == 0) continue;
                if (indexCurrent == i) continue;

                TaxonControlInfo info = FactoryOfTaxonControl.TaxonControlInfoFromType(Children[i].GetType());
                Draw.IconAndText(G, R, info.DisplayName, Font, info.Icon, Theme.Current.TabHeaderParams);
            }

            if (_PrevButtonVisible)
                ControlPaint.DrawScrollButton(G, _PrevButtonRect, ScrollButton.Left, ButtonState.Normal);
            if (_NextButtonVisible)
                ControlPaint.DrawScrollButton(G, _NextButtonRect, ScrollButton.Right, ButtonState.Normal);

            // draw at last selected tab
            {
                TaxonControlInfo info = FactoryOfTaxonControl.TaxonControlInfoFromType(Children[indexCurrent].GetType());
                Rectangle R = _ChildrenRect[indexCurrent];
                G.FillRectangle(Theme.Current.TabHeader_SelectedBackground, R);
                if (R.Right < ClientRectangle.Right)
                    G.DrawLine(Theme.Current.TabHeader_DarkShadow, R.Right, R.Top, R.Right, R.Bottom - 2);
                if (R.Left > 0)
                    G.DrawLine(Theme.Current.TabHeader_LightShadow, R.Left, R.Bottom, R.Left, R.Top);
                G.DrawLine(Theme.Current.TabHeader_LightShadow, R.Left, R.Top, R.Right, R.Top);
                Draw.IconAndText(G, R, info.DisplayName, Font, info.Icon, Theme.Current.SelectedTabHeaderParams);
            }
        }

        //-------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            _HeaderRect = ClientRectangle;
            _HeaderRect.Height = Theme.Current.TabHeader_Height;

            Rectangle R = _HeaderRect;
            R.Height -= 1;
            e.Graphics.FillRectangle(Theme.Current.TabHeader_Background, R);
            e.Graphics.DrawLine(Theme.Current.TabHeader_Shadow, R.Left, R.Top, R.Right, R.Top);
            e.Graphics.DrawLine(Theme.Current.TabHeader_LightShadow, R.Left, R.Bottom, R.Right, R.Bottom);

            _PrevButtonVisible = _NextButtonVisible = _HeaderVisible = false;
            base.OnPaint(e);
            PaintTabs(e.Graphics);
        }
    }
}
