// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2018, Triad National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Triad National Security, LLC. This software was produced under U.S. Government contract 89233218CNA000001 for Los Alamos National Laboratory (LANL), which is operated by Triad National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR TRIAD NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Triad National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY TRIAD NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL TRIAD NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.StripChartControlPanel = new System.Windows.Forms.Panel();
            this.StripChartsPanel = new System.Windows.Forms.Panel();
            this.StripChartsLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.StripChart0 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.StripChart3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.StripChart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.StripChart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.StripChartScroll = new System.Windows.Forms.HScrollBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SiteManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EventManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.launchInspectrumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.launchInspectaclesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newXYChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shortcutsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChannelsLabelPanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.AllPanelsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.BackwardButton = new System.Windows.Forms.ToolStripButton();
            this.ForwardButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ShiftStartButton = new System.Windows.Forms.ToolStripButton();
            this.ZoomFullRangeButton = new System.Windows.Forms.ToolStripButton();
            this.ShiftEndButton = new System.Windows.Forms.ToolStripButton();
            this.ViewStartToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.MarkerToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.MouseTimeToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.RightLeftPanel = new System.Windows.Forms.Panel();
            this.CollapseRightButton = new System.Windows.Forms.Button();
            this.RightRightPanel = new System.Windows.Forms.Panel();
            this.ChannelsPanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RefreshDataButton = new System.Windows.Forms.Button();
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
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.LeftLeftPanel = new System.Windows.Forms.Panel();
            this.SitesTreeView = new Omniscient.Controls.ResponsiveTreeView();
            this.TreeImageList = new System.Windows.Forms.ImageList(this.components);
            this.BottomLeftPanel = new System.Windows.Forms.Panel();
            this.GlobalEndTextBox = new System.Windows.Forms.TextBox();
            this.GlobalStartTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.PresetsGroupBox = new System.Windows.Forms.GroupBox();
            this.PresetsComboBox = new System.Windows.Forms.ComboBox();
            this.TopLeftPanel = new System.Windows.Forms.Panel();
            this.PresetDeleteButton = new System.Windows.Forms.Button();
            this.PresetSaveButton = new System.Windows.Forms.Button();
            this.PresetNameTextBox = new System.Windows.Forms.TextBox();
            this.LeftRightPanel = new System.Windows.Forms.Panel();
            this.CollapseLeftButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.CenterSplitContainer = new System.Windows.Forms.SplitContainer();
            this.BottomTabControl = new System.Windows.Forms.TabControl();
            this.EventsTabPage = new System.Windows.Forms.TabPage();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.EventGridView = new System.Windows.Forms.DataGridView();
            this.EventGenerator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Duration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeanValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Integral = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Comment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventControlPanel = new System.Windows.Forms.Panel();
            this.RightEventControlPanel = new System.Windows.Forms.Panel();
            this.HighlightEventsCheckBox = new System.Windows.Forms.CheckBox();
            this.EventsWarningLabel = new System.Windows.Forms.Label();
            this.ExportEventsButton = new System.Windows.Forms.Button();
            this.GenerateEventsButton = new System.Windows.Forms.Button();
            this.CollapseBottomButton = new System.Windows.Forms.Button();
            this.ButtonImageList = new System.Windows.Forms.ImageList(this.components);
            this.analyzerManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StripChartControlPanel.SuspendLayout();
            this.StripChartsPanel.SuspendLayout();
            this.StripChartsLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StripChart0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StripChart3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StripChart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StripChart2)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.ChannelsLabelPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.RightLeftPanel.SuspendLayout();
            this.RightRightPanel.SuspendLayout();
            this.ChannelsPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Chart1TabPage.SuspendLayout();
            this.Chart2TabPage.SuspendLayout();
            this.Chart3TabPage.SuspendLayout();
            this.Chart4TabPage.SuspendLayout();
            this.LeftPanel.SuspendLayout();
            this.LeftLeftPanel.SuspendLayout();
            this.BottomLeftPanel.SuspendLayout();
            this.PresetsGroupBox.SuspendLayout();
            this.TopLeftPanel.SuspendLayout();
            this.LeftRightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CenterSplitContainer)).BeginInit();
            this.CenterSplitContainer.Panel1.SuspendLayout();
            this.CenterSplitContainer.Panel2.SuspendLayout();
            this.CenterSplitContainer.SuspendLayout();
            this.BottomTabControl.SuspendLayout();
            this.EventsTabPage.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EventGridView)).BeginInit();
            this.EventControlPanel.SuspendLayout();
            this.RightEventControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // StripChartControlPanel
            // 
            this.StripChartControlPanel.Controls.Add(this.StripChartsPanel);
            this.StripChartControlPanel.Controls.Add(this.StripChartScroll);
            this.StripChartControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StripChartControlPanel.Location = new System.Drawing.Point(0, 0);
            this.StripChartControlPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StripChartControlPanel.Name = "StripChartControlPanel";
            this.StripChartControlPanel.Size = new System.Drawing.Size(856, 530);
            this.StripChartControlPanel.TabIndex = 1;
            // 
            // StripChartsPanel
            // 
            this.StripChartsPanel.Controls.Add(this.StripChartsLayoutPanel);
            this.StripChartsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StripChartsPanel.Location = new System.Drawing.Point(0, 0);
            this.StripChartsPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StripChartsPanel.Name = "StripChartsPanel";
            this.StripChartsPanel.Size = new System.Drawing.Size(856, 513);
            this.StripChartsPanel.TabIndex = 2;
            // 
            // StripChartsLayoutPanel
            // 
            this.StripChartsLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.StripChartsLayoutPanel.ColumnCount = 1;
            this.StripChartsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.StripChartsLayoutPanel.Controls.Add(this.StripChart0, 0, 0);
            this.StripChartsLayoutPanel.Controls.Add(this.StripChart3, 0, 3);
            this.StripChartsLayoutPanel.Controls.Add(this.StripChart1, 0, 1);
            this.StripChartsLayoutPanel.Controls.Add(this.StripChart2, 0, 2);
            this.StripChartsLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StripChartsLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.StripChartsLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StripChartsLayoutPanel.Name = "StripChartsLayoutPanel";
            this.StripChartsLayoutPanel.RowCount = 4;
            this.StripChartsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.StripChartsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.StripChartsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.StripChartsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.StripChartsLayoutPanel.Size = new System.Drawing.Size(856, 513);
            this.StripChartsLayoutPanel.TabIndex = 1;
            // 
            // StripChart0
            // 
            chartArea1.InnerPlotPosition.Auto = false;
            chartArea1.InnerPlotPosition.Height = 63.33112F;
            chartArea1.InnerPlotPosition.Width = 89.25123F;
            chartArea1.InnerPlotPosition.X = 9.39757F;
            chartArea1.InnerPlotPosition.Y = 9.49468F;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 94F;
            chartArea1.Position.Width = 77.70886F;
            chartArea1.Position.X = 3F;
            chartArea1.Position.Y = 3F;
            this.StripChart0.ChartAreas.Add(chartArea1);
            this.StripChart0.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.StripChart0.Legends.Add(legend1);
            this.StripChart0.Location = new System.Drawing.Point(6, 6);
            this.StripChart0.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StripChart0.Name = "StripChart0";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.StripChart0.Series.Add(series1);
            this.StripChart0.Size = new System.Drawing.Size(844, 117);
            this.StripChart0.TabIndex = 5;
            this.StripChart0.Text = "chart1";
            this.StripChart0.DoubleClick += new System.EventHandler(this.StripChart_DoubleClick);
            this.StripChart0.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StripChart_MouseDown);
            this.StripChart0.MouseUp += new System.Windows.Forms.MouseEventHandler(this.StripChart_MouseUp);
            // 
            // StripChart3
            // 
            chartArea2.InnerPlotPosition.Auto = false;
            chartArea2.InnerPlotPosition.Height = 63.33112F;
            chartArea2.InnerPlotPosition.Width = 89.25123F;
            chartArea2.InnerPlotPosition.X = 9.39757F;
            chartArea2.InnerPlotPosition.Y = 9.49468F;
            chartArea2.Name = "ChartArea1";
            chartArea2.Position.Auto = false;
            chartArea2.Position.Height = 94F;
            chartArea2.Position.Width = 77.70886F;
            chartArea2.Position.X = 3F;
            chartArea2.Position.Y = 3F;
            this.StripChart3.ChartAreas.Add(chartArea2);
            this.StripChart3.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.StripChart3.Legends.Add(legend2);
            this.StripChart3.Location = new System.Drawing.Point(6, 387);
            this.StripChart3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StripChart3.Name = "StripChart3";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.StripChart3.Series.Add(series2);
            this.StripChart3.Size = new System.Drawing.Size(844, 120);
            this.StripChart3.TabIndex = 6;
            this.StripChart3.Text = "chart1";
            this.StripChart3.DoubleClick += new System.EventHandler(this.StripChart_DoubleClick);
            this.StripChart3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StripChart_MouseDown);
            this.StripChart3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.StripChart_MouseUp);
            // 
            // StripChart1
            // 
            chartArea3.InnerPlotPosition.Auto = false;
            chartArea3.InnerPlotPosition.Height = 63.33112F;
            chartArea3.InnerPlotPosition.Width = 89.25123F;
            chartArea3.InnerPlotPosition.X = 9.39757F;
            chartArea3.InnerPlotPosition.Y = 9.49468F;
            chartArea3.Name = "ChartArea1";
            chartArea3.Position.Auto = false;
            chartArea3.Position.Height = 94F;
            chartArea3.Position.Width = 77.70886F;
            chartArea3.Position.X = 3F;
            chartArea3.Position.Y = 3F;
            this.StripChart1.ChartAreas.Add(chartArea3);
            this.StripChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.StripChart1.Legends.Add(legend3);
            this.StripChart1.Location = new System.Drawing.Point(6, 133);
            this.StripChart1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StripChart1.Name = "StripChart1";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.StripChart1.Series.Add(series3);
            this.StripChart1.Size = new System.Drawing.Size(844, 117);
            this.StripChart1.TabIndex = 7;
            this.StripChart1.Text = "chart2";
            this.StripChart1.DoubleClick += new System.EventHandler(this.StripChart_DoubleClick);
            this.StripChart1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StripChart_MouseDown);
            this.StripChart1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.StripChart_MouseUp);
            // 
            // StripChart2
            // 
            chartArea4.InnerPlotPosition.Auto = false;
            chartArea4.InnerPlotPosition.Height = 63.33112F;
            chartArea4.InnerPlotPosition.Width = 89.25123F;
            chartArea4.InnerPlotPosition.X = 9.39757F;
            chartArea4.InnerPlotPosition.Y = 9.49468F;
            chartArea4.Name = "ChartArea1";
            chartArea4.Position.Auto = false;
            chartArea4.Position.Height = 94F;
            chartArea4.Position.Width = 77.70886F;
            chartArea4.Position.X = 3F;
            chartArea4.Position.Y = 3F;
            this.StripChart2.ChartAreas.Add(chartArea4);
            this.StripChart2.Dock = System.Windows.Forms.DockStyle.Fill;
            legend4.Name = "Legend1";
            this.StripChart2.Legends.Add(legend4);
            this.StripChart2.Location = new System.Drawing.Point(6, 260);
            this.StripChart2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StripChart2.Name = "StripChart2";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.StripChart2.Series.Add(series4);
            this.StripChart2.Size = new System.Drawing.Size(844, 117);
            this.StripChart2.TabIndex = 8;
            this.StripChart2.Text = "chart3";
            this.StripChart2.DoubleClick += new System.EventHandler(this.StripChart_DoubleClick);
            this.StripChart2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StripChart_MouseDown);
            this.StripChart2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.StripChart_MouseUp);
            // 
            // StripChartScroll
            // 
            this.StripChartScroll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.StripChartScroll.Location = new System.Drawing.Point(0, 513);
            this.StripChartScroll.Name = "StripChartScroll";
            this.StripChartScroll.Size = new System.Drawing.Size(856, 17);
            this.StripChartScroll.TabIndex = 1;
            this.StripChartScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.StripChartScroll_Scroll);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1672, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(171, 26);
            this.exportToolStripMenuItem.Text = "Export Data";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SiteManagerToolStripMenuItem,
            this.EventManagerToolStripMenuItem,
            this.analyzerManagerToolStripMenuItem,
            this.launchInspectrumToolStripMenuItem,
            this.launchInspectaclesToolStripMenuItem,
            this.newXYChartToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // SiteManagerToolStripMenuItem
            // 
            this.SiteManagerToolStripMenuItem.Name = "SiteManagerToolStripMenuItem";
            this.SiteManagerToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.SiteManagerToolStripMenuItem.Text = "Site Manager";
            this.SiteManagerToolStripMenuItem.Click += new System.EventHandler(this.SiteManagerToolStripMenuItem_Click);
            // 
            // EventManagerToolStripMenuItem
            // 
            this.EventManagerToolStripMenuItem.Name = "EventManagerToolStripMenuItem";
            this.EventManagerToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.EventManagerToolStripMenuItem.Text = "Event Manager";
            this.EventManagerToolStripMenuItem.Click += new System.EventHandler(this.EventManagerToolStripMenuItem_Click);
            // 
            // launchInspectrumToolStripMenuItem
            // 
            this.launchInspectrumToolStripMenuItem.Name = "launchInspectrumToolStripMenuItem";
            this.launchInspectrumToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.launchInspectrumToolStripMenuItem.Text = "Launch Inspectrum";
            this.launchInspectrumToolStripMenuItem.Click += new System.EventHandler(this.launchInspectrumToolStripMenuItem_Click);
            // 
            // launchInspectaclesToolStripMenuItem
            // 
            this.launchInspectaclesToolStripMenuItem.Name = "launchInspectaclesToolStripMenuItem";
            this.launchInspectaclesToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.launchInspectaclesToolStripMenuItem.Text = "Launch Inspectacles";
            this.launchInspectaclesToolStripMenuItem.Click += new System.EventHandler(this.launchInspectaclesToolStripMenuItem_Click);
            // 
            // newXYChartToolStripMenuItem
            // 
            this.newXYChartToolStripMenuItem.Name = "newXYChartToolStripMenuItem";
            this.newXYChartToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.newXYChartToolStripMenuItem.Text = "New XY Chart";
            this.newXYChartToolStripMenuItem.Click += new System.EventHandler(this.newXYChartToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shortcutsMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // shortcutsMenuItem
            // 
            this.shortcutsMenuItem.Name = "shortcutsMenuItem";
            this.shortcutsMenuItem.Size = new System.Drawing.Size(221, 26);
            this.shortcutsMenuItem.Text = "Keyboard Shortcuts";
            this.shortcutsMenuItem.Click += new System.EventHandler(this.shortcutsMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(221, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // ChannelsLabelPanel
            // 
            this.ChannelsLabelPanel.Controls.Add(this.label5);
            this.ChannelsLabelPanel.Controls.Add(this.label4);
            this.ChannelsLabelPanel.Controls.Add(this.label3);
            this.ChannelsLabelPanel.Controls.Add(this.label2);
            this.ChannelsLabelPanel.Controls.Add(this.label1);
            this.ChannelsLabelPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChannelsLabelPanel.Location = new System.Drawing.Point(7, 0);
            this.ChannelsLabelPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ChannelsLabelPanel.Name = "ChannelsLabelPanel";
            this.ChannelsLabelPanel.Size = new System.Drawing.Size(422, 25);
            this.ChannelsLabelPanel.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(295, 2);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(261, 2);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(228, 2);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 2);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 2);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chart:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.AllPanelsButton,
            this.toolStripSeparator3,
            this.BackwardButton,
            this.ForwardButton,
            this.toolStripSeparator2,
            this.ShiftStartButton,
            this.ZoomFullRangeButton,
            this.ShiftEndButton,
            this.ViewStartToolStripLabel,
            this.MarkerToolStripLabel,
            this.MouseTimeToolStripLabel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 28);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1672, 27);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(29, 24);
            this.toolStripButton1.Text = "Open File";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // AllPanelsButton
            // 
            this.AllPanelsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AllPanelsButton.Image = global::Omniscient.Properties.Resources.DockPanel_16x;
            this.AllPanelsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AllPanelsButton.Name = "AllPanelsButton";
            this.AllPanelsButton.Size = new System.Drawing.Size(29, 24);
            this.AllPanelsButton.Text = "Hide/Show All Panels";
            this.AllPanelsButton.Click += new System.EventHandler(this.AllPanelsButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // BackwardButton
            // 
            this.BackwardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BackwardButton.Image = ((System.Drawing.Image)(resources.GetObject("BackwardButton.Image")));
            this.BackwardButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BackwardButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BackwardButton.Name = "BackwardButton";
            this.BackwardButton.Size = new System.Drawing.Size(29, 24);
            this.BackwardButton.Text = "Go Backward";
            this.BackwardButton.Click += new System.EventHandler(this.BackwardButton_Click);
            // 
            // ForwardButton
            // 
            this.ForwardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ForwardButton.Image = ((System.Drawing.Image)(resources.GetObject("ForwardButton.Image")));
            this.ForwardButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ForwardButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ForwardButton.Name = "ForwardButton";
            this.ForwardButton.Size = new System.Drawing.Size(29, 24);
            this.ForwardButton.Text = "Go Forward";
            this.ForwardButton.Click += new System.EventHandler(this.ForwardButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // ShiftStartButton
            // 
            this.ShiftStartButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ShiftStartButton.Image = global::Omniscient.Properties.Resources.ShiftToLeft_16x;
            this.ShiftStartButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ShiftStartButton.Name = "ShiftStartButton";
            this.ShiftStartButton.Size = new System.Drawing.Size(29, 24);
            this.ShiftStartButton.Text = "Shift to Start of Range";
            this.ShiftStartButton.ToolTipText = "Shift to Start of Range";
            this.ShiftStartButton.Click += new System.EventHandler(this.ShiftStartButton_Click);
            // 
            // ZoomFullRangeButton
            // 
            this.ZoomFullRangeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomFullRangeButton.Image = global::Omniscient.Properties.Resources.ZoomToWidth_16x;
            this.ZoomFullRangeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomFullRangeButton.Name = "ZoomFullRangeButton";
            this.ZoomFullRangeButton.Size = new System.Drawing.Size(29, 24);
            this.ZoomFullRangeButton.Text = "Zoom Out To Full Range";
            this.ZoomFullRangeButton.Click += new System.EventHandler(this.ZoomFullRangeButton_Click);
            // 
            // ShiftEndButton
            // 
            this.ShiftEndButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ShiftEndButton.Image = global::Omniscient.Properties.Resources.ShiftToRight_16x;
            this.ShiftEndButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ShiftEndButton.Name = "ShiftEndButton";
            this.ShiftEndButton.Size = new System.Drawing.Size(29, 24);
            this.ShiftEndButton.Text = "Shift to End of Range";
            this.ShiftEndButton.Click += new System.EventHandler(this.ShiftEndButton_Click);
            // 
            // ViewStartToolStripLabel
            // 
            this.ViewStartToolStripLabel.AutoSize = false;
            this.ViewStartToolStripLabel.Margin = new System.Windows.Forms.Padding(70, 2, 0, 3);
            this.ViewStartToolStripLabel.Name = "ViewStartToolStripLabel";
            this.ViewStartToolStripLabel.Size = new System.Drawing.Size(200, 22);
            this.ViewStartToolStripLabel.Text = "View Start:";
            this.ViewStartToolStripLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MarkerToolStripLabel
            // 
            this.MarkerToolStripLabel.AutoSize = false;
            this.MarkerToolStripLabel.Name = "MarkerToolStripLabel";
            this.MarkerToolStripLabel.Size = new System.Drawing.Size(240, 22);
            this.MarkerToolStripLabel.Text = "Marker Location: ";
            this.MarkerToolStripLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MouseTimeToolStripLabel
            // 
            this.MouseTimeToolStripLabel.AutoSize = false;
            this.MouseTimeToolStripLabel.Margin = new System.Windows.Forms.Padding(0, 2, 0, 3);
            this.MouseTimeToolStripLabel.Name = "MouseTimeToolStripLabel";
            this.MouseTimeToolStripLabel.Size = new System.Drawing.Size(240, 22);
            this.MouseTimeToolStripLabel.Text = "Mouse Location:";
            this.MouseTimeToolStripLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel,
            this.ToolStripProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 873);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1672, 35);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ToolStripStatusLabel
            // 
            this.ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            this.ToolStripStatusLabel.Size = new System.Drawing.Size(50, 29);
            this.ToolStripStatusLabel.Text = "Ready";
            // 
            // ToolStripProgressBar
            // 
            this.ToolStripProgressBar.Name = "ToolStripProgressBar";
            this.ToolStripProgressBar.Padding = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.ToolStripProgressBar.Size = new System.Drawing.Size(100, 27);
            // 
            // RightPanel
            // 
            this.RightPanel.AutoSize = true;
            this.RightPanel.Controls.Add(this.RightLeftPanel);
            this.RightPanel.Controls.Add(this.RightRightPanel);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightPanel.Location = new System.Drawing.Point(1210, 55);
            this.RightPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RightPanel.MinimumSize = new System.Drawing.Size(13, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(462, 818);
            this.RightPanel.TabIndex = 7;
            // 
            // RightLeftPanel
            // 
            this.RightLeftPanel.Controls.Add(this.CollapseRightButton);
            this.RightLeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.RightLeftPanel.Location = new System.Drawing.Point(0, 0);
            this.RightLeftPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RightLeftPanel.Name = "RightLeftPanel";
            this.RightLeftPanel.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RightLeftPanel.Size = new System.Drawing.Size(29, 818);
            this.RightLeftPanel.TabIndex = 8;
            // 
            // CollapseRightButton
            // 
            this.CollapseRightButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollapseRightButton.Location = new System.Drawing.Point(3, 2);
            this.CollapseRightButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CollapseRightButton.Name = "CollapseRightButton";
            this.CollapseRightButton.Size = new System.Drawing.Size(23, 814);
            this.CollapseRightButton.TabIndex = 0;
            this.CollapseRightButton.Text = ">";
            this.CollapseRightButton.UseVisualStyleBackColor = true;
            this.CollapseRightButton.Click += new System.EventHandler(this.CollapseRightButton_Click);
            // 
            // RightRightPanel
            // 
            this.RightRightPanel.Controls.Add(this.ChannelsPanel);
            this.RightRightPanel.Controls.Add(this.groupBox1);
            this.RightRightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightRightPanel.Location = new System.Drawing.Point(29, 0);
            this.RightRightPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RightRightPanel.Name = "RightRightPanel";
            this.RightRightPanel.Size = new System.Drawing.Size(433, 818);
            this.RightRightPanel.TabIndex = 7;
            // 
            // ChannelsPanel
            // 
            this.ChannelsPanel.AutoScroll = true;
            this.ChannelsPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ChannelsPanel.Controls.Add(this.ChannelsLabelPanel);
            this.ChannelsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChannelsPanel.Location = new System.Drawing.Point(0, 0);
            this.ChannelsPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ChannelsPanel.Name = "ChannelsPanel";
            this.ChannelsPanel.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.ChannelsPanel.Size = new System.Drawing.Size(433, 528);
            this.ChannelsPanel.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RefreshDataButton);
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
            this.groupBox1.Location = new System.Drawing.Point(0, 528);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(433, 290);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "View";
            // 
            // RefreshDataButton
            // 
            this.RefreshDataButton.Location = new System.Drawing.Point(333, 123);
            this.RefreshDataButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RefreshDataButton.Name = "RefreshDataButton";
            this.RefreshDataButton.Size = new System.Drawing.Size(79, 60);
            this.RefreshDataButton.TabIndex = 17;
            this.RefreshDataButton.Text = "Refresh Data";
            this.RefreshDataButton.UseVisualStyleBackColor = true;
            this.RefreshDataButton.Click += new System.EventHandler(this.RefreshDataButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Chart1TabPage);
            this.tabControl1.Controls.Add(this.Chart2TabPage);
            this.tabControl1.Controls.Add(this.Chart3TabPage);
            this.tabControl1.Controls.Add(this.Chart4TabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(4, 163);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(425, 123);
            this.tabControl1.TabIndex = 16;
            // 
            // Chart1TabPage
            // 
            this.Chart1TabPage.Controls.Add(this.C1LogScaleCheckBox);
            this.Chart1TabPage.Location = new System.Drawing.Point(4, 25);
            this.Chart1TabPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Chart1TabPage.Name = "Chart1TabPage";
            this.Chart1TabPage.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Chart1TabPage.Size = new System.Drawing.Size(417, 94);
            this.Chart1TabPage.TabIndex = 0;
            this.Chart1TabPage.Text = "Chart 1";
            this.Chart1TabPage.UseVisualStyleBackColor = true;
            // 
            // C1LogScaleCheckBox
            // 
            this.C1LogScaleCheckBox.AutoSize = true;
            this.C1LogScaleCheckBox.Location = new System.Drawing.Point(13, 12);
            this.C1LogScaleCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.C1LogScaleCheckBox.Name = "C1LogScaleCheckBox";
            this.C1LogScaleCheckBox.Size = new System.Drawing.Size(90, 20);
            this.C1LogScaleCheckBox.TabIndex = 0;
            this.C1LogScaleCheckBox.Text = "Log Scale";
            this.C1LogScaleCheckBox.UseVisualStyleBackColor = true;
            this.C1LogScaleCheckBox.CheckedChanged += new System.EventHandler(this.C1LogScaleCheckBox_CheckedChanged);
            // 
            // Chart2TabPage
            // 
            this.Chart2TabPage.Controls.Add(this.C2LogScaleCheckBox);
            this.Chart2TabPage.Location = new System.Drawing.Point(4, 25);
            this.Chart2TabPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Chart2TabPage.Name = "Chart2TabPage";
            this.Chart2TabPage.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Chart2TabPage.Size = new System.Drawing.Size(417, 94);
            this.Chart2TabPage.TabIndex = 1;
            this.Chart2TabPage.Text = "Chart 2";
            this.Chart2TabPage.UseVisualStyleBackColor = true;
            // 
            // C2LogScaleCheckBox
            // 
            this.C2LogScaleCheckBox.AutoSize = true;
            this.C2LogScaleCheckBox.Location = new System.Drawing.Point(13, 12);
            this.C2LogScaleCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.C2LogScaleCheckBox.Name = "C2LogScaleCheckBox";
            this.C2LogScaleCheckBox.Size = new System.Drawing.Size(90, 20);
            this.C2LogScaleCheckBox.TabIndex = 1;
            this.C2LogScaleCheckBox.Text = "Log Scale";
            this.C2LogScaleCheckBox.UseVisualStyleBackColor = true;
            this.C2LogScaleCheckBox.CheckedChanged += new System.EventHandler(this.C2LogScaleCheckBox_CheckedChanged);
            // 
            // Chart3TabPage
            // 
            this.Chart3TabPage.Controls.Add(this.C3LogScaleCheckBox);
            this.Chart3TabPage.Location = new System.Drawing.Point(4, 25);
            this.Chart3TabPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Chart3TabPage.Name = "Chart3TabPage";
            this.Chart3TabPage.Size = new System.Drawing.Size(417, 94);
            this.Chart3TabPage.TabIndex = 2;
            this.Chart3TabPage.Text = "Chart 3";
            this.Chart3TabPage.UseVisualStyleBackColor = true;
            // 
            // C3LogScaleCheckBox
            // 
            this.C3LogScaleCheckBox.AutoSize = true;
            this.C3LogScaleCheckBox.Location = new System.Drawing.Point(13, 12);
            this.C3LogScaleCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.C3LogScaleCheckBox.Name = "C3LogScaleCheckBox";
            this.C3LogScaleCheckBox.Size = new System.Drawing.Size(90, 20);
            this.C3LogScaleCheckBox.TabIndex = 1;
            this.C3LogScaleCheckBox.Text = "Log Scale";
            this.C3LogScaleCheckBox.UseVisualStyleBackColor = true;
            this.C3LogScaleCheckBox.CheckedChanged += new System.EventHandler(this.C3LogScaleCheckBox_CheckedChanged);
            // 
            // Chart4TabPage
            // 
            this.Chart4TabPage.Controls.Add(this.C4LogScaleCheckBox);
            this.Chart4TabPage.Location = new System.Drawing.Point(4, 25);
            this.Chart4TabPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Chart4TabPage.Name = "Chart4TabPage";
            this.Chart4TabPage.Size = new System.Drawing.Size(417, 94);
            this.Chart4TabPage.TabIndex = 3;
            this.Chart4TabPage.Text = "Chart 4";
            this.Chart4TabPage.UseVisualStyleBackColor = true;
            // 
            // C4LogScaleCheckBox
            // 
            this.C4LogScaleCheckBox.AutoSize = true;
            this.C4LogScaleCheckBox.Location = new System.Drawing.Point(13, 12);
            this.C4LogScaleCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.C4LogScaleCheckBox.Name = "C4LogScaleCheckBox";
            this.C4LogScaleCheckBox.Size = new System.Drawing.Size(90, 20);
            this.C4LogScaleCheckBox.TabIndex = 1;
            this.C4LogScaleCheckBox.Text = "Log Scale";
            this.C4LogScaleCheckBox.UseVisualStyleBackColor = true;
            this.C4LogScaleCheckBox.CheckedChanged += new System.EventHandler(this.C4LogScaleCheckBox_CheckedChanged);
            // 
            // RangeUpdateButton
            // 
            this.RangeUpdateButton.Location = new System.Drawing.Point(247, 123);
            this.RangeUpdateButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RangeUpdateButton.Name = "RangeUpdateButton";
            this.RangeUpdateButton.Size = new System.Drawing.Size(79, 32);
            this.RangeUpdateButton.TabIndex = 15;
            this.RangeUpdateButton.Text = "Update";
            this.RangeUpdateButton.UseVisualStyleBackColor = true;
            this.RangeUpdateButton.Click += new System.EventHandler(this.RangeUpdateButton_Click);
            // 
            // RangeComboBox
            // 
            this.RangeComboBox.FormattingEnabled = true;
            this.RangeComboBox.Items.AddRange(new object[] {
            "Seconds",
            "Minutes",
            "Hours",
            "Days",
            "Months",
            "Years"});
            this.RangeComboBox.Location = new System.Drawing.Point(188, 57);
            this.RangeComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RangeComboBox.Name = "RangeComboBox";
            this.RangeComboBox.Size = new System.Drawing.Size(136, 24);
            this.RangeComboBox.TabIndex = 14;
            this.RangeComboBox.SelectedIndexChanged += new System.EventHandler(this.RangeComboBox_SelectedIndexChanged);
            this.RangeComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RangeTextBox_KeyDown);
            // 
            // RangeTextBox
            // 
            this.RangeTextBox.Location = new System.Drawing.Point(63, 57);
            this.RangeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RangeTextBox.Name = "RangeTextBox";
            this.RangeTextBox.Size = new System.Drawing.Size(116, 22);
            this.RangeTextBox.TabIndex = 13;
            this.RangeTextBox.Text = "1";
            this.RangeTextBox.TextChanged += new System.EventHandler(this.RangeTextBox_TextChanged);
            this.RangeTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RangeTextBox_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 62);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 16);
            this.label6.TabIndex = 12;
            this.label6.Text = "Range";
            // 
            // EndTimePicker
            // 
            this.EndTimePicker.CustomFormat = "MMM dd, yyyy\'";
            this.EndTimePicker.Enabled = false;
            this.EndTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.EndTimePicker.Location = new System.Drawing.Point(63, 87);
            this.EndTimePicker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EndTimePicker.Name = "EndTimePicker";
            this.EndTimePicker.ShowUpDown = true;
            this.EndTimePicker.Size = new System.Drawing.Size(116, 22);
            this.EndTimePicker.TabIndex = 11;
            // 
            // StartTimePicker
            // 
            this.StartTimePicker.CustomFormat = "MMM dd, yyyy\'";
            this.StartTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.StartTimePicker.Location = new System.Drawing.Point(63, 26);
            this.StartTimePicker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StartTimePicker.Name = "StartTimePicker";
            this.StartTimePicker.ShowUpDown = true;
            this.StartTimePicker.Size = new System.Drawing.Size(116, 22);
            this.StartTimePicker.TabIndex = 10;
            this.StartTimePicker.ValueChanged += new System.EventHandler(this.StartTimePicker_ValueChanged);
            this.StartTimePicker.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RangeTextBox_KeyDown);
            // 
            // StartDatePicker
            // 
            this.StartDatePicker.CustomFormat = "MMM dd, yyyy\'";
            this.StartDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartDatePicker.Location = new System.Drawing.Point(188, 26);
            this.StartDatePicker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StartDatePicker.Name = "StartDatePicker";
            this.StartDatePicker.Size = new System.Drawing.Size(136, 22);
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
            this.EndDatePicker.Location = new System.Drawing.Point(188, 87);
            this.EndDatePicker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EndDatePicker.Name = "EndDatePicker";
            this.EndDatePicker.Size = new System.Drawing.Size(136, 22);
            this.EndDatePicker.TabIndex = 8;
            this.EndDatePicker.Value = new System.DateTime(2016, 12, 1, 11, 15, 0, 0);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(23, 92);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 16);
            this.label10.TabIndex = 6;
            this.label10.Text = "End";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 31);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(34, 16);
            this.label11.TabIndex = 5;
            this.label11.Text = "Start";
            // 
            // LeftPanel
            // 
            this.LeftPanel.AutoSize = true;
            this.LeftPanel.Controls.Add(this.LeftLeftPanel);
            this.LeftPanel.Controls.Add(this.LeftRightPanel);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(0, 55);
            this.LeftPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LeftPanel.MinimumSize = new System.Drawing.Size(13, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.LeftPanel.Size = new System.Drawing.Size(350, 818);
            this.LeftPanel.TabIndex = 5;
            // 
            // LeftLeftPanel
            // 
            this.LeftLeftPanel.Controls.Add(this.SitesTreeView);
            this.LeftLeftPanel.Controls.Add(this.BottomLeftPanel);
            this.LeftLeftPanel.Controls.Add(this.PresetsGroupBox);
            this.LeftLeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftLeftPanel.Location = new System.Drawing.Point(7, 6);
            this.LeftLeftPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LeftLeftPanel.Name = "LeftLeftPanel";
            this.LeftLeftPanel.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LeftLeftPanel.Size = new System.Drawing.Size(307, 806);
            this.LeftLeftPanel.TabIndex = 15;
            // 
            // SitesTreeView
            // 
            this.SitesTreeView.CheckBoxes = true;
            this.SitesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SitesTreeView.ImageIndex = 0;
            this.SitesTreeView.ImageList = this.TreeImageList;
            this.SitesTreeView.Location = new System.Drawing.Point(3, 105);
            this.SitesTreeView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SitesTreeView.Name = "SitesTreeView";
            this.SitesTreeView.SelectedImageIndex = 0;
            this.SitesTreeView.ShowNodeToolTips = true;
            this.SitesTreeView.Size = new System.Drawing.Size(301, 618);
            this.SitesTreeView.TabIndex = 11;
            this.SitesTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.SitesTreeView_AfterCheck);
            this.SitesTreeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SitesTreeView_MouseClick);
            // 
            // TreeImageList
            // 
            this.TreeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TreeImageList.ImageStream")));
            this.TreeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.TreeImageList.Images.SetKeyName(0, "globe");
            this.TreeImageList.Images.SetKeyName(1, "cylinder");
            this.TreeImageList.Images.SetKeyName(2, "cog");
            this.TreeImageList.Images.SetKeyName(3, "gauge");
            this.TreeImageList.Images.SetKeyName(4, "binoculars");
            // 
            // BottomLeftPanel
            // 
            this.BottomLeftPanel.Controls.Add(this.GlobalEndTextBox);
            this.BottomLeftPanel.Controls.Add(this.GlobalStartTextBox);
            this.BottomLeftPanel.Controls.Add(this.label9);
            this.BottomLeftPanel.Controls.Add(this.label8);
            this.BottomLeftPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomLeftPanel.Location = new System.Drawing.Point(3, 723);
            this.BottomLeftPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BottomLeftPanel.Name = "BottomLeftPanel";
            this.BottomLeftPanel.Size = new System.Drawing.Size(301, 81);
            this.BottomLeftPanel.TabIndex = 12;
            // 
            // GlobalEndTextBox
            // 
            this.GlobalEndTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.GlobalEndTextBox.Location = new System.Drawing.Point(136, 49);
            this.GlobalEndTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GlobalEndTextBox.Name = "GlobalEndTextBox";
            this.GlobalEndTextBox.Size = new System.Drawing.Size(112, 22);
            this.GlobalEndTextBox.TabIndex = 10;
            this.GlobalEndTextBox.Text = "Oct. 27, 2004";
            // 
            // GlobalStartTextBox
            // 
            this.GlobalStartTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.GlobalStartTextBox.Location = new System.Drawing.Point(136, 12);
            this.GlobalStartTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GlobalStartTextBox.Name = "GlobalStartTextBox";
            this.GlobalStartTextBox.Size = new System.Drawing.Size(112, 22);
            this.GlobalStartTextBox.TabIndex = 9;
            this.GlobalStartTextBox.Text = "Jan. 3, 1920";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 54);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 16);
            this.label9.TabIndex = 8;
            this.label9.Text = "Data End Date";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 17);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 16);
            this.label8.TabIndex = 7;
            this.label8.Text = "Data Start Date";
            // 
            // PresetsGroupBox
            // 
            this.PresetsGroupBox.AutoSize = true;
            this.PresetsGroupBox.Controls.Add(this.PresetsComboBox);
            this.PresetsGroupBox.Controls.Add(this.TopLeftPanel);
            this.PresetsGroupBox.Controls.Add(this.PresetNameTextBox);
            this.PresetsGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.PresetsGroupBox.Location = new System.Drawing.Point(3, 2);
            this.PresetsGroupBox.Margin = new System.Windows.Forms.Padding(9, 4, 9, 4);
            this.PresetsGroupBox.Name = "PresetsGroupBox";
            this.PresetsGroupBox.Padding = new System.Windows.Forms.Padding(9, 5, 9, 5);
            this.PresetsGroupBox.Size = new System.Drawing.Size(301, 103);
            this.PresetsGroupBox.TabIndex = 14;
            this.PresetsGroupBox.TabStop = false;
            this.PresetsGroupBox.Text = "Preset Views";
            // 
            // PresetsComboBox
            // 
            this.PresetsComboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.PresetsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PresetsComboBox.FormattingEnabled = true;
            this.PresetsComboBox.Items.AddRange(new object[] {
            "UCVS Default View"});
            this.PresetsComboBox.Location = new System.Drawing.Point(9, 74);
            this.PresetsComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PresetsComboBox.Name = "PresetsComboBox";
            this.PresetsComboBox.Size = new System.Drawing.Size(283, 24);
            this.PresetsComboBox.TabIndex = 11;
            this.PresetsComboBox.SelectedIndexChanged += new System.EventHandler(this.PresetsComboBox_SelectedIndexChanged);
            // 
            // TopLeftPanel
            // 
            this.TopLeftPanel.Controls.Add(this.PresetDeleteButton);
            this.TopLeftPanel.Controls.Add(this.PresetSaveButton);
            this.TopLeftPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopLeftPanel.Location = new System.Drawing.Point(9, 42);
            this.TopLeftPanel.Margin = new System.Windows.Forms.Padding(17, 16, 17, 16);
            this.TopLeftPanel.Name = "TopLeftPanel";
            this.TopLeftPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.TopLeftPanel.Size = new System.Drawing.Size(283, 32);
            this.TopLeftPanel.TabIndex = 13;
            // 
            // PresetDeleteButton
            // 
            this.PresetDeleteButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.PresetDeleteButton.Location = new System.Drawing.Point(194, 2);
            this.PresetDeleteButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PresetDeleteButton.Name = "PresetDeleteButton";
            this.PresetDeleteButton.Size = new System.Drawing.Size(89, 28);
            this.PresetDeleteButton.TabIndex = 14;
            this.PresetDeleteButton.Text = "Delete";
            this.PresetDeleteButton.UseVisualStyleBackColor = true;
            this.PresetDeleteButton.Click += new System.EventHandler(this.PresetDeleteButton_Click);
            // 
            // PresetSaveButton
            // 
            this.PresetSaveButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.PresetSaveButton.Location = new System.Drawing.Point(0, 2);
            this.PresetSaveButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PresetSaveButton.Name = "PresetSaveButton";
            this.PresetSaveButton.Size = new System.Drawing.Size(89, 28);
            this.PresetSaveButton.TabIndex = 13;
            this.PresetSaveButton.Text = "Save";
            this.PresetSaveButton.UseVisualStyleBackColor = true;
            this.PresetSaveButton.Click += new System.EventHandler(this.PresetSaveButton_Click);
            // 
            // PresetNameTextBox
            // 
            this.PresetNameTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.PresetNameTextBox.Location = new System.Drawing.Point(9, 20);
            this.PresetNameTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PresetNameTextBox.Name = "PresetNameTextBox";
            this.PresetNameTextBox.Size = new System.Drawing.Size(283, 22);
            this.PresetNameTextBox.TabIndex = 12;
            // 
            // LeftRightPanel
            // 
            this.LeftRightPanel.Controls.Add(this.CollapseLeftButton);
            this.LeftRightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.LeftRightPanel.Location = new System.Drawing.Point(314, 6);
            this.LeftRightPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LeftRightPanel.Name = "LeftRightPanel";
            this.LeftRightPanel.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LeftRightPanel.Size = new System.Drawing.Size(29, 806);
            this.LeftRightPanel.TabIndex = 14;
            // 
            // CollapseLeftButton
            // 
            this.CollapseLeftButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollapseLeftButton.Location = new System.Drawing.Point(3, 2);
            this.CollapseLeftButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CollapseLeftButton.Name = "CollapseLeftButton";
            this.CollapseLeftButton.Size = new System.Drawing.Size(23, 802);
            this.CollapseLeftButton.TabIndex = 0;
            this.CollapseLeftButton.Text = "<";
            this.CollapseLeftButton.UseVisualStyleBackColor = true;
            this.CollapseLeftButton.Click += new System.EventHandler(this.CollapseLeftButton_Click);
            // 
            // CenterSplitContainer
            // 
            this.CenterSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CenterSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CenterSplitContainer.Location = new System.Drawing.Point(350, 55);
            this.CenterSplitContainer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CenterSplitContainer.Name = "CenterSplitContainer";
            this.CenterSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // CenterSplitContainer.Panel1
            // 
            this.CenterSplitContainer.Panel1.Controls.Add(this.StripChartControlPanel);
            // 
            // CenterSplitContainer.Panel2
            // 
            this.CenterSplitContainer.Panel2.Controls.Add(this.BottomTabControl);
            this.CenterSplitContainer.Panel2.Controls.Add(this.CollapseBottomButton);
            this.CenterSplitContainer.Panel2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CenterSplitContainer_Panel2_MouseClick);
            this.CenterSplitContainer.Size = new System.Drawing.Size(860, 818);
            this.CenterSplitContainer.SplitterDistance = 534;
            this.CenterSplitContainer.SplitterWidth = 6;
            this.CenterSplitContainer.TabIndex = 9;
            // 
            // BottomTabControl
            // 
            this.BottomTabControl.Controls.Add(this.EventsTabPage);
            this.BottomTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BottomTabControl.Location = new System.Drawing.Point(0, 25);
            this.BottomTabControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BottomTabControl.Name = "BottomTabControl";
            this.BottomTabControl.SelectedIndex = 0;
            this.BottomTabControl.Size = new System.Drawing.Size(856, 249);
            this.BottomTabControl.TabIndex = 11;
            this.BottomTabControl.SelectedIndexChanged += new System.EventHandler(this.BottomTabControl_SelectedIndexChanged);
            this.BottomTabControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BottomTabControl_MouseClick);
            // 
            // EventsTabPage
            // 
            this.EventsTabPage.Controls.Add(this.BottomPanel);
            this.EventsTabPage.Location = new System.Drawing.Point(4, 25);
            this.EventsTabPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EventsTabPage.Name = "EventsTabPage";
            this.EventsTabPage.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EventsTabPage.Size = new System.Drawing.Size(848, 220);
            this.EventsTabPage.TabIndex = 0;
            this.EventsTabPage.Text = "Events";
            this.EventsTabPage.UseVisualStyleBackColor = true;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.EventGridView);
            this.BottomPanel.Controls.Add(this.EventControlPanel);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BottomPanel.Location = new System.Drawing.Point(4, 4);
            this.BottomPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(840, 212);
            this.BottomPanel.TabIndex = 9;
            // 
            // EventGridView
            // 
            this.EventGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EventGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EventGenerator,
            this.EventStart,
            this.EventEnd,
            this.Duration,
            this.MeanValue,
            this.Integral,
            this.MaxValue,
            this.MaxTime,
            this.Comment});
            this.EventGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventGridView.Location = new System.Drawing.Point(0, 42);
            this.EventGridView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EventGridView.Name = "EventGridView";
            this.EventGridView.RowHeadersWidth = 62;
            this.EventGridView.Size = new System.Drawing.Size(840, 170);
            this.EventGridView.TabIndex = 2;
            this.EventGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.EventGridView_CellDoubleClick);
            this.EventGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.EventGridView_CellMouseClick);
            // 
            // EventGenerator
            // 
            this.EventGenerator.HeaderText = "Event Generator";
            this.EventGenerator.MinimumWidth = 8;
            this.EventGenerator.Name = "EventGenerator";
            this.EventGenerator.Width = 150;
            // 
            // EventStart
            // 
            this.EventStart.HeaderText = "Event Start";
            this.EventStart.MinimumWidth = 8;
            this.EventStart.Name = "EventStart";
            this.EventStart.Width = 150;
            // 
            // EventEnd
            // 
            this.EventEnd.HeaderText = "Event End";
            this.EventEnd.MinimumWidth = 8;
            this.EventEnd.Name = "EventEnd";
            this.EventEnd.Width = 150;
            // 
            // Duration
            // 
            this.Duration.HeaderText = "Duration";
            this.Duration.MinimumWidth = 8;
            this.Duration.Name = "Duration";
            this.Duration.Width = 150;
            // 
            // MeanValue
            // 
            this.MeanValue.HeaderText = "Mean Value";
            this.MeanValue.MinimumWidth = 8;
            this.MeanValue.Name = "MeanValue";
            this.MeanValue.Width = 80;
            // 
            // Integral
            // 
            this.Integral.HeaderText = "Integral (hr)";
            this.Integral.MinimumWidth = 8;
            this.Integral.Name = "Integral";
            this.Integral.Width = 150;
            // 
            // MaxValue
            // 
            this.MaxValue.HeaderText = "Max Value";
            this.MaxValue.MinimumWidth = 8;
            this.MaxValue.Name = "MaxValue";
            this.MaxValue.Width = 80;
            // 
            // MaxTime
            // 
            this.MaxTime.HeaderText = "Max Time";
            this.MaxTime.MinimumWidth = 8;
            this.MaxTime.Name = "MaxTime";
            this.MaxTime.Width = 150;
            // 
            // Comment
            // 
            this.Comment.HeaderText = "Comment";
            this.Comment.MinimumWidth = 8;
            this.Comment.Name = "Comment";
            this.Comment.Width = 300;
            // 
            // EventControlPanel
            // 
            this.EventControlPanel.Controls.Add(this.RightEventControlPanel);
            this.EventControlPanel.Controls.Add(this.EventsWarningLabel);
            this.EventControlPanel.Controls.Add(this.ExportEventsButton);
            this.EventControlPanel.Controls.Add(this.GenerateEventsButton);
            this.EventControlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.EventControlPanel.Location = new System.Drawing.Point(0, 0);
            this.EventControlPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EventControlPanel.Name = "EventControlPanel";
            this.EventControlPanel.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EventControlPanel.Size = new System.Drawing.Size(840, 42);
            this.EventControlPanel.TabIndex = 3;
            // 
            // RightEventControlPanel
            // 
            this.RightEventControlPanel.Controls.Add(this.HighlightEventsCheckBox);
            this.RightEventControlPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightEventControlPanel.Location = new System.Drawing.Point(587, 4);
            this.RightEventControlPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RightEventControlPanel.Name = "RightEventControlPanel";
            this.RightEventControlPanel.Size = new System.Drawing.Size(249, 34);
            this.RightEventControlPanel.TabIndex = 3;
            // 
            // HighlightEventsCheckBox
            // 
            this.HighlightEventsCheckBox.AutoSize = true;
            this.HighlightEventsCheckBox.Location = new System.Drawing.Point(108, 10);
            this.HighlightEventsCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.HighlightEventsCheckBox.Name = "HighlightEventsCheckBox";
            this.HighlightEventsCheckBox.Size = new System.Drawing.Size(125, 20);
            this.HighlightEventsCheckBox.TabIndex = 1;
            this.HighlightEventsCheckBox.Text = "Highlight Events";
            this.HighlightEventsCheckBox.UseVisualStyleBackColor = true;
            this.HighlightEventsCheckBox.CheckedChanged += new System.EventHandler(this.HighlightEventsCheckBox_CheckedChanged);
            // 
            // EventsWarningLabel
            // 
            this.EventsWarningLabel.AutoSize = true;
            this.EventsWarningLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.EventsWarningLabel.Location = new System.Drawing.Point(264, 15);
            this.EventsWarningLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.EventsWarningLabel.Name = "EventsWarningLabel";
            this.EventsWarningLabel.Size = new System.Drawing.Size(0, 16);
            this.EventsWarningLabel.TabIndex = 2;
            // 
            // ExportEventsButton
            // 
            this.ExportEventsButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.ExportEventsButton.Location = new System.Drawing.Point(143, 4);
            this.ExportEventsButton.Margin = new System.Windows.Forms.Padding(16, 4, 4, 4);
            this.ExportEventsButton.Name = "ExportEventsButton";
            this.ExportEventsButton.Size = new System.Drawing.Size(113, 34);
            this.ExportEventsButton.TabIndex = 4;
            this.ExportEventsButton.Text = "Export Events";
            this.ExportEventsButton.UseVisualStyleBackColor = true;
            this.ExportEventsButton.Click += new System.EventHandler(this.ExportEventsButton_Click);
            // 
            // GenerateEventsButton
            // 
            this.GenerateEventsButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.GenerateEventsButton.Location = new System.Drawing.Point(4, 4);
            this.GenerateEventsButton.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.GenerateEventsButton.Name = "GenerateEventsButton";
            this.GenerateEventsButton.Size = new System.Drawing.Size(139, 34);
            this.GenerateEventsButton.TabIndex = 0;
            this.GenerateEventsButton.Text = "Generate Events";
            this.GenerateEventsButton.UseVisualStyleBackColor = true;
            this.GenerateEventsButton.Click += new System.EventHandler(this.GenerateEventsButton_Click);
            // 
            // CollapseBottomButton
            // 
            this.CollapseBottomButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.CollapseBottomButton.Location = new System.Drawing.Point(0, 0);
            this.CollapseBottomButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CollapseBottomButton.Name = "CollapseBottomButton";
            this.CollapseBottomButton.Size = new System.Drawing.Size(856, 25);
            this.CollapseBottomButton.TabIndex = 10;
            this.CollapseBottomButton.Text = "V";
            this.CollapseBottomButton.UseVisualStyleBackColor = true;
            this.CollapseBottomButton.Click += new System.EventHandler(this.CollapseBottomButton_Click);
            // 
            // ButtonImageList
            // 
            this.ButtonImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ButtonImageList.ImageStream")));
            this.ButtonImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ButtonImageList.Images.SetKeyName(0, "UpArrow");
            this.ButtonImageList.Images.SetKeyName(1, "DownArrow");
            this.ButtonImageList.Images.SetKeyName(2, "Plus");
            this.ButtonImageList.Images.SetKeyName(3, "Delete");
            // 
            // analyzerManagerToolStripMenuItem
            // 
            this.analyzerManagerToolStripMenuItem.Name = "analyzerManagerToolStripMenuItem";
            this.analyzerManagerToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.analyzerManagerToolStripMenuItem.Text = "Analyzer Manager";
            this.analyzerManagerToolStripMenuItem.Click += new System.EventHandler(this.analyzerManagerToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1679, 908);
            this.Controls.Add(this.CenterSplitContainer);
            this.Controls.Add(this.LeftPanel);
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 7, 0);
            this.Text = "Omniscient (v. Demo 12)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.StripChartControlPanel.ResumeLayout(false);
            this.StripChartsPanel.ResumeLayout(false);
            this.StripChartsLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StripChart0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StripChart3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StripChart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StripChart2)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ChannelsLabelPanel.ResumeLayout(false);
            this.ChannelsLabelPanel.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.RightPanel.ResumeLayout(false);
            this.RightLeftPanel.ResumeLayout(false);
            this.RightRightPanel.ResumeLayout(false);
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
            this.LeftPanel.ResumeLayout(false);
            this.LeftLeftPanel.ResumeLayout(false);
            this.LeftLeftPanel.PerformLayout();
            this.BottomLeftPanel.ResumeLayout(false);
            this.BottomLeftPanel.PerformLayout();
            this.PresetsGroupBox.ResumeLayout(false);
            this.PresetsGroupBox.PerformLayout();
            this.TopLeftPanel.ResumeLayout(false);
            this.LeftRightPanel.ResumeLayout(false);
            this.CenterSplitContainer.Panel1.ResumeLayout(false);
            this.CenterSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CenterSplitContainer)).EndInit();
            this.CenterSplitContainer.ResumeLayout(false);
            this.BottomTabControl.ResumeLayout(false);
            this.EventsTabPage.ResumeLayout(false);
            this.BottomPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.EventGridView)).EndInit();
            this.EventControlPanel.ResumeLayout(false);
            this.EventControlPanel.PerformLayout();
            this.RightEventControlPanel.ResumeLayout(false);
            this.RightEventControlPanel.PerformLayout();
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
        private System.Windows.Forms.Panel LeftPanel;
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
        private System.Windows.Forms.DataGridView EventGridView;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar ToolStripProgressBar;
        private System.Windows.Forms.Panel EventControlPanel;
        private System.Windows.Forms.Button GenerateEventsButton;
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
        private ToolStripMenuItem SiteManagerToolStripMenuItem;
        private ToolStripLabel MouseTimeToolStripLabel;
        private Panel BottomLeftPanel;
        private Panel TopLeftPanel;
        private Button PresetSaveButton;
        private TextBox PresetNameTextBox;
        private ComboBox PresetsComboBox;
        private TextBox GlobalEndTextBox;
        private TextBox GlobalStartTextBox;
        private Label label9;
        private Label label8;
        public ImageList TreeImageList;
        private CheckBox HighlightEventsCheckBox;
        private Label EventsWarningLabel;
        private Panel RightEventControlPanel;
        private ToolStripMenuItem EventManagerToolStripMenuItem;
        public ImageList ButtonImageList;
        private ToolStripMenuItem launchInspectaclesToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private TableLayoutPanel StripChartsLayoutPanel;
        private System.Windows.Forms.DataVisualization.Charting.Chart StripChart0;
        private System.Windows.Forms.DataVisualization.Charting.Chart StripChart3;
        private System.Windows.Forms.DataVisualization.Charting.Chart StripChart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart StripChart2;
        private ToolStripButton BackwardButton;
        private ToolStripButton ForwardButton;
        private Button ExportEventsButton;
        private Panel LeftLeftPanel;
        private Panel LeftRightPanel;
        private Button CollapseLeftButton;
        private Panel RightLeftPanel;
        private Button CollapseRightButton;
        private Panel RightRightPanel;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton ZoomFullRangeButton;
        private GroupBox PresetsGroupBox;
        private Button PresetDeleteButton;
        private ToolStripButton ShiftStartButton;
        private ToolStripButton ShiftEndButton;
        private Button CollapseBottomButton;
        private ToolStripButton AllPanelsButton;
        private ToolStripSeparator toolStripSeparator3;
        private TabControl BottomTabControl;
        private TabPage EventsTabPage;
        private ToolStripMenuItem newXYChartToolStripMenuItem;
        private Button RefreshDataButton;
        private DataGridViewTextBoxColumn EventGenerator;
        private DataGridViewTextBoxColumn EventStart;
        private DataGridViewTextBoxColumn EventEnd;
        private DataGridViewTextBoxColumn Duration;
        private DataGridViewTextBoxColumn MeanValue;
        private DataGridViewTextBoxColumn Integral;
        private DataGridViewTextBoxColumn MaxValue;
        private DataGridViewTextBoxColumn MaxTime;
        private DataGridViewTextBoxColumn Comment;
        private ToolStripMenuItem shortcutsMenuItem;
        private ToolStripLabel ViewStartToolStripLabel;
        private ToolStripMenuItem analyzerManagerToolStripMenuItem;
    }
}

