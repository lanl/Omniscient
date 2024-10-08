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
        string outputParam;

        public DeclarationToVariableAnalyzerStep(Analyzer parent, string name, uint id) : base(parent, name, id, "Declaration to Variable")
        {
            paramName = "";
            outputParam = "";
        }
        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new StringParameter("Parameter", paramName));
            parameters.Add(new StringParameter("Output Parameter", outputParam));
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
                        outputParam = param.Value;
                        break;
                }
            }
        }

        public override ReturnCode Run(AnalyzerRunData data)
        {
            if (!data.Declaration.Parameters.ContainsKey(paramName)) return ReturnCode.BAD_INPUT;
            if (!data.CustomParameters.ContainsKey(outputParam)) return ReturnCode.BAD_INPUT;

            data.CustomParameters[outputParam].Parameter.Value = data.Declaration.Parameters[paramName].Value;

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
