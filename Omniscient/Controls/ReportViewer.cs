using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omniscient.Controls
{
    public partial class ReportViewer : Form
    {
        public ReportViewer(AnalyzerReport report)
        {
            InitializeComponent();
            Icon = Properties.Resources.OmniscientIcon;
            Height = SystemInformation.PrimaryMonitorSize.Height - 60;
            

            Text = "Omniscient Report Viewer: " + report.Analyzer.Name;
            ReportTextBox.Text = report.ToString();
        }
    }
}
