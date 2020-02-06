using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TreeOfLife.Helpers
{
    public static class FileSystem
    {
        public static void EnsureReadOnlyFlagIsOff( string _file )
        {
            if (!File.Exists(_file)) return;
            FileAttributes attributes = File.GetAttributes(_file);
            if ((attributes & FileAttributes.ReadOnly) == 0) return;
            attributes -= FileAttributes.ReadOnly;
            File.SetAttributes(_file, attributes);
        }
    }
}
