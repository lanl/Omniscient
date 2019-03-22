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
    public partial class ChartOptionsDialog : Form
    {
        public double YMin { get; private set; }
        public double YMax { get; private set; }
        public ChartOptionsDialog(int chartNum, double yMin, double yMax)
        {
            InitializeComponent();
            YMin = yMin;
            YMax = yMax;

            BottomTextBox.Text = YMin.ToString();
            TopTextBox.Text = YMax.ToString();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Dispose();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            OK();
        }

        private void OK()
        {
            double yMin;
            double yMax;

            // Validate yMin
            if (!double.TryParse(BottomTextBox.Text, out yMin))
            {
                MessageBox.Show("Y-Axis Bottom must be a valid number!");
                return;
            }
            if (!(yMin >= (double)decimal.MinValue))
            {
                MessageBox.Show("Y-Axis Bottom must be at least " + decimal.MinValue.ToString());
                return;
            }
            if (!(yMin <= (double)decimal.MaxValue))
            {
                MessageBox.Show("Y-Axis Bottom must be at most " + decimal.MaxValue.ToString());
                return;
            }

            // Validate yMin
            if (!double.TryParse(TopTextBox.Text, out yMax))
            {
                MessageBox.Show("Y-Axis Top must be a valid number!");
                return;
            }
            if (!(yMax >= (double)decimal.MinValue))
            {
                MessageBox.Show("Y-Axis Top must be at least " + decimal.MinValue.ToString());
                return;
            }
            if (!(yMax <= (double)decimal.MaxValue))
            {
                MessageBox.Show("Y-Axis Top must be at most " + decimal.MaxValue.ToString());
                return;
            }

            // Make sure yMax is larger than yMin
            if (yMax <= yMin)
            {
                MessageBox.Show("Y-Axis Top must greater than Y-Axis Bottom!" + decimal.MaxValue.ToString());
                return;
            }

            YMin = yMin;
            YMax = yMax;
            DialogResult = DialogResult.OK;
            Dispose();
        }

        private void BottomTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                OK();
            }
        }

        private void TopTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                OK();
            }
        }
    }
}
