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

        public UsersDelegates GetBySipAccount(string delegateAccount) 
        {
            UsersDelegates delegates = new UsersDelegates();

            DataTable dt = new DataTable();
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Delegates.TableName), "DelegateAccount", delegateAccount);

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

        public UsersDelegates GetByDelegateAccount(string sipAccount) 
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

        public bool UpadeDelegate(string sipAccount) 
        {
            return false;
        }

        public bool DeleteDelegate(string sipAccount) 
        {
            return false;
        }

        public int AddDelegate(UsersDelegates DelegateAccount ) 
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if (DelegateAccount.SipAccount.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Delegates.SipAccount), DelegateAccount.SipAccount);

            if (DelegateAccount.DelegeeAccount.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Delegates.DelegeeAccount), DelegateAccount.DelegeeAccount);

            if (DelegateAccount.Description.ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Delegates.Description), DelegateAccount.Description);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Delegates.TableName), columnsValues, Enums.GetDescription(Enums.Delegates.ID));

            return rowID;
        }
    }

    
}