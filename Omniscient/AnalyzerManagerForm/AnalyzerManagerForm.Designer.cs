namespace Omniscient
{
    partial class AnalyzerManagerForm
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
            this.NamePanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.LeftRightPanel = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ParamListPanel = new Omniscient.Controls.ParameterListPanel();
            this.LeftPanel.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.InnerRightPanel.SuspendLayout();
            this.NamePanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // LeftPanel
            // 
            this.LeftPanel.Controls.Add(this.SitesTreeView);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Margin = new System.Windows.Forms.Padding(4);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.LeftPanel.Size = new System.Drawing.Size(533, 961);
            this.LeftPanel.TabIndex = 13;
            // 
            // SitesTreeView
            // 
            this.SitesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SitesTreeView.Location = new System.Drawing.Point(13, 12);
            this.SitesTreeView.Margin = new System.Windows.Forms.Padding(4);
            this.SitesTreeView.Name = "SitesTreeView";
            this.SitesTreeView.ShowNodeToolTips = true;
            this.SitesTreeView.Size = new System.Drawing.Size(507, 937);
            this.SitesTreeView.TabIndex = 0;
            this.SitesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SitesTreeView_AfterSelect);
            // 
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.InnerRightPanel);
            this.RightPanel.Controls.Add(this.LeftRightPanel);
            this.RightPanel.Controls.Add(this.panel3);
            this.RightPanel.Controls.Add(this.panel1);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightPanel.Location = new System.Drawing.Point(533, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(542, 961);
            this.RightPanel.TabIndex = 14;
            // 
            // InnerRightPanel
            // 
            this.InnerRightPanel.Controls.Add(this.ParamListPanel);
            this.InnerRightPanel.Controls.Add(this.NamePanel);
            this.InnerRightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InnerRightPanel.Location = new System.Drawing.Point(60, 36);
            this.InnerRightPanel.Name = "InnerRightPanel";
            this.InnerRightPanel.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.InnerRightPanel.Size = new System.Drawing.Size(482, 889);
            this.InnerRightPanel.TabIndex = 27;
            // 
            // NamePanel
            // 
            this.NamePanel.Controls.Add(this.label1);
            this.NamePanel.Controls.Add(this.NameTextBox);
            this.NamePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.NamePanel.Location = new System.Drawing.Point(13, 12);
            this.NamePanel.Margin = new System.Windows.Forms.Padding(4);
            this.NamePanel.Name = "NamePanel";
            this.NamePanel.Size = new System.Drawing.Size(456, 49);
            this.NamePanel.TabIndex = 34;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 26;
            this.label1.Text = "Name";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(140, 7);
            this.NameTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(228, 22);
            this.NameTextBox.TabIndex = 25;
            // 
            // LeftRightPanel
            // 
            this.LeftRightPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LeftRightPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftRightPanel.Location = new System.Drawing.Point(0, 36);
            this.LeftRightPanel.Margin = new System.Windows.Forms.Padding(4);
            this.LeftRightPanel.Name = "LeftRightPanel";
            this.LeftRightPanel.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LeftRightPanel.Size = new System.Drawing.Size(60, 889);
            this.LeftRightPanel.TabIndex = 26;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.SaveButton);
            this.panel3.Controls.Add(this.ExitButton);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 925);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(542, 36);
            this.panel3.TabIndex = 25;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(316, 4);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(4);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(100, 28);
            this.SaveButton.TabIndex = 24;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(424, 4);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(4);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(88, 28);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(542, 36);
            this.panel1.TabIndex = 24;
            // 
            // ParamListPanel
            // 
            this.ParamListPanel.AutoScroll = true;
            this.ParamListPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ParamListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParamListPanel.Location = new System.Drawing.Point(13, 61);
            this.ParamListPanel.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.ParamListPanel.Name = "ParamListPanel";
            this.ParamListPanel.Padding = new System.Windows.Forms.Padding(9, 7, 9, 7);
            this.ParamListPanel.Size = new System.Drawing.Size(456, 816);
            this.ParamListPanel.TabIndex = 36;
            // 
            // AnalyzerManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 961);
            this.ControlBox = false;
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.LeftPanel);
            this.Name = "AnalyzerManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AnalyzerManagerForm";
            this.Load += new System.EventHandler(this.AnalyzerManagerForm_Load);
            this.LeftPanel.ResumeLayout(false);
            this.RightPanel.ResumeLayout(false);
            this.InnerRightPanel.ResumeLayout(false);
            this.NamePanel.ResumeLayout(false);
            this.NamePanel.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.TreeView SitesTreeView;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Panel LeftRightPanel;
        private System.Windows.Forms.Panel InnerRightPanel;
        private System.Windows.Forms.Panel NamePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameTextBox;
        private Controls.ParameterListPanel ParamListPanel;
    }
}