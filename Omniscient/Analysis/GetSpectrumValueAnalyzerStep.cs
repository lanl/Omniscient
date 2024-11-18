using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Omniscient
{
    class GetSpectrumValueAnalyzerStep : AnalyzerStep
    {
        public enum SpectrumValueType { LiveTime, RealTime}

        string inputSpecParamName;
        SpectrumValueType ValueType;
        string outputParamName;
        ParameterType? outputType;

        public GetSpectrumValueAnalyzerStep(Analyzer analyzer, string name, uint id) : base(analyzer, name, id, "Get Spectrum Value")
        {
            inputSpecParamName = "";
            ValueType = SpectrumValueType.LiveTime;
            outputParamName = "";
            outputType = null;
        }

        public override List<Parameter> GetParameters()
        {
            string valueType = "";
            switch(ValueType)
            {
                case SpectrumValueType.LiveTime:
                    valueType = "Live Time";
                    break;
                case SpectrumValueType.RealTime:
                    valueType = "Real Time";
                    break;
            }

            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new StringParameter("Input Parameter", inputSpecParamName));
            parameters.Add(new EnumParameter("Value") { Value = valueType, ValidValues = new List<string>() { "Live Time", "Real Time" } });
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
                    case "Input Parameter":
                        inputSpecParamName = param.Value;
                        break;
                    case "Output Parameter":
                        outputParamName = param.Value;
                        break;
                    case "Value":
                        switch (param.Value)
                        {
                            case "Live Time":
                                ValueType = SpectrumValueType.LiveTime;
                                break;
                            case "Real Time":
                                ValueType = SpectrumValueType.RealTime;
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

        public override ReturnCode Run(AnalyzerRunData data)
        {
            // Validate parameters
            SpectrumParameter inputSpecParam;
            try
            {
                inputSpecParam = data.CustomParameters[inputSpecParamName].Parameter as SpectrumParameter;
            }
            catch (Exception ex) { return ReturnCode.BAD_INPUT; }

            Parameter outputParam = GetOrMakeVariable(data, outputParamName, outputType);
            if (outputParam is null) return ReturnCode.BAD_INPUT;

            if (!data.CustomParameters.ContainsKey(outputParamName)) return ReturnCode.BAD_INPUT;
            if (outputParam.Type != ParameterType.Double) return ReturnCode.BAD_INPUT;

            double result = 0;
            switch (ValueType)
            {
                case SpectrumValueType.LiveTime:
                    result = inputSpecParam.Spectrum.GetLiveTime();
                    break;
                case SpectrumValueType.RealTime:
                    result = inputSpecParam.Spectrum.GetRealTime();
                    break;
            }

            outputParam.Value = result.ToString();
            
            return ReturnCode.SUCCESS;
        }

    }

    public class GetSpectrumValueAnalyzerStepHookup : AnalyzerStepHookup
    {
        public GetSpectrumValueAnalyzerStepHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
                {
                    new ParameterTemplate("Input Parameter", ParameterType.String),
                    new ParameterTemplate("Value", ParameterType.Enum, new List<string>(){ "Live Time", "Real Time" }),
                    new ParameterTemplate("Output Parameter", ParameterType.String),
                    new ParameterTemplate("Output Type", ParameterType.String),
                };
        }

        public override string Type { get { return "Get Spectrum Value"; } }
        public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
        {
            GetSpectrumValueAnalyzerStep step = new GetSpectrumValueAnalyzerStep(parent, newName, id);
            step.ApplyParameters(parameters);
            return step;
        }
    }
}
