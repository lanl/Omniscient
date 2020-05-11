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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omniscient
{
    public partial class ExportDataDialog : Form
    {
        DateTime GlobalStart { get; set; }
        DateTime GlobalEnd { get; set; }
        DateTime ChartStartDate { get; set; }
        DateTime ChartStartTime { get; set; }
        DateTime ChartEndDate { get; set; }
        DateTime ChartEndTime { get; set; }
        List<Instrument> Instruments { get; set; }
        Instrument SelectedInstrument { get; set; }

        private string dialogFilter;
        public ExportDataDialog(List<Instrument> instruments, DateTime globalStart, DateTime globalEnd, DateTime startDate, DateTime startTime, DateTime endDate, DateTime endTime)
        {
            InitializeComponent();
            Instruments = instruments;
            GlobalStart = globalStart;
            GlobalEnd = globalEnd;
            ChartStartTime = startTime;
            ChartStartDate = startDate;
            ChartEndTime = endTime;
            ChartEndDate = endDate;

            InstrumentComboBox.Items.Clear();
            foreach(Instrument instrument in Instruments)
            {
                InstrumentComboBox.Items.Add(instrument);
            }
            InstrumentComboBox.SelectedIndex = 0;

            FileButton.Image = Properties.Resources.OpenIcon;

            StartDatePicker.Value = ChartStartDate;
            StartTimePicker.Value = ChartStartTime;
            EndDatePicker.Value = ChartEndDate;
            EndTimePicker.Value = ChartEndTime;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Dispose();
        }

        private void AllButton_Click(object sender, EventArgs e)
        {
            StartDatePicker.Value = GlobalStart;
            StartTimePicker.Value = GlobalStart;
            EndDatePicker.Value = GlobalEnd;
            EndTimePicker.Value = GlobalEnd;
        }

        private void ChartRangeButton_Click(object sender, EventArgs e)
        {
            StartDatePicker.Value = ChartStartDate;
            StartTimePicker.Value = ChartStartTime;
            EndDatePicker.Value = ChartEndDate;
            EndTimePicker.Value = ChartEndTime;
        }

        private void InstrumentComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedInstrument = InstrumentComboBox.SelectedItem as Instrument;

            ChannelTreeView.Nodes.Clear();
            foreach(Channel channel in SelectedInstrument.GetChannels())
            {
                TreeNode cNode = new TreeNode(channel.Name);
                cNode.Tag = channel;
                cNode.Checked = true;
                ChannelTreeView.Nodes.Add(cNode);
            }
        }

        private void FileButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = dialogFilter;
            dialog.Title = "Export File";
            dialog.OverwritePrompt = true;
            dialog.ValidateNames = true;
            dialog.AddExtension = true;

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                FileTextBox.Text = dialog.FileName;
            }
        }

        private void ExportCSV(string fileName, List<Channel> selectedChannels, DateTime start, DateTime end)
        {
            try
            {
                foreach (Channel chan in selectedChannels) chan.GetInstrument().LoadData(ChannelCompartment.Process, start, end);
                List<DateTime> dates = selectedChannels[0].GetTimeStamps(ChannelCompartment.Process);
                using (StreamWriter file = new StreamWriter(fileName))
                {
                    // Headers
                    if (WriteHeadersCheckBox.Checked)
                    {
                        file.Write("Time Stamp");
                        for (int c = 0; c < selectedChannels.Count; c++)
                        {
                            file.Write("," + selectedChannels[c].Name);
                        }
                        file.Write("\r\n");
                    }

                    // Content
                    for (int i = 0; i < dates.Count; i++)
                    {
                        if (dates[i] >= start && dates[i] <= end)
                        {
                            file.Write(dates[i].ToString("yyyy-MM-dd HH:mm:ss.fff"));
                            for (int c = 0; c < selectedChannels.Count; c++)
                            {
                                file.Write("," + selectedChannels[c].GetValues(ChannelCompartment.Process)[i]);
                            }
                            file.Write("\r\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export Error\n" + ex.Message);
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                return;
            }
            MessageBox.Show("Export complete!");
            DialogResult = DialogResult.OK;
            Dispose();
        }

        private void ExportNCC(string fileName, List<Channel> selectedChannels, DateTime start, DateTime end)
        {
            // Validate detector ID
            if (DetectorIDTextBox.Text.Length > 11)
            {
                MessageBox.Show("Detector ID must be 11 or fewer characters");
                return;
            }
            while (DetectorIDTextBox.Text.Length < 11)
            {
                DetectorIDTextBox.Text += " ";
            }

            // Validate item ID
            if (ItemIDTextBox.Text.Length > 12)
            {
                MessageBox.Show("Item ID must be 12 or fewer characters");
                return;
            }
            while (ItemIDTextBox.Text.Length < 12)
            {
                ItemIDTextBox.Text += " ";
            }

            NCCWriter writer = new NCCWriter();
            if (MeasurementTypeComboBox.SelectedIndex == MeasurementTypeComboBox.FindStringExact("Background"))
            {
                writer.NCCMode = NCCWriter.NCCType.BACKGROUND;
            }
            else if (MeasurementTypeComboBox.SelectedIndex == MeasurementTypeComboBox.FindStringExact("Normalization"))
            {
                writer.NCCMode = NCCWriter.NCCType.NORMALIZATION;
            }
            if (MeasurementTypeComboBox.SelectedIndex == MeasurementTypeComboBox.FindStringExact("Verification"))
            {
                writer.NCCMode = NCCWriter.NCCType.VERIFICATION;
            }

            writer.SetDetectorID(DetectorIDTextBox.Text);
            writer.SetItemID(ItemIDTextBox.Text);

            writer.Cycles = new List<NCCWriter.Cycle>();

            foreach (Channel chan in selectedChannels) chan.GetInstrument().LoadData(ChannelCompartment.Process, start, end);
            List<DateTime> dates = selectedChannels[0].GetTimeStamps(ChannelCompartment.Process);

            for (int i = 0; i < dates.Count; i++)
            {
                if (dates[i] >= start && dates[i] <= end)
                {
                    NCCWriter.Cycle cycle = new NCCWriter.Cycle();
                    cycle.DateAndTime = dates[i];
                    if (i < dates.Count - 1)
                    {
                        cycle.CountSeconds = (ushort)(dates[i + 1] - dates[i]).TotalSeconds;
                    }
                    else
                    {
                        cycle.CountSeconds = (ushort)(dates[i] - dates[i - 1]).TotalSeconds;
                    }

                    cycle.Totals = selectedChannels[0].GetValues(ChannelCompartment.Process)[i];

                    switch (selectedChannels.Count)
                    {
                        case 1:
                            cycle.RPlusA = 0;
                            cycle.A = 0;
                            cycle.Scaler1 = 0;
                            cycle.Scaler2 = 0;
                            break;
                        case 2:
                            cycle.RPlusA = selectedChannels[1].GetValues(ChannelCompartment.Process)[i];
                            cycle.A = 0;
                            cycle.Scaler1 = 0;
                            cycle.Scaler2 = 0;
                            break;
                        case 3:
                            cycle.RPlusA = selectedChannels[1].GetValues(ChannelCompartment.Process)[i];
                            cycle.A = selectedChannels[2].GetValues(ChannelCompartment.Process)[i];
                            cycle.Scaler1 = 0;
                            cycle.Scaler2 = 0;
                            break;
                        case 4:
                            cycle.RPlusA = selectedChannels[1].GetValues(ChannelCompartment.Process)[i];
                            cycle.A = selectedChannels[2].GetValues(ChannelCompartment.Process)[i];
                            cycle.Scaler1 = selectedChannels[3].GetValues(ChannelCompartment.Process)[i];
                            cycle.Scaler2 = 0;
                            break;
                        default:
                            cycle.RPlusA = selectedChannels[1].GetValues(ChannelCompartment.Process)[i];
                            cycle.A = selectedChannels[2].GetValues(ChannelCompartment.Process)[i];
                            cycle.Scaler1 = selectedChannels[3].GetValues(ChannelCompartment.Process)[i];
                            cycle.Scaler2 = selectedChannels[4].GetValues(ChannelCompartment.Process)[i];
                            break;
                    }
                    writer.Cycles.Add(cycle);
                }
            }

            try
            {
                writer.WriteNeutronCyclesFile(FileTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export Error\n" + ex.Message);
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                return;
            }

            MessageBox.Show("Export complete!");
            DialogResult = DialogResult.OK;
            Dispose();
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            // Validate channels
            List<Channel> selectedChannels = new List<Channel>();
            foreach (TreeNode node in ChannelTreeView.Nodes)
            {
                if (node.Checked) selectedChannels.Add((Channel)node.Tag);
            }
            if (selectedChannels.Count == 0)
            {
                MessageBox.Show("No channels selected!");
                return;
            }

            // Validate date range
            DateTime start = StartDatePicker.Value.Date + StartTimePicker.Value.TimeOfDay;
            DateTime end = EndDatePicker.Value.Date + EndTimePicker.Value.TimeOfDay;
            if (start >= end)
            {
                MessageBox.Show("Negative or 0 time range selected!");
                return;
            }

            // Validate file
            string fileName = FileTextBox.Text;
            try
            {
                File.WriteAllText(fileName, "");
            }
            catch
            {
                MessageBox.Show("Cannot write to file!");
                return;
            }

            // All tests pass: let's do this
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            if (FileFormatComboBox.SelectedIndex == FileFormatComboBox.FindStringExact("CSV"))
            {
                ExportCSV(fileName, selectedChannels, start, end);
            }
            else if (FileFormatComboBox.SelectedIndex == FileFormatComboBox.FindStringExact("NCC"))
            {
                ExportNCC(fileName, selectedChannels, start, end);
            }
        }

        private void ExportDataDialog_Load(object sender, EventArgs e)
        {
            FileFormatComboBox.SelectedIndex = FileFormatComboBox.FindStringExact("CSV");
            MeasurementTypeComboBox.SelectedIndex = MeasurementTypeComboBox.FindStringExact("Verification");
        }

        private void FileFormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileTextBox.Text = "";

            if (FileFormatComboBox.SelectedIndex == FileFormatComboBox.FindStringExact("CSV"))
            {
                dialogFilter = "CSV File |*.csv";

                WriteHeadersCheckBox.Enabled = true;

                MeasurementTypeComboBox.Enabled = false;
                DetectorIDTextBox.Enabled = false;
                ItemIDTextBox.Enabled = false;
            }
            else if (FileFormatComboBox.SelectedIndex == FileFormatComboBox.FindStringExact("NCC"))
            {
                dialogFilter = "NCC File |*.ncc";

                WriteHeadersCheckBox.Enabled = false;

                MeasurementTypeComboBox.Enabled = true;
                DetectorIDTextBox.Enabled = true;
                ItemIDTextBox.Enabled = true;
            }
        }

        private void SelectNoneButton_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in ChannelTreeView.Nodes)
            {
                node.Checked = false;
            }
        }
    }
}
