using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.GUI
{
    //=============================================================================================
    // List of all object using ControlContainerInterface in UI
    // 
    public class ControlContainerInterfaceList : List<ControlContainerInterface>
    {
        //-------------------------------------------------------------------
        // constructor is private => only one class of this type in Singleton
        private ControlContainerInterfaceList() { }
        static ControlContainerInterfaceList Singleton = new ControlContainerInterfaceList();

        //-------------------------------------------------------------------
        public static void register(ControlContainerInterface _interface)
        {
            if (Singleton.IndexOf(_interface) != -1) return;
            Singleton.Add(_interface);
        }

        //-------------------------------------------------------------------
        public static void clean()
        {
            List<ControlContainerInterface> temp = new List<ControlContainerInterface>();
            temp.AddRange(Singleton.ToArray());
            Singleton.Clear();
            foreach (ControlContainerInterface i in temp)
            {
                if (i.GetControl().IsDisposed) continue;
                Singleton.Add(i);
            }
        }

        //-------------------------------------------------------------------
        static AnchorOverlay _Anchors = null;

        //-------------------------------------------------------------------
        public static void showAnchors(ControlContainerInterface _on)
        {
            if (_on == null)
                hideAnchors();
            else
            {
                if (_Anchors == null) _Anchors = new AnchorOverlay();
                _Anchors.ShowOn(_on);
            }
        }

        //-------------------------------------------------------------------
        public static void hideAnchors()
        {
            if (_Anchors == null) return;
            _Anchors.Close();
            _Anchors = null;
        }

        //-------------------------------------------------------------------
        public static void updateAnchors()
        {
            if (_Anchors == null) return;
            _Anchors.HitTest();
        }

        //-------------------------------------------------------------------
        public static DockStyle pickAnchor(out ControlContainerInterface _in)
        {
            _in = null;
            if (_Anchors == null) return DockStyle.None;
            return _Anchors.HitTestResult(out _in);
        }

        //-------------------------------------------------------------------
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        static int GetZOrder(IntPtr hWnd)
        {
            var z = 0;
            for (IntPtr h = hWnd; h != IntPtr.Zero; h = GetWindow(h, 3)) z++;
            return z;
        }

        static int GetZOrder(Control ctrl)
        {
            while (ctrl != null && !(ctrl is Form))
                ctrl = ctrl.Parent;
            if (ctrl == null) return 0;

            return GetZOrder((ctrl as Form).Handle);
        }
        
        //-------------------------------------------------------------------
        public static ControlContainerInterface getUnderMouse(ControlContainerInterface _ignore)
        {
            clean();
            System.Drawing.Point pos = Cursor.Position;

            List<ControlContainerInterface> solutions = new List<ControlContainerInterface>();
            foreach (ControlContainerInterface i in Singleton)
            {
                Control ctrl = i.GetControl();
                if (ctrl == null) continue;
                if (ctrl == _ignore) continue;

                System.Drawing.Rectangle R = ctrl.Bounds;
                R = ctrl.RectangleToScreen(R);

                if (R.Contains(pos))
                    solutions.Add(i);
            }
            
            if (solutions.Count == 0)
                return null;
            if (solutions.Count == 1)
                return solutions[0];

            int bestResult = GetZOrder( solutions[0].GetControl());
            ControlContainerInterface bestSolution = null;
            foreach (ControlContainerInterface i in solutions)
            {
                int z = GetZOrder(i.GetControl());
                if (z < bestResult)
                {
                    bestResult = z;
                    bestSolution = i;
                }
            }
            return bestSolution;
        }
    }
}
