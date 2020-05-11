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
            Instrument existingInst;
            for (int i=system.GetInstruments().Count-1; i>=0; i--)
            {
                existingInst = system.GetInstruments()[i];
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
