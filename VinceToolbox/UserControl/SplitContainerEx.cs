using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace VinceToolbox.UserControl
{

    public class SplitContainerEx : SplitContainer
    {
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillRectangle(Brushes.Coral, SplitterRectangle);
            int x0 = SplitterRectangle.Left;
            int x1 = SplitterRectangle.Right - 1;
            int y0 = SplitterRectangle.Top;
            int y1 = SplitterRectangle.Bottom - 1;
            e.Graphics.DrawLine(Pens.IndianRed, x0, y1, x1, y1);
            e.Graphics.DrawLine(Pens.IndianRed, x1, y0, x1, y1);
            e.Graphics.DrawLine(Pens.BurlyWood, x0, y0, x1, y0);
            e.Graphics.DrawLine(Pens.BurlyWood, x0, y0, x0, y1);
        }
    }
}