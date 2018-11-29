namespace Omniscient
{
    partial class ExportDataDialog
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
            this.InstrumentComboBox = new System.Windows.Forms.ComboBox();
            this.ChannelTreeView = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FileTextBox = new System.Windows.Forms.TextBox();
            this.FileButton = new System.Windows.Forms.Button();
            this.EndTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartDatePicker = new System.Windows.Forms.DateTimePicker();
            this.EndDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.ChartRangeButton = new System.Windows.Forms.Button();
            this.AllButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.WriteHeadersCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // InstrumentComboBox
            // 
            this.InstrumentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InstrumentComboBox.FormattingEnabled = true;
            this.InstrumentComboBox.Location = new System.Drawing.Point(75, 11);
            this.InstrumentComboBox.Name = "InstrumentComboBox";
            this.InstrumentComboBox.Size = new System.Drawing.Size(307, 21);
            this.InstrumentComboBox.TabIndex = 0;
            this.InstrumentComboBox.SelectedIndexChanged += new System.EventHandler(this.InstrumentComboBox_SelectedIndexChanged);
            // 
            // ChannelTreeView
            // 
            this.ChannelTreeView.CheckBoxes = true;
            this.ChannelTreeView.Location = new System.Drawing.Point(75, 38);
            this.ChannelTreeView.Name = "ChannelTreeView";
            this.ChannelTreeView.Size = new System.Drawing.Size(307, 171);
            this.ChannelTreeView.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Instrument:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Channels:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 270);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "File:";
            // 
            // FileTextBox
            // 
            this.FileTextBox.Enabled = false;
            this.FileTextBox.Location = new System.Drawing.Point(75, 267);
            this.FileTextBox.Name = "FileTextBox";
            this.FileTextBox.Size = new System.Drawing.Size(281, 20);
            this.FileTextBox.TabIndex = 5;
            // 
            // FileButton
            // 
            this.FileButton.Location = new System.Drawing.Point(362, 266);
            this.FileButton.Name = "FileButton";
            this.FileButton.Size = new System.Drawing.Size(20, 20);
            this.FileButton.TabIndex = 6;
            this.FileButton.UseVisualStyleBackColor = true;
            this.FileButton.Click += new System.EventHandler(this.FileButton_Click);
            // 
            // EndTimePicker
            // 
            this.EndTimePicker.CustomFormat = "MMM dd, yyyy\'";
            this.EndTimePicker.Enabled = false;
            this.EndTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.EndTimePicker.Location = new System.Drawing.Point(75, 241);
            this.EndTimePicker.Name = "EndTimePicker";
            this.EndTimePicker.ShowUpDown = true;
            this.EndTimePicker.Size = new System.Drawing.Size(88, 20);
            this.EndTimePicker.TabIndex = 17;
            // 
            // StartTimePicker
            // 
            this.StartTimePicker.CustomFormat = "MMM dd, yyyy\'";
            this.StartTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.StartTimePicker.Location = new System.Drawing.Point(75, 215);
            this.StartTimePicker.Name = "StartTimePicker";
            this.StartTimePicker.ShowUpDown = true;
            this.StartTimePicker.Size = new System.Drawing.Size(88, 20);
            this.StartTimePicker.TabIndex = 16;
            // 
            // StartDatePicker
            // 
            this.StartDatePicker.CustomFormat = "MMM dd, yyyy\'";
            this.StartDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartDatePicker.Location = new System.Drawing.Point(178, 215);
            this.StartDatePicker.Name = "StartDatePicker";
            this.StartDatePicker.Size = new System.Drawing.Size(103, 20);
            this.StartDatePicker.TabIndex = 15;
            this.StartDatePicker.Value = new System.DateTime(2016, 11, 1, 11, 15, 0, 0);
            // 
            // EndDatePicker
            // 
            this.EndDatePicker.CustomFormat = "MMM dd, yyyy\'";
            this.EndDatePicker.Enabled = false;
            this.EndDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndDatePicker.Location = new System.Drawing.Point(178, 241);
            this.EndDatePicker.Name = "EndDatePicker";
            this.EndDatePicker.Size = new System.Drawing.Size(103, 20);
            this.EndDatePicker.TabIndex = 14;
            this.EndDatePicker.Value = new System.DateTime(2016, 12, 1, 11, 15, 0, 0);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(36, 241);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "End:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(35, 218);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "Start:";
            // 
            // ChartRangeButton
            // 
            this.ChartRangeButton.Location = new System.Drawing.Point(322, 215);
            this.ChartRangeButton.Name = "ChartRangeButton";
            this.ChartRangeButton.Size = new System.Drawing.Size(60, 46);
            this.ChartRangeButton.TabIndex = 18;
            this.ChartRangeButton.Text = "Chart Range";
            this.ChartRangeButton.UseVisualStyleBackColor = true;
            this.ChartRangeButton.Click += new System.EventHandler(this.ChartRangeButton_Click);
            // 
            // AllButton
            // 
            this.AllButton.Location = new System.Drawing.Point(288, 215);
            this.AllButton.Name = "AllButton";
            this.AllButton.Size = new System.Drawing.Size(30, 46);
            this.AllButton.TabIndex = 19;
            this.AllButton.Text = "All";
            this.AllButton.UseVisualStyleBackColor = true;
            this.AllButton.Click += new System.EventHandler(this.AllButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(288, 317);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(94, 28);
            this.CancelButton.TabIndex = 20;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(178, 317);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(94, 28);
            this.ExportButton.TabIndex = 21;
            this.ExportButton.Text = "Export";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // WriteHeadersCheckBox
            // 
            this.WriteHeadersCheckBox.AutoSize = true;
            this.WriteHeadersCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.WriteHeadersCheckBox.Checked = true;
            this.WriteHeadersCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.WriteHeadersCheckBox.Location = new System.Drawing.Point(282, 293);
            this.WriteHeadersCheckBox.Name = "WriteHeadersCheckBox";
            this.WriteHeadersCheckBox.Size = new System.Drawing.Size(100, 17);
            this.WriteHeadersCheckBox.TabIndex = 22;
            this.WriteHeadersCheckBox.Text = "Write Headers: ";
            this.WriteHeadersCheckBox.UseVisualStyleBackColor = true;
            // 
            // ExportDataDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 357);
            this.Controls.Add(this.WriteHeadersCheckBox);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.AllButton);
            this.Controls.Add(this.ChartRangeButton);
            this.Controls.Add(this.EndTimePicker);
            this.Controls.Add(this.StartTimePicker);
            this.Controls.Add(this.StartDatePicker);
            this.Controls.Add(this.EndDatePicker);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.FileButton);
            this.Controls.Add(this.FileTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ChannelTreeView);
            this.Controls.Add(this.InstrumentComboBox);
            this.Name = "ExportDataDialog";
            this.Text = "ExportDataDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox InstrumentComboBox;
        private System.Windows.Forms.TreeView ChannelTreeView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox FileTextBox;
        private System.Windows.Forms.Button FileButton;
        private System.Windows.Forms.DateTimePicker EndTimePicker;
        private System.Windows.Forms.DateTimePicker StartTimePicker;
        private System.Windows.Forms.DateTimePicker StartDatePicker;
        private System.Windows.Forms.DateTimePicker EndDatePicker;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button ChartRangeButton;
        private System.Windows.Forms.Button AllButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.CheckBox WriteHeadersCheckBox;
    }
}