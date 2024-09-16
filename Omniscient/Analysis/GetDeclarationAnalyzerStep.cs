using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    /// <summary>
    /// Prompts the user for a declaration and stores it in the AnalyzerRunData
    /// </summary>
    public class GetDeclarationAnalyzerStep : AnalyzerStep
    {
        public GetDeclarationAnalyzerStep(Analyzer parent, string name, uint id) : base(parent, name, id, "Get Declaration")
        {
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            return;
        }

        public override List<Parameter> GetParameters()
        {
            return new List<Parameter>();
        }

        public override ReturnCode Run(AnalyzerRunData data)
        {
            DeclarationEditor editor = new DeclarationEditor(ParentAnalyzer.DetectionSystem);
            if (editor.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return ReturnCode.FAIL;
            if (editor.Declaration is null) return ReturnCode.FAIL;
            data.Declaration = editor.Declaration;
            return ReturnCode.SUCCESS;
        }
    }

    public class GetDeclarationAnalyzerStepHookup : AnalyzerStepHookup
    {
        public GetDeclarationAnalyzerStepHookup()
        {
            TemplateParameters = new List<ParameterTemplate>();
        }

        public override string Type { get { return "Get Declaration"; } }
        public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
        {
            GetDeclarationAnalyzerStep step = new GetDeclarationAnalyzerStep(parent, newName, id);
            step.ApplyParameters(parameters);
            return step;
        }
    }
}
