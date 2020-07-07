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
    partial class Inspectrum
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Inspectrum));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.OpenToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ZoomToRangeButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.LiveTimeTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.DeadTimeStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.NextSpecButton = new System.Windows.Forms.Button();
            this.PreviousSpecButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.SpectrumNumberUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CalResetButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CalSlopeTextBox = new System.Windows.Forms.TextBox();
            this.CalZeroTextBox = new System.Windows.Forms.TextBox();
            this.TimeTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DateTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.SpecPanel = new System.Windows.Forms.Panel();
            this.SpecChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.HScroll = new System.Windows.Forms.HScrollBar();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.LeftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpectrumNumberUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SpecPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpecChart)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1023, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripMenuItem,
            this.ExportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripButton,
            this.toolStripSeparator1,
            this.ZoomToRangeButton,
            this.toolStripLabel1,
            this.LiveTimeTextBox,
            this.toolStripLabel2,
            this.DeadTimeStripTextBox});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1023, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // OpenToolStripButton
            // 
            this.OpenToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("OpenToolStripButton.Image")));
            this.OpenToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenToolStripButton.Name = "OpenToolStripButton";
            this.OpenToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.OpenToolStripButton.Click += new System.EventHandler(this.OpenToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ZoomToRangeButton
            // 
            this.ZoomToRangeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomToRangeButton.Image = global::Omniscient.Properties.Resources.ZoomToWidth_16x;
            this.ZoomToRangeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomToRangeButton.Margin = new System.Windows.Forms.Padding(0, 1, 50, 2);
            this.ZoomToRangeButton.Name = "ZoomToRangeButton";
            this.ZoomToRangeButton.Size = new System.Drawing.Size(23, 22);
            this.ZoomToRangeButton.Text = "Zoom to Fit";
            this.ZoomToRangeButton.Click += new System.EventHandler(this.ZoomToRangeButton_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(57, 22);
            this.toolStripLabel1.Text = "Live Time";
            // 
            // LiveTimeTextBox
            // 
            this.LiveTimeTextBox.Enabled = false;
            this.LiveTimeTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LiveTimeTextBox.Name = "LiveTimeTextBox";
            this.LiveTimeTextBox.Size = new System.Drawing.Size(100, 25);
            this.LiveTimeTextBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(63, 22);
            this.toolStripLabel2.Text = "Dead Time";
            // 
            // DeadTimeStripTextBox
            // 
            this.DeadTimeStripTextBox.Enabled = false;
            this.DeadTimeStripTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.DeadTimeStripTextBox.Name = "DeadTimeStripTextBox";
            this.DeadTimeStripTextBox.Size = new System.Drawing.Size(100, 25);
            this.DeadTimeStripTextBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LeftPanel
            // 
            this.LeftPanel.Controls.Add(this.NextSpecButton);
            this.LeftPanel.Controls.Add(this.PreviousSpecButton);
            this.LeftPanel.Controls.Add(this.label6);
            this.LeftPanel.Controls.Add(this.SpectrumNumberUpDown);
            this.LeftPanel.Controls.Add(this.groupBox1);
            this.LeftPanel.Controls.Add(this.TimeTextBox);
            this.LeftPanel.Controls.Add(this.label3);
            this.LeftPanel.Controls.Add(this.DateTextBox);
            this.LeftPanel.Controls.Add(this.label2);
            this.LeftPanel.Controls.Add(this.label1);
            this.LeftPanel.Controls.Add(this.FileNameTextBox);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(0, 49);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Size = new System.Drawing.Size(220, 529);
            this.LeftPanel.TabIndex = 3;
            // 
            // NextSpecButton
            // 
            this.NextSpecButton.Enabled = false;
            this.NextSpecButton.Location = new System.Drawing.Point(117, 348);
            this.NextSpecButton.Name = "NextSpecButton";
            this.NextSpecButton.Size = new System.Drawing.Size(80, 23);
            this.NextSpecButton.TabIndex = 10;
            this.NextSpecButton.Text = "Next";
            this.NextSpecButton.UseVisualStyleBackColor = true;
            this.NextSpecButton.Click += new System.EventHandler(this.NextSpecButton_Click);
            // 
            // PreviousSpecButton
            // 
            this.PreviousSpecButton.Enabled = false;
            this.PreviousSpecButton.Location = new System.Drawing.Point(15, 348);
            this.PreviousSpecButton.Name = "PreviousSpecButton";
            this.PreviousSpecButton.Size = new System.Drawing.Size(80, 23);
            this.PreviousSpecButton.TabIndex = 9;
            this.PreviousSpecButton.Text = "Previous";
            this.PreviousSpecButton.UseVisualStyleBackColor = true;
            this.PreviousSpecButton.Click += new System.EventHandler(this.PreviousSpecButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(52, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Spectrum #";
            // 
            // SpectrumNumberUpDown
            // 
            this.SpectrumNumberUpDown.Location = new System.Drawing.Point(120, 141);
            this.SpectrumNumberUpDown.Name = "SpectrumNumberUpDown";
            this.SpectrumNumberUpDown.Size = new System.Drawing.Size(80, 20);
            this.SpectrumNumberUpDown.TabIndex = 7;
            this.SpectrumNumberUpDown.ValueChanged += new System.EventHandler(this.SpectrumNumberUpDown_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CalResetButton);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.CalSlopeTextBox);
            this.groupBox1.Controls.Add(this.CalZeroTextBox);
            this.groupBox1.Location = new System.Drawing.Point(3, 235);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 107);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Calibration";
            // 
            // CalResetButton
            // 
            this.CalResetButton.Location = new System.Drawing.Point(13, 64);
            this.CalResetButton.Name = "CalResetButton";
            this.CalResetButton.Size = new System.Drawing.Size(181, 26);
            this.CalResetButton.TabIndex = 8;
            this.CalResetButton.Text = "Reset to File Calibration";
            this.CalResetButton.UseVisualStyleBackColor = true;
            this.CalResetButton.Click += new System.EventHandler(this.CalResetButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(111, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "keV/Channel";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Zero";
            // 
            // CalSlopeTextBox
            // 
            this.CalSlopeTextBox.Location = new System.Drawing.Point(114, 38);
            this.CalSlopeTextBox.Name = "CalSlopeTextBox";
            this.CalSlopeTextBox.Size = new System.Drawing.Size(80, 20);
            this.CalSlopeTextBox.TabIndex = 5;
            this.CalSlopeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CalSlopeTextBox.TextChanged += new System.EventHandler(this.CalSlopeTextBox_TextChanged);
            this.CalSlopeTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CalSlopeTextBox_KeyDown);
            // 
            // CalZeroTextBox
            // 
            this.CalZeroTextBox.Location = new System.Drawing.Point(12, 38);
            this.CalZeroTextBox.Name = "CalZeroTextBox";
            this.CalZeroTextBox.Size = new System.Drawing.Size(80, 20);
            this.CalZeroTextBox.TabIndex = 4;
            this.CalZeroTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CalZeroTextBox.TextChanged += new System.EventHandler(this.CalZeroTextBox_TextChanged);
            this.CalZeroTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CalZeroTextBox_KeyDown);
            // 
            // TimeTextBox
            // 
            this.TimeTextBox.Enabled = false;
            this.TimeTextBox.Location = new System.Drawing.Point(120, 98);
            this.TimeTextBox.Name = "TimeTextBox";
            this.TimeTextBox.Size = new System.Drawing.Size(80, 20);
            this.TimeTextBox.TabIndex = 5;
            this.TimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(117, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Time";
            // 
            // DateTextBox
            // 
            this.DateTextBox.Enabled = false;
            this.DateTextBox.Location = new System.Drawing.Point(12, 98);
            this.DateTextBox.Name = "DateTextBox";
            this.DateTextBox.Size = new System.Drawing.Size(80, 20);
            this.DateTextBox.TabIndex = 3;
            this.DateTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "File Name";
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Enabled = false;
            this.FileNameTextBox.Location = new System.Drawing.Point(12, 30);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(200, 20);
            this.FileNameTextBox.TabIndex = 0;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(220, 461);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(803, 117);
            this.BottomPanel.TabIndex = 4;
            // 
            // SpecPanel
            // 
            this.SpecPanel.Controls.Add(this.SpecChart);
            this.SpecPanel.Controls.Add(this.HScroll);
            this.SpecPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpecPanel.Location = new System.Drawing.Point(220, 49);
            this.SpecPanel.Name = "SpecPanel";
            this.SpecPanel.Size = new System.Drawing.Size(803, 412);
            this.SpecPanel.TabIndex = 5;
            // 
            // SpecChart
            // 
            chartArea1.Name = "ChartArea1";
            this.SpecChart.ChartAreas.Add(chartArea1);
            this.SpecChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpecChart.Location = new System.Drawing.Point(0, 0);
            this.SpecChart.Name = "SpecChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series1.Name = "Series1";
            this.SpecChart.Series.Add(series1);
            this.SpecChart.Size = new System.Drawing.Size(803, 395);
            this.SpecChart.TabIndex = 3;
            this.SpecChart.Text = "chart1";
            // 
            // HScroll
            // 
            this.HScroll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.HScroll.Location = new System.Drawing.Point(0, 395);
            this.HScroll.Name = "HScroll";
            this.HScroll.Size = new System.Drawing.Size(803, 17);
            this.HScroll.TabIndex = 1;
            this.HScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HScroll_Scroll);
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.OpenToolStripMenuItem.Text = "Open";
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // ExportToolStripMenuItem
            // 
            this.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            this.ExportToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ExportToolStripMenuItem.Text = "Export";
            this.ExportToolStripMenuItem.Click += new System.EventHandler(this.ExportToolStripMenuItem_Click);
            // 
            // Inspectrum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1023, 578);
            this.Controls.Add(this.SpecPanel);
            this.Controls.Add(this.BottomPanel);
            this.Controls.Add(this.LeftPanel);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Inspectrum";
            this.Text = "Inspectrum";
            this.Load += new System.EventHandler(this.Inspectrum_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Inspectrum_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Inspectrum_KeyUp);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.LeftPanel.ResumeLayout(false);
            this.LeftPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpectrumNumberUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.SpecPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SpecChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton OpenToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox LiveTimeTextBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox DeadTimeStripTextBox;
        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox CalSlopeTextBox;
        private System.Windows.Forms.TextBox CalZeroTextBox;
        private System.Windows.Forms.TextBox TimeTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox DateTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Panel SpecPanel;
        private System.Windows.Forms.HScrollBar HScroll;
        private System.Windows.Forms.Button CalResetButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart SpecChart;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown SpectrumNumberUpDown;
        private System.Windows.Forms.ToolStripButton ZoomToRangeButton;
        private System.Windows.Forms.Button NextSpecButton;
        private System.Windows.Forms.Button PreviousSpecButton;
        private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
    }
}