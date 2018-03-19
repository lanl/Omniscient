using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Omniscient
{
    public class SPEParser : SpectrumParser
    {
        const string PARSER_TYPE = "SPE";

        private double realTime;
        private double liveTime;
        private DateTime startDateTime;

        private int[] counts;

        private float zero;
        private float keVPerChannel;

        public SPEParser() : base(PARSER_TYPE)
        {
            realTime = 0.0;
            liveTime = 0.0;
            startDateTime = DateTime.Parse("14 April 1943");
            counts = new int[0];
            zero = 0;
            keVPerChannel = 0;
        }

        public override Spectrum GetSpectrum()
        {
            Spectrum spec = new Spectrum(zero, keVPerChannel, counts);
            spec.SetRealTime(realTime);
            spec.SetLiveTime(liveTime);
            spec.SetStartTime(startDateTime);
            return spec;
        }

        public override ReturnCode ParseSpectrumFile(string newFileName)
        {
            string[] lines;
            try
            {
                lines = File.ReadAllLines(newFileName);
            }
            catch
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            // Grab the counts
            int dataLine = -1;
            for (int l = 0; l < lines.Length; l++)
            {
                if (lines[l].ToUpper().StartsWith("$DATA"))
                {
                    dataLine = l;
                    break;
                }
            }
            if (dataLine < 0) return ReturnCode.CORRUPTED_FILE;
            try
            {
                string[] tokens = lines[dataLine + 1].Split(' ');
                int first_chan = int.Parse(tokens[0]);
                int last_chan = int.Parse(tokens[1]);
                int nChannels = last_chan - first_chan + 1;
                counts = new int[nChannels];
                for (int chan = 0; chan < nChannels; chan++)
                {
                    counts[chan] = int.Parse(lines[dataLine + chan + 2]);
                }
            }
            catch
            {
                return ReturnCode.CORRUPTED_FILE;
            }

            // Grab measurement time
            int measTimLine = -1;
            for (int l = 0; l < lines.Length; l++)
            {
                if (lines[l].ToUpper().StartsWith("$MEAS_TIM"))
                {
                    measTimLine = l;
                    break;
                }
            }
            if (measTimLine > 0)
            {
                try
                {
                    string[] tokens = lines[measTimLine + 1].Split(' ');
                    liveTime = double.Parse(tokens[0]);
                    realTime = double.Parse(tokens[1]);
                }
                catch
                {
                    return ReturnCode.CORRUPTED_FILE;
                }
            }

            // Grab start DateTime
            int dateMeaLine = -1;
            for (int l = 0; l < lines.Length; l++)
            {
                if (lines[l].ToUpper().StartsWith("$DATE_MEA"))
                {
                    dateMeaLine = l;
                    break;
                }
            }
            if (dateMeaLine > 0)
            {
                try
                {
                    string[] tokens = lines[dateMeaLine + 1].Replace('-',' ').Replace(':', ' ').Split(' ');
                    int month = int.Parse(tokens[0]);
                    int day = int.Parse(tokens[1]);
                    int year = int.Parse(tokens[2]);
                    int hour = int.Parse(tokens[3]);
                    int min = int.Parse(tokens[4]);
                    int sec = int.Parse(tokens[5]);
                    startDateTime = new DateTime(year, month, day, hour, min, sec);
                }
                catch
                {
                    return ReturnCode.CORRUPTED_FILE;
                }
            }

            // Grab calibration
            int enerFitLine = -1;
            for (int l = 0; l < lines.Length; l++)
            {
                if (lines[l].ToUpper().StartsWith("$ENER_FIT"))
                {
                    enerFitLine = l;
                    break;
                }
            }
            if (enerFitLine > 0)
            {
                try
                {
                    string[] tokens = lines[enerFitLine + 1].Split(' ');
                    zero = float.Parse(tokens[0]);
                    keVPerChannel = float.Parse(tokens[1]);
                }
                catch
                {
                    return ReturnCode.CORRUPTED_FILE;
                }
            }
            return ReturnCode.SUCCESS;
        }
    }
}
