using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class Rates
    {
        public string GatewayName { get; set; }
        public string FixedlineRate { get; set; }
        public string MobileLineRate { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }

        public int INSERT()
        {
            return 0;
        }

        public List<Rates> SELECT()
        {
            List<Rates> rates = new List<Rates>();

            return rates;
        }

        public void UPDATE()
        {

        }

        public void DELETE()
        {

        }
    }
}