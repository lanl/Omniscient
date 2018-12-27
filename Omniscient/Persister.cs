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
        public virtual Persister Parent { get; set; }
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
        //public abstract void ToXML(XmlWriter xmlWriter);

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
            if (Parent != null)
            {
                Parent.Children.Remove(this);
            }
            TakenIDs.Remove(ID);
        }
    }
}
