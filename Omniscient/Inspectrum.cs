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
        InspectrumCore Core;

        bool calibrationChanged = false;

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

        bool autoScale;
        bool logScale;

        public Inspectrum()
        {
            Core = new InspectrumCore();
            autoScale = true;
            logScale = false;
            InitializeComponent();
            SpecChart.SuppressExceptions = true;
        }

        public void EnterInstrumentMode(string startFile, DateTime startTime, string[] files, DateTime[] dates)
        {
            Core.EnterInstrumentMode(startFile, startTime, files, dates);
            RefreshDisplay();
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
            for (int i = 0; i < Core.Counts.Length; ++i) //
            {
                energy = Core.CalibrationZero + i * Core.CalibrationSlope;
                energies.Add(energy);
                vals.Add(Core.Counts[i]);

                series.Points.AddXY(energy, Core.Counts[i]);
                if (energy > maxX) maxX = energy;
                if (Core.Counts[i] > maxY) maxY = Core.Counts[i];
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
            Core.LoadSpectrumFile(fileName, specTime);
            RefreshDisplay();
        }

        public void SumAndDisplaySpectra(List<Spectrum> spectra)
        {
            Core.SumAndLoadSpectra(spectra);
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            FileNameTextBox.Text = Core.FileName;
            DateTextBox.Text = Core.SpectrumStart.ToString("dd-MMM-yyyy");
            TimeTextBox.Text = Core.SpectrumStart.ToString("HH:mm:ss");
            CalZeroTextBox.Text = string.Format("{0:F3}", Core.CalibrationZero);
            CalSlopeTextBox.Text = string.Format("{0:F4}", Core.CalibrationSlope);

            LiveTimeTextBox.Text = string.Format("{0:F1} sec", Core.SpectrumLiveTime.TotalSeconds);
            DeadTimeStripTextBox.Text = string.Format("{0:F2} %", Core.SpectrumDeadTimePercent);

            if (Core.FileSpectraCount > 1)
            {
                SpectrumNumberUpDown.Enabled = true;
                SpectrumNumberUpDown.Value = Core.FileSpectrumNumber;
            }
            else
            {
                SpectrumNumberUpDown.Enabled = false;
                SpectrumNumberUpDown.Value = 1;
            }

            if (Core.InstrumentMode)
            {
                if (Core.EarlierSpectra) PreviousSpecButton.Enabled = true;
                else PreviousSpecButton.Enabled = false;
                if (Core.LaterSpectra) NextSpecButton.Enabled = true;
                else NextSpecButton.Enabled = false;
            }
            else
            {
                NextSpecButton.Enabled = false;
                PreviousSpecButton.Enabled = false;
            }

            DrawSpectrum();
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

            if (Core.FileLoaded) DrawSpectrum();
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
            else if (e.Button == MouseButtons.Right)
            {
                CreateChartContextMenu(e);
            }
        }

        private void ToggleYAxisAutoScale(object sender, EventArgs e)
        {
            autoScale = !autoScale;
            if (autoScale)
            {
                AutoScaleRange(SpecChart.ChartAreas[0].AxisY.Minimum, SpecChart.ChartAreas[0].AxisY.Maximum);
            }
        }

        private void ToggleYAxisLogScale(object sender, EventArgs e)
        {
            logScale = !logScale;
            SpecChart.ChartAreas[0].AxisY.IsLogarithmic = logScale;
            DrawSpectrum();
        }

        public void CreateChartContextMenu(MouseEventArgs e)
        {
            ContextMenu chartMenu = new ContextMenu();

            // Toggle auto scale
            if (autoScale)
            {
                MenuItem menuItem = new MenuItem("Do not auto-scale Y-Axis");
                menuItem.Click += ToggleYAxisAutoScale;
                chartMenu.MenuItems.Add(menuItem);
            }
            else
            {
                MenuItem menuItem = new MenuItem("Auto-scale Y-Axis");
                menuItem.Click += ToggleYAxisAutoScale;
                chartMenu.MenuItems.Add(menuItem);
            }

            // Toggle log scale
            if (logScale)
            {
                MenuItem menuItem = new MenuItem("Switch to linear Y-Axis");
                menuItem.Click += ToggleYAxisLogScale;
                chartMenu.MenuItems.Add(menuItem);
            }
            else
            {
                MenuItem menuItem = new MenuItem("Switch to log Y-Axis");
                menuItem.Click += ToggleYAxisLogScale;
                chartMenu.MenuItems.Add(menuItem);
            }

            chartMenu.Show(SpecChart, new Point((int)e.X, (int)e.Y));
        }

        private void UpdateCalibration()
        {
            try
            {
                Core.CalibrationZero = double.Parse(CalZeroTextBox.Text);
                Core.CalibrationSlope = double.Parse(CalSlopeTextBox.Text);
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
            if(Core.FileLoaded)
            {
                Core.ResetCalibration();
                RefreshDisplay();
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

            if (autoScale)
            {
                AutoScaleRange(min, max);
            }
        }

        private void AutoScaleRange(double min, double max)
        {
            double m = Core.CalibrationSlope;
            double b = Core.CalibrationZero;
            double bin;
            int minCount = int.MaxValue;
            int maxCount = int.MinValue;
            int[] counts = Core.Counts;
            for (int i = 0; i < counts.Length; i++)
            {
                bin = m * i + b;
                if (bin >= min && bin <= max)
                {
                    if (counts[i] > maxCount) maxCount = counts[i];
                    if (counts[i] < minCount) minCount = counts[i];
                }
            }
            SetChartYBounds(minCount, maxCount);
        }

        private void SetChartYBounds(double min, double max)
        {
            if (autoScale)
            { 
                if (min < 0) min = 0;

                double maxOrderOfMagnitude = Math.Pow(10, Math.Floor(Math.Log10(max)));
                double firstDigit = Math.Floor(max / maxOrderOfMagnitude);
                double maxMinRatio = max / min;

                Tuple<double, double> result = ChartingUtil.AutoRoundRange(min, max, false);
                min = result.Item1;
                max = result.Item2;
            }
            SpecChart.ChartAreas[0].AxisY.Minimum = min;
            SpecChart.ChartAreas[0].AxisY.Maximum = max;
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
            if (SpectrumNumberUpDown.Enabled && Core.FileSpectraCount > 1)
            { 
                int spectrumNumber = (int)SpectrumNumberUpDown.Value;

                if (spectrumNumber < 1) SpectrumNumberUpDown.Value = 1;
                else if (spectrumNumber > Core.FileSpectraCount) SpectrumNumberUpDown.Value = Core.FileSpectraCount;
                else
                {
                    Core.SwitchFileSpectrum(spectrumNumber);
                    RefreshDisplay();
                }
            }
        }

        private void ZoomToRangeButton_Click(object sender, EventArgs e)
        {
            ZoomFullView();
        }

        private void PreviousSpecButton_Click(object sender, EventArgs e)
        {
            Core.LoadPreviousInstrumentSpectrum();
            RefreshDisplay();
        }

        private void NextSpecButton_Click(object sender, EventArgs e)
        {
            Core.LoadNextInstrumentSpectrum();
            RefreshDisplay();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV File |*.csv|chn files (*.chn)|*.chn";
            dialog.Title = "Export File";
            dialog.OverwritePrompt = true;
            dialog.ValidateNames = true;
            dialog.AddExtension = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (Core.ExportSpectrum(dialog.FileName) == ReturnCode.SUCCESS)
                {
                    MessageBox.Show("Exported spectrum!");
                }
                else
                {
                    MessageBox.Show("Export failed :-(");
                }
            }
        }
    }
}
