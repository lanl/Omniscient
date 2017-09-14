using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Omniscient.Parsers;

namespace Omniscient
{
    public partial class Inspectrum : Form
    {

        CHNParser chnParser;

        public Inspectrum()
        {
            InitializeComponent();
        }

        private void LoadCHNFile(string fileName)
        {
            if (chnParser.ParseFile(fileName) == CHNParser.ReturnCode.SUCCESS)
            {
                FileNameTextBox.Text = fileName;
                DateTextBox.Text = chnParser.GetStartDateTime().ToString("dd-MMM-yyyy");
                TimeTextBox.Text = chnParser.GetStartDateTime().ToString("HH:mm:ss");
                CalZeroTextBox.Text = string.Format("{0:F3}", chnParser.GetCalibrationZero());
                CalSlopeTextBox.Text = string.Format("{0:F4}", chnParser.GetCalibrationSlope());

                LiveTimeTextBox.Text = string.Format("{0:F1} sec", chnParser.GetLiveTime());
                double deadTimePerc = 100 * (chnParser.GetRealTime() - chnParser.GetLiveTime()) / chnParser.GetRealTime();
                DeadTimeStripTextBox.Text = string.Format("{0:F2} %", deadTimePerc);

            }
            else
            {
                MessageBox.Show("Error opening file!");
            }

        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "chn files (*.chn)|*.chn|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadCHNFile(openFileDialog.FileName);
            }

        }

        private void OpenToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void Inspectrum_Load(object sender, EventArgs e)
        {
            chnParser = new CHNParser();
        }
    }
}
