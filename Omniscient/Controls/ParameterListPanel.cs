using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omniscient.Controls
{
    public partial class ParameterListPanel : UserControl
    {
        public List<Parameter> Parameters { get; private set; }
        List<ParameterPanel> paramPanels;
        public ParameterListPanel()
        {
            InitializeComponent();
        }

        public void LoadParameters(List<Parameter> parameters)
        {
            SuspendLayout();
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                if (Controls[i] is ParameterPanel) Controls.RemoveAt(i);
            }

            Parameters = parameters;
            paramPanels = new List<ParameterPanel>();
            for (int i= parameters.Count-1; i>=0; i--)
            {
                ParameterPanel panel = new ParameterPanel(parameters[i]);
                panel.Dock = DockStyle.Top;
                Controls.Add(panel);
                paramPanels.Add(panel);
            }
            ResumeLayout();
        }

        public bool ValidateInput()
        {
            foreach(ParameterPanel panel in paramPanels)
            {
                if (panel.ValidateInput() == false)
                {
                    MessageBox.Show("Invalid input for " + panel.parameter.Name + "!");
                    return false;
                }
            }
            return true;
        }

        public void Scrape()
        {
            Parameters.Clear();
            foreach (ParameterPanel panel in paramPanels)
            {
                panel.Scrape();
                Parameters.Add(panel.parameter);
            }

        }
    }
}
