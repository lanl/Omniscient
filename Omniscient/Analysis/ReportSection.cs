using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class ReportSection
    {
        public string Title { get; set; }
        public List<Parameter> Parameters { get; set; }

        public ReportSection(string title="", List<Parameter> parameters=null) 
        {
            Title = title;
            Parameters = parameters;
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(Title, 512);

            foreach (Parameter parameter in Parameters)
            {
                switch(parameter.Type)
                {
                    case ParameterType.Int:
                        builder.AppendFormat("\n {0,32}: {1,13}", parameter.Name, parameter.Value);
                        break;
                    case ParameterType.Double:
                        builder.AppendFormat("\n {0,32}: {1,13:F3}", parameter.Name, (parameter as DoubleParameter).ToDouble());
                        break;
                    default:
                        builder.AppendFormat("\n {0,32}: {1}", parameter.Name, parameter.Value);
                        break;
                }
                
            }
            return builder.ToString();
        }
    }

    public class AnalyzerHeaderReportSection : ReportSection
    {
        public AnalyzerHeaderReportSection(Analyzer analyzer) : base()
        {
            Title = analyzer.Name;
            Parameters = new List<Parameter>
            {
                new StringParameter("Site", analyzer.DetectionSystem.ParentFacility.ParentSite.Name),
                new StringParameter("Facility", analyzer.DetectionSystem.ParentFacility.Name),
                new StringParameter("System", analyzer.DetectionSystem.Name)
            };
        }
    }

    public class EventReportSection : ReportSection
    { 
        public EventReportSection(Event eve) : base("Event")
        {
            Parameters = new List<Parameter>
            {
                new StringParameter("Event genrator", eve.GetEventGenerator().Name),
                new StringParameter("Start time", eve.StartTime.ToString("yyyy-MM-dd HH:mm:ss.fff")),
                new StringParameter("End time", eve.EndTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))
            };
        }
    }

    public class DeclarationReportSection : ReportSection
    {
        public DeclarationReportSection(Declaration declaration) : base("Declaration")
        {
            Parameters = new List<Parameter>
            {
                new StringParameter("Item ID", declaration.ItemID)
            };
            foreach (Parameter parameter in declaration.Parameters.Values)
            {
                Parameters.Add(parameter);
            }
        }
    }
}
