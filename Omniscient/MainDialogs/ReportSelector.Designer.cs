namespace Omniscient.MainDialogs
{
    partial class ReportSelector
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
            this.ReportGrid = new System.Windows.Forms.DataGridView();
            this.AnalyzerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.AnalyzerComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.ReportGrid)).BeginInit();
            this.RightPanel.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.TopPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ReportGrid
            // 
            this.ReportGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ReportGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AnalyzerColumn,
            this.StartTimeColumn,
            this.FileNameColumn});
            this.ReportGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReportGrid.Location = new System.Drawing.Point(3, 3);
            this.ReportGrid.Name = "ReportGrid";
            this.ReportGrid.RowHeadersWidth = 62;
            this.ReportGrid.RowTemplate.Height = 28;
            this.ReportGrid.Size = new System.Drawing.Size(846, 389);
            this.ReportGrid.TabIndex = 0;
            this.ReportGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ReportGrid_CellClick);
            // 
            // AnalyzerColumn
            // 
            this.AnalyzerColumn.HeaderText = "Analyzer";
            this.AnalyzerColumn.MinimumWidth = 8;
            this.AnalyzerColumn.Name = "AnalyzerColumn";
            this.AnalyzerColumn.ReadOnly = true;
            this.AnalyzerColumn.Width = 150;
            // 
            // StartTimeColumn
            // 
            this.StartTimeColumn.HeaderText = "Start Time";
            this.StartTimeColumn.MinimumWidth = 8;
            this.StartTimeColumn.Name = "StartTimeColumn";
            this.StartTimeColumn.ReadOnly = true;
            this.StartTimeColumn.Width = 150;
            // 
            // FileNameColumn
            // 
            this.FileNameColumn.HeaderText = "File Name";
            this.FileNameColumn.MinimumWidth = 50;
            this.FileNameColumn.Name = "FileNameColumn";
            this.FileNameColumn.ReadOnly = true;
            this.FileNameColumn.Width = 300;
            // 
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.CancelButton);
            this.RightPanel.Controls.Add(this.OkButton);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightPanel.Location = new System.Drawing.Point(852, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Padding = new System.Windows.Forms.Padding(5);
            this.RightPanel.Size = new System.Drawing.Size(150, 450);
            this.RightPanel.TabIndex = 1;
            // 
            // CancelButton
            // 
            this.CancelButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.CancelButton.Location = new System.Drawing.Point(5, 55);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(10);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(140, 50);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OkButton
            // 
            this.OkButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.OkButton.Enabled = false;
            this.OkButton.Location = new System.Drawing.Point(5, 5);
            this.OkButton.Margin = new System.Windows.Forms.Padding(10);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(140, 50);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.ReportGrid);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 55);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Padding = new System.Windows.Forms.Padding(3);
            this.MainPanel.Size = new System.Drawing.Size(852, 395);
            this.MainPanel.TabIndex = 2;
            // 
            // TopPanel
            // 
            this.TopPanel.Controls.Add(this.label1);
            this.TopPanel.Controls.Add(this.AnalyzerComboBox);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(852, 55);
            this.TopPanel.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Analyzer";
            // 
            // AnalyzerComboBox
            // 
            this.AnalyzerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AnalyzerComboBox.FormattingEnabled = true;
            this.AnalyzerComboBox.Location = new System.Drawing.Point(88, 17);
            this.AnalyzerComboBox.Name = "AnalyzerComboBox";
            this.AnalyzerComboBox.Size = new System.Drawing.Size(258, 28);
            this.AnalyzerComboBox.TabIndex = 0;
            this.AnalyzerComboBox.SelectedIndexChanged += new System.EventHandler(this.AnalyzerComboBox_SelectedIndexChanged);
            // 
            // ReportSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 450);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.TopPanel);
            this.Controls.Add(this.RightPanel);
            this.Name = "ReportSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ReportSelector";
            ((System.ComponentModel.ISupportInitialize)(this.ReportGrid)).EndInit();
            this.RightPanel.ResumeLayout(false);
            this.MainPanel.ResumeLayout(false);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ReportGrid;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn AnalyzerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileNameColumn;
        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox AnalyzerComboBox;
    }
}