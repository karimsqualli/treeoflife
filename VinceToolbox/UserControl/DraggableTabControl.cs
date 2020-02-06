using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace VinceToolbox.UserControl
{
    //=============================================================================================================
    //
    // DraggableTabControl 
    //  can drag/drop tabs 
    //
    //=============================================================================================================

    public class DraggableTabControl : TabControl
    {
        //-------------------------------------------------------------------------------------------------------------
        [DefaultValue(false)]
        public bool CanDragLast { get; set; }

        public event EventHandler<EventArgs> OnTabDropped;

        public class BeforeTabDroppedArgs : EventArgs
        {
            [DefaultValue(null)]
            public TabPage TabPage { get; private set; }

            [DefaultValue(false)]
            public bool Cancel { get; set; }

            public BeforeTabDroppedArgs(TabPage _page) { TabPage = _page; }
        }

        public event EventHandler<BeforeTabDroppedArgs> OnBeforeTabDropped;

        //-------------------------------------------------------------------------------------------------------------
        private System.ComponentModel.Container components = null;

        //-------------------------------------------------------------------------------------------------------------
        public DraggableTabControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            AllowDrop = true;
        }

        //-------------------------------------------------------------------------------------------------------------
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                    components.Dispose();
            }
            base.Dispose( disposing );
        }

        //-------------------------------------------------------------------------------------------------------------
        private void InitializeComponent() {}

        //-------------------------------------------------------------------------------------------------------------
        protected override void OnDragOver(System.Windows.Forms.DragEventArgs e)
        {
            base.OnDragOver(e);
        
            Point pt = new Point(e.X, e.Y);
            //We need client coordinates.
            pt = PointToClient(pt);
            
            //Get the tab we are hovering over.
            TabPage hover_tab = GetTabPageByTab(pt);

            //Make sure we are on a tab.
            if(hover_tab != null)
            {
                //Make sure there is a TabPage being dragged.
                if(e.Data.GetDataPresent(typeof(TabPage)))
                {
                    e.Effect = DragDropEffects.Move;
                    TabPage drag_tab = (TabPage)e.Data.GetData(typeof(TabPage));
                    if (drag_tab == GetRealTabPageByTab(pt))
                        clearTabRect();

                    int item_drag_index = FindIndex(drag_tab);
                    int drop_location_index = FindIndex(hover_tab);

                    //Don't do anything if we are hovering over ourself.
                    if (item_drag_index != drop_location_index)
                    {
                        DraggableTabControl dragOwner = this;
                        if (drag_tab.Parent != this)
                            dragOwner = drag_tab.Parent as DraggableTabControl;
                        if (dragOwner != null)
                        {
                            dragOwner.TabPages.Remove(drag_tab);
                            TabPages.Insert(drop_location_index, drag_tab);
                            SelectedTab = drag_tab;
                        }
                    }

                    if (OnTabDropped != null) OnTabDropped(this, e);
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!CanDragLast && TabPages.Count == 1) return;
            
            Point pt = new Point(e.X, e.Y);
            clearTabRect();
            TabPage tp = GetRealTabPageByTab(pt);
            if(tp == null) return;

            if (OnBeforeTabDropped != null)
            {
                BeforeTabDroppedArgs args = new BeforeTabDroppedArgs(tp);
                OnBeforeTabDropped(this, args);
                if (args.Cancel) return;
            }

            if (e.Button == MouseButtons.Left)
                DoDragDrop(tp, DragDropEffects.All);
            if (e.Button == MouseButtons.Right)
                SelectedTab = tp;
        }

        //-------------------------------------------------------------------------------------------------------------
        List<Rectangle> tabRect = null;

        private void copyTabRect()
        {
            tabRect = new List<Rectangle>();
            for (int i = 0; i < TabPages.Count; i++)
                tabRect.Add(GetTabRect(i));
        }

        private void clearTabRect() { tabRect = null; }

        private TabPage GetTabPageByTab(Point pt)
        {
            if (tabRect == null || tabRect.Count != TabPages.Count) copyTabRect();
            for(int i = 0; i < TabPages.Count; i++)
            {
                if(tabRect[i].Contains(pt))
                    return TabPages[i];
            }
            return null;
        }

        private TabPage GetRealTabPageByTab(Point pt)
        {
            for (int i = 0; i < TabPages.Count; i++)
            {
                if (GetTabRect(i).Contains(pt))
                    return TabPages[i];
            }
            return null;
        }

        //-------------------------------------------------------------------------------------------------------------
        private int FindIndex(TabPage page)
        {
            for(int i = 0; i < TabPages.Count; i++)
                if(TabPages[i] == page)
                    return i;
            return -1;
        }
    }
}
