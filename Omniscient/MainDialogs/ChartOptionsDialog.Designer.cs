namespace Omniscient.MainDialogs
{
    partial class ChartOptionsDialog
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
            this.ButtonFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.YAxisTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BottomTextBox = new System.Windows.Forms.TextBox();
            this.TopTextBox = new System.Windows.Forms.TextBox();
            this.ButtonFlowLayoutPanel.SuspendLayout();
            this.YAxisTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonFlowLayoutPanel
            // 
            this.ButtonFlowLayoutPanel.AutoSize = true;
            this.ButtonFlowLayoutPanel.Controls.Add(this.CancelButton);
            this.ButtonFlowLayoutPanel.Controls.Add(this.OkButton);
            this.ButtonFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ButtonFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.ButtonFlowLayoutPanel.Location = new System.Drawing.Point(0, 108);
            this.ButtonFlowLayoutPanel.Name = "ButtonFlowLayoutPanel";
            this.ButtonFlowLayoutPanel.Size = new System.Drawing.Size(224, 33);
            this.ButtonFlowLayoutPanel.TabIndex = 3;
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(144, 5);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(5);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 5;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(59, 5);
            this.OkButton.Margin = new System.Windows.Forms.Padding(5);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 4;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // YAxisTableLayoutPanel
            // 
            this.YAxisTableLayoutPanel.ColumnCount = 2;
            this.YAxisTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.YAxisTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.YAxisTableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.YAxisTableLayoutPanel.Controls.Add(this.label2, 0, 1);
            this.YAxisTableLayoutPanel.Controls.Add(this.label3, 0, 2);
            this.YAxisTableLayoutPanel.Controls.Add(this.BottomTextBox, 1, 1);
            this.YAxisTableLayoutPanel.Controls.Add(this.TopTextBox, 1, 2);
            this.YAxisTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.YAxisTableLayoutPanel.Location = new System.Drawing.Point(0, 19);
            this.YAxisTableLayoutPanel.Name = "YAxisTableLayoutPanel";
            this.YAxisTableLayoutPanel.RowCount = 3;
            this.YAxisTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.YAxisTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.YAxisTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.YAxisTableLayoutPanel.Size = new System.Drawing.Size(224, 89);
            this.YAxisTableLayoutPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Y-Axis";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 29);
            this.label2.TabIndex = 1;
            this.label2.Text = "Bottom";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 31);
            this.label3.TabIndex = 2;
            this.label3.Text = "Top";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BottomTextBox
            // 
            this.BottomTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BottomTextBox.Location = new System.Drawing.Point(115, 33);
            this.BottomTextBox.Name = "BottomTextBox";
            this.BottomTextBox.Size = new System.Drawing.Size(100, 20);
            this.BottomTextBox.TabIndex = 1;
            this.BottomTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BottomTextBox_KeyPress);
            // 
            // TopTextBox
            // 
            this.TopTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TopTextBox.Location = new System.Drawing.Point(115, 63);
            this.TopTextBox.Name = "TopTextBox";
            this.TopTextBox.Size = new System.Drawing.Size(100, 20);
            this.TopTextBox.TabIndex = 2;
            this.TopTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TopTextBox_KeyPress);
            // 
            // ChartOptionsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 141);
            this.Controls.Add(this.YAxisTableLayoutPanel);
            this.Controls.Add(this.ButtonFlowLayoutPanel);
            this.Name = "ChartOptionsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chart Options";
            this.ButtonFlowLayoutPanel.ResumeLayout(false);
            this.YAxisTableLayoutPanel.ResumeLayout(false);
            this.YAxisTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel ButtonFlowLayoutPanel;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.TableLayoutPanel YAxisTableLayoutPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox BottomTextBox;
        private System.Windows.Forms.TextBox TopTextBox;
    }
}