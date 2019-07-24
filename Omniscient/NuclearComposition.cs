// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2018, Triad National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Triad National Security, LLC. This software was produced under U.S. Government contract 89233218CNA000001 for Los Alamos National Laboratory (LANL), which is operated by Triad National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR TRIAD NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Triad National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY TRIAD NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL TRIAD NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class MeasuredValue
    {
        public const double NOT_SET = double.MinValue;

        double val;
        public double Value
        {
            get { return val; }
            set { val = value; }
        }

        double uncertainty;
        public double Uncertainty
        {
            get { return uncertainty; }
            set { uncertainty = value; }
        }

        public double RelativeUncertainty
        {
            get { return uncertainty / val; }
            set { uncertainty = value * val; }
        }

        public MeasuredValue()
        {
            val = NOT_SET;
            uncertainty = NOT_SET;
        }

        public bool IsSet()
        {
            if (val == NOT_SET) return false;
            return true;
        }

        public void Unset()
        {
            val = NOT_SET;
            uncertainty = NOT_SET;
        }
    }

    public class NuclearComposition
    {
        public MeasuredValue PuMass { get; set; }
        public MeasuredValue Pu238MassPercent { get; set; }
        public MeasuredValue Pu239MassPercent { get; set; }
        public MeasuredValue Pu240MassPercent { get; set; }
        public MeasuredValue Pu241MassPercent { get; set; }
        public MeasuredValue Pu242MassPercent { get; set; }
        public MeasuredValue Am241MassPercent { get; set; }
        public DateTime PuDate { get; set; }
        public DateTime AmDate { get; set; }

        public MeasuredValue UMass { get; set; }
        public MeasuredValue U234MassPercent { get; set; }
        public MeasuredValue U235MassPercent { get; set; }
        public MeasuredValue U236MassPercent { get; set; }
        public MeasuredValue U238MassPercent { get; set; }

        public NuclearComposition()
        {
            PuMass = new MeasuredValue();
            Pu238MassPercent = new MeasuredValue();
            Pu239MassPercent = new MeasuredValue();
            Pu240MassPercent = new MeasuredValue();
            Pu241MassPercent = new MeasuredValue();
            Pu242MassPercent = new MeasuredValue();
            Am241MassPercent = new MeasuredValue();
            PuDate = new DateTime(1776, 7, 4);
            AmDate = new DateTime(1776, 7, 4);

            UMass = new MeasuredValue();
            U234MassPercent = new MeasuredValue();
            U235MassPercent = new MeasuredValue();
            U236MassPercent = new MeasuredValue();
            U238MassPercent = new MeasuredValue();
        }
    }
}
