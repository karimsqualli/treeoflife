
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace TreeOfLife.Tools
{
    [Description("List all taxon with no children and that are not species or subspecies")]
    [DisplayName("List non species leaves")]
    [Category("Tools/Taxons")]
    //[PresentInMode(false, true)]
    class TaxonListOfNonSpeciesLeaves : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        List<TaxonTreeNode> _Result;
        public void Activate()
        {
            _Result = new List<TaxonTreeNode>();
            TaxonUtils.OriginalRoot.ParseNode(CheckLeaf);

            TaxonList list = new TaxonList { HasFile = true, FileName = Path.Combine(TaxonUtils.GetLogPath(), "ListOfNonSpeciesLeaves.lot") };
            list.FromTaxonTreeNodeList(_Result);
            list.Save(false, TaxonList.FileFilterIndexEnum.ListOfTaxons);

            string message = string.Format("{0} taxons with no children and that are not species or sub species", _Result.Count);

            string logFile = Path.Combine(TaxonUtils.GetLogPath(), "ListOfNonSpeciesLeaves.log");
            using (StreamWriter log = new StreamWriter(logFile))
            {
                log.WriteLine(message);
                log.WriteLine();
                foreach (TaxonTreeNode node in _Result)
                    log.WriteLine(node.GetHierarchicalName());
            }
            message += string.Format("\ntaxons found are saved as filter list in {0}", list.FileName);
            message += string.Format("\nfor more details, look at {0}", logFile);
            Loggers.WriteInformation(LogTags.Data, message);
        }

        void CheckLeaf(TaxonTreeNode _taxon)
        {
            if (_taxon.HasChildren) return;
            if (_taxon.Desc.ClassicRank == ClassicRankEnum.Espece || _taxon.Desc.ClassicRank == ClassicRankEnum.SousEspece) return;
            if (_taxon.Desc.ClassicRank == ClassicRankEnum.WithoutLatinName || _taxon.Desc.ClassicRank == ClassicRankEnum.None) return;
            _Result.Add(_taxon);
        }
    }
}

