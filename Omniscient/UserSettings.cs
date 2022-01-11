using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omniscient
{
    class UserSettings
    {
        bool showLeftPanel = true;
        public bool ShowLeftPanel 
        { 
            get { return showLeftPanel; } 
            set 
            { 
                showLeftPanel = value;
                Save();
            } 
        }

        bool showRightPanel = true;
        public bool ShowRightPanel
        {
            get { return showRightPanel; }
            set
            {
                showRightPanel = value;
                Save();
            }
        }

        bool showEventsPanel = true;
        public bool ShowEventsPanel
        {
            get { return showEventsPanel; }
            set
            {
                showEventsPanel = value;
                Save();
            }
        }

        public bool SuspendSaving { get; set; } = false;

        string settingsFile;

        public UserSettings(string path)
        {
            settingsFile = path;
        }

        public ReturnCode Save()
        {
            if (SuspendSaving) return ReturnCode.SUCCESS;
            return WriteToXML(settingsFile);
        }

        public ReturnCode Reload()
        {
            return LoadFromXML(settingsFile);
        }

        public ReturnCode WriteNew()
        {
            XmlWriter xmlWriter = XmlWriter.Create(settingsFile, new XmlWriterSettings()
            {
                Indent = true,
            });

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Settings");
            xmlWriter.WriteAttributeString("Omniscient_Version", OmniscientCore.VERSION);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return ReturnCode.SUCCESS;
        }

        public ReturnCode LoadFromXML(string fileName)
        {
            if (!File.Exists(fileName)) return ReturnCode.FILE_DOESNT_EXIST;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.Load(fileName);
                XmlNode node = doc.DocumentElement;
                showLeftPanel = node["ShowLeftPanel"].InnerText == "True";
                showRightPanel = node["ShowRightPanel"].InnerText == "True";
                showEventsPanel = node["ShowEventsPanel"].InnerText == "True";
            }
            catch
            {
                return ReturnCode.FAIL;
            }
            return ReturnCode.SUCCESS;
        }
        private ReturnCode WriteToXML(string fileName)
        {
            XmlWriter xmlWriter = XmlWriter.Create(fileName, new XmlWriterSettings()
            {
                Indent = true,
            });

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Settings");
            xmlWriter.WriteAttributeString("Omniscient_Version", OmniscientCore.VERSION);
            xmlWriter.WriteElementString("ShowLeftPanel", ShowLeftPanel ? "True" : "False");
            xmlWriter.WriteElementString("ShowRightPanel", ShowRightPanel ? "True" : "False");
            xmlWriter.WriteElementString("ShowEventsPanel", ShowEventsPanel ? "True" : "False");
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return ReturnCode.SUCCESS;
        }
    }
}
