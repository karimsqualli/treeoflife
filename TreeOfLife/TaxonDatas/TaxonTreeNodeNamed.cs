using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeOfLife
{
    public class TaxonTreeNodeNamed : IComparable
    {
        public string Name { get; private set; }
        public TaxonTreeNode TaxonTreeNode { get; private set; }

        public TaxonTreeNodeNamed(string _name, TaxonTreeNode _taxon)
        {
            Name = _name;
            TaxonTreeNode = _taxon;
        }

        public override string ToString()
        {
            return Name;
        }

        //=========================================================================================
        // comparison interface
        //

        //---------------------------------------------------------------------------------
        public int CompareTo(object obj)
        {
            if (!(obj is TaxonTreeNodeNamed)) return 0;
            return Compare(this, obj as TaxonTreeNodeNamed);
        }

        //---------------------------------------------------------------------------------
        public static int Compare(TaxonTreeNodeNamed t1, TaxonTreeNodeNamed t2)
        {
            return String.Compare(t1.Name, t2.Name);
        }
    }
}
