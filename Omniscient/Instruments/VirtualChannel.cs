﻿// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2018, Los Alamos National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Los Alamos National Security, LLC. This software was produced under U.S. Government contract DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Los Alamos National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY LOS ALAMOS NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL LOS ALAMOS NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omniscient
{
    /// <summary>
    /// Creates a channel using data from other channels.</summary>
    /// <remarks>Requires channels that have the same number of data points, 
    /// corresponding to the same timestamps.</remarks>
    public abstract class VirtualChannel : Channel
    {
        public static readonly VirtualChannelHookup[] Hookups = new VirtualChannelHookup[]
        {
            new TwoChannelVCHookup(),
            new ScalarOperationVCHookup(),
            new DelayVCHookup(),
            new ConvolveVCHookup(),
            new LocalStatisticVCHookup()
        };

        public enum VirtualChannelType { RATIO, SUM, DIFFERENCE, ADD_CONST, SCALE, DELAY, ROI, CONVOLVE, LOCAL_MAX, LOCAL_MIN}

        public string VCType { get; protected set; }

        public List<Channel> Dependencies { get; protected set; }

        public VirtualChannel(string newName, Instrument parent, ChannelType newType) : base(newName, parent, newType)
        {
            Dependencies = new List<Channel>();
        }

        public abstract List<Parameter> GetParameters();

        public abstract void CalculateValues();

        public static VirtualChannelHookup GetHookup(string type)
        {
            foreach (VirtualChannelHookup hookup in Hookups)
            {
                if (hookup.Type == type)
                {
                    return hookup;
                }
            }
            return null;
        }

        public static VirtualChannel FromXML(XmlNode node, Instrument instrument)
        {
            string name = node.Attributes["Name"]?.InnerText;
            VirtualChannelHookup hookup = GetHookup(node.Attributes["Type"]?.InnerText);
            List<Parameter> parameters = Parameter.FromXML(node, hookup.TemplateParameters, null, instrument);
            return hookup?.FromParameters(instrument, name, parameters);
        }

        public static void ToXML(XmlWriter xmlWriter, VirtualChannel channel)
        {
            xmlWriter.WriteStartElement("VirtualChannel");
            xmlWriter.WriteAttributeString("Name", channel.GetName());
            xmlWriter.WriteAttributeString("Type", channel.VCType);
            List<Parameter> parameters = channel.GetParameters();
            foreach (Parameter param in parameters)
            {
                xmlWriter.WriteAttributeString(param.Name.Replace(' ', '_'), param.Value);
            }
        }
    }

    public abstract class VirtualChannelHookup
    {
        public abstract VirtualChannel FromParameters(Instrument parent, string newName, List<Parameter> parameters);
        public abstract string Type { get; }
        public List<ParameterTemplate> TemplateParameters { get; set; }
    }

    /// <summary>
    /// VirtualChannel performing a common operation on two Channels
    /// </summary>
    public class TwoChannelVC : VirtualChannel
    {
        public enum OperationType { Sum, Difference, Product, Ratio}
        private Channel _channelA = null;
        public Channel ChannelA
        {
            get { return _channelA; }
            set
            {
                for (int i = 0; i < Dependencies.Count; i++)
                {
                    if (Dependencies[i] == _channelA)
                    {
                        Dependencies.Remove(_channelA);
                        break;
                    }
                }
                Dependencies.Add(value);
                _channelA = value;
            }
        }

        private Channel _channelB = null;
        public Channel ChannelB
        {
            get { return _channelB; }
            set
            {
                for (int i = 0; i < Dependencies.Count; i++)
                {
                    if (Dependencies[i] == _channelB)
                    {
                        Dependencies.Remove(_channelB);
                        break;
                    }
                }
                Dependencies.Add(value);
                _channelB = value;
            }
        }

        public OperationType Operation { get; set; }

        public TwoChannelVC(string newName, Instrument parent, ChannelType newType) : base(newName, parent, newType)
        {
            VCType = "Two Channel";
            Operation = OperationType.Sum;
        }

        public override void CalculateValues()
        {
            double[] arrayVals = new double[ChannelA.GetValues().Count];
            if (channelType == ChannelType.DURATION_VALUE)
                durations = ChannelA.GetDurations();
            List<double> A = ChannelA.GetValues();
            List<double> B = ChannelB.GetValues();

            timeStamps = ChannelA.GetTimeStamps();
            
            switch (Operation)
            {
                case OperationType.Sum:
                    for (int i = 0; i < A.Count; i++)
                        arrayVals[i] = A[i] + B[i];
                    break;
                case OperationType.Difference:
                    for (int i = 0; i < A.Count; i++)
                        arrayVals[i] = A[i] - B[i];
                    break;
                case OperationType.Product:
                    for (int i = 0; i < A.Count; i++)
                        arrayVals[i] = A[i] * B[i];
                    break;
                case OperationType.Ratio:
                    for (int i = 0; i < A.Count; i++)
                        if (B[i] == 0)
                            arrayVals[i] = 0;
                        else
                            arrayVals[i] = A[i] / B[i];
                    break;
            }
            values = arrayVals.ToList();
        }

        public override List<Parameter> GetParameters()
        {
            Channel[] channels = instrument.GetChannels();
            int cIndex = -1;
            for (int i=0; i<channels.Length; i++)
            {
                if (channels[i] == this)
                {
                    cIndex = i;
                    break;
                }
            }
            string operation = "";
            switch (Operation)
            {
                case OperationType.Sum:
                    operation = "Sum";
                    break;
                case OperationType.Difference:
                    operation = "Difference";
                    break;
                case OperationType.Product:
                    operation = "Product";
                    break;
                case OperationType.Ratio:
                    operation = "Ratio";
                    break;
            }
            return new List<Parameter>()
            {
                new EnumParameter("Operation")
                {
                    Value = operation,
                    ValidValues = new List<string>(){ "Sum", "Difference", "Product", "Ratio"}
                },
                new InstrumentChannelParameter("Channel A", instrument, cIndex-1)
                {
                    Value = ChannelA.GetName()
                },
                new InstrumentChannelParameter("Channel B", instrument, cIndex-1)
                {
                    Value = ChannelB.GetName()
                }
            };
        }
    }

    public class TwoChannelVCHookup : VirtualChannelHookup
    {
        public TwoChannelVCHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Operation", ParameterType.Enum, new List<string>(){ "Sum", "Difference", "Product", "Ratio" }),
                new ParameterTemplate("Channel A", ParameterType.InstrumentChannel),
                new ParameterTemplate("Channel B", ParameterType.InstrumentChannel)
            };
        }

        public override string Type { get { return "Two Channel"; } }
        public override VirtualChannel FromParameters(Instrument parent, string newName, List<Parameter> parameters)
        {
            Channel channelA = null;
            Channel channelB = null;
            TwoChannelVC.OperationType operation = TwoChannelVC.OperationType.Sum;
            foreach(Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Channel A":
                        channelA = ((InstrumentChannelParameter)param).ToChannel();
                        break;
                    case "Channel B":
                        channelB = ((InstrumentChannelParameter)param).ToChannel();
                        break;
                    case "Operation":
                        switch(param.Value)
                        {
                            case "Sum":
                                operation = TwoChannelVC.OperationType.Sum;
                                break;
                            case "Difference":
                                operation = TwoChannelVC.OperationType.Difference;
                                break;
                            case "Product":
                                operation = TwoChannelVC.OperationType.Product;
                                break;
                            case "Ratio":
                                operation = TwoChannelVC.OperationType.Ratio;
                                break;
                        }
                        break;
                }
            }
            return new TwoChannelVC(newName, parent, channelA.GetChannelType())
            {
                Operation = operation,
                ChannelA = channelA,
                ChannelB = channelB
            };
        }
    }

    /// <summary>
    /// VirtualChannel performing a common operation on a constant scalar and a 
    /// Channel
    /// </summary>
    public class ScalarOperationVC : VirtualChannel
    {
        public enum OperationType { Sum, Product, Power}

        private Channel _channel;
        public Channel Channel
        {
            get { return _channel; }
            set
            {
                Dependencies.Clear();
                Dependencies.Add(value);
                _channel = value;
            }
        }
        public double Constant { get; set; }
        public OperationType Operation { get; set; }

        public ScalarOperationVC(string newName, Instrument parent, ChannelType newType) : base(newName, parent, newType)
        {
            VCType = "Scalar Operation";
            Channel = null;
            Constant = double.NaN;
            Operation = OperationType.Sum;
        }

        public override void CalculateValues()
        {
            double[] arrayVals = new double[Channel.GetValues().Count];
            if (channelType == ChannelType.DURATION_VALUE)
                durations = Channel.GetDurations();
            List<double> A = Channel.GetValues();

            timeStamps = Channel.GetTimeStamps();

            switch (Operation)
            {
                case OperationType.Sum:
                    for (int i = 0; i < A.Count; i++)
                        arrayVals[i] = A[i] + Constant;
                    break;
                case OperationType.Product:
                    for (int i = 0; i < A.Count; i++)
                        arrayVals[i] = A[i] * Constant;
                    break;
                case OperationType.Power:
                    for (int i = 0; i < A.Count; i++)
                        arrayVals[i] = Math.Pow(A[i], Constant);
                    break;
            }
            values = arrayVals.ToList();
        }

        public override List<Parameter> GetParameters()
        {
            Channel[] channels = instrument.GetChannels();
            int cIndex = -1;
            for (int i = 0; i < channels.Length; i++)
            {
                if (channels[i] == this)
                {
                    cIndex = i;
                    break;
                }
            }
            string operation = "";
            switch (Operation)
            {
                case OperationType.Sum:
                    operation = "Sum";
                    break;
                case OperationType.Product:
                    operation = "Product";
                    break;
                case OperationType.Power:
                    operation = "Power";
                    break;
            }
            return new List<Parameter>()
            {
                new EnumParameter("Operation")
                {
                    Value = operation,
                    ValidValues = new List<string>(){ "Sum", "Product", "Power"}
                },
                new InstrumentChannelParameter("Channel", instrument, cIndex-1)
                {
                    Value = Channel.GetName()
                },
                new DoubleParameter("Constant")
                {
                    Value = Constant.ToString()
                }
            };
        }
    }

    public class ScalarOperationVCHookup : VirtualChannelHookup
    {
        public ScalarOperationVCHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Operation", ParameterType.Enum, new List<string>(){ "Sum", "Product", "Power" }),
                new ParameterTemplate("Channel", ParameterType.InstrumentChannel),
                new ParameterTemplate("Constant", ParameterType.Double)
            };
        }

        public override string Type { get { return "Scalar Operation"; } }

        public override VirtualChannel FromParameters(Instrument parent, string newName, List<Parameter> parameters)
        {
            Channel channel = null;
            double constant = double.NaN;
            ScalarOperationVC.OperationType operation = ScalarOperationVC.OperationType.Sum;
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Channel":
                        channel = ((InstrumentChannelParameter)param).ToChannel();
                        break;
                    case "Constant":
                        constant = ((DoubleParameter)param).ToDouble();
                        break;
                    case "Operation":
                        switch (param.Value)
                        {
                            case "Sum":
                                operation = ScalarOperationVC.OperationType.Sum;
                                break;
                            case "Product":
                                operation = ScalarOperationVC.OperationType.Product;
                                break;
                            case "Power":
                                operation = ScalarOperationVC.OperationType.Power;
                                break;
                        }
                        break;
                }
            }
            return new ScalarOperationVC(newName, parent, channel.GetChannelType())
            {
                Operation = operation,
                Channel = channel,
                Constant = constant
            };
        }
    }

    /// <summary>
    /// VirtualChannel that applies a delay on a Channel
    /// </summary>
    public class DelayVC : VirtualChannel
    {
        public DelayVC(string newName, Instrument parent, ChannelType newType) : base(newName, parent, newType)
        {
            VCType = "Delay";
            Channel = null;
            Delay = TimeSpan.FromTicks(0);
        }

        private Channel _channel;
        public Channel Channel
        {
            get { return _channel; }
            set
            {
                Dependencies.Clear();
                Dependencies.Add(value);
                _channel = value;
            }
        }
        public TimeSpan Delay { get; set; }

        public override void CalculateValues()
        {
            values = Channel.GetValues();
            List<DateTime> times = Channel.GetTimeStamps();
            DateTime[] arrayTimeStamps = new DateTime[times.Count];
            for (int i = 0; i < times.Count; i++)
                arrayTimeStamps[i] = times[i].AddTicks(Delay.Ticks);
            timeStamps = arrayTimeStamps.ToList();
        }

        public override List<Parameter> GetParameters()
        {
            Channel[] channels = instrument.GetChannels();
            int cIndex = -1;
            for (int i = 0; i < channels.Length; i++)
            {
                if (channels[i] == this)
                {
                    cIndex = i;
                    break;
                }
            }
            return new List<Parameter>()
            {
                new InstrumentChannelParameter("Channel", instrument, cIndex-1)
                {
                    Value = Channel.GetName()
                },
                new TimeSpanParameter("Delay")
                {
                    Value = Delay.TotalSeconds.ToString()
                }
            };
        }
    }

    public class DelayVCHookup : VirtualChannelHookup
    {
        public DelayVCHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Channel", ParameterType.InstrumentChannel),
                new ParameterTemplate("Delay", ParameterType.TimeSpan)
            };
        }

        public override string Type { get { return "Delay"; } }

        public override VirtualChannel FromParameters(Instrument parent, string newName, List<Parameter> parameters)
        {
            Channel channel = null;
            TimeSpan delay = TimeSpan.FromTicks(0);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Channel":
                        channel = ((InstrumentChannelParameter)param).ToChannel();
                        break;
                    case "Delay":
                        delay = ((TimeSpanParameter)param).ToTimeSpan();
                        break;
                }
            }
            return new DelayVC(newName, parent, channel.GetChannelType())
            {
                Channel = channel,
                Delay = delay
            };
        }
    }

    /// <summary>
    /// VirtualChannel that applies a convolution on a Channel
    /// </summary>
    public class ConvolveVC : VirtualChannel
    {
        public ConvolveVC(string newName, Instrument parent, ChannelType newType) : base(newName, parent, newType)
        {
            VCType = "Convolve";
            Channel = null;
            File = "";
        }

        private Channel _channel;
        public Channel Channel
        {
            get { return _channel; }
            set
            {
                Dependencies.Clear();
                Dependencies.Add(value);
                _channel = value;
            }
        }
        public string File { get; set; }

        public override void CalculateValues()
        {
            if (channelType == ChannelType.DURATION_VALUE)
                durations = Channel.GetDurations();
            timeStamps = Channel.GetTimeStamps();
            double[] arrayVals = SignalProcessor.Convolve(Channel.GetValues().ToArray(), SignalProcessor.FromFile(File));
            values = arrayVals.ToList();
        }

        public override List<Parameter> GetParameters()
        {
            Channel[] channels = instrument.GetChannels();
            int cIndex = -1;
            for (int i = 0; i < channels.Length; i++)
            {
                if (channels[i] == this)
                {
                    cIndex = i;
                    break;
                }
            }
            return new List<Parameter>()
            {
                new InstrumentChannelParameter("Channel", instrument, cIndex-1)
                {
                    Value = Channel.GetName()
                },
                new FileNameParameter("File")
                {
                    Value = File
                }
            };
        }
    }

    public class ConvolveVCHookup : VirtualChannelHookup
    {
        public ConvolveVCHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Channel", ParameterType.InstrumentChannel),
                new ParameterTemplate("File", ParameterType.FileName)
            };
        }

        public override string Type { get { return "Convolve"; } }

        public override VirtualChannel FromParameters(Instrument parent, string newName, List<Parameter> parameters)
        {
            Channel channel = null;
            string file = "";
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Channel":
                        channel = ((InstrumentChannelParameter)param).ToChannel();
                        break;
                    case "File":
                        file = param.Value;
                        break;
                }
            }
            return new ConvolveVC(newName, parent, channel.GetChannelType())
            {
                Channel = channel,
                File = file
            };
        }
    }

    /// <summary>
    /// VirtualChannel that calculates a statistic over a period of a data 
    /// points in a Channel
    /// </summary>
    public class LocalStatisticVC : VirtualChannel
    {
        public enum StatisticType { Max, Min, Average, StandardDeviation }
        private Channel _channel;
        public Channel Channel
        {
            get { return _channel; }
            set
            {
                Dependencies.Clear();
                Dependencies.Add(value);
                _channel = value;
            }
        }
        public int Period { get; set; }
        public StatisticType Statistic { get; set; }

        public LocalStatisticVC(string newName, Instrument parent, ChannelType newType) : base(newName, parent, newType)
        {
            VCType = "Local Statistic";
            Channel = null;
            Period = 0;
            Statistic = StatisticType.Max;
        }

        public override void CalculateValues()
        {
            double[] arrayVals = new double[Channel.GetValues().Count];

            if (channelType == ChannelType.DURATION_VALUE)
                durations = Channel.GetDurations();

            List<double> A = Channel.GetValues();
            timeStamps = Channel.GetTimeStamps();

            switch (Statistic)
            {
                case StatisticType.Max:
                    double max;
                    for (int i = 0; i < A.Count; i++)
                    {
                        max = double.MinValue;
                        for (int j = i; j > i - Period; j--)
                        {
                            if (j < 0) break;
                            if (A[j] > max) max = A[j];
                        }
                        arrayVals[i] = max;
                    }
                    break;
                case StatisticType.Min:
                    double min;
                    for (int i = 0; i < A.Count; i++)
                    {
                        min = double.MaxValue;
                        for (int j = i; j >= i - Period; j--)
                        {
                            if (j < 0) break;
                            if (A[j] < min) min = A[j];
                        }
                        arrayVals[i] = min;
                    }
                    break;
                case StatisticType.Average:
                    double sum = 0;
                    for (int i = 0; i < A.Count; i++)
                    {
                        sum += A[i];
                        if (i<Period)
                        {
                            arrayVals[i] = sum/i;
                        }
                        else
                        {
                            sum -= A[i - Period];
                            arrayVals[i] = sum/Period;
                        }
                    }
                    break;
                case StatisticType.StandardDeviation:
                    double movingSum = 0;
                    double average = 0;
                    double sumSq = 0;
                    for (int i = 0; i < A.Count; i++)
                    {
                        sumSq = 0;
                        movingSum += A[i];
                        if (i < Period)
                        {
                            average = movingSum / i;
                            for (int j = i; j >= 0; j--)
                            {
                                sumSq += (A[i] - average)* (A[i] - average);
                            }
                            arrayVals[i] = Math.Sqrt(sumSq / i);
                        }
                        else
                        {
                            movingSum -= A[i - Period];
                            average = movingSum / Period;
                            for (int j = i; j > i-Period; j--)
                            {
                                sumSq += (A[i] - average) * (A[i] - average);
                            }
                            arrayVals[i] = Math.Sqrt(sumSq / Period);
                        }
                    }
                    break;
            }
            values = arrayVals.ToList();
        }

        public override List<Parameter> GetParameters()
        {
            Channel[] channels = instrument.GetChannels();
            int cIndex = -1;
            for (int i = 0; i < channels.Length; i++)
            {
                if (channels[i] == this)
                {
                    cIndex = i;
                    break;
                }
            }
            string statistic = "";
            switch (Statistic)
            {
                case StatisticType.Max:
                    statistic = "Max";
                    break;
                case StatisticType.Min:
                    statistic = "Min";
                    break;
                case StatisticType.Average:
                    statistic = "Average";
                    break;
                case StatisticType.StandardDeviation:
                    statistic = "Standard Deviation";
                    break;
            }
            return new List<Parameter>()
            {
                new EnumParameter("Statistic")
                {
                    Value = statistic,
                    ValidValues = new List<string>(){ "Max", "Min", "Average", "Standard Deviation" }
                },
                new InstrumentChannelParameter("Channel", instrument, cIndex-1)
                {
                    Value = Channel.GetName()
                },
                new IntParameter("Period") { Value = Period.ToString() }
            };
        }
    }

    public class LocalStatisticVCHookup : VirtualChannelHookup
    {
        public LocalStatisticVCHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Statistic", ParameterType.Enum, new List<string>(){ "Max", "Min", "Average", "Standard Deviation" }),
                new ParameterTemplate("Channel", ParameterType.InstrumentChannel),
                new ParameterTemplate("Period", ParameterType.Int)
            };
        }

        public override string Type { get { return "Local Statistic"; } }
        
        public override VirtualChannel FromParameters(Instrument parent, string newName, List<Parameter> parameters)
        {
            Channel channel = null;
            int period = 0;
            LocalStatisticVC.StatisticType statistic = LocalStatisticVC.StatisticType.Max;
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Channel":
                        channel = ((InstrumentChannelParameter)param).ToChannel();
                        break;
                    case "Period":
                        period = ((IntParameter)param).ToInt();
                        break;
                    case "Statistic":
                        switch (param.Value)
                        {
                            case "Max":
                                statistic = LocalStatisticVC.StatisticType.Max;
                                break;
                            case "Min":
                                statistic = LocalStatisticVC.StatisticType.Min;
                                break;
                            case "Average":
                                statistic = LocalStatisticVC.StatisticType.Average;
                                break;
                            case "Standard Deviation":
                                statistic = LocalStatisticVC.StatisticType.StandardDeviation;
                                break;
                        }
                        break;
                }
            }
            return new LocalStatisticVC(newName, parent, channel.GetChannelType())
            {
                Statistic = statistic,
                Channel = channel,
                Period = period
            };
        }
    }
}
