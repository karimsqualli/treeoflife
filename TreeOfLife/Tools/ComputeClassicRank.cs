using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    [Description("Clean all classik rank")]
    [DisplayName("Clean classik ranks")]
    [Category("Tools/Taxons")]
    [PresentInMode(false, true)]
    class CleanClassicRank : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            TaxonUtils.OriginalRoot.ParseNodeDesc(clear);
        }

        void clear(TaxonDesc _desc)
        {
            _desc.ClassicRank = ClassicRankEnum.None;
        }
    }

    [Description("Try to deduct classic ranks from tree and taxon name")]
    [DisplayName("Compute classik ranks")]
    [Category("Tools/Taxons")]
    [PresentInMode(false, true)]
    class ComputeClassicRank : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        ComputeClassicRankData _Data = null;
        public void Activate()
        {
            _Data = new ComputeClassicRankData();
            TaxonUtils.OriginalRoot.GetAllLastChildrenRecursively(_Data.Leaves);

            string file = Path.Combine(TaxonUtils.GetLogPath(), "ComputeClassicRank.log");
            if (File.Exists(file)) File.Delete(file);

            using (_Data.Writer = new StreamWriter(file))
            {
                ComputeTwoWordsSpeciesSupSpecies();
                ComputeWithoutLatinName();
                ComputeSubSpeciesSecond();
                ComputeSpeciesFirst();
                ComputeSubGenreFirst();
                ComputeGenreFirst();

                _Data.Holozoa = TaxonUtils.OriginalRoot.FindTaxonByName("holozoa");
                if (_Data.Holozoa != null)
                    Compute4Animals();

                Compute4Others();
            }

            string message = "";
            message += String.Format("detect {0} new sub species\n", _Data.NewSubSpecies);
            message += String.Format("detect {0} new species\n", _Data.NewSpecies);
            message += String.Format("detect {0} new sub genre\n", _Data.NewSousGenre);
            message += String.Format("detect {0} new genre\n", _Data.NewGenre);
            message += String.Format("detect {0} new sous tribu\n", _Data.NewSousTribu);
            message += String.Format("detect {0} new tribus\n", _Data.NewTribu);
            message += String.Format("detect {0} new sous familles\n", _Data.NewSousFamille);
            message += String.Format("detect {0} new familles\n", _Data.NewFamille);
            message += String.Format("detect {0} new super famille\n", _Data.NewSuperFamille);
            message += String.Format("detect {0} new order\n", _Data.NewOrder);
            message += String.Format("detect {0} new without latin name\n", _Data.WithoutLatinName.Count);
            message += String.Format("for more details, look at ComputeClassicRank.log file");
            Loggers.WriteInformation(LogTags.Data, message);
        }

        //----------------------------------------------------------------------------------------
        // one more pass on subspecies before without latin name pass : 
        //   Si un taxon est en bout d’arbre ET est en au moins 3 mots et que son pere se nomme avec les 2 premiers mots
        //   >>>alors Classic rank = SousEspece et Classik rank du pere = Espece
        //
        private void ComputeTwoWordsSpeciesSupSpecies()
        {
            _Data.Writer.WriteLine("------------------------------------------------------------");
            _Data.Writer.WriteLine(String.Format("Sub species first pass, parsing {0} leaves", _Data.Leaves.Count));
            uint oldNumber = _Data.NewSubSpecies;
            foreach (TaxonTreeNode node in _Data.Leaves)
            {
                if (node.Desc.ClassicRank != ClassicRankEnum.None)
                    continue;
                string name = node.Desc.RefMultiName.Main;
                string nameNorm = name.ToLower();

                string[] splitName = nameNorm.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (splitName.Length > 2 && node.Father != null && node.Father.Desc.ClassicRank == ClassicRankEnum.None)
                {
                    string[] fatherNameSplit = node.Father.Desc.RefMultiName.Main.ToLower().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (fatherNameSplit.Length >= 2 && splitName[0] == fatherNameSplit[0] && splitName[1] == fatherNameSplit[1])
                    {
                        node.Desc.ClassicRank = ClassicRankEnum.SousEspece;
                        node.Father.Desc.ClassicRank = ClassicRankEnum.Espece;
                        _Data.NewSubSpecies++;
                        _Data.NewSpecies++;
                        _Data.Writer.WriteLine("    " + name + ", it's father became also species");
                        continue;
                    }
                }
            }
            if (oldNumber != _Data.NewSubSpecies)
                _Data.Writer.WriteLine(String.Format("  {0} new sub species and new species", _Data.NewSubSpecies - oldNumber));
            else
                _Data.Writer.WriteLine("  No new species / sub species detected");
        }

        //----------------------------------------------------------------------------------------
        // first pass sub species : 
        //   Si un taxon est en bout d’arbre ET (est en trois mots OU contient le mot « subsp » OU le mot « var » dedans*) ET ne contient pas la string «  sp » ** ET ne contient pas de nombres
        //   >>>alors Classic rank = SousEspece
        //   * : avec un espace (ou un underscore) de chaque côté de «subsp» ou de «var»
        //   ** : avec un espace (ou un underscore) de chaque côté de «sp» parce qu’il y a par exemple des espèces comme « Pentaphragma_sp_Duangjai_49 »
        //
        private void ComputeSubSpeciesSecond()
        {
            _Data.Writer.WriteLine("------------------------------------------------------------");
            _Data.Writer.WriteLine(String.Format("Sub species second pass, parsing {0} leaves", _Data.Leaves.Count));
            uint oldNumber = _Data.NewSubSpecies;
            foreach (TaxonTreeNode node in _Data.Leaves)
            {
                if (node.Desc.ClassicRank != ClassicRankEnum.None)
                    continue;
                string name = node.Desc.RefMultiName.Main;
                if (_Data.RegexNumber.IsMatch(name)) continue;

                string nameNorm = name.ToLower();
                if (nameNorm.Contains(" sp ")) continue;
                if (!nameNorm.Contains(" subsp ") && !nameNorm.Contains(" var ") && nameNorm.Split(_Data.SpaceSep).Length != 3) continue;

                node.Desc.ClassicRank = ClassicRankEnum.SousEspece;
                if (oldNumber == _Data.NewSubSpecies)
                    _Data.Writer.WriteLine("  New sub species detected:");
                _Data.NewSubSpecies++;
                _Data.Writer.WriteLine("    " + name);
            }
            if (oldNumber != _Data.NewSubSpecies)
                _Data.Writer.WriteLine(String.Format("  {0} new sub species", _Data.NewSubSpecies - oldNumber));
            else
                _Data.Writer.WriteLine("  No new sub species detected");
        }

        //----------------------------------------------------------------------------------------
        // first pass species : 
        // Si un taxon  
        // -          n’a pas déjà de Classic Rank ET est en bout d’arbre ET ( est en deux mots OU contient la string « sp ») *
        // * avec un espace de chaque côté de « sp »
        // >>>alors Classic rank = Espece
        //
        private void ComputeSpeciesFirst()
        {
            _Data.Writer.WriteLine("------------------------------------------------------------");
            _Data.Writer.WriteLine(String.Format("Species first pass, parsing {0} leaves", _Data.Leaves.Count));
            uint oldNumber = _Data.NewSpecies;
            foreach (TaxonTreeNode node in _Data.Leaves)
            {
                TaxonTreeNode nodeToTreat = node;
                string name;
                if (node.Desc.ClassicRank == ClassicRankEnum.SousEspece)
                {
                    nodeToTreat = node.Father;
                    if (nodeToTreat.Desc.ClassicRank != ClassicRankEnum.None)
                        continue;
                    name = nodeToTreat.Desc.RefMultiName.Main;
                }
                else if (node.Desc.ClassicRank == ClassicRankEnum.None)
                {
                    name = node.Desc.RefMultiName.Main;
                    string nameNorm = name.ToLower();
                    if (!nameNorm.Contains(" sp ") && nameNorm.Split(_Data.SpaceSep).Length != 2) continue;
                }
                else
                    continue;

                nodeToTreat.Desc.ClassicRank = ClassicRankEnum.Espece;
                if (oldNumber == _Data.NewSpecies)
                    _Data.Writer.WriteLine("  New species detected:");
                _Data.NewSpecies++;
                _Data.Writer.WriteLine("    " + name);
            }

            if (oldNumber != _Data.NewSpecies)
                _Data.Writer.WriteLine(String.Format("  {0} new species", _Data.NewSpecies - oldNumber));
            else
                _Data.Writer.WriteLine("  No new species detected");
        }
        
        //----------------------------------------------------------------------------------------
        // first pass sub genre
        private void ComputeSubGenreFirst()
        {
            _Data.Writer.WriteLine("------------------------------------------------------------");
            _Data.Writer.WriteLine(String.Format("Sub genre first pass, parsing all nodes"));

            uint oldNumber = _Data.NewSousGenre;
            TaxonUtils.OriginalRoot.ParseNodeDesc(ComputeSubGenreFirst);
            if (oldNumber != _Data.NewSousGenre)
                _Data.Writer.WriteLine(String.Format("  {0} new sub genre", _Data.NewSousGenre - oldNumber));
            else
                _Data.Writer.WriteLine("  No new sub genre detected");
        }

        void ComputeSubGenreFirst(TaxonDesc _desc)
        {
            if (_desc.ClassicRank != ClassicRankEnum.None) return;
            if (!_desc.RefMultiName.Main.ToLower().Contains("subgenus")) return;
            _desc.ClassicRank = ClassicRankEnum.SousGenre;
            _Data.NewSousGenre++;
            _Data.Writer.WriteLine("    " + _desc.RefMultiName.Main);
        }


        //----------------------------------------------------------------------------------------
        // first pass : 
        private void ComputeGenreFirst()
        {
            _Data.Writer.WriteLine("------------------------------------------------------------");
            _Data.Writer.WriteLine(String.Format("Genre first pass, parsing all species"));
            uint oldNumber = _Data.NewGenre;

            List<TaxonTreeNode> species = new List<TaxonTreeNode>();
            TaxonUtils.OriginalRoot.GetAllChildrenRecursively(species, ClassicRankEnum.Espece);

            foreach (TaxonTreeNode spec in species)
            {
                int index = spec.Desc.RefMultiName.Main.IndexOf(' ');
                if (index == -1) continue;
                string genreName = spec.Desc.RefMultiName.Main.Substring(0, index);

                TaxonTreeNode parent = spec.Father;
                TaxonTreeNode parentWithSameName = null;
                while (parent != null)
                {
                    if (parent.Desc.ClassicRank == ClassicRankEnum.Genre)
                        break;
                    if (parent.Desc.RefMultiName.Main == genreName)
                        parentWithSameName = parent;
                    parent = parent.Father;
                }
                if (parent != null) continue;
                if (parentWithSameName == null) continue;
                parentWithSameName.Desc.ClassicRank = ClassicRankEnum.Genre;
                _Data.NewGenre++;
                _Data.Writer.WriteLine("    " + parentWithSameName.Desc.RefMultiName.Main);
            }

            if (oldNumber != _Data.NewGenre)
                _Data.Writer.WriteLine(String.Format("  {0} new genre", _Data.NewGenre - oldNumber));
            else
                _Data.Writer.WriteLine("  No new genre detected");

        }

        //----------------------------------------------------------------------------------------
        // animals  
        private void Compute4Animals()
        {
            if (_Data.Holozoa == null) return;
            
            _Data.Writer.WriteLine("------------------------------------------------------------");
            _Data.Writer.WriteLine(String.Format("Animal pass, parsing all Holozoa nodes"));

            TaxonUtils.OriginalRoot.ParseNode(Compute4AnimalsFromGenre);
            TaxonUtils.OriginalRoot.ParseNode(Compute4AnimalsFromFamille);
            
            _Data.Writer.WriteLine(String.Format("  {0} new sous tribu ", _Data.AnimalSousTribu.Count ));
            _Data.Writer.WriteLine(String.Format("  {0} new tribu ", _Data.AnimalTribu.Count));
            _Data.Writer.WriteLine(String.Format("  {0} new sous famille", _Data.AnimalSousFamille.Count));
            _Data.Writer.WriteLine(String.Format("  {0} new famille", _Data.AnimalFamille.Count));
            _Data.Writer.WriteLine(String.Format("  {0} new super famille", _Data.AnimalSuperFamille.Count));
            _Data.Writer.WriteLine(String.Format("  {0} new ordre", _Data.AnimalOrdre.Count));

            if (_Data.AnimalSousTribu.Count > 0)
            {
                _Data.Writer.WriteLine(String.Format("  {0} new sous tribu ", _Data.AnimalSousTribu.Count));
                foreach (TaxonTreeNode node in _Data.AnimalSousTribu)
                    _Data.Writer.WriteLine("      " + node.Desc.RefMultiName.Main);
            }

            if (_Data.AnimalTribu.Count > 0)
            {
                _Data.Writer.WriteLine(String.Format("  {0} new tribu ", _Data.AnimalTribu.Count));
                foreach (TaxonTreeNode node in _Data.AnimalTribu)
                    _Data.Writer.WriteLine("      " + node.Desc.RefMultiName.Main);
            }

            if (_Data.AnimalSousFamille.Count > 0)
            {
                _Data.Writer.WriteLine(String.Format("  {0} new sous famille", _Data.AnimalSousFamille.Count));
                foreach (TaxonTreeNode node in _Data.AnimalSousFamille)
                    _Data.Writer.WriteLine("      " + node.Desc.RefMultiName.Main);
            }

            if (_Data.AnimalFamille.Count > 0)
            {
                _Data.Writer.WriteLine(String.Format("  {0} new famille", _Data.AnimalFamille.Count));
                foreach (TaxonTreeNode node in _Data.AnimalFamille)
                    _Data.Writer.WriteLine("      " + node.Desc.RefMultiName.Main);
            }

            if (_Data.AnimalSuperFamille.Count > 0)
            {
                _Data.Writer.WriteLine(String.Format("  {0} new super famille", _Data.AnimalSuperFamille.Count));
                foreach (TaxonTreeNode node in _Data.AnimalSuperFamille)
                    _Data.Writer.WriteLine("      " + node.Desc.RefMultiName.Main);
            }

            if (_Data.AnimalOrdre.Count > 0)
            {
                _Data.Writer.WriteLine(String.Format("  {0} new ordre", _Data.AnimalOrdre.Count));
                foreach (TaxonTreeNode node in _Data.AnimalOrdre)
                    _Data.Writer.WriteLine("      " + node.Desc.RefMultiName.Main);
            }
        }

        void Compute4AnimalsFromGenre(TaxonTreeNode _node)
        {
            if (_node.Desc.ClassicRank != ClassicRankEnum.None) return;

            if (_node.Desc.RefMultiName.Main.Contains(' '))
                return;

            if (_node.Desc.RefMultiName.Main.EndsWith("ina"))
            {
                List<TaxonTreeNode> descendants = new List<TaxonTreeNode>();
                _node.GetAllChildrenRecursively(descendants, ClassicRankEnum.Genre);
                if (descendants.Count > 0)
                {
                    _node.Desc.ClassicRank = ClassicRankEnum.SousTribu;
                    _Data.NewSousTribu++;
                    _Data.AnimalSousTribu.Add(_node);
                }
            }
            else if (_node.Desc.RefMultiName.Main.EndsWith("ini"))
            {
                List<TaxonTreeNode> descendants = new List<TaxonTreeNode>();
                _node.GetAllChildrenRecursively(descendants, ClassicRankEnum.Genre);
                if (descendants.Count > 0)
                {
                    bool ok = true;
                    _node.GetAllChildrenRecursively(descendants);
                    foreach (TaxonTreeNode child in descendants)
                    {
                        if (child.Desc.ClassicRank == ClassicRankEnum.Genre) continue;
                        if (child.Desc.ClassicRank == ClassicRankEnum.Espece) continue;
                        if (!child.Desc.RefMultiName.Main.EndsWith("ini")) continue;
                        ok = false;
                        break;
                    }

                    if (ok)
                    {
                        _node.Desc.ClassicRank = ClassicRankEnum.Tribu;
                        _Data.NewTribu++;
                        _Data.AnimalTribu.Add(_node);
                    }
                }
            }
            else if (_node.Desc.RefMultiName.Main.EndsWith("inae"))
            {
                List<TaxonTreeNode> descendants = new List<TaxonTreeNode>();
                _node.GetAllChildrenRecursively(descendants, ClassicRankEnum.Genre);
                if (descendants.Count > 0)
                {
                    _node.Desc.ClassicRank = ClassicRankEnum.SousFamille;
                    _Data.NewSousFamille++;
                    _Data.AnimalSousFamille .Add(_node);
                }
            }
            else if (_node.Desc.RefMultiName.Main.EndsWith("idae"))
            {
                List<TaxonTreeNode> descendants = new List<TaxonTreeNode>();
                _node.GetAllChildrenRecursively(descendants, ClassicRankEnum.Genre);
                if (descendants.Count > 0)
                {
                    _node.Desc.ClassicRank = ClassicRankEnum.Famille;
                    _Data.NewFamille++;
                    _Data.AnimalFamille.Add(_node);
                }
            }
        }

        void Compute4AnimalsFromFamille(TaxonTreeNode _node)
        {
            if (_node.Desc.ClassicRank != ClassicRankEnum.None) return;

            if (_node.Desc.RefMultiName.Main.Contains(' '))
                return;

            if (_node.Desc.RefMultiName.Main.EndsWith("oidea"))
            {
                List<TaxonTreeNode> descendants = new List<TaxonTreeNode>();
                _node.GetAllChildrenRecursively(descendants, ClassicRankEnum.Famille);
                if (descendants.Count > 0)
                {
                    _node.Desc.ClassicRank = ClassicRankEnum.SuperFamille;
                    _Data.NewSuperFamille++;
                    _Data.AnimalSuperFamille.Add(_node);
                }
            }
            else if (_node.Desc.RefMultiName.Main.EndsWith("formes") ||_node.Desc.RefMultiName.Main.EndsWith("formia"))
            {
                List<TaxonTreeNode> descendants = new List<TaxonTreeNode>();
                _node.GetAllChildrenRecursively(descendants, ClassicRankEnum.Famille);
                if (descendants.Count > 0)
                {
                    _node.Desc.ClassicRank = ClassicRankEnum.Ordre;
                    _Data.NewOrder++;
                    _Data.AnimalOrdre.Add(_node);
                }
            }
        }

        //----------------------------------------------------------------------------------------
        // others  
        private void Compute4Others()
        {
            _Data.Writer.WriteLine("------------------------------------------------------------");
            _Data.Writer.WriteLine(String.Format("Other pass, parsing all nodes"));

            TaxonUtils.OriginalRoot.ParseNode(Compute4OthersFromGenre);
            TaxonUtils.OriginalRoot.ParseNode(Compute4OthersFromFamille);

            _Data.Writer.WriteLine(String.Format("  {0} new sous famille", _Data.OtherSousFamille.Count));
            _Data.Writer.WriteLine(String.Format("  {0} new famille", _Data.OtherFamille.Count));
            _Data.Writer.WriteLine(String.Format("  {0} new super famille", _Data.OtherSuperFamille.Count));
            _Data.Writer.WriteLine(String.Format("  {0} new ordre", _Data.OtherOrdre.Count));

            if (_Data.OtherSousFamille.Count > 0)
            {
                _Data.Writer.WriteLine(String.Format("  {0} new sous famille", _Data.OtherSousFamille.Count));
                foreach (TaxonTreeNode node in _Data.OtherSousFamille)
                    _Data.Writer.WriteLine("      " + node.Desc.RefMultiName.Main);
            }

            if (_Data.OtherFamille.Count > 0)
            {
                _Data.Writer.WriteLine(String.Format("  {0} new famille", _Data.OtherFamille.Count));
                foreach (TaxonTreeNode node in _Data.OtherFamille)
                    _Data.Writer.WriteLine("      " + node.Desc.RefMultiName.Main);
            }

            if (_Data.OtherSuperFamille.Count > 0)
            {
                _Data.Writer.WriteLine(String.Format("  {0} new super famille", _Data.OtherSuperFamille.Count));
                foreach (TaxonTreeNode node in _Data.OtherSuperFamille)
                    _Data.Writer.WriteLine("      " + node.Desc.RefMultiName.Main);
            }

            if (_Data.OtherOrdre.Count > 0)
            {
                _Data.Writer.WriteLine(String.Format("  {0} new ordre", _Data.OtherOrdre.Count));
                foreach (TaxonTreeNode node in _Data.OtherOrdre)
                    _Data.Writer.WriteLine("      " + node.Desc.RefMultiName.Main);
            }
        }

        void Compute4OthersFromGenre(TaxonTreeNode _node)
        {
            if (_node.Desc.ClassicRank != ClassicRankEnum.None) return;

            if (_node.Desc.RefMultiName.Main.Contains(' '))
                return;

            List<TaxonTreeNode> parents = new List<TaxonTreeNode>();
            _node.GetAllParents(parents, true );
            if (parents.Contains(_Data.Holozoa)) 
                return;

            if (_node.Desc.RefMultiName.Main.EndsWith("eae") && !_node.Desc.RefMultiName.Main.EndsWith("aceae") && !_node.Desc.RefMultiName.Main.EndsWith("oideae"))
            {
                List<TaxonTreeNode> descendants = new List<TaxonTreeNode>();
                _node.GetAllChildrenRecursively(descendants, ClassicRankEnum.Genre);
                if (descendants.Count > 0)
                {
                    _node.Desc.ClassicRank = ClassicRankEnum.SousFamille;
                    _Data.NewSousFamille++;
                    _Data.OtherSousFamille.Add(_node);
                }
            }
            else if (_node.Desc.RefMultiName.Main.EndsWith("aceae") || _node.Desc.RefMultiName.Main.EndsWith("yceae"))
            {
                List<TaxonTreeNode> descendants = new List<TaxonTreeNode>();
                _node.GetAllChildrenRecursively(descendants, ClassicRankEnum.Genre);
                if (descendants.Count > 0)
                {
                    _node.Desc.ClassicRank = ClassicRankEnum.Famille;
                    _Data.NewFamille++;
                    _Data.OtherFamille.Add(_node);
                }
            }
        }

        void Compute4OthersFromFamille(TaxonTreeNode _node)
        {
            if (_node.Desc.ClassicRank != ClassicRankEnum.None) return;

            if (_node.Desc.RefMultiName.Main.Contains(' '))
                return;

            List<TaxonTreeNode> parents = new List<TaxonTreeNode>();
            _node.GetAllParents(parents, true);
            if (parents.Contains(_Data.Holozoa))
                return;

            if (_node.Desc.RefMultiName.Main.EndsWith("oideae"))
            {
                List<TaxonTreeNode> descendants = new List<TaxonTreeNode>();
                _node.GetAllChildrenRecursively(descendants, ClassicRankEnum.Famille);
                if (descendants.Count > 0)
                {
                    _node.Desc.ClassicRank = ClassicRankEnum.SuperFamille;
                    _Data.NewSuperFamille++;
                    _Data.OtherSuperFamille.Add(_node);
                }
            }
            else if (_node.Desc.RefMultiName.Main.EndsWith("ales") || _node.Desc.RefMultiName.Main.EndsWith("formia"))
            {
                List<TaxonTreeNode> descendants = new List<TaxonTreeNode>();
                _node.GetAllChildrenRecursively(descendants, ClassicRankEnum.Famille);
                if (descendants.Count > 0)
                {
                    _node.Desc.ClassicRank = ClassicRankEnum.Ordre;
                    _Data.NewOrder++;
                    _Data.OtherOrdre.Add(_node);
                }
            }
        }

        //----------------------------------------------------------------------------------------
        void ComputeWithoutLatinName()
        {
            _Data.Writer.WriteLine("------------------------------------------------------------");
            _Data.Writer.WriteLine(String.Format("Detection of WithoutLatinName"));

            TaxonUtils.OriginalRoot.ParseNode(ComputeWLN);

            _Data.Writer.WriteLine(String.Format("  {0} new WithouLatinName : ", _Data.WithoutLatinName.Count));
            if (_Data.WithoutLatinName.Count > 0)
            {
                foreach (TaxonTreeNode node in _Data.WithoutLatinName)
                    _Data.Writer.WriteLine("      " + node.Desc.RefMultiName.Main);
            }
        }

        bool IsWithoutLatinName(string _name)
        {
            if (_name.StartsWith("'")) return true;
            if (_Data.RegexNumber.IsMatch(_name)) return true;
            _name = _name.ToLower();
            if (_name.Contains("uncultured")) return true;
            if (_name.Contains("cf ")) return true;
            if (_name.Contains(" sp ")) return true;
            if (_name.Contains("candidatus")) return true;
            if (_name.Contains("candidate")) return true;
            return false;
        }

        void ComputeWLN(TaxonTreeNode _node)
        {
            if (_node.Desc.ClassicRank != ClassicRankEnum.None) return;

            if (IsWithoutLatinName(_node.Desc.RefMultiName.Main))
            {
                _node.Desc.ClassicRank = ClassicRankEnum.WithoutLatinName;
                _Data.WithoutLatinName.Add(_node);
            }
        }

        //----------------------------------------------------------------------------------------
        internal class ComputeClassicRankData
        {
            public List<TaxonTreeNode> Leaves = new List<TaxonTreeNode>();

            public TaxonTreeNode Holozoa = null;

            public StreamWriter Writer;

            public uint NewSubSpecies = 0;
            public uint NewSpecies = 0;
            public uint NewSousGenre = 0;
            public uint NewGenre = 0;
            public uint NewSousTribu = 0;
            public uint NewTribu = 0;
            public uint NewSousFamille = 0;
            public uint NewFamille = 0;
            public uint NewSuperFamille = 0;
            public uint NewOrder = 0;

            public Char[] SpaceSep = new Char[] { ' ' };
            public Regex RegexNumber = new Regex(@"\d", RegexOptions.Compiled);


            public List<TaxonTreeNode> AnimalSousTribu = new List<TaxonTreeNode>();
            public List<TaxonTreeNode> AnimalTribu = new List<TaxonTreeNode>();
            public List<TaxonTreeNode> AnimalSousFamille = new List<TaxonTreeNode>();
            public List<TaxonTreeNode> AnimalFamille = new List<TaxonTreeNode>();
            public List<TaxonTreeNode> AnimalSuperFamille = new List<TaxonTreeNode>();
            public List<TaxonTreeNode> AnimalOrdre = new List<TaxonTreeNode>();

            public List<TaxonTreeNode> OtherSousFamille = new List<TaxonTreeNode>();
            public List<TaxonTreeNode> OtherFamille = new List<TaxonTreeNode>();
            public List<TaxonTreeNode> OtherSuperFamille = new List<TaxonTreeNode>();
            public List<TaxonTreeNode> OtherOrdre = new List<TaxonTreeNode>();

            public List<TaxonTreeNode> WithoutLatinName = new List<TaxonTreeNode>();
        }
    }
}
