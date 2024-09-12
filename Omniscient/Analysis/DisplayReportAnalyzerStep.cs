using Omniscient.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using static Omniscient.AnalyzerStep;

namespace Omniscient
{
    public class DisplayReportAnalyzerStep : AnalyzerStep
    {
        bool save;
        public DisplayReportAnalyzerStep(Analyzer analyzer, string name, uint id) : base(analyzer, name, id, AnalyzerStepType.DISPLAY_REPORT)
        {
            save = false;
        }
        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new BoolParameter("Save", save));
            return parameters;
        }
        public override void ApplyParameters(List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Save":
                        save = (param as BoolParameter).ToBool();
                        break;
                }
            }
        }
        public override ReturnCode Run(AnalyzerRunData data)
        {
            
            if (save) 
            {
                string path = ParentAnalyzer.DetectionSystem.GetDataDirectory();
                try
                {
                    // TODO: Give this a unique file name
                    data.Report.ToFile(System.IO.Path.Combine(path, "report.rep"));
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Failed to save report.\n" + ex.Message);
                }
            }
            
            ReportViewer reportViewer = new ReportViewer(data.Report);
            reportViewer.Show();
            
            return ReturnCode.SUCCESS;
        }
    }

    public class DisplayReportAnalyzerStepHookup : AnalyzerStepHookup
    {
        public DisplayReportAnalyzerStepHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
                {
                    new ParameterTemplate("Save", ParameterType.Bool)
                };
        }

        public override string Type { get { return "Display Report"; } }
        public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
        {
            DisplayReportAnalyzerStep step = new DisplayReportAnalyzerStep(parent, newName, id);
            step.ApplyParameters(parameters);
            return step;
        }
    }
}
