using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VinceToolbox.UserControl
{
    //=============================================================================================================
    //
    // Control to display image in an area that implement zoom/translation
    //
    //=============================================================================================================

    public partial class TextureAreaControl : System.Windows.Forms.UserControl
    {
        private TextureArea m_textureArea = new TextureArea();

        //-------------------------------------------------------------------------------------------------------------
        private Image m_image = null;
        public Image textureImage
        {
            get { return m_image; }
            set
            {
                if (m_image != null) m_image.Dispose();
                m_image = value;
                m_textureArea.setTexture(m_image);
                Invalidate();
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        public TextureAreaControl()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.EnableNotifyMessage, true);

            m_textureArea.setBackColor(BackColor);
            m_textureArea.setBilinearFilter(true);
            m_textureArea.setWinRect( ClientRectangle );
        }

        //-------------------------------------------------------------------------------------------------------------
        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            m_textureArea.setWinRect(ClientRectangle);
        }

        //-------------------------------------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            m_textureArea.paint(e.Graphics);
        }

        //-------------------------------------------------------------------------------------------------------------
        private bool m_capture = false;

        //-------------------------------------------------------------------------------------------------------------
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (m_textureArea.mouseDown(e.Location))
            {
                Capture = true;
                m_capture = true;
            }
        }
        
        //-------------------------------------------------------------------------------------------------------------
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (m_capture)
            {
                m_textureArea.mouseMove(e.Location);
                Invalidate();
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (m_capture)
            {
                m_textureArea.mouseUp(e.Location);
                m_capture = false;
                Capture = false;
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (m_textureArea.mouseWheel(e.Location, e.Delta < 0))
                Invalidate();
        }

    }
}
