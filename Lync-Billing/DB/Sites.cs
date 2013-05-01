using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class Sites
    {
        public DBLib DBRoutines = new DBLib();
        
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string CountryCode { get; set; }
        public string SiteLocation { get; set; }

        public  List<Sites> GetSites(List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits)
        {
            List<Sites> sites = new List<Sites>();

            return sites;
        }

        public int InsertSite(Rates rate)
        {

            return 0;
        }

        public bool UpdateSite(Rates rate)
        {
            bool status = false;


            return status;
        }

        public bool DeleteSite(Rates rate)
        {
            bool status = false;


            return status;
        }

        

    }
}