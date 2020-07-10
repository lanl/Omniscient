namespace Omniscient.MainDialogs
{
    partial class NewInstrumentDialog
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
            this.UpperPanel = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.SystemComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.FacilityComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SiteComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.InstrumentButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.UpperPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.ButtonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // UpperPanel
            // 
            this.UpperPanel.Controls.Add(this.panel3);
            this.UpperPanel.Controls.Add(this.panel2);
            this.UpperPanel.Controls.Add(this.panel1);
            this.UpperPanel.Controls.Add(this.panel4);
            this.UpperPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UpperPanel.Location = new System.Drawing.Point(0, 0);
            this.UpperPanel.Name = "UpperPanel";
            this.UpperPanel.Size = new System.Drawing.Size(315, 152);
            this.UpperPanel.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.SystemComboBox);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 70);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(315, 30);
            this.panel3.TabIndex = 3;
            // 
            // SystemComboBox
            // 
            this.SystemComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SystemComboBox.FormattingEnabled = true;
            this.SystemComboBox.Location = new System.Drawing.Point(74, 0);
            this.SystemComboBox.Name = "SystemComboBox";
            this.SystemComboBox.Size = new System.Drawing.Size(231, 21);
            this.SystemComboBox.TabIndex = 3;
            this.SystemComboBox.SelectedIndexChanged += new System.EventHandler(this.SystemComboBox_SelectedIndexChanged);
            this.SystemComboBox.TextUpdate += new System.EventHandler(this.SystemComboBox_TextUpdate);
            this.SystemComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SystemComboBox_KeyDown);
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Location = new System.Drawing.Point(305, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(10, 30);
            this.label4.TabIndex = 3;
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 30);
            this.label3.TabIndex = 1;
            this.label3.Text = "System:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.FacilityComboBox);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(315, 30);
            this.panel2.TabIndex = 1;
            // 
            // FacilityComboBox
            // 
            this.FacilityComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FacilityComboBox.FormattingEnabled = true;
            this.FacilityComboBox.Location = new System.Drawing.Point(74, 0);
            this.FacilityComboBox.Name = "FacilityComboBox";
            this.FacilityComboBox.Size = new System.Drawing.Size(231, 21);
            this.FacilityComboBox.TabIndex = 2;
            this.FacilityComboBox.SelectedIndexChanged += new System.EventHandler(this.FacilityComboBox_SelectedIndexChanged);
            this.FacilityComboBox.TextUpdate += new System.EventHandler(this.FacilityComboBox_TextUpdate);
            this.FacilityComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FacilityComboBox_KeyDown);
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Right;
            this.label5.Location = new System.Drawing.Point(305, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 30);
            this.label5.TabIndex = 4;
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "Facility:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SiteComboBox);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(315, 30);
            this.panel1.TabIndex = 0;
            // 
            // SiteComboBox
            // 
            this.SiteComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SiteComboBox.FormattingEnabled = true;
            this.SiteComboBox.Location = new System.Drawing.Point(74, 0);
            this.SiteComboBox.Name = "SiteComboBox";
            this.SiteComboBox.Size = new System.Drawing.Size(231, 21);
            this.SiteComboBox.TabIndex = 1;
            this.SiteComboBox.SelectedIndexChanged += new System.EventHandler(this.SiteComboBox_SelectedIndexChanged);
            this.SiteComboBox.TextUpdate += new System.EventHandler(this.SiteComboBox_TextUpdate);
            this.SiteComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SiteComboBox_KeyDown);
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Right;
            this.label6.Location = new System.Drawing.Point(305, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 30);
            this.label6.TabIndex = 4;
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Site:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(315, 10);
            this.panel4.TabIndex = 2;
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.Controls.Add(this.InstrumentButton);
            this.ButtonPanel.Controls.Add(this.CancelButton);
            this.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ButtonPanel.Location = new System.Drawing.Point(0, 112);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Padding = new System.Windows.Forms.Padding(5);
            this.ButtonPanel.Size = new System.Drawing.Size(315, 40);
            this.ButtonPanel.TabIndex = 4;
            // 
            // InstrumentButton
            // 
            this.InstrumentButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.InstrumentButton.Location = new System.Drawing.Point(160, 5);
            this.InstrumentButton.Margin = new System.Windows.Forms.Padding(5);
            this.InstrumentButton.Name = "InstrumentButton";
            this.InstrumentButton.Size = new System.Drawing.Size(75, 30);
            this.InstrumentButton.TabIndex = 4;
            this.InstrumentButton.Text = "Next";
            this.InstrumentButton.UseVisualStyleBackColor = true;
            this.InstrumentButton.Click += new System.EventHandler(this.InstrumentButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.CancelButton.Location = new System.Drawing.Point(235, 5);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(5);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 30);
            this.CancelButton.TabIndex = 5;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // NewInstrumentDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 152);
            this.Controls.Add(this.ButtonPanel);
            this.Controls.Add(this.UpperPanel);
            this.Name = "NewInstrumentDialog";
            this.Text = "Create Instrument";
            this.Load += new System.EventHandler(this.NewInstrumentDialog_Load);
            this.UpperPanel.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ButtonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel UpperPanel;
        private System.Windows.Forms.Panel ButtonPanel;
        private System.Windows.Forms.Button InstrumentButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox SystemComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox FacilityComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox SiteComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel4;
    }
}