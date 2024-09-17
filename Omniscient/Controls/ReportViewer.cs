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

namespace Omniscient.Controls
{
    public partial class ReportViewer : Form
    {
        private void Init()
        {
            InitializeComponent();
            Icon = Properties.Resources.OmniscientIcon;
            Height = SystemInformation.PrimaryMonitorSize.Height - 60;
        }
        public ReportViewer(AnalyzerReport report)
        {
            Init();

            Text = "Omniscient Report Viewer: " + report.Analyzer.Name;
            ReportTextBox.Text = report.ToString();
        }
        public ReportViewer(string fileName)
        {
            Init();

            if (fileName.Contains('\\')) Text = "Omniscient Report Viewer: " + fileName.Substring(fileName.LastIndexOf(@"\\") + 1);
            else Text = "Omniscient Report Viewer: " + fileName;
            string report = "";
            try
            {
                report = File.ReadAllText(fileName);
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show("Failed to read report.");
                report = "";
            }
            ReportTextBox.Text = report;
        }
    }
}
