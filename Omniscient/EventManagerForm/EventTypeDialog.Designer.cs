namespace Omniscient
{
    partial class EventTypeDialog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.CancelButton = new System.Windows.Forms.Button();
            this.CoincidenceButton = new System.Windows.Forms.Button();
            this.ThresholdButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CancelButton);
            this.panel1.Controls.Add(this.CoincidenceButton);
            this.panel1.Controls.Add(this.ThresholdButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(184, 86);
            this.panel1.TabIndex = 7;
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(52, 50);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(80, 30);
            this.CancelButton.TabIndex = 9;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // CoincidenceButton
            // 
            this.CoincidenceButton.Location = new System.Drawing.Point(94, 10);
            this.CoincidenceButton.Name = "CoincidenceButton";
            this.CoincidenceButton.Size = new System.Drawing.Size(80, 30);
            this.CoincidenceButton.TabIndex = 8;
            this.CoincidenceButton.Text = "Coincidence";
            this.CoincidenceButton.UseVisualStyleBackColor = true;
            this.CoincidenceButton.Click += new System.EventHandler(this.CoincidenceButton_Click);
            // 
            // ThresholdButton
            // 
            this.ThresholdButton.Location = new System.Drawing.Point(10, 10);
            this.ThresholdButton.Name = "ThresholdButton";
            this.ThresholdButton.Size = new System.Drawing.Size(80, 30);
            this.ThresholdButton.TabIndex = 7;
            this.ThresholdButton.Text = "Threshold";
            this.ThresholdButton.UseVisualStyleBackColor = true;
            this.ThresholdButton.Click += new System.EventHandler(this.ThresholdButton_Click);
            // 
            // EventTypeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 86);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Name = "EventTypeDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Event Type";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button CoincidenceButton;
        private System.Windows.Forms.Button ThresholdButton;
    }
}