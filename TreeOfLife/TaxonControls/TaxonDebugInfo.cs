using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife
{
    [Description("Debug information, may be slow")]
    [DisplayName("Debug info")]
    [Controls.IconAttribute("TaxonDebugInfo")]
    public partial class TaxonDebugInfo : Controls.TaxonControl
    {
        //---------------------------------------------------------------------------------
        public TaxonDebugInfo()
        {
            InitializeComponent();
            ShowInfo(TaxonUtils.SelectedTaxon());
        }

        //---------------------------------------------------------------------------------
        public override string ToString() { return "Debug info"; }

        //---------------------------------------------------------------------------------
        public override void OnSelectTaxon(TaxonTreeNode _taxon) 
        {
            ShowInfo(_taxon);
        }

        //---------------------------------------------------------------------------------
        public void ShowInfo(TaxonTreeNode _taxon)
        {
            if (_taxon == null)
            {
                textBox1.Text = "";
                return;
            }

            //---------------------------------------------------------------------
            // nom du père
            TaxonTreeNode DirectParentList = _taxon.Father;

            string text = "";

            //crée une string avec le text du parent
            string DirectParent = "pas de parent direct, c'est le root de l'arbre";
            if (DirectParentList != null)
                DirectParent = "le parent direct est: " + DirectParentList.Desc.RefAllNames;
            text += DirectParent + "\r\n\r\n";

            //---------------------------------------------------------------------
            // génération : 
            text += "Generation = " + _taxon.GetGeneration().ToString() + "\r\n";
            text += "    Max level descendant = " + _taxon.GetMaxDescendentGeneration().ToString() + "\r\n";
            text += "    Last All visible generation= " + GetLastFullVisibleGeneration(_taxon).ToString() + "\r\n\r\n";

            //---------------------------------------------------------------------
            // nom de tous les enfants
            // un peu trop long maintenant
            /*
            List<Taxon> AllChildList = new List<Taxon>();
            _taxon.getAllChildrenRecursively(AllChildList);
            string AllChildListAllElements;
            if (AllChildList.Count == 0)
                AllChildListAllElements = "pas d'enfants.";
            else
            {
                AllChildListAllElements = "La liste de tous les enfants est : \r\n" + AllChildList[0].Name;
                for (int i = 1; i < AllChildList.Count; i++)
                    AllChildListAllElements += ", " + AllChildList[i].Name;
            }
            text += AllChildListAllElements + "\r\n\r\n";
            */
            //---------------------------------------------------------------------
            List<TaxonTreeNode> DirectChildList = _taxon.Children;
            string DirectChildListAllElements;
            //crée une string avec le text de tous les labels de la liste
            if (DirectChildList.Count == 0)
                DirectChildListAllElements = "pas d'enfants directs";
            else
            {
                DirectChildListAllElements = "les enfants directs sont : \r\n" + DirectChildList[0].Desc.RefMainName;
                for (int i = 1; i < DirectChildList.Count; i++)
                    DirectChildListAllElements += ", " + DirectChildList[i].Desc.RefMainName;
            }
            text += DirectChildListAllElements + "\r\n\r\n";

            //---------------------------------------------------------------------
            List<TaxonTreeNode> AllParentList = new List<TaxonTreeNode>();
            _taxon.GetAllParents(AllParentList);
            string AllParentListAllElements = "les ascendants sont : \r\n";
            //crée une string avec le text de tous les labels de la liste
            for (int i = 0; i < AllParentList.Count; i++)
                AllParentListAllElements += AllParentList[i].Desc.RefMainName + ", ";
            text += AllParentListAllElements + "\r\n\r\n";

            //-----------------------------------------------------------------------
            List<TaxonTreeNode> genList = new List<TaxonTreeNode>();
            _taxon.GetDescendentLevel(4, genList);
            string GivenGenerationChildListAllElements = "les enfants de génération 4 de ce label sont : \r\n";
            //crée une string avec le text de tous les labels de la liste
            for (int i = 0; i < genList.Count; i++)
                GivenGenerationChildListAllElements += genList[i].Desc.RefMainName + ", ";
            text += GivenGenerationChildListAllElements;

            textBox1.Text = text;
        }

        // retourne la dernière gen avec tous les taxon visible
        private int GetLastFullVisibleGeneration(TaxonTreeNode _origin, List<TaxonTreeNode> _taxons)
        {
            foreach (TaxonTreeNode t in _taxons)
            {
                if (t.HasChildren && !t.HasAllChildVisible)
                    return t.GetGeneration() - _origin.GetGeneration();
            }

            List<TaxonTreeNode> _child = new List<TaxonTreeNode>();
            foreach (TaxonTreeNode t in _taxons)
                _child.AddRange(t.Children);

            if (_child.Count == 0)
                return _taxons[0].GetGeneration() - _origin.GetGeneration();

            return GetLastFullVisibleGeneration(_origin, _child);
        }

        public int GetLastFullVisibleGeneration(TaxonTreeNode _origin)
        {
            List<TaxonTreeNode> list = new List<TaxonTreeNode> { _origin };
            return GetLastFullVisibleGeneration(_origin, list);
        }
    }
}
