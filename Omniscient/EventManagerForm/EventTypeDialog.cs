using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omniscient
{
    public partial class EventTypeDialog : Form
    {
        public string eventType;

        public EventTypeDialog()
        {
            InitializeComponent();
        }

        private void ThresholdButton_Click(object sender, EventArgs e)
        {
            eventType = "Threshold";
            DialogResult = DialogResult.OK;
            Dispose();
        }

        private void CoincidenceButton_Click(object sender, EventArgs e)
        {
            eventType = "Coincidence";
            DialogResult = DialogResult.OK;
            Dispose();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Dispose();
        }
    }
}
