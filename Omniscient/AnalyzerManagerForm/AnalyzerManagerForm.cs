using Omniscient.Controls;
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
    public partial class AnalyzerManagerForm : Form
    {
        MainForm main;
        SiteManager siteMan;

        bool siteManChanged = false;

        public AnalyzerManagerForm(MainForm master, SiteManager newSiteMan)
        {
            main = master;
            siteMan = newSiteMan;

            InitializeComponent();
        }

        private void AnalyzerManagerForm_Load(object sender, EventArgs e)
        {
            SitesTreeView.ImageList = main.TreeImageList;
            UpdateSitesTree();
        }

        /// <summary>
        /// UpdateSitesTree() builds the tree view of the SiteManager.</summary>
        private void UpdateSitesTree()
        {
            SitesTreeView.Nodes.Clear();
            foreach (Site site in siteMan.GetSites())
            {
                TreeNode siteNode = new TreeNode(site.Name);
                siteNode.Name = site.ID.ToString();
                siteNode.Tag = site;
                siteNode.ImageIndex = 0;
                siteNode.SelectedImageIndex = 0;
                siteNode.ToolTipText = siteNode.Text;
                foreach (Facility fac in site.GetFacilities())
                {
                    TreeNode facNode = new TreeNode(fac.Name);
                    facNode.Name = fac.ID.ToString();
                    facNode.Tag = fac;
                    facNode.ImageIndex = 1;
                    facNode.SelectedImageIndex = 1;
                    facNode.ToolTipText = facNode.Text;
                    foreach (DetectionSystem sys in fac.GetSystems())
                    {
                        TreeNode sysNode = new TreeNode(sys.Name);
                        sysNode.Name = sys.ID.ToString();
                        sysNode.Tag = sys;
                        sysNode.ImageIndex = 2;
                        sysNode.SelectedImageIndex = 2;
                        sysNode.ToolTipText = sysNode.Text;
                        foreach (Analyzer analyzer in sys.GetAnalyzers())
                        {
                            TreeNode analyzerNode = new TreeNode(analyzer.Name);
                            analyzerNode.Name = analyzer.ID.ToString();
                            analyzerNode.Tag = analyzer;
                            analyzerNode.ImageIndex = 3;
                            analyzerNode.SelectedImageIndex = 3;
                            analyzerNode.ToolTipText = analyzerNode.Text;
                            sysNode.Nodes.Add(analyzerNode);
                        }
                        facNode.Nodes.Add(sysNode);
                    }
                    facNode.Expand();
                    siteNode.Nodes.Add(facNode);
                }
                siteNode.Expand();
                SitesTreeView.Nodes.Add(siteNode);
            }
        }

        public void ResetFields()
        {
            TreeNode node = SitesTreeView.SelectedNode;

            if (node.Tag is Analyzer)
            {
                DetectionSystem eventWatcher = (DetectionSystem)node.Parent.Tag;

                Analyzer analyzer = (Analyzer)node.Tag;
                List<Parameter> parameters = analyzer.CustomParameters.Select(c => c.Value.Parameter).ToList();
                ParamListPanel.LoadParameters(parameters);
                ParamListPanel.Visible = true;
                NameTextBox.Text = analyzer.Name;

                SaveButton.Enabled = true;
            }
            else if (node.Tag is DetectionSystem)
            {
                NameTextBox.Text = "";
                NameTextBox.Enabled = false;

                ParamListPanel.Visible = false;

                SaveButton.Enabled = false;
            }
            else
            {
                NameTextBox.Text = "";
                NameTextBox.Enabled = false;

                ParamListPanel.Visible = false;
                SaveButton.Enabled = false;
            }
        }

        private void SitesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ResetFields();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            if (siteManChanged)
            {
                main.ClearPanels();
                if (main.Core.SiteManager.Reload() != ReturnCode.SUCCESS) MessageBox.Show("Warning: Bad trouble loading the site manager!");
                if (main.presetMan.Reload() != ReturnCode.SUCCESS) MessageBox.Show("Warning: Bad trouble loading the preset manager!");
                main.UpdateSitesTree();
            }
            Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            TreeNode node = SitesTreeView.SelectedNode;

            if (node.Tag is Analyzer)
            {
                DetectionSystem eventWatcher = (DetectionSystem)node.Parent.Tag;
                Analyzer analyzer = (Analyzer)node.Tag;

                if (!ParamListPanel.ValidateInput()) return;
                ParamListPanel.Scrape();
                foreach (Parameter param in ParamListPanel.Parameters)
                {
                    analyzer.CustomParameters[param.Name].Parameter.Value = param.Value;
                }

                siteMan.Save();
                UpdateSitesTree();
                siteManChanged = true;
                SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(analyzer.ID.ToString(), true)[0];
            }
        }
    }
}
