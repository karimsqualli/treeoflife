using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TreeOfLife.TaxonDatas
{
    [XmlRoot("SoundCollection")]
    public class SoundCollection
    {
        [XmlElement("Name")]
        public string Name;

        [XmlElement("Location")]
        public string Location;
    }
}
