using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeOfLife
{
    public enum MessageLevelEnum
    {
        FatalError,
        Error,
        HighLevelWarning,
        Warning,
        LowLevelWarning,
        Information
    }

    public enum VerboseModeEnum
    {
        NothingAtAll,
        OnlyErrors,
        ErrorsAndWarnings,
        Full
    }

    public static class LogTags
    {
        public const string Congif = "Config";
        public const string Data = "Data";
        public const string Collection = "Collection";
        public const string Comment = "Comment";
        public const string Image = "Image";
        public const string Program = "Program";
        public const string Sound = "Sound";
        public const string Synonyms = "Synonyms";
        public const string Tags = "Tags";
        public const string UI = "UI";
        public const string Location = "Location";
    }
}
