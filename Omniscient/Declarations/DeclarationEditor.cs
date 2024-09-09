using Omniscient.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omniscient
{
    public partial class DeclarationEditor : Form
    {
        public DetectionSystem DetSystem;
        public Declaration Declaration;
        public DeclarationEditor(DetectionSystem system, Declaration declaration=null)
        {
            InitializeComponent();
            DetSystem = system;
            system.LoadDeclarations();
            this.Declaration = declaration;

            this.Text = "Declaration Editor for System: " + system.Name;

            ItemIDComboBox.Items.AddRange(system.Declarations.Keys.ToArray());

            if (declaration != null) LoadDeclaration(declaration);
            else
            {
                List<Parameter> parameters = new List<Parameter>();
                foreach(CustomParameter cParam in system.DeclarationTemplate.CustomParameters.Values)
                {
                    parameters.Add(cParam.Parameter);
                }
                ParamListPanel.LoadParameters(parameters);
                SetTabs();
            }
        }

        public ReturnCode LoadDeclaration(Declaration declaration)
        {
            ParamListPanel.LoadParameters(declaration.Parameters.Values.ToList());
            SetTabs();
            return ReturnCode.SUCCESS;
        }

        private ReturnCode Save()
        {
            // Validate Item_ID
            string itemID = ItemIDComboBox.Text;
            if (!IOUtility.ValidFileName(itemID))
            {
                MessageBox.Show("Invalid Item ID");
                return ReturnCode.FAIL;
            }

            // Validate Parameters
            if (!ParamListPanel.ValidateInput()) return ReturnCode.FAIL;

            ParamListPanel.Scrape();

            Declaration declaration = new Declaration(DetSystem);
            declaration.ItemID = itemID;
            foreach (Parameter param in ParamListPanel.Parameters)
                declaration.Parameters[param.Name] = param;

            string fileName = Path.Combine(DetSystem.GetDataDirectory(), declaration.ItemID + ".dec");
            declaration.FileName = fileName;
            try
            {
                declaration.ToFile(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save declaration for " + declaration.ItemID);
                return ReturnCode.FAIL;
            }
            Declaration = declaration;
            return ReturnCode.SUCCESS;
        }

        private void SetTabs()
        {
            int nextTab = ParamListPanel.SetTabs(2);
            OKButton.TabIndex = nextTab;
            CancelButton.TabIndex = nextTab + 1;
        }

        private void ItemIDComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DetSystem.Declarations.Keys.Contains(ItemIDComboBox.Text))
            {
                LoadDeclaration(DetSystem.Declarations[ItemIDComboBox.Text]);
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (Save() != ReturnCode.SUCCESS) return;
            DialogResult = DialogResult.OK;
            Dispose();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Dispose();
        }
    }
}
