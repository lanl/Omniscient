// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2020, International Atomic Energy Agency (IAEA), IAEA.org
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

using System.Windows.Media;
using System.Configuration;

using Omniscient.MainDialogs;

namespace Omniscient
{
    /// <summary>
    /// MainForm is the window that appears when opening Omniscient as is where most of the user-interaction occurs.
    /// </summary>
    public partial class MainForm : Form
    {
        ///////////////////////////////////////////////////////////////////////
        public OmniscientCore Core;

        private const int N_CHARTS = 4;
        private Chart[] charts;
        private const int MAX_HIGHLIGHTED_EVENTS = 60;

        public PresetManager presetMan;
        List<ChannelPanel> chPanels;

        private bool rangeChanged = false;
        private bool bootingUp = false;

        private bool[] logScale;
        private bool[] autoScale;
        private bool[] showLegend;
        double[] chartMaxPointValue;
        double[] chartMinPointValue;

        double[] chartMinY;
        double[] chartMaxY;

        // The following are used to show a time marker when a user clicks a chart
        Chart activeChart;
        double mouseX = 0;
        double mouseY = 0;
        double mouseDownX = 0;
        double mouseDownY = 0;
        double mouseUpX = 0;
        double mouseUpY = 0;
        DateTime mouseTime;
        private bool showMarker = false;
        private double markerValue = 0;
        IntervalOfReview manualIOR = null;
        ///////////////////////////////////////////////////////////////////////

        string appDataDirectory;
        UserSettings userSettings;

        /// <summary>
        /// MainForm constructor
        /// </summary>
        public MainForm()
        {
            // Get the App Data Directory
            appDataDirectory = Properties.Settings.Default.AppDataDirectory;
            if (appDataDirectory.ToLower() == "appdata") appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Omniscient";
            try { if (appDataDirectory != "" && !Directory.Exists(appDataDirectory)) Directory.CreateDirectory(appDataDirectory); }
            catch (Exception ex)
            {
                MessageBox.Show("Could not access application data directory. Check config file.");
            }
            Persister.AppDataDirectory = appDataDirectory;

            Core = new OmniscientCore(appDataDirectory);
            if (Core.ErrorMessage != "")
            {
                MessageBox.Show(Core.ErrorMessage);
            }
            logScale = new bool[N_CHARTS];
            autoScale = new bool[N_CHARTS];
            showLegend = new bool[N_CHARTS];
            for (int c = 0; c < N_CHARTS; c++)
            {
                logScale[c] = false;
                autoScale[c] = true;
                showLegend[c] = true;
            }
            InitializeComponent();
            Icon = Properties.Resources.OmniscientIcon;
            charts = new Chart[] { StripChart0, StripChart1, StripChart2, StripChart3 };
            Core.ViewChanged += Core_OnViewChanged;
            UpdateForwardBackButtons();
        }

        private void Core_OnViewChanged(object sender, EventArgs e)
        {
            StartDatePicker.Value = Core.ViewStart.Date;
            StartTimePicker.Value = Core.ViewStart;
            EndDatePicker.Value = Core.ViewEnd.Date;
            EndTimePicker.Value = Core.ViewEnd;

            long range = (Core.ViewEnd - Core.ViewStart).Ticks;

            if (range % TimeSpan.FromDays(1).Ticks == 0)
            {
                RangeComboBox.Text = "Days";
                RangeTextBox.Text = (range / TimeSpan.FromDays(1).Ticks).ToString();
            }
            else if (range % TimeSpan.FromHours(1).Ticks == 0)
            {
                RangeComboBox.Text = "Hours";
                RangeTextBox.Text = (range / TimeSpan.FromHours(1).Ticks).ToString();
            }
            else if (range % TimeSpan.FromMinutes(1).Ticks == 0)
            {
                RangeComboBox.Text = "Minutes";
                RangeTextBox.Text = (range / TimeSpan.FromMinutes(1).Ticks).ToString();
            }
            else
            {
                RangeComboBox.Text = "Seconds";
                RangeTextBox.Text = (range / TimeSpan.FromSeconds(1).Ticks).ToString();
            }

            UpdateRange();
        }

        /// <summary>
        /// MainForm_Load initializes several variables after the form is constructed.</summary>
        /// <remarks>Event hanlders for mouse events on the charts are handled 
        /// here because they cause errors when put in the form designer. </remarks>
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = "Omniscient - Version " + OmniscientCore.VERSION;
            bootingUp = true;

            for (int i = 0; i < N_CHARTS; i++)
            {
                Chart chart = GetChart(i);
                chart.MouseDown += new MouseEventHandler(StripChart_MouseClick);
                chart.MouseMove += new MouseEventHandler(StripChart_MouseMoved);
                chart.MouseWheel += StripChart_MouseWheel;
                chart.Paint += new PaintEventHandler(StripChart_Paint);
                chart.Resize += Chart_Resize;

                chart.SuppressExceptions = true;
                chart.Update();  // Avoids NullReferenceExceptions from MSChart when resizing the window before viewing a chart
            }

            GlobalStartTextBox.Text = Core.GlobalStart.ToString("MMM dd, yyyy");
            GlobalEndTextBox.Text = Core.GlobalEnd.ToString("MMM dd, yyyy");
            StartDatePicker.Value = Core.ViewStart;
            EndDatePicker.Value = Core.ViewEnd;
            StartTimePicker.Value = Core.ViewStart.Date;
            EndTimePicker.Value = Core.ViewEnd.Date;
            chPanels = new List<ChannelPanel>();
            
            UpdateSitesTree();
            RangeTextBox.Text = "1";
            RangeComboBox.Text = "Days";
            InitializeCharts();
            bootingUp = false;
            UpdateRange();
            UpdatesCharts();

            LoadPresets();
            SelectDefaultPreset();
            LoadUserSettings();
        }

        private void SelectDefaultPreset()
        {
            List<Preset> presets = presetMan.GetPresets();
            foreach(Preset preset in presets)
            {
                if (preset.Name == "default")
                {
                    PresetsComboBox.Text = "default";
                    break;
                }
            }
        }

        private void Chart_Resize(object sender, EventArgs e)
        {
            Chart chart = sender as Chart;
            if (!chart.Visible) return;
            if (chart.Width > 100)
            { 
                float positionX = 0;
                float positionY = 0;
                float positionWidth = 99.9F;
                float positionHeight = 99.9F;
                float plotX = 100.0F * 60.0F / chart.Width;
                float plotWidth = 99.9F - 1.5F * plotX;

                chart.SuspendLayout();
                if (chart.Height > 52)
                { 
                    float plotY = 100.0F * 20.0F / chart.Height;
                    float plotHeight = 99.9F - 2.5F * plotY;
                    chart.ChartAreas[0].Position.X = positionX;
                    chart.ChartAreas[0].Position.Y = positionY;
                    chart.ChartAreas[0].Position.Width = positionWidth;
                    chart.ChartAreas[0].Position.Height = positionHeight;
                    chart.ChartAreas[0].InnerPlotPosition.X = plotX;
                    chart.ChartAreas[0].InnerPlotPosition.Y = plotY;
                    chart.ChartAreas[0].InnerPlotPosition.Width = plotWidth;
                    chart.ChartAreas[0].InnerPlotPosition.Height = plotHeight;
                }
                chart.ResumeLayout();
            }
        }

        private void StripChart_MouseWheel(object sender, MouseEventArgs e)
        {
            // Vertical zoom
            Chart chart = (Chart)sender;
            int chartNum = (int)chart.Tag;
            if (e.Delta < 0)
            {
                if (chart.ChartAreas[0].AxisY.Maximum < 1e6 * chartMaxPointValue[chartNum])
                    chart.ChartAreas[0].AxisY.Maximum *= 1.1;
            }
            else
            {
                if (chart.ChartAreas[0].AxisY.Maximum > 1e-6 * chartMaxPointValue[chartNum])
                    chart.ChartAreas[0].AxisY.Maximum *= 0.9;
            }
        }

        public void CheckFullNodes()
        {
            foreach (TreeNode node in SitesTreeView.Nodes)
                CheckFullNodes(node);
        }

        public void CheckFullNodes(TreeNode topNode)
        {
            if (topNode.Nodes.Count == 0) return;

            bool allChecked = true;
            foreach (TreeNode child in topNode.Nodes)
            {
                CheckFullNodes(child);
                if (!child.Checked) allChecked = false;
            }
            if (allChecked && (!topNode.Checked))
                topNode.Checked = true;
        }

        public void CheckChildrenNodes(TreeNode node)
        {
            if (!node.Checked)
                node.Checked = true;
            foreach (TreeNode child in node.Nodes)
            {
                CheckChildrenNodes(child);
            }
        }

        public void UncheckChildrenNodes(TreeNode node)
        {
            if (node.Checked)
                node.Checked = false;
            foreach (TreeNode child in node.Nodes)
            {
                UncheckChildrenNodes(child);
            }
        }

        public void ClearPanels()
        {
            foreach (TreeNode child in SitesTreeView.Nodes)
            {
                UncheckChildrenNodes(child);
            }

            while (Core.ActiveInstruments.Count>0)
            {
                //SitesTreeView.Nodes.Find(activeInstruments[0].ID.ToString(), true)[0].Checked = false;
                RemoveChannelPanels(Core.ActiveInstruments[0]);
            }
        }
        
        public void LoadPresets()
        {
            string presetsPath = Path.Combine(appDataDirectory, "Presets.xml");
            presetMan = new PresetManager(presetsPath, Core.SiteManager);
            ReturnCode returnCode = presetMan.Reload();
            if (returnCode == ReturnCode.FILE_DOESNT_EXIST)
            {
                presetMan.WriteBlank();
            }
            else if (returnCode != ReturnCode.SUCCESS) MessageBox.Show("Warning: Bad trouble loading the preset manager!");
            PresetsComboBox.Items.Clear();
            foreach (Preset preset in presetMan.GetPresets())
            {
                PresetsComboBox.Items.Add(preset.Name);
            }
        }

        public void LoadUserSettings()
        {
            string settingsPath = Path.Combine(appDataDirectory, "Settings.xml");
            userSettings = new UserSettings(settingsPath);
            ReturnCode returnCode = userSettings.Reload();
            if (returnCode == ReturnCode.FILE_DOESNT_EXIST)
            {
                userSettings.WriteNew();
            }
            else if (returnCode == ReturnCode.SUCCESS)
            {
                if (!userSettings.ShowLeftPanel) CollapseLeftPanel();
                if (!userSettings.ShowRightPanel) CollapseRightPanel();
                if (!userSettings.ShowEventsPanel) CollapseBottomPanel();
            }
        }

        /// <summary>
        /// UpdateSitesTree() builds the tree view of the SiteManager.</summary>
        public void UpdateSitesTree()
        {
            SitesTreeView.Nodes.Clear();
            foreach (Site site in Core.SiteManager.GetSites())
            {
                TreeNode siteNode = new TreeNode(site.Name);
                siteNode.Name = site.ID.ToString();
                siteNode.Tag = site;
                siteNode.ImageIndex = 0;
                siteNode.SelectedImageIndex = 0;
                siteNode.ToolTipText = siteNode.Text;
                foreach (Facility fac in site.GetFacilities())
                {
                    TreeNode facNode = new TreeNode(fac.Name);
                    facNode.Name = fac.ID.ToString();
                    facNode.Tag = fac;
                    facNode.ImageIndex = 1;
                    facNode.SelectedImageIndex = 1;
                    facNode.ToolTipText = facNode.Text;
                    foreach (DetectionSystem sys in fac.GetSystems())
                    {
                        TreeNode sysNode = new TreeNode(sys.Name);
                        sysNode.Name = sys.ID.ToString();
                        sysNode.Tag = sys;
                        sysNode.ImageIndex = 2;
                        sysNode.SelectedImageIndex = 2;
                        sysNode.ToolTipText = sysNode.Text;
                        foreach (Instrument inst in sys.GetInstruments())
                        {
                            TreeNode instNode = new TreeNode(inst.Name);
                            instNode.Name = inst.ID.ToString();
                            instNode.Tag = inst;
                            instNode.ImageIndex = 3;
                            instNode.SelectedImageIndex = 3;
                            instNode.ToolTipText = instNode.Text;
                            sysNode.Nodes.Add(instNode);
                        }
                        foreach (EventGenerator eg in sys.GetEventGenerators())
                        {
                            TreeNode egNode = new TreeNode(eg.Name);
                            egNode.Name = eg.ID.ToString();
                            egNode.Tag = eg;
                            egNode.ImageIndex = 4;
                            egNode.SelectedImageIndex = 4;
                            egNode.ToolTipText = egNode.Text;
                            sysNode.Nodes.Add(egNode);
                        }
                        facNode.Nodes.Add(sysNode);
                    }
                    facNode.Expand();
                    siteNode.Nodes.Add(facNode);
                }
                siteNode.Expand();
                SitesTreeView.Nodes.Add(siteNode);
            }
        }

        /// <summary>
        /// GetChart is used to easily loop through the charts in other methods.</summary>
        private Chart GetChart(int chartNum)
        {
            switch (chartNum)
            {
                case 0:
                    return StripChart0;
                case 1:
                    return StripChart1;
                case 2:
                    return StripChart2;
                default:
                    return StripChart3;
            }
        }

        private void launchInspectrumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Inspectrum inspectrum = new Inspectrum();
            inspectrum.Show();
        }

        /// <summary>
        /// InitializeCharts is called when the form is loaded. </summary>
        private void InitializeCharts()
        {
            chartMinY = new double[N_CHARTS];
            chartMaxY = new double[N_CHARTS];
            chartMaxPointValue = new double[N_CHARTS];
            chartMinPointValue = new double[N_CHARTS];
            for (int chartNum = 0; chartNum < N_CHARTS; chartNum++)
            {
                Chart chart;
                chart = GetChart(chartNum);
                chart.AntiAliasing = AntiAliasingStyles.All;
                chart.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                chart.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                chart.Tag = chartNum;
                chart.GetToolTipText += new EventHandler<ToolTipEventArgs>(GetChartToolTip);
                // Initizialize chart values
                /*
                chart.AxisX.Clear();
                chart.AxisY.Clear();
                chart.AxisX.Add(new Axis
                {
                    LabelFormatter = val => new System.DateTime((long)val).ToString("MM DD YYY"),
                    MinValue = DateTime.Today.Ticks,
                    MaxValue = DateTime.Today.Ticks + TimeSpan.TicksPerDay,
                    //Separator = sep
                });
                */
                Series series = chart.Series[0];
                
                series.Points.Clear();
                chartMinY[chartNum] = 0;
                chartMaxY[chartNum] = 0;
                chartMaxPointValue[chartNum] = 0;
                chartMinPointValue[chartNum] = 0;
            }
        }

        private void GetChartToolTip(object sender, ToolTipEventArgs e)
        {
            Chart chart = (Chart)sender;
           
            double distSq = 1e9;
            double thisDistSq;
            double yVal = 0;
            string sName = "";
            foreach(Series series in chart.Series)
            {
                foreach(DataPoint point in series.Points)
                {
                    double deltaX = Math.Abs(e.X - chart.ChartAreas[0].AxisX.ValueToPixelPosition(point.XValue));
                    double deltaY = Math.Abs(e.Y - chart.ChartAreas[0].AxisY.ValueToPixelPosition(point.YValues[0]));
                    thisDistSq = deltaX * deltaX + deltaY * deltaY;
                    if (thisDistSq < distSq)
                    {
                        distSq = thisDistSq;
                        yVal = point.YValues[0];
                        sName = series.Name;
                    }
                }
            }
            if (distSq < 100)
                e.Text = (sName + "\n" + yVal.ToString());
        }

        /// <summary>
        /// UpdateChart is called whenever the data to be displayed on a chart changes. </summary>
        private void UpdateChart(int chartNum)
        {
            const double MAX_VALUE = (double)decimal.MaxValue;
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            Chart chart;
            chart = GetChart(chartNum);
            chart.SuspendLayout();

            // Needed for speedy loading
            DateTime start = Core.ViewStart;
              DateTime end = Core.ViewEnd;

            if (logScale[chartNum])
                chart.ChartAreas[0].AxisY.IsLogarithmic = true;
            else
                chart.ChartAreas[0].AxisY.IsLogarithmic = false;
            chart.Series.Clear();
            chartMaxPointValue[chartNum] = double.MinValue;
            chartMinPointValue[chartNum] = double.MaxValue;
            foreach (ChannelPanel chanPan in chPanels)
            {
                bool plotChan = false;
                switch (chartNum)
                {
                    case 0:
                        if (chanPan.Chart1CheckBox.Checked)
                            plotChan = true;
                        break;
                    case 1:
                        if (chanPan.Chart2CheckBox.Checked)
                            plotChan = true;
                        break;
                    case 2:
                        if (chanPan.Chart3CheckBox.Checked)
                            plotChan = true;
                        break;
                    case 3:
                        if (chanPan.Chart4CheckBox.Checked)
                            plotChan = true;
                        break;
                }
                if (plotChan)
                {
                    Channel chan = chanPan.GetChannel();

                    List<DateTime> dates = chan.GetTimeStamps(ChannelCompartment.View);
                    List<double> vals = chan.GetValues(ChannelCompartment.View);

                    Series series = new Series(chan.Name);
                    series.Points.SuspendUpdates();
                    series.ChartType = SeriesChartType.FastLine;
                    series.XValueType = ChartValueType.DateTime;
                    if (chanPan.Config.Symbol == ChannelDisplayConfig.SymbolType.Dot)
                    {
                        series.ChartType = SeriesChartType.Point;
                    }
                    else if (chanPan.Config.Symbol == ChannelDisplayConfig.SymbolType.Line)
                    {
                        series.ChartType = SeriesChartType.Line;
                    }
                    else
                    {
                        series.ChartType = SeriesChartType.Line;
                        series.MarkerStyle = MarkerStyle.Circle; 
                    }
                    series.Color = chanPan.Config.SeriesColor;
                    
                    if (chan.GetChannelType() == Channel.ChannelType.DURATION_VALUE)
                    {
                        series.BorderWidth = 3;
                        List<TimeSpan> durations = chan.GetDurations(ChannelCompartment.View);
                        if (logScale[chartNum])
                        {
                            for (int i = 0; i < dates.Count; ++i)
                            {
                                if (!(double.IsNaN(vals[i])) && vals[i] < MAX_VALUE && vals[i] > 0 && dates[i] + durations[i] >= start && dates[i] <= end)
                                {
                                    series.Points.AddXY(dates[i].ToOADate(), vals[i]);
                                    series.Points.AddXY((dates[i] + durations[i]).ToOADate(), vals[i]);
                                    series.Points.AddXY((dates[i] + durations[i].Add(TimeSpan.FromTicks(1))).ToOADate(), double.NaN);
                                    if (vals[i] > chartMaxPointValue[chartNum]) chartMaxPointValue[chartNum] = vals[i];
                                    if (vals[i] < chartMinPointValue[chartNum]) chartMinPointValue[chartNum] = vals[i];
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dates.Count; ++i) //
                            {
                                if (!(double.IsNaN(vals[i])) && vals[i] < MAX_VALUE && dates[i] + durations[i] >= start && dates[i] <= end)
                                {
                                    series.Points.AddXY(dates[i].ToOADate(), vals[i]);
                                    series.Points.AddXY((dates[i] + durations[i]).ToOADate(), vals[i]);
                                    series.Points.AddXY((dates[i] + durations[i].Add(TimeSpan.FromTicks(1))).ToOADate(), double.NaN);
                                    if (vals[i] > chartMaxPointValue[chartNum]) chartMaxPointValue[chartNum] = vals[i];
                                    if (vals[i] < chartMinPointValue[chartNum]) chartMinPointValue[chartNum] = vals[i];
                                }
                            }
                        }
                    }
                    else
                    {
                        // Load up the chart values
                        DataDecimator dataDecimator = new DataDecimator();
                        dataDecimator.TimeStamps = dates;
                        dataDecimator.Values = vals;
                        dataDecimator.Decimate(1000, start, end);
                        dates = dataDecimator.TimeStamps;
                        vals = dataDecimator.Values;

                        if (logScale[chartNum])
                        {
                            for (int i = 0; i < dates.Count; ++i) //
                            {
                                if (!(double.IsNaN(vals[i])) && vals[i] > 0 && vals[i] < MAX_VALUE && dates[i] >= start && dates[i] <= end)
                                {
                                    series.Points.AddXY(dates[i].ToOADate(), vals[i]);
                                    if (vals[i] > chartMaxPointValue[chartNum]) chartMaxPointValue[chartNum] = vals[i];
                                    if (vals[i] < chartMinPointValue[chartNum]) chartMinPointValue[chartNum] = vals[i];
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dates.Count; ++i) //
                            {
                                if (!(double.IsNaN(vals[i])) && vals[i] < MAX_VALUE && dates[i] >= start && dates[i] <= end)
                                {
                                    series.Points.AddXY(dates[i].ToOADate(), vals[i]);
                                    if (vals[i] > chartMaxPointValue[chartNum]) chartMaxPointValue[chartNum] = vals[i];
                                    if (vals[i] < chartMinPointValue[chartNum]) chartMinPointValue[chartNum] = vals[i];
                                }
                            }
                        }
                    }

                    bool alreadyExists = false;
                    foreach (Series existingSeries in chart.Series)
                    {
                        if (series.Name == existingSeries.Name)
                        {
                            alreadyExists = true;
                            break;
                        }
                    }

                    if (alreadyExists)
                    {
                        int existingCount = 2;
                        foreach (Series existingSeries in chart.Series)
                        {
                            if (existingSeries.Name.StartsWith(series.Name + " ("))
                            {
                                existingCount++;
                            }
                        }
                        series.Name = series.Name + " (" + existingCount.ToString() + ")";
                    }

                    chart.Series.Add(series);
                    series.Points.ResumeUpdates();
                }
            }
            if(autoScale[chartNum]) AutoScaleYAxes(chartNum);
            chart.Legends[0].Enabled = showLegend[chartNum];
            chart.ChartAreas[0].AxisY.Minimum = chartMinY[chartNum];
            chart.ChartAreas[0].AxisY.Maximum = chartMaxY[chartNum];
            chart.ResumeLayout();
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }

        private void AutoScaleYAxes(int chartNum)
        {
            Tuple<double, double> result = ChartingUtil.AutoRoundRange(chartMinPointValue[chartNum],
                chartMaxPointValue[chartNum],
                logScale[chartNum]);

            chartMinY[chartNum] = result.Item1;
            chartMaxY[chartNum] = result.Item2;
        }

        private void DrawSections()
        {
            DateTime start = Core.ViewStart;
            DateTime end = Core.ViewEnd;
            int eventCount = 0;
            Chart chart;
            bool overHeightWarning = false;
            for (int chartNum = 0; chartNum < N_CHARTS; chartNum++)
            {               
                chart = GetChart(chartNum);
                chart.Annotations.Clear();
                chart.Update();

                if (markerValue > start.ToOADate() && markerValue < end.ToOADate())
                {
                    VerticalLineAnnotation line = new VerticalLineAnnotation();
                    line.AxisX = chart.ChartAreas[0].AxisX;
                    line.IsInfinitive = true;
                    line.X = markerValue;
                    line.LineColor = System.Drawing.Color.FromArgb(64, 64, 64);
                    line.Width = 1;
                    chart.Annotations.Add(line);
                }
                eventCount = 0;
                if (HighlightEventsCheckBox.Checked)
                {
                    foreach (Event eve in Core.Events)
                    {
                        if (eve.StartTime < end && eve.EndTime > start && eventCount < MAX_HIGHLIGHTED_EVENTS)
                        {
                            double width = eve.EndTime.ToOADate() - eve.StartTime.ToOADate();
                            RectangleAnnotation rect = new RectangleAnnotation();
                            rect.AxisX = chart.ChartAreas[0].AxisX;
                            rect.AxisY = chart.ChartAreas[0].AxisY;
                            rect.AnchorX = eve.StartTime.ToOADate() + width/2;
                            rect.AnchorY = rect.AxisY.Minimum;
                            rect.IsSizeAlwaysRelative = false;
                            rect.LineWidth = 0;

                            rect.BackColor = System.Drawing.Color.FromArgb(128, 0, 255, 0);
                            if (eve.GetEventGenerator() is CoincidenceEG)
                            {
                                rect.BackColor = System.Drawing.Color.FromArgb(128, 243, 243, 21);
                                rect.BackHatchStyle = ChartHatchStyle.WideUpwardDiagonal;
                            }
                            //rect.Width = (eve.EndTime.Ticks - eve.StartTime.Ticks > 0) ? 
                            //    rect.AxisX.ValueToPixelPosition(eve.EndTime.ToOADate()) - rect.AxisX.ValueToPixelPosition(eve.StartTime.ToOADate()) :
                            //    rect.AxisX.ValueToPixelPosition((eve.StartTime + TimeSpan.FromSeconds(5)).ToOADate()) - rect.AxisX.ValueToPixelPosition(eve.StartTime.ToOADate());
                            rect.Width = eve.EndTime.ToOADate() - eve.StartTime.ToOADate();
                            double tempHeight = rect.AxisY.Maximum - rect.AxisY.Minimum;
                            if (tempHeight < 290000000) rect.Height = rect.AxisY.Maximum - rect.AxisY.Minimum;
                            else if(!double.IsNaN(tempHeight))
                            {
                                rect.Height = 290000000 - 1;
                                overHeightWarning = true;
                            }
                            chart.Annotations.Add(rect);
                            eventCount++;
                        }
                    }
                }
            }
            if (overHeightWarning)
            {
                EventsWarningLabel.Text = "Warning: Y-Axis range may be too large to properly display highlighted events!";
            }
            else if (eventCount == MAX_HIGHLIGHTED_EVENTS)
                EventsWarningLabel.Text = "Warning: Too many events in view. Not all are highlighted";
            else
                EventsWarningLabel.Text = "";
        }

        /// <summary>
        /// UpdateChartVisibility is called whenever the user clicks on a 
        /// checkbox in a channel panel.</summary>
        /// <remarks>Determines which charts have data to be displayed and 
        /// adjusts the size of the table layout panel accordingly.</remarks>
        private void UpdateChartVisibility()
        {
            StripChartsLayoutPanel.SuspendLayout();
            int numVisible = 0;
            for (int chartNum=0; chartNum< N_CHARTS; chartNum++)
            {
                Chart chart = GetChart(chartNum);
                if (chart.Series.Count > 0) numVisible++;
            }
            for (int chartNum = 0; chartNum < N_CHARTS; chartNum++)
            {
                Chart chart = GetChart(chartNum);
                if (chart.Series.Count > 0)
                {
                    chart.Visible = true;
                    StripChartsLayoutPanel.RowStyles[chartNum].SizeType = SizeType.Percent;
                    StripChartsLayoutPanel.RowStyles[chartNum].Height = (float)100.0 / numVisible;
                }
                else
                {
                    chart.Visible = false;
                    StripChartsLayoutPanel.RowStyles[chartNum].Height = 0;
                    chart.ChartAreas[0].AxisY.Maximum = 1;
                    chart.ChartAreas[0].AxisY.Minimum = 0;
                }
            }
            StripChartsLayoutPanel.ResumeLayout();
        }

        /// <summary>
        /// OnChannelPannelCheckChanged is directly called whenever the user 
        /// clicks on a checkbox in a channel panel.</summary>
        private void OnChannelPannelCheckChanged(object sender, EventArgs e)
        {
            CheckBox checker = (CheckBox)sender;
            UpdateChart((int)checker.Tag);
            UpdateChartVisibility();
        }

        /// <summary>
        /// UpdateGlobalStartEnd is called whenever a channel panel is either
        /// added or removed.</summary>
        /// <remarks>It updates globalStart and globalEnd and changes the
        /// start/end pickers if they are out of the global range.</remarks>
        private void UpdateGlobalStartEnd()
        {
            // Update global start and end
            GlobalStartTextBox.Text = Core.GlobalStart.ToString("MMM dd, yyyy");
            GlobalEndTextBox.Text = Core.GlobalEnd.ToString("MMM dd, yyyy");

            if (StartDatePicker.Value.Date.Add(StartTimePicker.Value.TimeOfDay) != Core.ViewStart ||
                EndDatePicker.Value.Date.Add(EndTimePicker.Value.TimeOfDay) != Core.ViewEnd)
            {
                if (TimeSpan.FromTicks(Core.ViewEnd.Ticks - Core.ViewStart.Ticks).TotalDays > 1)
                {
                    RangeTextBox.Text = "1";
                    RangeComboBox.Text = "Days";
                }
                else if (TimeSpan.FromTicks(Core.ViewEnd.Ticks - Core.ViewStart.Ticks).TotalDays > 1)
                {
                    RangeTextBox.Text = "24";
                    RangeComboBox.Text = "Hours";
                }
                else if (TimeSpan.FromTicks(Core.ViewEnd.Ticks - Core.ViewStart.Ticks).TotalHours > 1)
                {
                    RangeTextBox.Text = "60";
                    RangeComboBox.Text = "Minutes";
                }
                else
                {
                    RangeTextBox.Text = "";
                    RangeComboBox.Text = "Minutes";
                }
                StartDatePicker.Value = Core.ViewStart.Date;
                StartTimePicker.Value = Core.ViewStart;
                EndDatePicker.Value = Core.ViewEnd.Date;
                EndTimePicker.Value = Core.ViewEnd;
            }
            UpdateRange();
        }

        /// <summary>
        /// When a user selects an instrument in the tree view of the site 
        /// manager, AddChannelPanels is called to populate the appropriate
        /// channel panels.</summary>
        bool addingChannelPanels = false;
        private void AddChannelPanels(Instrument inst)
        {
            addingChannelPanels = true;
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            Core.ActivateInstrument(inst);
            System.Windows.Forms.Cursor.Current = Cursors.Default;

            ChannelsPanel.SuspendLayout();

            foreach (Channel ch in inst.GetChannels())
            {
                if (!ch.Hidden)
                {
                    ChannelPanel chanPan = new ChannelPanel(ch);
                    chanPan.Dock = DockStyle.Top;
                    chanPan.CheckChanged += new EventHandler(OnChannelPannelCheckChanged);
                    chanPan.SymbolChanged += new EventHandler(OnChannelPannelSymbolChanged);
                    ChannelsPanel.Controls.Add(chanPan);
                    chPanels.Add(chanPan);
                }
            }

            for (int i = chPanels.Count - 1; i >= 0; i--)
                chPanels[i].SendToBack();

            ChannelsLabelPanel.SendToBack();
            ChannelsPanel.ResumeLayout();
            UpdateGlobalStartEnd();
            addingChannelPanels = false;
        }

        private void OnChannelPannelSymbolChanged(object sender, EventArgs e)
        {
            if (!addingChannelPanels) UpdatesCharts();
        }

        private void RemoveChannelPanels(Instrument inst)
        {
            ChannelsPanel.SuspendLayout();
            Core.DeactivateInstrument(inst);

            List<ChannelPanel> chToGo = new List<ChannelPanel>();
            foreach (ChannelPanel chanPan in chPanels)
            {
                if (Object.ReferenceEquals(chanPan.GetChannel().GetInstrument(), inst))
                {
                    chToGo.Add(chanPan);
                }
            }
            foreach(ChannelPanel chanPan in chToGo)
            {
                ChannelsPanel.Controls.Remove(chanPan);
                chPanels.Remove(chanPan);
            }
            chToGo.Clear();
            ChannelsPanel.ResumeLayout();
            UpdateGlobalStartEnd();
            UpdatesCharts();
            UpdateChartVisibility();
        }

        private void UncheckParentNodes(TreeNode node)
        {
            if (node.Parent != null)
            {
                if (node.Parent.Checked)
                {
                    node.Parent.Checked = false;
                }
                UncheckParentNodes(node.Parent);
            }
        }

        bool checkingChildren = false;
        bool checkingParents = false;
        /// <summary>
        /// Determines which instrument was selected or deselected and
        /// either adds or removes channel panels as appropriate.</summary>
        private void SitesTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if ( e.Node.Tag is Instrument)
            {
                Instrument inst = (Instrument)e.Node.Tag;
                if (e.Node.Checked)
                    AddChannelPanels(inst);
                else
                {
                    RemoveChannelPanels(inst);
                    checkingParents = true;
                    UncheckParentNodes(e.Node);
                    checkingParents = false;
                }
            }
            else if(e.Node.Tag is EventGenerator)
            {
                EventGenerator eventGenerator = (EventGenerator)e.Node.Tag;
                if (e.Node.Checked)
                    Core.ActivateEventGenerator(eventGenerator);
                else
                {
                    Core.DeactivateEventGenerator(eventGenerator);
                    checkingParents = true;
                    UncheckParentNodes(e.Node);
                    checkingParents = false;
                }
            }
            else
            {
                if(!checkingChildren && !checkingParents)
                {
                    checkingChildren = true;
                    // Check/uncheck all children
                    if (e.Node.Checked)
                        CheckChildrenNodes(e.Node);
                    else
                    {
                        UncheckChildrenNodes(e.Node);
                        checkingParents = true;
                        UncheckParentNodes(e.Node);
                        checkingParents = false;
                    }
                    checkingChildren = false;
                }
            }
            CheckFullNodes();
        }

        private void UpdatesCharts()
        {
            StripChartsPanel.SuspendLayout();
            for (int i = 0; i < N_CHARTS; ++i)
                UpdateChart(i);
            StripChartsPanel.ResumeLayout();
        }

        private void StartDatePicker_ValueChanged(object sender, EventArgs e)
        {
            rangeChanged = true;
        }

        private void StartTimePicker_ValueChanged(object sender, EventArgs e)
        {
            rangeChanged = true;
        }

        /// <summary>
        /// Changes the viewed end date when either the start date or interval
        /// changes.</summary>
        private void UpdateEndPickers()
        {
            int range = int.Parse(RangeTextBox.Text);
            DateTime newEnd;
            StartDatePicker.Value = StartDatePicker.Value.Date;
            switch (RangeComboBox.Text)
            {
                case "Years":
                    newEnd = StartDatePicker.Value.AddYears(range);
                    break;
                case "Months":
                    newEnd = StartDatePicker.Value.AddMonths(range);
                    break;
                case "Days":
                    newEnd = StartDatePicker.Value.AddDays(range);
                    break;
                case "Hours":
                    newEnd = StartDatePicker.Value.AddHours(range);
                    break;
                case "Minutes":
                    newEnd = StartDatePicker.Value.AddMinutes(range);
                    break;
                case "Seconds":
                    newEnd = StartDatePicker.Value.AddSeconds(range);
                    break;
                default:
                    newEnd = StartDatePicker.Value.AddMinutes(range);
                    break;
            }
            newEnd = newEnd.AddTicks(StartTimePicker.Value.TimeOfDay.Ticks);
            if (StartDatePicker.Value.Date.AddTicks(StartTimePicker.Value.TimeOfDay.Ticks) != Core.ViewStart ||
                newEnd != Core.ViewEnd)
            { 
                ChangeView(StartDatePicker.Value.Date.AddTicks(StartTimePicker.Value.TimeOfDay.Ticks),
                    newEnd);
            }
            EndDatePicker.Value = Core.ViewEnd.Date;
            EndTimePicker.Value = Core.ViewEnd;
        }

        /// <summary>
        /// Changes the x-axis of the charts whenever the viewing range
        /// changes.</summary>
        private void UpdateRange()
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            if (bootingUp) return;
            if (RangeTextBox.Text == "") return;
            if (RangeTextBox.Text == "0") RangeTextBox.Text = "1";

            int range;
            try
            {
                range = int.Parse(RangeTextBox.Text);
                UpdateEndPickers();
            }
            catch
            {
                RangeTextBox.Text = "1";
                UpdateEndPickers();
            }

            StripChartsPanel.SuspendLayout();

            // Setup x-axis format
            DateTime start = Core.ViewStart;
            DateTime end = Core.ViewEnd;

            //if (start >= end) return;
            int startSeconds = DateTimeToReferenceSeconds(start);
            int endSeconds = DateTimeToReferenceSeconds(end);
            // Update Scrollbar
            StripChartScroll.Minimum =  DateTimeToReferenceSeconds(Core.GlobalStart);
            StripChartScroll.Maximum = DateTimeToReferenceSeconds(Core.GlobalEnd);
            if (startSeconds < StripChartScroll.Minimum) startSeconds = StripChartScroll.Minimum;
            if (startSeconds > StripChartScroll.Maximum) startSeconds = StripChartScroll.Maximum;
            if (endSeconds < StripChartScroll.Minimum) endSeconds = StripChartScroll.Minimum;
            if (endSeconds > StripChartScroll.Maximum) endSeconds = StripChartScroll.Maximum;
            StripChartScroll.Value = startSeconds;
            StripChartScroll.SmallChange = endSeconds - startSeconds;
            StripChartScroll.LargeChange = endSeconds - startSeconds;

            string xLabelFormat;

            DateTimeIntervalType intervalType;
            double daysInRange = TimeSpan.FromTicks(end.Ticks - start.Ticks).TotalDays;
            // Choose an appropriate x-axis label format
            if (daysInRange > 1460)
            {
                xLabelFormat = "yyyy";
                intervalType = DateTimeIntervalType.Years;
                //sep.Step = TimeSpan.FromDays(365).Ticks;
            }
            else if (daysInRange > 540)
            {
                xLabelFormat = "MM/yyyy";
                intervalType = DateTimeIntervalType.Months;
                //sep.Step = TimeSpan.FromDays(90).Ticks;
            }
            else if (daysInRange > 180)
            {
                xLabelFormat = "MM/yyyy";
                intervalType = DateTimeIntervalType.Months;
                //sep.Step = TimeSpan.FromDays(30).Ticks;
            }
            else if (daysInRange > 60)
            {
                xLabelFormat = "MMM dd";
                intervalType = DateTimeIntervalType.Days;
                //sep.Step = TimeSpan.FromDays(10).Ticks;
            }
            else if (daysInRange > 20)
            {
                xLabelFormat = "MMM dd";
                intervalType = DateTimeIntervalType.Days;
                //sep.Step = TimeSpan.FromDays(3).Ticks;
            }
            else if (daysInRange > 2)
            {
                xLabelFormat = "MMM dd";
                intervalType = DateTimeIntervalType.Days;
                //sep.Step = TimeSpan.FromDays(1).Ticks;
            }
            else
            {
                double hoursInRange = TimeSpan.FromTicks(end.Ticks - start.Ticks).TotalHours;
                if (hoursInRange > 12)
                {
                    xLabelFormat = "HH:mm";
                    intervalType = DateTimeIntervalType.Hours;
                    //sep.Step = TimeSpan.FromHours(2).Ticks;
                }
                else if (hoursInRange > 4)
                {
                    xLabelFormat = "HH:mm";
                    intervalType = DateTimeIntervalType.Hours;
                    //sep.Step = TimeSpan.FromHours(1).Ticks;
                }
                else if (hoursInRange > 1)
                {
                    xLabelFormat = "HH:mm";
                    intervalType = DateTimeIntervalType.Minutes;
                    //sep.Step = TimeSpan.FromHours(0.25).Ticks;
                }
                else
                {
                    double minInRange = TimeSpan.FromTicks(end.Ticks - start.Ticks).TotalMinutes;
                    if (minInRange > 30)
                    {
                        xLabelFormat = "HH:mm:ss";
                        intervalType = DateTimeIntervalType.Minutes;
                        //sep.Step = TimeSpan.FromMinutes(5).Ticks;
                    }
                    else if (minInRange > 15)
                    {
                        xLabelFormat = "HH:mm:ss";
                        intervalType = DateTimeIntervalType.Minutes;
                        //sep.Step = TimeSpan.FromMinutes(2).Ticks;
                    }
                    else if (minInRange > 5)
                    {
                        xLabelFormat = "HH:mm:ss";
                        intervalType = DateTimeIntervalType.Minutes;
                        //sep.Step = TimeSpan.FromMinutes(1).Ticks;
                    }
                    else if (minInRange > 1)
                    {
                        xLabelFormat = "HH:mm:ss";
                        intervalType = DateTimeIntervalType.Seconds;
                        //sep.Step = TimeSpan.FromMinutes(0.5).Ticks;
                    }
                    else
                    {
                        xLabelFormat = "HH:mm:ss";
                        intervalType = DateTimeIntervalType.Seconds;
                        //sep.Step = TimeSpan.FromMilliseconds(5000).Ticks;
                    }
                }
            }
            
            for (int i = 0; i < N_CHARTS; ++i)
            {
                Chart chart = GetChart(i);
                chart.ChartAreas[0].AxisX.LabelStyle.Format = xLabelFormat;
                chart.ChartAreas[0].AxisX.IntervalType = intervalType;
                chart.ChartAreas[0].AxisX.Minimum = start.ToOADate();
                chart.ChartAreas[0].AxisX.Maximum = end.ToOADate();
            }
            System.Windows.Forms.Cursor.Current = Cursors.Default;
            UpdatesCharts();
            DrawSections();
            StripChartsPanel.ResumeLayout();
        }

        private void RangeTextBox_TextChanged(object sender, EventArgs e)
        {
            rangeChanged = true;
        }

        private void RangeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (rangeChanged & e.KeyCode == Keys.Enter)
            {
                UpdateRange();
            }
            rangeChanged = false;
        }

        private void RangeUpdateButton_Click(object sender, EventArgs e)
        {
            if (rangeChanged)
            {
                UpdateRange();
            }
            rangeChanged = false;
        }

        private void StripChartScroll_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type == ScrollEventType.EndScroll) return;
            if ((e.NewValue + StripChartScroll.LargeChange >= StripChartScroll.Maximum) &&
                (e.NewValue > e.OldValue))
            {
                e.NewValue = StripChartScroll.Maximum - StripChartScroll.LargeChange;
            }
            ShiftView(ReferenceSecondsToDateTime(e.NewValue) - ReferenceSecondsToDateTime(e.OldValue));
        }

        /// <summary>
        /// When a user clicks on a chart, DrawMarker puts a 
        /// marker/indicator/cursor on the same spot on all of the charts.</summary>
        private void DrawMarker()
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            DrawSections();
            DateTime markerTime = DateTime.FromOADate(markerValue);
            MarkerToolStripLabel.Text = "Marker Location: " + markerTime.ToString("MMM dd, yyyy  HH:mm:ss");
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }

        private void CreateChartContextMenu()
        {
            ContextMenu chartMenu = new ContextMenu();

            // ID the active chart
            int chartNum = 0;
            for (int i = 0; i < N_CHARTS; i++)
                if (activeChart == GetChart(i)) chartNum = i;

            // Set Y-Axis range
            MenuItem ChartOptionsMenuItem = new MenuItem("Set Y-Axis Range");
            ChartOptionsMenuItem.Tag = chartNum;
            ChartOptionsMenuItem.Click += ChartOptionsMenuItem_Click;
            chartMenu.MenuItems.Add(ChartOptionsMenuItem);

            // Toggle auto scale
            if (autoScale[chartNum])
            {
                MenuItem menuItem = new MenuItem("Do not auto-scale Y-Axis");
                menuItem.Tag = chartNum;
                menuItem.Click += ToggleYAxisAutoScale;
                chartMenu.MenuItems.Add(menuItem);
            }
            else
            {
                MenuItem menuItem = new MenuItem("Auto-scale Y-Axis");
                menuItem.Tag = chartNum;
                menuItem.Click += ToggleYAxisAutoScale;
                chartMenu.MenuItems.Add(menuItem);
            }

            // Toggle legend
            if (showLegend[chartNum])
            {
                MenuItem menuItem = new MenuItem("Hide legend");
                menuItem.Tag = chartNum;
                menuItem.Click += ShowLegendMenuClick;
                chartMenu.MenuItems.Add(menuItem);
            }
            else
            {
                MenuItem menuItem = new MenuItem("Show legend");
                menuItem.Tag = chartNum;
                menuItem.Click += ShowLegendMenuClick;
                chartMenu.MenuItems.Add(menuItem);
            }

            // Toggle log scale
            if (logScale[chartNum])
            {
                MenuItem menuItem = new MenuItem("Switch to linear Y-Axis");
                menuItem.Tag = chartNum;
                menuItem.Click += ToggleYAxisLogScale;
                chartMenu.MenuItems.Add(menuItem);
            }
            else
            {
                MenuItem menuItem = new MenuItem("Switch to log Y-Axis");
                menuItem.Tag = chartNum;
                menuItem.Click += ToggleYAxisLogScale;
                chartMenu.MenuItems.Add(menuItem);
            }

            List<MenuItem> OpenExplorerMenuItems = new List<MenuItem>();

            // Determine which MCAInstruments are being plotted on the chart
            foreach (ChannelPanel chanPan in chPanels)
            {
                Channel chan = chanPan.GetChannel();
                Instrument inst = chan.GetInstrument();

                bool plotChan = false;
                switch (chartNum)
                {
                    case 0:
                        if (chanPan.Chart1CheckBox.Checked)
                            plotChan = true;
                        break;
                    case 1:
                        if (chanPan.Chart2CheckBox.Checked)
                            plotChan = true;
                        break;
                    case 2:
                        if (chanPan.Chart3CheckBox.Checked)
                            plotChan = true;
                        break;
                    case 3:
                        if (chanPan.Chart4CheckBox.Checked)
                            plotChan = true;
                        break;
                }
                if (plotChan)
                {
                    List<DateTime> timeStamps = chan.GetTimeStamps(ChannelCompartment.View);
                    List<TimeSpan> durations = null;

                    // Gather "Open In Explorer" MenuItems
                    if (chan.GetChannelType() == Channel.ChannelType.DURATION_VALUE ||
                        chan.GetChannelType() == Channel.ChannelType.GAMMA_SPECTRUM)
                    {
                        // Duration values are easy to pick off...
                        durations = chan.GetDurations(ChannelCompartment.View);
                        for (int meas = 0; meas < timeStamps.Count(); meas++)
                        {
                            if (timeStamps[meas] <= mouseTime && mouseTime <= timeStamps[meas] + durations[meas])
                            {
                                MenuItem menuItem = new MenuItem("Show " + chan.Name + " file in Explorer");
                                menuItem.Tag = chan.GetFiles(ChannelCompartment.View)[meas].FileName;
                                menuItem.Click += OpenExplorerMenuItem_Click;
                                OpenExplorerMenuItems.Add(menuItem);
                            }
                        }
                    }
                    else
                    {
                        // Point values require more effort
                        if (timeStamps.Count == 1)
                        {
                            MenuItem menuItem = new MenuItem("Show " + chan.Name + " file in Explorer");
                            menuItem.Tag = chan.GetFiles(ChannelCompartment.View)[0].FileName;
                            menuItem.Click += OpenExplorerMenuItem_Click;
                            OpenExplorerMenuItems.Add(menuItem);
                        }
                        else
                        {
                            if (mouseTime.Ticks <= (timeStamps[0].Ticks + timeStamps[1].Ticks)/2)
                            {
                                MenuItem menuItem = new MenuItem("Show " + chan.Name + " file in Explorer");
                                menuItem.Tag = chan.GetFiles(ChannelCompartment.View)[0].FileName;
                                menuItem.Click += OpenExplorerMenuItem_Click;
                                OpenExplorerMenuItems.Add(menuItem);
                            }
                            else if (mouseTime.Ticks > (timeStamps[timeStamps.Count-1].Ticks + timeStamps[timeStamps.Count - 2].Ticks) / 2)
                            {
                                MenuItem menuItem = new MenuItem("Show " + chan.Name + " file in Explorer");
                                menuItem.Tag = chan.GetFiles(ChannelCompartment.View)[timeStamps.Count - 1].FileName;
                                menuItem.Click += OpenExplorerMenuItem_Click;
                                OpenExplorerMenuItems.Add(menuItem);
                            }
                            else if (timeStamps.Count > 2)
                            {
                                long lastTick = timeStamps[0].Ticks;
                                long thisTick = timeStamps[1].Ticks;
                                long nextTick = timeStamps[2].Ticks;
                                for (int meas = 1; meas < timeStamps.Count()-1; meas++)
                                {
                                    if (mouseTime.Ticks > (thisTick+lastTick)/2 && mouseTime.Ticks <= (thisTick + nextTick)/2)
                                    {
                                        MenuItem menuItem = new MenuItem("Show " + chan.Name + " file in Explorer");
                                        menuItem.Tag = chan.GetFiles(ChannelCompartment.View)[meas].FileName;
                                        menuItem.Click += OpenExplorerMenuItem_Click;
                                        OpenExplorerMenuItems.Add(menuItem);
                                    }
                                    lastTick = thisTick;
                                    thisTick = nextTick;
                                    nextTick = timeStamps[meas + 1].Ticks;
                                }
                            }
                        }
                    }

                    if (inst is MCAInstrument || inst is DeclarationInstrument)
                    {
                        // Determine whether the user clicked within a measurement
                        for (int meas = 0; meas < timeStamps.Count(); meas++)
                        {
                            if (timeStamps[meas] <= mouseTime && mouseTime <= timeStamps[meas] + durations[meas])
                            {
                                if (chan.GetInstrument() is MCAInstrument)
                                {
                                    MenuItem menuItem = new MenuItem("View " + chan.Name + " in Inspectrum");
                                    menuItem.Tag = new Tuple<string, DateTime, string[], DateTime[]>(inst.GetChannels()[0].GetFiles(ChannelCompartment.View)[meas].FileName,
                                                                                timeStamps[meas],
                                                                                inst.GetDataFileNames(),
                                                                                inst.GetDataFileDates());   // refer to main channel - virtual channels have issues with files
                                    menuItem.Click += PlotSpectrumMenuItem_Click;
                                    chartMenu.MenuItems.Add(menuItem);
                                }
                                else if (chan.GetInstrument() is DeclarationInstrument)
                                {
                                    MenuItem menuItem = new MenuItem("View " + chan.Name + " in Declaration Editor");
                                    menuItem.Tag = chan.GetFiles(ChannelCompartment.View)[meas].FileName;
                                    menuItem.Click += DeclarationMenuItem_Click;
                                    chartMenu.MenuItems.Add(menuItem);
                                }
                            }
                        }

                        if (manualIOR != null && 
                            chan.GetInstrument() is MCAInstrument &&
                            mouseTime >= manualIOR.TimeRange.Start && 
                            mouseTime <= manualIOR.TimeRange.End)
                        {
                            MenuItem menuItem = new MenuItem("View sum of " + chan.Name + " in Inspectrum");
                            menuItem.Tag = chan;
                            menuItem.Click += PlotSummedSpectraMenuItem_Click;
                            chartMenu.MenuItems.Add(menuItem);
                        }
                    }
                    else if (inst is ImageInstrument)
                    {
                        if (timeStamps.Count == 0)
                        {
                            continue;
                        }

                        // Find closest image (within a day)
                        long distance = TimeSpan.TicksPerDay;
                        long thisDist = 0;
                        int index = -1;
                        for (int meas = 0; meas < timeStamps.Count(); meas++)
                        {
                            thisDist = Math.Abs(timeStamps[meas].Ticks - mouseTime.Ticks);
                            if (thisDist < distance)
                            {
                                index = meas;
                                distance = thisDist;
                            }
                        }
                        if (index >= 0)
                        {
                            MenuItem menuItem = new MenuItem("View " + chan.Name + " in Inspectacles");
                            menuItem.Tag = Tuple.Create(chan.GetFiles(ChannelCompartment.View)[index].FileName, timeStamps[index]);
                            menuItem.Click += ImageMenuItem_Click;
                            chartMenu.MenuItems.Add(menuItem);
                        }
                    }
                }
            }

            if (OpenExplorerMenuItems.Count > 0)
            {
                chartMenu.MenuItems.Add("-");
                foreach (MenuItem menuItem in OpenExplorerMenuItems)
                {
                    chartMenu.MenuItems.Add(menuItem);
                }
            }

            // Check for a ManualEG
            if (!(manualIOR is null))
            {
                if (mouseTime >= manualIOR.TimeRange.Start && mouseTime <= manualIOR.TimeRange.End)
                {
                    chartMenu.MenuItems.Add("-");
                    foreach (EventGenerator eg in Core.ActiveEventGenerators)
                    {
                        if (eg is ManualEG)
                        {
                            ManualEG manualEG = eg as ManualEG;
                            MenuItem menuItem = new MenuItem("Add event to " + manualEG.Name);
                            menuItem.Tag = manualEG;
                            menuItem.Click += ManualEGMenuItem_Click;
                            chartMenu.MenuItems.Add(menuItem);
                        }
                    }
                }
            }
            chartMenu.Show(activeChart, new Point((int)mouseX, (int)mouseY));
        }

        private void OpenExplorerMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = (string)(sender as MenuItem).Tag;
            System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + fileName + "\"") ;
        }

        private void PlotSummedSpectraMenuItem_Click(object sender, EventArgs e)
        {
            // Get a list of files in the range
            Channel chan = ((MenuItem)sender).Tag as Channel;
            MCAInstrument inst = chan.GetInstrument() as MCAInstrument;
            List<DateTime> timeStamps = chan.GetTimeStamps(ChannelCompartment.View);
            List<TimeSpan> durations = chan.GetDurations(ChannelCompartment.View);
            List<DataFile> dataFiles = chan.GetFiles(ChannelCompartment.View);
            List<string> files = new List<string>();
            List<Spectrum> spectra = new List<Spectrum>();
            List<Spectrum> fileSpectra = new List<Spectrum>();

            inst.ClearData(ChannelCompartment.Process);
            for (int meas = 0; meas < timeStamps.Count(); meas++)
            {
                if (timeStamps[meas] >= manualIOR.TimeRange.Start &&
                    timeStamps[meas] + durations[meas] <= manualIOR.TimeRange.End)
                {
                    if (!files.Contains(dataFiles[meas].FileName))
                    {
                        files.Add(dataFiles[meas].FileName);
                        inst.IngestFile(ChannelCompartment.Process, dataFiles[meas].FileName);
                        fileSpectra = inst.SpectrumParser.GetSpectra();

                        foreach (Spectrum spectrum in fileSpectra)
                        {
                            if (spectrum.GetStartTime() >= manualIOR.TimeRange.Start &&
                                spectrum.GetStartTime().AddSeconds(spectrum.GetRealTime()) <= manualIOR.TimeRange.End)
                            {
                                spectra.Add(spectrum);
                            }
                        }
                    }
                }
            }

            Inspectrum inspectrum = new Inspectrum();
            inspectrum.SumAndDisplaySpectra(spectra);
            inspectrum.Show();
        }

        private void ShowLegendMenuClick(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            int chartNum = (int)menuItem.Tag;
            showLegend[chartNum] = !showLegend[chartNum];
            UpdateChart(chartNum);
        }

        private void ManualEGMenuItem_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            ManualEG manualEG = menuItem.Tag as ManualEG;
            manualEG.AddEvent(manualIOR.TimeRange.Start, manualIOR.TimeRange.End, "Manual Event");
        }

        private void ImageMenuItem_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            Tuple<string, DateTime> imageData = (Tuple<string, DateTime>)menuItem.Tag;
            Inspectacles inspectacles = new Inspectacles();
            inspectacles.LoadPicture(imageData.Item1, imageData.Item2);
            inspectacles.Show();
        }

        private void ChartOptionsMenuItem_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            int chartNum = (int)menuItem.Tag;
            ChartOptionsDialog chartOptionsDialog = new ChartOptionsDialog(chartNum, chartMinY[chartNum], chartMaxY[chartNum]);
            if (chartOptionsDialog.ShowDialog() == DialogResult.OK)
            {
                chartMinY[chartNum] = chartOptionsDialog.YMin;
                chartMaxY[chartNum] = chartOptionsDialog.YMax;
                autoScale[chartNum] = false;
                UpdateChart(chartNum);
            }
        }

        private void ToggleYAxisAutoScale(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            int chartNum = (int)menuItem.Tag;
            autoScale[chartNum] = !autoScale[chartNum];
            UpdateChart(chartNum);
        }

        private void ToggleYAxisLogScale(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            int chartNum = (int)menuItem.Tag;
            logScale[chartNum] = !logScale[chartNum];
            switch(chartNum)
            {
                case 0:
                    C1LogScaleCheckBox.Checked = logScale[chartNum];
                    break;
                case 1:
                    C2LogScaleCheckBox.Checked = logScale[chartNum];
                    break;
                case 2:
                    C3LogScaleCheckBox.Checked = logScale[chartNum];
                    break;
                case 3:
                    C4LogScaleCheckBox.Checked = logScale[chartNum];
                    break;
            }
            UpdateChart(chartNum);
        }

        private void DeclarationMenuItem_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            DeclarationEditor declarationEditor = new DeclarationEditor();
            declarationEditor.LoadDECFile(menuItem.Tag.ToString());
            declarationEditor.Show();
        }

        private void PlotSpectrumMenuItem_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            Inspectrum inspectrum = new Inspectrum();
            Tuple<string, DateTime, string[], DateTime[]> tuple = menuItem.Tag as Tuple<string, DateTime, string[], DateTime[]>;
            inspectrum.EnterInstrumentMode(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
            inspectrum.Show();
        }

        /// <summary>
        /// Called when a user clicks on any of the charts.</summary>
        private void StripChart_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Chart chart = (Chart)sender;
                DateTime clickTime = DateTime.FromOADate( chart.ChartAreas[0].AxisX.PixelPositionToValue(mouseX));
                markerValue = clickTime.ToOADate();
                showMarker = true;

                DrawMarker();
            }
            else if (e.Button == MouseButtons.Right)
            {
                CreateChartContextMenu();
            }
        }

        /// <summary>
        /// Called when a user moves the mouse over any of the charts.</summary>
        public void StripChart_MouseMoved(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Chart chart = (Chart)sender;
            activeChart = chart;
            mouseX = e.X;
            mouseY = e.Y;
            if (mouseY < 0) mouseY = 0;
            try
            {
                mouseTime = DateTime.FromOADate(chart.ChartAreas[0].AxisX.PixelPositionToValue(mouseX));
                MouseTimeToolStripLabel.Text = "Mouse Location: " + mouseTime.ToString("MMM dd, yyyy  HH:mm:ss");
            }
            catch
            { }
            if (drawingXZoomBox)
            {
                for (int i=0; i<N_CHARTS; i++)
                {
                    GetChart(i).Invalidate();
                }
                if (chart.ChartAreas[0].AxisX.Maximum < mouseTime.ToOADate() ||
                    (chart.ChartAreas[0].AxisX.Minimum > mouseTime.ToOADate()))
                {
                    StripChart_MouseUp(downChart, e);
                }
            }
            if (drawingYZoomBox)
            {
                for (int i = 0; i < N_CHARTS; i++)
                {
                    GetChart(i).Invalidate();
                }
                if (chart.ChartAreas[0].AxisY.Maximum < chart.ChartAreas[0].AxisY.PixelPositionToValue(mouseY) ||
                    (chart.ChartAreas[0].AxisY.Minimum > chart.ChartAreas[0].AxisY.PixelPositionToValue(mouseY)))
                {
                    StripChart_MouseUp(downChart, e);
                }
            }
        }

        private void C1LogScaleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            logScale[0] = C1LogScaleCheckBox.Checked;
            UpdateChart(0);
        }

        private void C2LogScaleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            logScale[1] = C2LogScaleCheckBox.Checked;
            UpdateChart(1);
        }

        private void C3LogScaleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            logScale[2] = C3LogScaleCheckBox.Checked;
            UpdateChart(2);
        }

        private void C4LogScaleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            logScale[3] = C4LogScaleCheckBox.Checked;
            UpdateChart(3);
        }

        private void RangeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            rangeChanged = true;
        }

        private void SiteManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SiteManagerForm siteManForm = new SiteManagerForm(this, Core.SiteManager);
            siteManForm.ShowDialog();
        }

        private void PresetsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyPreset(PresetsComboBox.Text);
        }

        /// <summary>
        /// Called when a user selects a preset from the preset ComboBox.</summary>
        private void ApplyPreset(string presetName)
        {
            DateTime startView = Core.ViewStart;
            DateTime endView = Core.ViewEnd;

            Preset preset = presetMan.GetPresets().Single(p => p.Name == presetName);
            ClearPanels();

            foreach (Instrument inst in preset.GetActiveInstruments())
            {
                try
                {
                    SitesTreeView.Nodes.Find(inst.ID.ToString(), true)[0].Checked = true;
                }
                catch { }
            }

            foreach(Tuple<Channel, ChannelDisplayConfig> channelPreset in preset.ChannelPresets)
            {
                try
                {
                    chPanels.Single(cp => cp.GetChannel().ID == channelPreset.Item1.ID).ApplyConfiguration(channelPreset.Item2);
                }
                catch { }
            }

            foreach (EventGenerator eventGenerator in preset.GetActiveEventGenerators())
            {
                try
                {
                    SitesTreeView.Nodes.Find(eventGenerator.ID.ToString(), true)[0].Checked = true;
                }
                catch { }
            }

            // Close all charts first before making new ones
            for (int i=BottomTabControl.TabCount-1; i>0; i--)
            {
                CloseXYChartTab(BottomTabControl.TabPages[i]);
            }
            foreach (XYPanelSettings xyPanelSettings in preset.XYPanels)
            {
                XYPanel xyPanel = CreateNewXYChart(false);
                xyPanel.ApplySettings(xyPanelSettings);
            }

            PresetNameTextBox.Text = preset.Name;

            if (startView >= Core.GlobalStart && endView <= Core.GlobalEnd)
                ChangeView(startView, endView, true);
        }

        private void SavePreset(string newName)
        {
            if (newName == "") return;

            // Remove preset with the same name, if it exists
            int indexToDelete = -1;
            for (int p=0; p <presetMan.GetPresets().Count; p++)
                if (presetMan.GetPresets()[p].Name == newName)
                    indexToDelete = p;
            if (indexToDelete >= 0)
            {
                presetMan.GetPresets().RemoveAt(indexToDelete);
            }

            // Make the new preset
            Preset preset = new Preset(newName);
            foreach (Instrument inst in Core.ActiveInstruments)
                preset.GetActiveInstruments().Add(inst);
            foreach (ChannelPanel cp in chPanels)
            {
                preset.AddChannel(cp.GetChannel(), cp.Config.Copy());
            }

            //  Put all of the checked systems in the SitesTreeView in eventWatchers
            foreach (TreeNode siteNode in SitesTreeView.Nodes)
            {
                foreach (TreeNode facNode in siteNode.Nodes)
                {
                    foreach (TreeNode sysNode in facNode.Nodes)
                    {
                        foreach (TreeNode node in sysNode.Nodes)
                        {
                            if (node.Tag is EventGenerator && node.Checked)
                            {
                                preset.GetActiveEventGenerators().Add((EventGenerator)node.Tag);
                            }
                        }
                    }
                }
            }

            // Get XYCharts
            for (int t = 1; t < BottomTabControl.TabCount; t++)
                preset.XYPanels.Add((BottomTabControl.TabPages[t].Tag as XYPanel).GetSettings());

            if (indexToDelete >= 0)
            {
                presetMan.GetPresets().Insert(indexToDelete, preset);
            }
            else
            {
                presetMan.GetPresets().Add(preset);
                PresetsComboBox.Items.Add(preset.Name);
            }
            PresetsComboBox.SelectedItem = preset.Name;

            // Save the new preset to xml
            presetMan.Save();
            MessageBox.Show("Saved preset \"" + preset.Name + "\"", "Save Preset");
        }

        private void PresetSaveButton_Click(object sender, EventArgs e)
        {
            SavePreset(PresetNameTextBox.Text);
        }

        private void GenerateEventsButton_Click(object sender, EventArgs e)
        {
            GenerateEventsDialog dialog = new GenerateEventsDialog(Core);
            if (dialog.ShowDialog() != DialogResult.OK) return;

            GenerateEvents(dialog.StartTime, dialog.EndTime);
        }

        private void GenerateEvents(DateTime start, DateTime end)
        {
            Core.GenerateEvents(start, end);
            DisplayEvents();
        }

        private void DisplayEvents()
        {
            EventGridView.Rows.Clear();
            for (int i = 0; i < Core.Events.Count(); i++)
            {
                EventGridView.Rows.Add(
                    Core.Events[i].GetEventGenerator().Name,
                    Core.Events[i].StartTime.ToString("MM/dd/yy HH:mm:ss"),
                    Core.Events[i].EndTime.ToString("MM/dd/yy HH:mm:ss"),
                    Core.Events[i].GetDuration().TotalSeconds,
                    Core.Events[i].MaxValue,
                    Core.Events[i].MaxTime.ToString("MM/dd/yy HH:mm:ss"),
                    Core.Events[i].Comment);
                EventGridView.Rows[i].Tag = Core.Events[i];
            }
            if (HighlightEventsCheckBox.Checked)
                DrawSections();
            ToolStripStatusLabel.Text = Core.Events.Count.ToString() + " Events Generated";
        }

        private void HighlightEventsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DrawSections();
        }

        private void EventManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventManagerForm eventManForm = new EventManagerForm(this, Core.SiteManager);
            eventManForm.ShowDialog();
        }

        private void launchInspectaclesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Inspectacles inspectacles = new Inspectacles();
            inspectacles.Show();
        }

        private void EventGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (EventGridView.Rows[e.RowIndex].Tag is Event)
            {
                EventViewerForm eventViewerForm = new EventViewerForm((Event)EventGridView.Rows[e.RowIndex].Tag);
                eventViewerForm.Show();
            }
        }

        private void declarationEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeclarationEditor declarationEditor = new DeclarationEditor();
            declarationEditor.Show();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Core.ActiveInstruments.Count == 0)
            {
                MessageBox.Show("No active Instruments for exporting data!");
                return;
            }
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;

            ExportDataDialog dialog = new ExportDataDialog(Core.ActiveInstruments, Core.GlobalStart, Core.GlobalEnd,
                StartDatePicker.Value, StartTimePicker.Value, EndDatePicker.Value, EndTimePicker.Value);
            dialog.ShowDialog();
            return;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox(OmniscientCore.VERSION);
            aboutBox.ShowDialog();
        }

        private int DateTimeToReferenceSeconds(DateTime dateTime)
        {
            DateTime reference = new DateTime(2000, 1, 1, 0, 0, 0);
            return (int)((dateTime.Ticks - reference.Ticks)/1e7);
        }

        private DateTime ReferenceSecondsToDateTime(int time)
        {
            DateTime reference = new DateTime(2000, 1, 1, 0, 0, 0);
            return reference.AddSeconds(time);
        }

        private void StripChart_DoubleClick(object sender, EventArgs e)
        {
            Chart chart = (Chart)sender;
            using (MemoryStream ms = new MemoryStream())
            {
                chart.SaveImage(ms, ChartImageFormat.Bmp);
                Bitmap bm = new Bitmap(ms);
                Clipboard.SetImage(bm);
            }
        }

        Chart downChart;
        bool drawingXZoomBox = false;
        bool drawingYZoomBox = false;
        private void StripChart_MouseDown(object sender, MouseEventArgs e)
        {
            downChart = (Chart)sender;
            mouseDownX = downChart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
            mouseDownY = downChart.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);
            if (e.Button == MouseButtons.Left &&
                mouseDownX >= downChart.ChartAreas[0].AxisX.Minimum &&
                mouseDownX <= downChart.ChartAreas[0].AxisX.Maximum)
            {
                drawingXZoomBox = true;
            }
            else if (e.Button == MouseButtons.Left &&
                mouseDownX < downChart.ChartAreas[0].AxisX.Minimum &&
                mouseDownY >= downChart.ChartAreas[0].AxisY.Minimum &&
                mouseDownY <= downChart.ChartAreas[0].AxisY.Maximum)
            {
                drawingYZoomBox = true;
            }
        }

        string boxData;
        private void CalculateHighlightBoxValues()
        {
            boxData = "";
            int chartNum = (int)downChart.Tag;
            DateTime start = manualIOR.TimeRange.Start;
            DateTime end = manualIOR.TimeRange.End;
            foreach (ChannelPanel chanPan in chPanels)
            {
                bool plotChan = false;
                switch (chartNum)
                {
                    case 0:
                        if (chanPan.Chart1CheckBox.Checked)
                            plotChan = true;
                        break;
                    case 1:
                        if (chanPan.Chart2CheckBox.Checked)
                            plotChan = true;
                        break;
                    case 2:
                        if (chanPan.Chart3CheckBox.Checked)
                            plotChan = true;
                        break;
                    case 3:
                        if (chanPan.Chart4CheckBox.Checked)
                            plotChan = true;
                        break;    
                }
                if(plotChan)
                {
                    Channel ch = chanPan.GetChannel();
                    boxData += "--" + ch.Name + "--\n";
                    boxData += "μ:\t" + ch.GetAverage(ChannelCompartment.View, start, end).ToString("G6") + "\n";
                    boxData += "σ:\t" + ch.GetStandardDeviation(ChannelCompartment.View, start, end).ToString("G6") + "\n";
                    boxData += "Max:\t" + ch.GetMax(ChannelCompartment.View, start, end).ToString("G6") + "\n";
                    boxData += "Min:\t" + ch.GetMin(ChannelCompartment.View, start, end).ToString("G6") + "\n";
					boxData += "\n";
                }
            }
        }

        private void StripChart_MouseUp(object sender, MouseEventArgs e)
        {
            if (!drawingXZoomBox && !drawingYZoomBox) return;

            Chart chart = (Chart)sender;
            if (chart != downChart) return;
            int chartNum = (int)chart.Tag;

            if (drawingYZoomBox)
            {
                double mouseY = e.Y > 0 ? chart.ChartAreas[0].AxisY.PixelPositionToValue(e.Y) : 0;
                double newMin;
                double newMax;
                if (mouseY > mouseDownY)
                {
                    newMin = mouseDownY;
                    newMax = mouseY;
                }
                else
                {
                    double mid = (chart.ChartAreas[0].AxisY.Maximum + chart.ChartAreas[0].AxisY.Minimum)/2;
                    double radius = mid - chart.ChartAreas[0].AxisY.Minimum;
                    newMin = mid - 2 * radius;
                    newMax = mid + 2 * radius;
                }
                if(!controlPressed)
                { 
                    Tuple<double, double> rounded = ChartingUtil.AutoRoundRange(newMin, newMax, logScale[chartNum]);
                    newMin = rounded.Item1;
                    newMax = rounded.Item2;
                }
                chartMinY[chartNum] = newMin;
                chartMaxY[chartNum] = newMax;
                autoScale[chartNum] = false;
                drawingYZoomBox = false;
                UpdateChart(chartNum);
                return;
            }


            mouseUpX = 0;
            try
            {
                mouseUpX = chart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
            }
            catch
            {
                return;
            }
            drawingXZoomBox = false;
            drawingYZoomBox = false;
            
            double mouseDelta = mouseUpX - mouseDownX;
            double range = chart.ChartAreas[0].AxisX.Maximum - chart.ChartAreas[0].AxisX.Minimum;

            if (e.Button == MouseButtons.Right)
            {
                // Drag Range
                ShiftView(DateTime.FromOADate(chart.ChartAreas[0].AxisX.Minimum) -
                    DateTime.FromOADate(chart.ChartAreas[0].AxisX.Minimum+mouseDelta));
            }
            else if (mouseDelta/range < -0.02 && !controlPressed)
            {
                // Zoom out
                ChangeView(DateTime.FromOADate(chart.ChartAreas[0].AxisX.Minimum - range / 2),
                    DateTime.FromOADate(chart.ChartAreas[0].AxisX.Maximum + range / 2));
            }
            else if (mouseDelta / range > 0.02 && !controlPressed)
            {
                // Zoom in
                ChangeView(DateTime.FromOADate(mouseDownX),
                    DateTime.FromOADate(mouseUpX));
            }
            else if (!controlPressed || 
                ((mouseDelta / range < 0.02) && (mouseDelta/range > -0.02)))
            {
                // This is nothing - just do nothing
                drawingXZoomBox = false;
                return;
            }
            else
            {
                // Draw statistics box
                CreateManualIOR();
                return;
            }
        }

        Chart manualIORChart = null;
        public void CreateManualIOR()
        {
            DateTime start = DateTime.FromOADate(mouseDownX);
            DateTime end = DateTime.FromOADate(mouseUpX);
            if (end < start)
            {
                DateTime dateTimesScrap = start;
                start = end;
                end = dateTimesScrap;
            }
            manualIOR = new IntervalOfReview(start, end);
            manualIORChart = downChart;

            // Draw statistics box
            CalculateHighlightBoxValues();

            foreach (Chart chart in charts)
            {
                chart.Invalidate();
            }
        }

        public void StripChart_Paint(object sender, PaintEventArgs e)
        {
            Chart chart = (Chart)sender;
            int chartNum = (int)chart.Tag;
            Axis X = chart.ChartAreas[0].AxisX;
            Axis Y = chart.ChartAreas[0].AxisY;
            if (drawingXZoomBox)
            {
                int xStart;
                int xNow;
                try
                { 
                    xStart = (int)X.ValueToPixelPosition(mouseDownX);
                    xNow = (int)X.ValueToPixelPosition(mouseTime.ToOADate());
                }
                catch
                {
                    // Sometimes MSChart bugs out on ValueToPixelPosition (NullReferenceException)
                    // This catches that and says "fine, I won't draw my fancy chart pictures this time but please don't crash"
                    return;         
                }
                // Don't go out of range
                int xMin = Math.Min(xStart, xNow);
                if (xMin < X.ValueToPixelPosition(X.Minimum)) xMin = (int) X.ValueToPixelPosition(X.Minimum);
                int xMax = Math.Max(xStart, xNow);
                if (xMax >= X.ValueToPixelPosition(X.Maximum)) xMax = (int)X.ValueToPixelPosition(X.Maximum)-1;

                e.Graphics.DrawRectangle(Pens.Gray, xMin, (int)Y.ValueToPixelPosition(Y.Maximum),
                    xMax-xMin, (int)Y.ValueToPixelPosition(Y.Minimum) - (int)Y.ValueToPixelPosition(Y.Maximum));
            }
            if (drawingYZoomBox && chartNum == (int)downChart.Tag)
            {
                int yStart = (int)Y.ValueToPixelPosition(mouseDownY);
                int yNow = (int)mouseY;

                e.Graphics.DrawRectangle(Pens.Gray, (int)X.ValueToPixelPosition(X.Minimum), Math.Min(yStart, yNow),
                    (int)X.ValueToPixelPosition(X.Maximum) - (int)X.ValueToPixelPosition(X.Minimum), Math.Abs(yStart - yNow));
            }
            if (!(manualIOR is null) && chart == manualIORChart)
            {
                e.Graphics.DrawRectangle(Pens.Gray, 
                    (float)X.ValueToPixelPosition(manualIOR.TimeRange.Start.ToOADate()), 
                    (int)Y.ValueToPixelPosition(Y.Maximum),
                    (float) (X.ValueToPixelPosition(manualIOR.TimeRange.End.ToOADate()) - X.ValueToPixelPosition(manualIOR.TimeRange.Start.ToOADate())), 
                    (int)Y.ValueToPixelPosition(Y.Minimum) - (int)Y.ValueToPixelPosition(Y.Maximum));
                e.Graphics.DrawString(boxData, SystemFonts.DefaultFont, SystemBrushes.InfoText, 
                    new Point((int)X.ValueToPixelPosition(manualIOR.TimeRange.Start.ToOADate()) + 5, (int)Y.ValueToPixelPosition(Y.Maximum) + 5));
            }
        }
        
        bool controlPressed = false;
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                controlPressed = true;
            }
            if (e.KeyCode == Keys.Delete)
            {
                if (!(manualIOR is null) && (markerValue >= manualIOR.TimeRange.Start.ToOADate()) && (markerValue <= manualIOR.TimeRange.End.ToOADate()))
                {
                    manualIOR = null;
                    foreach(Chart chart in charts)
                    {
                        chart.Invalidate();
                    }
                }
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                controlPressed = false;
            }
            if (e.KeyCode == Keys.Left && controlPressed)
            {
                GoBack();
            }
            if (e.KeyCode == Keys.Right && controlPressed)
            {
                GoForward();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Core.Shutdown();
        }

        private void EventGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Event eve = EventGridView.Rows[e.RowIndex].Tag as Event;
                if (eve is null) return;
                ContextMenu menu = new ContextMenu();
                // Export data range
                MenuItem menuItem = new MenuItem("Export data during event");
                menuItem.Tag = eve;
                menuItem.Click += ExportDataMenuItem_Click;
                menu.MenuItems.Add(menuItem);

                // Remove from manual event generator
                if (eve.GetEventGenerator() is ManualEG)
                {
                    menuItem = new MenuItem("Remove event from " + eve.GetEventGenerator().Name);
                    menuItem.Tag = eve;
                    menuItem.Click += RemoveEventMenuItem_Click;
                    menu.MenuItems.Add(menuItem);
                }

                menuItem = new MenuItem("View event");
                menuItem.Tag = eve;
                menuItem.Click += ViewEvent_Click;
                menu.MenuItems.Add(menuItem);

                Point location = EventGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location;
                location.X += e.X;
                location.Y += e.Y;
                menu.Show(EventGridView,location);
            }
        }

        private void ViewEvent_Click(object sender, EventArgs e)
        {
            Event eve = (sender as MenuItem).Tag as Event;
            DateTime start = eve.StartTime;
            DateTime end = eve.EndTime;
            DateTime eventMid = eve.StartTime.AddTicks((end - start).Ticks / 2);
            TimeSpan range;
            if (end- start < Core.ViewEnd - Core.ViewStart)
            {
                range = Core.ViewEnd - Core.ViewStart;
            }
            else
            {
                range = TimeSpan.FromTicks((end - start).Ticks*2);
            }

            ChangeView(eventMid.AddTicks(-range.Ticks / 2), eventMid.AddTicks(range.Ticks / 2));
        }

        private void ExportDataMenuItem_Click(object sender, EventArgs e)
        {
            Event eve = (sender as MenuItem).Tag as Event;

            if (Core.ActiveInstruments.Count == 0)
            {
                MessageBox.Show("No active Instruments for exporting data!");
                return;
            }
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;

            ExportDataDialog dialog = new ExportDataDialog(Core.ActiveInstruments, Core.GlobalStart, Core.GlobalEnd,
                eve.StartTime, eve.StartTime, eve.EndTime, eve.EndTime);
            dialog.ShowDialog();
            return;
        }

        private void RemoveEventMenuItem_Click(object sender, EventArgs e)
        {
            Event eve = (sender as MenuItem).Tag as Event;
            ManualEG eg = eve.GetEventGenerator() as ManualEG;
            eg.RemoveEvent(eve);

            Core.Events.Remove(eve);
            DisplayEvents();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;

            AutoConfigurator autoConfigurator = new AutoConfigurator(Core.SiteManager);
            OpenFileDialog dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "Open Data File",
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (autoConfigurator.ConfigFromFile(dialog.FileName) != ReturnCode.SUCCESS) MessageBox.Show("Could not open file!");
                ClearPanels();
                if (Core.SiteManager.Reload() != ReturnCode.SUCCESS) MessageBox.Show("Warning: Bad trouble loading the site manager!");
                if (presetMan.Reload() != ReturnCode.SUCCESS) MessageBox.Show("Warning: Bad trouble loading the preset manager!");
                UpdateSitesTree();
                if (!(autoConfigurator.ConfiguredInstrument is null))
                {
                    SitesTreeView.Nodes.Find(autoConfigurator.ConfiguredInstrument.ID.ToString(), true)[0].Checked = true;

                    // Zoom out, if reasonable
                    TimeSpan totalSpan = Core.GlobalEnd - Core.GlobalStart;
                    if (totalSpan.TotalDays < 32)
                    {
                        ZoomToFullRange();
                    }

                    // Select channels, if reasonable
                    if (chPanels.Count < 5)
                    {
                        foreach (ChannelPanel channelPanel in chPanels)
                        {
                            channelPanel.Chart1CheckBox.Checked = true;
                        }
                    }
                    else if (chPanels.Count < 9)
                    {
                        for (int i=0; i<(chPanels.Count/2 + chPanels.Count%2); i++)
                        {
                            chPanels[i].Chart1CheckBox.Checked = true;
                        }
                        for (int i = (chPanels.Count / 2 + chPanels.Count % 2); i < chPanels.Count; i++)
                        {
                            chPanels[i].Chart2CheckBox.Checked = true;
                        }
                    }
                    else
                    {
                        chPanels[0].Chart1CheckBox.Checked = true;
                    }
                }
            }

            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }

        LinkedList<DateTimeRange> BackHistory = new LinkedList<DateTimeRange>();
        LinkedList<DateTimeRange> ForwardHistory = new LinkedList<DateTimeRange>();

        private void ChangeView(DateTime startTime, DateTime endTime, bool holdRange = false)
        {
            startTime = new DateTime(startTime.Ticks - (startTime.Ticks % TimeSpan.TicksPerSecond));
            endTime = new DateTime((endTime.Ticks - 1) - ((endTime.Ticks-1) % TimeSpan.TicksPerSecond)).AddSeconds(1);
            if (BackHistory.Count == 0 ||
                (BackHistory.Last.Value.Start != Core.ViewStart ||
                BackHistory.Last.Value.End != Core.ViewEnd))
            { 
                BackHistory.AddLast(new LinkedListNode<DateTimeRange>(new DateTimeRange(Core.ViewStart, Core.ViewEnd)));
                ForwardHistory.Clear();
            }
            Core.ChangeView(startTime, endTime, holdRange);
            UpdateForwardBackButtons();
        }

        private void ShiftView(TimeSpan shift)
        {
            shift = TimeSpan.FromSeconds(Math.Ceiling(shift.TotalSeconds));
            if (BackHistory.Count == 0 ||
                (BackHistory.Last.Value.Start != Core.ViewStart ||
                BackHistory.Last.Value.End != Core.ViewEnd))
            {
                BackHistory.AddLast(new LinkedListNode<DateTimeRange>(new DateTimeRange(Core.ViewStart, Core.ViewEnd)));
                ForwardHistory.Clear();
            }
            Core.ShiftView(shift);
            UpdateForwardBackButtons();
        }

        private void GoBack()
        {
            if (BackHistory.Count == 0) return;
            ForwardHistory.AddLast(new LinkedListNode<DateTimeRange>(new DateTimeRange(Core.ViewStart, Core.ViewEnd)));
            Core.ChangeView(BackHistory.Last.Value.Start, BackHistory.Last.Value.End);
            BackHistory.RemoveLast();
            UpdateForwardBackButtons();
        }

        private void GoForward()
        {
            if (ForwardHistory.Count == 0) return;
            BackHistory.AddLast(new LinkedListNode<DateTimeRange>(new DateTimeRange(Core.ViewStart, Core.ViewEnd)));
            Core.ChangeView(ForwardHistory.Last.Value.Start, ForwardHistory.Last.Value.End);
            ForwardHistory.RemoveLast();
            UpdateForwardBackButtons();
        }

        private void BackwardButton_Click(object sender, EventArgs e)
        {
            GoBack();
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            GoForward();
        }

        private void UpdateForwardBackButtons()
        {
            if (BackHistory.Count == 0) BackwardButton.Enabled = false;
            else BackwardButton.Enabled = true;

            if (ForwardHistory.Count == 0) ForwardButton.Enabled = false;
            else ForwardButton.Enabled = true;
        }

        private void ExportEventsButton_Click(object sender, EventArgs e)
        {
            // Nobody wants an empty file
            if (Core.Events.Count == 0) return;

            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = "csv",
                OverwritePrompt = true,
                FileName = "events.csv"
            };
            if (dialog.ShowDialog() != DialogResult.OK) return;

            // Abort if file location is unwritable
            try
            {
                File.WriteAllText(dialog.FileName, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not write to file location!");
                return;
            }

            // Write file
            EventWriter writer = new EventWriter();
            writer.WriteEventFile(dialog.FileName, Core.Events);
        }

        private void CollapseLeftButton_Click(object sender, EventArgs e)
        {
            if (LeftLeftPanel.Visible)
                CollapseLeftPanel();
            else
                ExpandLeftPanel();
        }

        private void CollapseLeftPanel()
        {
            LeftLeftPanel.Visible = false;
            CollapseLeftButton.Text = ">";
            userSettings.ShowLeftPanel = false;
        }

        private void ExpandLeftPanel()
        {
            LeftLeftPanel.Visible = true;
            CollapseLeftButton.Text = "<";
            userSettings.ShowLeftPanel = true;
        }

        private void CollapseRightButton_Click(object sender, EventArgs e)
        {
            if (RightRightPanel.Visible)
                CollapseRightPanel();
            else
                ExpandRightPanel();
        }

        private void CollapseRightPanel()
        {
            RightRightPanel.Visible = false;
            CollapseRightButton.Text = "<";
            userSettings.ShowRightPanel = false;
        }

        private void ExpandRightPanel()
        {
            RightRightPanel.Visible = true;
            CollapseRightButton.Text = ">";
            userSettings.ShowRightPanel = true;
        }

        private void ZoomFullRangeButton_Click(object sender, EventArgs e)
        {
            TimeSpan totalSpan = Core.GlobalEnd - Core.GlobalStart;
            if (totalSpan.TotalDays > 32)
            {
                if (MessageBox.Show("You are about to display " + ((int)totalSpan.TotalDays).ToString() + " days of data. Do you want to contiune?", "That's a long time!", MessageBoxButtons.OKCancel)
                    == DialogResult.Cancel)
                    return;
            }
            ZoomToFullRange();
        }

        private void ZoomToFullRange()
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            ChangeView(Core.GlobalStart, Core.GlobalEnd, true);
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }

        private void PresetDeleteButton_Click(object sender, EventArgs e)
        {
            DeletePreset(PresetNameTextBox.Text);
        }

        private void DeletePreset(string name)
        {
            int indexToDelete = -1;
            for (int p = 0; p < presetMan.GetPresets().Count; p++)
                if (presetMan.GetPresets()[p].Name == name)
                    indexToDelete = p;
            if (indexToDelete >= 0)
            {
                if (MessageBox.Show("Are you sure you want to delete preset: " + name + "?", "Delete Preset?", MessageBoxButtons.OKCancel)
                    == DialogResult.Cancel)
                {
                    return;
                }
                presetMan.GetPresets().RemoveAt(indexToDelete);
                presetMan.Save();
                LoadPresets();
            }
            else
            {
                MessageBox.Show("Preset \"" + name + "\" does not exist.", "Nope");
            }
        }

        private void ShiftStartButton_Click(object sender, EventArgs e)
        {
            ShiftView(Core.GlobalStart - Core.ViewStart);
        }

        private void ShiftEndButton_Click(object sender, EventArgs e)
        {
            ShiftView(Core.GlobalEnd - Core.ViewEnd);
        }

        double relativeSplitterDistance = 0.75;
        private void CollapseBottomButton_Click(object sender, EventArgs e)
        {
            if (!bottomPanelCollapsed)
                CollapseBottomPanel();
            else
                ExpandBottomPanel();
        }

        bool bottomPanelCollapsed = false;
        private void CollapseBottomPanel()
        {
            relativeSplitterDistance = (double)CenterSplitContainer.SplitterDistance/CenterSplitContainer.Height;
            bottomPanelCollapsed = true;
            CenterSplitContainer.SplitterDistance = CenterSplitContainer.Height - CollapseBottomButton.Height;
            CenterSplitContainer.IsSplitterFixed = true;
            CollapseBottomButton.Text = "^";
            userSettings.ShowEventsPanel = false;
        }

        private void ExpandBottomPanel()
        {
            bottomPanelCollapsed = false;
            int newSplitterDistance = (int)(relativeSplitterDistance * CenterSplitContainer.Height);
            if (newSplitterDistance < 0) newSplitterDistance = 0;
            CenterSplitContainer.SplitterDistance = newSplitterDistance;
            CenterSplitContainer.IsSplitterFixed = false;
            CollapseBottomButton.Text = "v";
            userSettings.ShowEventsPanel = true;
        }

        private void AllPanelsButton_Click(object sender, EventArgs e)
        {
            userSettings.SuspendSaving = true;
            if (RightRightPanel.Visible || LeftLeftPanel.Visible || !bottomPanelCollapsed )
            {
                if (RightRightPanel.Visible) CollapseRightPanel();
                if (LeftLeftPanel.Visible) CollapseLeftPanel();
                if (!bottomPanelCollapsed) CollapseBottomPanel();
            }
            else
            {
                ExpandRightPanel();
                ExpandLeftPanel();
                ExpandBottomPanel();
            }
            userSettings.SuspendSaving = false;
            userSettings.Save();
        }

        private void SitesTreeView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                CreateSitesTreeContextMenu(SitesTreeView.GetNodeAt(e.X, e.Y), e);
            }
        }

        public void CreateSitesTreeContextMenu(TreeNode node, MouseEventArgs e)
        {
            Persister persister = node.Tag as Persister;

            if (persister is EventGenerator) return;    // Not supported yet
            else
            { 
                ContextMenu chartMenu = new ContextMenu();

                MenuItem showInSiteManagerMenuItem = new MenuItem("Show " + persister.Name + " in the Site Manager");
                showInSiteManagerMenuItem.Tag = persister;
                showInSiteManagerMenuItem.Click += ShowInSiteManagerMenuItem_Click;
                chartMenu.MenuItems.Add(showInSiteManagerMenuItem);

                MenuItem newInstrumentMenuItem = new MenuItem("Create new Instrument");
                newInstrumentMenuItem.Tag = node;
                newInstrumentMenuItem.Click += NewInstrumentSitesTreeMenuItem_Click;
                chartMenu.MenuItems.Add(newInstrumentMenuItem);

                MenuItem removeMenuItem = new MenuItem("Remove " + persister.Species + ": " + persister.Name);
                removeMenuItem.Tag = node;
                removeMenuItem.Click += RemoveSitesTreeMenuItem_Click;
                chartMenu.MenuItems.Add(removeMenuItem);

                chartMenu.Show(SitesTreeView, new Point(e.X, e.Y));
            }
        }

        private void ShowInSiteManagerMenuItem_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            Persister persister = menuItem.Tag as Persister;
            SiteManagerForm siteManForm = new SiteManagerForm(this, Core.SiteManager, persister);
            siteManForm.ShowDialog();
        }

        private void NewInstrumentSitesTreeMenuItem_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            TreeNode node = menuItem.Tag as TreeNode;
            Persister persister = node.Tag as Persister;

            NewInstrumentDialog dialog;
            if (persister is Site)
            {
                dialog = new NewInstrumentDialog(Core, persister as Site);
            }
            else if (persister is Facility)
            {
                Facility facility = persister as Facility;
                dialog = new NewInstrumentDialog(Core, facility.Parent as Site, facility);
            }
            else if (persister is DetectionSystem)
            {
                DetectionSystem system = persister as DetectionSystem;
                dialog = new NewInstrumentDialog(Core, system.Parent.Parent as Site,
                    system.Parent as Facility, system);
            }
            else if (persister is Instrument)
            {
                DetectionSystem system = (persister as Instrument).Parent as DetectionSystem;
                dialog = new NewInstrumentDialog(Core, system.Parent.Parent as Site,
                    system.Parent as Facility, system);
            }
            else dialog = new NewInstrumentDialog(Core);

            if (dialog.ShowDialog() != DialogResult.OK) return;
            SiteManagerForm siteManForm = new SiteManagerForm(this, Core.SiteManager, dialog.SelectedSystem, true);
            siteManForm.ShowDialog();
        }

        private void RemoveSitesTreeMenuItem_Click(object sender, EventArgs e)
        {
            
            MenuItem menuItem = sender as MenuItem;
            TreeNode node = menuItem.Tag as TreeNode;
            Persister persister = node.Tag as Persister;

            if (MessageBox.Show("Are your sure you want to remove " + persister.Name + "?", "Remove " + persister.Species, MessageBoxButtons.OKCancel)
               != DialogResult.OK) return;

            node.Checked = false;
            node.Remove();
            persister.Delete();
            Core.SiteManager.Save();
        }

        private void newXYChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewXYChart();
        }

        private XYPanel CreateNewXYChart(bool selectTab=true)
        {
            XYPanel xyPanel = new XYPanel(Core);
            xyPanel.SuspendLayout();
            xyPanel.Dock = DockStyle.Fill;
            

            TabPage tabPage = new TabPage("Chart");
            tabPage.SuspendLayout();
            tabPage.Controls.Add(xyPanel);
            tabPage.Tag = xyPanel;

            xyPanel.ChartTitle = "Chart";
            xyPanel.ChartTitleChanged += XYPanel_ChartTitleChanged;

            BottomTabControl.TabPages.Add(tabPage);
            tabPage.ResumeLayout();
            xyPanel.ResumeLayout();

            if(selectTab) BottomTabControl.SelectedTab = tabPage;
            return xyPanel;
        }

        private void XYPanel_ChartTitleChanged(object sender, EventArgs e)
        {
            for(int t = 1; t < BottomTabControl.TabCount; t++)
            {
                XYPanel xyPanel = BottomTabControl.TabPages[t].Tag as XYPanel;
                if (sender == xyPanel)
                {
                    BottomTabControl.TabPages[t].Text = xyPanel.ChartTitle;
                }
            }
        }

        private void BottomTabControl_MouseClick(object sender, MouseEventArgs e)
        {
            // Right click only
            if (e.Button != MouseButtons.Right) return;

            TabPage tabPage = null;

            for (int t = 1; t < BottomTabControl.TabCount; t++)
            {
                if (BottomTabControl.GetTabRect(t).Contains(e.Location))
                {
                    tabPage = BottomTabControl.TabPages[t];
                    break;
                }
            }
            if (tabPage is null) return;

            ContextMenu menu = new ContextMenu();
            MenuItem menuItem = new MenuItem("Close XY Chart", CloseXYChartMenuItem_Click)
            {
                Tag = tabPage
            };
            menu.MenuItems.Add(menuItem);

            menu.Show(BottomTabControl, new Point(e.X, e.Y));
        }

        private void CloseXYChartTab(TabPage tabPage)
        {
            XYPanel xyPanel = tabPage.Tag as XYPanel;
            xyPanel.Unsubscribe();
            BottomTabControl.TabPages.Remove(tabPage);
            tabPage.Dispose();
            xyPanel.Dispose();
        }

        private void CloseXYChartMenuItem_Click(object sender, EventArgs e)
        {
            CloseXYChartTab((sender as MenuItem).Tag as TabPage);
        }

        private void CenterSplitContainer_Panel2_MouseClick(object sender, MouseEventArgs e)
        {
            // Right click only
            if (e.Button != MouseButtons.Right) return;

            ContextMenu menu = new ContextMenu();
            MenuItem menuItem = new MenuItem("New XY Chart", NewXYChartMenuItem_Click);
            menu.MenuItems.Add(menuItem);

            menu.Show(CenterSplitContainer.Panel2, new Point(e.X, e.Y));
        }

        private void NewXYChartMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewXYChart();
        }

        private void BottomTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int t = 1; t < BottomTabControl.TabCount; t++)
            {
                XYPanel xyPanel = BottomTabControl.TabPages[t].Tag as XYPanel;
                if (BottomTabControl.TabPages[t] == BottomTabControl.SelectedTab) xyPanel.Active = true;
                else xyPanel.Active = false;
            }
        }
    }
}
