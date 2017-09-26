using System.Windows.Forms;

namespace Omniscient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.StripChartControlPanel = new System.Windows.Forms.Panel();
            this.StripChartsPanel = new System.Windows.Forms.Panel();
            this.StripChartsLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.StripChart3 = new LiveCharts.WinForms.CartesianChart();
            this.StripChart2 = new LiveCharts.WinForms.CartesianChart();
            this.StripChart1 = new LiveCharts.WinForms.CartesianChart();
            this.StripChart0 = new LiveCharts.WinForms.CartesianChart();
            this.StripChartScroll = new System.Windows.Forms.HScrollBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.launchInspectrumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChannelsLabelPanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.ChannelsPanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Chart1TabPage = new System.Windows.Forms.TabPage();
            this.C1LogScaleCheckBox = new System.Windows.Forms.CheckBox();
            this.Chart2TabPage = new System.Windows.Forms.TabPage();
            this.C2LogScaleCheckBox = new System.Windows.Forms.CheckBox();
            this.Chart3TabPage = new System.Windows.Forms.TabPage();
            this.C3LogScaleCheckBox = new System.Windows.Forms.CheckBox();
            this.Chart4TabPage = new System.Windows.Forms.TabPage();
            this.C4LogScaleCheckBox = new System.Windows.Forms.CheckBox();
            this.RangeUpdateButton = new System.Windows.Forms.Button();
            this.RangeComboBox = new System.Windows.Forms.ComboBox();
            this.RangeTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.EndTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartDatePicker = new System.Windows.Forms.DateTimePicker();
            this.EndDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SitesTreeView = new Omniscient.Controls.ResponsiveTreeView();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.CenterSplitContainer = new System.Windows.Forms.SplitContainer();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.EventStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Instrument0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Instrument1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Instrument2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Instrument3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventControlPanel = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MarkerToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.StripChartControlPanel.SuspendLayout();
            this.StripChartsPanel.SuspendLayout();
            this.StripChartsLayoutPanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.ChannelsLabelPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.ChannelsPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Chart1TabPage.SuspendLayout();
            this.Chart2TabPage.SuspendLayout();
            this.Chart3TabPage.SuspendLayout();
            this.Chart4TabPage.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CenterSplitContainer)).BeginInit();
            this.CenterSplitContainer.Panel1.SuspendLayout();
            this.CenterSplitContainer.Panel2.SuspendLayout();
            this.CenterSplitContainer.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.EventControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // StripChartControlPanel
            // 
            this.StripChartControlPanel.Controls.Add(this.StripChartsPanel);
            this.StripChartControlPanel.Controls.Add(this.StripChartScroll);
            this.StripChartControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StripChartControlPanel.Location = new System.Drawing.Point(0, 0);
            this.StripChartControlPanel.Name = "StripChartControlPanel";
            this.StripChartControlPanel.Size = new System.Drawing.Size(735, 436);
            this.StripChartControlPanel.TabIndex = 1;
            // 
            // StripChartsPanel
            // 
            this.StripChartsPanel.Controls.Add(this.StripChartsLayoutPanel);
            this.StripChartsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StripChartsPanel.Location = new System.Drawing.Point(0, 0);
            this.StripChartsPanel.Name = "StripChartsPanel";
            this.StripChartsPanel.Size = new System.Drawing.Size(735, 419);
            this.StripChartsPanel.TabIndex = 2;
            // 
            // StripChartsLayoutPanel
            // 
            this.StripChartsLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.StripChartsLayoutPanel.ColumnCount = 1;
            this.StripChartsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.StripChartsLayoutPanel.Controls.Add(this.StripChart3, 0, 2);
            this.StripChartsLayoutPanel.Controls.Add(this.StripChart2, 0, 1);
            this.StripChartsLayoutPanel.Controls.Add(this.StripChart1, 0, 1);
            this.StripChartsLayoutPanel.Controls.Add(this.StripChart0, 0, 0);
            this.StripChartsLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StripChartsLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.StripChartsLayoutPanel.Name = "StripChartsLayoutPanel";
            this.StripChartsLayoutPanel.RowCount = 4;
            this.StripChartsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.StripChartsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.StripChartsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.StripChartsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.StripChartsLayoutPanel.Size = new System.Drawing.Size(735, 419);
            this.StripChartsLayoutPanel.TabIndex = 1;
            // 
            // StripChart3
            // 
            this.StripChart3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StripChart3.Location = new System.Drawing.Point(5, 317);
            this.StripChart3.Name = "StripChart3";
            this.StripChart3.Size = new System.Drawing.Size(725, 97);
            this.StripChart3.TabIndex = 4;
            this.StripChart3.Text = "cartesianChart3";
            // 
            // StripChart2
            // 
            this.StripChart2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StripChart2.Location = new System.Drawing.Point(5, 213);
            this.StripChart2.Name = "StripChart2";
            this.StripChart2.Size = new System.Drawing.Size(725, 96);
            this.StripChart2.TabIndex = 3;
            this.StripChart2.Text = "cartesianChart2";
            // 
            // StripChart1
            // 
            this.StripChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StripChart1.Location = new System.Drawing.Point(5, 109);
            this.StripChart1.Name = "StripChart1";
            this.StripChart1.Size = new System.Drawing.Size(725, 96);
            this.StripChart1.TabIndex = 2;
            this.StripChart1.Text = "cartesianChart1";
            // 
            // StripChart0
            // 
            this.StripChart0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StripChart0.Location = new System.Drawing.Point(5, 5);
            this.StripChart0.Name = "StripChart0";
            this.StripChart0.Size = new System.Drawing.Size(725, 96);
            this.StripChart0.TabIndex = 1;
            this.StripChart0.Text = "cartesianChart1";
            // 
            // StripChartScroll
            // 
            this.StripChartScroll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.StripChartScroll.Location = new System.Drawing.Point(0, 419);
            this.StripChartScroll.Name = "StripChartScroll";
            this.StripChartScroll.Size = new System.Drawing.Size(735, 17);
            this.StripChartScroll.TabIndex = 1;
            this.StripChartScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.StripChartScroll_Scroll);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1254, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.launchInspectrumToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // launchInspectrumToolStripMenuItem
            // 
            this.launchInspectrumToolStripMenuItem.Name = "launchInspectrumToolStripMenuItem";
            this.launchInspectrumToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.launchInspectrumToolStripMenuItem.Text = "Launch Inspectrum";
            this.launchInspectrumToolStripMenuItem.Click += new System.EventHandler(this.launchInspectrumToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // ChannelsLabelPanel
            // 
            this.ChannelsLabelPanel.Controls.Add(this.label5);
            this.ChannelsLabelPanel.Controls.Add(this.label4);
            this.ChannelsLabelPanel.Controls.Add(this.label3);
            this.ChannelsLabelPanel.Controls.Add(this.label2);
            this.ChannelsLabelPanel.Controls.Add(this.label1);
            this.ChannelsLabelPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChannelsLabelPanel.Location = new System.Drawing.Point(5, 0);
            this.ChannelsLabelPanel.Name = "ChannelsLabelPanel";
            this.ChannelsLabelPanel.Size = new System.Drawing.Size(306, 20);
            this.ChannelsLabelPanel.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(221, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(196, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(146, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chart:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.MarkerToolStripLabel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1254, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.ToolStripProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 716);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1254, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel1.Text = "Ready";
            // 
            // ToolStripProgressBar
            // 
            this.ToolStripProgressBar.Name = "ToolStripProgressBar";
            this.ToolStripProgressBar.Padding = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.ToolStripProgressBar.Size = new System.Drawing.Size(320, 16);
            // 
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.ChannelsPanel);
            this.RightPanel.Controls.Add(this.groupBox1);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightPanel.Location = new System.Drawing.Point(939, 49);
            this.RightPanel.MinimumSize = new System.Drawing.Size(295, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(315, 667);
            this.RightPanel.TabIndex = 7;
            // 
            // ChannelsPanel
            // 
            this.ChannelsPanel.AutoScroll = true;
            this.ChannelsPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ChannelsPanel.Controls.Add(this.ChannelsLabelPanel);
            this.ChannelsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChannelsPanel.Location = new System.Drawing.Point(0, 0);
            this.ChannelsPanel.Name = "ChannelsPanel";
            this.ChannelsPanel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ChannelsPanel.Size = new System.Drawing.Size(315, 431);
            this.ChannelsPanel.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Controls.Add(this.RangeUpdateButton);
            this.groupBox1.Controls.Add(this.RangeComboBox);
            this.groupBox1.Controls.Add(this.RangeTextBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.EndTimePicker);
            this.groupBox1.Controls.Add(this.StartTimePicker);
            this.groupBox1.Controls.Add(this.StartDatePicker);
            this.groupBox1.Controls.Add(this.EndDatePicker);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 431);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 236);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "View";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Chart1TabPage);
            this.tabControl1.Controls.Add(this.Chart2TabPage);
            this.tabControl1.Controls.Add(this.Chart3TabPage);
            this.tabControl1.Controls.Add(this.Chart4TabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(3, 133);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(309, 100);
            this.tabControl1.TabIndex = 16;
            // 
            // Chart1TabPage
            // 
            this.Chart1TabPage.Controls.Add(this.C1LogScaleCheckBox);
            this.Chart1TabPage.Location = new System.Drawing.Point(4, 22);
            this.Chart1TabPage.Name = "Chart1TabPage";
            this.Chart1TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.Chart1TabPage.Size = new System.Drawing.Size(301, 74);
            this.Chart1TabPage.TabIndex = 0;
            this.Chart1TabPage.Text = "Chart 1";
            this.Chart1TabPage.UseVisualStyleBackColor = true;
            // 
            // C1LogScaleCheckBox
            // 
            this.C1LogScaleCheckBox.AutoSize = true;
            this.C1LogScaleCheckBox.Location = new System.Drawing.Point(10, 10);
            this.C1LogScaleCheckBox.Name = "C1LogScaleCheckBox";
            this.C1LogScaleCheckBox.Size = new System.Drawing.Size(74, 17);
            this.C1LogScaleCheckBox.TabIndex = 0;
            this.C1LogScaleCheckBox.Text = "Log Scale";
            this.C1LogScaleCheckBox.UseVisualStyleBackColor = true;
            this.C1LogScaleCheckBox.CheckedChanged += new System.EventHandler(this.C1LogScaleCheckBox_CheckedChanged);
            // 
            // Chart2TabPage
            // 
            this.Chart2TabPage.Controls.Add(this.C2LogScaleCheckBox);
            this.Chart2TabPage.Location = new System.Drawing.Point(4, 22);
            this.Chart2TabPage.Name = "Chart2TabPage";
            this.Chart2TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.Chart2TabPage.Size = new System.Drawing.Size(301, 74);
            this.Chart2TabPage.TabIndex = 1;
            this.Chart2TabPage.Text = "Chart 2";
            this.Chart2TabPage.UseVisualStyleBackColor = true;
            // 
            // C2LogScaleCheckBox
            // 
            this.C2LogScaleCheckBox.AutoSize = true;
            this.C2LogScaleCheckBox.Location = new System.Drawing.Point(10, 10);
            this.C2LogScaleCheckBox.Name = "C2LogScaleCheckBox";
            this.C2LogScaleCheckBox.Size = new System.Drawing.Size(74, 17);
            this.C2LogScaleCheckBox.TabIndex = 1;
            this.C2LogScaleCheckBox.Text = "Log Scale";
            this.C2LogScaleCheckBox.UseVisualStyleBackColor = true;
            this.C2LogScaleCheckBox.CheckedChanged += new System.EventHandler(this.C2LogScaleCheckBox_CheckedChanged);
            // 
            // Chart3TabPage
            // 
            this.Chart3TabPage.Controls.Add(this.C3LogScaleCheckBox);
            this.Chart3TabPage.Location = new System.Drawing.Point(4, 22);
            this.Chart3TabPage.Name = "Chart3TabPage";
            this.Chart3TabPage.Size = new System.Drawing.Size(301, 74);
            this.Chart3TabPage.TabIndex = 2;
            this.Chart3TabPage.Text = "Chart 3";
            this.Chart3TabPage.UseVisualStyleBackColor = true;
            // 
            // C3LogScaleCheckBox
            // 
            this.C3LogScaleCheckBox.AutoSize = true;
            this.C3LogScaleCheckBox.Location = new System.Drawing.Point(10, 10);
            this.C3LogScaleCheckBox.Name = "C3LogScaleCheckBox";
            this.C3LogScaleCheckBox.Size = new System.Drawing.Size(74, 17);
            this.C3LogScaleCheckBox.TabIndex = 1;
            this.C3LogScaleCheckBox.Text = "Log Scale";
            this.C3LogScaleCheckBox.UseVisualStyleBackColor = true;
            this.C3LogScaleCheckBox.CheckedChanged += new System.EventHandler(this.C3LogScaleCheckBox_CheckedChanged);
            // 
            // Chart4TabPage
            // 
            this.Chart4TabPage.Controls.Add(this.C4LogScaleCheckBox);
            this.Chart4TabPage.Location = new System.Drawing.Point(4, 22);
            this.Chart4TabPage.Name = "Chart4TabPage";
            this.Chart4TabPage.Size = new System.Drawing.Size(301, 74);
            this.Chart4TabPage.TabIndex = 3;
            this.Chart4TabPage.Text = "Chart 4";
            this.Chart4TabPage.UseVisualStyleBackColor = true;
            // 
            // C4LogScaleCheckBox
            // 
            this.C4LogScaleCheckBox.AutoSize = true;
            this.C4LogScaleCheckBox.Location = new System.Drawing.Point(10, 10);
            this.C4LogScaleCheckBox.Name = "C4LogScaleCheckBox";
            this.C4LogScaleCheckBox.Size = new System.Drawing.Size(74, 17);
            this.C4LogScaleCheckBox.TabIndex = 1;
            this.C4LogScaleCheckBox.Text = "Log Scale";
            this.C4LogScaleCheckBox.UseVisualStyleBackColor = true;
            this.C4LogScaleCheckBox.CheckedChanged += new System.EventHandler(this.C4LogScaleCheckBox_CheckedChanged);
            // 
            // RangeUpdateButton
            // 
            this.RangeUpdateButton.Location = new System.Drawing.Point(185, 100);
            this.RangeUpdateButton.Name = "RangeUpdateButton";
            this.RangeUpdateButton.Size = new System.Drawing.Size(59, 26);
            this.RangeUpdateButton.TabIndex = 15;
            this.RangeUpdateButton.Text = "Update";
            this.RangeUpdateButton.UseVisualStyleBackColor = true;
            this.RangeUpdateButton.Click += new System.EventHandler(this.RangeUpdateButton_Click);
            // 
            // RangeComboBox
            // 
            this.RangeComboBox.FormattingEnabled = true;
            this.RangeComboBox.Items.AddRange(new object[] {
            "Minutes",
            "Hours",
            "Days",
            "Months",
            "Years"});
            this.RangeComboBox.Location = new System.Drawing.Point(141, 46);
            this.RangeComboBox.Name = "RangeComboBox";
            this.RangeComboBox.Size = new System.Drawing.Size(103, 21);
            this.RangeComboBox.TabIndex = 14;
            this.RangeComboBox.SelectedIndexChanged += new System.EventHandler(this.RangeComboBox_SelectedIndexChanged);
            this.RangeComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RangeTextBox_KeyDown);
            // 
            // RangeTextBox
            // 
            this.RangeTextBox.Location = new System.Drawing.Point(47, 46);
            this.RangeTextBox.Name = "RangeTextBox";
            this.RangeTextBox.Size = new System.Drawing.Size(88, 20);
            this.RangeTextBox.TabIndex = 13;
            this.RangeTextBox.Text = "1";
            this.RangeTextBox.TextChanged += new System.EventHandler(this.RangeTextBox_TextChanged);
            this.RangeTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RangeTextBox_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Range";
            // 
            // EndTimePicker
            // 
            this.EndTimePicker.CustomFormat = "MMM dd, yyyy\'";
            this.EndTimePicker.Enabled = false;
            this.EndTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.EndTimePicker.Location = new System.Drawing.Point(47, 71);
            this.EndTimePicker.Name = "EndTimePicker";
            this.EndTimePicker.ShowUpDown = true;
            this.EndTimePicker.Size = new System.Drawing.Size(88, 20);
            this.EndTimePicker.TabIndex = 11;
            // 
            // StartTimePicker
            // 
            this.StartTimePicker.CustomFormat = "MMM dd, yyyy\'";
            this.StartTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.StartTimePicker.Location = new System.Drawing.Point(47, 21);
            this.StartTimePicker.Name = "StartTimePicker";
            this.StartTimePicker.ShowUpDown = true;
            this.StartTimePicker.Size = new System.Drawing.Size(88, 20);
            this.StartTimePicker.TabIndex = 10;
            this.StartTimePicker.ValueChanged += new System.EventHandler(this.StartTimePicker_ValueChanged);
            this.StartTimePicker.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RangeTextBox_KeyDown);
            // 
            // StartDatePicker
            // 
            this.StartDatePicker.CustomFormat = "MMM dd, yyyy\'";
            this.StartDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartDatePicker.Location = new System.Drawing.Point(141, 21);
            this.StartDatePicker.Name = "StartDatePicker";
            this.StartDatePicker.Size = new System.Drawing.Size(103, 20);
            this.StartDatePicker.TabIndex = 9;
            this.StartDatePicker.Value = new System.DateTime(2016, 11, 1, 11, 15, 0, 0);
            this.StartDatePicker.ValueChanged += new System.EventHandler(this.StartDatePicker_ValueChanged);
            this.StartDatePicker.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RangeTextBox_KeyDown);
            // 
            // EndDatePicker
            // 
            this.EndDatePicker.CustomFormat = "MMM dd, yyyy\'";
            this.EndDatePicker.Enabled = false;
            this.EndDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndDatePicker.Location = new System.Drawing.Point(141, 71);
            this.EndDatePicker.Name = "EndDatePicker";
            this.EndDatePicker.Size = new System.Drawing.Size(103, 20);
            this.EndDatePicker.TabIndex = 8;
            this.EndDatePicker.Value = new System.DateTime(2016, 12, 1, 11, 15, 0, 0);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(26, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "End";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "Start";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.SitesTreeView);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.textBox7);
            this.panel2.Controls.Add(this.comboBox5);
            this.panel2.Controls.Add(this.textBox6);
            this.panel2.Controls.Add(this.textBox5);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 49);
            this.panel2.MinimumSize = new System.Drawing.Size(200, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 667);
            this.panel2.TabIndex = 5;
            // 
            // SitesTreeView
            // 
            this.SitesTreeView.CheckBoxes = true;
            this.SitesTreeView.Location = new System.Drawing.Point(15, 73);
            this.SitesTreeView.Name = "SitesTreeView";
            this.SitesTreeView.Size = new System.Drawing.Size(167, 215);
            this.SitesTreeView.TabIndex = 11;
            this.SitesTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.SitesTreeView_AfterCheck);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(134, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(15, 20);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(113, 20);
            this.textBox7.TabIndex = 9;
            // 
            // comboBox5
            // 
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Items.AddRange(new object[] {
            "UCVS Default View"});
            this.comboBox5.Location = new System.Drawing.Point(15, 46);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(140, 21);
            this.comboBox5.TabIndex = 8;
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.SystemColors.Control;
            this.textBox6.Location = new System.Drawing.Point(101, 361);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(85, 20);
            this.textBox6.TabIndex = 6;
            this.textBox6.Text = "Oct. 27, 2004";
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.SystemColors.Control;
            this.textBox5.Location = new System.Drawing.Point(101, 334);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(85, 20);
            this.textBox5.TabIndex = 5;
            this.textBox5.Text = "Jan. 3, 1920";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(44, 365);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "End Date";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(41, 338);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Start Date";
            // 
            // CenterSplitContainer
            // 
            this.CenterSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CenterSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CenterSplitContainer.Location = new System.Drawing.Point(200, 49);
            this.CenterSplitContainer.Name = "CenterSplitContainer";
            this.CenterSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // CenterSplitContainer.Panel1
            // 
            this.CenterSplitContainer.Panel1.Controls.Add(this.StripChartControlPanel);
            // 
            // CenterSplitContainer.Panel2
            // 
            this.CenterSplitContainer.Panel2.Controls.Add(this.BottomPanel);
            this.CenterSplitContainer.Size = new System.Drawing.Size(739, 667);
            this.CenterSplitContainer.SplitterDistance = 440;
            this.CenterSplitContainer.SplitterWidth = 5;
            this.CenterSplitContainer.TabIndex = 9;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.dataGridView1);
            this.BottomPanel.Controls.Add(this.EventControlPanel);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BottomPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(735, 218);
            this.BottomPanel.TabIndex = 9;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EventStart,
            this.EventEnd,
            this.Instrument0,
            this.Instrument1,
            this.Instrument2,
            this.Instrument3});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 34);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(735, 184);
            this.dataGridView1.TabIndex = 2;
            // 
            // EventStart
            // 
            this.EventStart.HeaderText = "Event Start";
            this.EventStart.Name = "EventStart";
            // 
            // EventEnd
            // 
            this.EventEnd.HeaderText = "Event End";
            this.EventEnd.Name = "EventEnd";
            // 
            // Instrument0
            // 
            this.Instrument0.HeaderText = "ISR";
            this.Instrument0.Name = "Instrument0";
            // 
            // Instrument1
            // 
            this.Instrument1.HeaderText = "UDCM";
            this.Instrument1.Name = "Instrument1";
            // 
            // Instrument2
            // 
            this.Instrument2.HeaderText = "MCA";
            this.Instrument2.Name = "Instrument2";
            // 
            // Instrument3
            // 
            this.Instrument3.HeaderText = "Video";
            this.Instrument3.Name = "Instrument3";
            // 
            // EventControlPanel
            // 
            this.EventControlPanel.Controls.Add(this.button2);
            this.EventControlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.EventControlPanel.Location = new System.Drawing.Point(0, 0);
            this.EventControlPanel.Name = "EventControlPanel";
            this.EventControlPanel.Size = new System.Drawing.Size(735, 34);
            this.EventControlPanel.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(5, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "Generate Events";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // MarkerToolStripLabel
            // 
            this.MarkerToolStripLabel.Margin = new System.Windows.Forms.Padding(200, 1, 0, 2);
            this.MarkerToolStripLabel.Name = "MarkerToolStripLabel";
            this.MarkerToolStripLabel.Size = new System.Drawing.Size(99, 22);
            this.MarkerToolStripLabel.Text = "Marker Location: ";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1259, 738);
            this.Controls.Add(this.CenterSplitContainer);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.Text = "Omniscient (v. Demo 6)";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.StripChartControlPanel.ResumeLayout(false);
            this.StripChartsPanel.ResumeLayout(false);
            this.StripChartsLayoutPanel.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ChannelsLabelPanel.ResumeLayout(false);
            this.ChannelsLabelPanel.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.RightPanel.ResumeLayout(false);
            this.ChannelsPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.Chart1TabPage.ResumeLayout(false);
            this.Chart1TabPage.PerformLayout();
            this.Chart2TabPage.ResumeLayout(false);
            this.Chart2TabPage.PerformLayout();
            this.Chart3TabPage.ResumeLayout(false);
            this.Chart3TabPage.PerformLayout();
            this.Chart4TabPage.ResumeLayout(false);
            this.Chart4TabPage.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.CenterSplitContainer.Panel1.ResumeLayout(false);
            this.CenterSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CenterSplitContainer)).EndInit();
            this.CenterSplitContainer.ResumeLayout(false);
            this.BottomPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.EventControlPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel StripChartControlPanel;
        private System.Windows.Forms.HScrollBar StripChartScroll;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Panel ChannelsLabelPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel StripChartsPanel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel ChannelsPanel;
        private System.Windows.Forms.DateTimePicker EndDatePicker;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker EndTimePicker;
        private System.Windows.Forms.DateTimePicker StartTimePicker;
        private System.Windows.Forms.DateTimePicker StartDatePicker;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.SplitContainer CenterSplitContainer;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn Instrument0;
        private System.Windows.Forms.DataGridViewTextBoxColumn Instrument1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Instrument2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Instrument3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar ToolStripProgressBar;
        private System.Windows.Forms.TableLayoutPanel StripChartsLayoutPanel;
        private LiveCharts.WinForms.CartesianChart StripChart1;
        private LiveCharts.WinForms.CartesianChart StripChart0;
        private LiveCharts.WinForms.CartesianChart StripChart3;
        private LiveCharts.WinForms.CartesianChart StripChart2;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Panel EventControlPanel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem launchInspectrumToolStripMenuItem;
        private Controls.ResponsiveTreeView SitesTreeView;
        private ComboBox RangeComboBox;
        private TextBox RangeTextBox;
        private Label label6;
        private Button RangeUpdateButton;
        private TabControl tabControl1;
        private TabPage Chart1TabPage;
        private CheckBox C1LogScaleCheckBox;
        private TabPage Chart2TabPage;
        private TabPage Chart3TabPage;
        private TabPage Chart4TabPage;
        private CheckBox C2LogScaleCheckBox;
        private CheckBox C3LogScaleCheckBox;
        private CheckBox C4LogScaleCheckBox;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel MarkerToolStripLabel;
    }
}

