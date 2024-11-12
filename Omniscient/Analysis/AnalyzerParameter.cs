using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omniscient
{
    public class AnalyzerParameter : Persister
    {
        public override string Species { get { return "AnalyzerParameter"; } }
        public Analyzer ParentAnalyzer { get; }
        public ParameterTemplate Template { get; private set; }
        public Parameter Parameter { get; private set; }
        public bool IsVariable { get; private set; }
        public AnalyzerParameter(Analyzer parent, string name, uint id, ParameterTemplate template, Parameter parameter, bool isVariable) : base(parent, name, id)
        {
            parent.GetAnalyzerParameters().Add(name, this);
            ParentAnalyzer = parent;
            Template = template;
            Parameter = parameter;
            IsVariable = isVariable;
        }

        public static AnalyzerParameter FromXML(XmlNode node, Analyzer analyzer)
        {
            string name;
            uint id;
            Persister.StartFromXML(node, out name, out id);
            string type = node.Attributes["Type"]?.InnerText;
            string value = node.Attributes["Value"]?.InnerText;
            bool isVariable = node.Attributes["IsVariable"]?.InnerText == "True";
            ParameterTemplate template;
            Parameter param;
            switch (type)
            {
                case "String":
                    template = new ParameterTemplate(name, ParameterType.String);
                    param = new StringParameter(name, value);
                    break;
                case "Int":
                    template = new ParameterTemplate(name, ParameterType.Int);
                    param = new IntParameter(name) { Value = value };
                    break;
                case "Double":
                    template = new ParameterTemplate(name, ParameterType.Double);
                    param = new DoubleParameter(name) { Value = value };
                    break;
                case "Bool":
                    template = new ParameterTemplate(name, ParameterType.Bool);
                    param = new BoolParameter(name) { Value = value };
                    break;
                case "SystemChannel":
                    template = new ParameterTemplate(name, ParameterType.SystemChannel);
                    param = new SystemChannelParameter(name, analyzer.ParentDetectionSystem) { Value = value };
                    break;
                case "TimeSpan":
                    template = new ParameterTemplate(name, ParameterType.TimeSpan);
                    param = new TimeSpanParameter(name) { Value = value };
                    break;
                case "FileName":
                    template = new ParameterTemplate(name, ParameterType.FileName);
                    param = new FileNameParameter(name) { Value = value };
                    break;
                case "Directory":
                    template = new ParameterTemplate(name, ParameterType.Directory);
                    param = new DirectoryParameter(name) { Value = value };
                    break;
                default:
                    throw new ArgumentException("Invalid AnalyzerParameter type!");
            }
            return new AnalyzerParameter(analyzer, name, id, template, param, isVariable);
        }
        public override void ToXML(XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            base.Delete();
            ParentAnalyzer.GetAnalyzerParameters().Remove(this.Name);
        }
    }
}
