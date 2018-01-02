using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Omniscient
{
    class FRAMParser
    {
        string[] lines;

        private NuclearComposition nuclearComposition;

        public FRAMParser()
        {
            lines = null;
            nuclearComposition = new NuclearComposition();
        }

        public ReturnCode ReadFRAMOutput(string fileName)
        {
            nuclearComposition = new NuclearComposition();

            if (!File.Exists(fileName))
                return ReturnCode.COULD_NOT_OPEN_FILE;
            lines = File.ReadAllLines(fileName);

            return ReturnCode.SUCCESS;
        }

        public ReturnCode ParseUraniumResults()
        {
            // Locate the correct part of the file to parse
            int targetLine = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("5Isotopic"))
                {
                    targetLine = i;
                    break;
                }
            }
            if (targetLine < 0 || !lines[targetLine + 3].Contains("U235") || !lines[targetLine + 4].StartsWith("5mass%"))
                return ReturnCode.CORRUPTED_FILE;

            // Read mass percents
            string[] tokens = lines[targetLine + 4].Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            try
            {
                nuclearComposition.U234MassPercent.Value = double.Parse(tokens[1]);
                nuclearComposition.U235MassPercent.Value = double.Parse(tokens[2]);
                nuclearComposition.U236MassPercent.Value = double.Parse(tokens[3]);
                nuclearComposition.U238MassPercent.Value = double.Parse(tokens[4]);
            }
            catch
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            // Read mass sigmas
            tokens = lines[targetLine + 5].Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            try
            {
                nuclearComposition.U234MassPercent.Uncertainty = double.Parse(tokens[1]);
                nuclearComposition.U235MassPercent.Uncertainty = double.Parse(tokens[2]);
                nuclearComposition.U236MassPercent.Uncertainty = double.Parse(tokens[3]);
                nuclearComposition.U238MassPercent.Uncertainty = double.Parse(tokens[4]);
            }
            catch
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            return ReturnCode.SUCCESS;
        }

        public ReturnCode ParsePlutoniumResults()
        {
            // Locate the correct part of the file to parse
            int targetLine = -1;
            for (int i=0; i<lines.Length; i++)
            {
                if (lines[i].StartsWith("5Isotopic"))
                {
                    targetLine = i;
                    break;
                }
            }
            if (targetLine < 0 || !lines[targetLine + 3].Contains("Pu239") || !lines[targetLine+4].StartsWith("5mass%"))
                return ReturnCode.CORRUPTED_FILE;

            // Read mass percents
            string[] tokens = lines[targetLine + 4].Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            try
            {
                nuclearComposition.Pu238MassPercent.Value = double.Parse(tokens[1]);
                nuclearComposition.Pu239MassPercent.Value = double.Parse(tokens[2]);
                nuclearComposition.Pu240MassPercent.Value = double.Parse(tokens[3]);
                nuclearComposition.Pu241MassPercent.Value = double.Parse(tokens[4]);
                nuclearComposition.Pu242MassPercent.Value = double.Parse(tokens[5]);
                nuclearComposition.Am241MassPercent.Value = double.Parse(tokens[6]);
            }
            catch
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            // Read mass sigmas
            tokens = lines[targetLine + 5].Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            try
            {
                nuclearComposition.Pu238MassPercent.Uncertainty = double.Parse(tokens[1]);
                nuclearComposition.Pu239MassPercent.Uncertainty = double.Parse(tokens[2]);
                nuclearComposition.Pu240MassPercent.Uncertainty = double.Parse(tokens[3]);
                nuclearComposition.Pu241MassPercent.Uncertainty = double.Parse(tokens[4]);
                nuclearComposition.Pu242MassPercent.Uncertainty = double.Parse(tokens[5]);
                nuclearComposition.Am241MassPercent.Uncertainty = double.Parse(tokens[6]);
            }
            catch
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            return ReturnCode.SUCCESS;
        }

        public NuclearComposition GetNuclearComposition() { return nuclearComposition; }
    }
}
