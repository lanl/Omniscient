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
using Omniscient.Events;

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

            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
        }

        private void SiteManagerForm_Load(object sender, EventArgs e)
        {
            SitesTreeView.ImageList = main.TreeImageList;
            UpButton.Image = main.ButtonImageList.Images[0];
            DownButton.Image = main.ButtonImageList.Images[1];           
            RemoveButton.Image = main.ButtonImageList.Images[3];
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
                siteNode.Name = site.GetName();
                siteNode.Tag = site;
                siteNode.ImageIndex = 0;
                siteNode.SelectedImageIndex = 0;
                siteNode.ToolTipText = siteNode.Text;
                foreach (Facility fac in site.GetFacilities())
                {
                    TreeNode facNode = new TreeNode(fac.GetName());
                    facNode.Name = fac.GetName();
                    facNode.Tag = fac;
                    facNode.ImageIndex = 1;
                    facNode.SelectedImageIndex = 1;
                    facNode.ToolTipText = facNode.Text;
                    foreach (DetectionSystem sys in fac.GetSystems())
                    {
                        TreeNode sysNode = new TreeNode(sys.GetName());
                        sysNode.Name = sys.GetName();
                        sysNode.Tag = sys;
                        sysNode.ImageIndex = 2;
                        sysNode.SelectedImageIndex = 2;
                        sysNode.ToolTipText = sysNode.Text;
                        foreach (Instrument inst in sys.GetInstruments())
                        {
                            TreeNode instNode = new TreeNode(inst.GetName());
                            instNode.Name = inst.GetName();
                            instNode.Tag = inst;
                            instNode.ImageIndex = 3;
                            instNode.SelectedImageIndex = 3;
                            instNode.ToolTipText = instNode.Text;
                            sysNode.Nodes.Add(instNode);
                        }
                        foreach (EventGenerator eg in sys.GetEventGenerators())
                        {
                            TreeNode egNode = new TreeNode(eg.GetName());
                            egNode.Name = eg.GetName();
                            egNode.ForeColor = System.Drawing.SystemColors.GrayText;
                            egNode.Tag = eg;
                            egNode.ImageIndex = 4;
                            egNode.SelectedImageIndex = 4;
                            egNode.ToolTipText = egNode.Text;
                            sysNode.Nodes.Add(egNode);
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

        private void SetupVirtualChannelGroupBox()
        {
            Instrument inst = (Instrument)SitesTreeView.SelectedNode.Tag;
            VirtualChannel chan = null;
            foreach (VirtualChannel otherChan in inst.GetVirtualChannels())
            {
                if (otherChan.GetName() == VirtualChannelsComboBox.Text)
                    chan = otherChan;
            }
            Channel[] instChannels = inst.GetChannels();
            int chanIndex = -1;
            for (int i=0; i< instChannels.Length;i++)
            {
                if (instChannels[i] == chan)
                {
                    chanIndex = i;
                    break;
                }
            }
            VirtualChannelNameTextBox.Text = chan.GetName();
            switch(chan.GetVirtualChannelType())
            {
                case VirtualChannel.VirtualChannelType.RATIO:
                    VirtualChannelTypeComboBox.Text = "Ratio";
                    VCTwoChannelPanel.Visible = true;
                    VCChannelConstPanel.Visible = false;

                    break;
                case VirtualChannel.VirtualChannelType.SUM:
                    VirtualChannelTypeComboBox.Text = "Sum";
                    break;
                case VirtualChannel.VirtualChannelType.DIFFERENCE:
                    VirtualChannelTypeComboBox.Text = "Difference";
                    break;
                case VirtualChannel.VirtualChannelType.ADD_CONST:
                    VirtualChannelTypeComboBox.Text = "Add Constant";
                    break;
                case VirtualChannel.VirtualChannelType.SCALE:
                    VirtualChannelTypeComboBox.Text = "Scale";
                    break;
                case VirtualChannel.VirtualChannelType.DELAY:
                    VirtualChannelTypeComboBox.Text = "Delay";
                    break;
            }
            switch (chan.GetVirtualChannelType())
            {
                case VirtualChannel.VirtualChannelType.RATIO:
                case VirtualChannel.VirtualChannelType.SUM:
                case VirtualChannel.VirtualChannelType.DIFFERENCE:
                    PopulateVCTwoChannelPanel(chan, inst);
                    VirtualChannelChanAComboBox.Text = chan.GetChannelA().GetName();
                    VirtualChannelChanBComboBox.Text = chan.GetChannelB().GetName();
                    VCTwoChannelPanel.Visible = true;
                    VCChannelConstPanel.Visible = false;
                    VCDelayPanel.Visible = false;
                    break;
                case VirtualChannel.VirtualChannelType.ADD_CONST:
                case VirtualChannel.VirtualChannelType.SCALE:
                    PopulateVCChannelConstPanel(chan, inst);
                    VirtualChannelChannelComboBox.Text = chan.GetChannelA().GetName();
                    VirtualChannelConstantTextBox.Text = chan.GetConstant().ToString();
                    VCTwoChannelPanel.Visible = false;
                    VCChannelConstPanel.Visible = true;
                    VCDelayPanel.Visible = false;
                    break;
                case VirtualChannel.VirtualChannelType.DELAY:
                    PopulateDelayPanel(chan, inst);
                    DelayChannelComboBox.Text = chan.GetChannelA().GetName();
                    TimeSpan delay = chan.GetDelay();
                    if (delay.TotalSeconds == 0)
                    {
                        DelayTextBox.Text = "0";
                        DelayComboBox.Text = "Seconds";
                    }
                    else if (Math.Abs(delay.TotalDays % 1) <= (Double.Epsilon * 100))
                    {
                        DelayTextBox.Text = delay.TotalDays.ToString();
                        DelayComboBox.Text = "Days";
                    }
                    else if (Math.Abs(delay.TotalHours % 1) <= (Double.Epsilon * 100))
                    {
                        DelayTextBox.Text = delay.TotalHours.ToString();
                        DelayComboBox.Text = "Hours";
                    }
                    else if (Math.Abs(delay.TotalMinutes % 1) <= (Double.Epsilon * 100))
                    {
                        DelayTextBox.Text = delay.TotalMinutes.ToString();
                        DelayComboBox.Text = "Minutes";
                    }
                    else if (Math.Abs(delay.TotalSeconds % 1) <= (Double.Epsilon * 100))
                    {
                        DelayTextBox.Text = delay.TotalSeconds.ToString();
                        DelayComboBox.Text = "Seconds";
                    }
                    break;
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

                InstrumentPanel.Visible = false;
                NewInstrumentButton.Enabled = false;
                NewSystemButton.Enabled = false;
            }
            else if (node.Tag is Facility)
            {
                Facility fac = (Facility)node.Tag;
                TypeLabel.Text = "Facility";

                InstrumentPanel.Visible = false;
                NewInstrumentButton.Enabled = false;
                NewSystemButton.Enabled = true;
            }
            else if (node.Tag is DetectionSystem)
            {
                DetectionSystem sys = (DetectionSystem)node.Tag;
                TypeLabel.Text = "System";

                InstrumentPanel.Visible = false;
                NewInstrumentButton.Enabled = true;
                NewSystemButton.Enabled = true;
            }
            else if (node.Tag is Instrument)
            {
                Instrument inst = (Instrument)node.Tag;
                TypeLabel.Text = "Instrument";
                if (inst is MCAInstrument)
                    InstTypeComboBox.Text = "MCA";
                else if (inst is ISRInstrument)
                    InstTypeComboBox.Text = "ISR";
                else if (inst is GRANDInstrument)
                    InstTypeComboBox.Text = "GRAND";
                PrefixTextBox.Text = inst.GetFilePrefix();
                DirectoryTextBox.Text = inst.GetDataFolder();

                VirtualChannelsComboBox.Items.Clear();
                if (inst.GetVirtualChannels().Count > 0)
                {
                    foreach (VirtualChannel vc in inst.GetVirtualChannels())
                        VirtualChannelsComboBox.Items.Add(vc.GetName());
                    VirtualChannelsComboBox.Text = inst.GetVirtualChannels()[0].GetName();
                    VirtualChannelGroupBox.Visible = true;
                    SetupVirtualChannelGroupBox();
                }
                else
                    VirtualChannelGroupBox.Visible = false;


                InstrumentPanel.Visible = true;
                NewInstrumentButton.Enabled = true;
                NewSystemButton.Enabled = true;
            }
            else if (node.Tag is EventGenerator)
            {
                EventGenerator eg = (EventGenerator)node.Tag;
                TypeLabel.Text = "Event Generator";

                InstrumentPanel.Visible = false;
                NewInstrumentButton.Enabled = false;
                NewSystemButton.Enabled = false;
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

        private void SaveVirtualChannel(Instrument inst, VirtualChannel chan)
        {
            chan.SetName(VirtualChannelNameTextBox.Text);
            switch (VirtualChannelTypeComboBox.Text)
            {
                case "Ratio":
                    chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.RATIO);
                    break;
                case "Sum":
                    chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.SUM);
                    break;
                case "Difference":
                    chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.DIFFERENCE);
                    break;
                case "Add Constant":
                    chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.ADD_CONST);
                    break;
                case "Scale":
                    chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.SCALE);
                    break;
                case "Delay":
                    chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.DELAY);
                    break;
                default:
                    MessageBox.Show("Invalid virtual channel type!");
                    return;
            }
            switch (VirtualChannelTypeComboBox.Text)
            {
                case "Ratio":
                case "Sum":
                case "Difference":
                    foreach(Channel otherChan in inst.GetChannels())
                    {
                        if (otherChan.GetName() == VirtualChannelChanAComboBox.Text)
                            chan.SetChannelA(otherChan);
                        if (otherChan.GetName() == VirtualChannelChanBComboBox.Text)
                            chan.SetChannelB(otherChan);
                    }
                    break;
                case "Add Constant":
                case "Scale":
                    foreach (Channel otherChan in inst.GetChannels())
                    {
                        if (otherChan.GetName() == VirtualChannelChannelComboBox.Text)
                            chan.SetChannelA(otherChan);
                    }
                    try
                    {
                        chan.SetConstant(double.Parse(VirtualChannelConstantTextBox.Text));
                    }
                    catch { } //One day, this will be done correctly...
                    break;
                case "Delay":
                    foreach (Channel otherChan in inst.GetChannels())
                    {
                        if (otherChan.GetName() == DelayChannelComboBox.Text)
                            chan.SetChannelA(otherChan);
                    }
                    switch(DelayComboBox.Text)
                    {
                        case "Days":
                            chan.SetDelay(TimeSpan.FromDays(double.Parse(DelayTextBox.Text)));
                            break;
                        case "Hours":
                            chan.SetDelay(TimeSpan.FromHours(double.Parse(DelayTextBox.Text)));
                            break;
                        case "Minutes":
                            chan.SetDelay(TimeSpan.FromMinutes(double.Parse(DelayTextBox.Text)));
                            break;
                        case "Seconds":
                            chan.SetDelay(TimeSpan.FromSeconds(double.Parse(DelayTextBox.Text)));
                            break;
                        default:
                            MessageBox.Show("Invalid delay time unit!");
                            return;
                    }
                    break;
                default:
                    MessageBox.Show("Invalid virtual channel type!");
                    return;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            TreeNode node = SitesTreeView.SelectedNode;
            string nodeName = node.Name;

            if (node.Tag is Site)
            {
                Site site = (Site)node.Tag;
                site.SetName(NameTextBox.Text);
                nodeName = site.GetName();
            }
            else if (node.Tag is Facility)
            {
                Facility fac = (Facility)node.Tag;
                fac.SetName(NameTextBox.Text);
                nodeName = fac.GetName();
            }
            else if (node.Tag is DetectionSystem)
            {
                DetectionSystem sys = (DetectionSystem)node.Tag;
                sys.SetName(NameTextBox.Text);
                nodeName = sys.GetName();
            }
            else if (node.Tag is Instrument)
            {
                Instrument inst = (Instrument)node.Tag;

                inst.SetName(NameTextBox.Text);
                inst.SetFilePrefix(PrefixTextBox.Text);
                inst.SetDataFolder(DirectoryTextBox.Text);
                foreach(VirtualChannel chan in inst.GetVirtualChannels())
                {
                    if (chan.GetName() == VirtualChannelsComboBox.Text)
                    {
                        SaveVirtualChannel(inst, chan);
                        break;
                    }
                }
                nodeName = inst.GetName();
            }
            else if (node.Tag is EventGenerator)
            {
                MessageBox.Show("Use the Event Manager to edit events.");
                return;
            }
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(nodeName, true)[0];
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
            else if (node.Tag is EventGenerator)
            {
                EventGenerator eg = (EventGenerator)node.Tag;
                DetectionSystem sys = (DetectionSystem)node.Parent.Tag;
                sys.GetEventGenerators().Remove(eg);
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

        private void AddVirtualChannelButton_Click(object sender, EventArgs e)
        {
            Instrument inst = (Instrument)SitesTreeView.SelectedNode.Tag;
            if (inst.GetChannels().Length == 0)
            {
                MessageBox.Show("The instrument has no channels to make a virtual instrument from!");
            }
            VirtualChannel virtualChannel = new VirtualChannel("New-VC", inst, inst.GetChannels()[0].GetChannelType());
            virtualChannel.SetChannelA(inst.GetChannels()[0]);
            virtualChannel.SetVirtualChannelType(VirtualChannel.VirtualChannelType.ADD_CONST);
            virtualChannel.SetConstant(0);
            inst.GetVirtualChannels().Add(virtualChannel);
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(inst.GetName(), true)[0];
        }

        private void PopulateVCTwoChannelPanel(VirtualChannel chan, Instrument inst)
        {
            VirtualChannelChanAComboBox.Items.Clear();
            VirtualChannelChanBComboBox.Items.Clear();
            Channel[] instChannels = inst.GetChannels();
            int chanIndex = -1;
            for (int i = 0; i < instChannels.Length; i++)
            {
                if (instChannels[i] == chan)
                {
                    chanIndex = i;
                    break;
                }
            }
            for (int i = 0; i < chanIndex; i++)
            {
                VirtualChannelChanAComboBox.Items.Add(instChannels[i].GetName());
                VirtualChannelChanBComboBox.Items.Add(instChannels[i].GetName());
            }
            VirtualChannelChanAComboBox.Text = chan.GetChannelA().GetName();
            if (chan.GetChannelB() != null)
                VirtualChannelChanBComboBox.Text = chan.GetChannelB().GetName();
            else
                VirtualChannelChanBComboBox.Text = VirtualChannelChanBComboBox.Items[0].ToString();
        }


        private void PopulateVCChannelConstPanel(VirtualChannel chan, Instrument inst)
        {
            VirtualChannelChanAComboBox.Items.Clear();
            Channel[] instChannels = inst.GetChannels();
            int chanIndex = -1;
            for (int i = 0; i < instChannels.Length; i++)
            {
                if (instChannels[i] == chan)
                {
                    chanIndex = i;
                    break;
                }
            }
            VirtualChannelChannelComboBox.Items.Clear();
            for (int i = 0; i < chanIndex; i++)
            {
                VirtualChannelChannelComboBox.Items.Add(instChannels[i].GetName());
            }
            VirtualChannelChannelComboBox.Text = chan.GetChannelA().GetName();
            VirtualChannelConstantTextBox.Text = chan.GetConstant().ToString();
        }

        private void PopulateDelayPanel(VirtualChannel chan, Instrument inst)
        {
            DelayChannelComboBox.Items.Clear();
            Channel[] instChannels = inst.GetChannels();
            int chanIndex = -1;
            for (int i = 0; i < instChannels.Length; i++)
            {
                if (instChannels[i] == chan)
                {
                    chanIndex = i;
                    break;
                }
            }
            DelayChannelComboBox.Items.Clear();
            for (int i = 0; i < chanIndex; i++)
            {
                DelayChannelComboBox.Items.Add(instChannels[i].GetName());
            }
            DelayChannelComboBox.Text = chan.GetChannelA().GetName();
            TimeSpan delay = chan.GetDelay();
            if (delay.TotalSeconds == 0)
            {
                DelayTextBox.Text = "0";
                DelayComboBox.Text = "Seconds";
            }
            else if (Math.Abs(delay.TotalDays % 1) <= (Double.Epsilon * 100))
            {
                DelayTextBox.Text = delay.TotalDays.ToString();
                DelayComboBox.Text = "Days";
            }
            else if (Math.Abs(delay.TotalHours % 1) <= (Double.Epsilon * 100))
            {
                DelayTextBox.Text = delay.TotalHours.ToString();
                DelayComboBox.Text = "Hours";
            }
            else if (Math.Abs(delay.TotalMinutes % 1) <= (Double.Epsilon * 100))
            {
                DelayTextBox.Text = delay.TotalMinutes.ToString();
                DelayComboBox.Text = "Minutes";
            }
            else if (Math.Abs(delay.TotalSeconds % 1) <= (Double.Epsilon * 100))
            {
                DelayTextBox.Text = delay.TotalSeconds.ToString();
                DelayComboBox.Text = "Seconds";
            }

        }

        private void VirtualChannelTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Instrument inst = (Instrument)SitesTreeView.SelectedNode.Tag;
            VirtualChannel chan = null;
            foreach (VirtualChannel otherChan in inst.GetVirtualChannels())
            {
                if (otherChan.GetName() == VirtualChannelsComboBox.Text)
                    chan = otherChan;
            }
            switch (VirtualChannelTypeComboBox.Text)
            {
                case "Ratio":
                case "Sum":
                case "Difference":
                    PopulateVCTwoChannelPanel(chan, inst);
                    VCTwoChannelPanel.Visible = true;
                    VCChannelConstPanel.Visible = false;
                    VCDelayPanel.Visible = false;
                    break;
                case "Add Constant":
                case "Scale":
                    PopulateVCChannelConstPanel(chan, inst);
                    VCTwoChannelPanel.Visible = false;
                    VCChannelConstPanel.Visible = true;
                    VCDelayPanel.Visible = false;
                    break;
                case "Delay":
                    PopulateDelayPanel(chan, inst);
                    VCTwoChannelPanel.Visible = false;
                    VCChannelConstPanel.Visible = false;
                    VCDelayPanel.Visible = true;
                    break;
                default:
                    MessageBox.Show("Invalid virtual channel type!");
                    return;
            }
        }

        private void VirtualChannelsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupVirtualChannelGroupBox();
        }

        private void RemoveVirtualChannelButton_Click(object sender, EventArgs e)
        {
            Instrument inst = (Instrument)SitesTreeView.SelectedNode.Tag;
            VirtualChannel chan = null;
            foreach (VirtualChannel otherChan in inst.GetVirtualChannels())
            {
                if (otherChan.GetName() == VirtualChannelsComboBox.Text)
                    chan = otherChan;
            }
            inst.GetVirtualChannels().Remove(chan);
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(inst.GetName(), true)[0];
            ResetFields();
        }
    }
}
