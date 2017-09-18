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
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random rand = new Random();
            // 0000000000000000000000000000000000000000000000000000000000000000
            // Chart 0
            StripChart0.AxisX.Add(new Axis
            {
                LabelFormatter = x => new DateTime((long)x).ToString("MMM dd, yyyy"),
                FontSize = 10,
                MinValue = new System.DateTime(2012, 9, 1).Ticks,
                MaxValue = new System.DateTime(2012, 9, 10).Ticks
            });
            StripChart0.AxisY.Add(new Axis
            {
                MinValue = 0,
                MaxValue = 1
            });

            StepLineSeries isrSeries = new StepLineSeries
            {
                Title = "ISR",
                //PointGeometry = null,
                Values = new ChartValues<DateTimePoint>
                {
                    new DateTimePoint(new System.DateTime(2012, 9, 1), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 2), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 3), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 4), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 5), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 6), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 7), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 8), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 9), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 10), rand.NextDouble())
                }
            };
            

            SeriesCollection stripChart0Series = new SeriesCollection();
            stripChart0Series.Add(isrSeries);
            StripChart0.Series = stripChart0Series;
            StripChart0.LegendLocation = LegendLocation.Right;
            // 0000000000000000000000000000000000000000000000000000000000000000
            // 0000000000000000000000000000000000000000000000000000000000000000
            // Chart 1
            StripChart1.AxisX.Add(new Axis
            {
                LabelFormatter = x => new DateTime((long)x).ToString("MMM dd, yyyy"),
                FontSize = 10,
                MinValue = new System.DateTime(2012, 9, 1).Ticks,
                MaxValue = new System.DateTime(2012, 9, 10).Ticks
            });
            StripChart1.AxisY.Add(new Axis
            {
                MinValue = 0,
                MaxValue = 1
            });

            StepLineSeries udcmSeries = new StepLineSeries
            {
                Title = "UDCM",
                //PointGeometry = null,
                Values = new ChartValues<DateTimePoint>
                {
                    new DateTimePoint(new System.DateTime(2012, 9, 1), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 2), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 3), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 4), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 5), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 6), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 7), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 8), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 9), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 10), rand.NextDouble())
                }
            };


            SeriesCollection stripChart1Series = new SeriesCollection();
            stripChart1Series.Add(udcmSeries);
            StripChart1.Series = stripChart1Series;
            StripChart1.LegendLocation = LegendLocation.Right;
            // 0000000000000000000000000000000000000000000000000000000000000000
            // 0000000000000000000000000000000000000000000000000000000000000000
            // Chart 2
            StripChart2.AxisX.Add(new Axis
            {
                LabelFormatter = x => new DateTime((long)x).ToString("MMM dd, yyyy"),
                FontSize = 10,
                MinValue = new System.DateTime(2012, 9, 1).Ticks,
                MaxValue = new System.DateTime(2012, 9, 10).Ticks
            });
            StripChart2.AxisY.Add(new Axis
            {
                MinValue = 0,
                MaxValue = 1
            });

            StepLineSeries mcaSeries = new StepLineSeries
            {
                Title = "MCA",
                //PointGeometry = null,
                Values = new ChartValues<DateTimePoint>
                {
                    new DateTimePoint(new System.DateTime(2012, 9, 1), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 2), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 3), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 4), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 5), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 6), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 7), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 8), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 9), rand.NextDouble()),
                    new DateTimePoint(new System.DateTime(2012, 9, 10), rand.NextDouble())
                }
            };


            SeriesCollection stripChart2Series = new SeriesCollection();
            stripChart2Series.Add(mcaSeries);
            StripChart2.Series = stripChart2Series;
            StripChart2.LegendLocation = LegendLocation.Right;
            // 0000000000000000000000000000000000000000000000000000000000000000
            // 0000000000000000000000000000000000000000000000000000000000000000
            // Chart 3
            StripChart3.AxisX.Add(new Axis
            {
                LabelFormatter = x => new DateTime((long)x).ToString("MMM dd, yyyy"),
                FontSize = 10,
                MinValue = new System.DateTime(2012, 9, 1).Ticks,
                MaxValue = new System.DateTime(2012, 9, 10).Ticks
            });
            StripChart3.AxisY.Add(new Axis
            {
                MinValue = 0,
                MaxValue = 2,
                ShowLabels = false                
            });

            ChartValues<DateTimePoint> vals = new ChartValues<DateTimePoint>();
            int r;
            for (int i = 1; i < 10; i++)
            {
                r = (int)Math.Floor(1.999*rand.NextDouble());
                if (r == 1) vals.Add(new DateTimePoint(new System.DateTime(2012, 9, i), 1));
            }

            ScatterSeries vidSeries = new ScatterSeries
            {
                Title = "VID",
                Values = vals
            };


            SeriesCollection stripChart3Series = new SeriesCollection();
            stripChart3Series.Add(vidSeries);
            StripChart3.Series = stripChart3Series;
            StripChart3.LegendLocation = LegendLocation.Right;
            // 0000000000000000000000000000000000000000000000000000000000000000

        }

        private void launchInspectrumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Inspectrum inspectrum = new Inspectrum();
            inspectrum.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ISRInstrument isr = new ISRInstrument("ISR-1");
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                isr.SetDataFolder((folderBrowser.SelectedPath));
            }
            else
            {
                return;
            }

            isr.LoadData(new DateTime(1900, 1, 1), new DateTime(2100, 1, 1));

            List<DateTime> ch0Dates = isr.GetChannel(0).GetTimeStamps();
            List<double> ch0Vals = isr.GetChannel(0).GetValues();




            StripChart0.DisableAnimations = true;
            StripChart0.Hoverable = false;
            StripChart0.DataTooltip = null;

            StripChart0.AxisX.Clear();
            StripChart0.AxisY.Clear();
            StripChart0.AxisX.Add(new Axis
            {
                LabelFormatter = val => new System.DateTime((long)val).ToString("MM/dd/yyyy"),
                MinValue = ch0Dates[0].Ticks,
                MaxValue = ch0Dates[ch0Dates.Count - 1].Ticks
            });
            StripChart0.AxisY.Add(new Axis
            {
                MinValue = 0,
                //MaxValue = 200
            });

            // Load up the chart values
            GearedValues<DateTimePoint> chartVals = new GearedValues<DateTimePoint>();
            List<DateTimePoint> list = new List<DateTimePoint>();
            for (int i = 0; i < ch0Dates.Count; ++i) //
            {
                list.Add(new DateTimePoint(ch0Dates[i], ch0Vals[i]));
            }
            chartVals = list.AsGearedValues().WithQuality(Quality.Highest);

            StripChart0.Series = new SeriesCollection()
            {
                new GStepLineSeries()
                {
                    Title = "Ch-0",
                    PointGeometry = null,
                    Values = chartVals
                }
            };


        }

        private void CenterSplitContainer_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
