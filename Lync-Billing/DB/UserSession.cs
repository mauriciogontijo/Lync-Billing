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
        public List<UserRole> Roles { set; get; }

    }
}