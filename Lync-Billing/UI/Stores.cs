using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.DB;

namespace Lync_Billing.UI
{
    public class Stores
    {
        public static List<PhoneCall> phoneCallsHistoryStoreDataSource = new List<PhoneCall>();
        public static List<PhoneCall> phoeCallsmanagementStoreDataSource = new List<PhoneCall>();
        public static List<UsersCallsSummaryChartData> phoneCallsSummaryChartData = new List<UsersCallsSummaryChartData>();
    }
}