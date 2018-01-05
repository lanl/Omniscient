using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Omniscient
{
    public class NCCWriter
    {
        public enum NCCType { BACKGROUND, NORMALIZATION, VERIFICATION };

        public struct Cycle
        {
            public DateTime DateAndTime;
            public ushort CountSeconds;
            public double Totals;
            public double RPlusA;
            public double A;
            public double Scaler1;
            public double Scaler2;
            public UInt32[] MultiplicityRPlusA;
            public UInt32[] MultiplicityA;
        }

        public NCCType NCCMode;
        private string detectorType;
        private string detectorID;
        private string configurationID;
        private string itemID;
        public List<Cycle> Cycles;

        private const string FILE_HEADER_CHECK = "IREV";

        public NCCWriter()
        {
            NCCMode = NCCType.VERIFICATION;
            detectorType = "XXXX";
            detectorID = "YYY";
            configurationID = "ZZ";
            itemID = "ItemID      ";
            Cycles = new List<Cycle>();
        }

        public ReturnCode WriteNeutronCyclesFile(string fileName)
        {
            // Make sure there is data to write
            if (Cycles.Count < 1) return ReturnCode.FAIL;

            // Open file
            FileStream writeStream;
            try { writeStream = new FileStream(fileName, FileMode.Create); }
            catch { return ReturnCode.COULD_NOT_OPEN_FILE; }
            BinaryWriter binaryWriter = new BinaryWriter(writeStream);

            // Write Header
            binaryWriter.Write(FILE_HEADER_CHECK.ToCharArray());
            switch(NCCMode)
            {
                case NCCType.BACKGROUND:
                    binaryWriter.Write('B');
                    break;
                case NCCType.NORMALIZATION:
                    binaryWriter.Write('N');
                    break;
                case NCCType.VERIFICATION:
                    binaryWriter.Write('V');
                    break;
            }
            binaryWriter.Write(detectorType.ToCharArray());       // 4 characters
            binaryWriter.Write('/');
            binaryWriter.Write(detectorID.ToCharArray());         // 3 characters
            binaryWriter.Write('/');
            binaryWriter.Write(configurationID.ToCharArray());    // 2 characters
            binaryWriter.Write(itemID.ToCharArray());             // 12 characters
            binaryWriter.Write(Cycles[0].DateAndTime.ToString("yy.MM.ddHH:mm:ss").ToCharArray());
            binaryWriter.Write((ushort)Cycles.Count);

            // Write Cycles
            foreach(Cycle cycle in Cycles)
            {
                binaryWriter.Write(cycle.DateAndTime.ToString("yy.MM.ddHH:mm:ss").ToCharArray());
                binaryWriter.Write(cycle.CountSeconds);
                binaryWriter.Write(cycle.Totals);
                binaryWriter.Write(cycle.RPlusA);
                binaryWriter.Write(cycle.A);
                binaryWriter.Write(cycle.Scaler1);
                binaryWriter.Write(cycle.Scaler2);
                binaryWriter.Write((ushort)cycle.MultiplicityRPlusA.Length);
                for(int i=0; i< cycle.MultiplicityRPlusA.Length; ++i)
                    binaryWriter.Write(cycle.MultiplicityRPlusA[i]);
                for (int i = 0; i < cycle.MultiplicityA.Length; ++i)
                    binaryWriter.Write(cycle.MultiplicityA[i]);
            }

            writeStream.Close();

            return ReturnCode.SUCCESS;
        }

        public ReturnCode SetDetectorType(string newType)
        {
            if (newType.Length != 4) return ReturnCode.BAD_INPUT;
            detectorType = newType;
            return ReturnCode.SUCCESS;
        }

        public ReturnCode SetDetectorID(string newID)
        {
            if (newID.Length != 3) return ReturnCode.BAD_INPUT;
            detectorID = newID;
            return ReturnCode.SUCCESS;
        }

        public ReturnCode SetConfigurationID(string newID)
        {
            if (newID.Length != 2) return ReturnCode.BAD_INPUT;
            configurationID = newID;
            return ReturnCode.SUCCESS;
        }

        public ReturnCode SetItemID(string newID)
        {
            if (newID.Length > 12) return ReturnCode.BAD_INPUT;
            itemID = newID;
            while (itemID.Length < 12) itemID = itemID + ' ';
            return ReturnCode.SUCCESS;
        }

        public string GetDetectorType() { return detectorType; }
        public string GetDetectorID() { return detectorID; }
        public string GetConfigurationID() { return configurationID; }
        public string GetItemID() { return itemID.TrimEnd(' '); }
    }
}
