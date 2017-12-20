using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omniscient
{
    public partial class NuclearCompositionPanel : UserControl
    {
        public NuclearComposition Composition { get; set; }

        public NuclearCompositionPanel()
        {
            InitializeComponent();
            Composition = null;
        }

        public void UpdateFields()
        {
            if (Composition == null) return;
            PuMassTextBox.Text = Composition.PuMass.Value.ToString();
            PuMassUncertaintyTextBox.Text = Composition.PuMass.Uncertainty.ToString();

            Pu238MassPercentTextBox.Text = Composition.Pu238MassPercent.Value.ToString();
            Pu238UncertaintyTextBox.Text = Composition.Pu238MassPercent.Uncertainty.ToString();

            Pu239MassPercentTextBox.Text = Composition.Pu239MassPercent.Value.ToString();
            Pu239UncertaintyTextBox.Text = Composition.Pu239MassPercent.Uncertainty.ToString();

            Pu240MassPercentTextBox.Text = Composition.Pu240MassPercent.Value.ToString();
            Pu240UncertaintyTextBox.Text = Composition.Pu240MassPercent.Uncertainty.ToString();

            Pu241MassPercentTextBox.Text = Composition.Pu241MassPercent.Value.ToString();
            Pu241UncertaintyTextBox.Text = Composition.Pu241MassPercent.Uncertainty.ToString();

            Pu242MassPercentTextBox.Text = Composition.Pu242MassPercent.Value.ToString();
            Pu242UncertaintyTextBox.Text = Composition.Pu242MassPercent.Uncertainty.ToString();

            Am241MassPercentTextBox.Text = Composition.Am241MassPercent.Value.ToString();
            Am241UncertaintyTextBox.Text = Composition.Am241MassPercent.Uncertainty.ToString();

            UMassTextBox.Text = Composition.UMass.Value.ToString();
            UMassUncertaintyTextBox.Text = Composition.UMass.Uncertainty.ToString();

            U234MassPercentTextBox.Text = Composition.U234MassPercent.Value.ToString();
            U234UncertaintyTextBox.Text = Composition.U234MassPercent.Uncertainty.ToString();

            U235MassPercentTextBox.Text = Composition.U235MassPercent.Value.ToString();
            U235UncertaintyTextBox.Text = Composition.U235MassPercent.Uncertainty.ToString();

            U236MassPercentTextBox.Text = Composition.U236MassPercent.Value.ToString();
            U236UncertaintyTextBox.Text = Composition.U236MassPercent.Uncertainty.ToString();

            U238MassPercentTextBox.Text = Composition.U238MassPercent.Value.ToString();
            U238UncertaintyTextBox.Text = Composition.U238MassPercent.Uncertainty.ToString();
        }

    }
}
