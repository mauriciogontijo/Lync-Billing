using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Lync_Billing.DB
{
    public class Rates
    {
        public int RateID { get; set; }
        public string GatewayName { get; set; }
        public string FixedlineRate { get; set; }
        public string MobileLineRate { get; set; }

        public string GetRatesTableName(string gatewayName) 
        {
            StringBuilder RatesTableName = new StringBuilder();
            RatesTableName.Append("Rates_" + gatewayName + "_" + DateTime.Now.Date.ToString("yyyymmdd"));
           
            return RatesTableName.ToString();
        }

        private int GetRates(string ratesTableName, List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits)
        {

            return 0;
        }

        private List<Users> InsertRates(string ratesTableName, List<Rates> rates)
        {
            List<Users> users = new List<Users>();

            return users;
        }

        private bool UpdateRates(string ratesTableName, List<Rates> rates)
        {
            bool status = false;


            return status;
        }
        
    }
}