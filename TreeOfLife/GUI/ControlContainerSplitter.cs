using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.GUI
{
    public class ControlContainerSplitter : SplitContainer, ControlContainerInterface
    {
        //-------------------------------------------------------------------
        public ControlContainerSplitter()
        {
            SplitterWidth = 4;
            //BackColor = Theme.Current.Control_Background;
        }
        //-------------------------------------------------------------------
        public Control GetControl() { return this; }

        //-------------------------------------------------------------------
        public void SetFocus(GuiControlInterface _control) { }

        //-------------------------------------------------------------------
        public void Add(GuiControlInterface _control, bool _visible = true) 
        {
            throw new NotImplementedException();
        }

        //-------------------------------------------------------------------
        public void Remove(GuiControlInterface _control, bool _closeIfEmpty)
        {
            if (Panel1.Controls.Count == 1 && Panel1.Controls[0] is ControlContainerInterface)
                (Panel1.Controls[0] as ControlContainerInterface).Remove(_control, _closeIfEmpty);
            if (Panel2.Controls.Count == 1 && Panel2.Controls[0] is ControlContainerInterface)
                (Panel2.Controls[0] as ControlContainerInterface).Remove(_control, _closeIfEmpty);
        }

        //-------------------------------------------------------------------
        public ControlContainerInterface Split(DockStyle _dock)
        {
            throw new NotImplementedException();
        }

        //-------------------------------------------------------------------
        public void GetAll( List <GuiControlInterface> _allControls )
        {
            if (Panel1.Controls.Count == 1 && Panel1.Controls[0] is ControlContainerInterface cci1)
                cci1.GetAll(_allControls);
            if (Panel2.Controls.Count == 1 && Panel2.Controls[0] is ControlContainerInterface cci2)
                cci2.GetAll(_allControls);
        }

        //-------------------------------------------------------------------
        public void DetachAll()
        {
            if (Panel1.Controls.Count == 1 && Panel1.Controls[0] is ControlContainerInterface cci1)
                cci1.DetachAll();
            if (Panel2.Controls.Count == 1 && Panel2.Controls[0] is ControlContainerInterface cci2)
                cci2.DetachAll();
        }

        //-------------------------------------------------------------------
        /*protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Rectangle r = this.ClientRectangle;
            base.OnPaint(e);
            g.FillRectangle(new SolidBrush(this.BackColor), r);

            g.FillRectangle(Brushes.Black, r);
        }*/
    }

}
