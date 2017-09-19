using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.WinForms;
using LiveCharts.Defaults;
using LiveCharts.Geared;

using Omniscient.Parsers;
using Omniscient.Instruments;

namespace Omniscient
{
    public partial class MainForm : Form
    {
        SiteManager siteMan;
        List<ChannelPanel> chPanels;
        List<Instrument> activeInstruments;

        public MainForm()
        {
            activeInstruments = new List<Instrument>();
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            chPanels = new List<ChannelPanel>();
            siteMan = new SiteManager();
            siteMan.LoadFromXML("SiteManager.xml");
            UpdateSiteTree();
            RangeTextBox.Text = "7";
            RangeComboBox.Text = "Days";

        }

        private void UpdateSiteTree()
        {
            SitesTreeView.Nodes.Clear();
            foreach(Site site in siteMan.GetSites())
            {
                TreeNode siteNode = new TreeNode(site.GetName());
                foreach(Facility fac in site.GetFacilities())
                {
                    TreeNode facNode = new TreeNode(fac.GetName());
                    foreach (DetectionSystem sys in fac.GetSystems())
                    {
                        TreeNode sysNode = new TreeNode(sys.GetName());
                        foreach (Instrument inst in sys.GetInstruments())
                        {
                            TreeNode instNode = new TreeNode(inst.GetName());
                            sysNode.Nodes.Add(instNode);
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

        private void launchInspectrumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Inspectrum inspectrum = new Inspectrum();
            inspectrum.Show();
        }

        private void UpdateInstrumentData(Instrument inst)
        {
            inst.LoadData(new DateTime(1900, 1, 1), new DateTime(2100, 1, 1));
        }

        private void UpdateData()
        {
            foreach(Instrument inst in activeInstruments)
            {
                UpdateInstrumentData(inst);
            }
        }

        private void UpdateChart(int chartNum)
        {
            LiveCharts.WinForms.CartesianChart chart;
            switch (chartNum)
            {
                case 0:
                    chart = StripChart0;
                    break;
                case 1:
                    chart = StripChart1;
                    break;
                case 2:
                    chart = StripChart2;
                    break;
                case 3:
                    chart = StripChart3;
                    break;
                default:                    // This should really never be necessary but the compilar gets angry without it
                    chart = StripChart0;
                    break;
            }
            chart.DisableAnimations = true;
            chart.Hoverable = false;
            chart.DataTooltip = null;

            chart.AxisX.Clear();
            chart.AxisY.Clear();
            DateTime start = StartDatePicker.Value.Date;
            start = start.Add(StartTimePicker.Value.TimeOfDay);
            DateTime end = EndDatePicker.Value.Date;
            end = end.Add(EndTimePicker.Value.TimeOfDay);

            if (start >= end) return;

            string xLabelFormat;
            Separator sep = new Separator();
            double daysInRange = TimeSpan.FromTicks(end.Ticks - start.Ticks).TotalDays;
            // Choose an appropriate x-axis label format
            if (daysInRange > 1460)
            {
                xLabelFormat = "yyyy";
                sep.Step = TimeSpan.FromDays(365).Ticks;
            }
            else if (daysInRange > 540)
            {
                xLabelFormat = "MM/yyyy";
                sep.Step = TimeSpan.FromDays(90).Ticks;
            }
            else if (daysInRange > 180)
            {
                xLabelFormat = "MM/yyyy";
                sep.Step = TimeSpan.FromDays(30).Ticks;
            }
            else if (daysInRange > 60)
            {
                xLabelFormat = "MMM dd";
                sep.Step = TimeSpan.FromDays(10).Ticks;
            }
            else if (daysInRange > 20)
            {
                xLabelFormat = "MMM dd";
                sep.Step = TimeSpan.FromDays(3).Ticks;
            }
            else if (daysInRange > 2)
            {
                xLabelFormat = "MMM dd";
                sep.Step = TimeSpan.FromDays(1).Ticks;
            }
            else
            {
                double hoursInRange = TimeSpan.FromTicks(end.Ticks - start.Ticks).TotalHours;
                if (hoursInRange > 12)
                {
                    xLabelFormat = "HH:mm";
                    sep.Step = TimeSpan.FromHours(2).Ticks;
                }
                else if (hoursInRange > 4)
                {
                    xLabelFormat = "HH:mm";
                    sep.Step = TimeSpan.FromHours(1).Ticks;
                }
                else if (hoursInRange > 1)
                {
                    xLabelFormat = "HH:mm";
                    sep.Step = TimeSpan.FromHours(0.25).Ticks;
                }
                else
                {
                    double minInRange = TimeSpan.FromTicks(end.Ticks - start.Ticks).TotalMinutes;
                    if (minInRange > 30)
                    {
                        xLabelFormat = "HH:mm:ss";
                        sep.Step = TimeSpan.FromMinutes(5).Ticks;
                    }
                    else if (minInRange > 15)
                    {
                        xLabelFormat = "HH:mm:ss";
                        sep.Step = TimeSpan.FromMinutes(2).Ticks;
                    }
                    else if (minInRange > 5)
                    {
                        xLabelFormat = "HH:mm:ss";
                        sep.Step = TimeSpan.FromMinutes(1).Ticks;
                    }
                    else if (minInRange > 1)
                    {
                        xLabelFormat = "HH:mm:ss";
                        sep.Step = TimeSpan.FromMinutes(0.5).Ticks;
                    }
                    else
                    {
                        xLabelFormat = "HH:mm:ss";
                        sep.Step = TimeSpan.FromMilliseconds(5000).Ticks;
                    }
                }
            }


            chart.AxisX.Add(new Axis
            {
                LabelFormatter = val => new System.DateTime((long)val).ToString(xLabelFormat),
                MinValue = start.Ticks,
                MaxValue = end.Ticks,
                Separator = sep
            });

            

            chart.AxisY.Add(new Axis
            {
                MinValue = 0,
                //MaxValue = 200
            });
            SeriesCollection seriesColl = new SeriesCollection();
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

                    List<DateTime> dates = chan.GetTimeStamps();
                    List<double> vals = chan.GetValues();

                    // Load up the chart values
                    GearedValues<DateTimePoint> chartVals = new GearedValues<DateTimePoint>();
                    List<DateTimePoint> list = new List<DateTimePoint>();
                    for (int i = 0; i < dates.Count; ++i) //
                    {
                        list.Add(new DateTimePoint(dates[i], vals[i]));
                    }
                    if (list.Count < 100)
                        chartVals = list.AsGearedValues().WithQuality(Quality.Highest);
                    else if(list.Count < 1000)
                        chartVals = list.AsGearedValues().WithQuality(Quality.High);
                    else if (list.Count < 10000)
                        chartVals = list.AsGearedValues().WithQuality(Quality.Medium);
                    else
                        chartVals = list.AsGearedValues().WithQuality(Quality.Low);

                    GStepLineSeries series = new GStepLineSeries()
                    {
                        Title = chan.GetName(),
                        PointGeometry = null,
                        Values = chartVals
                    };
                    seriesColl.Add(series);
                }
            }

            chart.Series = seriesColl;
            chart.LegendLocation = LegendLocation.Right;
        }


        private void OnChannelPannelCheckChanged(object sender, EventArgs e)
        {
            CheckBox checker = (CheckBox)sender;
            UpdateChart((int)checker.Tag);
        }

        private void AddChannelPanels(Instrument inst)
        {
            activeInstruments.Add(inst);
            UpdateInstrumentData(inst);

            ChannelsPanel.SuspendLayout();
            foreach (Channel ch in inst.GetChannels())
            {
                ChannelPanel chanPan = new ChannelPanel(ch);
                chanPan.Dock = DockStyle.Top;
                chanPan.CheckChanged += new EventHandler(OnChannelPannelCheckChanged);
                ChannelsPanel.Controls.Add(chanPan);
                chPanels.Add(chanPan);
            }

            for (int i = chPanels.Count - 1; i >= 0; i--)
                chPanels[i].SendToBack();

            ChannelsLabelPanel.SendToBack();
            ChannelsPanel.ResumeLayout();
        }

        private void RemoveChannelPanels(Instrument inst)
        {
            activeInstruments.Remove(inst);
            inst.ClearData();

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
            UpdatesCharts();
        }

        private void SitesTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            const int INST_LEVEL = 3;
            if ( e.Node.Level == INST_LEVEL)
            {
                string instName = e.Node.Text;
                string sysName = e.Node.Parent.Text;
                string facName = e.Node.Parent.Parent.Text;
                string siteName = e.Node.Parent.Parent.Parent.Text;
                foreach (Site site in siteMan.GetSites())
                {
                    if (site.GetName().Equals(siteName))
                    {
                        foreach (Facility fac in site.GetFacilities())
                        {
                            if (fac.GetName().Equals(facName))
                            {
                                foreach (DetectionSystem sys in fac.GetSystems())
                                {
                                    if (sys.GetName().Equals(sysName))
                                    {
                                        foreach (Instrument inst in sys.GetInstruments())
                                        {
                                            if(inst.GetName().Equals(instName))
                                            {
                                                if (e.Node.Checked)
                                                    AddChannelPanels(inst);
                                                else
                                                    RemoveChannelPanels(inst);
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }

                }
            }
        }

        private void UpdatesCharts()
        {
            StripChartsPanel.SuspendLayout();
            for (int i = 0; i < 4; ++i)
                UpdateChart(i);
            StripChartsPanel.ResumeLayout();
        }

        private void StartDatePicker_ValueChanged(object sender, EventArgs e)
        {
            rangeChanged = true;
        }

        private void EndDatePicker_ValueChanged(object sender, EventArgs e)
        {
            //UpdatesCharts();
        }

        private void StartTimePicker_ValueChanged(object sender, EventArgs e)
        {
            rangeChanged = true;
        }

        private void EndTimePicker_ValueChanged(object sender, EventArgs e)
        {
            //UpdatesCharts();
        }

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
                default:
                    newEnd = StartDatePicker.Value.AddMinutes(range);
                    break;
            }
            newEnd = newEnd.AddTicks(StartTimePicker.Value.TimeOfDay.Ticks);
            EndDatePicker.Value = newEnd.Date;
            EndTimePicker.Value = newEnd;

        }


        private bool rangeChanged = false;

        private void updateRange()
        {
            if (RangeTextBox.Text == "") return;

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
            UpdatesCharts();
        }

        private void RangeTextBox_TextChanged(object sender, EventArgs e)
        {
            rangeChanged = true;
        }

        private void RangeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEndPickers();
            UpdatesCharts();
        }

        private void RangeTextBox_Leave(object sender, EventArgs e)
        {
            if (rangeChanged)
            {
                updateRange();
            }
            rangeChanged = false;
        }

        private void RangeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (rangeChanged & e.KeyCode == Keys.Enter)
            {
                updateRange();
            }
            rangeChanged = false;
        }

        private void StartDatePicker_Leave(object sender, EventArgs e)
        {
            if (rangeChanged)
            {
                updateRange();
            }
            rangeChanged = false;
        }
    }
}
