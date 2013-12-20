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

        //The following are logical representation of existing data, they don't belong to the table
        public string SiteName { get; set; }
        public string RoleOwnerName { get; set; }
        public string RoleDescription { get; set; }

        //To be used from outside the class as lookup values
        public static int DeveloperRoleID { get { return Convert.ToInt32(Enums.GetDescription(Enums.SystemRoles.DeveloperRoleID)); } }
        public static int SystemAdminRoleID { get { return Convert.ToInt32(Enums.GetDescription(Enums.SystemRoles.SystemAdminRoleID)); } }
        public static int SiteAdminRoleID { get { return Convert.ToInt32(Enums.GetDescription(Enums.SystemRoles.SiteAdminRoleID)); } }
        public static int SiteAccountantRoleID { get { return Convert.ToInt32(Enums.GetDescription(Enums.SystemRoles.SiteAccountantRoleID)); } }
        
        //"This" System Role Flags
        public bool IsDeveloper() { return this.RoleID == DeveloperRoleID ? true : false; }
        public bool IsSystemAdmin() { return this.RoleID == SystemAdminRoleID ? true : false; }
        public bool IsSiteAdmin() { return this.RoleID == SiteAdminRoleID ? true : false; }
        public bool IsSiteAccountant() { return this.RoleID == SiteAccountantRoleID ? true : false; }

        
        public static string GetRoleDescription(int RoleTypeID)
        {
            if (RoleTypeID == DeveloperRoleID)
                return Enums.GetDescription(Enums.SystemRoles.DeveloperRoleDescription);
            else if (RoleTypeID == SystemAdminRoleID)
                return Enums.GetDescription(Enums.SystemRoles.SystemAdminRoleDescription);
            else if (RoleTypeID == SiteAdminRoleID)
                return Enums.GetDescription(Enums.SystemRoles.SiteAdminRoleDescription);
            else if (RoleTypeID == SiteAccountantRoleID)
                return Enums.GetDescription(Enums.SystemRoles.SiteAccountantRoleDescription);
            else
                return null;
        }


        //Get Users SystemRoles
        public static List<SystemRole> GetSystemRoles(List<string> columns = null, Dictionary<string, object> wherePart = null, int limits = 0)
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
                    if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.ID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.ID = (int)row[column.ColumnName];

                    else if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.RoleID) && row[column.ColumnName] != System.DBNull.Value)
                    {
                        userRole.RoleID = (int)row[column.ColumnName];
                        userRole.RoleDescription = GetRoleDescription(userRole.RoleID);
                    }

                    else if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.SipAccount) && row[column.ColumnName] != System.DBNull.Value)
                    {
                        userRole.SipAccount = (string)row[column.ColumnName];

                        var userInfo = Users.GetUser(userRole.SipAccount);
                        userRole.RoleOwnerName = HelperFunctions.FormatUserDisplayName(userInfo.FullName, userInfo.SipAccount, returnNameIfExists: true, returnAddressPartIfExists: true);
                    }

                    else if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.SiteID) && row[column.ColumnName] != System.DBNull.Value)
                    {
                        userRole.SiteID = (int)row[column.ColumnName];
                        userRole.SiteName = Site.GetSiteName(userRole.SiteID);
                    }
                    
                    else if (column.ColumnName == Enums.GetDescription(Enums.SystemRoles.Description))
                        userRole.Description = (string)row[column.ColumnName];
                }

                roles.Add(userRole);
            }

            return roles;
        }

        public static List<SystemRole> GetSystemRolesPerSipAccount(string sipAccount) 
        {
            List<string> columns = new List<string>();
            Dictionary<string, object> whereClause = new Dictionary<string, object>
            {
                { Enums.GetDescription(Enums.SystemRoles.SipAccount), sipAccount }
            };


            return GetSystemRoles(columns, whereClause, 0);
        }


        public static int AddSystemRole(SystemRole userRole)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;
          
            if(userRole.RoleID > 0 && !string.IsNullOrEmpty(userRole.RoleID.ToString()) && !string.IsNullOrEmpty(userRole.SipAccount))
            {
                columnsValues.Add(Enums.GetDescription(Enums.SystemRoles.RoleID), userRole.RoleID);
                columnsValues.Add(Enums.GetDescription(Enums.SystemRoles.SipAccount), userRole.SipAccount);

                if(userRole.SiteID > 0 && !string.IsNullOrEmpty(userRole.SiteID.ToString()))
                    columnsValues.Add(Enums.GetDescription(Enums.SystemRoles.SiteID), userRole.SiteID);

                if (!string.IsNullOrEmpty(userRole.Description))
                    columnsValues.Add(Enums.GetDescription(Enums.SystemRoles.Description), userRole.Description);

                //Execute Insert
                rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.SystemRoles.TableName), columnsValues, Enums.GetDescription(Enums.SystemRoles.ID));
            }
          
            return rowID;
        }


        public static bool UpdateUsersRole(SystemRole userRole)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            if (userRole.RoleID > 0 && !string.IsNullOrEmpty(userRole.RoleID.ToString()) && !string.IsNullOrEmpty(userRole.SipAccount))
            {
                setPart.Add(Enums.GetDescription(Enums.SystemRoles.RoleID), userRole.RoleID);
                setPart.Add(Enums.GetDescription(Enums.SystemRoles.SipAccount), userRole.SipAccount);

                if (userRole.SiteID > 0 && !string.IsNullOrEmpty(userRole.SiteID.ToString()))
                    setPart.Add(Enums.GetDescription(Enums.SystemRoles.SiteID), userRole.SiteID);

                if (userRole.Description != null)
                    setPart.Add(Enums.GetDescription(Enums.SystemRoles.Description), userRole.Description);

                //Execute Update
                status = DBRoutines.UPDATE(Enums.GetDescription(Enums.SystemRoles.TableName), setPart, Enums.GetDescription(Enums.SystemRoles.ID), userRole.ID);
            }
            
            return status;
        }


        public static bool DeleteSystemRole(SystemRole userRole)
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