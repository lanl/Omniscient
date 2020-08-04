using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Omniscient
{
    public partial class XYPanel : UserControl
    {
        private enum Fit_Type { NONE, LINEAR};
        OmniscientCore Core;

        private Instrument _selectedInstrument;
        public Instrument SelectedInstrument { 
            get { return _selectedInstrument; }
            private set { ChangeInstrument(value); } 
        }
        public Channel XChannel { get; private set; }
        public Channel YChannel { get; private set; }


        double chartMinX;
        double chartMaxX;
        double chartMinY;
        double chartMaxY;

        private Fit_Type FitType;

        public XYPanel(OmniscientCore core)
        {
            Core = core;
            SelectedInstrument = null;
            XChannel = null;
            YChannel = null;
            FitType = Fit_Type.NONE;

            InitializeComponent();
            UpdateControls();
            PopulateFitTypeCombo();
            core.ViewChanged += Core_ViewChanged;
            core.InstrumentActivated += Core_InstrumentActivated;
            core.InstrumentDeactivated += Core_InstrumentDeactivated;
        }

        private void PopulateFitTypeCombo()
        {
            string[] types = new string[] { "None", "Linear" };
            FitTypeComboBox.Items.AddRange(types);
            FitTypeComboBox.SelectedItem = "None";
        }

        private void Core_InstrumentDeactivated(object sender, InstrumentEventArgs e)
        {
            if (SelectedInstrument == e.Instrument)
            {
                SelectedInstrument = null;
            }
            InstrumentComboBox.Items.Remove(e.Instrument);
        }

        private void Core_InstrumentActivated(object sender, InstrumentEventArgs e)
        {
            InstrumentComboBox.Items.Add(e.Instrument);
        }

        private void Core_ViewChanged(object sender, EventArgs e)
        {
            UpdateChart();
        }

        private void UpdateControls()
        {
            XYChart.SuppressExceptions = true;
            XYChart.Update(); 

            InstrumentComboBox.Items.AddRange(Core.ActiveInstruments.ToArray());
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
        }

        private void ExpandRightPanel()
        {
            RightRightPanel.Visible = true;
            CollapseRightButton.Text = ">";
        }

        private void InstrumentComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Instrument instrument = InstrumentComboBox.SelectedItem as Instrument;
            if (SelectedInstrument != instrument) ChangeInstrument(instrument);
        }

        private void ChangeInstrument(Instrument instrument)
        {
            if (instrument == _selectedInstrument) return;
            _selectedInstrument = instrument;
            if (InstrumentComboBox.SelectedItem != instrument) InstrumentComboBox.SelectedItem = instrument;

            XChannel = null;
            YChannel = null;
            XChannelComboBox.Items.Clear();
            YChannelComboBox.Items.Clear();

            if (!(instrument is null))
            { 
                Channel[] channels = instrument.GetChannels();
                XChannelComboBox.Items.AddRange(channels);
                YChannelComboBox.Items.AddRange(channels);
            }
            UpdateChart();
        }

        private void UpdateChart()
        {
            XYChart.Series.Clear();
            XYChart.Annotations.Clear();
            RSquaredTextBox.Text = "";

            // Check for complete settings
            if (XChannel is null || YChannel is null)
            {
                ShowNoData();
                return;
            }

            // Check for presence of data


            List<DateTime> dates = XChannel.GetTimeStamps(ChannelCompartment.View);
            List<double> xVals = XChannel.GetValues(ChannelCompartment.View);
            List<double> yVals = YChannel.GetValues(ChannelCompartment.View);
            int count = dates.Count;
            if (count == 0 || count != xVals.Count || count != yVals.Count)
            {
                ShowNoData();
                return;
            }

            // Build series
            chartMinX = double.MaxValue;
            chartMaxX = double.MinValue;
            chartMinY = double.MaxValue;
            chartMaxY = double.MinValue;

            DateTime start = Core.ViewStart;
            DateTime end = Core.ViewEnd;
            Series series = new Series("Data");
            series.Points.SuspendUpdates();
            series.ChartType = SeriesChartType.Point;
            double x, y;
            List<double> X = new List<double>();
            List<double> Y = new List<double>();
            for (int i=0; i<count; ++i)
            {
                if (dates[i] >= start && dates[i] <=end)
                {
                    x = xVals[i];
                    y = yVals[i];
                    series.Points.AddXY(x, y);
                    if (x < chartMinX) chartMinX = x;
                    if (x > chartMaxX) chartMaxX = x;
                    if (y < chartMinY) chartMinY = y;
                    if (y > chartMaxY) chartMaxY = y;
                    X.Add(x);
                    Y.Add(y);
                }
            }
            if (series.Points.Count == 0)
            {
                ShowNoData();
                return;
            }

            // Update chart control
            XYChart.SuspendLayout();
            XYChart.Series.Add(series);

            ChartArea chartArea = XYChart.ChartAreas[0];

            Tuple<double, double> roundedX = ChartingUtil.AutoRoundRange(chartMinX, chartMaxX, false);
            Tuple<double, double> roundedY = ChartingUtil.AutoRoundRange(chartMinY, chartMaxY, false);

            chartArea.AxisX.Minimum = roundedX.Item1;
            chartArea.AxisX.Maximum = roundedX.Item2;
            chartArea.AxisY.Minimum = roundedY.Item1;
            chartArea.AxisY.Maximum = roundedY.Item2;

            chartArea.AxisX.Title = XChannel.Name;
            chartArea.AxisY.Title = YChannel.Name;

            FitData(X.ToArray(), Y.ToArray());

            if (XYChart.Series.Count > 1)
            { 
                XYChart.Legends[0].Enabled = true;
            }
            else
                XYChart.Legends[0].Enabled = false;

            series.Points.ResumeUpdates();
            XYChart.ResumeLayout();
        }

        private void FitData(double[] x, double[] y)
        {
            if (FitType == Fit_Type.NONE) return;

            try // Don't break if fitting fails
            {
                CurveFitter curveFitter = new CurveFitter(x, y);
                if (FitType == Fit_Type.LINEAR && x.Length > 1)
                {
                    Tuple<double, double> coefficients = curveFitter.LinearLeastSq();
                    double m = coefficients.Item1;
                    double b = coefficients.Item2;
                    string bSign = (b >= 0) ? " + " : " - ";
                    Series series = new Series("y = " +
                        ChartingUtil.FormatDoubleNicely(m) + "x" + bSign +
                        ChartingUtil.FormatDoubleNicely(Math.Abs(b)));
                    series.ChartType = SeriesChartType.Line;
                    series.Points.AddXY(chartMinX, m * chartMinX + b);
                    series.Points.AddXY(chartMaxX, m * chartMaxX + b);
                    
                    XYChart.Series.Add(series);
                    RSquaredTextBox.Text = ChartingUtil.FormatDoubleNicely(curveFitter.R_Sq);
                }
            }
            catch { }
        }

        private void ShowNoData()
        {
            TextAnnotation textA = new TextAnnotation()
            {
                Text = "No Data",
                AnchorX = 50,
                AnchorY = 50,
                Alignment = ContentAlignment.MiddleCenter
            };
            XYChart.Annotations.Add(textA);
        }

        private void XChannelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Channel channel = XChannelComboBox.SelectedItem as Channel;
            if (XChannel != channel) ChangeXChannel(channel);
        }

        private void ChangeXChannel(Channel channel)
        {
            XChannel = channel;
            if (XChannelComboBox.SelectedItem != channel) XChannelComboBox.SelectedItem = channel;

            UpdateChart();
        }

        private void YChannelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Channel channel = YChannelComboBox.SelectedItem as Channel;
            if (YChannel != channel) ChangeYChannel(channel);
        }

        private void ChangeYChannel(Channel channel)
        {
            YChannel = channel;
            if (YChannelComboBox.SelectedItem != channel) YChannelComboBox.SelectedItem = channel;

            UpdateChart();
        }

        public void Unsubscribe()
        {
            Core.ViewChanged -= Core_ViewChanged;
            Core.InstrumentActivated -= Core_InstrumentActivated;
            Core.InstrumentDeactivated -= Core_InstrumentDeactivated;
        }

        private void FitTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fit_Type selectedType = Fit_Type.NONE;
            switch (FitTypeComboBox.SelectedItem as string)
            {
                case "None":
                    selectedType = Fit_Type.NONE;
                    break;
                case "Linear":
                    selectedType = Fit_Type.LINEAR;
                    break;
            }

            if (FitType != selectedType)
            {
                FitType = selectedType;
                UpdateChart();
            }
        }
    }
}
