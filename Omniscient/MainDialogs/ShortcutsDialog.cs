using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omniscient.MainDialogs
{
    public partial class ShortcutsDialog : Form
    {
        public ShortcutsDialog()
        {
            InitializeComponent();
            ShortcutsDataGridView.Rows.Add("Navigate backward", "Alt+Left");
            ShortcutsDataGridView.Rows.Add("Navigate forward", "Alt+Right");
            ShortcutsDataGridView.Rows.Add("Shift view back 100%", "Ctrl+Left");
            ShortcutsDataGridView.Rows.Add("Shift view forward 100%", "Ctrl+Right");
            ShortcutsDataGridView.Rows.Add("Shift view back 10%", "Ctrl+Shift+Left");
            ShortcutsDataGridView.Rows.Add("Shift view forward 10%", "Ctrl+Shift+Right");
            ShortcutsDataGridView.Rows.Add("Shift view to start of data", "Ctrl+Home");
            ShortcutsDataGridView.Rows.Add("Shift view to end of data", "Ctrl+End");
            ShortcutsDataGridView.Rows.Add("Zoom in", "Ctrl+Up");
            ShortcutsDataGridView.Rows.Add("Zoom out", "Ctrl+Down");
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
