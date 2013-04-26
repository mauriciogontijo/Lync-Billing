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
    }
}