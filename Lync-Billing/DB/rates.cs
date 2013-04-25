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

        private int GetRates()
        {

            return 0;
        }

        private List<Users> InsertRates()
        {
            List<Users> users = new List<Users>();

            return users;
        }

        private bool UpdateRates()
        {
            bool status = false;


            return status;
        }
        
    }
}