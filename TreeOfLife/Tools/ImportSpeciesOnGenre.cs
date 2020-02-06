using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    //Create and plug species on genera

    [Description("Import list of species from a file, (one line per species) search for genre in treee, if found and species does not exist in genre, add it")]
    [DisplayName("Create and plug species on geenre")]
    [Category("Tools/Taxons")]
    [PresentInMode(false, true)]
    class ImportSpeciesOnGenre : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Species list (*.txt)|*.txt",
                Multiselect = false,
                AddExtension = true
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            if (!File.Exists(ofd.FileName)) return;

            string shortName = Path.GetFileNameWithoutExtension(ofd.FileName);

            int totalLines = 0;
            int ErrorNoTwoWords = 0;
            int ErrorGenreNotFound = 0;
            int ErrorGenreFoundMoreThanOnce = 0;
            int WarningSpeciesAlreadyInGenre = 0;
            int NoErrors = 0;

            string logFilenameErrors = Path.Combine(TaxonUtils.GetLogPath(), "ImportSpeciesOnGenre_" + shortName + "_errors.log");
            using (StreamWriter log = new StreamWriter(logFilenameErrors))
            {
                TaxonSearch searchTool = new TaxonSearch(TaxonUtils.OriginalRoot, false, false);
                searchTool.Init((n) =>
                {
                    if (n.Desc.IsUnnamed) return;
                    if (n.Desc.ClassicRank == ClassicRankEnum.Genre)
                        searchTool.Add(n.Desc.RefMultiName.Main, n);
                });

                using (StreamReader file = new StreamReader(ofd.FileName))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        line = line.Trim();

                        totalLines++;

                        List<string> names = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (names.Count != 2)
                        {
                            log.WriteLine("Error, species must have two names: " + line);
                            ErrorNoTwoWords++;
                            continue;
                        }

                        List<TaxonTreeNode> searchResult = searchTool.FindAll(names[0]);

                        if (searchResult == null || searchResult.Count == 0)
                        {
                            log.WriteLine("Error, does not found genre for: " + line);
                            ErrorGenreNotFound++;
                            continue;
                        }
                        if (searchResult.Count > 1)
                        {
                            log.WriteLine("Error, more than one genre for: " + line);
                            ErrorGenreFoundMoreThanOnce++;
                            continue;
                        }

                        string searchString = line.ToLower().Replace('-', ' ');
                        TaxonTreeNode taxonGenre = searchResult[0];
                        bool speciesFound = false;
                        foreach (TaxonTreeNode child in taxonGenre.Children)
                        {
                            string formated = child.Desc.RefMultiName.Main.ToLower().Replace('-', ' ');
                            if (formated == searchString)
                                speciesFound = true;
                        }

                        if (speciesFound)
                        {
                            log.WriteLine("Warning, species already exists: " + line);
                            WarningSpeciesAlreadyInGenre++;
                            continue;
                        }

                        TaxonDesc newTaxon = new TaxonDesc(line)
                        {
                            ClassicRank = ClassicRankEnum.Espece
                        };
                        taxonGenre.AddChild(new TaxonTreeNode(newTaxon));
                        NoErrors++;
                    }
                }
            }


            string message = "Importing 'species on genre' from: " + ofd.FileName + "\n";
            message += String.Format("    Total lines: {0}\n", totalLines);
            message += String.Format("    lines with species with less or more than two words : {0}\n", ErrorNoTwoWords);
            message += String.Format("    lines without associated genre : {0}\n", ErrorGenreNotFound);
            message += String.Format("    lines with more than one associated genre: {0}\n", ErrorGenreFoundMoreThanOnce);
            message += String.Format("    lines with species already in genre: {0}\n", WarningSpeciesAlreadyInGenre);
            message += String.Format("    lines with imported species: {0}\n", NoErrors);
            message += String.Format("for more details, look at ImportSpeciesOnGenre_{0}_Errors.log files", shortName);
            Loggers.WriteInformation(LogTags.Data, message);
        }
    }
}
