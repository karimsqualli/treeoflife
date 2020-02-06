using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VinceToolbox.UserControl.Graph
{
    // Represents something that a TreeNode can draw.
    public abstract class IDrawable
    {
        public enum selectionMode
        {
            none = 0,
            selected = 1,
            overview = 2,
            unselected = 4,
            user = 8,
        }

        public static int NoLimitCapacity = 1000;

        // Color
        public static Color UserColor = Color.MediumOrchid;

        public static Pen BorderPenNormal = Pens.Black;
        public static Pen BorderPenSelected = Pens.Red;
        public static Pen BorderPenOverview = Pens.RoyalBlue;
        public static Pen BorderPenUser = Pens.BlueViolet;

        public static float LinkerGapBefore = 5;
        public static float LinkerGapBetween = 2;
        public static float LinkerGapAfter = 5;
        public static PointF LinkerSize = new PointF(5, 6);
        public static float getLinkersHeight( int _linkerNumber )
        {
            if (_linkerNumber == 0) return 0;
            return LinkerGapBefore + LinkerGapAfter + (_linkerNumber * LinkerSize.Y) + ((_linkerNumber - 1) * LinkerGapBetween);
        }

        public static float getLinkerY(float _height, int _index, int _linkNumber )
        {
            if (_linkNumber == 0) return _height / 2;
            float height = getLinkersHeight(_linkNumber);
            if (_index >= _linkNumber) _index = _linkNumber - 1;
            float y = LinkerGapBefore + (_height - height) / 2;
            y += _index * (LinkerSize.Y * LinkerGapBetween);
            return y;
        }
        
        public Pen BorderPen
        {
            get 
            {
                if (User) return BorderPenUser;
                if (Selected) return BorderPenSelected;
                if (Overview) return BorderPenOverview;
                return BorderPenNormal;
            }
        }
        
        // Return the object's needed size.
        public virtual SizeF getSize(Graphics gr, Font font)
        {
            int maxAnchor = Math.Max( RightLinksAnchorNumber, LeftLinksAnchorNumber);
            return new SizeF(0, getLinkersHeight(maxAnchor));
        }

        // Draw the object centered at (x, y).
        public abstract void draw( Rectangle _bounds, Graphics gr, Pen pen, Brush bg_brush, Brush text_brush, Font font);


        public bool rightLocked = true;
        public bool rightOneAnchor = true;
        public int rightCapacity = 0;
        public int rightNumber = 0;
        public int RightLinksAnchorNumber
        {
            get
            {
                if (rightOneAnchor) return rightNumber > 0 ? 1 : 0;
                if (rightCapacity > 8) return rightNumber + (rightLocked ? 0 : 1);
                return rightCapacity;
            }
        }
        public float getRightAnchorY( float _height, int _index )
        {
            return getLinkerY(_height, _index, RightLinksAnchorNumber);
        }

        public bool leftLocked = true;
        public bool leftOneAnchor = true;
        public int leftCapacity = 0;
        public int leftNumber = 0;
        public int LeftLinksAnchorNumber
        {
            get
            {
                if (leftOneAnchor) return leftNumber > 0 ? 1 : 0;
                if (leftCapacity > 8) return leftNumber + (leftLocked ? 0 : 1);
                return leftCapacity;
            }
        }
        public float getLeftAnchorY(float _height, int _index)
        {
            return getLinkerY(_height, _index, LeftLinksAnchorNumber);
        }

        public bool Selected = false;
        public bool Overview = false;
        public bool User = false;
    }
}
