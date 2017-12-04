using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class SpectrumCompiler : DataCompiler
    {
        SpectrumParser spectrumParser;
        SpectrumWriter spectrumWriter;

        public SpectrumCompiler(string newName) : base(newName)
        {
        }

        public override ReturnCode Compile(List<string> sourceFiles, DateTime start, DateTime end, string targetFileName)
        {
            if (sourceFiles.Count() == 0) return ReturnCode.FAIL;

            spectrumParser.ParseSpectrumFile(sourceFiles[0]);
            Spectrum result = spectrumParser.GetSpectrum();

            for (int f=1; f<sourceFiles.Count(); f++)
            {
                spectrumParser.ParseSpectrumFile(sourceFiles[f]);
                result.Add(spectrumParser.GetSpectrum());
            };

            spectrumWriter.SetDateTime(start);
            spectrumWriter.SetSpectrum(result);
            spectrumWriter.WriteSpectrumFile(targetFileName);
            return ReturnCode.SUCCESS;
        }

        public SpectrumParser GetSpectrumParser() { return spectrumParser; }
        public SpectrumWriter GetSpectrumWriter() { return spectrumWriter; }

        public void SetSpectrumParser(SpectrumParser specP) { spectrumParser = specP; }
        public void SetSpectrumWriter(SpectrumWriter specW) { spectrumWriter = specW; }
    }
}
