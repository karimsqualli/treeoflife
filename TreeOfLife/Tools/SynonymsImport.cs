using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    [Description("Import synonyms from a text file")]
    [DisplayName("Import")]
    [Category("Tools/Synonyms")]
    [PresentInMode(false, true)]
    class SynonymsImport : ITool
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
            int lineWithSeveralTaxon = 0;
            Dictionary<TaxonTreeNode, string> firstSame = new Dictionary<TaxonTreeNode, string>();
            Dictionary<TaxonTreeNode, string> firstDiff = new Dictionary<TaxonTreeNode, string>();
            Dictionary<TaxonTreeNode, string> notFirst = new Dictionary<TaxonTreeNode, string>();
            Dictionary<TaxonTreeNode, List<string>> severalLines = new Dictionary<TaxonTreeNode, List<string>>();

            string logFilenameErrors = Path.Combine(TaxonUtils.GetLogPath(), "ImportSynonym_" + shortName + "_errors.log");
            using (StreamWriter log = new StreamWriter(logFilenameErrors))
            {
                TaxonSearch searchTool = new TaxonSearch(TaxonUtils.OriginalRoot);
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

                        List<TaxonTreeNode> nodes = new List<TaxonTreeNode>();
                        names.ForEach(t => nodes.Add(searchTool.FindOne(t)));

                        int countNodes = nodes.Count(t => t != null);
                        if (countNodes == 0)
                        {
                            log.WriteLine("Error, no matching nodes found for that line: " + line);
                            lineWithNoTaxon++;
                            continue;
                        }

                        if (countNodes > 1)
                        {
                            log.WriteLine("Error, two many matching nodes (" + countNodes.ToString() + ") found for that line: " + line);
                            lineWithSeveralTaxon++;
                            continue;
                        }

                        bool firstIsGood = nodes[0] != null;
                        TaxonTreeNode node = nodes.Where(t => t != null).First();

                        if (severalLines.ContainsKey(node))
                        {
                            severalLines[node].Add(line);
                            continue;
                        }

                        if (firstSame.ContainsKey(node))
                        {
                            severalLines[node] = new List<string>() { firstSame[node], line };
                            firstSame.Remove(node);
                            continue;
                        }

                        if (firstDiff.ContainsKey(node))
                        {
                            severalLines[node] = new List<string>() { firstDiff[node], line };
                            firstDiff.Remove(node);
                            continue;
                        }

                        if (notFirst.ContainsKey(node))
                        {
                            severalLines[node] = new List<string>() { notFirst[node], line };
                            notFirst.Remove(node);
                            continue;
                        }

                        if (firstIsGood && names[0] == node.Desc.RefMultiName.Main)
                            firstSame[node] = line;
                        else if (firstIsGood)
                            firstDiff[node] = line;
                        else
                            notFirst[node] = line;
                    }
                }
            }

            string logFilenameFirst = Path.Combine(TaxonUtils.GetLogPath(), "ImportSynonym_" + shortName + "_first.log");
            using (StreamWriter log = new StreamWriter(logFilenameFirst))
            {
                log.WriteLine(firstSame.Count.ToString() + " exact matches found");
                foreach (KeyValuePair<TaxonTreeNode, string> pair in firstSame)
                {
                    log.WriteLine("   replace " + pair.Key.Desc.RefAllNames + " => " + pair.Value);
                    List<string> names = pair.Value.Split(Helpers.MultiName.SeparatorAsCharArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                    pair.Key.Desc.Name = string.Join(Helpers.MultiName.SeparatorAsString, names);
                }
                log.WriteLine();

                log.WriteLine(firstDiff.Count.ToString() + " first match found but diff case");
                foreach (KeyValuePair<TaxonTreeNode, string> pair in firstDiff)
                {
                    log.WriteLine("   replace " + pair.Key.Desc.RefAllNames + " => " + pair.Value);
                    List<string> names = pair.Value.Split(Helpers.MultiName.SeparatorAsCharArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                    pair.Key.Desc.Name = string.Join(Helpers.MultiName.SeparatorAsString, names);
                }
                log.WriteLine();
            }

            string logFilenameNotFirst = Path.Combine(TaxonUtils.GetLogPath(), "ImportSynonym_" + shortName + "_notfirst.log");
            using (StreamWriter log = new StreamWriter(logFilenameNotFirst))
            {
                log.WriteLine(notFirst.Count.ToString() + " match found with one synonym");
                foreach (KeyValuePair<TaxonTreeNode, string> pair in notFirst)
                {
                    log.WriteLine("   replace " + pair.Key.Desc.RefAllNames + " => " + pair.Value);
                    List<string> names = pair.Value.Split(Helpers.MultiName.SeparatorAsCharArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                    pair.Key.Desc.Name = string.Join(Helpers.MultiName.SeparatorAsString, names);
                }
                log.WriteLine();
            }

            string logFilenameSeveral = Path.Combine(TaxonUtils.GetLogPath(), "ImportSynonym_" + shortName + "_several.log");
            using (StreamWriter log = new StreamWriter(logFilenameSeveral))
            {
                log.WriteLine(severalLines.Count.ToString() + " node match with several lines");
                foreach (KeyValuePair<TaxonTreeNode, List<string>> pair in severalLines)
                {
                    log.WriteLine("   " + pair.Value.Count.ToString() + " matches for node: " + pair.Key.Desc.RefAllNames);
                    foreach (string s in pair.Value)
                        log.WriteLine("        " + s);
                }
                log.WriteLine();
            }

            string message = "Importing synonyms from: " + ofd.FileName + "\n";
            message += String.Format("    Total lines: {0}\n", totalLines);
            message += String.Format("    lines with only one name: {0}\n", lineWithOnlyOneName);
            message += String.Format("    lines with no associated taxons: {0}\n", lineWithNoTaxon);
            message += String.Format("    lines with more than one associated taxons: {0}\n", lineWithSeveralTaxon);
            message += String.Format("    lines with one ass. taxon, and with same first name: {0}\n", firstSame.Count );
            message += String.Format("    lines with one ass. taxon, and with first name with some lettercase difference: {0}\n", firstDiff.Count);
            message += String.Format("    lines with one ass. taxon, but not with with first name: {0}\n", notFirst.Count);
            message += String.Format("    several lines for same taxon: {0}\n", severalLines.Count);
            message += String.Format("for more details, look at ImportSynonym_{0}_*.log files", shortName);
            Loggers.WriteInformation(LogTags.Data, message);
        }
    }
}
