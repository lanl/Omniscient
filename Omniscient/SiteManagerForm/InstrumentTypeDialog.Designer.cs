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
            this.CancelButton.Location = new System.Drawing.Point(98, 56);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(80, 30);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // InstrumentTypeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 98);
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
    }
}