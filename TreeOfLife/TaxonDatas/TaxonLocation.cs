using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TreeOfLife.TaxonDialog;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TreeOfLife
{
    public class TaxonLocation
    {
        public string locationId = "";
        public List<TaxonTreeNode> Taxons;
    }

    public class TaxonLocations
    {
        public List<TaxonLocation> List = new List<TaxonLocation>();
        public Dictionary<TaxonTreeNode, string> LocationByTaxon = new Dictionary<TaxonTreeNode, string>();
        readonly string _Path;
        readonly TaxonTreeNode _Root;

        public TaxonLocations( string _path, TaxonTreeNode _root )
        {
            List = new List<TaxonLocation>();
            _Path = _path;
            _Root = _root;

            BackgroundWorker bw = new BackgroundWorker()
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false
            };
            bw.DoWork += new DoWorkEventHandler(BWDoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BWCompleted);
            bw.RunWorkerAsync();
        }

        private void BWDoWork(object sender, DoWorkEventArgs e)
        {
            string[] files = Directory.GetFiles(_Path, "*.xml");
            Parallel.ForEach(files, (file) =>
               {
                   string id = Path.GetFileNameWithoutExtension(file);
                   TaxonList list = TaxonList.Load(file);
                   if (list != null)
                       List.Add(new TaxonLocation() { locationId = id, Taxons = list.ToTaxonTreeNodeList(_Root) });
                   Console.WriteLine(file + " done!");
               });
            Console.WriteLine("location end");
        }

        private void BWCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BuildTaxonLocation();
            string message = "Location data loaded, " + List.Count.ToString() + " files loaded";
            Loggers.WriteInformation(LogTags.Data, message);
        }

        public void BuildTaxonLocation()
        {
            LocationByTaxon = new Dictionary<TaxonTreeNode, string>();
            foreach (TaxonLocation loc in List)
            {
                foreach (TaxonTreeNode node in loc.Taxons)
                    if (LocationByTaxon.TryGetValue(node, out string nodeloc))
                        LocationByTaxon[node] = nodeloc + "|" + loc.locationId;
                    else
                        LocationByTaxon[node] = loc.locationId;
            }
        }

        public static TaxonLocations Load(TaxonTreeNode _root)
        {
            string path = TaxonUtils.GetTaxonLocationPath();
            return !Directory.Exists(path) ? null : new TaxonLocations(path, _root);
        }

        public static void CreateFromDirectory( TaxonTreeNode _root, string path )
        {
            TaxonSearch searchTool = new TaxonSearch(_root, true, true);
            int countFound = 0;
            int countNotFound = 0;

            string[] files = Directory.GetFiles(path, "*.txt");

            string logFilename = Path.Combine(TaxonUtils.GetTaxonLocationPath(), "CreateFromDirectory.log");
            using (StreamWriter log = new StreamWriter(logFilename))
            {
                using (ProgressDialog progressDlg = new ProgressDialog())
                {
                    progressDlg.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                    progressDlg.Show();

                    ProgressItem parseFiles = progressDlg.Add("parseFiles", "", 0, files.Length);
                    foreach (string file in files)
                    {
                        parseFiles.Inc(file);
                        log.WriteLine("Import " + file + ":");
                        France.Departement dep = France.Data.Departements.GetDepartementFromName(Path.GetFileNameWithoutExtension(file));
                        if (dep == null)
                        {
                            log.WriteLine("  associated departement not found");
                            continue;
                        }

                        TaxonList.ImportFileResult resultImport = TaxonList.ImportFile(file, searchTool);
                        log.WriteLine("  " + resultImport.TaxonsFound + " taxons found");
                        log.WriteLine("  " + resultImport.TaxonNotFound + " taxons not found");

                        countFound += resultImport.TaxonsFound;
                        countNotFound += resultImport.TaxonNotFound;

                        TaxonList taxons = new TaxonList();
                        taxons.FromTaxonTreeNodeList(resultImport.List);
                        taxons.HasFile = true;
                        taxons.FileName = Path.Combine(TaxonUtils.GetTaxonLocationPath(), dep.Id + ".xml");
                        taxons.Save();
                    }
                }
            }

            string message = "Create location data from directory " + path + ": \n";
            message += String.Format("    taxons found: {0}\n", countFound);
            message += String.Format("    taxons not found: {0}\n", countNotFound );
            message += String.Format("for more details, look at " + logFilename + " file, and all other generated logs");
            Loggers.WriteInformation(LogTags.Location, message);
        }
    }
}
