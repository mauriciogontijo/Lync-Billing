﻿using System;
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

        public int InsertUsersRoles(List<UsersRoles> usersRoles)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            foreach (UsersRoles userRole in usersRoles)
            {

                //Set Part
                if (userRole.SiteID != null)
                    columnsValues.Add(Enums.GetDescription(Enums.UsersRoles.SiteID), userRole.SiteID);

                if (userRole.PoolID != null)
                    columnsValues.Add(Enums.GetDescription(Enums.UsersRoles.PoolID), userRole.PoolID);

                if (userRole.GatewayID != null)
                    columnsValues.Add(Enums.GetDescription(Enums.UsersRoles.GatewayID), userRole.GatewayID);

                if (userRole.SipAccount != null)
                    columnsValues.Add(Enums.GetDescription(Enums.UsersRoles.SipAccount), userRole.SipAccount);

                if (userRole.RoleID != null)
                    columnsValues.Add(Enums.GetDescription(Enums.UsersRoles.RoleID), userRole.RoleID);

                if (userRole.Notes != null)
                    columnsValues.Add(Enums.GetDescription(Enums.UsersRoles.Notes), userRole.Notes);

                //Execute Insert
                rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.UsersRoles.TableName), columnsValues);
            }
            return rowID;
        }

        public bool UpdateUsersRoles(List<UsersRoles> usersRoles,int ID)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();
          
            foreach (UsersRoles userRole in usersRoles)
            {
              
                //Set Part
                if (userRole.SiteID != null)
                    setPart.Add(Enums.GetDescription(Enums.UsersRoles.SiteID), userRole.SiteID);

                if (userRole.PoolID != null)
                    setPart.Add(Enums.GetDescription(Enums.UsersRoles.PoolID), userRole.PoolID);

                if (userRole.GatewayID != null)
                    setPart.Add(Enums.GetDescription(Enums.UsersRoles.GatewayID), userRole.GatewayID);

                if (userRole.SipAccount != null)
                    setPart.Add(Enums.GetDescription(Enums.UsersRoles.SipAccount), userRole.SipAccount);

                if (userRole.RoleID != null)
                    setPart.Add(Enums.GetDescription(Enums.UsersRoles.RoleID), userRole.RoleID);

                if (userRole.Notes != null)
                    setPart.Add(Enums.GetDescription(Enums.UsersRoles.Notes), userRole.Notes);

                //Execute Update
                status = DBRoutines.UPDATE(Enums.GetDescription(Enums.UsersRoles.TableName), setPart, Enums.GetDescription(Enums.UsersRoles.UsersRolesID),ID);

                if (status == false)
                {
                    //throw error message
                }
            }
            return true;
        }

        public bool DeleteFromUsersRoles(int ID)
        {
            bool status = false;

            status = DBRoutines.DELETE(Enums.GetDescription(Enums.UsersRoles.TableName), Enums.GetDescription(Enums.UsersRoles.UsersRolesID), ID);
          
            return status;
        }

        public bool ValidateUsersRoles(string SipAccount, int RoleID)
        {

            DataTable dt = new DataTable();

            List<UsersRoles> roles = new List<UsersRoles>();
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