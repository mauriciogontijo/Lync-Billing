using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class RolesPerUsers
    {
        public DBLib DBRoutines = new DBLib();
        
        public int RolePerUserID { set; get; }
        public int EmployeeID { get; set; }
        public int RoleID { get; set; }
        public int SiteID { get; set; }
        public int PoolID { get; set; }
        public int GatewayID { get; set; }
        public string Notes { get; set; }

        public List<RolesPerUsers> GetRolesPerUsers(List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits)
        {
            List<RolesPerUsers> rolesPerUsers = new List<RolesPerUsers>();

            return rolesPerUsers;
        }

        public int InsertRolesPerUsers(List<RolesPerUsers> rolesPerUsers)
        {


            return 0;
        }

        public bool UpdateRolesPerUsers(List<RolesPerUsers> rolesPerUsers)
        {
            bool status = false;


            return status;
        }

        public bool DeleteRolesPerUsers(List<RolesPerUsers> rolesPerUsers)
        {
            bool status = false;


            return status;
        }

        public bool ValidateRolePerUsers(int employeeID, string RoleName)
        {

            return false;
        }

    }
}