using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TreeOfLife.Controls;
using TreeOfLife.GUI;
using TreeOfLife.Localization;

namespace TreeOfLife
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        private bool quit = false;
        //---------------------------------------------------------------------------------
        // debut de database
        //Database.SQL DataBase = new Database.SQL();

        //---------------------------------------------------------------------------------
        public Form1(string[] args)
        {
            //----- config
            FormAbout.SetSplashScreenMessage(".. Loading config ...");
            TaxonUtils.MyConfig = Config.Load("auto");
            //TaxonUtils.MyConfig.ToData();

            if (! TaxonUtils.MyConfig.dataInitialized)
            {
                quit = ! TOLData.Init();

                if (quit)
                {
                    Shown += (sender, e) => CloseOnStart();
                    return;
                }
            } else
            {
                TOLData.offline = TaxonUtils.MyConfig.offline;
                TOLData.rootDirectory = TaxonUtils.MyConfig.rootDirectory;
            }

            TOLData.initSounds();
            TaxonUtils.initCollections();

            //----- tip manager
            TipManager.Start();

            //----- menu
            InitializeComponent();
            ApplyTheme();
            UpdateUI();

            //----- args
            if (args.Length > 0 && File.Exists(args[0]))
            {
                FileInfo fi = new FileInfo(args[0]);
                TaxonUtils.MyConfig.TaxonPath = fi.Directory.FullName;
                TaxonUtils.MyConfig.TaxonFileName = fi.Name;
            }

            //----- load
            DateTime startLoad = DateTime.Now;
            TaxonTreeNode loadedNode = null;
            if (TaxonUtils.Exists())
                loadedNode = TaxonTreeNode.Load(TaxonUtils.GetTaxonFileName());
            if (loadedNode == null)
            {
                if (!TaxonTreeNode.LoadHasBeenCanceled() && ! TaxonUtils.emptyTreeAtStartup)
                {
                    Loggers.WriteError(LogTags.Data, "Cannot open taxon file data : \n\n    " + TaxonUtils.GetTaxonFileName());
                }
            }
            else
            {
                string message = "open " + TaxonUtils.GetTaxonFileName() + " successful";
                message += "\n    " + loadedNode.Count() + " taxon loaded";
                message += "\n    " + loadedNode.Count(ClassicRankEnum.Espece) + " " + VinceToolbox.Helpers.enumHelper.GetEnumDescription(ClassicRankEnum.Espece);
                message += "," + loadedNode.Count(ClassicRankEnum.SousEspece) + " " + VinceToolbox.Helpers.enumHelper.GetEnumDescription(ClassicRankEnum.SousEspece);
                Loggers.WriteInformation(LogTags.Data, message);
            }

            FormAbout.SetSplashScreenMessage(".. End initialization ...");
            TaxonUtils.SetOriginalRoot(loadedNode);
            TaxonUtils.MainWindow = this;

            DateTime endLoad = DateTime.Now;

            TaxonControlList.OnRegisterTaxonControl += TaxonControlList_OnRegisterTaxonControl;
            TaxonControlList.OnInitTaxonControlAfterLoad += TaxonControlList_OnInitTaxonControlAfterLoad;
            TaxonControlList.OnUnregisterTaxonControl += TaxonControlList_OnUnregisterTaxonControl;
            SystemConfig.OnRunningModeChanged += SystemConfig_OnRunningModeChanged;
            SystemConfig_OnRunningModeChanged(null, EventArgs.Empty);

            TaxonUtils.MyConfig.ToUI();
            taxonGraph_AddOneIfNone();

            Loggers.WriteInformation(LogTags.Data, "Total loading time: " + (int)((endLoad - startLoad).TotalMilliseconds));
        }

        private void CloseOnStart()
        {
            Close();
            Application.Exit();
        }

        //---------------------------------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            Localization.Manager.DoMenu(menuMain);
            Localization.Manager.OnCurrentLanguageChanged += OnCurrentLanguageChanged;

            FormAbout.EndSplashScreen();
        }

        //---------------------------------------------------------------------------------
        private void OnCurrentLanguageChanged( object sender, EventArgs e )
        {
            Localization.Manager.DoMenu(menuMain);
            Invalidate(true);
        }   

        //---------------------------------------------------------------------------------
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DataAreDirty)
            {
                DialogResult result = MessageBox.Show("Something has changed, really want to quit without saving ?", "Quit ?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            TaxonUtils.MyConfig.FromUI();
            TaxonUtils.MyConfig.Save();

            TipManager.Stop();
            TaxonComments.Manager.Stop();
            Localization.Manager.StopWatcher();
            Localization.Manager.Save();
        }

        //---------------------------------------------------------------------------------
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SystemConfig.OnRunningModeChanged -= SystemConfig_OnRunningModeChanged;

            TaxonControlList.OnRegisterTaxonControl -= TaxonControlList_OnRegisterTaxonControl;
            TaxonControlList.OnInitTaxonControlAfterLoad -= TaxonControlList_OnInitTaxonControlAfterLoad;
            TaxonControlList.OnUnregisterTaxonControl -= TaxonControlList_OnUnregisterTaxonControl;

            Localization.Manager.OnCurrentLanguageChanged -= OnCurrentLanguageChanged;
        }

        //---------------------------------------------------------------------------------
        public Panel GetMainPanel() { return panel1; }

        //---------------------------------------------------------------------------------
        void UpdateUI()
        {
            Text = "Tree Of Life";
#if USER
            Text += " Viewer";
#endif
            Text += (DataAreDirty ? " (*)" : "" );
        }

        
        //---------------------------------------------------------------------------------
        void ApplyTheme()
        {
            Theme.Current.Menu_ApplyTheme(menuMain);
        }



        //====================================================================================
        // Running mode changing
        //

        //---------------------------------------------------------------------------------
        private void SystemConfig_OnRunningModeChanged(object sender, EventArgs e)
        {
            bool userMode = SystemConfig.IsInUserMode;
            

            // file / load menu
            fileToolStripMenuItem.Visible = true;
            userModeLoadToolStripMenuItem.Visible = false;

            // database menu
            databaseToolStripMenuItem.Visible = false;

            // role menu
#if USER
            roleToolStripMenuItem.Visible = false;
#endif

            // tool menu
            List<ToolStripItem> toRemove = new List<ToolStripItem>();
            foreach (ToolStripItem item in menuMain.Items)
            {
                if (item is ToolStripMenuItem && item.Tag is string && item.Tag as string == "FromFactoryOfTool")
                    toRemove.Add(item);
            }
            foreach (ToolStripItem item in toRemove)
                menuMain.Items.Remove(item);

            ToolStripMenuItem menuToAdd = TreeOfLife.Tools.FactoryOfTool.BuildMenus(userMode);

            int index = menuMain.Items.IndexOf(aboutToolStripMenuItem);
            while (menuToAdd.DropDownItems.Count > 0)
            {
                menuToAdd.DropDownItems[0].Tag = "FromFactoryOfTool";
                if (index != -1)
                    menuMain.Items.Insert(index, menuToAdd.DropDownItems[0]);
                else
                    menuMain.Items.Add(menuToAdd.DropDownItems[0]);
            }

            // close unauthorized window
            List<TaxonControl> toClose = new List<TaxonControl>();

            if (userMode)
            {
                toClose.Add(TaxonControlList.FindTaxonControl<TaxonOptions>());
                toClose.Add(TaxonControlList.FindTaxonControl<TaxonFavorites>());
                toClose.Add(TaxonControlList.FindTaxonControl<TaxonEditInfo>());
                toClose.Add(TaxonControlList.FindTaxonControl<TaxonHistory>());
                toClose.Add(TaxonControlList.FindTaxonControl<TaxonAscendants>());
            }

            if (SystemConfig.RunningMode != SystemConfig.RunningModeEnum.Debugger)
                toClose.Add(TaxonControlList.FindTaxonControl<TaxonDebugInfo>());

            foreach( TaxonControl control in toClose)
            {
                if (control == null) continue;
                control.OwnerContainer.Remove(control, true);
                TaxonControlList.UnregisterTaxonControl(control);
            }
        }

        //====================================================================================
        // Misc functions
        //

        //---------------------------------------------------------------------------------
        public void Save()
        {
            if (TaxonUtils.Save())
            {
                DataAreDirty = false;
                UpdateUI();
            }
        }

        //====================================================================================
        // File menus event
        //

        //---------------------------------------------------------------------------------
        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            bool userMode = SystemConfig.IsInUserMode;
            saveToolStripMenuItem.Visible = true;
            saveAsToolStripMenuItem.Visible = true;
            importOpenTreeOToolStripMenuItem.Visible = !userMode;
            fileSeparator1MenuItem.Visible = !userMode;
            createNewTreeToolStripMenuItem.Visible = !userMode;
        }

        //---------------------------------------------------------------------------------
        public void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = TaxonUtils.GetTaxonFileName();
            TaxonTreeNode node = TaxonUtils.Load(ref filename);
            if (node == null) return;
            TaxonUtils.MyConfig.TaxonPath = Path.GetDirectoryName(filename);
            TaxonUtils.MyConfig.TaxonFileName = Path.GetFileName(filename);
            TaxonUtils.SetOriginalRoot(node);
            TaxonUtils.Invalidate();
            TaxonUtils.GotoTaxon(node);
        }

        //---------------------------------------------------------------------------------
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        //---------------------------------------------------------------------------------
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TaxonUtils.Save(null, true, true))
            {
                DataAreDirty = false;
                UpdateUI();
            }
        }

        //---------------------------------------------------------------------------------
        private void createNewTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaxonTreeNode root = new TaxonTreeNode(new TaxonDesc("root"));
            TaxonUtils.Save(root, true, true);
            TaxonUtils.SetOriginalRoot(root);
        }

        //=========================================================================================
        // File/OTT menu event
        //

        //---------------------------------------------------------------------------------
        private void whatIsThatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/OpenTreeOfLife/reference-taxonomy/wiki/Interim-taxonomy-file-format");
        }

        //---------------------------------------------------------------------------------
        private void oTTDepotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://files.opentreeoflife.org/ott/");
        }

        //---------------------------------------------------------------------------------
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            TaxonTreeNode node = TaxonTreeNode.ImportOTT(fbd.SelectedPath);
            if (node == null) return;

            if (TaxonUtils.Save(node, true, true))
                TaxonUtils.SetOriginalRoot( node);
        }

        //=========================================================================================
        // Display menu event
        //

        //---------------------------------------------------------------------------------
        private void languageToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            languageToolStripMenuItem.DropDownItems.Clear();
            List<Language> list = Localization.Manager.Languages.GetAvailableLanguages();
            foreach( Language lang in list )
            {
                ToolStripMenuItem item = new ToolStripMenuItem(string.Format("{0} ({1})", lang.Name, lang.Iso), null, menuLanguage_Click)
                {
                    Checked = lang.Iso == Localization.Manager.CurrentLanguage,
                    Tag = lang.Iso
                };
                languageToolStripMenuItem.DropDownItems.Add(item);
            }
            languageToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
            languageToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem(Localization.Manager.Get("_Save", "Save"), null, menuSaveLanguage_Click) { Enabled = Localization.Manager.IsDirty } );
            languageToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem(Localization.Manager.Get("_ExportAll", "Export AllTexts.csv"), null, menuExportLanguage_Click) );
            languageToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem(Localization.Manager.Get("_ImportAll", "Import AllTexts.csv"), null, menuImportLanguage_Click));
        }

        //---------------------------------------------------------------------------------
        public void menuLanguage_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item == null) return;
            Localization.Manager.CurrentLanguage = (string) item.Tag;
        }

        //---------------------------------------------------------------------------------
        public void menuSaveLanguage_Click(object sender, EventArgs e)
        {
            Localization.Manager.Save();
        }

        //---------------------------------------------------------------------------------
        public void menuExportLanguage_Click(object sender, EventArgs e)
        {
            Localization.Manager.ExportAllLanguages();
        }

        //---------------------------------------------------------------------------------
        public void menuImportLanguage_Click(object sender, EventArgs e)
        {
            Localization.Manager.ImportAllLanguages();
        }

        //=========================================================================================
        // Graph menu event
        //

        //---------------------------------------------------------------------------------
        private void graphToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripItem item in graphToolStripMenuItem.DropDownItems)
                item.Enabled = TaxonUtils.MainGraph != null;
            ClearAllFilterToolStripMenuItem.Enabled = !TaxonUtils.CurrentFilters.IsEmpty;
        }

        //---------------------------------------------------------------------------------
        private void ResetViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TaxonUtils.MainGraph == null) return;
            TaxonUtils.MainGraph.ResetView();
        }


        //---------------------------------------------------------------------------------
        private void ClearAllFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.Clear());
        }

        //---------------------------------------------------------------------------------
        private void ClearCommonFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaxonUtils.UpdateCurrentFilters(() =>
            {
                TaxonUtils.CurrentFilters.RemoveFilter<TaxonFilterUnnamed>();
                TaxonUtils.CurrentFilters.RemoveFilter<TaxonFilterRedListCategory>();
            });
        }

        //---------------------------------------------------------------------------------
        private void HideUnnamedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TaxonUtils.CurrentFilters.GetFilter<TaxonFilterUnnamed>() != null)
                TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.RemoveFilter<TaxonFilterUnnamed>());
            else
                TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.AddFilter<TaxonFilterUnnamed>());
        }

        //---------------------------------------------------------------------------------
        private void HighlightFrenchName(TaxonTreeNode _node)
        {
            _node.Highlight = _node.Desc.HasFrenchName;
            foreach (TaxonTreeNode child in _node.Children)
                HighlightFrenchName(child);
        }
        //-----
        private void withFrenchNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HighlightFrenchName(TaxonUtils.Root);
            TaxonUtils.Invalidate();
        }

        //---------------------------------------------------------------------------------
        private void HighlightClassikRank(TaxonTreeNode _node)
        {
            _node.Highlight = _node.Desc.ClassicRank != ClassicRankEnum.None;
            //_node.Highlight = _node.Desc.ClassicRank == ClassicRankEnum.Classe;
            foreach (TaxonTreeNode child in _node.Children)
                HighlightClassikRank(child);
        }
        //-----
        private void withClassicRankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HighlightClassikRank(TaxonUtils.Root);
            TaxonUtils.Invalidate();
        }

        //---------------------------------------------------------------------------------
        private void HighlightImage(TaxonTreeNode _node)
        {
            _node.Highlight = _node.Desc.HasImage;
            foreach (TaxonTreeNode child in _node.Children)
                HighlightImage(child);
        }
        //-----
        private void withImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HighlightImage(TaxonUtils.Root);
            TaxonUtils.Invalidate();
        }

        //---------------------------------------------------------------------------------
        private void HighlightSound(TaxonTreeNode _node)
        {
            _node.Highlight = _node.Desc.HasSound;
            foreach (TaxonTreeNode child in _node.Children)
                HighlightSound(child);
        }
        //-----
        private void withSoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HighlightSound(TaxonUtils.Root);
            TaxonUtils.Invalidate();
        }

        //---------------------------------------------------------------------------------
        private void clearHighlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaxonUtils.Root.HighlightClear();
            TaxonUtils.Invalidate();
        }


        //=========================================================================================
        // windows menu command
        //

        //---------------------------------------------------------------------------------
        private void windowToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            bool userMode = SystemConfig.IsInUserMode;
            editToolStripMenuItem.Visible = !userMode;
            favoritesToolStripMenuItem.Visible = !userMode;
            historyToolStripMenuItem.Visible = !userMode;
            ascendantsToolStripMenuItem.Visible = !userMode;
            optionsToolStripMenuItem.Visible = !userMode;

            debugInfoToolStripMenuItem.Visible = SystemConfig.IsInDebuggerMode;

            foreach (ToolStripItem item in windowToolStripMenuItem.DropDownItems)
            {
                if (!(item is ToolStripMenuItem)) continue;
                bool check = false;
                if (item != null && item.Tag != null && (item.Tag is Type))
                {
                    Type type = item.Tag as Type;
                    ITaxonControl itc = TaxonControlList.FindTaxonControl(item.Tag as Type);
                    check = itc != null;
                }
                (item as ToolStripMenuItem).Checked = check;
            }
        }

        //---------------------------------------------------------------------------------
        private void windowToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            Type type = item.Tag as Type;
            if (type == null) return;

            ITaxonControl itc = TaxonControlList.FindTaxonControl(type);
            if (itc != null)
            {
                if (itc.OwnerContainer == null) return;
                if (itc is TaxonControl)
                {
                    if ((itc as TaxonControl).CanBeClosed)
                    {
                        itc.OwnerContainer.Remove(itc as TaxonControl, true);
                        TaxonControlList.UnregisterTaxonControl(itc);
                    }
                    else // cannot close, just set focus then
                        itc.OwnerContainer.SetFocus(itc);
                }
            }
            else
            {
                object o = type.GetConstructor(new Type[] { }).Invoke(new object[] { });
                new FormContainer(o as TaxonControl).Show(this);
            }
        }

        //=========================================================================================
        // Database menu callback
        //

        //---------------------------------------------------------------------------------
        private void generateToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        //=========================================================================================
        // about menu callback
        //
        
        //---------------------------------------------------------------------------------
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout aboutForm = new FormAbout();
            aboutForm.ShowDialog();
        }

        //=========================================================================================
        // role menu callback
        //

        //---------------------------------------------------------------------------------
        private void roleToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            userToolStripMenuItem.Checked = SystemConfig.RunningMode == SystemConfig.RunningModeEnum.User;
            adminToolStripMenuItem.Checked = SystemConfig.RunningMode == SystemConfig.RunningModeEnum.Admin;
            debuggerToolStripMenuItem.Checked = SystemConfig.RunningMode == SystemConfig.RunningModeEnum.Debugger;
        }

        //---------------------------------------------------------------------------------
        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemConfig.RunningMode = SystemConfig.RunningModeEnum.User;
        }

        //---------------------------------------------------------------------------------
        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemConfig.RunningMode = SystemConfig.RunningModeEnum.Admin;
        }

        //---------------------------------------------------------------------------------
        private void debuggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemConfig.RunningMode = SystemConfig.RunningModeEnum.Debugger;
        }

        //====================================================================================
        // Taxon control list changed
        //

        //---------------------------------------------------------------------------------
        private void TaxonControlList_OnRegisterTaxonControl(object sender, TaxonControlEventArgs e)
        {
            /*if (e.ITC is TaxonGraphPanel)
            {
                TaxonUtils.MainGraph = e.ITC as TaxonGraphPanel;
                TaxonUtils.MainGraph.OnSelectedChanged += taxonGraph_OnSelectedChanged;
                TaxonUtils.MainGraph.OnReselect += taxonGraph_OnReselect;
                TaxonUtils.MainGraph.OnGraphBelowChanged += taxonGraph_OnBelowChanged;
                TaxonUtils.MainGraph.OnGraphRefreshed += taxonGraph_OnGraphRefreshed;
                TaxonUtils.MainGraph.OnPaintRectangleChanged += taxonGraph_OnPaintRectangleChanged;
                TaxonUtils.MainGraph.Root = TaxonUtils.Root;
            }
            else if (e.ITC is TaxonGraph)
            {
            }
            else 
            */

            if (e.ITC is TaxonEditInfo)
            {
                TaxonEditInfo control = e.ITC as TaxonEditInfo;
                control.OnEditedTaxonChanged += taxonEditorInfo_OnTaxonChanged;
            }
            else if (e.ITC is TaxonNavigator)
            {
                TaxonNavigator control = e.ITC as TaxonNavigator;
                control.OnMoveRectangle += taxonNavigator_OnMoveRectangle;
            }
        }

        //---------------------------------------------------------------------------------
        private void TaxonControlList_OnInitTaxonControlAfterLoad(object sender, TaxonControlEventArgs e)
        {
            Console.WriteLine("type : " + e.ITC.GetType());
            if (e.ITC is TaxonGraph)
            {
                TaxonUtils.MainGraph = e.ITC as TaxonGraph;
                TaxonUtils.MainGraph.Graph.OnSelectedChanged += taxonGraph_OnSelectedChanged;
                TaxonUtils.MainGraph.Graph.OnReselect += taxonGraph_OnReselect;
                TaxonUtils.MainGraph.Graph.OnGraphBelowChanged += taxonGraph_OnBelowChanged;
                TaxonUtils.MainGraph.Graph.OnGraphRefreshed += taxonGraph_OnGraphRefreshed;
                TaxonUtils.MainGraph.Graph.OnPaintRectangleChanged += taxonGraph_OnPaintRectangleChanged;
                TaxonUtils.MainGraph.Root = TaxonUtils.Root;
            }
        }

        //---------------------------------------------------------------------------------
        private void TaxonControlList_OnUnregisterTaxonControl(object sender, TaxonControlEventArgs e)
        {
            if (e.ITC is TaxonGraph)
            {
                TaxonUtils.MainGraph.Graph.OnSelectedChanged -= taxonGraph_OnSelectedChanged;
                TaxonUtils.MainGraph.Graph.OnReselect -= taxonGraph_OnReselect;
                TaxonUtils.MainGraph.Graph.OnGraphBelowChanged -= taxonGraph_OnBelowChanged;
                TaxonUtils.MainGraph.Graph.OnGraphRefreshed -= taxonGraph_OnGraphRefreshed;
                TaxonUtils.MainGraph.Graph.OnPaintRectangleChanged -= taxonGraph_OnPaintRectangleChanged;
                TaxonUtils.MainGraph = null;
            }
            else if (e.ITC is TaxonEditInfo)
            {
                TaxonEditInfo control = e.ITC as TaxonEditInfo;
                control.OnEditedTaxonChanged -= taxonEditorInfo_OnTaxonChanged;
            }
            else if (e.ITC is TaxonNavigator)
            {
                TaxonNavigator control = e.ITC as TaxonNavigator;
                control.OnMoveRectangle -= taxonNavigator_OnMoveRectangle;
            }
        }

        //=========================================================================================
        // Taxon graph 
        //
        
        //---------------------------------------------------------------------------------
        private void taxonGraph_AddOneIfNone()
        {
            TaxonGraphPanel graph = TaxonControlList.FindTaxonControl<TaxonGraphPanel>();

            // create main taxon graph
            if (graph == null)
            {
                AddGraph();
            }
        }

        private void AddGraph()
        {
            TaxonGraphPanel graph = null;
            //panel1.Controls.Clear();
            ControlContainerTabs tabControl = null;
            foreach (Control control in panel1.Controls)
                if (control is ControlContainerTabs)
                {
                    tabControl = control as ControlContainerTabs;
                    break;
                }
            if (tabControl == null)
            {
                tabControl = new ControlContainerTabs();
                tabControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
                tabControl.Current = null;
                tabControl.ShowHeaderWhenOnlyOne = false;
                tabControl.TabIndex = 0;
                panel1.Controls.Add(tabControl);
            }
            graph = new TaxonGraphPanel();
            TaxonGraph trueGraph = new TaxonGraph();
            trueGraph.Root = TaxonUtils.Root;
            tabControl.Add(trueGraph);
        }

        //=========================================================================================
        // Taxon graph messages
        //

        //---------------------------------------------------------------------------------
        private void taxonGraph_OnSelectedChanged(object sender, EventArgs e)
        {
            TaxonUtils.HistoryAdd(TaxonUtils.MainGraph.Selected);
            TaxonControlList.OnSelectTaxon(TaxonUtils.MainGraph.Selected);
            //TaxonControlList.OnSelectTaxon(TaxonUtils.MainGraph.SelectedInOriginal);
        }

        //---------------------------------------------------------------------------------
        private void taxonGraph_OnReselect(object sender, EventArgs e) 
        {
            TaxonControlList.OnReselectTaxon(TaxonUtils.MainGraph.Selected);
            //TaxonControlList.OnReselectTaxon(TaxonUtils.MainGraph.SelectedInOriginal);
        }

        //---------------------------------------------------------------------------------
        private void taxonGraph_OnBelowChanged(object sender, EventArgs e)
        {
            if (sender is TaxonGraphPanel)
            {
                TaxonControlList.OnBelowChanged((sender as TaxonGraphPanel).BelowMouse);
                TipManager.SetTaxon((sender as TaxonGraphPanel).BelowMouse, null);
            }
        }

        //---------------------------------------------------------------------------------
        private void taxonGraph_OnGraphRefreshed(object sender, EventArgs e) 
        { 
            TaxonControlList.OnRefreshAll();
        }

        //---------------------------------------------------------------------------------
        private void taxonGraph_OnPaintRectangleChanged(object sender, TaxonGraphPanel.OnPaintRectangleChangedArgs e) 
        { 
            TaxonControlList.OnViewRectangleChanged(e.R);
        }

        //=========================================================================================
        // Taxon navigator messages
        //

        //---------------------------------------------------------------------------------
        public void taxonNavigator_OnMoveRectangle(object sender, TaxonNavigator.OnMoveRectangleArgs e) 
        {
            if (TaxonUtils.MainGraph == null) return;
            TaxonUtils.MainGraph.Offset(e.X, e.Y); 
        }

        //=========================================================================================
        // Taxon editor info messages
        //
        bool DataAreDirty = false;
        public void taxonEditorInfo_OnTaxonChanged(object sender, TaxonEditInfo.OnTaxonChangedArgs e) 
        { 
            DataAreDirty = true;
            UpdateUI();
            TaxonControlList.OnTaxonChanged( sender, e.taxon );
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaxonDialog.NewTaxon dlg = new TaxonDialog.NewTaxon(null)
            {
                TopMost = true,
                CheckNameUsage = true
            };
            dlg.ShowDialog();
            if (dlg.DialogResult != DialogResult.OK) return;

            TaxonDesc desc = new TaxonDesc(dlg.TaxonName);

            TaxonTreeNode root = new TaxonTreeNode(desc);
            TaxonUtils.SetOriginalRoot(root);
            TaxonUtils.MyConfig.TaxonFileName = "New_tree";
            TaxonUtils.MyConfig.saved = false;
            TaxonUtils.MainGraph.Root = root;
            TaxonUtils.MainGraph.ResetView();
        }
    }
}

