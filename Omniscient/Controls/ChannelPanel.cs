// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2020, International Atomic Energy Agency (IAEA), IAEA.org
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
using System.Xml;

namespace Omniscient
{
    public class ChannelDisplayConfig
    {
        private const int N_CHARTS = 4;
        public bool[] ChartActive { get; set; }
        public enum SymbolType { Line, Dot, LineAndDot }
        public SymbolType Symbol { get; set; }
        public Color SeriesColor { get; set; }

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

        public ChannelDisplayConfig()
        {
            ChartActive = new bool[N_CHARTS];
            
            SeriesColor = DefaultColors[defaultColorCounter];
            defaultColorCounter++;
            if (defaultColorCounter == DefaultColors.Length) defaultColorCounter = 0;
        }

        public ChannelDisplayConfig Copy()
        {
            ChannelDisplayConfig config = new ChannelDisplayConfig();
            config.SeriesColor = SeriesColor;
            config.Symbol = Symbol;
            for (int chart = 0; chart < N_CHARTS; chart++)
            {
                config.ChartActive[chart] = ChartActive[chart];
            }
            return config;
        }

        public void ToXML(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("ChannelDisplayConfig");
            xmlWriter.WriteAttributeString("SeriesColor", SeriesColor.ToArgb().ToString());
            switch (Symbol)
            {
                case SymbolType.Line:
                    xmlWriter.WriteAttributeString("Symbol", "Line");
                    break;
                case SymbolType.Dot:
                    xmlWriter.WriteAttributeString("Symbol", "Dot");
                    break;
                case SymbolType.LineAndDot:
                    xmlWriter.WriteAttributeString("Symbol", "LineAndDot");
                    break;
            }
            for(int chart=0; chart<N_CHARTS; chart++)
            {
                xmlWriter.WriteAttributeString("DisplayOnChart"+chart.ToString(), ChartActive[chart].ToString());
            }
            xmlWriter.WriteEndElement();
        }

        static public ChannelDisplayConfig FromXML(XmlNode node)
        {
            ChannelDisplayConfig config = new ChannelDisplayConfig();

            config.SeriesColor = Color.FromArgb(int.Parse(node.Attributes["SeriesColor"]?.InnerText));

            if (node.Attributes["Symbol"]?.InnerText == "Line")
            {
                config.Symbol = SymbolType.Line;
            }
            else if (node.Attributes["Symbol"]?.InnerText == "Dot")
            {
                config.Symbol = SymbolType.Dot;
            }
            else if (node.Attributes["Symbol"]?.InnerText == "LineAndDot")
            {
                config.Symbol = SymbolType.LineAndDot;
            }
            for (int chart = 0; chart < N_CHARTS; chart++)
            {
                config.ChartActive[chart] = node.Attributes["DisplayOnChart" + chart.ToString()]?.InnerText == "True";
            }
            return config;
        }
    }

    public partial class ChannelPanel : UserControl
    {
        private const string LINE = "━";
        private const string DOT = "●";
        private const string LINE_AND_DOT = "!";

        public event EventHandler CheckChanged;
        public event EventHandler SymbolChanged;

        private Channel channel;

        public ChannelDisplayConfig Config { get; private set; }

        public ChannelPanel(Channel ch)
        {
            channel = ch;

            Config = new ChannelDisplayConfig();

            InitializeComponent();
        }

        public void ApplyConfiguration(ChannelDisplayConfig config)
        {
            Config = config.Copy();
            
            Chart1CheckBox.Checked = Config.ChartActive[0];
            Chart2CheckBox.Checked = Config.ChartActive[1];
            Chart3CheckBox.Checked = Config.ChartActive[2];
            Chart4CheckBox.Checked = Config.ChartActive[3];

            switch (Config.Symbol)
            {
                case ChannelDisplayConfig.SymbolType.Dot:
                    SymbolComboBox.SelectedItem = DOT;
                    break;
                case ChannelDisplayConfig.SymbolType.Line:
                    SymbolComboBox.SelectedItem = LINE;
                    break;
                case ChannelDisplayConfig.SymbolType.LineAndDot:
                    SymbolComboBox.SelectedItem = LINE_AND_DOT;
                    break;
            }

            ColorButton.BackColor = Config.SeriesColor;
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
            SymbolComboBox.Items.Add(LINE_AND_DOT);
            SymbolComboBox.SelectedItem = LINE;

            ColorButton.BackColor = Config.SeriesColor;

            NameToolTip.SetToolTip(NameTextBox, NameTextBox.Text);
        }

        public Channel GetChannel() { return channel; }

        private void Chart1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Config.ChartActive[0] = Chart1CheckBox.Checked;
            CheckChanged?.Invoke(sender, e);
        }

        private void Chart2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Config.ChartActive[1] = Chart2CheckBox.Checked;
            CheckChanged?.Invoke(sender, e);
        }

        private void Chart3CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Config.ChartActive[2] = Chart3CheckBox.Checked;
            CheckChanged?.Invoke(sender, e);
        }

        private void Chart4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Config.ChartActive[3] = Chart4CheckBox.Checked;
            CheckChanged?.Invoke(sender, e);
        }

        private void SymbolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)SymbolComboBox.SelectedItem == LINE)
            {
                Config.Symbol = ChannelDisplayConfig.SymbolType.Line;
            }
            else if ((string)SymbolComboBox.SelectedItem == DOT)
            {
                Config.Symbol = ChannelDisplayConfig.SymbolType.Dot;
            }
            else
            {
                Config.Symbol = ChannelDisplayConfig.SymbolType.LineAndDot;
            }
            SymbolChanged?.Invoke(sender, e);
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = Config.SeriesColor;
            int[] customColors = new int[ChannelDisplayConfig.DefaultColors.Length];
            for (int i=0; i< ChannelDisplayConfig.DefaultColors.Length; i++)
            {
                customColors[i] = ((int)ChannelDisplayConfig.DefaultColors[i].B << 16) + 
                    ((int)ChannelDisplayConfig.DefaultColors[i].G << 8) + 
                    ((int)ChannelDisplayConfig.DefaultColors[i].R);
            }
            colorDialog.CustomColors = customColors;
            colorDialog.ShowDialog();
            Config.SeriesColor = colorDialog.Color;
            ColorButton.BackColor = Config.SeriesColor;
            SymbolChanged?.Invoke(sender, e);
        }
    }
}
