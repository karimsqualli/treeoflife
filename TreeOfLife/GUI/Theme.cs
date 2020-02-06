using System.Drawing;
using System.Windows.Forms;

namespace TreeOfLife.GUI
{
    public class Theme
    {
        //===============================================================================
        // Menu
        //
        public virtual Color Menu_Background { get => Color.LightCyan; }
        public virtual Color Menu_Forecolor { get => Color.Black; }
        public virtual void Menu_ApplyTheme(MenuStrip _menu) { }

        //===============================================================================
        // Tabs
        //
       
        public virtual int TabHeader_IconSize { get => 12; }
        public virtual int TabHeader_Height { get => 28; }
        public virtual int TabHeader_Margin { get => 10; }
        public virtual int TabHeader_GapBetweenIconAndText { get => 2; }

        public virtual StringFormat TabHeader_StringFormat { get => _TabHeaderStringFormat; }
        static readonly StringFormat _TabHeaderStringFormat = new StringFormat()
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Near,
            FormatFlags = StringFormatFlags.NoWrap,
            Trimming = StringTrimming.EllipsisCharacter
        };

        public virtual Draw.IconAndTextParams SelectedTabHeaderParams { get; }
        public virtual Draw.IconAndTextParams TabHeaderParams { get; }

        public virtual Brush TabHeader_SelectedBackground { get; }
        public virtual Brush TabHeader_Background { get; }
        public virtual Pen TabHeader_DarkShadow { get; }
        public virtual Pen TabHeader_Shadow { get; }
        public virtual Pen TabHeader_LightShadow { get; }


        //===============================================================================
        // Control
        //
        public virtual Color Control_Background { get => Color.FromArgb(114, 127, 149); }
        public virtual Color Control_Forecolor { get => Color.Black; }

        //===============================================================================
        // Splitter
        //
        public virtual bool Splitter_PaintThin(PaintEventArgs _args) { return false; }

        //===============================================================================
        // Flat button
        //
        public virtual void FlatCheckBox_Paint(CheckBox _button, Graphics G, Rectangle R) { }

        //===============================================================================
        static Theme()
        {
                Current = new ThemeFlat();
        }

        //===============================================================================
        // Current theme
        //
        public static Theme Current = null;
    }
}
