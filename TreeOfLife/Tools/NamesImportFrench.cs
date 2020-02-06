using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    [Description("Import french names from a file, each line begins with taxon latin name followed by at least one french name, each name is separated by a semi colo")]
    [DisplayName("Import French Names")]
    [Category("Tools/Names")]
    [PresentInMode(false, true)]
    class ImportFrenchNames : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "txt files (*.txt)|*.txt";
            ofd.Multiselect = false;
            ofd.AddExtension = true;
            if (ofd.ShowDialog() != DialogResult.OK) return;
            if (!File.Exists(ofd.FileName)) return;

            List<string> notFound = new List<string>();
            List<Tuple<string, TaxonTreeNode>> found = new List<Tuple<string, TaxonTreeNode>>();

            string shortName = Path.GetFileNameWithoutExtension(ofd.FileName);

            int totalLines = 0;
            int lineWithOnlyOneName = 0;
            int lineWithNoTaxon = 0;
            int nameExistsAlready = 0;
            int nameAdded = 0;
            int changedTaxon = 0;
            Dictionary<TaxonTreeNode, string> infoImport = new Dictionary<TaxonTreeNode, string>();
            
            string logFilenameErrors = Path.Combine(TaxonUtils.GetLogPath(), "ImportFrenchNames_" + shortName + "_errors.log");
            using (StreamWriter log = new StreamWriter(logFilenameErrors))
            {
                TaxonSearch searchTool = new TaxonSearch(TaxonUtils.OriginalRoot, true, true);
                using (StreamReader file = new StreamReader(ofd.FileName))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        totalLines++;

                        List<string> names = line.Split(Helpers.MultiName.SeparatorAsCharArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (names.Count == 1)
                        {
                            log.WriteLine("Error, only one name in line: " + line);
                            lineWithOnlyOneName++;
                            continue;
                        }

                        string latin = names[0];
                        names.RemoveAt(0);

                        TaxonTreeNode node = TaxonUtils.OriginalRoot.FindTaxonByFullName(latin);
                        if (node == null)
                        {
                            int idx = latin.LastIndexOf(TaxonTreeNode.HierarchicalNameSeparator);
                            if (idx != -1)
                                latin = latin.Substring(idx + 1);
                            List<TaxonTreeNode> nodes = searchTool.FindAll(latin);
                            if (nodes != null)
                            {
                                if (nodes.Count > 1)
                                {
                                    log.WriteLine("Error, two many nodes fit with that line: " + line);
                                    lineWithNoTaxon++;
                                    continue;
                                }
                                if (nodes.Count == 1)
                                    node = nodes[0];
                            }
                        }

                        if (node == null)
                        {
                            log.WriteLine("Error, no matching nodes found for that line: " + line);
                            lineWithNoTaxon++;
                            continue;
                        }

                        List<string> previousNames = null;
                        if (node.Desc.FrenchMultiName == null)
                            previousNames = new List<string>();
                        else
                            previousNames = node.Desc.FrenchMultiName.Full.Split(Helpers.MultiName.SeparatorAsCharArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                        List<string> previousNamesTest = previousNames.Select(n => n.Trim().ToLower()).ToList();

                        List<string> infos = new List<string>();
                        int previousNameAdded = nameAdded;
                        foreach (string name in names)
                        {
                            string testname = name.ToLower().Trim();
                            if (previousNamesTest.IndexOf(testname) != -1)
                            {
                                nameExistsAlready++;
                                infos.Add(name + " exists");
                                continue;
                            }
                            nameAdded++;
                            infos.Add(name + " added");
                            previousNames.Add(name);
                            previousNamesTest.Add(testname);
                        }
                        infoImport[node] = string.Join(", ", infos);
                        if (previousNameAdded == nameAdded)
                            continue;
                        changedTaxon++;
                        node.Desc.FrenchMultiName = new Helpers.MultiName(previousNames);
                    }
                }
            }

            string logFilenameFirst = Path.Combine(TaxonUtils.GetLogPath(), "ImportFrenchNames_" + shortName + "_infos.log");
            using (StreamWriter log = new StreamWriter(logFilenameFirst))
            {
                foreach (KeyValuePair<TaxonTreeNode, string> pair in infoImport)
                {
                    log.WriteLine(pair.Key.Desc.Name + " => " + pair.Value);
                }
            }

            string message = "Importing french names from: " + ofd.FileName + "\n";
            message += String.Format("    Total lines: {0}\n", totalLines);
            message += String.Format("    lines with only one name: {0}\n", lineWithOnlyOneName);
            message += String.Format("    lines with no associated taxons: {0}\n", lineWithNoTaxon);
            message += String.Format("    associated taxon found: {0}\n", infoImport.Count);
            message += String.Format("    changed taxons: {0}\n", changedTaxon );
            message += String.Format("    french names already there: {0}\n", nameExistsAlready);
            message += String.Format("    imported french names: {0}\n", nameAdded);
            message += String.Format("for more details, look at ImportFrenchNames_{0}_*.log files", shortName);
            Loggers.WriteInformation(LogTags.Data, message);
        }
    }
}
