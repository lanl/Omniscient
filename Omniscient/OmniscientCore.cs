using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class OmniscientCore
    {
        /// <summary>
        /// Version of Omniscient
        /// </summary>
        public const string VERSION = "0.3.2";

        /// <summary>
        /// Contains all of the Sites in the instance of Omniscient
        /// </summary>
        public SiteManager SiteManager { get; set; }

        /// <summary>
        /// The earliest time off any data that is active
        /// </summary>
        public DateTime GlobalStart { get; set; }
        
        /// <summary>
        /// The latest time off any data that is active
        /// </summary>
        public DateTime GlobalEnd { get; set; }

        /// <summary>
        /// If any errors occur, a message is put in ErrorMessage
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
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
