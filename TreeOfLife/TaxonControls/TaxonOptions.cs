using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife
{
    [Description("Options")]
    [DisplayName("Options")]
    [Controls.IconAttribute("TaxonOptions")]
    public partial class TaxonOptions : Controls.TaxonControl
    {
        //---------------------------------------------------------------------------------
        public TaxonOptions()
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = TaxonUtils.MyConfig.Options;
        }

        //---------------------------------------------------------------------------------
        public override string ToString() { return "Options"; }

    }
}
