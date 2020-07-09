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
    partial class InstTypeDialog
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
            this.ButtPanel = new System.Windows.Forms.Panel();
            this.CancelButton = new System.Windows.Forms.Button();
            this.TopButtPanel = new System.Windows.Forms.Panel();
            this.BottomButtPanel = new System.Windows.Forms.Panel();
            this.ButtPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtPanel
            // 
            this.ButtPanel.Controls.Add(this.BottomButtPanel);
            this.ButtPanel.Controls.Add(this.TopButtPanel);
            this.ButtPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtPanel.Location = new System.Drawing.Point(10, 10);
            this.ButtPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtPanel.Name = "ButtPanel";
            this.ButtPanel.Padding = new System.Windows.Forms.Padding(5);
            this.ButtPanel.Size = new System.Drawing.Size(266, 141);
            this.ButtPanel.TabIndex = 7;
            // 
            // CancelButton
            // 
            this.CancelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CancelButton.Location = new System.Drawing.Point(10, 151);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(266, 30);
            this.CancelButton.TabIndex = 9;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // TopButtPanel
            // 
            this.TopButtPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopButtPanel.Location = new System.Drawing.Point(5, 5);
            this.TopButtPanel.Name = "TopButtPanel";
            this.TopButtPanel.Size = new System.Drawing.Size(256, 64);
            this.TopButtPanel.TabIndex = 0;
            // 
            // BottomButtPanel
            // 
            this.BottomButtPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.BottomButtPanel.Location = new System.Drawing.Point(5, 69);
            this.BottomButtPanel.Name = "BottomButtPanel";
            this.BottomButtPanel.Size = new System.Drawing.Size(256, 64);
            this.BottomButtPanel.TabIndex = 1;
            // 
            // InstTypeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 191);
            this.ControlBox = false;
            this.Controls.Add(this.ButtPanel);
            this.Controls.Add(this.CancelButton);
            this.Name = "InstTypeDialog";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Instrument Type";
            this.Load += new System.EventHandler(this.EventTypeDialog_Load);
            this.ButtPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ButtPanel;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Panel BottomButtPanel;
        private System.Windows.Forms.Panel TopButtPanel;
    }
}