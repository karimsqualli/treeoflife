using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace VinceToolbox.UserControl.Graph
{
    public class SimpleTree : System.Windows.Forms.UserControl
    {
        //-------------------------------------------------------------------------------------------------------------
        [DefaultValue(false)]
        public bool CanMoveNode { get; set; }
        [DefaultValue(false)]
        public bool CanEdit { get; set; }

        private float m_ratioMin = 0.1f;
        public float RatioMin { get { return m_ratioMin; } set { m_ratioMin = value; } }
        private float m_ratioMax = 10.0f;
        public float RatioMax { get { return m_ratioMax; } set { m_ratioMax = value; } }

        private float m_initRatioMin = 0.1f;
        public float InitRatioMin { get { return m_initRatioMin; } set { m_initRatioMin = value; } }
        private float m_initRatioMax = 2.0f;
        public float InitRatioMax { get { return m_initRatioMax; } set { m_initRatioMax = value; } }


        public class SelectedNodeChangedEventArgs : EventArgs
        {
            [DefaultValue(null)]
            public TNode Node { get; private set; }

            public SelectedNodeChangedEventArgs(TNode _node) { Node = _node; }
        }

        public event EventHandler<SelectedNodeChangedEventArgs> OnSelectedNodeChanged;

        //-----------------------------------------------------------------------------------------
        // currently edited data
        //
        private TreeData m_tree = null;
        public TreeData Tree { get { return m_tree; } }
        private void setEditedData( TreeData _tree, TNode _node )
        {
            m_tree = _tree;
            GraphTreeNode<TNode> graphNode = (_node != null) ? m_tree.GetGraphTreeNode(_node.Id) : m_tree.Root;
            m_tree.RootSelected = graphNode;
        }

        private void clearEditedData()
        {
            m_tree = null;
        }

        private bool validEditedData()
        {
            return m_tree != null && m_tree.RootSelected != null;
        }

        private GraphTreeNode<TNode> m_selectedNode = null;
        private GraphTreeNode<TNode> SelectedNode { get { return m_selectedNode; } set { m_selectedNode = value; } }

        private GraphTreeNode<TNode> m_overviewNode = null;
        private GraphTreeNode<TNode> OverviewNode { get { return m_overviewNode; } set { m_overviewNode = value; } }

        public PointF m_viewPos = new PointF(0.0f, 0.0f);
        public float m_viewRatio = 1.0f;

        

        //-----------------------------------------------------------------------------------------
        // Constructor
        public SimpleTree()
        {
            this.DoubleClick += new System.EventHandler(this.PanelGraphic_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PanelGraphic_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelGraphic_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelGraphic_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelGraphic_MouseUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.PanelGraphic_MouseWheel);

            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        //-----------------------------------------------------------------------------------------
        // Clear
        public void Clear()
        {
            clearEditedData();
            Refresh();
        }

        //=========================================================================================
        // drawing, picking functions
        //=========================================================================================

        //-----------------------------------------------------------------------------------------
        // paint
        protected override void OnPaint(PaintEventArgs paintEvnt)
        {
            if (!validEditedData()) return;

            Graphics g = paintEvnt.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.ScaleTransform(m_viewRatio, m_viewRatio);
            g.TranslateTransform(m_viewPos.X / m_viewRatio, m_viewPos.Y / m_viewRatio);

            Tree.RootSelected.DrawTree(g);

            g.ResetTransform();

            moveNodeMode_draw(g);
        }

        //-----------------------------------------------------------------------------------------
        // compute tree placement
        public void ArrangeTree(TreeData _tree, TNode node, bool restoreTransformation)
        {
            setEditedData(_tree, node);
            ArrangeTree(restoreTransformation);
        }
        
        private void ArrangeTree(bool restoreTransformation)
        {
            if (!validEditedData()) return;

            using (Graphics gr = CreateGraphics())
            {
                // Arrange the tree once to see how big it is.
                float xmin = 0, ymin = 0;
                Tree.RootSelected.Arrange(gr, ref xmin, ref ymin);
                if (restoreTransformation)
                {
                    float ratioHeight = (ClientSize.Height - 8) / Tree.RootSelected.m_sizeWithChildren.Height;
                    float ratioWidth = (ClientSize.Width - 8) / Tree.RootSelected.m_sizeWithChildren.Width;
                    float ratio = Math.Min(ratioHeight, ratioWidth);

                    ratio = Math.Max( InitRatioMin, Math.Min(InitRatioMax, ratio));

                    m_viewRatio = ratio;
                    m_viewPos.X = (this.ClientSize.Width - Tree.RootSelected.m_sizeWithChildren.Width * ratio) / 2f;
                    m_viewPos.Y = (this.ClientSize.Height - Tree.RootSelected.m_sizeWithChildren.Height * ratio) / 2f;
                }
            }
            Refresh();
        }

        //-----------------------------------------------------------------------------------------
        //! move point from window to graph coordinates
        //! \param _pointWin windows position relative to control
        //! \returns position inside graph
        //
        private PointF posWinToGraph( PointF _pointWin )
        {
            PointF pointGraph = new PointF();
            pointGraph.X = (_pointWin.X - m_viewPos.X) / m_viewRatio;
            pointGraph.Y = (_pointWin.Y - m_viewPos.Y) / m_viewRatio;
            return pointGraph;
        }

        //-----------------------------------------------------------------------------------------
        //! Move point from graph to window coordinates
        //! \param _pointGraph position relative to graph
        //! \returns windows position
        //
        private PointF posGraphToWin(PointF _pointGraph)
        {
            PointF pointWin = new PointF();
            pointWin.X = (_pointGraph.X * m_viewRatio) + m_viewPos.X;
            pointWin.Y = (_pointGraph.Y * m_viewRatio) + m_viewPos.Y;
            return pointWin;
        }

        //-----------------------------------------------------------------------------------------
        // Set SelectedNode to the node under the mouse.
        private GraphTreeNode<TNode> pickGraphNode(PointF _point )
        {
            if (!validEditedData()) return null;
            _point = posWinToGraph(_point);
            return Tree.RootSelected.pickNode(_point);
        }

        //=========================================================================================
        // view change (pane / zoom)
        //=========================================================================================

        [DllImport("user32.dll")]
        static extern short GetKeyState(System.Windows.Forms.Keys vKey);
        //-----------------------------------------------------------------------------------------
        public bool viewKeyPressed()
        {
            return (GetKeyState(Keys.Space) & 0x80) != 0;
        }

        //-----------------------------------------------------------------------------------------
        bool m_viewPaneMode = false;
        private bool ViewPaneMode { get { return m_viewPaneMode; } set { m_viewPaneMode = value; } }

        Point m_viewPaneMode_startMousePos;
        PointF m_viewPaneMode_startGraphPos; 
        private bool viewPaneMode_enter( Point _location )
        {
            if (!viewKeyPressed()) return false;

            m_viewPaneMode_startMousePos = _location;
            m_viewPaneMode_startGraphPos = new PointF(m_viewPos.X, m_viewPos.Y);

            ViewPaneMode = true;
            return true;
        }
        private void viewPaneMode_exit()
        {
            ViewPaneMode = false;
        }

        //-----------------------------------------------------------------------------------------
        private bool view_pane( Point _location )
        {
            if (!ViewPaneMode) return false;
            Cursor.Current = Cursors.SizeAll;
            m_viewPos.X = m_viewPaneMode_startGraphPos.X + _location.X - m_viewPaneMode_startMousePos.X;
            m_viewPos.Y = m_viewPaneMode_startGraphPos.Y + _location.Y - m_viewPaneMode_startMousePos.Y;
            Refresh();
            return true;
        }

        //-----------------------------------------------------------------------------------------
        private bool view_centerOnSelection()
        {
            if (SelectedNode == null)
                view_zoomFitAll();
            else
            {
                PointF posCenter = posGraphToWin(SelectedNode.getCenter());
                float x = (float) (ClientRectangle.X + ClientRectangle.Width / 2);
                float y = (float) (ClientRectangle.Y + ClientRectangle.Height / 2);
                m_viewPos.X += x - posCenter.X;
                m_viewPos.Y += y - posCenter.Y;   
                Refresh();
            }
            return true;
        }

        //-----------------------------------------------------------------------------------------
        private bool view_zoomFitAll()
        {
            ArrangeTree(true);
            return true;
        }

        //-----------------------------------------------------------------------------------------
        private bool view_zoom( Point _location, int _delta )
        {
            if (!viewKeyPressed()) return false;

            PointF posWinBefore = _location;
            PointF posGraph = posWinToGraph(posWinBefore);
            m_viewRatio *= (_delta > 0) ? 1.05f : 0.95f;
            m_viewRatio = Math.Max(m_ratioMin, Math.Min(m_ratioMax, m_viewRatio));

            PointF posWinAfter = posGraphToWin(posGraph);
            m_viewPos.X -= posWinAfter.X - posWinBefore.X;
            m_viewPos.Y -= posWinAfter.Y - posWinBefore.Y;

            Refresh();
            return true;
        }

        //=========================================================================================
        // move node
        //=========================================================================================

        //-----------------------------------------------------------------------------------------
        bool m_moveNodeMode = false;
        private bool MoveNodeMode { get { return m_moveNodeMode; } set { m_moveNodeMode = value; } }
        Point m_moveNode_startPos;
        Point m_moveNode_currentPos;
        bool m_moveNode_started = false;
        bool m_moveNode_enabled = false;

        //-----------------------------------------------------------------------------------------
        private bool moveNodeMode_enter(Point _location)
        {
            if (!CanMoveNode) return false;

            if (SelectedNode == null || SelectedNode.ParentNode == Tree.Root)
                return false;

            MoveNodeMode = true;
            m_moveNode_startPos = _location;
            m_moveNode_started = false;
            m_moveNode_enabled = false;
            return true;
        }

        //-----------------------------------------------------------------------------------------
        private void moveNodeMode_exit(bool _cancel)
        {
            if (!MoveNodeMode) return;
            MoveNodeMode = false;
            if (_cancel || !m_moveNode_enabled) return;

            //TODO SendMoveLinkNode(SelectedNode.Data.Id, OverviewNode.Data.Id);
            Refresh();
        }

        //-----------------------------------------------------------------------------------------
        private void moveNodeMode_update(Point _location)
        {
            if (!MoveNodeMode) return;

            m_moveNode_currentPos = _location;

            if (!m_moveNode_started)
            {
                int delta = Math.Abs(_location.X - m_moveNode_startPos.X);
                delta += Math.Abs(_location.Y - m_moveNode_startPos.Y);
                if (delta <= 5) return;
                m_moveNode_started = true;
            }

            if (OverviewNode == null)
                m_moveNode_enabled = false;
            else if (OverviewNode == SelectedNode)
                m_moveNode_enabled = false;
            else if (SelectedNode.ParentNode == OverviewNode)
                m_moveNode_enabled = false;
            else if (SelectedNode.ContaintRecursive(OverviewNode))
                m_moveNode_enabled = false;
            else if (OverviewNode.Children.Count >= OverviewNode.Data.rightCapacity)
                m_moveNode_enabled = false;
            else
                m_moveNode_enabled = true;

            Cursor.Current = m_moveNode_enabled ? Cursors.Hand : Cursors.No;
            
            Refresh();            

        }

        //-----------------------------------------------------------------------------------------
        private void moveNodeMode_draw( Graphics _graphics )
        {
            if (!MoveNodeMode || !m_moveNode_started) return;

            float[] dashValues = { 2, 2 };
            Pen linePen = new Pen(m_moveNode_enabled ? Color.DarkGreen : Color.Firebrick, 2);
            linePen.DashPattern = dashValues;
            _graphics.DrawLine(linePen, m_moveNode_startPos, m_moveNode_currentPos);
        }

        //=========================================================================================
        // mouse/keyboard events
        //=========================================================================================

        //-----------------------------------------------------------------------------------------
        private void selectNode( GraphTreeNode<TNode> _node )
        {
            if (_node == SelectedNode) return;

            m_tree.UnselectedAllGraphTreeNode();
            SelectedNode = _node;
            if ( SelectedNode != null )
                SelectedNode.Data.Selected = true;

            if (OnSelectedNodeChanged != null)
            {
                SelectedNodeChangedEventArgs args = new SelectedNodeChangedEventArgs(_node == null ? null : _node.Data);
                OnSelectedNodeChanged(this, args);
            }

        }
        
        //=========================================================================================
        // mouse/keyboard events
        //=========================================================================================

        //-----------------------------------------------------------------------------------------
        private void PanelGraphic_MouseDown(object sender, MouseEventArgs e)
        {
            if (!validEditedData()) return;
            
            if (ViewPaneMode) return;
            if ((e.Button == MouseButtons.Left) && viewPaneMode_enter(e.Location))
                return;

            // Find the node under the mouse.
            selectNode(pickGraphNode(e.Location));
            
            if (MoveNodeMode)
            {
                if (e.Button == MouseButtons.Right)
                    moveNodeMode_exit(true);
                return;
            }
            else if ((e.Button == MouseButtons.Left) && moveNodeMode_enter(e.Location))
                return;

            //TODO
//             if (e.Button == MouseButtons.Right)
//             {
//                 // If there is a node under the mouse, display the context menu.
//                 if (SelectedNode != null)
//                     contextMenuNode.Show(this, e.Location);
//                 else
//                     contextMenuPanel.Show(this, e.Location);
//             }
        }

        //-----------------------------------------------------------------------------------------
        private void PanelGraphic_MouseMove(object sender, MouseEventArgs e)
        {
            if (!validEditedData()) return;

            Cursor.Current = Cursors.Default;

            if (ViewPaneMode)
            {
                if (e.Button != MouseButtons.Left)
                    viewPaneMode_exit();
                else
                    view_pane(e.Location);
                return;
            }

            // Find the node under the mouse.
            GraphTreeNode<TNode> nodeUnderMouse = pickGraphNode(e.Location);
            if (nodeUnderMouse != OverviewNode)
            {
                m_tree.UnoverviewAllGraphTreeNode();
                OverviewNode = nodeUnderMouse;
                if (OverviewNode != null)
                    OverviewNode.Data.Overview = true;
                Refresh();
            }

            if (MoveNodeMode)
            {
                if (e.Button != MouseButtons.Left)
                    moveNodeMode_exit(true);
                else
                    moveNodeMode_update(e.Location);
                return;
            }
        }

        //-----------------------------------------------------------------------------------------
        private void PanelGraphic_MouseUp(object sender, MouseEventArgs e)
        {
            viewPaneMode_exit();
            moveNodeMode_exit(false);
            OverviewNode = null;
        }

        //-----------------------------------------------------------------------------------------
        void PanelGraphic_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (view_zoom(e.Location, e.Delta))
                return;
        }

        //-----------------------------------------------------------------------------------------
        private void PanelGraphic_DoubleClick(object sender, EventArgs e)
        {
            if (view_centerOnSelection())
                return;
        }

        //-----------------------------------------------------------------------------------------
        private void PanelGraphic_KeyDown(object sender, KeyEventArgs e)
        {
            if (!CanEdit) return;
            if (e.KeyData == Keys.Delete && SelectedNode != null)
            {
                //TODO SendRemoveNode(SelectedNode.Data.Id);
                return;
            }

            if (!CanMoveNode) return;
            else if (e.KeyData == Keys.PageUp && SelectedNode != null)
            {
                //TODO if (SelectedNode.PositionChild > 0)
                //TODO  SendMovePositionChild(SelectedNode.Data.Id, (uint)SelectedNode.PositionChild - 1);
            }
            else if (e.KeyData == Keys.PageDown && SelectedNode != null)
            {
                //TODO if (SelectedNode.PositionChild < SelectedNode.NumberOfBrother - 1)
                //TODO SendMovePositionChild(SelectedNode.Data.Id, (uint)SelectedNode.PositionChild + 1);
            }
        }

        //=========================================================================================
        // menu on panel
        //=========================================================================================

        //-----------------------------------------------------------------------------------------
        private void contextMenuPanel_Opening(object sender, CancelEventArgs e)
        {
            //zoomFitToolStripMenuItem.Enabled = validEditedData();
        }

        //-----------------------------------------------------------------------------------------
        private void specyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SpecifyColorForm form = new SpecifyColorForm(this);
            //form.ShowDialog(this);
        }

        //-----------------------------------------------------------------------------------------
        private void zoomFitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            view_zoomFitAll();
        }

        //=========================================================================================
        // menu on node 
        //

        //-----------------------------------------------------------------------------------------
        private void contextMenuNode_Opening(object sender, CancelEventArgs e)
        {
//             bool enableAddChild = SelectedNode != null && SelectedNode.Children.Count < SelectedNode.Data.Capacity;
//             addChildToolStripMenuItem.Enabled = enableAddChild;
// 
//             moveUpToolStripMenuItem.Enabled = SelectedNode.PositionChild > 0;
//             moveDownToolStripMenuItem.Enabled = SelectedNode.PositionChild < SelectedNode.NumberOfBrother - 1;
        }

        //-----------------------------------------------------------------------------------------
        private void addNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
//             FormAddNode addNodeForm = new FormAddNode(GetClassNameList(), Tree, m_AnimCompControl.radioButtonTransitions.Checked);
//             addNodeForm.IdParent = SelectedNode.Data.Id;            
//             addNodeForm.ShowDialog(this);
// 
//             if(addNodeForm.DialogResult == DialogResult.OK)
//             {
//                 GraphNode node = addNodeForm.GetNewNode();
//                 SendAddNode( node, SelectedNode.Data.Id);
//             }
        }

        //-----------------------------------------------------------------------------------------
        private void removeNodeAndChildrenToolStripMenuItem_Click(object sender, EventArgs e)
        {
//             if (SelectedNode != null)
//                 SendRemoveNode(SelectedNode.Data.Id);
        }

        

        //-----------------------------------------------------------------------------------------
        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
//             if (SelectedNode.PositionChild > 0)
//                 SendMovePositionChild(SelectedNode.Data.Id, (uint)SelectedNode.PositionChild - 1);
        }

        //-----------------------------------------------------------------------------------------
        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
//             if (SelectedNode.PositionChild < SelectedNode.NumberOfBrother - 1)
//                 SendMovePositionChild(SelectedNode.Data.Id, (uint)SelectedNode.PositionChild + 1);
        }

        //-----------------------------------------------------------------------------------------
        //public XmlBlendTree m_copyBlendTree = null;
        //public string m_copyName;

        //-----------------------------------------------------------------------------------------
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
//             m_copyName = SelectedNode.Data.NameId;
//             m_copyBlendTree = null;
//             SendGetNodeData(SelectedNode.Data.Id, true, true, "copy");
        }

        //-----------------------------------------------------------------------------------------
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
//             if (m_copyBlendTree == null) return;
//             SendAddTree(m_copyName, m_copyBlendTree, SelectedNode.Data.Id);
        }

    }
}
