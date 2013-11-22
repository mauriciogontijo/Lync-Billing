using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Lync_Billing.Libs;

namespace Lync_Billing.DB.Roles
{
    public class DelegateRole
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
            List<DelegateRole> delegatedAccounts = GetDelegees(delegateAccount, DelegateRole.UserDelegeeTypeID);
            
            return (delegatedAccounts.Count > 0 ? true : false);
        }

        public static bool IsDepartmentDelegate(string delegateAccount)
        {
            List<DelegateRole> delegatedAccounts = GetDelegees(delegateAccount, DelegateRole.DepartmentDelegeeTypeID);

            return (delegatedAccounts.Count > 0 ? true : false);
        }

        public static bool IsSiteDelegate(string delegateAccount)
        {
            List<DelegateRole> delegatedAccounts = GetDelegees(delegateAccount, DelegateRole.SiteDelegeeTypeID);

            return (delegatedAccounts.Count > 0 ? true : false);
        }

        public static List<DelegateRole> GetDelegees(string delegateAccount) 
        {
            DelegateRole delegatedAccount;
            List<DelegateRole> DelegatedAccounts = new List<DelegateRole>();
            DataTable dt = new DataTable();
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DelegateRoles.TableName), Enums.GetDescription(Enums.DelegateRoles.Delegee), delegateAccount);

            foreach (DataRow row in dt.Rows)
            {
                delegatedAccount = new DelegateRole();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.ID) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.ID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.Delegee) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.DelegeeAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.DelegeeType) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.DelegeeType = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.SipAccount) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.Description) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.Description = (string)row[column.ColumnName];
                }
                DelegatedAccounts.Add(delegatedAccount);
            }

            return DelegatedAccounts;
        }

        public static List<DelegateRole> GetDelegees(string degateAccount, int delegeeType) 
        {
            DelegateRole delegatedAccount;
            List<DelegateRole> DelegatedAccounts = new List<DelegateRole>();
            DataTable dt = new DataTable();

            Dictionary<string, object> wherePart = new Dictionary<string, object>
            {
                {Enums.GetDescription(Enums.DelegateRoles.DelegeeType),delegeeType},
                {Enums.GetDescription(Enums.DelegateRoles.Delegee) ,degateAccount}
            };

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DelegateRoles.TableName),null,wherePart,0);

            foreach (DataRow row in dt.Rows)
            {
                delegatedAccount = new DelegateRole();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.ID) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.ID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.Delegee) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.DelegeeAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.DelegeeType) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.DelegeeType = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.SipAccount) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.Description) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.Description = (string)row[column.ColumnName];
                }
                DelegatedAccounts.Add(delegatedAccount);
            }

            return DelegatedAccounts;
        }

        public static List<DelegateRole> GetDelegees() 
        {
            DelegateRole delegatedAccount;
            List<DelegateRole> DelegatedAccounts = new List<DelegateRole>();
            DataTable dt = new DataTable();
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.DelegateRoles.TableName));

            foreach (DataRow row in dt.Rows)
            {
                delegatedAccount = new DelegateRole();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.ID) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.ID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.Delegee) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.DelegeeAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.DelegeeType) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.DelegeeType = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.SipAccount) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.DelegateRoles.Description) && row[column.ColumnName] != System.DBNull.Value)
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
            List<DelegateRole> delegatesList;
            Dictionary<string, string> DelegatedAccounts = new Dictionary<string, string>();

            delegatesList = GetDelegees(delegeeSipAccount, delegateType);

            foreach (var delegateAccount in delegatesList)
            {
                delegateName = string.Empty;

                //If the delegate account is not a user's account
                if (delegateType == DelegateRole.DepartmentDelegeeTypeID || delegateType == DelegateRole.SiteDelegeeTypeID)
                {
                    delegateName = delegateAccount.SipAccount;
                }
                else
                {
                    //Try to get the user from the system by the associated delegateAccount.SipAccount
                    userInfo = Users.GetUserInfo(delegateAccount.SipAccount);
                    delegateName = (userInfo == null) ? delegateAccount.SipAccount : HelperFunctions.ReturnEmptyIfNull(userInfo.FirstName) + " " + HelperFunctions.ReturnEmptyIfNull(userInfo.LastName);
                }
                
                DelegatedAccounts.Add(delegateAccount.SipAccount, delegateName);
            }

            return DelegatedAccounts;
        }
    
        public static bool UpadeDelegate(DelegateRole delegee) 
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part
            if (delegee.SipAccount.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.DelegateRoles.SipAccount), delegee.SipAccount);

            if (delegee.DelegeeAccount.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.DelegateRoles.Delegee), delegee.DelegeeAccount);

            if (delegee.DelegeeType.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.DelegateRoles.DelegeeType), delegee.DelegeeType);

            if (delegee.Description.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.DelegateRoles.Description), delegee.Description);

            //Execute Update
            status = DBRoutines.UPDATE(
                Enums.GetDescription(Enums.DelegateRoles.TableName),
                setPart,
                Enums.GetDescription(Enums.DelegateRoles.ID),
                delegee.ID);

            if (status == false)
            {
                //throw error message
            }

            return true;
        }

        public static bool DeleteDelegate(DelegateRole delegee) 
        {
            bool status = false;

            Dictionary<string,object> wherePart = new Dictionary<string,object>();
            wherePart.Add(Enums.GetDescription(Enums.DelegateRoles.SipAccount),delegee.SipAccount);
            wherePart.Add(Enums.GetDescription(Enums.DelegateRoles.Delegee), delegee.DelegeeAccount);

            status = DBRoutines.DELETE(Enums.GetDescription(Enums.DelegateRoles.TableName), wherePart);

            if (status == false)
            {
                //throw error message
            }
            return status;
        }

        public static int AddDelegate(DelegateRole delegee) 
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if (delegee.SipAccount.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.SipAccount), delegee.SipAccount);

            if (delegee.DelegeeAccount.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.Delegee), delegee.DelegeeAccount);

            if (delegee.DelegeeType.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.Delegee), delegee.DelegeeType);

            if (delegee.Description.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.DelegateRoles.Description), delegee.Description);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.DelegateRoles.TableName), columnsValues, Enums.GetDescription(Enums.DelegateRoles.ID));

            return rowID;
        }
    }

    
}