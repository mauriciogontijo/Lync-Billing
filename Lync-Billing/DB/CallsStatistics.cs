using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class CallsStatistics
    {
    }


    public class TopCountries 
    {
        public string CountryName { private set; get; }
        public long TotalDuration { private set; get; }
        public decimal TotalCost { private set; get; }
    }

    public class TopDestinations 
    {
        public string PhoneNumber { private set; get; }
        public string Internal { private set; get; }
        public long NumberOfPhoneCalls { private set; get; }

        public static List<TopDestinations> GetTopDestinations(string sipAccount)
        {
            DBLib DBRoutines = new DBLib();
            DataTable dt = new DataTable();

            List<TopDestinations> topDestinations = new List<TopDestinations>();
            
            List<object> parameters = new List<object>();

            parameters.Add(sipAccount);

            TopDestinations topDestination;

            dt = DBRoutines.SELECT_FROM_FUNCTION("fnc_GetTop5DestinationNumbersByCount", parameters, null);




            
        }
    }

}