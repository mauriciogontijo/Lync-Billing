using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class UserRole
    {
        public DBLib DBRoutines = new DBLib();

        public int UsersRolesID { set; get; }
        public string SipAccount { get; set; }
        public int RoleID { get; set; }
        public int SiteID { get; set; }
        public int PoolID { get; set; }
        public int GatewayID { get; set; }
        public string Notes { get; set; }

        public List<UserRole> GetUsersRoles(List<string> columns, Dictionary<string, object> wherePart, int limits)
        {
            UserRole userRole;
            DataTable dt = new DataTable();
            List<UserRole> roles = new List<UserRole>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.UsersRoles.TableName), columns, wherePart, limits);

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

        public int InsertUserRole(UserRole userRole)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;
          
            //Set Part
            if ((userRole.SiteID).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.UsersRoles.SiteID), userRole.SiteID);

            if ((userRole.PoolID).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.UsersRoles.PoolID), userRole.PoolID);

            if ((userRole.GatewayID).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.UsersRoles.GatewayID), userRole.GatewayID);

            if (userRole.SipAccount != null)
                columnsValues.Add(Enums.GetDescription(Enums.UsersRoles.SipAccount), userRole.SipAccount);

            if ((userRole.RoleID).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.UsersRoles.RoleID), userRole.RoleID);

            if (userRole.Notes != null)
                columnsValues.Add(Enums.GetDescription(Enums.UsersRoles.Notes), userRole.Notes);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.UsersRoles.TableName), columnsValues);
          
            return rowID;
        }

        public bool UpdateUsersRole(UserRole userRole)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();
              
            //Set Part
            if ((userRole.SiteID).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.UsersRoles.SiteID), userRole.SiteID);

            if ((userRole.PoolID).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.UsersRoles.PoolID), userRole.PoolID);

            if ((userRole.GatewayID).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.UsersRoles.GatewayID), userRole.GatewayID);

            if (userRole.SipAccount != null)
                setPart.Add(Enums.GetDescription(Enums.UsersRoles.SipAccount), userRole.SipAccount);

            if ((userRole.RoleID).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.UsersRoles.RoleID), userRole.RoleID);

            if (userRole.Notes != null)
                setPart.Add(Enums.GetDescription(Enums.UsersRoles.Notes), userRole.Notes);

            //Execute Update
            status = DBRoutines.UPDATE(Enums.GetDescription(Enums.UsersRoles.TableName), setPart, Enums.GetDescription(Enums.UsersRoles.UsersRolesID), userRole.UsersRolesID);

            if (status == false)
            {
                //throw error message
            }
            
            return true;
        }

        public bool DeleteFromUsersRole(UserRole userRole)
        {
            bool status = false;

            status = DBRoutines.DELETE(
                Enums.GetDescription(Enums.UsersRoles.TableName), 
                Enums.GetDescription(Enums.UsersRoles.UsersRolesID), 
                userRole.UsersRolesID);
          
            return status;
        }

        public bool ValidateUsersRoles(string SipAccount, int RoleID)
        {

            DataTable dt = new DataTable();

            List<UserRole> roles = new List<UserRole>();
            List<string> columns = new List<string>();
            
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
           
            columns.Add(Enums.GetDescription(Enums.UsersRoles.RoleID));
            
            wherePart.Add(Enums.GetDescription(Enums.UsersRoles.SipAccount), SipAccount);
            wherePart.Add(Enums.GetDescription(Enums.UsersRoles.RoleID), RoleID);

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.UsersRoles.TableName), columns, wherePart, 0);

            if(dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
    }

}