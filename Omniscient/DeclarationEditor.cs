using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omniscient
{
    public partial class DeclarationEditor : Form
    {
        DECFile decFile;

        public DeclarationEditor()
        {
            decFile = new DECFile();
            InitializeComponent();
        }

        private void LoadDECFile(string fileName)
        {
            if (decFile.ParseDeclarationFile(fileName) == ReturnCode.SUCCESS)
            {
                FacilityTextBox.Text = decFile.Facility;
                MBATextBox.Text = decFile.MBA;
                FromTimePicker.Value = decFile.FromTime;
                FromDatePicker.Value = decFile.FromTime;
                ToTimePicker.Value = decFile.ToTime;
                ToDatePicker.Value = decFile.ToTime;

                ItemNameTextBox.Text = decFile.ItemName;
                OriginTimePicker.Value = decFile.ItemOriginDate;
                OriginDatePicker.Value = decFile.ItemOriginDate;
                FullNameTextBox.Text = decFile.FullName;
                BarCodeTextBox.Text = decFile.Barcode;
                MassTextBox.Text = decFile.Mass.ToString();
                MassTimePicker.Value = decFile.MassDate;
                MassDatePicker.Value = decFile.MassDate;

                BatchNameTextBox.Text = decFile.BatchName;
                BatchSourceTextBox.Text = decFile.BatchSource;
                MaterialPanel.Composition = decFile.Material;
                MaterialPanel.UpdateFields();

                CreationTimePicker.Value = decFile.CreationDate;
                ModificationDatePicker.Value = decFile.ModificationDate;
            }
            else
            {
                MessageBox.Show("Error opening file!");
            }
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "dec files (*.dec)|*.dec|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadDECFile(openFileDialog.FileName);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }
    }
}
