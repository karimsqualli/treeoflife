using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace VinceToolbox.UserControl.Graph
{
    public abstract class TNode : IDrawable
    {
        public const int InvalideId = -1;

        [DefaultValueAttribute(TNode.InvalideId)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string NType { get; set; }
        public string DataBuffer { get; set; }
        
        public TNode()
        {
            DataBuffer = null;
            Id = TNode.InvalideId;
            Name = "";
            NType = "";
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
