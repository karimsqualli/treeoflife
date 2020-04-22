using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using VinceToolbox;
using TreeOfLife.TaxonDialog;

namespace TreeOfLife
{
    static public class SystemConfig
    {
        public enum RunningModeEnum
        {
            Undefined,
            Debugger,
            Admin,
            User
        }

        static private RunningModeEnum _RunningMode = RunningModeEnum.Undefined;
        static public RunningModeEnum RunningMode
        {
            get { return _RunningMode; }
            set
            {
                if (_RunningMode == value) return;
                if (OnRunningModeChanging != null)
                    OnRunningModeChanging(null, new EventArgs());

                Loggers.WriteInformation(LogTags.Program, "Changing running mode from " + _RunningMode.ToString() + " to " + value.ToString());
                _RunningMode = value;
                
                if (OnRunningModeChanged != null)
                    OnRunningModeChanged(null, new EventArgs());
            }
        }
        static public bool IsInUserMode { get { return _RunningMode == RunningModeEnum.User; } }
        static public bool IsInDebuggerMode { get { return _RunningMode == RunningModeEnum.Debugger; } }

        static public event EventHandler OnRunningModeChanging = null;
        static public event EventHandler OnRunningModeChanged = null;

        
    }

    public class Config
    {
        public Config(string _name = null) { Name = _name; }
        public Config() : this("emptyConstructor") { }

        //=========================================================================================
        public Options Options = new Options();

        //=========================================================================================
        // data
        //
        public string TaxonPath = "";
        public string TaxonRelativePath = "";
        public string TaxonFileName = "";
        public bool dataInitialized = false;

        //=========================================================================================
        // main window placement
        //
        public winFunctions.winPlacement MainWindowPlacement;

        void MainWindowPlacementToUI()
        {
            if (winFunctions.winPlacementIsValid(MainWindowPlacement))
                winFunctions.winSetPlacement(TaxonUtils.MainWindow.Handle, MainWindowPlacement);
        }

        void MainWindowPlacementFromUI()
        {
            MainWindowPlacement = winFunctions.winGetPlacement(TaxonUtils.MainWindow.Handle);
        }

        //=========================================================================================
        // main window content
        //
        GUI.ControlContainerTabsDesc _TaxonTabControlsDescriptor = null;
        public GUI.ControlContainerTabsDesc TaxonTabControlsDescriptor
        {
            get { return _TaxonTabControlsDescriptor; }
            set { _TaxonTabControlsDescriptor = value; }
        }

        GUI.ControlContainerSplitterDesc _SplitterContainerDescriptor = null;
        public GUI.ControlContainerSplitterDesc SplitterContainerDescriptor
        {
            get { return _SplitterContainerDescriptor; }
            set { _SplitterContainerDescriptor = value; }
        }

        //-----------------------------------------------------------------------------------------
        void MainWindowContentToUI()
        {
            Panel mainPanel = TaxonUtils.MainWindow.GetMainPanel();
            if (_SplitterContainerDescriptor != null)
            {
                GUI.ControlContainerSplitter split = _SplitterContainerDescriptor.Rebuild();
                mainPanel.Controls.Add(split);
                _SplitterContainerDescriptor.AfterAdd(split);
            }
            else if (_TaxonTabControlsDescriptor != null)
            {
                GUI.ControlContainerTabs tabControl = _TaxonTabControlsDescriptor.Rebuild();
                mainPanel.Controls.Add(tabControl);
            }
        }

        //-----------------------------------------------------------------------------------------
        void MainWindowContentFromUI()
        {
            TaxonTabControlsDescriptor = null;
            SplitterContainerDescriptor = null;

            Panel mainPanel = TaxonUtils.MainWindow.GetMainPanel();
            if (mainPanel.Controls[0] is GUI.ControlContainerTabs)
                TaxonTabControlsDescriptor = GUI.ControlContainerTabsDesc.FromTaxonTabControls(mainPanel.Controls[0] as GUI.ControlContainerTabs);
            else if (mainPanel.Controls[0] is GUI.ControlContainerSplitter)
                SplitterContainerDescriptor = GUI.ControlContainerSplitterDesc.FromTaxonSplitterContainer(mainPanel.Controls[0] as GUI.ControlContainerSplitter);
        }

        //=========================================================================================
        // about window
        //
        DateTime _DateOfLastNews = DateTime.MinValue;
        public DateTime DateOfLastNews
        {
            get { return _DateOfLastNews; }
            set { _DateOfLastNews = value; }
        }

        //=========================================================================================
        // floating window content
        //
        public List<GUI.FormContainerDesc> FloatingFormsDescriptors = new List<GUI.FormContainerDesc>();

        //-----------------------------------------------------------------------------------------
        void FloatingFormsFromUI()
        {
            List<GUI.FormContainer> list = Controls.TaxonControlList.GetAllFloating();
            FloatingFormsDescriptors.Clear();
            foreach (GUI.FormContainer form in list)
            {
                FloatingFormsDescriptors.Add(GUI.FormContainerDesc.FromFormContainer(form));
            }
        }

        //-----------------------------------------------------------------------------------------
        void FloatingFormsToUI()
        {
            foreach (GUI.FormContainerDesc formDesc in FloatingFormsDescriptors)
            {
                formDesc.Rebuild();
            }
        }

        //=========================================================================================
        // Favorites
        //
        List<string> _Favorites = new List<string>();
        public List<string> Favorites
        {
            get { return _Favorites; }
            set { _Favorites = value; }
        }

        //-----------------------------------------------------------------------------------------
        void FavoritesToUI()
        {
            TaxonTreeNode root = TaxonUtils.OriginalRoot;
            if (root == null) return;

            List<TaxonTreeNode> favFound = new List<TaxonTreeNode>();
            foreach (string fav in Favorites)
            {
                TaxonTreeNode taxon = root.FindTaxonByFullName(fav);
                if (taxon != null) favFound.Add(taxon);
            }
            TaxonUtils.FavoritesSet(favFound);
        }

        //-----------------------------------------------------------------------------------------
        void FavoritesFromUI()
        {
            Favorites.Clear();
            foreach (TaxonTreeNode taxon in TaxonUtils.Favorites)
                Favorites.Add(taxon.GetHierarchicalName());
        }

        //=========================================================================================
        // collection window options
        //
        public winFunctions.winPlacement CollectionWindowPlacement;

        //=========================================================================================
        // collection window options
        //
        public GenerateNewDatabaseConfig GenerateNewDatabaseConfig;

        //=========================================================================================
        // configto UI and UI to config functions
        //

        //-----------------------------------------------------------------------------------------
        public void ToData()
        {
            TaxonUtils.TaxonDataInit();

        }

        //-----------------------------------------------------------------------------------------
        public void ToUI()
        {
            FavoritesToUI();
            MainWindowPlacementToUI();
            MainWindowContentToUI();
            FloatingFormsToUI();

            // TODO bof code
            Controls.TaxonListBox.BrushSelection = Options.FullTreeColor.SelectedBrush;
            Controls.TaxonListBox.BrushHover = Options.FullTreeColor.HoverBrush;
            TaxonComments.Manager.OnConfigLoaded();
        }

        //-----------------------------------------------------------------------------------------
        public void FromUI()
        {
            FavoritesFromUI();
            MainWindowPlacementFromUI();
            MainWindowContentFromUI();
            FloatingFormsFromUI();
        }

        //=========================================================================================
        // File functions : save / load
        //

        [XmlIgnore]
        public string Name { get; set; }

        //-----------------------------------------------------------------------------------------
        //méthode Save        
        public void Save()
        {
            Save(TaxonUtils.GetConfigFileName(Name));
        }

        public void Save( string _filename, string _exeRef = null)
        {
            BeforeSave(_exeRef);
            string filepath = Path.GetDirectoryName(_filename);
            try
            {
                if (!Directory.Exists(filepath)) Directory.CreateDirectory(filepath);

                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                using (TextWriter writer = new StreamWriter(_filename))
                {
                    serializer.Serialize(writer, this);
                }
            }
            catch (Exception e)
            {
                Loggers.WriteError(LogTags.Congif, "Exception while saving config file : \n    " + _filename + "\n" + e.Message);
            }
        }

        //-----------------------------------------------------------------------------------------
        //méthode load
        public static Config Load(string _name)
        {
            string filename = TaxonUtils.GetConfigFileName(_name);
            if (File.Exists(filename))
            {
                try
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(Config));
                    TextReader reader = new StreamReader(filename);
                    object obj = deserializer.Deserialize(reader);
                    reader.Close();
                    (obj as Config).Name = _name;
                    (obj as Config).AfterLoad();
                    return obj as Config;
                }
                catch (Exception e)
                {
                    Loggers.WriteError(LogTags.Congif, "Exception while loading config file : \n    " + filename + "\n" + e.Message);
                }
            }
            Config result = new Config(_name);
            result.AfterLoad();
            return result;
        }

        //-----------------------------------------------------------------------------------------
        public void BeforeSave( string _exeRef = null )
        {
            TaxonRelativePath = "";
            if (TaxonPath != "")
            {
                try
                {
                    if (_exeRef == null) _exeRef = exeFunctions.exePath();
                    string from = _exeRef.ToLower().Replace('/', '\\').Trim('\\');
                    string to = TaxonPath.ToLower().Replace('/', '\\').Trim('\\');

                    // not in same HD, no relative path to compute
                    if (from[0] != to[0])
                        return;

                    string[] fromParts = from.Split('\\');
                    string[] toParts = to.Split('\\');
                    if (fromParts.Length == 0 || toParts.Length == 0) return;

                    int i;
                    int length = 0;
                    for (i = 0; i < fromParts.Length && i < toParts.Length; i++)
                    {
                        if (fromParts[i] == toParts[i])
                            length += toParts[i].Length + 1;
                        else
                            break;
                    }

                    string result = "";
                    while (i < fromParts.Length)
                    {
                        result += "..\\";
                        i++;
                    }
                    //result += string.Join("\\", toParts.ToList().GetRange(i, toParts.Length - i));
                    result += TaxonPath.Substring(length);
                    TaxonRelativePath = result;
                }
                catch { }
            }
        }

        //-----------------------------------------------------------------------------------------
        public void AfterLoad()
        {
            TaxonPath = TaxonPath.TrimEnd('\\');
            TaxonRelativePath = TaxonRelativePath.TrimEnd('\\');

            if (TaxonRelativePath != "")
            {
                try
                {
                    string path = Path.Combine( exeFunctions.exePath(), TaxonRelativePath);
                    if (Directory.Exists(path))
                    {
                        DirectoryInfo dir = new DirectoryInfo(path);
                        path = dir.FullName;
                    }
                    TaxonPath = path;
                }
                catch { }
            }
            Options.AfterLoad();
        }

    }
}
