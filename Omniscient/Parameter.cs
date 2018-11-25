using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace Omniscient
{
    public enum ParameterType { Int, Double, Enum, TimeSpan, SystemChannel, SystemEventGenerator, FileName, InstrumentChannel }

    /// <summary>
    /// A description of a Parameter. Used in Hookups
    /// </summary>
    public class ParameterTemplate
    {
        public string Name { get; private set; }
        public ParameterType Type { get; private set; }
        public List<string> ValidValues { get; private set; }
        public ParameterTemplate(string name, ParameterType type)
        {
            Name = name;
            Type = type;
            ValidValues = new List<string>();
        }
        public ParameterTemplate(string name, ParameterType type, List<string> validValues)
        {
            Name = name;
            Type = type;
            ValidValues = validValues;
        }
    }

    /// <summary>
    /// The base class for all Parameter classes.
    /// </summary>
    /// <remarks>
    /// The idea behind Parameters is to make it easier to add new Instruments
    /// and EventGenerators to Omniscient. It will simplify the way 
    /// Omniscient's GUI works by allowing standard interfaces for Instrument
    /// and EventGenerator parameters.
    /// </remarks>
    public abstract class Parameter
    {
        public static List<Parameter> FromXML(XmlNode node, List<ParameterTemplate> templates, DetectionSystem system, Instrument instrument=null)
        {
            List<Parameter> parameters = new List<Parameter>();

            foreach (ParameterTemplate pTemplate in templates)
            {
                string paramNameStr = pTemplate.Name.Replace(' ', '_');
                switch (pTemplate.Type)
                {
                    case ParameterType.Int:
                        parameters.Add(new IntParameter(pTemplate.Name) { Value = node.Attributes[paramNameStr]?.InnerText });
                        break;
                    case ParameterType.Double:
                        parameters.Add(new DoubleParameter(pTemplate.Name) { Value = node.Attributes[paramNameStr]?.InnerText });
                        break;
                    case ParameterType.Enum:
                        parameters.Add(new EnumParameter(pTemplate.Name)
                        {
                            Value = node.Attributes[paramNameStr]?.InnerText,
                            ValidValues = pTemplate.ValidValues
                        });
                        break;
                    case ParameterType.SystemChannel:
                        parameters.Add(new SystemChannelParameter(pTemplate.Name, system) { Value = node.Attributes[paramNameStr]?.InnerText });
                        break;
                    case ParameterType.SystemEventGenerator:
                        parameters.Add(new SystemEventGeneratorParameter(pTemplate.Name, system) { Value = node.Attributes[paramNameStr]?.InnerText });
                        break;
                    case ParameterType.TimeSpan:
                        parameters.Add(new TimeSpanParameter(pTemplate.Name) { Value = node.Attributes[paramNameStr]?.InnerText });
                        break;
                    case ParameterType.FileName:
                        parameters.Add(new FileNameParameter(pTemplate.Name) { Value = node.Attributes[paramNameStr]?.InnerText });
                        break;
                    case ParameterType.InstrumentChannel:
                        parameters.Add(new InstrumentChannelParameter(pTemplate.Name, instrument) { Value = node.Attributes[paramNameStr]?.InnerText });
                        break;
                }
            }
            return parameters;
        }
        public ParameterType Type { get; private set; }
        public static string TypeToString(ParameterType type)
        {
            switch(type)
            {
                case ParameterType.Int:
                    return "Int";
                case ParameterType.Double:
                    return "Double";
                case ParameterType.Enum:
                    return "Enum";
                case ParameterType.SystemChannel:
                    return "SystemChannel";
                case ParameterType.SystemEventGenerator:
                    return "SystemEventGenerator";
                case ParameterType.TimeSpan:
                    return "TimeSpan";
                case ParameterType.FileName:
                    return "FileName";
                case ParameterType.InstrumentChannel:
                    return "InstrumentChannel";
            }
            return "";
        }

        public static ParameterType StringToType(string type)
        {
            switch(type)
            {
                case "Int":
                    return ParameterType.Int;
                case "Double":
                    return ParameterType.Double;
                case "Enum":
                    return ParameterType.Enum;
                case "SystemChannel":
                    return ParameterType.SystemChannel;
                case "SystemEventGenerator":
                    return ParameterType.SystemEventGenerator;
                case "TimeSpan":
                    return ParameterType.TimeSpan;
                case "FileName":
                    return ParameterType.FileName;
                case "InstrumentChannel":
                    return ParameterType.InstrumentChannel;
            }
            return ParameterType.Double;
        }

        public string Name { get; set; }
        public string Value { get; set; }

        public Parameter(string name, ParameterType type)
        {
            Value = "";
            Name = name;
            Type = type;
        }

        public abstract bool Validate();
    }

    /// <summary>
    /// Base class for all Parameter classes that have a limited set of valid
    /// values.
    /// </summary>
    public abstract class LimitedValueParameter : Parameter
    {
        public List<string> ValidValues { get; set; }

        public LimitedValueParameter(string name, ParameterType type) : base(name, type) { }

        public override bool Validate()
        {
            foreach (string validValue in ValidValues)
            {
                if (Value == validValue) return true;
            }
            return false;
        }
    }

    /// <summary>
    /// A simple Parameter for an integer.
    /// </summary>
    public class IntParameter : Parameter
    {
        public IntParameter(string name) : base(name, ParameterType.Int) { }
        public override bool Validate() { return int.TryParse(Value, out int result); }
        public int ToInt() { return int.Parse(Value); }
    }

    /// <summary>
    /// A simple Parameter for a double (i.e. a floating point number).
    /// </summary>
    public class DoubleParameter : Parameter
    {
        public DoubleParameter(string name) : base(name, ParameterType.Double) { }
        public override bool Validate() { return double.TryParse(Value, out double result); }
        public double ToDouble() { return double.Parse(Value); }
    }

    /// <summary>
    /// An EnumParameter is a parameter that can take on a fixed set of values.
    /// </summary>
    public class EnumParameter : LimitedValueParameter
    {
        public EnumParameter(string name) : base(name, ParameterType.Enum)
        {
            ValidValues = new List<string>();
        }

        public int ToInt()
        {
            for (int i = 0; i < ValidValues.Count; i++)
            {
                if (Value == ValidValues[i]) return i;
            }
            return -1;
        }
    }

    /// <summary>
    /// A TimeSpanParameter is internally stored as a string of a double 
    /// representing seconds.
    /// </summary>
    public class TimeSpanParameter : Parameter
    {
        public TimeSpanParameter(string name) : base(name, ParameterType.TimeSpan) {}

        public override bool Validate() { return double.TryParse(Value, out double result); }

        public TimeSpan ToTimeSpan()
        {
            return TimeSpan.FromSeconds(double.Parse(Value));
        }
    }

    /// <summary>
    /// A SystemChannelParameter is a Channel within a particular 
    /// DetectionSystem.
    /// </summary>
    public class SystemChannelParameter : LimitedValueParameter
    {
        public DetectionSystem System { get; set; }

        public SystemChannelParameter(string name, DetectionSystem system) : base(name, ParameterType.SystemChannel)
        {
            System = system;
            ValidValues = new List<string>();
            foreach (Instrument inst in System.GetInstruments())
            {
                foreach (Channel ch in inst.GetChannels())
                {
                    ValidValues.Add(ch.GetName());
                }
            }
        }

        public Channel ToChannel()
        {
            foreach (Instrument inst in System.GetInstruments())
            {
                foreach (Channel ch in inst.GetChannels())
                {
                    if (Value == ch.GetName()) return ch;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// A Parameter for an EventGenerator within a particular DetectionSystem
    /// </summary>
    public class SystemEventGeneratorParameter : LimitedValueParameter
    {
        DetectionSystem System { get; set; }
        public SystemEventGeneratorParameter(string name, DetectionSystem system) : base(name, ParameterType.SystemEventGenerator)
        {
            System = system;
            ValidValues = new List<string>();
            foreach (EventGenerator eg in System.GetEventGenerators())
            {
                ValidValues.Add(eg.GetName());
            }
        }

        public EventGenerator ToEventGenerator()
        {
            foreach (EventGenerator eg in System.GetEventGenerators())
            {
                if (Value == eg.GetName()) return eg;
            }
            return null;
        }
    }

    /// <summary>
    /// A FileNameParameter stores the name of a file on the system.
    /// </summary>
    public class FileNameParameter : Parameter
    {
        public FileNameParameter(string name) : base(name, ParameterType.FileName) {}

        public override bool Validate()
        {
            if (File.Exists(Value)) return true;
            return false;
        }
    }

    /// <summary>
    /// An InstrumentChannelParameter is a Channel within a particular 
    /// Instrument.
    /// </summary>
    public class InstrumentChannelParameter : LimitedValueParameter
    {
        public Instrument Instrument { get; set; }

        public InstrumentChannelParameter(string name, Instrument inst, int maxChannel=int.MaxValue) : base(name, ParameterType.InstrumentChannel)
        {
            Instrument = inst;
            ValidValues = new List<string>();
            Channel[] channels = Instrument.GetChannels();
            int nChannels = channels.Length;
            if (maxChannel > nChannels - 1) maxChannel = nChannels - 1;
            for (int i=0; i<=maxChannel; i++)
            {
                ValidValues.Add(channels[i].GetName());
            }
        }

        public Channel ToChannel()
        {
            foreach (Channel ch in Instrument.GetChannels())
            {
                if (Value == ch.GetName()) return ch;
            }
            return null;
        }
    }
}
