using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media;

namespace VinceToolbox.Helpers
{
    public static class BitmapHelper
    {
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct Win32Point
        {
            public int x;
            public int y;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct Win32Size
        {
            public int cx;
            public int cy;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct ShDragImage
        {
            public Win32Size sizeDragImage;
            public Win32Point ptOffset;
            public IntPtr hbmpDragImage;
            public int crColorKey;
        }

        static public BitmapSource GetBitmapSource(MemoryStream _stream)
        {
            try
            {
                _stream.Seek(0, SeekOrigin.Begin);
                BinaryReader br = new BinaryReader(_stream);
                ShDragImage shDragImage;
                shDragImage.sizeDragImage.cx = br.ReadInt32();
                shDragImage.sizeDragImage.cy = br.ReadInt32();
                shDragImage.ptOffset.x = br.ReadInt32();
                shDragImage.ptOffset.y = br.ReadInt32();
                shDragImage.hbmpDragImage = new IntPtr(br.ReadInt32()); // I do not know what this is for!
                shDragImage.crColorKey = br.ReadInt32();
                int stride = shDragImage.sizeDragImage.cx * 4;
                var imageData = new byte[stride * shDragImage.sizeDragImage.cy];
                // We must read the image data as a loop, so it's in a flipped format
                for (int i = (shDragImage.sizeDragImage.cy - 1) * stride; i >= 0; i -= stride)
                {
                    br.Read(imageData, i, stride);
                }
                BitmapSource bitmapSource = BitmapSource.Create
                    (
                        shDragImage.sizeDragImage.cx, shDragImage.sizeDragImage.cy, 96, 96,
                        PixelFormats.Bgra32, null, imageData, stride
                    );
                return bitmapSource;
            }
            catch { }
            return null;
        }

        static public Bitmap GetBitmap(BitmapSource _source)
        {
            System.Drawing.Imaging.PixelFormat pf = System.Drawing.Imaging.PixelFormat.Format32bppPArgb;
            Bitmap bmp = new Bitmap(_source.PixelWidth, _source.PixelHeight, pf);
            BitmapData data = bmp.LockBits( new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size), ImageLockMode.WriteOnly,pf );
            _source.CopyPixels(System.Windows.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        static public Bitmap GetBitmap(MemoryStream _stream)
        {
            BitmapSource source = GetBitmapSource(_stream);
            if (source == null) return null;
            return GetBitmap(source);
        }

        static public Image GetImage(string path)
        {
            var bytes = File.ReadAllBytes(path);
            var ms = new MemoryStream(bytes);
            var img = Image.FromStream(ms);
            //img.Save("c:/GetImage.bmp");
            return img;
        }
    }
}
