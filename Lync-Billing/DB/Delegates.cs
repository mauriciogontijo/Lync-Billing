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
        public string SipAccount { get; set; }
        public string DelegateAccount { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public Delegates GetBySipAccount(string delegateAccount) 
        {
            Delegates delegates = new Delegates();

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

        public Delegates GetByDelegateAccount(string sipAccount) 
        {
            Delegates delegates = new Delegates();

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

        public bool AddDelegate(string sipAccount) 
        {
            return false;
        }
    }

    
}