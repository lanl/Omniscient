using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class OmniscientCore
    {
        public const string VERSION = "0.3.2";

        public SiteManager SiteManager { get; set; }

        public DateTime GlobalStart { get; set; }
        public DateTime GlobalEnd { get; set; }

        public string ErrorMessage { get; private set; }
        public OmniscientCore()
        {
            ErrorMessage = "";
            SiteManager = new SiteManager("SiteManager.xml", VERSION);
            ReturnCode returnCode = SiteManager.Reload();
            if (returnCode == ReturnCode.FILE_DOESNT_EXIST)
            {
                SiteManager.WriteBlank();
            }
            else if (returnCode != ReturnCode.SUCCESS)
            {
                ErrorMessage = "Warning: Bad trouble loading the site manager!";
            }

            GlobalStart = DateTime.Today.AddDays(-1);
            GlobalEnd = DateTime.Today;
        }
    }
}
