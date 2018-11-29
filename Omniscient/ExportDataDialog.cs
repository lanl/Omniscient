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
                InstrumentComboBox.Items.Add(instrument.GetName());
            }
            InstrumentComboBox.SelectedIndex = 0;

            FileButton.Image = Properties.Resources.OpenIcon;

            StartDatePicker.Value = GlobalStart;
            StartTimePicker.Value = GlobalStart;
            EndDatePicker.Value = GlobalEnd;
            EndTimePicker.Value = GlobalEnd;
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
            foreach(Instrument instrument in Instruments)
            {
                if (instrument.GetName() == InstrumentComboBox.Text)
                {
                    SelectedInstrument = instrument;
                    break;
                }
            }

            ChannelTreeView.Nodes.Clear();
            foreach(Channel channel in SelectedInstrument.GetChannels())
            {
                TreeNode cNode = new TreeNode(channel.GetName());
                cNode.Tag = channel;
                cNode.Checked = true;
                ChannelTreeView.Nodes.Add(cNode);
            }
        }

        private void FileButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV File |*.csv";
            dialog.Title = "Export File";
            dialog.OverwritePrompt = true;
            dialog.ValidateNames = true;
            dialog.AddExtension = true;

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                FileTextBox.Text = dialog.FileName;
            }
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
            try
            {
                List<DateTime> dates = selectedChannels[0].GetTimeStamps();
                using (StreamWriter file = new StreamWriter(fileName))
                {
                    // Headers
                    if (WriteHeadersCheckBox.Checked)
                    {
                        file.Write("Time Stamp");
                        for (int c = 0; c < selectedChannels.Count; c++)
                        {
                            file.Write("," + selectedChannels[c].GetName());
                        }
                        file.Write("\r\n");
                    }

                    // Content
                    for (int i = 0; i < dates.Count; i++)
                    {
                        if (dates[i] >= start && dates[i] <= end)
                        {
                            file.Write(dates[i].ToString("yyyy-MM-dd HH:mm:ss"));
                            for (int c = 0; c < selectedChannels.Count; c++)
                            {
                                file.Write("," + selectedChannels[c].GetValues()[i]);
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
    }
}
