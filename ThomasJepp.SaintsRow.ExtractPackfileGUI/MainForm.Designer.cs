namespace ThomasJepp.SaintsRow.ExtractPackfileGUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ProgressIndicator = new System.Windows.Forms.ProgressBar();
            this.ExtractPackfileButton = new System.Windows.Forms.Button();
            this.ProgressMessage = new System.Windows.Forms.Label();
            this.OpenPackfileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ExtractToPath = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // ProgressIndicator
            // 
            this.ProgressIndicator.Location = new System.Drawing.Point(12, 12);
            this.ProgressIndicator.Name = "ProgressIndicator";
            this.ProgressIndicator.Size = new System.Drawing.Size(481, 23);
            this.ProgressIndicator.TabIndex = 0;
            // 
            // ExtractPackfileButton
            // 
            this.ExtractPackfileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ExtractPackfileButton.Location = new System.Drawing.Point(418, 41);
            this.ExtractPackfileButton.Name = "ExtractPackfileButton";
            this.ExtractPackfileButton.Size = new System.Drawing.Size(75, 23);
            this.ExtractPackfileButton.TabIndex = 1;
            this.ExtractPackfileButton.Text = "Extract";
            this.ExtractPackfileButton.UseVisualStyleBackColor = true;
            this.ExtractPackfileButton.Click += new System.EventHandler(this.ExtractPackfileButton_Click);
            // 
            // ProgressMessage
            // 
            this.ProgressMessage.BackColor = System.Drawing.Color.Transparent;
            this.ProgressMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ProgressMessage.Location = new System.Drawing.Point(12, 41);
            this.ProgressMessage.Name = "ProgressMessage";
            this.ProgressMessage.Size = new System.Drawing.Size(400, 23);
            this.ProgressMessage.TabIndex = 2;
            // 
            // OpenPackfileDialog
            // 
            this.OpenPackfileDialog.DefaultExt = "vpp_pc";
            this.OpenPackfileDialog.Filter = "Packfiles|*.vpp_pc;*.str2_pc|All files|*.*";
            this.OpenPackfileDialog.Title = "Select a packfile";
            // 
            // ExtractToPath
            // 
            this.ExtractToPath.Description = "Select a folder to extract this packfile to:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 72);
            this.Controls.Add(this.ProgressMessage);
            this.Controls.Add(this.ExtractPackfileButton);
            this.Controls.Add(this.ProgressIndicator);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Saints Row IV Packfile Extractor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar ProgressIndicator;
        private System.Windows.Forms.Button ExtractPackfileButton;
        private System.Windows.Forms.Label ProgressMessage;
        private System.Windows.Forms.OpenFileDialog OpenPackfileDialog;
        private System.Windows.Forms.FolderBrowserDialog ExtractToPath;
    }
}

