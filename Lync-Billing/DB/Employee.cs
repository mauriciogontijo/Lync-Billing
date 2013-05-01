using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class Employee : ADUserInfo 
    {
        public static DBLib DBRoutines = new DBLib();

        public static ADUserInfo GetUserInfo(string emailAddress)
        {
            AdLib adConnector = new AdLib();
            return adConnector.getUserAttributes(emailAddress);
        }

        public static List<UserRole> GetUserRoles(string sipAccount)
        {
            UserRole userRole;
            DataTable dt = new DataTable();
            List<UserRole> roles = new List<UserRole>();

            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            wherePart.Add("SipAccount", sipAccount);

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.UsersRoles.TableName), null, wherePart, 0);

            foreach (DataRow row in dt.Rows)
            {
                userRole = new UserRole();

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