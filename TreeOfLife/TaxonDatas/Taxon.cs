using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;

namespace TreeOfLife
{
    public class FilteredTaxon
    {
        public Taxon original;
        public FilteredTaxon Father;
        public List<FilteredTaxon> Children = null;
    }

    public class Taxon : IComparable
    {
        //---------------------------------------------------------------------------------
        //définit le nom du membre
        string _Name = "";
        [XmlAttribute]
        public string Name
        {
            get { return _Name; }
            set 
            {
                _Name = value;      
                _DisplayName = _Name.Replace('_', ' ');
            }
        }
        //---------------------------------------------------------------------------------
        [XmlIgnore]
        string _DisplayName;
        public string DisplayName { get { return _DisplayName; } }

        //---------------------------------------------------------------------------------
        //ça c'est un accesseur, mon jaco. ça permet d'utiliser "FrenchName" comme une variable à l'extérieur alors qu'en douce, ça appelle une fonction.
        //définit le nom du taxon en français
        string _FrenchName = "";
        [XmlAttribute]
        public string FrenchName
        {
            get { return _FrenchName; }
            set { _FrenchName = value; }
        }

        //---------------------------------------------------------------------------------
        ClassicRankEnum _ClassicRank = ClassicRankEnum.None;
        [XmlAttribute]
        public ClassicRankEnum ClassicRank
        {
            get { return _ClassicRank; }
            set { _ClassicRank = value; }
        }

        //---------------------------------------------------------------------------------
        public override string ToString() { return "Taxon: " + Name; }

        //---------------------------------------------------------------------------------
        bool _HasImage = false;
        [XmlAttribute]
        public bool HasImage
        {
            get { return _HasImage; }
            set { _HasImage = value; }
        }

        //---------------------------------------------------------------------------------
        public string getFullName()
        {
            string fullName = Name;
            
            Taxon current = _Father;
            while (current != null)
            {
                fullName = current.Name + "|" + fullName;
                current = current._Father;
            }
            return fullName;
        }
        
        //---------------------------------------------------------------------------------
        Taxon _Father = null;
        [XmlIgnore] //ligne suivante ignorée lors de la génération du fichier Xml
        public Taxon Father
        {
            get { return _Father; }
            set { _Father = value; }
        }

        //crée la liste (vide) des enfants
        List<Taxon> _Children = new List<Taxon>();
        public List<Taxon> Children
        {
            get { return _Children; }
            set { _Children = value; }
        }

        [XmlIgnore]
        public bool HasChildren { get { return _Children.Count != 0; } }

        [XmlIgnore]
        public List<Taxon> VisibleChildren
        {
            get
            {
                List<Taxon> visibleChild = new List<Taxon>();
                foreach (Taxon child in Children)
                    if (child.Visible) visibleChild.Add(child);
                return visibleChild;
            }
        }

        [XmlIgnore]
        public bool HasHiddenChildren
        {
            get
            {
                foreach (Taxon child in Children)
                    if (!child.Visible) return true;
                return false;
            }
        }

        [XmlIgnore]
        public bool HasChildVisible
        {
            get
            {
                foreach (Taxon child in Children)
                    if (child.Visible) return true;
                return false;
            }
        }

        [XmlIgnore]
        public bool HasAllChildVisible { get { return !HasHiddenChildren; } }

        //=========================================================================================
        // convert to new data
        //
        public TaxonTreeNode ConvertToDescAndNodeTree()
        {
            TaxonDesc desc = new TaxonDesc(Name);
            desc.ClassicRank = ClassicRank;
            desc.FrenchName = FrenchName;
            desc.HasImage = HasImage;

            TaxonTreeNode node = new TaxonTreeNode(desc);
            node.Visible = Visible;
            
            foreach (Taxon child in Children)
            {
                TaxonTreeNode nodeChild = child.ConvertToDescAndNodeTree();
                nodeChild.Father = node;
                node.Children.Add(nodeChild);
            }

            return node;
        }

        //=========================================================================================
        // Constructors
        //

        //constructeur (méthode particulière appelée quand tu crées une nouvelle instance de la classe (i.e new taxon)
        //(met en majuscules)
        public Taxon(string _name)
        {
            Name = _name.Substring(0, 1).ToUpper() + _name.Substring(1);
        }

        //méthode sans attributs ( je sais plus pourquoi, il en faut une , pour les save/load )
        // presque exact : c'est pour la sérialisation XML utilisée pour les save/load
        //aaah d'accord ...et sinon ça va toi ?
        // oui ca va bien, et toi, les enfants tout ca tout ca ....
        public Taxon() { }

        //=========================================================================================
        // comparison interface
        //
        public int CompareTo(object obj)
        {
            if (!(obj is Taxon)) return 0;
            return DisplayName.CompareTo((obj as Taxon).DisplayName);
        }

        //---------------------------------------------------------------------------------
        public static int CompareTaxon(Taxon t1, Taxon t2)
        {
            if (t1 == t2) return 0;

            if (t1.Father == t2.Father)
            {
                return (t1.Father.Children.IndexOf(t1) < t2.Father.Children.IndexOf(t2)) ? -1 : 1;
            }

            int level1 = t1.getGeneration();
            int level2 = t2.getGeneration();

            if (level1 < level2)
                return CompareTaxon(t1, t2.Father);
            if (level2 < level1)
                return CompareTaxon(t1.Father, t2);
            return CompareTaxon(t1.Father, t2.Father);
        }

        //=========================================================================================
        // Parent functions
        //
        //------------------------------------------
        // mets à jour de manière récursive tous les papas
        public void updateFather()
        {
            foreach (Taxon child in _Children)
            {
                child.Father = this;
                child.updateFather();
            }
        }

        //méthode pour retourner la liste des parents 

        public void getAllParents(List<Taxon> _list, bool _includeFirst = true, bool _fromFatherToChild = true )
        {
            Taxon current = this;
            if (!_includeFirst) current = current.Father;
            while (current != null)
            {
                _list.Add(current);
                current = current._Father;
            }
            if (_fromFatherToChild)
                _list.Reverse();
        }

        //=========================================================================================
        // Count functions
        //
        
        //-----------------------------------------
        public int Count()
        {
            int count = 1;
            foreach (Taxon child in _Children)
                count += child.Count();
            return count;
        }

        //-----------------------------------------
        public int Count( ClassicRankEnum _classikRank )
        {
            int count = ClassicRank == _classikRank ? 1 : 0;
            foreach (Taxon child in _Children)
                count += child.Count(_classikRank);
            return count;
        }

        //=========================================================================================
        // Children functions
        //

        //------------------------------------------
        public Taxon FindTaxonByName( string _name )
        {
            if (Name.ToLower() == _name) return this;
            foreach (Taxon child in _Children)
            {
                Taxon result = child.FindTaxonByName(_name);
                if (result != null) return result;
            }
            return null;
        }

        //------------------------------------------
        public Taxon FindTaxonByFullName(string _fullname)
        {
            if (_fullname == null) return null;
            string[] separate = _fullname.ToLower().Split( new char[] { '|' } );
            if (separate == null || separate.Count() < 1) return null;

            if (separate[0] != Name.ToLower()) return null;
            if (separate.Count() == 1) return this;

            Taxon current = this;
            int index = 1;
            while (index < separate.Count())
            {
                bool found = false;
                foreach (Taxon child in current._Children)
                {
                    if (child.Name.ToLower() == separate[index])
                    {
                        index++;
                        if (index == separate.Count()) return current;
                        current = child;
                        found = true;
                        break;
                    }
                }
                if (!found) return null;
            }
            return null;
        }

        //------------------------------------------
        //méthode qui rajoute un taxon à la liste des enfants (_children)
        public void AddChild(Taxon _child)
        {
            _child.Father = this;
            _Children.Add(_child);
        }

        //------------------------------------------
        //?? différence avec la précédente ??
        // la précédente ajoute un taxon déja créé, alors que celle la prend un nom, construit un nouveau taxon et le rajoute 
        public Taxon AddChild(string _nameChild)
        {
            Taxon child = new Taxon(_nameChild);
            child.Father = this;
            _Children.Add(child);
            return child;
        }

        //------------------------------------------
        // recupère tous les fils quelquesoit leur génération
        public void getAllChildrenRecursively(List<Taxon> _allChildren)
        {
            foreach (Taxon child in _Children)
            {
                _allChildren.Add(child);
                child.getAllChildrenRecursively(_allChildren);
            }
        }

        //------------------------------------------
        // recupère tous les fils quelquesoit leur génération
        public void getAllChildrenRecursively(List<Taxon> _allChildren, ClassicRankEnum _classikRank)
        {
            if (ClassicRank == _classikRank)
                _allChildren.Add(this);

            foreach (Taxon child in _Children)
                child.getAllChildrenRecursively(_allChildren, _classikRank);
        }

        //------------------------------------------
        // recupère tous les fils quelle que soit leur génération mais qui n'ont plus de descendants
        public void getAllLastChildrenRecursively(List<Taxon> _allLastChildren)
        {
            if (_Children.Count == 0)
                _allLastChildren.Add(this);
            else
            {
                foreach (Taxon child in _Children)
                    child.getAllLastChildrenRecursively(_allLastChildren);
            }
        }

        //------------------------------------------
        // recupère tous les fils quelquesoit leur génération mais avec une image
        public void getAllChildrenWithImageRecursively(List<Taxon> _allChildren)
        {
            if (HasImage)
                _allChildren.Add(this);

            foreach (Taxon child in _Children)
                child.getAllChildrenWithImageRecursively(_allChildren);
        }

        //------------------------------------------
        // méthode to get all descendant level x    >>>>>>>>>>>>>>>>>   j'ai compris !!!!!! :):):):)
        public void getDescendentLevel(int _level, List<Taxon> _list)
        {
            if (_level == 1)
                _list.Add(this);
            else
            {
                foreach (Taxon child in _Children)
                {
                    child.getDescendentLevel(_level - 1, _list);
                }
            }
        }

        //=========================================================================================
        // Generation functions
        //

        // méthode pour retourner la génération d'un taxon
        public int getGeneration()
        {
            int level = 0;
            Taxon current = this;
            while (current != null)
            {
                level++;
                current = current._Father;
            }
            return level;
        }

        // nombre max de génération
        // vite fait, pas vérifiée si elle marche correctement
        public int getMaxDescendentGeneration()
        {
            if (!HasChildren) return 0;

            int maxLevel = 0;
            foreach (Taxon child in _Children)
            {
                int level = child.getMaxDescendentGeneration() + 1;
                if (level > maxLevel) maxLevel = level;
            }
            return maxLevel;
        }

        // retourne la dernière gen avec tous les taxon visible
        private int getLastFullVisibleGeneration(List<Taxon> _taxons)
        {
            foreach (Taxon t in _taxons)
            {
                if (t.HasChildren && !t.HasAllChildVisible)
                    return t.getGeneration() - getGeneration();
            }

            List<Taxon> _child = new List<Taxon>();
            foreach (Taxon t in _taxons)
                _child.AddRange(t.Children);

            if (_child.Count == 0)
                return _taxons[0].getGeneration() - getGeneration();

            return getLastFullVisibleGeneration(_child);
        }

        public int getLastFullVisibleGeneration()
        {
            List<Taxon> list = new List<Taxon>();
            list.Add(this);
            return getLastFullVisibleGeneration(list);
        }




        //=========================================================================================
        // expand/collapse functions
        //

        // expand : tous les fils directs deviennent visibles
        public void expand()
        {
            foreach (Taxon child in Children)
                child.Visible = true;
        }

        // expand extended : expand tous les taxons de la génération la plus proche non totalement visible
        public void expandExtended()
        {
            // cas spécial, si il est pas expanded, just expand it
            if (!HasAllChildVisible)
            {
                expand();
                return;
            }

            // parcours tous les enfants pour voir si y'en a au moins un qui n'est pas expanded
            foreach (Taxon child in Children)
            {
                if (!child.HasAllChildVisible)
                {
                    foreach (Taxon child2 in Children)
                        child2.expand();
                    return;
                }
            }

            // si on est la c'est que tout les enfants sont expanded => passe récursivement à la 
            foreach (Taxon child in Children)
            {
                child.expandExtended();
            }
        }

        // rend tout visible
        public void expandAll()
        {
            Visible = true;
            foreach (Taxon child in Children)
                child.expandAll();
        }

        // cache tous les fils
        public void collapse()
        {
            foreach (Taxon child in Children)
                child.Visible = false;
        }

        // cache tous les fils de la denière génération visible (i.e. qui a au moins un taxon visible)
        private void collapseExtended(List<Taxon> _toCollapse)
        {
            List<Taxon> visibleChildren = new List<Taxon>();
            foreach (Taxon t in _toCollapse)
                visibleChildren.AddRange(t.VisibleChildren);

            if (visibleChildren.Count == 0)
            {
                foreach (Taxon t in _toCollapse)
                    t.Visible = false;
                return;
            }

            collapseExtended(visibleChildren);
        }

        public void collapseExtended()
        {
            if (!HasChildren) return;
            if (!HasChildVisible) return;
            List<Taxon> visibleChildren = new List<Taxon>(VisibleChildren);
            collapseExtended(visibleChildren);
        }

        // collapse tout
        public void collapseAll()
        {
            foreach (Taxon child in Children)
            {
                child.Visible = false;
                child.collapseAll();
            }
        }


        //=========================================================================================
        // File functions : save / load
        //

        //méthode Save        
        public void Save(string _fileName)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Taxon));
                using (TextWriter writer = new StreamWriter(_fileName))
                {
                    serializer.Serialize(writer, this);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception while saving " + _fileName + "\n" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //méthode load
        public static Taxon Load(string _fileName)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Taxon));
                TextReader reader = new StreamReader(_fileName);
                object obj = deserializer.Deserialize(reader);
                reader.Close();
                (obj as Taxon).updateFather();
                (obj as Taxon).Visible = true;
                return obj as Taxon;
            }
            catch (Exception e)
            {
                string message = "Exception while loading " + _fileName + " : \n\n";
                message += e.Message;
                if (e.InnerException != null)
                    message += "\n" + e.InnerException.Message;

                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        // méthode pour créer un exemple simple de taxon
        // on va pouvoir le virer ce truc la, qu'en penses tu ?
        public static void CreateSimpleSample(string _fileName)
        {
            Taxon Taxon01_ = new Taxon("êtres vivants");
            Taxon Taxon01_01 = Taxon01_.AddChild("végétaux");
            Taxon Taxon01_01_01 = Taxon01_01.AddChild("fabacées");
            Taxon01_01_01.AddChild("trèfle blanc");
            Taxon01_01_01.AddChild("luzerne");
            Taxon Taxon01_01_02 = Taxon01_01.AddChild("orchidacées");
            Taxon01_01_02.AddChild("orchis singe");
            Taxon01_01_02.AddChild("orchis bouc");
            Taxon Taxon01_02 = Taxon01_.AddChild("animaux");
            Taxon Taxon01_02_01 = Taxon01_02.AddChild("oiseaux");
            Taxon01_02_01.AddChild("jaco");
            Taxon01_02_01.AddChild("Ara chloroptère");
            Taxon Taxon01_02_02 = Taxon01_02.AddChild("mammifères");
            Taxon Taxon01_02_02_01 = Taxon01_02_02.AddChild("primates");
            Taxon01_02_02_01.AddChild("Bonobo");
            Taxon01_02_02_01.AddChild("Gorille");
            Taxon01_02_02.AddChild("Ours blanc");
            Taxon Taxon01_02_03 = Taxon01_02.AddChild("reptiles");
            Taxon01_02_03.AddChild("Varan de Komodo");
            Taxon01_02_03.AddChild("orvet");

            Taxon01_.Save("c:/taxons.xml");
        }

        //=========================================================================================
        // Filter part
        //
        public enum FilterResult
        {
            No,
            Yes,
            YesAndChildToo
        }

        public delegate FilterResult filter(Taxon _taxon);

        public FilterResult filterNonNommé(Taxon _taxon)
        {
            return (_taxon.Name == "non nommé") ? FilterResult.Yes : FilterResult.No;
        }

        public FilteredTaxon BuildFiltered(filter _filter, FilteredTaxon _father )
        {
            FilterResult result = _filter(this);
            
            if (result == FilterResult.YesAndChildToo)
                return null;

            if (result == FilterResult.No)
            {
                FilteredTaxon ft = new FilteredTaxon();
                ft.original = this;
                ft.Father = _father;
                _father = ft;
            }

            foreach (Taxon child in Children)
            {
                FilteredTaxon ftChild = BuildFiltered(_filter, _father);
                if (ftChild == null) continue;
                if (_father.Children == null)
                    _father.Children = new List<FilteredTaxon>();
                _father.Children.Add(ftChild);
            }

            return _father;
        }

        //=========================================================================================
        // GRAPH PART
        //

        // taille d'affichage d'un taxon simple
        public static int RectWidth = 100;
        public static int RectHeight = 30;

        // R : coordonnées d'affichage du taxon (sur sa colonne)
        [XmlIgnore]
        public System.Drawing.Rectangle R;
        // WithChildren : taille du rectangle englobant le taxon et tout ses fils
        [XmlIgnore]
        public int WidthWithChildren, HeightWithChildren;

        // To affiche or not to affiche ?
        [XmlIgnore]
        public bool Visible = false;

        // retourne le rectangle avec les petiots (englobant)
        public System.Drawing.Rectangle RectangleWithChildren
        {
            get
            {
                System.Drawing.Rectangle RWC = R;
                RWC.Width = WidthWithChildren;
                return RWC;
            }
        }

        // pour les taxon a la fin (species), calcule la taille du texte
        [XmlIgnore]
        public System.Drawing.Size TextSize;

        // recalcul toutes les positions récursivement
        public void computelayout(int x0, int y0)
        {
            R.X = x0;
            R.Y = y0;

            if (!Visible)
            {
                R.Width = R.Height = WidthWithChildren = HeightWithChildren = 0;
                return;
            }

            WidthWithChildren = R.Width = RectWidth;
            HeightWithChildren = R.Height = RectHeight;
            if (Children.Count == 0) return;

            int ychild = y0;
            int widthmax = 0;
            foreach (Taxon child in Children)
            {
                child.computelayout(x0 + R.Width, ychild);
                ychild += child.HeightWithChildren;
                if (child.WidthWithChildren > widthmax) widthmax = child.WidthWithChildren;
            }

            WidthWithChildren += widthmax;
            if (ychild - y0 > HeightWithChildren) HeightWithChildren = ychild - y0;
            R.Height = HeightWithChildren;
        }

    }
}
