using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Drawing;
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
        private ComboBox cmbQuickSaveFileType;

        private TextBox txtCuraPath;
        private TextBox txtBambuLabPath;
        private TextBox txtAnkerMakePath;
        private TextBox txtPrusaPath;
        private TextBox txtSlic3rPath;
        private TextBox txtExportPath;

        private CheckBox chkCuraEnabled;
        private CheckBox chkSlic3rEnabled;
        private CheckBox chkPrusaEnabled;
        private CheckBox chkAnkerMakeEnabled;
        private CheckBox chkBambuEnabled;

        private Button btnSave;

        public string ExportPath => txtExportPath.Text;
        public string CuraPath => txtCuraPath.Text;
        public string BambuLabPath => txtBambuLabPath.Text;
        public string AnkerMakePath => txtAnkerMakePath.Text;
        public string PrusaPath => txtPrusaPath.Text;
        public string Slic3rPath => txtSlic3rPath.Text;
        public string ExportFormatCura => cmbExportFormatCura.SelectedItem.ToString();
        public string ExportFormatBambuLab => cmbExportFormatBambuLab.SelectedItem.ToString();
        public string ExportFormatAnkerMake => cmbExportFormatAnkerMake.SelectedItem.ToString();
        public string ExportFormatPrusa => cmbExportFormatPrusa.SelectedItem.ToString();
        public string ExportFormatSlic3r => cmbExportFormatSlic3r.SelectedItem.ToString();
        public string ExportFormatQuickSave => cmbQuickSaveFileType.SelectedItem.ToString();

        public SettingsDialog(
            ApplicationSettings.AddinSettings addInSettings,
            ApplicationSettings.CuraSettings curaSettings,
            ApplicationSettings.BambuSettings bambuSettings,
            ApplicationSettings.AnkerMakeSettings ankerMakeSettings,
            ApplicationSettings.PrusaSettings prusaSettings,
            ApplicationSettings.Slic3rSettings slic3rSettings)
        {
            InitializeComponents();

            txtExportPath.Text = addInSettings?.ExportPath ?? string.Empty;

            txtCuraPath.Text = curaSettings?.Path ?? string.Empty;
            cmbExportFormatCura.SelectedItem = curaSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;
            chkCuraEnabled.Checked = curaSettings.Enabled;

            txtBambuLabPath.Text = bambuSettings?.Path ?? string.Empty;
            cmbExportFormatBambuLab.SelectedItem = bambuSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;
            chkBambuEnabled.Checked = bambuSettings.Enabled;

            txtAnkerMakePath.Text = ankerMakeSettings?.Path ?? string.Empty;
            cmbExportFormatAnkerMake.SelectedItem = ankerMakeSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;
            chkAnkerMakeEnabled.Checked = ankerMakeSettings.Enabled;

            txtPrusaPath.Text = prusaSettings?.Path ?? string.Empty;
            cmbExportFormatPrusa.SelectedItem = prusaSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;
            chkPrusaEnabled.Checked = prusaSettings.Enabled;

            txtSlic3rPath.Text = slic3rSettings?.Path ?? string.Empty;
            cmbExportFormatSlic3r.SelectedItem = slic3rSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;
            chkSlic3rEnabled.Checked = slic3rSettings.Enabled;

            cmbQuickSaveFileType.SelectedItem = addInSettings?.QuickSaveType.ToString().TrimStart('_') ?? string.Empty;
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
            lblCuraSettingsTitle.Font = new Font(lblCuraSettingsTitle.Font, FontStyle.Bold);
            lblCuraSettingsTitle.Font = new Font(lblCuraSettingsTitle.Font.FontFamily, lblCuraSettingsTitle.Font.Size + 1, FontStyle.Bold);
            chkCuraEnabled = new CheckBox { Text = "Cura Enabled", Location = new Point(10, 50), Size = new Size(150, 20)};
            Label lblCuraFormat = new() { Text = "Cura Filetype:", Location = new Point(10, 80), Size = new Size(150, 20) };
            cmbExportFormatCura = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(170, 80), Size = new Size(220, 20) };
            Label lblCuraPath = new() { Text = "Cura .EXE Path:", Location = new Point(10, 110), Size = new Size(150, 20) };
            txtCuraPath = new TextBox { Location = new Point(170, 110), Size = new Size(220, 20) };
            Button btnBrowseCuraPath = new Button { Text = "Browse", Location = new Point(400, 110), Size = new Size(75, 20) };
            btnBrowseCuraPath.Click += (sender, e) => {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Executable files (*.exe)|*.exe";
                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    txtCuraPath.Text = openFileDialog.FileName;
                }
            };

            lblCuraFormat.Visible = chkCuraEnabled.Checked;
            cmbExportFormatCura.Visible = chkCuraEnabled.Checked;
            lblCuraPath.Visible = chkCuraEnabled.Checked;
            txtCuraPath.Visible = chkCuraEnabled.Checked;
            btnBrowseCuraPath.Visible = chkCuraEnabled.Checked;

            chkCuraEnabled.CheckedChanged += (sender, e) =>
            {
                lblCuraFormat.Visible = chkCuraEnabled.Checked;
                cmbExportFormatCura.Visible = chkCuraEnabled.Checked;
                lblCuraPath.Visible = chkCuraEnabled.Checked;
                txtCuraPath.Visible = chkCuraEnabled.Checked;
                btnBrowseCuraPath.Visible = chkCuraEnabled.Checked;
            };

            // Bambu Lab Components
            Label lblBambuSettingsTitle = new() { Text = "Bambu Lab", Location = new Point(10, 140), Size = new Size(150, 20) };
            lblBambuSettingsTitle.Font = new Font(lblBambuSettingsTitle.Font, FontStyle.Bold);
            lblBambuSettingsTitle.Font = new Font(lblBambuSettingsTitle.Font.FontFamily, lblBambuSettingsTitle.Font.Size + 1, FontStyle.Bold);
            chkBambuEnabled = new CheckBox { Text = "Bambu Enabled", Location = new Point(10, 170), Size = new Size(150, 20)};
            Label lblBambuFormat = new() { Text = "Bambu Filetype:", Location = new Point(10, 200), Size = new Size(150, 20) };
            cmbExportFormatBambuLab = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(170, 200), Size = new Size(220, 20) };
            Label lblBambuPath = new() { Text = "Bambu .EXE Path:", Location = new Point(10, 230), Size = new Size(150, 20) };
            txtBambuLabPath = new TextBox { Location = new Point(170, 230), Size = new Size(220, 20) };
            Button btnBrowseBambuPath = new Button { Text = "Browse", Location = new Point(400, 230), Size = new Size(75, 20) };
            btnBrowseBambuPath.Click += (sender, e) => {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Executable files (*.exe)|*.exe";
                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    txtBambuLabPath.Text = openFileDialog.FileName;
                }
            };

            lblBambuFormat.Visible = chkBambuEnabled.Checked;
            cmbExportFormatBambuLab.Visible = chkBambuEnabled.Checked;
            lblBambuPath.Visible = chkBambuEnabled.Checked;
            txtBambuLabPath.Visible = chkBambuEnabled.Checked;
            btnBrowseBambuPath.Visible = chkBambuEnabled.Checked;

            chkBambuEnabled.CheckedChanged += (sender, e) =>
            {
                lblBambuFormat.Visible = chkBambuEnabled.Checked;
                cmbExportFormatBambuLab.Visible = chkBambuEnabled.Checked;
                lblBambuPath.Visible = chkBambuEnabled.Checked;
                txtBambuLabPath.Visible = chkBambuEnabled.Checked;
                btnBrowseBambuPath.Visible = chkBambuEnabled.Checked;
            };

            // AnkerMake Components
            Label lblAnkerMakeSettingsTitle = new() { Text = "AnkerMake Studio", Location = new Point(10, 260), Size = new Size(150, 20) };
            lblAnkerMakeSettingsTitle.Font = new Font(lblAnkerMakeSettingsTitle.Font, FontStyle.Bold);
            lblAnkerMakeSettingsTitle.Font = new Font(lblAnkerMakeSettingsTitle.Font.FontFamily, lblAnkerMakeSettingsTitle.Font.Size + 1, FontStyle.Bold);
            chkAnkerMakeEnabled = new CheckBox { Text = "Anker Enabled", Location = new Point(10, 290), Size = new Size(150, 20)};
            Label lblAnkerMakeFormat = new() { Text = "Anker Filetype:", Location = new Point(10, 320), Size = new Size(150, 20) };
            cmbExportFormatAnkerMake = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(170, 320), Size = new Size(220, 20) };
            Label lblAnkerMakePath = new() { Text = "Anker .EXE Path:", Location = new Point(10, 350), Size = new Size(150, 20) };
            txtAnkerMakePath = new TextBox { Location = new Point(170, 350), Size = new Size(220, 20) };
            Button btnBrowseAnkerMakePath = new Button { Text = "Browse", Location = new Point(400, 350), Size = new Size(75, 20) };
            btnBrowseAnkerMakePath.Click += (sender, e) => {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Executable files (*.exe)|*.exe";
                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    txtAnkerMakePath.Text = openFileDialog.FileName;
                }
            };

            lblAnkerMakeFormat.Visible = chkAnkerMakeEnabled.Checked;
            cmbExportFormatAnkerMake.Visible = chkAnkerMakeEnabled.Checked;
            lblAnkerMakePath.Visible = chkAnkerMakeEnabled.Checked;
            txtAnkerMakePath.Visible = chkAnkerMakeEnabled.Checked;
            btnBrowseAnkerMakePath.Visible = chkAnkerMakeEnabled.Checked;

            chkAnkerMakeEnabled.CheckedChanged += (sender, e) =>
            {
                lblAnkerMakeFormat.Visible = chkAnkerMakeEnabled.Checked;
                cmbExportFormatAnkerMake.Visible = chkAnkerMakeEnabled.Checked;
                lblAnkerMakePath.Visible = chkAnkerMakeEnabled.Checked;
                txtAnkerMakePath.Visible = chkAnkerMakeEnabled.Checked;
                btnBrowseAnkerMakePath.Visible = chkAnkerMakeEnabled.Checked;
            };

            // Prusa Components
            Label lblPrusaSettingsTitle = new() { Text = "Prusa", Location = new Point(10, 380), Size = new Size(150, 20) };
            lblPrusaSettingsTitle.Font = new Font(lblPrusaSettingsTitle.Font, FontStyle.Bold);
            lblPrusaSettingsTitle.Font = new Font(lblPrusaSettingsTitle.Font.FontFamily, lblPrusaSettingsTitle.Font.Size + 1, FontStyle.Bold);
            chkPrusaEnabled = new CheckBox { Text = "Prusa Enabled", Location = new Point(10, 410), Size = new Size(150, 20)};
            Label lblPrusaFormat = new() { Text = "Prusa Filetype:", Location = new Point(10, 440), Size = new Size(150, 20) };
            cmbExportFormatPrusa = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(170, 440), Size = new Size(220, 20) };
            Label lblPrusaPath = new() { Text = "Prusa .EXE Path:", Location = new Point(10, 470), Size = new Size(150, 20) };
            txtPrusaPath = new TextBox { Location = new Point(170, 470), Size = new Size(220, 20) };
            Button btnBrowsePrusaPath = new Button { Text = "Browse", Location = new Point(400, 470), Size = new Size(75, 20) };
            btnBrowsePrusaPath.Click += (sender, e) => {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Executable files (*.exe)|*.exe";
                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    txtPrusaPath.Text = openFileDialog.FileName;
                }
            };

            lblPrusaFormat.Visible = chkPrusaEnabled.Checked;
            cmbExportFormatPrusa.Visible = chkPrusaEnabled.Checked;
            lblPrusaPath.Visible = chkPrusaEnabled.Checked;
            txtPrusaPath.Visible = chkPrusaEnabled.Checked;
            btnBrowsePrusaPath.Visible = chkPrusaEnabled.Checked;

            chkPrusaEnabled.CheckedChanged += (sender, e) =>
            {
                lblPrusaFormat.Visible = chkPrusaEnabled.Checked;
                cmbExportFormatPrusa.Visible = chkPrusaEnabled.Checked;
                lblPrusaPath.Visible = chkPrusaEnabled.Checked;
                txtPrusaPath.Visible = chkPrusaEnabled.Checked;
                btnBrowsePrusaPath.Visible = chkPrusaEnabled.Checked;
            };

            // Slic3r Components
            Label lblSlic3rSettingsTitle = new() { Text = "Slic3r", Location = new Point(10, 500), Size = new Size(150, 20) };
            lblSlic3rSettingsTitle.Font = new Font(lblSlic3rSettingsTitle.Font, FontStyle.Bold);
            lblSlic3rSettingsTitle.Font = new Font(lblSlic3rSettingsTitle.Font.FontFamily, lblSlic3rSettingsTitle.Font.Size + 1, FontStyle.Bold);
            chkSlic3rEnabled = new CheckBox { Text = "Slic3r Enabled", Location = new Point(10, 530), Size = new Size(150, 20) };
            Label lblSlic3rFormat = new() { Text = "Slic3r Filetype:", Location = new Point(10, 560), Size = new Size(150, 20) };
            cmbExportFormatSlic3r = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(170, 560), Size = new Size(220, 20) };
            Label lblSlic3rPath = new() { Text = "Slic3r .EXE Path:", Location = new Point(10, 590), Size = new Size(150, 20) };
            txtSlic3rPath = new TextBox { Location = new Point(170, 590), Size = new Size(220, 20) };
            Button btnBrowseSlic3rPath = new Button { Text = "Browse", Location = new Point(400, 590), Size = new Size(75, 20) };
            btnBrowseSlic3rPath.Click += (sender, e) => {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Executable files (*.exe)|*.exe";
                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    txtSlic3rPath.Text = openFileDialog.FileName;
                }
            };

            lblSlic3rFormat.Visible = chkSlic3rEnabled.Checked;
            cmbExportFormatSlic3r.Visible = chkSlic3rEnabled.Checked;
            lblSlic3rPath.Visible = chkSlic3rEnabled.Checked;
            txtSlic3rPath.Visible = chkSlic3rEnabled.Checked;
            btnBrowseSlic3rPath.Visible = chkSlic3rEnabled.Checked;

            chkSlic3rEnabled.CheckedChanged += (sender, e) =>
            {
                lblSlic3rFormat.Visible = chkSlic3rEnabled.Checked;
                cmbExportFormatSlic3r.Visible = chkSlic3rEnabled.Checked;
                lblSlic3rPath.Visible = chkSlic3rEnabled.Checked;
                txtSlic3rPath.Visible = chkSlic3rEnabled.Checked;
                btnBrowseSlic3rPath.Visible = chkSlic3rEnabled.Checked;
            };

            // Add-in settings Components
            Label lblExportedTitle = new() { Text = "Add-In Settings", Location = new Point(10, 620), Size = new Size(150, 20) };
            lblExportedTitle.Font = new Font(lblExportedTitle.Font, FontStyle.Bold);
            lblExportedTitle.Font = new Font(lblExportedTitle.Font.FontFamily, lblExportedTitle.Font.Size + 1, FontStyle.Bold);
            Label lblQuickSaveFileTypeTitle = new() { Text = "QuickSave Filetype", Location = new Point(10, 650), Size = new Size(150, 20) };
            cmbQuickSaveFileType = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(170, 650), Size = new Size(220, 20) };
            Label lblExportPath = new() { Text = "File Export Path:", Location = new Point(10, 680), Size = new Size(150, 20) };
            txtExportPath = new TextBox { Location = new Point(170, 680), Size = new Size(220, 20) };
            Button btnBrowseExportPath = new Button { Text = "Browse", Location = new Point(400, 680), Size = new Size(75, 20) };
            btnBrowseExportPath.Click += (sender, e) => {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                    txtExportPath.Text = folderBrowserDialog.SelectedPath;
                }
            };

            // Save button
            btnSave = new Button { Text = "Save", Location = new Point(10, 720), Size = new Size(450, 30) };

            // Populate ComboBoxes
            cmbExportFormatCura.Items.AddRange(new string[] { "OBJ", "STL", "3MF" });
            cmbExportFormatBambuLab.Items.AddRange(new string[] { "OBJ", "STL", "STEP", "3MF" });
            cmbExportFormatAnkerMake.Items.AddRange(new string[] { "OBJ", "STL", "3MF" }); // Potentially add PLY & AMF in the future
            cmbExportFormatPrusa.Items.AddRange(new string[] { "OBJ", "STL", "STEP", "3MF" });
            cmbExportFormatSlic3r.Items.AddRange(new string[] { "OBJ", "STL", "3MF" }); // Potentially add AMF in the future
            cmbQuickSaveFileType.Items.AddRange(new string[] { "OBJ", "STL", "STEP", "3MF" });

            btnSave.Click += (sender, e) =>
            {
                FileType exportFormatCura = (!string.IsNullOrEmpty(this.ExportFormatCura)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatCura) : FileType._NONE;
                FileType exportFormatBambu = (!string.IsNullOrEmpty(this.ExportFormatBambuLab)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatBambuLab) : FileType._NONE;
                FileType exportFormatAnkerMake = (!string.IsNullOrEmpty(this.ExportFormatAnkerMake)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatAnkerMake) : FileType._NONE;
                FileType exportFormatPrusa = (!string.IsNullOrEmpty(this.ExportFormatPrusa)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatPrusa) : FileType._NONE;
                FileType exportFormatSlic3r = (!string.IsNullOrEmpty(this.ExportFormatSlic3r)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatSlic3r) : FileType._NONE;
                FileType quickSaveFileType = (!string.IsNullOrEmpty(this.ExportFormatQuickSave)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatQuickSave) : FileType._NONE;

                SaveSettings(this.CuraPath, this.ExportPath, exportFormatCura, exportFormatBambu, this.BambuLabPath, this.AnkerMakePath, exportFormatAnkerMake, this.PrusaPath, exportFormatPrusa, this.Slic3rPath, exportFormatSlic3r, quickSaveFileType, chkCuraEnabled.Checked, chkBambuEnabled.Checked, chkAnkerMakeEnabled.Checked, chkPrusaEnabled.Checked, chkSlic3rEnabled.Checked);
            };

            // Add components to the form
            Controls.AddRange(new Control[] {
                lblCuraSettingsTitle, chkCuraEnabled, lblCuraFormat, cmbExportFormatCura, lblCuraPath, txtCuraPath, btnBrowseCuraPath,
                lblBambuSettingsTitle, chkBambuEnabled, lblBambuFormat, cmbExportFormatBambuLab, lblBambuPath, txtBambuLabPath, btnBrowseBambuPath,
                lblAnkerMakeSettingsTitle, chkAnkerMakeEnabled, lblAnkerMakeFormat, cmbExportFormatAnkerMake, lblAnkerMakePath, txtAnkerMakePath, btnBrowseAnkerMakePath,
                lblPrusaSettingsTitle, chkPrusaEnabled, lblPrusaFormat, cmbExportFormatPrusa, lblPrusaPath, txtPrusaPath, btnBrowsePrusaPath,
                lblSlic3rSettingsTitle, chkSlic3rEnabled, lblSlic3rFormat, cmbExportFormatSlic3r, lblSlic3rPath, txtSlic3rPath, btnBrowseSlic3rPath,
                lblExportedTitle, lblExportPath, txtExportPath, btnBrowseExportPath,
                lblQuickSaveFileTypeTitle, cmbQuickSaveFileType, btnSave
            });

        // Set the size of the form
        Size = new Size(500, 800);
        }

        private void SaveSettings(string curaPath, string exportPath, FileType exportFormatCura, FileType exportFormatBambu, string bambuPath, string ankerMakePath, FileType exportFormatAnkerMake, string prusaPath, FileType exportFormatPrusa, string slic3rPath, FileType exportFormatSlic3r, FileType quickSaveFileType, bool curaEnabled, bool bambuEnabled, bool ankerMakeEnabled, bool prusaEnabled, bool slic3rEnabled)
        {
            var settings = new
            {
                CuraPath = curaPath ?? "",
                ExportPath = exportPath ?? "",
                ExportFormatCura = exportFormatCura,
                CuraEnabled = curaEnabled,
                ExportFormatBambu = exportFormatBambu,
                BambuPath = bambuPath ?? "",
                BambuEnabled = bambuEnabled,
                AnkerMakePath = ankerMakePath ?? "",
                ExportFormatAnkerMake = exportFormatAnkerMake,
                AnkerMakeEnabled = ankerMakeEnabled,
                PrusaPath = prusaPath ?? "",
                ExportFormatPrusa = exportFormatPrusa,
                PrusaEnabled = prusaEnabled,
                Slic3rPath = slic3rPath ?? "",
                ExportFormatSlic3r = exportFormatSlic3r,
                Slic3rEnabled = slic3rEnabled,
                ExportFormatQuickSave = quickSaveFileType
            };

            AddinSettings addinSettings = new AddinSettings();
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented, new StringEnumConverter());
            File.WriteAllText(addinSettings.DataPath, json);

            MessageBox.Show("Settings saved.\n\nEnable state will update correctly after the next restart. (Or open and close settings again)");
            this.Close();
        }
    }
}