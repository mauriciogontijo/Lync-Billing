using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.DB;

namespace Lync_Billing.UI
{
    public class Stores
    {
        public  List<PhoneCall> phoneCallsHistoryStoreDataSource = new List<PhoneCall>();
        public  List<PhoneCall> phoeCallsmanagementStoreDataSource = new List<PhoneCall>();
        public  List<UsersCallsSummaryChartData> phoneCallsSummaryChartData = new List<UsersCallsSummaryChartData>();

        public void SET_PhoneCallsHistoryStoreDataSource(List<PhoneCall> phoneCallsHistoryStoreDataSource) 
        {
            this.phoneCallsHistoryStoreDataSource = phoneCallsHistoryStoreDataSource;
        }

        public void SET_PhoneCallsHistoryStoreDataSource(List<PhoneCall> phoeCallsmanagementStoreDataSource)
        {
            this.phoeCallsmanagementStoreDataSource = phoeCallsmanagementStoreDataSource;
        }

        public void SET_PhoneCallsHistoryStoreDataSource(List<UsersCallsSummaryChartData> phoneCallsSummaryChartData)
        {
            this.phoneCallsSummaryChartData = phoneCallsSummaryChartData;
        }

        public List<PhoneCall> GET_PhoneCallsHistoryStoreDataSource()
        {
            return phoneCallsHistoryStoreDataSource;
        }

        public List<PhoneCall> GET_PhoneCallsHistoryStoreDataSource()
        {
            return phoeCallsmanagementStoreDataSource;
        }

        public List<UsersCallsSummaryChartData> GET_PhoneCallsHistoryStoreDataSource()
        {
            return  phoneCallsSummaryChartData;
        }

        public static Stores Stores() 
        {
            return new Stores();
        }
    }
}