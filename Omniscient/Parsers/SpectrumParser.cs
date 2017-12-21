using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public abstract class SpectrumParser
    {
        string parserType;

        public SpectrumParser(string type) { parserType = type; }

        abstract public ReturnCode ParseSpectrumFile(string newFileName);
        abstract public Spectrum GetSpectrum();

        public string GetParserType() { return parserType; }
    }
}
