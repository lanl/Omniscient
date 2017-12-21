using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class AnalysisAction : Action
    {
        List<DataCompiler> dataCompilers;
        Analysis analysis;
        List<Channel> channels;
        string compiledFileName;

        public AnalysisAction(EventGenerator parent, string newName) : base(parent, newName)
        {
            actionType = "Analysis";
            dataCompilers = new List<DataCompiler>();
            analysis = new Analysis();
            channels = new List<Channel>();
            compiledFileName = "";
        }

        public override void Execute(Event eve)
        {
            List<string> rawFiles;
            string targetFile;
            List<string> targetFiles = new List<string>();
            for (int dc = 0; dc < dataCompilers.Count(); dc++)
            {
                rawFiles = channels[dc].GetFiles(eve.GetStartTime(), eve.GetEndTime());
                targetFile = compiledFileName.Replace("||s||", eve.GetStartTime().ToString("yyyy-MM-dd-HH-mm-ss"));
                targetFile = targetFile.Replace("||#||", dc.ToString());
                targetFiles.Add(targetFile);
                if (dataCompilers[dc] != null)
                {
                    dataCompilers[dc].Compile(rawFiles, eve.GetStartTime(), eve.GetEndTime(), targetFiles[dc]);
                }
            }
            analysis.Run(targetFiles);
            eve.AddAnalysisResults(analysis.GetResults());
        }

        public override void SetName(string newName)
        {
            name = newName;
        }

        public void AddChannel(Channel chan) { channels.Add(chan); }
        public void RemoveChannel(Channel chan) { channels.Remove(chan); }

        public void SetAnalysis(Analysis ana) { analysis = ana; }
        public void SetCompiledFileName(string fileName) { compiledFileName = fileName; }

        public Analysis GetAnalysis() { return analysis; }
        public List<DataCompiler> GetDataCompilers() { return dataCompilers; }
        public List<Channel> GetChannels() { return channels; }
        public string GetCompiledFileName() { return compiledFileName; }
    }
}
