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
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.SitesTreeView = new System.Windows.Forms.TreeView();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.InnerRightPanel = new System.Windows.Forms.Panel();
            this.ThresholdPanel = new System.Windows.Forms.Panel();
            this.DebounceComboBox = new System.Windows.Forms.ComboBox();
            this.DebounceTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ChannelComboBox = new System.Windows.Forms.ComboBox();
            this.ThresholdTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CoincidencePanel = new System.Windows.Forms.Panel();
            this.MinDifferenceComboBox = new System.Windows.Forms.ComboBox();
            this.MinDifferenceTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TimingTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.EventGeneratorBComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.EventGeneratorAComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.WindowComboBox = new System.Windows.Forms.ComboBox();
            this.WindowTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CoincidenceTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
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
            this.ExitButton = new System.Windows.Forms.Button();
            this.LeftPanel.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.InnerRightPanel.SuspendLayout();
            this.ThresholdPanel.SuspendLayout();
            this.CoincidencePanel.SuspendLayout();
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
            this.LeftPanel.Size = new System.Drawing.Size(400, 581);
            this.LeftPanel.TabIndex = 13;
            // 
            // SitesTreeView
            // 
            this.SitesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SitesTreeView.Location = new System.Drawing.Point(10, 10);
            this.SitesTreeView.Name = "SitesTreeView";
            this.SitesTreeView.ShowNodeToolTips = true;
            this.SitesTreeView.Size = new System.Drawing.Size(380, 561);
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
            this.RightPanel.Size = new System.Drawing.Size(346, 581);
            this.RightPanel.TabIndex = 14;
            // 
            // InnerRightPanel
            // 
            this.InnerRightPanel.Controls.Add(this.ThresholdPanel);
            this.InnerRightPanel.Controls.Add(this.SaveButton);
            this.InnerRightPanel.Controls.Add(this.CoincidencePanel);
            this.InnerRightPanel.Controls.Add(this.NamePanel);
            this.InnerRightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InnerRightPanel.Location = new System.Drawing.Point(46, 29);
            this.InnerRightPanel.Name = "InnerRightPanel";
            this.InnerRightPanel.Size = new System.Drawing.Size(300, 523);
            this.InnerRightPanel.TabIndex = 34;
            // 
            // ThresholdPanel
            // 
            this.ThresholdPanel.Controls.Add(this.DebounceComboBox);
            this.ThresholdPanel.Controls.Add(this.DebounceTextBox);
            this.ThresholdPanel.Controls.Add(this.label4);
            this.ThresholdPanel.Controls.Add(this.label3);
            this.ThresholdPanel.Controls.Add(this.ChannelComboBox);
            this.ThresholdPanel.Controls.Add(this.ThresholdTextBox);
            this.ThresholdPanel.Controls.Add(this.label2);
            this.ThresholdPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ThresholdPanel.Location = new System.Drawing.Point(0, 229);
            this.ThresholdPanel.Name = "ThresholdPanel";
            this.ThresholdPanel.Size = new System.Drawing.Size(300, 200);
            this.ThresholdPanel.TabIndex = 25;
            // 
            // DebounceComboBox
            // 
            this.DebounceComboBox.FormattingEnabled = true;
            this.DebounceComboBox.Items.AddRange(new object[] {
            "Seconds",
            "Minutes",
            "Hours",
            "Days"});
            this.DebounceComboBox.Location = new System.Drawing.Point(178, 68);
            this.DebounceComboBox.Name = "DebounceComboBox";
            this.DebounceComboBox.Size = new System.Drawing.Size(114, 21);
            this.DebounceComboBox.TabIndex = 31;
            // 
            // DebounceTextBox
            // 
            this.DebounceTextBox.Location = new System.Drawing.Point(120, 68);
            this.DebounceTextBox.Name = "DebounceTextBox";
            this.DebounceTextBox.Size = new System.Drawing.Size(52, 20);
            this.DebounceTextBox.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Debounce Time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(61, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Threshold";
            // 
            // ChannelComboBox
            // 
            this.ChannelComboBox.FormattingEnabled = true;
            this.ChannelComboBox.Location = new System.Drawing.Point(120, 10);
            this.ChannelComboBox.Name = "ChannelComboBox";
            this.ChannelComboBox.Size = new System.Drawing.Size(172, 21);
            this.ChannelComboBox.TabIndex = 27;
            // 
            // ThresholdTextBox
            // 
            this.ThresholdTextBox.Location = new System.Drawing.Point(120, 39);
            this.ThresholdTextBox.Name = "ThresholdTextBox";
            this.ThresholdTextBox.Size = new System.Drawing.Size(172, 20);
            this.ThresholdTextBox.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(69, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Channel";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(204, 232);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 29);
            this.SaveButton.TabIndex = 24;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CoincidencePanel
            // 
            this.CoincidencePanel.Controls.Add(this.MinDifferenceComboBox);
            this.CoincidencePanel.Controls.Add(this.MinDifferenceTextBox);
            this.CoincidencePanel.Controls.Add(this.label6);
            this.CoincidencePanel.Controls.Add(this.TimingTypeComboBox);
            this.CoincidencePanel.Controls.Add(this.label10);
            this.CoincidencePanel.Controls.Add(this.EventGeneratorBComboBox);
            this.CoincidencePanel.Controls.Add(this.label9);
            this.CoincidencePanel.Controls.Add(this.EventGeneratorAComboBox);
            this.CoincidencePanel.Controls.Add(this.label8);
            this.CoincidencePanel.Controls.Add(this.WindowComboBox);
            this.CoincidencePanel.Controls.Add(this.WindowTextBox);
            this.CoincidencePanel.Controls.Add(this.label5);
            this.CoincidencePanel.Controls.Add(this.CoincidenceTypeComboBox);
            this.CoincidencePanel.Controls.Add(this.label7);
            this.CoincidencePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CoincidencePanel.Location = new System.Drawing.Point(0, 29);
            this.CoincidencePanel.Name = "CoincidencePanel";
            this.CoincidencePanel.Size = new System.Drawing.Size(300, 200);
            this.CoincidencePanel.TabIndex = 32;
            // 
            // MinDifferenceComboBox
            // 
            this.MinDifferenceComboBox.FormattingEnabled = true;
            this.MinDifferenceComboBox.Items.AddRange(new object[] {
            "Seconds",
            "Minutes",
            "Hours",
            "Days"});
            this.MinDifferenceComboBox.Location = new System.Drawing.Point(178, 135);
            this.MinDifferenceComboBox.Name = "MinDifferenceComboBox";
            this.MinDifferenceComboBox.Size = new System.Drawing.Size(114, 21);
            this.MinDifferenceComboBox.TabIndex = 40;
            // 
            // MinDifferenceTextBox
            // 
            this.MinDifferenceTextBox.Location = new System.Drawing.Point(120, 135);
            this.MinDifferenceTextBox.Name = "MinDifferenceTextBox";
            this.MinDifferenceTextBox.Size = new System.Drawing.Size(52, 20);
            this.MinDifferenceTextBox.TabIndex = 39;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 38;
            this.label6.Text = "Min Difference";
            // 
            // TimingTypeComboBox
            // 
            this.TimingTypeComboBox.FormattingEnabled = true;
            this.TimingTypeComboBox.Items.AddRange(new object[] {
            "Start to Start",
            "Start to End",
            "End to Start",
            "End to End",
            "Max to Max"});
            this.TimingTypeComboBox.Location = new System.Drawing.Point(120, 81);
            this.TimingTypeComboBox.Name = "TimingTypeComboBox";
            this.TimingTypeComboBox.Size = new System.Drawing.Size(172, 21);
            this.TimingTypeComboBox.TabIndex = 37;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(50, 85);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 13);
            this.label10.TabIndex = 36;
            this.label10.Text = "Timing Type";
            // 
            // EventGeneratorBComboBox
            // 
            this.EventGeneratorBComboBox.FormattingEnabled = true;
            this.EventGeneratorBComboBox.Location = new System.Drawing.Point(120, 30);
            this.EventGeneratorBComboBox.Name = "EventGeneratorBComboBox";
            this.EventGeneratorBComboBox.Size = new System.Drawing.Size(172, 21);
            this.EventGeneratorBComboBox.TabIndex = 35;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "Event Generator B";
            // 
            // EventGeneratorAComboBox
            // 
            this.EventGeneratorAComboBox.FormattingEnabled = true;
            this.EventGeneratorAComboBox.Location = new System.Drawing.Point(120, 3);
            this.EventGeneratorAComboBox.Name = "EventGeneratorAComboBox";
            this.EventGeneratorAComboBox.Size = new System.Drawing.Size(172, 21);
            this.EventGeneratorAComboBox.TabIndex = 33;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 13);
            this.label8.TabIndex = 32;
            this.label8.Text = "Event Generator A";
            // 
            // WindowComboBox
            // 
            this.WindowComboBox.FormattingEnabled = true;
            this.WindowComboBox.Items.AddRange(new object[] {
            "Seconds",
            "Minutes",
            "Hours",
            "Days"});
            this.WindowComboBox.Location = new System.Drawing.Point(178, 108);
            this.WindowComboBox.Name = "WindowComboBox";
            this.WindowComboBox.Size = new System.Drawing.Size(114, 21);
            this.WindowComboBox.TabIndex = 31;
            // 
            // WindowTextBox
            // 
            this.WindowTextBox.Location = new System.Drawing.Point(120, 108);
            this.WindowTextBox.Name = "WindowTextBox";
            this.WindowTextBox.Size = new System.Drawing.Size(52, 20);
            this.WindowTextBox.TabIndex = 30;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(69, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Window";
            // 
            // CoincidenceTypeComboBox
            // 
            this.CoincidenceTypeComboBox.FormattingEnabled = true;
            this.CoincidenceTypeComboBox.Items.AddRange(new object[] {
            "A then B",
            "B then A",
            "Either order"});
            this.CoincidenceTypeComboBox.Location = new System.Drawing.Point(120, 57);
            this.CoincidenceTypeComboBox.Name = "CoincidenceTypeComboBox";
            this.CoincidenceTypeComboBox.Size = new System.Drawing.Size(172, 21);
            this.CoincidenceTypeComboBox.TabIndex = 27;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Coincidence Type";
            // 
            // NamePanel
            // 
            this.NamePanel.Controls.Add(this.label1);
            this.NamePanel.Controls.Add(this.NameTextBox);
            this.NamePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.NamePanel.Location = new System.Drawing.Point(0, 0);
            this.NamePanel.Name = "NamePanel";
            this.NamePanel.Size = new System.Drawing.Size(300, 29);
            this.NamePanel.TabIndex = 33;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Name";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(120, 4);
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
            this.LeftRightPanel.Size = new System.Drawing.Size(46, 523);
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
            this.DownButton.Location = new System.Drawing.Point(2, 477);
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
            this.panel1.Size = new System.Drawing.Size(346, 29);
            this.panel1.TabIndex = 23;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ExitButton);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 552);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(346, 29);
            this.panel3.TabIndex = 12;
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(244, 0);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(66, 23);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // EventManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 581);
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
            this.ThresholdPanel.ResumeLayout(false);
            this.ThresholdPanel.PerformLayout();
            this.CoincidencePanel.ResumeLayout(false);
            this.CoincidencePanel.PerformLayout();
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
        private System.Windows.Forms.Panel ThresholdPanel;
        private System.Windows.Forms.ComboBox DebounceComboBox;
        private System.Windows.Forms.TextBox DebounceTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ChannelComboBox;
        private System.Windows.Forms.TextBox ThresholdTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel CoincidencePanel;
        private System.Windows.Forms.ComboBox WindowComboBox;
        private System.Windows.Forms.TextBox WindowTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CoincidenceTypeComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel NamePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.ComboBox MinDifferenceComboBox;
        private System.Windows.Forms.TextBox MinDifferenceTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox TimingTypeComboBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox EventGeneratorBComboBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox EventGeneratorAComboBox;
        private System.Windows.Forms.Label label8;
    }
}