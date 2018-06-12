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
    partial class InstrumentTypeDialog
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
            this.GRANDButton = new System.Windows.Forms.Button();
            this.ISRButton = new System.Windows.Forms.Button();
            this.MCAButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.NGAMButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GRANDButton
            // 
            this.GRANDButton.Location = new System.Drawing.Point(12, 12);
            this.GRANDButton.Name = "GRANDButton";
            this.GRANDButton.Size = new System.Drawing.Size(80, 30);
            this.GRANDButton.TabIndex = 0;
            this.GRANDButton.Text = "GRAND";
            this.GRANDButton.UseVisualStyleBackColor = true;
            this.GRANDButton.Click += new System.EventHandler(this.GRANDButton_Click);
            // 
            // ISRButton
            // 
            this.ISRButton.Location = new System.Drawing.Point(98, 12);
            this.ISRButton.Name = "ISRButton";
            this.ISRButton.Size = new System.Drawing.Size(80, 30);
            this.ISRButton.TabIndex = 1;
            this.ISRButton.Text = "ISR";
            this.ISRButton.UseVisualStyleBackColor = true;
            this.ISRButton.Click += new System.EventHandler(this.ISRButton_Click);
            // 
            // MCAButton
            // 
            this.MCAButton.Location = new System.Drawing.Point(184, 12);
            this.MCAButton.Name = "MCAButton";
            this.MCAButton.Size = new System.Drawing.Size(80, 30);
            this.MCAButton.TabIndex = 2;
            this.MCAButton.Text = "MCA";
            this.MCAButton.UseVisualStyleBackColor = true;
            this.MCAButton.Click += new System.EventHandler(this.MCAButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(140, 56);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(80, 30);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // NGAMButton
            // 
            this.NGAMButton.Location = new System.Drawing.Point(270, 12);
            this.NGAMButton.Name = "NGAMButton";
            this.NGAMButton.Size = new System.Drawing.Size(80, 30);
            this.NGAMButton.TabIndex = 4;
            this.NGAMButton.Text = "NGAM";
            this.NGAMButton.UseVisualStyleBackColor = true;
            this.NGAMButton.Click += new System.EventHandler(this.NGAMButton_Click);
            // 
            // InstrumentTypeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 98);
            this.Controls.Add(this.NGAMButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.MCAButton);
            this.Controls.Add(this.ISRButton);
            this.Controls.Add(this.GRANDButton);
            this.Name = "InstrumentTypeDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Instrument Type";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button GRANDButton;
        private System.Windows.Forms.Button ISRButton;
        private System.Windows.Forms.Button MCAButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button NGAMButton;
    }
}