using System;
using System.Web;
using System.Text;
using System.Linq;
using System.Data.OleDb;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;


namespace Lync_Billing.Libs
{
    
    public class DBLib
    {
        //public static string ConnectionString_Lync = @"Provider=SQLOLEDB.1;Data Source=10.1.60.55;Persist Security Info=True;Password='=25_ar;p1100';User ID=sa;Initial Catalog=LyncBilling";
        public static string ConnectionString_Lync = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString();

        private OleDbConnection DBInitializeConnection(string connectionString) 
        {
            return new OleDbConnection(connectionString);
        }

        /// <summary>
        /// Construct Generic Select Statemnet 
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="whereField">Where statemnet Field</param>
        /// <param name="whereValue">Where statemnet Value</param>
        /// <returns> DataTable Object</returns>
        public DataTable SELECT(string tableName, string whereField, object whereValue) 
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;

            StringBuilder selectedfields = new StringBuilder();

            if (whereValue.GetType().Equals(typeof(int)) || whereValue.GetType().Equals(typeof(double)))
                selectQuery = string.Format("SELECT * FROM  ['{2}'] WHERE '{3}'={4}", tableName, whereField,whereValue);
            else
                selectQuery = string.Format("SELECT * FROM  ['{2}'] WHERE '{3}'='{4}'", tableName, whereField, whereValue);

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

        /// <summary>
        /// Construct Generic Select Statemnet 
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="fields">List of Fields to be fetched from Database</param>
        /// <param name="whereClause"> A dictionary which holds the fields and its related values to be able to construct Where Statemnet
        /// 1. Null if there is no condition
        /// 2. Dictionary holds fields names and values if there is a condition
        /// </param>
        /// <param name="limits">Holds how many rows to be fetched from the database table. 
        /// 1. 0 if all Rows 
        /// 2. Value for number of rows</param>
        /// <returns>DataTable Object</returns>
        public DataTable SELECT(string tableName,List <string> fields, Dictionary<string, object> whereClause, int limits)
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;

            StringBuilder selectedfields = new StringBuilder();
            StringBuilder whereStatement = new StringBuilder();
            StringBuilder orderBy = new StringBuilder();

            if (tableName == "PhoneCalls")
                orderBy.Append("ORDER BY [SessionIdTime] DESC");
            else
                orderBy.Append("");

            if (fields != null)
            {
                if (fields.Count != 0)
                {
                    foreach (string fieldName in fields)
                    {
                        selectedfields.Append(fieldName + ",");
                    }
                    selectedfields.Remove(selectedfields.Length - 1, 1);
                }
                else
                    selectedfields.Append("*");
            }
            else 
                selectedfields.Append("*");

           if(whereClause.Count != 0)
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
               whereStatement.Remove(whereStatement.Length-5, 5);
            }

            if (limits == 0)
            {
                if (whereClause !=null)
                    selectQuery = string.Format("SELECT {0} FROM  [{1}] {2} {3}", selectedfields.ToString(), tableName, whereStatement.ToString(), orderBy);
                else
                    selectQuery = string.Format("SELECT {0} FROM  ['{1}'] {2}", selectedfields.ToString(), tableName, orderBy);
            }
            else
            {
                if (whereClause != null)
                    selectQuery = string.Format("SELECT TOP({0}) {1} FROM [{2}] {3} {4}", limits, selectedfields.ToString(), tableName, whereStatement.ToString(), orderBy);
                else
                    selectQuery = string.Format("SELECT TOP({0}) {1} FROM [{2}] {3}", limits, selectedfields.ToString(), tableName, orderBy);
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

        /// <summary>
        /// Construct Generic Select Statemnet 
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="fields">List of Fields to be fetched from Database</param>
        /// <param name="whereClause"> A dictionary which holds the fields and its related values to be able to construct Where Statemnet
        /// 1. Null if there is no condition
        /// 2. Dictionary holds fields names and values if there is a condition
        /// </param>
        /// <param name="limits">Holds how many rows to be fetched from the database table. 
        /// 1. 0 if all Rows 
        /// 2. Value for number of rows</param>
        /// <returns>DataTable Object</returns>
        public DataTable SELECT_USER_STATISTICS(string tableName, Dictionary<string, object> whereClause)
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;

            StringBuilder whereStatement = new StringBuilder();
            StringBuilder fields = new StringBuilder();
           //SourceUserUri

            if (whereClause.ContainsKey("startingDate") && whereClause.ContainsKey("endingDate"))
            {
                whereStatement.Append(String.Format(" WHERE [SourceUserUri] = '{0}' AND [SessionIdTime] >= '{1}' AND [SessionIdTime] < '{2}' and [marker_CallTypeID]=1", whereClause["SourceUserUri"].ToString(), whereClause["startingDate"].ToString(), whereClause["endingDate"].ToString()));
            }
            else 
            {
                whereStatement.Append(String.Format(" WHERE [SourceUserUri] = '{0}'", whereClause["SourceUserUri"].ToString()));
            }

            selectQuery = String.Format(
                "SELECT COUNT(*) ui_IsPersonal, ui_IsPersonal as PhoneCallType, SUM([PhoneCalls].[Duration]) as TotalDuration, SUM([PhoneCalls].[marker_CallCost]) as TotalCost from PhoneCalls {0} group by ui_IsPersonal",
                whereStatement.ToString()
            );

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

        /// <summary>
        /// Construct Generic INSERT Statement
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="columnsValues">Dictionary Holds Fields and Values to be inserted</param>
        /// <returns>Row ID </returns>
        public int INSERT(string tableName, Dictionary<string, object> columnsValues,string idFieldName) 
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

            string insertQuery = string.Format("INSERT INTO [{0}] {1} OUTPUT INSERTED.{2}  VALUES {3}", tableName, fields, idFieldName ,values);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(insertQuery, conn);

            int recordID = 0;
            try
            {
                conn.Open();
                recordID = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch (Exception ex) 
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx; 
            }
            finally { conn.Close(); }

            return recordID;
        }

        /// <summary>
        /// Construct Generic UPDATE Statement
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="columnsValues">Dictionary Holds Fields and Values to be inserted</param>
        /// <param name="idFieldName">ID Field name </param>
        /// <param name="ID">ID Value</param>
        /// <returns>Row ID</returns>
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

            string insertQuery = string.Format("UPDATE  [{0}] SET {1} WHERE [{2}]={3}", tableName, fieldsValues, idFieldName,ID);

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
        
        /// <summary>
        /// Construct Generic UPDATE Statement
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="columnsValues">Dictionary Holds Fields and Values to be inserted</param>
        /// <param name="wherePart">Dictionary Holds Fields and Values to be able to construct Where Statemnet</param>
        /// <returns></returns>
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
                    fieldsValues.Append("[" +pair.Key + "]=" + "'" + pair.Value  + "'" + ",");  
            }
            
            fieldsValues.Remove(fieldsValues.Length - 1, 1);

            foreach (KeyValuePair<string, object> pair in wherePart)
            {
                Type valueType = pair.Value.GetType();

                if (valueType == typeof(int) || valueType == typeof(Double))
                    whereStatement.Append("[" + pair.Key + "]=" + pair.Value + " AND ");
                else
                    whereStatement.Append("[" + pair.Key + "]='" + pair.Value + "' AND ");

            }
            whereStatement.Remove(whereStatement.Length - 5, 5);

            string insertQuery = string.Format("UPDATE  [{0}] SET {1} WHERE {2}", tableName, fieldsValues, whereStatement);

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

        /// <summary>
        /// Construct Generic DELETE Statement
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="idFieldName">ID Field Name</param>
        /// <param name="ID">ID Field Value</param>
        /// <returns>True if Row has been deleted. </returns>
        public bool DELETE(string tableName, string idFieldName, int ID) 
        {
            string deleteQuery = string.Format("DELETE FROM [{0}] WHERE [{1}]={2}", tableName, idFieldName,ID);

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
        
        /// <summary>
        /// Construct Generic DELETE Statement
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="wherePart">Dictionary Holds Fields and Values to be able to construct Where Statemnet</param>
        /// <returns>True if Row has been deleted.</returns>
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
            whereStatement.Remove(whereStatement.Length - 5, 5);

            string deleteQuery = string.Format("DELETE FROM [{0}] WHERE {1}", tableName, whereStatement);

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

        private static string GetDateForDatabase(DateTime dt)
        {
            return dt.Year + "-" + dt.Month + "-" + dt.Day + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second +"." + dt.Millisecond;
        }
   }
}