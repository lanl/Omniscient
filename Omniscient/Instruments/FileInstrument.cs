using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Omniscient
{
    class FileInstrument : Instrument
    {
        public string DatePattern { get; set; }
        public string DateRegexPattern { get; set; }

        public FileInstrument(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {


            InstrumentType = "File";
            FileExtension = "txt";
            filePrefix = "";
            fileSuffix = "";
            numChannels = 1;
            channels = new Channel[numChannels];
            channels[0] = new Channel(Name + "-File", this, Channel.ChannelType.COUNT_RATE, 0);
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Date Pattern":
                        DatePattern = param.Value;
                        DateRegexPattern = (param as DateTimeFormatParameter).GetRegexPattern();
                        break;
                    case "Extension":
                        FileExtension = param.Value;
                        break;
                }
            }
            ApplyStandardInstrumentParameters(this, parameters);
        }

        public override DateTime GetFileDate(string file)
        {
            string fileAbrev = file.Substring(file.LastIndexOf('\\') + 1);
            string fileStrippedName = fileAbrev.Substring(filePrefix.Length,
                fileAbrev.Length - (filePrefix.Length + fileSuffix.Length + FileExtension.Length + 1));

            Regex regex = new Regex(DateRegexPattern);
            Match match = regex.Match(fileStrippedName);
            if (match.Success)
            {
                return DateTime.ParseExact(match.Value, DatePattern, CultureInfo.InvariantCulture);
            }
            throw new FormatException("File does not contain a valid date");
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = GetStandardInstrumentParameters();
            parameters.Add(new StringParameter("Extension")
            {
                Value = FileExtension
            });
            parameters.Add(new DateTimeFormatParameter("Date Pattern")
            {
                Value = DatePattern
            });
            return parameters;
        }

        public override ReturnCode IngestFile(ChannelCompartment compartment, string fileName)
        {
            DateTime dateTime;
            try
            { 
                dateTime = GetFileDate(fileName);
            }
            catch
            {
                return ReturnCode.FAIL;
            }
            DataFile dataFile = new DataFile(fileName, dateTime);

            channels[0].AddDataPoint(compartment, dateTime, 1, dataFile);

            return ReturnCode.SUCCESS;
        }

        public override ReturnCode AutoIngestFile(ChannelCompartment compartment, string fileName)
        {
            return ReturnCode.FAIL;
        }
    }

    public class FileInstrumentHookup : InstrumentHookup
    {
        public FileInstrumentHookup()
        {
            TemplateParameters.Add(new ParameterTemplate("Extension", ParameterType.String));
            TemplateParameters.Add(new ParameterTemplate("Date Pattern", ParameterType.DateTimeFormat));
        }

        public override string Type { get { return "File"; } }

        public override Instrument FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            FileInstrument instrument = new FileInstrument(parent, newName, id);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Date Pattern":
                        instrument.DatePattern = param.Value;
                        instrument.DateRegexPattern = (param as DateTimeFormatParameter).GetRegexPattern();
                        break;
                    case "Extension":
                        instrument.FileExtension = param.Value;
                        break;
                }
            }
            Instrument.ApplyStandardInstrumentParameters(instrument, parameters);
            return instrument;
        }
    }
}
