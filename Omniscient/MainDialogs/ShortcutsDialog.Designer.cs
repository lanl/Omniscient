namespace Omniscient.MainDialogs
{
    partial class ShortcutsDialog
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
            this.shortcutsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShortcutsDataGridView = new System.Windows.Forms.DataGridView();
            this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Shortcut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OkButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.ShortcutsDataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // shortcutsMenuItem
            // 
            this.shortcutsMenuItem.Name = "shortcutsMenuItem";
            this.shortcutsMenuItem.Size = new System.Drawing.Size(270, 34);
            this.shortcutsMenuItem.Text = "Keyboard Shortcuts";
            // 
            // ShortcutsDataGridView
            // 
            this.ShortcutsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ShortcutsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Action,
            this.Shortcut});
            this.ShortcutsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShortcutsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.ShortcutsDataGridView.Name = "ShortcutsDataGridView";
            this.ShortcutsDataGridView.RowHeadersWidth = 62;
            this.ShortcutsDataGridView.Size = new System.Drawing.Size(465, 299);
            this.ShortcutsDataGridView.TabIndex = 0;
            // 
            // Action
            // 
            this.Action.HeaderText = "Action";
            this.Action.MinimumWidth = 8;
            this.Action.Name = "Action";
            this.Action.ReadOnly = true;
            this.Action.Width = 250;
            // 
            // Shortcut
            // 
            this.Shortcut.HeaderText = "Shortcut";
            this.Shortcut.MinimumWidth = 8;
            this.Shortcut.Name = "Shortcut";
            this.Shortcut.ReadOnly = true;
            this.Shortcut.Width = 150;
            // 
            // OkButton
            // 
            this.OkButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.OkButton.Location = new System.Drawing.Point(387, 3);
            this.OkButton.Margin = new System.Windows.Forms.Padding(5);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 24);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.OkButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 299);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(465, 30);
            this.panel1.TabIndex = 2;
            // 
            // ShortcutsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 329);
            this.Controls.Add(this.ShortcutsDataGridView);
            this.Controls.Add(this.panel1);
            this.Name = "ShortcutsDialog";
            this.Text = "Keyboard Shortcuts";
            ((System.ComponentModel.ISupportInitialize)(this.ShortcutsDataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem shortcutsMenuItem;
        private System.Windows.Forms.DataGridView ShortcutsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn Shortcut;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Panel panel1;
    }
}