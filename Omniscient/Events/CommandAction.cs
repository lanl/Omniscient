using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class CommandAction : Action
    {
        string command;

        public CommandAction(string newName) : base(newName)
        {
            actionType = "Command";
            command = "";
        }

        public override void Execute(Event eve)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            process.Close();
        }

        public string GetCommand() { return command; }

        public override void SetName(string newName) { name = newName; }
        public void SetCommand(string newCommand) { command = newCommand; }
    }
}
