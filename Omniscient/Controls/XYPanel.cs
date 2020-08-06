/*
This source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

This source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2020, International Atomic Energy Agency (IAEA), IAEA.org

   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of IAEA nor the names of its contributors
      may be used to endorse or promote products derived from this software
      without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS"AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

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
using System.Xml;

namespace Omniscient
{
    public partial class XYPanel : UserControl
    {
        public event EventHandler ChartTitleChanged;

        public enum Fit_Type { NONE, PROPORTIONAL, LINEAR, POWER_LAW, EXPONENTIAL, LOG};
        OmniscientCore Core;

        private Instrument _selectedInstrument;
        public Instrument SelectedInstrument { 
            get { return _selectedInstrument; }
            private set { ChangeInstrument(value); } 
        }
        public Channel XChannel { get; private set; }
        public Channel YChannel { get; private set; }

        private bool _active;
        /// <summary>
        /// While false, chart is not updated
        /// </summary>
        public bool Active { 
            get { return _active; } 
            set { 
                _active = value;
                if (_active) UpdateChart();
            } 
        }

        private string _chartTitle;
        public string ChartTitle
        {
            get { return _chartTitle; }
            set { 
                _chartTitle = value;
                if (ChartTitleTextBox.Text != value) ChartTitleTextBox.Text = value;
                ChartTitleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        double chartMinX;
        double chartMaxX;
        double chartMinY;
        double chartMaxY;

        private Fit_Type FitType;

        public XYPanel(OmniscientCore core)
        {
            Active = false;
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
            string[] types = new string[] { "None", "Proportional", "Linear", "Power", "Exponential", "Log" };
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
            if (!Active) return;

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
            series.ChartType = SeriesChartType.FastPoint;
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
            if (FitType == Fit_Type.NONE || x.Length < 2) return;

            try // Don't break if fitting fails
            {
                switch(FitType)
                {
                    case Fit_Type.PROPORTIONAL:
                        ProportionalFit(x, y);
                        break;
                    case Fit_Type.LINEAR:
                        LinearFit(x, y);
                        break;
                    case Fit_Type.POWER_LAW:
                        PowerFit(x, y);
                        break;
                    case Fit_Type.EXPONENTIAL:
                        ExponentialFit(x, y);
                        break;
                    case Fit_Type.LOG:
                        LogFit(x, y);
                        break;
                }
            }
            catch { }
        }

        private void ProportionalFit(double[] x, double[] y)
        {
            CurveFitter curveFitter = new CurveFitter(x, y);
            double c = curveFitter.ProportionalLeastSq();
            Series series = new Series("y = " +
                ChartingUtil.FormatDoubleNicely(c) + "x");
            series.ChartType = SeriesChartType.Line;
            series.Points.AddXY(chartMinX, c * chartMinX);
            series.Points.AddXY(chartMaxX, c * chartMaxX);

            XYChart.Series.Add(series);
            RSquaredTextBox.Text = ChartingUtil.FormatDoubleNicely(curveFitter.R_Sq);
        }

        private void LinearFit(double[] x, double[] y)
        {
            CurveFitter curveFitter = new CurveFitter(x, y);
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

        private void PowerFit(double[] x, double[] y)
        {
            const int N_FIT_POINTS = 400;

            CurveFitter curveFitter = new CurveFitter(x, y);
            Tuple<double, double> coefficients = curveFitter.PowerLawLeastSq();
            double a = coefficients.Item1;
            double b = coefficients.Item2;
            Series series = new Series("y = " +
                ChartingUtil.FormatDoubleNicely(a) + "x^(" +
                ChartingUtil.FormatDoubleNicely(b) + ")");
            series.ChartType = SeriesChartType.Line;

            double delta = (chartMaxX - chartMinX) / N_FIT_POINTS;
            double fitx = chartMinX;
            for (int i=0; i< N_FIT_POINTS; i++)
            {
                series.Points.AddXY(fitx, a * Math.Pow(fitx, b));
                fitx += delta;
            }

            XYChart.Series.Add(series);
            RSquaredTextBox.Text = ChartingUtil.FormatDoubleNicely(curveFitter.R_Sq);
        }

        private void ExponentialFit(double[] x, double[] y)
        {
            const int N_FIT_POINTS = 400;

            CurveFitter curveFitter = new CurveFitter(x, y);
            Tuple<double, double> coefficients = curveFitter.ExponentialLeastSq();
            double a = coefficients.Item1;
            double b = coefficients.Item2;
            Series series = new Series("y = " +
                ChartingUtil.FormatDoubleNicely(a) + "exp(" +
                ChartingUtil.FormatDoubleNicely(b) + "x)");
            series.ChartType = SeriesChartType.Line;

            double delta = (chartMaxX - chartMinX) / N_FIT_POINTS;
            double fitx = chartMinX;
            for (int i = 0; i < N_FIT_POINTS; i++)
            {
                series.Points.AddXY(fitx, a * Math.Exp(b*fitx));
                fitx += delta;
            }

            XYChart.Series.Add(series);
            RSquaredTextBox.Text = ChartingUtil.FormatDoubleNicely(curveFitter.R_Sq);
        }

        private void LogFit(double[] x, double[] y)
        {
            const int N_FIT_POINTS = 400;

            CurveFitter curveFitter = new CurveFitter(x, y);
            Tuple<double, double> coefficients = curveFitter.LogLeastSq();
            double a = coefficients.Item1;
            double b = coefficients.Item2;
            string bSign = (b >= 0) ? " + " : " - ";
            Series series = new Series("y = " +
                ChartingUtil.FormatDoubleNicely(a) + bSign +
                ChartingUtil.FormatDoubleNicely(Math.Abs(b)) + "ln(x)");
            series.ChartType = SeriesChartType.Line;

            double delta = (chartMaxX - chartMinX) / N_FIT_POINTS;
            double fitx = chartMinX;
            for (int i = 0; i < N_FIT_POINTS; i++)
            {
                series.Points.AddXY(fitx, a + b*Math.Log(fitx));
                fitx += delta;
            }

            XYChart.Series.Add(series);
            RSquaredTextBox.Text = ChartingUtil.FormatDoubleNicely(curveFitter.R_Sq);
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
            Fit_Type selectedType = StringToFitType(FitTypeComboBox.SelectedItem as string);

            if (FitType != selectedType)
            {
                FitType = selectedType;
                UpdateChart();
            }
        }

        private void ChartTitleTextBox_TextChanged(object sender, EventArgs e)
        {
            ChartTitle = ChartTitleTextBox.Text;
        }

        public static string FitTypeToString(Fit_Type type)
        {
            switch(type)
            {
                case Fit_Type.NONE:
                    return "None";
                case Fit_Type.PROPORTIONAL:
                    return "Proportional";
                case Fit_Type.LINEAR:
                    return "Linear";
                case Fit_Type.POWER_LAW:
                    return "Power";
                case Fit_Type.EXPONENTIAL:
                    return "Exponential";
                case Fit_Type.LOG:
                    return "Log";
            }
            throw new ArgumentException("Not a valid fit type!");
        }

        public static Fit_Type StringToFitType(string str)
        {
            switch (str)
            {
                case "None":
                    return Fit_Type.NONE;
                case "Proportional":
                    return Fit_Type.PROPORTIONAL;
                case "Linear":
                    return Fit_Type.LINEAR;
                case "Power":
                    return Fit_Type.POWER_LAW;
                case "Exponential":
                    return Fit_Type.EXPONENTIAL;
                case "Log":
                    return Fit_Type.LOG;
            }
            throw new ArgumentException("Not a valid fit type!");
        }

        public XYPanelSettings GetSettings()
        {
            return new XYPanelSettings()
            {
                ChartTitle = ChartTitle,
                SelectedInstrumentID = SelectedInstrument?.ID ?? 0,
                XChannelID = XChannel?.ID ?? 0,
                YChannelID = YChannel?.ID ?? 0,
                FitType = FitType
            };
        }

        public void ApplySettings(XYPanelSettings settings)
        {
            try { ChartTitle = settings.ChartTitle; }
            catch { }
            try
            {
                SelectedInstrument = Core.ActiveInstruments.Single(i => i.ID == settings.SelectedInstrumentID);
                ChangeXChannel(SelectedInstrument.GetChannels().Single(c => c.ID == settings.XChannelID));
                ChangeYChannel(SelectedInstrument.GetChannels().Single(c => c.ID == settings.YChannelID));
            }
            catch { }
            try 
            { 
                FitType = settings.FitType;
                FitTypeComboBox.SelectedItem = FitTypeToString(FitType);
            }
            catch { }
        }
    }

    public class XYPanelSettings
    {
        public string ChartTitle;
        public uint SelectedInstrumentID;
        public uint XChannelID;
        public uint YChannelID;
        public XYPanel.Fit_Type FitType;

        public void ToXML(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("XYChart");
            xmlWriter.WriteAttributeString("Title", ChartTitle);
            xmlWriter.WriteAttributeString("Instrument", SelectedInstrumentID.ToString("X8"));
            xmlWriter.WriteAttributeString("XChannel", XChannelID.ToString("X8"));
            xmlWriter.WriteAttributeString("YChannel", YChannelID.ToString("X8"));
            xmlWriter.WriteAttributeString("FitType", XYPanel.FitTypeToString(FitType));
            xmlWriter.WriteEndElement();
        }

        public static XYPanelSettings FromXML(XmlNode node)
        {
            XYPanelSettings settings = new XYPanelSettings();
            try { settings.ChartTitle = node.Attributes["Title"]?.InnerText; }
            catch { }
            try
            {
                settings.SelectedInstrumentID = uint.Parse(node.Attributes["Instrument"].InnerText, System.Globalization.NumberStyles.HexNumber);
                settings.XChannelID = uint.Parse(node.Attributes["XChannel"].InnerText, System.Globalization.NumberStyles.HexNumber);
                settings.YChannelID = uint.Parse(node.Attributes["YChannel"].InnerText, System.Globalization.NumberStyles.HexNumber);
            }
            catch { }
            try { settings.FitType = XYPanel.StringToFitType(node.Attributes["FitType"]?.InnerText); }
            catch { }
            return settings;
        }
    }
}
