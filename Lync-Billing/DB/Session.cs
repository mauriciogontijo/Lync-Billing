using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class Session
    {
        public static List<Session> sessions = new List<Session>();
       
        public int EmployeeID { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string SipAccount { get; set; }
        public string SiteName { get; set; }
        public string ActiveRoleName { get; set; }
        public string SrcIPAddress { get; set; }
        
        public DateTime LastLoginTime { get; set; }
        public int Duration { get; set; }

        public List<UserRole> userRoles { get; set; }
    
        public static void AddUserSession(Session userSession)
        {
            if (!sessions.Contains(userSession))
                sessions.Add(userSession);
        }

        public static void RemoveUserSession(string emailAddress) 
        {
            foreach(Session session in sessions)
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