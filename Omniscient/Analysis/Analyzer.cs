using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omniscient
{
    public class Analyzer : PersisterWithCustomParameters
    {
        public override string Species { get { return "Analyzer"; } }

        List<AnalyzerStep> steps;

        public Analyzer(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            parent.GetAnalyzers().Add(this);
            DetectionSystem = parent;
            steps = new List<AnalyzerStep>();
        }
        public List<AnalyzerStep> GetSteps() => steps;

        public static Analyzer FromXML(XmlNode node, DetectionSystem system)
        {
            string name;
            uint id;
            Persister.StartFromXML(node, out name, out id);
            Analyzer analyzer = new Analyzer(system, name, id);
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "CustomParameter") CustomParameter.FromXML(childNode, analyzer);
                else if (childNode.Name == "AnalyzerStep") AnalyzerStep.FromXML(childNode, analyzer);
            }
            return analyzer;
        }

        public ReturnCode Run(Event eve)
        {
            foreach (AnalyzerStep step in steps) 
            {
                step.Run(eve, CustomParameters);
            }

            // Delete temporary variables
            string key;
            for (int i = CustomParameters.Count - 1; i >= 0; i--)
            {
                if (CustomParameters.ElementAt(i).Value.IsTemporary)
                {
                    key = CustomParameters.ElementAt(i).Value.Name;
                    CustomParameters.ElementAt(i).Value.Delete();
                    CustomParameters.Remove(key);
                }
            }

            return ReturnCode.SUCCESS;
        }

        public override void ToXML(XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public override bool SetIndex(int index)
        {
            base.SetIndex(index);
            this.DetectionSystem.GetAnalyzers().Remove(this);
            this.DetectionSystem.GetAnalyzers().Insert(index, this);
            return true;
        }
        public override void Delete()
        {
            base.Delete();
            this.DetectionSystem.GetAnalyzers().Remove(this);
        }
    }
}
