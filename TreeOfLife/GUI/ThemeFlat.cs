using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace TreeOfLife.GUI
{
    public class ThemeFlat : Theme
    {
        static ThemeFlat()
        {
            TabHeader_Init();
        }

        //===============================================================================
        // Menu
        //
        //public override Color Menu_Background { get => Color.LightSlateGray; }
        public override Color Menu_Background { get => Color.FromArgb(92,99,106); }
        public override Color Menu_Forecolor { get => Color.White; }

        public override void Menu_ApplyTheme(MenuStrip _menu)
        {
            _menu.BackColor = Theme.Current.Menu_Background;
            _menu.ForeColor = Theme.Current.Menu_Forecolor;
            _menu.Renderer = new MainMenuRenderer();
        }

        private class MainMenuRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                if (e.Item.Selected)
                {
                    Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
                    e.Graphics.FillRectangle(Brushes.DarkGray, rc);
                }
                else if (e.Item.Pressed)
                {
                    Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
                    e.Graphics.FillRectangle(Brushes.DimGray, rc);
                }
                else
                    base.OnRenderMenuItemBackground(e);
            }
        }


        //===============================================================================
        // Tabs
        //

        //-------------------------------------------------------------------
        public override Draw.IconAndTextParams SelectedTabHeaderParams{ get => _SelectedTabHeaderParams; }
        static Draw.IconAndTextParams _SelectedTabHeaderParams;

        public override Draw.IconAndTextParams TabHeaderParams { get => _TabHeaderParams; }
        static Draw.IconAndTextParams _TabHeaderParams;

        public override Brush TabHeader_SelectedBackground { get => _TabHeader_SelectedBackground; }
        //readonly Brush _TabHeader_SelectedBackground = Brushes.LightSlateGray;
        readonly Brush _TabHeader_SelectedBackground = new SolidBrush(Color.FromArgb(92, 99, 106));

        public override Brush TabHeader_Background { get => _TabHeader_Background; }
        readonly Brush _TabHeader_Background = new SolidBrush(Color.FromArgb(89, 101, 113));

        public override Pen TabHeader_DarkShadow { get => _TabHeader_DarkShadow; }
        readonly Pen _TabHeader_DarkShadow = new Pen(Color.FromArgb(73, 85, 97));

        public override Pen TabHeader_Shadow { get => _TabHeader_Shadow; }
        readonly Pen _TabHeader_Shadow = new Pen(Color.FromArgb(117, 132, 147));

        public override Pen TabHeader_LightShadow { get => _TabHeader_LightShadow; }
        readonly Pen _TabHeader_LightShadow = new Pen(Color.FromArgb(161, 180, 197));

        static void TabHeader_Init()
        {
            _SelectedTabHeaderParams = new Draw.IconAndTextParams() { TextBrush = Brushes.White };

            Color color = Color.FromArgb(144, 155, 167);
            _TabHeaderParams = new Draw.IconAndTextParams()
            {
                TextBrush = new SolidBrush(color),
                ImageAttributes = Resources.BuildImageAttributesWhiteToColor(color)
            };
        }


        //===============================================================================
        // Control
        //
        //public override Color Control_Background { get => Color.LightSlateGray; }
        public override Color Control_Background { get => Color.FromArgb(92, 99, 106); }
        public override Color Control_Forecolor { get => Color.White; }

        //===============================================================================
        // Splitter
        //
        public override bool Splitter_PaintThin(PaintEventArgs _args)
        {
            Rectangle R = _args.ClipRectangle;
            _args.Graphics.DrawLine(TabHeader_DarkShadow, R.Right, R.Top, R.Left, R.Top);
            _args.Graphics.DrawLine(TabHeader_LightShadow, R.Right, R.Bottom - 1, R.Left, R.Bottom - 1);
            return true;
        }

        //===============================================================================
        // Flat button
        //

        static readonly Brush FlatCheckBox_BackgroundChecked = new SolidBrush(Color.FromArgb(80, 91, 103));
        //static readonly Brush FlatCheckBox_BackgroundUnchecked = Brushes.LightSlateGray;
        static readonly Brush FlatCheckBox_BackgroundUnchecked = new SolidBrush(Color.FromArgb(92, 99, 106));

        static readonly Draw.IconAndTextParams FlatCheckBoxParams = new Draw.IconAndTextParams()
        {
            Margin = 4,
            BetweenIconAndText = 4,
            TextBrush = Brushes.White,
        };

        public override void FlatCheckBox_Paint( CheckBox _button, Graphics G, Rectangle R)
        {
            Brush background = _button.Checked ? FlatCheckBox_BackgroundChecked : FlatCheckBox_BackgroundUnchecked;
            G.FillRectangle(background, R);
            if (_button.Image != null)
                Draw.IconAndText(G, R, _button.Text, _button.Font, _button.Image, FlatCheckBoxParams);
            else
                Draw.Text(G, R, _button.Text, _button.Font, FlatCheckBoxParams);
        }
    }
}
