using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omniscient.MainDialogs
{
    public partial class GenerateEventsDialog : Form
    {
        OmniscientCore Core;

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public GenerateEventsDialog(OmniscientCore core)
        {
            InitializeComponent();
            Core = core;

            StartDatePicker.Value = Core.GlobalStart;
            StartTimePicker.Value = Core.GlobalStart;
            EndDatePicker.Value = Core.GlobalEnd;
            EndTimePicker.Value = Core.GlobalEnd;
        }

        private void GenerateEventsDialog_Load(object sender, EventArgs e)
        {
            
        }

        private void AllButton_Click(object sender, EventArgs e)
        {
            StartDatePicker.Value = Core.GlobalStart;
            StartTimePicker.Value = Core.GlobalStart;
            EndDatePicker.Value = Core.GlobalEnd;
            EndTimePicker.Value = Core.GlobalEnd;
        }

        private void ChartRangeButton_Click(object sender, EventArgs e)
        {
            StartDatePicker.Value = Core.ViewStart;
            StartTimePicker.Value = Core.ViewStart;
            EndDatePicker.Value = Core.ViewEnd;
            EndTimePicker.Value = Core.ViewEnd;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Dispose();
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            // Validate date range
            StartTime = StartDatePicker.Value.Date + StartTimePicker.Value.TimeOfDay;
            EndTime = EndDatePicker.Value.Date + EndTimePicker.Value.TimeOfDay;
            if (StartTime >= EndTime)
            {
                MessageBox.Show("Negative or 0 time range selected!");
                return;
            }

            // Ok!
            DialogResult = DialogResult.OK;
            Dispose();
        }
    }
}
