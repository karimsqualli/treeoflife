using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VinceToolbox.UserControl.Graph
{
    public class SpaceGraphData
    {
        //-----------------------------------------------------------------------------------------
        public SpaceGraphData()
        {
            Console.WriteLine( "SpaceGraphData constructor");
        }

        //-----------------------------------------------------------------------------------------
        public delegate void Callback(SpaceGraphData _data );

        //-----------------------------------------------------------------------------------------
        private Callback m_onRootChanged = null;
        public Callback OnRootChanged { set { m_onRootChanged = value; } }
        private SpaceGraphItem m_root;
        public SpaceGraphItem Root
        {
            get { return m_root; }
            set
            {
                if (m_root == value) return;
                m_root = value;
                m_selectedItems = null;
                m_hoverItem = null;
                if (m_onRootChanged != null) m_onRootChanged(this);
            }
        }
        
        //-----------------------------------------------------------------------------------------
        public bool IsReadyForDrawing { get { return m_root != null && m_root.isDone(); } }

        //-----------------------------------------------------------------------------------------
        private Callback m_onSelectedItemsChanged = null;
        public Callback OnSelectedItemsChanged { set { m_onSelectedItemsChanged = value; } }
        public  List<SpaceGraphItem> m_selectedItems = null;
        public void selectItems( List<SpaceGraphItem> _list )
        {
            if (_list == null && m_selectedItems == null) return;
            if (_list == null)
                m_selectedItems = null;
            else
            {
                if (m_selectedItems == null) m_selectedItems = new List<SpaceGraphItem>();
                m_selectedItems.Clear();
                m_selectedItems.AddRange(_list.ToArray());
            }
            if (m_onSelectedItemsChanged  != null) m_onSelectedItemsChanged(this);
        }
        
        //-----------------------------------------------------------------------------------------
        private Callback m_onHoverItemChanged = null;
        public Callback OnHoverItemChanged { set { m_onHoverItemChanged = value; } }
        private SpaceGraphItem m_hoverItem = null;
        public SpaceGraphItem HoverItem
        {
            get { return m_hoverItem; }
            set
            {
                if (m_hoverItem == value) return;
                m_hoverItem = value;
                if (m_onHoverItemChanged != null) m_onHoverItemChanged(this);
            }
        }
    }
}
