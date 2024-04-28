namespace Easy3DPrint_NetFW
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
            public string ExportPath { get; set; } = "";

            public ExportSettings() { }

            public ExportSettings(string path)
            {
                ExportPath = path;
            }
        }

        public class CuraSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._STL;

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
            public FileType FileType { get; set; } = FileType._STL;

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
            public FileType FileType { get; set; } = FileType._STL;

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
            public FileType FileType { get; set; } = FileType._STL;

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
            public FileType FileType { get; set; } = FileType._STL;

            public Slic3rSettings() { }

            public Slic3rSettings(string path, FileType fileType)
            {
                Path = path;
                FileType = fileType;            }
        }

    }
}