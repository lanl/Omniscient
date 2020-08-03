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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.XYChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.CollapseRightButton = new System.Windows.Forms.Button();
            this.RightRightPanel = new System.Windows.Forms.Panel();
            this.InstrumentComboBox = new System.Windows.Forms.ComboBox();
            this.XChannelComboBox = new System.Windows.Forms.ComboBox();
            this.YChannelComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.XYChart)).BeginInit();
            this.RightPanel.SuspendLayout();
            this.RightRightPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // XYChart
            // 
            chartArea3.Name = "ChartArea1";
            this.XYChart.ChartAreas.Add(chartArea3);
            this.XYChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.XYChart.Legends.Add(legend3);
            this.XYChart.Location = new System.Drawing.Point(0, 0);
            this.XYChart.Name = "XYChart";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.XYChart.Series.Add(series3);
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
            this.RightRightPanel.Controls.Add(this.label3);
            this.RightRightPanel.Controls.Add(this.label2);
            this.RightRightPanel.Controls.Add(this.label1);
            this.RightRightPanel.Controls.Add(this.YChannelComboBox);
            this.RightRightPanel.Controls.Add(this.XChannelComboBox);
            this.RightRightPanel.Controls.Add(this.InstrumentComboBox);
            this.RightRightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightRightPanel.Location = new System.Drawing.Point(18, 0);
            this.RightRightPanel.Name = "RightRightPanel";
            this.RightRightPanel.Size = new System.Drawing.Size(220, 767);
            this.RightRightPanel.TabIndex = 2;
            // 
            // InstrumentComboBox
            // 
            this.InstrumentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InstrumentComboBox.FormattingEnabled = true;
            this.InstrumentComboBox.Location = new System.Drawing.Point(68, 3);
            this.InstrumentComboBox.Name = "InstrumentComboBox";
            this.InstrumentComboBox.Size = new System.Drawing.Size(149, 21);
            this.InstrumentComboBox.TabIndex = 0;
            this.InstrumentComboBox.SelectedIndexChanged += new System.EventHandler(this.InstrumentComboBox_SelectedIndexChanged);
            // 
            // XChannelComboBox
            // 
            this.XChannelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XChannelComboBox.FormattingEnabled = true;
            this.XChannelComboBox.Location = new System.Drawing.Point(68, 30);
            this.XChannelComboBox.Name = "XChannelComboBox";
            this.XChannelComboBox.Size = new System.Drawing.Size(149, 21);
            this.XChannelComboBox.TabIndex = 1;
            this.XChannelComboBox.SelectedIndexChanged += new System.EventHandler(this.XChannelComboBox_SelectedIndexChanged);
            // 
            // YChannelComboBox
            // 
            this.YChannelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.YChannelComboBox.FormattingEnabled = true;
            this.YChannelComboBox.Location = new System.Drawing.Point(68, 57);
            this.YChannelComboBox.Name = "YChannelComboBox";
            this.YChannelComboBox.Size = new System.Drawing.Size(149, 21);
            this.YChannelComboBox.TabIndex = 2;
            this.YChannelComboBox.SelectedIndexChanged += new System.EventHandler(this.YChannelComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Instrument";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "X Data:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Y Data:";
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
            this.RightRightPanel.PerformLayout();
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
    }
}
