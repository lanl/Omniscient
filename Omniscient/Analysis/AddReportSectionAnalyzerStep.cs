using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class AddReportSectionAnalyzerStep : AnalyzerStep
    {
        string title;
        string[] parameterNames;

        public AddReportSectionAnalyzerStep(Analyzer analyzer, string name, uint id) : base(analyzer, name, id, AnalyzerStepType.ADD_REPORT_SECTION)
        {
            title = "";
            parameterNames = new string[0];
        }
        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new StringParameter("Title", title));
            parameters.Add(new StringArrayParameter("Parameters", parameterNames));
            return parameters;
        }
        public override void ApplyParameters(List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Title":
                        title = param.Value;
                        break;
                    case "Parameters":
                        parameterNames = (param as StringArrayParameter).ToStringArray();
                        break;
                }
            }
        }
        public override ReturnCode Run(AnalyzerRunData data)
        {
            if (data.Report == null) return ReturnCode.BAD_INPUT;

            List<Parameter> parameters = new List<Parameter>();
            foreach (string name in parameterNames)
            {
                if (!data.CustomParameters.ContainsKey(name)) return ReturnCode.BAD_INPUT;
                parameters.Add(data.CustomParameters[name].Parameter);
            }

            data.Report.Sections.Add(new ReportSection(title, parameters));

            return ReturnCode.SUCCESS;
        }
    }

    public class AddReportSectionAnalyzerStepStepHookup : AnalyzerStepHookup
    {
        public AddReportSectionAnalyzerStepStepHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
                {
                    new ParameterTemplate("Title", ParameterType.String),
                    new ParameterTemplate("Parameters", ParameterType.StringArray)
                };
        }

        public override string Type { get { return "Add Report Section"; } }
        public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
        {
            AddReportSectionAnalyzerStep step = new AddReportSectionAnalyzerStep(parent, newName, id);
            step.ApplyParameters(parameters);
            return step;
        }
    }
}
