using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omniscient
{
    public class N42Parser : SpectrumParser
    {
        const string PARSER_TYPE = "N42";

        private int[] counts;
        private double realTime;
        private double liveTime;
        private DateTime startDateTime;

        private float zero;
        private float keVPerChannel;

        public N42Parser() : base(PARSER_TYPE)
        {
            zero = 0;
            keVPerChannel = 1;

            realTime = 0.0;
            liveTime = 0.0;
            startDateTime = DateTime.Parse("14 April 1943");
            counts = new int[0];
        }

        public override Spectrum GetSpectrum() 
        {
            Spectrum spec = new Spectrum(zero, keVPerChannel, counts);
            spec.SetRealTime(realTime);
            spec.SetLiveTime(liveTime);
            spec.SetStartTime(startDateTime);
            return spec;
        }

        /// <summary>
        /// Parses the Spectrum node in an N42 file
        /// </summary>
        /// <param name="specNode"></param>
        /// <returns></returns>
        private ReturnCode ParseSpectrumNode(XmlNode specNode)
        {
            foreach (XmlNode childNode in specNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "StartTime":
                        try
                        {
                            string tString = childNode.InnerText;
                            startDateTime = DateTimeOffset.Parse(tString, null).DateTime;
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                    case "RealTime":
                        try
                        {
                            string tString = childNode.InnerText;
                            string[] badChars = new string[] { " ", "P", "T", "S"};
                            foreach(string c in badChars)
                            {
                                tString = tString.Replace(c, "");
                            }
                            realTime = double.Parse(tString);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                    case "LiveTime":
                        try
                        {
                            string tString = childNode.InnerText;
                            string[] badChars = new string[] { " ", "P", "T", "S" };
                            foreach (string c in badChars)
                            {
                                tString = tString.Replace(c, "");
                            }
                            liveTime = double.Parse(tString);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                    case "ChannelData":
                        try
                        {
                            string channelStr = childNode.InnerText;
                            string[] countStr = channelStr.Trim().Split(new char[] { ' ' });
                            counts = new int[countStr.Length];
                            for (int i = 0; i < countStr.Length; ++i)
                            {
                                counts[i] = int.Parse(countStr[i]);
                            }
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                }
            }
            return ReturnCode.SUCCESS;
        }

        private ReturnCode ParseMeasurementNode(XmlNode measNode)
        {
            foreach (XmlNode childNode in measNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Spectrum":
                        ReturnCode returnCode = ParseSpectrumNode(childNode);
                        if (returnCode != ReturnCode.SUCCESS) return returnCode;
                        break;
                }
            }
            return ReturnCode.SUCCESS;
        }

        public override ReturnCode ParseSpectrumFile(string newFileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(newFileName);

            XmlNode N42Node = doc["N42InstrumentData"];
            foreach (XmlNode childNode in N42Node.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Measurement":
                        ReturnCode returnCode = ParseMeasurementNode(childNode);
                        if (returnCode != ReturnCode.SUCCESS) return returnCode;
                        break;
                }
            }
            return ReturnCode.SUCCESS;
        }
    }
}
