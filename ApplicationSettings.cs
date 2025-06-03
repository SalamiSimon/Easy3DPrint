using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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
            public string DataPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Easy3DPrintSettings_" + ConfigVersion + ".json");
            public List<FileType> QuickSaveTypes { get; set; } = new List<FileType> { FileType._STL };
            public bool QuietMode { get; set; } = false;
            public const string ConfigVersion = "V1.0.2"; // Oldest compatible settings config file structure
            public string CurrentVersion { get; } = "v1.1.2";

            public AddinSettings() { }

            public AddinSettings(string path, List<FileType> quickSaveTypes, bool quietMode)
            {
                ExportPath = path;
                QuickSaveTypes = quickSaveTypes;
                QuietMode = quietMode;
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

        public static class VersionComparer
        {
            public static bool IsNewerVersion(string currentVersion, string latestVersion)
            {
                currentVersion = currentVersion.TrimStart('v', 'V');
                latestVersion = latestVersion.TrimStart('v', 'V');

                var currentParts = currentVersion.Split('.').Select(int.Parse).ToArray();
                var latestParts = latestVersion.Split('.').Select(int.Parse).ToArray();

                Array.Resize(ref currentParts, 3);
                Array.Resize(ref latestParts, 3);

                for (int i = 0; i < 3; i++)
                {
                    if (latestParts[i] > currentParts[i])
                        return true;
                    else if (latestParts[i] < currentParts[i])
                        return false;
                }

                return false;
            }
        }
    }
}