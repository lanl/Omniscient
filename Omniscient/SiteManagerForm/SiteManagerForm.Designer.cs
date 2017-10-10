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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SiteManagerForm));
            this.SitesTreeView = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.InstTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.InstrumentPanel = new System.Windows.Forms.Panel();
            this.VirtualChannelGroupBox = new System.Windows.Forms.GroupBox();
            this.VCChannelConstPanel = new System.Windows.Forms.Panel();
            this.VirtualChannelConstantTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.VirtualChannelChannelComboBox = new System.Windows.Forms.ComboBox();
            this.VCDelayPanel = new System.Windows.Forms.Panel();
            this.DelayComboBox = new System.Windows.Forms.ComboBox();
            this.DelayTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.DelayChannelComboBox = new System.Windows.Forms.ComboBox();
            this.VCTwoChannelPanel = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.VirtualChannelChanBComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.VirtualChannelChanAComboBox = new System.Windows.Forms.ComboBox();
            this.VCTopPanel = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.VirtualChannelTypeComboBox = new System.Windows.Forms.ComboBox();
            this.VirtualChannelNameTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.RemoveVirtualChannelButton = new System.Windows.Forms.Button();
            this.AddVirtualChannelButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.VirtualChannelsComboBox = new System.Windows.Forms.ComboBox();
            this.DirectoryButton = new System.Windows.Forms.Button();
            this.PrefixTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DirectoryTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NewButtonPanel = new System.Windows.Forms.Panel();
            this.NewInstrumentButton = new System.Windows.Forms.Button();
            this.NewSystemButton = new System.Windows.Forms.Button();
            this.NewFacilityButton = new System.Windows.Forms.Button();
            this.NewSiteButton = new System.Windows.Forms.Button();
            this.BottomRightPanel = new System.Windows.Forms.Panel();
            this.DiscardButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.InnerRightPanel = new System.Windows.Forms.Panel();
            this.NamePanel = new System.Windows.Forms.Panel();
            this.LeftRightPanel = new System.Windows.Forms.Panel();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.UpButton = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ExportButton = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.InstrumentPanel.SuspendLayout();
            this.VirtualChannelGroupBox.SuspendLayout();
            this.VCChannelConstPanel.SuspendLayout();
            this.VCDelayPanel.SuspendLayout();
            this.VCTwoChannelPanel.SuspendLayout();
            this.VCTopPanel.SuspendLayout();
            this.NewButtonPanel.SuspendLayout();
            this.BottomRightPanel.SuspendLayout();
            this.LeftPanel.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.InnerRightPanel.SuspendLayout();
            this.NamePanel.SuspendLayout();
            this.LeftRightPanel.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // SitesTreeView
            // 
            this.SitesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SitesTreeView.Location = new System.Drawing.Point(13, 12);
            this.SitesTreeView.Margin = new System.Windows.Forms.Padding(4);
            this.SitesTreeView.Name = "SitesTreeView";
            this.SitesTreeView.ShowNodeToolTips = true;
            this.SitesTreeView.Size = new System.Drawing.Size(507, 765);
            this.SitesTreeView.TabIndex = 0;
            this.SitesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SitesTreeView_AfterSelect);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(81, 41);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(136, 36);
            this.NameTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(212, 22);
            this.NameTextBox.TabIndex = 3;
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Location = new System.Drawing.Point(132, 16);
            this.TypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(0, 17);
            this.TypeLabel.TabIndex = 4;
            // 
            // InstTypeComboBox
            // 
            this.InstTypeComboBox.FormattingEnabled = true;
            this.InstTypeComboBox.Items.AddRange(new object[] {
            "ISR",
            "GRAND",
            "MCA"});
            this.InstTypeComboBox.Location = new System.Drawing.Point(136, 10);
            this.InstTypeComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.InstTypeComboBox.Name = "InstTypeComboBox";
            this.InstTypeComboBox.Size = new System.Drawing.Size(212, 24);
            this.InstTypeComboBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 9);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Instrument Type";
            // 
            // InstrumentPanel
            // 
            this.InstrumentPanel.Controls.Add(this.VirtualChannelGroupBox);
            this.InstrumentPanel.Controls.Add(this.RemoveVirtualChannelButton);
            this.InstrumentPanel.Controls.Add(this.AddVirtualChannelButton);
            this.InstrumentPanel.Controls.Add(this.label6);
            this.InstrumentPanel.Controls.Add(this.VirtualChannelsComboBox);
            this.InstrumentPanel.Controls.Add(this.DirectoryButton);
            this.InstrumentPanel.Controls.Add(this.PrefixTextBox);
            this.InstrumentPanel.Controls.Add(this.label4);
            this.InstrumentPanel.Controls.Add(this.DirectoryTextBox);
            this.InstrumentPanel.Controls.Add(this.label5);
            this.InstrumentPanel.Controls.Add(this.label3);
            this.InstrumentPanel.Controls.Add(this.InstTypeComboBox);
            this.InstrumentPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.InstrumentPanel.Location = new System.Drawing.Point(0, 155);
            this.InstrumentPanel.Margin = new System.Windows.Forms.Padding(4);
            this.InstrumentPanel.Name = "InstrumentPanel";
            this.InstrumentPanel.Size = new System.Drawing.Size(446, 453);
            this.InstrumentPanel.TabIndex = 9;
            // 
            // VirtualChannelGroupBox
            // 
            this.VirtualChannelGroupBox.Controls.Add(this.VCChannelConstPanel);
            this.VirtualChannelGroupBox.Controls.Add(this.VCDelayPanel);
            this.VirtualChannelGroupBox.Controls.Add(this.VCTwoChannelPanel);
            this.VirtualChannelGroupBox.Controls.Add(this.VCTopPanel);
            this.VirtualChannelGroupBox.Location = new System.Drawing.Point(4, 150);
            this.VirtualChannelGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.VirtualChannelGroupBox.Name = "VirtualChannelGroupBox";
            this.VirtualChannelGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.VirtualChannelGroupBox.Size = new System.Drawing.Size(437, 283);
            this.VirtualChannelGroupBox.TabIndex = 21;
            this.VirtualChannelGroupBox.TabStop = false;
            this.VirtualChannelGroupBox.Text = "Virtual Channel";
            // 
            // VCChannelConstPanel
            // 
            this.VCChannelConstPanel.Controls.Add(this.VirtualChannelConstantTextBox);
            this.VCChannelConstPanel.Controls.Add(this.label11);
            this.VCChannelConstPanel.Controls.Add(this.label12);
            this.VCChannelConstPanel.Controls.Add(this.VirtualChannelChannelComboBox);
            this.VCChannelConstPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.VCChannelConstPanel.Location = new System.Drawing.Point(4, 237);
            this.VCChannelConstPanel.Margin = new System.Windows.Forms.Padding(4);
            this.VCChannelConstPanel.Name = "VCChannelConstPanel";
            this.VCChannelConstPanel.Size = new System.Drawing.Size(429, 71);
            this.VCChannelConstPanel.TabIndex = 8;
            this.VCChannelConstPanel.Visible = false;
            // 
            // VirtualChannelConstantTextBox
            // 
            this.VirtualChannelConstantTextBox.Location = new System.Drawing.Point(125, 41);
            this.VirtualChannelConstantTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.VirtualChannelConstantTextBox.Name = "VirtualChannelConstantTextBox";
            this.VirtualChannelConstantTextBox.Size = new System.Drawing.Size(212, 22);
            this.VirtualChannelConstantTextBox.TabIndex = 23;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(49, 44);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 17);
            this.label11.TabIndex = 22;
            this.label11.Text = "Constant";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(53, 11);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 17);
            this.label12.TabIndex = 20;
            this.label12.Text = "Channel";
            // 
            // VirtualChannelChannelComboBox
            // 
            this.VirtualChannelChannelComboBox.FormattingEnabled = true;
            this.VirtualChannelChannelComboBox.Location = new System.Drawing.Point(127, 7);
            this.VirtualChannelChannelComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.VirtualChannelChannelComboBox.Name = "VirtualChannelChannelComboBox";
            this.VirtualChannelChannelComboBox.Size = new System.Drawing.Size(211, 24);
            this.VirtualChannelChannelComboBox.TabIndex = 19;
            // 
            // VCDelayPanel
            // 
            this.VCDelayPanel.Controls.Add(this.DelayComboBox);
            this.VCDelayPanel.Controls.Add(this.DelayTextBox);
            this.VCDelayPanel.Controls.Add(this.label13);
            this.VCDelayPanel.Controls.Add(this.label14);
            this.VCDelayPanel.Controls.Add(this.DelayChannelComboBox);
            this.VCDelayPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.VCDelayPanel.Location = new System.Drawing.Point(4, 166);
            this.VCDelayPanel.Margin = new System.Windows.Forms.Padding(4);
            this.VCDelayPanel.Name = "VCDelayPanel";
            this.VCDelayPanel.Size = new System.Drawing.Size(429, 71);
            this.VCDelayPanel.TabIndex = 9;
            this.VCDelayPanel.Visible = false;
            // 
            // DelayComboBox
            // 
            this.DelayComboBox.FormattingEnabled = true;
            this.DelayComboBox.Items.AddRange(new object[] {
            "Seconds",
            "Minutes",
            "Hours",
            "Days"});
            this.DelayComboBox.Location = new System.Drawing.Point(204, 38);
            this.DelayComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.DelayComboBox.Name = "DelayComboBox";
            this.DelayComboBox.Size = new System.Drawing.Size(151, 24);
            this.DelayComboBox.TabIndex = 33;
            // 
            // DelayTextBox
            // 
            this.DelayTextBox.Location = new System.Drawing.Point(127, 38);
            this.DelayTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.DelayTextBox.Name = "DelayTextBox";
            this.DelayTextBox.Size = new System.Drawing.Size(68, 22);
            this.DelayTextBox.TabIndex = 32;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(49, 44);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 17);
            this.label13.TabIndex = 22;
            this.label13.Text = "Delay";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(53, 11);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 17);
            this.label14.TabIndex = 20;
            this.label14.Text = "Channel";
            // 
            // DelayChannelComboBox
            // 
            this.DelayChannelComboBox.FormattingEnabled = true;
            this.DelayChannelComboBox.Location = new System.Drawing.Point(127, 7);
            this.DelayChannelComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.DelayChannelComboBox.Name = "DelayChannelComboBox";
            this.DelayChannelComboBox.Size = new System.Drawing.Size(211, 24);
            this.DelayChannelComboBox.TabIndex = 19;
            // 
            // VCTwoChannelPanel
            // 
            this.VCTwoChannelPanel.Controls.Add(this.label10);
            this.VCTwoChannelPanel.Controls.Add(this.VirtualChannelChanBComboBox);
            this.VCTwoChannelPanel.Controls.Add(this.label9);
            this.VCTwoChannelPanel.Controls.Add(this.VirtualChannelChanAComboBox);
            this.VCTwoChannelPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.VCTwoChannelPanel.Location = new System.Drawing.Point(4, 90);
            this.VCTwoChannelPanel.Margin = new System.Windows.Forms.Padding(4);
            this.VCTwoChannelPanel.Name = "VCTwoChannelPanel";
            this.VCTwoChannelPanel.Size = new System.Drawing.Size(429, 76);
            this.VCTwoChannelPanel.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(40, 44);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 17);
            this.label10.TabIndex = 22;
            this.label10.Text = "Channel B";
            // 
            // VirtualChannelChanBComboBox
            // 
            this.VirtualChannelChanBComboBox.FormattingEnabled = true;
            this.VirtualChannelChanBComboBox.Location = new System.Drawing.Point(127, 41);
            this.VirtualChannelChanBComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.VirtualChannelChanBComboBox.Name = "VirtualChannelChanBComboBox";
            this.VirtualChannelChanBComboBox.Size = new System.Drawing.Size(211, 24);
            this.VirtualChannelChanBComboBox.TabIndex = 21;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(40, 11);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 17);
            this.label9.TabIndex = 20;
            this.label9.Text = "Channel A";
            // 
            // VirtualChannelChanAComboBox
            // 
            this.VirtualChannelChanAComboBox.FormattingEnabled = true;
            this.VirtualChannelChanAComboBox.Location = new System.Drawing.Point(127, 7);
            this.VirtualChannelChanAComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.VirtualChannelChanAComboBox.Name = "VirtualChannelChanAComboBox";
            this.VirtualChannelChanAComboBox.Size = new System.Drawing.Size(211, 24);
            this.VirtualChannelChanAComboBox.TabIndex = 19;
            // 
            // VCTopPanel
            // 
            this.VCTopPanel.Controls.Add(this.label8);
            this.VCTopPanel.Controls.Add(this.VirtualChannelTypeComboBox);
            this.VCTopPanel.Controls.Add(this.VirtualChannelNameTextBox);
            this.VCTopPanel.Controls.Add(this.label7);
            this.VCTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.VCTopPanel.Location = new System.Drawing.Point(4, 19);
            this.VCTopPanel.Margin = new System.Windows.Forms.Padding(4);
            this.VCTopPanel.Name = "VCTopPanel";
            this.VCTopPanel.Size = new System.Drawing.Size(429, 71);
            this.VCTopPanel.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(73, 39);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 17);
            this.label8.TabIndex = 20;
            this.label8.Text = "Type";
            // 
            // VirtualChannelTypeComboBox
            // 
            this.VirtualChannelTypeComboBox.FormattingEnabled = true;
            this.VirtualChannelTypeComboBox.Items.AddRange(new object[] {
            "Ratio",
            "Sum",
            "Difference",
            "Add Constant",
            "Scale",
            "Delay"});
            this.VirtualChannelTypeComboBox.Location = new System.Drawing.Point(128, 36);
            this.VirtualChannelTypeComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.VirtualChannelTypeComboBox.Name = "VirtualChannelTypeComboBox";
            this.VirtualChannelTypeComboBox.Size = new System.Drawing.Size(211, 24);
            this.VirtualChannelTypeComboBox.TabIndex = 19;
            this.VirtualChannelTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.VirtualChannelTypeComboBox_SelectedIndexChanged);
            // 
            // VirtualChannelNameTextBox
            // 
            this.VirtualChannelNameTextBox.Location = new System.Drawing.Point(127, 4);
            this.VirtualChannelNameTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.VirtualChannelNameTextBox.Name = "VirtualChannelNameTextBox";
            this.VirtualChannelNameTextBox.Size = new System.Drawing.Size(212, 22);
            this.VirtualChannelNameTextBox.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(72, 9);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 17);
            this.label7.TabIndex = 4;
            this.label7.Text = "Name";
            // 
            // RemoveVirtualChannelButton
            // 
            this.RemoveVirtualChannelButton.Image = ((System.Drawing.Image)(resources.GetObject("RemoveVirtualChannelButton.Image")));
            this.RemoveVirtualChannelButton.Location = new System.Drawing.Point(401, 108);
            this.RemoveVirtualChannelButton.Margin = new System.Windows.Forms.Padding(4);
            this.RemoveVirtualChannelButton.Name = "RemoveVirtualChannelButton";
            this.RemoveVirtualChannelButton.Size = new System.Drawing.Size(37, 34);
            this.RemoveVirtualChannelButton.TabIndex = 20;
            this.RemoveVirtualChannelButton.UseVisualStyleBackColor = true;
            // 
            // AddVirtualChannelButton
            // 
            this.AddVirtualChannelButton.Image = ((System.Drawing.Image)(resources.GetObject("AddVirtualChannelButton.Image")));
            this.AddVirtualChannelButton.Location = new System.Drawing.Point(356, 108);
            this.AddVirtualChannelButton.Margin = new System.Windows.Forms.Padding(4);
            this.AddVirtualChannelButton.Name = "AddVirtualChannelButton";
            this.AddVirtualChannelButton.Size = new System.Drawing.Size(37, 34);
            this.AddVirtualChannelButton.TabIndex = 19;
            this.AddVirtualChannelButton.UseVisualStyleBackColor = true;
            this.AddVirtualChannelButton.Click += new System.EventHandler(this.AddVirtualChannelButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 118);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 17);
            this.label6.TabIndex = 18;
            this.label6.Text = "Virtual Channels";
            // 
            // VirtualChannelsComboBox
            // 
            this.VirtualChannelsComboBox.FormattingEnabled = true;
            this.VirtualChannelsComboBox.Location = new System.Drawing.Point(136, 114);
            this.VirtualChannelsComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.VirtualChannelsComboBox.Name = "VirtualChannelsComboBox";
            this.VirtualChannelsComboBox.Size = new System.Drawing.Size(211, 24);
            this.VirtualChannelsComboBox.TabIndex = 17;
            this.VirtualChannelsComboBox.SelectedIndexChanged += new System.EventHandler(this.VirtualChannelsComboBox_SelectedIndexChanged);
            // 
            // DirectoryButton
            // 
            this.DirectoryButton.Image = ((System.Drawing.Image)(resources.GetObject("DirectoryButton.Image")));
            this.DirectoryButton.Location = new System.Drawing.Point(356, 73);
            this.DirectoryButton.Margin = new System.Windows.Forms.Padding(4);
            this.DirectoryButton.Name = "DirectoryButton";
            this.DirectoryButton.Size = new System.Drawing.Size(37, 34);
            this.DirectoryButton.TabIndex = 16;
            this.DirectoryButton.UseVisualStyleBackColor = true;
            this.DirectoryButton.Click += new System.EventHandler(this.DirectoryButton_Click);
            // 
            // PrefixTextBox
            // 
            this.PrefixTextBox.Location = new System.Drawing.Point(136, 43);
            this.PrefixTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.PrefixTextBox.Name = "PrefixTextBox";
            this.PrefixTextBox.Size = new System.Drawing.Size(212, 22);
            this.PrefixTextBox.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 42);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "File Prefix";
            // 
            // DirectoryTextBox
            // 
            this.DirectoryTextBox.Location = new System.Drawing.Point(136, 78);
            this.DirectoryTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.DirectoryTextBox.Name = "DirectoryTextBox";
            this.DirectoryTextBox.Size = new System.Drawing.Size(212, 22);
            this.DirectoryTextBox.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 82);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Data Directory";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(81, 16);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "Type";
            // 
            // NewButtonPanel
            // 
            this.NewButtonPanel.Controls.Add(this.NewInstrumentButton);
            this.NewButtonPanel.Controls.Add(this.NewSystemButton);
            this.NewButtonPanel.Controls.Add(this.NewFacilityButton);
            this.NewButtonPanel.Controls.Add(this.NewSiteButton);
            this.NewButtonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.NewButtonPanel.Location = new System.Drawing.Point(0, 0);
            this.NewButtonPanel.Margin = new System.Windows.Forms.Padding(4);
            this.NewButtonPanel.Name = "NewButtonPanel";
            this.NewButtonPanel.Size = new System.Drawing.Size(446, 87);
            this.NewButtonPanel.TabIndex = 10;
            // 
            // NewInstrumentButton
            // 
            this.NewInstrumentButton.Location = new System.Drawing.Point(193, 47);
            this.NewInstrumentButton.Margin = new System.Windows.Forms.Padding(4);
            this.NewInstrumentButton.Name = "NewInstrumentButton";
            this.NewInstrumentButton.Size = new System.Drawing.Size(133, 28);
            this.NewInstrumentButton.TabIndex = 3;
            this.NewInstrumentButton.Text = "New Instrument";
            this.NewInstrumentButton.UseVisualStyleBackColor = true;
            this.NewInstrumentButton.Click += new System.EventHandler(this.NewInstrumentButton_Click);
            // 
            // NewSystemButton
            // 
            this.NewSystemButton.Location = new System.Drawing.Point(193, 12);
            this.NewSystemButton.Margin = new System.Windows.Forms.Padding(4);
            this.NewSystemButton.Name = "NewSystemButton";
            this.NewSystemButton.Size = new System.Drawing.Size(133, 28);
            this.NewSystemButton.TabIndex = 2;
            this.NewSystemButton.Text = "New System";
            this.NewSystemButton.UseVisualStyleBackColor = true;
            this.NewSystemButton.Click += new System.EventHandler(this.NewSystemButton_Click);
            // 
            // NewFacilityButton
            // 
            this.NewFacilityButton.Location = new System.Drawing.Point(13, 47);
            this.NewFacilityButton.Margin = new System.Windows.Forms.Padding(4);
            this.NewFacilityButton.Name = "NewFacilityButton";
            this.NewFacilityButton.Size = new System.Drawing.Size(133, 28);
            this.NewFacilityButton.TabIndex = 1;
            this.NewFacilityButton.Text = "New Facility";
            this.NewFacilityButton.UseVisualStyleBackColor = true;
            this.NewFacilityButton.Click += new System.EventHandler(this.NewFacilityButton_Click);
            // 
            // NewSiteButton
            // 
            this.NewSiteButton.Location = new System.Drawing.Point(13, 12);
            this.NewSiteButton.Margin = new System.Windows.Forms.Padding(4);
            this.NewSiteButton.Name = "NewSiteButton";
            this.NewSiteButton.Size = new System.Drawing.Size(133, 28);
            this.NewSiteButton.TabIndex = 0;
            this.NewSiteButton.Text = "New Site";
            this.NewSiteButton.UseVisualStyleBackColor = true;
            this.NewSiteButton.Click += new System.EventHandler(this.NewSiteButton_Click);
            // 
            // BottomRightPanel
            // 
            this.BottomRightPanel.Controls.Add(this.DiscardButton);
            this.BottomRightPanel.Controls.Add(this.SaveButton);
            this.BottomRightPanel.Controls.Add(this.ExitButton);
            this.BottomRightPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomRightPanel.Location = new System.Drawing.Point(7, 709);
            this.BottomRightPanel.Margin = new System.Windows.Forms.Padding(4);
            this.BottomRightPanel.Name = "BottomRightPanel";
            this.BottomRightPanel.Size = new System.Drawing.Size(506, 74);
            this.BottomRightPanel.TabIndex = 11;
            // 
            // DiscardButton
            // 
            this.DiscardButton.Location = new System.Drawing.Point(273, 7);
            this.DiscardButton.Margin = new System.Windows.Forms.Padding(4);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(131, 28);
            this.DiscardButton.TabIndex = 17;
            this.DiscardButton.Text = "Discard Changes";
            this.DiscardButton.UseVisualStyleBackColor = true;
            this.DiscardButton.Click += new System.EventHandler(this.DiscardButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(412, 7);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(4);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(88, 28);
            this.SaveButton.TabIndex = 16;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(412, 43);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(4);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(88, 28);
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
            this.LeftPanel.Margin = new System.Windows.Forms.Padding(4);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.LeftPanel.Size = new System.Drawing.Size(533, 789);
            this.LeftPanel.TabIndex = 12;
            // 
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.InnerRightPanel);
            this.RightPanel.Controls.Add(this.LeftRightPanel);
            this.RightPanel.Controls.Add(this.BottomRightPanel);
            this.RightPanel.Controls.Add(this.panel4);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightPanel.Location = new System.Drawing.Point(533, 0);
            this.RightPanel.Margin = new System.Windows.Forms.Padding(4);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.RightPanel.Size = new System.Drawing.Size(520, 789);
            this.RightPanel.TabIndex = 13;
            // 
            // InnerRightPanel
            // 
            this.InnerRightPanel.Controls.Add(this.InstrumentPanel);
            this.InnerRightPanel.Controls.Add(this.NamePanel);
            this.InnerRightPanel.Controls.Add(this.NewButtonPanel);
            this.InnerRightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InnerRightPanel.Location = new System.Drawing.Point(67, 80);
            this.InnerRightPanel.Margin = new System.Windows.Forms.Padding(4);
            this.InnerRightPanel.Name = "InnerRightPanel";
            this.InnerRightPanel.Size = new System.Drawing.Size(446, 629);
            this.InnerRightPanel.TabIndex = 13;
            // 
            // NamePanel
            // 
            this.NamePanel.Controls.Add(this.label2);
            this.NamePanel.Controls.Add(this.TypeLabel);
            this.NamePanel.Controls.Add(this.label1);
            this.NamePanel.Controls.Add(this.NameTextBox);
            this.NamePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.NamePanel.Location = new System.Drawing.Point(0, 87);
            this.NamePanel.Margin = new System.Windows.Forms.Padding(4);
            this.NamePanel.Name = "NamePanel";
            this.NamePanel.Size = new System.Drawing.Size(446, 68);
            this.NamePanel.TabIndex = 11;
            // 
            // LeftRightPanel
            // 
            this.LeftRightPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LeftRightPanel.Controls.Add(this.RemoveButton);
            this.LeftRightPanel.Controls.Add(this.DownButton);
            this.LeftRightPanel.Controls.Add(this.UpButton);
            this.LeftRightPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftRightPanel.Location = new System.Drawing.Point(7, 80);
            this.LeftRightPanel.Margin = new System.Windows.Forms.Padding(4);
            this.LeftRightPanel.Name = "LeftRightPanel";
            this.LeftRightPanel.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LeftRightPanel.Size = new System.Drawing.Size(60, 629);
            this.LeftRightPanel.TabIndex = 14;
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(3, 252);
            this.RemoveButton.Margin = new System.Windows.Forms.Padding(4);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(53, 49);
            this.RemoveButton.TabIndex = 3;
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // DownButton
            // 
            this.DownButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DownButton.Location = new System.Drawing.Point(3, 574);
            this.DownButton.Margin = new System.Windows.Forms.Padding(4);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(50, 49);
            this.DownButton.TabIndex = 1;
            this.DownButton.UseVisualStyleBackColor = true;
            // 
            // UpButton
            // 
            this.UpButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.UpButton.Location = new System.Drawing.Point(3, 2);
            this.UpButton.Margin = new System.Windows.Forms.Padding(4);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(50, 49);
            this.UpButton.TabIndex = 0;
            this.UpButton.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.ExportButton);
            this.panel4.Controls.Add(this.ImportButton);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(7, 6);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(506, 74);
            this.panel4.TabIndex = 12;
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(96, 4);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(4);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(88, 28);
            this.ExportButton.TabIndex = 1;
            this.ExportButton.Text = "Export";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.Location = new System.Drawing.Point(4, 4);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(4);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(88, 28);
            this.ImportButton.TabIndex = 0;
            this.ImportButton.Text = "Import";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // SiteManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 789);
            this.ControlBox = false;
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.LeftPanel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(677, 619);
            this.Name = "SiteManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Site Manager";
            this.Load += new System.EventHandler(this.SiteManagerForm_Load);
            this.InstrumentPanel.ResumeLayout(false);
            this.InstrumentPanel.PerformLayout();
            this.VirtualChannelGroupBox.ResumeLayout(false);
            this.VCChannelConstPanel.ResumeLayout(false);
            this.VCChannelConstPanel.PerformLayout();
            this.VCDelayPanel.ResumeLayout(false);
            this.VCDelayPanel.PerformLayout();
            this.VCTwoChannelPanel.ResumeLayout(false);
            this.VCTwoChannelPanel.PerformLayout();
            this.VCTopPanel.ResumeLayout(false);
            this.VCTopPanel.PerformLayout();
            this.NewButtonPanel.ResumeLayout(false);
            this.BottomRightPanel.ResumeLayout(false);
            this.LeftPanel.ResumeLayout(false);
            this.RightPanel.ResumeLayout(false);
            this.InnerRightPanel.ResumeLayout(false);
            this.NamePanel.ResumeLayout(false);
            this.NamePanel.PerformLayout();
            this.LeftRightPanel.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView SitesTreeView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.ComboBox InstTypeComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel InstrumentPanel;
        private System.Windows.Forms.TextBox PrefixTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox DirectoryTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel NewButtonPanel;
        private System.Windows.Forms.Button NewInstrumentButton;
        private System.Windows.Forms.Button NewSystemButton;
        private System.Windows.Forms.Button NewFacilityButton;
        private System.Windows.Forms.Button NewSiteButton;
        private System.Windows.Forms.Panel BottomRightPanel;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DirectoryButton;
        private System.Windows.Forms.Button DiscardButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Panel InnerRightPanel;
        private System.Windows.Forms.Panel LeftRightPanel;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.Panel NamePanel;
        private System.Windows.Forms.Button RemoveVirtualChannelButton;
        private System.Windows.Forms.Button AddVirtualChannelButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox VirtualChannelsComboBox;
        private System.Windows.Forms.GroupBox VirtualChannelGroupBox;
        private System.Windows.Forms.Panel VCTopPanel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox VirtualChannelTypeComboBox;
        private System.Windows.Forms.TextBox VirtualChannelNameTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel VCTwoChannelPanel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox VirtualChannelChanBComboBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox VirtualChannelChanAComboBox;
        private System.Windows.Forms.Panel VCChannelConstPanel;
        private System.Windows.Forms.TextBox VirtualChannelConstantTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox VirtualChannelChannelComboBox;
        private System.Windows.Forms.Panel VCDelayPanel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox DelayChannelComboBox;
        private System.Windows.Forms.ComboBox DelayComboBox;
        private System.Windows.Forms.TextBox DelayTextBox;
    }
}