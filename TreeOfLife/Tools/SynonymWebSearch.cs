using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace TreeOfLife.Tools
{
    /*
    [Description("Search names in a file for correspondent existent taxon")]
    [DisplayName("Search on web")]
    [Category("Tools/Synonyms")]
    [PresentInMode(false, false)]
    class SynonymWebSearch : ITool
    {
        public bool CanActivate()
        {
            //return false;
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose a file containing one synonym per line";
            ofd.Filter = "txt files (*.txt)|*.txt";
            ofd.Multiselect = false;
            ofd.AddExtension = true;
            if (ofd.ShowDialog() != DialogResult.OK) return;
            if (!File.Exists(ofd.FileName)) return;

            TaxonSearch searchTool = new TaxonSearch(TaxonUtils.OriginalRoot, true, true);
            string[] urlStart = new string[] { "https://en.wikipedia.org", "https://fr.wikipedia.org" };

            string lofFilename = TaxonUtils.GetLogPath() + "SynonymWebSearch.log";
            if (File.Exists(lofFilename)) File.Delete(lofFilename);
            using (StreamWriter outfile = new StreamWriter(lofFilename))
            {
                outfile.WriteLine("SynonymWebSearch results for name in: ");
                outfile.WriteLine("    " + ofd.FileName);
                outfile.WriteLine();

                using (StreamReader file = new StreamReader(ofd.FileName))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        line = line.Trim().ToLower();
                        outfile.WriteLine(line);
                        Console.WriteLine(line);

                        using (WebClient client = new WebClient())
                        {
                            string googleUrl = "https://www.google.fr/search?q=" + line.Replace(" ", "%20");
                            string s = client.DownloadString(googleUrl);

                            foreach (string url in urlStart)
                            {
                                int indexStart = s.IndexOf("<cite>" + url);
                                if (indexStart == -1) continue;
                                indexStart += 6;
                                int indexEnd = s.IndexOf("</cite>", indexStart);
                                if (indexEnd == -1) continue;

                                string urlComplete = s.Substring(indexStart, indexEnd - indexStart);
                                urlComplete = urlComplete.Replace("<b>", "");
                                urlComplete = urlComplete.Replace("</b>", "");
                                outfile.WriteLine("    " + urlComplete);
                                string content = null;
                                try
                                {
                                    content = client.DownloadString(urlComplete);
                                }
                                catch (Exception e)
                                {
                                    outfile.WriteLine("        error " + e.Message);
                                    continue;
                                }

                                indexStart = content.IndexOf(">Synonymes<");
                                if (indexStart == -1) indexStart = content.IndexOf(">Synonyms<");
                                if (indexStart == -1) continue;
                                int indexListStart = content.IndexOf("<ul>", indexStart);
                                if (indexListStart == -1 || indexListStart - indexStart > 100)
                                    continue;
                                indexStart = indexListStart;
                                indexEnd = content.IndexOf("</ul>", indexStart);
                                if (indexEnd == -1)
                                    continue;

                                List<string> synonyms = new List<string>();

                                int indexCur = content.IndexOf("<li><i>", indexStart);
                                while (indexCur != -1 && indexCur < indexEnd)
                                {
                                    indexCur += 7;
                                    int indexCurEnd = content.IndexOf("</i>", indexCur);
                                    if (indexCurEnd == -1)
                                        break;
                                    synonyms.Add(content.Substring(indexCur, indexCurEnd - indexCur));
                                    indexCur = content.IndexOf("<li><i>", indexCurEnd);
                                }

                                if (synonyms.Count == 0)
                                    outfile.WriteLine("        no synonyms found");
                                else
                                    outfile.WriteLine("        " + synonyms.Count.ToString() + " found");

                                bool found = false;
                                foreach (string synonym in synonyms)
                                {
                                    TaxonTreeNode node = searchTool.Find(synonym);
                                    if (node == null)
                                    {
                                        outfile.WriteLine("            " + synonym + " x");
                                        continue;
                                    }

                                    List<string> alternativeNames = node.Desc.GetAlternativeNameList();
                                    if (!alternativeNames.Contains(synonym))
                                    {
                                        outfile.WriteLine("            " + synonym + " add as synonyms for " + node.Desc.MainName);
                                        alternativeNames.Add(synonym);
                                        node.Desc.SetAlternativeName(alternativeNames);
                                        found = true;
                                    }
                                    else
                                    {
                                        outfile.WriteLine("            " + synonym + " already in synonyms for " + node.Desc.MainName);
                                    }
                                }
                                if (found)
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
    */
}
