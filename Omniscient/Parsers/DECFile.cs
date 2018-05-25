// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2018, Los Alamos National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Los Alamos National Security, LLC. This software was produced under U.S. Government contract DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Los Alamos National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY LOS ALAMOS NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL LOS ALAMOS NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omniscient
{
    public class DECFile
    {
        private string facility;
        public string Facility { get { return facility; } set { facility = value; } }

        private string mba;
        public string MBA { get { return mba; } set { mba = value; } }

        private DateTime fromTime;
        public DateTime FromTime { get { return fromTime; } set { fromTime = value; } }

        private DateTime toTime;
        public DateTime ToTime { get { return toTime; } set { toTime = value; } }

        private string itemName;
        public string ItemName { get { return itemName; } set { itemName = value; } }

        private DateTime itemOriginDate;
        public DateTime ItemOriginDate { get { return itemOriginDate; } set { itemOriginDate = value; } }

        private string fullName;
        public string FullName { get { return fullName; } set { fullName = value; } }

        private string barcode;
        public string Barcode { get { return barcode; } set { barcode = value; } }

        private DateTime massDate;
        public DateTime MassDate { get { return massDate; } set { massDate = value; } }

        private double mass;
        public double Mass { get { return mass; } set { mass = value; } }

        private string batchName;
        public string BatchName { get { return batchName; } set { batchName = value; } }

        private string batchSource;
        public string BatchSource { get { return batchSource; } set { batchSource = value; } }

        public NuclearComposition Material;

        private DateTime creationDate;
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        private DateTime modificationDate;
        public DateTime ModificationDate { get { return modificationDate; } set { modificationDate = value; } }

        private string note;
        public string Note { get { return note; } set { note = value; } }

        public DECFile()
        {
            facility = "";
            mba = "";
            fromTime = new DateTime(1942, 12, 2);
            toTime = new DateTime(1942, 12, 2);
            itemName = "";
            itemOriginDate = new DateTime(1942, 12, 2);
            fullName = "";
            barcode = "";
            massDate = new DateTime(1942, 12, 2);
            mass = 0.0;
            batchName = "";
            batchSource = "";
            Material = new NuclearComposition();
            creationDate = new DateTime(1942, 12, 2);
            modificationDate = new DateTime(1942, 12, 2);
            note = "";
        }

        private ReturnCode ParseUraniumNode(XmlNode uNode)
        {
            foreach (XmlNode childNode in uNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "U234":
                        try
                        {
                            Material.U234MassPercent.Value = double.Parse(childNode["WeightPercent"].InnerText);
                            Material.U234MassPercent.Uncertainty = double.Parse(childNode["Uncertainty"].InnerText);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                    case "U235":
                        try
                        {
                            Material.U235MassPercent.Value = double.Parse(childNode["WeightPercent"].InnerText);
                            Material.U235MassPercent.Uncertainty = double.Parse(childNode["Uncertainty"].InnerText);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                    case "U236":
                        try
                        {
                            Material.U236MassPercent.Value = double.Parse(childNode["WeightPercent"].InnerText);
                            Material.U236MassPercent.Uncertainty = double.Parse(childNode["Uncertainty"].InnerText);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                    case "U238":
                        try
                        {
                            Material.U238MassPercent.Value = double.Parse(childNode["WeightPercent"].InnerText);
                            Material.U238MassPercent.Uncertainty = double.Parse(childNode["Uncertainty"].InnerText);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                }
            }
            return ReturnCode.SUCCESS;
        }

        private ReturnCode ParsePlutoniumNode(XmlNode puNode)
        {
            foreach (XmlNode childNode in puNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Date":
                        Material.PuDate = DateTime.Parse(childNode.InnerText);
                        break;
                    case "Pu238":
                        try
                        {
                            Material.Pu238MassPercent.Value = double.Parse(childNode["WeightPercent"].InnerText);
                            Material.Pu238MassPercent.Uncertainty = double.Parse(childNode["Uncertainty"].InnerText);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                    case "Pu239":
                        try
                        {
                            Material.Pu239MassPercent.Value = double.Parse(childNode["WeightPercent"].InnerText);
                            Material.Pu239MassPercent.Uncertainty = double.Parse(childNode["Uncertainty"].InnerText);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                    case "Pu240":
                        try
                        {
                            Material.Pu240MassPercent.Value = double.Parse(childNode["WeightPercent"].InnerText);
                            Material.Pu240MassPercent.Uncertainty = double.Parse(childNode["Uncertainty"].InnerText);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                    case "Pu241":
                        try
                        {
                            Material.Pu241MassPercent.Value = double.Parse(childNode["WeightPercent"].InnerText);
                            Material.Pu241MassPercent.Uncertainty = double.Parse(childNode["Uncertainty"].InnerText);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                    case "Pu242":
                        try
                        {
                            Material.Pu242MassPercent.Value = double.Parse(childNode["WeightPercent"].InnerText);
                            Material.Pu242MassPercent.Uncertainty = double.Parse(childNode["Uncertainty"].InnerText);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                }
            }
            return ReturnCode.SUCCESS;
        }

        private ReturnCode ParseAmericiumNode(XmlNode amNode)
        {
            foreach (XmlNode childNode in amNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Date":
                        Material.AmDate = DateTime.Parse(childNode.InnerText);
                        break;
                    case "Am241":
                        try
                        {
                            Material.Am241MassPercent.Value = double.Parse(childNode["WeightPercent"].InnerText);
                            Material.Am241MassPercent.Uncertainty = double.Parse(childNode["Uncertainty"].InnerText);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                }
            }
            return ReturnCode.SUCCESS;
        }

        private ReturnCode ParseBatchNode(XmlNode batchNode)
        {
            ReturnCode returnCode;
            foreach (XmlNode childNode in batchNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Name":
                        batchName = childNode.InnerText;
                        break;
                    case "Source":
                        batchSource = childNode.InnerText;
                        break;
                    case "Americium":
                        returnCode = ParseAmericiumNode(childNode);
                        if (returnCode != ReturnCode.SUCCESS) return returnCode;
                        break;
                    case "Plutonium":
                        returnCode = ParsePlutoniumNode(childNode);
                        if (returnCode != ReturnCode.SUCCESS) return returnCode;
                        break;
                    case "Uranium":
                        returnCode = ParseUraniumNode(childNode);
                        if (returnCode != ReturnCode.SUCCESS) return returnCode;
                        break;
                }
            }
            return ReturnCode.SUCCESS;
        }

        private ReturnCode ParseItemNode(XmlNode itemNode)
        {
            foreach (XmlNode childNode in itemNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Name":
                        itemName = childNode.InnerText;
                        break;
                    case "OriginatingDate":
                        itemOriginDate = DateTime.Parse(childNode.InnerText);
                        break;
                    case "FullName":
                        fullName = childNode.InnerText;
                        break;
                    case "Barcode":
                        barcode = childNode.InnerText;
                        break;
                    case "Mass":
                        massDate = DateTime.Parse(childNode["Date"].InnerText);
                        try
                        {
                            mass = Double.Parse(childNode["Grams"].InnerText);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                    case "Batch":
                        ReturnCode returnCode = ParseBatchNode(childNode);
                        if (returnCode != ReturnCode.SUCCESS) return returnCode;
                        break;
                    default:
                        return ReturnCode.CORRUPTED_FILE;
                }
            }
            return ReturnCode.SUCCESS;
        }

        public ReturnCode ParseDeclarationFile(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            XmlNode declarationNode = doc["Declaration"];
            foreach(XmlNode childNode in declarationNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Facility":
                        facility = childNode.InnerText;
                        break;
                    case "MBA":
                        mba = childNode.InnerText;
                        break;
                    case "TimeRange":
                        try
                        {
                            fromTime = DateTime.Parse(childNode["From"].InnerText);
                            toTime = DateTime.Parse(childNode["To"].InnerText);
                        }
                        catch
                        {
                            return ReturnCode.CORRUPTED_FILE;
                        }
                        break;
                    case "Item":
                        ReturnCode returnCode = ParseItemNode(childNode);
                        if (returnCode != ReturnCode.SUCCESS) return returnCode;
                        break;
                    case "CreationDate":
                        creationDate = DateTime.Parse(childNode.InnerText);
                        break;
                    case "ModificationDate":
                        modificationDate = DateTime.Parse(childNode.InnerText);
                        break;
                    case "Note":
                        note = childNode.InnerText;
                        break;
                    default:
                        return ReturnCode.CORRUPTED_FILE;
                }
            }
            return ReturnCode.SUCCESS;
        }

        public ReturnCode WriteDeclarationFile(string fileName)
        {
            XmlWriter xmlWriter = XmlWriter.Create(fileName, new XmlWriterSettings()
            {
                Indent = true,
            });
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Declaration");

            xmlWriter.WriteStartElement("Facility");
            xmlWriter.WriteString(facility);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MBA");
            xmlWriter.WriteString(mba);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("TimeRange");
            xmlWriter.WriteStartElement("From");
            xmlWriter.WriteString(fromTime.ToString("yyyy-MM-ddTHH:mm:ss"));
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("To");
            xmlWriter.WriteString(toTime.ToString("yyyy-MM-ddTHH:mm:ss"));
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Item");

            xmlWriter.WriteStartElement("Name");
            xmlWriter.WriteString(itemName);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("OriginatingDate");
            xmlWriter.WriteString(itemOriginDate.ToString("yyyy-MM-ddTHH:mm:ss"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("FullName");
            xmlWriter.WriteString(fullName);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Barcode");
            xmlWriter.WriteString(barcode);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Mass");
            xmlWriter.WriteStartElement("Date");
            xmlWriter.WriteString(massDate.ToString("yyyy-MM-ddTHH:mm:ss"));
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("Grams");
            xmlWriter.WriteString(mass.ToString());
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Batch");

            xmlWriter.WriteStartElement("Name");
            xmlWriter.WriteString(batchName);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Source");
            xmlWriter.WriteString(batchSource);
            xmlWriter.WriteEndElement();

            if (Material.U235MassPercent.IsSet())
            {
                xmlWriter.WriteStartElement("Uranium");
                xmlWriter.WriteStartElement("U234");
                xmlWriter.WriteStartElement("WeightPercent");
                xmlWriter.WriteString(Material.U234MassPercent.Value.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Uncertainty");
                xmlWriter.WriteString(Material.U234MassPercent.Uncertainty.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("U235");
                xmlWriter.WriteStartElement("WeightPercent");
                xmlWriter.WriteString(Material.U235MassPercent.Value.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Uncertainty");
                xmlWriter.WriteString(Material.U235MassPercent.Uncertainty.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("U236");
                xmlWriter.WriteStartElement("WeightPercent");
                xmlWriter.WriteString(Material.U236MassPercent.Value.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Uncertainty");
                xmlWriter.WriteString(Material.U236MassPercent.Uncertainty.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("U238");
                xmlWriter.WriteStartElement("WeightPercent");
                xmlWriter.WriteString(Material.U238MassPercent.Value.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Uncertainty");
                xmlWriter.WriteString(Material.U238MassPercent.Uncertainty.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
            if (Material.Am241MassPercent.IsSet())
            {
                xmlWriter.WriteStartElement("Americium");
                xmlWriter.WriteStartElement("Date");
                xmlWriter.WriteString(Material.AmDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Am241");
                xmlWriter.WriteStartElement("WeightPercent");
                xmlWriter.WriteString(Material.Am241MassPercent.Value.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Uncertainty");
                xmlWriter.WriteString(Material.Am241MassPercent.Uncertainty.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
            if (Material.Pu239MassPercent.IsSet())
            {
                xmlWriter.WriteStartElement("Plutonium");
                xmlWriter.WriteStartElement("Date");
                xmlWriter.WriteString(Material.PuDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Pu238");
                xmlWriter.WriteStartElement("WeightPercent");
                xmlWriter.WriteString(Material.Pu238MassPercent.Value.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Uncertainty");
                xmlWriter.WriteString(Material.Pu238MassPercent.Uncertainty.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Pu239");
                xmlWriter.WriteStartElement("WeightPercent");
                xmlWriter.WriteString(Material.Pu239MassPercent.Value.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Uncertainty");
                xmlWriter.WriteString(Material.Pu239MassPercent.Uncertainty.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Pu240");
                xmlWriter.WriteStartElement("WeightPercent");
                xmlWriter.WriteString(Material.Pu240MassPercent.Value.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Uncertainty");
                xmlWriter.WriteString(Material.Pu240MassPercent.Uncertainty.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Pu241");
                xmlWriter.WriteStartElement("WeightPercent");
                xmlWriter.WriteString(Material.Pu241MassPercent.Value.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Uncertainty");
                xmlWriter.WriteString(Material.Pu241MassPercent.Uncertainty.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Pu242");
                xmlWriter.WriteStartElement("WeightPercent");
                xmlWriter.WriteString(Material.Pu242MassPercent.Value.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Uncertainty");
                xmlWriter.WriteString(Material.Pu242MassPercent.Uncertainty.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement(); // End of Batch element
            xmlWriter.WriteEndElement();    // End of Item element

            xmlWriter.WriteStartElement("CreationDate");
            xmlWriter.WriteString(creationDate.ToString("yyyy-MM-ddTHH:mm:ss"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ModificationDate");
            xmlWriter.WriteString(modificationDate.ToString("yyyy-MM-ddTHH:mm:ss"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Note");
            xmlWriter.WriteString(note);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            return ReturnCode.SUCCESS;
        }
    }
}
