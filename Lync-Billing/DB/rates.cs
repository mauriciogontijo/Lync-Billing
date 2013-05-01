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

        public List<Users> GetRates(string ratesTableName, List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits)
        {
            List<Users> users = new List<Users>();

            return users;
            
        }

        public  int InsertRate(Rates rates)
        {
            return 0;
        }

        public bool UpdateRate(Rates rates)
        {
            bool status = false;


            return status;
        }
        
    }
}