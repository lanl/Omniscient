/*
This source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

This source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2020, International Atomic Energy Agency (IAEA), IAEA.org

   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of IAEA nor the names of its contributors
      may be used to endorse or promote products derived from this software
      without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS"AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
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
    public partial class NewInstrumentDialog : Form
    {
        public Site SelectedSite { get; set; }
        public Facility SelectedFacility { get; set; }
        public DetectionSystem SelectedSystem { get; set; }
        private OmniscientCore Core;

        public NewInstrumentDialog(OmniscientCore core, Site site=null, Facility facility=null, DetectionSystem system=null)
        {
            Core = core;
            SelectedSite = site;
            SelectedFacility = facility;
            SelectedSystem = system;

            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
        }

        private void NewInstrumentDialog_Load(object sender, EventArgs e)
        {
            foreach(Site site in Core.SiteManager.GetSites())
            {
                SiteComboBox.Items.Add(site.Name);
            }

            if (SelectedSite != null)
            {
                SiteComboBox.Text = SelectedSite.Name;
            }
            if (SelectedFacility != null)
            {
                FacilityComboBox.Text = SelectedFacility.Name;
            }
            if (SelectedSystem != null)
            {
                SystemComboBox.Text = SelectedSystem.Name;
            }
        }

        private void SetSite(Site site)
        {
            SelectedSite = site;
            FacilityComboBox.Items.Clear();
            foreach (Facility facility in site.GetFacilities())
            {
                FacilityComboBox.Items.Add(facility.Name);
            }
        }

        private void SetFacility(Facility facility)
        {
            SelectedFacility = facility;
            SystemComboBox.Items.Clear();
            foreach (DetectionSystem system in facility.GetSystems())
            {
                SystemComboBox.Items.Add(system.Name);
            }
        }

        private void SiteComboBox_TextUpdate(object sender, EventArgs e)
        {
            foreach (Site site in Core.SiteManager.GetSites())
            {
                if (SiteComboBox.Text == site.Name)
                {
                    SetSite(site);
                    return;
                }
            }
        }

        private void FacilityComboBox_TextUpdate(object sender, EventArgs e)
        {
            if (SelectedSite is null) return;
            foreach (Facility facility in SelectedSite.GetFacilities())
            {
                if (FacilityComboBox.Text == facility.Name)
                {
                    SetFacility(facility);
                    return;
                }
            }
        }

        private void SystemComboBox_TextUpdate(object sender, EventArgs e)
        {
            if (SelectedFacility is null) return;
            foreach (DetectionSystem system in SelectedFacility.GetSystems())
            {
                if (SystemComboBox.Text == system.Name)
                {
                    SelectedSystem = system;
                    return;
                }
            }
        }

        private void SiteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SiteComboBox_TextUpdate(sender, e);
        }

        private void FacilityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FacilityComboBox_TextUpdate(sender, e);
        }
        private void SystemComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SystemComboBox_TextUpdate(sender, e);
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Dispose();
        }

        private void Next()
        {
            SiteManager siteMan = Core.SiteManager;
            string siteText = SiteComboBox.Text;
            string facilityText = FacilityComboBox.Text;
            string systemText = SystemComboBox.Text;

            // Site
            SelectedSite = null;
            foreach (Site site in Core.SiteManager.GetSites())
            {
                if (siteText == site.Name)
                {
                    SelectedSite = site;
                    break;
                }
            }
            if (SelectedSite is null)
            {
                if (siteText == "")
                {
                    MessageBox.Show("Choose or name a Site!");
                    return;
                }
                else
                {
                    if (MessageBox.Show("Create new Site: " + siteText + "?", "New Site", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        return;
                    SelectedSite = new Site(siteMan, siteText, 0);
                    siteMan.Save();
                }
            }

            // Facility
            SelectedFacility = null;
            foreach (Facility facility in SelectedSite.GetFacilities())
            {
                if (FacilityComboBox.Text == facility.Name)
                {
                    SelectedFacility = facility;
                    break;
                }
            }
            if (SelectedFacility is null)
            {
                if (facilityText == "")
                {
                    MessageBox.Show("Choose or name a Facility!");
                    return;
                }
                else
                {
                    if (MessageBox.Show("Create new Facility: " + facilityText + "?", "New Facility", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        return;
                    SelectedFacility = new Facility(SelectedSite, facilityText, 0);
                    siteMan.Save();
                }
            }

            // System
            SelectedSystem = null;
            foreach (DetectionSystem system in SelectedFacility.GetSystems())
            {
                if (SystemComboBox.Text == system.Name)
                {
                    SelectedSystem = system;
                    break;
                }
            }
            if (SelectedSystem is null)
            {
                if (systemText == "")
                {
                    MessageBox.Show("Choose or name a System!");
                    return;
                }
                else
                {
                    if (MessageBox.Show("Create new System: " + systemText + "?", "New System", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        return;
                    SelectedSystem = new DetectionSystem(SelectedFacility, systemText, 0);
                    siteMan.Save();
                }
            }
            DialogResult = DialogResult.OK;
            Dispose();
        }

        private void InstrumentButton_Click(object sender, EventArgs e)
        {
            Next();
        }

        private void SiteComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Next();
            }
        }

        private void FacilityComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Next();
            }
        }

        private void SystemComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Next();
            }
        }
    }
}
