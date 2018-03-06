namespace Omniscient
{
    partial class DataCompilerPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataCompilerPanel));
            this.CompiledFileTextBox = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.DataCompilersComboBox = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.ChannelComboBox = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CompiledFileTextBox
            // 
            this.CompiledFileTextBox.Location = new System.Drawing.Point(68, 98);
            this.CompiledFileTextBox.Multiline = true;
            this.CompiledFileTextBox.Name = "CompiledFileTextBox";
            this.CompiledFileTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.CompiledFileTextBox.Size = new System.Drawing.Size(190, 80);
            this.CompiledFileTextBox.TabIndex = 42;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(3, 101);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(58, 13);
            this.label19.TabIndex = 41;
            this.label19.Text = "Output File";
            // 
            // DataCompilersComboBox
            // 
            this.DataCompilersComboBox.FormattingEnabled = true;
            this.DataCompilersComboBox.Items.AddRange(new object[] {
            "Analysis",
            "Command"});
            this.DataCompilersComboBox.Location = new System.Drawing.Point(86, 68);
            this.DataCompilersComboBox.Name = "DataCompilersComboBox";
            this.DataCompilersComboBox.Size = new System.Drawing.Size(172, 21);
            this.DataCompilersComboBox.TabIndex = 40;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(3, 71);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(73, 13);
            this.label18.TabIndex = 39;
            this.label18.Text = "Data Compiler";
            // 
            // ChannelComboBox
            // 
            this.ChannelComboBox.FormattingEnabled = true;
            this.ChannelComboBox.Location = new System.Drawing.Point(56, 41);
            this.ChannelComboBox.Name = "ChannelComboBox";
            this.ChannelComboBox.Size = new System.Drawing.Size(202, 21);
            this.ChannelComboBox.TabIndex = 38;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 45);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(46, 13);
            this.label15.TabIndex = 37;
            this.label15.Text = "Channel";
            // 
            // RemoveButton
            // 
            this.RemoveButton.Image = ((System.Drawing.Image)(resources.GetObject("RemoveButton.Image")));
            this.RemoveButton.Location = new System.Drawing.Point(230, 7);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(28, 28);
            this.RemoveButton.TabIndex = 44;
            this.RemoveButton.UseVisualStyleBackColor = true;
            // 
            // AddButton
            // 
            this.AddButton.Image = ((System.Drawing.Image)(resources.GetObject("AddButton.Image")));
            this.AddButton.Location = new System.Drawing.Point(199, 7);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(28, 28);
            this.AddButton.TabIndex = 43;
            this.AddButton.UseVisualStyleBackColor = true;
            // 
            // DataCompilerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.CompiledFileTextBox);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.DataCompilersComboBox);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.ChannelComboBox);
            this.Controls.Add(this.label15);
            this.Name = "DataCompilerPanel";
            this.Size = new System.Drawing.Size(267, 200);
            this.Load += new System.EventHandler(this.DataCompilerPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox CompiledFileTextBox;
        private System.Windows.Forms.Label label19;
        public System.Windows.Forms.ComboBox DataCompilersComboBox;
        private System.Windows.Forms.Label label18;
        public System.Windows.Forms.ComboBox ChannelComboBox;
        private System.Windows.Forms.Label label15;
        public System.Windows.Forms.Button RemoveButton;
        public System.Windows.Forms.Button AddButton;
    }
}
