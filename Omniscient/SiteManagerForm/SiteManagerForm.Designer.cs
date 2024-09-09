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
namespace Omniscient
{
    partial class SiteManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SiteManagerForm));
            this.SitesTreeView = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.InstrumentPanel = new System.Windows.Forms.Panel();
            this.ChannelSettingsPanel = new System.Windows.Forms.Panel();
            this.VirtualChannelGroupBox = new System.Windows.Forms.GroupBox();
            this.VCTopPanel = new System.Windows.Forms.Panel();
            this.VirtualChannelTypeTextBox = new System.Windows.Forms.TextBox();
            this.VCDownButton = new System.Windows.Forms.Button();
            this.VCUpButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.VirtualChannelNameTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ChannelGroupBox = new System.Windows.Forms.GroupBox();
            this.ChannelNameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.InstTypeTextBox = new System.Windows.Forms.TextBox();
            this.RemoveVirtualChannelButton = new System.Windows.Forms.Button();
            this.AddVirtualChannelButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.ChannelsComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.NewButtonPanel = new System.Windows.Forms.Panel();
            this.NewInstrumentButton = new System.Windows.Forms.Button();
            this.NewSystemButton = new System.Windows.Forms.Button();
            this.NewFacilityButton = new System.Windows.Forms.Button();
            this.NewSiteButton = new System.Windows.Forms.Button();
            this.BottomRightPanel = new System.Windows.Forms.Panel();
            this.DiscardButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.InnerRightPanel = new System.Windows.Forms.Panel();
            this.SystemPanel = new System.Windows.Forms.Panel();
            this.NamePanel = new System.Windows.Forms.Panel();
            this.LeftRightPanel = new System.Windows.Forms.Panel();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.UpButton = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ExportButton = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.VCParameterListPanel = new Omniscient.Controls.ParameterListPanel();
            this.ChannelParameterListPanel = new Omniscient.Controls.ParameterListPanel();
            this.InstrumentParameterListPanel = new Omniscient.Controls.ParameterListPanel();
            this.InstrumentPanel.SuspendLayout();
            this.ChannelSettingsPanel.SuspendLayout();
            this.VirtualChannelGroupBox.SuspendLayout();
            this.VCTopPanel.SuspendLayout();
            this.ChannelGroupBox.SuspendLayout();
            this.NewButtonPanel.SuspendLayout();
            this.BottomRightPanel.SuspendLayout();
            this.LeftPanel.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.InnerRightPanel.SuspendLayout();
            this.SystemPanel.SuspendLayout();
            this.NamePanel.SuspendLayout();
            this.LeftRightPanel.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // SitesTreeView
            // 
            this.SitesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SitesTreeView.Location = new System.Drawing.Point(10, 10);
            this.SitesTreeView.Name = "SitesTreeView";
            this.SitesTreeView.ShowNodeToolTips = true;
            this.SitesTreeView.Size = new System.Drawing.Size(380, 821);
            this.SitesTreeView.TabIndex = 0;
            this.SitesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SitesTreeView_AfterSelect);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(102, 29);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(160, 20);
            this.NameTextBox.TabIndex = 3;
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Location = new System.Drawing.Point(99, 13);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(0, 13);
            this.TypeLabel.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Instrument Type";
            // 
            // InstrumentPanel
            // 
            this.InstrumentPanel.Controls.Add(this.ChannelSettingsPanel);
            this.InstrumentPanel.Controls.Add(this.InstTypeTextBox);
            this.InstrumentPanel.Controls.Add(this.InstrumentParameterListPanel);
            this.InstrumentPanel.Controls.Add(this.RemoveVirtualChannelButton);
            this.InstrumentPanel.Controls.Add(this.AddVirtualChannelButton);
            this.InstrumentPanel.Controls.Add(this.label6);
            this.InstrumentPanel.Controls.Add(this.ChannelsComboBox);
            this.InstrumentPanel.Controls.Add(this.label3);
            this.InstrumentPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.InstrumentPanel.Location = new System.Drawing.Point(0, 126);
            this.InstrumentPanel.Name = "InstrumentPanel";
            this.InstrumentPanel.Size = new System.Drawing.Size(362, 517);
            this.InstrumentPanel.TabIndex = 9;
            // 
            // ChannelSettingsPanel
            // 
            this.ChannelSettingsPanel.Controls.Add(this.VirtualChannelGroupBox);
            this.ChannelSettingsPanel.Controls.Add(this.ChannelGroupBox);
            this.ChannelSettingsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ChannelSettingsPanel.Location = new System.Drawing.Point(0, 241);
            this.ChannelSettingsPanel.Name = "ChannelSettingsPanel";
            this.ChannelSettingsPanel.Size = new System.Drawing.Size(362, 276);
            this.ChannelSettingsPanel.TabIndex = 24;
            // 
            // VirtualChannelGroupBox
            // 
            this.VirtualChannelGroupBox.Controls.Add(this.VCParameterListPanel);
            this.VirtualChannelGroupBox.Controls.Add(this.VCTopPanel);
            this.VirtualChannelGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.VirtualChannelGroupBox.Location = new System.Drawing.Point(0, 100);
            this.VirtualChannelGroupBox.Name = "VirtualChannelGroupBox";
            this.VirtualChannelGroupBox.Size = new System.Drawing.Size(362, 256);
            this.VirtualChannelGroupBox.TabIndex = 21;
            this.VirtualChannelGroupBox.TabStop = false;
            this.VirtualChannelGroupBox.Text = "Virtual Channel";
            // 
            // VCTopPanel
            // 
            this.VCTopPanel.Controls.Add(this.VirtualChannelTypeTextBox);
            this.VCTopPanel.Controls.Add(this.VCDownButton);
            this.VCTopPanel.Controls.Add(this.VCUpButton);
            this.VCTopPanel.Controls.Add(this.label8);
            this.VCTopPanel.Controls.Add(this.VirtualChannelNameTextBox);
            this.VCTopPanel.Controls.Add(this.label7);
            this.VCTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.VCTopPanel.Location = new System.Drawing.Point(3, 16);
            this.VCTopPanel.Name = "VCTopPanel";
            this.VCTopPanel.Size = new System.Drawing.Size(356, 62);
            this.VCTopPanel.TabIndex = 6;
            // 
            // VirtualChannelTypeTextBox
            // 
            this.VirtualChannelTypeTextBox.Enabled = false;
            this.VirtualChannelTypeTextBox.Location = new System.Drawing.Point(95, 29);
            this.VirtualChannelTypeTextBox.Name = "VirtualChannelTypeTextBox";
            this.VirtualChannelTypeTextBox.Size = new System.Drawing.Size(160, 20);
            this.VirtualChannelTypeTextBox.TabIndex = 23;
            // 
            // VCDownButton
            // 
            this.VCDownButton.Image = ((System.Drawing.Image)(resources.GetObject("VCDownButton.Image")));
            this.VCDownButton.Location = new System.Drawing.Point(291, 30);
            this.VCDownButton.Name = "VCDownButton";
            this.VCDownButton.Size = new System.Drawing.Size(28, 28);
            this.VCDownButton.TabIndex = 22;
            this.VCDownButton.UseVisualStyleBackColor = true;
            this.VCDownButton.Click += new System.EventHandler(this.VCDownButton_Click);
            // 
            // VCUpButton
            // 
            this.VCUpButton.Image = ((System.Drawing.Image)(resources.GetObject("VCUpButton.Image")));
            this.VCUpButton.Location = new System.Drawing.Point(291, 0);
            this.VCUpButton.Name = "VCUpButton";
            this.VCUpButton.Size = new System.Drawing.Size(28, 28);
            this.VCUpButton.TabIndex = 21;
            this.VCUpButton.UseVisualStyleBackColor = true;
            this.VCUpButton.Click += new System.EventHandler(this.VCUpButton_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(55, 32);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Type";
            // 
            // VirtualChannelNameTextBox
            // 
            this.VirtualChannelNameTextBox.Location = new System.Drawing.Point(95, 3);
            this.VirtualChannelNameTextBox.Name = "VirtualChannelNameTextBox";
            this.VirtualChannelNameTextBox.Size = new System.Drawing.Size(160, 20);
            this.VirtualChannelNameTextBox.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(54, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Name";
            // 
            // ChannelGroupBox
            // 
            this.ChannelGroupBox.Controls.Add(this.ChannelParameterListPanel);
            this.ChannelGroupBox.Controls.Add(this.ChannelNameTextBox);
            this.ChannelGroupBox.Controls.Add(this.label4);
            this.ChannelGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChannelGroupBox.Location = new System.Drawing.Point(0, 0);
            this.ChannelGroupBox.Name = "ChannelGroupBox";
            this.ChannelGroupBox.Size = new System.Drawing.Size(362, 100);
            this.ChannelGroupBox.TabIndex = 22;
            this.ChannelGroupBox.TabStop = false;
            this.ChannelGroupBox.Text = "Channel";
            // 
            // ChannelNameTextBox
            // 
            this.ChannelNameTextBox.Location = new System.Drawing.Point(97, 19);
            this.ChannelNameTextBox.Name = "ChannelNameTextBox";
            this.ChannelNameTextBox.Size = new System.Drawing.Size(160, 20);
            this.ChannelNameTextBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Name";
            // 
            // InstTypeTextBox
            // 
            this.InstTypeTextBox.Enabled = false;
            this.InstTypeTextBox.Location = new System.Drawing.Point(102, 8);
            this.InstTypeTextBox.Name = "InstTypeTextBox";
            this.InstTypeTextBox.Size = new System.Drawing.Size(160, 20);
            this.InstTypeTextBox.TabIndex = 23;
            // 
            // RemoveVirtualChannelButton
            // 
            this.RemoveVirtualChannelButton.Image = ((System.Drawing.Image)(resources.GetObject("RemoveVirtualChannelButton.Image")));
            this.RemoveVirtualChannelButton.Location = new System.Drawing.Point(301, 207);
            this.RemoveVirtualChannelButton.Name = "RemoveVirtualChannelButton";
            this.RemoveVirtualChannelButton.Size = new System.Drawing.Size(28, 28);
            this.RemoveVirtualChannelButton.TabIndex = 20;
            this.RemoveVirtualChannelButton.UseVisualStyleBackColor = true;
            this.RemoveVirtualChannelButton.Click += new System.EventHandler(this.RemoveVirtualChannelButton_Click);
            // 
            // AddVirtualChannelButton
            // 
            this.AddVirtualChannelButton.Image = ((System.Drawing.Image)(resources.GetObject("AddVirtualChannelButton.Image")));
            this.AddVirtualChannelButton.Location = new System.Drawing.Point(267, 207);
            this.AddVirtualChannelButton.Name = "AddVirtualChannelButton";
            this.AddVirtualChannelButton.Size = new System.Drawing.Size(28, 28);
            this.AddVirtualChannelButton.TabIndex = 19;
            this.AddVirtualChannelButton.UseVisualStyleBackColor = true;
            this.AddVirtualChannelButton.Click += new System.EventHandler(this.AddVirtualChannelButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 215);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Channels";
            // 
            // ChannelsComboBox
            // 
            this.ChannelsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChannelsComboBox.FormattingEnabled = true;
            this.ChannelsComboBox.Location = new System.Drawing.Point(102, 212);
            this.ChannelsComboBox.Name = "ChannelsComboBox";
            this.ChannelsComboBox.Size = new System.Drawing.Size(159, 21);
            this.ChannelsComboBox.TabIndex = 17;
            this.ChannelsComboBox.SelectedIndexChanged += new System.EventHandler(this.VirtualChannelsComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Type";
            // 
            // NewButtonPanel
            // 
            this.NewButtonPanel.Controls.Add(this.NewInstrumentButton);
            this.NewButtonPanel.Controls.Add(this.NewSystemButton);
            this.NewButtonPanel.Controls.Add(this.NewFacilityButton);
            this.NewButtonPanel.Controls.Add(this.NewSiteButton);
            this.NewButtonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.NewButtonPanel.Location = new System.Drawing.Point(0, 0);
            this.NewButtonPanel.Name = "NewButtonPanel";
            this.NewButtonPanel.Size = new System.Drawing.Size(362, 71);
            this.NewButtonPanel.TabIndex = 10;
            // 
            // NewInstrumentButton
            // 
            this.NewInstrumentButton.Location = new System.Drawing.Point(145, 38);
            this.NewInstrumentButton.Name = "NewInstrumentButton";
            this.NewInstrumentButton.Size = new System.Drawing.Size(100, 23);
            this.NewInstrumentButton.TabIndex = 3;
            this.NewInstrumentButton.Text = "New Instrument";
            this.NewInstrumentButton.UseVisualStyleBackColor = true;
            this.NewInstrumentButton.Click += new System.EventHandler(this.NewInstrumentButton_Click);
            // 
            // NewSystemButton
            // 
            this.NewSystemButton.Location = new System.Drawing.Point(145, 10);
            this.NewSystemButton.Name = "NewSystemButton";
            this.NewSystemButton.Size = new System.Drawing.Size(100, 23);
            this.NewSystemButton.TabIndex = 2;
            this.NewSystemButton.Text = "New System";
            this.NewSystemButton.UseVisualStyleBackColor = true;
            this.NewSystemButton.Click += new System.EventHandler(this.NewSystemButton_Click);
            // 
            // NewFacilityButton
            // 
            this.NewFacilityButton.Location = new System.Drawing.Point(10, 38);
            this.NewFacilityButton.Name = "NewFacilityButton";
            this.NewFacilityButton.Size = new System.Drawing.Size(100, 23);
            this.NewFacilityButton.TabIndex = 1;
            this.NewFacilityButton.Text = "New Facility";
            this.NewFacilityButton.UseVisualStyleBackColor = true;
            this.NewFacilityButton.Click += new System.EventHandler(this.NewFacilityButton_Click);
            // 
            // NewSiteButton
            // 
            this.NewSiteButton.Location = new System.Drawing.Point(10, 10);
            this.NewSiteButton.Name = "NewSiteButton";
            this.NewSiteButton.Size = new System.Drawing.Size(100, 23);
            this.NewSiteButton.TabIndex = 0;
            this.NewSiteButton.Text = "New Site";
            this.NewSiteButton.UseVisualStyleBackColor = true;
            this.NewSiteButton.Click += new System.EventHandler(this.NewSiteButton_Click);
            // 
            // BottomRightPanel
            // 
            this.BottomRightPanel.Controls.Add(this.DiscardButton);
            this.BottomRightPanel.Controls.Add(this.SaveButton);
            this.BottomRightPanel.Controls.Add(this.ExitButton);
            this.BottomRightPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomRightPanel.Location = new System.Drawing.Point(5, 776);
            this.BottomRightPanel.Name = "BottomRightPanel";
            this.BottomRightPanel.Size = new System.Drawing.Size(408, 60);
            this.BottomRightPanel.TabIndex = 11;
            // 
            // DiscardButton
            // 
            this.DiscardButton.Location = new System.Drawing.Point(205, 6);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(98, 23);
            this.DiscardButton.TabIndex = 17;
            this.DiscardButton.Text = "Discard Changes";
            this.DiscardButton.UseVisualStyleBackColor = true;
            this.DiscardButton.Click += new System.EventHandler(this.DiscardButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(309, 6);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(66, 23);
            this.SaveButton.TabIndex = 16;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(309, 35);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(66, 23);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // LeftPanel
            // 
            this.LeftPanel.Controls.Add(this.SitesTreeView);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Padding = new System.Windows.Forms.Padding(10);
            this.LeftPanel.Size = new System.Drawing.Size(400, 841);
            this.LeftPanel.TabIndex = 12;
            // 
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.InnerRightPanel);
            this.RightPanel.Controls.Add(this.LeftRightPanel);
            this.RightPanel.Controls.Add(this.BottomRightPanel);
            this.RightPanel.Controls.Add(this.panel4);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightPanel.Location = new System.Drawing.Point(400, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Padding = new System.Windows.Forms.Padding(5);
            this.RightPanel.Size = new System.Drawing.Size(418, 841);
            this.RightPanel.TabIndex = 13;
            // 
            // InnerRightPanel
            // 
            this.InnerRightPanel.Controls.Add(this.SystemPanel);
            this.InnerRightPanel.Controls.Add(this.InstrumentPanel);
            this.InnerRightPanel.Controls.Add(this.NamePanel);
            this.InnerRightPanel.Controls.Add(this.NewButtonPanel);
            this.InnerRightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InnerRightPanel.Location = new System.Drawing.Point(51, 35);
            this.InnerRightPanel.Name = "InnerRightPanel";
            this.InnerRightPanel.Size = new System.Drawing.Size(362, 741);
            this.InnerRightPanel.TabIndex = 13;
            // 
            // SystemPanel
            // 
            this.SystemPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.SystemPanel.Location = new System.Drawing.Point(0, 643);
            this.SystemPanel.Name = "SystemPanel";
            this.SystemPanel.Size = new System.Drawing.Size(362, 100);
            this.SystemPanel.TabIndex = 12;
            // 
            // NamePanel
            // 
            this.NamePanel.Controls.Add(this.label2);
            this.NamePanel.Controls.Add(this.TypeLabel);
            this.NamePanel.Controls.Add(this.label1);
            this.NamePanel.Controls.Add(this.NameTextBox);
            this.NamePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.NamePanel.Location = new System.Drawing.Point(0, 71);
            this.NamePanel.Name = "NamePanel";
            this.NamePanel.Size = new System.Drawing.Size(362, 55);
            this.NamePanel.TabIndex = 11;
            // 
            // LeftRightPanel
            // 
            this.LeftRightPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LeftRightPanel.Controls.Add(this.RemoveButton);
            this.LeftRightPanel.Controls.Add(this.DownButton);
            this.LeftRightPanel.Controls.Add(this.UpButton);
            this.LeftRightPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftRightPanel.Location = new System.Drawing.Point(5, 35);
            this.LeftRightPanel.Name = "LeftRightPanel";
            this.LeftRightPanel.Padding = new System.Windows.Forms.Padding(2);
            this.LeftRightPanel.Size = new System.Drawing.Size(46, 741);
            this.LeftRightPanel.TabIndex = 14;
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(2, 205);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(40, 40);
            this.RemoveButton.TabIndex = 3;
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // DownButton
            // 
            this.DownButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DownButton.Location = new System.Drawing.Point(2, 695);
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
            // panel4
            // 
            this.panel4.Controls.Add(this.ExportButton);
            this.panel4.Controls.Add(this.ImportButton);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(5, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(408, 30);
            this.panel4.TabIndex = 12;
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(72, 3);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(66, 23);
            this.ExportButton.TabIndex = 1;
            this.ExportButton.Text = "Export";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.Location = new System.Drawing.Point(3, 3);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(66, 23);
            this.ImportButton.TabIndex = 0;
            this.ImportButton.Text = "Import";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // VCParameterListPanel
            // 
            this.VCParameterListPanel.AutoScroll = true;
            this.VCParameterListPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.VCParameterListPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.VCParameterListPanel.Location = new System.Drawing.Point(3, 78);
            this.VCParameterListPanel.Name = "VCParameterListPanel";
            this.VCParameterListPanel.Padding = new System.Windows.Forms.Padding(5);
            this.VCParameterListPanel.Size = new System.Drawing.Size(356, 185);
            this.VCParameterListPanel.TabIndex = 11;
            // 
            // ChannelParameterListPanel
            // 
            this.ChannelParameterListPanel.AutoScroll = true;
            this.ChannelParameterListPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ChannelParameterListPanel.Location = new System.Drawing.Point(6, 45);
            this.ChannelParameterListPanel.Name = "ChannelParameterListPanel";
            this.ChannelParameterListPanel.Padding = new System.Windows.Forms.Padding(5);
            this.ChannelParameterListPanel.Size = new System.Drawing.Size(350, 49);
            this.ChannelParameterListPanel.TabIndex = 23;
            // 
            // InstrumentParameterListPanel
            // 
            this.InstrumentParameterListPanel.AutoScroll = true;
            this.InstrumentParameterListPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.InstrumentParameterListPanel.Location = new System.Drawing.Point(6, 34);
            this.InstrumentParameterListPanel.Name = "InstrumentParameterListPanel";
            this.InstrumentParameterListPanel.Padding = new System.Windows.Forms.Padding(5);
            this.InstrumentParameterListPanel.Size = new System.Drawing.Size(350, 167);
            this.InstrumentParameterListPanel.TabIndex = 22;
            // 
            // SiteManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 841);
            this.ControlBox = false;
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.LeftPanel);
            this.MinimumSize = new System.Drawing.Size(512, 509);
            this.Name = "SiteManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Site Manager";
            this.Load += new System.EventHandler(this.SiteManagerForm_Load);
            this.InstrumentPanel.ResumeLayout(false);
            this.InstrumentPanel.PerformLayout();
            this.ChannelSettingsPanel.ResumeLayout(false);
            this.VirtualChannelGroupBox.ResumeLayout(false);
            this.VCTopPanel.ResumeLayout(false);
            this.VCTopPanel.PerformLayout();
            this.ChannelGroupBox.ResumeLayout(false);
            this.ChannelGroupBox.PerformLayout();
            this.NewButtonPanel.ResumeLayout(false);
            this.BottomRightPanel.ResumeLayout(false);
            this.LeftPanel.ResumeLayout(false);
            this.RightPanel.ResumeLayout(false);
            this.InnerRightPanel.ResumeLayout(false);
            this.SystemPanel.ResumeLayout(false);
            this.SystemPanel.PerformLayout();
            this.NamePanel.ResumeLayout(false);
            this.NamePanel.PerformLayout();
            this.LeftRightPanel.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView SitesTreeView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel InstrumentPanel;
        private System.Windows.Forms.Panel NewButtonPanel;
        private System.Windows.Forms.Button NewInstrumentButton;
        private System.Windows.Forms.Button NewSystemButton;
        private System.Windows.Forms.Button NewFacilityButton;
        private System.Windows.Forms.Button NewSiteButton;
        private System.Windows.Forms.Panel BottomRightPanel;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DiscardButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Panel InnerRightPanel;
        private System.Windows.Forms.Panel LeftRightPanel;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.Panel NamePanel;
        private System.Windows.Forms.Button RemoveVirtualChannelButton;
        private System.Windows.Forms.Button AddVirtualChannelButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox ChannelsComboBox;
        private System.Windows.Forms.GroupBox VirtualChannelGroupBox;
        private System.Windows.Forms.Panel VCTopPanel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox VirtualChannelNameTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button VCDownButton;
        private System.Windows.Forms.Button VCUpButton;
        private System.Windows.Forms.Panel SystemPanel;
        private Controls.ParameterListPanel VCParameterListPanel;
        private System.Windows.Forms.TextBox VirtualChannelTypeTextBox;
        private Controls.ParameterListPanel InstrumentParameterListPanel;
        private System.Windows.Forms.TextBox InstTypeTextBox;
        private System.Windows.Forms.Panel ChannelSettingsPanel;
        private System.Windows.Forms.GroupBox ChannelGroupBox;
        private System.Windows.Forms.TextBox ChannelNameTextBox;
        private System.Windows.Forms.Label label4;
        private Controls.ParameterListPanel ChannelParameterListPanel;
    }
}