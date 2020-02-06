namespace TreeOfLife
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.userModeLoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.createNewTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSeparator1MenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.importOpenTreeOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.whatIsThatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oTTDepotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearAllFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.highlightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.withFrenchNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.withClassicRankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.withImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.withSoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearHighlightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enEnglishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debuggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.graphtreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ascendantsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.favoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.finderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.historyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.informationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.navigatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quizzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hangmanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.menuMain.AutoSize = false;
            this.menuMain.BackColor = System.Drawing.Color.LightCyan;
            this.menuMain.Dock = System.Windows.Forms.DockStyle.None;
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userModeLoadToolStripMenuItem,
            this.fileToolStripMenuItem,
            this.graphToolStripMenuItem,
            this.displayToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.databaseToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.roleToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(2, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(783, 24);
            this.menuMain.TabIndex = 7;
            this.menuMain.Text = "menuMain";
            // 
            // userModeLoadToolStripMenuItem
            // 
            this.userModeLoadToolStripMenuItem.Name = "userModeLoadToolStripMenuItem";
            this.userModeLoadToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.userModeLoadToolStripMenuItem.Text = "Load";
            this.userModeLoadToolStripMenuItem.Visible = false;
            this.userModeLoadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.createNewTreeToolStripMenuItem,
            this.fileSeparator1MenuItem,
            this.importOpenTreeOToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.DropDownOpening += new System.EventHandler(this.fileToolStripMenuItem_DropDownOpening);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.saveAsToolStripMenuItem.Text = "Save As ...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(274, 6);
            // 
            // createNewTreeToolStripMenuItem
            // 
            this.createNewTreeToolStripMenuItem.Name = "createNewTreeToolStripMenuItem";
            this.createNewTreeToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.createNewTreeToolStripMenuItem.Text = "Create new tree";
            this.createNewTreeToolStripMenuItem.Click += new System.EventHandler(this.createNewTreeToolStripMenuItem_Click);
            // 
            // fileSeparator1MenuItem
            // 
            this.fileSeparator1MenuItem.Name = "fileSeparator1MenuItem";
            this.fileSeparator1MenuItem.Size = new System.Drawing.Size(274, 6);
            // 
            // importOpenTreeOToolStripMenuItem
            // 
            this.importOpenTreeOToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.toolStripMenuItem5,
            this.whatIsThatToolStripMenuItem,
            this.oTTDepotToolStripMenuItem});
            this.importOpenTreeOToolStripMenuItem.Name = "importOpenTreeOToolStripMenuItem";
            this.importOpenTreeOToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.importOpenTreeOToolStripMenuItem.Text = "Open Tree of Life Reference Taxonomy";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(142, 6);
            // 
            // whatIsThatToolStripMenuItem
            // 
            this.whatIsThatToolStripMenuItem.Name = "whatIsThatToolStripMenuItem";
            this.whatIsThatToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.whatIsThatToolStripMenuItem.Text = "What is that ?";
            this.whatIsThatToolStripMenuItem.Click += new System.EventHandler(this.whatIsThatToolStripMenuItem_Click);
            // 
            // oTTDepotToolStripMenuItem
            // 
            this.oTTDepotToolStripMenuItem.Name = "oTTDepotToolStripMenuItem";
            this.oTTDepotToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.oTTDepotToolStripMenuItem.Text = "OTT depot";
            this.oTTDepotToolStripMenuItem.Click += new System.EventHandler(this.oTTDepotToolStripMenuItem_Click);
            // 
            // graphToolStripMenuItem
            // 
            this.graphToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetViewToolStripMenuItem,
            this.ClearAllFilterToolStripMenuItem,
            this.toolStripMenuItem3,
            this.highlightToolStripMenuItem,
            this.clearHighlightToolStripMenuItem});
            this.graphToolStripMenuItem.Name = "graphToolStripMenuItem";
            this.graphToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.graphToolStripMenuItem.Text = "Graph";
            this.graphToolStripMenuItem.DropDownOpening += new System.EventHandler(this.graphToolStripMenuItem_DropDownOpening);
            // 
            // resetViewToolStripMenuItem
            // 
            this.resetViewToolStripMenuItem.Name = "resetViewToolStripMenuItem";
            this.resetViewToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.resetViewToolStripMenuItem.Text = "Reset view";
            this.resetViewToolStripMenuItem.Click += new System.EventHandler(this.ResetViewToolStripMenuItem_Click);
            // 
            // ClearAllFilterToolStripMenuItem
            // 
            this.ClearAllFilterToolStripMenuItem.Name = "ClearAllFilterToolStripMenuItem";
            this.ClearAllFilterToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.ClearAllFilterToolStripMenuItem.Text = "Clear all filters";
            this.ClearAllFilterToolStripMenuItem.Click += new System.EventHandler(this.ClearAllFilterToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(151, 6);
            // 
            // highlightToolStripMenuItem
            // 
            this.highlightToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.withFrenchNameToolStripMenuItem,
            this.withClassicRankToolStripMenuItem,
            this.withImageToolStripMenuItem,
            this.withSoundToolStripMenuItem});
            this.highlightToolStripMenuItem.Name = "highlightToolStripMenuItem";
            this.highlightToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.highlightToolStripMenuItem.Text = "Highlight";
            // 
            // withFrenchNameToolStripMenuItem
            // 
            this.withFrenchNameToolStripMenuItem.Name = "withFrenchNameToolStripMenuItem";
            this.withFrenchNameToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.withFrenchNameToolStripMenuItem.Text = "With French name";
            this.withFrenchNameToolStripMenuItem.Click += new System.EventHandler(this.withFrenchNameToolStripMenuItem_Click);
            // 
            // withClassicRankToolStripMenuItem
            // 
            this.withClassicRankToolStripMenuItem.Name = "withClassicRankToolStripMenuItem";
            this.withClassicRankToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.withClassicRankToolStripMenuItem.Text = "With Classic rank";
            this.withClassicRankToolStripMenuItem.Click += new System.EventHandler(this.withClassicRankToolStripMenuItem_Click);
            // 
            // withImageToolStripMenuItem
            // 
            this.withImageToolStripMenuItem.Name = "withImageToolStripMenuItem";
            this.withImageToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.withImageToolStripMenuItem.Text = "With Image";
            this.withImageToolStripMenuItem.Click += new System.EventHandler(this.withImageToolStripMenuItem_Click);
            // 
            // withSoundToolStripMenuItem
            // 
            this.withSoundToolStripMenuItem.Name = "withSoundToolStripMenuItem";
            this.withSoundToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.withSoundToolStripMenuItem.Text = "With Sound";
            this.withSoundToolStripMenuItem.Click += new System.EventHandler(this.withSoundToolStripMenuItem_Click);
            // 
            // clearHighlightToolStripMenuItem
            // 
            this.clearHighlightToolStripMenuItem.Name = "clearHighlightToolStripMenuItem";
            this.clearHighlightToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.clearHighlightToolStripMenuItem.Text = "Clear Highlight";
            this.clearHighlightToolStripMenuItem.Click += new System.EventHandler(this.clearHighlightToolStripMenuItem_Click);
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.languageToolStripMenuItem});
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.displayToolStripMenuItem.Text = "Display";
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enEnglishToolStripMenuItem});
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.languageToolStripMenuItem.Text = "Language";
            this.languageToolStripMenuItem.DropDownOpening += new System.EventHandler(this.languageToolStripMenuItem_DropDownOpening);
            // 
            // enEnglishToolStripMenuItem
            // 
            this.enEnglishToolStripMenuItem.Name = "enEnglishToolStripMenuItem";
            this.enEnglishToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.enEnglishToolStripMenuItem.Text = "(en) English";
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.graphtreeToolStripMenuItem,
            this.toolStripSeparator1,
            this.ascendantsToolStripMenuItem,
            this.favoritesToolStripMenuItem,
            this.finderToolStripMenuItem,
            this.historyToolStripMenuItem,
            this.informationToolStripMenuItem,
            this.navigatorToolStripMenuItem,
            this.tagsToolStripMenuItem,
            this.toolStripSeparator2,
            this.debugInfoToolStripMenuItem,
            this.editToolStripMenuItem,
            this.logToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.toolStripSeparator3,
            this.quizzToolStripMenuItem,
            this.hangmanToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.windowToolStripMenuItem.Text = "Window";
            this.windowToolStripMenuItem.DropDownOpening += new System.EventHandler(this.windowToolStripMenuItem_DropDownOpening);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(186, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(186, 6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(186, 6);
            // 
            // databaseToolStripMenuItem
            // 
            this.databaseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateToolStripMenuItem});
            this.databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            this.databaseToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.databaseToolStripMenuItem.Text = "Database";
            // 
            // generateToolStripMenuItem
            // 
            this.generateToolStripMenuItem.Name = "generateToolStripMenuItem";
            this.generateToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.generateToolStripMenuItem.Text = "Generate";
            this.generateToolStripMenuItem.Click += new System.EventHandler(this.generateToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // roleToolStripMenuItem
            // 
            this.roleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userToolStripMenuItem,
            this.adminToolStripMenuItem,
            this.debuggerToolStripMenuItem});
            this.roleToolStripMenuItem.Name = "roleToolStripMenuItem";
            this.roleToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.roleToolStripMenuItem.Text = "Role";
            this.roleToolStripMenuItem.DropDownOpening += new System.EventHandler(this.roleToolStripMenuItem_DropDownOpening);
            // 
            // userToolStripMenuItem
            // 
            this.userToolStripMenuItem.Name = "userToolStripMenuItem";
            this.userToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.userToolStripMenuItem.Text = "User";
            this.userToolStripMenuItem.Click += new System.EventHandler(this.userToolStripMenuItem_Click);
            // 
            // adminToolStripMenuItem
            // 
            this.adminToolStripMenuItem.Name = "adminToolStripMenuItem";
            this.adminToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.adminToolStripMenuItem.Text = "Admin";
            this.adminToolStripMenuItem.Click += new System.EventHandler(this.adminToolStripMenuItem_Click);
            // 
            // debuggerToolStripMenuItem
            // 
            this.debuggerToolStripMenuItem.Name = "debuggerToolStripMenuItem";
            this.debuggerToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.debuggerToolStripMenuItem.Text = "Debugger";
            this.debuggerToolStripMenuItem.Click += new System.EventHandler(this.debuggerToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(115)))), ((int)(((byte)(135)))));
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(785, 537);
            this.panel1.TabIndex = 11;
            // 
            // graphtreeToolStripMenuItem
            // 
            this.graphtreeToolStripMenuItem.Name = "graphtreeToolStripMenuItem";
            this.graphtreeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.graphtreeToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.graphtreeToolStripMenuItem.Tag = typeof(TreeOfLife.TaxonGraph);
            this.graphtreeToolStripMenuItem.Text = "Graph";
            this.graphtreeToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // ascendantsToolStripMenuItem
            // 
            this.ascendantsToolStripMenuItem.Name = "ascendantsToolStripMenuItem";
            this.ascendantsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.ascendantsToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.ascendantsToolStripMenuItem.Tag = typeof(TreeOfLife.TaxonAscendants);
            this.ascendantsToolStripMenuItem.Text = "Ascendants";
            this.ascendantsToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // favoritesToolStripMenuItem
            // 
            this.favoritesToolStripMenuItem.Name = "favoritesToolStripMenuItem";
            this.favoritesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F)));
            this.favoritesToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.favoritesToolStripMenuItem.Tag = typeof(TreeOfLife.TaxonFavorites);
            this.favoritesToolStripMenuItem.Text = "Favorites";
            this.favoritesToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // finderToolStripMenuItem
            // 
            this.finderToolStripMenuItem.Name = "finderToolStripMenuItem";
            this.finderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.finderToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.finderToolStripMenuItem.Tag = typeof(TreeOfLife.TaxonFinder);
            this.finderToolStripMenuItem.Text = "Finder";
            this.finderToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // historyToolStripMenuItem
            // 
            this.historyToolStripMenuItem.Name = "historyToolStripMenuItem";
            this.historyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.historyToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.historyToolStripMenuItem.Tag = typeof(TreeOfLife.TaxonHistory);
            this.historyToolStripMenuItem.Text = "History";
            this.historyToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // informationToolStripMenuItem
            // 
            this.informationToolStripMenuItem.Name = "informationToolStripMenuItem";
            this.informationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.informationToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.informationToolStripMenuItem.Tag = typeof(TreeOfLife.TaxonInfo);
            this.informationToolStripMenuItem.Text = "Information";
            this.informationToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // navigatorToolStripMenuItem
            // 
            this.navigatorToolStripMenuItem.Name = "navigatorToolStripMenuItem";
            this.navigatorToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.navigatorToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.navigatorToolStripMenuItem.Tag = typeof(TreeOfLife.TaxonNavigator);
            this.navigatorToolStripMenuItem.Text = "Navigator";
            this.navigatorToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // tagsToolStripMenuItem
            // 
            this.tagsToolStripMenuItem.Name = "tagsToolStripMenuItem";
            this.tagsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.tagsToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.tagsToolStripMenuItem.Tag = typeof(TreeOfLife.TaxonTags);
            this.tagsToolStripMenuItem.Text = "Tags";
            this.tagsToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // debugInfoToolStripMenuItem
            // 
            this.debugInfoToolStripMenuItem.Name = "debugInfoToolStripMenuItem";
            this.debugInfoToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.debugInfoToolStripMenuItem.Tag = typeof(TreeOfLife.TaxonDebugInfo);
            this.debugInfoToolStripMenuItem.Text = "Debug Info";
            this.debugInfoToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.editToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.editToolStripMenuItem.Tag = typeof(TreeOfLife.TaxonEditInfo);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.logToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.logToolStripMenuItem.Tag = typeof(TreeOfLife.TaxonLog);
            this.logToolStripMenuItem.Text = "Log";
            this.logToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.optionsToolStripMenuItem.Tag = typeof(TreeOfLife.TaxonOptions);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // quizzToolStripMenuItem
            // 
            this.quizzToolStripMenuItem.Name = "quizzToolStripMenuItem";
            this.quizzToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.quizzToolStripMenuItem.Tag = typeof(TreeOfLife.TaxonGameQuizz);
            this.quizzToolStripMenuItem.Text = "Quizz";
            this.quizzToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // hangmanToolStripMenuItem
            // 
            this.hangmanToolStripMenuItem.Name = "hangmanToolStripMenuItem";
            this.hangmanToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.hangmanToolStripMenuItem.Tag = typeof(TreeOfLife.TaxonGameHangman);
            this.hangmanToolStripMenuItem.Text = "Hangman";
            this.hangmanToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(94)))), ((int)(((byte)(111)))));
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuMain);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.Text = "Tree of Life";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem userModeLoadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem finderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphtreeToolStripMenuItem; 
        private System.Windows.Forms.ToolStripMenuItem ascendantsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem historyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem favoritesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem navigatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tagsToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem informationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem quizzToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hangmanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearAllFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem highlightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem withFrenchNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem withClassicRankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearHighlightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem withImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem withSoundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator fileSeparator1MenuItem;
        private System.Windows.Forms.ToolStripMenuItem importOpenTreeOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem whatIsThatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oTTDepotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem roleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debuggerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem createNewTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enEnglishToolStripMenuItem;
    }
}

