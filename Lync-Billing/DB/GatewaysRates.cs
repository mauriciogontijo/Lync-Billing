using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class GatewaysRates
    {
        public DBLib DBRoutines = new DBLib();

        public int RatesPerGatewaysID { set; get; }
        public int GatewayID { set; get; }
        public string RatesTaleName { set; get; }
        public DateTime StartingDate { set; get; }
        public DateTime EndingDate { set; get; }
        public string ProviderName { set; get; }
        public string CurrencyCode { set; get; }

        public List<GatewaysRates> GetGatewaysRates(List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits)
        {
            List<GatewaysRates> gatewaysRates = new List<GatewaysRates>();

            return gatewaysRates;
        }

        public int InsertGatewaysRates(List<GatewaysRates> ratesPerGateways)
        {


            return 0;
        }

        public bool UpdateGatewaysRates(List<GatewaysRates> ratesPerGateways)
        {
            bool status = false;


            return status;
        }

        public bool DeleteFromGatewaysRates(List<GatewaysRates> ratesPerGateways)
        {
            bool status = false;


            return status;
        }

    }
}