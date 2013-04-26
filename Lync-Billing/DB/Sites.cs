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
        public string SiteUPN { get; set; }
        public string CountryCode { get; set; }
        public string SiteLocation { get; set; }

        public  List<Sites> GetSites(List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits)
        {
            List<Sites> sites = new List<Sites>();

            return sites;
        }

        public int InsertSite(List<Rates> rates)
        {

            return 0;
        }

        public bool UpdateRates(List<Rates> rates)
        {
            bool status = false;


            return status;
        }

        public bool DeleteSites(List<Rates> rates) 
        {
            bool status = false;


            return status;
        }

        

    }
}