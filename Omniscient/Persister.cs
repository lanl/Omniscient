using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            private set
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
        public Persister(Persister parent, string name)
        {
            Parent = parent;
            if (Parent !=null) Parent.Children.Add(this);

            // Assign an ID
            _id = (uint)(random.NextDouble() * uint.MaxValue);
            while (TakenIDs.BinarySearch(ID) >= 0) _id = (uint)(random.NextDouble() * uint.MaxValue);
            TakenIDs.Add(_id);
            TakenIDs.Sort();

            Name = name;

            Children = new List<Persister>();
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
        }
    }
}
