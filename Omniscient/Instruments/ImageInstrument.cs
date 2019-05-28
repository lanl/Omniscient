using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Omniscient
{
    public class ImageInstrument : Instrument
    {
        private const string FILE_EXTENSION = "jpg";

        private Dictionary<string, string> DatePatterns { get; set; }
        public string DatePattern { get; set; }

        public ImageInstrument(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            InstrumentType = "Image";
            FileExtension = FILE_EXTENSION;
            filePrefix = "";

            numChannels = 1;
            channels = new Channel[numChannels];
            channels[0] = new Channel(Name + "-Channel", this, Channel.ChannelType.VIDEO, 0);

            DatePatterns = GenerateDatePatterns();
            DatePattern = DatePatterns.Keys.ElementAt(0);
        }

        public static Dictionary<string, string> GenerateDatePatterns()
        {
            Dictionary<string, string> patterns = new Dictionary<string, string>();
            patterns.Add("yyyy-MM-dd HH:mm:ss", @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}");
            patterns.Add("yyyy-MM-ddTHH:mm:ss", @"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}");
            patterns.Add("yyyyMMddTHHmmss", @"\d{8}T\d{6}");
            return patterns;
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Date Pattern":
                        DatePattern = param.Value;
                        break;
                }
            }
            ApplyStandardInstrumentParameters(this, parameters);
        }

        public override void ClearData(ChannelCompartment compartment)
        {
            foreach (Channel ch in channels)
            {
                ch.ClearData(compartment);
            }
        }

        public override DateTime GetFileDate(string file)
        {
            Regex regex = new Regex(DatePatterns[DatePattern]);
            Match match = regex.Match(file);
            if (match.Success)
            {
                return DateTime.ParseExact(match.Value, DatePattern, CultureInfo.InvariantCulture);
            }
            throw new FormatException("File does not contain a valid date");
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
            parameters.Add(new EnumParameter("Date Pattern")
            {
                Value = DatePattern,
                ValidValues = DatePatterns.Keys.ToList()
            });
            return parameters;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            DateTime dateTime = GetFileDate(fileName);
            DataFile dataFile = new DataFile(fileName, dateTime);

            channels[0].AddDataPoint(compartment, dateTime, 1, dataFile);

            return ReturnCode.SUCCESS;
        }
    }

    public class ImageInstrumentHookup : InstrumentHookup
    {
        public ImageInstrumentHookup()
        {
            TemplateParameters.Add(new ParameterTemplate("Date Pattern", ParameterType.Enum, ImageInstrument.GenerateDatePatterns().Keys.ToList()));
        }

        public override string Type { get { return "Image"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            ImageInstrument instrument = new ImageInstrument(parent, newName, id);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Date Pattern":
                        instrument.DatePattern = param.Value;
                        break;
                }
            }
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            return instrument;
        }
    }
}
