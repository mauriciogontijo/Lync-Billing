using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class rates
    {
        public string RateTableName { get; set; }
        public string FixedlineRate { get; set; }
        public string MobileLineRate { get; set; }
    }
}