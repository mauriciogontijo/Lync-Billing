using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Lync_Billing.Libs;

namespace Lync_Billing.DB.Roles
{
    public class SystemRole
    {
        private static DBLib DBRoutines = new DBLib();

        public int UsersRolesID { set; get; }
        public string SipAccount { get; set; }
        public int RoleID { get; set; }
        public int SiteID { get; set; }
        public int PoolID { get; set; }
        public int GatewayID { get; set; }
        public string Notes { get; set; }

        //To be used from outside the class as lookup values
        public static int DeveloperRoleID { get { return 10; } }
        public static int SystemAdminRoleID { get { return 20; } }
        public static int SiteAdminRoleID { get { return 30; } }
        public static int SiteAccountantRoleID { get { return 40; } }
        public static int DepartmentHeadRoleID { get { return 50; } }
        public static int DelegeeRoleID { get { return 60; } }


        //Generic User Roles
        public bool IsDeveloper()
        {
            return this.RoleID == 10 ? true : false; 
        }

        public bool IsSystemAdmin()
        {
            return this.RoleID == 20 ? true : false; 
        }

        public bool IsSiteAdmin()
        { 
            return this.RoleID == 30 ? true : false; 
        }

        public bool IsSiteAccountant()
        {
            return this.RoleID == 40 ? true : false; 
        }

        public bool IsDepartmentHead()
        {
            return this.RoleID == 50 ? true : false; 
        }


        //Get Users Roles
        public static List<SystemRole> GetUsersRoles(List<string> columns, Dictionary<string, object> wherePart, int limits)
        {
            SystemRole userRole;
            DataTable dt = new DataTable();
            List<SystemRole> roles = new List<SystemRole>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.SystemRoles.TableName), columns, wherePart, limits);

            foreach (DataRow row in dt.Rows)
            {
                userRole = new SystemRole();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.RoleID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.RoleID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.EmailAddress) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.SiteID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.SiteID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.PoolID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.PoolID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.GatewayID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.UsersRolesID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.UsersRolesID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.Notes))
                        userRole.Notes = (string)row[column.ColumnName];
                    
                }
                roles.Add(userRole);
            }
            return roles;
        }

        public static List<SystemRole> GetRolesPerSipAccount(string sipAccount) 
        {
            SystemRole userRole;
            DataTable dt = new DataTable();
            List<SystemRole> roles = new List<SystemRole>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.SystemRoles.TableName),"SipAccount",sipAccount);

            foreach (DataRow row in dt.Rows)
            {
                userRole = new SystemRole();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.RoleID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.RoleID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.EmailAddress) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.SiteID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.SiteID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.PoolID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.PoolID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.GatewayID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.UsersRolesID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.UsersRolesID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.Notes))
                        userRole.Notes = (string)row[column.ColumnName];

                }
                roles.Add(userRole);
            }
            return roles;


        }

        public static int InsertUserRole(SystemRole userRole)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;
          
            //Set Part
            if ((userRole.SiteID).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.SystemRoles.SiteID), userRole.SiteID);

            if ((userRole.PoolID).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.SystemRoles.PoolID), userRole.PoolID);

            if ((userRole.GatewayID).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.SystemRoles.GatewayID), userRole.GatewayID);

            if (userRole.SipAccount != null)
                columnsValues.Add(Enums.GetDescription(Enums.SystemRoles.EmailAddress), userRole.SipAccount);

            if ((userRole.RoleID).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.SystemRoles.RoleID), userRole.RoleID);

            if (userRole.Notes != null)
                columnsValues.Add(Enums.GetDescription(Enums.SystemRoles.Notes), userRole.Notes);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.SystemRoles.TableName), columnsValues, Enums.GetDescription(Enums.SystemRoles.UsersRolesID));
          
            return rowID;
        }

        public static bool UpdateUsersRole(SystemRole userRole)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();
              
            //Set Part
            if ((userRole.SiteID).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.SystemRoles.SiteID), userRole.SiteID);

            if ((userRole.PoolID).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.SystemRoles.PoolID), userRole.PoolID);

            if ((userRole.GatewayID).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.SystemRoles.GatewayID), userRole.GatewayID);

            if (userRole.SipAccount != null)
                setPart.Add(Enums.GetDescription(Enums.SystemRoles.EmailAddress), userRole.SipAccount);

            if ((userRole.RoleID).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.SystemRoles.RoleID), userRole.RoleID);

            if (userRole.Notes != null)
                setPart.Add(Enums.GetDescription(Enums.SystemRoles.Notes), userRole.Notes);

            //Execute Update
            status = DBRoutines.UPDATE(Enums.GetDescription(Enums.SystemRoles.TableName), setPart, Enums.GetDescription(Enums.SystemRoles.UsersRolesID), userRole.UsersRolesID);

            if (status == false)
            {
                //throw error message
            }
            
            return true;
        }

        public static bool DeleteFromUsersRole(SystemRole userRole)
        {
            bool status = false;

            status = DBRoutines.DELETE(
                Enums.GetDescription(Enums.SystemRoles.TableName), 
                Enums.GetDescription(Enums.SystemRoles.UsersRolesID), 
                userRole.UsersRolesID);
          
            return status;
        }

        public static bool ValidateUsersRoles(string EmailAddress, int RoleID)
        {

            DataTable dt = new DataTable();

            List<SystemRole> roles = new List<SystemRole>();
            List<string> columns = new List<string>();
            
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
           
            columns.Add(Enums.GetDescription(Enums.SystemRoles.RoleID));

            wherePart.Add(Enums.GetDescription(Enums.SystemRoles.EmailAddress), EmailAddress);
            wherePart.Add(Enums.GetDescription(Enums.SystemRoles.RoleID), RoleID);

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.SystemRoles.TableName), columns, wherePart, 0);

            if(dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
    
        
    
    }

}