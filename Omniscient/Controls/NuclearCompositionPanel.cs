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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omniscient
{
    public partial class NuclearCompositionPanel : UserControl
    {
        public DateTime ReferenceDate;
        public NuclearComposition Composition { get; set; }

        public NuclearCompositionPanel()
        {
            ReferenceDate = new DateTime(1800, 1, 1);
            InitializeComponent();
            Composition = null;
        }

        public ReturnCode InputWithValidation(MeasuredValue measuredValue, string value, string uncertainty)
        {
            if (value == "" && uncertainty == "")
            {
                measuredValue.Unset();
            }
            else
            {
                try
                {
                    measuredValue.Value = double.Parse(value);
                    measuredValue.Uncertainty = double.Parse(uncertainty);
                }
                catch
                {
                    return ReturnCode.BAD_INPUT;
                }
            }
            return ReturnCode.SUCCESS;
        }

        public ReturnCode Scrape()
        {
            if (InputWithValidation(Composition.PuMass, PuMassTextBox.Text, PuMassUncertaintyTextBox.Text) != ReturnCode.SUCCESS)
            {
                MessageBox.Show("Invalid Pu mass input!");
                return ReturnCode.BAD_INPUT;
            }
            if (InputWithValidation(Composition.Pu238MassPercent, Pu238MassPercentTextBox.Text, Pu238UncertaintyTextBox.Text) != ReturnCode.SUCCESS)
            {
                MessageBox.Show("Invalid Pu-238 input!");
                return ReturnCode.BAD_INPUT;
            }
            if (InputWithValidation(Composition.Pu239MassPercent, Pu239MassPercentTextBox.Text, Pu239UncertaintyTextBox.Text) != ReturnCode.SUCCESS)
            {
                MessageBox.Show("Invalid Pu-239 input!");
                return ReturnCode.BAD_INPUT;
            }
            if (InputWithValidation(Composition.Pu240MassPercent, Pu240MassPercentTextBox.Text, Pu240UncertaintyTextBox.Text) != ReturnCode.SUCCESS)
            {
                MessageBox.Show("Invalid Pu-240 input!");
                return ReturnCode.BAD_INPUT;
            }
            if (InputWithValidation(Composition.Pu241MassPercent, Pu241MassPercentTextBox.Text, Pu241UncertaintyTextBox.Text) != ReturnCode.SUCCESS)
            {
                MessageBox.Show("Invalid Pu-241 input!");
                return ReturnCode.BAD_INPUT;
            }
            if (InputWithValidation(Composition.Pu242MassPercent, Pu242MassPercentTextBox.Text, Pu242UncertaintyTextBox.Text) != ReturnCode.SUCCESS)
            {
                MessageBox.Show("Invalid Pu-242 input!");
                return ReturnCode.BAD_INPUT;
            }
            if (InputWithValidation(Composition.Am241MassPercent, Am241MassPercentTextBox.Text, Am241UncertaintyTextBox.Text) != ReturnCode.SUCCESS)
            {
                MessageBox.Show("Invalid Am-241 input!");
                return ReturnCode.BAD_INPUT;
            }
            if (InputWithValidation(Composition.UMass, UMassTextBox.Text, UMassUncertaintyTextBox.Text) != ReturnCode.SUCCESS)
            {
                MessageBox.Show("Invalid U mass input!");
                return ReturnCode.BAD_INPUT;
            }
            if (InputWithValidation(Composition.U234MassPercent, U234MassPercentTextBox.Text, U234UncertaintyTextBox.Text) != ReturnCode.SUCCESS)
            {
                MessageBox.Show("Invalid U234 input!");
                return ReturnCode.BAD_INPUT;
            }
            if (InputWithValidation(Composition.U235MassPercent, U235MassPercentTextBox.Text, U235UncertaintyTextBox.Text) != ReturnCode.SUCCESS)
            {
                MessageBox.Show("Invalid U235 input!");
                return ReturnCode.BAD_INPUT;
            }
            if (InputWithValidation(Composition.U236MassPercent, U236MassPercentTextBox.Text, U236UncertaintyTextBox.Text) != ReturnCode.SUCCESS)
            {
                MessageBox.Show("Invalid U236 input!");
                return ReturnCode.BAD_INPUT;
            }
            if (InputWithValidation(Composition.U238MassPercent, U238MassPercentTextBox.Text, U238UncertaintyTextBox.Text) != ReturnCode.SUCCESS)
            {
                MessageBox.Show("Invalid U238 input!");
                return ReturnCode.BAD_INPUT;
            }

            Composition.PuDate = PuDatePicker.Value.Date.Add(PuTimePicker.Value.TimeOfDay);
            Composition.AmDate = AmDatePicker.Value.Date.Add(AmTimePicker.Value.TimeOfDay);

            return ReturnCode.SUCCESS;
        }

        public void UpdateFields()
        {
            if (Composition == null) return;
            if (Composition.PuMass.IsSet())
            {
                PuMassTextBox.Text = Composition.PuMass.Value.ToString();
                PuMassUncertaintyTextBox.Text = Composition.PuMass.Uncertainty.ToString();
            }

            if (Composition.Pu238MassPercent.IsSet())
            {
                Pu238MassPercentTextBox.Text = Composition.Pu238MassPercent.Value.ToString();
                Pu238UncertaintyTextBox.Text = Composition.Pu238MassPercent.Uncertainty.ToString();
            }

            if (Composition.Pu239MassPercent.IsSet())
            {
                Pu239MassPercentTextBox.Text = Composition.Pu239MassPercent.Value.ToString();
                Pu239UncertaintyTextBox.Text = Composition.Pu239MassPercent.Uncertainty.ToString();
            }

            if (Composition.Pu240MassPercent.IsSet())
            {
                Pu240MassPercentTextBox.Text = Composition.Pu240MassPercent.Value.ToString();
                Pu240UncertaintyTextBox.Text = Composition.Pu240MassPercent.Uncertainty.ToString();
            }

            if (Composition.Pu241MassPercent.IsSet())
            {
                Pu241MassPercentTextBox.Text = Composition.Pu241MassPercent.Value.ToString();
                Pu241UncertaintyTextBox.Text = Composition.Pu241MassPercent.Uncertainty.ToString();
            }

            if (Composition.Pu242MassPercent.IsSet())
            {
                Pu242MassPercentTextBox.Text = Composition.Pu242MassPercent.Value.ToString();
                Pu242UncertaintyTextBox.Text = Composition.Pu242MassPercent.Uncertainty.ToString();
            }

            if (Composition.Am241MassPercent.IsSet())
            {
                Am241MassPercentTextBox.Text = Composition.Am241MassPercent.Value.ToString();
                Am241UncertaintyTextBox.Text = Composition.Am241MassPercent.Uncertainty.ToString();
            }

            if (Composition.UMass.IsSet())
            {
                UMassTextBox.Text = Composition.UMass.Value.ToString();
                UMassUncertaintyTextBox.Text = Composition.UMass.Uncertainty.ToString();
            }

            if (Composition.U234MassPercent.IsSet())
            {
                U234MassPercentTextBox.Text = Composition.U234MassPercent.Value.ToString();
                U234UncertaintyTextBox.Text = Composition.U234MassPercent.Uncertainty.ToString();
            }

            if (Composition.U235MassPercent.IsSet())
            {
                U235MassPercentTextBox.Text = Composition.U235MassPercent.Value.ToString();
                U235UncertaintyTextBox.Text = Composition.U235MassPercent.Uncertainty.ToString();
            }

            if (Composition.U236MassPercent.IsSet())
            {
                U236MassPercentTextBox.Text = Composition.U236MassPercent.Value.ToString();
                U236UncertaintyTextBox.Text = Composition.U236MassPercent.Uncertainty.ToString();
            }

            if (Composition.U238MassPercent.IsSet())
            {
                U238MassPercentTextBox.Text = Composition.U238MassPercent.Value.ToString();
                U238UncertaintyTextBox.Text = Composition.U238MassPercent.Uncertainty.ToString();
            }

            if (Composition.PuDate > ReferenceDate)
            {
                PuDatePicker.Value = Composition.PuDate;
                PuTimePicker.Value = Composition.PuDate;
            }

            if (Composition.AmDate > ReferenceDate)
            {
                AmDatePicker.Value = Composition.AmDate;
                AmTimePicker.Value = Composition.AmDate;
            }
        }

    }
}
