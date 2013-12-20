using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Lync_Billing.Libs;

namespace Lync_Billing.Backend.Roles
{
    public class SystemRole
    {
        private static DBLib DBRoutines = new DBLib();

        public int ID { set; get; }
        public string SipAccount { get; set; }
        public int RoleID { get; set; }
        public int SiteID { get; set; }
        public string Description { get; set; }

        //To be used from outside the class as lookup values
        public static int DeveloperRoleID { get { return Convert.ToInt32(Enums.GetDescription(Enums.SystemRoles.DeveloperRoleID)); } }
        public static int SystemAdminRoleID { get { return Convert.ToInt32(Enums.GetDescription(Enums.SystemRoles.SystemAdminRoleID)); } }
        public static int SiteAdminRoleID { get { return Convert.ToInt32(Enums.GetDescription(Enums.SystemRoles.SiteAdminRoleID)); } }
        public static int SiteAccountantRoleID { get { return Convert.ToInt32(Enums.GetDescription(Enums.SystemRoles.SiteAccountantRoleID)); } }
        

        //Generic User SystemRoles
        public bool IsDeveloper()
        {
            return this.RoleID == DeveloperRoleID ? true : false; 
        }

        public bool IsSystemAdmin()
        {
            return this.RoleID == SystemAdminRoleID ? true : false; 
        }

        public bool IsSiteAdmin()
        {
            return this.RoleID == SiteAdminRoleID ? true : false; 
        }

        public bool IsSiteAccountant()
        {
            return this.RoleID == SiteAccountantRoleID ? true : false; 
        }


        //Get Users SystemRoles
        public static List<SystemRole> GetSystemRoles(List<string> columns, Dictionary<string, object> wherePart, int limits)
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

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.SipAccount) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.SiteID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.SiteID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.ID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.ID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.Description))
                        userRole.Description = (string)row[column.ColumnName];
                    
                }
                roles.Add(userRole);
            }
            return roles;
        }

        public static List<SystemRole> GetSystemRolesPerSipAccount(string sipAccount) 
        {
            SystemRole userRole;
            DataTable dt = new DataTable();
            List<SystemRole> roles = new List<SystemRole>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.SystemRoles.TableName), Enums.GetDescription(Enums.SystemRoles.SipAccount), sipAccount);

            foreach (DataRow row in dt.Rows)
            {
                userRole = new SystemRole();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.RoleID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.RoleID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.SipAccount) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.SiteID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.SiteID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.ID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.ID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.Description))
                        userRole.Description = (string)row[column.ColumnName];

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

            if (userRole.SipAccount != null)
                columnsValues.Add(Enums.GetDescription(Enums.SystemRoles.SipAccount), userRole.SipAccount);

            if ((userRole.RoleID).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.SystemRoles.RoleID), userRole.RoleID);

            if (userRole.Description != null)
                columnsValues.Add(Enums.GetDescription(Enums.SystemRoles.Description), userRole.Description);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.SystemRoles.TableName), columnsValues, Enums.GetDescription(Enums.SystemRoles.ID));
          
            return rowID;
        }


        public static bool UpdateUsersRole(SystemRole userRole)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();
              
            //Set Part
            if ((userRole.SiteID).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.SystemRoles.SiteID), userRole.SiteID);

            if (userRole.SipAccount != null)
                setPart.Add(Enums.GetDescription(Enums.SystemRoles.SipAccount), userRole.SipAccount);

            if ((userRole.RoleID).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.SystemRoles.RoleID), userRole.RoleID);

            if (userRole.Description != null)
                setPart.Add(Enums.GetDescription(Enums.SystemRoles.Description), userRole.Description);

            //Execute Update
            status = DBRoutines.UPDATE(Enums.GetDescription(Enums.SystemRoles.TableName), setPart, Enums.GetDescription(Enums.SystemRoles.ID), userRole.ID);
                        
            return status;
        }


        public static bool DeleteFromUsersRole(SystemRole userRole)
        {
            bool status = false;

            status = DBRoutines.DELETE(Enums.GetDescription(Enums.SystemRoles.TableName), Enums.GetDescription(Enums.SystemRoles.ID), userRole.ID);
          
            return status;
        }


        public static bool ValidateUsersRoles(string EmailAddress, int RoleID)
        {

            DataTable dt = new DataTable();

            List<SystemRole> roles = new List<SystemRole>();
            List<string> columns = new List<string>();
            
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
           
            columns.Add(Enums.GetDescription(Enums.SystemRoles.RoleID));

            wherePart.Add(Enums.GetDescription(Enums.SystemRoles.SipAccount), EmailAddress);
            wherePart.Add(Enums.GetDescription(Enums.SystemRoles.RoleID), RoleID);

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.SystemRoles.TableName), columns, wherePart, 0);

            
            return (dt.Rows.Count > 0) ? true : false;
        }

    }

}