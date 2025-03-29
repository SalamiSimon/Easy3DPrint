using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static Easy3DPrint_NetFW.ApplicationSettings;
using Formatting = Newtonsoft.Json.Formatting;
using MessageBox = System.Windows.Forms.MessageBox;
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
        private ComboBox cmbExportFormatOrca;
        private ComboBox cmbQuickSaveFileType;

        private TextBox txtCuraPath;
        private TextBox txtBambuLabPath;
        private TextBox txtAnkerMakePath;
        private TextBox txtPrusaPath;
        private TextBox txtSlic3rPath;
        private TextBox txtOrcaPath;
        private TextBox txtExportPath;

        private CheckBox chkQuietMode;
        private CheckBox chkCuraEnabled;
        private CheckBox chkSlic3rEnabled;
        private CheckBox chkPrusaEnabled;
        private CheckBox chkAnkerMakeEnabled;
        private CheckBox chkBambuEnabled;
        private CheckBox chkOrcaEnabled;

        private Button btnSave;

        public string ExportPath => txtExportPath?.Text ?? string.Empty;
        public string CuraPath => txtCuraPath?.Text ?? string.Empty;
        public string BambuLabPath => txtBambuLabPath?.Text ?? string.Empty;
        public string AnkerMakePath => txtAnkerMakePath?.Text ?? string.Empty;
        public string PrusaPath => txtPrusaPath?.Text ?? string.Empty;
        public string Slic3rPath => txtSlic3rPath?.Text ?? string.Empty;
        public string OrcaPath => txtOrcaPath?.Text ?? string.Empty;
        public string ExportFormatCura => cmbExportFormatCura?.SelectedItem?.ToString() ?? string.Empty;
        public string ExportFormatBambuLab => cmbExportFormatBambuLab?.SelectedItem?.ToString() ?? string.Empty;
        public string ExportFormatAnkerMake => cmbExportFormatAnkerMake?.SelectedItem?.ToString() ?? string.Empty;
        public string ExportFormatPrusa => cmbExportFormatPrusa?.SelectedItem?.ToString() ?? string.Empty;
        public string ExportFormatSlic3r => cmbExportFormatSlic3r?.SelectedItem?.ToString() ?? string.Empty;
        public string ExportFormatOrca => cmbExportFormatOrca?.SelectedItem?.ToString() ?? string.Empty;
        public string ExportFormatQuickSave => cmbQuickSaveFileType?.SelectedItem?.ToString() ?? string.Empty;

        public SettingsDialog(
            ApplicationSettings.AddinSettings addInSettings,
            ApplicationSettings.CuraSettings curaSettings,
            ApplicationSettings.BambuSettings bambuSettings,
            ApplicationSettings.AnkerMakeSettings ankerMakeSettings,
            ApplicationSettings.PrusaSettings prusaSettings,
            ApplicationSettings.Slic3rSettings slic3rSettings,
            ApplicationSettings.OrcaSettings orcaSettings)
        {
            InitializeComponents();

            txtExportPath.Text = addInSettings?.ExportPath ?? string.Empty;
            chkQuietMode.Checked = addInSettings?.QuietMode ?? false;

            txtCuraPath.Text = curaSettings?.Path ?? string.Empty;
            cmbExportFormatCura.SelectedItem = curaSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;
            chkCuraEnabled.Checked = curaSettings?.Enabled ?? false;

            txtBambuLabPath.Text = bambuSettings?.Path ?? string.Empty;
            cmbExportFormatBambuLab.SelectedItem = bambuSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;
            chkBambuEnabled.Checked = bambuSettings?.Enabled ?? false;

            txtAnkerMakePath.Text = ankerMakeSettings?.Path ?? string.Empty;
            cmbExportFormatAnkerMake.SelectedItem = ankerMakeSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;
            chkAnkerMakeEnabled.Checked = ankerMakeSettings?.Enabled ?? false;

            txtPrusaPath.Text = prusaSettings?.Path ?? string.Empty;
            cmbExportFormatPrusa.SelectedItem = prusaSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;
            chkPrusaEnabled.Checked = prusaSettings?.Enabled ?? false;

            txtSlic3rPath.Text = slic3rSettings?.Path ?? string.Empty;
            cmbExportFormatSlic3r.SelectedItem = slic3rSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;
            chkSlic3rEnabled.Checked = slic3rSettings?.Enabled ?? false;

            txtOrcaPath.Text = orcaSettings?.Path ?? string.Empty;
            cmbExportFormatOrca.SelectedItem = orcaSettings?.FileType.ToString().TrimStart('_') ?? string.Empty;
            chkOrcaEnabled.Checked = orcaSettings?.Enabled ?? false;

            cmbQuickSaveFileType.SelectedItem = addInSettings?.QuickSaveType.ToString().TrimStart('_') ?? string.Empty;
        }

        private void InitializeComponents()
        {
            this.Text = "Settings";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AutoScaleMode = AutoScaleMode.Dpi;

            // Make it scrollable
            Panel mainPanel = new()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            this.Controls.Add(mainPanel);

            // Create a TableLayoutPanel
            TableLayoutPanel tableLayoutPanel = new()
            {
                ColumnCount = 3,
                RowCount = 0,
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(10)
            };

            // Add columns
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            // Add rows dynamically
            void AddRow(Control label, Control control, Control button = null)
            {
                tableLayoutPanel.RowCount++;
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tableLayoutPanel.Controls.Add(label, 0, tableLayoutPanel.RowCount - 1);
                tableLayoutPanel.Controls.Add(control, 1, tableLayoutPanel.RowCount - 1);
                if (button != null)
                {
                    tableLayoutPanel.Controls.Add(button, 2, tableLayoutPanel.RowCount - 1);
                }
            }

            // Ultimaker Cura
            AddRow(new Label { Text = "UltiMaker Cura", Font = new Font(Font, FontStyle.Bold), AutoSize = true }, new Label());
            chkCuraEnabled = new CheckBox { Text = "UltiMaker Cura Enabled", AutoSize = true };
            AddRow(chkCuraEnabled, new Control());
            cmbExportFormatCura = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 250 };
            AddRow(new Label { Text = "UltiMaker Cura Filetype:", AutoSize = true }, cmbExportFormatCura);
            txtCuraPath = new TextBox { Width = 250 };
            Button btnBrowseCuraPath = new() { Text = "Browse" };
            btnBrowseCuraPath.Click += (sender, e) =>
            {
                OpenFileDialog openFileDialog = new()
                {
                    Filter = "Executable files (*.exe)|*.exe"
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtCuraPath.Text = openFileDialog.FileName;
                }
            };
            AddRow(new Label { Text = "UltiMaker Cura .EXE Path:", AutoSize = true }, txtCuraPath, btnBrowseCuraPath);

            // Bambu Lab
            AddRow(new Label { Text = "Bambu Lab", Font = new Font(Font, FontStyle.Bold), AutoSize = true }, new Label());
            chkBambuEnabled = new CheckBox { Text = "Bambu Lab Enabled", AutoSize = true };
            AddRow(chkBambuEnabled, new Control());
            cmbExportFormatBambuLab = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 250 };
            AddRow(new Label { Text = "Bambu Lab Filetype:", AutoSize = true }, cmbExportFormatBambuLab);
            txtBambuLabPath = new TextBox { Width = 250 };
            Button btnBrowseBambuPath = new() { Text = "Browse" };
            btnBrowseBambuPath.Click += (sender, e) =>
            {
                OpenFileDialog openFileDialog = new()
                {
                    Filter = "Executable files (*.exe)|*.exe"
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtBambuLabPath.Text = openFileDialog.FileName;
                }
            };
            AddRow(new Label { Text = "Bambu Lab .EXE Path:", AutoSize = true }, txtBambuLabPath, btnBrowseBambuPath);

            // AnkerMake
            AddRow(new Label { Text = "AnkerMake Studio", Font = new Font(Font, FontStyle.Bold), AutoSize = true }, new Label());
            chkAnkerMakeEnabled = new CheckBox { Text = "AnkerMake Enabled", AutoSize = true };
            AddRow(chkAnkerMakeEnabled, new Control());
            cmbExportFormatAnkerMake = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 250 };
            AddRow(new Label { Text = "AnkerMake Filetype:", AutoSize = true }, cmbExportFormatAnkerMake);
            txtAnkerMakePath = new TextBox { Width = 250 };
            Button btnBrowseAnkerMakePath = new() { Text = "Browse" };
            btnBrowseAnkerMakePath.Click += (sender, e) =>
            {
                OpenFileDialog openFileDialog = new()
                {
                    Filter = "Executable files (*.exe)|*.exe"
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtAnkerMakePath.Text = openFileDialog.FileName;
                }
            };
            AddRow(new Label { Text = "AnkerMake .EXE Path:", AutoSize = true }, txtAnkerMakePath, btnBrowseAnkerMakePath);

            // Prusa
            AddRow(new Label { Text = "Prusa", Font = new Font(Font, FontStyle.Bold), AutoSize = true }, new Label());
            chkPrusaEnabled = new CheckBox { Text = "Prusa Enabled", AutoSize = true };
            AddRow(chkPrusaEnabled, new Control());
            cmbExportFormatPrusa = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 250 };
            AddRow(new Label { Text = "Prusa Filetype:", AutoSize = true }, cmbExportFormatPrusa);
            txtPrusaPath = new TextBox { Width = 250 };
            Button btnBrowsePrusaPath = new() { Text = "Browse" };
            btnBrowsePrusaPath.Click += (sender, e) =>
            {
                OpenFileDialog openFileDialog = new()
                {
                    Filter = "Executable files (*.exe)|*.exe"
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtPrusaPath.Text = openFileDialog.FileName;
                }
            };
            AddRow(new Label { Text = "Prusa .EXE Path:", AutoSize = true }, txtPrusaPath, btnBrowsePrusaPath);

            // Slic3r
            AddRow(new Label { Text = "Slic3r", Font = new Font(Font, FontStyle.Bold), AutoSize = true }, new Label());
            chkSlic3rEnabled = new CheckBox { Text = "Slic3r Enabled", AutoSize = true };
            AddRow(chkSlic3rEnabled, new Control());
            cmbExportFormatSlic3r = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 250 };
            AddRow(new Label { Text = "Slic3r Filetype:" }, cmbExportFormatSlic3r);
            txtSlic3rPath = new TextBox { Width = 250 };
            Button btnBrowseSlic3rPath = new() { Text = "Browse" };
            btnBrowseSlic3rPath.Click += (sender, e) =>
            {
                OpenFileDialog openFileDialog = new()
                {
                    Filter = "Executable files (*.exe)|*.exe"
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtSlic3rPath.Text = openFileDialog.FileName;
                }
            };
            AddRow(new Label { Text = "Slic3r .EXE Path:", AutoSize = true }, txtSlic3rPath, btnBrowseSlic3rPath);

            // Orca
            AddRow(new Label { Text = "Orca Slicer", Font = new Font(Font, FontStyle.Bold), AutoSize = true }, new Label());
            chkOrcaEnabled = new CheckBox { Text = "Orca Slicer Enabled", AutoSize = true };
            AddRow(chkOrcaEnabled, new Control());
            cmbExportFormatOrca = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 250 };
            AddRow(new Label { Text = "Orca Slicer Filetype:", AutoSize = true }, cmbExportFormatOrca);
            txtOrcaPath = new TextBox { Width = 250 };
            Button btnBrowseOrcaPath = new() { Text = "Browse" };
            btnBrowseOrcaPath.Click += (sender, e) =>
            {
                OpenFileDialog openFileDialog = new()
                {
                    Filter = "Executable files (*.exe)|*.exe"
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtOrcaPath.Text = openFileDialog.FileName;
                }
            };
            AddRow(new Label { Text = "Orca Slicer .EXE Path:", AutoSize = true }, txtOrcaPath, btnBrowseOrcaPath);

            // Add-in settings
            AddRow(new Label { Text = "Add-In Settings", Font = new Font(Font, FontStyle.Bold), AutoSize = true }, new Label());
            cmbQuickSaveFileType = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 250 };
            AddRow(new Label { Text = "QuickSave Filetype", AutoSize = true }, cmbQuickSaveFileType);
            txtExportPath = new TextBox { Width = 250 };
            Button btnBrowseExportPath = new() { Text = "Browse" };
            btnBrowseExportPath.Click += (sender, e) =>
            {
                FolderBrowserDialog folderBrowserDialog = new();
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    txtExportPath.Text = folderBrowserDialog.SelectedPath;
                }
            };
            AddRow(new Label { Text = "File Export Path:", AutoSize = true }, txtExportPath, btnBrowseExportPath);
            chkQuietMode = new CheckBox { Text = "Quiet Mode", AutoSize = true };
            AddRow(chkQuietMode, new Control());

            // Save button
            btnSave = new Button
            {
                Text = "Save",
                Dock = DockStyle.Fill,
                Margin = new Padding(20, 30, 20, 30)
            };
            tableLayoutPanel.Controls.Add(btnSave, 0, tableLayoutPanel.RowCount);
            tableLayoutPanel.SetColumnSpan(btnSave, 3);

            // Add the TableLayoutPanel to the form
            mainPanel.Controls.Add(tableLayoutPanel);

            // Set the size of the form
            this.MinimumSize = new Size(550, 650);
            this.Size = new Size(550, 850);
            this.ClientSize = new Size(this.ClientSize.Width, Math.Min(tableLayoutPanel.Height + 50, 850));

            // Populate ComboBoxes
            cmbExportFormatCura.Items.AddRange(new string[] { "AMF", "STL", "3MF", "STEP" });
            cmbExportFormatBambuLab.Items.AddRange(new string[] { "AMF", "STL", "STEP", "3MF" });
            cmbExportFormatAnkerMake.Items.AddRange(new string[] { "AMF", "STL", "3MF", "STEP" });
            cmbExportFormatPrusa.Items.AddRange(new string[] { "AMF", "STL", "3MF", "STEP" });
            cmbExportFormatSlic3r.Items.AddRange(new string[] { "AMF", "STL", "3MF", "STEP" });
            cmbExportFormatOrca.Items.AddRange(new string[] { "AMF", "STL", "3MF", "STEP" });

            cmbQuickSaveFileType.Items.AddRange(new string[] { "AMF", "STL", "STEP", "3MF", "SLDPRT", "PLY" });

            btnSave.Click += (sender, e) =>
            {
                FileType exportFormatCura = (!string.IsNullOrEmpty(this.ExportFormatCura)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatCura) : FileType._NONE;
                FileType exportFormatBambu = (!string.IsNullOrEmpty(this.ExportFormatBambuLab)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatBambuLab) : FileType._NONE;
                FileType exportFormatAnkerMake = (!string.IsNullOrEmpty(this.ExportFormatAnkerMake)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatAnkerMake) : FileType._NONE;
                FileType exportFormatPrusa = (!string.IsNullOrEmpty(this.ExportFormatPrusa)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatPrusa) : FileType._NONE;
                FileType exportFormatSlic3r = (!string.IsNullOrEmpty(this.ExportFormatSlic3r)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatSlic3r) : FileType._NONE;
                FileType exportFormatOrca = (!string.IsNullOrEmpty(this.ExportFormatOrca)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatOrca) : FileType._NONE;
                FileType quickSaveFileType = (!string.IsNullOrEmpty(this.ExportFormatQuickSave)) ? (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatQuickSave) : FileType._NONE;

                SaveSettings(
                    this.ExportPath,
                    this.CuraPath,
                    this.BambuLabPath,
                    this.AnkerMakePath,
                    this.PrusaPath,
                    this.Slic3rPath,
                    this.OrcaPath,
                    exportFormatCura,
                    exportFormatBambu,
                    exportFormatAnkerMake,
                    exportFormatPrusa,
                    exportFormatSlic3r,
                    exportFormatOrca,
                    quickSaveFileType,
                    chkCuraEnabled.Checked,
                    chkBambuEnabled.Checked,
                    chkAnkerMakeEnabled.Checked,
                    chkPrusaEnabled.Checked,
                    chkSlic3rEnabled.Checked,
                    chkOrcaEnabled.Checked,
                    chkQuietMode.Checked
                );
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            if (!btnSave.Visible || btnSave.Bottom > this.ClientSize.Height)
            {
                this.ClientSize = new Size(this.ClientSize.Width, btnSave.Bottom + 20);
            }
        }

        private void SaveSettings(
            string exportPath,
            string curaPath,
            string bambuPath,
            string ankerMakePath,
            string prusaPath,
            string slic3rPath,
            string orcaPath,
            FileType exportFormatCura,
            FileType exportFormatBambu,
            FileType exportFormatAnkerMake,
            FileType exportFormatPrusa,
            FileType exportFormatSlic3r,
            FileType exportFormatOrca,
            FileType quickSaveFileType,
            bool curaEnabled,
            bool bambuEnabled,
            bool ankerMakeEnabled,
            bool prusaEnabled,
            bool slic3rEnabled,
            bool orcaEnabled,
            bool quietMode
        )
        {
            var settings = new
            {
                ExportPath = exportPath ?? "",
                CuraPath = curaPath ?? "",
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
                OrcaPath = orcaPath ?? "",
                ExportFormatOrca = exportFormatOrca,
                OrcaEnabled = orcaEnabled,
                ExportFormatQuickSave = quickSaveFileType,
                QuietMode = quietMode
            };

            AddinSettings addinSettings = new();
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented, new StringEnumConverter());
            File.WriteAllText(addinSettings.DataPath, json);

            MessageBox.Show("Settings saved.");
            this.Close();
        }
    }
}