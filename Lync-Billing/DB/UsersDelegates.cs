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
        public string SipAccount { get; set; }
        public string DelegateAccount { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public UsersDelegates GetBySipAccount(string delegateAccount) 
        {
            UsersDelegates delegates = new UsersDelegates();

            DataTable dt = new DataTable();
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Delegates.TableName), "DelegateAccount", delegateAccount);

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.DelegateAccount) && row[column.ColumnName] != System.DBNull.Value)
                        delegates.DelegateAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.SipAccount) && row[column.ColumnName] != System.DBNull.Value)
                        delegates.SipAccount = (string)row[column.ColumnName];
                }
            }

            return delegates;
        }

        public UsersDelegates GetByDelegateAccount(string sipAccount) 
        {
            UsersDelegates delegates = new UsersDelegates();

            DataTable dt = new DataTable();
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Delegates.TableName), "SipAccount", sipAccount);

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.DelegateAccount) && row[column.ColumnName] != System.DBNull.Value)
                        delegates.DelegateAccount = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Delegates.SipAccount) && row[column.ColumnName] != System.DBNull.Value)
                        delegates.SipAccount = (string)row[column.ColumnName];
                }
            }

            return delegates;
        }

        public bool UpadeDelegate(string sipAccount) 
        {
            return false;
        }

        public bool DeleteDelegate(string sipAccount) 
        {
            return false;
        }

        public bool AddDelegate(UsersDelegates DelegateAccount ) 
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if (DelegateAccount.SipAccount.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Delegates.SipAccount), DelegateAccount.SipAccount);

            if (DelegateAccount.DelegateAccount.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Delegates.DelegateAccount), DelegateAccount.DelegateAccount);

            return false;
        }
    }

    
}