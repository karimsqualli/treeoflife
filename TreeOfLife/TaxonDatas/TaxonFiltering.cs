using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeOfLife
{
    //=========================================================================================
    // Filter possible return value
    //
    public enum FilterResultEnum { No, Yes, YesAndChildToo, YesIfNoChild }

    //=========================================================================================
    // Filter interface
    //
    public interface ITaxonFilter
    {
        // filter whole tree
        void Init(TaxonTreeNode _node);
        FilterResultEnum Evaluate(TaxonTreeNode _node);
        void End(TaxonTreeNode _node);
        // filter one node
        bool Match(TaxonTreeNode _node);
        // name
        string Name { get; }
    }

    //=========================================================================================
    // Unnamed filter
    //
    public class TaxonFilterUnnamed : ITaxonFilter
    {
        public void Init(TaxonTreeNode _node) { }

        public FilterResultEnum Evaluate(TaxonTreeNode _node)
        {
            return (_node.Desc.RefMultiName.IsUnnamed()) ? FilterResultEnum.Yes : FilterResultEnum.No;
        }

        public void End(TaxonTreeNode _node) { }

        public bool Match( TaxonTreeNode _node )
        {
            return (_node.Desc.RefMultiName.IsUnnamed()) ? false : true;
        }

        public string Name { get => Localization.Manager.Get("_FilterName_HideUnnamed", "Hide unnamed"); }
    }

    //=========================================================================================
    // Red list category filter
    //
    public class TaxonFilterRedListCategory : ITaxonFilter
    {
        public void Init(TaxonTreeNode _node)
        {
            _CheckExtinctSubdivision = (HiddenCategory & (1 << (int)RedListCategoryEnum.Extinct)) != 0;
        }

        public FilterResultEnum Evaluate(TaxonTreeNode _node)
        {
            if ((_node.RedListCategoryFlags & HiddenCategory) == _node.RedListCategoryFlags)
                return FilterResultEnum.YesAndChildToo;
            if (_CheckExtinctSubdivision && _node.Desc.HasFlag(FlagsEnum.ExtinctInherited))
                return FilterResultEnum.Yes;
            return FilterResultEnum.No;
        }

        public void End(TaxonTreeNode _node)
        {
            if (_CheckExtinctSubdivision)
            {
                _node.ParseNode((n) => 
                {
                    if (n.HasVisibleChild && n.HasHiddenChildren)
                        n.Expand();
                } );
            }
        }

        public bool Match(TaxonTreeNode _node)
        {
            int flag = 1 << (int)_node.Desc.RedListCategory;
            if ((flag & HiddenCategory) == flag)
                return false;
            return true;
        }

        public ushort HiddenCategory { get; set; }
        bool _CheckExtinctSubdivision;

        public string Name { get => Localization.Manager.Get("_FilterName_HideRedListCategory", "Hide red list categories"); }
    }


    //=========================================================================================
    // Filters management
    //
    public class TaxonFilters 
    {
        //-----------------------------------------
        List<ITaxonFilter> Filters = new List<ITaxonFilter>();

        //-----------------------------------------
        public override string ToString()
        {
            if (IsEmpty) return null;
            return string.Join(", ", Filters.Select(f => f.Name));
        }

        //-----------------------------------------
        public void Clear() { if (!IsEmpty) { Filters.Clear(); FiltersChanged(); } }
        
        //-----------------------------------------
        public bool IsEmpty { get { return Filters.Count == 0; } }

        //-----------------------------------------
        public ITaxonFilter GetFilter<T>() where T : ITaxonFilter
        {
            foreach (ITaxonFilter f in Filters)
                if ( f is T) return f;
            return default(T);
        }

        //-----------------------------------------
        public ITaxonFilter AddFilter<T>() where T : ITaxonFilter, new()
        {
            if (GetFilter<T>() != null) return null;
            ITaxonFilter filter = new T();
            Filters.Add(filter);
            FiltersChanged();
            return filter;
        }

        //-----------------------------------------
        public void AddFilter( ITaxonFilter filter )
        {
            Filters.Add(filter);
            FiltersChanged();
        }

        //-----------------------------------------
        public void RemoveFilter<T>() where T : ITaxonFilter
        {
            ITaxonFilter f = GetFilter<T>();
            if (f == null) return;
            Filters.Remove(f);
            FiltersChanged();
        }

        //-----------------------------------------
        public bool HasFilter<T>() where T : ITaxonFilter { return GetFilter<T>() != null; }

        //=================================================
        // Events
        public event EventHandler OnFiltersChanged = null;
        void FiltersChanged()
        {
            OnFiltersChanged?.Invoke(this, EventArgs.Empty);
        }

        //=================================================
        // Filtering tree

        //-----------------------------------------
        private List<TaxonTreeNodeFilter> EvaluateTreeNode(ITaxonFilter _filter, TaxonTreeNode _taxon)
        {
            // never filter first node !!
            FilterResultEnum result = _filter.Evaluate(_taxon);

            if (result == FilterResultEnum.YesAndChildToo)
                return null;

            List<TaxonTreeNodeFilter> resultList = new List<TaxonTreeNodeFilter>();

            if (result == FilterResultEnum.No)
            {
                TaxonTreeNodeFilter ft = new TaxonTreeNodeFilter(_taxon);
                resultList.Add(ft);

                foreach (TaxonTreeNode child in _taxon.Children)
                {
                    List<TaxonTreeNodeFilter> ftChildren = EvaluateTreeNode( _filter, child);
                    if (ftChildren == null || ftChildren.Count == 0) continue;
                    if (ft.Children == null) ft.Children = new List<TaxonTreeNode>();
                    ft.Children.AddRange(ftChildren);
                }
            }
            else if (result == FilterResultEnum.YesIfNoChild)
            {
                TaxonTreeNodeFilter ft2 = null;

                foreach (TaxonTreeNode child2 in _taxon.Children)
                {
                    List<TaxonTreeNodeFilter> ftChildren2 = EvaluateTreeNode(_filter, child2);
                    if (ftChildren2 == null || ftChildren2.Count == 0) continue;
                    
                    if (ft2 == null)
                    {
                        ft2 = new TaxonTreeNodeFilter(_taxon);
                        resultList.Add(ft2);
                        ft2.Children = new List<TaxonTreeNode>();
                    }
                    ft2.Children.AddRange(ftChildren2);
                }
            }
            else
            {
                foreach (TaxonTreeNode child in _taxon.Children)
                {
                    List<TaxonTreeNodeFilter> ftChildren = EvaluateTreeNode(_filter, child);
                    if (ftChildren == null) continue;
                    resultList.AddRange(ftChildren);
                }
            }
            return resultList;
        }

        //-----------------------------------------
        public TaxonTreeNode Evaluate(TaxonTreeNode _root)
        {
            if (IsEmpty) return _root;

            foreach (ITaxonFilter tf in Filters)
            {
                tf.Init(_root);
                List<TaxonTreeNodeFilter> listForRoot = EvaluateTreeNode(tf, _root);

                TaxonTreeNode result = null;
                if (listForRoot == null || listForRoot.Count == 0)
                {
                    result = new TaxonTreeNodeFilter();
                    return result;
                }
                else if (listForRoot.Count == 1)
                    result = listForRoot[0];
                else
                    result = new TaxonTreeNodeFilter(_root) { Children = listForRoot.ToList<TaxonTreeNode>() };
                result.UpdateFather();
                result.UpdateRedListCategoryFlags();
                tf.End(result);

                _root = result;
            }

            _root.UpdateRedListCategoryFlags();

            return _root;
        }

        //-----------------------------------------
        public bool Match(TaxonTreeNode _node)
        {
            if (IsEmpty) return true;

            foreach (ITaxonFilter tf in Filters)
            {
                if (!tf.Match(_node)) return false;
            }
            return true;
        }
    }
}
