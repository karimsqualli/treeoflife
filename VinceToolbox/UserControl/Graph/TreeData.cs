using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace VinceToolbox.UserControl.Graph
{
    //=============================================================================================================
    //
    // Tree data : all data for tree management (list of node / links / compute / selection .... )
    //
    //=============================================================================================================

    public class TreeData
    {
        private TNode m_rootNode = null;
        public TNode RootNode { get { return m_rootNode; } private set { m_rootNode = value; } }

        private GraphTreeNode<TNode> m_root = null;
        public GraphTreeNode<TNode> Root { get { return m_root; } private set { m_root = value; } }
        public GraphTreeNode<TNode> RootSelected { get; set; }

        //=============================================================================================================
        // Constructor
        //
        
        //-------------------------------------------------------------------------------------------------------------
        public TreeData(TNode _root)
        {
            //Root = new GraphTreeNode < TNode >(_root);
            AddNode(_root);
            RootNode = m_listOfNodes[_root.Id];
        }

        //-------------------------------------------------------------------------------------------------------------
        public bool IsReady()
        {
            return m_listOfLinks.Count != 0 && m_listOfNodes.Count != 0;
        }

        //-------------------------------------------------------------------------------------------------------------
        public void Clear()
        {
            ClearListOfNode();
            ClearListOfLinks();
        }

        //=============================================================================================================
        // list of nodes
        //
        private Dictionary<int, TNode> m_listOfNodes = new Dictionary<int, TNode>();

        //-------------------------------------------------------------------------------------------------------------
        public void AddNode(TNode _node)
        {
            Debug.Assert(_node.Id != TNode.InvalideId, "Node id isn't valid");
            if (m_listOfNodes.ContainsKey(_node.Id))
                return;
            m_listOfNodes[_node.Id] = _node;
        }

        //-------------------------------------------------------------------------------------------------------------
        public TNode GetNode(int id)
        {
            if (m_listOfNodes.ContainsKey(id))
                return m_listOfNodes[id];
            return null;
        }

        //-------------------------------------------------------------------------------------------------------------
        public void ClearListOfNode()
        {
            m_listOfNodes.Clear();
        }

        //-------------------------------------------------------------------------------------------------------------
        public int NodeCount()
        {
            return m_listOfNodes.Count;
        }

        //-------------------------------------------------------------------------------------------------------------
        public bool IsExistNodeName(string _nodeName, int _id)
        {
            string nodeNameTrim = _nodeName.Trim();
            if (nodeNameTrim.Length == 0)
                return false;

            foreach (TNode node in m_listOfNodes.Values)
            {
                GraphTreeNode<TNode> gNode = new GraphTreeNode<TNode>(node);
                if (node.Name.Equals(nodeNameTrim, StringComparison.OrdinalIgnoreCase) && gNode.Data.Id != _id)
                    return true;
            }
            return false;
        }

        //=============================================================================================================
        // list of links key is node id, value is node's parent id
        //
        private Dictionary<int, int> m_listOfLinks = new Dictionary<int, int>();

        //-------------------------------------------------------------------------------------------------------------
        public void ClearListOfLinks()
        {
            m_listOfLinks.Clear();
        }

        //-------------------------------------------------------------------------------------------------------------
        public void AddLink(int _id, int _parent)
        {
            m_listOfLinks[_id] = _parent;
        }

        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<int, int>.KeyCollection GetLinksKeys()
        {
            return m_listOfLinks.Keys;
        }

        //-------------------------------------------------------------------------------------------------------------
        public int GetParentLink(int id)
        {
            return m_listOfLinks[id];
        }

        //-------------------------------------------------------------------------------------------------------------
        public List<int> GetIdRoots()
        {
            List<int> roots = new List<int>();
            foreach (int id in m_listOfLinks.Keys)
            {
                int idParent = m_listOfLinks[id];
                if (idParent == TNode.InvalideId)
                    roots.Add(id);
            }
            return roots;
        }

        //=============================================================================================================
        // compute
        //

        //-------------------------------------------------------------------------------------------------------------
        public void computeTree()
        {
            Dictionary<int, GraphTreeNode<TNode>> list = new Dictionary<int, GraphTreeNode<TNode>>();

            // clear root

            foreach (TNode node in m_listOfNodes.Values)
            {
                GraphTreeNode<TNode> gNode = new GraphTreeNode<TNode>(node);
                list[node.Id] = gNode;
            }

            if (list.ContainsKey(RootNode.Id))
                Root = list[RootNode.Id];
            else
                Root = null;

            // assign children to their parents
            Dictionary<int, int>.KeyCollection keys = GetLinksKeys();
            foreach (int id in keys)
            {
                GraphTreeNode<TNode> node = list[id];
                int idParent = GetParentLink(id);

                GraphTreeNode<TNode> nodeParent;
                nodeParent = (idParent == TNode.InvalideId) ? Root : list[idParent];
                if (nodeParent != null)
                    nodeParent.AddChild(node);
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        public GraphTreeNode<TNode> GetGraphTreeNode(int id)
        {
            return GetGraphTreeNode(Root, id);
        }

        //-------------------------------------------------------------------------------------------------------------
        public GraphTreeNode<TNode> GetGraphTreeNode(GraphTreeNode<TNode> node, int id)
        {
            if (id == node.Data.Id)
                return node;

            List<GraphTreeNode<TNode>> listChildren = node.Children;
            foreach (GraphTreeNode<TNode> child in listChildren)
            {
                GraphTreeNode<TNode> ret = GetGraphTreeNode(child, id);
                if (ret != null)
                    return ret;
            }
            return null;
        }

        //-------------------------------------------------------------------------------------------------------------
        public void Arrange(Graphics gr, ref float xmin, ref float ymin)
        {
            Root.Arrange(gr, ref xmin, ref ymin);
        }
        
        //-------------------------------------------------------------------------------------------------------------
        public void Arrange(Graphics gr, ref float xmin, ref float ymin, GraphTreeNode<TNode> node)
        {
            node.Arrange(gr, ref xmin, ref ymin);
        }

        //-------------------------------------------------------------------------------------------------------------
        public void UnselectedAllGraphTreeNode()
        {
            UnselectedAllGraphTreeNode(Root);
        }

        //-------------------------------------------------------------------------------------------------------------
        public void UnselectedAllGraphTreeNode(GraphTreeNode<TNode> node)
        {
            node.Data.Selected = false;
            List<GraphTreeNode<TNode>> listChildren = node.Children;
            foreach (GraphTreeNode<TNode> child in listChildren)
                UnselectedAllGraphTreeNode(child);
        }

        //-------------------------------------------------------------------------------------------------------------
        public void UnoverviewAllGraphTreeNode()
        {
            UnoverviewAllGraphTreeNode(Root);
        }

        //-------------------------------------------------------------------------------------------------------------
        public void UnoverviewAllGraphTreeNode(GraphTreeNode<TNode> node)
        {
            node.Data.Overview = false;
            List<GraphTreeNode<TNode>> listChildren = node.Children;
            foreach (GraphTreeNode<TNode> child in listChildren)
                UnoverviewAllGraphTreeNode(child);
        }
    }
}
