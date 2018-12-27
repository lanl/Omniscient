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
    public partial class EventManagerForm : Form
    {
        // SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
        // State -- This is a little bit of an experiment in coding style.
        //          If this works better than the current (clearly bad) system,
        //          it should be implemented on all "state-like" parameters on
        //          the form -- be sure to delete this comment at the end of
        //          the experiment!
        Action selectedAction;
        Channel selectedActionChannel;
        // SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS

        MainForm main;
        SiteManager siteMan;

        bool siteManChanged = false;

        public EventManagerForm(MainForm master, SiteManager newSiteMan)
        {
            main = master;
            siteMan = newSiteMan;

            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
        }

        private void EventManagerForm_Load(object sender, EventArgs e)
        {
            SitesTreeView.ImageList = main.TreeImageList;
            UpButton.Image = main.ButtonImageList.Images[0];
            DownButton.Image = main.ButtonImageList.Images[1];
            AddButton.Image = main.ButtonImageList.Images[2];
            DeleteButton.Image = main.ButtonImageList.Images[3];
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
                siteNode.Name = site.Name;
                siteNode.Tag = site;
                siteNode.ImageIndex = 0;
                siteNode.SelectedImageIndex = 0;
                siteNode.ToolTipText = siteNode.Text;
                foreach (Facility fac in site.GetFacilities())
                {
                    TreeNode facNode = new TreeNode(fac.Name);
                    facNode.Name = fac.Name;
                    facNode.Tag = fac;
                    facNode.ImageIndex = 1;
                    facNode.SelectedImageIndex = 1;
                    facNode.ToolTipText = facNode.Text;
                    foreach (DetectionSystem sys in fac.GetSystems())
                    {
                        TreeNode sysNode = new TreeNode(sys.Name);
                        sysNode.Name = sys.Name;
                        sysNode.Tag = sys;
                        sysNode.ImageIndex = 2;
                        sysNode.SelectedImageIndex = 2;
                        sysNode.ToolTipText = sysNode.Text;
                        foreach (Instrument inst in sys.GetInstruments())
                        {
                            TreeNode instNode = new TreeNode(inst.Name);
                            instNode.Name = inst.Name;
                            instNode.Tag = inst;
                            instNode.ImageIndex = 3;
                            instNode.SelectedImageIndex = 3;
                            instNode.ToolTipText = instNode.Text;
                            sysNode.Nodes.Add(instNode);
                        }
                        foreach (EventGenerator eg in sys.GetEventGenerators())
                        {
                            TreeNode egNode = new TreeNode(eg.Name);
                            egNode.Name = eg.Name;
                            egNode.NodeFont = new Font(SitesTreeView.Font, FontStyle.Bold);
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

        public void SetupActionGroupBox()
        {
            EventGenerator eg = (EventGenerator)SitesTreeView.SelectedNode.Tag;
            Action action = null;
            foreach(Action otherAction in eg.GetActions())
            {
                if (otherAction.Name == ActionsComboBox.Text)
                    action = otherAction;
            }
            selectedAction = action;
            ActionNameTextBox.Text = action.Name;
            if(action is AnalysisAction)
            {
                ActionTypeComboBox.Text = "Analysis";
                PopulateAnalysisPanels((AnalysisAction)action, eg);
            }
            else if (action is CommandAction)
            {
                ActionTypeComboBox.Text = "Command";
                PopulateCommandPanels((CommandAction)action, eg);
            }
        }

        public void ResetFields()
        {
            selectedAction = null;
            selectedActionChannel = null;
            TreeNode node = SitesTreeView.SelectedNode;

            if (node.Tag is EventGenerator)
            {
                DetectionSystem eventWatcher = (DetectionSystem)node.Parent.Tag;

                NameTextBox.Enabled = true;

                EventGenerator eg = (EventGenerator)node.Tag;
                List<Parameter> parameters = eg.GetParameters();
                ParamListPanel.LoadParameters(parameters);
                ParamListPanel.Visible = true;
                NameTextBox.Text = eg.Name;

                ActionPanel.Visible = true;
                ActionsComboBox.Items.Clear();
                ActionsComboBox.Text = "";
                if (eg.GetActions().Count > 0)
                {
                    foreach (Action action in eg.GetActions())
                        ActionsComboBox.Items.Add(action.Name);
                    ActionsComboBox.Text = eg.GetActions()[0].Name;
                    selectedAction = eg.GetActions()[0];
                    SetupActionGroupBox();
                    ActionGroupBox.Visible = true;
                }
                else
                    ActionGroupBox.Visible = false;

                UpButton.Enabled = true;
                DownButton.Enabled = true;
                AddButton.Enabled = true;
                DeleteButton.Enabled = true;
                SaveButton.Enabled = true;
            }
            else if (node.Tag is DetectionSystem)
            {
                NameTextBox.Text = "";
                NameTextBox.Enabled = false;

                ParamListPanel.Visible = false;
                ActionPanel.Visible = false;

                UpButton.Enabled = false;
                DownButton.Enabled = false;
                AddButton.Enabled = true;
                DeleteButton.Enabled = false;
                SaveButton.Enabled = false;
            }
            else
            {
                NameTextBox.Text = "";
                NameTextBox.Enabled = false;

                ParamListPanel.Visible = false;
                ActionPanel.Visible = false;

                UpButton.Enabled = false;
                DownButton.Enabled = false;
                AddButton.Enabled = false;
                DeleteButton.Enabled = false;
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
                if (main.siteMan.Reload() != ReturnCode.SUCCESS) MessageBox.Show("Warning: Bad trouble loading the site manager!");
                if (main.presetMan.Reload() != ReturnCode.SUCCESS) MessageBox.Show("Warning: Bad trouble loading the preset manager!");
                main.UpdateSitesTree();
            }
            Close();
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            TreeNode node = SitesTreeView.SelectedNode;

            if (node.Tag is EventGenerator)
            {
                EventGenerator eg = (EventGenerator)node.Tag;
                DetectionSystem eventWatcher = (DetectionSystem)node.Parent.Tag;
                int index = eventWatcher.GetEventGenerators().IndexOf(eg);
                if (index > 0)
                {
                    eg.SetIndex(index - 1);

                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(eg.Name, true)[0];
                }
            }
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            TreeNode node = SitesTreeView.SelectedNode;

            if (node.Tag is EventGenerator)
            {
                EventGenerator eg = (EventGenerator)node.Tag;
                DetectionSystem eventWatcher = (DetectionSystem)node.Parent.Tag;
                int index = eventWatcher.GetEventGenerators().IndexOf(eg);
                if (index < eventWatcher.GetEventGenerators().Count-1)
                {
                    eg.SetIndex(index + 1);
                    
                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(eg.Name, true)[0];
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            TreeNode node = SitesTreeView.SelectedNode;

            if (node.Tag is EventGenerator)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete " + node.Text + "?", "Delete Item", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                    return;

                EventGenerator eg = (EventGenerator)node.Tag;
                DetectionSystem sys = (DetectionSystem)node.Parent.Tag;
                eg.Delete();

                siteMan.Save();
                UpdateSitesTree();
                siteManChanged = true;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            EventTypeDialog dialog = new EventTypeDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.Cancel) return;

            TreeNode node = SitesTreeView.SelectedNode;
            DetectionSystem eventWatcher;

            int insertIndex = -1;
            if (node.Tag is DetectionSystem)
            {
                eventWatcher = (DetectionSystem)node.Tag;
            }
            else
            {
                eventWatcher = (DetectionSystem)node.Parent.Tag;
                insertIndex = eventWatcher.GetEventGenerators().IndexOf((EventGenerator)node.Tag) + 1;
            }

            EventGeneratorHookup hookup = EventGenerator.GetHookup(dialog.eventType);
            string name = "";

            bool uniqueName = false;
            int iteration = 0;
            while (!uniqueName)
            {
                iteration++;
                name = "New-" + hookup.Type + "-" + iteration.ToString();
                uniqueName = !siteMan.ContainsName(name);
            }

            List<string> validSystemChannels = new List<string>();
            foreach(Instrument inst in ((DetectionSystem)eventWatcher).GetInstruments())
            {
                foreach(Channel chan in inst.GetChannels())
                {
                    validSystemChannels.Add(chan.Name);
                }
            }

            List<string> validEGs = new List<string>();
            foreach (EventGenerator otherEG in eventWatcher.GetEventGenerators())
            {
                validEGs.Add(otherEG.Name);
            }

            List<Parameter> parameters = new List<Parameter>();
            foreach (ParameterTemplate paramTemp in hookup.TemplateParameters)
            {
                switch (paramTemp.Type)
                {
                    case ParameterType.Double:
                        parameters.Add(new DoubleParameter(paramTemp.Name) { Value = "0" });
                        break;
                    case ParameterType.Enum:
                        parameters.Add(new EnumParameter(paramTemp.Name) { Value = paramTemp.ValidValues[0], ValidValues = paramTemp.ValidValues });
                        break;
                    case ParameterType.SystemChannel:
                        if(validSystemChannels.Count == 0)
                        {
                            MessageBox.Show("No channels to watch in this system!");
                            return;
                        }
                        parameters.Add(new SystemChannelParameter(paramTemp.Name, (DetectionSystem)eventWatcher)
                        {
                            Value = validSystemChannels[0],
                            ValidValues = validSystemChannels
                        });
                        break;
                    case ParameterType.SystemEventGenerator:
                        if (validEGs.Count == 0)
                        {
                            MessageBox.Show("No event generators to watch in this system!");
                            return;
                        }
                        parameters.Add(new SystemEventGeneratorParameter(paramTemp.Name, (DetectionSystem)eventWatcher)
                        {
                            Value = validEGs[0],
                            ValidValues = validEGs
                        });
                        break;
                    case ParameterType.TimeSpan:
                        parameters.Add(new TimeSpanParameter(paramTemp.Name) { Value = "0" });
                        break;
                }
            }

            EventGenerator eg = hookup.FromParameters(eventWatcher, name, parameters, 0);

            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(eg.Name, true)[0];
        }

        private void SaveAction(EventGenerator eg, Action action)
        {
            TreeNode node = SitesTreeView.SelectedNode;
            DetectionSystem eventWatcher = (DetectionSystem)node.Parent.Tag;
            DetectionSystem sys = (DetectionSystem)eventWatcher;

            if (action.Name != ActionsComboBox.Text && siteMan.ContainsName(ActionsComboBox.Text))
            {
                MessageBox.Show("All items in the Site Manager require a unique name!");
                return;
            }
            action.Name = ActionNameTextBox.Text;
            switch (ActionTypeComboBox.Text)
            {
                case "Analysis":
                    AnalysisAction analysisAction = (AnalysisAction)action;
                    analysisAction.GetChannels().Clear();
                    foreach (Instrument inst in sys.GetInstruments())
                    {
                        foreach (Channel ch in inst.GetChannels())
                        {
                            if (ch.Name == DataCompilerPanel1.ChannelComboBox.Text)
                            {
                                analysisAction.AddChannel(ch);
                                break;
                            }
                        }
                    }
                    analysisAction.GetDataCompilers().Clear();
                    switch (DataCompilerPanel1.DataCompilersComboBox.Text)
                    {
                        case "Spectrum Compiler":
                            analysisAction.GetDataCompilers().Add(new SpectrumCompiler("", new CHNParser(), new CHNWriter()));
                            break;
                        case "File List":
                            analysisAction.GetDataCompilers().Add(new FileListCompiler(""));
                            break;
                        default:
                            MessageBox.Show("Invalid data compiler type!");
                            return;
                    }
                    analysisAction.GetAnalysis().SetCommand(AnalysisCommandTextBox.Text);
                    analysisAction.SetCompiledFileName(DataCompilerPanel1.CompiledFileTextBox.Text);
                    switch (ResultParserComboBox.Text)
                    {
                        case "FRAM-Pu":
                            analysisAction.GetAnalysis().SetResultParser(new FRAMPlutoniumResultParser());
                            break;
                        case "FRAM-U":
                            analysisAction.GetAnalysis().SetResultParser(new FRAMUraniumResultParser());
                            break;
                        default:
                            MessageBox.Show("Invalid result parser type!");
                            return;
                    }
                    analysisAction.GetAnalysis().SetResultsFile(ResultFileTextBox.Text);
                    break;
                case "Command":
                    ((CommandAction)action).SetCommand(ActionCommandTextBox.Text);
                    break;
                default:
                    MessageBox.Show("Invalid action type!");
                    return;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            TreeNode node = SitesTreeView.SelectedNode;
            Action act = null;
            if (node.Tag is EventGenerator)
            {
                DetectionSystem eventWatcher = (DetectionSystem)node.Parent.Tag;
                EventGenerator eg = (EventGenerator)node.Tag;
                if (eg.Name != NameTextBox.Text && siteMan.ContainsName(NameTextBox.Text))
                {
                    MessageBox.Show("All items in the Site Manager and Event Manager require a unique name!");
                    return;
                }
                if (!ParamListPanel.ValidateInput()) return;
                EventGeneratorHookup hookup = EventGenerator.GetHookup(eg.GetEventGeneratorType());

                int index = 0;
                List<EventGenerator> egs = (eg.Parent as DetectionSystem).GetEventGenerators();
                for (int i=0; i<egs.Count; i++)
                {
                    if (eg.ID == egs[i].ID)
                    {
                        index = i;
                        break;
                    }
                }
                eg.Delete();
                eg = hookup.FromParameters(eventWatcher, NameTextBox.Text, ParamListPanel.Parameters, eg.ID);
                eg.SetIndex(index);

                foreach (Action action in eg.GetActions())
                {
                    if(action.Name == ActionsComboBox.Text)
                    {
                        SaveAction(eg, action);
                        act = action;
                        break;
                    }
                }

                siteMan.Save();
                UpdateSitesTree();
                siteManChanged = true;
                SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(eg.Name, true)[0];
                if(act!=null)
                {
                    ActionsComboBox.Text = act.Name;
                }
            }
        }

        private void AddActionButton_Click(object sender, EventArgs e)
        {
            EventGenerator eg = (EventGenerator)SitesTreeView.SelectedNode.Tag;
            int iteration = 0;
            bool uniqueName = false;
            string name = "";
            while (!uniqueName)
            {
                iteration++;
                name = "New-Action-" + iteration.ToString();
                uniqueName = !siteMan.ContainsName(name);
            }
            CommandAction action = new CommandAction(eg, name, 0);
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(eg.Name, true)[0];
            ActionsComboBox.Text = name;
            selectedAction = action;
        }

        private void ActionsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupActionGroupBox();
        }

        private void RemoveActionButton_Click(object sender, EventArgs e)
        {
            EventGenerator eg = (EventGenerator)SitesTreeView.SelectedNode.Tag;
            Action action = null;
            foreach (Action otherAction in eg.GetActions())
            {
                if (otherAction.Name == ActionsComboBox.Text)
                    action = otherAction;
            }
            action.Delete();
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(eg.Name, true)[0];
            ResetFields();
        }

        private void PopulateCommandPanels(CommandAction action, EventGenerator eg)
        {
            ActionCommandTextBox.Text = action.GetCommand();
        }

        private void PopulateDataSourceTab(AnalysisAction action, int compiler)
        {
            DetectionSystem sys = (DetectionSystem)action.GetEventGenerator().GetEventWatcher();
            DataSourceTabControl.TabPages.Add("Data Source " + (compiler + 1).ToString());
            DataCompilerPanel compilerPanel = new DataCompilerPanel();
            compilerPanel.Dock = DockStyle.Fill;
            DataSourceTabControl.TabPages[compiler].Controls.Add(compilerPanel);

            compilerPanel.ChannelComboBox.Items.Clear();
            foreach (Instrument inst in sys.GetInstruments())
            {
                foreach (Channel ch in inst.GetChannels())
                {
                    compilerPanel.ChannelComboBox.Items.Add(ch.Name);
                }
            }
            compilerPanel.ChannelComboBox.Text = action.GetChannels()[compiler].Name;
        }

        private void PopulateAnalysisPanels(AnalysisAction action, EventGenerator eg)
        {
            TreeNode node = SitesTreeView.SelectedNode;

            DataSourceTabControl.TabPages.Clear();
            for (int i =0; i< action.GetDataCompilers().Count; ++i)
            {
                PopulateDataSourceTab(action, i);
            }

            selectedActionChannel = action.GetChannels()[0];
            DataCompilerPanel1.ChannelComboBox.Text = action.GetChannels()[0].Name;
            AnalysisCommandTextBox.Text = action.GetAnalysis().GetCommand();
            DataCompilerPanel1.CompiledFileTextBox.Text = action.GetCompiledFileName();
            ResultFileTextBox.Text = action.GetAnalysis().GetResultsFile();
            if(action.GetAnalysis().GetResultParser() is FRAMPlutoniumResultParser)
            {
                ResultParserComboBox.Text = "FRAM-Pu";
            }
            else if (action.GetAnalysis().GetResultParser() is FRAMUraniumResultParser)
            {
                ResultParserComboBox.Text = "FRAM-U";
            }
        }


        private void ActionTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventGenerator eg = (EventGenerator)SitesTreeView.SelectedNode.Tag;
            DetectionSystem eventWatcher = (DetectionSystem)SitesTreeView.SelectedNode.Parent.Tag;
            Action action = null;
            foreach(Action otherAction in eg.GetActions())
            {
                if (otherAction.Name == ActionsComboBox.Text)
                    action = otherAction;
            }
            selectedAction = action;
            switch (ActionTypeComboBox.Text)
            {
                case "Analysis":
                    if (!(action is AnalysisAction))
                    {
                        DialogResult dialogResult = MessageBox.Show("Are you sure you want to switch action to a Analysis?\nThis will overwrite the current action.", "Switch to Analysis", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            ActionTypeComboBox.Text = action.GetActionType();
                            return;
                        }
                        action.Delete();

                        AnalysisAction analysisAction = new AnalysisAction(eg, action.Name, 0);
                        analysisAction.AddChannel(((DetectionSystem)eventWatcher).GetInstruments()[0].GetChannels()[0]);
                        analysisAction.GetDataCompilers().Add(new SpectrumCompiler("", new CHNParser(), new CHNWriter()));
                        analysisAction.GetAnalysis().SetResultParser(new FRAMPlutoniumResultParser());

                        action = analysisAction;
                    }
                    PopulateAnalysisPanels((AnalysisAction)action, eg);
                    AnalysisPanel.Visible = true;
                    CommandPanel.Visible = false;
                    break;
                case "Command":
                    if (!(action is CommandAction))
                    {
                        DialogResult dialogResult = MessageBox.Show("Are you sure you want to switch action to a Command?\nThis will overwrite the current action.", "Switch to Command", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            ActionTypeComboBox.Text = action.GetActionType();
                            return;
                        }
                        action.Delete();
                        CommandAction analysisAction = new CommandAction(eg, action.Name, 0);
                        action = analysisAction;
                    }
                    PopulateCommandPanels((CommandAction)action, eg);
                    AnalysisPanel.Visible = false;
                    CommandPanel.Visible = true;
                    break;
                default:
                    MessageBox.Show("Invalid action type!");
                    return;
            }

        }

        private void UpdateDataCompilers()
        {
            if (selectedActionChannel.GetInstrument() is MCAInstrument)
            {
                DataCompilerPanel1.DataCompilersComboBox.Items.Clear();
                DataCompilerPanel1.DataCompilersComboBox.Items.Add("File List");
                DataCompilerPanel1.DataCompilersComboBox.Items.Add("Spectrum Compiler");
            }
            else
            {
                DataCompilerPanel1.DataCompilersComboBox.Items.Clear();
                DataCompilerPanel1.DataCompilersComboBox.Items.Add("File List");
            }

            if (((AnalysisAction)selectedAction).GetDataCompilers()[0] is SpectrumCompiler)
            {
                if (DataCompilerPanel1.DataCompilersComboBox.Items.Contains("Spectrum Compiler"))
                    DataCompilerPanel1.DataCompilersComboBox.Text = "Spectrum Compiler";
            }
            else if (((AnalysisAction)selectedAction).GetDataCompilers()[0] is FileListCompiler)
            {
                if (DataCompilerPanel1.DataCompilersComboBox.Items.Contains("File List"))
                    DataCompilerPanel1.DataCompilersComboBox.Text = "File List";
            }
        }

        private void AnalysisChannelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool breakout = false;
            foreach(Instrument inst in ((DetectionSystem) selectedAction.GetEventGenerator().GetEventWatcher()).GetInstruments())
            {
                foreach(Channel chan in inst.GetChannels())
                {
                    if (chan.Name == DataCompilerPanel1.ChannelComboBox.Text)
                    {
                        selectedActionChannel = chan;
                        breakout = true;
                        break;
                    }
                }
                if (breakout) break;
            }
            UpdateDataCompilers();
        }
    }
}
