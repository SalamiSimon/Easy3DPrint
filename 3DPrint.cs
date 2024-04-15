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


        private readonly string settingsFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Easy3DPrintSettings.json");

        [Title("Easy3DPrint")]

        public enum Commands_e 
        {
            [Title("Open in Ultimaker Cura")]
            [Description("Opens the model in Ultimaker Cura")]
            [Icon(typeof(Resources), nameof(Resources.cura))]
            OpenInUltimakerCura,
            [Title("Open in Bambu Studio")]
            [Description("Opens the model in Bambu Lab")]
            [Icon(typeof(Resources), nameof(Resources.bambu))]
            OpenInBambuLab,
            [Title("Settings")]
            [Description("Easy3DPrint Settings")]
            [Icon(typeof(Resources), nameof(Resources.settings))]
            Settings
        }
        public override void OnConnect()
        {
            if (!LoadSettings())
            {
                Application.ShowMessageBox("Before use, enter executable paths and filetype in Easy3DPrint settings.");
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
                if (settings.ExportPath != null)
                    exportSettings.Path = settings.ExportPath;

                if (settings.CuraPath != null)
                    curaSettings.Path = settings.CuraPath;

                if (settings.ExportFormatCura != null)
                    curaSettings.FileType = settings.ExportFormatCura;

                if (settings.BambuPath != null)
                    bambuSettings.Path = settings.BambuPath;

                if (settings.ExportFormatBambu != null)
                    bambuSettings.FileType = settings.ExportFormatBambu;

                return true; // Settings loaded successfully
            }
            // Settings not loaded
            return false;
        }

        private void ShowSettingsDialog()
        {
            SettingsDialog settingsDialog = new SettingsDialog(exportSettings, curaSettings, bambuSettings);
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
                        FilePathCura = SaveCurrentPart(exportSettings.Path, curaSettings.FileType);
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
                        FilePathBambu = SaveCurrentPart(exportSettings.Path, bambuSettings.FileType);
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
                        this.Application.ShowMessageBox($"Part saved as {format} at: {fullPath}");
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