using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeOfLife
{
    public class TaxonSearch
    {
        TaxonTreeNode _Root = null;
        Dictionary<string, TaxonTreeNode> _Dico = null;
        Dictionary<string, List<TaxonTreeNode>> _DicoList = null;

        public TaxonSearch(TaxonTreeNode _root, bool _init = true, bool _useAlternativeNames = false )
        {
            _Root = _root;
            if (_init) Init(_useAlternativeNames);
        }

        void Init(bool _useAlternativeNames)
        {
            if (_Dico != null) return;
            _Dico = new Dictionary<string, TaxonTreeNode>();
            _DicoList = new Dictionary<string, List<TaxonTreeNode>>();
            if (_useAlternativeNames)
                _Root.ParseNode(RegisterTaxonWithAlternativeNames);
            else
                _Root.ParseNode(RegisterTaxon);
            //Console.WriteLine(_DicoList.Count);
        }

        public void Init(TaxonTreeNode.NodeAction _action)
        {
            if (_Dico != null) return;
            _Dico = new Dictionary<string, TaxonTreeNode>();
            _DicoList = new Dictionary<string, List<TaxonTreeNode>>();
            _Root.ParseNode(_action);
        }

        public void Add(string _name, TaxonTreeNode _node)
        {
            string key = _name.ToLower().Replace('-', ' ');
            if (_Dico.TryGetValue(key, out TaxonTreeNode other))
            {
                if (_DicoList.TryGetValue(key, out List<TaxonTreeNode> list))
                    list.Add(_node);
                else
                    _DicoList[key] = new List<TaxonTreeNode>() { other, _node };
            }
            else
                _Dico[key] = _node;
        }

        void RegisterTaxonWithAlternativeNames(TaxonTreeNode _node)
        {
            if (_node.Desc.IsUnnamed) return;
            Add(_node.Desc.RefMultiName.Main, _node);
            if (_node.Desc.RefMultiName.HasSynonym)
            {
                foreach (string s in _node.Desc.RefMultiName.GetSynonymsArray())
                    Add(s, _node);
            }
        }

        void RegisterTaxon(TaxonTreeNode _node)
        {
            if (_node.Desc.IsUnnamed) return;
            Add(_node.Desc.RefMultiName.Main, _node);
        }

        public TaxonTreeNode FindOne(string _name)
        {
            string key = _name.Trim().ToLower().Replace('-', ' ');
            if (_Dico.TryGetValue(key, out TaxonTreeNode node))
                return node;
            return null;
        }

        public List<TaxonTreeNode> FindAll(string _name)
        {
            string key = _name.ToLower().Replace('-', ' ');
            if (_DicoList.TryGetValue(key, out List<TaxonTreeNode> list))
                return list;
            if (_Dico.TryGetValue(key, out TaxonTreeNode node))
                return new List<TaxonTreeNode>() { node };
            return null;
        }

    }
}
