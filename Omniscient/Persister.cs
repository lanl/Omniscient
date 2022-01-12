// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2020, International Atomic Energy Agency (IAEA), IAEA.org
// Copyright (c) 2018, Triad National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Triad National Security, LLC. This software was produced under U.S. Government contract 89233218CNA000001 for Los Alamos National Laboratory (LANL), which is operated by Triad National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR TRIAD NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Triad National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY TRIAD NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL TRIAD NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omniscient
{
    /// <summary>
    /// A Persister is an object in Omniscient which continues to exist between
    /// sessions.
    /// </summary>
    public abstract class Persister
    {
        public static string AppDataDirectory = "";
        private static Random random = new Random();
        /// <summary>
        /// A list of taken Persister IDs. All Persisters have a unique ID.
        /// </summary>
        public static List<uint> TakenIDs = new List<uint>(256);

        private uint _id;
        
        /// <summary>
        /// An ID for internal use. Not meant for user viewing.
        /// </summary>
        public uint ID
        {
            get { return _id; }
            protected set
            {
                if(TakenIDs.BinarySearch(value) >=0)
                {
                    throw new ArgumentException("Every Persister must have a unique ID!");
                }
                TakenIDs[TakenIDs.BinarySearch(_id)] = value;
                _id = value;
                TakenIDs.Sort();
            }
        }

        protected string _name;
        /// <summary>
        /// A name for user interaction and identification of the object
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public abstract string Species { get; }
        public virtual Persister Parent { get; }
        public virtual List<Persister> Children { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public Persister(Persister parent, string name, uint id)
        {
            Parent = parent;
            if (Parent !=null) Parent.Children.Add(this);

            if (id == 0)
            {
                // Assign an ID (not 0)
                _id = (uint)(random.NextDouble() * uint.MaxValue);
                while (TakenIDs.BinarySearch(_id) >= 0 || _id == 0) _id = (uint)(random.NextDouble() * uint.MaxValue);
                TakenIDs.Add(_id);
                TakenIDs.Sort();
            }
            else _id = id;

            Name = name;

            Children = new List<Persister>();
        }

        /// <summary>
        /// Writes the Persisters to an XML node
        /// </summary>
        /// <param name="xmlWriter"></param>
        public abstract void ToXML(XmlWriter xmlWriter);

        /// <summary>
        /// Reads the common elements of a Persister from XML
        /// </summary>
        protected static ReturnCode StartFromXML(XmlNode node, out string name, out uint id)
        {
            if (node.Attributes["Name"] != null)
            {
                name = node.Attributes["Name"]?.InnerText;
            } else if (node.Attributes["name"] != null)
            {
                name = node.Attributes["name"]?.InnerText;
            } else
            {
                throw new ApplicationException("No name in XML node!");
            }

            if (node.Attributes["ID"] == null)
            {
                id = 0;
            }
            else
            {
                id = uint.Parse(node.Attributes["ID"].InnerText, System.Globalization.NumberStyles.HexNumber);
            }
            return ReturnCode.SUCCESS;
        }

        /// <summary>
        /// Writes the common elements of a Persister to XML
        /// </summary>
        /// <param name="xmlWriter"></param>
        protected void StartToXML(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(Species.Replace(" ", ""));
            xmlWriter.WriteAttributeString("ID", ID.ToString("X8"));
            xmlWriter.WriteAttributeString("Name", Name);
        }

        /// <summary>
        /// Sets the index in the parent's list of children
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual bool SetIndex(int index)
        {
            Parent.Children.Remove(this);
            Parent.Children.Insert(index, this);
            return true;
        }

        /// <summary>
        /// Removes the Persister from Omniscient.
        /// </summary>
        public virtual void Delete()
        {
            int nChildren = Children.Count();
            for (int i = nChildren-1;  i>=0; i--) Children[i].Delete();
            if (Parent != null)
            {
                Parent.Children.Remove(this);
            }
            TakenIDs.Remove(ID);
        }

        /// <summary>
        /// Returns true if any of the persister's immediate children have the given name.
        /// Note: not recursive
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ChildrenContainName(string name)
        {
            foreach (Persister child in Children)
            {
                if (child.Name == name) return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if any of the persister's children (recursive) have the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ChildrenContainNameRecursive(string name)
        {
            foreach (Persister child in Children)
            {
                if (child.Name == name) return true;
                if (child.ChildrenContainNameRecursive(name)) return true;
            }
            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
