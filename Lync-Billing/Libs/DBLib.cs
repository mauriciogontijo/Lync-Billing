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

        public DataTable SELECT(string tableName,List <string> fields, Dictionary<string, object> whereClause, int limits, bool allFields = false)
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;

            StringBuilder selectedfields = new StringBuilder();
            StringBuilder whereStatement = new StringBuilder();

            if (allFields == false)
            {
                foreach (string fieldName in fields)
                {
                    selectedfields.Append(fieldName + ",");
                }
                selectedfields.Remove(selectedfields.Length - 1, 1);
            }
            else
            {
                selectedfields.Append("*");
            }


           if(whereClause != null)
           {
               whereStatement.Append("WHERE ");
               foreach (KeyValuePair<string, object> pair in whereClause)
               {
                   Type valueType = pair.Value.GetType();
                   if (valueType == typeof(int) || valueType == typeof(Double))
                   {
                       whereStatement.Append("[" + pair.Key + "]=" + pair.Value + " AND ");
                   }
                   else
                   {
                       whereStatement.Append("[" + pair.Key + "]='" + pair.Value + "' AND ");
                   }
               }
               whereStatement.Remove(whereStatement.Length - 1, 5);
            }

            if (limits == 0)
            {
                if (whereClause !=null)
                    selectQuery = string.Format("SELECT '{1}' FROM  ['{2}'] WHERE '{3}'", selectedfields.ToString(), tableName, whereStatement.ToString());
                else
                    selectQuery = string.Format("SELECT '{1}' FROM  ['{2}']", selectedfields.ToString(), tableName);
            }
            else
            {
                if (whereClause != null)
                    selectQuery = string.Format("SELECT TOP({1}) '{2}' FROM ['{3}'] WHERE '{4}'", limits, selectedfields.ToString(), tableName, whereStatement.ToString());
                else
                    selectQuery = string.Format("SELECT TOP({1}) '{2}' FROM ['{3}']", limits, selectedfields.ToString(), tableName);
            }
                    
            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(selectQuery, conn);

            try
            {
                conn.Open();
                dr = comm.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception ex) 
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx; 
            }
            finally { conn.Close(); }

            return dt;
        }

        public int INSERT(string tableName, Dictionary<string, object> columnsValues) 
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

            string insertQuery = string.Format("INSERT INTO ['{1}'] '{2}' VALUES '{3}'; SELECT SCOPE_IDENTITY();", tableName, fields, values);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(insertQuery, conn);

            int recordID = 0;
            try
            {
                conn.Open();
                recordID = (int)comm.ExecuteScalar();
            }
            catch (Exception ex) 
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx; 
            }
            finally { conn.Close(); }

            return recordID;
        }

        public bool UPDATE(string tableName, Dictionary<string, object> columnsValues,string idFieldName, int ID) 
        {
            StringBuilder fieldsValues = new StringBuilder();

            foreach (KeyValuePair<string, object> pair in columnsValues)
            {

                Type valueType = pair.Value.GetType();

                if (valueType == typeof(int) || valueType == typeof(Double))
                    fieldsValues.Append("[" + pair.Key + "]=" + pair.Value + ",");
                else
                    fieldsValues.Append(pair.Key + "=" + "'" + pair.Value + "'" + ",");
            }

            fieldsValues.Remove(fieldsValues.Length - 1, 1).Append(")");

            string insertQuery = string.Format("UPDATE  ['{1}'] SET '{2}' WHERE '{3}'={4}", tableName, fieldsValues, idFieldName,ID);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(insertQuery, conn);

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }
           
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
            catch (Exception ex) 
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx; 
            }
            finally { conn.Close(); }

        }

        public bool DELETE(string tableName, string idFieldName, int ID) 
        {
             string deleteQuery = string.Format("DELETE FROM ['{1}'] WHERE '{2}={3}'", tableName, idFieldName,ID);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(deleteQuery, conn);

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex) 
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx; 
            }
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
            catch (Exception ex) 
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx; 
            }
            finally { conn.Close(); }
        }


   }
}