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
    public partial class InstrumentTypeDialog : Form
    {
        public string InstrumentType = "";

        public InstrumentTypeDialog()
        {
            InitializeComponent();
        }

        private void GRANDButton_Click(object sender, EventArgs e)
        {
            InstrumentType = "GRAND";
            DialogResult = DialogResult.OK;
            Dispose();
        }

        private void ISRButton_Click(object sender, EventArgs e)
        {
            InstrumentType = "ISR";
            DialogResult = DialogResult.OK;
            Dispose();
        }

        private void MCAButton_Click(object sender, EventArgs e)
        {
            InstrumentType = "MCA";
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
