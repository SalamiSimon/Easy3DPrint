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
            _OBJ,
            _STEP,
            _3MF
        }

        public class AddinSettings
        {
            public string ExportPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AutoExportSW");
            public string DataPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Easy3DPrintSettings_" + Version + ".json");
            public const string Version = "V1.0.2";

            public AddinSettings() { }

            public AddinSettings(string path)
            {
                ExportPath = path;
            }
        }

       public class CuraSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._STL;
            private bool _enabled = false;

            public event EventHandler<bool> EnabledChanged;

            public bool Enabled
            {
                get => _enabled;
                set
                {
                    if (_enabled != value)
                    {
                        _enabled = value;
                        EnabledChanged?.Invoke(this, value);
                    }
                }
            }

            public CuraSettings() { }

            public CuraSettings(string path, FileType fileType, bool enabled)
            {
                Path = path;
                FileType = fileType;
                _enabled = enabled;
            }
        }

        public class BambuSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._STL;
            private bool _enabled = false;

            public event EventHandler<bool> EnabledChanged;

            public bool Enabled
            {
                get => _enabled;
                set
                {
                    if (_enabled != value)
                    {
                        _enabled = value;
                        EnabledChanged?.Invoke(this, value);
                    }
                }
            }

            public BambuSettings() { }

            public BambuSettings(string path, FileType fileType, bool enabled)
            {
                Path = path;
                FileType = fileType;
                _enabled = enabled;
            }
        }

        public class AnkerMakeSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._STL;
            private bool _enabled = false;

            public event EventHandler<bool> EnabledChanged;

            public bool Enabled
            {
                get => _enabled;
                set
                {
                    if (_enabled != value)
                    {
                        _enabled = value;
                        EnabledChanged?.Invoke(this, value);
                    }
                }
            }

            public AnkerMakeSettings() { }

            public AnkerMakeSettings(string path, FileType fileType, bool enabled)
            {
                Path = path;
                FileType = fileType;
                _enabled = enabled;
            }
        }

        public class PrusaSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._STL;
            private bool _enabled = false;

            public event EventHandler<bool> EnabledChanged;

            public bool Enabled
            {
                get => _enabled;
                set
                {
                    if (_enabled != value)
                    {
                        _enabled = value;
                        EnabledChanged?.Invoke(this, value);
                    }
                }
            }

            public PrusaSettings() { }

            public PrusaSettings(string path, FileType fileType, bool enabled)
            {
                Path = path;
                FileType = fileType;
                _enabled = enabled;
            }
        }

        public class Slic3rSettings
        {
            public string Path { get; set; } = "";
            public FileType FileType { get; set; } = FileType._STL;
            private bool _enabled = false;

            public event EventHandler<bool> EnabledChanged;

            public bool Enabled
            {
                get => _enabled;
                set
                {
                    if (_enabled != value)
                    {
                        _enabled = value;
                        EnabledChanged?.Invoke(this, value);
                    }
                }
            }

            public Slic3rSettings() { }

            public Slic3rSettings(string path, FileType fileType, bool enabled)
            {
                Path = path;
                FileType = fileType;
                _enabled = enabled;
            }
        }

    }
}