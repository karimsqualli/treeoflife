using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TreeOfLife.GUI
{
    public class ControlContainerSplitterDesc
    {
        //-------------------------------------------------------------------
        public ControlContainerSplitterDesc()
        {
            SplitterDistance = 200;
            Orientation = Orientation.Vertical;
            Panel1TabDesc = null;
            Panel2TabDesc = null;
            Panel1SplitterDesc = null;
            Panel2SplitterDesc = null;
        }

        //-------------------------------------------------------------------
        public bool IsValid()
        {
            if (Panel1TabDesc == null && Panel1SplitterDesc == null) return false;
            if (Panel2TabDesc == null && Panel2SplitterDesc == null) return false;
            return true;
        }

        //-------------------------------------------------------------------
        // Datas
        //

        public Orientation Orientation { get; set;}
        public int SplitterDistance { get; set; }

        [XmlElement("panel1TabDesc")]
        public ControlContainerTabsDesc Panel1TabDesc { get; set; }
        [XmlElement("panel2TabDesc")]
        public ControlContainerTabsDesc Panel2TabDesc { get; set; }
        [XmlElement("panel1SplitterDesc")]
        public ControlContainerSplitterDesc Panel1SplitterDesc { get; set; }
        [XmlElement("panel2SplitterDesc")]
        public ControlContainerSplitterDesc Panel2SplitterDesc { get; set; }

        //-------------------------------------------------------------------
        public ControlContainerSplitter Rebuild()
        {
            ControlContainerSplitter split = new ControlContainerSplitter
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation
            };

            if (Panel1SplitterDesc != null)
                split.Panel1.Controls.Add(Panel1SplitterDesc.Rebuild());
            else if (Panel1TabDesc != null)
                split.Panel1.Controls.Add(Panel1TabDesc.Rebuild());

            if (Panel2SplitterDesc != null)
                split.Panel2.Controls.Add(Panel2SplitterDesc.Rebuild());
            else if (Panel2TabDesc != null)
                split.Panel2.Controls.Add(Panel2TabDesc.Rebuild());

            split.SplitterDistance = SplitterDistance;

            return split;
        }

        //-------------------------------------------------------------------
        public void AfterAdd(ControlContainerSplitter _split)
        {
            if (_split == null) return;

            if (SplitterDistance >= _split.Panel1MinSize && SplitterDistance <= _split.Width - _split.Panel2MinSize )
                _split.SplitterDistance = SplitterDistance;

            if (_split.Panel1.Controls.Count == 1)
            {
                if (Panel1SplitterDesc != null)
                {
                    ControlContainerSplitter split = _split.Panel1.Controls[0] as ControlContainerSplitter;
                    Panel1SplitterDesc.AfterAdd(split);
                }
            }

            if (_split.Panel2.Controls.Count == 1)
            {
                if (Panel2SplitterDesc != null)
                {
                    ControlContainerSplitter split = _split.Panel2.Controls[0] as ControlContainerSplitter;
                    Panel2SplitterDesc.AfterAdd(split);
                }
            }
        }

        //-------------------------------------------------------------------
        public static ControlContainerSplitterDesc FromTaxonSplitterContainer(ControlContainerSplitter _split)
        {
            ControlContainerSplitterDesc desc = new ControlContainerSplitterDesc
            {
                Orientation = _split.Orientation,
                SplitterDistance = _split.SplitterDistance
            };

            if (_split.Panel1.Controls[0] is ControlContainerTabs)
                desc.Panel1TabDesc = ControlContainerTabsDesc.FromTaxonTabControls(_split.Panel1.Controls[0] as ControlContainerTabs);
            else if (_split.Panel1.Controls[0] is ControlContainerSplitter)
                desc.Panel1SplitterDesc = FromTaxonSplitterContainer(_split.Panel1.Controls[0] as ControlContainerSplitter);

            if (_split.Panel2.Controls[0] is ControlContainerTabs)
                desc.Panel2TabDesc = ControlContainerTabsDesc.FromTaxonTabControls(_split.Panel2.Controls[0] as ControlContainerTabs);
            else if (_split.Panel2.Controls[0] is ControlContainerSplitter)
                desc.Panel2SplitterDesc = FromTaxonSplitterContainer(_split.Panel2.Controls[0] as ControlContainerSplitter);
            return desc;
        }
    }
}
