using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Omniscient
{
    class AutoConfigurator
    {
        SiteManager siteManager;

        public AutoConfigurator(SiteManager manager)
        {
            siteManager = manager;
        }

        public ReturnCode ConfigFromFile(string fileName)
        {
            FileInfo info = new FileInfo(fileName);
            if (!info.Exists) return ReturnCode.FILE_DOESNT_EXIST;

            // Get instrument directory/name
            DirectoryInfo instDirectory = info.Directory;
            string instName = instDirectory.Name;
            if (instName.Length > 3 && instName[0] == '2' && instName[1] == '0' && Char.IsDigit(instName[2]) && Char.IsDigit(instName[3]))
            {
                instDirectory = info.Directory.Parent;
            }
            instName = instDirectory.Name + '_' + info.Name;

            // Get system directory/name
            DirectoryInfo sysDirectory = instDirectory.Parent;
            string sysName;
            if (sysDirectory.Exists)
            {
                sysName = sysDirectory.Name;
            }
            else
            {
                sysName = "DefaultSys";
            }

            // Get facility directory/name
            DirectoryInfo facDirectory = sysDirectory.Parent;
            string facName;
            if (facDirectory.Exists)
            {
                facName = facDirectory.Name;
            }
            else
            {
                facName = "DefaultFac";
            }

            // Find/make AutoConfig site
            List<Site> sites = siteManager.GetSites();
            Site autoConfigSite = null;
            foreach (Site site in sites)
            {
                if (site.Name == "AutoConfig")
                {
                    autoConfigSite = site;
                    break;
                }
            }
            if (autoConfigSite is null)
            {
                autoConfigSite = new Site(siteManager, "AutoConfig", 0);
            }
            Facility facility = null;
            foreach (Facility existingFacility in autoConfigSite.GetFacilities())
            {
                if (existingFacility.Name == facName)
                {
                    facility = existingFacility;
                    break;
                }
            }
            if (facility is null)
            {
                facility = new Facility(autoConfigSite, facName, 0);
            }
            DetectionSystem system = null;
            foreach (DetectionSystem existingSysten in facility.GetSystems())
            {
                if (existingSysten.Name == sysName)
                {
                    system = existingSysten;
                    break;
                }
            }
            if (system is null)
            {
                system = new DetectionSystem(facility, sysName, 0);
            }

            // Make sure the instrument doesn't already exist
            foreach(Instrument existingInst in system.GetInstruments())
            {
                if (existingInst.Name == instName)
                {
                    existingInst.Delete();
                }
            }

            Instrument inst;
            foreach (InstrumentHookup hookup in Instrument.Hookups)
            {
                inst = hookup.FromParameters(system, instName, new List<Parameter>(), 0);
                try
                {
                    if(inst.AutoIngestFile(ChannelCompartment.View, fileName) == ReturnCode.SUCCESS &&
                        inst.GetChannels()[0].GetValues(ChannelCompartment.View).Count > 0)
                    {
                        inst.FileExtension = info.Extension.Replace(".","").ToLower();
                        inst.SetDataFolder(instDirectory.FullName);
                        inst.FileMode = true;
                        inst.FileModeFile = fileName;

                        siteManager.Save();
                        return ReturnCode.SUCCESS;
                    }
                }
                catch (Exception ex)
                {
                    
                }
                inst.Delete();
            }

            siteManager.Save();

            return ReturnCode.FAIL;
        }
    }
}
