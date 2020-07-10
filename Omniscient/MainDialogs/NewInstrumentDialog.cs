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
