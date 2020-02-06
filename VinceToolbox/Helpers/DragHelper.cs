using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;

namespace VinceToolbox.Helpers
{
    public static class DragHelper
    {
        public static bool HasImage(DragEventArgs e)
        {
            Image unused = null;
            return GetImage(e, true, ref unused);
        }

        public static Image GetImage(DragEventArgs e)
        {
            Image image = null;
            if (!GetImage(e, false, ref image))
                return null;
            return image;
        }

        private static bool GetImage(DragEventArgs e, bool _onlyPresence, ref Image _image )
        {
            foreach (string format in e.Data.GetFormats())
                Console.WriteLine(format);

            if (e.Data.GetFormats().Contains("FileDrop"))
            {
                string[] filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                string filename = filenames[0];
                if  (
                        System.IO.Path.GetExtension(filename).ToUpperInvariant() == ".JPG" ||
                        System.IO.Path.GetExtension(filename).ToUpperInvariant() == ".PNG" ||
                        System.IO.Path.GetExtension(filename).ToUpperInvariant() == ".GIF"
                    )
                {
                    if (_onlyPresence) return true;
                    _image = Bitmap.FromFile(filename);
                    return _image != null;
                }
            }

//             if (e.Data.GetFormats().Contains("FileContents") && e.Data.GetFormats().Contains("FileGroupDescriptorW"))
//             {
//                 //System.IO.MemoryStream info = e.Data.GetData("FileGroupDescriptorW") as System.IO.MemoryStream;
//                 //System.IO.MemoryStream content = e.Data.GetData("FileContents") as System.IO.MemoryStream;
// 
//                  //get the names and data streams of the files dropped
//                 string[] filenames = GetFileGroupDescriptor(e.Data);
//                 MemoryStream[] filestreams = (MemoryStream[]) e.Data.GetData("FileContents");
// 
//                 for (int fileIndex = 0; fileIndex < filenames.Length; fileIndex++)
//                 {
//                     //use the fileindex to get the name and data stream
//                     string filename = filenames[fileIndex];
//                     MemoryStream filestream = filestreams[fileIndex];
// 
//                     //save the file stream using its name to the application path
//                     FileStream outputStream = File.Create(filename);
//                     filestream.WriteTo(outputStream);
//                     outputStream.Close();
//                 }
//             }
// 
//             if (e.Data.GetFormats().Contains("UniformResourceLocator"))
//             {
//                 string url = GetUri(e.Data);
//                 Image bmp = GetImageFromUrl(url);
//                 bmp.Save(TaxonUtils.GetImageFullPath(taxonImageControl.CurrentTaxon));
//                 taxonImageControl.ImageControl.ReloadImage();
//                 return;
//             }

//             if (e.Data.GetFormats().Contains("DragImageBits"))
//             {
//                 MemoryStream imageStream = e.Data.GetData("DragImageBits") as MemoryStream;
// 
//                 Bitmap bmp = VinceToolbox.Helpers.BitmapHelper.GetBitmap(imageStream);
//                 bmp.Save(TaxonUtils.GetImageFullPath(taxonImageControl.CurrentTaxon));
//                 taxonImageControl.ImageControl.ReloadImage();
//                 return;
//             }

            return false;
        }

        private class NativeMethods
        {
            [DllImport("kernel32.dll")]
            static extern IntPtr GlobalLock(IntPtr hMem);

            [DllImport("ole32.dll", PreserveSig = false)]
            internal static extern ILockBytes CreateILockBytesOnHGlobal(IntPtr hGlobal, bool fDeleteOnRelease);

            [DllImport("OLE32.DLL", CharSet = CharSet.Auto, PreserveSig = false)]
            internal static extern IntPtr GetHGlobalFromILockBytes(ILockBytes pLockBytes);

            [DllImport("OLE32.DLL", CharSet = CharSet.Unicode, PreserveSig = false)]
            internal static extern IStorage StgCreateDocfileOnILockBytes(ILockBytes plkbyt, uint grfMode, uint reserved);

            [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000000B-0000-0000-C000-000000000046")]
            internal interface IStorage
            {
                [return: MarshalAs(UnmanagedType.Interface)]
                IStream CreateStream([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved1, [In, MarshalAs(UnmanagedType.U4)] int reserved2);
                [return: MarshalAs(UnmanagedType.Interface)]
                IStream OpenStream([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, IntPtr reserved1, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved2);
                [return: MarshalAs(UnmanagedType.Interface)]
                IStorage CreateStorage([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved1, [In, MarshalAs(UnmanagedType.U4)] int reserved2);
                [return: MarshalAs(UnmanagedType.Interface)]
                IStorage OpenStorage([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, IntPtr pstgPriority, [In, MarshalAs(UnmanagedType.U4)] int grfMode, IntPtr snbExclude, [In, MarshalAs(UnmanagedType.U4)] int reserved);
                void CopyTo(int ciidExclude, [In, MarshalAs(UnmanagedType.LPArray)] Guid[] pIIDExclude, IntPtr snbExclude, [In, MarshalAs(UnmanagedType.Interface)] IStorage stgDest);
                void MoveElementTo([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.Interface)] IStorage stgDest, [In, MarshalAs(UnmanagedType.BStr)] string pwcsNewName, [In, MarshalAs(UnmanagedType.U4)] int grfFlags);
                void Commit(int grfCommitFlags);
                void Revert();
                void EnumElements([In, MarshalAs(UnmanagedType.U4)] int reserved1, IntPtr reserved2, [In, MarshalAs(UnmanagedType.U4)] int reserved3, [MarshalAs(UnmanagedType.Interface)] out object ppVal);
                void DestroyElement([In, MarshalAs(UnmanagedType.BStr)] string pwcsName);
                void RenameElement([In, MarshalAs(UnmanagedType.BStr)] string pwcsOldName, [In, MarshalAs(UnmanagedType.BStr)] string pwcsNewName);
                void SetElementTimes([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In] System.Runtime.InteropServices.ComTypes.FILETIME pctime, [In] System.Runtime.InteropServices.ComTypes.FILETIME patime, [In] System.Runtime.InteropServices.ComTypes.FILETIME pmtime);
                void SetClass([In] ref Guid clsid);
                void SetStateBits(int grfStateBits, int grfMask);
                void Stat([Out]out System.Runtime.InteropServices.ComTypes.STATSTG pStatStg, int grfStatFlag);
            }

            [ComImport, Guid("0000000A-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            internal interface ILockBytes
            {
                void ReadAt([In, MarshalAs(UnmanagedType.U8)] long ulOffset, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, [In, MarshalAs(UnmanagedType.U4)] int cb, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pcbRead);
                void WriteAt([In, MarshalAs(UnmanagedType.U8)] long ulOffset, IntPtr pv, [In, MarshalAs(UnmanagedType.U4)] int cb, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pcbWritten);
                void Flush();
                void SetSize([In, MarshalAs(UnmanagedType.U8)] long cb);
                void LockRegion([In, MarshalAs(UnmanagedType.U8)] long libOffset, [In, MarshalAs(UnmanagedType.U8)] long cb, [In, MarshalAs(UnmanagedType.U4)] int dwLockType);
                void UnlockRegion([In, MarshalAs(UnmanagedType.U8)] long libOffset, [In, MarshalAs(UnmanagedType.U8)] long cb, [In, MarshalAs(UnmanagedType.U4)] int dwLockType);
                void Stat([Out]out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, [In, MarshalAs(UnmanagedType.U4)] int grfStatFlag);
            }

            [StructLayout(LayoutKind.Sequential)]
            internal sealed class POINTL
            {
                internal int x;
                internal int y;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal sealed class SIZEL
            {
                internal int cx;
                internal int cy;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            internal sealed class FILEGROUPDESCRIPTORA
            {
                internal uint cItems;
                internal FILEDESCRIPTORA[] fgd;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            internal sealed class FILEDESCRIPTORA
            {
                internal uint dwFlags;
                internal Guid clsid;
                internal SIZEL sizel;
                internal POINTL pointl;
                internal uint dwFileAttributes;
                internal System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
                internal System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
                internal System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
                internal uint nFileSizeHigh;
                internal uint nFileSizeLow;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                internal string cFileName;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal sealed class FILEGROUPDESCRIPTORW
            {
                internal uint cItems;
                internal FILEDESCRIPTORW[] fgd;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal sealed class FILEDESCRIPTORW
            {
                internal uint dwFlags;
                internal Guid clsid;
                internal SIZEL sizel;
                internal POINTL pointl;
                internal uint dwFileAttributes;
                internal System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
                internal System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
                internal System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
                internal uint nFileSizeHigh;
                internal uint nFileSizeLow;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 520)]
                internal string cFileName;
            }
        }

        static string[] GetFileGroupDescriptor(System.Windows.Forms.IDataObject _data)
        {
            MemoryStream fileGroupDescriptorStream = (MemoryStream)_data.GetData("FileGroupDescriptor");
            if (fileGroupDescriptorStream != null)
            {
                IntPtr fileGroupDescriptorAPointer = IntPtr.Zero;
                try
                {
                    byte[] fileGroupDescriptorBytes = new byte[fileGroupDescriptorStream.Length];
                    fileGroupDescriptorStream.Read(fileGroupDescriptorBytes, 0, fileGroupDescriptorBytes.Length);
                    fileGroupDescriptorStream.Close();

                    fileGroupDescriptorAPointer = Marshal.AllocHGlobal(fileGroupDescriptorBytes.Length);
                    Marshal.Copy(fileGroupDescriptorBytes, 0, fileGroupDescriptorAPointer, fileGroupDescriptorBytes.Length);

                    object fileGroupDescriptorObject = Marshal.PtrToStructure(fileGroupDescriptorAPointer, typeof(NativeMethods.FILEGROUPDESCRIPTORA));
                    NativeMethods.FILEGROUPDESCRIPTORA fileGroupDescriptor = (NativeMethods.FILEGROUPDESCRIPTORA)fileGroupDescriptorObject;

                    string[] fileNames = new string[fileGroupDescriptor.cItems];

                    IntPtr fileDescriptorPointer = (IntPtr)((int)fileGroupDescriptorAPointer + Marshal.SizeOf(fileGroupDescriptor.cItems));

                    for (int fileDescriptorIndex = 0; fileDescriptorIndex < fileGroupDescriptor.cItems; fileDescriptorIndex++)
                    {
                        NativeMethods.FILEDESCRIPTORA fileDescriptor = (NativeMethods.FILEDESCRIPTORA)Marshal.PtrToStructure(fileDescriptorPointer, typeof(NativeMethods.FILEDESCRIPTORA));
                        fileNames[fileDescriptorIndex] = fileDescriptor.cFileName;

                        fileDescriptorPointer = (IntPtr)((int)fileDescriptorPointer + Marshal.SizeOf(fileDescriptor));
                    }

                    return fileNames;
                }
                finally
                {
                    Marshal.FreeHGlobal(fileGroupDescriptorAPointer);
                }
            }

            fileGroupDescriptorStream = (MemoryStream)_data.GetData("FileGroupDescriptorW");
            if (fileGroupDescriptorStream != null)
            {
                IntPtr fileGroupDescriptorWPointer = IntPtr.Zero;
                try
                {
                    byte[] fileGroupDescriptorBytes = new byte[fileGroupDescriptorStream.Length];
                    int length = fileGroupDescriptorStream.Read(fileGroupDescriptorBytes, 0, fileGroupDescriptorBytes.Length);
                    fileGroupDescriptorStream.Close();

                    fileGroupDescriptorWPointer = Marshal.AllocHGlobal(length);
                    Marshal.Copy(fileGroupDescriptorBytes, 0, fileGroupDescriptorWPointer, length);

                    object fileGroupDescriptorObject = Marshal.PtrToStructure(fileGroupDescriptorWPointer, typeof(NativeMethods.FILEGROUPDESCRIPTORW));
                    NativeMethods.FILEGROUPDESCRIPTORW fileGroupDescriptor = (NativeMethods.FILEGROUPDESCRIPTORW)fileGroupDescriptorObject;

                    string[] fileNames = new string[fileGroupDescriptor.cItems];

                    IntPtr fileDescriptorPointer = (IntPtr)((int)fileGroupDescriptorWPointer + Marshal.SizeOf(fileGroupDescriptor.cItems));

                    for (int fileDescriptorIndex = 0; fileDescriptorIndex < fileGroupDescriptor.cItems; fileDescriptorIndex++)
                    {
                        NativeMethods.FILEDESCRIPTORW fileDescriptor = (NativeMethods.FILEDESCRIPTORW)Marshal.PtrToStructure(fileDescriptorPointer, typeof(NativeMethods.FILEDESCRIPTORW));
                        fileNames[fileDescriptorIndex] = fileDescriptor.cFileName;

                        fileDescriptorPointer = (IntPtr)((int)fileDescriptorPointer + Marshal.SizeOf(fileDescriptor));
                    }

                    return fileNames;
                }
                finally
                {
                    Marshal.FreeHGlobal(fileGroupDescriptorWPointer);
                }
            }

            return null;
        }

        public static string GetUri(System.Windows.Forms.IDataObject _iDataObject)
        {
            if (_iDataObject.GetDataPresent("UniformResourceLocator"))
            {
                MemoryStream stream = _iDataObject.GetData("UniformResourceLocator") as MemoryStream;
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return ASCIIEncoding.UTF8.GetString(buffer, 0, buffer.Length).TrimEnd('\0');
            }
            return null;
        }

        public static Image GetImageFromUrl(string url)
        {
            using (var webClient = new WebClient())
            {
                return ByteArrayToImage(webClient.DownloadData(url));
            }
        }

        public static Image ByteArrayToImage(byte[] fileBytes)
        {
            using (var stream = new MemoryStream(fileBytes))
            {
                return Image.FromStream(stream);
            }
        }
    }
}
