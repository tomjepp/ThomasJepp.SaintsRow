using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using ThomasJepp.SaintsRow.Packfiles;

namespace ThomasJepp.SaintsRow.ExtractPackfileGUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ExtractPackfileButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = OpenPackfileDialog.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                dr = ExtractToPath.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    BeginExtract(OpenPackfileDialog.FileName, ExtractToPath.SelectedPath);
                }
            }
        }

        internal class ExtractOptions
        {
            public string Source { get; set; }
            public string Destination { get; set; }

            public ExtractOptions(string source, string destination)
            {
                Source = source;
                Destination = destination;
            }
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
            ExtractPackfileButton.Enabled = false;
        }

        public void EnableButton()
        {
            this.Invoke(new SimpleDelegate(EnableButton_Internal));
        }

        private void EnableButton_Internal()
        {
            ExtractPackfileButton.Enabled = true;
        }
        #endregion

        private void BeginExtract(string source, string destination)
        {
            var options = new ExtractOptions(source, destination);

            ParameterizedThreadStart pts = new ParameterizedThreadStart(DoExtract);
            Thread t = new Thread(pts);
            t.Start(options);
        }

        private void DoExtract(object o)
        {
            DisableButton();
            ExtractOptions options = (ExtractOptions)o;

            SetProgressBarSettings(0, 100, 1, ProgressBarStyle.Marquee);
            SetText("Opening packfile...");

            using (Stream stream = File.OpenRead(options.Source))
            {
                var packfile = Packfile.FromStream(stream, Path.GetExtension(options.Source) == ".str2_pc");

                string filename = Path.GetFileName(options.Source);
                string outputDir = Path.Combine(options.Destination, filename);
                if (File.Exists(outputDir))
                {
                    outputDir = Path.Combine(options.Destination, "extracted-" + filename);
                }
                Directory.CreateDirectory(outputDir);                

                SetProgressBarSettings(0, packfile.Files.Count, 1, ProgressBarStyle.Continuous);
                SetText("Extracting {0}...", filename);

                foreach (IPackfileEntry entry in packfile.Files)
                {
                    using (Stream outputStream = File.OpenWrite(Path.Combine(outputDir, entry.Name)))
                    {
                        using (Stream inputStream = entry.GetStream())
                        {
                            inputStream.CopyTo(outputStream);
                        }
                        outputStream.Flush();
                    }

                    Step();
                }
            }

            SetText("Finished!");
            EnableButton();
        }
    }
}
