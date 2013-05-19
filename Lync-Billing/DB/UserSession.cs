using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class UserSession
    { 
        public string EmailAddress { set; get; }
        public string DisplayName { set; get; }
        public string TelephoneNumber { set; get; }
        public string ActiveRoleName { set; get; }
        public string SiteName { set; get; }
        public string EmployeeID { set; get; }
        public string SipAccount { set; get; }
        public string IpAddress {set;get;}
        public string UserAgent { set; get; }

        public static List<PhoneCall> phoneCallsHistoryStoreDataSource;
        public static List<PhoneCall> phoeCallsmanagementStoreDataSource;
        public static List<UsersCallsSummaryChartData> phoneCallsSummaryChartData;


        public List<UserRole> Roles { set; get; }
        public Dictionary<string, string> ClientData { set; get; }

        private static List<UserSession> usersSessions = new List<UserSession>();

        public void AddUserSession(UserSession userSession)
        {
            if (!usersSessions.Contains(userSession))
                usersSessions.Add(userSession);
        }

        public void RemoveUserSession(UserSession userSession)
        {
            if (!usersSessions.Contains(userSession))
                usersSessions.Remove(userSession);
        }
    }

    
}