﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class UserRole
    {
        private static DBLib DBRoutines = new DBLib();

        public int UsersRolesID { set; get; }
        public string SipAccount { get; set; }
        public int RoleID { get; set; }
        public int SiteID { get; set; }
        public int PoolID { get; set; }
        public int GatewayID { get; set; }
        public string Notes { get; set; }

        //Generic User Roles
        public bool IsDeveloper
        {
            get { 
                return this.RoleID == 10 ? true : false; 
            }
        }

        public bool IsSystemAdmin
        {
            get { 
                return this.RoleID == 20 ? true : false; 
            }
        }

        public bool IsSiteAdmin
        {
            get { 
                return this.RoleID == 30 ? true : false; 
            }
        }

        public bool IsSiteAccountant
        {
            get { 
                return this.RoleID == 40 ? true : false; 
            }
        }


        //Get Users Roles
        public static List<UserRole> GetUsersRoles(List<string> columns, Dictionary<string, object> wherePart, int limits)
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
                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.RoleID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.RoleID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.EmailAddress) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.SiteID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.SiteID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.PoolID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.PoolID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.GatewayID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.UsersRolesID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.UsersRolesID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.Notes))
                        userRole.Notes = (string)row[column.ColumnName];
                    
                }
                roles.Add(userRole);
            }
            return roles;
        }

        public static List<UserRole> GetRolesPerSipAccount(string sipAccount) 
        {
            UserRole userRole;
            DataTable dt = new DataTable();
            List<UserRole> roles = new List<UserRole>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.UsersRoles.TableName),"SipAccount",sipAccount);

            foreach (DataRow row in dt.Rows)
            {
                userRole = new UserRole();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.RoleID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.RoleID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.EmailAddress) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.SiteID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.SiteID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.PoolID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.PoolID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.GatewayID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.UsersRolesID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.UsersRolesID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.Notes))
                        userRole.Notes = (string)row[column.ColumnName];

                }
                roles.Add(userRole);
            }
            return roles;


        }

        public static int InsertUserRole(UserRole userRole)
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
                columnsValues.Add(Enums.GetDescription(Enums.UsersRoles.EmailAddress), userRole.SipAccount);

            if ((userRole.RoleID).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.UsersRoles.RoleID), userRole.RoleID);

            if (userRole.Notes != null)
                columnsValues.Add(Enums.GetDescription(Enums.UsersRoles.Notes), userRole.Notes);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.UsersRoles.TableName), columnsValues, Enums.GetDescription(Enums.UsersRoles.UsersRolesID));
          
            return rowID;
        }

        public static bool UpdateUsersRole(UserRole userRole)
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
                setPart.Add(Enums.GetDescription(Enums.UsersRoles.EmailAddress), userRole.SipAccount);

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

        public static bool DeleteFromUsersRole(UserRole userRole)
        {
            bool status = false;

            status = DBRoutines.DELETE(
                Enums.GetDescription(Enums.UsersRoles.TableName), 
                Enums.GetDescription(Enums.UsersRoles.UsersRolesID), 
                userRole.UsersRolesID);
          
            return status;
        }

        public static bool ValidateUsersRoles(string EmailAddress, int RoleID)
        {

            DataTable dt = new DataTable();

            List<UserRole> roles = new List<UserRole>();
            List<string> columns = new List<string>();
            
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
           
            columns.Add(Enums.GetDescription(Enums.UsersRoles.RoleID));

            wherePart.Add(Enums.GetDescription(Enums.UsersRoles.EmailAddress), EmailAddress);
            wherePart.Add(Enums.GetDescription(Enums.UsersRoles.RoleID), RoleID);

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.UsersRoles.TableName), columns, wherePart, 0);

            if(dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
    
        
    
    }

}