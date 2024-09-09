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
            this.panel1 = new System.Windows.Forms.Panel();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ItemIDComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ParamListPanel = new Omniscient.Controls.ParameterListPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CancelButton);
            this.panel1.Controls.Add(this.OKButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(680, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(120, 450);
            this.panel1.TabIndex = 1;
            // 
            // CancelButton
            // 
            this.CancelButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.CancelButton.Location = new System.Drawing.Point(10, 55);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(10);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(100, 45);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.OKButton.Location = new System.Drawing.Point(10, 10);
            this.OKButton.Margin = new System.Windows.Forms.Padding(10);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(100, 45);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ItemIDComboBox);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(680, 55);
            this.panel2.TabIndex = 2;
            // 
            // ItemIDComboBox
            // 
            this.ItemIDComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ItemIDComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ItemIDComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemIDComboBox.FormattingEnabled = true;
            this.ItemIDComboBox.Location = new System.Drawing.Point(82, 10);
            this.ItemIDComboBox.Name = "ItemIDComboBox";
            this.ItemIDComboBox.Size = new System.Drawing.Size(588, 28);
            this.ItemIDComboBox.TabIndex = 1;
            this.ItemIDComboBox.SelectedIndexChanged += new System.EventHandler(this.ItemIDComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(5);
            this.label1.Size = new System.Drawing.Size(72, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Item ID";
            // 
            // ParamListPanel
            // 
            this.ParamListPanel.AutoScroll = true;
            this.ParamListPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ParamListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParamListPanel.Location = new System.Drawing.Point(0, 0);
            this.ParamListPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ParamListPanel.Name = "ParamListPanel";
            this.ParamListPanel.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.ParamListPanel.Size = new System.Drawing.Size(680, 395);
            this.ParamListPanel.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ParamListPanel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 55);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(680, 395);
            this.panel3.TabIndex = 3;
            // 
            // DeclarationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "DeclarationEditor";
            this.Text = "Declaration Editor";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ParameterListPanel ParamListPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox ItemIDComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Panel panel3;
    }
}