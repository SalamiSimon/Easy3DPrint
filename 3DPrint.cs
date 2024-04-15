using System.ComponentModel;
using System.Runtime.InteropServices;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.Documents;
using Xarial.XCad.SolidWorks;
using Xarial.XCad.UI.Commands;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Windows.Forms;
using System.Drawing;
using Newtonsoft.Json;
using System.IO;

namespace _3DPrint_SW
{
    [ComVisible(true)]
    [Guid("0FE9F3CB-62B7-4604-9D33-7918B49E4675")]
    [Title("Easy3DPrint")]

    public class Easy3DPrint : SwAddInEx
    {
        private string exportPath = "";

        private string curaPath = "";
        private string exportFormatCura = "";

        private string bambuPath = "";
        private string exportFormatBambu = "";


        private readonly string settingsFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Easy3DPrintSettings.json");

        [Title("Easy3DPrint")]

        public enum Commands_e 
        {
            [Title("Open in Ultimaker Cura")]
            [Description("Opens the model in Ultimaker Cura")]
            OpenInUltimakerCura,
            [Title("Open in Bambu Lab")]
            [Description("Opens the model in Bambu Lab")]
            OpenInBambuLab,
            [Title("Settings")]
            [Description("Easy3DPrint Settings")]
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
                //if (settings.CuraPath != null)
                exportPath = settings.ExportPath;

                curaPath = settings.CuraPath;
                exportFormatCura = settings.ExportFormatCura;

                bambuPath = settings.BambuPath;
                exportFormatBambu = settings.ExportFormatBambu;
                return true; // Settings loaded successfully
            }
            // Settings not loaded
            return false;
        }

        private void ShowSettingsDialog()
        {
            SettingsDialog settingsDialog = new SettingsDialog(curaPath, exportPath, exportFormatCura, bambuPath, exportFormatBambu);
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

                    if (exportFormatCura == "STL")
                    {
                        FilePathCura = SaveCurrentPartAsSTL(exportPath);
                    }
                    else if (exportFormatCura == "OBJ")
                    {
                        FilePathCura = SaveCurrentPartAsOBJ(exportPath);
                    }
                    else if (exportFormatCura == "3MF")
                    {
                        FilePathCura = SaveCurrentPartAs3MF(exportPath);
                    } else
                    {
                        Application.ShowMessageBox("Select file format in settings.");
                    }

                    if (!string.IsNullOrEmpty(FilePathCura) && !string.IsNullOrEmpty(curaPath))
                    {
                        System.Diagnostics.Process.Start(curaPath, $"\"{FilePathCura}\"");
                    } else
                    {
                        Application.ShowMessageBox("No Cura executable path entered in settings or file not saved sucessfully.");
                    }
                break;

                case Commands_e.OpenInBambuLab:
                    string? FilePathBambu = null;

                    if (exportFormatBambu == "STL")
                    {
                        FilePathBambu = SaveCurrentPartAsSTL(exportPath);
                    }
                    else if (exportFormatBambu == "OBJ")
                    {
                        FilePathBambu = SaveCurrentPartAsOBJ(exportPath);
                    }
                    else if (exportFormatBambu == "3MF")
                    {
                        FilePathBambu = SaveCurrentPartAs3MF(exportPath);
                    }
                    else if (exportFormatBambu == "STEP")
                    {
                        FilePathBambu = SaveCurrentPartAsSTEP(exportPath);
                    }
                    else
                    {
                        Application.ShowMessageBox("Select file format in settings.");
                    }

                    if (!string.IsNullOrEmpty(FilePathBambu) && !string.IsNullOrEmpty(bambuPath))
                    {
                        System.Diagnostics.Process.Start(bambuPath, $"\"{FilePathBambu}\"");
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

        private string? SaveCurrentPartAsSTL(string savePath)
        {
            var activeDoc = this.Application.Documents.Active;
            if (activeDoc != null)
            {
                var swModel = activeDoc.Model as ModelDoc2;
                if (swModel != null)
                {
                    string fileName = System.IO.Path.ChangeExtension(swModel.GetTitle(), "stl");
                    string? fullPath = System.IO.Path.Combine(savePath, fileName);

                    swModel.Extension?.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swSTLQuality, 0, (int)swSTLQuality_e.swSTLQuality_Fine);
                    swModel.Extension?.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swSTLBinaryFormat, 0, true);

                    int errors = 0;
                    int warnings = 0;

                    ModelDocExtension? extension = swModel.Extension;
                    bool status = extension.SaveAs(fullPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref errors, ref warnings);
                    if (status)
                    {
                        this.Application.ShowMessageBox($"Part saved as STL at: {fullPath}");
                        return fullPath; // Return the path of the saved file
                    }
                    else
                    {
                        this.Application.ShowMessageBox("Failed to save part as STL.");
                    }
                }
            }
            else
            {
                this.Application.ShowMessageBox("No active part document found or the document is not a part.");
            }
            return null; // Return null if saving fails or no document is active
        }

        private string? SaveCurrentPartAsSTEP(string savePath)
        {
            var activeDoc = this.Application.Documents.Active;
            if (activeDoc != null)
            {
                ModelDoc2? swModel = activeDoc.Model as ModelDoc2;
                if (swModel != null)
                {
                    string fileName = System.IO.Path.ChangeExtension(swModel.GetTitle(), "step");
                    string fullPath = System.IO.Path.Combine(savePath, fileName);

                    int errors = 0;
                    int warnings = 0;

                    bool status = swModel.Extension.SaveAs(fullPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref errors, ref warnings);
                    if (status)
                    {
                        this.Application.ShowMessageBox($"Part saved as STEP at: {fullPath}");
                        return fullPath;
                    }
                    else
                    {
                        this.Application.ShowMessageBox("Failed to save part as STEP.");
                    }
                }
            }
            else
            {
                this.Application.ShowMessageBox("No active part document found or the document is not a part. Canceled");
            }
            return null;
        }

        private string? SaveCurrentPartAsOBJ(string savePath)
        {
            var activeDoc = this.Application.Documents.Active;
            if (activeDoc != null)
            {
                var swModel = activeDoc.Model as ModelDoc2;
                if (swModel != null)
                {
                    string fileName = System.IO.Path.ChangeExtension(swModel.GetTitle(), "obj");
                    string? fullPath = System.IO.Path.Combine(savePath, fileName);

                    int errors = 0;
                    int warnings = 0;

                    ModelDocExtension? extension = swModel.Extension;
                    bool status = extension.SaveAs(fullPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref errors, ref warnings);
                    if (status)
                    {
                        this.Application.ShowMessageBox($"Part saved as OBJ at: {fullPath}");
                        return fullPath; // Return the path of the saved file
                    }
                    else
                    {
                        this.Application.ShowMessageBox("Failed to save part as OBJ.");
                    }
                }
            }
            else
            {
                this.Application.ShowMessageBox("No active part document found or the document is not a part.");
            }
            return null; // Return null if saving fails or no document is active
        }


        private string? SaveCurrentPartAs3MF(string savePath)
        {
            var activeDoc = this.Application.Documents.Active;
            if (activeDoc != null)
            {
                var swModel = activeDoc.Model as ModelDoc2;
                if (swModel != null)
                {
                    string fileName = System.IO.Path.ChangeExtension(swModel.GetTitle(), "3mf");
                    string? fullPath = System.IO.Path.Combine(savePath, fileName);

                    int errors = 0;
                    int warnings = 0;

                    ModelDocExtension? extension = swModel.Extension;
                    bool status = extension.SaveAs(fullPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref errors, ref warnings);
                    if (status)
                    {
                        this.Application.ShowMessageBox($"Part saved as 3MF at: {fullPath}");
                        return fullPath; // Return the path of the saved file
                    }
                    else
                    {
                        this.Application.ShowMessageBox("Failed to save part as 3MF.");
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

public class SettingsDialog : Form
{
    private readonly string settingsFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Easy3DPrintSettings.json");

    private ComboBox cmbExportFormatCura;
    private ComboBox cmbExportFormatBambuLab;
    private TextBox txtCuraPath;
    private TextBox txtBambuLabPath;
    private TextBox txtExportPath;
    private Button btnSave;

    public string CuraPath => txtCuraPath.Text;
    public string BambuLabPath => txtBambuLabPath.Text;
    public string ExportPath => txtExportPath.Text;
    public string ExportFormatCura => cmbExportFormatCura.SelectedItem.ToString();
    public string ExportFormatBambuLab => cmbExportFormatBambuLab.SelectedItem.ToString();

    public SettingsDialog(string curaPath, string exportPath, string exportFormatCura, string bambuPath, string exportFormatBambulab)
    {
        InitializeComponents();

        txtCuraPath.Text = curaPath;
        txtExportPath.Text = exportPath;
        cmbExportFormatCura.SelectedItem = exportFormatCura;
        txtBambuLabPath.Text = bambuPath;
        cmbExportFormatBambuLab.Text = exportFormatBambulab;
    }

    private void InitializeComponents()
    {
        this.Text = "Settings";
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;

        
        Label lblCuraSettingsTitle = new Label { Text = "UltiMaker Cura", Location = new Point(10, 20), Size = new Size(150, 20) };

        Label lblCuraFormat = new Label { Text = "Cura Filetype:", Location = new Point(10, 50), Size = new Size(150, 20) };
        cmbExportFormatCura = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(170, 50), Size = new Size(220, 20) };

        Label lblCuraPath = new Label { Text = "Cura .EXE Path:", Location = new Point(10, 80), Size = new Size(150, 20) };
        txtCuraPath = new TextBox { Location = new Point(170, 80), Size = new Size(220, 20) };

        Label lblBambuSettingsTitle = new Label { Text = "Bambu Lab", Location = new Point(10, 120), Size = new Size(150, 20) };

        Label lblBambuFormat = new Label { Text = "Bambu Filetype:", Location = new Point(10, 150), Size = new Size(150, 20) };
        cmbExportFormatBambuLab = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(170, 150), Size = new Size(220, 20) };

        Label lblBambuPath = new Label { Text = "Bambu .EXE Path:", Location = new Point(10, 180), Size = new Size(150, 20) };
        txtBambuLabPath = new TextBox { Location = new Point(170, 180), Size = new Size(220, 20) };

        Label lblExportedTitle = new Label { Text = "Exported File Path", Location = new Point(10, 220), Size = new Size(150, 20) };

        Label lblExportPath = new Label { Text = "Export Path:", Location = new Point(10, 250), Size = new Size(150, 20) };
        txtExportPath = new TextBox { Location = new Point(170, 250), Size = new Size(220, 20) };

        btnSave = new Button { Text = "Save", Location = new Point(10, 290), Size = new Size(380, 30) };

        // Populate ComboBoxes
        cmbExportFormatCura.Items.AddRange(new string[] { "OBJ", "STL", "3MF" });
        cmbExportFormatBambuLab.Items.AddRange(new string[] { "OBJ", "STL", "STEP", "3MF" });

        btnSave.Click += (sender, e) => SaveSettings(this.CuraPath, this.ExportPath, this.ExportFormatCura, this.BambuLabPath, this.ExportFormatBambuLab);

        // Add components to the form
        Controls.Add(lblCuraSettingsTitle);
        Controls.Add(lblCuraFormat);
        Controls.Add(cmbExportFormatCura);
        Controls.Add(lblCuraPath);
        Controls.Add(txtCuraPath);

        Controls.Add(lblBambuSettingsTitle);
        Controls.Add(lblBambuFormat);
        Controls.Add(cmbExportFormatBambuLab);
        Controls.Add(lblBambuPath);
        Controls.Add(txtBambuLabPath);

        Controls.Add(lblExportedTitle);
        Controls.Add(lblExportPath);
        Controls.Add(txtExportPath);
        Controls.Add(btnSave);

        // Set the size of the form
        Size = new System.Drawing.Size(450, 500); //x,y
    }

    private void SaveSettings(string curaPath, string exportPath, string exportFormatCura, string bambuPath, string exportFormatBambu)
    {
        var settings = new
        {
            CuraPath = curaPath,
            ExportPath = exportPath,
            ExportFormatCura = exportFormatCura,
            ExportFormatBambu = exportFormatBambu,
            BambuPath = bambuPath
        };

        string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
        File.WriteAllText(settingsFilePath, json);

        MessageBox.Show("Settings saved.");
        this.Close();
    }
}