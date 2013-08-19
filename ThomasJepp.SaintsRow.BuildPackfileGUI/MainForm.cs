using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using ThomasJepp.SaintsRow.Packfiles;
using ThomasJepp.SaintsRow.Stream2;

namespace ThomasJepp.SaintsRow.BuildPackfileGUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        #region Thread-safe Update Functions
        private delegate void SimpleDelegate();

        private delegate void StringDelegate(string text);

        public void SetTitle(string title, params object[] parameters)
        {
            string text = String.Format(title, parameters);
            this.Invoke(new StringDelegate(SetTitle_Internal), text);
        }

        private void SetTitle_Internal(string title)
        {
            this.Text = title;
        }

        public void SetText(string text, params object[] parameters)
        {
            text = String.Format(text, parameters);
            this.Invoke(new StringDelegate(SetText_Internal), text);
        }

        private void SetText_Internal(string text)
        {
            ProgressMessage.Text = text;
        }

        public void SetProgressBarSettings(int minimum, int maximum, int step, ProgressBarStyle style)
        {
            this.Invoke(new SetProgressBarSettings_Delegate(SetProgressBarSettings_Internal), minimum, maximum, step, style);
        }

        private delegate void SetProgressBarSettings_Delegate(int minimum, int maximum, int step, ProgressBarStyle style);
        private void SetProgressBarSettings_Internal(int minimum, int maximum, int step, ProgressBarStyle style)
        {
            ProgressIndicator.Minimum = minimum;
            ProgressIndicator.Maximum = maximum;
            ProgressIndicator.Step = step;
            ProgressIndicator.Style = style;
            ProgressIndicator.Value = minimum;
        }

        public void Step()
        {
            this.Invoke(new SimpleDelegate(ProgressIndicator.PerformStep));
        }

        public void DisableButton()
        {
            this.Invoke(new SimpleDelegate(DisableButton_Internal));
        }

        private void DisableButton_Internal()
        {
            BuildPackfileButton.Enabled = false;
        }

        public void EnableButton()
        {
            this.Invoke(new SimpleDelegate(EnableButton_Internal));
        }

        private void EnableButton_Internal()
        {
            BuildPackfileButton.Enabled = true;
        }
        #endregion

        private void BuildPackfileButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = PackfileSaveDialog.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                BeginBuild(SourceFolderPath.Text, PackfileSaveDialog.FileName, UpdateASM.Checked ? AsmPath.Text : null);
            }
        }

        internal class BuildOptions
        {
            public string Source { get; set; }
            public string Asm { get; set; }
            public string Destination { get; set; }

            public BuildOptions(string source, string destination, string asm)
            {
                Source = source;
                Destination = destination;
                Asm = asm;
            }
        }

        private void BeginBuild(string source, string destination, string asm)
        {
            var options = new BuildOptions(source, destination, asm);

            ParameterizedThreadStart pts = new ParameterizedThreadStart(DoBuild);
            Thread t = new Thread(pts);
            t.Start(options);
        }

        private void DoBuild(object o)
        {
            DisableButton();
            BuildOptions options = (BuildOptions)o;

            SetProgressBarSettings(0, 100, 1, ProgressBarStyle.Marquee);

            IPackfile packfile = new Packfiles.Version0A.Packfile();
            Stream2File asm = null;
            Stream2.Container thisContainer = null;

            SetText("Setting up...");

            if (Path.GetExtension(options.Destination) == ".str2_pc")
            {
                packfile.IsCondensed = true;
                packfile.IsCompressed = true;

                if (options.Asm != null)
                {
                    using (Stream asmStream = File.OpenRead(options.Asm))
                    {
                        asm = new Stream2File(asmStream);
                    }
                }
            }

            if (asm != null)
            {
                string containerName = Path.GetFileNameWithoutExtension(options.Destination);

                foreach (var container in asm.Containers)
                {
                    string tempContainerName = Path.GetFileNameWithoutExtension(container.Name);
                    if (tempContainerName == containerName)
                    {
                        thisContainer = container;
                        break;
                    }
                }

                if (thisContainer == null)
                {
                    SetText("Couldn't find a container called {0} in the selected asm_pc file!", containerName);
                    SetProgressBarSettings(0, 100, 0, ProgressBarStyle.Continuous);
                    EnableButton();
                    return;
                }

                SetProgressBarSettings(0, thisContainer.PrimitiveCount, 0, ProgressBarStyle.Continuous);
                SetText("Adding files...");

                foreach (Primitive primitive in thisContainer.Primitives)
                {
                    string primitiveFile = Path.Combine(options.Source, primitive.Name);
                    if (!File.Exists(primitiveFile))
                    {
                        SetText("Couldn't find a container called {0} in the selected asm_pc file!", containerName);
                        SetProgressBarSettings(0, 100, 0, ProgressBarStyle.Continuous);
                        EnableButton();
                        return;
                    }

                    string filename = Path.GetFileName(primitiveFile);

                    Stream stream = File.OpenRead(primitiveFile);
                    packfile.AddFile(stream, filename);

                    string extension = Path.GetExtension(primitiveFile);
                    string gpuExtension = "";
                    switch (extension)
                    {
                        default:
                            {
                                if (extension.StartsWith(".c"))
                                    gpuExtension = ".g" + extension.Remove(0, 2);
                                break;
                            }
                    }


                    string gpuFile = Path.ChangeExtension(primitiveFile, gpuExtension);
                    if (File.Exists(gpuFile))
                    {
                        string gpuFilename = Path.GetFileName(gpuFile);
                        Stream gpuStream = File.OpenRead(gpuFile);
                        packfile.AddFile(gpuStream, gpuFilename);
                    }

                    Step();
                }
            }
            else
            {
                string[] files = Directory.GetFiles(options.Source);

                SetProgressBarSettings(0, files.Length, 0, ProgressBarStyle.Continuous);
                SetText("Adding files...");

                foreach (string file in files)
                {
                    string filename = Path.GetFileName(file);

                    Stream stream = File.OpenRead(file);
                    packfile.AddFile(stream, filename);
                }
            }

            SetProgressBarSettings(0, 100, 0, ProgressBarStyle.Marquee);

            SetText("Writing output file: {0}", options.Destination);
            using (Stream output = File.Open(options.Destination, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                packfile.Save(output);
            }

            if (asm != null)
            {
                SetText("Updating asm_pc file: {0}", options.Asm);
                packfile.Update(thisContainer);

                using (Stream asmStream = File.OpenWrite(options.Asm))
                {
                    asm.Save(asmStream);
                }
                Console.WriteLine("done.");
            }

            SetProgressBarSettings(0, 1, 1, ProgressBarStyle.Continuous);
            Step();
            SetText("Finished!");
            EnableButton();
        }

        private void UpdateASM_CheckedChanged(object sender, EventArgs e)
        {
            AsmPath.Enabled = UpdateASM.Checked;
            AsmBrowse.Enabled = UpdateASM.Checked;
        }

        private void SourceBrowse_Click(object sender, EventArgs e)
        {
            DialogResult dr = SourceBrowser.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                SourceFolderPath.Text = SourceBrowser.SelectedPath;
            }
        }

        private void AsmBrowse_Click(object sender, EventArgs e)
        {
            DialogResult dr = AsmFilePicker.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                AsmPath.Text = AsmFilePicker.FileName;
            }
        }
    }
}
