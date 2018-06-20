// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2018, Los Alamos National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Los Alamos National Security, LLC. This software was produced under U.S. Government contract DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Los Alamos National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY LOS ALAMOS NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL LOS ALAMOS NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
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
    public partial class SiteManagerForm : Form
    {
        string[] DEFAULT_VIRTUAL_CHANNEL_TYPES = {"Ratio", "Sum", "Difference", "Add Constant", "Scale", "Delay", "Convolve"};

        // SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
        // State
        DetectionSystem selectedSystem;

        // SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS

        MainForm main;
        SiteManager siteMan;

        bool siteManChanged = false;

        public SiteManagerForm(MainForm master, SiteManager newSiteMan)
        {
            main = master;
            siteMan = newSiteMan;

            selectedSystem = null;

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

            // G - Disable buttons lower on the tree until a site exists
            NewFacilityButton.Enabled = false;
            NewSystemButton.Enabled = false;
            NewInstrumentButton.Enabled = false;
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
                            if (!(inst is DeclarationInstrument))
                            {
                                TreeNode instNode = new TreeNode(inst.GetName());
                                instNode.Name = inst.GetName();
                                instNode.Tag = inst;
                                instNode.ImageIndex = 3;
                                instNode.SelectedImageIndex = 3;
                                instNode.ToolTipText = instNode.Text;
                                sysNode.Nodes.Add(instNode);
                            }
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

        public string VCTypeToString(VirtualChannel.VirtualChannelType vcType)
        {
            switch (vcType)
            {
                case VirtualChannel.VirtualChannelType.RATIO:
                    return "Ratio";
                case VirtualChannel.VirtualChannelType.SUM:
                    return "Sum";
                case VirtualChannel.VirtualChannelType.DIFFERENCE:
                    return "Difference";
                case VirtualChannel.VirtualChannelType.ADD_CONST:
                    return "Add Constant";
                case VirtualChannel.VirtualChannelType.SCALE:
                    return "Scale";
                case VirtualChannel.VirtualChannelType.DELAY:
                    return "Delay";
                case VirtualChannel.VirtualChannelType.CONVOLVE:
                    return "Convolve";
                case VirtualChannel.VirtualChannelType.ROI:
                    return "ROI";
            }
            return "Weird Things Are Happening";
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
            VirtualChannelTypeComboBox.Items.Clear();
            VirtualChannelTypeComboBox.Items.AddRange(DEFAULT_VIRTUAL_CHANNEL_TYPES);
            if (inst is MCAInstrument)
                VirtualChannelTypeComboBox.Items.Add("ROI");
            VirtualChannelTypeComboBox.Text = VCTypeToString(chan.GetVirtualChannelType());
            if (chan.GetVirtualChannelType() == VirtualChannel.VirtualChannelType.RATIO)
            {
                VCTwoChannelPanel.Visible = true;
                VCChannelConstPanel.Visible = false;
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
                    ROIVCPanel.Visible = false;
                    break;
                case VirtualChannel.VirtualChannelType.ADD_CONST:
                case VirtualChannel.VirtualChannelType.SCALE:
                    PopulateVCChannelConstPanel(chan, inst);
                    VirtualChannelChannelComboBox.Text = chan.GetChannelA().GetName();
                    VirtualChannelConstantTextBox.Text = chan.GetConstant().ToString();
                    VCTwoChannelPanel.Visible = false;
                    VCChannelConstPanel.Visible = true;
                    VCDelayPanel.Visible = false;
                    ROIVCPanel.Visible = false;
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
                case VirtualChannel.VirtualChannelType.ROI:
                    PopulateROIVCPanel(chan, inst);
                    VCTwoChannelPanel.Visible = false;
                    VCChannelConstPanel.Visible = false;
                    VCDelayPanel.Visible = false;
                    ROIVCPanel.Visible = true;
                    break;
            }
        }

        private void SetupSystemPanel()
        {
            if (selectedSystem.HasDeclarationInstrument())
            {
                DeclarationCheckBox.Checked = true;
                DeclarationPrefixTextBox.Enabled = true;
                DeclarationPrefixTextBox.Text = selectedSystem.GetDeclarationInstrument().GetFilePrefix();
                DeclarationDirectoryTextBox.Enabled = true;
                DeclarationDirectoryTextBox.Text = selectedSystem.GetDeclarationInstrument().GetDataFolder();
            }
            else
            {
                DeclarationCheckBox.Checked = false;
                DeclarationPrefixTextBox.Enabled = false;
                DeclarationPrefixTextBox.Text = "";
                DeclarationDirectoryTextBox.Enabled = false;
                DeclarationDirectoryTextBox.Text = "";
            }
        }

        private void ResetFields()
        {
            selectedSystem = null;
            TreeNode node = SitesTreeView.SelectedNode;
            NameTextBox.Text = node.Text;

            if (node.Tag is Site)
            {
                Site site = (Site)node.Tag;
                TypeLabel.Text = "Site";

                SystemPanel.Visible = false;
                InstrumentPanel.Visible = false;
                NewInstrumentButton.Enabled = false;
                NewSystemButton.Enabled = false;
            }
            else if (node.Tag is Facility)
            {
                Facility fac = (Facility)node.Tag;
                TypeLabel.Text = "Facility";

                SystemPanel.Visible = false;
                InstrumentPanel.Visible = false;
                NewInstrumentButton.Enabled = false;
                NewSystemButton.Enabled = true;
            }
            else if (node.Tag is DetectionSystem)
            {
                DetectionSystem sys = (DetectionSystem)node.Tag;
                selectedSystem = sys;
                TypeLabel.Text = "System";

                SetupSystemPanel();

                SystemPanel.Visible = true;
                InstrumentPanel.Visible = false;
                NewInstrumentButton.Enabled = true;
                NewSystemButton.Enabled = true;
            }
            else if (node.Tag is Instrument)
            {
                Instrument inst = (Instrument)node.Tag;
                TypeLabel.Text = "Instrument";
                if (inst is MCAInstrument)
                {
                    InstTypeComboBox.Text = "MCA";
                    FileExtensionComboBox.Items.Clear();
                    FileExtensionComboBox.Items.Add("chn");
                    FileExtensionComboBox.Items.Add("spe");
                }
                else if (inst is ISRInstrument)
                { 
                    InstTypeComboBox.Text = "ISR";
                    FileExtensionComboBox.Items.Clear();
                    FileExtensionComboBox.Items.Add("isr");
                    FileExtensionComboBox.Items.Add("jsr");
                    FileExtensionComboBox.Items.Add("hmr");
                }
                else if (inst is GRANDInstrument)
                { 
                    InstTypeComboBox.Text = "GRAND";
                    FileExtensionComboBox.Items.Clear();
                    FileExtensionComboBox.Items.Add("bid");
                }
                else if (inst is NGAMInstrument)
                {
                    InstTypeComboBox.Text = "NGAM";
                    FileExtensionComboBox.Items.Clear();
                    FileExtensionComboBox.Items.Add("vbf");
                }
                FileExtensionComboBox.Text = inst.FileExtension;
                PrefixTextBox.Text = inst.GetFilePrefix();
                DirectoryTextBox.Text = inst.GetDataFolder();

                VirtualChannelsComboBox.Items.Clear();
                VirtualChannelsComboBox.Text = "";
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

                SystemPanel.Visible = false;
                InstrumentPanel.Visible = true;
                NewInstrumentButton.Enabled = true;
                NewSystemButton.Enabled = true;
            }
            else if (node.Tag is EventGenerator)
            {
                EventGenerator eg = (EventGenerator)node.Tag;
                TypeLabel.Text = "Event Generator";

                SystemPanel.Visible = false;
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
                if (main.siteMan.Reload() != ReturnCode.SUCCESS) MessageBox.Show("Warning: Bad trouble loading the site manager!");
                if (main.presetMan.Reload() != ReturnCode.SUCCESS) MessageBox.Show("Warning: Bad trouble loading the preset manager!");
                main.UpdateSitesTree();
            }
            Close();
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Check if declaration exists
            if (DeclarationCheckBox.Checked)
            {
                try
                {
                    selectedSystem.GetDeclarationInstrument().SetFilePrefix(DeclarationPrefixTextBox.Text);
                    selectedSystem.GetDeclarationInstrument().SetDataFolder(DeclarationDirectoryTextBox.Text);
                }

                catch
                {
                    MessageBox.Show("Enter a declaration or uncheck the declaration box.");
                    return;
                }

            }
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

            // G - Enable buttons which create objects lower on the tree
            NewFacilityButton.Enabled = true;
            NewSystemButton.Enabled = true;
            NewInstrumentButton.Enabled = true;

            // G - Select first node
            SitesTreeView.SelectedNode = SitesTreeView.TopNode;

        }

        private void DiscardButton_Click(object sender, EventArgs e)
        {
            ResetFields();
        }

        private void SaveVirtualChannel(Instrument inst, VirtualChannel chan)
        {
            if (chan.GetName() != VirtualChannelNameTextBox.Text && siteMan.ContainsName(VirtualChannelNameTextBox.Text))
            {
                MessageBox.Show("All items in the Site Manager require a unique name!");
                return;
            }

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
                case "Convolve":
                    chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.CONVOLVE);
                    break;
                case "ROI":
                    chan.SetVirtualChannelType(VirtualChannel.VirtualChannelType.ROI);
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
                case "Convolve":
                    foreach (Channel otherChan in inst.GetChannels())
                    {
                        if (otherChan.GetName() == ConvolveChannelComboBox.Text)
                            chan.SetChannelA(otherChan);
                    }
                    if (FilterFileTextBox.Text == "")
                    {
                        MessageBox.Show("Don't forget your filter file!");
                        return;
                    }
                    chan.SetDataFileName(FilterFileTextBox.Text);
                    break;
                case "ROI":
                    ROIChannel roiChan = (ROIChannel)chan;
                    ROI roi = roiChan.GetROI();
                    try
                    {
                        roi.SetROIStart(double.Parse(ROIStartTextBox.Text));
                        roi.SetROIEnd(double.Parse(ROIEndTextBox.Text));
                        roi.SetBG1Start(double.Parse(BG1StartTextBox.Text));
                        roi.SetBG1End(double.Parse(BG1EndTextBox.Text));
                        roi.SetBG2Start(double.Parse(BG2StartTextBox.Text));
                        roi.SetBG2End(double.Parse(BG2EndTextBox.Text));
                    }
                    catch
                    {
                        MessageBox.Show("Invalid ROI or BG bounds!");
                        return;
                    }
                    switch(ROIBackgroundComboBox.Text)
                    {
                        case "None":
                            roi.SetBGType(ROI.BG_Type.NONE);
                            break;
                        case "Flat":
                            roi.SetBGType(ROI.BG_Type.FLAT);
                            break;
                        case "Linear":
                            roi.SetBGType(ROI.BG_Type.LINEAR);
                            break;
                        default:
                            MessageBox.Show("Invalid background type!");
                            return;
                    }
                    roiChan.SetROI(roi);
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

            VirtualChannel virtualChan = null;

            if (node.Tag is Site)
            {
                Site site = (Site)node.Tag;
                if(site.GetName() != NameTextBox.Text && siteMan.ContainsName(NameTextBox.Text))
                {
                    MessageBox.Show("All items in the Site Manager require a unique name!");
                    return;
                }
                site.SetName(NameTextBox.Text);
                nodeName = site.GetName();
            }
            else if (node.Tag is Facility)
            {
                Facility fac = (Facility)node.Tag;
                if (fac.GetName() != NameTextBox.Text && siteMan.ContainsName(NameTextBox.Text))
                {
                    MessageBox.Show("All items in the Site Manager require a unique name!");
                    return;
                }
                fac.SetName(NameTextBox.Text);
                nodeName = fac.GetName();
            }
            else if (node.Tag is DetectionSystem)
            {
                DetectionSystem sys = (DetectionSystem)node.Tag;
                if (sys.GetName() != NameTextBox.Text && siteMan.ContainsName(NameTextBox.Text))
                {
                    MessageBox.Show("All items in the Site Manager require a unique name!");
                    return;
                }
                sys.SetName(NameTextBox.Text);
                nodeName = sys.GetName();
                if(DeclarationCheckBox.Checked)
                {
                    try
                    {
                        selectedSystem.GetDeclarationInstrument().SetFilePrefix(DeclarationPrefixTextBox.Text);
                        selectedSystem.GetDeclarationInstrument().SetDataFolder(DeclarationDirectoryTextBox.Text);
                    }

                    catch
                    {
                        MessageBox.Show("Enter a declaration or uncheck the declaration box.");
                    }


                }
            }
            else if (node.Tag is Instrument)
            {
                Instrument inst = (Instrument)node.Tag;
                if (inst.GetName() != NameTextBox.Text && siteMan.ContainsName(NameTextBox.Text))
                {
                    MessageBox.Show("All items in the Site Manager require a unique name!");
                    return;
                }
                inst.SetName(NameTextBox.Text);
                try
                {
                    inst.FileExtension = FileExtensionComboBox.Text;
                }
                catch
                {
                    MessageBox.Show("Invalid file extension!");
                    return;
                }
                inst.SetFilePrefix(PrefixTextBox.Text);
                inst.SetDataFolder(DirectoryTextBox.Text);
                foreach(VirtualChannel chan in inst.GetVirtualChannels())
                {
                    if (chan.GetName() == VirtualChannelsComboBox.Text)
                    {
                        SaveVirtualChannel(inst, chan);
                        virtualChan = chan;
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
            if (siteMan.Save() != ReturnCode.SUCCESS) MessageBox.Show("Bad trouble saving the site manager!");
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(nodeName, true)[0];
            if(virtualChan!=null)
            {
                VirtualChannelsComboBox.Text = virtualChan.GetName();
            }
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

            // G - Enable buttons which create objects lower on the tree
            NewFacilityButton.Enabled = true;
            NewSystemButton.Enabled = true;
            NewInstrumentButton.Enabled = true;

            string name = "";
            while (!uniqueName)
            {
                iteration++;
                name = "New-Site-" + iteration.ToString();
                uniqueName = !siteMan.ContainsName(name);
            }
            siteMan.GetSites().Add(new Site(name));
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(name, true)[0];
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
                uniqueName = !siteMan.ContainsName(name);
            }
            site.GetFacilities().Insert(index, new Facility(name));
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(name, true)[0];
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
                uniqueName = !siteMan.ContainsName(name);
            }
            fac.GetSystems().Insert(index, new DetectionSystem(name));
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(name, true)[0];
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
                        uniqueName = !siteMan.ContainsName(name);
                    }
                    newInstrument = new GRANDInstrument(name);
                    break;
                case "NGAM":
                    while (!uniqueName)
                    {
                        iteration++;
                        name = "New-NGAM-" + iteration.ToString();
                        uniqueName = !siteMan.ContainsName(name);
                    }
                    newInstrument = new NGAMInstrument(name);
                    break;
                case "ISR":
                    while (!uniqueName)
                    {
                        iteration++;
                        name = "New-ISR-" + iteration.ToString();
                        uniqueName = !siteMan.ContainsName(name);
                    }
                    newInstrument = new ISRInstrument(name);
                    break;
                default:                // MCA
                    while (!uniqueName)
                    {
                        iteration++;
                        name = "New-MCA-" + iteration.ToString();
                        uniqueName = !siteMan.ContainsName(name);
                    }
                    newInstrument = new MCAInstrument(name);
                    break;
            }

            sys.GetInstruments().Insert(index, newInstrument);
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(name, true)[0];
        }

        private void AddVirtualChannelButton_Click(object sender, EventArgs e)
        {
            Instrument inst = (Instrument)SitesTreeView.SelectedNode.Tag;
            if (inst.GetChannels().Length == 0)
            {
                MessageBox.Show("The instrument has no channels to make a virtual instrument from!");
            }
            int iteration = 0;
            bool uniqueName = false;
            string name = "";
            while (!uniqueName)
            {
                iteration++;
                name = "New-VC-" + iteration.ToString();
                uniqueName = !siteMan.ContainsName(name);
            }
            VirtualChannel virtualChannel = new VirtualChannel(name, inst, inst.GetChannels()[0].GetChannelType());
            virtualChannel.SetChannelA(inst.GetChannels()[0]);
            virtualChannel.SetVirtualChannelType(VirtualChannel.VirtualChannelType.ADD_CONST);
            virtualChannel.SetConstant(0);
            inst.GetVirtualChannels().Add(virtualChannel);
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(inst.GetName(), true)[0];
            VirtualChannelsComboBox.Text = name;
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

        private void PopulateROIVCPanel(VirtualChannel chan, Instrument inst)
        {
            ROIChannel roiChan = (ROIChannel)chan;
            ROI roi = roiChan.GetROI();
            ROIStartTextBox.Text = roi.GetROIStart().ToString();
            ROIEndTextBox.Text = roi.GetROIEnd().ToString();
            
            switch(roiChan.GetROI().GetBGType())
            {
                case ROI.BG_Type.NONE:
                    ROIBackgroundComboBox.Text = "None";
                    break;
                case ROI.BG_Type.FLAT:
                    ROIBackgroundComboBox.Text = "Flat";
                    break;
                case ROI.BG_Type.LINEAR:
                    ROIBackgroundComboBox.Text = "Linear";
                    break;
            }

            BG1StartTextBox.Text = roi.GetBG1Start().ToString();
            BG1EndTextBox.Text = roi.GetBG1End().ToString();
            BG2StartTextBox.Text = roi.GetBG2Start().ToString();
            BG2EndTextBox.Text = roi.GetBG2End().ToString();
            
        }

        private void PopulateConvolvePanel(VirtualChannel chan, Instrument inst)
        {
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
            ConvolveChannelComboBox.Items.Clear();
            for (int i = 0; i < chanIndex; i++)
            {
                ConvolveChannelComboBox.Items.Add(instChannels[i].GetName());
            }
            ConvolveChannelComboBox.Text = chan.GetChannelA().GetName();
            FilterFileTextBox.Text = chan.GetDataFileName();
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
                    ROIVCPanel.Visible = false;
                    ConvolvePanel.Visible = false;
                    break;
                case "Add Constant":
                case "Scale":
                    PopulateVCChannelConstPanel(chan, inst);
                    VCTwoChannelPanel.Visible = false;
                    VCChannelConstPanel.Visible = true;
                    VCDelayPanel.Visible = false;
                    ROIVCPanel.Visible = false;
                    ConvolvePanel.Visible = false;
                    break;
                case "Delay":
                    PopulateDelayPanel(chan, inst);
                    VCTwoChannelPanel.Visible = false;
                    VCChannelConstPanel.Visible = false;
                    VCDelayPanel.Visible = true;
                    ROIVCPanel.Visible = false;
                    ConvolvePanel.Visible = false;
                    break;
                case "Convolve":
                    PopulateConvolvePanel(chan, inst);
                    VCTwoChannelPanel.Visible = false;
                    VCChannelConstPanel.Visible = false;
                    VCDelayPanel.Visible = false;
                    ROIVCPanel.Visible = false;
                    ConvolvePanel.Visible = true;
                    break;
                case "ROI":
                    if (!(chan is ROIChannel))
                    {
                        DialogResult dialogResult = MessageBox.Show("Are you sure you want to switch virtual channel to a ROI?\nThis will overwrite the current virutal channel.", "Switch to ROI", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            VirtualChannelTypeComboBox.Text = VCTypeToString(chan.GetVirtualChannelType());
                            return;
                        }
                        inst.GetVirtualChannels().Remove(chan);
                        ROIChannel roiChannel = new ROIChannel(chan.GetName(), (MCAInstrument)inst, Channel.ChannelType.DURATION_VALUE);
                        inst.GetVirtualChannels().Add(roiChannel);
                        chan = roiChannel;
                    }
                    PopulateROIVCPanel(chan, inst);
                    VCTwoChannelPanel.Visible = false;
                    VCChannelConstPanel.Visible = false;
                    VCDelayPanel.Visible = false;
                    ROIVCPanel.Visible = true;
                    ConvolvePanel.Visible = false;
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

        private void UpButton_Click(object sender, EventArgs e)
        {
            TreeNode node = SitesTreeView.SelectedNode;

            if (node.Tag is Site)
            {
                Site site = (Site)node.Tag;
                int index = siteMan.GetSites().IndexOf(site);
                if (index > 0)
                {
                    siteMan.GetSites().RemoveAt(index);
                    siteMan.GetSites().Insert(index - 1, site);

                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(site.GetName(), true)[0];
                }
            }
            else if (node.Tag is Facility)
            {
                Facility fac = (Facility)node.Tag;
                Site site = (Site)node.Parent.Tag;
                int index = site.GetFacilities().IndexOf(fac);
                if (index > 0)
                {
                    site.GetFacilities().RemoveAt(index);
                    site.GetFacilities().Insert(index - 1, fac);

                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(fac.GetName(), true)[0];
                }
            }
            else if (node.Tag is DetectionSystem)
            {
                DetectionSystem sys = (DetectionSystem)node.Tag;
                Facility fac = (Facility)node.Parent.Tag;
                int index = fac.GetSystems().IndexOf(sys);
                if (index > 0)
                {
                    fac.GetSystems().RemoveAt(index);
                    fac.GetSystems().Insert(index - 1, sys);

                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(sys.GetName(), true)[0];
                }
            }
            else if (node.Tag is Instrument)
            {
                Instrument inst = (Instrument)node.Tag;
                DetectionSystem sys = (DetectionSystem)node.Parent.Tag;
                int index = sys.GetInstruments().IndexOf(inst);
                if (index > 0)
                {
                    sys.GetInstruments().RemoveAt(index);
                    sys.GetInstruments().Insert(index - 1, inst);

                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(inst.GetName(), true)[0];
                }
            }
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            TreeNode node = SitesTreeView.SelectedNode;

            if (node.Tag is Site)
            {
                Site site = (Site)node.Tag;
                int index = siteMan.GetSites().IndexOf(site);
                if (index < siteMan.GetSites().Count - 1)
                {
                    siteMan.GetSites().RemoveAt(index);
                    siteMan.GetSites().Insert(index + 1, site);

                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(site.GetName(), true)[0];
                }
            }
            else if (node.Tag is Facility)
            {
                Facility fac = (Facility)node.Tag;
                Site site = (Site)node.Parent.Tag;
                int index = site.GetFacilities().IndexOf(fac);
                if (index < site.GetFacilities().Count - 1)
                {
                    site.GetFacilities().RemoveAt(index);
                    site.GetFacilities().Insert(index + 1, fac);

                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(fac.GetName(), true)[0];
                }
            }
            else if (node.Tag is DetectionSystem)
            {
                DetectionSystem sys = (DetectionSystem)node.Tag;
                Facility fac = (Facility)node.Parent.Tag;
                int index = fac.GetSystems().IndexOf(sys);
                if (index < fac.GetSystems().Count - 1)
                {
                    fac.GetSystems().RemoveAt(index);
                    fac.GetSystems().Insert(index + 1, sys);

                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(sys.GetName(), true)[0];
                }
            }
            else if (node.Tag is Instrument)
            {
                Instrument inst = (Instrument)node.Tag;
                DetectionSystem sys = (DetectionSystem)node.Parent.Tag;
                int index = sys.GetInstruments().IndexOf(inst);
                if (index < sys.GetInstruments().Count - 1)
                {
                    sys.GetInstruments().RemoveAt(index);
                    sys.GetInstruments().Insert(index + 1, inst);

                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(inst.GetName(), true)[0];
                }
            }
        }

        private void VCUpButton_Click(object sender, EventArgs e)
        {
            Instrument inst = (Instrument)SitesTreeView.SelectedNode.Tag;
            VirtualChannel chan = null;
            foreach (VirtualChannel otherChan in inst.GetVirtualChannels())
            {
                if (otherChan.GetName() == VirtualChannelsComboBox.Text)
                {
                    chan = otherChan;
                    break;
                }
            }

            int index = inst.GetVirtualChannels().IndexOf(chan);
            if (index > 0)
            {
                // Check references to early channels don't get messed up
                if (chan.GetChannelA().Equals(inst.GetVirtualChannels()[index-1]))
                {
                    MessageBox.Show("Cannot move channel up: it references the virtual channel above it!");
                    return;
                }
                if ((chan.GetVirtualChannelType() == VirtualChannel.VirtualChannelType.RATIO ||
                    chan.GetVirtualChannelType() == VirtualChannel.VirtualChannelType.SUM ||
                    chan.GetVirtualChannelType() == VirtualChannel.VirtualChannelType.DIFFERENCE) && chan.GetChannelB().Equals(inst.GetVirtualChannels()[index - 1]))
                {
                    MessageBox.Show("Cannot move channel up: it references the the virtual channel above it!");
                    return;
                }

                // Ok, move the channel up
                    inst.GetVirtualChannels().RemoveAt(index);
                inst.GetVirtualChannels().Insert(index - 1, chan);

                siteMan.Save();
                UpdateSitesTree();
                siteManChanged = true;
                SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(inst.GetName(), true)[0];
                VirtualChannelsComboBox.Text = chan.GetName();
            }
        }

        private void VCDownButton_Click(object sender, EventArgs e)
        {
            Instrument inst = (Instrument)SitesTreeView.SelectedNode.Tag;
            VirtualChannel chan = null;
            foreach (VirtualChannel otherChan in inst.GetVirtualChannels())
            {
                if (otherChan.GetName() == VirtualChannelsComboBox.Text)
                {
                    chan = otherChan;
                    break;
                }
            }

            int index = inst.GetVirtualChannels().IndexOf(chan);
            if (index < inst.GetVirtualChannels().Count() - 1)
            {
                VirtualChannel nextChan = inst.GetVirtualChannels()[index + 1];
                // Check references to early channels don't get messed up
                if (chan.Equals(nextChan.GetChannelA()))
                {
                    MessageBox.Show("Cannot move channel down: it is referenced by the virtual channel below it!");
                    return;
                }
                if ((nextChan.GetVirtualChannelType() == VirtualChannel.VirtualChannelType.RATIO ||
                    nextChan.GetVirtualChannelType() == VirtualChannel.VirtualChannelType.SUM ||
                    nextChan.GetVirtualChannelType() == VirtualChannel.VirtualChannelType.DIFFERENCE) && nextChan.GetChannelB().Equals(chan))
                {
                    MessageBox.Show("Cannot move channel down: it is referenced by the virtual channel below it!");
                    return;
                }

                // Ok, move the channel up
                inst.GetVirtualChannels().RemoveAt(index);
                inst.GetVirtualChannels().Insert(index + 1, chan);

                siteMan.Save();
                UpdateSitesTree();
                siteManChanged = true;
                SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(inst.GetName(), true)[0];
                VirtualChannelsComboBox.Text = chan.GetName();
            }
        }

        private void DeclarationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (DeclarationCheckBox.Checked)
            {
                selectedSystem.SetDeclarationInstrument(new DeclarationInstrument(selectedSystem.GetName() + "_Declarations"));
                SetupSystemPanel();
            }
            else
            {
                selectedSystem.RemoveDeclarationInstrument();
                SetupSystemPanel();
            }
        }

        private void DeclarationsDirectoryButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                DeclarationDirectoryTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void FilterFileBbutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FilterFileTextBox.Text = dialog.FileName;
            }
        }
    }
}
