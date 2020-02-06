using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.GUI
{
    public class LabelVerticalLine : Label
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle R = ClientRectangle;
            int x = R.Left + R.Width / 2;
            e.Graphics.DrawLine(Pens.Black, x, R.Top, x, R.Bottom);
        }
    }
}
