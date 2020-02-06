using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace TreeOfLife
{
    //=============================================================================================
    public class Synonym
    {
        public Synonym(TaxonDesc _desc)
        {
            _Desc = _desc;
            AllSynonyms = string.Join( Helpers.MultiName.SeparatorAsString, _desc.RefMultiName.GetSynonymsArray());
        }

        public Synonym(string _name, string _synonym )
        {
            _Name = _name;
            AllSynonyms = _synonym;
        }

        private TaxonDesc _Desc = null;
        public TaxonDesc Desc
        {
            get { return _Desc; }
            set { _Desc = value; }
        }

        private string _Name = "";
        public string Name
        {
            get { return _Desc == null ? _Name : _Desc.RefMultiName.Main; }
            set { _Name = value == null ? "" : value.Trim(); }
        }

        public List<string> Synonyms;

        public string AllSynonyms
        {
            get { return Synonyms == null ? "" : string.Join(",", Synonyms.ToArray()); }
            set
            {
                Synonyms = new List<string>();
                if (!String.IsNullOrEmpty(value))
                {
                    string[] words = value.Split(new Char[] { ',' });
                    foreach (string word in words)
                    {
                        string normalized = word.ToLower().Trim();
                        if (String.IsNullOrEmpty(normalized)) continue;
                        Synonyms.Add(normalized);
                    }
                }
            }
        }
    }

    //=============================================================================================
    public class Synonyms : List<Synonym>
    {
        public Synonyms(TaxonTreeNode _root)
        {
            _root.ParseNodeDesc((d) => { if (d.RefMultiName.HasSynonym) this.Add(new Synonym(d)); } );
        }

        //---------------------------------------------------------------------------------
        public void Edit()
        {
            TaxonDialog.SynonymsManagerDialog dlg = new TaxonDialog.SynonymsManagerDialog(this);
            dlg.ShowDialog();
        }
    }
}
