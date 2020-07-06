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
using System.Windows.Forms.DataVisualization.Charting;

namespace Omniscient
{
    public partial class Inspectrum : Form
    {

        CHNParser chnParser;
        SPEParser speParser;
        N42Parser n42Parser;
        HGMParser hgmParser;

        bool calibrationChanged = false;
        bool fileLoaded = false;

        double calibrationZero;
        double calibrationSlope;
        int[] counts;

        int mouseX = 0;
        int mouseY = 0;
        double mouseDownX = 0;
        double mouseDownY = 0;
        double mouseUpX = 0;
        double mouseUpY = 0;

        double maxX = 1;
        double maxY = 1;
        double minDelta = 1;

        bool drawingXZoomBox = false;
        bool drawingYZoomBox = false;

        bool controlPressed = false;

        const int H_SCROLL_MAX = 10000;
        
        public string FileName { get; private set; }
        public string FileExt { get; private set; }

        public Inspectrum()
        {
            calibrationZero = 0;
            calibrationSlope = 1;
            counts = new int[0];
            InitializeComponent();
            chnParser = new CHNParser();
            speParser = new SPEParser();
            n42Parser = new N42Parser();
            hgmParser = new HGMParser();
        }

        private void DrawSpectrum()
        {
            SpecChart.SuspendLayout();
            SpecChart.Series.Clear();

            Series series = new Series("Spectrum");
            series.Points.SuspendUpdates();
            series.ChartType = SeriesChartType.StepLine;
            series.XValueType = ChartValueType.Double;
            series.BorderWidth = 2;

            series.Color = Color.Blue;

            List<double> energies = new List<double>();
            List<double> vals = new List<double>();
            double energy = 1.0;
            maxX = 1;
            maxY = 1;
            minDelta = double.MaxValue;
            for (int i = 0; i < counts.Length; ++i) //
            {
                energy = calibrationZero + i * calibrationSlope;
                energies.Add(energy);
                vals.Add(counts[i]);

                series.Points.AddXY(energy, counts[i]);
                if (energy > maxX) maxX = energy;
                if (counts[i] > maxY) maxY = counts[i];
                if (i > 0)
                {
                    if (energy - energies[i - 1] < minDelta) minDelta = energy - energies[i - 1];
                }
            }
            SpecChart.Series.Add(series);
            series.Points.ResumeUpdates();

            if (minDelta >= 1)
            {
                SpecChart.ChartAreas[0].AxisX.LabelStyle.Format = "#";
            }
            else if (minDelta >= 0.1)
            {
                SpecChart.ChartAreas[0].AxisX.LabelStyle.Format = "#.##";
            }
            else if (minDelta >= 0.01)
            {
                SpecChart.ChartAreas[0].AxisX.LabelStyle.Format = "#.##";
            }
            else if (minDelta >= 0.001)
            {
                SpecChart.ChartAreas[0].AxisX.LabelStyle.Format = "#.###";
            }

            ZoomFullView();
            SpecChart.ResumeLayout();
        }

        public void LoadSpectrumFile(string fileName, DateTime? specTime = null)
        {
            Spectrum spectrum;
            string fileAbrev = fileName.Substring(fileName.LastIndexOf('\\') + 1);
            FileExt = fileAbrev.Substring(fileAbrev.Length - 3).ToLower();
            if (FileExt == "chn")
            {
                SpectrumNumberUpDown.Value = 1;
                SpectrumNumberUpDown.Enabled = false;
                chnParser.ParseSpectrumFile(fileName);
                spectrum = chnParser.GetSpectrum();
            }
            else if (FileExt == "spe")
            {
                SpectrumNumberUpDown.Value = 1;
                SpectrumNumberUpDown.Enabled = false;
                speParser.ParseSpectrumFile(fileName);
                spectrum = speParser.GetSpectrum();
            }
            else if (FileExt == "n42")
            {
                SpectrumNumberUpDown.Value = 1;
                SpectrumNumberUpDown.Enabled = false;
                n42Parser.ParseSpectrumFile(fileName);
                spectrum = n42Parser.GetSpectrum();
            }
            else if (FileExt == "hgm")
            {
                SpectrumNumberUpDown.Enabled = true;
                hgmParser.ParseSpectrumFile(fileName);
                if (specTime != null)
                {
                    List<Spectrum> spectra = hgmParser.Spectra;

                    // If need be, fail gracefully
                    spectrum = spectra[0];
                    SpectrumNumberUpDown.Value = 1;

                    for (int i=0; i< spectra.Count; ++i)
                    {
                        if (spectra[i].GetStartTime() == specTime)
                        {
                            spectrum = spectra[i];
                            SpectrumNumberUpDown.Value = i+1;
                            break;
                        }
                    }
                }
                else
                { 
                    spectrum = hgmParser.GetSpectrum();
                    SpectrumNumberUpDown.Value = 1;
                }
            }
            else
            {
                MessageBox.Show("Invalid file type!");
                return;
            }

            // Populate text fields
            FileNameTextBox.Text = fileName;

            LoadSpectrum(spectrum);

            FileName = fileName;
            fileLoaded = true;
        }

        private void LoadSpectrum(Spectrum spectrum)
        {
            DateTextBox.Text = spectrum.GetStartTime().ToString("dd-MMM-yyyy");
            TimeTextBox.Text = spectrum.GetStartTime().ToString("HH:mm:ss");
            CalZeroTextBox.Text = string.Format("{0:F3}", spectrum.GetCalibrationZero());
            CalSlopeTextBox.Text = string.Format("{0:F4}", spectrum.GetCalibrationSlope());

            LiveTimeTextBox.Text = string.Format("{0:F1} sec", spectrum.GetLiveTime());
            double deadTimePerc = 100 * (spectrum.GetRealTime() - spectrum.GetLiveTime()) / spectrum.GetRealTime();
            DeadTimeStripTextBox.Text = string.Format("{0:F2} %", deadTimePerc);

            calibrationZero = spectrum.GetCalibrationZero();
            calibrationSlope = spectrum.GetCalibrationSlope();
            counts = spectrum.GetCounts();
            DrawSpectrum();
        }

        public void LoadCHNFile(string fileName)
        {
            if (chnParser.ParseSpectrumFile(fileName) == ReturnCode.SUCCESS)
            {
                // Populate text fields
                FileNameTextBox.Text = fileName;
                DateTextBox.Text = chnParser.GetStartDateTime().ToString("dd-MMM-yyyy");
                TimeTextBox.Text = chnParser.GetStartDateTime().ToString("HH:mm:ss");
                CalZeroTextBox.Text = string.Format("{0:F3}", chnParser.GetCalibrationZero());
                CalSlopeTextBox.Text = string.Format("{0:F4}", chnParser.GetCalibrationSlope());

                LiveTimeTextBox.Text = string.Format("{0:F1} sec", chnParser.GetLiveTime());
                double deadTimePerc = 100 * (chnParser.GetRealTime() - chnParser.GetLiveTime()) / chnParser.GetRealTime();
                DeadTimeStripTextBox.Text = string.Format("{0:F2} %", deadTimePerc);

                calibrationZero = chnParser.GetCalibrationZero();
                calibrationSlope = chnParser.GetCalibrationSlope();
                counts = chnParser.GetCounts();
                DrawSpectrum();

                fileLoaded = true;
            }
            else
            {
                MessageBox.Show("Error opening file!");
            }
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Spectrum Files|*.chn;*.spe;*.n42;*.hgm|chn files (*.chn)|*.chn|spe files (*.spe)|*.spe|N42 files (*.n42)|*.n42|HGM files (*.hgm)|*.hgm|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadSpectrumFile(openFileDialog.FileName);
            }
        }

        private void OpenToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void Inspectrum_Load(object sender, EventArgs e)
        {
            SpecChart.MouseDown += SpecChart_MouseDown;
            SpecChart.MouseMove += SpecChart_MouseMove;
            SpecChart.MouseUp += SpecChart_MouseUp;
            SpecChart.Paint += SpecChart_Paint;
            SpecChart.Resize += SpecChart_Resize;
            SpecChart.GetToolTipText += SpecChart_GetToolTipText;

            SpaceOutChart();

            HScroll.Minimum = 0;
            HScroll.Maximum = H_SCROLL_MAX;

            if (fileLoaded) DrawSpectrum();
        }

        private void SpecChart_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            double distSq = 1e9;
            double thisDistSq;
            double xVal = 0;
            double yVal = 0;
            string sName = "";
            foreach (Series series in SpecChart.Series)
            {
                foreach (DataPoint point in series.Points)
                {
                    double deltaX = Math.Abs(e.X - SpecChart.ChartAreas[0].AxisX.ValueToPixelPosition(point.XValue));
                    double deltaY = Math.Abs(e.Y - SpecChart.ChartAreas[0].AxisY.ValueToPixelPosition(point.YValues[0]));
                    thisDistSq = deltaX * deltaX + deltaY * deltaY;
                    if (thisDistSq < distSq)
                    {
                        distSq = thisDistSq;
                        xVal = point.XValue;
                        yVal = point.YValues[0];
                        sName = series.Name;
                    }
                }
            }
            if (distSq < 100)
                e.Text = ("x: " + xVal.ToString() + "\ny: " + yVal.ToString());
        }

        private void SpecChart_Resize(object sender, EventArgs e)
        {
            SpaceOutChart();
        }

        private void SpaceOutChart()
        {
            if (SpecChart.Width > 100)
            {
                float positionX = 0;
                float positionY = 0;
                float positionWidth = 99.9F;
                float positionHeight = 99.9F;
                float plotX = 100.0F * 60.0F / SpecChart.Width;
                float plotWidth = 99.9F - 1.5F * plotX;

                if (SpecChart.Height > 52)
                {
                    float plotY = 100.0F * 20.0F / SpecChart.Height;
                    float plotHeight = 99.9F - 2.5F * plotY;
                    SpecChart.ChartAreas[0].Position.X = positionX;
                    SpecChart.ChartAreas[0].Position.Y = positionY;
                    SpecChart.ChartAreas[0].Position.Width = positionWidth;
                    SpecChart.ChartAreas[0].Position.Height = positionHeight;
                    SpecChart.ChartAreas[0].InnerPlotPosition.X = plotX;
                    SpecChart.ChartAreas[0].InnerPlotPosition.Y = plotY;
                    SpecChart.ChartAreas[0].InnerPlotPosition.Width = plotWidth;
                    SpecChart.ChartAreas[0].InnerPlotPosition.Height = plotHeight;
                }
            }
        }

        private void SpecChart_Paint(object sender, PaintEventArgs e)
        {
            Axis X = SpecChart.ChartAreas[0].AxisX;
            Axis Y = SpecChart.ChartAreas[0].AxisY;
            if (drawingXZoomBox)
            {
                int xStart = (int)X.ValueToPixelPosition(mouseDownX);
                int xNow = mouseX;

                // Don't go out of range
                int xMin = Math.Min(xStart, xNow);
                if (xMin < X.ValueToPixelPosition(X.Minimum)) xMin = (int)X.ValueToPixelPosition(X.Minimum);
                int xMax = Math.Max(xStart, xNow);
                if (xMax >= X.ValueToPixelPosition(X.Maximum)) xMax = (int)X.ValueToPixelPosition(X.Maximum) - 1;

                e.Graphics.DrawRectangle(Pens.Gray, xMin, (int)Y.ValueToPixelPosition(Y.Maximum),
                    xMax - xMin, (int)Y.ValueToPixelPosition(Y.Minimum) - (int)Y.ValueToPixelPosition(Y.Maximum));
            }
            if (drawingYZoomBox)
            {
                int yStart = (int)Y.ValueToPixelPosition(mouseDownY);
                int yNow = mouseY;

                e.Graphics.DrawRectangle(Pens.Gray, (int)X.ValueToPixelPosition(X.Minimum), Math.Min(yStart, yNow),
                    (int)X.ValueToPixelPosition(X.Maximum) - (int)X.ValueToPixelPosition(X.Minimum), Math.Abs(yStart - yNow));
            }
        }

        private void SpecChart_MouseUp(object sender, MouseEventArgs e)
        {
            if (!drawingXZoomBox && !drawingYZoomBox) return;
            if (drawingYZoomBox)
            {
                double mouseY = e.Y > 0 ? SpecChart.ChartAreas[0].AxisY.PixelPositionToValue(e.Y) : 0;
                double newMin;
                double newMax;
                if (mouseY > mouseDownY)
                {
                    newMin = mouseDownY;
                    newMax = mouseY;
                }
                else
                {
                    double mid = (SpecChart.ChartAreas[0].AxisY.Maximum + SpecChart.ChartAreas[0].AxisY.Minimum) / 2;
                    double radius = mid - SpecChart.ChartAreas[0].AxisY.Minimum;
                    newMin = mid - 2 * radius;
                    newMax = mid + 2 * radius;
                }
                /*
                if (!controlPressed)
                {
                    Tuple<double, double> rounded = AutoRoundRange(newMin, newMax, logScale[chartNum]);
                    newMin = rounded.Item1;
                    newMax = rounded.Item2;
                }
                */

                SetChartYBounds(newMin, newMax);
                drawingYZoomBox = false;
                return;
            }

            mouseUpX = 0;
            try
            {
                mouseUpX = SpecChart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
            }
            catch
            {
                return;
            }
            drawingXZoomBox = false;
            drawingYZoomBox = false;

            double mouseDelta = mouseUpX - mouseDownX;
            double range = SpecChart.ChartAreas[0].AxisX.Maximum - SpecChart.ChartAreas[0].AxisX.Minimum;

            if (mouseDelta / range < -0.02 && !controlPressed)
            {
                // Zoom out
                SetChartRange(SpecChart.ChartAreas[0].AxisX.Minimum - range / 2,
                    SpecChart.ChartAreas[0].AxisX.Maximum + range / 2);
            }
            else if (mouseDelta / range > 0.02 && !controlPressed)
            {
                // Zoom in
                SetChartRange(mouseDownX, mouseUpX);
            }
            else if (!controlPressed ||
                ((mouseDelta / range < 0.02) && (mouseDelta / range > -0.02)))
            {
                // This is nothing - just do nothing
                drawingXZoomBox = false;
            }
            else
            {
                // TODO: Do some ROI stuff
            }
            SpecChart.Invalidate();
        }

        private void SpecChart_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
            if (mouseX < 0) mouseX = 0;
            if (mouseY < 0) mouseY = 0;
            if (drawingXZoomBox)
            {
                SpecChart.Invalidate();
                if (SpecChart.ChartAreas[0].AxisX.Maximum < SpecChart.ChartAreas[0].AxisX.PixelPositionToValue(mouseX) ||
                    (SpecChart.ChartAreas[0].AxisX.Minimum > SpecChart.ChartAreas[0].AxisX.PixelPositionToValue(mouseX)))
                {
                    SpecChart_MouseUp(SpecChart, e);
                }
            }
            if (drawingYZoomBox)
            {
                SpecChart.Invalidate();
                if (SpecChart.ChartAreas[0].AxisY.Maximum < SpecChart.ChartAreas[0].AxisY.PixelPositionToValue(mouseY) ||
                    (SpecChart.ChartAreas[0].AxisY.Minimum > SpecChart.ChartAreas[0].AxisY.PixelPositionToValue(mouseY)))
                {
                    SpecChart_MouseUp(SpecChart, e);
                }
            }
        }

        private void SpecChart_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownX = SpecChart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
            mouseDownY = SpecChart.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);

            if (e.Button == MouseButtons.Left &&
                mouseDownX >= SpecChart.ChartAreas[0].AxisX.Minimum &&
                mouseDownX <= SpecChart.ChartAreas[0].AxisX.Maximum)
            {
                drawingXZoomBox = true;
            }
            else if (e.Button == MouseButtons.Left &&
                mouseDownX < SpecChart.ChartAreas[0].AxisX.Minimum &&
                mouseDownY >= SpecChart.ChartAreas[0].AxisY.Minimum &&
                mouseDownY <= SpecChart.ChartAreas[0].AxisY.Maximum)
            {
                drawingYZoomBox = true;
            }
        }

        private void UpdateCalibration()
        {
            try
            {
                calibrationZero = double.Parse(CalZeroTextBox.Text);
                calibrationSlope = double.Parse(CalSlopeTextBox.Text);
                DrawSpectrum();
            }
            catch
            {
                MessageBox.Show("Invalid calibration!");
            }
        }

        private void CalZeroTextBox_TextChanged(object sender, EventArgs e)
        {
            calibrationChanged = true;
        }

        private void CalSlopeTextBox_TextChanged(object sender, EventArgs e)
        {
            calibrationChanged = true;
        }

        private void CalZeroTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (calibrationChanged & e.KeyCode == Keys.Enter)
            {
                UpdateCalibration();
            }
            calibrationChanged = false;
        }

        private void CalSlopeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (calibrationChanged & e.KeyCode == Keys.Enter)
            {
                UpdateCalibration();
            }
            calibrationChanged = false;
        }

        private void CalResetButton_Click(object sender, EventArgs e)
        {
            if(fileLoaded)
            {
                Spectrum spectrum;
                if (FileExt == "chn")
                {
                    spectrum = chnParser.GetSpectrum();
                }
                else if (FileExt == "spe")
                {
                    spectrum = speParser.GetSpectrum();
                }
                else if (FileExt == "n42")
                {
                    spectrum = n42Parser.GetSpectrum();
                }
                else
                {
                    spectrum = hgmParser.GetSpectrum();
                }

                calibrationZero = spectrum.GetCalibrationZero();
                calibrationSlope = spectrum.GetCalibrationSlope();
                CalZeroTextBox.Text = string.Format("{0:F3}", calibrationZero);
                CalSlopeTextBox.Text = string.Format("{0:F4}", calibrationSlope);
                DrawSpectrum();
            }
        }

        private void Inspectrum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                controlPressed = true;
            }
        }

        private void Inspectrum_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                controlPressed = false;
            }
        }

        private void SetChartRange(double min, double max)
        {
            // Limiting values
            if (min < 0) min = 0;
            if (max > maxX) max = maxX;
            if (max - min < minDelta) return;

            // Deal with scrollbar
            int scrollValue = (int)Math.Floor(min / maxX * H_SCROLL_MAX);
            if (scrollValue < 0) scrollValue = 0;
            if (scrollValue > H_SCROLL_MAX) scrollValue = H_SCROLL_MAX;
            HScroll.Value = scrollValue;
            HScroll.SmallChange = (int)(H_SCROLL_MAX * (max - min) / maxX);
            HScroll.LargeChange = (int)(H_SCROLL_MAX * (max - min) / maxX);

            // Set the actual axis
            SpecChart.ChartAreas[0].AxisX.Minimum = min;
            SpecChart.ChartAreas[0].AxisX.Maximum = max;
        }

        private void SetChartYBounds(double min, double max)
        {
            if (min < 0) min = 0;

            double maxOrderOfMagnitude = Math.Pow(10, Math.Floor(Math.Log10(max)));
            double firstDigit = Math.Floor(max / maxOrderOfMagnitude);
            double maxMinRatio = max / min;

            Tuple<double, double> result = AutoRoundRange(min, max, false);
            min = result.Item1;
            max = result.Item2;

            SpecChart.ChartAreas[0].AxisY.Minimum = min;
            SpecChart.ChartAreas[0].AxisY.Maximum = max;
        }

        private Tuple<double, double> AutoRoundRange(double min, double max, bool log)
        {
            double maxOrderOfMagnitude = Math.Pow(10, Math.Floor(Math.Log10(max)));
            double firstDigit = Math.Floor(max / maxOrderOfMagnitude);
            double maxMinRatio = max / min;

            if (log)
            {
                double minOrderOfMagnitude = Math.Pow(10, Math.Floor(Math.Log10(min)));
                return new Tuple<double, double>(minOrderOfMagnitude, maxOrderOfMagnitude * 10);
            }
            else if (maxMinRatio > 2)
            {
                return new Tuple<double, double>(0, (firstDigit + 1) * maxOrderOfMagnitude);
            }
            else
            {
                double minOrderOfMagnitude = Math.Pow(10, Math.Floor(Math.Log10(min)));
                double maxMinDifference = max - min;
                double diffOoM = Math.Pow(10, Math.Floor(Math.Log10(maxMinDifference)));

                return new Tuple<double, double>(Math.Floor(min / (diffOoM)) * diffOoM, Math.Ceiling(max / (diffOoM)) * diffOoM);
            }
        }

        private void HScroll_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type == ScrollEventType.EndScroll) return;
            if ((e.NewValue + HScroll.LargeChange >= HScroll.Maximum) &&
                (e.NewValue > e.OldValue))
            {
                e.NewValue = HScroll.Maximum - HScroll.LargeChange;
            }

            double range = SpecChart.ChartAreas[0].AxisX.Maximum - SpecChart.ChartAreas[0].AxisX.Minimum;
            double min = ((double)e.NewValue / H_SCROLL_MAX) * maxX;
            SetChartRange(min, min+range);
        }

        private void ZoomFullView()
        {
            SetChartRange(0, maxX);
            SetChartYBounds(0, maxY);
        }

        private void SpectrumNumberUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (SpectrumNumberUpDown.Enabled && hgmParser.Spectra.Count > 0)
            { 
                int spectrumNumber = (int)SpectrumNumberUpDown.Value;

                if (spectrumNumber < 1) SpectrumNumberUpDown.Value = 1;
                else if (spectrumNumber > hgmParser.Spectra.Count) SpectrumNumberUpDown.Value = hgmParser.Spectra.Count;
                else
                {
                    spectrumNumber -= 1;
                    LoadSpectrum(hgmParser.Spectra[spectrumNumber]);
                }
            }
        }
    }
}
