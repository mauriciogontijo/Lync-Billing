using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class Users
    {
        
        public string EmployeeID { get; set; }
        public string LoginName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string SiteName { get; set; }
        public string PoolFQDN { get; set; }
        public string UserUPN { get; set; }
        public bool IsNormalUser { get; set; }

        private List<Users> GetUsers(List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits) 
        {
            List<Users> users = new List<Users>();

            return users;
            
        }

        private int InsertUsers(List<Users> users) 
        {
            

            return 0;
        }

        private bool UpdateUsers(List<Users> users)
        {
            bool status = false;


            return status;
        }
    }
}