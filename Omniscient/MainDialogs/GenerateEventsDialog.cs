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
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omniscient.MainDialogs
{
    public partial class GenerateEventsDialog : Form
    {
        OmniscientCore Core;

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public GenerateEventsDialog(OmniscientCore core)
        {
            InitializeComponent();
            Core = core;

            StartDatePicker.Value = Core.GlobalStart;
            StartTimePicker.Value = Core.GlobalStart;
            EndDatePicker.Value = Core.GlobalEnd;
            EndTimePicker.Value = Core.GlobalEnd;
        }

        private void GenerateEventsDialog_Load(object sender, EventArgs e)
        {
            
        }

        private void AllButton_Click(object sender, EventArgs e)
        {
            StartDatePicker.Value = Core.GlobalStart;
            StartTimePicker.Value = Core.GlobalStart;
            EndDatePicker.Value = Core.GlobalEnd;
            EndTimePicker.Value = Core.GlobalEnd;
        }

        private void ChartRangeButton_Click(object sender, EventArgs e)
        {
            StartDatePicker.Value = Core.ViewStart;
            StartTimePicker.Value = Core.ViewStart;
            EndDatePicker.Value = Core.ViewEnd;
            EndTimePicker.Value = Core.ViewEnd;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Dispose();
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            // Validate date range
            StartTime = StartDatePicker.Value.Date + StartTimePicker.Value.TimeOfDay;
            EndTime = EndDatePicker.Value.Date + EndTimePicker.Value.TimeOfDay;
            if (StartTime >= EndTime)
            {
                MessageBox.Show("Negative or 0 time range selected!");
                return;
            }

            // Ok!
            DialogResult = DialogResult.OK;
            Dispose();
        }
    }
}
