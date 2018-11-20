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
namespace Omniscient
{
    partial class EventManagerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventManagerForm));
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.SitesTreeView = new System.Windows.Forms.TreeView();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.InnerRightPanel = new System.Windows.Forms.Panel();
            this.ActionPanel = new System.Windows.Forms.Panel();
            this.ActionGroupBox = new System.Windows.Forms.GroupBox();
            this.CommandPanel = new System.Windows.Forms.Panel();
            this.ActionCommandTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.AnalysisPanel = new System.Windows.Forms.Panel();
            this.AnalysisTabControl = new System.Windows.Forms.TabControl();
            this.DataTabPage = new System.Windows.Forms.TabPage();
            this.DataSourceTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.AnalysisTabPage = new System.Windows.Forms.TabPage();
            this.AnalysisCommandTextBox = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.ResultsTabPage = new System.Windows.Forms.TabPage();
            this.ResultParserComboBox = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.ResultFileTextBox = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.ActionSubPanel = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.ActionNameTextBox = new System.Windows.Forms.TextBox();
            this.ActionTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.RemoveActionButton = new System.Windows.Forms.Button();
            this.AddActionButton = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.ActionsComboBox = new System.Windows.Forms.ComboBox();
            this.NamePanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.LeftRightPanel = new System.Windows.Forms.Panel();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.UpButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.DataCompilerPanel1 = new Omniscient.DataCompilerPanel();
            this.ParamListPanel = new Omniscient.Controls.ParameterListPanel();
            this.LeftPanel.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.InnerRightPanel.SuspendLayout();
            this.ActionPanel.SuspendLayout();
            this.ActionGroupBox.SuspendLayout();
            this.CommandPanel.SuspendLayout();
            this.AnalysisPanel.SuspendLayout();
            this.AnalysisTabControl.SuspendLayout();
            this.DataTabPage.SuspendLayout();
            this.DataSourceTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.AnalysisTabPage.SuspendLayout();
            this.ResultsTabPage.SuspendLayout();
            this.ActionSubPanel.SuspendLayout();
            this.NamePanel.SuspendLayout();
            this.LeftRightPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // LeftPanel
            // 
            this.LeftPanel.Controls.Add(this.SitesTreeView);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Padding = new System.Windows.Forms.Padding(10);
            this.LeftPanel.Size = new System.Drawing.Size(400, 781);
            this.LeftPanel.TabIndex = 13;
            // 
            // SitesTreeView
            // 
            this.SitesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SitesTreeView.Location = new System.Drawing.Point(10, 10);
            this.SitesTreeView.Name = "SitesTreeView";
            this.SitesTreeView.ShowNodeToolTips = true;
            this.SitesTreeView.Size = new System.Drawing.Size(380, 761);
            this.SitesTreeView.TabIndex = 0;
            this.SitesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SitesTreeView_AfterSelect);
            // 
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.InnerRightPanel);
            this.RightPanel.Controls.Add(this.LeftRightPanel);
            this.RightPanel.Controls.Add(this.panel1);
            this.RightPanel.Controls.Add(this.panel3);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightPanel.Location = new System.Drawing.Point(400, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(406, 781);
            this.RightPanel.TabIndex = 14;
            // 
            // InnerRightPanel
            // 
            this.InnerRightPanel.Controls.Add(this.ActionPanel);
            this.InnerRightPanel.Controls.Add(this.ParamListPanel);
            this.InnerRightPanel.Controls.Add(this.NamePanel);
            this.InnerRightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InnerRightPanel.Location = new System.Drawing.Point(46, 29);
            this.InnerRightPanel.Name = "InnerRightPanel";
            this.InnerRightPanel.Padding = new System.Windows.Forms.Padding(10);
            this.InnerRightPanel.Size = new System.Drawing.Size(360, 723);
            this.InnerRightPanel.TabIndex = 34;
            // 
            // ActionPanel
            // 
            this.ActionPanel.Controls.Add(this.ActionGroupBox);
            this.ActionPanel.Controls.Add(this.RemoveActionButton);
            this.ActionPanel.Controls.Add(this.AddActionButton);
            this.ActionPanel.Controls.Add(this.label11);
            this.ActionPanel.Controls.Add(this.ActionsComboBox);
            this.ActionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ActionPanel.Location = new System.Drawing.Point(10, 250);
            this.ActionPanel.Name = "ActionPanel";
            this.ActionPanel.Size = new System.Drawing.Size(340, 384);
            this.ActionPanel.TabIndex = 34;
            // 
            // ActionGroupBox
            // 
            this.ActionGroupBox.Controls.Add(this.CommandPanel);
            this.ActionGroupBox.Controls.Add(this.AnalysisPanel);
            this.ActionGroupBox.Controls.Add(this.ActionSubPanel);
            this.ActionGroupBox.Location = new System.Drawing.Point(5, 37);
            this.ActionGroupBox.Name = "ActionGroupBox";
            this.ActionGroupBox.Size = new System.Drawing.Size(350, 314);
            this.ActionGroupBox.TabIndex = 25;
            this.ActionGroupBox.TabStop = false;
            this.ActionGroupBox.Text = "Action";
            // 
            // CommandPanel
            // 
            this.CommandPanel.Controls.Add(this.ActionCommandTextBox);
            this.CommandPanel.Controls.Add(this.label14);
            this.CommandPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CommandPanel.Location = new System.Drawing.Point(3, 311);
            this.CommandPanel.Name = "CommandPanel";
            this.CommandPanel.Size = new System.Drawing.Size(344, 28);
            this.CommandPanel.TabIndex = 34;
            // 
            // ActionCommandTextBox
            // 
            this.ActionCommandTextBox.Location = new System.Drawing.Point(91, 3);
            this.ActionCommandTextBox.Name = "ActionCommandTextBox";
            this.ActionCommandTextBox.Size = new System.Drawing.Size(227, 20);
            this.ActionCommandTextBox.TabIndex = 32;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(32, 6);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(54, 13);
            this.label14.TabIndex = 31;
            this.label14.Text = "Command";
            // 
            // AnalysisPanel
            // 
            this.AnalysisPanel.Controls.Add(this.AnalysisTabControl);
            this.AnalysisPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.AnalysisPanel.Location = new System.Drawing.Point(3, 71);
            this.AnalysisPanel.Name = "AnalysisPanel";
            this.AnalysisPanel.Size = new System.Drawing.Size(344, 240);
            this.AnalysisPanel.TabIndex = 35;
            // 
            // AnalysisTabControl
            // 
            this.AnalysisTabControl.Controls.Add(this.DataTabPage);
            this.AnalysisTabControl.Controls.Add(this.AnalysisTabPage);
            this.AnalysisTabControl.Controls.Add(this.ResultsTabPage);
            this.AnalysisTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AnalysisTabControl.Location = new System.Drawing.Point(0, 0);
            this.AnalysisTabControl.Name = "AnalysisTabControl";
            this.AnalysisTabControl.SelectedIndex = 0;
            this.AnalysisTabControl.Size = new System.Drawing.Size(344, 240);
            this.AnalysisTabControl.TabIndex = 0;
            // 
            // DataTabPage
            // 
            this.DataTabPage.Controls.Add(this.DataSourceTabControl);
            this.DataTabPage.Location = new System.Drawing.Point(4, 22);
            this.DataTabPage.Name = "DataTabPage";
            this.DataTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.DataTabPage.Size = new System.Drawing.Size(336, 214);
            this.DataTabPage.TabIndex = 0;
            this.DataTabPage.Text = "Data";
            this.DataTabPage.UseVisualStyleBackColor = true;
            // 
            // DataSourceTabControl
            // 
            this.DataSourceTabControl.Controls.Add(this.tabPage1);
            this.DataSourceTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataSourceTabControl.Location = new System.Drawing.Point(3, 3);
            this.DataSourceTabControl.Name = "DataSourceTabControl";
            this.DataSourceTabControl.SelectedIndex = 0;
            this.DataSourceTabControl.Size = new System.Drawing.Size(330, 208);
            this.DataSourceTabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.DataCompilerPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(322, 182);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Data Source 1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // AnalysisTabPage
            // 
            this.AnalysisTabPage.Controls.Add(this.AnalysisCommandTextBox);
            this.AnalysisTabPage.Controls.Add(this.label16);
            this.AnalysisTabPage.Location = new System.Drawing.Point(4, 22);
            this.AnalysisTabPage.Name = "AnalysisTabPage";
            this.AnalysisTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.AnalysisTabPage.Size = new System.Drawing.Size(320, 214);
            this.AnalysisTabPage.TabIndex = 1;
            this.AnalysisTabPage.Text = "Analysis";
            this.AnalysisTabPage.UseVisualStyleBackColor = true;
            // 
            // AnalysisCommandTextBox
            // 
            this.AnalysisCommandTextBox.Location = new System.Drawing.Point(68, 6);
            this.AnalysisCommandTextBox.Multiline = true;
            this.AnalysisCommandTextBox.Name = "AnalysisCommandTextBox";
            this.AnalysisCommandTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AnalysisCommandTextBox.Size = new System.Drawing.Size(240, 150);
            this.AnalysisCommandTextBox.TabIndex = 34;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(9, 9);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(54, 13);
            this.label16.TabIndex = 33;
            this.label16.Text = "Command";
            // 
            // ResultsTabPage
            // 
            this.ResultsTabPage.Controls.Add(this.ResultParserComboBox);
            this.ResultsTabPage.Controls.Add(this.label20);
            this.ResultsTabPage.Controls.Add(this.ResultFileTextBox);
            this.ResultsTabPage.Controls.Add(this.label17);
            this.ResultsTabPage.Location = new System.Drawing.Point(4, 22);
            this.ResultsTabPage.Name = "ResultsTabPage";
            this.ResultsTabPage.Size = new System.Drawing.Size(320, 214);
            this.ResultsTabPage.TabIndex = 2;
            this.ResultsTabPage.Text = "Results";
            this.ResultsTabPage.UseVisualStyleBackColor = true;
            // 
            // ResultParserComboBox
            // 
            this.ResultParserComboBox.FormattingEnabled = true;
            this.ResultParserComboBox.Items.AddRange(new object[] {
            "FRAM-Pu",
            "FRAM-U"});
            this.ResultParserComboBox.Location = new System.Drawing.Point(86, 29);
            this.ResultParserComboBox.Name = "ResultParserComboBox";
            this.ResultParserComboBox.Size = new System.Drawing.Size(227, 21);
            this.ResultParserComboBox.TabIndex = 38;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(8, 32);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(70, 13);
            this.label20.TabIndex = 37;
            this.label20.Text = "Result Parser";
            // 
            // ResultFileTextBox
            // 
            this.ResultFileTextBox.Location = new System.Drawing.Point(86, 3);
            this.ResultFileTextBox.Name = "ResultFileTextBox";
            this.ResultFileTextBox.Size = new System.Drawing.Size(227, 20);
            this.ResultFileTextBox.TabIndex = 36;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(22, 6);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(56, 13);
            this.label17.TabIndex = 35;
            this.label17.Text = "Result File";
            // 
            // ActionSubPanel
            // 
            this.ActionSubPanel.Controls.Add(this.label12);
            this.ActionSubPanel.Controls.Add(this.ActionNameTextBox);
            this.ActionSubPanel.Controls.Add(this.ActionTypeComboBox);
            this.ActionSubPanel.Controls.Add(this.label13);
            this.ActionSubPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ActionSubPanel.Location = new System.Drawing.Point(3, 16);
            this.ActionSubPanel.Name = "ActionSubPanel";
            this.ActionSubPanel.Size = new System.Drawing.Size(344, 55);
            this.ActionSubPanel.TabIndex = 33;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(50, 8);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "Name";
            // 
            // ActionNameTextBox
            // 
            this.ActionNameTextBox.Location = new System.Drawing.Point(90, 5);
            this.ActionNameTextBox.Name = "ActionNameTextBox";
            this.ActionNameTextBox.Size = new System.Drawing.Size(172, 20);
            this.ActionNameTextBox.TabIndex = 27;
            // 
            // ActionTypeComboBox
            // 
            this.ActionTypeComboBox.FormattingEnabled = true;
            this.ActionTypeComboBox.Items.AddRange(new object[] {
            "Analysis",
            "Command"});
            this.ActionTypeComboBox.Location = new System.Drawing.Point(90, 31);
            this.ActionTypeComboBox.Name = "ActionTypeComboBox";
            this.ActionTypeComboBox.Size = new System.Drawing.Size(172, 21);
            this.ActionTypeComboBox.TabIndex = 29;
            this.ActionTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.ActionTypeComboBox_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(49, 34);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(31, 13);
            this.label13.TabIndex = 30;
            this.label13.Text = "Type";
            // 
            // RemoveActionButton
            // 
            this.RemoveActionButton.Image = ((System.Drawing.Image)(resources.GetObject("RemoveActionButton.Image")));
            this.RemoveActionButton.Location = new System.Drawing.Point(310, 3);
            this.RemoveActionButton.Name = "RemoveActionButton";
            this.RemoveActionButton.Size = new System.Drawing.Size(28, 28);
            this.RemoveActionButton.TabIndex = 24;
            this.RemoveActionButton.UseVisualStyleBackColor = true;
            this.RemoveActionButton.Click += new System.EventHandler(this.RemoveActionButton_Click);
            // 
            // AddActionButton
            // 
            this.AddActionButton.Image = ((System.Drawing.Image)(resources.GetObject("AddActionButton.Image")));
            this.AddActionButton.Location = new System.Drawing.Point(279, 3);
            this.AddActionButton.Name = "AddActionButton";
            this.AddActionButton.Size = new System.Drawing.Size(28, 28);
            this.AddActionButton.TabIndex = 23;
            this.AddActionButton.UseVisualStyleBackColor = true;
            this.AddActionButton.Click += new System.EventHandler(this.AddActionButton_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(54, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "Actions";
            // 
            // ActionsComboBox
            // 
            this.ActionsComboBox.FormattingEnabled = true;
            this.ActionsComboBox.Location = new System.Drawing.Point(105, 7);
            this.ActionsComboBox.Name = "ActionsComboBox";
            this.ActionsComboBox.Size = new System.Drawing.Size(172, 21);
            this.ActionsComboBox.TabIndex = 21;
            this.ActionsComboBox.SelectedIndexChanged += new System.EventHandler(this.ActionsComboBox_SelectedIndexChanged);
            // 
            // NamePanel
            // 
            this.NamePanel.Controls.Add(this.label1);
            this.NamePanel.Controls.Add(this.NameTextBox);
            this.NamePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.NamePanel.Location = new System.Drawing.Point(10, 10);
            this.NamePanel.Name = "NamePanel";
            this.NamePanel.Size = new System.Drawing.Size(340, 40);
            this.NamePanel.TabIndex = 33;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Name";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(105, 6);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(172, 20);
            this.NameTextBox.TabIndex = 25;
            // 
            // LeftRightPanel
            // 
            this.LeftRightPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LeftRightPanel.Controls.Add(this.DeleteButton);
            this.LeftRightPanel.Controls.Add(this.AddButton);
            this.LeftRightPanel.Controls.Add(this.DownButton);
            this.LeftRightPanel.Controls.Add(this.UpButton);
            this.LeftRightPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftRightPanel.Location = new System.Drawing.Point(0, 29);
            this.LeftRightPanel.Name = "LeftRightPanel";
            this.LeftRightPanel.Padding = new System.Windows.Forms.Padding(2);
            this.LeftRightPanel.Size = new System.Drawing.Size(46, 723);
            this.LeftRightPanel.TabIndex = 13;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(2, 255);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(40, 40);
            this.DeleteButton.TabIndex = 3;
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(2, 205);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(40, 40);
            this.AddButton.TabIndex = 2;
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // DownButton
            // 
            this.DownButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DownButton.Location = new System.Drawing.Point(2, 677);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(38, 40);
            this.DownButton.TabIndex = 1;
            this.DownButton.UseVisualStyleBackColor = true;
            this.DownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // UpButton
            // 
            this.UpButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.UpButton.Location = new System.Drawing.Point(2, 2);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(38, 40);
            this.UpButton.TabIndex = 0;
            this.UpButton.UseVisualStyleBackColor = true;
            this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(406, 29);
            this.panel1.TabIndex = 23;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.SaveButton);
            this.panel3.Controls.Add(this.ExitButton);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 752);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(406, 29);
            this.panel3.TabIndex = 12;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(237, 3);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 24;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(318, 3);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(66, 23);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // DataCompilerPanel1
            // 
            this.DataCompilerPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataCompilerPanel1.Location = new System.Drawing.Point(3, 3);
            this.DataCompilerPanel1.Name = "DataCompilerPanel1";
            this.DataCompilerPanel1.Size = new System.Drawing.Size(316, 176);
            this.DataCompilerPanel1.TabIndex = 0;
            // 
            // ParamListPanel
            // 
            this.ParamListPanel.AutoScroll = true;
            this.ParamListPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ParamListPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ParamListPanel.Location = new System.Drawing.Point(10, 50);
            this.ParamListPanel.Margin = new System.Windows.Forms.Padding(10);
            this.ParamListPanel.Name = "ParamListPanel";
            this.ParamListPanel.Size = new System.Drawing.Size(340, 200);
            this.ParamListPanel.TabIndex = 35;
            // 
            // EventManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 781);
            this.ControlBox = false;
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.LeftPanel);
            this.MinimumSize = new System.Drawing.Size(762, 597);
            this.Name = "EventManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Event Manager";
            this.Load += new System.EventHandler(this.EventManagerForm_Load);
            this.LeftPanel.ResumeLayout(false);
            this.RightPanel.ResumeLayout(false);
            this.InnerRightPanel.ResumeLayout(false);
            this.ActionPanel.ResumeLayout(false);
            this.ActionPanel.PerformLayout();
            this.ActionGroupBox.ResumeLayout(false);
            this.CommandPanel.ResumeLayout(false);
            this.CommandPanel.PerformLayout();
            this.AnalysisPanel.ResumeLayout(false);
            this.AnalysisTabControl.ResumeLayout(false);
            this.DataTabPage.ResumeLayout(false);
            this.DataSourceTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.AnalysisTabPage.ResumeLayout(false);
            this.AnalysisTabPage.PerformLayout();
            this.ResultsTabPage.ResumeLayout(false);
            this.ResultsTabPage.PerformLayout();
            this.ActionSubPanel.ResumeLayout(false);
            this.ActionSubPanel.PerformLayout();
            this.NamePanel.ResumeLayout(false);
            this.NamePanel.PerformLayout();
            this.LeftRightPanel.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.TreeView SitesTreeView;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Panel LeftRightPanel;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Panel InnerRightPanel;
        private System.Windows.Forms.Panel NamePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Panel ActionPanel;
        private System.Windows.Forms.Button RemoveActionButton;
        private System.Windows.Forms.Button AddActionButton;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox ActionsComboBox;
        private System.Windows.Forms.GroupBox ActionGroupBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox ActionNameTextBox;
        private System.Windows.Forms.TextBox ActionCommandTextBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox ActionTypeComboBox;
        private System.Windows.Forms.Panel CommandPanel;
        private System.Windows.Forms.Panel ActionSubPanel;
        private System.Windows.Forms.Panel AnalysisPanel;
        private System.Windows.Forms.TabControl AnalysisTabControl;
        private System.Windows.Forms.TabPage DataTabPage;
        private System.Windows.Forms.TabPage AnalysisTabPage;
        private System.Windows.Forms.TabPage ResultsTabPage;
        private System.Windows.Forms.TextBox AnalysisCommandTextBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox ResultFileTextBox;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox ResultParserComboBox;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TabControl DataSourceTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private DataCompilerPanel DataCompilerPanel1;
        private Controls.ParameterListPanel ParamListPanel;
    }
}