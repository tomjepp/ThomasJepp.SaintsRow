namespace ThomasJepp.SaintsRow.ModSaveEditor
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
            this.TabView = new System.Windows.Forms.TabControl();
            this.PlayerDataTabPage = new System.Windows.Forms.TabPage();
            this.LoadFromDiskButton = new System.Windows.Forms.Button();
            this.SaveToDiskButton = new System.Windows.Forms.Button();
            this.OpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.PlayerCashOnHandField = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PlayerOrbsField = new System.Windows.Forms.NumericUpDown();
            this.TabView.SuspendLayout();
            this.PlayerDataTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCashOnHandField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerOrbsField)).BeginInit();
            this.SuspendLayout();
            // 
            // TabView
            // 
            this.TabView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabView.Controls.Add(this.PlayerDataTabPage);
            this.TabView.Location = new System.Drawing.Point(13, 13);
            this.TabView.Name = "TabView";
            this.TabView.SelectedIndex = 0;
            this.TabView.Size = new System.Drawing.Size(476, 375);
            this.TabView.TabIndex = 0;
            // 
            // PlayerDataTabPage
            // 
            this.PlayerDataTabPage.Controls.Add(this.label2);
            this.PlayerDataTabPage.Controls.Add(this.PlayerOrbsField);
            this.PlayerDataTabPage.Controls.Add(this.label1);
            this.PlayerDataTabPage.Controls.Add(this.PlayerCashOnHandField);
            this.PlayerDataTabPage.Location = new System.Drawing.Point(4, 22);
            this.PlayerDataTabPage.Name = "PlayerDataTabPage";
            this.PlayerDataTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.PlayerDataTabPage.Size = new System.Drawing.Size(468, 349);
            this.PlayerDataTabPage.TabIndex = 0;
            this.PlayerDataTabPage.Text = "Player";
            this.PlayerDataTabPage.UseVisualStyleBackColor = true;
            // 
            // LoadFromDiskButton
            // 
            this.LoadFromDiskButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadFromDiskButton.Location = new System.Drawing.Point(333, 394);
            this.LoadFromDiskButton.Name = "LoadFromDiskButton";
            this.LoadFromDiskButton.Size = new System.Drawing.Size(75, 23);
            this.LoadFromDiskButton.TabIndex = 1;
            this.LoadFromDiskButton.Text = "Load";
            this.LoadFromDiskButton.UseVisualStyleBackColor = true;
            this.LoadFromDiskButton.Click += new System.EventHandler(this.LoadFromDiskButton_Click);
            // 
            // SaveToDiskButton
            // 
            this.SaveToDiskButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveToDiskButton.Location = new System.Drawing.Point(414, 394);
            this.SaveToDiskButton.Name = "SaveToDiskButton";
            this.SaveToDiskButton.Size = new System.Drawing.Size(75, 23);
            this.SaveToDiskButton.TabIndex = 2;
            this.SaveToDiskButton.Text = "Save";
            this.SaveToDiskButton.UseVisualStyleBackColor = true;
            this.SaveToDiskButton.Click += new System.EventHandler(this.SaveToDiskButton_Click);
            // 
            // OpenDialog
            // 
            this.OpenDialog.Filter = "Saints Row IV Save File|*.sr3s_pc";
            this.OpenDialog.InitialDirectory = "D:\\Steam\\userdata\\3512696\\206420\\remote";
            this.OpenDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenDialog_FileOk);
            // 
            // PlayerCashOnHandField
            // 
            this.PlayerCashOnHandField.DecimalPlaces = 2;
            this.PlayerCashOnHandField.Location = new System.Drawing.Point(90, 6);
            this.PlayerCashOnHandField.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            131072});
            this.PlayerCashOnHandField.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147352576});
            this.PlayerCashOnHandField.Name = "PlayerCashOnHandField";
            this.PlayerCashOnHandField.Size = new System.Drawing.Size(120, 20);
            this.PlayerCashOnHandField.TabIndex = 4;
            this.PlayerCashOnHandField.ThousandsSeparator = true;
            this.PlayerCashOnHandField.ValueChanged += new System.EventHandler(this.PlayerCashOnHandField_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Cash on Hand:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Orbs:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PlayerOrbsField
            // 
            this.PlayerOrbsField.Location = new System.Drawing.Point(90, 32);
            this.PlayerOrbsField.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            131072});
            this.PlayerOrbsField.Name = "PlayerOrbsField";
            this.PlayerOrbsField.Size = new System.Drawing.Size(120, 20);
            this.PlayerOrbsField.TabIndex = 6;
            this.PlayerOrbsField.ThousandsSeparator = true;
            this.PlayerOrbsField.ValueChanged += new System.EventHandler(this.PlayerOrbsField_ValueChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 429);
            this.Controls.Add(this.SaveToDiskButton);
            this.Controls.Add(this.LoadFromDiskButton);
            this.Controls.Add(this.TabView);
            this.Name = "MainForm";
            this.Text = "Saints Row IV Save Editor (Mod saves only)";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.TabView.ResumeLayout(false);
            this.PlayerDataTabPage.ResumeLayout(false);
            this.PlayerDataTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCashOnHandField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerOrbsField)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabView;
        private System.Windows.Forms.TabPage PlayerDataTabPage;
        private System.Windows.Forms.Button LoadFromDiskButton;
        private System.Windows.Forms.Button SaveToDiskButton;
        private System.Windows.Forms.OpenFileDialog OpenDialog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown PlayerOrbsField;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown PlayerCashOnHandField;
    }
}

