using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class MeasuredValue
    {
        public const double NOT_SET = double.MinValue;

        double val;
        public double Value
        {
            get { return val; }
            set { val = value; }
        }

        double uncertainty;
        public double Uncertainty
        {
            get { return uncertainty; }
            set { uncertainty = value; }
        }

        public double RelativeUncertainty
        {
            get { return uncertainty / val; }
            set { uncertainty = value * val; }
        }

        public MeasuredValue()
        {
            val = NOT_SET;
            uncertainty = NOT_SET;
        }

        public bool IsSet()
        {
            if (val == NOT_SET) return false;
            return true;
        }
    }

    public class NuclearComposition
    {
        public MeasuredValue PuMass { get; set; }
        public MeasuredValue Pu238MassPercent { get; set; }
        public MeasuredValue Pu239MassPercent { get; set; }
        public MeasuredValue Pu240MassPercent { get; set; }
        public MeasuredValue Pu241MassPercent { get; set; }
        public MeasuredValue Pu242MassPercent { get; set; }
        public MeasuredValue Am241MassPercent { get; set; }
        public DateTime PuDate { get; set; }
        public DateTime AmDate { get; set; }

        public MeasuredValue UMass { get; set; }
        public MeasuredValue U234MassPercent { get; set; }
        public MeasuredValue U235MassPercent { get; set; }
        public MeasuredValue U236MassPercent { get; set; }
        public MeasuredValue U238MassPercent { get; set; }

        public NuclearComposition()
        {
            PuMass = new MeasuredValue();
            Pu238MassPercent = new MeasuredValue();
            Pu239MassPercent = new MeasuredValue();
            Pu240MassPercent = new MeasuredValue();
            Pu241MassPercent = new MeasuredValue();
            Pu242MassPercent = new MeasuredValue();
            Am241MassPercent = new MeasuredValue();

            UMass = new MeasuredValue();
            U234MassPercent = new MeasuredValue();
            U235MassPercent = new MeasuredValue();
            U236MassPercent = new MeasuredValue();
            U238MassPercent = new MeasuredValue();
        }
    }
}
