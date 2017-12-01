using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class AnalysisAction : Action
    {
        DataCompiler dataCompiler;
        Analysis analysis;
        List<Channel> channels;

        public AnalysisAction(string newName) : base(newName)
        {
        }

        public override void Execute(Event eve)
        {
            
        }

        public override void SetName(string newName)
        {
            name = newName;
        }
    }
}
