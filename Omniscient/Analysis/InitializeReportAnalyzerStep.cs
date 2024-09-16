using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class InitializeReportAnalyzerStep : AnalyzerStep
    {
        bool hasEvent;
        bool hasDeclaration;
        public InitializeReportAnalyzerStep(Analyzer analyzer, string name, uint id) : base(analyzer, name, id, "Initialize Report")
        {
            hasEvent = false;
            hasDeclaration = false;
        }
        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new BoolParameter("Has Event", hasEvent));
            parameters.Add(new BoolParameter("Has Declaration", hasDeclaration));
            return parameters;
        }
        public override void ApplyParameters(List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Has Event":
                        hasEvent = (param as BoolParameter).ToBool();
                        break;
                    case "Has Declaration":
                        hasDeclaration = (param as BoolParameter).ToBool();
                        break;
                }
            }
        }

        public override ReturnCode Run(AnalyzerRunData data)
        {
            data.Report = new AnalyzerReport(ParentAnalyzer);
            if (hasEvent) data.Report.Sections.Add(new EventReportSection(data.Event));
            if (hasDeclaration) data.Report.Sections.Add(new DeclarationReportSection(data.Declaration));

            return ReturnCode.SUCCESS;
        }
        public class InitializeReportAnalyzerStepHookup : AnalyzerStepHookup
        {
            public InitializeReportAnalyzerStepHookup()
            {
                TemplateParameters = new List<ParameterTemplate>()
                {
                    new ParameterTemplate("Has Event", ParameterType.Bool),
                    new ParameterTemplate("Has Declaration", ParameterType.Bool)
                };
            }

            public override string Type { get { return "Initialize Report"; } }
            public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
            {
                InitializeReportAnalyzerStep step = new InitializeReportAnalyzerStep(parent, newName, id);
                step.ApplyParameters(parameters);
                return step;
            }
        }
    }
}
