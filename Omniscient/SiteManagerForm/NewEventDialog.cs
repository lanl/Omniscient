using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Omniscient.Instruments;

namespace Omniscient
{
    public partial class NewEventDialog : Form
    {
        DetectionSystem sys;

        public string name;
        public Channel channel;
        public double threshold;

        public NewEventDialog(DetectionSystem newSys)
        {
            sys = newSys;
            InitializeComponent();
        }

        private void NewEventDialog_Load(object sender, EventArgs e)
        {
            ChannelComboBox.Items.Clear();
            
            foreach (Instrument inst in sys.GetInstruments())
            {
                foreach (Channel ch in inst.GetChannels())
                {
                    ChannelComboBox.Items.Add(ch.GetName());
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Dispose();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (NameTextBox.Text == "")
            {
                MessageBox.Show("You must name your Event Generator!");
                return;
            }
            name = NameTextBox.Text;

            if (ChannelComboBox.Text == "")
            {
                MessageBox.Show("You must select a Channel!");
                return;
            }

            foreach (Instrument inst in sys.GetInstruments())
            {
                foreach (Channel ch in inst.GetChannels())
                {
                    if (ch.GetName() == ChannelComboBox.Text)
                    {
                        channel = ch;
                        break;
                    }
                }
            }

            try
            {
                threshold = double.Parse(ThresholdTextBox.Text);
            }
            catch
            {
                MessageBox.Show("You must enter a valid threshold!");
                return;
            }

            DialogResult = DialogResult.OK;
            Dispose();
        }
    }
}
