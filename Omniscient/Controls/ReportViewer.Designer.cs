namespace Omniscient.Controls
{
    partial class ReportViewer
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
            this.ReportTextBox = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ReportTextBox
            // 
            this.ReportTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ReportTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ReportTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReportTextBox.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReportTextBox.Location = new System.Drawing.Point(5, 5);
            this.ReportTextBox.Name = "ReportTextBox";
            this.ReportTextBox.ReadOnly = true;
            this.ReportTextBox.Size = new System.Drawing.Size(854, 688);
            this.ReportTextBox.TabIndex = 0;
            this.ReportTextBox.Text = "";
            this.ReportTextBox.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.ReportTextBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(868, 702);
            this.panel1.TabIndex = 1;
            // 
            // ReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 712);
            this.Controls.Add(this.panel1);
            this.Name = "ReportViewer";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox ReportTextBox;
        private System.Windows.Forms.Panel panel1;
    }
}