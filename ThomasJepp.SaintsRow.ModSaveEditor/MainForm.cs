using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ThomasJepp.SaintsRow.Saves.SaintsRowIVMod;

namespace ThomasJepp.SaintsRow.ModSaveEditor
{
    public partial class MainForm : Form
    {
        public string FilePath;
        public SaveFile SaveFile;

        public MainForm()
        {
            InitializeComponent();
        }

        private void OpenDialog_FileOk(object sender, CancelEventArgs e)
        {
            string filePath = OpenDialog.FileName;
            string backupFilePath = Path.ChangeExtension(filePath, ".bak");
            if (File.Exists(backupFilePath))
                File.Delete(backupFilePath);
            File.Copy(filePath, backupFilePath);

            using (Stream s = File.OpenRead(filePath))
            {
                SaveFile = new SaveFile(s);
                FilePath = filePath;
            }

            PlayerCashOnHandField.Value = SaveFile.Player.CashOnHand;
            PlayerOrbsField.Value = SaveFile.Player.Orbs;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            OpenDialog.ShowDialog();
        }

        private void LoadFromDiskButton_Click(object sender, EventArgs e)
        {
            OpenDialog.ShowDialog();
        }

        private void SaveToDiskButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
            using (Stream s = File.OpenWrite(FilePath))
            {
                SaveFile.Save(s);
                s.Flush();
            }
        }

        private void PlayerCashOnHandField_ValueChanged(object sender, EventArgs e)
        {
            SaveFile.Player.CashOnHand = PlayerCashOnHandField.Value;
        }

        private void PlayerOrbsField_ValueChanged(object sender, EventArgs e)
        {
            SaveFile.Player.Orbs = (int)PlayerOrbsField.Value;
        }
    }
}
