﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static Omniscient.AnalyzerStep;

namespace Omniscient
{
    /// <summary>
    /// An analyzer runs a series of steps in order to perform an analysis associated with an Event. 
    /// Each type of step inherits from this abstract class
    /// </summary>
    public abstract class AnalyzerStep : Persister
    {
        public override string Species { get { return "AnalyzerStep"; } }
        public static readonly AnalyzerStepHookup[] Hookups = new AnalyzerStepHookup[]
        {
            new TwoParameterAnalyzerStepHookup()
        };
        public enum AnalyzerStepType { TWOPARAMETER }
        public AnalyzerStepType StepType { get; private set; }
        public Analyzer ParentAnalyzer { get; }

        public AnalyzerStep(Analyzer parent, string name, uint id, AnalyzerStepType stepType) : base(parent, name, id)
        {
            parent.GetSteps().Add(this);
            ParentAnalyzer = parent;
            StepType = stepType;
        }

        public abstract ReturnCode Run(Event eve, Dictionary<string, AnalyzerParameter> analysisParams);

        public abstract List<Parameter> GetParameters();
        public abstract void ApplyParameters(List<Parameter> parameters);

        public static AnalyzerStepHookup GetHookup(string type)
        {
            foreach (AnalyzerStepHookup hookup in Hookups)
            {
                if (hookup.Type == type)
                {
                    return hookup;
                }
            }
            return null;
        }
        public static AnalyzerStep FromXML(XmlNode node, Analyzer analyzer)
        {
            string name;
            uint id;
            Persister.StartFromXML(node, out name, out id);
            AnalyzerStepHookup hookup = GetHookup(node.Attributes["Type"]?.InnerText);
            List<Parameter> parameters = Parameter.FromXML(node, hookup.TemplateParameters, analyzer.ParentDetectionSystem);
            return hookup?.FromParameters(analyzer, name, parameters, id);
        }
        public override void ToXML(XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public override bool SetIndex(int index)
        {
            base.SetIndex(index);
            ParentAnalyzer.GetSteps().Remove(this);
            ParentAnalyzer.GetSteps().Insert(index, this);
            return true;
        }
        public override void Delete()
        {
            base.Delete();
            ParentAnalyzer.GetSteps().Remove(this);
        }
    }

    /// <summary>
    /// Hookups facilitate creation of AnalyzerSteps from the UI and XML
    /// </summary>
    public abstract class AnalyzerStepHookup
    {
        public abstract AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id);
        public abstract string Type { get; }
        public List<ParameterTemplate> TemplateParameters { get; set; }
    }


    /// <summary>
    /// Combines two AnalyzerParameters through addition, subtration, multiplication, or division
    /// </summary>
    public class TwoParameterAnalyzerStep : AnalyzerStep
    {
        public enum OperationType { Sum, Difference, Product, Ratio }
        OperationType Operation { get; set; }
        string param1Name;
        string param2Name;
        string outputParamName;

        public TwoParameterAnalyzerStep(Analyzer analyzer, string name, uint id) : base(analyzer, name, id, AnalyzerStepType.TWOPARAMETER) 
        {
            param1Name = "";
            param2Name = "";
            outputParamName = "";
        }
        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
            string operation = "";
            switch (Operation)
            {
                case OperationType.Sum:
                    operation = "Sum";
                    break;
                case OperationType.Difference:
                    operation = "Difference";
                    break;
                case OperationType.Product:
                    operation = "Product";
                    break;
                case OperationType.Ratio:
                    operation = "Ratio";
                    break;
            }
            parameters.Add(new EnumParameter("Operation") { Value=operation, ValidValues= new List<string>() { "Sum", "Difference", "Product", "Ratio" } });
            parameters.Add(new StringParameter("Input Parameter 1", param1Name));
            parameters.Add(new StringParameter("Input Parameter 2", param2Name));
            parameters.Add(new StringParameter("Output Parameter", param2Name));
            return parameters;
        }
        public override void ApplyParameters(List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Input Parameter 1":
                        param1Name = param.Value;
                        break;
                    case "Input Parameter 2":
                        param2Name = param.Value;
                        break;
                    case "Output Parameter":
                        outputParamName = param.Value;
                        break;
                    case "Operation":
                        switch (param.Value)
                        {
                            case "Sum":
                                Operation = OperationType.Sum;
                                break;
                            case "Difference":
                                Operation = OperationType.Difference;
                                break;
                            case "Product":
                                Operation = OperationType.Product;
                                break;
                            case "Ratio":
                                Operation = OperationType.Ratio;
                                break;
                        }
                        break;
                }
            }
        }

        public override ReturnCode Run(Event eve, Dictionary<string, AnalyzerParameter> analysisParams)
        {
            Parameter param1, param2, outputParam;
            try
            {
                param1 = analysisParams[param1Name].Parameter;
                param2 = analysisParams[param2Name].Parameter;
                outputParam = analysisParams[outputParamName].Parameter;
            }
            catch (Exception ex) { return ReturnCode.BAD_INPUT; }
            if (param1.Type != ParameterType.Int && param1.Type != ParameterType.Double) return ReturnCode.BAD_INPUT;
            if (param2.Type != ParameterType.Int && param2.Type != ParameterType.Double) return ReturnCode.BAD_INPUT;
            if (outputParam.Type != ParameterType.Int && outputParam.Type != ParameterType.Double) return ReturnCode.BAD_INPUT;

            if (outputParam.Type == ParameterType.Int && param1.Type == ParameterType.Int && param2.Type == ParameterType.Int)
            {
                int param1I, param2I;
                int resultI = 0;
                param1I = (param1 as IntParameter).ToInt();
                param2I = (param2 as IntParameter).ToInt();
                switch (Operation)
                {
                    case OperationType.Sum:
                        resultI = param1I + param2I;
                        break;
                    case OperationType.Difference:
                        resultI = param1I - param2I;
                        break;
                    case OperationType.Product:
                        resultI = param1I * param2I;
                        break;
                    case OperationType.Ratio:
                        resultI = param1I / param2I;
                        break;
                }
                outputParam.Value = resultI.ToString();
            }
            else
            {
                double param1D, param2D;
                double resultD = 0;
                if (param1.Type == ParameterType.Int) param1D = (param1 as IntParameter).ToInt();
                else param1D = (param1 as DoubleParameter).ToDouble();
                if (param2.Type == ParameterType.Int) param2D = (param2 as IntParameter).ToInt();
                else param2D = (param2 as DoubleParameter).ToDouble();
                switch (Operation)
                {
                    case OperationType.Sum:
                        resultD = param1D + param2D;
                        break;
                    case OperationType.Difference:
                        resultD = param1D - param2D;
                        break;
                    case OperationType.Product:
                        resultD = param1D * param2D;
                        break;
                    case OperationType.Ratio:
                        resultD = param1D / param2D;
                        break;
                }
                if (outputParam.Type == ParameterType.Double)
                    outputParam.Value = resultD.ToString();
                else
                    outputParam.Value = ((int)resultD).ToString();
            }
            return ReturnCode.SUCCESS;
        }
    }

    public class TwoParameterAnalyzerStepHookup : AnalyzerStepHookup
    {
        public TwoParameterAnalyzerStepHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Operation", ParameterType.Enum, new List<string>(){ "Sum", "Difference", "Product", "Ratio" }),
                new ParameterTemplate("Input Parameter 1", ParameterType.String),
                new ParameterTemplate("Input Parameter 2", ParameterType.String),
                new ParameterTemplate("Output Parameter", ParameterType.String)
            };
        }

        public override string Type { get { return "Two Parameter"; } }
        public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
        {
            TwoParameterAnalyzerStep step = new TwoParameterAnalyzerStep(parent, newName, id);
            step.ApplyParameters(parameters);
            return step;
        }
    }

}
