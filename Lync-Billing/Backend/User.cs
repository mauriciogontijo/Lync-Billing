using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Lync_Billing.Libs;
using Lync_Billing.Backend.Roles;

namespace Lync_Billing.Backend
{
    public class User
    {
        public static DBLib DBRoutines = new DBLib();
        
        //Default class instance variables.
        public int EmployeeID { get; set; }
        public string FullName { get; set; }
        public string SipAccount { get; set; }
        public string SiteName { get; set; }
        public string Department { get; set; }
        public string TelephoneNumber { get; set; }
        
        //This is a logical representation, it doesn't exist in the database.
        //This is a copy from the ADUserInfo record for this user.
        public string DisplayName { get; set; }

        public static List<User> GetUsers(List<string> columns, Dictionary<string, object> wherePart, int limits)
        {
            User user = new User();
            DataTable dt = new DataTable();
            List<User> users = new List<User>();

            dt = DBRoutines.SELECT(
                Enums.GetDescription(Enums.Users.TableName),
                columns,
                wherePart,
                limits);

            foreach (DataRow row in dt.Rows)
            {
                user = new User();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Users.EmployeeID))
                        user.EmployeeID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.SipAccount))
                        user.SipAccount = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.SiteName))
                        user.SiteName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.DisplayName))
                    {
                        user.FullName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                        user.DisplayName = HelperFunctions.FormatUserDisplayName(user.FullName, user.SipAccount);
                    }

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.Department))
                        user.Department = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.TelephoneNumber))
                        user.TelephoneNumber = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }

                users.Add(user);
            }

            return users;

        }

        public static List<User> GetUsers(string siteName) 
        {
            User user = new User();
            DataTable dt = new DataTable();
            List<User> users = new List<User>();

            dt = DBRoutines.SELECT(
                Enums.GetDescription(Enums.Users.TableName),
                Enums.GetDescription(Enums.Users.SiteName),
                siteName);

            foreach (DataRow row in dt.Rows)
            {
                user = new User();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Users.EmployeeID))
                        user.EmployeeID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.SipAccount))
                        user.SipAccount = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.SiteName))
                        user.SiteName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.DisplayName))
                    {
                        user.FullName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                        user.DisplayName = HelperFunctions.FormatUserDisplayName(user.FullName, user.SipAccount);
                    }

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.Department))
                        user.Department = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.TelephoneNumber))
                        user.TelephoneNumber = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }

                users.Add(user);
            }

            return users;
        }

        public static List<User> SearchForUsers(string searchTerm)
        {
            User user = new User();
            DataTable dt = new DataTable();
            List<User> users = new List<User>();

            List<string> columns = new List<string>()
            {
                Enums.GetDescription(Enums.Users.DisplayName),
                Enums.GetDescription(Enums.Users.SipAccount)
            };

            Dictionary<string, object> wherePart = new Dictionary<string, object>()
            {
                { Enums.GetDescription(Enums.Users.SipAccount), String.Format("like '%{0}%'", searchTerm) },
                { Enums.GetDescription(Enums.Users.DisplayName), String.Format("like '%{0}%'", searchTerm) }
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Users.TableName), columns, wherePart, 0, setWhereStatementOperatorToOR: true);

            foreach (DataRow row in dt.Rows)
            {
                user = new User();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Users.EmployeeID))
                        user.EmployeeID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.SipAccount))
                        user.SipAccount = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.SiteName))
                        user.SiteName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.DisplayName))
                    {
                        user.FullName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                        user.DisplayName = HelperFunctions.FormatUserDisplayName(user.FullName, user.SipAccount, returnNameIfExists: true);
                    }

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.Department))
                        user.Department = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.TelephoneNumber))
                        user.TelephoneNumber = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }

                users.Add(user);
            }

            return users;
        }

        public static User GetUser(string sipAccount) 
        {
            User user = new User();
            DataTable dt = new DataTable();

            dt = DBRoutines.SELECT(
                Enums.GetDescription(Enums.Users.TableName),
                Enums.GetDescription(Enums.Users.SipAccount),
                sipAccount);

            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];

                foreach(DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Users.EmployeeID))
                        user.EmployeeID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.SipAccount))
                        user.SipAccount = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.SiteName))
                        user.SiteName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.DisplayName))
                    {
                        user.FullName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                        user.DisplayName = HelperFunctions.FormatUserDisplayName(user.FullName, user.SipAccount);
                    }

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.Department))
                        user.Department = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.TelephoneNumber))
                        user.TelephoneNumber = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }

                return user;
            }
            else 
            {
                return null;
            }
            
        }

        public static User GetUser(int employeeID)
        {
            User user = new User();
            DataTable dt = new DataTable();

            dt = DBRoutines.SELECT(
                Enums.GetDescription(Enums.Users.TableName),
                Enums.GetDescription(Enums.Users.EmployeeID),
                employeeID);

            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Users.EmployeeID))
                        user.EmployeeID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.SipAccount))
                        user.SipAccount = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.SiteName))
                        user.SiteName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.DisplayName))
                    {
                        user.FullName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                        user.DisplayName = HelperFunctions.FormatUserDisplayName(user.FullName, user.SipAccount);
                    }

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.Department))
                        user.Department = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.Users.TelephoneNumber))
                        user.TelephoneNumber = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }

                return user;
            }
            else
            {
                return null;
            }

        }

        public static string GetUserSite(string sipAccount)
        {
            User user = new User();
            DataTable dt = new DataTable();

            dt = DBRoutines.SELECT(
                Enums.GetDescription(Enums.Users.TableName),
                Enums.GetDescription(Enums.Users.SipAccount),
                sipAccount);

            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                return Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[Enums.GetDescription(Enums.Users.SiteName)]));
            }
            else
                return null;
        }

        public static int InsertUser(User user)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            columnsValues.Add(Enums.GetDescription(Enums.Users.EmployeeID), user.EmployeeID);

            if (!string.IsNullOrEmpty(user.SipAccount))
                columnsValues.Add(Enums.GetDescription(Enums.Users.SipAccount), user.SipAccount);

            if (!string.IsNullOrEmpty(user.SiteName))
                columnsValues.Add(Enums.GetDescription(Enums.Users.SiteName), user.SiteName.ToString());
            else
                columnsValues.Add(Enums.GetDescription(Enums.Users.SiteName), "UNIDENTIFIED");

            if (!string.IsNullOrEmpty(user.FullName))
                columnsValues.Add(Enums.GetDescription(Enums.Users.DisplayName), user.FullName);

            if (!string.IsNullOrEmpty(user.Department))
                columnsValues.Add(Enums.GetDescription(Enums.Users.Department), user.Department.ToString());
            else
                columnsValues.Add(Enums.GetDescription(Enums.Users.Department), "UNIDENTIFIED");

            if (!string.IsNullOrEmpty(user.TelephoneNumber))
                columnsValues.Add(Enums.GetDescription(Enums.Users.TelephoneNumber), user.TelephoneNumber);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Users.TableName), columnsValues,Enums.GetDescription(Enums.Users.EmployeeID));

            return rowID;
        }

        public static bool UpdateUser(User user)
        {
            bool status = false;
            
            Dictionary<string,object> wherePart = new Dictionary<string,object>
            {
                { Enums.GetDescription(Enums.Users.SipAccount), user.SipAccount }
            };
            
            Dictionary<string, object> setPart = new Dictionary<string, object>();

            setPart.Add(Enums.GetDescription(Enums.Users.EmployeeID), user.EmployeeID);

            if (!string.IsNullOrEmpty(user.SipAccount))
                setPart.Add(Enums.GetDescription(Enums.Users.SipAccount), user.SipAccount);

            if (!string.IsNullOrEmpty(user.SiteName))
                setPart.Add(Enums.GetDescription(Enums.Users.SiteName), user.SiteName.ToString());
            else
                setPart.Add(Enums.GetDescription(Enums.Users.SiteName), "UNIDENTIFIED");

            if (!string.IsNullOrEmpty(user.FullName))
                setPart.Add(Enums.GetDescription(Enums.Users.DisplayName), user.FullName);

            if (!string.IsNullOrEmpty(user.Department))
                setPart.Add(Enums.GetDescription(Enums.Users.Department), user.Department.ToString());
            else
                setPart.Add(Enums.GetDescription(Enums.Users.Department), "UNIDENTIFIED");

            if (!string.IsNullOrEmpty(user.TelephoneNumber))
                setPart.Add(Enums.GetDescription(Enums.Users.TelephoneNumber), user.TelephoneNumber);

            status = DBRoutines.UPDATE(
                Enums.GetDescription(Enums.Users.TableName),
                setPart,
                wherePart);

            return status;
        }

        public static bool DeleteUser(User user)
        {
            bool status = false;

            status = DBRoutines.DELETE(
                Enums.GetDescription(Enums.Users.TableName),
                Enums.GetDescription(Enums.Users.EmployeeID), user.EmployeeID);

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
            return adConnector.GetUserAttributes(emailAddress);
        }

    }
}