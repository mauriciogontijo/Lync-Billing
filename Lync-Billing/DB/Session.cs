using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class Session
    {
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
    }

    
}