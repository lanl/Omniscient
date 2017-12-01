using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omniscient
{
    public partial class ChannelPanel : UserControl
    {

        public event EventHandler CheckChanged;

        private Channel channel;

        public ChannelPanel(Channel ch)
        {
            channel = ch;
            InitializeComponent();
        }

        private void ChannelPanel_Load(object sender, EventArgs e)
        {
            NameTextBox.Text = channel.GetName();
            Chart1CheckBox.Tag = 0;
            Chart2CheckBox.Tag = 1;
            Chart3CheckBox.Tag = 2;
            Chart4CheckBox.Tag = 3;

            NameToolTip.SetToolTip(NameTextBox, NameTextBox.Text);
        }

        public Channel GetChannel() { return channel; }

        private void Chart1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckChanged != null)
                CheckChanged(sender, e);
        }

        private void Chart2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckChanged != null)
                CheckChanged(sender, e);
        }

        private void Chart3CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckChanged != null)
                CheckChanged(sender, e);
        }

        private void Chart4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckChanged != null)
                CheckChanged(sender, e);
        }
    }
}
