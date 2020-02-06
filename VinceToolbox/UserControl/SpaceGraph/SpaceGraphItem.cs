using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VinceToolbox.UserControl.Graph
{
    //
    // Item. Interface which must be supported by the tree items.
    // If you prefer to use the getHead()/getNext() pattern rather
    // than using an array for the children, you will have to
    // rewrite CTreemap.
    // 
    public class SpaceGraphItem
    {
        public virtual bool TmiIsLeaf() { return m_children == null || m_children.Count == 0; }

        private Rectangle m_rectangle;
        public virtual Rectangle TmiGetRectangle() { return m_rectangle; }
        public virtual void TmiSetRectangle(Rectangle rectangle) { m_rectangle = rectangle; }

        private Color m_Color = Color.LightGreen;
        public virtual Color TmiGetGraphColor() { return m_Color; }
        public virtual void TmiSetGraphColor(Color _color ) { m_Color = _color; } 

        SpaceGraphItem m_parent = null;
        List<SpaceGraphItem> m_children = null;
        public virtual int TmiGetChildrenCount() { return m_children == null ? 0 : m_children.Count; }
        public virtual SpaceGraphItem TmiGetChild(int c) 
        {
            if (m_children == null) return null;
            if (c < 0 || c >= m_children.Count) return null;
            return m_children[c];
        }
        public virtual void AddChild(SpaceGraphItem _item)
        {
            _item.m_parent = this;
            if (m_children == null) m_children = new List<SpaceGraphItem>();
            m_children.Add(_item);
        }

        UInt32 m_size = 0;
        public virtual UInt32 TmiGetSize() { return m_size; }
        public virtual void TmiSetSize(UInt32 _size) 
        {
            if (m_parent != null) m_parent.TmiSetSize( m_parent.TmiGetSize() - m_size);
            m_size = _size;
            if (m_parent != null) m_parent.TmiSetSize(m_parent.TmiGetSize() + m_size);
        }

        
        public virtual bool isDone() { return true; }

        

        
    }
}
