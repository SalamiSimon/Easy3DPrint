using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DPrint_SW
{
    public class ApplicationSettings
    {
        public enum FileType
        {
            _NONE,
            _STL,
            _OBJ,
            _STEP,
            _3MF
        }
        
        public class ExportSettings
        {

            public string Path { get; set; } = "";

            public ExportSettings() { }

            public ExportSettings(string path)
            {
                Path = path;
            }
        }

        public class CuraSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._NONE;

            public CuraSettings() { }

            public CuraSettings(string path, FileType fileType)
            {
                Path = path;
                FileType = fileType;
            }
        }

        public class BambuSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._NONE;

            public BambuSettings() { }

            public BambuSettings(string path, FileType fileType)
            {
                Path = path;
                FileType = fileType;
            }
        }

        public class AnkerMakeSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._NONE;

            public AnkerMakeSettings() { }

            public AnkerMakeSettings(string path, FileType fileType)
            {
                Path = path;
                FileType = fileType;
            }
        }

        public class PrusaSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._NONE;

            public PrusaSettings() { }

            public PrusaSettings(string path, FileType fileType)
            {
                Path = path;
                FileType = fileType;
            }
        }

        public class Slic3rSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._NONE;

            public Slic3rSettings() { }

            public Slic3rSettings(string path, FileType fileType)
            {
                Path = path;
                FileType = fileType;
            }
        }

    }
}
