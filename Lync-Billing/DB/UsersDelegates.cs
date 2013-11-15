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
        public int ID { set; get; }
        public string SipAccount { get; set; }
        public string DelegeeAccount { get; set; }
        public int DelegeeType { get; set; }
        public string Description { get; set; }

        private static DBLib DBRoutines = new DBLib();



        public static bool IsUserDelegate(string delegateAccount) 
        {
            List<Delegates> delegatedAccounts = new List<Delegates>();

            delegatedAccounts = GetDelegees(delegateAccount,1);

            if (delegatedAccounts.Count != 0)
                return true;
            else 
            {
                return false;
            }
                
        }

        public static bool IsDepartmentDelegate(string delegateAccount)
        {
            List<Delegates> delegatedAccounts = new List<Delegates>();

            delegatedAccounts = GetDelegees(delegateAccount, 2);

            if (delegatedAccounts.Count != 0)
                return true;
            else
            {
                return false;
            }

        }

        public static bool IsSiteDelegate(string delegateAccount)
        {
            List<Delegates> delegatedAccounts = new List<Delegates>();

            delegatedAccounts = GetDelegees(delegateAccount, 3);

            if (delegatedAccounts.Count != 0)
                return true;
            else
            {
                return false;
            }

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
        public static Dictionary<string, string> GetDelegeesNames(string delegateAccount)
        {
            Dictionary<string, string> DelegatedAccounts = new Dictionary<string, string>();
            string columnName = Enums.GetDescription(Enums.Delegates.SipAccount);

            ADUserInfo userInfo;
            string sipAccount = string.Empty;

            DataTable dt = new DataTable();
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Delegates.TableName), Enums.GetDescription(Enums.Delegates.Delegee), delegateAccount);


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