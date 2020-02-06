using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;

namespace TreeOfLife
{
    public enum ClassicRankEnum
    {
        [Description("<None>"), Category("none")]
        None,
        [Description("Incertae sedis"), Category("none")]
        IncertaeSedis,
        [Description("Domaine"), Category("Domaine")]
        Domaine,
        [Description("Sous-Domaine"), Category("Domaine")]
        SousDomaine,
        [Description("Super-Règne"), Category("Règne")]
        SuperRegne,
        [Description("Règne"), Category("Règne")]
        Regne,
        [Description("Sous-Règne"), Category("Règne")]
        SousRègne,
        [Description("Rameau"), Category("Règne")]
        Rameau,
        [Description("Infra-Règne"), Category("Règne")]
        InfraRegne,
        [Description("Super-Embranchement"), Category("Embranchement")]
        SuperEmbranchement,
        [Description("Embranchement"), Category("Embranchement")]
        Embranchement,
        [Description("Sous-Embranchement"), Category("Embranchement")]
        SousEmbranchement,
        [Description("Infra-Embranchement"), Category("Embranchement")]
        InfraEmbranchement,
        [Description("Micro-Embranchement"), Category("Embranchement")]
        MicroEmbranchement,
        [Description("Super-Classe"), Category("Classe")]
        SuperClasse,
        [Description("Classe"), Category("Classe")]
        Classe,
        [Description("Sous-Classe"), Category("Classe")]
        SousClasse,
        [Description("Infra-Classe"), Category("Classe")]
        InfraClasse,
        [Description("Micro-Classe"), Category("Classe")]
        MicroClasse,
        [Description("Super-Ordre"), Category("Ordre")]
        SuperOrdre,
        [Description("Ordre"), Category("Ordre")]
        Ordre,
        [Description("Sous-Ordre"), Category("Ordre")]
        SousOrdre,
        [Description("Infra-Ordre"), Category("Ordre")]
        InfraOrdre,
        [Description("Micro-Ordre"), Category("Ordre")]
        MicroOrdre,
        [Description("Super-Famille"), Category("Famille")]
        SuperFamille,
        [Description("Famille"), Category("Famille")]
        Famille,
        [Description("Sous-Famille"), Category("Famille")]
        SousFamille,
        [Description("Tribu"), Category("Famille")]
        Tribu,
        [Description("Sous-Tribu"), Category("Famille")]
        SousTribu,
        [Description("Genre"), Category("Genre")]
        Genre,
        [Description("Sous-Genre"), Category("Genre")]
        SousGenre,
        [Description("Section"), Category("Genre")]
        Section,
        [Description("Espèce"), Category("Genre")]
        Espece,
        [Description("Sous-Espèce"), Category("Genre")]
        SousEspece,
        [Description("Without Latin Name"), Category("Misc")]
        WithoutLatinName
    }

    static public class ClassicRankEnumExt
    {
        static List<int> _ClassicRankColumns = null;

        static public List<int> ClassicRankColumns
        {
            get 
            {
                if (_ClassicRankColumns == null)
                {
                    _ClassicRankColumns = new List<int>();
                    foreach(ClassicRankEnum rank in Enum.GetValues(typeof(ClassicRankEnum)))
                    {
                        if (rank == ClassicRankEnum.None || rank == ClassicRankEnum.IncertaeSedis)
                            _ClassicRankColumns.Add(-1);
                        else
                            _ClassicRankColumns.Add(1 + (int)rank - (int)ClassicRankEnum.Domaine);
                    }
                }
                return _ClassicRankColumns;
            }
        }

        static Dictionary<string, ClassicRankEnum> dico;

        static void BuildDico()
        {
            dico = new Dictionary<string, ClassicRankEnum>();

            foreach (ClassicRankEnum rank in Enum.GetValues(typeof(ClassicRankEnum)))
            {
                string rankName = rank.ToString().ToLower();
                dico[rankName] = rank;

                FieldInfo fi = rank.GetType().GetField(rank.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    string otherName = attributes[0].Description.ToLower();
                    if (otherName != rankName)
                        dico[otherName] = rank;
                }
            }
        }

        public static ClassicRankEnum FromString(string _value)
        {
            if (dico == null) BuildDico();
            if (dico.TryGetValue(_value.Trim().ToLower(), out ClassicRankEnum rank))
                return rank;
            return ClassicRankEnum.None;
        }
    }

    public enum FlagsEnum : uint
    {
        Barren                      = 0x00000001,       // ott flag :  there are only higher taxa at and below this node, no species or unranked tips
        notused_0x00000002          = 0x00000002,   // unused
        Environmental               = 0x00000004,       // ott flag : 'Incertae sedis'-like flags
        EnvironmentalInherited      = 0x00000008,       // ott flag : 'Incertae sedis'-like flags
        Extinct                     = 0x00000010,   // TOL FLAG && ott flag :  node is annotated as extinct (usually but not always by IRMNG)
        ExtinctInherited            = 0x00000020,       // ott flag : descends from a node flagged extinct. 
        IncludeIdInFilenames        = 0x00000040,   // TOL FLAG
        Hidden                      = 0x00000080,       // ott flag : marked hidden due to Open Tree curatorial decision (e.g. microbes from GBIF)
        HiddenInherited             = 0x00000100,       // ott flag : descends from node flagged hidden
        Hybrid                      = 0x00000200,       // ott flag :  taxon name contains "hybrid" or " x " indicating that it is a hybrid. Also, any node descended from such a node.
        IncertaeSedis               = 0x00000400,       // ott flag : 'Incertae sedis'-like flags
        IncertaeSedisInherited      = 0x00000800,       // ott flag : 'Incertae sedis'-like flags
        Inconsistent                = 0x00001000,       // ott flag : 'Incertae sedis'-like flags
        Infraspecific               = 0x00002000,       // ott flag : descends from a node with rank "species"
        MajorRankConflict           = 0x00004000,       // ott flag : 'Incertae sedis'-like flags
        MajorRankConflictInherited  = 0x00008000,       // ott flag : 'Incertae sedis'-like flags
        Merged                      = 0x00010000,       // ott flag : 'Incertae sedis'-like flags
        NotOtu                      = 0x00020000,       // ott flag : the name suggests that this is not a taxon. Keywords interpreted this way include "uncultured", "unclassified", "unidentified", "unknown", "metagenome", "other sequences", "artificial", "libraries", "tranposons", and a few others. Also "sp." when at the end of a name. Also, any node descended from such a node. This flag is applied to NCBI taxa but not to SILVA taxa.
        SiblingHigher               = 0x00040000,       // ott flag : 'Incertae sedis'-like flags
        notused_0x00080000          = 0x00080000,   // unused
        notUsed_0x00100000          = 0x00100000,   // unused
        Unplaced                    = 0x00200000,       // ott flag : 'Incertae sedis'-like flags
        UnplacedInherited           = 0x00400000,       // ott flag : 'Incertae sedis'-like flags
        Viral                       = 0x00800000,       // ott flag : the taxon name suggests that it has something to do with viruses. Also, any node descended from such a node.
        WasContainer                = 0x01000000,       // ott flag : this node used to be a container pseudo-taxon (incertae sedis, environmental samples, etc.) but its children have all been flagged and moved to the node's parent
        Unnamed                     = 0x02000000,   // TOL FLAG
        HasImage                    = 0x04000000,   // TOL FLAG
        HasSound                    = 0x08000000,   // TOL FLAG
        User0                       = 0x10000000,   // TOL FLAG
        User1                       = 0x20000000,   // TOL FLAG
        User2                       = 0x40000000,   // TOL FLAG
        User3                       = 0x80000000    // TOL FLAG
    }

    static public class FlagsEnumExt
    {
        public static uint FromString( string _value )
        {
            uint result = 0;
            _value = _value.ToLower();
            if (_value.Contains("subdivision for extinct"))
            {
                result |= (uint)FlagsEnum.ExtinctInherited;
                _value = _value.Replace("subdivision for extinct", "");
            }
            return result;
        }
    }

    public enum RedListCategoryEnum : byte
    {
        NotEvaluated = 0,
        DataDeficient = 1,
        LeastConcern = 2,
        NearThreatened = 3,
        Vulnerable = 4,
        Endangered = 5,
        CriticallyEndangered = 6,
        ExtinctInTheWild = 7,
        Extinct = 8
    }

    static public class RedListCategoryExt
    {
        static RedListCategoryExt()
        {
            _StringToEnum = new Dictionary<string, RedListCategoryEnum>();
            foreach (RedListCategoryEnum val in Enum.GetValues(typeof(RedListCategoryEnum)))
                _StringToEnum[GetAbbreviation(val)] = val;

            _Brushes = new List<Brush>();
            foreach (RedListCategoryEnum val in Enum.GetValues(typeof(RedListCategoryEnum)))
                _Brushes.Add(new SolidBrush(_Colors[(int)val]));
        }

        static readonly string[] _Abbreviations = { "NE", "DD", "LC", "NT", "VU", "EN", "CR", "EW", "EX" };
        static public string GetAbbreviation(RedListCategoryEnum _category )
        {
            return _Abbreviations[(int)_category];
        }

        static readonly string[] _Names = { "Not Evaluated", "Data Deficient", "Least Concern", "Near Threatened", "Vulnerable", "Endangered", "Critically Endangered", "Extinct In The Wild", "Extinct" };
        static public string GetName(RedListCategoryEnum _category)
        {
            return _Names[(int)_category];
        }

        static List<Brush> _Brushes;
        static public Brush GetBackBrush(RedListCategoryEnum _category)
        {
            return _Brushes[(int)_category];
        }

        static public Brush GetForeBrush(RedListCategoryEnum _category)
        {
            if (_category >= RedListCategoryEnum.ExtinctInTheWild)
                return  Brushes.White;
            else
                return Brushes.Black;
        }

        static readonly Dictionary<string, RedListCategoryEnum> _StringToEnum;
        static public RedListCategoryEnum FromString( string _str )
        {
            if (_StringToEnum.TryGetValue(_str, out RedListCategoryEnum val)) return val;
            return RedListCategoryEnum.NotEvaluated;
        }

        static readonly Color[] _Colors =
        {
            Color.FromArgb(255,255,255),
            Color.FromArgb(209,209,198),
            Color.FromArgb(2,202,2),    // Color.FromArgb(96,198,89),
            Color.FromArgb(168,227,52), // Color.FromArgb(204,226,38),
            Color.FromArgb(255,255,0),  // Color.FromArgb(249,232,20),
            Color.FromArgb(252,127,63),
            Color.FromArgb(216,30,5),
            Color.FromArgb(82,33,66),
            Color.FromArgb(0,0,0),
        };

        static public void SetupToggleButton(RedListCategoryEnum _categ,  System.Windows.Forms.CheckBox _cb)
        {
            _cb.Tag = _categ;
            _cb.Text = _Abbreviations[(int)_categ];
            _cb.BackColor = Color.Transparent;
            _cb.FlatAppearance.CheckedBackColor = _Colors[(int)_categ];
            _cb.FlatAppearance.MouseDownBackColor = _Colors[(int)_categ];
            if (_cb.Checked)
                if (_categ >= RedListCategoryEnum.ExtinctInTheWild)
                    _cb.ForeColor = Color.White;
                else
                    _cb.ForeColor = Color.Black;
            else
                _cb.ForeColor = Color.DimGray;
        }

        static public void PaintTag( RedListCategoryEnum _categ, Graphics _graphics, Rectangle R, Font _font)
        {
            StringFormat centeredText = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            _graphics.FillRectangle(GetBackBrush(_categ), R);
            _graphics.DrawString(GetAbbreviation(_categ), _font, GetForeBrush(_categ), R, centeredText);
        }
        
    }

}
