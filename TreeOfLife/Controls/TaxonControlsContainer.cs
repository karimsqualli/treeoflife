using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Controls
{
    public partial class TaxonControlsContainer : UserControl
    {
        private System.ComponentModel.IContainer components = null;
        public TaxonControlsContainer()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        List<UserControl> UserControls = null;

        public bool IsMainGraph
        {
            get { return (UserControls != null) && UserControls.Count == 1 && (UserControls[0] is TaxonGraph) ;}
        }
    }

    public partial class TaxonSplitter : Splitter
    {
        public TaxonSplitter()
        {
        }


        
    }
}
