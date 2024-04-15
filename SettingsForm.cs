using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Drawing;
using System.Windows.Forms;
using static _3DPrint_SW.ApplicationSettings;

namespace _3DPrint_SW
{
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

        //public SettingsDialog(string curaPath, string exportPath, string exportFormatCura, string bambuPath, string exportFormatBambulab)
        public SettingsDialog(ApplicationSettings.ExportSettings exportSettings, ApplicationSettings.CuraSettings curaSettings, ApplicationSettings.BambuSettings bambuSettings)
        {
            InitializeComponents();

            txtCuraPath.Text = curaSettings.Path;
            txtExportPath.Text = exportSettings.Path;
            cmbExportFormatCura.SelectedItem = curaSettings.FileType.ToString();
            txtBambuLabPath.Text = bambuSettings.Path;
            cmbExportFormatBambuLab.Text = bambuSettings.FileType.ToString();
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

            btnSave.Click += (sender, e) => 
            {
                FileType exportFormatCura = (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatCura);
                FileType exportFormatBambu = (FileType)Enum.Parse(typeof(FileType), "_" + this.ExportFormatBambuLab);
                SaveSettings(this.CuraPath, this.ExportPath, exportFormatCura, exportFormatBambu, this.BambuLabPath);
            };
            
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
            Size = new System.Drawing.Size(450, 500);
        }


        private void SaveSettings(string curaPath, string exportPath, FileType exportFormatCura, FileType exportFormatBambu, string bambuPath)
        {
            var settings = new
            {
                CuraPath = curaPath,
                ExportPath = exportPath,
                ExportFormatCura = exportFormatCura,
                ExportFormatBambu = exportFormatBambu,
                BambuPath = bambuPath
            };

            string json = JsonConvert.SerializeObject(settings, Formatting.Indented, new StringEnumConverter());
            File.WriteAllText(settingsFilePath, json);

            MessageBox.Show("Settings saved.");
            this.Close();
        }
    }
}
