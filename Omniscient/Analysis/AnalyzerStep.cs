using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static Omniscient.AnalyzerStep;

namespace Omniscient
{
    /// <summary>
    /// An analyzer runs a series of steps in order to perform an analysis associated with an Event. 
    /// Each type of step inherits from this abstract class
    /// </summary>
    public abstract class AnalyzerStep : Persister
    {
        public override string Species { get { return "AnalyzerStep"; } }
        public static readonly AnalyzerStepHookup[] Hookups = new AnalyzerStepHookup[]
        {
            new CreateVariableAnalyzerStepHookup(),
            new SetEqualAnalyzerStepHookup(),
            new TwoParameterAnalyzerStepHookup(),
            new ChannelRangeStatisticAnalyzerStepHookup(),
            new SumSpectraAnalyzerStepHookup(),
            new AppendStringAnalyzerStepHookup()
        };
        public enum AnalyzerStepType { CREATE_VARIABLE, SET_EQUAL, TWO_PARAMETER, CHANNEL_RANGE_STATISTIC, SUM_SPECTRA, APPEND_STRING }
        public AnalyzerStepType StepType { get; private set; }
        public Analyzer ParentAnalyzer { get; }

        public AnalyzerStep(Analyzer parent, string name, uint id, AnalyzerStepType stepType) : base(parent, name, id)
        {
            parent.GetSteps().Add(this);
            ParentAnalyzer = parent;
            StepType = stepType;
        }

        public abstract ReturnCode Run(Event eve, Dictionary<string, AnalyzerParameter> analysisParams);

        public abstract List<Parameter> GetParameters();
        public abstract void ApplyParameters(List<Parameter> parameters);

        public static AnalyzerStepHookup GetHookup(string type)
        {
            foreach (AnalyzerStepHookup hookup in Hookups)
            {
                if (hookup.Type == type)
                {
                    return hookup;
                }
            }
            return null;
        }
        public static AnalyzerStep FromXML(XmlNode node, Analyzer analyzer)
        {
            string name;
            uint id;
            Persister.StartFromXML(node, out name, out id);
            AnalyzerStepHookup hookup = GetHookup(node.Attributes["Type"]?.InnerText);
            List<Parameter> parameters = Parameter.FromXML(node, hookup.TemplateParameters, analyzer.ParentDetectionSystem);
            return hookup?.FromParameters(analyzer, name, parameters, id);
        }
        public override void ToXML(XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public override bool SetIndex(int index)
        {
            base.SetIndex(index);
            ParentAnalyzer.GetSteps().Remove(this);
            ParentAnalyzer.GetSteps().Insert(index, this);
            return true;
        }
        public override void Delete()
        {
            base.Delete();
            ParentAnalyzer.GetSteps().Remove(this);
        }
    }

    /// <summary>
    /// Hookups facilitate creation of AnalyzerSteps from the UI and XML
    /// </summary>
    public abstract class AnalyzerStepHookup
    {
        public abstract AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id);
        public abstract string Type { get; }
        public List<ParameterTemplate> TemplateParameters { get; set; }
    }

    /// <summary>
    /// Creates a temporary variable that is deleted after an analyzer finishes running.
    /// </summary>
    public class CreateVariableAnalyzerStep : AnalyzerStep
    {
        string variableName;
        ParameterType variableType;

        public CreateVariableAnalyzerStep(Analyzer analyzer, string name, uint id) : base(analyzer, name, id, AnalyzerStepType.CREATE_VARIABLE)
        {
            variableName = "";
            variableType = ParameterType.Double;
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new StringParameter("Variable Name", variableName));
            parameters.Add(new StringParameter("Variable Type", variableType.ToString()));
            return parameters;
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Variable Name":
                        variableName = param.Value;
                        break;
                    case "Variable Type":
                        variableType = Parameter.TypeFromString(param.Value);
                        break;
                }
            }
        }
        public override ReturnCode Run(Event eve, Dictionary<string, AnalyzerParameter> analysisParams)
        {
            ParameterTemplate template = new ParameterTemplate(variableName, variableType);
            Parameter param = Parameter.Make(ParentAnalyzer.ParentDetectionSystem, template);
            AnalyzerParameter newVariable = new AnalyzerParameter(ParentAnalyzer, variableName, 0, template, param, true);

            return ReturnCode.SUCCESS;
        }
    }

    public class CreateVariableAnalyzerStepHookup : AnalyzerStepHookup
    {
        public CreateVariableAnalyzerStepHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Variable Name", ParameterType.String),
                new ParameterTemplate("Variable Type", ParameterType.String)
            };
        }

        public override string Type { get { return "Create Variable"; } }
        public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
        {
            CreateVariableAnalyzerStep step = new CreateVariableAnalyzerStep(parent, newName, id);
            step.ApplyParameters(parameters);
            return step;
        }
    }

    /// <summary>
    /// Sets one parameter to the value of another.
    /// </summary>
    public class SetEqualAnalyzerStep : AnalyzerStep
    {
        string inputParamName;
        string outputParamName;

        public SetEqualAnalyzerStep(Analyzer analyzer, string name, uint id) : base(analyzer, name, id, AnalyzerStepType.SET_EQUAL)
        {
            inputParamName = "";
            outputParamName = "";
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new StringParameter("Input Parameter", inputParamName));
            parameters.Add(new StringParameter("Output Parameter", outputParamName));
            return parameters;
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Input Parameter":
                        inputParamName = param.Value;
                        break;
                    case "Output Parameter":
                        outputParamName = param.Value;
                        break;
                }
            }
        }

        public override ReturnCode Run(Event eve, Dictionary<string, AnalyzerParameter> analysisParams)
        {
            // Validate parameters
            Parameter inputParam, outputParam;
            try
            {
                inputParam = analysisParams[inputParamName].Parameter;
                outputParam = analysisParams[outputParamName].Parameter;
            }
            catch (Exception ex) { return ReturnCode.BAD_INPUT; }
            if ((outputParam.Type == ParameterType.Int || outputParam.Type == ParameterType.Double) &&
                !(outputParam.Type == ParameterType.Int || outputParam.Type == ParameterType.Double)) return ReturnCode.BAD_INPUT;
            else if (outputParam.Type == ParameterType.String) { } // Anything can be set to a string
            else if (outputParam.Type != inputParam.Type) return ReturnCode.BAD_INPUT; // Everything else should be like-to-like

            if (outputParam.Type == ParameterType.Int && inputParam.Type == ParameterType.Double)
            {
                double inputD = (inputParam as DoubleParameter).ToDouble();
                outputParam.Value = ((int)inputD).ToString();
            }
            else
            {
                outputParam.Value = inputParam.Value;
            }
            return ReturnCode.SUCCESS;
        }
    }
    public class SetEqualAnalyzerStepHookup : AnalyzerStepHookup
    {
        public SetEqualAnalyzerStepHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Input Parameter", ParameterType.String),
                new ParameterTemplate("Output Parameter", ParameterType.String)
            };
        }

        public override string Type { get { return "Set Equal"; } }
        public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
        {
            SetEqualAnalyzerStep step = new SetEqualAnalyzerStep(parent, newName, id);
            step.ApplyParameters(parameters);
            return step;
        }
    }

    /// <summary>
    /// Combines two AnalyzerParameters through addition, subtration, multiplication, or division
    /// </summary>
    public class TwoParameterAnalyzerStep : AnalyzerStep
    {
        public enum OperationType { Sum, Difference, Product, Ratio }
        OperationType Operation { get; set; }
        string param1Name;
        string param2Name;
        string outputParamName;

        public TwoParameterAnalyzerStep(Analyzer analyzer, string name, uint id) : base(analyzer, name, id, AnalyzerStepType.TWO_PARAMETER)
        {
            param1Name = "";
            param2Name = "";
            outputParamName = "";
        }
        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
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
            parameters.Add(new EnumParameter("Operation") { Value = operation, ValidValues = new List<string>() { "Sum", "Difference", "Product", "Ratio" } });
            parameters.Add(new StringParameter("Input Parameter 1", param1Name));
            parameters.Add(new StringParameter("Input Parameter 2", param2Name));
            parameters.Add(new StringParameter("Output Parameter", outputParamName));
            return parameters;
        }
        public override void ApplyParameters(List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Input Parameter 1":
                        param1Name = param.Value;
                        break;
                    case "Input Parameter 2":
                        param2Name = param.Value;
                        break;
                    case "Output Parameter":
                        outputParamName = param.Value;
                        break;
                    case "Operation":
                        switch (param.Value)
                        {
                            case "Sum":
                                Operation = OperationType.Sum;
                                break;
                            case "Difference":
                                Operation = OperationType.Difference;
                                break;
                            case "Product":
                                Operation = OperationType.Product;
                                break;
                            case "Ratio":
                                Operation = OperationType.Ratio;
                                break;
                        }
                        break;
                }
            }
        }

        public override ReturnCode Run(Event eve, Dictionary<string, AnalyzerParameter> analysisParams)
        {
            Parameter param1, param2, outputParam;
            try
            {
                param1 = analysisParams[param1Name].Parameter;
                param2 = analysisParams[param2Name].Parameter;
                outputParam = analysisParams[outputParamName].Parameter;
            }
            catch (Exception ex) { return ReturnCode.BAD_INPUT; }
            if (param1.Type != ParameterType.Int && param1.Type != ParameterType.Double) return ReturnCode.BAD_INPUT;
            if (param2.Type != ParameterType.Int && param2.Type != ParameterType.Double) return ReturnCode.BAD_INPUT;
            if (outputParam.Type != ParameterType.Int && outputParam.Type != ParameterType.Double) return ReturnCode.BAD_INPUT;

            if (outputParam.Type == ParameterType.Int && param1.Type == ParameterType.Int && param2.Type == ParameterType.Int)
            {
                int param1I, param2I;
                int resultI = 0;
                param1I = (param1 as IntParameter).ToInt();
                param2I = (param2 as IntParameter).ToInt();
                switch (Operation)
                {
                    case OperationType.Sum:
                        resultI = param1I + param2I;
                        break;
                    case OperationType.Difference:
                        resultI = param1I - param2I;
                        break;
                    case OperationType.Product:
                        resultI = param1I * param2I;
                        break;
                    case OperationType.Ratio:
                        resultI = param1I / param2I;
                        break;
                }
                outputParam.Value = resultI.ToString();
            }
            else
            {
                double param1D, param2D;
                double resultD = 0;
                if (param1.Type == ParameterType.Int) param1D = (param1 as IntParameter).ToInt();
                else param1D = (param1 as DoubleParameter).ToDouble();
                if (param2.Type == ParameterType.Int) param2D = (param2 as IntParameter).ToInt();
                else param2D = (param2 as DoubleParameter).ToDouble();
                switch (Operation)
                {
                    case OperationType.Sum:
                        resultD = param1D + param2D;
                        break;
                    case OperationType.Difference:
                        resultD = param1D - param2D;
                        break;
                    case OperationType.Product:
                        resultD = param1D * param2D;
                        break;
                    case OperationType.Ratio:
                        resultD = param1D / param2D;
                        break;
                }
                if (outputParam.Type == ParameterType.Double)
                    outputParam.Value = resultD.ToString();
                else
                    outputParam.Value = ((int)resultD).ToString();
            }
            return ReturnCode.SUCCESS;
        }
    }

    public class TwoParameterAnalyzerStepHookup : AnalyzerStepHookup
    {
        public TwoParameterAnalyzerStepHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Operation", ParameterType.Enum, new List<string>(){ "Sum", "Difference", "Product", "Ratio" }),
                new ParameterTemplate("Input Parameter 1", ParameterType.String),
                new ParameterTemplate("Input Parameter 2", ParameterType.String),
                new ParameterTemplate("Output Parameter", ParameterType.String)
            };
        }

        public override string Type { get { return "Two Parameter"; } }
        public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
        {
            TwoParameterAnalyzerStep step = new TwoParameterAnalyzerStep(parent, newName, id);
            step.ApplyParameters(parameters);
            return step;
        }
    }

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

        public ChannelRangeStatisticAnalyzerStep(Analyzer analyzer, string name, uint id) : base(analyzer, name, id, AnalyzerStepType.CHANNEL_RANGE_STATISTIC)
        {
            channelParamName = "";
            outputParamName = "";
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

            return Math.Sqrt(sumSq/count);
        }

        public override ReturnCode Run(Event eve, Dictionary<string, AnalyzerParameter> analysisParams)
        {
            // Validate parameters
            SystemChannelParameter channelParam;
            Parameter outputParam;
            try
            {
                channelParam = analysisParams[channelParamName].Parameter as SystemChannelParameter;
                outputParam = analysisParams[outputParamName].Parameter;
            }
            catch (Exception ex) { return ReturnCode.BAD_INPUT; }
            if (outputParam.Type != ParameterType.Int && outputParam.Type != ParameterType.Double) return ReturnCode.BAD_INPUT;

            // Load channel and get the times/vals/durations
            Channel channel = channelParam.ToChannel();
            channel.GetInstrument().LoadData(ChannelCompartment.Process, eve.StartTime, eve.EndTime);
            List<DateTime> times = channel.GetTimeStamps(ChannelCompartment.Process);
            List<double> vals = channel.GetValues(ChannelCompartment.Process);
            List<TimeSpan> durations = null;
            if (channel.GetChannelType() == Channel.ChannelType.DURATION_VALUE)
                durations = channel.GetDurations(ChannelCompartment.Process);

            // Fast forward to start time
            int startIndex = 0;
            while (startIndex < times.Count && times[startIndex] <= eve.StartTime) startIndex++;

            int count;
            double sum, stat;
            switch (Operation)
            {
                case OperationType.Count:
                    count = Count(eve, times, startIndex);
                    outputParam.Value = count.ToString();
                    break;
                case OperationType.Sum:
                    sum = Sum(eve, times, vals, startIndex);
                    if (outputParam.Type == ParameterType.Int) outputParam.Value = ((int)sum).ToString();
                    else outputParam.Value = sum.ToString();
                    break;
                case OperationType.Average:
                    count = Count(eve, times, startIndex);
                    sum = Sum(eve, times, vals, startIndex);
                    double average = sum / count;
                    if (outputParam.Type == ParameterType.Int) outputParam.Value = ((int)average).ToString();
                    else outputParam.Value = average.ToString();
                    break;
                case OperationType.Max:
                    stat = Max(eve, times, vals, startIndex);
                    if (outputParam.Type == ParameterType.Int) outputParam.Value = ((int)stat).ToString();
                    else outputParam.Value = stat.ToString();
                    break;
                case OperationType.Min:
                    stat = Min(eve, times, vals, startIndex);
                    if (outputParam.Type == ParameterType.Int) outputParam.Value = ((int)stat).ToString();
                    else outputParam.Value = stat.ToString();
                    break;
                case OperationType.StandardDeviation:
                    stat = StandardDeviation(eve, times, vals, startIndex);
                    if (outputParam.Type == ParameterType.Int) outputParam.Value = ((int)stat).ToString();
                    else outputParam.Value = stat.ToString();
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
                new ParameterTemplate("Output Parameter", ParameterType.String)
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

    /// <summary>
    /// Sums all of the spectra within the event and stores it in a spectrum variable
    /// </summary>
    public class SumSpectraAnalyzerStep : AnalyzerStep
    {
        string channelParamName;
        string outputParamName;

        public SumSpectraAnalyzerStep(Analyzer analyzer, string name, uint id) : base(analyzer, name, id, AnalyzerStepType.CHANNEL_RANGE_STATISTIC)
        {
            channelParamName = "";
            outputParamName = "";
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new StringParameter("Channel Parameter", channelParamName));
            parameters.Add(new StringParameter("Output Parameter", outputParamName));
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
                }
            }
        }

        public override ReturnCode Run(Event eve, Dictionary<string, AnalyzerParameter> analysisParams)
        {
            // Validate parameters
            SystemChannelParameter channelParam;
            SpectrumParameter specParam;
            try
            {
                channelParam = analysisParams[channelParamName].Parameter as SystemChannelParameter;
                specParam = analysisParams[outputParamName].Parameter as SpectrumParameter;
            }
            catch (Exception ex) { return ReturnCode.BAD_INPUT; }
            Channel chan = channelParam.ToChannel();
            if (chan.GetInstrument().GetInstrumentType() != "MCA") return ReturnCode.BAD_INPUT;

            // Collect spectra in the channel during the event
            MCAInstrument inst = chan.GetInstrument() as MCAInstrument;
            inst.ClearData(ChannelCompartment.Process);
            inst.LoadData(ChannelCompartment.Process, eve.StartTime, eve.EndTime);
            List<Spectrum> spectra = new List<Spectrum>();
            List<DateTime> timeStamps = chan.GetTimeStamps(ChannelCompartment.Process);
            List<TimeSpan> durations = chan.GetDurations(ChannelCompartment.Process);
            List<DataFile> dataFiles = chan.GetFiles(ChannelCompartment.Process);
            List<string> files = new List<string>();
            inst.ClearData(ChannelCompartment.Process);
            for (int meas = 0; meas < timeStamps.Count(); meas++)
            {
                if (timeStamps[meas] >= eve.StartTime &&
                    timeStamps[meas] + durations[meas] <= eve.EndTime)
                {
                    if (!files.Contains(dataFiles[meas].FileName))
                    {
                        files.Add(dataFiles[meas].FileName);
                        inst.IngestFile(ChannelCompartment.Process, dataFiles[meas].FileName);
                        spectra.Add(inst.SpectrumParser.GetSpectrum());
                    }
                }
            }

            // Sum spectra and store in output
            specParam.Spectrum = Spectrum.Sum(spectra);
            return ReturnCode.SUCCESS;
        }
    }

    public class SumSpectraAnalyzerStepHookup : AnalyzerStepHookup
    {
        public SumSpectraAnalyzerStepHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Channel Parameter", ParameterType.String),
                new ParameterTemplate("Output Parameter", ParameterType.String)
            };
        }

        public override string Type { get { return "Sum Spectra"; } }
        public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
        {
            SumSpectraAnalyzerStep step = new SumSpectraAnalyzerStep(parent, newName, id);
            step.ApplyParameters(parameters);
            return step;
        }
    }

    /// <summary>
    /// Appends a string to a string parameter's value
    /// Values of other parameters can be inserted in the appended string by placing the other parameter names in {} in the string
    /// </summary>
    public class AppendStringAnalyzerStep : AnalyzerStep
    {
        string inputParamName;
        string stringParam;

        public AppendStringAnalyzerStep(Analyzer analyzer, string name, uint id) : base(analyzer, name, id, AnalyzerStepType.APPEND_STRING)
        {
            inputParamName = "";
            stringParam = "";
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new StringParameter("Input Parameter", inputParamName));
            parameters.Add(new StringParameter("String", stringParam));
            return parameters;
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Input Parameter":
                        inputParamName = param.Value;
                        break;
                    case "String":
                        stringParam = param.Value;
                        break;
                }
            }
        }

        public override ReturnCode Run(Event eve, Dictionary<string, AnalyzerParameter> analysisParams)
        {
            // Validate parameters
            Parameter inputParam;
            try
            {
                inputParam = analysisParams[inputParamName].Parameter;
            }
            catch (Exception ex) { return ReturnCode.BAD_INPUT; }
            if (inputParam.Type != ParameterType.String) return ReturnCode.BAD_INPUT;

            string result = stringParam;

            int startIndex = result.IndexOf('{');
            int endIndex;
            string paramName;
            Parameter subParam;
            string subParamVal;
            while (startIndex > 0 )
            {
                endIndex = result.IndexOf('}');
                if (endIndex == -1) break;
                paramName = result.Substring(startIndex+1, endIndex - startIndex - 1);
                try
                {
                    subParam = analysisParams[paramName].Parameter;
                    subParamVal = subParam.Value;
                }
                catch 
                { 
                    subParamVal = "INVALID"; 
                }
                result = result.Replace("{" + paramName + "}", subParamVal);
                startIndex = result.IndexOf('{');
            }

            inputParam.Value += result;

            return ReturnCode.SUCCESS;
        }
    }

    public class AppendStringAnalyzerStepHookup : AnalyzerStepHookup
    {
        public AppendStringAnalyzerStepHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Input Parameter", ParameterType.String),
                new ParameterTemplate("String", ParameterType.String)
            };
        }

        public override string Type { get { return "Append String"; } }
        public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
        {
            AppendStringAnalyzerStep step = new AppendStringAnalyzerStep(parent, newName, id);
            step.ApplyParameters(parameters);
            return step;
        }
    }
}