using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Omniscient
{
    /// <summary>
    /// Applies a convolution to a spectrum
    /// </summary>
    public class ConvoluteSpectrumAnalyzerStep : AnalyzerStep
    {
        string inputSpecParamName;
        double[] filter;
        string outputSpecParamName;

        public ConvoluteSpectrumAnalyzerStep(Analyzer analyzer, string name, uint id) : base(analyzer, name, id, AnalyzerStepType.CONVOLUTE_SPECTRUM)
        {
            inputSpecParamName = "";
            filter = new double[1];
            outputSpecParamName = "";
        }

        public override List<Parameter> GetParameters()
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new StringParameter("Input Parameter", inputSpecParamName));
            parameters.Add(new DoubleArrayParameter("Filter", filter));
            parameters.Add(new StringParameter("Output Parameter", outputSpecParamName));
            return parameters;
        }

        public override void ApplyParameters(List<Parameter> parameters)
        {
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Input Parameter":
                        inputSpecParamName = param.Value;
                        break;
                    case "Filter":
                        filter = (param as DoubleArrayParameter).ToDoubleArray();
                        break;
                    case "Output Parameter":
                        outputSpecParamName = param.Value;
                        break;
                }
            }
        }

        public override ReturnCode Run(AnalyzerRunData data)
        {
            // Validate parameters
            SpectrumParameter inputSpecParam, outputSpecParam;
            try
            {
                inputSpecParam = data.CustomParameters[inputSpecParamName].Parameter as SpectrumParameter;
                outputSpecParam = data.CustomParameters[outputSpecParamName].Parameter as SpectrumParameter;
            }
            catch (Exception ex) { return ReturnCode.BAD_INPUT; }

            double[] specDouble = inputSpecParam.Spectrum.GetCounts().Select(Convert.ToDouble).ToArray();
            double[] ouputDouble = SignalProcessor.Convolve(specDouble, filter);

            Spectrum spectrum = new Spectrum(inputSpecParam.Spectrum);
            spectrum.SetCounts(ouputDouble.Select(Convert.ToInt32).ToArray());

            outputSpecParam.Spectrum = spectrum;
            return ReturnCode.SUCCESS;
        }
    }

    public class ConvoluteSpectrumAnalyzerStepHookup : AnalyzerStepHookup
    {
        public ConvoluteSpectrumAnalyzerStepHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Input Parameter", ParameterType.String),
                new ParameterTemplate("Filter", ParameterType.DoubleArray),
                new ParameterTemplate("Output Parameter", ParameterType.String)
            };
        }

        public override string Type { get { return "Convolute Spectrum"; } }
        public override AnalyzerStep FromParameters(Analyzer parent, string newName, List<Parameter> parameters, uint id)
        {
            ConvoluteSpectrumAnalyzerStep step = new ConvoluteSpectrumAnalyzerStep(parent, newName, id);
            step.ApplyParameters(parameters);
            return step;
        }
    }
}
