using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVG
{
    public static class Tools
    {

        static bool PointFromString(ref string _svg, out string _key, out PointF _pt)
        {
            _key = null;
            _pt = new PointF(0, 0);
            _svg = _svg.TrimStart();
            string[] parts = _svg.Split(" ".ToCharArray(), 3);
            if (parts.Length < 3) return false;
            _key = parts[0];
            _svg = parts[2];
            parts = parts[1].Split(",".ToCharArray());
            if (parts.Length != 2) return false;
            if (!float.TryParse(parts[0], System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out float x)) return false;
            if (!float.TryParse(parts[1], System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out float y)) return false;
            _pt.X = x;
            _pt.Y = y;
            return true;
        }

        public static Tuple<PointF[], Byte[]> PointsFromString(string _svg)
        {
            List<PointF> pts = new List<PointF>();
            List<Byte> flags = new List<Byte>();

            while (PointFromString(ref _svg, out string key, out PointF pt))
            {
                pts.Add(pt);
                if (key == "M")
                    flags.Add((Byte)PathPointType.Start);
                else
                    flags.Add((Byte)PathPointType.Line);
            }
            return new Tuple<PointF[], Byte[]>(pts.ToArray(), flags.ToArray());
        }
    }
}
