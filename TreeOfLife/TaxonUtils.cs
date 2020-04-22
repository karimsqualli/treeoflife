using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace TreeOfLife
{
    public static class TaxonUtils
    {
        //=========================================================================================
        static TaxonUtils()
        {
            RandomInit();
        }

        //=========================================================================================
        // Config file / folder
        //
        static public string GetConfigFilePath() {
            //return "Config";
            return Path.Combine(TOLData.DataFolder(), "Config");
        }
        static public string GetConfigFileName(string _name) {
            //return "Config\\TreeOfLifeConfig_" + _name + ".xml"; 
            return Path.Combine(TOLData.DataFolder(), "Config", "TreeOfLifeConfig_" + _name + ".xml");
        }

        public static Config MyConfig = null;

        //=========================================================================================
        // Log
        //
        static public string GetLogPath()
        {
            string path = Path.Combine( GetTaxonPath(), "Logs");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        //=========================================================================================
        // Temp directory
        //
        static public string GetTempPath()
        {
            string path = Path.Combine(GetTaxonPath(), "Temp");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        static public string GetImageCachePath()
        {
            string path = Path.Combine(GetTempPath(), "ImageCache" );
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        //=========================================================================================
        // Taxon tree
        //
        public static void SetOriginalRoot(TaxonTreeNode _node)
        {
            if (OriginalRoot == _node) return;
            OriginalRoot = _node;
            Locations = TaxonLocations.Load(_OriginalRoot);
            SubRoots = null;
            CurrentFilters.Clear();
            Root = OriginalRoot;
            OnOriginalRootChanged?.Invoke(Root, EventArgs.Empty);
        }
        public static event EventHandler OnOriginalRootChanged;

        private static TaxonTreeNode _OriginalRoot = null;
        public static TaxonTreeNode OriginalRoot
        {
            get => _OriginalRoot;
            private set => _OriginalRoot = value;
        }

        private static TaxonTreeNode _Root = null;
        public static TaxonTreeNode Root
        {
            get => _Root;
            set
            {
                if (_Root == value) return;
                _Root = value;
                Controls.TaxonControlList.SetRoot(_Root);
            }
        }

        public static TaxonLocations Locations = null;

        //=========================================================================================
        // CurrentFilter
        //
        public static TaxonFilters CurrentFilters = new TaxonFilters();

        public static void UpdateCurrentFilters( Action _update )
        {
            TaxonTreeNode nodeToFocus = MainGraph?.Graph.SelectedInOriginal;
            List<TaxonTreeNode> saveSubRoots = null;
            if (SubRoots != null)
                saveSubRoots = SubRoots.Select(n => n.GetOriginal()).ToList();

            // apply expand / collapse state from current root to original
            if (Root != OriginalRoot)
                Root.ParseNode((n) => {
                    if (n.IsExpanded())
                        n.GetOriginal()?.Expand();
                    else
                        n.GetOriginal()?.Collapse();
                });

            // update the filter
            _update();

            // compute filteredRoot
            TaxonTreeNode filteredRoot = CurrentFilters.Evaluate(OriginalRoot);

            // apply expand / collapse state from original to filtered root
            if (filteredRoot != OriginalRoot)
                Root.ParseNode((n) => n.Visible = n.GetOriginal()?.Visible ?? false);

            // restore subroots
            if (CurrentFilters.IsEmpty)
                SubRoots = saveSubRoots;
            else if (saveSubRoots != null)
            {
                SubRoots = new List<TaxonTreeNode>();
                foreach (TaxonTreeNode node in saveSubRoots)
                {
                    TaxonTreeNode filteredNode = node.GetFiltered(filteredRoot);
                    if (filteredNode == null)
                        break;
                    SubRoots.Add(filteredNode);
                }
                if (SubRoots.Count == 0) SubRoots = null;
            }

            // set root
            Root = (SubRoots != null && SubRoots.Count > 0) ? SubRoots[SubRoots.Count - 1] : filteredRoot;
            
            if (nodeToFocus != null)
            {
                RefreshMainGraph();
                TaxonTreeNode nodeToSelect = nodeToFocus.GetFiltered(MainGraph.Root);
                while (nodeToSelect == null)
                {
                    nodeToFocus = nodeToFocus.Father;
                    nodeToSelect = nodeToFocus.GetFiltered(MainGraph.Root);
                }
                SelectTaxon(nodeToFocus);
                GotoTaxon(nodeToFocus);
            }
            else
                Invalidate();

        }

        public static bool MatchCurrentFilters( TaxonTreeNode _node )
        {
            return CurrentFilters.Match(_node);
        }

        //=========================================================================================
        // User filter
        //
        public static TaxonFilterListInFile UserFilter = new TaxonFilterListInFile();

        //=========================================================================================
        // Sub roots
        //
        private static List<TaxonTreeNode> SubRoots = null;

        public static bool CanPushSubRoot(TaxonTreeNode _node)
        {
            if (_node == null) return false;
            if (!_node.IsChildOf(Root)) return false;
            if (SubRoots != null && SubRoots.Count > 0 && !_node.IsChildOf(SubRoots[SubRoots.Count - 1]))
                return false;
            return true;
        }

        public static void PushSubRoot( TaxonTreeNode _node )
        {
            if (_node == null) return;
            if (!_node.IsChildOf(Root)) return;
            if (SubRoots == null)
                SubRoots = new List<TaxonTreeNode>();
            else if (SubRoots.Count > 0 && !_node.IsChildOf(SubRoots[SubRoots.Count - 1]))
                return;
            SubRoots.Add(_node);
            Root = _node;
            GotoTaxon(Root);
        }

        public static void PopSubRoot()
        {
            if (SubRoots == null || SubRoots.Count == 0) return;
            TaxonTreeNode node = SubRoots[SubRoots.Count - 1];
            SubRoots.RemoveAt(SubRoots.Count - 1);
            Root = node;
        }

        public static void CleanSubRoots()
        {
            if (SubRoots != null && SubRoots.Count > 0)
            {
                TaxonTreeNode nodeToFocus = SubRoots[0].GetOriginal();

                if (CurrentFilters.IsEmpty)
                    Root = OriginalRoot;
                else
                    Root = CurrentFilters.Evaluate(TaxonUtils.OriginalRoot);
                TaxonUtils.SelectTaxon(nodeToFocus);
                TaxonUtils.GotoTaxon(nodeToFocus);
            }
            SubRoots = null;
        }

        public static bool HasSubRoots()
        {
            return SubRoots != null && SubRoots.Count > 0;
        }

        //=========================================================================================
        // some helpers on main window
        //
        public static Form1 MainWindow = null;

        public static void Invalidate()
        {
            if (MainWindow != null)
                MainWindow.Invalidate(true);
        }

        public static void LockMainWindow()
        {
            if (MainWindow != null)
                MainWindow.Enabled = false;
        }

        public static void UnlockMainWindow()
        {
            if (MainWindow != null)
                MainWindow.Enabled = true;
        }

        //=========================================================================================
        // some helpers on graph
        //
        //public static TaxonGraphPanel MainGraph = null;
        public static TaxonGraph MainGraph = null;
        public static void RefreshMainGraph()
        {
            MainGraph?.RefreshGraph();
        }

        public static void ShowTaxon(TaxonTreeNode _taxon)
        {
            if (MainGraph == null || _taxon == null) return;
            if (_taxon.Visible) return;
            List<TaxonTreeNode> ascendant = new List<TaxonTreeNode>();
            _taxon.GetAllParents(ascendant, false);
            foreach (TaxonTreeNode parent in ascendant)
                parent.Expand();
            MainGraph?.RefreshGraph();
        }

        public static void GotoTaxon(TaxonTreeNode _taxon, bool _setVisible = true)
        {
            if (_taxon == null) return;
            if (MainGraph != null)
            {
                _taxon = _taxon.GetFiltered(MainGraph.Root);
                if (_taxon == null) return;
                if (_setVisible) ShowTaxon(_taxon);
                MainGraph?.Goto(_taxon);
            }
        }

        public static void MoveTaxonTo( TaxonTreeNode _taxon, Rectangle _to, bool _setVisible = true)
        {
            if (_taxon == null) return;
            if (MainGraph != null)
            {
                _taxon = _taxon.GetFiltered(MainGraph.Root);
                if (_taxon == null) return;
                if (_setVisible) ShowTaxon(_taxon);
                MainGraph?.MoveTo(_taxon, _to);
            }
        }

        public static TaxonTreeNode SelectedTaxon()
        {
            if (MainGraph != null)
                return MainGraph.Selected;
            return null;
        }

        public static void SelectTaxon(TaxonTreeNode _taxon)
        {
            if (_taxon == null) return;
            if (MainGraph != null)
            {
                _taxon = _taxon.GetFiltered(MainGraph.Root);
                if (_taxon == null) return;
                if (!_taxon.Visible) return;
                MainGraph.Selected = _taxon;
            }
        }

        public static void ReselectTaxon()
        {
            if (MainGraph != null) 
                Controls.TaxonControlList.OnReselectTaxon(MainGraph.Selected);
        }

        //=========================================================================================
        // some helpers on tree graph
        //
        //public static TaxonGraph MainTreeGraph = null;

        /*public static void RefreshMainTreeGraph()
        {
            MainTreeGraph?.RefreshGraph();
        }*/

       
        //=========================================================================================
        // Path and filename
        //
        static public bool Exists() { return File.Exists(GetTaxonFileName()); }

        static public string GetTaxonPath() { return MyConfig.TaxonPath; }
        static public string GetTaxonFileName() { return Path.Combine( MyConfig.TaxonPath, MyConfig.TaxonFileName); }

        static public string GetTaxonLocationPath()
        {
            //string path = Path.Combine(GetTaxonPath(), Path.GetFileNameWithoutExtension(MyConfig.TaxonFileName)+"_location" );
            string path = TOLData.LocationPath(Path.GetFileNameWithoutExtension(MyConfig.TaxonFileName));
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }

        //----- Sound
        static public string GetSoundDirectory() { return Path.Combine( MyConfig.TaxonPath, "Sounds"); }
        // static public string GetSoundDirectory() { return Path.Combine(TaxonUtils.Datas.SoundsDataPath(), "Sounds"); }

        static public string GetSoundFullPath(TaxonDesc _taxon) { return GetSoundFullPath(_taxon.SoundName); }
        static public string GetSoundFullPath(TaxonTreeNode _node) { return _node == null ? null : GetSoundFullPath(_node.Desc.SoundName); }

        static public string GetSoundRelativePath(TaxonDesc _taxon) { return GetSoundRelativePath(_taxon.SoundName); }
        static public string GetSoundRelativePath(TaxonTreeNode _node) { return GetSoundRelativePath(_node.Desc.SoundName); }

        static string GetSoundFullPath(string _name) { return GetSoundDirectory() + "\\" + _name; }
        static string GetSoundRelativePath(string _name) { return "Sounds\\" + _name; }
        
        //=========================================================================================
        // Init data
        //
        static public void TaxonDataInit()
        {
            if (!File.Exists(GetTaxonFileName()))
            {
                string filename = ChooseFileToOpen(MyConfig.TaxonFileName);
                if (filename == null)
                    return;
            
                MyConfig.TaxonPath = Path.GetDirectoryName(filename);
                MyConfig.TaxonFileName = Path.GetFileName(filename);
            }
        }

        static public void initCollections()
        {
            TaxonImages.Manager.Path = TOLData.ImageDataPath();
            TaxonComments.Manager.Path = TOLData.CommentDataPath();
        }

        //=========================================================================================
        // Save / Load
        //

        //---------------------------------------------------------------------------------
        public static bool Save(TaxonTreeNode _node = null, bool saveAs = false, bool _setAsTaxonFileName = false)
        {
            if (_node == null) _node = OriginalRoot;
            if (_node == null) return false;

            string filename = GetTaxonFileName();
            if (saveAs)
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    FileName = _node.Desc.RefMultiName.Main,
                    InitialDirectory = GetTaxonPath(),
                    Filter = "xml files (*.xml)|*.xml|tol files (*.tol)|*.tol",
                    AddExtension = true
                };
                if (sfd.ShowDialog() != DialogResult.OK)
                    return false;

                filename = sfd.FileName;
                if (_setAsTaxonFileName)
                {
                    MyConfig.TaxonPath = Path.GetDirectoryName(filename);
                    MyConfig.TaxonFileName = Path.GetFileName(filename);
                }
            }

            Cursor.Current = Cursors.WaitCursor;
            string extension = Path.GetExtension(filename).ToLower();
            if (extension == ".xml")
                _node.SaveXML(filename);
            else if (extension == ".tol")
                _node.SaveBin(filename);
            Cursor.Current = Cursors.Default;
            return true;
        }

        //---------------------------------------------------------------------------------
        public static bool Save(TaxonTreeNode _node, string _filename)
        {
            if (_node == null) return false;

            Directory.CreateDirectory(Path.GetDirectoryName(_filename));
            
            string extension = Path.GetExtension(_filename).ToLower();
            if (extension == ".xml")
                _node.SaveXML(_filename);
            else if (extension == ".tol")
                _node.SaveBin(_filename);
            return true;
        }

        //---------------------------------------------------------------------------------
        public static string ChooseFileToOpen(string _filename)
        {
            int filterIndex = 1;
            if (_filename != null)
            {
                string extension = Path.GetExtension(_filename).ToLower();
                if (extension == ".xml") filterIndex = 2;
                if (extension == ".txt") filterIndex = 3;
            }

            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = TaxonUtils.GetTaxonPath(),
                Filter = "tol files (*.tol)|*.tol",
                Multiselect = false,
                AddExtension = true,
                FileName = _filename
            };
            if (!SystemConfig.IsInUserMode)
                ofd.Filter += "|xml files (*.xml)|*.xml|Newick string / Jac file (*.txt)|*.txt";
            ofd.FilterIndex = filterIndex;

            if (ofd.ShowDialog() != DialogResult.OK) return null;
            if (!File.Exists(ofd.FileName)) return null;
            return ofd.FileName;
        }

        //---------------------------------------------------------------------------------
        public static TaxonTreeNode Load(ref string _filename )
        {
            _filename = ChooseFileToOpen( _filename);
            if (_filename == null) return null;
            MyConfig.TaxonPath = Path.GetDirectoryName(_filename);
            MyConfig.TaxonFileName = Path.GetFileName(_filename);
            TaxonDataInit();
            return TaxonTreeNode.Load(_filename);
        }

        //---------------------------------------------------------------------------------
        public static TaxonTreeNode Import(ref string _filename)
        {
            _filename = ChooseFileToOpen(_filename);
            if (_filename == null) return null;
            return TaxonTreeNode.Load(_filename);
        }

        //=========================================================================================
        // Random
        //
        public static Random random = null;

        //----------------------------------------------------------------
        static void RandomInit()
        {
            random = new Random(DateTime.Now.ToString().GetHashCode());
        }

        //----------------------------------------------------------------
        // renvoie une liste d'entiers aléatoires (on lui rentre la longueur de la liste de départ et celle d'arrivée)
        static List<int> RandomIndex(int _listLength, int _indexNumber)
        {
            List<int> result = new List<int>();
            if (_indexNumber >= _listLength)
            {
                for (int i = 0; i < _listLength; i++)
                    result.Add(i);
            }
            else if (_indexNumber == 1)
            {
                result.Add(random.Next(_listLength));
            }
            else if (_indexNumber > 0) 
            {
                List<int> temp = new List<int>();
                for (int i = 0; i < _listLength; i++)
                    temp.Add(i);
                while (_indexNumber > 0)
                {
                    int tempIndex = random.Next(temp.Count);
                    result.Add(temp[tempIndex]);
                    temp.RemoveAt(tempIndex);
                    _indexNumber--;
                }
            }
            return result;
        }


        //----------------------------------------------------------------
        //renvoie une liste de taxon aléatoires 
        static public List<TaxonTreeNode> RandomTaxon(List<TaxonTreeNode> list, int _indexNumber)
        {
            List<TaxonTreeNode> result = new List<TaxonTreeNode>();

            List<int> listIndex = RandomIndex(list.Count, _indexNumber);

            foreach (int index in listIndex)
                result.Add(list[index]);
            return result;
        }

        //---------------------------------------------------------------
        static public List<TaxonTreeNode> RandomTaxon(TaxonTreeNode _root, int _indexNumber)
        {
            _root.FlagSet();
            List<TaxonTreeNode> result = new List<TaxonTreeNode>();
            for (int i = 0; i < _indexNumber; i++)
            {
                TaxonTreeNode res = RandomTaxon(_root);
                if (res == null) break;
                result.Add(res);
            }
            return result;
        }

        //---------------------------------------------------------------
        static TaxonTreeNode RandomTaxon(TaxonTreeNode _root)
        {
            List<TaxonTreeNode> validChildren = _root.Children.Where(x => x.Flag).ToList();
            while (validChildren.Count > 0)
            {
                int index = random.Next(validChildren.Count);
                TaxonTreeNode choosen = validChildren[index];
                if (choosen.Desc.HasImage)
                {
                    choosen.Flag = false;
                    return choosen;
                }
                
                choosen.Flag = false;
                TaxonTreeNode result = RandomTaxon(choosen);
                if (result != null) return result;

                choosen.Flag = false;
                validChildren.Remove(choosen);
            }
            return null;
        }

        

        //=========================================================================================
        // History
        //
        public static List<TaxonTreeNode> History = new List<TaxonTreeNode>();

        public static void HistoryAdd(TaxonTreeNode _taxon)
        {
            _taxon = _taxon?.GetOriginal();
            if (_taxon == null) return;
            History.Remove(_taxon);
            History.Insert(0, _taxon);
            if (!_LockHistoryParser)
                _HistoryParser = null;
        }

        public class HistoryParser
        {
            public HistoryParser(List<TaxonTreeNode> _history )
            {
                if (_history.Count > 1)
                {
                    _History = new List<TaxonTreeNode>();
                    _History.AddRange(_history);
                    _Index = 0;
                }
            }

            List<TaxonTreeNode> _History;
            int _Index;

            public TaxonTreeNode Previous()
            {
                if (_History == null) return null;
                if (_Index + 1 >= _History.Count) return null;
                return _History[++_Index];
            }

            public TaxonTreeNode Next()
            {
                if (_History == null) return null;
                if (_Index < 1) return null;
                return _History[--_Index];
            }
        }

        static bool _LockHistoryParser = false;
        static HistoryParser _HistoryParser = null;
        public static void ParseHistoryPrevious()
        {
            if (_HistoryParser == null)
                _HistoryParser = new HistoryParser(History);
            TaxonTreeNode node = _HistoryParser.Previous();
            if (node != null)
            {
                _LockHistoryParser = true;
                TaxonUtils.GotoTaxon(node);
                TaxonUtils.SelectTaxon(node);
                _LockHistoryParser = false;
            }
        }

        public static void ParseHistoryNext()
        {
            if (_HistoryParser == null) return;
            TaxonTreeNode node = _HistoryParser.Next();
            if (node != null)
            {
                _LockHistoryParser = true;
                TaxonUtils.GotoTaxon(node);
                TaxonUtils.SelectTaxon(node);
                _LockHistoryParser = false;
            }
        }

        //=========================================================================================
        // Favorites
        //
        public static List<TaxonTreeNode> Favorites = new List<TaxonTreeNode>();

        public static event EventHandler OnFavoritesChanged;

        public static void FavoritesSet(List<TaxonTreeNode> _list)
        {
            if (_list == null) return;
            Favorites.Clear();
            Favorites.AddRange(_list);
            Favorites.Sort(TaxonTreeNode.CompareOnlyName);
            OnFavoritesChanged?.Invoke(Favorites, new EventArgs());
        }

        public static void FavoritesAdd(TaxonTreeNode _taxon)
        {
            _taxon = _taxon?.GetOriginal();
            if (_taxon == null) return;
            Favorites.Remove(_taxon);
            Favorites.Insert(0, _taxon);
            Favorites.Sort(TaxonTreeNode.CompareOnlyName);
            OnFavoritesChanged?.Invoke(Favorites, new EventArgs());
        }

        public static void FavoritesRemove(TaxonTreeNode _taxon)
        {
            _taxon = _taxon?.GetOriginal();
            if (_taxon == null) return;
            Favorites.Remove(_taxon);
            OnFavoritesChanged?.Invoke(Favorites, new EventArgs());
        }

        public static string FavoritesMenuItemText(TaxonTreeNode _taxon)
        {
            _taxon = _taxon?.GetOriginal();
            if (_taxon == null || SystemConfig.IsInUserMode) return null;
            if (Favorites.Contains(_taxon)) return Localization.Manager.Get("_RemoveFavorite", "Remove {0} from favorites", _taxon.Desc.RefMainName); 
            if (!_taxon.Desc.IsUnnamed) return Localization.Manager.Get("_AddFavorite", "Add {0} to favorites", _taxon.Desc.RefMainName);
            return null;
        }

        public static ToolStripMenuItem FavoritesMenuItem(TaxonTreeNode _taxon)
        {
            _taxon = _taxon?.GetOriginal();
            if (_taxon == null || SystemConfig.IsInUserMode) return null;
            ToolStripMenuItem menuItem = null;
            if (Favorites.Contains(_taxon))
                menuItem = new ToolStripMenuItem(Localization.Manager.Get("_RemoveFavorite", "Remove {0} from favorites", _taxon.Desc.RefMainName), null, onRemoveFavorites);
            else if (!_taxon.Desc.IsUnnamed)
                menuItem = new ToolStripMenuItem(Localization.Manager.Get("_AddFavorite", "Add {0} to favorites", _taxon.Desc.RefMainName), null, onAddFavorites);
            else
                return null;
            menuItem.Tag = _taxon;
            return menuItem;
        }

        private static void onRemoveFavorites(object sender, EventArgs e)
        {
            if ((sender is ToolStripMenuItem) && ((sender as ToolStripMenuItem).Tag is TaxonTreeNode))
                FavoritesRemove((sender as ToolStripMenuItem).Tag as TaxonTreeNode);
        }

        private static void onAddFavorites(object sender, EventArgs e)
        {
            if ((sender is ToolStripMenuItem) && ((sender as ToolStripMenuItem).Tag is TaxonTreeNode))
                FavoritesAdd((sender as ToolStripMenuItem).Tag as TaxonTreeNode);
        }
    }
}
