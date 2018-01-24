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

        private void SaveDECFile(string fileName)
        {
            decFile = new DECFile();
            ReturnCode returnCode = Scrape();
            if (returnCode != ReturnCode.SUCCESS) return;
            if (decFile.WriteDeclarationFile(fileName) != ReturnCode.SUCCESS)
            {
                MessageBox.Show("Error writing declaration file. Sorry!");
            }
        }

        private ReturnCode Scrape()
        {
            decFile.Facility = FacilityTextBox.Text;
            decFile.MBA = MBATextBox.Text;
            decFile.FromTime = FromDatePicker.Value.Date.Add(FromTimePicker.Value.TimeOfDay);
            decFile.ToTime = ToDatePicker.Value.Date.Add(ToTimePicker.Value.TimeOfDay);
            decFile.ItemName = ItemNameTextBox.Text;
            decFile.ItemOriginDate = OriginDatePicker.Value.Date.Add(OriginTimePicker.Value.TimeOfDay);
            decFile.FullName = FullNameTextBox.Text;
            decFile.Barcode = BarCodeTextBox.Text;
            try
            {
                decFile.Mass = double.Parse(MassTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Invalid mass!");
                return ReturnCode.BAD_INPUT;
            }
            decFile.MassDate = MassDatePicker.Value.Date.Add(MassTimePicker.Value.TimeOfDay);
            decFile.BatchName = BatchNameTextBox.Text;
            decFile.BatchSource = BatchSourceTextBox.Text;
            MaterialPanel.Composition = new NuclearComposition();
            ReturnCode returnCode = MaterialPanel.Scrape();
            if (returnCode != ReturnCode.SUCCESS) return returnCode;
            decFile.Material = MaterialPanel.Composition;
            decFile.CreationDate = CreationDatePicker.Value.Date.Add(CreationTimePicker.Value.TimeOfDay);
            decFile.ModificationDate = DateTime.Now;

            return ReturnCode.SUCCESS;
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

        private void SaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "dec files (*.dec)|*.dec|All files (*.*)|*.*";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                SaveDECFile(saveFileDialog.FileName);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }
    }
}
