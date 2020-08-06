namespace Omniscient
{
    partial class XYPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.XYChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.CollapseRightButton = new System.Windows.Forms.Button();
            this.RightRightPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.RSquaredTextBox = new System.Windows.Forms.TextBox();
            this.FitTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DataSelectPanel = new System.Windows.Forms.Panel();
            this.YChannelComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.InstrumentComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.XChannelComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ChartTitlePanel = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.ChartTitleTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.XYChart)).BeginInit();
            this.RightPanel.SuspendLayout();
            this.RightRightPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.DataSelectPanel.SuspendLayout();
            this.ChartTitlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // XYChart
            // 
            chartArea1.Name = "ChartArea1";
            this.XYChart.ChartAreas.Add(chartArea1);
            this.XYChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.XYChart.Legends.Add(legend1);
            this.XYChart.Location = new System.Drawing.Point(0, 0);
            this.XYChart.Name = "XYChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.XYChart.Series.Add(series1);
            this.XYChart.Size = new System.Drawing.Size(630, 767);
            this.XYChart.TabIndex = 0;
            this.XYChart.Text = "chart1";
            // 
            // RightPanel
            // 
            this.RightPanel.AutoSize = true;
            this.RightPanel.Controls.Add(this.CollapseRightButton);
            this.RightPanel.Controls.Add(this.RightRightPanel);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightPanel.Location = new System.Drawing.Point(630, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(238, 767);
            this.RightPanel.TabIndex = 1;
            // 
            // CollapseRightButton
            // 
            this.CollapseRightButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.CollapseRightButton.Location = new System.Drawing.Point(0, 0);
            this.CollapseRightButton.Name = "CollapseRightButton";
            this.CollapseRightButton.Size = new System.Drawing.Size(18, 767);
            this.CollapseRightButton.TabIndex = 1;
            this.CollapseRightButton.Text = ">";
            this.CollapseRightButton.UseVisualStyleBackColor = true;
            this.CollapseRightButton.Click += new System.EventHandler(this.CollapseRightButton_Click);
            // 
            // RightRightPanel
            // 
            this.RightRightPanel.Controls.Add(this.panel1);
            this.RightRightPanel.Controls.Add(this.DataSelectPanel);
            this.RightRightPanel.Controls.Add(this.ChartTitlePanel);
            this.RightRightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightRightPanel.Location = new System.Drawing.Point(18, 0);
            this.RightRightPanel.Name = "RightRightPanel";
            this.RightRightPanel.Size = new System.Drawing.Size(220, 767);
            this.RightRightPanel.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.RSquaredTextBox);
            this.panel1.Controls.Add(this.FitTypeComboBox);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 128);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(220, 189);
            this.panel1.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "R^2:";
            // 
            // RSquaredTextBox
            // 
            this.RSquaredTextBox.Enabled = false;
            this.RSquaredTextBox.Location = new System.Drawing.Point(66, 33);
            this.RSquaredTextBox.Name = "RSquaredTextBox";
            this.RSquaredTextBox.Size = new System.Drawing.Size(149, 20);
            this.RSquaredTextBox.TabIndex = 6;
            // 
            // FitTypeComboBox
            // 
            this.FitTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FitTypeComboBox.FormattingEnabled = true;
            this.FitTypeComboBox.Location = new System.Drawing.Point(66, 6);
            this.FitTypeComboBox.Name = "FitTypeComboBox";
            this.FitTypeComboBox.Size = new System.Drawing.Size(149, 21);
            this.FitTypeComboBox.TabIndex = 4;
            this.FitTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.FitTypeComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Fit Type:";
            // 
            // DataSelectPanel
            // 
            this.DataSelectPanel.Controls.Add(this.YChannelComboBox);
            this.DataSelectPanel.Controls.Add(this.label3);
            this.DataSelectPanel.Controls.Add(this.InstrumentComboBox);
            this.DataSelectPanel.Controls.Add(this.label2);
            this.DataSelectPanel.Controls.Add(this.XChannelComboBox);
            this.DataSelectPanel.Controls.Add(this.label1);
            this.DataSelectPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.DataSelectPanel.Location = new System.Drawing.Point(0, 30);
            this.DataSelectPanel.Name = "DataSelectPanel";
            this.DataSelectPanel.Size = new System.Drawing.Size(220, 98);
            this.DataSelectPanel.TabIndex = 6;
            // 
            // YChannelComboBox
            // 
            this.YChannelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.YChannelComboBox.FormattingEnabled = true;
            this.YChannelComboBox.Location = new System.Drawing.Point(66, 65);
            this.YChannelComboBox.Name = "YChannelComboBox";
            this.YChannelComboBox.Size = new System.Drawing.Size(149, 21);
            this.YChannelComboBox.TabIndex = 2;
            this.YChannelComboBox.SelectedIndexChanged += new System.EventHandler(this.YChannelComboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Y Data:";
            // 
            // InstrumentComboBox
            // 
            this.InstrumentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InstrumentComboBox.FormattingEnabled = true;
            this.InstrumentComboBox.Location = new System.Drawing.Point(66, 11);
            this.InstrumentComboBox.Name = "InstrumentComboBox";
            this.InstrumentComboBox.Size = new System.Drawing.Size(149, 21);
            this.InstrumentComboBox.TabIndex = 0;
            this.InstrumentComboBox.SelectedIndexChanged += new System.EventHandler(this.InstrumentComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "X Data:";
            // 
            // XChannelComboBox
            // 
            this.XChannelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XChannelComboBox.FormattingEnabled = true;
            this.XChannelComboBox.Location = new System.Drawing.Point(66, 38);
            this.XChannelComboBox.Name = "XChannelComboBox";
            this.XChannelComboBox.Size = new System.Drawing.Size(149, 21);
            this.XChannelComboBox.TabIndex = 1;
            this.XChannelComboBox.SelectedIndexChanged += new System.EventHandler(this.XChannelComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Instrument:";
            // 
            // ChartTitlePanel
            // 
            this.ChartTitlePanel.Controls.Add(this.label6);
            this.ChartTitlePanel.Controls.Add(this.ChartTitleTextBox);
            this.ChartTitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChartTitlePanel.Location = new System.Drawing.Point(0, 0);
            this.ChartTitlePanel.Name = "ChartTitlePanel";
            this.ChartTitlePanel.Size = new System.Drawing.Size(220, 30);
            this.ChartTitlePanel.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Chart Title:";
            // 
            // ChartTitleTextBox
            // 
            this.ChartTitleTextBox.Location = new System.Drawing.Point(66, 6);
            this.ChartTitleTextBox.Name = "ChartTitleTextBox";
            this.ChartTitleTextBox.Size = new System.Drawing.Size(149, 20);
            this.ChartTitleTextBox.TabIndex = 8;
            this.ChartTitleTextBox.TextChanged += new System.EventHandler(this.ChartTitleTextBox_TextChanged);
            // 
            // XYPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.XYChart);
            this.Controls.Add(this.RightPanel);
            this.Name = "XYPanel";
            this.Size = new System.Drawing.Size(868, 767);
            ((System.ComponentModel.ISupportInitialize)(this.XYChart)).EndInit();
            this.RightPanel.ResumeLayout(false);
            this.RightRightPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.DataSelectPanel.ResumeLayout(false);
            this.DataSelectPanel.PerformLayout();
            this.ChartTitlePanel.ResumeLayout(false);
            this.ChartTitlePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart XYChart;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Button CollapseRightButton;
        private System.Windows.Forms.Panel RightRightPanel;
        private System.Windows.Forms.ComboBox YChannelComboBox;
        private System.Windows.Forms.ComboBox XChannelComboBox;
        private System.Windows.Forms.ComboBox InstrumentComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox FitTypeComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel DataSelectPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox RSquaredTextBox;
        private System.Windows.Forms.Panel ChartTitlePanel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ChartTitleTextBox;
    }
}
