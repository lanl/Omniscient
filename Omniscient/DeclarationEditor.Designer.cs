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
    partial class DeclarationEditor
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreationTimePicker = new System.Windows.Forms.DateTimePicker();
            this.CreationDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ModificationTimePicker = new System.Windows.Forms.DateTimePicker();
            this.ModificationDatePicker = new System.Windows.Forms.DateTimePicker();
            this.ItemGroupBox = new System.Windows.Forms.GroupBox();
            this.MassDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.MassTimePicker = new System.Windows.Forms.DateTimePicker();
            this.MassTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.BatchGroupBox = new System.Windows.Forms.GroupBox();
            this.BatchSourceTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.BatchNameTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.BarCodeTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.FullNameTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ItemNameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.OriginDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.OriginTimePicker = new System.Windows.Forms.DateTimePicker();
            this.ToTimePicker = new System.Windows.Forms.DateTimePicker();
            this.FromDatePicker = new System.Windows.Forms.DateTimePicker();
            this.ToDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.FromTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.FacilityTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.MBATextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.MaterialPanel = new Omniscient.NuclearCompositionPanel();
            this.menuStrip1.SuspendLayout();
            this.ItemGroupBox.SuspendLayout();
            this.BatchGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(346, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // CreationTimePicker
            // 
            this.CreationTimePicker.CustomFormat = "MMM dd, yyyy\'";
            this.CreationTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.CreationTimePicker.Location = new System.Drawing.Point(114, 760);
            this.CreationTimePicker.Name = "CreationTimePicker";
            this.CreationTimePicker.ShowUpDown = true;
            this.CreationTimePicker.Size = new System.Drawing.Size(88, 20);
            this.CreationTimePicker.TabIndex = 57;
            // 
            // CreationDatePicker
            // 
            this.CreationDatePicker.CustomFormat = "MMM dd, yyyy\'";
            this.CreationDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CreationDatePicker.Location = new System.Drawing.Point(208, 760);
            this.CreationDatePicker.Name = "CreationDatePicker";
            this.CreationDatePicker.Size = new System.Drawing.Size(103, 20);
            this.CreationDatePicker.TabIndex = 58;
            this.CreationDatePicker.Value = new System.DateTime(2016, 11, 1, 11, 15, 0, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 764);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Creation Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 791);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Modification Date";
            // 
            // ModificationTimePicker
            // 
            this.ModificationTimePicker.CustomFormat = "MMM dd, yyyy\'";
            this.ModificationTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.ModificationTimePicker.Location = new System.Drawing.Point(114, 787);
            this.ModificationTimePicker.Name = "ModificationTimePicker";
            this.ModificationTimePicker.ShowUpDown = true;
            this.ModificationTimePicker.Size = new System.Drawing.Size(88, 20);
            this.ModificationTimePicker.TabIndex = 59;
            // 
            // ModificationDatePicker
            // 
            this.ModificationDatePicker.CustomFormat = "MMM dd, yyyy\'";
            this.ModificationDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ModificationDatePicker.Location = new System.Drawing.Point(208, 787);
            this.ModificationDatePicker.Name = "ModificationDatePicker";
            this.ModificationDatePicker.Size = new System.Drawing.Size(103, 20);
            this.ModificationDatePicker.TabIndex = 60;
            this.ModificationDatePicker.Value = new System.DateTime(2016, 11, 1, 11, 15, 0, 0);
            // 
            // ItemGroupBox
            // 
            this.ItemGroupBox.Controls.Add(this.MassDatePicker);
            this.ItemGroupBox.Controls.Add(this.label14);
            this.ItemGroupBox.Controls.Add(this.MassTimePicker);
            this.ItemGroupBox.Controls.Add(this.MassTextBox);
            this.ItemGroupBox.Controls.Add(this.label13);
            this.ItemGroupBox.Controls.Add(this.BatchGroupBox);
            this.ItemGroupBox.Controls.Add(this.BarCodeTextBox);
            this.ItemGroupBox.Controls.Add(this.label6);
            this.ItemGroupBox.Controls.Add(this.FullNameTextBox);
            this.ItemGroupBox.Controls.Add(this.label5);
            this.ItemGroupBox.Controls.Add(this.ItemNameTextBox);
            this.ItemGroupBox.Controls.Add(this.label4);
            this.ItemGroupBox.Controls.Add(this.OriginDatePicker);
            this.ItemGroupBox.Controls.Add(this.label3);
            this.ItemGroupBox.Controls.Add(this.OriginTimePicker);
            this.ItemGroupBox.Location = new System.Drawing.Point(12, 135);
            this.ItemGroupBox.Name = "ItemGroupBox";
            this.ItemGroupBox.Size = new System.Drawing.Size(325, 619);
            this.ItemGroupBox.TabIndex = 7;
            this.ItemGroupBox.TabStop = false;
            this.ItemGroupBox.Text = "Item";
            // 
            // MassDatePicker
            // 
            this.MassDatePicker.CustomFormat = "MMM dd, yyyy\'";
            this.MassDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.MassDatePicker.Location = new System.Drawing.Point(196, 145);
            this.MassDatePicker.Name = "MassDatePicker";
            this.MassDatePicker.Size = new System.Drawing.Size(103, 20);
            this.MassDatePicker.TabIndex = 30;
            this.MassDatePicker.Value = new System.DateTime(2016, 11, 1, 11, 15, 0, 0);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(38, 148);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 13);
            this.label14.TabIndex = 31;
            this.label14.Text = "Mass Date";
            // 
            // MassTimePicker
            // 
            this.MassTimePicker.CustomFormat = "MMM dd, yyyy\'";
            this.MassTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.MassTimePicker.Location = new System.Drawing.Point(102, 145);
            this.MassTimePicker.Name = "MassTimePicker";
            this.MassTimePicker.ShowUpDown = true;
            this.MassTimePicker.Size = new System.Drawing.Size(88, 20);
            this.MassTimePicker.TabIndex = 29;
            // 
            // MassTextBox
            // 
            this.MassTextBox.Location = new System.Drawing.Point(102, 119);
            this.MassTextBox.Name = "MassTextBox";
            this.MassTextBox.Size = new System.Drawing.Size(98, 20);
            this.MassTextBox.TabIndex = 27;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(49, 122);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(47, 13);
            this.label13.TabIndex = 28;
            this.label13.Text = "Mass (g)";
            // 
            // BatchGroupBox
            // 
            this.BatchGroupBox.Controls.Add(this.BatchSourceTextBox);
            this.BatchGroupBox.Controls.Add(this.label12);
            this.BatchGroupBox.Controls.Add(this.BatchNameTextBox);
            this.BatchGroupBox.Controls.Add(this.label11);
            this.BatchGroupBox.Controls.Add(this.MaterialPanel);
            this.BatchGroupBox.Location = new System.Drawing.Point(6, 177);
            this.BatchGroupBox.Name = "BatchGroupBox";
            this.BatchGroupBox.Size = new System.Drawing.Size(313, 436);
            this.BatchGroupBox.TabIndex = 13;
            this.BatchGroupBox.TabStop = false;
            this.BatchGroupBox.Text = "Batch";
            // 
            // BatchSourceTextBox
            // 
            this.BatchSourceTextBox.Location = new System.Drawing.Point(96, 41);
            this.BatchSourceTextBox.Name = "BatchSourceTextBox";
            this.BatchSourceTextBox.Size = new System.Drawing.Size(197, 20);
            this.BatchSourceTextBox.TabIndex = 15;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(50, 46);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 13);
            this.label12.TabIndex = 21;
            this.label12.Text = "Source";
            // 
            // BatchNameTextBox
            // 
            this.BatchNameTextBox.Location = new System.Drawing.Point(96, 15);
            this.BatchNameTextBox.Name = "BatchNameTextBox";
            this.BatchNameTextBox.Size = new System.Drawing.Size(197, 20);
            this.BatchNameTextBox.TabIndex = 14;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(56, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "Name";
            // 
            // BarCodeTextBox
            // 
            this.BarCodeTextBox.Location = new System.Drawing.Point(102, 93);
            this.BarCodeTextBox.Name = "BarCodeTextBox";
            this.BarCodeTextBox.Size = new System.Drawing.Size(197, 20);
            this.BarCodeTextBox.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Barcode";
            // 
            // FullNameTextBox
            // 
            this.FullNameTextBox.Location = new System.Drawing.Point(102, 69);
            this.FullNameTextBox.Name = "FullNameTextBox";
            this.FullNameTextBox.Size = new System.Drawing.Size(197, 20);
            this.FullNameTextBox.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(43, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Full Name";
            // 
            // ItemNameTextBox
            // 
            this.ItemNameTextBox.Location = new System.Drawing.Point(102, 18);
            this.ItemNameTextBox.Name = "ItemNameTextBox";
            this.ItemNameTextBox.Size = new System.Drawing.Size(197, 20);
            this.ItemNameTextBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Name";
            // 
            // OriginDatePicker
            // 
            this.OriginDatePicker.CustomFormat = "MMM dd, yyyy\'";
            this.OriginDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.OriginDatePicker.Location = new System.Drawing.Point(196, 43);
            this.OriginDatePicker.Name = "OriginDatePicker";
            this.OriginDatePicker.Size = new System.Drawing.Size(103, 20);
            this.OriginDatePicker.TabIndex = 10;
            this.OriginDatePicker.Value = new System.DateTime(2016, 11, 1, 11, 15, 0, 0);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Originating Date";
            // 
            // OriginTimePicker
            // 
            this.OriginTimePicker.CustomFormat = "MMM dd, yyyy\'";
            this.OriginTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.OriginTimePicker.Location = new System.Drawing.Point(102, 43);
            this.OriginTimePicker.Name = "OriginTimePicker";
            this.OriginTimePicker.ShowUpDown = true;
            this.OriginTimePicker.Size = new System.Drawing.Size(88, 20);
            this.OriginTimePicker.TabIndex = 9;
            // 
            // ToTimePicker
            // 
            this.ToTimePicker.CustomFormat = "MMM dd, yyyy\'";
            this.ToTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.ToTimePicker.Location = new System.Drawing.Point(114, 109);
            this.ToTimePicker.Name = "ToTimePicker";
            this.ToTimePicker.ShowUpDown = true;
            this.ToTimePicker.Size = new System.Drawing.Size(88, 20);
            this.ToTimePicker.TabIndex = 5;
            // 
            // FromDatePicker
            // 
            this.FromDatePicker.CustomFormat = "MMM dd, yyyy\'";
            this.FromDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.FromDatePicker.Location = new System.Drawing.Point(208, 82);
            this.FromDatePicker.Name = "FromDatePicker";
            this.FromDatePicker.Size = new System.Drawing.Size(103, 20);
            this.FromDatePicker.TabIndex = 4;
            this.FromDatePicker.Value = new System.DateTime(2016, 11, 1, 11, 15, 0, 0);
            // 
            // ToDatePicker
            // 
            this.ToDatePicker.CustomFormat = "MMM dd, yyyy\'";
            this.ToDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ToDatePicker.Location = new System.Drawing.Point(208, 109);
            this.ToDatePicker.Name = "ToDatePicker";
            this.ToDatePicker.Size = new System.Drawing.Size(103, 20);
            this.ToDatePicker.TabIndex = 6;
            this.ToDatePicker.Value = new System.DateTime(2016, 11, 1, 11, 15, 0, 0);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(79, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "From";
            // 
            // FromTimePicker
            // 
            this.FromTimePicker.CustomFormat = "MMM dd, yyyy\'";
            this.FromTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.FromTimePicker.Location = new System.Drawing.Point(114, 82);
            this.FromTimePicker.Name = "FromTimePicker";
            this.FromTimePicker.ShowUpDown = true;
            this.FromTimePicker.Size = new System.Drawing.Size(88, 20);
            this.FromTimePicker.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(89, 109);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "To";
            // 
            // FacilityTextBox
            // 
            this.FacilityTextBox.Location = new System.Drawing.Point(114, 27);
            this.FacilityTextBox.Name = "FacilityTextBox";
            this.FacilityTextBox.Size = new System.Drawing.Size(197, 20);
            this.FacilityTextBox.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(70, 32);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "Facility";
            // 
            // MBATextBox
            // 
            this.MBATextBox.Location = new System.Drawing.Point(114, 53);
            this.MBATextBox.Name = "MBATextBox";
            this.MBATextBox.Size = new System.Drawing.Size(197, 20);
            this.MBATextBox.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(79, 58);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 26;
            this.label10.Text = "MBA";
            // 
            // MaterialPanel
            // 
            this.MaterialPanel.Composition = null;
            this.MaterialPanel.Location = new System.Drawing.Point(11, 67);
            this.MaterialPanel.Name = "MaterialPanel";
            this.MaterialPanel.Size = new System.Drawing.Size(282, 363);
            this.MaterialPanel.TabIndex = 16;
            // 
            // DeclarationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 819);
            this.Controls.Add(this.MBATextBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.FacilityTextBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ToTimePicker);
            this.Controls.Add(this.FromDatePicker);
            this.Controls.Add(this.ToDatePicker);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.FromTimePicker);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ItemGroupBox);
            this.Controls.Add(this.ModificationTimePicker);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.CreationDatePicker);
            this.Controls.Add(this.ModificationDatePicker);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CreationTimePicker);
            this.Controls.Add(this.label2);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DeclarationEditor";
            this.Text = "Declaration Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ItemGroupBox.ResumeLayout(false);
            this.ItemGroupBox.PerformLayout();
            this.BatchGroupBox.ResumeLayout(false);
            this.BatchGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.DateTimePicker CreationTimePicker;
        private System.Windows.Forms.DateTimePicker CreationDatePicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker ModificationTimePicker;
        private System.Windows.Forms.DateTimePicker ModificationDatePicker;
        private System.Windows.Forms.GroupBox ItemGroupBox;
        private System.Windows.Forms.TextBox BarCodeTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox FullNameTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ItemNameTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker OriginDatePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker OriginTimePicker;
        private System.Windows.Forms.DateTimePicker ToTimePicker;
        private System.Windows.Forms.DateTimePicker FromDatePicker;
        private System.Windows.Forms.DateTimePicker ToDatePicker;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker FromTimePicker;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox FacilityTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox MBATextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox BatchGroupBox;
        private System.Windows.Forms.TextBox BatchSourceTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox BatchNameTextBox;
        private System.Windows.Forms.Label label11;
        private NuclearCompositionPanel MaterialPanel;
        private System.Windows.Forms.DateTimePicker MassDatePicker;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker MassTimePicker;
        private System.Windows.Forms.TextBox MassTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    }
}