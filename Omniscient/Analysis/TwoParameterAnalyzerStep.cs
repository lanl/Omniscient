using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    /// <summary>
    /// Combines two AnalyzerParameters through addition, subtration, multiplication, or division
    /// </summary>
    public class TwoParameterAnalyzerStep : AnalyzerStep
    {
        public enum OperationType { Sum, Difference, Product, Ratio, Percent }
        OperationType Operation { get; set; }
        string param1Name;
        string param2Name;
        string outputParamName;
        ParameterType? outputType;

        public TwoParameterAnalyzerStep(Analyzer analyzer, string name, uint id) : base(analyzer, name, id, "Two Parameter")
        {
            param1Name = "";
            param2Name = "";
            outputParamName = "";
            outputType = null;
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
                case OperationType.Percent:
                    operation = "Percent";
                    break;
            }
            parameters.Add(new EnumParameter("Operation") { Value = operation, ValidValues = new List<string>() { "Sum", "Difference", "Product", "Ratio", "Percent" } });
            parameters.Add(new StringParameter("Input Parameter 1", param1Name));
            parameters.Add(new StringParameter("Input Parameter 2", param2Name));
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
                            case "Percent":
                                Operation = OperationType.Percent;
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
            Parameter param1, param2, outputParam;
            try
            {
                param1 = data.CustomParameters[param1Name].Parameter;
                param2 = data.CustomParameters[param2Name].Parameter;
            }
            catch (Exception ex) { return ReturnCode.BAD_INPUT; }
            if (param1.Type != ParameterType.Int && param1.Type != ParameterType.Double && param1.Type != ParameterType.DoubleWithUncertainty) return ReturnCode.BAD_INPUT;
            if (param2.Type != ParameterType.Int && param2.Type != ParameterType.Double && param2.Type != ParameterType.DoubleWithUncertainty) return ReturnCode.BAD_INPUT;

            if (data.CustomParameters.ContainsKey(outputParamName))
            {
                try
                {
                    outputParam = data.CustomParameters[outputParamName].Parameter;
                }
                catch (Exception ex) { return ReturnCode.BAD_INPUT; }
            }
            else
            {
                if (outputType is null) return ReturnCode.BAD_INPUT;
                ParameterTemplate template = new ParameterTemplate(outputParamName, (ParameterType)outputType);
                Parameter param = Parameter.Make(ParentAnalyzer.DetectionSystem, template);
                CustomParameter newVariable = new CustomParameter(ParentAnalyzer, outputParamName, 0, template, param, true);
                outputParam = newVariable.Parameter;
            }

            if (outputParam.Type != ParameterType.Int && outputParam.Type != ParameterType.Double && outputParam.Type != ParameterType.DoubleWithUncertainty) return ReturnCode.BAD_INPUT;

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
                    case OperationType.Percent:
                        resultI = 100 * param1I / param2I;
                        break;
                }
                outputParam.Value = resultI.ToString();
            }
            else
            {
                bool doUncertainty = false;
                if (outputParam.Type == ParameterType.DoubleWithUncertainty) doUncertainty = true;
                double param1D, param2D;
                double param1U = 0;
                double param2U = 0;
                double resultD = 0;
                double resultU = 0;
                if (param1.Type == ParameterType.Int) param1D = (param1 as IntParameter).ToInt();
                else if (param1.Type == ParameterType.Double) param1D = (param1 as DoubleParameter).ToDouble();
                else
                {
                    param1D = (param1 as DoubleWithUncertaintyParameter).DoubleValue();
                    param1U = (param1 as DoubleWithUncertaintyParameter).DoubleUncertainty();
                }
                if (param2.Type == ParameterType.Int) param2D = (param2 as IntParameter).ToInt();
                else if (param2.Type == ParameterType.Double) param2D = (param2 as DoubleParameter).ToDouble();
                else
                {
                    param2D = (param2 as DoubleWithUncertaintyParameter).DoubleValue();
                    param2U = (param2 as DoubleWithUncertaintyParameter).DoubleUncertainty();
                }
                switch (Operation)
                {
                    case OperationType.Sum:
                        resultD = param1D + param2D;
                        if (doUncertainty) resultU = Math.Sqrt(param1U * param1U + param2U * param2U);
                        break;
                    case OperationType.Difference:
                        resultD = param1D - param2D;
                        if (doUncertainty) resultU = Math.Sqrt(param1U * param1U + param2U * param2U);
                        break;
                    case OperationType.Product:
                        resultD = param1D * param2D;
                        if (doUncertainty)
                        {
                            resultU = Math.Abs(resultD) * Math.Sqrt((param1U / param1D) * (param1U / param1D) +
                                                          (param2U / param2D) * (param2U / param2D));
                        }
                        break;
                    case OperationType.Ratio:
                        resultD = param1D / param2D;
                        if (doUncertainty)
                        {
                            resultU = Math.Abs(resultD) * Math.Sqrt((param1U / param1D) * (param1U / param1D) +
                                                          (param2U / param2D) * (param2U / param2D));
                        }
                        break;
                    case OperationType.Percent:
                        resultD = 100.0 * param1D / param2D;
                        if (doUncertainty)
                        {
                            resultU = Math.Abs(resultD) * Math.Sqrt((param1U / param1D) * (param1U / param1D) +
                                                          (param2U / param2D) * (param2U / param2D));
                        }
                        break;
                }
                if (outputParam.Type == ParameterType.Double || outputParam.Type == ParameterType.DoubleWithUncertainty)
                    outputParam.Value = resultD.ToString();
                else
                    outputParam.Value = ((int)resultD).ToString();
                if (doUncertainty) outputParam.Value += " +- " + resultU.ToString();
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
                new ParameterTemplate("Output Parameter", ParameterType.String),
                new ParameterTemplate("Output Type", ParameterType.String)
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
}
