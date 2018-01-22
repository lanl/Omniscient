using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class DetectionSystem : EventWatcher
    {
        List<Instrument> instruments;
        private DeclarationInstrument declarationInstrument;
        string name;

        public DetectionSystem(string newName) : base()
        {
            name = newName;
            instruments = new List<Instrument>();
            declarationInstrument = null;
        }

        public void AddInstrument(Instrument newInstrument)
        {
            if (newInstrument is DeclarationInstrument)
                SetDeclarationInstrument((DeclarationInstrument)newInstrument);
            else
                instruments.Add(newInstrument);
        }

        public List<Instrument> GetInstruments()
        {
            return instruments;
        }

        public void SetName(string newName)
        {
            name = newName;
        }

        public string GetName() { return name; }

        public void SetDeclarationInstrument(DeclarationInstrument inst)
        {
            if (declarationInstrument != null)
            {
                instruments.Remove(declarationInstrument);
            }
            declarationInstrument = inst;
            instruments.Add(inst);
        }

        public void RemoveDeclarationInstrument()
        {
            if (declarationInstrument != null)
            {
                instruments.Remove(declarationInstrument);
            }
            declarationInstrument = null;
        }

        public DeclarationInstrument GetDeclarationInstrument() { return declarationInstrument; }

        public bool HasDeclarationInstrument()
        {
            if (declarationInstrument == null) return false;
            return true;
        }
    }
}
