using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeOfLife
{
    class TaxonTreeNodeFilter : TaxonTreeNode
    {
        public TaxonTreeNodeFilter(TaxonTreeNode _original) : base()
        {
            Desc = _original.Desc;
            Visible = _original.Visible;

            _NodeInOriginal = _original;
            while (_NodeInOriginal != _NodeInOriginal.GetOriginal())
                _NodeInOriginal = _NodeInOriginal.GetOriginal();
        }

        public TaxonTreeNodeFilter() : base()
        {
            Desc = new TaxonDesc("Null graph");
            _NodeInOriginal = null;
        }

        private TaxonTreeNode _NodeInOriginal;

        public override bool IsFiltered() { return true; }
        public override TaxonTreeNode GetOriginal() { return _NodeInOriginal; }
        public override TaxonTreeNode GetFiltered(TaxonTreeNode _root) { return this; }
    }
}
