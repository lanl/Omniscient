using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Omniscient
{
    public class Declaration
    {
        public string FileName { get; set; }
        public DeclarationTemplate Template { get; set; }
        public string ItemID { get; set; }
        public Dictionary<string, Parameter> Parameters { get; set; }
        string templateName;
        public Declaration(DetectionSystem system, string tempName="")
        {
            templateName = tempName;
            Template = system.DeclarationTemplate;
            ItemID = "";
            Parameters = new Dictionary<string, Parameter>();
            CustomParameter custParam;
            foreach(KeyValuePair<string, CustomParameter> cpPair in Template.CustomParameters)
            {
                custParam = cpPair.Value;
                Parameters.Add(custParam.Name, Parameter.Make(system, custParam.Template));
            }
        }

        public ReturnCode ToXML(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("Declaration");
            xmlWriter.WriteAttributeString("Template", Template.Name);

            xmlWriter.WriteStartElement("Item_ID");
            xmlWriter.WriteString(ItemID);
            xmlWriter.WriteEndElement();

            foreach (Parameter parameter in Parameters.Values)
            {
                xmlWriter.WriteStartElement("Parameter");
                xmlWriter.WriteAttributeString("Name", parameter.Name);
                xmlWriter.WriteString(parameter.Value);
                xmlWriter.WriteEndElement();
            }

            return ReturnCode.SUCCESS;
        }

        public ReturnCode ToFile(string fileName)
        {
            XmlWriter xmlWriter = XmlWriter.Create(fileName, new XmlWriterSettings()
            {
                Indent = true,
            });

            xmlWriter.WriteStartDocument();
            ToXML(xmlWriter);
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return ReturnCode.SUCCESS;
        }

        public static Declaration FromXML(XmlNode node, DetectionSystem system)
        {
            string templateName = node.Attributes["Template"]?.InnerText;
            Declaration declaration = new Declaration(system, templateName);
            foreach(XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "Item_ID")
                {
                    declaration.ItemID = childNode.InnerText;
                }
                else if (childNode.Name == "Parameter")
                {
                    declaration.Parameters[childNode.Attributes["Name"].InnerText].Value = childNode.InnerText;
                    if(!declaration.Parameters[childNode.Attributes["Name"].InnerText].Validate())
                    {
                        throw new Exception("Declaration parameter failed validation: " +
                            declaration.Parameters[childNode.Attributes["Name"].InnerText].Name + "\n" +
                            declaration.Parameters[childNode.Attributes["Name"].InnerText].Value
                            );
                    }
                }
            }
            return declaration;
        }

        public static Declaration FromFile(string fileName, DetectionSystem system)
        {
            if (!File.Exists(fileName)) throw new FileNotFoundException();
            XmlDocument doc;

            doc = new XmlDocument();
            doc.XmlResolver = null;
            doc.Load(fileName);
            return FromXML(doc.DocumentElement, system);
        }

        public static Dictionary<string, Declaration> FromDirectory(string directory, DetectionSystem system)
        {
            string filePattern = "*.dec";
            Dictionary<string, Declaration> declarations = new Dictionary<string, Declaration>();
            if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory)) return declarations;
            IEnumerable<string> patternFiles = Directory.EnumerateFiles(directory, filePattern, SearchOption.TopDirectoryOnly);
            foreach (string file in patternFiles)
            {
                try
                {
                    Declaration declaration = FromFile(file, system);
                    declarations.Add(declaration.ItemID, declaration);
                }
                catch (Exception ex) { }
            }
            return declarations;
        }
    }
}
