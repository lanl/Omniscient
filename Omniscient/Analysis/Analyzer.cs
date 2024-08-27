using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omniscient
{
    public class Analyzer : Persister
    {
        public override string Species { get { return "Analyzer"; } }
        public DetectionSystem ParentDetectionSystem { get; }
        Dictionary<string, AnalyzerParameter> analyzerParameters;
        List<AnalyzerStep> steps;

        public Analyzer(DetectionSystem parent, string name, uint id) : base(parent, name, id)
        {
            parent.GetAnalyzers().Add(this);
            ParentDetectionSystem = parent;
            analyzerParameters = new Dictionary<string, AnalyzerParameter>();
            steps = new List<AnalyzerStep>();
        }

        public Dictionary<string, AnalyzerParameter> GetAnalyzerParameters() => analyzerParameters;
        public List<AnalyzerStep> GetSteps() => steps;

        public static Analyzer FromXML(XmlNode node, DetectionSystem system)
        {
            string name;
            uint id;
            Persister.StartFromXML(node, out name, out id);
            Analyzer analyzer = new Analyzer(system, name, id);
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "AnalyzerParameter") AnalyzerParameter.FromXML(childNode, analyzer);
                else if (childNode.Name == "AnalyzerStep") AnalyzerStep.FromXML(childNode, analyzer);
            }
            return analyzer;
        }

        public ReturnCode Run()
        {
            foreach (AnalyzerStep step in steps) 
            {
                step.Run(analyzerParameters);
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
            ParentDetectionSystem.GetAnalyzers().Remove(this);
            ParentDetectionSystem.GetAnalyzers().Insert(index, this);
            return true;
        }
        public override void Delete()
        {
            base.Delete();
            ParentDetectionSystem.GetAnalyzers().Remove(this);
        }
    }
}
