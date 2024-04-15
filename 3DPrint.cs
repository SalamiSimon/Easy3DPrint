using System.ComponentModel;
using System.Runtime.InteropServices;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.SolidWorks;
using Xarial.XCad.UI.Commands;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Windows.Forms;
using System.Drawing;
using Newtonsoft.Json;
using System.IO;
using static _3DPrint_SW.ApplicationSettings;
using _3DPrint_SW.Properties;

namespace _3DPrint_SW
{
    [ComVisible(true)]
    [Guid("0FE9F3CB-62B7-4604-9D33-7918B49E4675")]
    [Title("Easy3DPrint")]

    public class Easy3DPrint : SwAddInEx
    {
        private ExportSettings exportSettings = new ExportSettings();
        private CuraSettings curaSettings = new CuraSettings();
        private BambuSettings bambuSettings = new BambuSettings();
        private AnkerMakeSettings ankerMakeSettings = new AnkerMakeSettings();
        private PrusaSettings prusaSettings = new PrusaSettings();
        private Slic3rSettings slic3rSettings = new Slic3rSettings();

        private readonly string? settingsDirectoryPath;

        private readonly string settingsFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Easy3DPrintSettings.json");

        [Title("Easy3DPrint")]
        [Description("Open parts directly in slicing apps")]

        public enum Commands_e 
        {
            [Title("Open in Ultimaker Cura")]
            [Description("Opens the model in Ultimaker Cura")]
            /* Menu won't load with custom icons enabled ? */
            //[Icon(typeof(Resources), nameof(Resources.cura))]
            OpenInUltimakerCura,

            [Title("Open in Bambu Studio")]
            [Description("Opens the model in Bambu Lab")]
            //[Icon(typeof(Resources), nameof(Resources.bambu))]
            OpenInBambuLab,

            [Title("Open in AnkerMake Studio")]
            [Description("Opens the model in AnkerMake Studio")]
            OpenInAnkerMake,

            [Title("Open in Prusa")]
            [Description("Opens the model in Prusa")]
            OpenInPrusa,

            [Title("Open in Slic3r")]
            [Description("Opens the model in Slic3r")]
            OpenInSlic3r,

            [Title("Settings")]
            [Description("Easy3DPrint Settings")]
            //[Icon(typeof(Resources), nameof(Resources.settings))]
            Settings
        }

        public Easy3DPrint()
        {
            settingsDirectoryPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "AutoExportSW");
        }

        public override void OnConnect()
        {
            if (!LoadSettings())
            {
                Application.ShowMessageBox("Before use, enter executable paths and filetype in Easy3DPrint settings.");

                if (!Directory.Exists(settingsDirectoryPath))
                {
                    Directory.CreateDirectory(settingsDirectoryPath);
                    exportSettings.ExportPath = settingsDirectoryPath;
                    ShowSettingsDialog();
                    LoadSettings();
                }

            }
            var cmdGrp = this.CommandManager.AddCommandGroup<Commands_e>();
            cmdGrp.CommandClick += OnCommandClick;
        }

        private bool LoadSettings()
        {
            if (File.Exists(settingsFilePath))
            {
                string json = File.ReadAllText(settingsFilePath);
                var settings = JsonConvert.DeserializeObject<dynamic>(json);

                // Load settings
                exportSettings.ExportPath = settings.ExportPath;

                curaSettings.Path = settings.CuraPath;
                curaSettings.FileType = settings.ExportFormatCura;

                bambuSettings.Path = settings.BambuPath;
                bambuSettings.FileType = settings.ExportFormatBambu;

                ankerMakeSettings.Path = settings.AnkerMakePath;
                ankerMakeSettings.FileType = settings.ExportFormatAnkerMake;

                slic3rSettings.Path = settings.Slic3rPath;
                slic3rSettings.FileType = settings.ExportFormatSlic3r;

                prusaSettings.Path = settings.PrusaPath;
                prusaSettings.FileType = settings.ExportFormatPrusa;

                return true; // Settings loaded successfully
            }
            // Settings not loaded

            return false;
        }

        public void ShowSettingsDialog()
        {
            SettingsDialog settingsDialog = new SettingsDialog(exportSettings, curaSettings, bambuSettings, ankerMakeSettings, prusaSettings, slic3rSettings);
            if (settingsDialog.ShowDialog() == DialogResult.OK)
            {
                LoadSettings();
            }
        }

        private void OnCommandClick(Commands_e spec)
        {
            switch (spec)
            {
                case Commands_e.OpenInUltimakerCura:
                    string? FilePathCura = null;

                    if (curaSettings.FileType != FileType._NONE)
                    {
                        FilePathCura = SaveCurrentPart(exportSettings.ExportPath, curaSettings.FileType);
                    }
                    else
                    {
                        Application.ShowMessageBox("Select file format in settings.");
                    }

                    if (!string.IsNullOrEmpty(FilePathCura) && !string.IsNullOrEmpty(curaSettings.Path))
                    {
                        System.Diagnostics.Process.Start(curaSettings.Path, $"\"{FilePathCura}\"");
                    } else
                    {
                        Application.ShowMessageBox("No Cura executable path entered in settings or file not saved sucessfully.");
                    }
                break;

                case Commands_e.OpenInBambuLab:
                    string? FilePathBambu = null;

                    if (bambuSettings.FileType != FileType._NONE)
                    {
                        FilePathBambu = SaveCurrentPart(exportSettings.ExportPath, bambuSettings.FileType);
                    }
                    else
                    {
                        Application.ShowMessageBox("Select file format in settings.");
                    }

                    if (!string.IsNullOrEmpty(FilePathBambu) && !string.IsNullOrEmpty(bambuSettings.Path))
                    {
                        System.Diagnostics.Process.Start(bambuSettings.Path, $"\"{FilePathBambu}\"");
                    }
                    else
                    {
                        Application.ShowMessageBox("No BambuLab executable path entered in settings or file not saved sucessfully.");
                    }
                    break;

                case Commands_e.OpenInAnkerMake:
                    string? FilePathAnker = null;

                    if (ankerMakeSettings.FileType != FileType._NONE)
                    {
                        FilePathAnker = SaveCurrentPart(exportSettings.ExportPath, ankerMakeSettings.FileType);
                    }
                    else
                    {
                        Application.ShowMessageBox("Select file format in settings.");
                    }

                    if (!string.IsNullOrEmpty(FilePathAnker) && !string.IsNullOrEmpty(ankerMakeSettings.Path))
                    {
                        System.Diagnostics.Process.Start(ankerMakeSettings.Path, $"\"{FilePathAnker}\"");
                    }
                    else
                    {
                        Application.ShowMessageBox("No AnkerMake executable path entered in settings or file not saved sucessfully.");
                    }
                    break;

                case Commands_e.OpenInPrusa:
                    string? FilePathPrusa = null;

                    if (prusaSettings.FileType != FileType._NONE)
                    {
                        FilePathPrusa = SaveCurrentPart(exportSettings.ExportPath, prusaSettings.FileType);
                    }
                    else
                    {
                        Application.ShowMessageBox("Select file format in settings.");
                    }

                    if (!string.IsNullOrEmpty(FilePathPrusa) && !string.IsNullOrEmpty(prusaSettings.Path))
                    {
                        System.Diagnostics.Process.Start(prusaSettings.Path, $"\"{FilePathPrusa}\"");
                    }
                    else
                    {
                        Application.ShowMessageBox("No Prusa executable path entered in settings or file not saved sucessfully.");
                    }
                    break;

                case Commands_e.OpenInSlic3r:
                    string? FilePathSlic3r = null;

                    if (slic3rSettings.FileType != FileType._NONE)
                    {
                        FilePathSlic3r = SaveCurrentPart(exportSettings.ExportPath, slic3rSettings.FileType);
                    }
                    else
                    {
                        Application.ShowMessageBox("Select file format in settings.");
                    }

                    if (!string.IsNullOrEmpty(FilePathSlic3r) && !string.IsNullOrEmpty(slic3rSettings.Path))
                    {
                        System.Diagnostics.Process.Start(slic3rSettings.Path, $"\"{FilePathSlic3r}\"");
                    }
                    else
                    {
                        Application.ShowMessageBox("No Slic3r executable path entered in settings or file not saved sucessfully.");
                    }
                    break;

                case Commands_e.Settings:
                    LoadSettings();
                    ShowSettingsDialog();
                    break;
            }
        }

        private string? SaveCurrentPart(string savePath, FileType format)
        {
            var activeDoc = this.Application.Documents.Active;
            if (activeDoc != null)
            {
                var swModel = activeDoc.Model as ModelDoc2;
                if (swModel != null)
                {
                    string fileName = System.IO.Path.ChangeExtension(swModel.GetTitle(), format.ToString().ToLower().TrimStart('_'));
                    string? fullPath = System.IO.Path.Combine(savePath, fileName);

                    int errors = 0;
                    int warnings = 0;

                    ModelDocExtension? extension = swModel.Extension;
                    bool status = extension.SaveAs(fullPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref errors, ref warnings);
                    if (status)
                    {
                        this.Application.ShowMessageBox($"Part saved as {format.ToString().TrimStart('_')} at: {fullPath}");
                        return fullPath; // Return the path of the saved file
                    }
                    else
                    {
                        this.Application.ShowMessageBox($"Failed to save part as {format}.");
                    }
                }
            }
            else
            {
                this.Application.ShowMessageBox("No active part document found or the document is not a part.");
            }
            return null; // Return null if saving fails or no document is active
        }
    }
}