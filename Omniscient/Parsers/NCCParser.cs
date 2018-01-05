using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Omniscient
{
    public class NCCParser
    {
        public NCCWriter.NCCType NCCMode;
        private string detectorType;
        private string detectorID;
        private string configurationID;
        private string itemID;
        private DateTime startDateTime;
        public List<NCCWriter.Cycle> Cycles;

        private string fileTypeCheck;

        public NCCParser()
        {
            NCCMode = NCCWriter.NCCType.VERIFICATION;
            detectorType = "XXXX";
            detectorID = "YYY";
            configurationID = "ZZ";
            itemID = "ItemID      ";
            Cycles = new List<NCCWriter.Cycle>();
        }

        private ReturnCode ParseHeader(BinaryReader readBinary)
        {
            fileTypeCheck = new string(readBinary.ReadChars(4));
            char modeChar = readBinary.ReadChar();
            switch (modeChar)
            {
                case 'B':
                    NCCMode = NCCWriter.NCCType.BACKGROUND;
                    break;
                case 'N':
                    NCCMode = NCCWriter.NCCType.NORMALIZATION;
                    break;
                case 'V':
                    NCCMode = NCCWriter.NCCType.VERIFICATION;
                    break;
                default:
                    return ReturnCode.CORRUPTED_FILE;
            }
            detectorType = new string(readBinary.ReadChars(4));
            readBinary.ReadChar();
            detectorID = new string(readBinary.ReadChars(3));
            readBinary.ReadChar();
            configurationID = new string(readBinary.ReadChars(2));
            itemID = new string(readBinary.ReadChars(12));
            string dateTimeString = new string(readBinary.ReadChars(16));
            startDateTime = new DateTime(2000 + int.Parse(dateTimeString.Substring(0, 2)),
                                            int.Parse(dateTimeString.Substring(3, 2)),
                                            int.Parse(dateTimeString.Substring(6, 2)),
                                            int.Parse(dateTimeString.Substring(8, 2)),
                                            int.Parse(dateTimeString.Substring(11, 2)),
                                            int.Parse(dateTimeString.Substring(14, 2)));
            // Stop short of cycles
            return ReturnCode.SUCCESS;
        }

        private ReturnCode ParseCycles(BinaryReader readBinary)
        {
            ushort nCycles = readBinary.ReadUInt16();
            Cycles = new List<NCCWriter.Cycle>(nCycles);
            NCCWriter.Cycle cycle;
            ushort nMultiplicityBins;
            for (int c = 0; c < nCycles; ++c)
            {
                cycle = new NCCWriter.Cycle();
                string dateTimeString = new string(readBinary.ReadChars(16));
                cycle.DateAndTime = new DateTime(2000 + int.Parse(dateTimeString.Substring(0, 2)),
                                                int.Parse(dateTimeString.Substring(3, 2)),
                                                int.Parse(dateTimeString.Substring(6, 2)),
                                                int.Parse(dateTimeString.Substring(8, 2)),
                                                int.Parse(dateTimeString.Substring(11, 2)),
                                                int.Parse(dateTimeString.Substring(14, 2)));
                cycle.CountSeconds = readBinary.ReadUInt16();
                cycle.Totals = readBinary.ReadDouble();
                cycle.RPlusA = readBinary.ReadDouble();
                cycle.A = readBinary.ReadDouble();
                cycle.Scaler1 = readBinary.ReadDouble();
                cycle.Scaler2 = readBinary.ReadDouble();
                nMultiplicityBins = readBinary.ReadUInt16();
                cycle.MultiplicityRPlusA = new UInt32[nMultiplicityBins];
                for (int m = 0; m < nMultiplicityBins; ++m)
                    cycle.MultiplicityRPlusA[m] = readBinary.ReadUInt32();
                cycle.MultiplicityA = new UInt32[nMultiplicityBins];
                for (int m = 0; m < nMultiplicityBins; ++m)
                    cycle.MultiplicityA[m] = readBinary.ReadUInt32();
                Cycles.Add(cycle);
            }

            return ReturnCode.SUCCESS;
        }

        public ReturnCode ReadNeutronCyclesFile(string fileName)
        {
            FileStream readStream;

            try
            {
                readStream = new FileStream(fileName, FileMode.Open);
                BinaryReader readBinary = new BinaryReader(readStream);

                // Read header
                ReturnCode returnCode = ParseHeader(readBinary);
                if (returnCode != ReturnCode.SUCCESS) return returnCode;

                // Read cycles
                returnCode = ParseCycles(readBinary);
                if (returnCode != ReturnCode.SUCCESS) return returnCode;

                readStream.Close();
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            return ReturnCode.SUCCESS;
        }

        public string GetDetectorType() { return detectorType; }
        public string GetDetectorID() { return detectorID; }
        public string GetConfigurationID() { return configurationID; }
        public string GetItemID() { return itemID.TrimEnd(' '); }
        public DateTime GetStartDateTime() { return startDateTime; }
    }
}
