using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class Users
    {
        public static DBLib DBRoutines = new DBLib();

        public int UserID { get; set; }
        public string SipAccount { get; set; }
        public string SiteName { get; set; }

        public static List<Users> GetUsers(List<string> columns, Dictionary<string, object> wherePart, int limits)
        {
            Users user = new Users();
            DataTable dt = new DataTable();
            List<Users> users = new List<Users>();

            dt = DBRoutines.SELECT(
                Enums.GetDescription(Enums.Users.TableName),
                columns,
                wherePart,
                limits);

            foreach (DataRow row in dt.Rows)
            {
                user = new Users();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Users.UserID))
                        user.UserID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.SipAccount))
                        user.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.SiteName))
                        user.SiteName = (string)row[column.ColumnName];
                }
                users.Add(user);
            }

            return users;

        }

        public static int InsertUser(Users user)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if ((user.UserID).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Users.UserID), user.UserID);

            if ((user.SipAccount).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Users.SipAccount), user.SipAccount);

            if ((user.SiteName).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Users.SiteName), user.SiteName);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Users.TableName), columnsValues,Enums.GetDescription(Enums.Users.UserID));

            return rowID;
        }

        public static bool UpdateUser(Users user)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();
            //Set Part
             if ((user.UserID).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Users.UserID), user.UserID);
            
            if ((user.SipAccount).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Users.SipAccount), user.SipAccount);

            if (user.SiteName != null)
                setPart.Add(Enums.GetDescription(Enums.Users.SiteName), user.SiteName);

            //Execute Update
            status = DBRoutines.UPDATE(
                Enums.GetDescription(Enums.Users.TableName),
                setPart,
                Enums.GetDescription(Enums.Users.UserID), 
                user.UserID);
                

            return status;
        }

        public static bool DeleteUser(Users user)
        {
            bool status = false;

            status = DBRoutines.DELETE(
                Enums.GetDescription(Enums.Users.TableName),
                Enums.GetDescription(Enums.Users.UserID), user.UserID);

            return status;
        }


        /// <summary>
        /// Get All Related User Information From Active Directory
        /// </summary>
        /// <param name="emailAddress">User Email Address</param>
        /// <returns>Onject Holds all related user information</returns>
        public static ADUserInfo GetUserInfo(string emailAddress)
        {
            AdLib adConnector = new AdLib();
            return adConnector.getUserAttributes(emailAddress);
        }

        /// <summary>
        /// Get User Role
        /// </summary>
        /// <param name="sipAccount">User Sip Account</param>
        /// <returns>List of all User Roles</returns>
        public static List<UserRole> GetUserRoles(string sipAccount)
        {
            UserRole userRole;
            DataTable dt = new DataTable();
            List<UserRole> roles = new List<UserRole>();

            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            wherePart.Add("SipAccount", sipAccount);

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.UsersRoles.TableName), null , wherePart, 0);

            foreach (DataRow row in dt.Rows)
            {
                userRole = new UserRole();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.RoleID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.RoleID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.EmailAddress) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.EmailAddress = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.EmailAddress) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.EmailAddress = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.PoolID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.PoolID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.GatewayID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.UsersRolesID) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.UsersRolesID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.UsersRoles.Notes) && row[column.ColumnName] != System.DBNull.Value)
                        userRole.Notes = (string)row[column.ColumnName];

                }
                roles.Add(userRole);
            }
            return roles;
        }
       
    }
}