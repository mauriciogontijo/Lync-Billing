using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class UserSession
    {
        public static List<UserSession> sessions = new List<UserSession>();
       
        public int EmployeeID { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string SipAccount { get; set; }
        public string SiteName { get; set; }
        public string ActiveRoleName { get; set; }
        public string SrcIPAddress { get; set; }
        public string TelephoneNumber { get; set; }
        
        public DateTime LastLoginTime { get; set; }
        public int Duration { get; set; }

        public List<UserRole> UserRoles { get; set; }
    
        public static void AddUserSession(UserSession userSession)
        {
            if (!sessions.Contains(userSession))
                sessions.Add(userSession);
        }

        public static void RemoveUserSession(string emailAddress) 
        {
            foreach(UserSession session in sessions)
            {
                if (session.EmailAddress == emailAddress)
                {
                    sessions.Remove(session);
                    break;
                }
            }
        }
    }

    
}