using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Omniscient.MainDialogs
{
    public partial class ReportSelector : Form
    {
        public DetectionSystem DetSystem { get; private set; }
        public Dictionary<string, Dictionary<string, string>> Report {  get; private set; }

        int selectedRow;
        public ReportSelector(DetectionSystem detSystem, string analyzer="Any")
        {
            InitializeComponent();
            Icon = Properties.Resources.OmniscientIcon;
            DetSystem = detSystem;
            Report = null;

            DetSystem.LoadReports();
            PopulateAnalyzerComboBox(analyzer);

            if (analyzer != "Any")
            {
                AnalyzerComboBox.Enabled = false;
                this.Text = "Select Report from " + analyzer;
            }
            selectedRow = -1;
        }

        private void PopulateAnalyzerComboBox(string analyzer)
        {
            Dictionary<string, Dictionary<string, string>> report;
            List<string> analyzers = new List<string>();
            for (int i = 0; i < DetSystem.Reports.Count; i++)
            {
                report = DetSystem.Reports[i];

                if (!analyzers.Contains(report["Header"]["Type"]))
                {
                    analyzers.Add(report["Header"]["Type"]);
                }
            }
            
            analyzers.Sort();
            analyzers.Insert(0, "Any");
            ReportGrid.AutoResizeColumns();

            AnalyzerComboBox.Items.Clear();
            AnalyzerComboBox.Items.AddRange(analyzers.ToArray());
            AnalyzerComboBox.Text = analyzer;
        }

        private void DisplayReports(string analyzer)
        {
            int index;
            Dictionary<string, Dictionary<string, string>> report;
            List<string> analyzers = new List<string>();

            ReportGrid.Rows.Clear();
            for (int i=0; i < DetSystem.Reports.Count; i++)
            {
                report = DetSystem.Reports[i];
                if (analyzer == "Any" || analyzer == report["Header"]["Type"])
                {
                    string fileName = report["Header"]["File Name"];
                    if (fileName.Contains('\\')) fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                    if (report.ContainsKey("Event"))
                    {
                        index = ReportGrid.Rows.Add(report["Header"]["Type"], report["Event"]["Start Time"], fileName);
                    }
                    else
                    {
                        index = ReportGrid.Rows.Add(report["Header"]["Type"], "", fileName);
                    }
                    ReportGrid.Rows[index].Tag = report;
                }
            }
            Report = null;
            OkButton.Enabled = false;
            DeleteButton.Enabled = false;
            selectedRow = -1;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (Report != null)
            {
                DialogResult = DialogResult.OK;
                Dispose();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult= DialogResult.Cancel;
            Dispose();
        }

        private void ReportGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Report = ReportGrid.Rows[e.RowIndex].Tag as Dictionary<string, Dictionary<string, string>>;
                OkButton.Enabled = true;
                DeleteButton.Enabled = true;
                selectedRow = e.RowIndex;
            }
        }

        private void AnalyzerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayReports(AnalyzerComboBox.Text);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Report is null) return;
                DialogResult dialogResult = MessageBox.Show("Are you sure your want to delete the report?", "Confirm Delete Report", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string fileName = Report["Header"]["File Name"];
                    File.Delete(fileName);
                    ReportGrid.Rows.RemoveAt(selectedRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught while attempting to delete report:\n" + ex.Message);
            }
        }
    }
}
