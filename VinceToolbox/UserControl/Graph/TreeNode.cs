using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace VinceToolbox.UserControl.Graph
{
    //=============================================================================================================
    //
    // Tree node
    //
    //=============================================================================================================

    public class GraphTreeNode<T> where T : IDrawable
    {
        // The data.
        public T Data;

        // Child nodes in the tree.
        public List<GraphTreeNode<T>> Children = new List<GraphTreeNode<T>>();

        // parent and siblings data
        public GraphTreeNode<T> ParentNode = null;
        public int PositionChild   = -1;
        public int NumberOfBrother = -1;
        
        // Space to skip horizontally between siblings
        // and vertically between generations.
        private const float Hoffset = 75;
        private const float Voffset = 15;

        // The node's center after arranging.
        public SizeF m_size;
        public PointF m_pos;
        public float m_childOffsetY;
        public SizeF m_sizeWithChildren;
        public PointF m_posWithChildren;

        public PointF getCenter() { return new PointF(m_pos.X + m_size.Width / 2, m_pos.Y + m_size.Height / 2); }
        public RectangleF getBoundsF() { return RectangleF.FromLTRB(m_pos.X, m_pos.Y, m_pos.X + m_size.Width, m_pos.Y + m_size.Height); }
        public Rectangle getBounds() { return Rectangle.FromLTRB((int)m_pos.X, (int)m_pos.Y, (int)(m_pos.X + m_size.Width), (int)(m_pos.Y + m_size.Height)); }
        
        public RectangleF getBoundsFWithChildren() { return RectangleF.FromLTRB(m_posWithChildren.X, m_posWithChildren.Y, m_posWithChildren.X + m_sizeWithChildren.Width, m_posWithChildren.Y + m_sizeWithChildren.Height); }

        // Drawing properties.
        public Font MyFont = null;
        public Pen MyPen = new Pen(Color.Black);
        public Brush FontBrush = Brushes.Black;
        public Brush BgBrush = Brushes.White;

        public Pen penAddingNode = Pens.Gray;

        static public Font s_defaultFont = new Font("Times New Roman", 10);

        //-------------------------------------------------------------------------------------------------------------
        // Constructor.
        //
        public GraphTreeNode(T _data) : this( _data, s_defaultFont) {}
        public GraphTreeNode(T _data, Font fg_font)
        {
            Data = _data;
            MyFont = fg_font;
        }

        //-------------------------------------------------------------------------------------------------------------
        // Add a TreeNode to out Children list.
        //
        public void AddChild(GraphTreeNode<T> child)
        {
            child.PositionChild = Children.Count; // the first one is on position: 0
            Children.Add(child);
            child.ParentNode = this;
            
            int count = Children.Count;
            foreach (GraphTreeNode<T> node in Children)
            {
                node.NumberOfBrother = count;
            }            
        }

        //-------------------------------------------------------------------------------------------------------------
        // remove a node from child list
        //
        public void RemoveChild(GraphTreeNode<T> child)
        {
            Children.Remove(child);
            child.ParentNode = null;

            int index = 0;
            int count = Children.Count;
            foreach (GraphTreeNode<T> node in Children)
            {
                node.NumberOfBrother = count;
                node.PositionChild = index;
                index++;
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        // Delete a target node from this node's subtree. Return true if we delete the node.
        //
        public bool DeleteNode(GraphTreeNode<T> target)
        {
            // See if the target is in our subtree.
            foreach (GraphTreeNode<T> child in Children)
            {
                // See if it's the child.
                if (child == target)
                {
                    // Delete this child.
                    Children.Remove(child);
                    return true;
                }

                // See if it's in the child's subtree.
                if (child.DeleteNode(target)) return true;
            }

            // It's not in our subtree.
            return false;
        }

        //-------------------------------------------------------------------------------------------------------------
        // Delete a target node from this node's subtree.
        // Return true if we delete the node.
        //
        public void DeleteAllChildren()
        {
            // See if the target is in our subtree.
            foreach (GraphTreeNode<T> child in Children)
            {
                // See if it's in the child's subtree.
                child.DeleteAllChildren();
            }
            Children.Clear();
        }

        //-------------------------------------------------------------------------------------------------------------
        // Arrange the node and its children in the allowed area.
        // Set xmin to indicate the right edge of our subtree.
        // Set ymin to indicate the bottom edge of our subtree.
        //
        public void computeSize(Graphics gr)
        {
            // See how big this node is.
            m_size = Data.getSize(gr, MyFont);
            m_sizeWithChildren = m_size;
            if (Children.Count == 0)
                return;

            // Recursively arrange our children, allowing room for this node.
            SizeF childrenSize = new SizeF(0, 0);
            foreach (GraphTreeNode<T> child in Children)
            {
                // Arrange this child's subtree.
                child.computeSize(gr);

                if (childrenSize.Height == 0)
                    childrenSize = child.m_size;
                else
                {
                    childrenSize.Height += child.m_size.Height + Voffset;
                    childrenSize.Width = Math.Max(child.m_size.Width, childrenSize.Width);
                }
            }

            m_sizeWithChildren.Width += Hoffset + childrenSize.Width;
            if (m_sizeWithChildren.Height > childrenSize.Height)
                m_childOffsetY = (m_sizeWithChildren.Height - childrenSize.Height) / 2;
            else
                m_sizeWithChildren.Height = childrenSize.Height;
        }

        //-------------------------------------------------------------------------------------------------------------
        public void computePos( PointF _topLeft )
        {
            m_posWithChildren = _topLeft;
            m_pos = _topLeft;
            
            if (Children.Count == 0)
                return;

            m_pos.Y += (m_sizeWithChildren.Height - m_size.Height) / 2;

            PointF childTopLeft = _topLeft;
            childTopLeft.X += m_size.Width + Hoffset;
            childTopLeft.Y += m_childOffsetY;

            foreach (GraphTreeNode<T> child in Children)
            {
                child.computePos(childTopLeft);
                childTopLeft.Y += child.m_sizeWithChildren.Height + Voffset;
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        // Arrange the node and its children in the allowed area.
        // Set xmin to indicate the right edge of our subtree.
        // Set ymin to indicate the bottom edge of our subtree.
        //
        public void Arrange(Graphics gr, ref float xmin, ref float ymin)
        {
            computeSize(gr);
            computePos( new PointF(xmin, ymin));
        }
        
        //-------------------------------------------------------------------------------------------------------------
        // Draw the subtree rooted at this node with the given upper left corner.
        //
        public void DrawTree(Graphics gr, ref float x, float y)
        {
            Arrange(gr, ref x, ref y);
            DrawTree(gr);
        }

        //-------------------------------------------------------------------------------------------------------------
        // Draw the subtree rooted at this node.
        //
        public void DrawTree(Graphics gr)
        {
            DrawSubtreeLinks(gr);
            DrawSubtreeNodes(gr);
        }

        //-------------------------------------------------------------------------------------------------------------
        // Draw the links for the subtree rooted at this node.
        //
        private void DrawSubtreeLinks(Graphics gr)
        {
            MyPen.Color = Color.Black;
            MyPen.Width = 1;

            int capacity = Data.RightLinksAnchorNumber;
            //float offset = IDrawable.LinkerSize.Y * (capacity / 2);

            float anchorWidth = IDrawable.LinkerSize.X;
            float anchorHeight = IDrawable.LinkerSize.Y;

            //SizeF size = Data.getSize(gr, MyFont);
            float x0 = m_pos.X;
            float x1 = m_pos.X + m_size.Width;
            float y0 = m_pos.Y;
            float y1 = m_pos.Y + m_size.Height;
            
            // TODO : left anchor (do not draw link cause drawn from left to right)

            if (Data.leftNumber != 0)
                gr.DrawRectangle(MyPen, x0 - anchorWidth, (y0 + y1 - anchorHeight) / 2, anchorWidth, anchorHeight);

            int index = 0;
            for (; index < Data.rightNumber; index++)
            {
                GraphTreeNode<T> child = Children[index];

                float yAnchor = y0 + Data.getRightAnchorY( m_size.Height, index);
                float xOut = x1 + anchorWidth;
                float yOut = yAnchor + anchorHeight / 2;
                
                SizeF sizeChild = child.Data.getSize(gr, MyFont);
                PointF center = child.getCenter();
                float xIn = center.X - sizeChild.Width / 2 - anchorWidth;
                float yIn = center.Y;

                float deltaX = (xIn - xOut) / 2;
                GraphicsPath path = new GraphicsPath();
                path.AddBezier( xOut, yOut, xOut + deltaX, yOut, xIn - deltaX, yIn, xIn, yIn);
                gr.DrawPath(MyPen, path);
                gr.DrawRectangle(MyPen, x1, yAnchor, anchorWidth, anchorHeight);

                float x = (xOut + xIn) / 2;
                float y = (yOut + yIn) / 2;
                gr.FillEllipse(Brushes.Yellow, x - 3, y - 3, 6, 6);
                gr.DrawEllipse(MyPen, x - 3, y - 3, 6, 6);

                // Recursively make the child draw its subtree nodes.
                child.DrawSubtreeLinks(gr);
            }

            for (; index < capacity; index++)
            {
                PointF linkOut = new PointF(x1, y0 + Data.getRightAnchorY(m_size.Height, index));
                gr.DrawRectangle(penAddingNode, linkOut.X, linkOut.Y, anchorWidth, anchorHeight);
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        // Draw the nodes for the subtree rooted at this node.
        //
        private void DrawSubtreeNodes(Graphics gr)
        {
            MyPen.Color = Color.Black;
            MyPen.Width = 1;

            // Draw this node.
            PointF center = getCenter();
            Data.draw(getBounds(), gr, MyPen, BgBrush, FontBrush, MyFont);

            // Recursively make the child draw its subtree nodes.
            foreach (GraphTreeNode<T> child in Children)
                child.DrawSubtreeNodes(gr);
        }

        //-------------------------------------------------------------------------------------------------------------
        // Return the TreeNode at this point (or null if there isn't one there).
        public GraphTreeNode<T> pickNode(PointF target_pt)
        {
            if (!getBoundsFWithChildren().Contains(target_pt)) return null;
            if (getBoundsF().Contains(target_pt)) return this;

            foreach (GraphTreeNode<T> child in Children)
            {
                GraphTreeNode<T> hit_node = child.pickNode(target_pt);
                if (hit_node != null)
                    return hit_node;
            }
            return null;
        }

        //-------------------------------------------------------------------------------------------------------------
        public bool ContaintRecursive(GraphTreeNode<T> _childFound)
        {
            foreach (GraphTreeNode<T> child in Children)
            {
                if (child == _childFound)
                    return true;

                if (child.ContaintRecursive(_childFound))
                    return true;
            }
            return false;
        }

    }
}
