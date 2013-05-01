using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class Users : ADUserInfo 
    {
        public DBLib DBRoutines = new DBLib();

        public ADUserInfo GetUserInfo(string emailAddress)
        {
            AdLib adConnector = new AdLib();
            return adConnector.getUserAttributes(emailAddress);
        }

        public List<UsersRoles> GetUserRoles(string emailAddress)
        {
            UsersRoles userRole;
            DataTable dt = new DataTable();
            List<UsersRoles> roles = new List<UsersRoles>();

            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            wherePart.Add("SipAccount", emailAddress);

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.UsersRoles.TableName), null, wherePart, 0, true);

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
}