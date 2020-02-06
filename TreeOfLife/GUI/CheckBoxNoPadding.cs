using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TreeOfLife.GUI
{
    public class CheckBoxNoPadding : CheckBox
    {

        private string _textCurrent;

        private string _Text;
        [Category("Appearance")]
        public override string Text
        {
            get { return _Text; }
            set
            {
                if (value != _Text)
                {
                    _Text = value;
                    Invalidate();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _textCurrent = Text;
            _Text = string.Empty;
            base.OnPaint(e);
            _Text = _textCurrent;

            using (var brush = new SolidBrush(ForeColor))
            {
                using (var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    //Rectangle R = Rectangle.Inflate(ClientRectangle, -2, 0);
                    //R.Y -= 1;
                    Rectangle R = ClientRectangle;
                    e.Graphics.DrawString(Text, Font, brush, R, stringFormat);
                }
            }

        }

    }
}