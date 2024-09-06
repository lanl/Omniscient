using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omniscient
{
    public class DeclarationTemplate : PersisterWithCustomParameters
    {
        public override string Species { get { return "DeclarationTemplate"; } }

        public DeclarationTemplate(DetectionSystem parent, string name, uint id) : base(parent, name, id) 
        {
            DetectionSystem = parent;
            DetectionSystem.DeclarationTemplate = this;
        }
        public static DeclarationTemplate FromXML(XmlNode node, DetectionSystem system)
        {
            string name;
            uint id;
            Persister.StartFromXML(node, out name, out id);
            DeclarationTemplate dTemplate = new DeclarationTemplate(system, name, id);
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "CustomParameter") CustomParameter.FromXML(childNode, dTemplate);
            }
            return dTemplate;
        }

        public override void ToXML(XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }
        public override void Delete()
        {
            base.Delete();
            DetectionSystem.DeclarationTemplate = null;
        }
    }
}
