#nullable enable
using Easy3DPrint_NetFW.Properties;
using Newtonsoft.Json;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.SolidWorks;
using Xarial.XCad.UI.Commands;
using Xarial.XCad.UI.Commands.Structures;
using static Easy3DPrint_NetFW.ApplicationSettings;

namespace Easy3DPrint_NetFW
{
    [ComVisible(true)]
    [Guid("0FE9F3CB-62B7-4604-9D33-7918B49E4675")]
    [Title("Easy3DPrint")]

    public class Easy3DPrint : SwAddInEx
    {
        private readonly AddinSettings addInSettings = new();
        private readonly CuraSettings curaSettings = new();
        private readonly BambuSettings bambuSettings = new();
        private readonly AnkerMakeSettings ankerMakeSettings = new();
        private readonly PrusaSettings prusaSettings = new();
        private readonly Slic3rSettings slic3rSettings = new();
        private readonly OrcaSettings orcaSettings = new();

        [Title("Easy3DPrint")]
        [Description("Open parts directly in slicing apps")]

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

            [Title("Open in eufyMake Studio")]
            [Description("Opens the model in eufyMake Studio")]
            [Icon(typeof(Resources), nameof(Resources.ankermake))]
            OpenInAnkerMake,

            [Title("Open in Prusa")]
            [Description("Opens the model in Prusa")]
            [Icon(typeof(Resources), nameof(Resources.prusa))]
            OpenInPrusa,

            [Title("Open in Slic3r")]
            [Description("Opens the model in Slic3r")]
            [Icon(typeof(Resources), nameof(Resources.slic3r))]
            OpenInSlic3r,

            [Title("Open in Orca")]
            [Description("Opens the model in Orca")]
            [Icon(typeof(Resources), nameof(Resources.orca))]
            OpenInOrca,

            [Title("Settings")]
            [Description("Easy3DPrint Settings")]
            [Icon(typeof(Resources), nameof(Resources.settings))]
            Settings,

            [Title("Quick Save")]
            [Description("Quick Export File")]
            [Icon(typeof(Resources), nameof(Resources.quicksave))]
            QuickSave,

            [Title("View Github Repo")]
            [Description("Easy3DPrint Github Repo")]
            [Icon(typeof(Resources), nameof(Resources.github))]
            Github,

            [Title("Check for Update")]
            [Description("Check for update")]
            [Icon(typeof(Resources), nameof(Resources.update))]
            UpdateCheck,

            [Title("Support Me!")]
            [Description("Buy me a coffee")]
            [Icon(typeof(Resources), nameof(Resources.coffee))]
            BuyMeCoffe
        }

        public override void OnConnect()
        {

            if (!LoadSettings())
            {
                Application.ShowMessageBox("Before use, enter executable paths and filetype in Easy3DPrint settings.");

                ShowSettingsDialog();

                if (!Directory.Exists(addInSettings.ExportPath))
                {
                    Directory.CreateDirectory(addInSettings.ExportPath);
                }

                LoadSettings();
            }
            var cmdGrp = CommandManager.AddCommandGroup<Commands_e>();
            cmdGrp.CommandStateResolve += OnButtonEnable;
            cmdGrp.CommandClick += OnCommandClick;

        }

        private void OnButtonEnable(Commands_e cmd, CommandState state)
        {
            switch (cmd)
            {
                case Commands_e.OpenInUltimakerCura:
                    state.Enabled = curaSettings.Enabled;
                    break;
                case Commands_e.OpenInBambuLab:
                    state.Enabled = bambuSettings.Enabled;
                    break;
                case Commands_e.OpenInAnkerMake:
                    state.Enabled = ankerMakeSettings.Enabled;
                    break;
                case Commands_e.OpenInPrusa:
                    state.Enabled = prusaSettings.Enabled;
                    break;
                case Commands_e.OpenInSlic3r:
                    state.Enabled = slic3rSettings.Enabled;
                    break;
                case Commands_e.OpenInOrca:
                    state.Enabled = orcaSettings.Enabled;
                    break;
                case Commands_e.UpdateCheck:
                    state.Enabled = true;
                    break;
                case Commands_e.BuyMeCoffe:
                    state.Enabled = true;
                    break;
            }
        }

        public bool LoadSettings()
        {
            if (File.Exists(addInSettings.DataPath))
            {
                try
                {
                    string json = File.ReadAllText(addInSettings.DataPath);
                    var settings = JsonConvert.DeserializeObject<dynamic>(json);
                    
                    // Check if settings is null before accessing properties
                    if (settings == null)
                    {
                        return false;
                    }

                    // Load settings
                    addInSettings.ExportPath = settings?.ExportPath ?? addInSettings.ExportPath;
                    addInSettings.QuietMode = settings?.QuietMode ?? false;

                    // Add null checks for all settings
                    curaSettings.Path = settings?.CuraPath ?? string.Empty;
                    curaSettings.FileType = settings?.ExportFormatCura ?? FileType._NONE;
                    curaSettings.Enabled = settings?.CuraEnabled ?? false;

                    bambuSettings.Path = settings?.BambuPath ?? string.Empty;
                    bambuSettings.FileType = settings?.ExportFormatBambu ?? FileType._NONE;
                    bambuSettings.Enabled = settings?.BambuEnabled ?? false;

                    ankerMakeSettings.Path = settings?.AnkerMakePath ?? string.Empty;
                    ankerMakeSettings.FileType = settings?.ExportFormatAnkerMake ?? FileType._NONE;
                    ankerMakeSettings.Enabled = settings?.AnkerMakeEnabled ?? false;

                    slic3rSettings.Path = settings?.Slic3rPath ?? string.Empty;
                    slic3rSettings.FileType = settings?.ExportFormatSlic3r ?? FileType._NONE;
                    slic3rSettings.Enabled = settings?.Slic3rEnabled ?? false;

                    prusaSettings.Path = settings?.PrusaPath ?? string.Empty;
                    prusaSettings.FileType = settings?.ExportFormatPrusa ?? FileType._NONE;
                    prusaSettings.Enabled = settings?.PrusaEnabled ?? false;

                    orcaSettings.Path = settings?.OrcaPath ?? string.Empty;
                    orcaSettings.FileType = settings?.ExportFormatOrca ?? FileType._NONE;
                    orcaSettings.Enabled = settings?.OrcaEnabled ?? false;

                    if (settings?.ExportFormatQuickSave != null)
                    {
                        if (settings.ExportFormatQuickSave is Newtonsoft.Json.Linq.JArray arr)
                        {
                            addInSettings.QuickSaveTypes = arr
                                .Select(x => Enum.TryParse<FileType>(x.ToString(), out var val) ? val : FileType._NONE)
                                .Where(x => x != FileType._NONE)
                                .ToList();
                        }
                        else
                        {
                            // Single value fallback
                            var valStr = settings.ExportFormatQuickSave.ToString();
                            if (Enum.TryParse<FileType>(valStr, out FileType val) && val != FileType._NONE)
                                addInSettings.QuickSaveTypes = new List<FileType> { val };
                        }
                    }

                    return true; // Settings loaded successfully
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while reading the settings file: " + ex.Message);
                }
            }
            // Settings not loaded
            return false;
        }

        public void ShowSettingsDialog()
        {
            SettingsDialog settingsDialog = new(addInSettings, curaSettings, bambuSettings, ankerMakeSettings, prusaSettings, slic3rSettings, orcaSettings);
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
                        FilePathCura = SaveCurrentPart(addInSettings.ExportPath, curaSettings.FileType);
                    }
                    else
                    {
                        Application.ShowMessageBox("Select file format in settings.");
                    }

                    if (!string.IsNullOrEmpty(FilePathCura) && !string.IsNullOrEmpty(curaSettings.Path))
                    {
                        System.Diagnostics.Process.Start(curaSettings.Path, $"\"{FilePathCura}\"");
                    }
                    else
                    {
                        Application.ShowMessageBox("No Cura executable path entered in settings or file not saved sucessfully.");
                    }
                    break;

                case Commands_e.OpenInBambuLab:
                    string? FilePathBambu = null;

                    if (bambuSettings.FileType != FileType._NONE)
                    {
                        FilePathBambu = SaveCurrentPart(addInSettings.ExportPath, bambuSettings.FileType);
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
                        FilePathAnker = SaveCurrentPart(addInSettings.ExportPath, ankerMakeSettings.FileType);
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
                        Application.ShowMessageBox("No eufyMake executable path entered in settings or file not saved sucessfully.");
                    }
                    break;

                case Commands_e.OpenInPrusa:
                    string? FilePathPrusa = null;

                    if (prusaSettings.FileType != FileType._NONE)
                    {
                        FilePathPrusa = SaveCurrentPart(addInSettings.ExportPath, prusaSettings.FileType);
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
                        FilePathSlic3r = SaveCurrentPart(addInSettings.ExportPath, slic3rSettings.FileType);
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

                case Commands_e.OpenInOrca:
                    string? FilePathOrca = null;

                    if (orcaSettings.FileType != FileType._NONE)
                    {
                        FilePathOrca = SaveCurrentPart(addInSettings.ExportPath, orcaSettings.FileType);
                    }
                    else
                    {
                        Application.ShowMessageBox("Select file format in settings.");
                    }

                    if (!string.IsNullOrEmpty(FilePathOrca) && !string.IsNullOrEmpty(orcaSettings.Path))
                    {
                        System.Diagnostics.Process.Start(orcaSettings.Path, $"\"{FilePathOrca}\"");
                    }
                    else
                    {
                        Application.ShowMessageBox("No Orca executable path entered in settings or file not saved sucessfully.");
                    }
                    break;

                case Commands_e.QuickSave:
                    if (addInSettings.QuickSaveTypes != null && addInSettings.QuickSaveTypes.Count > 0)
                    {
                        foreach (var format in addInSettings.QuickSaveTypes)
                        {
                            if (format != FileType._NONE)
                            {
                                SaveCurrentPart(addInSettings.ExportPath, format);
                            }
                        }
                    }
                    else
                    {
                        Application.ShowMessageBox("No QuickSave file formats entered in settings.");
                    }
                    break;

                case Commands_e.Github:
                    System.Diagnostics.Process.Start("https://github.com/SalamiSimon/Easy3DPrint");
                    break;

                case Commands_e.Settings:
                    ShowSettingsDialog();
                    LoadSettings();
                    break;

                case Commands_e.UpdateCheck:
                    CheckForUpdates();
                    break;

                case Commands_e.BuyMeCoffe:
                    var result = Application.ShowMessageBox(
                        "Support my work!\n\nGive a small donation by clicking the OK button below. Any amount is appreciated!",
                        Xarial.XCad.Base.Enums.MessageBoxIcon_e.Info,
                        Xarial.XCad.Base.Enums.MessageBoxButtons_e.OkCancel);
                    
                    if (result == Xarial.XCad.Base.Enums.MessageBoxResult_e.Ok)
                    {
                        OpenDonationPage();
                    }
                    break;
            }
        }

        private void OpenDonationPage()
        {
            try
            {
                string url = "https://buymeacoffee.com/SalamiSimon";
                
                var processStartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                Application.ShowMessageBox("Failed to open donation page.");
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        private async void CheckForUpdates()
        {
            string currentVersion = addInSettings.CurrentVersion;
            string repoOwner = "SalamiSimon";
            string repoName = "Easy3DPrint";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using WebClient client = new();
            try
            {
                client.Headers.Add("User-Agent", "Easy3DPrint/1.0");
                string response = await client.DownloadStringTaskAsync(new Uri($"https://api.github.com/repos/{repoOwner}/{repoName}/releases/latest"));
                dynamic latestRelease = JsonConvert.DeserializeObject(response);

                // Fix possible null reference
                string latestVersion = latestRelease?.tag_name?.ToString() ?? string.Empty;

                if (!string.IsNullOrEmpty(latestVersion) && VersionComparer.IsNewerVersion(currentVersion, latestVersion))
                {
                    Application.ShowMessageBox($"A new version ({latestVersion}) is available!\n\nClick on 'View Github Repo' to download the latest version.");
                }
                else
                {
                    Application.ShowMessageBox($" ({currentVersion}) is the latest version! No update is needed.");
                }
            }
            catch (WebException webEx)
            {
                Application.ShowMessageBox("An error occurred while sending the request.");
                Console.WriteLine("WebException: " + webEx.ToString());
            }
            catch (Exception ex)
            {
                Application.ShowMessageBox("An error occurred while checking for updates.");
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        private string SaveCurrentPart(string savePath, FileType format)
        {
            var activeDoc = this.Application.Documents.Active;
            if (activeDoc != null)
            {
                if (activeDoc.Model is ModelDoc2 swModel)
                {
                    string configName = swModel.ConfigurationManager?.ActiveConfiguration?.Name ?? "Default";
                    string fileName;

                    string ext = format.ToString().ToLower().TrimStart('_');
                    if (configName == "Default")
                    {
                        string baseName = System.IO.Path.GetFileNameWithoutExtension(swModel.GetTitle());
                        fileName = $"{baseName}.{ext}";
                    }
                    else
                    {
                        string baseName = System.IO.Path.GetFileNameWithoutExtension(swModel.GetTitle());
                        string configFileName = $"{baseName}_{configName}";
                        fileName = $"{configFileName}.{ext}";
                    }

                    string fullPath = System.IO.Path.Combine(savePath, fileName);

                    int errors = 0;
                    int warnings = 0;

                    ModelDocExtension extension = swModel.Extension;

                    bool status = extension.SaveAs(fullPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref errors, ref warnings);

                    if (status)
                    {
                        if (!addInSettings.QuietMode)
                        {
                            this.Application.ShowMessageBox($"Part saved as {format.ToString().TrimStart('_')} at: {fullPath}");
                        }
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
            return null!;
        }
    }
}