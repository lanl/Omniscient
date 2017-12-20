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
    public partial class EventViewerForm : Form
    {
        Event eve;

        public EventViewerForm(Event newEvent)
        {
            eve = newEvent;
            InitializeComponent();
        }

        private void EventViewerForm_Load(object sender, EventArgs e)
        {
            EventGeneratorTextBox.Text = eve.GetEventGenerator().GetName();
            StartTimeTextBox.Text = eve.GetStartTime().ToString("MM/dd/yy HH:mm:ss");
            EndTimeTextBox.Text = eve.GetEndTime().ToString("MM/dd/yy HH:mm:ss");
            DurationTextBox.Text = eve.GetDuration().TotalSeconds.ToString() + " s";
            MaxValueTextBox.Text = eve.GetMaxValue().ToString();
            MaxTimeTextBox.Text = eve.GetMaxTime().ToString("MM/dd/yy HH:mm:ss");
            CommentTextBox.Text = eve.GetComment();
        }
    }
}
