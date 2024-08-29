using System;
using System.IO;

namespace Easy3DPrint_NetFW
{
    public class ApplicationSettings
    {
        public enum FileType
        {
            _NONE,
            _STL,
            _STEP,
            _3MF,
            _AMF,
            _SLDPRT,
            _PLY
        }

        public class AddinSettings
        {
            public string ExportPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AutoExportSW");
            public string DataPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Easy3DPrintSettings_" + Version + ".json");
            public FileType QuickSaveType { get; set; } = FileType._STL;
            public const string Version = "V1.0.2"; // Oldest compatible settings config file structure 

            public AddinSettings() { }

            public AddinSettings(string path, FileType quickSaveType)
            {
                ExportPath = path;
                QuickSaveType = quickSaveType;
            }
        }

        public class CuraSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._STL;
            public bool Enabled { get; set; } = false;

            public CuraSettings() { }

            public CuraSettings(string path, FileType fileType, bool enabled)
            {
                Path = path;
                FileType = fileType;
                Enabled = enabled;
            }
        }

        public class BambuSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._STL;
            public bool Enabled { get; set; } = false;

            public BambuSettings() { }

            public BambuSettings(string path, FileType fileType, bool enabled)
            {
                Path = path;
                FileType = fileType;
                Enabled = enabled;
            }
        }

        public class AnkerMakeSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._STL;
            public bool Enabled { get; set; } = false;

            public AnkerMakeSettings() { }

            public AnkerMakeSettings(string path, FileType fileType, bool enabled)
            {
                Path = path;
                FileType = fileType;
                Enabled = enabled;
            }
        }

        public class PrusaSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._STL;
            public bool Enabled { get; set; } = false;

            public PrusaSettings() { }

            public PrusaSettings(string path, FileType fileType, bool enabled)
            {
                Path = path;
                FileType = fileType;
                Enabled = enabled;
            }
        }

        public class Slic3rSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._STL;
            public bool Enabled { get; set; } = false;

            public Slic3rSettings() { }

            public Slic3rSettings(string path, FileType fileType, bool enabled)
            {
                Path = path;
                FileType = fileType;
                Enabled = enabled;
            }
        }

        public class OrcaSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._STL;
            public bool Enabled { get; set; } = false;

            public OrcaSettings() { }

            public OrcaSettings(string path, FileType fileType, bool enabled)
            {
                Path = path;
                FileType = fileType;
                Enabled = enabled;
            }
        }

        public class CustomSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType {get; set; } = FileType._STL;
            public bool Enabled { get; set; } = false;
            public string imgPath { get; set; } = "";
            public string slicerName { get; set; } = "";

            public CustomSettings() {}

            public CustomSettings(string path, FileType filetype, bool enabled)
            {
                Path = path;
                FileType = filetype;
                Enabled = enabled;
            }
        }
    }
}