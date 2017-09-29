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
        MainForm main;
        SiteManager siteMan;

        bool siteManChanged = false;

        public SiteManagerForm(MainForm master, SiteManager newSiteMan)
        {
            main = master;
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

        private void ResetFields()
        {
            TreeNode node = SitesTreeView.SelectedNode;
            NameTextBox.Text = node.Text;

            if (node.Tag is Site)
            {
                Site site = (Site)node.Tag;
                TypeLabel.Text = "Site";
                InstTypeComboBox.Enabled = false;
                InstTypeComboBox.Text = "";
                PrefixTextBox.Enabled = false;
                PrefixTextBox.Text = "";
                DirectoryTextBox.Enabled = false;
                DirectoryTextBox.Text = "";
                DirectoryButton.Enabled = false;

                NewInstrumentButton.Enabled = false;
                NewSystemButton.Enabled = false;
            }
            else if (node.Tag is Facility)
            {
                Facility fac = (Facility)node.Tag;
                TypeLabel.Text = "Facility";
                InstTypeComboBox.Enabled = false;
                InstTypeComboBox.Text = "";
                PrefixTextBox.Enabled = false;
                PrefixTextBox.Text = "";
                DirectoryTextBox.Enabled = false;
                DirectoryTextBox.Text = "";
                DirectoryButton.Enabled = false;

                NewInstrumentButton.Enabled = false;
                NewSystemButton.Enabled = true;
            }
            else if (node.Tag is DetectionSystem)
            {
                DetectionSystem sys = (DetectionSystem)node.Tag;
                TypeLabel.Text = "System";
                InstTypeComboBox.Enabled = false;
                InstTypeComboBox.Text = "";
                PrefixTextBox.Enabled = false;
                PrefixTextBox.Text = "";
                DirectoryTextBox.Enabled = false;
                DirectoryTextBox.Text = "";
                DirectoryButton.Enabled = false;

                NewInstrumentButton.Enabled = true;
                NewSystemButton.Enabled = true;
            }
            else if (node.Tag is Instrument)
            {
                Instrument inst = (Instrument)node.Tag;
                TypeLabel.Text = "Instrument";
                InstTypeComboBox.Enabled = false;
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
                DirectoryButton.Enabled = true;

                NewInstrumentButton.Enabled = true;
                NewSystemButton.Enabled = true;
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
                main.siteMan.Reload();
                if (main.presetMan.Reload() != ReturnCode.SUCCESS) MessageBox.Show("Warning: Bad trouble loading the preset manager!");
                main.UpdateSitesTree();
            }
            Close();
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "xml files (*.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                siteMan.WriteToXML(saveFileDialog.FileName);
            }
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "xml files (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                siteMan.LoadFromXML(openFileDialog.FileName);
            }
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
        }

        private void DiscardButton_Click(object sender, EventArgs e)
        {
            ResetFields();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            TreeNode node = SitesTreeView.SelectedNode;

            if (node.Tag is Site)
            {
                Site site = (Site)node.Tag;
                site.SetName(NameTextBox.Text);
            }
            else if (node.Tag is Facility)
            {
                Facility fac = (Facility)node.Tag;
                fac.SetName(NameTextBox.Text);
            }
            else if (node.Tag is DetectionSystem)
            {
                DetectionSystem sys = (DetectionSystem)node.Tag;
                sys.SetName(NameTextBox.Text);
            }
            else if (node.Tag is Instrument)
            {
                Instrument inst = (Instrument)node.Tag;

                inst.SetName(NameTextBox.Text);
                inst.SetFilePrefix(PrefixTextBox.Text);
                inst.SetDataFolder(DirectoryTextBox.Text);
            }
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            TreeNode node = SitesTreeView.SelectedNode;
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete " + node.Text + "?", "Delete Item", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
                return;

            if (node.Tag is Site)
            {
                Site site = (Site)node.Tag;
                siteMan.GetSites().Remove(site);
            }
            else if (node.Tag is Facility)
            {
                Facility fac = (Facility)node.Tag;
                Site site = (Site)node.Parent.Tag;
                site.GetFacilities().Remove(fac);
            }
            else if (node.Tag is DetectionSystem)
            {
                DetectionSystem sys = (DetectionSystem)node.Tag;
                Facility fac = (Facility)node.Parent.Tag;
                fac.GetSystems().Remove(sys);
            }
            else if (node.Tag is Instrument)
            {
                Instrument inst = (Instrument)node.Tag;
                DetectionSystem sys = (DetectionSystem)node.Parent.Tag;
                sys.GetInstruments().Remove(inst);
            }
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
        }

        private void DirectoryButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                DirectoryTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void NewSiteButton_Click(object sender, EventArgs e)
        {
            bool uniqueName = false;
            int iteration = 0;

            string name = "";
            while (!uniqueName)
            {
                iteration++;
                name = "New-Site-" + iteration.ToString();
                uniqueName = true;
                foreach(Site site in siteMan.GetSites())
                {
                    if (site.GetName() == name) uniqueName = false;
                }
            }
            siteMan.GetSites().Add(new Site(name));
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
        }

        private void NewFacilityButton_Click(object sender, EventArgs e)
        {
            // Get parent site
            Site site;
            int index = -1;
            TreeNode node = SitesTreeView.SelectedNode;
            if (node.Tag is Site)
            {
                site = (Site)node.Tag;
            }
            else if (node.Tag is Facility)
            {
                Facility fac = (Facility)node.Tag;
                site = (Site)node.Parent.Tag;
                index = site.GetFacilities().IndexOf(fac) + 1;
            }
            else if (node.Tag is DetectionSystem)
            {
                Facility fac = (Facility)node.Parent.Tag;
                site = (Site)node.Parent.Parent.Tag;
                index = site.GetFacilities().IndexOf(fac) + 1;
            }
            else
            {
                Facility fac = (Facility)node.Parent.Parent.Tag;
                site = (Site)node.Parent.Parent.Parent.Tag;
                index = site.GetFacilities().IndexOf(fac) + 1;
            }

            if (index < 0)
                index = site.GetFacilities().Count;

            bool uniqueName = false;
            int iteration = 0;

            string name = "";
            while (!uniqueName)
            {
                iteration++;
                name = "New-Facility-" + iteration.ToString();
                uniqueName = true;
                foreach (Facility fac in site.GetFacilities())
                {
                    if (fac.GetName() == name) uniqueName = false;
                }
            }
            site.GetFacilities().Insert(index, new Facility(name));
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
        }

        private void NewSystemButton_Click(object sender, EventArgs e)
        {
            // Get parent facility
            int index = -1;
            TreeNode node = SitesTreeView.SelectedNode;
            Facility fac;
            if (node.Tag is Facility)
            {
                fac = (Facility)node.Tag;
            }
            else if (node.Tag is DetectionSystem)
            {
                fac = (Facility)node.Parent.Tag;
                DetectionSystem sys = (DetectionSystem)node.Tag;
                index = fac.GetSystems().IndexOf(sys) + 1;
            }
            else
            {
                fac = (Facility)node.Parent.Parent.Tag;
                DetectionSystem sys = (DetectionSystem)node.Parent.Tag;
                index = fac.GetSystems().IndexOf(sys) + 1;
            }

            if (index < 0)
                index = fac.GetSystems().Count;

            bool uniqueName = false;
            int iteration = 0;

            string name = "";
            while (!uniqueName)
            {
                iteration++;
                name = "New-System-" + iteration.ToString();
                uniqueName = true;
                foreach (DetectionSystem sys in fac.GetSystems())
                {
                    if (sys.GetName() == name) uniqueName = false;
                }
            }
            fac.GetSystems().Insert(index, new DetectionSystem(name));
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
        }

        private void NewInstrumentButton_Click(object sender, EventArgs e)
        {
            // Choose instrument type
            InstrumentTypeDialog dialog = new InstrumentTypeDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.Cancel) return;

            // Get parent system
            int index = -1;
            TreeNode node = SitesTreeView.SelectedNode;
            DetectionSystem sys;
            if (node.Tag is DetectionSystem)
            {
                sys = (DetectionSystem)node.Tag;
            }
            else
            {
                sys = (DetectionSystem)node.Parent.Tag;
                Instrument inst = (Instrument)node.Tag;
                index = sys.GetInstruments().IndexOf(inst) + 1;
            }

            if (index < 0)
                index = sys.GetInstruments().Count;

            bool uniqueName = false;
            int iteration = 0;

            string name = "";

            Instrument newInstrument;

            switch (dialog.InstrumentType)
            {
                case "GRAND":
                    while (!uniqueName)
                    {
                        iteration++;
                        name = "New-GRAND-" + iteration.ToString();
                        uniqueName = true;
                        foreach (Instrument inst in sys.GetInstruments())
                        {
                            if (sys.GetName() == name) uniqueName = false;
                        }
                    }
                    newInstrument = new GRANDInstrument(name);
                    break;
                case "ISR":
                    while (!uniqueName)
                    {
                        iteration++;
                        name = "New-ISR-" + iteration.ToString();
                        uniqueName = true;
                        foreach (Instrument inst in sys.GetInstruments())
                        {
                            if (sys.GetName() == name) uniqueName = false;
                        }
                    }
                    newInstrument = new ISRInstrument(name);
                    break;
                default:                // MCA
                    while (!uniqueName)
                    {
                        iteration++;
                        name = "New-MCA-" + iteration.ToString();
                        uniqueName = true;
                        foreach (Instrument inst in sys.GetInstruments())
                        {
                            if (sys.GetName() == name) uniqueName = false;
                        }
                    }
                    newInstrument = new MCAInstrument(name);
                    break;
            }

            sys.GetInstruments().Insert(index, newInstrument);
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
        }
    }
}
