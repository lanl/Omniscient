namespace Omniscient
{
    partial class EventManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventManagerForm));
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.SitesTreeView = new System.Windows.Forms.TreeView();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.InnerRightPanel = new System.Windows.Forms.Panel();
            this.ActionPanel = new System.Windows.Forms.Panel();
            this.ActionGroupBox = new System.Windows.Forms.GroupBox();
            this.CommandPanel = new System.Windows.Forms.Panel();
            this.ActionCommandTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.AnalysisPanel = new System.Windows.Forms.Panel();
            this.AnalysisTabControl = new System.Windows.Forms.TabControl();
            this.DataTabPage = new System.Windows.Forms.TabPage();
            this.DataCompilersComboBox = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.AnalysisChannelComboBox = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.AnalysisTabPage = new System.Windows.Forms.TabPage();
            this.AnalysisCommandTextBox = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.ResultsTabPage = new System.Windows.Forms.TabPage();
            this.ResultFileTextBox = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.ActionSubPanel = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.ActionNameTextBox = new System.Windows.Forms.TextBox();
            this.ActionTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.RemoveActionButton = new System.Windows.Forms.Button();
            this.AddActionButton = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.ActionsComboBox = new System.Windows.Forms.ComboBox();
            this.ThresholdPanel = new System.Windows.Forms.Panel();
            this.DebounceComboBox = new System.Windows.Forms.ComboBox();
            this.DebounceTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ChannelComboBox = new System.Windows.Forms.ComboBox();
            this.ThresholdTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CoincidencePanel = new System.Windows.Forms.Panel();
            this.MinDifferenceComboBox = new System.Windows.Forms.ComboBox();
            this.MinDifferenceTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TimingTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.EventGeneratorBComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.EventGeneratorAComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.WindowComboBox = new System.Windows.Forms.ComboBox();
            this.WindowTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CoincidenceTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.NamePanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.LeftRightPanel = new System.Windows.Forms.Panel();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.UpButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.CompiledFileTextBox = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.LeftPanel.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.InnerRightPanel.SuspendLayout();
            this.ActionPanel.SuspendLayout();
            this.ActionGroupBox.SuspendLayout();
            this.CommandPanel.SuspendLayout();
            this.AnalysisPanel.SuspendLayout();
            this.AnalysisTabControl.SuspendLayout();
            this.DataTabPage.SuspendLayout();
            this.AnalysisTabPage.SuspendLayout();
            this.ResultsTabPage.SuspendLayout();
            this.ActionSubPanel.SuspendLayout();
            this.ThresholdPanel.SuspendLayout();
            this.CoincidencePanel.SuspendLayout();
            this.NamePanel.SuspendLayout();
            this.LeftRightPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // LeftPanel
            // 
            this.LeftPanel.Controls.Add(this.SitesTreeView);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Padding = new System.Windows.Forms.Padding(10);
            this.LeftPanel.Size = new System.Drawing.Size(400, 581);
            this.LeftPanel.TabIndex = 13;
            // 
            // SitesTreeView
            // 
            this.SitesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SitesTreeView.Location = new System.Drawing.Point(10, 10);
            this.SitesTreeView.Name = "SitesTreeView";
            this.SitesTreeView.ShowNodeToolTips = true;
            this.SitesTreeView.Size = new System.Drawing.Size(380, 561);
            this.SitesTreeView.TabIndex = 0;
            this.SitesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SitesTreeView_AfterSelect);
            // 
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.InnerRightPanel);
            this.RightPanel.Controls.Add(this.LeftRightPanel);
            this.RightPanel.Controls.Add(this.panel1);
            this.RightPanel.Controls.Add(this.panel3);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightPanel.Location = new System.Drawing.Point(400, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(390, 581);
            this.RightPanel.TabIndex = 14;
            // 
            // InnerRightPanel
            // 
            this.InnerRightPanel.Controls.Add(this.ActionPanel);
            this.InnerRightPanel.Controls.Add(this.ThresholdPanel);
            this.InnerRightPanel.Controls.Add(this.CoincidencePanel);
            this.InnerRightPanel.Controls.Add(this.NamePanel);
            this.InnerRightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InnerRightPanel.Location = new System.Drawing.Point(46, 29);
            this.InnerRightPanel.Name = "InnerRightPanel";
            this.InnerRightPanel.Size = new System.Drawing.Size(344, 523);
            this.InnerRightPanel.TabIndex = 34;
            // 
            // ActionPanel
            // 
            this.ActionPanel.Controls.Add(this.ActionGroupBox);
            this.ActionPanel.Controls.Add(this.RemoveActionButton);
            this.ActionPanel.Controls.Add(this.AddActionButton);
            this.ActionPanel.Controls.Add(this.label11);
            this.ActionPanel.Controls.Add(this.ActionsComboBox);
            this.ActionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ActionPanel.Location = new System.Drawing.Point(0, 297);
            this.ActionPanel.Name = "ActionPanel";
            this.ActionPanel.Size = new System.Drawing.Size(344, 320);
            this.ActionPanel.TabIndex = 34;
            // 
            // ActionGroupBox
            // 
            this.ActionGroupBox.Controls.Add(this.CommandPanel);
            this.ActionGroupBox.Controls.Add(this.AnalysisPanel);
            this.ActionGroupBox.Controls.Add(this.ActionSubPanel);
            this.ActionGroupBox.Location = new System.Drawing.Point(5, 37);
            this.ActionGroupBox.Name = "ActionGroupBox";
            this.ActionGroupBox.Size = new System.Drawing.Size(334, 280);
            this.ActionGroupBox.TabIndex = 25;
            this.ActionGroupBox.TabStop = false;
            this.ActionGroupBox.Text = "Action";
            // 
            // CommandPanel
            // 
            this.CommandPanel.Controls.Add(this.ActionCommandTextBox);
            this.CommandPanel.Controls.Add(this.label14);
            this.CommandPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CommandPanel.Location = new System.Drawing.Point(3, 271);
            this.CommandPanel.Name = "CommandPanel";
            this.CommandPanel.Size = new System.Drawing.Size(328, 28);
            this.CommandPanel.TabIndex = 34;
            // 
            // ActionCommandTextBox
            // 
            this.ActionCommandTextBox.Location = new System.Drawing.Point(91, 3);
            this.ActionCommandTextBox.Name = "ActionCommandTextBox";
            this.ActionCommandTextBox.Size = new System.Drawing.Size(227, 20);
            this.ActionCommandTextBox.TabIndex = 32;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(32, 6);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(54, 13);
            this.label14.TabIndex = 31;
            this.label14.Text = "Command";
            // 
            // AnalysisPanel
            // 
            this.AnalysisPanel.Controls.Add(this.AnalysisTabControl);
            this.AnalysisPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.AnalysisPanel.Location = new System.Drawing.Point(3, 71);
            this.AnalysisPanel.Name = "AnalysisPanel";
            this.AnalysisPanel.Size = new System.Drawing.Size(328, 200);
            this.AnalysisPanel.TabIndex = 35;
            // 
            // AnalysisTabControl
            // 
            this.AnalysisTabControl.Controls.Add(this.DataTabPage);
            this.AnalysisTabControl.Controls.Add(this.AnalysisTabPage);
            this.AnalysisTabControl.Controls.Add(this.ResultsTabPage);
            this.AnalysisTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AnalysisTabControl.Location = new System.Drawing.Point(0, 0);
            this.AnalysisTabControl.Name = "AnalysisTabControl";
            this.AnalysisTabControl.SelectedIndex = 0;
            this.AnalysisTabControl.Size = new System.Drawing.Size(328, 200);
            this.AnalysisTabControl.TabIndex = 0;
            // 
            // DataTabPage
            // 
            this.DataTabPage.Controls.Add(this.CompiledFileTextBox);
            this.DataTabPage.Controls.Add(this.label19);
            this.DataTabPage.Controls.Add(this.DataCompilersComboBox);
            this.DataTabPage.Controls.Add(this.label18);
            this.DataTabPage.Controls.Add(this.AnalysisChannelComboBox);
            this.DataTabPage.Controls.Add(this.label15);
            this.DataTabPage.Location = new System.Drawing.Point(4, 22);
            this.DataTabPage.Name = "DataTabPage";
            this.DataTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.DataTabPage.Size = new System.Drawing.Size(320, 174);
            this.DataTabPage.TabIndex = 0;
            this.DataTabPage.Text = "Data";
            this.DataTabPage.UseVisualStyleBackColor = true;
            // 
            // DataCompilersComboBox
            // 
            this.DataCompilersComboBox.FormattingEnabled = true;
            this.DataCompilersComboBox.Items.AddRange(new object[] {
            "Analysis",
            "Command"});
            this.DataCompilersComboBox.Location = new System.Drawing.Point(86, 33);
            this.DataCompilersComboBox.Name = "DataCompilersComboBox";
            this.DataCompilersComboBox.Size = new System.Drawing.Size(172, 21);
            this.DataCompilersComboBox.TabIndex = 31;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(9, 36);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(73, 13);
            this.label18.TabIndex = 30;
            this.label18.Text = "Data Compiler";
            // 
            // AnalysisChannelComboBox
            // 
            this.AnalysisChannelComboBox.FormattingEnabled = true;
            this.AnalysisChannelComboBox.Location = new System.Drawing.Point(56, 6);
            this.AnalysisChannelComboBox.Name = "AnalysisChannelComboBox";
            this.AnalysisChannelComboBox.Size = new System.Drawing.Size(202, 21);
            this.AnalysisChannelComboBox.TabIndex = 29;
            this.AnalysisChannelComboBox.SelectedIndexChanged += new System.EventHandler(this.AnalysisChannelComboBox_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(5, 10);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(46, 13);
            this.label15.TabIndex = 28;
            this.label15.Text = "Channel";
            // 
            // AnalysisTabPage
            // 
            this.AnalysisTabPage.Controls.Add(this.AnalysisCommandTextBox);
            this.AnalysisTabPage.Controls.Add(this.label16);
            this.AnalysisTabPage.Location = new System.Drawing.Point(4, 22);
            this.AnalysisTabPage.Name = "AnalysisTabPage";
            this.AnalysisTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.AnalysisTabPage.Size = new System.Drawing.Size(320, 174);
            this.AnalysisTabPage.TabIndex = 1;
            this.AnalysisTabPage.Text = "Analysis";
            this.AnalysisTabPage.UseVisualStyleBackColor = true;
            // 
            // AnalysisCommandTextBox
            // 
            this.AnalysisCommandTextBox.Location = new System.Drawing.Point(68, 6);
            this.AnalysisCommandTextBox.Multiline = true;
            this.AnalysisCommandTextBox.Name = "AnalysisCommandTextBox";
            this.AnalysisCommandTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AnalysisCommandTextBox.Size = new System.Drawing.Size(240, 150);
            this.AnalysisCommandTextBox.TabIndex = 34;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(9, 9);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(54, 13);
            this.label16.TabIndex = 33;
            this.label16.Text = "Command";
            // 
            // ResultsTabPage
            // 
            this.ResultsTabPage.Controls.Add(this.ResultFileTextBox);
            this.ResultsTabPage.Controls.Add(this.label17);
            this.ResultsTabPage.Location = new System.Drawing.Point(4, 22);
            this.ResultsTabPage.Name = "ResultsTabPage";
            this.ResultsTabPage.Size = new System.Drawing.Size(320, 94);
            this.ResultsTabPage.TabIndex = 2;
            this.ResultsTabPage.Text = "Results";
            this.ResultsTabPage.UseVisualStyleBackColor = true;
            // 
            // ResultFileTextBox
            // 
            this.ResultFileTextBox.Location = new System.Drawing.Point(68, 3);
            this.ResultFileTextBox.Name = "ResultFileTextBox";
            this.ResultFileTextBox.Size = new System.Drawing.Size(227, 20);
            this.ResultFileTextBox.TabIndex = 36;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(9, 6);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(56, 13);
            this.label17.TabIndex = 35;
            this.label17.Text = "Result File";
            // 
            // ActionSubPanel
            // 
            this.ActionSubPanel.Controls.Add(this.label12);
            this.ActionSubPanel.Controls.Add(this.ActionNameTextBox);
            this.ActionSubPanel.Controls.Add(this.ActionTypeComboBox);
            this.ActionSubPanel.Controls.Add(this.label13);
            this.ActionSubPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ActionSubPanel.Location = new System.Drawing.Point(3, 16);
            this.ActionSubPanel.Name = "ActionSubPanel";
            this.ActionSubPanel.Size = new System.Drawing.Size(328, 55);
            this.ActionSubPanel.TabIndex = 33;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(50, 8);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "Name";
            // 
            // ActionNameTextBox
            // 
            this.ActionNameTextBox.Location = new System.Drawing.Point(90, 5);
            this.ActionNameTextBox.Name = "ActionNameTextBox";
            this.ActionNameTextBox.Size = new System.Drawing.Size(172, 20);
            this.ActionNameTextBox.TabIndex = 27;
            // 
            // ActionTypeComboBox
            // 
            this.ActionTypeComboBox.FormattingEnabled = true;
            this.ActionTypeComboBox.Items.AddRange(new object[] {
            "Analysis",
            "Command"});
            this.ActionTypeComboBox.Location = new System.Drawing.Point(90, 31);
            this.ActionTypeComboBox.Name = "ActionTypeComboBox";
            this.ActionTypeComboBox.Size = new System.Drawing.Size(172, 21);
            this.ActionTypeComboBox.TabIndex = 29;
            this.ActionTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.ActionTypeComboBox_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(49, 34);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(31, 13);
            this.label13.TabIndex = 30;
            this.label13.Text = "Type";
            // 
            // RemoveActionButton
            // 
            this.RemoveActionButton.Image = ((System.Drawing.Image)(resources.GetObject("RemoveActionButton.Image")));
            this.RemoveActionButton.Location = new System.Drawing.Point(310, 3);
            this.RemoveActionButton.Name = "RemoveActionButton";
            this.RemoveActionButton.Size = new System.Drawing.Size(28, 28);
            this.RemoveActionButton.TabIndex = 24;
            this.RemoveActionButton.UseVisualStyleBackColor = true;
            this.RemoveActionButton.Click += new System.EventHandler(this.RemoveActionButton_Click);
            // 
            // AddActionButton
            // 
            this.AddActionButton.Image = ((System.Drawing.Image)(resources.GetObject("AddActionButton.Image")));
            this.AddActionButton.Location = new System.Drawing.Point(279, 3);
            this.AddActionButton.Name = "AddActionButton";
            this.AddActionButton.Size = new System.Drawing.Size(28, 28);
            this.AddActionButton.TabIndex = 23;
            this.AddActionButton.UseVisualStyleBackColor = true;
            this.AddActionButton.Click += new System.EventHandler(this.AddActionButton_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(54, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "Actions";
            // 
            // ActionsComboBox
            // 
            this.ActionsComboBox.FormattingEnabled = true;
            this.ActionsComboBox.Location = new System.Drawing.Point(105, 7);
            this.ActionsComboBox.Name = "ActionsComboBox";
            this.ActionsComboBox.Size = new System.Drawing.Size(172, 21);
            this.ActionsComboBox.TabIndex = 21;
            this.ActionsComboBox.SelectedIndexChanged += new System.EventHandler(this.ActionsComboBox_SelectedIndexChanged);
            // 
            // ThresholdPanel
            // 
            this.ThresholdPanel.Controls.Add(this.DebounceComboBox);
            this.ThresholdPanel.Controls.Add(this.DebounceTextBox);
            this.ThresholdPanel.Controls.Add(this.label4);
            this.ThresholdPanel.Controls.Add(this.label3);
            this.ThresholdPanel.Controls.Add(this.ChannelComboBox);
            this.ThresholdPanel.Controls.Add(this.ThresholdTextBox);
            this.ThresholdPanel.Controls.Add(this.label2);
            this.ThresholdPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ThresholdPanel.Location = new System.Drawing.Point(0, 193);
            this.ThresholdPanel.Name = "ThresholdPanel";
            this.ThresholdPanel.Size = new System.Drawing.Size(344, 104);
            this.ThresholdPanel.TabIndex = 25;
            // 
            // DebounceComboBox
            // 
            this.DebounceComboBox.FormattingEnabled = true;
            this.DebounceComboBox.Items.AddRange(new object[] {
            "Seconds",
            "Minutes",
            "Hours",
            "Days"});
            this.DebounceComboBox.Location = new System.Drawing.Point(163, 64);
            this.DebounceComboBox.Name = "DebounceComboBox";
            this.DebounceComboBox.Size = new System.Drawing.Size(114, 21);
            this.DebounceComboBox.TabIndex = 31;
            // 
            // DebounceTextBox
            // 
            this.DebounceTextBox.Location = new System.Drawing.Point(105, 64);
            this.DebounceTextBox.Name = "DebounceTextBox";
            this.DebounceTextBox.Size = new System.Drawing.Size(52, 20);
            this.DebounceTextBox.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Debounce Time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Threshold";
            // 
            // ChannelComboBox
            // 
            this.ChannelComboBox.FormattingEnabled = true;
            this.ChannelComboBox.Location = new System.Drawing.Point(105, 6);
            this.ChannelComboBox.Name = "ChannelComboBox";
            this.ChannelComboBox.Size = new System.Drawing.Size(172, 21);
            this.ChannelComboBox.TabIndex = 27;
            // 
            // ThresholdTextBox
            // 
            this.ThresholdTextBox.Location = new System.Drawing.Point(105, 35);
            this.ThresholdTextBox.Name = "ThresholdTextBox";
            this.ThresholdTextBox.Size = new System.Drawing.Size(172, 20);
            this.ThresholdTextBox.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Channel";
            // 
            // CoincidencePanel
            // 
            this.CoincidencePanel.Controls.Add(this.MinDifferenceComboBox);
            this.CoincidencePanel.Controls.Add(this.MinDifferenceTextBox);
            this.CoincidencePanel.Controls.Add(this.label6);
            this.CoincidencePanel.Controls.Add(this.TimingTypeComboBox);
            this.CoincidencePanel.Controls.Add(this.label10);
            this.CoincidencePanel.Controls.Add(this.EventGeneratorBComboBox);
            this.CoincidencePanel.Controls.Add(this.label9);
            this.CoincidencePanel.Controls.Add(this.EventGeneratorAComboBox);
            this.CoincidencePanel.Controls.Add(this.label8);
            this.CoincidencePanel.Controls.Add(this.WindowComboBox);
            this.CoincidencePanel.Controls.Add(this.WindowTextBox);
            this.CoincidencePanel.Controls.Add(this.label5);
            this.CoincidencePanel.Controls.Add(this.CoincidenceTypeComboBox);
            this.CoincidencePanel.Controls.Add(this.label7);
            this.CoincidencePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CoincidencePanel.Location = new System.Drawing.Point(0, 29);
            this.CoincidencePanel.Name = "CoincidencePanel";
            this.CoincidencePanel.Size = new System.Drawing.Size(344, 164);
            this.CoincidencePanel.TabIndex = 32;
            // 
            // MinDifferenceComboBox
            // 
            this.MinDifferenceComboBox.FormattingEnabled = true;
            this.MinDifferenceComboBox.Items.AddRange(new object[] {
            "Seconds",
            "Minutes",
            "Hours",
            "Days"});
            this.MinDifferenceComboBox.Location = new System.Drawing.Point(163, 135);
            this.MinDifferenceComboBox.Name = "MinDifferenceComboBox";
            this.MinDifferenceComboBox.Size = new System.Drawing.Size(114, 21);
            this.MinDifferenceComboBox.TabIndex = 40;
            // 
            // MinDifferenceTextBox
            // 
            this.MinDifferenceTextBox.Location = new System.Drawing.Point(105, 135);
            this.MinDifferenceTextBox.Name = "MinDifferenceTextBox";
            this.MinDifferenceTextBox.Size = new System.Drawing.Size(52, 20);
            this.MinDifferenceTextBox.TabIndex = 39;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 38;
            this.label6.Text = "Min Difference";
            // 
            // TimingTypeComboBox
            // 
            this.TimingTypeComboBox.FormattingEnabled = true;
            this.TimingTypeComboBox.Items.AddRange(new object[] {
            "Start to Start",
            "Start to End",
            "End to Start",
            "End to End",
            "Max to Max"});
            this.TimingTypeComboBox.Location = new System.Drawing.Point(105, 81);
            this.TimingTypeComboBox.Name = "TimingTypeComboBox";
            this.TimingTypeComboBox.Size = new System.Drawing.Size(172, 21);
            this.TimingTypeComboBox.TabIndex = 37;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(35, 85);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 13);
            this.label10.TabIndex = 36;
            this.label10.Text = "Timing Type";
            // 
            // EventGeneratorBComboBox
            // 
            this.EventGeneratorBComboBox.FormattingEnabled = true;
            this.EventGeneratorBComboBox.Location = new System.Drawing.Point(105, 30);
            this.EventGeneratorBComboBox.Name = "EventGeneratorBComboBox";
            this.EventGeneratorBComboBox.Size = new System.Drawing.Size(172, 21);
            this.EventGeneratorBComboBox.TabIndex = 35;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "Event Generator B";
            // 
            // EventGeneratorAComboBox
            // 
            this.EventGeneratorAComboBox.FormattingEnabled = true;
            this.EventGeneratorAComboBox.Location = new System.Drawing.Point(105, 3);
            this.EventGeneratorAComboBox.Name = "EventGeneratorAComboBox";
            this.EventGeneratorAComboBox.Size = new System.Drawing.Size(172, 21);
            this.EventGeneratorAComboBox.TabIndex = 33;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 13);
            this.label8.TabIndex = 32;
            this.label8.Text = "Event Generator A";
            // 
            // WindowComboBox
            // 
            this.WindowComboBox.FormattingEnabled = true;
            this.WindowComboBox.Items.AddRange(new object[] {
            "Seconds",
            "Minutes",
            "Hours",
            "Days"});
            this.WindowComboBox.Location = new System.Drawing.Point(163, 108);
            this.WindowComboBox.Name = "WindowComboBox";
            this.WindowComboBox.Size = new System.Drawing.Size(114, 21);
            this.WindowComboBox.TabIndex = 31;
            // 
            // WindowTextBox
            // 
            this.WindowTextBox.Location = new System.Drawing.Point(105, 108);
            this.WindowTextBox.Name = "WindowTextBox";
            this.WindowTextBox.Size = new System.Drawing.Size(52, 20);
            this.WindowTextBox.TabIndex = 30;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(54, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Window";
            // 
            // CoincidenceTypeComboBox
            // 
            this.CoincidenceTypeComboBox.FormattingEnabled = true;
            this.CoincidenceTypeComboBox.Items.AddRange(new object[] {
            "A then B",
            "B then A",
            "Either order"});
            this.CoincidenceTypeComboBox.Location = new System.Drawing.Point(105, 57);
            this.CoincidenceTypeComboBox.Name = "CoincidenceTypeComboBox";
            this.CoincidenceTypeComboBox.Size = new System.Drawing.Size(172, 21);
            this.CoincidenceTypeComboBox.TabIndex = 27;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Coincidence Type";
            // 
            // NamePanel
            // 
            this.NamePanel.Controls.Add(this.label1);
            this.NamePanel.Controls.Add(this.NameTextBox);
            this.NamePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.NamePanel.Location = new System.Drawing.Point(0, 0);
            this.NamePanel.Name = "NamePanel";
            this.NamePanel.Size = new System.Drawing.Size(344, 29);
            this.NamePanel.TabIndex = 33;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Name";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(105, 6);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(172, 20);
            this.NameTextBox.TabIndex = 25;
            // 
            // LeftRightPanel
            // 
            this.LeftRightPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LeftRightPanel.Controls.Add(this.DeleteButton);
            this.LeftRightPanel.Controls.Add(this.AddButton);
            this.LeftRightPanel.Controls.Add(this.DownButton);
            this.LeftRightPanel.Controls.Add(this.UpButton);
            this.LeftRightPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftRightPanel.Location = new System.Drawing.Point(0, 29);
            this.LeftRightPanel.Name = "LeftRightPanel";
            this.LeftRightPanel.Padding = new System.Windows.Forms.Padding(2);
            this.LeftRightPanel.Size = new System.Drawing.Size(46, 523);
            this.LeftRightPanel.TabIndex = 13;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(2, 255);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(40, 40);
            this.DeleteButton.TabIndex = 3;
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(2, 205);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(40, 40);
            this.AddButton.TabIndex = 2;
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // DownButton
            // 
            this.DownButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DownButton.Location = new System.Drawing.Point(2, 477);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(38, 40);
            this.DownButton.TabIndex = 1;
            this.DownButton.UseVisualStyleBackColor = true;
            this.DownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // UpButton
            // 
            this.UpButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.UpButton.Location = new System.Drawing.Point(2, 2);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(38, 40);
            this.UpButton.TabIndex = 0;
            this.UpButton.UseVisualStyleBackColor = true;
            this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(390, 29);
            this.panel1.TabIndex = 23;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.SaveButton);
            this.panel3.Controls.Add(this.ExitButton);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 552);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(390, 29);
            this.panel3.TabIndex = 12;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(237, 3);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 24;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(318, 3);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(66, 23);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // CompiledFileTextBox
            // 
            this.CompiledFileTextBox.Location = new System.Drawing.Point(86, 63);
            this.CompiledFileTextBox.Multiline = true;
            this.CompiledFileTextBox.Name = "CompiledFileTextBox";
            this.CompiledFileTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.CompiledFileTextBox.Size = new System.Drawing.Size(227, 80);
            this.CompiledFileTextBox.TabIndex = 36;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(24, 66);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(58, 13);
            this.label19.TabIndex = 35;
            this.label19.Text = "Output File";
            // 
            // EventManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 581);
            this.ControlBox = false;
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.LeftPanel);
            this.MinimumSize = new System.Drawing.Size(762, 597);
            this.Name = "EventManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Event Manager";
            this.Load += new System.EventHandler(this.EventManagerForm_Load);
            this.LeftPanel.ResumeLayout(false);
            this.RightPanel.ResumeLayout(false);
            this.InnerRightPanel.ResumeLayout(false);
            this.ActionPanel.ResumeLayout(false);
            this.ActionPanel.PerformLayout();
            this.ActionGroupBox.ResumeLayout(false);
            this.CommandPanel.ResumeLayout(false);
            this.CommandPanel.PerformLayout();
            this.AnalysisPanel.ResumeLayout(false);
            this.AnalysisTabControl.ResumeLayout(false);
            this.DataTabPage.ResumeLayout(false);
            this.DataTabPage.PerformLayout();
            this.AnalysisTabPage.ResumeLayout(false);
            this.AnalysisTabPage.PerformLayout();
            this.ResultsTabPage.ResumeLayout(false);
            this.ResultsTabPage.PerformLayout();
            this.ActionSubPanel.ResumeLayout(false);
            this.ActionSubPanel.PerformLayout();
            this.ThresholdPanel.ResumeLayout(false);
            this.ThresholdPanel.PerformLayout();
            this.CoincidencePanel.ResumeLayout(false);
            this.CoincidencePanel.PerformLayout();
            this.NamePanel.ResumeLayout(false);
            this.NamePanel.PerformLayout();
            this.LeftRightPanel.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.TreeView SitesTreeView;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Panel LeftRightPanel;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Panel InnerRightPanel;
        private System.Windows.Forms.Panel ThresholdPanel;
        private System.Windows.Forms.ComboBox DebounceComboBox;
        private System.Windows.Forms.TextBox DebounceTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ChannelComboBox;
        private System.Windows.Forms.TextBox ThresholdTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel CoincidencePanel;
        private System.Windows.Forms.ComboBox WindowComboBox;
        private System.Windows.Forms.TextBox WindowTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CoincidenceTypeComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel NamePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.ComboBox MinDifferenceComboBox;
        private System.Windows.Forms.TextBox MinDifferenceTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox TimingTypeComboBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox EventGeneratorBComboBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox EventGeneratorAComboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel ActionPanel;
        private System.Windows.Forms.Button RemoveActionButton;
        private System.Windows.Forms.Button AddActionButton;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox ActionsComboBox;
        private System.Windows.Forms.GroupBox ActionGroupBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox ActionNameTextBox;
        private System.Windows.Forms.TextBox ActionCommandTextBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox ActionTypeComboBox;
        private System.Windows.Forms.Panel CommandPanel;
        private System.Windows.Forms.Panel ActionSubPanel;
        private System.Windows.Forms.Panel AnalysisPanel;
        private System.Windows.Forms.ComboBox AnalysisChannelComboBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TabControl AnalysisTabControl;
        private System.Windows.Forms.TabPage DataTabPage;
        private System.Windows.Forms.TabPage AnalysisTabPage;
        private System.Windows.Forms.TabPage ResultsTabPage;
        private System.Windows.Forms.TextBox AnalysisCommandTextBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox ResultFileTextBox;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox DataCompilersComboBox;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox CompiledFileTextBox;
        private System.Windows.Forms.Label label19;
    }
}