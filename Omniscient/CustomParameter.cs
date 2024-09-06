using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omniscient
{
    public abstract class PersisterWithCustomParameters : Persister
    {
        public Dictionary<string, CustomParameter> CustomParameters { get; set; }
        public DetectionSystem DetectionSystem { get; set; }
        protected PersisterWithCustomParameters(Persister parent, string name, uint id) : base(parent, name, id)
        {
            CustomParameters = new Dictionary<string, CustomParameter>();
        }
    }
    public class CustomParameter : Persister
    {
        public override string Species { get { return "CustomParameter"; } }
        public PersisterWithCustomParameters ParentWithCustomParameters { get; }
        public ParameterTemplate Template { get; private set; }
        public Parameter Parameter { get; private set; }
        public bool IsTemporary { get; private set; }
        public CustomParameter(PersisterWithCustomParameters parent, string name, uint id, ParameterTemplate template, Parameter parameter, bool isVariable) : base(parent, name, id)
        {
            ParentWithCustomParameters = parent;
            ParentWithCustomParameters.CustomParameters.Add(name, this);
            Template = template;
            Parameter = parameter;
            IsTemporary = isVariable;
        }

        public static CustomParameter FromXML(XmlNode node, PersisterWithCustomParameters parent)
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
                    param = new SystemChannelParameter(name, parent.DetectionSystem) { Value = value };
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
            return new CustomParameter(parent, name, id, template, param, isVariable);
        }
        public override void ToXML(XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            base.Delete();
            ParentWithCustomParameters.CustomParameters.Remove(this.Name);
        }
    }
}
