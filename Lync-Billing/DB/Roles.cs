using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;


namespace Lync_Billing.DB
{
    public class Roles
    {
        public DBLib DBRoutines = new DBLib();

        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }

        public List<Roles> GetRoles(List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits)
        {
            List<Roles> roles = new List<Roles>();

            return roles;

        }

        public int InsertRoles(List<Roles> roles)
        {


            return 0;
        }

        public bool UpdateRoles(List<Roles> roles)
        {
            bool status = false;


            return status;
        }

        public bool DeleteRoles(List<Roles> roles)
        {
            bool status = false;


            return status;
        }

    }
}