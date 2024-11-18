using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class DeclarationToVariableAnalyzerStep : AnalyzerStep
    {
        string paramName;
        string outputParamName;
        ParameterType? outputType;

        public DeclarationToVariableAnalyzerStep(Analyzer parent, string name, uint id) : base(parent, name, id, "Declaration to Variable")
        {
            paramName = "";
            outputParamName = "";
            outputType = null;
        }
        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new StringParameter("Parameter", paramName));
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
                    case "Parameter":
                        paramName = param.Value;
                        break;
                    case "Output Parameter":
                        outputParamName = param.Value;
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
            if (!data.Declaration.Parameters.ContainsKey(paramName)) return ReturnCode.BAD_INPUT;

            Parameter outputParam = GetOrMakeVariable(data, outputParamName, outputType);
            if (outputParam is null) return ReturnCode.BAD_INPUT;

            outputParam.Value = data.Declaration.Parameters[paramName].Value;

            return ReturnCode.SUCCESS;
        }
    }

    public class DeclarationToVariableAnalyzerStepHookup : AnalyzerStepHookup
    {
        public DeclarationToVariableAnalyzerStepHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
                {
                    new ParameterTemplate("Parameter", ParameterType.String),
                    new ParameterTemplate("Output Parameter", ParameterType.String),
                    new ParameterTemplate("Output Type", ParameterType.String),
                };
        }

        public override string Type { get { return "Declaration to Variable"; } }
        public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
        {
            DeclarationToVariableAnalyzerStep step = new DeclarationToVariableAnalyzerStep(parent, newName, id);
            step.ApplyParameters(parameters);
            return step;
        }
    }
}
