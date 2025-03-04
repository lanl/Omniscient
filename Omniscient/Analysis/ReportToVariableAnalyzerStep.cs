﻿using Omniscient.MainDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class ReportToVariableAnalyzerStep : AnalyzerStep
    {
        string reportName;
        string section;
        string paramName;
        string outputParamName;
        ParameterType? outputType;

        public ReportToVariableAnalyzerStep(Analyzer parent, string name, uint id) : base(parent, name, id, "Report to Variable")
        {
            reportName = "";
            section = "";
            paramName = "";
            outputParamName = "";
            outputType = null;
        }
        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new StringParameter("Report Name", reportName));
            parameters.Add(new StringParameter("Section", section));
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
                    case "Report Name":
                        reportName = param.Value;
                        break;
                    case "Section":
                        section = param.Value;
                        break;
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
            if (!data.ImportedReports.ContainsKey(reportName)) return ReturnCode.BAD_INPUT;
            if (!data.ImportedReports[reportName].ContainsKey(section)) return ReturnCode.BAD_INPUT;
            if (!data.ImportedReports[reportName][section].ContainsKey(paramName)) return ReturnCode.BAD_INPUT;

            Parameter outputParam = GetOrMakeVariable(data, outputParamName, outputType);
            if (outputParam is null) return ReturnCode.BAD_INPUT;

            outputParam.Value = data.ImportedReports[reportName][section][paramName];

            return ReturnCode.SUCCESS;
        }
    }

    public class ReportToVariableAnalyzerStepHookup : AnalyzerStepHookup
    {
        public ReportToVariableAnalyzerStepHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
                {
                    new ParameterTemplate("Report Name", ParameterType.String),
                    new ParameterTemplate("Section", ParameterType.String),
                    new ParameterTemplate("Parameter", ParameterType.String),
                    new ParameterTemplate("Output Parameter", ParameterType.String),
                    new ParameterTemplate("Output Type", ParameterType.String),
                };
        }

        public override string Type { get { return "Report to Variable"; } }
        public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
        {
            ReportToVariableAnalyzerStep step = new ReportToVariableAnalyzerStep(parent, newName, id);
            step.ApplyParameters(parameters);
            return step;
        }
    }
}
