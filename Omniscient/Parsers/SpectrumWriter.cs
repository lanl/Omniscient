using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public abstract class SpectrumWriter
    {
        string writerType;

        public SpectrumWriter(string type) { writerType = type; }

        protected Spectrum spectrum;

        public abstract ReturnCode WriteSpectrumFile(string fileName);

        public void SetSpectrum(Spectrum spec) { spectrum = spec; }

        public string GetWriterType() { return writerType; }
    }
}
