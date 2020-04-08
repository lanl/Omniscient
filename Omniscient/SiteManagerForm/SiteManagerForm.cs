// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2018, Triad National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Triad National Security, LLC. This software was produced under U.S. Government contract 89233218CNA000001 for Los Alamos National Laboratory (LANL), which is operated by Triad National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR TRIAD NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Triad National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY TRIAD NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL TRIAD NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
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
        // SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
        // State
        DetectionSystem selectedSystem;
        VirtualChannel selectedVirtualChannel;
        Channel selectedChannel;

        // SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS

        MainForm main;
        SiteManager siteMan;

        bool siteManChanged = false;

        public SiteManagerForm(MainForm master, SiteManager newSiteMan)
        {
            main = master;
            siteMan = newSiteMan;

            selectedSystem = null;
            selectedChannel = null;
            selectedVirtualChannel = null;

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

            if (this.Top < 1) this.Top = 1;
            if (this.Height > Screen.PrimaryScreen.WorkingArea.Height) this.Height = Screen.PrimaryScreen.WorkingArea.Height;
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
                        foreach (Instrument inst in sys.GetInstruments())
                        {
                            if (!(inst is DeclarationInstrument))
                            {
                                TreeNode instNode = new TreeNode(inst.Name);
                                instNode.Name = inst.ID.ToString();
                                instNode.Tag = inst;
                                instNode.ImageIndex = 3;
                                instNode.SelectedImageIndex = 3;
                                instNode.ToolTipText = instNode.Text;
                                sysNode.Nodes.Add(instNode);
                            }
                        }
                        foreach (EventGenerator eg in sys.GetEventGenerators())
                        {
                            TreeNode egNode = new TreeNode(eg.Name);
                            egNode.Name = eg.ID.ToString();
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
            VirtualChannel chan = selectedVirtualChannel;
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
            VirtualChannelNameTextBox.Text = chan.Name;
            VirtualChannelTypeTextBox.Text = chan.VCType;
            
            List<Parameter> parameters = chan.GetParameters();
            VCParameterListPanel.LoadParameters(parameters);
            VCParameterListPanel.Visible = true;
            VirtualChannelGroupBox.Visible = true;
            ChannelGroupBox.Visible = false;
        }

        private void SetupChannelGroupBox()
        {
            ChannelNameTextBox.Text = selectedChannel.Name;

            List<Parameter> parameters = selectedChannel.GetParameters();
            ChannelParameterListPanel.LoadParameters(parameters);

            VirtualChannelGroupBox.Visible = false;
            ChannelGroupBox.Visible = true;
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
            selectedChannel = null;
            selectedVirtualChannel = null;
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
                NewFacilityButton.Enabled = true;
            }
            else if (node.Tag is Facility)
            {
                Facility fac = (Facility)node.Tag;
                TypeLabel.Text = "Facility";

                SystemPanel.Visible = false;
                InstrumentPanel.Visible = false;
                NewInstrumentButton.Enabled = false;
                NewSystemButton.Enabled = true;
                NewFacilityButton.Enabled = true;
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
                NewFacilityButton.Enabled = true;
            }
            else if (node.Tag is Instrument)
            {
                Instrument inst = (Instrument)node.Tag;
                TypeLabel.Text = "Instrument";
                InstrumentParameterListPanel.LoadParameters(inst.GetParameters());
                InstTypeTextBox.Text = inst.InstrumentType;
                
                ChannelsComboBox.Items.Clear();
                Channel[] channels = inst.GetChannels();
                if (channels.Length > 0)
                {
                    foreach (Channel channel in channels)
                        ChannelsComboBox.Items.Add(channel.Name);
                    if (channels[0] is VirtualChannel)
                    {
                        selectedChannel = null;
                        selectedVirtualChannel = (VirtualChannel)channels[0];
                        ChannelsComboBox.SelectedItem = selectedVirtualChannel.Name;
                        SetupVirtualChannelGroupBox();
                    }
                    else
                    {
                        selectedChannel = channels[0];
                        selectedVirtualChannel = null;
                        ChannelsComboBox.SelectedItem = selectedChannel.Name;
                        SetupChannelGroupBox();
                    }
                }
                else
                    VirtualChannelGroupBox.Visible = false;

                SystemPanel.Visible = false;
                InstrumentPanel.Visible = true;
                NewInstrumentButton.Enabled = true;
                NewSystemButton.Enabled = true;
                NewFacilityButton.Enabled = true;
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
                if (main.Core.SiteManager.Reload() != ReturnCode.SUCCESS) MessageBox.Show("Warning: Bad trouble loading the site manager!");
                if (main.presetMan.Reload() != ReturnCode.SUCCESS) MessageBox.Show("Warning: Bad trouble loading the preset manager!");
                main.UpdateSitesTree();
            }
            Close();
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Check if declaration exists
            // Need to check EVERY system!
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

            // G - Select first node
            SitesTreeView.SelectedNode = SitesTreeView.TopNode;

        }

        private void DiscardButton_Click(object sender, EventArgs e)
        {
            ResetFields();
        }

        private Channel SaveChannel(Instrument inst, Channel chan)
        {
            // Validate input
            if (chan.Name != ChannelNameTextBox.Text && inst.ChildrenContainName(ChannelNameTextBox.Text))
            {
                MessageBox.Show("All items in the Instrument require a unique name!");
                return null;
            }
            if (!ChannelParameterListPanel.ValidateInput()) return null;

            // Apply new settings
            chan.Name = ChannelNameTextBox.Text;
            chan.ApplyParameters(ChannelParameterListPanel.Parameters);

            return chan;
        }

        private VirtualChannel SaveVirtualChannel(Instrument inst, VirtualChannel chan)
        {
            if (chan.Name != VirtualChannelNameTextBox.Text && inst.ChildrenContainName(VirtualChannelNameTextBox.Text))
            {
                MessageBox.Show("All items in the Instrument require a unique name!");
                return null;
            }

            string name = VirtualChannelNameTextBox.Text;
            string type = VirtualChannelTypeTextBox.Text;
            
            if (!VCParameterListPanel.ValidateInput()) return null;
            VirtualChannelHookup hookup = VirtualChannel.GetHookup(type);
            List<VirtualChannel> virtualChannels = inst.GetVirtualChannels();
            int index = -1;
            for (int i=0; i< virtualChannels.Count; i++)
            {
                if (virtualChannels[i] == chan)
                {
                    index = i;
                    break;
                }
            }

            chan.Delete();
            chan = hookup.FromParameters(inst, name, VCParameterListPanel.Parameters, chan.ID);
            chan.SetIndex(index);
            return chan;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if ( siteMan.GetSites().Count == 0 )
            {
                MessageBox.Show("May as well make some changes before you save, am I right?");
                return;
            }
            TreeNode node = SitesTreeView.SelectedNode;
            string nodeName = node.Name;

            Channel chan = null;


            if (node.Tag is Site)
            {
                Site site = (Site)node.Tag;
                if(site.Name != NameTextBox.Text && siteMan.ChildrenContainName(NameTextBox.Text))
                {
                    MessageBox.Show("Each Site requires a unique name!");
                    return;
                }
                site.Name = NameTextBox.Text;
                nodeName = site.ID.ToString();
            }
            else if (node.Tag is Facility)
            {
                Facility fac = (Facility)node.Tag;
                if (fac.Name != NameTextBox.Text && fac.Parent.ChildrenContainName(NameTextBox.Text))
                {
                    MessageBox.Show("Each Facility in a Site requires a unique name!");
                    return;
                }
                fac.Name = NameTextBox.Text;
                nodeName = fac.ID.ToString();
            }
            else if (node.Tag is DetectionSystem)
            {
                DetectionSystem sys = (DetectionSystem)node.Tag;
                if (sys.Name != NameTextBox.Text && sys.Parent.ChildrenContainName(NameTextBox.Text))
                {
                    MessageBox.Show("Each System in a Facility requires a unique name!");
                    return;
                }
                sys.Name = NameTextBox.Text;
                nodeName = sys.ID.ToString();
                if(DeclarationCheckBox.Checked)
                {
                    selectedSystem.GetDeclarationInstrument().SetFilePrefix(DeclarationPrefixTextBox.Text);
                    selectedSystem.GetDeclarationInstrument().SetDataFolder(DeclarationDirectoryTextBox.Text);
                    selectedSystem.GetDeclarationInstrument().ScanDataFolder();
                }
            }
            else if (node.Tag is Instrument)
            {
                Instrument inst = (Instrument)node.Tag;
                if (inst.Name != NameTextBox.Text && inst.Parent.ChildrenContainName(NameTextBox.Text))
                {
                    MessageBox.Show("Each item in a System requires a unique name!");
                    return;
                }
                if (!InstrumentParameterListPanel.ValidateInput()) return;
                string name = NameTextBox.Text;
                string type = inst.InstrumentType;

                if(name != inst.Name)
                    inst.Name = name;
                inst.ApplyParameters(InstrumentParameterListPanel.Parameters);
                // selectedChannel and selectedVirtualChannel might not exist anymore

                if (selectedVirtualChannel != null)
                {
                    foreach(VirtualChannel vc in inst.GetVirtualChannels())
                    {
                        if (vc.Name == selectedVirtualChannel.Name)
                        {
                            selectedVirtualChannel = vc;
                            break;
                        }
                    }
                    chan = SaveVirtualChannel(inst, selectedVirtualChannel);
                }
                else if (selectedChannel != null)
                {
                    foreach (Channel c in inst.GetStandardChannels())
                    {
                        if (c.Name == selectedChannel.Name)
                        {
                            selectedChannel = c;
                            break;
                        }
                    }
                    chan = SaveChannel(inst, selectedChannel);
                }
                nodeName = inst.ID.ToString();
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
            if(selectedVirtualChannel != null)
            {
                ChannelsComboBox.SelectedItem = selectedVirtualChannel.Name;
            }
            else if(selectedChannel != null)
            {
                ChannelsComboBox.SelectedItem = selectedChannel.Name;
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
                site.Delete();
                // G - Turn off other buttons if there are no longer any sites
                if (siteMan.GetSites().Count == 0)
                {
                    DisableButtons();
                }

            }
            else if (node.Tag is Facility)
            {
                Facility fac = (Facility)node.Tag;
                fac.Delete();
            }
            else if (node.Tag is DetectionSystem)
            {
                DetectionSystem sys = (DetectionSystem)node.Tag;
                sys.Delete();
            }
            else if (node.Tag is Instrument)
            {
                Instrument inst = (Instrument)node.Tag;
                inst.Delete();
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

            // G - Select new node after deleting
            SitesTreeView.SelectedNode = SitesTreeView.TopNode;
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
                uniqueName = !siteMan.ChildrenContainName(name);
            }
            Site newSite = new Site(siteMan, name, 0);
            string id = newSite.ID.ToString();
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(id, true)[0];
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
                uniqueName = !site.ChildrenContainName(name);
            }
            Facility newFacility = new Facility(site, name, 0);
            string id = newFacility.ID.ToString();
            newFacility.SetIndex(index);
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(id, true)[0];
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
                uniqueName = !fac.ChildrenContainName(name);
            }
            DetectionSystem newSys = new DetectionSystem(fac, name, 0);
            string id = newSys.ID.ToString();
            newSys.SetIndex(index);
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(id, true)[0];
        }

        private void NewInstrumentButton_Click(object sender, EventArgs e)
        {
            // Choose instrument type
            InstTypeDialog dialog = new InstTypeDialog();
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
            string type = dialog.instrumentType;

            while (!uniqueName)
            {
                iteration++;
                name = "New-" + type + "-" + iteration.ToString();
                uniqueName = !sys.ChildrenContainName(name);
            }

            InstrumentHookup hookup = Instrument.GetHookup(type);
            List<Parameter> parameters = new List<Parameter>();
            foreach (ParameterTemplate paramTemp in hookup.TemplateParameters)
            {
                switch (paramTemp.Type)
                {
                    case ParameterType.String:
                        parameters.Add(new StringParameter(paramTemp.Name) { Value = "" });
                        break;
                    case ParameterType.Int:
                        parameters.Add(new IntParameter(paramTemp.Name) { Value = "0" });
                        break;
                    case ParameterType.Double:
                        parameters.Add(new DoubleParameter(paramTemp.Name) { Value = "0" });
                        break;
                    case ParameterType.Enum:
                        parameters.Add(new EnumParameter(paramTemp.Name) { Value = paramTemp.ValidValues[0], ValidValues = paramTemp.ValidValues });
                        break;
                    case ParameterType.TimeSpan:
                        parameters.Add(new TimeSpanParameter(paramTemp.Name) { Value = "0" });
                        break;
                    case ParameterType.FileName:
                        parameters.Add(new FileNameParameter(paramTemp.Name) { Value = "" });
                        break;
                    case ParameterType.Directory:
                        parameters.Add(new DirectoryParameter(paramTemp.Name) { Value = "" });
                        break;
                }
            }
            Instrument newInstrument = hookup.FromParameters(sys, name, parameters, 0);
            string id = newInstrument.ID.ToString();
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(id, true)[0];
        }

        private void AddVirtualChannelButton_Click(object sender, EventArgs e)
        {
            Instrument inst = (Instrument)SitesTreeView.SelectedNode.Tag;
            if (inst.GetChannels().Length == 0)
            {
                MessageBox.Show("The instrument has no channels to make a virtual instrument from!");
            }
            bool isMCA = false;
            if (inst is MCAInstrument) isMCA = true;

            VirtualChannelTypeDialog dialog = new VirtualChannelTypeDialog(isMCA);
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.Cancel) return;

            int iteration = 0;
            bool uniqueName = false;
            string name = "";
            while (!uniqueName)
            {
                iteration++;
                name = "New-VC-" + iteration.ToString();
                uniqueName = !inst.ChildrenContainName(name);
            }

            VirtualChannelHookup hookup = VirtualChannel.GetHookup(dialog.vcType);

            List<string> validInstrumentChannels = new List<string>();
            foreach (Channel chan in inst.GetChannels())
            {
                validInstrumentChannels.Add(chan.Name);
            }
            
            List<Parameter> parameters = new List<Parameter>();
            foreach (ParameterTemplate paramTemp in hookup.TemplateParameters)
            {
                switch (paramTemp.Type)
                {
                    case ParameterType.Int:
                        parameters.Add(new IntParameter(paramTemp.Name) { Value = "0" });
                        break;
                    case ParameterType.Double:
                        parameters.Add(new DoubleParameter(paramTemp.Name) { Value = "0" });
                        break;
                    case ParameterType.Enum:
                        parameters.Add(new EnumParameter(paramTemp.Name) { Value = paramTemp.ValidValues[0], ValidValues = paramTemp.ValidValues });
                        break;
                    case ParameterType.TimeSpan:
                        parameters.Add(new TimeSpanParameter(paramTemp.Name) { Value = "0" });
                        break;
                    case ParameterType.FileName:
                        parameters.Add(new FileNameParameter(paramTemp.Name) { Value = "" });
                        break;
                    case ParameterType.InstrumentChannel:
                        parameters.Add(new InstrumentChannelParameter(paramTemp.Name, inst) { Value = validInstrumentChannels[0] });
                        break;
                }
            }
            VirtualChannel virtualChannel = hookup.FromParameters(inst, name, parameters, 0);
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(inst.ID.ToString(), true)[0];
            ChannelsComboBox.SelectedItem = name;
            selectedVirtualChannel = virtualChannel;
        }

        private void VirtualChannelsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Instrument inst = (Instrument)SitesTreeView.SelectedNode.Tag;
            foreach (Channel otherChan in inst.GetChannels())
            {
                if (otherChan.Name == ChannelsComboBox.Text)
                {
                    if (otherChan is VirtualChannel)
                    {
                        selectedChannel = null;
                        selectedVirtualChannel = (VirtualChannel)otherChan;
                        SetupVirtualChannelGroupBox();
                    }
                    else
                    {
                        selectedChannel = otherChan;
                        selectedVirtualChannel = null;
                        SetupChannelGroupBox();
                    }

                }
            }
        }

        private void RemoveVirtualChannelButton_Click(object sender, EventArgs e)
        {
            Instrument inst = (Instrument)SitesTreeView.SelectedNode.Tag;
            VirtualChannel chan = selectedVirtualChannel;
            inst.GetVirtualChannels().Remove(chan);
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(inst.ID.ToString(), true)[0];
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
                    site.SetIndex(index - 1);
                    
                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(site.ID.ToString(), true)[0];
                }
            }
            else if (node.Tag is Facility)
            {
                Facility fac = (Facility)node.Tag;
                Site site = (Site)node.Parent.Tag;
                int index = site.GetFacilities().IndexOf(fac);
                if (index > 0)
                {
                    fac.SetIndex(index - 1);
                    
                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(fac.ID.ToString(), true)[0];
                }
            }
            else if (node.Tag is DetectionSystem)
            {
                DetectionSystem sys = (DetectionSystem)node.Tag;
                Facility fac = (Facility)node.Parent.Tag;
                int index = fac.GetSystems().IndexOf(sys);
                if (index > 0)
                {
                    sys.SetIndex(index - 1);

                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(sys.ID.ToString(), true)[0];
                }
            }
            else if (node.Tag is Instrument)
            {
                Instrument inst = (Instrument)node.Tag;
                DetectionSystem sys = (DetectionSystem)node.Parent.Tag;
                int index = sys.GetInstruments().IndexOf(inst);
                if (index > 0)
                {
                    inst.SetIndex(index - 1);

                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(inst.ID.ToString(), true)[0];
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
                    site.SetIndex(index + 1);
                    
                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(site.ID.ToString(), true)[0];
                }
            }
            else if (node.Tag is Facility)
            {
                Facility fac = (Facility)node.Tag;
                Site site = (Site)node.Parent.Tag;
                int index = site.GetFacilities().IndexOf(fac);
                if (index < site.GetFacilities().Count - 1)
                {
                    site.SetIndex(index + 1);
                    
                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(fac.ID.ToString(), true)[0];
                }
            }
            else if (node.Tag is DetectionSystem)
            {
                DetectionSystem sys = (DetectionSystem)node.Tag;
                Facility fac = (Facility)node.Parent.Tag;
                int index = fac.GetSystems().IndexOf(sys);
                if (index < fac.GetSystems().Count - 1)
                {
                    fac.SetIndex(index + 1);
                    
                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(sys.ID.ToString(), true)[0];
                }
            }
            else if (node.Tag is Instrument)
            {
                Instrument inst = (Instrument)node.Tag;
                DetectionSystem sys = (DetectionSystem)node.Parent.Tag;
                int index = sys.GetInstruments().IndexOf(inst);
                if (index < sys.GetInstruments().Count - 1)
                {
                    inst.SetIndex(index + 1);

                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(inst.ID.ToString(), true)[0];
                }
            }
        }

        private void VCUpButton_Click(object sender, EventArgs e)
        {
            Instrument inst = (Instrument)SitesTreeView.SelectedNode.Tag;
            VirtualChannel chan = selectedVirtualChannel;

            int index = inst.GetVirtualChannels().IndexOf(chan);
            if (index > 0)
            {
                // Check references to early channels don't get messed up
                foreach(Channel dependent in chan.Dependencies)
                {
                    if (dependent == inst.GetVirtualChannels()[index - 1])
                    {
                        MessageBox.Show("Cannot move channel up: it references the virtual channel above it!");
                        return;
                    }
                }

                // Ok, move the channel up
                chan.SetIndex(index - 1);
                
                siteMan.Save();
                UpdateSitesTree();
                siteManChanged = true;
                SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(inst.ID.ToString(), true)[0];
                ChannelsComboBox.SelectedItem = chan.Name;
            }
        }

        private void VCDownButton_Click(object sender, EventArgs e)
        {
            Instrument inst = (Instrument)SitesTreeView.SelectedNode.Tag;
            VirtualChannel chan = selectedVirtualChannel;

            int index = inst.GetVirtualChannels().IndexOf(chan);
            if (index < inst.GetVirtualChannels().Count() - 1)
            {
                VirtualChannel nextChan = inst.GetVirtualChannels()[index + 1];
                // Check references to early channels don't get messed up
                foreach (Channel dependent in nextChan.Dependencies)
                {
                    if (dependent == chan)
                    {
                        MessageBox.Show("Cannot move channel down: it is referenced by the virtual channel below it!");
                        return;
                    }
                }

                // Ok, move the channel up
                chan.SetIndex(index + 1);
                
                siteMan.Save();
                UpdateSitesTree();
                siteManChanged = true;
                SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(inst.ID.ToString(), true)[0];
                ChannelsComboBox.SelectedItem = chan.Name;
            }
        }

        private void DeclarationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (DeclarationCheckBox.Checked)
            {
                selectedSystem.SetDeclarationInstrument(new DeclarationInstrument(selectedSystem, selectedSystem.Name + "_Declarations", 0));
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

        // G - Shorthand to disable buttons on lower trees
        private void DisableButtons()
        {
            NewFacilityButton.Enabled = false;
            NewSystemButton.Enabled = false;
            NewInstrumentButton.Enabled = false;
        }
    }
}
