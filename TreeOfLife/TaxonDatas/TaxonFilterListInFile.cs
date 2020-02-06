using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife
{
    //=========================================================================================
    // Filter associated with a list of taxon and eventually a file
    //
    public class TaxonFilterListInFile
    {
        //=================================================
        // List
        //
        public List<TaxonTreeNode> List = new List<TaxonTreeNode>();

        public void SetList(List<TaxonTreeNode> _list)
        {
            List = _list;
            ListChanged();
        }

        public void ClearList(bool _setModified)
        {
            List.Clear();
            ListModified = _setModified;
            ListChanged();
        }

        public void AddToList(TaxonTreeNode _node)
        {
            _node = _node?.GetOriginal();
            if (_node == null) return;
            if (List.Contains(_node)) return;
            List.Add(_node);
            ListChanged();
            ListModified = true;
        }

        public void AddToList(List<TaxonTreeNode> _nodes)
        {
            Dictionary<TaxonTreeNode, bool> AlreadyInList = new Dictionary<TaxonTreeNode, bool>();
            List.ForEach(n => AlreadyInList[n] = true);
            int oldCount = List.Count;
            _nodes.ForEach(n => { n = n?.GetOriginal(); if (!AlreadyInList.ContainsKey(n)) List.Add(n); });
            if (List.Count == oldCount) return;
            ListChanged();
            ListModified = true;
        }

        public void RemoveFromList( TaxonTreeNode _node )
        {
            List.Remove(_node);
            ListChanged();
            ListModified = true;
        }

        //=================================================
        // List modified
        //

        public bool ListModified
        {
            get => _ListModified;
            set
            {
                if (_ListModified == value) return;
                _ListModified = value;
                ListModifiedChanged();
            }
        }
        bool _ListModified = false;

        
        public event EventHandler OnListChanged = null;
        void ListChanged()
        {
            OnListChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler OnListModifiedChanged = null;
        void ListModifiedChanged()
        {
            OnListModifiedChanged?.Invoke(this, EventArgs.Empty);
        }

        //=================================================
        // Associated file
        //
        private string _LastFile = null;
        public string CurrentFile { get; private set; }
        public string CurrentName { get { return CurrentFile != null ? Path.GetFileNameWithoutExtension(CurrentFile) : null; } }

        //---------------------------------------------------------------------------------
        public void SetCurrentFilter(string _filename, bool _doLoad = true, bool _removeFilter = true)
        {
            CurrentFile = _filename;
            if (CurrentFile == null)
                ClearList(false);
            else
            {
                _LastFile = _filename;
                ListModified = false;
                if (_doLoad)
                {
                    TaxonList tl = TaxonList.Load(CurrentFile);
                    if (tl != null)
                        SetList(tl.ToTaxonTreeNodeList(TaxonUtils.OriginalRoot));
                    else
                        ClearList(false);
                }
                if (_removeFilter)
                    TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.RemoveFilter<TaxonFilterTag>());
            }
        }

        //---------------------------------------------------------------------------------
        public void BrowseAndOpen()
        {
            if (!IgnoreUnsavedModification())
                return;

            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = String.IsNullOrEmpty(_LastFile) ? TaxonList.GetFolder() : System.IO.Path.GetDirectoryName(_LastFile),
                Filter = TaxonList.FileFilters,
                FilterIndex = 2 * (int)TaxonList.FilterIndexFromFile(_LastFile, TaxonList.FileFilterIndexEnum.ListOfTaxons),
                Multiselect = false,
                AddExtension = true
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            if (!File.Exists(ofd.FileName)) return;
            SetCurrentFilter(ofd.FileName, true, true);
        }

        //---------------------------------------------------------------------------------
        public void Clear()
        {
            if (!IgnoreUnsavedModification())
                return;
            TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.RemoveFilter<TaxonFilterTag>());
            SetCurrentFilter(null, false, true);
        }

        //---------------------------------------------------------------------------------
        public bool IgnoreUnsavedModification()
        {
            if (!ListModified) return true;

            string message = "Some modifications haven't been saved !\n\n";
            message += "Click <Yes> to save them\n";
            message += "Click <No> to ignore them (and loose them)\n";
            message += "Click <Cancel> to keep current modifier filter";
            DialogResult result = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.Cancel)
                return false;

            if (result == DialogResult.Yes)
                return SaveCurrentOrAs();

            return true;
        }

        //=================================================================================
        // Filter
        //
        public bool FilterIsActive { get => TaxonUtils.CurrentFilters.GetFilter<TaxonFilterTag>() != null; }
        public bool FilterIsActivable { get => !FilterIsActive && List.Count > 0; }

        //=================================================================================
        // save function
        //

        //---------------------------------------------------------------------------------
        public bool CanSaveCurrentFile { get => CurrentFile != null && ListModified; }

        //---------------------------------------------------------------------------------
        public bool SaveCurrentFile()
        {
            if (CurrentFile == null) return false;
            TaxonList list = new TaxonList
            {
                HasFile = true,
                FileName = CurrentFile
            };
            list.FromTaxonTreeNodeList(List);
            bool result = list.Save(false, TaxonList.FileFilterIndexEnum.ListOfTaxons);
            ListModified = false;
            return result;
        }

        //---------------------------------------------------------------------------------
        public bool CanSaveAs { get => List.Count > 0; }

        //---------------------------------------------------------------------------------
        public bool SaveAs()
        {
            TaxonList list = new TaxonList
            {
                HasFile = true,
                FileName = CurrentFile
            };
            list.FromTaxonTreeNodeList(List);
            if (list.Save(true, TaxonList.FileFilterIndexEnum.ListOfTaxons))
            {
                SetCurrentFilter(list.FileName, false, false);
                return true;
            }
            return false;
        }

        //---------------------------------------------------------------------------------
        public bool CanSaveCurrentOrAs { get => (CurrentFile == null) ? CanSaveAs : CanSaveCurrentFile; }

        //---------------------------------------------------------------------------------
        public bool SaveCurrentOrAs() { return (CurrentFile == null) ? SaveAs() : SaveCurrentFile(); }
    }

    //=================================================================================
    // Filter
    //

    //---------------------------------------------------------------------------------
    public class TaxonFilterTag : ITaxonFilter
    {
        public TaxonFilterTag(List<TaxonTreeNode> _list) { _List = _list; }

        public void Init(TaxonTreeNode _node)
        {
            _node.ParseNodeDesc((d) => { d.UnsetFlag(FlagsEnum.User0); });
            foreach (TaxonTreeNode node in _List)
            {
                TaxonTreeNode current = node;
                while (current != null)
                {
                    current.Desc.SetFlag(FlagsEnum.User0);
                    current = current.Father;
                }
            }
        }

        public FilterResultEnum Evaluate(TaxonTreeNode _node)
        {
            return _node.Desc.HasFlag(FlagsEnum.User0) ? FilterResultEnum.No : FilterResultEnum.Yes;
        }

        public void End(TaxonTreeNode _node) { }

        public bool Match(TaxonTreeNode _node)
        {
            return _List.Contains(_node.GetOriginal());
        }

        private readonly List<TaxonTreeNode> _List;

        public string Name { get => "Tag"; }
    }
}
