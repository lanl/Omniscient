using Omniscient.MainDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class ImportReportAnalyzerStep : AnalyzerStep
    {
        string reportType;
        string reportName;
        public ImportReportAnalyzerStep(Analyzer parent, string name, uint id) : base(parent, name, id, "Import Report")
        {
            reportType = name;
            reportName = "";
        }
        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new StringParameter("Report Type", reportType));
            parameters.Add(new StringParameter("Report Name", reportName));
            return parameters;
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Report Type":
                        reportType = param.Value;
                        break;
                    case "Report Name":
                        reportName = param.Value;
                        break;
                }
            }
        }

        public override ReturnCode Run(AnalyzerRunData data)
        {
            ReportSelector reportSelector = new ReportSelector(ParentAnalyzer.DetectionSystem, reportType);
            if (reportSelector.ShowDialog() != System.Windows.Forms.DialogResult.OK) return ReturnCode.FAIL;
            Dictionary<string,Dictionary<string,string>> report = reportSelector.Report;
            data.ImportedReports.Add(reportName, report);

            return ReturnCode.SUCCESS;
        }
    }

    public class ImportReportAnalyzerStepHookup : AnalyzerStepHookup
    {
        public ImportReportAnalyzerStepHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
                {
                    new ParameterTemplate("Report Type", ParameterType.String),
                    new ParameterTemplate("Report Name", ParameterType.String)
                };
        }

        public override string Type { get { return "Import Report"; } }
        public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
        {
            ImportReportAnalyzerStep step = new ImportReportAnalyzerStep(parent, newName, id);
            step.ApplyParameters(parameters);
            return step;
        }
    }
}
