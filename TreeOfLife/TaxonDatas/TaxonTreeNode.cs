using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace TreeOfLife
{
    public partial class TaxonTreeNode : IComparable
    {
        //---------------------------------------------------------------------------------
        TaxonDesc _Desc = null;
        [XmlElement("Taxon")]
        public TaxonDesc Desc
        {
            get { return _Desc; }
            set { _Desc = value; }
        }

        //---------------------------------------------------------------------------------
        public static readonly string HierarchicalNameSeparator = "|";
        public string GetHierarchicalName()
        {
            if (_Father == null)
                return _Desc.RefMultiName.Main;
            return Father.GetHierarchicalName() + "|" + _Desc.RefMultiName.Main;
        }

        //---------------------------------------------------------------------------------
        TaxonTreeNode _Father = null;
        [XmlIgnore, ScriptIgnore] 
        public TaxonTreeNode Father
        {
            get { return _Father; }
            set { _Father = value; }
        }

        List<TaxonTreeNode> _Children = new List<TaxonTreeNode>();
        public List<TaxonTreeNode> Children
        {
            get { return _Children; }
            set { _Children = value; }
        }

        [XmlIgnore, ScriptIgnore]
        public bool HasChildren { get { return _Children.Count != 0; } }

        [XmlIgnore, ScriptIgnore]
        public bool IsEndLeaf { get { return (Desc.ClassicRank == ClassicRankEnum.Espece && Children.Count == 0) || (Desc.ClassicRank == ClassicRankEnum.SousEspece); } }

        [XmlIgnore, ScriptIgnore]
        public List<TaxonTreeNode> VisibleChildren
        {
            get
            {
                List<TaxonTreeNode> visibleChild = new List<TaxonTreeNode>();
                foreach (TaxonTreeNode child in Children)
                    if (child.Visible) visibleChild.Add(child);
                return visibleChild;
            }
        }

        [XmlIgnore, ScriptIgnore]
        public bool HasHiddenChildren
        {
            get
            {
                foreach (TaxonTreeNode child in Children)
                    if (!child.Visible) return true;
                return false;
            }
        }

        [XmlIgnore, ScriptIgnore]
        public bool HasVisibleChild
        {
            get
            {
                foreach (TaxonTreeNode child in Children)
                    if (child.Visible) return true;
                return false;
            }
        }

        [XmlIgnore, ScriptIgnore]
        public bool HasAllChildVisible { get { return !HasHiddenChildren; } }
         


        //=========================================================================================
        // Constructors
        //

        public TaxonTreeNode() { }
        public TaxonTreeNode(TaxonDesc _taxonDesc ) : this() { _Desc = _taxonDesc; }

        public TaxonTreeNode Duplicate()
        {
            MemoryStream s = SaveXMLInMemory();
            TaxonTreeNode duplicata = LoadXMLFromMemory(s);
            s.Dispose();
            return duplicata;
        }

        //---------------------------------------------------------------------------------
        public override string ToString() { return "TaxonTreeNode: " + Desc == null ? "<null>" : Desc.RefMultiName.Main; }

        //=========================================================================================
        // comparison interface
        //

        //---------------------------------------------------------------------------------
        public int CompareTo(object obj)
        {
            if (!(obj is TaxonTreeNode)) return 0;
            return Compare(this, obj as TaxonTreeNode);
        }

        
        //---------------------------------------------------------------------------------
        public static int Compare(TaxonTreeNode t1, TaxonTreeNode t2)
        {
            if (t1 == t2) return 0;

            if (t1.Father == t2.Father)
            {
                return (t1.Father.Children.IndexOf(t1) < t2.Father.Children.IndexOf(t2)) ? -1 : 1;
            }

            int level1 = t1.GetGeneration();
            int level2 = t2.GetGeneration();

            if (level1 < level2)
                return Compare(t1, t2.Father);
            if (level2 < level1)
                return Compare(t1.Father, t2);
            return Compare(t1.Father, t2.Father);
        }

        //---------------------------------------------------------------------------------
        public static int CompareOnlyName(TaxonTreeNode t1, TaxonTreeNode t2)
        {
            return String.Compare(t1.Desc.RefMultiName.Main, t2.Desc.RefMultiName.Main);
        }

        //---------------------------------------------------------------------------------
        public void SortChildren()
        {
            if (!HasChildren) return;
            Children.Sort((x, y) =>
            {
                if (x.Desc.IsExtinct != y.Desc.IsExtinct) return x.Desc.IsExtinct ? -1 : 1;
                if (x.Desc.IsUnnamed != y.Desc.IsUnnamed) return x.Desc.IsUnnamed ? 1 : -1;
                return x.Desc.RefMainName.CompareTo(y.Desc.RefMainName);
            });
        }

        //=========================================================================================
        // Parent functions
        //
        //------------------------------------------
        // mets à jour de manière récursive tous les papas
        public void UpdateFather()
        {
            foreach (TaxonTreeNode child in _Children)
            {
                child.Father = this;
                child.UpdateFather();
            }
        }

        public bool IsChildOf(TaxonTreeNode _taxon)
        {
            if (_taxon == null) return false;
            TaxonTreeNode current = Father;
            while (current != null)
            {
                if (current == _taxon) return true;
                current = current.Father;
            }
            return false;
        }


        //méthode pour retourner la liste des parents 
        public void GetAllParents(List<TaxonTreeNode> _list, bool _includeFirst = true, bool _fromFatherToChild = true, bool _includeUnnamed = true )
        {
            TaxonTreeNode current = this;
            if (!_includeFirst) current = current.Father;
            if (_includeUnnamed)
            {
                while (current != null)
                {
                    _list.Add(current);
                    current = current._Father;
                }
            }
            else
            {
                while (current != null)
                {
                    if (!current.Desc.IsUnnamed) _list.Add(current);
                    current = current._Father;
                }
            }
            if (_fromFatherToChild)
                _list.Reverse();
        }

        public TaxonTreeNode GetParentRoot()
        {
            TaxonTreeNode result = this;
            while (result.Father != null)
                result = result.Father;
            return result;
        }

        //=========================================================================================
        // Image functions
        //
        public bool HasChildrenWithImage = true;

        public void UpdateAvailableImagesRec()
        {
            Desc.UpdateAvailableImages();
            HasChildrenWithImage = false;
            foreach (TaxonTreeNode child in _Children)
            {
                child.UpdateAvailableImagesRec();
                HasChildrenWithImage |= child.Desc.HasImage | child.HasChildrenWithImage;
            }
        }

        public void UpdateAvailableImages()
        {
            DateTime ts_Start = DateTime.Now;
            UpdateAvailableImagesRec();

            int count = 0;
            ParseNodeDesc( (d) => { if (d.HasImage) count++;}  );

            Loggers.WriteInformation(LogTags.Image, string.Format("Update available images took {0} ms\n   {1} taxon have images", (DateTime.Now - ts_Start).Milliseconds, count));
        }
        //=========================================================================================
        // RedListCategoryFlags
        //

        [XmlIgnore, ScriptIgnore]
        public ushort RedListCategoryFlags { get; private set; } = 0;

        public void UpdateRedListCategoryFlags()
        {
            RedListCategoryFlags = (ushort)(1 << (int)Desc.RedListCategory);
            foreach (TaxonTreeNode child in Children)
            {
                child.UpdateRedListCategoryFlags();
                RedListCategoryFlags |= child.RedListCategoryFlags;
            }
        }


        //=========================================================================================
        // Count functions
        //

        //-----------------------------------------
        public int Count()
        {
            int count = 1;
            foreach (TaxonTreeNode child in _Children)
                count += child.Count();
            return count;
        }

        //-----------------------------------------
        public int Count( ClassicRankEnum _classikRank )
        {
            int count = _Desc.ClassicRank == _classikRank ? 1 : 0;
            foreach (TaxonTreeNode child in _Children)
                count += child.Count(_classikRank);
            return count;
        }

        //=========================================================================================
        // Children functions
        //

        //------------------------------------------
        public TaxonTreeNode FindTaxonByName(string _name)
        {
            if (_Desc.RefMultiName.Main.ToLower() == _name) return this;
            foreach (TaxonTreeNode child in _Children)
            {
                TaxonTreeNode result = child.FindTaxonByName(_name);
                if (result != null) return result;
            }
            return null;
        }

        //------------------------------------------
        public TaxonTreeNode FindTaxon(TaxonDesc _desc)
        {
            if (_Desc == _desc) return this;
            foreach (TaxonTreeNode child in _Children)
            {
                TaxonTreeNode result = child.FindTaxon(_desc);
                if (result != null) return result;
            }
            return null;
        }

        //------------------------------------------
        private TaxonTreeNode FindTaxonByFullNameRecursive(string _fullname)
        {
            int index = _fullname.IndexOf('|');
            if (index == -1)
            {
                foreach (TaxonTreeNode child in _Children)
                {
                    if (child.Desc.RefMultiName.Main == _fullname)
                        return child;
                }
            }
            else
            {
                string name = _fullname.Substring(0, index);

                foreach (TaxonTreeNode child in _Children)
                {
                    if (child.Desc.RefMultiName.Main == name)
                    {
                        TaxonTreeNode result = child.FindTaxonByFullNameRecursive(_fullname.Substring(index + 1));
                        if (result != null)
                            return result;
                    }
                }
            }
            return null;
        }

        //------------------------------------------
        public TaxonTreeNode FindTaxonByFullName(string _fullname)
        {
            int index = _fullname.IndexOf('|');
            if (index == -1) 
                return (Desc.RefMultiName.Main == _fullname) ? this : null;

            string name = _fullname.Substring(0, index);
            if (name != Desc.RefMultiName.Main) return null;

            return FindTaxonByFullNameRecursive(_fullname.Substring(index + 1));
        }

        //------------------------------------------
        public void AddChild(TaxonTreeNode _child)
        {
            _child.Father = this;
            _Children.Add(_child);
        }

        //------------------------------------------
        public void ReplaceChild(TaxonTreeNode _child, TaxonTreeNode _newChild )
        {
            int index = _Children.IndexOf(_child);
            if (index == -1) return;
            _newChild.Father = this;
            _Children[index] = _newChild;
        }

        //------------------------------------------
        public void AddSiblingBefore(TaxonTreeNode _sibling)
        {
            if (Father == null) return;
            int index = Father.Children.IndexOf(this);
            if (index == -1) return;
            Father.Children.Insert(index, _sibling);
            _sibling.Father = Father;
        }

        //------------------------------------------
        public void AddSiblingAfter(TaxonTreeNode _sibling)
        {
            if (Father == null) return;
            int index = Father.Children.IndexOf(this);
            if (index == -1) return;
            index++;
            if (index == Father.Children.Count)
                Father.Children.Add(_sibling);
            else
                Father.Children.Insert(index, _sibling);
            _sibling.Father = Father;
        }

        //------------------------------------------
        public bool CanMoveUp()
        {
            if (Father == null) return false;
            return (Father.Children.IndexOf(this) > 0);
        }

        public void MoveUp()
        {
            if (Father == null) return;
            int index = Father.Children.IndexOf(this);
            if (index <= 0) return;
            Father.Children.RemoveAt(index);
            Father.Children.Insert(index - 1, this);
        }

        public void MoveTop()
        {
            if (Father == null) return;
            int index = Father.Children.IndexOf(this);
            if (index <= 0) return;
            Father.Children.RemoveAt(index);
            Father.Children.Insert(0, this);
        }

        //------------------------------------------
        public bool CanMoveDown()
        {
            if (Father == null) return false;
            int index = Father.Children.IndexOf(this);
            if (index == -1) return false;
            if (index == Father.Children.Count - 1) return false;
            return true;
        }

        public void MoveDown()
        {
            if (Father == null) return;
            int index = Father.Children.IndexOf(this);
            if (index == -1) return;
            if (index == Father.Children.Count - 1) return;
            Father.Children.RemoveAt(index);
            index++;
            if (index == Father.Children.Count)
                Father.Children.Add(this);
            else
                Father.Children.Insert(index, this);
        }

        public void MoveBottom()
        {
            if (Father == null) return;
            int index = Father.Children.IndexOf(this);
            if (index == -1) return;
            if (index == Father.Children.Count - 1) return;
            Father.Children.RemoveAt(index);
            Father.Children.Add(this);
        }

        //------------------------------------------
        // recupère tous les fils quelquesoit leur génération
        public void GetAllChildrenRecursively(List<TaxonTreeNode> _allChildren)
        {
            foreach (TaxonTreeNode child in _Children)
            {
                _allChildren.Add(child);
                child.GetAllChildrenRecursively(_allChildren);
            }
        }

        //------------------------------------------
        // recupère tous les fils quelquesoit leur génération
        public void GetAllChildrenRecursively(List<TaxonTreeNode> _allChildren, ClassicRankEnum _classikRank)
        {
            if (_Desc.ClassicRank == _classikRank)
                _allChildren.Add(this);

            foreach (TaxonTreeNode child in _Children)
                child.GetAllChildrenRecursively(_allChildren, _classikRank);
        }

        //------------------------------------------
        // recupère tous les fils quelle que soit leur génération mais qui n'ont plus de descendants
        public void GetAllLastChildrenRecursively(List<TaxonTreeNode> _allLastChildren)
        {
            if (_Children.Count == 0)
                _allLastChildren.Add(this);
            else
            {
                foreach (TaxonTreeNode child in _Children)
                    child.GetAllLastChildrenRecursively(_allLastChildren);
            }
        }

        //------------------------------------------
        // recupère tous les fils quelquesoit leur génération mais avec une image
        public void GetAllChildrenWithImageRecursively(List<TaxonTreeNode> _allChildren)
        {
            if (Desc.HasImage && TaxonUtils.MatchCurrentFilters(this))
                _allChildren.Add(this);
            if (!HasChildrenWithImage)
                return;
            foreach (TaxonTreeNode child in _Children)
                child.GetAllChildrenWithImageRecursively(_allChildren);
        }

        //------------------------------------------
        // méthode to get all descendant level x    >>>>>>>>>>>>>>>>>   j'ai compris !!!!!! :):):):)
        public void GetDescendentLevel(int _level, List<TaxonTreeNode> _list)
        {
            if (_level == 1)
                _list.Add(this);
            else
            {
                foreach (TaxonTreeNode child in _Children)
                {
                    child.GetDescendentLevel(_level - 1, _list);
                }
            }
        }

        //=========================================================================================
        // Generation functions
        //

        // méthode pour retourner la génération d'un taxon
        public int GetGeneration()
        {
            int level = 0;
            TaxonTreeNode current = this;
            while (current != null)
            {
                level++;
                current = current._Father;
            }
            return level;
        }

        // nombre max de génération
        // vite fait, pas vérifiée si elle marche correctement
        public int GetMaxDescendentGeneration()
        {
            if (!HasChildren) return 0;

            int maxLevel = 0;
            foreach (TaxonTreeNode child in _Children)
            {
                int level = child.GetMaxDescendentGeneration() + 1;
                if (level > maxLevel) maxLevel = level;
            }
            return maxLevel;
        }

        //=========================================================================================
        // expand/collapse functions
        //

        public bool IsExpanded()
        {
            foreach (TaxonTreeNode child in Children)
                if (child.Visible) return true;
            return false;
        }

        // expand : tous les fils directs deviennent visibles
        public void Expand()
        {
            //if (GetOriginal() != this) GetOriginal().expand();
            foreach (TaxonTreeNode child in Children)
                child.Visible = true;
        }

        // expand extended : expand tous les taxons de la génération la plus proche non totalement visible
        public void ExpandExtended()
        {
            //if (GetOriginal() != this) GetOriginal().expandExtended();

            // cas spécial, si il est pas expanded, just expand it
            if (!HasAllChildVisible)
            {
                Expand();
                return;
            }

            // parcours tous les enfants pour voir si y'en a au moins un qui n'est pas expanded
            foreach (TaxonTreeNode child in Children)
            {
                if (!child.HasAllChildVisible)
                {
                    foreach (TaxonTreeNode child2 in Children)
                        child2.Expand();
                    return;
                }
            }

            // si on est la c'est que tout les enfants sont expanded => passe récursivement à la 
            foreach (TaxonTreeNode child in Children)
            {
                child.ExpandExtended();
            }
        }

        // rend tout visible
        public void ExpandAll()
        {
            //if (GetOriginal() != this) GetOriginal().expandAll();
            Visible = true;
            foreach (TaxonTreeNode child in Children)
                child.ExpandAll();
        }

        // cache tous les fils
        public void Collapse()
        {
            //if (GetOriginal() != this) GetOriginal().collapse();
            foreach (TaxonTreeNode child in Children)
                child.Visible = false;
        }

        // cache tous les fils de la denière génération visible (i.e. qui a au moins un taxon visible)
        private void CollapseExtended(List<TaxonTreeNode> _toCollapse)
        {
            List<TaxonTreeNode> visibleChildren = new List<TaxonTreeNode>();
            foreach (TaxonTreeNode t in _toCollapse)
                visibleChildren.AddRange(t.VisibleChildren);

            if (visibleChildren.Count == 0)
            {
                foreach (TaxonTreeNode t in _toCollapse)
                    t.Visible = false;
                return;
            }

            CollapseExtended(visibleChildren);
        }

        public void CollapseExtended()
        {
            //if (GetOriginal() != this) GetOriginal().collapseExtended();
            if (!HasChildren) return;
            if (!HasVisibleChild) return;
            List<TaxonTreeNode> visibleChildren = new List<TaxonTreeNode>(VisibleChildren);
            CollapseExtended(visibleChildren);
        }

        // collapse tout
        public void CollapseAll()
        {
            //if (GetOriginal() != this) GetOriginal().collapseAll();
            foreach (TaxonTreeNode child in Children)
            {
                child.Visible = false;
                child.CollapseAll();
            }
        }


        //=========================================================================================
        // Filter part
        //
        public virtual bool IsFiltered() { return false; }
        public virtual TaxonTreeNode GetOriginal() { return this; }
        public virtual TaxonTreeNode GetFiltered(TaxonTreeNode _root)
        {
            if (_root == null) return null;
            return _root.FindTaxon(Desc);
        }

        //=========================================================================================
        // Highlight 
        //

        // To affiche Highlight or not ?
        [XmlIgnore, ScriptIgnore]
        public bool Highlight = false;

        public void HighlightClear()
        {
            Highlight = false;
            foreach (TaxonTreeNode child in Children)
                child.HighlightClear();
        }

        //=========================================================================================
        // Tag
        //

        // User data
        [XmlIgnore, ScriptIgnore]
        public bool Flag = false;

        public void FlagClear()
        {
            Flag = false;
            foreach (TaxonTreeNode child in Children)
                child.FlagClear();
        }

        public void FlagSet()
        {
            Flag = true;
            foreach (TaxonTreeNode child in Children)
                child.FlagSet();
        }

        //=========================================================================================
        // Parse
        //
        public delegate void NodeDescAction( TaxonDesc _desc );
        public void ParseNodeDesc( NodeDescAction _action)
        {
            _action(Desc);
            foreach (TaxonTreeNode child in Children)
                child.ParseNodeDesc(_action);
        }

        public delegate void NodeDescActionWithParam(TaxonDesc _desc, object _param);
        public void ParseNodeDesc(NodeDescActionWithParam _action, object _param)
        {
            _action(Desc, _param);
            foreach (TaxonTreeNode child in Children)
                child.ParseNodeDesc(_action, _param);
        }

        public delegate void NodeAction(TaxonTreeNode _node);
        public void ParseNode(NodeAction _action)
        {
            _action(this);
            foreach (TaxonTreeNode child in Children)
                child.ParseNode(_action);
        }

        public void ParseNodeChildrenFirst(NodeAction _action)
        {
            foreach (TaxonTreeNode child in Children)
                child.ParseNodeChildrenFirst(_action);
            _action(this);
        }

        public delegate void NodeActionWithParam(TaxonTreeNode _node, object _param);
        public void ParseNode(NodeActionWithParam _action, object _param)
        {
            _action(this, _param);
            foreach (TaxonTreeNode child in Children)
                child.ParseNode(_action, _param);
        }

        //=========================================================================================
        // GRAPH PART
        //

        public class LayoutParams
        {
            public int width = 100;
            public int height = 30;
            public int columnInter = 30;
            public List<int> classicRankColumn = null;
        }

        // R : coordonnées d'affichage du taxon (sur sa colonne)
        [XmlIgnore, ScriptIgnore]
        public Rectangle R;
        [XmlIgnore, ScriptIgnore]
        public int RLeftMargin;
        // WithChildren : taille du rectangle englobant le taxon et tout ses fils
        [XmlIgnore, ScriptIgnore]
        public int WidthWithChildren, HeightWithChildren;
        
        // To affiche or not to affiche ?
        [XmlIgnore, ScriptIgnore]
        public bool Visible = false;

        // retourne le rectangle avec les petiots (englobant)
        public Rectangle RectangleWithChildren
        {
            get
            {
                System.Drawing.Rectangle RWC = R;
                RWC.Width = WidthWithChildren;
                return RWC;
            }
        }

        // recalcul toutes les positions récursivement
        public void Computelayout(int x0, int y0, LayoutParams _params)
        {
            if (_params.classicRankColumn != null)
                ComputeLayoutHierarchy(x0, y0, _params);
            else
                ComputeLayoutHierarchy(x0,y0,_params);
        }

        // recalcul toutes les positions récursivement
        void ComputeLayoutHierarchy(int x0, int y0, LayoutParams _params)
        {
            RLeftMargin = 0;
            R.X = x0;
            R.Y = y0;

            if (!Visible)
            {
                R.Width = R.Height = WidthWithChildren = HeightWithChildren = 0;
                return;
            }

            WidthWithChildren = R.Width = _params.width;
            HeightWithChildren = R.Height = _params.height;
            if (Children.Count == 0) return;

            int ychild = y0;
            int widthmax = 0;
            foreach (TaxonTreeNode child in Children)
            {
                child.ComputeLayoutHierarchy(x0 + R.Width + _params.columnInter, ychild, _params);
                ychild += child.HeightWithChildren;
                if (child.WidthWithChildren > widthmax) widthmax = child.WidthWithChildren;
            }

            if (widthmax > 0 )
                WidthWithChildren += widthmax + _params.columnInter;

            if (ychild - y0 > HeightWithChildren) HeightWithChildren = ychild - y0;
            R.Height = HeightWithChildren;
        }

        // recompute each node position when using classik rank rule.
        public void ComputeLayoutClassikRank(int x0, int y0, LayoutParams _params )
        {
            RLeftMargin = 0;
            R.X = x0;
            R.Y = y0;
            
            if (!Visible)
            {
                R.Width = R.Height = WidthWithChildren = HeightWithChildren = 0;
                return;
            }

            WidthWithChildren = R.Width = _params.width;
            HeightWithChildren = R.Height = _params.height;

            int column = _params.classicRankColumn[(int) Desc.ClassicRank];
            if (column != -1)
            {
                int x1 = column * _params.width;
                if (x1 > x0)
                {
                    RLeftMargin = x1 - x0;
                    x1 += _params.width;
                    if (x1 > R.Right)
                        WidthWithChildren = R.Width = x1 - x0;
                }
            }
            
            if (Children.Count == 0) return;

            int ychild = y0;
            int widthmax = 0;
            foreach (TaxonTreeNode child in Children)
            {
                child.ComputeLayoutClassikRank(x0 + R.Width + _params.columnInter, ychild, _params);
                ychild += child.HeightWithChildren;
                if (child.WidthWithChildren > widthmax) widthmax = child.WidthWithChildren;
            }

            WidthWithChildren += widthmax + _params.columnInter;
            if (ychild - y0 > HeightWithChildren) HeightWithChildren = ychild - y0;
            R.Height = HeightWithChildren;
        }

        //=========================================================================================
        // MORE GRAPH PART  but for CIRCULAR display
        //
        public class TaxonCircularInfo 
        {
            public double Angle;
            public double X;
            public double Y;
            public double Dx;
            public double Dy;
            public double Nx;
            public double Ny;
            public PointF[] Points;
            public bool InternalPoint;
            public float LengthBefore;
            public Rectangle Bounds;
        }

        public class CircularParams
        {
            public CircularParams( double _perimeter, int _taxonNumber, int _maxLevels, int _taxonWidth, int _taxonHeight )
            {
                Perimeter = _perimeter;
                TaxonNumber = _taxonNumber;
                TaxonWidth = _taxonWidth;
                TaxonHeight = _taxonHeight;
                TaxonHeightO2 = _taxonHeight / 2;
                TaxonMaxLevel = _maxLevels;

                Radius = Perimeter / (Math.PI * 2.0);
                AnglePerTaxon = (2 * Math.PI) / TaxonNumber;

                Width = (Radius + TaxonWidth) * 2;
                Height = Width;
                CenterX = (Radius + TaxonWidth);
                CenterY = (Radius + TaxonWidth);

                CurrentIndex = 0;
                CurrentLevel = 0;

                RadiusPart = Radius / (TaxonMaxLevel - 1);
                DemiRadiusPart = RadiusPart / 2;

                RadiusPerLevel = new List<double>(TaxonMaxLevel);
                for (int i = 0; i < TaxonMaxLevel; i++)
                    RadiusPerLevel.Add(RadiusPart * i);

                

            }

            public double Width { get; private set; }
            public double Height{ get; private set; }
            public double TaxonWidth{ get; private set; }
            public double TaxonHeight{ get; private set; }
            public double TaxonHeightO2{ get; private set; }
            public double CenterX{ get; private set; }
            public double CenterY{ get; private set; }
            public double Perimeter { get; private set; }
            public double Radius { get; private set; }
            public double AnglePerTaxon { get; private set; }

            public int TaxonNumber { get; private set; }
            public int CurrentIndex { get; private set; }

            public int TaxonMaxLevel { get; private set; }
            public int CurrentLevel { get; private set; }
            public List<double> RadiusPerLevel { get; private set; }
            
            public double RadiusPart { get; private set; }
            public double DemiRadiusPart { get; private set; }

            public TaxonCircularInfo TaxonNextPoint()
            {
                TaxonCircularInfo result = new TaxonCircularInfo { Angle = CurrentIndex * AnglePerTaxon };

                CurrentIndex++;
                result.Dx = Math.Cos( result.Angle );
                result.Dy = Math.Sin( result.Angle );
                result.X = CenterX + result.Dx * Radius;
                result.Y = CenterY + result.Dy * Radius;
                result.Nx = - result.Dy;
                result.Ny = result.Dx;

                double dx = TaxonHeightO2 * result.Nx;
                double dy = TaxonHeightO2 * result.Ny;
                result.Points = new PointF[4];
                result.Points[0].X = (float) (result.X + dx);
                result.Points[0].Y = (float) (result.Y + dy);
                result.Points[3].X = (float) (result.X - dx);
                result.Points[3].Y = (float) (result.Y - dy);
                //dx = TaxonWidth * result.Dx;
                //dy = TaxonWidth * result.Dy;
                dx = 10 * result.Dx;
                dy = 10 * result.Dy;
                result.Points[1].X = (float) (result.Points[0].X + dx);
                result.Points[1].Y = (float) (result.Points[0].Y + dy);
                result.Points[2].X = (float) (result.Points[3].X + dx);
                result.Points[2].Y = (float) (result.Points[3].Y + dy);

                result.InternalPoint = false;
                result.LengthBefore = (float)(DemiRadiusPart + (TaxonMaxLevel - CurrentLevel - 1) * RadiusPart);

                float xmin = result.Points[0].X, xmax = xmin, ymin = result.Points[0].Y, ymax = ymin;
                for (int i = 1; i < 4; i++)
                {
                    if (result.Points[i].X < xmin) xmin = result.Points[i].X;
                    if (result.Points[i].X > xmax) xmax = result.Points[i].X;
                    if (result.Points[i].Y < ymin) ymin = result.Points[i].Y;
                    if (result.Points[i].Y > ymax) ymax = result.Points[i].Y;
                }
                result.Bounds = Rectangle.FromLTRB((int)xmin, (int)ymin, (int)xmax, (int)ymax);

                return result;
            }

            public TaxonCircularInfo TaxonInternalPoint( double angle1, double angle2, int _level )
            {
                TaxonCircularInfo result = new TaxonCircularInfo { Angle = (angle1 + angle2) / 2 };

                result.Dx = Math.Cos(result.Angle);
                result.Dy = Math.Sin(result.Angle);
                result.X = CenterX + result.Dx * RadiusPerLevel[_level];
                result.Y = CenterY + result.Dy * RadiusPerLevel[_level];
                result.Nx = -result.Dy;
                result.Ny = result.Dx;

                result.InternalPoint = true;

                double angleStep = 0.01f;
                float radius = (float) (RadiusPerLevel[_level] + DemiRadiusPart);
                int nb = (int) ((angle2 - angle1) / angleStep);
                if (nb > 0)
                {
                    angleStep = (angle2 - angle1);
                    if (angleStep < 0) angleStep += Math.PI * 2;
                    angleStep /= nb;
                    nb--;
                }

                result.Points = new PointF[nb + 2];
                float x = (float) Math.Cos(angle1);
                float y = (float) Math.Sin(angle1);
                result.Points[0].X = (float)(CenterX + x * radius);
                result.Points[0].Y = (float)(CenterY + y * radius);

                for (int i = 1; i <= nb; i++)
                {
                    float angle = (float)( angle1 + angleStep * i);
                    x = (float)Math.Cos(angle);
                    y = (float)Math.Sin(angle);
                    result.Points[i].X = (float)(CenterX + x * radius);
                    result.Points[i].Y = (float)(CenterY + y * radius);
                }
                x = (float)Math.Cos(angle2);
                y = (float)Math.Sin(angle2);
                result.Points[nb + 1].X = (float)(CenterX + x * radius);
                result.Points[nb + 1].Y = (float)(CenterY + y * radius);

                result.LengthBefore = (float)(DemiRadiusPart + (TaxonMaxLevel - _level) * radius);
                
                return result;
            }

            public void TaxonLevelUp() { CurrentLevel++; }
            public void TaxonLevelDown() { CurrentLevel--; }

        }

        public CircularParams ComputeLayoutCircular(int x0, int y0, LayoutParams _params)
        {
            double perimeter = HeightWithChildren;
            double number = perimeter / _params.height;
            double level = WidthWithChildren / _params.width;
            CircularParams cp = new CircularParams(HeightWithChildren, (int)number, (int) level, _params.width, _params.height);
            ComputeLayoutCircularInternal(cp);
            return cp;
        }

        public TaxonCircularInfo CircularInfo = null;
        void ComputeLayoutCircularInternal(CircularParams _cp )
        {
            if (WidthWithChildren == R.Width)
            {
                CircularInfo = _cp.TaxonNextPoint(); 
                return;
            }

            if (Children.Count == 0)
            {
                CircularInfo = null;
                return;
            }

            _cp.TaxonLevelUp();

            int xmin = int.MaxValue;
            int xmax = int.MinValue;
            int ymin = int.MaxValue;
            int ymax = int.MinValue;
            foreach (TaxonTreeNode child in Children)
            {
                child.ComputeLayoutCircularInternal(_cp);
                if (child.CircularInfo.Bounds.Left < xmin) xmin = child.CircularInfo.Bounds.Left;
                if (child.CircularInfo.Bounds.Right > xmax) xmax = child.CircularInfo.Bounds.Right;
                if (child.CircularInfo.Bounds.Top < ymin) ymin = child.CircularInfo.Bounds.Top;
                if (child.CircularInfo.Bounds.Bottom > ymax) ymax = child.CircularInfo.Bounds.Bottom;
            }
            _cp.TaxonLevelDown();

            double firstAngle = Children[0].CircularInfo.Angle;
            double secondAngle = Children[Children.Count - 1].CircularInfo.Angle;

            CircularInfo = _cp.TaxonInternalPoint(firstAngle, secondAngle, _cp.CurrentLevel);
            if (CircularInfo.X < xmin) xmin = (int) CircularInfo.X;
            if (CircularInfo.X > xmax) xmax = (int) CircularInfo.X;
            if (CircularInfo.Y < ymin) ymin = (int) CircularInfo.Y;
            if (CircularInfo.Y > ymax) ymax = (int) CircularInfo.Y;
            CircularInfo.Bounds = Rectangle.FromLTRB(xmin, ymin, xmax, ymax);
        }
    }
}
