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
    public partial class SiteManagerForm : Form
    {
        SiteManager siteMan;

        public SiteManagerForm(SiteManager newSiteMan)
        {
            siteMan = newSiteMan;
            InitializeComponent();
        }

        private void SiteManagerForm_Load(object sender, EventArgs e)
        {
            UpdateSitesTree();
        }

        /// <summary>
        /// UpdateSitesTree() builds the tree view of the SiteManager.</summary>
        private void UpdateSitesTree()
        {
            SitesTreeView.Nodes.Clear();
            foreach (Site site in siteMan.GetSites())
            {
                TreeNode siteNode = new TreeNode(site.GetName());
                siteNode.Tag = site;
                foreach (Facility fac in site.GetFacilities())
                {
                    TreeNode facNode = new TreeNode(fac.GetName());
                    facNode.Tag = fac;
                    foreach (DetectionSystem sys in fac.GetSystems())
                    {
                        TreeNode sysNode = new TreeNode(sys.GetName());
                        sysNode.Tag = sys;
                        foreach (Instrument inst in sys.GetInstruments())
                        {
                            TreeNode instNode = new TreeNode(inst.GetName());
                            instNode.Tag = inst;
                            sysNode.Nodes.Add(instNode);
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

        private void SitesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            NameTextBox.Text = e.Node.Text;

            if (e.Node.Tag is Site)
            {
                Site site = (Site) e.Node.Tag;
                TypeLabel.Text = "Site";
                InstTypeComboBox.Enabled = false;
                InstTypeComboBox.Text = "";
                PrefixTextBox.Enabled = false;
                PrefixTextBox.Text = "";
                DirectoryTextBox.Enabled = false;
                DirectoryTextBox.Text = "";
            }
            else if (e.Node.Tag is Facility)
            {
                Facility fac = (Facility)e.Node.Tag;
                TypeLabel.Text = "Facility";
                InstTypeComboBox.Enabled = false;
                InstTypeComboBox.Text = "";
                PrefixTextBox.Enabled = false;
                PrefixTextBox.Text = "";
                DirectoryTextBox.Enabled = false;
                DirectoryTextBox.Text = "";
            }
            else if (e.Node.Tag is DetectionSystem)
            {
                DetectionSystem sys = (DetectionSystem)e.Node.Tag;
                TypeLabel.Text = "System";
                InstTypeComboBox.Enabled = false;
                InstTypeComboBox.Text = "";
                PrefixTextBox.Enabled = false;
                PrefixTextBox.Text = "";
                DirectoryTextBox.Enabled = false;
                DirectoryTextBox.Text = "";
            }
            else if (e.Node.Tag is Instrument)
            {
                Instrument inst = (Instrument)e.Node.Tag;
                TypeLabel.Text = "Instrument";
                InstTypeComboBox.Enabled = true;
                if (inst is MCAInstrument)
                    InstTypeComboBox.Text = "MCA";
                else if (inst is ISRInstrument)
                    InstTypeComboBox.Text = "ISR";
                else if (inst is GRANDInstrument)
                    InstTypeComboBox.Text = "GRAND";
                PrefixTextBox.Enabled = true;
                PrefixTextBox.Text = inst.GetFilePrefix();
                DirectoryTextBox.Enabled = true;
                DirectoryTextBox.Text = inst.GetDataFolder();
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
