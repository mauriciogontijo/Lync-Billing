﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class RatesPerGateways
    {
        public DBLib DBRoutines = new DBLib();

        public int RatesPerGatewaysID { set; get; }
        public int GatewayID { set; get; }
        public string RatesTaleName { set; get; }
        public DateTime StartingDate { set; get; }
        public DateTime EndingDate { set; get; }
        public string ProviderName { set; get; }
        public string CurrencyCode { set; get; }

        public List<RatesPerGateways> GetRolesPerUsers(List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits)
        {
            List<RatesPerGateways> ratesPerGateways = new List<RatesPerGateways>();

            return ratesPerGateways;
        }

        public int InsertRolesPerGateways(List<RatesPerGateways> ratesPerGateways)
        {


            return 0;
        }

        public bool UpdateRolesPerGateways(List<RatesPerGateways> ratesPerGateways)
        {
            bool status = false;


            return status;
        }

        public bool DeleteRolesPerGateways(List<RatesPerGateways> ratesPerGateways)
        {
            bool status = false;


            return status;
        }

    }
}