using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ConvertDocXToHtml
{
    public class winFunctions
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref winPlacement lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPlacement(IntPtr hWnd, ref winPlacement lpwndpl);

        public struct winPlacement
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        const UInt32 SW_HIDE = 0;
        const UInt32 SW_SHOWNORMAL = 1;
        const UInt32 SW_NORMAL = 1;
        const UInt32 SW_SHOWMINIMIZED = 2;
        const UInt32 SW_SHOWMAXIMIZED = 3;
        const UInt32 SW_MAXIMIZE = 3;
        const UInt32 SW_SHOWNOACTIVATE = 4;
        const UInt32 SW_SHOW = 5;
        const UInt32 SW_MINIMIZE = 6;
        const UInt32 SW_SHOWMINNOACTIVE = 7;
        const UInt32 SW_SHOWNA = 8;
        const UInt32 SW_RESTORE = 9;

        static public void winPlacementSetInvalid(winPlacement _placement)
        {
            _placement.length = 0;
        }

        static public bool winPlacementIsValid(winPlacement _placement)
        {
            return (_placement.length == Marshal.SizeOf(_placement));
        }

        static public winPlacement winGetPlacement(IntPtr _windowHandle)
        {
            winPlacement placement = new winPlacement();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(_windowHandle, ref placement);
            placement.rcNormalPosition.Width = placement.rcNormalPosition.Width - placement.rcNormalPosition.Left;
            placement.rcNormalPosition.Height = placement.rcNormalPosition.Height - placement.rcNormalPosition.Top;
            return placement;
        }

        static public void winSetPlacement(IntPtr _windowHandle, winPlacement _placement)
        {
            winPlacement placement = new winPlacement();
            placement.length = Marshal.SizeOf(placement);
            placement.flags = _placement.flags;
            placement.showCmd = _placement.showCmd;
            placement.ptMinPosition = _placement.ptMinPosition;
            placement.ptMaxPosition = _placement.ptMaxPosition;
            placement.rcNormalPosition = _placement.rcNormalPosition;
            placement.rcNormalPosition.Width = placement.rcNormalPosition.Width + placement.rcNormalPosition.Left;
            placement.rcNormalPosition.Height = placement.rcNormalPosition.Height + placement.rcNormalPosition.Top;
            SetWindowPlacement(_windowHandle, ref placement);
        }

        static public bool winSetPlacement(IntPtr _windowHandle, winPlacement _placement, double _visibilityThreshold)
        {
            if (_placement.showCmd == SW_NORMAL && GetRatioInScreen(_placement.rcNormalPosition) < _visibilityThreshold)
                return false;

            try
            {
                winPlacement placement = new winPlacement();
                placement.length = Marshal.SizeOf(placement);
                placement.flags = _placement.flags;
                placement.showCmd = _placement.showCmd;
                placement.ptMinPosition = _placement.ptMinPosition;
                placement.ptMaxPosition = _placement.ptMaxPosition;
                placement.rcNormalPosition = _placement.rcNormalPosition;
                placement.rcNormalPosition.Width = placement.rcNormalPosition.Width + placement.rcNormalPosition.Left;
                placement.rcNormalPosition.Height = placement.rcNormalPosition.Height + placement.rcNormalPosition.Top;
                SetWindowPlacement(_windowHandle, ref placement);
            }
            catch (System.Exception)
            {
                return false;
            }

            return true;
        }

        static public double GetRatioInScreen(IntPtr _windowHandle)
        {
            Rectangle rWin = winGetPlacement(_windowHandle).rcNormalPosition;
            return GetRatioInScreen(rWin);
        }

        static public double GetRatioInScreen(Rectangle _rectangle)
        {
            double area = _rectangle.Width * _rectangle.Height;
            if (area == 0) return 0;
            double ratio = 0;

            System.Windows.Forms.Screen[] screens = System.Windows.Forms.Screen.AllScreens;
            foreach (System.Windows.Forms.Screen screen in screens)
            {
                Rectangle rScreen = screen.Bounds;
                rScreen.Intersect(_rectangle);
                ratio += (rScreen.Width * rScreen.Height) / area;
            }

            return ratio;
        }

        public class windowPlacement
        {
            public windowPlacement() { }

            public int left = 0;
            public int width = -1;
            public int top = 0;
            public int height = -1;
            public bool minimized = false;
            public bool maximized = false;

            public bool isValid() { return width >= 0 && height >= 0; }
        }

        static public windowPlacement getWindowPlacement(IntPtr _windowHandle)
        {
            winPlacement wp = winGetPlacement(_windowHandle);
            windowPlacement placement = new windowPlacement();
            placement.left = wp.rcNormalPosition.Left;
            placement.top = wp.rcNormalPosition.Top;
            placement.width = wp.rcNormalPosition.Width;
            placement.height = wp.rcNormalPosition.Height;
            placement.maximized = wp.showCmd == SW_MAXIMIZE;
            placement.minimized = wp.showCmd == SW_MINIMIZE;
            return placement;
        }

        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        static public Rectangle GetWindowRect(IntPtr _windowHandle)
        {
            RECT rect = new RECT();
            //var window = typeof(ToolTip).GetField("window", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this) as NativeWindow;
            GetWindowRect(_windowHandle, ref rect);
            return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr GetFocus();

        static public System.Windows.Forms.Control GetFocusedControl()
        {
            System.Windows.Forms.Control focusedControl = null;
            // To get hold of the focused control:
            IntPtr focusedHandle = GetFocus();
            if (focusedHandle != IntPtr.Zero)
                // Note that if the focused Control is not a .Net control, then this will return null.
                focusedControl = System.Windows.Forms.Control.FromHandle(focusedHandle);
            return focusedControl;
        }

    }
}
