using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Omniscient
{
    public enum ParameterType { Double, Enum, TimeSpan, SystemChannel, SystemEventGenerator, FileName }

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
        public ParameterType Type { get; private set; }
        public static string TypeToString(ParameterType type)
        {
            switch(type)
            {
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
            }
            return "";
        }

        public static ParameterType StringToType(string type)
        {
            switch(type)
            {
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

    public class FileNameParameter : Parameter
    {
        public FileNameParameter(string name) : base(name, ParameterType.FileName) {}

        public override bool Validate()
        {
            if (File.Exists(Value)) return true;
            return false;
        }
    }
}
