using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    /// <summary>
    /// Calculates a value from a range of channel data within the event
    /// Operations include count, sum, average, max, min
    /// </summary>
    public class ChannelRangeStatisticAnalyzerStep : AnalyzerStep
    {
        public enum OperationType { Count, Sum, Average, Max, Min, StandardDeviation }
        OperationType Operation { get; set; }
        string channelParamName;
        string outputParamName;
        ParameterType? outputType;

        public ChannelRangeStatisticAnalyzerStep(Analyzer analyzer, string name, uint id) : base(analyzer, name, id, "Channel Range Statistic")
        {
            channelParamName = "";
            outputParamName = "";
            outputType = null;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
            string operation = "";
            switch (Operation)
            {
                case OperationType.Count:
                    operation = "Count";
                    break;
                case OperationType.Sum:
                    operation = "Sum";
                    break;
                case OperationType.Average:
                    operation = "Average";
                    break;
                case OperationType.Max:
                    operation = "Max";
                    break;
                case OperationType.Min:
                    operation = "Min";
                    break;
                case OperationType.StandardDeviation:
                    operation = "StandardDeviation";
                    break;
            }
            parameters.Add(new EnumParameter("Operation") { Value = operation, ValidValues = new List<string>() { "Sum", "Difference", "Product", "Ratio" } });
            parameters.Add(new StringParameter("Channel Parameter", channelParamName));
            parameters.Add(new StringParameter("Output Parameter", outputParamName));
            parameters.Add(new StringParameter("Output Type", outputType?.ToString() ?? ""));
            return parameters;
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Channel Parameter":
                        channelParamName = param.Value;
                        break;
                    case "Output Parameter":
                        outputParamName = param.Value;
                        break;
                    case "Operation":
                        switch (param.Value)
                        {
                            case "Count":
                                Operation = OperationType.Count;
                                break;
                            case "Sum":
                                Operation = OperationType.Sum;
                                break;
                            case "Average":
                                Operation = OperationType.Average;
                                break;
                            case "Max":
                                Operation = OperationType.Max;
                                break;
                            case "Min":
                                Operation = OperationType.Min;
                                break;
                            case "Standard Deviation":
                                Operation = OperationType.StandardDeviation;
                                break;
                        }
                        break;
                    case "Output Type":
                        if (String.IsNullOrWhiteSpace(param.Value))
                            outputType = null;
                        else
                            outputType = Parameter.TypeFromString(param.Value);
                        break;
                }
            }
        }

        private int Count(Event eve, List<DateTime> times, int startIndex)
        {
            int count = 0;
            for (int i = startIndex; i < times.Count; i++)
            {
                if (times[i] > eve.EndTime) break;
                count++;
            }
            return count;
        }
        private double Sum(Event eve, List<DateTime> times, List<double> vals, int startIndex)
        {
            double sum = 0;
            for (int i = startIndex; i < times.Count; i++)
            {
                if (times[i] > eve.EndTime) break;
                sum += vals[i];
            }
            return sum;
        }

        private double Max(Event eve, List<DateTime> times, List<double> vals, int startIndex)
        {
            double max = double.MinValue;
            for (int i = startIndex; i < times.Count; i++)
            {
                if (times[i] > eve.EndTime) break;
                if (vals[i] > max) max = vals[i];
            }
            return max;
        }
        private double Min(Event eve, List<DateTime> times, List<double> vals, int startIndex)
        {
            double min = double.MaxValue;
            for (int i = startIndex; i < times.Count; i++)
            {
                if (times[i] > eve.EndTime) break;
                if (vals[i] < min) min = vals[i];
            }
            return min;
        }
        private double StandardDeviation(Event eve, List<DateTime> times, List<double> vals, int startIndex)
        {
            int count = Count(eve, times, startIndex);
            double average = Sum(eve, times, vals, startIndex) / count;
            double sumSq = 0;
            for (int i = startIndex; i < times.Count; i++)
            {
                if (times[i] > eve.EndTime) break;
                sumSq += (vals[i] - average) * (vals[i] - average);
            }

            return Math.Sqrt(sumSq / (count - 1));
        }

        public override ReturnCode Run(AnalyzerRunData data)
        {
            // Validate parameters
            SystemChannelParameter channelParam;
            Parameter outputParam;
            try
            {
                channelParam = data.CustomParameters[channelParamName].Parameter as SystemChannelParameter;
            }
            catch (Exception ex) { return ReturnCode.BAD_INPUT; }
            
            outputParam = GetOrMakeVariable(data, outputParamName, outputType);
            if (outputParam is null) return ReturnCode.BAD_INPUT;

            if (outputParam.Type != ParameterType.Int &&
                outputParam.Type != ParameterType.Double &&
                outputParam.Type != ParameterType.DoubleWithUncertainty) return ReturnCode.BAD_INPUT;

            bool doUncertainty = false;
            if (outputParam.Type == ParameterType.DoubleWithUncertainty) doUncertainty = true;

            // Load channel and get the times/vals/durations
            Channel channel = channelParam.ToChannel();
            channel.GetInstrument().LoadData(ChannelCompartment.Process, data.Event.StartTime, data.Event.EndTime);
            List<DateTime> times = channel.GetTimeStamps(ChannelCompartment.Process);
            List<double> vals = channel.GetValues(ChannelCompartment.Process);
            List<TimeSpan> durations = null;
            if (channel.GetChannelType() == Channel.ChannelType.DURATION_VALUE)
                durations = channel.GetDurations(ChannelCompartment.Process);

            // Fast forward to start time
            int startIndex = 0;
            while (startIndex < times.Count && times[startIndex] <= data.Event.StartTime) startIndex++;

            int count;
            double sum, stat;
            switch (Operation)
            {
                case OperationType.Count:
                    count = Count(data.Event, times, startIndex);
                    outputParam.Value = count.ToString();
                    if (doUncertainty) outputParam.Value += " +- 0.0";
                    break;
                case OperationType.Sum:
                    sum = Sum(data.Event, times, vals, startIndex);
                    if (outputParam.Type == ParameterType.Int) outputParam.Value = ((int)sum).ToString();
                    else outputParam.Value = sum.ToString();
                    if (doUncertainty) outputParam.Value += " +- 0.0";
                    break;
                case OperationType.Average:
                    count = Count(data.Event, times, startIndex);
                    sum = Sum(data.Event, times, vals, startIndex);
                    double average = sum / count;
                    if (outputParam.Type == ParameterType.Int) outputParam.Value = ((int)average).ToString();
                    else outputParam.Value = average.ToString();
                    if (doUncertainty)
                    {
                        stat = StandardDeviation(data.Event, times, vals, startIndex);
                        count = Count(data.Event, times, startIndex);
                        stat = stat / Math.Sqrt(count);
                        outputParam.Value += " +- " + stat.ToString();
                    }
                    break;
                case OperationType.Max:
                    stat = Max(data.Event, times, vals, startIndex);
                    if (outputParam.Type == ParameterType.Int) outputParam.Value = ((int)stat).ToString();
                    else outputParam.Value = stat.ToString();
                    if (doUncertainty) outputParam.Value += " +- 0.0";
                    break;
                case OperationType.Min:
                    stat = Min(data.Event, times, vals, startIndex);
                    if (outputParam.Type == ParameterType.Int) outputParam.Value = ((int)stat).ToString();
                    else outputParam.Value = stat.ToString();
                    if (doUncertainty) outputParam.Value += " +- 0.0";
                    break;
                case OperationType.StandardDeviation:
                    stat = StandardDeviation(data.Event, times, vals, startIndex);
                    if (outputParam.Type == ParameterType.Int) outputParam.Value = ((int)stat).ToString();
                    else outputParam.Value = stat.ToString();
                    if (doUncertainty) outputParam.Value += " +- 0.0";
                    break;

            }
            return ReturnCode.SUCCESS;
        }
    }
    public class ChannelRangeStatisticAnalyzerStepHookup : AnalyzerStepHookup
    {
        public ChannelRangeStatisticAnalyzerStepHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Operation", ParameterType.Enum, new List<string>(){ "Count", "Sum", "Average", "Max", "Min", "Standard Deviation" }),
                new ParameterTemplate("Channel Parameter", ParameterType.String),
                new ParameterTemplate("Output Parameter", ParameterType.String),
                new ParameterTemplate("Output Type", ParameterType.String)
            };
        }

        public override string Type { get { return "Channel Range Statistic"; } }
        public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
        {
            ChannelRangeStatisticAnalyzerStep step = new ChannelRangeStatisticAnalyzerStep(parent, newName, id);
            step.ApplyParameters(parameters);
            return step;
        }
    }
}
