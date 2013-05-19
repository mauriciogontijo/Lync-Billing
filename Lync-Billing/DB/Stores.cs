using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.DB;

namespace Lync_Billing.UI
{
    public class Stores
    {
        public List<PhoneCall> phoneCallsHistoryStoreDataSource;
        public List<PhoneCall> phoeCallsmanagementStoreDataSource;
        public List<UsersCallsSummaryChartData> phoneCallsSummaryChartData;
    }
}