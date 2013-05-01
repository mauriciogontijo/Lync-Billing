using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class UsersRoles
    {
        public DBLib DBRoutines = new DBLib();

        public int UsersRolesID { set; get; }
        public string SipAccount { get; set; }
        public int RoleID { get; set; }
        public int SiteID { get; set; }
        public int PoolID { get; set; }
        public int GatewayID { get; set; }
        public string Notes { get; set; }

        public List<UsersRoles> GetUsersRoles(List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits)
        {
            UsersRoles userRole;
            DataTable dt = new DataTable();
            List<UsersRoles> roles = new List<UsersRoles>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.UsersRoles.TableName), columns, wherePart, limits, allFields);

            foreach (DataRow row in dt.Rows)
            {
                userRole = new UsersRoles();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.RoleID))
                        userRole.RoleID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.SipAccount))
                        userRole.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.SiteID))
                        userRole.SiteID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.PoolID))
                        userRole.PoolID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.GatewayID))
                        userRole.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.UsersRolesID))
                        userRole.UsersRolesID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.Notes))
                        userRole.Notes = (string)row[column.ColumnName];
                    
                }
                roles.Add(userRole);
            }
            return roles;
        }
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