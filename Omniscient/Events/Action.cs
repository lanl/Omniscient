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
    public abstract class Action : Persister
    {
        public override string Species { get { return "Action"; } }

        protected string actionType;
        protected EventGenerator eventGenerator;

        public Action(EventGenerator parent, string name, uint id) : base(parent, name, id)
        {
            eventGenerator = parent;
            eventGenerator.GetActions().Add(this);
        }

        public abstract void Execute(Event eve);
        
        public string GetActionType() { return actionType; }
        public EventGenerator GetEventGenerator() { return eventGenerator; }

        public static Action FromXML(XmlNode actionNode, EventGenerator eg)
        {
            Action action;
            string name;
            uint id;
            Persister.StartFromXML(actionNode, out name, out id);
            switch (actionNode.Attributes["type"]?.InnerText)
            {
                case "Analysis":
                    AnalysisAction analysisAction = new AnalysisAction(eg, name, id);
                    analysisAction.GetAnalysis().SetCommand(actionNode.Attributes["command"]?.InnerText);
                    foreach (Instrument inst in (eg.Parent as DetectionSystem).GetInstruments())
                    {
                        foreach (Channel ch in inst.GetChannels())
                        {
                            if (ch.Name == actionNode.Attributes["channel"]?.InnerText)
                                analysisAction.AddChannel(ch);
                        }
                    }
                    analysisAction.SetCompiledFileName(actionNode.Attributes["compiled_file"]?.InnerText);
                    analysisAction.GetAnalysis().SetResultsFile(actionNode.Attributes["result_file"]?.InnerText);
                    switch (actionNode.Attributes["result_parser"]?.InnerText)
                    {
                        case "FRAM-Pu":
                            analysisAction.GetAnalysis().SetResultParser(new FRAMPlutoniumResultParser());
                            break;
                        case "FRAM-U":
                            analysisAction.GetAnalysis().SetResultParser(new FRAMUraniumResultParser());
                            break;
                        default:
                            throw new ApplicationException("Corrupted XML node!");
                    }
                    foreach (XmlNode dataCompilerNode in actionNode.ChildNodes)
                    {
                        if (dataCompilerNode.Name != "DataCompiler") throw new ApplicationException("Corrupted XML node!");
                        switch (dataCompilerNode.Attributes["type"]?.InnerText)
                        {
                            case "SpectrumCompiler":
                                SpectrumCompiler spectrumCompiler = new SpectrumCompiler("");
                                switch (dataCompilerNode.Attributes["parser"]?.InnerText)
                                {
                                    case "CHN":
                                        spectrumCompiler.SetSpectrumParser(new CHNParser());
                                        break;
                                    default:
                                        throw new ApplicationException("Corrupted XML node!");
                                }
                                switch (dataCompilerNode.Attributes["writer"]?.InnerText)
                                {
                                    case "CHN":
                                        spectrumCompiler.SetSpectrumWriter(new CHNWriter());
                                        break;
                                    default:
                                        throw new ApplicationException("Corrupted XML node!");
                                }
                                analysisAction.GetDataCompilers().Add(spectrumCompiler);
                                break;
                            case "FileListCompiler":
                                analysisAction.GetDataCompilers().Add(new FileListCompiler(""));
                                break;
                            default:
                                throw new ApplicationException("Corrupted XML node!");
                        }
                    }
                    action = analysisAction;
                    break;
                case "Command":
                    action = new CommandAction(eg, name, id);
                    ((CommandAction)action).SetCommand(actionNode.Attributes["command"]?.InnerText);
                    break;
                default:
                    throw new ApplicationException("Corrupted XML node!");
            }
            return action;
        }

        public override bool SetIndex(int index)
        {
            eventGenerator.GetActions().Remove(this);
            eventGenerator.GetActions().Insert(index, this);
            return base.SetIndex(index);
        }

        public override void Delete()
        {
            eventGenerator.GetActions().Remove(this);
            base.Delete();
        }
    }
}
