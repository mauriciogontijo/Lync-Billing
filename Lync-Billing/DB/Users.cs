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
        public string UPN { get; set; }
        public List<int> PoolIDs { get; set; }
        public List<int> GatewayIDs { get; set; }
        public string RoleID { get; set; }

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