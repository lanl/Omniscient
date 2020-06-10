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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omniscient
{
    public partial class ChannelPanel : UserControl
    {
        public static readonly Color[] DefaultColors = new Color[]
        {
            Color.Blue,
            Color.Red,
            Color.Green,
            Color.Cyan,
            Color.Magenta,
            Color.Coral,
            Color.Black,
            Color.Olive,
            Color.Gold
        };

        public static int defaultColorCounter = 0;

        private const string LINE = "_";
        private const string DOT = "o";

        public event EventHandler CheckChanged;
        public event EventHandler SymbolChanged;

        public enum SymbolType { Line, Dot }

        public SymbolType Symbol { get; private set; }

        public Color ChannelColor { get; private set; }

        private Channel channel;

        public ChannelPanel(Channel ch)
        {
            channel = ch;

            ChannelColor = DefaultColors[defaultColorCounter];
            defaultColorCounter++;
            if (defaultColorCounter == DefaultColors.Length) defaultColorCounter = 0;

            InitializeComponent();
        }

        private void ChannelPanel_Load(object sender, EventArgs e)
        {
            NameTextBox.Text = channel.Name;
            Chart1CheckBox.Tag = 0;
            Chart2CheckBox.Tag = 1;
            Chart3CheckBox.Tag = 2;
            Chart4CheckBox.Tag = 3;

            SymbolComboBox.Items.Add(LINE);
            SymbolComboBox.Items.Add(DOT);
            SymbolComboBox.SelectedItem = LINE;
            Symbol = SymbolType.Line;

            ColorButton.BackColor = ChannelColor;

            NameToolTip.SetToolTip(NameTextBox, NameTextBox.Text);
        }

        public Channel GetChannel() { return channel; }

        private void Chart1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckChanged?.Invoke(sender, e);
        }

        private void Chart2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckChanged?.Invoke(sender, e);
        }

        private void Chart3CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckChanged?.Invoke(sender, e);
        }

        private void Chart4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckChanged?.Invoke(sender, e);
        }

        private void SymbolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)SymbolComboBox.SelectedItem == LINE)
            {
                Symbol = SymbolType.Line;
            }
            else
            {
                Symbol = SymbolType.Dot;
            }
            SymbolChanged?.Invoke(sender, e);
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = ChannelColor;
            int[] customColors = new int[DefaultColors.Length];
            for (int i=0; i<DefaultColors.Length; i++)
            {
                customColors[i] = ((int)DefaultColors[i].B << 16) + 
                    ((int)DefaultColors[i].G << 8) + 
                    ((int)DefaultColors[i].R);
            }
            colorDialog.CustomColors = customColors;
            colorDialog.ShowDialog();
            ChannelColor = colorDialog.Color;
            ColorButton.BackColor = ChannelColor;
            SymbolChanged?.Invoke(sender, e);
        }
    }
}
