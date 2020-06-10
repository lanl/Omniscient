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
    partial class ChannelPanel
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
            this.components = new System.ComponentModel.Container();
            this.SymbolComboBox = new System.Windows.Forms.ComboBox();
            this.Chart4CheckBox = new System.Windows.Forms.CheckBox();
            this.Chart3CheckBox = new System.Windows.Forms.CheckBox();
            this.Chart2CheckBox = new System.Windows.Forms.CheckBox();
            this.Chart1CheckBox = new System.Windows.Forms.CheckBox();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.NameToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ColorButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SymbolComboBox
            // 
            this.SymbolComboBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.SymbolComboBox.Font = new System.Drawing.Font("Microsoft PhagsPa", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SymbolComboBox.FormattingEnabled = true;
            this.SymbolComboBox.Location = new System.Drawing.Point(360, 0);
            this.SymbolComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SymbolComboBox.Name = "SymbolComboBox";
            this.SymbolComboBox.Size = new System.Drawing.Size(54, 30);
            this.SymbolComboBox.TabIndex = 12;
            this.SymbolComboBox.SelectedIndexChanged += new System.EventHandler(this.SymbolComboBox_SelectedIndexChanged);
            // 
            // Chart4CheckBox
            // 
            this.Chart4CheckBox.AutoSize = true;
            this.Chart4CheckBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.Chart4CheckBox.Location = new System.Drawing.Point(322, 0);
            this.Chart4CheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Chart4CheckBox.Name = "Chart4CheckBox";
            this.Chart4CheckBox.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Chart4CheckBox.Size = new System.Drawing.Size(38, 37);
            this.Chart4CheckBox.TabIndex = 11;
            this.Chart4CheckBox.Tag = "4";
            this.Chart4CheckBox.UseVisualStyleBackColor = true;
            this.Chart4CheckBox.CheckedChanged += new System.EventHandler(this.Chart4CheckBox_CheckedChanged);
            // 
            // Chart3CheckBox
            // 
            this.Chart3CheckBox.AutoSize = true;
            this.Chart3CheckBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.Chart3CheckBox.Location = new System.Drawing.Point(284, 0);
            this.Chart3CheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Chart3CheckBox.Name = "Chart3CheckBox";
            this.Chart3CheckBox.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Chart3CheckBox.Size = new System.Drawing.Size(38, 37);
            this.Chart3CheckBox.TabIndex = 10;
            this.Chart3CheckBox.Tag = "3";
            this.Chart3CheckBox.UseVisualStyleBackColor = true;
            this.Chart3CheckBox.CheckedChanged += new System.EventHandler(this.Chart3CheckBox_CheckedChanged);
            // 
            // Chart2CheckBox
            // 
            this.Chart2CheckBox.AutoSize = true;
            this.Chart2CheckBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.Chart2CheckBox.Location = new System.Drawing.Point(246, 0);
            this.Chart2CheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Chart2CheckBox.Name = "Chart2CheckBox";
            this.Chart2CheckBox.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Chart2CheckBox.Size = new System.Drawing.Size(38, 37);
            this.Chart2CheckBox.TabIndex = 9;
            this.Chart2CheckBox.Tag = "2";
            this.Chart2CheckBox.UseVisualStyleBackColor = true;
            this.Chart2CheckBox.CheckedChanged += new System.EventHandler(this.Chart2CheckBox_CheckedChanged);
            // 
            // Chart1CheckBox
            // 
            this.Chart1CheckBox.AutoSize = true;
            this.Chart1CheckBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.Chart1CheckBox.Location = new System.Drawing.Point(208, 0);
            this.Chart1CheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Chart1CheckBox.Name = "Chart1CheckBox";
            this.Chart1CheckBox.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Chart1CheckBox.Size = new System.Drawing.Size(38, 37);
            this.Chart1CheckBox.TabIndex = 8;
            this.Chart1CheckBox.Tag = "1";
            this.Chart1CheckBox.UseVisualStyleBackColor = true;
            this.Chart1CheckBox.CheckedChanged += new System.EventHandler(this.Chart1CheckBox_CheckedChanged);
            // 
            // NameTextBox
            // 
            this.NameTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.NameTextBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.NameTextBox.Location = new System.Drawing.Point(0, 0);
            this.NameTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(208, 26);
            this.NameTextBox.TabIndex = 7;
            // 
            // NameToolTip
            // 
            this.NameToolTip.AutomaticDelay = 125;
            this.NameToolTip.AutoPopDelay = 5000;
            this.NameToolTip.InitialDelay = 125;
            this.NameToolTip.ReshowDelay = 25;
            this.NameToolTip.ShowAlways = true;
            // 
            // ColorButton
            // 
            this.ColorButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.ColorButton.Location = new System.Drawing.Point(414, 0);
            this.ColorButton.Margin = new System.Windows.Forms.Padding(8);
            this.ColorButton.Name = "ColorButton";
            this.ColorButton.Size = new System.Drawing.Size(37, 37);
            this.ColorButton.TabIndex = 13;
            this.ColorButton.UseVisualStyleBackColor = true;
            this.ColorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // ChannelPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ColorButton);
            this.Controls.Add(this.SymbolComboBox);
            this.Controls.Add(this.Chart4CheckBox);
            this.Controls.Add(this.Chart3CheckBox);
            this.Controls.Add(this.Chart2CheckBox);
            this.Controls.Add(this.Chart1CheckBox);
            this.Controls.Add(this.NameTextBox);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ChannelPanel";
            this.Size = new System.Drawing.Size(467, 37);
            this.Load += new System.EventHandler(this.ChannelPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox SymbolComboBox;
        public System.Windows.Forms.CheckBox Chart4CheckBox;
        public System.Windows.Forms.CheckBox Chart3CheckBox;
        public System.Windows.Forms.CheckBox Chart2CheckBox;
        public System.Windows.Forms.CheckBox Chart1CheckBox;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.ToolTip NameToolTip;
        private System.Windows.Forms.Button ColorButton;
    }
}
