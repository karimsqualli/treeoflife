using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife
{
    public partial class TaxonGraphPanel
    {
        bool EditionToolKeyOn()
        {
            if (SystemConfig.IsInUserMode) return false;
            if (!TaxonUtils.CurrentFilters.IsEmpty) return false;
            return ModifierKeys == (Keys.Control | Keys.Shift);
        }

        bool EditionToolKeyOffByFilter()
        {
            if (SystemConfig.IsInUserMode) return false;
            if (ModifierKeys == (Keys.Control | Keys.Shift))
                if (!TaxonUtils.CurrentFilters.IsEmpty) return true;
            return false;
        }

        bool EditionToolUpdate( TaxonTreeNode _node )
        {
            bool on = EditionToolKeyOn();
            if (!on && EditionToolKeyOffByFilter())
                TipManager.SetText(Localization.Manager.Get("_NoEditWithFilterOn", "Can't edit when some filter active"));
            else
                TipManager.SetText(null);

            TaxonTreeNode newNode = EditionToolKeyOn() ? _node : null;
            if (newNode == _EditionToolInfo.Taxon) return false;
            _EditionToolInfo.Taxon = newNode;
            TipManager.Pause(_EditionToolInfo.Taxon != null);
            return true;
        }

        void EditionToolPaint( Graphics _graphics)
        {
            if (Selected != null && _EditionToolInfo.Taxon == Selected)
                _EditionToolInfo.Paint(_graphics, mousePaintX, mousePaintY);
        }
    }

    public class TaxonGraphEditionTool
    {
        public TaxonGraphEditionTool(TaxonGraphPanel _owner)
        {
            Owner = _owner;
        }

        public TaxonGraphPanel Owner;

        private TaxonTreeNode _Taxon = null;
        public TaxonTreeNode Taxon
        {
            get { return _Taxon; }
            set
            {
                if (_Taxon == value) return;
                _Taxon = value;
                _Init = false;
            }
        }

        private bool _Init;
        public void Init( ref Rectangle _backRectangle, ref Rectangle _textR)
        {
            TextRect = _textR;
            if (_Init) return;
            _Init = true;
            BackRect = _backRectangle;
        }

        public Rectangle BackRect;
        public Rectangle TextRect;

        string ButtonInside = null;

        public bool DoAction()
        {
            if (Taxon == null || ButtonInside == null) return false;
            if (ButtonInside == "moveup") Owner.MoveUpTaxon(Taxon);
            else if (ButtonInside == "movetop") Owner.MoveTopTaxon(Taxon);
            else if (ButtonInside == "movedown") Owner.MoveDownTaxon(Taxon);
            else if (ButtonInside == "movebottom") Owner.MoveBottomTaxon(Taxon);
            else if (ButtonInside == "addfather") Owner.AddFather(Taxon);
            else if (ButtonInside == "addfatherforall") Owner.AddFatherAll(Taxon);
            else if (ButtonInside == "addsiblingabove") Owner.AddSiblingAbove(Taxon);
            else if (ButtonInside == "addsiblingbelow") Owner.AddSiblingBelow(Taxon);
            else if (ButtonInside == "addchild") Owner.AddChild(Taxon);
            else
                return false;
            Taxon = null;
            return true;
        }

        int _MouseX = int.MinValue;
        int _MouseY = int.MinValue;

        public void SetMousePosition(int _x, int _y)
        {
            _MouseX = _x;
            _MouseY = _y;
        }

        public enum ButtonEnum { plus, top, up, down, bottom };

        public void PaintButton(Graphics _graphics, string id, ButtonEnum _type, int x, int y )
        {
            bool inside = x <= _MouseX && x + 16 >= _MouseX && y <= _MouseY && y + 16 >= _MouseY;
            if (inside) ButtonInside = id;
            Bitmap bitmap = null;

            switch (_type)
            {
                case ButtonEnum.plus: bitmap = inside ? Properties.Resources.plus_16x16_purple : Properties.Resources.plus_16x16_black; break;
                case ButtonEnum.top: bitmap = inside ? Properties.Resources.top_16x16_purple : Properties.Resources.top_16x16; break;
                case ButtonEnum.up: bitmap = inside ? Properties.Resources.up_16x16_purple : Properties.Resources.up_16x16; break;
                case ButtonEnum.down: bitmap = inside ? Properties.Resources.down_16x16_purple : Properties.Resources.down_16x16; break;
                case ButtonEnum.bottom: bitmap = inside ? Properties.Resources.bottom_16x16_purple : Properties.Resources.bottom_16x16; break;
                default:return;
            }
            _graphics.DrawImage(bitmap, x, y, 16, 16);
        }

        public void Paint(Graphics _graphics, int _x, int _y)
        {
            if (Taxon == null) return;

            SetMousePosition(_x, _y);

            int x0 = BackRect.Left - 16;
            int x2 = TextRect.Right;
            int x1 = TextRect.Right - 50;
            int y0 = BackRect.Top - 16;
            int y2 = BackRect.Bottom;
            int y1 = (y0 + y2) / 2;

            ButtonInside = null;

            bool isRoot = Taxon == Owner.Root;

            if (!isRoot && Taxon.Father.Children.Count > 1)
            {
                int index = Taxon.Father.Children.IndexOf(Taxon);
                if (index > 0)
                {
                    PaintButton(_graphics, "moveup", ButtonEnum.up, x1 + 15, y0);
                    PaintButton(_graphics, "movetop", ButtonEnum.top, x1 + 15, y0 - 16);
                }
                if (index < Taxon.Father.Children.Count - 1)
                {
                    PaintButton(_graphics, "movedown", ButtonEnum.down, x1 + 15, y2);
                    PaintButton(_graphics, "movebottom", ButtonEnum.bottom, x1 + 15, y2 + 16);
                }
            }

            if (!isRoot)
            {
                PaintButton(_graphics, "addfather", ButtonEnum.plus, x0, y1);
                PaintButton(_graphics, "addfatherforall", ButtonEnum.plus, x0-20, y1);
                PaintButton(_graphics, "addsiblingabove", ButtonEnum.plus, x1 - 15, y0);
                PaintButton(_graphics, "addsiblingbelow", ButtonEnum.plus, x1 - 15, y2);
            }
            PaintButton(_graphics, "addchild", ButtonEnum.plus, x2, y1);
        }
    }
}
