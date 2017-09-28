namespace Omniscient
{
    partial class SiteManagerForm
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
            this.SitesTreeView = new System.Windows.Forms.TreeView();
            this.SaveButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.InstTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PrefixTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DirectoryTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.NewInstrumentButton = new System.Windows.Forms.Button();
            this.NewSystemButton = new System.Windows.Forms.Button();
            this.NewFacilityButton = new System.Windows.Forms.Button();
            this.NewSiteButton = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ExitButton = new System.Windows.Forms.Button();
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ExportButton = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.DiscardButton = new System.Windows.Forms.Button();
            this.DirectoryButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.LeftPanel.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // SitesTreeView
            // 
            this.SitesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SitesTreeView.Location = new System.Drawing.Point(10, 10);
            this.SitesTreeView.Name = "SitesTreeView";
            this.SitesTreeView.Size = new System.Drawing.Size(200, 453);
            this.SitesTreeView.TabIndex = 0;
            this.SitesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SitesTreeView_AfterSelect);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(197, 178);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(66, 23);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(99, 31);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(160, 20);
            this.NameTextBox.TabIndex = 3;
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Location = new System.Drawing.Point(96, 15);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(0, 13);
            this.TypeLabel.TabIndex = 4;
            // 
            // InstTypeComboBox
            // 
            this.InstTypeComboBox.FormattingEnabled = true;
            this.InstTypeComboBox.Items.AddRange(new object[] {
            "ISR",
            "GRAND",
            "MCA"});
            this.InstTypeComboBox.Location = new System.Drawing.Point(99, 58);
            this.InstTypeComboBox.Name = "InstTypeComboBox";
            this.InstTypeComboBox.Size = new System.Drawing.Size(160, 21);
            this.InstTypeComboBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Instrument Type";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.DirectoryButton);
            this.panel1.Controls.Add(this.DiscardButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.RemoveButton);
            this.panel1.Controls.Add(this.PrefixTextBox);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.DirectoryTextBox);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.NameTextBox);
            this.panel1.Controls.Add(this.SaveButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.TypeLabel);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.InstTypeComboBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(5, 167);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(266, 204);
            this.panel1.TabIndex = 9;
            // 
            // PrefixTextBox
            // 
            this.PrefixTextBox.Location = new System.Drawing.Point(99, 85);
            this.PrefixTextBox.Name = "PrefixTextBox";
            this.PrefixTextBox.Size = new System.Drawing.Size(160, 20);
            this.PrefixTextBox.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "File Prefix";
            // 
            // DirectoryTextBox
            // 
            this.DirectoryTextBox.Location = new System.Drawing.Point(99, 111);
            this.DirectoryTextBox.Name = "DirectoryTextBox";
            this.DirectoryTextBox.Size = new System.Drawing.Size(160, 20);
            this.DirectoryTextBox.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Data Directory";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.NewInstrumentButton);
            this.panel2.Controls.Add(this.NewSystemButton);
            this.panel2.Controls.Add(this.NewFacilityButton);
            this.panel2.Controls.Add(this.NewSiteButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(5, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(266, 129);
            this.panel2.TabIndex = 10;
            // 
            // NewInstrumentButton
            // 
            this.NewInstrumentButton.Location = new System.Drawing.Point(158, 93);
            this.NewInstrumentButton.Name = "NewInstrumentButton";
            this.NewInstrumentButton.Size = new System.Drawing.Size(100, 23);
            this.NewInstrumentButton.TabIndex = 3;
            this.NewInstrumentButton.Text = "New Instrument";
            this.NewInstrumentButton.UseVisualStyleBackColor = true;
            // 
            // NewSystemButton
            // 
            this.NewSystemButton.Location = new System.Drawing.Point(158, 64);
            this.NewSystemButton.Name = "NewSystemButton";
            this.NewSystemButton.Size = new System.Drawing.Size(100, 23);
            this.NewSystemButton.TabIndex = 2;
            this.NewSystemButton.Text = "New System";
            this.NewSystemButton.UseVisualStyleBackColor = true;
            // 
            // NewFacilityButton
            // 
            this.NewFacilityButton.Location = new System.Drawing.Point(158, 35);
            this.NewFacilityButton.Name = "NewFacilityButton";
            this.NewFacilityButton.Size = new System.Drawing.Size(100, 23);
            this.NewFacilityButton.TabIndex = 1;
            this.NewFacilityButton.Text = "New Facility";
            this.NewFacilityButton.UseVisualStyleBackColor = true;
            // 
            // NewSiteButton
            // 
            this.NewSiteButton.Location = new System.Drawing.Point(158, 6);
            this.NewSiteButton.Name = "NewSiteButton";
            this.NewSiteButton.Size = new System.Drawing.Size(100, 23);
            this.NewSiteButton.TabIndex = 0;
            this.NewSiteButton.Text = "New Site";
            this.NewSiteButton.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ExitButton);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(5, 439);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(266, 29);
            this.panel3.TabIndex = 11;
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(197, 3);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(66, 23);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // LeftPanel
            // 
            this.LeftPanel.Controls.Add(this.SitesTreeView);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Padding = new System.Windows.Forms.Padding(10);
            this.LeftPanel.Size = new System.Drawing.Size(220, 473);
            this.LeftPanel.TabIndex = 12;
            // 
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.panel1);
            this.RightPanel.Controls.Add(this.panel3);
            this.RightPanel.Controls.Add(this.panel2);
            this.RightPanel.Controls.Add(this.panel4);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightPanel.Location = new System.Drawing.Point(220, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Padding = new System.Windows.Forms.Padding(5);
            this.RightPanel.Size = new System.Drawing.Size(276, 473);
            this.RightPanel.TabIndex = 13;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.ExportButton);
            this.panel4.Controls.Add(this.ImportButton);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(5, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(266, 33);
            this.panel4.TabIndex = 12;
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(72, 3);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(66, 23);
            this.ExportButton.TabIndex = 1;
            this.ExportButton.Text = "Export";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.Location = new System.Drawing.Point(3, 3);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(66, 23);
            this.ImportButton.TabIndex = 0;
            this.ImportButton.Text = "Import";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(3, 178);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(66, 23);
            this.RemoveButton.TabIndex = 13;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Type";
            // 
            // DiscardButton
            // 
            this.DiscardButton.Location = new System.Drawing.Point(93, 178);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(98, 23);
            this.DiscardButton.TabIndex = 15;
            this.DiscardButton.Text = "Discard Changes";
            this.DiscardButton.UseVisualStyleBackColor = true;
            this.DiscardButton.Click += new System.EventHandler(this.DiscardButton_Click);
            // 
            // DirectoryButton
            // 
            this.DirectoryButton.Location = new System.Drawing.Point(158, 137);
            this.DirectoryButton.Name = "DirectoryButton";
            this.DirectoryButton.Size = new System.Drawing.Size(100, 23);
            this.DirectoryButton.TabIndex = 16;
            this.DirectoryButton.Text = "Select Directory";
            this.DirectoryButton.UseVisualStyleBackColor = true;
            // 
            // SiteManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 473);
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.LeftPanel);
            this.MinimumSize = new System.Drawing.Size(512, 512);
            this.Name = "SiteManagerForm";
            this.Text = "SiteManagerForm";
            this.Load += new System.EventHandler(this.SiteManagerForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.LeftPanel.ResumeLayout(false);
            this.RightPanel.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView SitesTreeView;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.ComboBox InstTypeComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox PrefixTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox DirectoryTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button NewInstrumentButton;
        private System.Windows.Forms.Button NewSystemButton;
        private System.Windows.Forms.Button NewFacilityButton;
        private System.Windows.Forms.Button NewSiteButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DiscardButton;
        private System.Windows.Forms.Button DirectoryButton;
    }
}