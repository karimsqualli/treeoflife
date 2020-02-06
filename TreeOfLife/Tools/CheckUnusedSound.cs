using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    [Description("Parse sound directory to get unused sounds")]
    [DisplayName("List unused sounds")]
    [Category("Tools/Sounds")]
    class CheckUnusedsounds : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            // retrouve la liste de toutes les especes / sous especes
            List<TaxonTreeNode> Species = new List<TaxonTreeNode>();
            TaxonUtils.OriginalRoot.GetAllChildrenRecursively(Species, ClassicRankEnum.Espece);
            List<TaxonTreeNode> SubSpecies = new List<TaxonTreeNode>();
            TaxonUtils.OriginalRoot.GetAllChildrenRecursively(SubSpecies, ClassicRankEnum.SousEspece);
            List<string> Incoherences = new List<string>();

            // et la liste des fichiers dans le répertoires de sounds
            string PathSounds = TaxonUtils.GetSoundDirectory();
            List<string> Files = Directory.GetFiles(PathSounds).ToList();
            int TotalSounds = Files.Count;

            int SpeciesWithSoundCount = 0;
            int SpeciesWithoutSoundCount = 0;
            foreach (TaxonTreeNode node in Species)
            {
                int index = Files.IndexOf(TaxonUtils.GetSoundFullPath(node));
                if (index != -1)
                {
                    if (!node.Desc.HasSound)
                        Incoherences.Add(String.Format("taxon ({0}) has no sound flag but a sound file has been found : {1}", node.Desc.RefMultiName.Main, Files[index]));
                    SpeciesWithSoundCount++;
                    Files.RemoveAt(index);
                }
                else
                {
                    if (node.Desc.HasSound)
                        Incoherences.Add(String.Format("taxon ({0}) has sound flag but no sound file has been found", node.Desc.RefMultiName.Main));
                    SpeciesWithoutSoundCount++;
                }
            }

            int SubSpeciesWithSoundCount = 0;
            int SubSpeciesWithoutSoundCount = 0;
            foreach (TaxonTreeNode node in SubSpecies)
            {
                int index = Files.IndexOf(TaxonUtils.GetSoundFullPath(node));
                if (index != -1)
                {
                    if (!node.Desc.HasSound)
                        Incoherences.Add(String.Format("taxon ({0}) has no sound flag but a sound file has been found : {1}", node.Desc.RefMultiName.Main, Files[index]));
                    SubSpeciesWithSoundCount++;
                    Files.RemoveAt(index);
                }
                else
                {
                    if (node.Desc.HasSound)
                        Incoherences.Add(String.Format("taxon ({0}) has sound flag but no sound file has been found", node.Desc.RefMultiName.Main));
                    SubSpeciesWithoutSoundCount++;
                }
            }

            string message = "Check unused sounds summary : \n";
            message += String.Format("    Total species     : {0}, {1} with sounds, {2} without\n", Species.Count, SpeciesWithSoundCount, SpeciesWithoutSoundCount);
            message += String.Format("    Total sub/species : {0}, {1} with sounds, {2} without\n", SubSpecies.Count, SubSpeciesWithSoundCount, SubSpeciesWithoutSoundCount);
            message += String.Format("    Total sounds      : {0}, {1} are unused\n", TotalSounds, Files.Count);
            if (Incoherences.Count > 0)
                message += String.Format("!! {0} incoherences found, probably needs to UpdateSoundFlags !!!\n", Incoherences.Count);
            message += String.Format("for more details, look at CheckUnusedSounds.log file");
            Loggers.WriteInformation(LogTags.Sound, message);

            try
            {
                string file = Path.Combine(TaxonUtils.GetLogPath(), "CheckUnusedSounds.log");
                if (File.Exists(file)) File.Delete(file);
                using (StreamWriter outfile = new StreamWriter(file))
                {
                    outfile.WriteLine( "CheckUnusedSounds result ( " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " )\n");
                    outfile.WriteLine(string.Format("Unused sounds ( {0} ) :", Files.Count));
                    foreach (string unusedFile in Files)
                        outfile.WriteLine(string.Format("    " + unusedFile));
                    outfile.WriteLine("\n");

                    outfile.WriteLine(string.Format("Incoherences ( {0} ) :", Incoherences.Count));
                    foreach (string line in Incoherences)
                        outfile.WriteLine("    " + line);
                    outfile.WriteLine("\n");
                }
            }
            catch (Exception e)
            {
                string error = "Exception while saving results in CheckUnusedSounds.log: \n\n";
                error += e.Message;
                if (e.InnerException != null)
                    error += "\n" + e.InnerException.Message;
                Loggers.WriteError(LogTags.Sound, error);
            }
        }
    }
}
