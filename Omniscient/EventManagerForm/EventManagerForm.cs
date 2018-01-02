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

        public void PopulateChannelCombo(DetectionSystem sys)
        {
            ChannelComboBox.Items.Clear();
            foreach(Instrument inst in sys.GetInstruments())
            {
                foreach(Channel ch in inst.GetChannels())
                {
                    ChannelComboBox.Items.Add(ch.GetName());
                }
            }
        }

        public void SetupActionGroupBox()
        {
            EventGenerator eg = (EventGenerator)SitesTreeView.SelectedNode.Tag;
            Action action = null;
            foreach(Action otherAction in eg.GetActions())
            {
                if (otherAction.GetName() == ActionsComboBox.Text)
                    action = otherAction;
            }
            selectedAction = action;
            ActionNameTextBox.Text = action.GetName();
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
                EventWatcher eventWatcher = (EventWatcher)node.Parent.Tag;

                NameTextBox.Enabled = true;
                ChannelComboBox.Enabled = true;
                ThresholdTextBox.Enabled = true;
                DebounceTextBox.Enabled = true;
                DebounceComboBox.Enabled = true;

                EventGenerator eg = (EventGenerator)node.Tag;
                NameTextBox.Text = eg.GetName();
                if (eg is ThresholdEG)
                {
                    ThresholdEG threshEg = (ThresholdEG)eg;
                    PopulateChannelCombo((DetectionSystem)eventWatcher);
                    ChannelComboBox.Text = threshEg.GetChannel().GetName();
                    ThresholdTextBox.Text = threshEg.GetThreshold().ToString();
                    TimeSpan debounce = threshEg.GetDebounceTime();
                    if (debounce.TotalSeconds == 0)
                    {
                        DebounceTextBox.Text = "0";
                        DebounceComboBox.Text = "Seconds";
                    }
                    else if (Math.Abs(debounce.TotalDays % 1) <= (Double.Epsilon * 100))
                    {
                        DebounceTextBox.Text = debounce.TotalDays.ToString();
                        DebounceComboBox.Text = "Days";
                    }
                    else if (Math.Abs(debounce.TotalHours % 1) <= (Double.Epsilon * 100))
                    {
                        DebounceTextBox.Text = debounce.TotalHours.ToString();
                        DebounceComboBox.Text = "Hours";
                    }
                    else if (Math.Abs(debounce.TotalMinutes % 1) <= (Double.Epsilon * 100))
                    {
                        DebounceTextBox.Text = debounce.TotalMinutes.ToString();
                        DebounceComboBox.Text = "Minutes";
                    }
                    else if (Math.Abs(debounce.TotalSeconds % 1) <= (Double.Epsilon * 100))
                    {
                        DebounceTextBox.Text = debounce.TotalSeconds.ToString();
                        DebounceComboBox.Text = "Seconds";
                    }

                    ThresholdPanel.Visible = true;
                    CoincidencePanel.Visible = false;
                }
                else if (eg is CoincidenceEG)
                {
                    CoincidenceEG coinkEG = (CoincidenceEG)eg;
                    int index = eventWatcher.GetEventGenerators().IndexOf(eg);
                    EventGeneratorAComboBox.Items.Clear();
                    EventGeneratorBComboBox.Items.Clear();
                    EventGenerator otherEG;
                    for(int i=0; i< index;i++)
                    {
                        otherEG = eventWatcher.GetEventGenerators()[i];
                        EventGeneratorAComboBox.Items.Add(otherEG.GetName());
                        EventGeneratorBComboBox.Items.Add(otherEG.GetName());
                    }
                    EventGeneratorAComboBox.Text = coinkEG.GetEventGeneratorA().GetName();
                    EventGeneratorBComboBox.Text = coinkEG.GetEventGeneratorB().GetName();

                    switch (coinkEG.GetCoincidenceType())
                    {
                        case CoincidenceEG.CoincidenceType.A_THEN_B:
                            CoincidenceTypeComboBox.Text = "A then B";
                            break;
                        case CoincidenceEG.CoincidenceType.B_THEN_A:
                            CoincidenceTypeComboBox.Text = "B then A";
                            break;
                        case CoincidenceEG.CoincidenceType.EITHER_ORDER:
                            CoincidenceTypeComboBox.Text = "Either order";
                            break;
                    }
                    switch (coinkEG.GetTimingType())
                    {
                        case CoincidenceEG.TimingType.START_TO_START:
                            TimingTypeComboBox.Text = "Start to Start";
                            break;
                        case CoincidenceEG.TimingType.START_TO_END:
                            TimingTypeComboBox.Text = "Start to End";
                            break;
                        case CoincidenceEG.TimingType.END_TO_START:
                            TimingTypeComboBox.Text = "End to Start";
                            break;
                        case CoincidenceEG.TimingType.END_TO_END:
                            TimingTypeComboBox.Text = "End to End";
                            break;
                        case CoincidenceEG.TimingType.MAX_TO_MAX:
                            TimingTypeComboBox.Text = "Max to Max";
                            break;
                    }

                    TimeSpan window = coinkEG.GetWindow();
                    if (window.TotalSeconds == 0)
                    {
                        WindowTextBox.Text = "0";
                        WindowComboBox.Text = "Seconds";
                    }
                    else if (Math.Abs(window.TotalDays % 1) <= (Double.Epsilon * 100))
                    {
                        WindowTextBox.Text = window.TotalDays.ToString();
                        WindowComboBox.Text = "Days";
                    }
                    else if (Math.Abs(window.TotalHours % 1) <= (Double.Epsilon * 100))
                    {
                        WindowTextBox.Text = window.TotalHours.ToString();
                        WindowComboBox.Text = "Hours";
                    }
                    else if (Math.Abs(window.TotalMinutes % 1) <= (Double.Epsilon * 100))
                    {
                        WindowTextBox.Text = window.TotalMinutes.ToString();
                        WindowComboBox.Text = "Minutes";
                    }
                    else if (Math.Abs(window.TotalSeconds % 1) <= (Double.Epsilon * 100))
                    {
                        WindowTextBox.Text = window.TotalSeconds.ToString();
                        WindowComboBox.Text = "Seconds";
                    }

                    TimeSpan minDiff = coinkEG.GetMinDifference();
                    if (minDiff.TotalSeconds == 0)
                    {
                        MinDifferenceTextBox.Text = "0";
                        MinDifferenceComboBox.Text = "Seconds";
                    }
                    else if (Math.Abs(minDiff.TotalDays % 1) <= (Double.Epsilon * 100))
                    {
                        MinDifferenceTextBox.Text = minDiff.TotalDays.ToString();
                        MinDifferenceComboBox.Text = "Days";
                    }
                    else if (Math.Abs(minDiff.TotalHours % 1) <= (Double.Epsilon * 100))
                    {
                        MinDifferenceTextBox.Text = minDiff.TotalHours.ToString();
                        MinDifferenceComboBox.Text = "Hours";
                    }
                    else if (Math.Abs(minDiff.TotalMinutes % 1) <= (Double.Epsilon * 100))
                    {
                        MinDifferenceTextBox.Text = minDiff.TotalMinutes.ToString();
                        MinDifferenceComboBox.Text = "Minutes";
                    }
                    else if (Math.Abs(minDiff.TotalSeconds % 1) <= (Double.Epsilon * 100))
                    {
                        MinDifferenceTextBox.Text = window.TotalSeconds.ToString();
                        MinDifferenceComboBox.Text = "Seconds";
                    }

                    ThresholdPanel.Visible = false;
                    CoincidencePanel.Visible = true;
                }

                ActionPanel.Visible = true;
                ActionsComboBox.Items.Clear();
                ActionsComboBox.Text = "";
                if (eg.GetActions().Count > 0)
                {
                    foreach (Action action in eg.GetActions())
                        ActionsComboBox.Items.Add(action.GetName());
                    ActionsComboBox.Text = eg.GetActions()[0].GetName();
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
            else if (node.Tag is EventWatcher)
            {
                NameTextBox.Text = "";
                NameTextBox.Enabled = false;

                ThresholdPanel.Visible = false;
                CoincidencePanel.Visible = false;
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

                ThresholdPanel.Visible = false;
                CoincidencePanel.Visible = false;
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
                EventWatcher eventWatcher = (EventWatcher)node.Parent.Tag;
                int index = eventWatcher.GetEventGenerators().IndexOf(eg);
                if (index > 0)
                {
                    eventWatcher.GetEventGenerators().RemoveAt(index);
                    eventWatcher.GetEventGenerators().Insert(index - 1, eg);

                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(eg.GetName(), true)[0];
                }
            }
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            TreeNode node = SitesTreeView.SelectedNode;

            if (node.Tag is EventGenerator)
            {
                EventGenerator eg = (EventGenerator)node.Tag;
                EventWatcher eventWatcher = (EventWatcher)node.Parent.Tag;
                int index = eventWatcher.GetEventGenerators().IndexOf(eg);
                if (index < eventWatcher.GetEventGenerators().Count-1)
                {
                    eventWatcher.GetEventGenerators().RemoveAt(index);
                    eventWatcher.GetEventGenerators().Insert(index+1, eg);

                    siteMan.Save();
                    UpdateSitesTree();
                    siteManChanged = true;
                    SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(eg.GetName(), true)[0];
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
                sys.GetEventGenerators().Remove(eg);

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
            EventWatcher eventWatcher;

            int insertIndex = -1;
            if (node.Tag is EventWatcher)
            {
                eventWatcher = (EventWatcher)node.Tag;
            }
            else
            {
                eventWatcher = (EventWatcher)node.Parent.Tag;
                insertIndex = eventWatcher.GetEventGenerators().IndexOf((EventGenerator)node.Tag) + 1;
            }

            bool uniqueName = false;
            int iteration = 0;

            string name = "";

            EventGenerator eg = null;
            switch (dialog.eventType)
            {
                case "Threshold":
                    if(((DetectionSystem)eventWatcher).GetInstruments().Count == 0)
                    {
                        MessageBox.Show("No instruments to watch in this system!");
                        return;
                    }
                    while (!uniqueName)
                    {
                        iteration++;
                        name = "New-ThresholdEG-" + iteration.ToString();
                        uniqueName = !siteMan.ContainsName(name);
                    }
                    eg = new ThresholdEG(eventWatcher, name, ((DetectionSystem)eventWatcher).GetInstruments()[0].GetChannels()[0], 0);
                    break;
                case "Coincidence":
                    if(eventWatcher.GetEventGenerators().Count == 0)
                    {
                        MessageBox.Show("No event generators to watch in this system!");
                        return;
                    }
                    while (!uniqueName)
                    {
                        iteration++;
                        name = "New-CoincidenceEG-" + iteration.ToString();
                        uniqueName = !siteMan.ContainsName(name);
                    }
                    eg = new CoincidenceEG(eventWatcher, name);
                    ((CoincidenceEG)eg).SetEventGeneratorA(eventWatcher.GetEventGenerators()[0]);
                    ((CoincidenceEG)eg).SetEventGeneratorB(eventWatcher.GetEventGenerators()[0]);
                    break;
            }

            //if (dialog.eventType == "Threshold")
            //{
            //    NewEventDialog tDialog = new NewEventDialog((DetectionSystem)eventWatcher);  // Probably not good for long term...
            //    DialogResult tResult = tDialog.ShowDialog();
            //    if (tResult == DialogResult.Cancel) return;

            //    eg = new ThresholdEG(tDialog.name, tDialog.channel, tDialog.threshold, tDialog.debounceTime);
            //}
            //else if (dialog.eventType == "Coincidence")
            //{
            //    MessageBox.Show("Let's make a coincidence event!");
            //}
            //else return;

            eventWatcher.GetEventGenerators().Add(eg);

            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(eg.GetName(), true)[0];
        }

        private void SaveAction(EventGenerator eg, Action action)
        {
            TreeNode node = SitesTreeView.SelectedNode;
            EventWatcher eventWatcher = (EventWatcher)node.Parent.Tag;
            DetectionSystem sys = (DetectionSystem)eventWatcher;

            if (action.GetName() != ActionsComboBox.Text && siteMan.ContainsName(ActionsComboBox.Text))
            {
                MessageBox.Show("All items in the Site Manager require a unique name!");
                return;
            }
            action.SetName(ActionNameTextBox.Text);
            switch (ActionTypeComboBox.Text)
            {
                case "Analysis":
                    AnalysisAction analysisAction = (AnalysisAction)action;
                    analysisAction.GetChannels().Clear();
                    foreach (Instrument inst in sys.GetInstruments())
                    {
                        foreach (Channel ch in inst.GetChannels())
                        {
                            if (ch.GetName() == AnalysisChannelComboBox.Text)
                            {
                                analysisAction.AddChannel(ch);
                                break;
                            }
                        }
                    }
                    analysisAction.GetDataCompilers().Clear();
                    switch (DataCompilersComboBox.Text)
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
                    analysisAction.SetCompiledFileName(CompiledFileTextBox.Text);
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
                EventWatcher eventWatcher = (EventWatcher)node.Parent.Tag;
                EventGenerator eg = (EventGenerator)node.Tag;
                if (eg.GetName() != NameTextBox.Text && siteMan.ContainsName(NameTextBox.Text))
                {
                    MessageBox.Show("All items in the Site Manager and Event Manager require a unique name!");
                    return;
                }
                eg.SetName(NameTextBox.Text);
                if (eg is ThresholdEG)
                {
                    DetectionSystem sys = (DetectionSystem)eventWatcher;
                    ThresholdEG threshEg = (ThresholdEG)eg;

                    foreach (Instrument inst in sys.GetInstruments())
                    {
                        foreach (Channel ch in inst.GetChannels())
                        {
                            if (ch.GetName() == ChannelComboBox.Text)
                            {
                                threshEg.SetChannel(ch);
                                break;
                            }
                        }
                    }

                    threshEg.SetThreshold(double.Parse(ThresholdTextBox.Text));

                    TimeSpan debounceTime;
                    try
                    {
                        double debTextVal = double.Parse(DebounceTextBox.Text);
                        switch (DebounceComboBox.Text)
                        {
                            case "Seconds":
                                debounceTime = TimeSpan.FromSeconds(debTextVal);
                                break;
                            case "Minutes":
                                debounceTime = TimeSpan.FromMinutes(debTextVal);
                                break;
                            case "Hours":
                                debounceTime = TimeSpan.FromHours(debTextVal);
                                break;
                            case "Days":
                                debounceTime = TimeSpan.FromDays(debTextVal);
                                break;
                            default:
                                MessageBox.Show("Invalid debounce time unit!");
                                return;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Invalid debounce time!");
                        return;
                    }
                    threshEg.SetDebounceTime(debounceTime);
                }
                else if (eg is CoincidenceEG)
                {
                    CoincidenceEG coinkEG = (CoincidenceEG)eg;

                    foreach(EventGenerator otherEg in eventWatcher.GetEventGenerators())
                    {
                        if (otherEg.GetName() == EventGeneratorAComboBox.Text)
                            coinkEG.SetEventGeneratorA(otherEg);
                        if (otherEg.GetName() == EventGeneratorBComboBox.Text)
                            coinkEG.SetEventGeneratorB(otherEg);
                    }
                    switch (CoincidenceTypeComboBox.Text)
                    {
                        case "A then B":
                            coinkEG.SetCoincidenceType(CoincidenceEG.CoincidenceType.A_THEN_B);
                            break;
                        case "B then A":
                            coinkEG.SetCoincidenceType(CoincidenceEG.CoincidenceType.A_THEN_B);
                            break;
                        case "Either order":
                            coinkEG.SetCoincidenceType(CoincidenceEG.CoincidenceType.EITHER_ORDER);
                            break;
                        default:
                            MessageBox.Show("Invalid coincidence type!");
                            return;
                    }
                    switch(TimingTypeComboBox.Text)
                    {
                        case "Start to Start":
                            coinkEG.SetTimingType(CoincidenceEG.TimingType.START_TO_START);
                            break;
                        case "Start to End":
                            coinkEG.SetTimingType(CoincidenceEG.TimingType.START_TO_END);
                            break;
                        case "End to Start":
                            coinkEG.SetTimingType(CoincidenceEG.TimingType.END_TO_START);
                            break;
                        case "End to End":
                            coinkEG.SetTimingType(CoincidenceEG.TimingType.END_TO_END);
                            break;
                        case "Max to Max":
                            coinkEG.SetTimingType(CoincidenceEG.TimingType.MAX_TO_MAX);
                            break;
                        default:
                            MessageBox.Show("Invalid timing type!");
                            return;
                    }
                    TimeSpan window;
                    try
                    {
                        double windowTextVal = double.Parse(WindowTextBox.Text);
                        switch (WindowComboBox.Text)
                        {
                            case "Seconds":
                                window = TimeSpan.FromSeconds(windowTextVal);
                                break;
                            case "Minutes":
                                window = TimeSpan.FromMinutes(windowTextVal);
                                break;
                            case "Hours":
                                window = TimeSpan.FromHours(windowTextVal);
                                break;
                            case "Days":
                                window = TimeSpan.FromDays(windowTextVal);
                                break;
                            default:
                                MessageBox.Show("Invalid window time unit!");
                                return;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Invalid window time!");
                        return;
                    }
                    coinkEG.SetWindow(window);

                    TimeSpan minDiff;
                    try
                    {
                        double minDiffTextVal = double.Parse(MinDifferenceTextBox.Text);
                        switch (MinDifferenceComboBox.Text)
                        {
                            case "Seconds":
                                minDiff = TimeSpan.FromSeconds(minDiffTextVal);
                                break;
                            case "Minutes":
                                minDiff = TimeSpan.FromMinutes(minDiffTextVal);
                                break;
                            case "Hours":
                                minDiff = TimeSpan.FromHours(minDiffTextVal);
                                break;
                            case "Days":
                                minDiff = TimeSpan.FromDays(minDiffTextVal);
                                break;
                            default:
                                MessageBox.Show("Invalid min difference time unit!");
                                return;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Invalid min difference time!");
                        return;
                    }
                    coinkEG.SetMinDifference(minDiff);
                }

                foreach(Action action in eg.GetActions())
                {
                    if(action.GetName() == ActionsComboBox.Text)
                    {
                        SaveAction(eg, action);
                        act = action;
                        break;
                    }
                }

                siteMan.Save();
                UpdateSitesTree();
                siteManChanged = true;
                SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(eg.GetName(), true)[0];
                if(act!=null)
                {
                    ActionsComboBox.Text = act.GetName();
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
            CommandAction action = new CommandAction(eg, name);
            eg.GetActions().Add(action);
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(eg.GetName(), true)[0];
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
                if (otherAction.GetName() == ActionsComboBox.Text)
                    action = otherAction;
            }
            eg.GetActions().Remove(action);
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(eg.GetName(), true)[0];
            ResetFields();
        }

        private void PopulateCommandPanels(CommandAction action, EventGenerator eg)
        {
            ActionCommandTextBox.Text = action.GetCommand();
        }


        public void PopulateAnalysisChannelCombo(DetectionSystem sys)
        {
            AnalysisChannelComboBox.Items.Clear();
            foreach (Instrument inst in sys.GetInstruments())
            {
                foreach (Channel ch in inst.GetChannels())
                {
                    AnalysisChannelComboBox.Items.Add(ch.GetName());
                }
            }
        }

        private void PopulateAnalysisPanels(AnalysisAction action, EventGenerator eg)
        {
            TreeNode node = SitesTreeView.SelectedNode;
            selectedActionChannel = action.GetChannels()[0];
            PopulateAnalysisChannelCombo((DetectionSystem)node.Parent.Tag);
            AnalysisChannelComboBox.Text = action.GetChannels()[0].GetName();
            AnalysisCommandTextBox.Text = action.GetAnalysis().GetCommand();
            CompiledFileTextBox.Text = action.GetCompiledFileName();
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
            EventWatcher eventWatcher = (EventWatcher)SitesTreeView.SelectedNode.Parent.Tag;
            Action action = null;
            foreach(Action otherAction in eg.GetActions())
            {
                if (otherAction.GetName() == ActionsComboBox.Text)
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
                        eg.GetActions().Remove(action);

                        AnalysisAction analysisAction = new AnalysisAction(eg, action.GetName());
                        analysisAction.AddChannel(((DetectionSystem)eventWatcher).GetInstruments()[0].GetChannels()[0]);
                        analysisAction.GetDataCompilers().Add(new SpectrumCompiler("", new CHNParser(), new CHNWriter()));
                        analysisAction.GetAnalysis().SetResultParser(new FRAMPlutoniumResultParser());

                        eg.GetActions().Add(analysisAction);
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
                        eg.GetActions().Remove(action);
                        CommandAction analysisAction = new CommandAction(eg, action.GetName());
                        eg.GetActions().Add(analysisAction);
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
                DataCompilersComboBox.Items.Clear();
                DataCompilersComboBox.Items.Add("File List");
                DataCompilersComboBox.Items.Add("Spectrum Compiler");
            }
            else
            {
                DataCompilersComboBox.Items.Clear();
                DataCompilersComboBox.Items.Add("File List");
            }

            if (((AnalysisAction)selectedAction).GetDataCompilers()[0] is SpectrumCompiler)
            {
                if (DataCompilersComboBox.Items.Contains("Spectrum Compiler"))
                    DataCompilersComboBox.Text = "Spectrum Compiler";
            }
            else if (((AnalysisAction)selectedAction).GetDataCompilers()[0] is FileListCompiler)
            {
                if (DataCompilersComboBox.Items.Contains("File List"))
                    DataCompilersComboBox.Text = "File List";
            }
        }

        private void AnalysisChannelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool breakout = false;
            foreach(Instrument inst in ((DetectionSystem) selectedAction.GetEventGenerator().GetEventWatcher()).GetInstruments())
            {
                foreach(Channel chan in inst.GetChannels())
                {
                    if (chan.GetName() == AnalysisChannelComboBox.Text)
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
