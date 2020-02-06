using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace VinceToolbox.Helpers
{
    public class RichTextBoxFunctions
    {
        //---------
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public Int32 left;
            public Int32 top;
            public Int32 right;
            public Int32 bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SCROLLBARINFO
        {
            public Int32 cbSize;
            public RECT rcScrollBar;
            public Int32 dxyLineButton;
            public Int32 xyThumbTop;
            public Int32 xyThumbBottom;
            public Int32 reserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public Int32[] rgstate;
        }

        static UInt32 SB_VERT = 1;
        static UInt32 OBJID_VSCROLL = 0xFFFFFFFB;

        [DllImport("user32.dll")]
        private static extern Int32 GetScrollRange(IntPtr hWnd, UInt32 nBar, out Int32 lpMinPos, out Int32 lpMaxPos);

        [DllImport("user32.dll")]
        private static extern Int32 GetScrollBarInfo(IntPtr hWnd, UInt32 idObject, ref SCROLLBARINFO psbi);

        public static int CalculateRichTextHeight(RichTextBox _rtb)
        {
            int nHeight = 0;

            SCROLLBARINFO psbi = new SCROLLBARINFO();
            psbi.cbSize = Marshal.SizeOf(psbi);
            _rtb.ScrollBars = RichTextBoxScrollBars.Vertical;
            int nResult = GetScrollBarInfo(_rtb.Handle, OBJID_VSCROLL, ref psbi);
            if (psbi.rgstate[0] == 0)
            {
                int nMin = 0, nMax = 0;
                GetScrollRange(_rtb.Handle, SB_VERT, out nMin, out nMax);
                nHeight = (nMax - nMin);
            }

            return nHeight;
        }
    }
}
