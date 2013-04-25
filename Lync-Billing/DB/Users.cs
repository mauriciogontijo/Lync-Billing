using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class Users
    {
        
        public string employeeID { get; set; }
        public string userName { get; set; }
        public string UPN { get; set; }
        public List<int> PoolIDs { get; set; }
        public List<int> GatewayIDs { get; set; }
        public string RoleID { get; set; }

        private int GetUsers() 
        {

            return 0;
        }

        private List<Users> InsertUsers() 
        {
            List<Users> users = new List<Users>();

            return users;
        }

        private bool UpdateUsers()
        {
            bool status = false;


            return status;
        }

        private bool DeleteUsers() 
        {
            bool status = false;


            return status;
        }

    }
}