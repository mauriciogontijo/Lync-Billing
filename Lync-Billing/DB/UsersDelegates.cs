using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class Delegates
    {
        private static DBLib DBRoutines = new DBLib();
        
        public int ID { set; get; }
        public string SipAccount { get; set; }
        public string DelegeeAccount { get; set; }
        public int DelegeeType { get; set; }
        public string Description { get; set; }

        //These are for lookup use only in the application
        public static int UserDelegeeTypeID { get { return 1; } }
        public static int DepartmentDelegeeTypeID { get { return 2; } }
        public static int SiteDelegeeTypeID { get { return 3; } }

        public static bool IsUserDelegate(string delegateAccount) 
        {
            //List<string> columns;
            //Dictionary<string, object> wherePart;
            //List<UserRole> delegeeRoles;
            //List<Delegates> delegatedAccounts = new List<Delegates>();

            //columns = new List<string>();
            //wherePart = new Dictionary<string,object>
            //{
            //    { Enums.GetDescription(Enums.UsersRoles.EmailAddress), delegateAccount },
            //    { Enums.GetDescription(Enums.UsersRoles.RoleID), UserRole.DelegeeRoleID }
            //};

            //delegeeRoles = UserRole.GetUsersRoles(columns, wherePart, 0);

            //if (delegeeRoles.Count > 0)
            //{
            //    delegatedAccounts = GetDelegees(delegateAccount, Delegates.UserDelegeeTypeID);

            //    if (delegatedAccounts.Count > 0)
            //        return true;
            //}

            List<Delegates> delegatedAccounts = new List<Delegates>();

            delegatedAccounts = GetDelegees(delegateAccount, Delegates.UserDelegeeTypeID);

            if (delegatedAccounts.Count > 0)
                return true;

            return false;
        }

        public static bool IsDepartmentDelegate(string delegateAccount)
        {
            List<string> columns;
            Dictionary<string, object> wherePart;
            List<UserRole> delegeeRoles;
            List<Delegates> delegatedAccounts = new List<Delegates>();

            columns = new List<string>();
            wherePart = new Dictionary<string, object>
            {
                { Enums.GetDescription(Enums.UsersRoles.EmailAddress), delegateAccount },
                { Enums.GetDescription(Enums.UsersRoles.RoleID), UserRole.DelegeeRoleID }
            };

            delegeeRoles = UserRole.GetUsersRoles(columns, wherePart, 0);

            if (delegeeRoles.Count > 0)
            {
                delegatedAccounts = GetDelegees(delegateAccount, Delegates.DepartmentDelegeeTypeID);

                if (delegatedAccounts.Count > 0)
                    return true;
            }

            return false;
        }

        public static bool IsSiteDelegate(string delegateAccount)
        {
            List<string> columns;
            Dictionary<string, object> wherePart;
            List<UserRole> delegeeRoles;
            List<Delegates> delegatedAccounts = new List<Delegates>();

            columns = new List<string>();
            wherePart = new Dictionary<string, object>
            {
                { Enums.GetDescription(Enums.UsersRoles.EmailAddress), delegateAccount },
                { Enums.GetDescription(Enums.UsersRoles.RoleID), UserRole.DelegeeRoleID }
            };

            delegeeRoles = UserRole.GetUsersRoles(columns, wherePart, 0);

            if (delegeeRoles.Count > 0)
            {
                delegatedAccounts = GetDelegees(delegateAccount, Delegates.SiteDelegeeTypeID);

                if (delegatedAccounts.Count > 0)
                    return true;
            }

            return false;
        }

        public static List<Delegates> GetDelegees(string delegateAccount) 
        {
            Delegates delegatedAccount;
            List<Delegates> DelegatedAccounts = new List<Delegates>();
            DataTable dt = new DataTable();
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Delegates.TableName), Enums.GetDescription(Enums.Delegates.Delegee), delegateAccount);

            foreach (DataRow row in dt.Rows)
            {
                delegatedAccount = new Delegates();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.ID) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.ID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.Delegee) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.DelegeeAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.DelegeeType) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.DelegeeType = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.SipAccount) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.Description) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.Description = (string)row[column.ColumnName];
                }
                DelegatedAccounts.Add(delegatedAccount);
            }

            return DelegatedAccounts;
        }

        public static List<Delegates> GetDelegees(string degateAccount, int delegeeType) 
        {
            Delegates delegatedAccount;
            List<Delegates> DelegatedAccounts = new List<Delegates>();
            DataTable dt = new DataTable();

            Dictionary<string, object> wherePart = new Dictionary<string, object>
            {
                {Enums.GetDescription(Enums.Delegates.DelegeeType),delegeeType},
                {Enums.GetDescription(Enums.Delegates.Delegee) ,degateAccount}
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Delegates.TableName),null,wherePart,0);

            foreach (DataRow row in dt.Rows)
            {
                delegatedAccount = new Delegates();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.ID) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.ID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.Delegee) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.DelegeeAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.DelegeeType) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.DelegeeType = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.SipAccount) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.Description) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.Description = (string)row[column.ColumnName];
                }
                DelegatedAccounts.Add(delegatedAccount);
            }

            return DelegatedAccounts;
        }

        public static List<Delegates> GetDelegees() 
        {
            Delegates delegatedAccount;
            List<Delegates> DelegatedAccounts = new List<Delegates>();
            DataTable dt = new DataTable();
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Delegates.TableName));

            foreach (DataRow row in dt.Rows)
            {
                delegatedAccount = new Delegates();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.ID) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.ID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.Delegee) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.DelegeeAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.DelegeeType) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.DelegeeType = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.SipAccount) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.Description) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.Description = (string)row[column.ColumnName];
                }
                DelegatedAccounts.Add(delegatedAccount);
            }

            return DelegatedAccounts;
        }
        
        /*
         * This function returns a dictionary of the delegees sip-accounts and names {sip => name}, if they exist!
         **/
        public static Dictionary<string, string> GetDelegeesNames(string delegeeSipAccount, int delegateType)
        {
            ADUserInfo userInfo;
            string delegateName;
            List<Delegates> delegatesList;
            Dictionary<string, string> DelegatedAccounts = new Dictionary<string, string>();

            delegatesList = GetDelegees(delegeeSipAccount, delegateType);

            foreach (var delegateAccount in delegatesList)
            {
                delegateName = string.Empty;

                //If the delegate account is not a user's account
                if (delegateType == Delegates.DepartmentDelegeeTypeID || delegateType == Delegates.SiteDelegeeTypeID)
                {
                    delegateName = delegateAccount.SipAccount;
                }
                else
                {
                    //Try to get the user from the system by the associated delegateAccount.SipAccount
                    userInfo = Users.GetUserInfo(delegateAccount.SipAccount);
                    delegateName = (userInfo == null) ? delegateAccount.SipAccount : Misc.ReturnEmptyIfNull(userInfo.FirstName) + " " + Misc.ReturnEmptyIfNull(userInfo.LastName);
                }
                
                DelegatedAccounts.Add(delegateAccount.SipAccount, delegateName);
            }

            return DelegatedAccounts;
        }
    
        public static bool UpadeDelegate(Delegates delegee) 
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part
            if (delegee.SipAccount.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Delegates.SipAccount), delegee.SipAccount);

            if (delegee.DelegeeAccount.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Delegates.Delegee), delegee.DelegeeAccount);

            if (delegee.DelegeeType.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Delegates.DelegeeType), delegee.DelegeeType);

            if (delegee.Description.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Delegates.Description), delegee.Description);

            //Execute Update
            status = DBRoutines.UPDATE(
                Enums.GetDescription(Enums.Delegates.TableName),
                setPart,
                Enums.GetDescription(Enums.Delegates.ID),
                delegee.ID);

            if (status == false)
            {
                //throw error message
            }

            return true;
        }

        public static bool DeleteDelegate(Delegates delegee) 
        {
            bool status = false;

            Dictionary<string,object> wherePart = new Dictionary<string,object>();
            wherePart.Add(Enums.GetDescription(Enums.Delegates.SipAccount),delegee.SipAccount);
            wherePart.Add(Enums.GetDescription(Enums.Delegates.Delegee), delegee.DelegeeAccount);

            status = DBRoutines.DELETE(Enums.GetDescription(Enums.Delegates.TableName), wherePart);

            if (status == false)
            {
                //throw error message
            }
            return status;
        }

        public static int AddDelegate(Delegates delegee) 
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if (delegee.SipAccount.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Delegates.SipAccount), delegee.SipAccount);

            if (delegee.DelegeeAccount.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Delegates.Delegee), delegee.DelegeeAccount);

            if (delegee.DelegeeType.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Delegates.Delegee), delegee.DelegeeType);

            if (delegee.Description.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Delegates.Description), delegee.Description);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Delegates.TableName), columnsValues, Enums.GetDescription(Enums.Delegates.ID));

            return rowID;
        }
    }

    
}