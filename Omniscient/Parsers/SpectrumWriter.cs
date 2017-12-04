using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public abstract class SpectrumWriter
    {
        Spectrum spectrum;
        DateTime dateTime;

        public abstract ReturnCode WriteSpectrumFile(string fileName);

        public void SetSpectrum(Spectrum spec) { spectrum = spec; }
        public void SetDateTime(DateTime specTime) { dateTime = specTime; }
    }
}
