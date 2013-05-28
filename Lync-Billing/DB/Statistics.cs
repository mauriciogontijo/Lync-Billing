using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class Statistics
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
    }

}