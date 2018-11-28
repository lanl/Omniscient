using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Omniscient
{
    public partial class ParameterPanel : UserControl
    {
        public Parameter parameter;

        private List<Control> paramControls;

        public ParameterPanel(Parameter param)
        {
            parameter = param;
            paramControls = new List<Control>();
            InitializeComponent();

            this.SuspendLayout();
            switch (parameter.Type)
            {
                case ParameterType.Bool:
                    InitializeCheckBox();
                    break;
                case ParameterType.String:
                case ParameterType.Int:
                case ParameterType.Double:
                    InitializeSimpleTextBox();
                    break;
                case ParameterType.Enum:
                case ParameterType.SystemChannel:
                case ParameterType.SystemEventGenerator:
                case ParameterType.InstrumentChannel:
                    InitializeLimitedValues();
                    break;
                case ParameterType.TimeSpan:
                    InitializeTimeSpan();
                    break;
                case ParameterType.FileName:
                    InitializeFileName();
                    break;
                case ParameterType.Directory:
                    InitializeDirectory();
                    break;
            }
            this.ResumeLayout();
            NameLabel.Text = param.Name +":";
        }

        private void InitializeCheckBox()
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Dock = System.Windows.Forms.DockStyle.Left;
            checkBox.Margin = new System.Windows.Forms.Padding(5);
            checkBox.Checked = parameter.Value == BoolParameter.True;
            paramControls.Add(checkBox);
            this.Controls.Add(checkBox);
            checkBox.BringToFront();
        }

        public bool Scrape()
        {
            switch (parameter.Type)
            {
                case ParameterType.Bool:
                    if (((CheckBox)paramControls[0]).Checked) parameter.Value = BoolParameter.True;
                    else parameter.Value = BoolParameter.False;
                    break;
                case ParameterType.FileName:
                case ParameterType.Directory:
                case ParameterType.String:
                case ParameterType.Int:
                case ParameterType.Double:
                    parameter.Value = ((TextBox)paramControls[0]).Text;
                    break;
                case ParameterType.Enum:
                case ParameterType.SystemChannel:
                case ParameterType.SystemEventGenerator:
                case ParameterType.InstrumentChannel:
                    parameter.Value = ((ComboBox)paramControls[0]).Text;
                    break;
                case ParameterType.TimeSpan:
                    double val = 0;
                    double textVal;
                    try { textVal = double.Parse(((TextBox)paramControls[0]).Text); }
                    catch { return false; }
                    switch (((ComboBox)paramControls[1]).Text)
                    {
                        case "Seconds":
                            val = textVal;
                            break;
                        case "Minutes":
                            val = textVal*60;
                            break;
                        case "Hours":
                            val = textVal * 3600;
                            break;
                        case "Days":
                            val = textVal * 3600 * 24;
                            break;
                    }
                    parameter.Value = val.ToString();
                    break;
            }
            return true;
        }

        public bool ValidateInput()
        {
            Scrape();
            return parameter.Validate();
        }

        public Parameter GetInput()
        {
            Scrape();
            return parameter;
        }

        private void InitializeFileName()
        {
            TextBox textBox = new TextBox();
            textBox.Dock = System.Windows.Forms.DockStyle.Left;
            textBox.Margin = new System.Windows.Forms.Padding(5);
            textBox.Width = 180;
            textBox.Text = parameter.Value;
            paramControls.Add(textBox);
            this.Controls.Add(textBox);
            textBox.BringToFront();

            Button button = new Button();
            button.Dock = DockStyle.Left;
            button.Margin = new Padding(5);
            button.Width = 20;
            button.Height = 20;
            button.Image = Properties.Resources.OpenIcon;
            button.Click += OpenFileButton_Click;
            paramControls.Add(button);
            this.Controls.Add(button);
            button.BringToFront();
        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (File.Exists(((TextBox)paramControls[0]).Text))
            {
                FileInfo info = new FileInfo(((TextBox)paramControls[0]).Text);
                dialog.InitialDirectory = info.DirectoryName;
            }
            else if (Directory.Exists(((TextBox)paramControls[0]).Text))
            {
                dialog.InitialDirectory = ((TextBox)paramControls[0]).Text;
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ((TextBox)paramControls[0]).Text = dialog.FileName;
            }
        }

        private void InitializeDirectory()
        {
            TextBox textBox = new TextBox();
            textBox.Dock = System.Windows.Forms.DockStyle.Left;
            textBox.Margin = new System.Windows.Forms.Padding(5);
            textBox.Width = 180;
            textBox.Text = parameter.Value;
            paramControls.Add(textBox);
            this.Controls.Add(textBox);
            textBox.BringToFront();

            Button button = new Button();
            button.Dock = DockStyle.Left;
            button.Margin = new Padding(5);
            button.Width = 20;
            button.Height = 20;
            button.Image = Properties.Resources.OpenIcon;
            button.Click += OpenDirectory_Click;
            paramControls.Add(button);
            this.Controls.Add(button);
            button.BringToFront();
        }

        private void OpenDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            if (Directory.Exists(((TextBox)paramControls[0]).Text))
            {
                dialog.SelectedPath = ((TextBox)paramControls[0]).Text;
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ((TextBox)paramControls[0]).Text = dialog.SelectedPath;
            }
        }

        private void InitializeSimpleTextBox()
        {
            TextBox textBox = new TextBox();
            textBox.Dock = System.Windows.Forms.DockStyle.Left;
            textBox.Margin = new System.Windows.Forms.Padding(5);
            textBox.Width = 120;
            textBox.Text = parameter.Value;
            paramControls.Add(textBox);
            this.Controls.Add(textBox);
            textBox.BringToFront();
        }

        private void InitializeLimitedValues()
        {
            ComboBox comboBox = new ComboBox();
            comboBox.Dock = DockStyle.Left;
            comboBox.Margin = new Padding(5);
            comboBox.Width = 180;
            foreach(string value in ((LimitedValueParameter)parameter).ValidValues)
            {
                comboBox.Items.Add(value);
            }
            comboBox.Text = parameter.Value;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            paramControls.Add(comboBox);
            Controls.Add(comboBox);
            comboBox.BringToFront();
        }

        private void InitializeTimeSpan()
        {
            TextBox textBox = new TextBox();
            textBox.Dock = System.Windows.Forms.DockStyle.Left;
            textBox.Margin = new System.Windows.Forms.Padding(5);
            textBox.Width = 60;
            paramControls.Add(textBox);
            this.Controls.Add(textBox);
            textBox.BringToFront();

            ComboBox comboBox = new ComboBox();
            comboBox.Dock = DockStyle.Left;
            comboBox.Margin = new Padding(5);
            comboBox.Width = 120;
            comboBox.Items.AddRange(new string[] { "Seconds", "Minutes", "Hours", "Days" });
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            double val = double.Parse(parameter.Value);
            comboBox.Text = "Seconds";
            textBox.Text = val.ToString();
            if (val>0)
            {
                if(val%(3600*24)==0)
                {
                    comboBox.Text = "Days";
                    textBox.Text = (val / (3600 * 24)).ToString();
                }
                else if (val % 3600 == 0)
                {
                    comboBox.Text = "Hours";
                    textBox.Text = (val / 3600).ToString();
                }
                else if (val % 60 == 0)
                {
                    comboBox.Text = "Minutes";
                    textBox.Text = (val / 60).ToString();
                }
            }

            paramControls.Add(comboBox);
            Controls.Add(comboBox);
            comboBox.BringToFront();
        }
    }
}
