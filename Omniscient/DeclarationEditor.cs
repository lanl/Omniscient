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

        public void LoadDECFile(string fileName)
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
