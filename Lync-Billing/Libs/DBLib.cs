using System;
using System.Web;
using System.Text;
using System.Linq;
using System.Data.OleDb;
using System.Collections;
using System.Collections.Generic;


namespace Lync_Billing.Libs
{
    
    public class DBLib
    {
        //public static string ConnectionString_Lync = @"Provider=SQLOLEDB.1;Data Source=10.1.60.55;Persist Security Info=True;Password='=25_ar;p1100';User ID=sa;Initial Catalog=LyncBilling";

        private OleDbConnection DBInitializeConnection(string connectionString) 
        {
            return new OleDbConnection(connectionString);
        }

        public OleDbDataReader selectFrom(string tableName,Dictionary<string,object> columns,string whereStatemnet)
        {
            OleDbDataReader dr = new OleDbDataReader();
            //string inserQuery = string.Format("select ")
            return dr;
        }

        public int insertTo(string tableName, Dictionary<string, object> columnValue) 
        {
            return 0;
        }

        public int update(string tablename, Dictionary<string, object> columnValue, string whereStatement)
        {
            return 0;
        }

        public bool deleteFrom(string tableName, string whereStatement) 
        {
            return false;
        }

   }
}