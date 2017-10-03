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
    public partial class EventManagerForm : Form
    {
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

        public void ResetFields()
        {
            TreeNode node = SitesTreeView.SelectedNode;

            if (node.Tag is EventGenerator)
            {
                NameTextBox.Enabled = true;
                ChannelComboBox.Enabled = true;
                ThresholdTextBox.Enabled = true;
                DebounceTextBox.Enabled = true;
                DebounceComboBox.Enabled = true;

                EventGenerator eg = (EventGenerator)node.Tag;
                NameTextBox.Text = eg.GetName();
                ThresholdEG threshEg = (ThresholdEG)eg;
                PopulateChannelCombo((DetectionSystem)node.Parent.Tag);
                ChannelComboBox.Text = threshEg.GetChannel().GetName();
                ThresholdTextBox.Text = threshEg.GetThreshold().ToString();
                TimeSpan debounce = threshEg.GetDebounceTime();
                if(debounce.TotalSeconds == 0)
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
                ChannelComboBox.Text = "";
                ChannelComboBox.Enabled = false;
                ThresholdTextBox.Text = "";
                ThresholdTextBox.Enabled = false;
                DebounceTextBox.Text = "";
                DebounceTextBox.Enabled = false;
                DebounceComboBox.Enabled = false;

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
                ChannelComboBox.Text = "";
                ChannelComboBox.Enabled = false;
                ThresholdTextBox.Text = "";
                ThresholdTextBox.Enabled = false;
                DebounceTextBox.Text = "";
                DebounceTextBox.Enabled = false;
                DebounceComboBox.Enabled = false;

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
                main.siteMan.Reload();
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

            NewEventDialog dialog = new NewEventDialog((DetectionSystem)eventWatcher);  // Probably not good for long term...
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.Cancel) return;

            EventGenerator eg = new ThresholdEG(dialog.name, dialog.channel, dialog.threshold, dialog.debounceTime);
            eventWatcher.GetEventGenerators().Add(eg);
            siteMan.Save();
            UpdateSitesTree();
            siteManChanged = true;
            SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(eg.GetName(), true)[0];
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            TreeNode node = SitesTreeView.SelectedNode;
            if (node.Tag is EventGenerator)
            {
                DetectionSystem sys = (DetectionSystem)node.Parent.Tag;
                EventGenerator eg = (EventGenerator)node.Tag;

                eg.SetName(NameTextBox.Text);

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

                siteMan.Save();
                UpdateSitesTree();
                siteManChanged = true;
                SitesTreeView.SelectedNode = SitesTreeView.Nodes.Find(eg.GetName(), true)[0];
            }
        }
    }
}
