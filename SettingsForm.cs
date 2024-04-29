using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Windows.Forms;
using static Easy3DPrint_NetFW.ApplicationSettings;
using Formatting = Newtonsoft.Json.Formatting;
using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace Easy3DPrint_NetFW
{
    public class SettingsDialog : Form
    {
        private ComboBox cmbExportFormatCura;
        private ComboBox cmbExportFormatBambuLab;
        private ComboBox cmbExportFormatAnkerMake;
        private ComboBox cmbExportFormatPrusa;
        private ComboBox cmbExportFormatSlic3r;
        private TextBox txtCuraPath;
        private TextBox txtBambuLabPath;
        private TextBox txtAnkerMakePath;
        private TextBox txtPrusaPath;
        private TextBox txtSlic3rPath;
        private TextBox txtExportPath;
        private Button? btnSave;

        public string CuraPath => txtCuraPath.Text;
        public string BambuLabPath => txtBambuLabPath.Text;
        public string AnkerMakePath => txtAnkerMakePath.Text;
        public string PrusaPath => txtPrusaPath.Text;
        public string Slic3rPath => txtSlic3rPath.Text;
        public string ExportPath => txtExportPath.Text;
        public string ExportFormatCura => cmbExportFormatCura.SelectedItem.ToString();
        public string ExportFormatBambuLab => cmbExportFormatBambuLab.SelectedItem.ToString();
        public string ExportFormatAnkerMake => cmbExportFormatAnkerMake.SelectedItem.ToString();
        public string ExportFormatPrusa => cmbExportFormatPrusa.SelectedItem.ToString();
        public string ExportFormatSlic3r => cmbExportFormatSlic3r.SelectedItem.ToString();

        public SettingsDialog(
            ApplicationSettings.AddinSettings exportSettings,
            ApplicationSettings.CuraSettings curaSettings,
            ApplicationSettings.BambuSettings bambuSettings,
            ApplicationSettings.AnkerMakeSettings ankerMakeSettings,
            ApplicationSettings.PrusaSettings prusaSettings,
            ApplicationSettings.Slic3rSettings slic3rSettings)
        {
            InitializeComponents();

            txtExportPath.Text = exportSettings?.ExportPath ?? string.Empty;

            txtCuraPath.Text = curaSettings?.Path ?? string.Empty;
            cmbExportFormatCura.SelectedItem = curaSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;

            txtBambuLabPath.Text = bambuSettings?.Path ?? string.Empty;
            cmbExportFormatBambuLab.SelectedItem = bambuSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;

            txtAnkerMakePath.Text = ankerMakeSettings?.Path ?? string.Empty;
            cmbExportFormatAnkerMake.SelectedItem = ankerMakeSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;

            txtPrusaPath.Text = prusaSettings?.Path ?? string.Empty;
            cmbExportFormatPrusa.SelectedItem = prusaSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;

            txtSlic3rPath.Text = slic3rSettings?.Path ?? string.Empty;
            cmbExportFormatSlic3r.SelectedItem = slic3rSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;
        }

        private void InitializeComponents()
        {
            this.Text = "Settings";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Cura Components
            Label lblCuraSettingsTitle = new Label { Text = "UltiMaker Cura", Location = new Point(10, 20), Size = new Size(150, 20) };
            Label lblCuraFormat = new() { Text = "Cura Filetype:", Location = new Point(10, 50), Size = new Size(150, 20) };
            cmbExportFormatCura = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(170, 50), Size = new Size(220, 20) };
            Label lblCuraPath = new() { Text = "Cura .EXE Path:", Location = new Point(10, 80), Size = new Size(150, 20) };
            txtCuraPath = new TextBox { Location = new Point(170, 80), Size = new Size(220, 20) };
            CheckBox chkCuraEnabled = new CheckBox { Text = "Enabled", Location = new Point(10, 110), Size = new Size(150, 20), Checked = true };

            // Bambu Lab Components
            Label lblBambuSettingsTitle = new() { Text = "Bambu Lab", Location = new Point(10, 140), Size = new Size(150, 20) };
            Label lblBambuFormat = new() { Text = "Bambu Filetype:", Location = new Point(10, 170), Size = new Size(150, 20) };
            cmbExportFormatBambuLab = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(170, 170), Size = new Size(220, 20) };
            Label lblBambuPath = new() { Text = "Bambu .EXE Path:", Location = new Point(10, 200), Size = new Size(150, 20) };
            txtBambuLabPath = new TextBox { Location = new Point(170, 200), Size = new Size(220, 20) };
            CheckBox chkBambuEnabled = new CheckBox { Text = "Enabled", Location = new Point(10, 230), Size = new Size(150, 20), Checked = true };

            // AnkerMake Components
            Label lblAnkerMakeSettingsTitle = new() { Text = "AnkerMake", Location = new Point(10, 260), Size = new Size(150, 20) };
            Label lblAnkerMakeFormat = new() { Text = "AnkerMake Filetype:", Location = new Point(10, 290), Size = new Size(150, 20) };
            cmbExportFormatAnkerMake = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(170, 290), Size = new Size(220, 20) };
            Label lblAnkerMakePath = new() { Text = "AnkerMake .EXE Path:", Location = new Point(10, 320), Size = new Size(150, 20) };
            txtAnkerMakePath = new TextBox { Location = new Point(170, 320), Size = new Size(220, 20) };
            CheckBox chkAnkerMakeEnabled = new CheckBox { Text = "Enabled", Location = new Point(10, 350), Size = new Size(150, 20), Checked = true };

            // Prusa Components
            Label lblPrusaSettingsTitle = new() { Text = "Prusa", Location = new Point(10, 380), Size = new Size(150, 20) };
            Label lblPrusaFormat = new() { Text = "Prusa Filetype:", Location = new Point(10, 410), Size = new Size(150, 20) };
            cmbExportFormatPrusa = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(170, 410), Size = new Size(220, 20) };
            Label lblPrusaPath = new() { Text = "Prusa .EXE Path:", Location = new Point(10, 440), Size = new Size(150, 20) };
            txtPrusaPath = new TextBox { Location = new Point(170, 440), Size = new Size(220, 20) };
            CheckBox chkPrusaEnabled = new CheckBox { Text = "Enabled", Location = new Point(10, 470), Size = new Size(150, 20), Checked = true };

            // Slic3r Components
            Label lblSlic3rSettingsTitle = new() { Text = "Slic3r", Location = new Point(10, 500), Size = new Size(150, 20) };
            Label lblSlic3rFormat = new() { Text = "Slic3r Filetype:", Location = new Point(10, 530), Size = new Size(150, 20) };
            cmbExportFormatSlic3r = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(170, 530), Size = new Size(220, 20) };
            Label lblSlic3rPath = new() { Text = "Slic3r .EXE Path:", Location = new Point(10, 560), Size = new Size(150, 20) };
            txtSlic3rPath = new TextBox { Location = new Point(170, 560), Size = new Size(220, 20) };
            CheckBox chkSlic3rEnabled = new CheckBox { Text = "Enabled", Location = new Point(10, 590), Size = new Size(150, 20), Checked = true };

            // Export Path Components
            Label lblExportedTitle = new() { Text = "Exported File Path", Location = new Point(10, 620), Size = new Size(150, 20) };
            Label lblExportPath = new() { Text = "Export Path:", Location = new Point(10, 650), Size = new Size(150, 20) };
            txtExportPath = new TextBox { Location = new Point(170, 650), Size = new Size(220, 20) };

            btnSave = new Button { Text = "Save", Location = new Point(10, 680), Size = new Size(380, 30) };


            // Populate ComboBoxes
            cmbExportFormatCura.Items.AddRange(new string[] { "OBJ", "STL", "3MF" });
            cmbExportFormatBambuLab.Items.AddRange(new string[] { "OBJ", "STL", "STEP", "3MF" });
            cmbExportFormatAnkerMake.Items.AddRange(new string[] { "OBJ", "STL", "3MF" }); // Potentially add PLY & AMF in the future
            cmbExportFormatPrusa.Items.AddRange(new string[] { "OBJ", "STL", "STEP", "3MF" });
            cmbExportFormatSlic3r.Items.AddRange(new string[] { "OBJ", "STL", "3MF" }); // Potentially add AMF in the future

            btnSave.Click += (sender, e) =>
            {
                FileType exportFormatCura = (!string.IsNullOrEmpty(this.ExportFormatCura)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatCura) : FileType._NONE;
                FileType exportFormatBambu = (!string.IsNullOrEmpty(this.ExportFormatBambuLab)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatBambuLab) : FileType._NONE;
                FileType exportFormatAnkerMake = (!string.IsNullOrEmpty(this.ExportFormatAnkerMake)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatAnkerMake) : FileType._NONE;
                FileType exportFormatPrusa = (!string.IsNullOrEmpty(this.ExportFormatPrusa)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatPrusa) : FileType._NONE;
                FileType exportFormatSlic3r = (!string.IsNullOrEmpty(this.ExportFormatSlic3r)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatSlic3r) : FileType._NONE;

                SaveSettings(this.CuraPath, this.ExportPath, exportFormatCura, exportFormatBambu, this.BambuLabPath, this.AnkerMakePath, exportFormatAnkerMake, this.PrusaPath, exportFormatPrusa, this.Slic3rPath, exportFormatSlic3r, chkCuraEnabled.Checked, chkBambuEnabled.Checked, chkAnkerMakeEnabled.Checked, chkPrusaEnabled.Checked, chkSlic3rEnabled.Checked);
            };

            // Add components to the form
            Controls.AddRange(new Control[] {
                lblCuraSettingsTitle, lblCuraFormat, cmbExportFormatCura, lblCuraPath, txtCuraPath, chkCuraEnabled,
                lblBambuSettingsTitle, lblBambuFormat, cmbExportFormatBambuLab, lblBambuPath, txtBambuLabPath, chkBambuEnabled,
                lblAnkerMakeSettingsTitle, lblAnkerMakeFormat, cmbExportFormatAnkerMake, lblAnkerMakePath, txtAnkerMakePath, chkAnkerMakeEnabled,
                lblPrusaSettingsTitle, lblPrusaFormat, cmbExportFormatPrusa, lblPrusaPath, txtPrusaPath, chkPrusaEnabled,
                lblSlic3rSettingsTitle, lblSlic3rFormat, cmbExportFormatSlic3r, lblSlic3rPath, txtSlic3rPath, chkSlic3rEnabled,
                lblExportedTitle, lblExportPath, txtExportPath, btnSave
            });

    // Set the size of the form
    Size = new Size(450, 750);
        }

        private void SaveSettings(string curaPath, string exportPath, FileType exportFormatCura, FileType exportFormatBambu, string bambuPath, string ankerMakePath, FileType exportFormatAnkerMake, string prusaPath, FileType exportFormatPrusa, string slic3rPath, FileType exportFormatSlic3r, bool curaEnabled, bool bambuEnabled, bool ankerMakeEnabled, bool prusaEnabled, bool slic3rEnabled)
        {
            var settings = new
            {
                CuraPath = curaPath ?? "",
                ExportPath = exportPath ?? "",
                ExportFormatCura = exportFormatCura,
                ExportFormatBambu = exportFormatBambu,
                BambuPath = bambuPath ?? "",
                AnkerMakePath = ankerMakePath ?? "",
                ExportFormatAnkerMake = exportFormatAnkerMake,
                PrusaPath = prusaPath ?? "",
                ExportFormatPrusa = exportFormatPrusa,
                Slic3rPath = slic3rPath ?? "",
                ExportFormatSlic3r = exportFormatSlic3r,
                CuraEnabled = curaEnabled,
                BambuEnabled = bambuEnabled,
                AnkerMakeEnabled = ankerMakeEnabled,
                PrusaEnabled = prusaEnabled,
                Slic3rEnabled = slic3rEnabled
            };

            AddinSettings addinSettings = new AddinSettings();
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented, new StringEnumConverter());
            File.WriteAllText(addinSettings.DataPath, json);

            MessageBox.Show("Settings saved.");
            this.Close();
        }
    }
}