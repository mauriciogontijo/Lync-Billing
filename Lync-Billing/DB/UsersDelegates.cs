using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class UsersDelegates
    {
        public int ID { set; get; }
        public string SipAccount { get; set; }
        public string DelegeeAccount { get; set; }
        public string Description { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static bool IsDelegate(string delegateAccount) 
        {
            List<UsersDelegates> delegatedAccounts = new List<UsersDelegates>();

            delegatedAccounts = GetDelegees(delegateAccount);

            if (delegatedAccounts.Count == 0)
                return false;
            else
                return true;
        }

        public static List<UsersDelegates> GetDelegees(string delegateAccount) 
        {
            UsersDelegates delegatedAccount;
            List<UsersDelegates> DelegatedAccounts = new List<UsersDelegates>();
            DataTable dt = new DataTable();
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Delegates.TableName), "DelegeeAccount", delegateAccount);

            foreach (DataRow row in dt.Rows)
            {
                delegatedAccount = new UsersDelegates();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.ID) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.ID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.DelegeeAccount) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.DelegeeAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.SipAccount) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.Description) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.Description = (string)row[column.ColumnName];
                }
                DelegatedAccounts.Add(delegatedAccount);
            }

            return DelegatedAccounts;
        }

        public static List<UsersDelegates> GetDelgatees() 
        {
            UsersDelegates delegatedAccount;
            List<UsersDelegates> DelegatedAccounts = new List<UsersDelegates>();
            DataTable dt = new DataTable();
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Delegates.TableName));

            foreach (DataRow row in dt.Rows)
            {
                delegatedAccount = new UsersDelegates();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.ID) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.ID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.DelegeeAccount) && row[column.ColumnName] != System.DBNull.Value)
                        delegatedAccount.DelegeeAccount = (string)row[column.ColumnName];

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
        public static Dictionary<string, string> GetDelegeesNames(string delegateAccount)
        {
            Dictionary<string, string> DelegatedAccounts = new Dictionary<string, string>();
            string columnName = Enums.GetDescription(Enums.Delegates.SipAccount);

            ADUserInfo userInfo;
            string sipAccount = string.Empty;

            DataTable dt = new DataTable();
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Delegates.TableName), "DelegeeAccount", delegateAccount);


            foreach (DataRow row in dt.Rows)
            {
                userInfo = new ADUserInfo();

                if (row[Enums.GetDescription(Enums.Delegates.SipAccount)] != System.DBNull.Value)
                {
                    sipAccount = (string)row[columnName];
                    userInfo = Users.GetUserInfo(sipAccount);

                    DelegatedAccounts.Add(sipAccount, userInfo.FirstName.ToString() + " " + userInfo.LastName.ToString());
                }
            }

            return DelegatedAccounts;
        }


        /*
         * This function returns a list of delegees sip accounts only, if they exist.
         **/
        public static List<string> GetListOfDelegeesSipAccounts(string delegateAccount)
        {
            List<string> DelegatedAccounts = new List<string>();
            string columnName = Enums.GetDescription(Enums.Delegates.SipAccount);

            DataTable dt = new DataTable();
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Delegates.TableName), "DelegeeAccount", delegateAccount);

            foreach (DataRow row in dt.Rows)
            {
                if (row[Enums.GetDescription(Enums.Delegates.SipAccount)] != System.DBNull.Value)
                {
                    DelegatedAccounts.Add(
                        (string)row[columnName]
                    );
                }
            }

            return DelegatedAccounts;
        }

        public static UsersDelegates GetDelegateAccount(string sipAccount) 
        {
            UsersDelegates delegates = new UsersDelegates();

            DataTable dt = new DataTable();
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Delegates.TableName), "SipAccount", sipAccount);

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.ID) && row[column.ColumnName] != System.DBNull.Value)
                        delegates.ID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.DelegeeAccount) && row[column.ColumnName] != System.DBNull.Value)
                        delegates.DelegeeAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.SipAccount) && row[column.ColumnName] != System.DBNull.Value)
                        delegates.SipAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.Description) && row[column.ColumnName] != System.DBNull.Value)
                        delegates.Description = (string)row[column.ColumnName];
                }
            }

            return delegates;
        }

        public static bool UpadeDelegate(UsersDelegates delegee) 
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part
            if (delegee.SipAccount.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Delegates.SipAccount), delegee.SipAccount);

            if (delegee.DelegeeAccount.ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Delegates.DelegeeAccount), delegee.DelegeeAccount);

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

        public static bool DeleteDelegate(UsersDelegates delegee) 
        {
            bool status = false;

            Dictionary<string,object> wherePart = new Dictionary<string,object>();
            wherePart.Add("SipAccount",delegee.SipAccount);
            wherePart.Add("DelegeeAccount", delegee.DelegeeAccount);

            status = DBRoutines.DELETE(Enums.GetDescription(Enums.Delegates.TableName), wherePart);

            if (status == false)
            {
                //throw error message
            }
            return status;
        }

        public static int AddDelegate(UsersDelegates delegee) 
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if (delegee.SipAccount.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Delegates.SipAccount), delegee.SipAccount);

            if (delegee.DelegeeAccount.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Delegates.DelegeeAccount), delegee.DelegeeAccount);

            if (delegee.Description.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Delegates.Description), delegee.Description);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Delegates.TableName), columnsValues, Enums.GetDescription(Enums.Delegates.ID));

            return rowID;
        }
    }

    
}