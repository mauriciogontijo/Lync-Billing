using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class UsersRoles
    {
        public DBLib DBRoutines = new DBLib();
        
        public int RolePerUserID { set; get; }
        public int EmployeeID { get; set; }
        public int RoleID { get; set; }
        public int SiteID { get; set; }
        public int PoolID { get; set; }
        public int GatewayID { get; set; }
        public string Notes { get; set; }

        public List<UsersRoles> GetUsersRoles(List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits)
        {
            List<UsersRoles> usersRoles = new List<UsersRoles>();

            return usersRoles;
        }

        public int InsertUsersRoles(List<UsersRoles> rolesPerUsers)
        {


            return 0;
        }

        public bool UpdateUsersRoles(List<UsersRoles> rolesPerUsers)
        {
            bool status = false;


            return status;
        }

        public bool DeleteFromUsersRoles(List<UsersRoles> rolesPerUsers)
        {
            bool status = false;


            return status;
        }

        public bool ValidateUsersRoles(int employeeID, string RoleName)
        {

            return false;
        }

    }
}