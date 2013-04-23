using System;
using System.Web;
using System.Text;
using System.Linq;
using System.Data.OleDb;
using System.Collections;
using System.Collections.Generic;
using System.Data;


namespace Lync_Billing.Libs
{
    
    public class DBLib
    {
        public static string ConnectionString_Lync = @"Provider=SQLOLEDB.1;Data Source=10.1.60.55;Persist Security Info=True;Password='=25_ar;p1100';User ID=sa;Initial Catalog=LyncBilling";

        private OleDbConnection DBInitializeConnection(string connectionString) 
        {
            return new OleDbConnection(connectionString);
        }

        public DataTable SELECT(string tableName, Dictionary<string, object> columnsValues, int limits, bool allRows = false)
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;

            StringBuilder fields = new StringBuilder();
            StringBuilder whereStatement = new StringBuilder();
            
            foreach(KeyValuePair<string,object>  pair in columnsValues)
            {
                fields.Append("[" + pair.Key +"],");

                Type valueType = pair.Value.GetType();

                if (valueType == typeof(int) || valueType == typeof(Double))
                    whereStatement.Append("[" + pair.Key + "]=" + pair.Value + " AND ");
                else
                    whereStatement.Append("[" + pair.Key + "]='" + pair.Value + "' AND ");
                
            }

            fields.Remove(fields.Length - 1, 1);
            whereStatement.Remove(whereStatement.Length - 1, 5);
           
            if (limits == 0)
            {
                if (allRows == false)
                    selectQuery = string.Format("SELECT '{1}' FROM  ['{2}'] WHERE '{3}'", fields.ToString(), tableName, whereStatement.ToString());
                else
                    selectQuery = string.Format("SELECT * FROM ['{1}'] WHERE '{2}'", tableName, whereStatement.ToString());
            }
            else
            {
                if (allRows == false)
                    selectQuery = string.Format("SELECT TOP({1}) ['{2}'] FROM '{3}' WHERE '{4}'", limits, fields.ToString(), tableName, whereStatement.ToString());
                else
                    selectQuery = string.Format("SELECT TOP({1}) * FROM ['{2}'] WHERE '{3}'", limits, tableName, whereStatement.ToString());
            }
            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(selectQuery, conn);

            try
            {
                conn.Open();
                dr = comm.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception ex) { throw ex; }
            finally { conn.Close(); }

            return dt;
        }

        public int INSERT(string tableName, Dictionary<string, object> columnsValues, Dictionary<string,object> wherePart) 
        {
            StringBuilder fields = new StringBuilder(); fields.Append("(");
            StringBuilder values = new StringBuilder(); values.Append("(");
            StringBuilder whereStatement = new StringBuilder();

            //Fields and values
            foreach (KeyValuePair<string, object> pair in columnsValues)
            {
                fields.Append("[" + pair.Key + "],");

                Type valueType = pair.Value.GetType();

                if (valueType == typeof(int) || valueType == typeof(Double))
                    values.Append(pair.Value + ",");
                else
                    values.Append("'" + pair.Value + "'" + ",");
            }

            fields.Remove(fields.Length - 1, 1).Append(")");
            values.Remove(values.Length - 1, 1).Append(")");


            //Where Statement

            foreach (KeyValuePair<string, object> pair in wherePart) 
            {
                Type valueType = pair.Value.GetType();

                if (valueType == typeof(int) || valueType == typeof(Double))
                    whereStatement.Append("[" + pair.Key + "]=" + pair.Value + " AND ");
                else
                    whereStatement.Append("[" + pair.Key + "]='" + pair.Value + "' AND ");
        
            }
            whereStatement.Remove(whereStatement.Length - 1, 5);

            string insertQuery = string.Format("INSERT INTO ['{1}'] '{2}' VALUES '{3}' WHERE '{4}'; SELECT SCOPE_IDENTITY();", tableName, fields, values, whereStatement);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(insertQuery, conn);

            int recordID = 0;
            try
            {
                conn.Open();
                recordID = (int)comm.ExecuteScalar();
            }
            catch (Exception ex) { throw ex; }
            finally { conn.Close(); }

            return recordID;
        }

        public bool UPDATE(string tableName, Dictionary<string, object> columnsValues, Dictionary<string,object> wherePart)
        {

            StringBuilder fieldsValues = new StringBuilder();
            StringBuilder whereStatement = new StringBuilder();
            

            foreach (KeyValuePair<string, object> pair in columnsValues)
            {

                Type valueType = pair.Value.GetType();

                if (valueType == typeof(int) || valueType == typeof(Double))
                    fieldsValues.Append("[" + pair.Key + "]=" + pair.Value + ",");              
                else
                    fieldsValues.Append(pair.Key + "=" + "'" + pair.Value  + "'" + ",");  
            }
            
            fieldsValues.Remove(fieldsValues.Length - 1, 1).Append(")");

            foreach (KeyValuePair<string, object> pair in wherePart)
            {
                Type valueType = pair.Value.GetType();

                if (valueType == typeof(int) || valueType == typeof(Double))
                    whereStatement.Append("[" + pair.Key + "]=" + pair.Value + " AND ");
                else
                    whereStatement.Append("[" + pair.Key + "]='" + pair.Value + "' AND ");

            }
            whereStatement.Remove(whereStatement.Length - 1, 5);

            string insertQuery = string.Format("UPDATE  ['{1}'] SET '{2}' WHERE '{3}'", tableName, fieldsValues, whereStatement);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(insertQuery, conn);

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex) { throw ex; }
            finally { conn.Close(); }

        }

        public bool DELETE(string tableName, Dictionary<string, object> wherePart) 
        {
            StringBuilder whereStatement = new StringBuilder();

            foreach (KeyValuePair<string, object> pair in wherePart)
            {
                Type valueType = pair.Value.GetType();

                if (valueType == typeof(int) || valueType == typeof(Double))
                    whereStatement.Append("[" + pair.Key + "]=" + pair.Value + " AND ");
                else
                    whereStatement.Append("[" + pair.Key + "]='" + pair.Value + "' AND ");

            }
            whereStatement.Remove(whereStatement.Length - 1, 5);

            string deleteQuery = string.Format("DELETE FROM ['{1}'] WHERE '{2}'", tableName, whereStatement);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(deleteQuery, conn);

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex) { throw ex; }
            finally { conn.Close(); }
        }

   }
}