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
﻿namespace Omniscient
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
            this.FileFormatComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SelectNoneButton = new System.Windows.Forms.Button();
            this.MeasurementTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.DetectorIDTextBox = new System.Windows.Forms.TextBox();
            this.ItemIDTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.ChannelTreeView.Location = new System.Drawing.Point(75, 67);
            this.ChannelTreeView.Name = "ChannelTreeView";
            this.ChannelTreeView.Size = new System.Drawing.Size(307, 142);
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
            this.label2.Location = new System.Drawing.Point(15, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Channels:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 462);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "File:";
            // 
            // FileTextBox
            // 
            this.FileTextBox.Enabled = false;
            this.FileTextBox.Location = new System.Drawing.Point(75, 459);
            this.FileTextBox.Name = "FileTextBox";
            this.FileTextBox.Size = new System.Drawing.Size(281, 20);
            this.FileTextBox.TabIndex = 5;
            // 
            // FileButton
            // 
            this.FileButton.Location = new System.Drawing.Point(362, 458);
            this.FileButton.Name = "FileButton";
            this.FileButton.Size = new System.Drawing.Size(20, 20);
            this.FileButton.TabIndex = 6;
            this.FileButton.UseVisualStyleBackColor = true;
            this.FileButton.Click += new System.EventHandler(this.FileButton_Click);
            // 
            // EndTimePicker
            // 
            this.EndTimePicker.CustomFormat = "MMM dd, yyyy\'";
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
            this.CancelButton.Location = new System.Drawing.Point(288, 485);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(94, 28);
            this.CancelButton.TabIndex = 20;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(178, 484);
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
            this.WriteHeadersCheckBox.Location = new System.Drawing.Point(6, 19);
            this.WriteHeadersCheckBox.Name = "WriteHeadersCheckBox";
            this.WriteHeadersCheckBox.Size = new System.Drawing.Size(100, 17);
            this.WriteHeadersCheckBox.TabIndex = 22;
            this.WriteHeadersCheckBox.Text = "Write Headers: ";
            this.WriteHeadersCheckBox.UseVisualStyleBackColor = true;
            // 
            // FileFormatComboBox
            // 
            this.FileFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FileFormatComboBox.FormattingEnabled = true;
            this.FileFormatComboBox.Items.AddRange(new object[] {
            "CSV",
            "NCC"});
            this.FileFormatComboBox.Location = new System.Drawing.Point(75, 268);
            this.FileFormatComboBox.Name = "FileFormatComboBox";
            this.FileFormatComboBox.Size = new System.Drawing.Size(88, 21);
            this.FileFormatComboBox.TabIndex = 23;
            this.FileFormatComboBox.SelectedIndexChanged += new System.EventHandler(this.FileFormatComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 271);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "File Format:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.WriteHeadersCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(72, 295);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 49);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CSV";
            // 
            // SelectNoneButton
            // 
            this.SelectNoneButton.Location = new System.Drawing.Point(75, 38);
            this.SelectNoneButton.Name = "SelectNoneButton";
            this.SelectNoneButton.Size = new System.Drawing.Size(85, 23);
            this.SelectNoneButton.TabIndex = 27;
            this.SelectNoneButton.Text = "Select None";
            this.SelectNoneButton.UseVisualStyleBackColor = true;
            this.SelectNoneButton.Click += new System.EventHandler(this.SelectNoneButton_Click);
            // 
            // MeasurementTypeComboBox
            // 
            this.MeasurementTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MeasurementTypeComboBox.FormattingEnabled = true;
            this.MeasurementTypeComboBox.Items.AddRange(new object[] {
            "Background",
            "Normalization",
            "Verification"});
            this.MeasurementTypeComboBox.Location = new System.Drawing.Point(113, 13);
            this.MeasurementTypeComboBox.Name = "MeasurementTypeComboBox";
            this.MeasurementTypeComboBox.Size = new System.Drawing.Size(130, 21);
            this.MeasurementTypeComboBox.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Measurement Type:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(42, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 28;
            this.label7.Text = "Detector ID:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(63, 73);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "Item ID:";
            // 
            // DetectorIDTextBox
            // 
            this.DetectorIDTextBox.Location = new System.Drawing.Point(113, 41);
            this.DetectorIDTextBox.Name = "DetectorIDTextBox";
            this.DetectorIDTextBox.Size = new System.Drawing.Size(130, 20);
            this.DetectorIDTextBox.TabIndex = 32;
            // 
            // ItemIDTextBox
            // 
            this.ItemIDTextBox.Location = new System.Drawing.Point(113, 69);
            this.ItemIDTextBox.Name = "ItemIDTextBox";
            this.ItemIDTextBox.Size = new System.Drawing.Size(129, 20);
            this.ItemIDTextBox.TabIndex = 34;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ItemIDTextBox);
            this.groupBox2.Controls.Add(this.DetectorIDTextBox);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.MeasurementTypeComboBox);
            this.groupBox2.Location = new System.Drawing.Point(75, 353);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(307, 99);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "NCC";
            // 
            // ExportDataDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 518);
            this.Controls.Add(this.SelectNoneButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.FileFormatComboBox);
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
            this.Load += new System.EventHandler(this.ExportDataDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.ComboBox FileFormatComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button SelectNoneButton;
        private System.Windows.Forms.ComboBox MeasurementTypeComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox DetectorIDTextBox;
        private System.Windows.Forms.TextBox ItemIDTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}