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

        public int INSERT() 
        {
            return 0;
        }

        public List<Users> SELECT() 
        {
            List<Users> users = new List<Users>();

            return users;
        }

        public void UPDATE()
        {

        }

        public void DELETE() 
        {

        }

    }
}