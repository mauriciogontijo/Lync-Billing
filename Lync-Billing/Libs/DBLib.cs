﻿using System;
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
        //public static string ConnectionString_Lync = @"Provider=SQLOLEDB.1;Data Source=10.1..55;Persist Security Info=True;Password='=25_ar;p1100';Users ID=sa;Initial Catalog=tBill";
        public static string ConnectionString_Lync = ConfigurationManager.ConnectionStrings["LyncConnectionString"].ConnectionString.ToString();

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

            if (whereValue.GetType().Equals(typeof(int)) || 
                whereValue.GetType().Equals(typeof(double)) ||
                whereValue.GetType().Equals(typeof(decimal))||
                whereValue.GetType().Equals(typeof(Int32)) ||
                whereValue.GetType().Equals(typeof(Int64)))
                selectQuery = string.Format("SELECT * FROM  [{0}] WHERE [{1}]={2}", tableName, whereField,whereValue);
            else
                selectQuery = string.Format("SELECT * FROM  [{0}] WHERE [{1}]='{2}'", tableName, whereField, whereValue);

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

        public DataTable SELECT(string tableName) 
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;

            StringBuilder selectedfields = new StringBuilder();
           
            selectQuery = string.Format("SELECT * FROM  [{0}]", tableName);

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
        /// <param name="fieldsList">List of Fields to be fetched from Database</param>
        /// <param name="whereClause"> A dictionary which holds the fields and its related values to be able to construct Where Statemnet
        /// 1. Null if there is no condition
        /// 2. Dictionary holds fields names and values if there is a condition
        /// </param>
        /// <param name="limits">Holds how many rows to be fetched from the database table. 
        /// 1. 0 if all Rows 
        /// 2. Value for number of rows</param>
        /// <param name="setWhereStatementOperatorToOR">The default operator in the where statement is "AND", if you set this one to true, the operator will be turned to "OR" in the where statement</param>
        /// <returns>DataTable Object</returns>
        public DataTable SELECT(string tableName,List <string> fieldsList, Dictionary<string, object> whereClause, int limits, bool setWhereStatementOperatorToOR = false)
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;
            string OPERATOR = (setWhereStatementOperatorToOR == true) ? " OR " : " AND ";

            StringBuilder selectedfields = new StringBuilder();
            StringBuilder whereStatement = new StringBuilder();
            StringBuilder orderBy = new StringBuilder();


            //Handle tableName
            if (tableName.Contains("Phonecalls"))
            {
                orderBy.Append("ORDER BY [SessionIdTime] DESC");
            }
            else
            {
                orderBy.Append("");
            }


            //Handle the fields collection
            if (fieldsList != null)
            {
                if (fieldsList.Count != 0)
                {
                    foreach (string field in fieldsList)
                    {
                        //selectedfields.Append(fieldName + ",");
                        if (!string.IsNullOrEmpty(field))
                        {
                            if (field.Contains("COUNT") || field.Contains("SUM") || field.Contains("YEAR") || field.Contains("MONTH") || field.Contains("DISTINCT"))
                                selectedfields.Append(field + ",");
                            else
                                selectedfields.Append("[" + field + "],");
                        }
                    }
                    selectedfields.Remove(selectedfields.Length - 1, 1);
                }
                else
                    selectedfields.Append("*");
            }
            else
            {
                selectedfields.Append("*");
            }


            //Handle the whereClause collection
            if (whereClause != null && whereClause.Count != 0)
            {
               whereStatement.Append("WHERE ");

               foreach (KeyValuePair<string, object> pair in whereClause)
               {
                    if (pair.Value == null)
                    {
                        whereStatement.Append("[" + pair.Key + "] IS NULL " + OPERATOR);
                    }

                    else if (pair.Value.ToString() == "!null") 
                    {
                        whereStatement.Append("[" + pair.Key + "] IS NOT NULL " + OPERATOR);
                    }
    
                    else if (pair.Value.ToString() == "!=0")
                    {
                        whereStatement.Append("[" + pair.Key + "] <> 0 " + OPERATOR);
                    }

                    else if (pair.Value is string && pair.Value.ToString().ToLower().Contains("like"))
                    {
                        whereStatement.Append("[" + pair.Key + "] " + pair.Value + OPERATOR);
                    }

                    else if (pair.Value is string && (pair.Value.ToString()).Contains(","))
                    {
                        var betweenStatementParts = (pair.Value.ToString()).Split(',');

                        whereStatement.Append("[" + pair.Key + "] BETWEEN ");
                        whereStatement.Append("'" + betweenStatementParts[0] + "'" + OPERATOR);
                        whereStatement.Append("'" + betweenStatementParts[1] + "' ");

                        whereStatement.Append(OPERATOR);
                    }

                    else if (pair.Value is List<int>)
                    {
                        whereStatement.Append("[" + pair.Key + "] in ( ");

                        foreach (var item in (List<int>)pair.Value)
                        {
                            whereStatement.Append(item.ToString() + ",");
                        }
                        //Remove last ','
                        whereStatement.Remove(whereStatement.Length - 1, 1);

                        whereStatement.Append(" ) " + OPERATOR);
                    }

                    else if (pair.Value is List<string>)
                    {
                        whereStatement.Append("[" + pair.Key + "] in ( ");

                        foreach (var item in (List<string>)pair.Value)
                        {
                            whereStatement.Append(item.ToString() + ",");
                        }
                        //Remove last ','
                        whereStatement.Remove(whereStatement.Length - 1, 1);

                        whereStatement.Append(" ) " + OPERATOR);
                    }

                    else
                    {
                        Type valueType = pair.Value.GetType();
                        if (valueType == typeof(int) || valueType == typeof(Double))
                        {
                            whereStatement.Append("[" + pair.Key + "]=" + pair.Value + OPERATOR);
                        }
                        else
                        {
                            whereStatement.Append("[" + pair.Key + "]='" + pair.Value + "'" + OPERATOR);
                        }
                    }
                }

                //Trim the whereStatement
                if(setWhereStatementOperatorToOR == true)
                    whereStatement.Remove(whereStatement.Length - 4, 4);
                else
                    whereStatement.Remove(whereStatement.Length - 5, 5);
            }

            if (limits == 0)
            {
                if (whereClause !=null)
                    selectQuery = string.Format("SELECT {0} FROM  [{1}] {2} {3}", selectedfields.ToString(), tableName, whereStatement.ToString(), orderBy);
                else
                    selectQuery = string.Format("SELECT {0} FROM  [{1}] {2}", selectedfields.ToString(), tableName, orderBy);
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
        
        public DataTable SELECT_FROM_FUNCTION(string databaseFunctionName, List<object> functionParams, Dictionary<string,object> whereClause, List<string> selectColumnsList = null, List<string> groupByColumnsList = null) 
        {
            DataTable dt = new DataTable();

            OleDbDataReader dr;
            string FinalSelectQuery = string.Empty;

            StringBuilder Parameters = new StringBuilder();
            StringBuilder WhereStatement = new StringBuilder();
            StringBuilder SelectColumns = new StringBuilder();
            StringBuilder GroupByFields = new StringBuilder();

            if (functionParams != null && functionParams.Count != 0)
            {
                foreach (object obj in functionParams)
                {
                    Type valueType = obj.GetType();

                    if (valueType == typeof(string) || valueType == typeof(DateTime))
                        Parameters.Append("'" + obj.ToString() + "',");
                    else
                        Parameters.Append(obj + ",");
                }
                Parameters.Remove(Parameters.Length - 1, 1);
            }

            if (selectColumnsList != null && selectColumnsList.Count != 0)
            {
                foreach (string field in selectColumnsList)
                {
                    if (!string.IsNullOrEmpty(field))
                    {
                        if (field.Contains("COUNT") || field.Contains("SUM") || field.Contains("YEAR") || field.Contains("MONTH") || field.Contains("DISTINCT"))
                            SelectColumns.Append(field + ",");
                        else
                            SelectColumns.Append("[" + field + "],");
                    }
                }
                SelectColumns.Remove(SelectColumns.Length - 1, 1);
            }

            if (groupByColumnsList != null && groupByColumnsList.Count != 0)
            {
                foreach (string field in groupByColumnsList)
                {
                    if (!string.IsNullOrEmpty(field))
                    {
                        GroupByFields.Append("[" + field + "],");
                    }
                }
                GroupByFields.Remove(GroupByFields.Length - 1, 1);
            }

            if (whereClause != null && whereClause.Count != 0)
            {
                foreach (KeyValuePair<string, object> pair in whereClause)
                {
                    if (pair.Value == null)
                    {
                        WhereStatement.Append("[" + pair.Key + "] IS NULL" + " AND ");
                    }

                    else if (pair.Value.ToString() == "!null")
                    {
                        WhereStatement.Append("[" + pair.Key + "] IS NOT NULL" + " AND ");
                    }

                    else if (pair.Value.ToString() == "!=0")
                    {
                        WhereStatement.Append("[" + pair.Key + "] <> 0" + " AND ");
                    }

                    else if (pair.Value is string && (pair.Value.ToString()).Contains(","))
                    {
                        var betweenStatementParts = (pair.Value.ToString()).Split(',');

                        WhereStatement.Append("[" + pair.Key + "] BETWEEN ");
                        WhereStatement.Append("'" + betweenStatementParts[0] + "' AND ");
                        WhereStatement.Append("'" + betweenStatementParts[1] + "' ");

                        WhereStatement.Append(" AND ");
                    }

                    else if (pair.Value is List<int>)
                    {
                        WhereStatement.Append("[" + pair.Key + "] in ( ");

                        foreach (var item in (List<int>)pair.Value)
                        {
                            WhereStatement.Append(item.ToString() + ",");
                        }
                        //Remove last ','
                        WhereStatement.Remove(WhereStatement.Length - 1, 1);

                        WhereStatement.Append(" ) AND ");
                    }

                    else if (pair.Value is List<string>)
                    {
                        WhereStatement.Append("[" + pair.Key + "] in ( ");

                        foreach (var item in (List<string>)pair.Value)
                        {
                            WhereStatement.Append("'" + item.ToString() + "',");
                        }
                        //Remove last ','
                        WhereStatement.Remove(WhereStatement.Length - 1, 1);

                        WhereStatement.Append(" ) AND ");
                    }

                    else
                    {
                        Type valueType = pair.Value.GetType();
                        if (valueType == typeof(int) || valueType == typeof(Double))
                        {
                            WhereStatement.Append("[" + pair.Key + "]=" + pair.Value + " AND ");
                        }
                        else
                        {
                            WhereStatement.Append("[" + pair.Key + "]='" + pair.Value + "' AND ");
                        }
                    }
                }
                WhereStatement.Remove(WhereStatement.Length - 5, 5);
            }

            if (selectColumnsList != null && selectColumnsList.Count > 0)
                FinalSelectQuery = String.Format("SELECT {0} ", SelectColumns.ToString());
            else
                FinalSelectQuery = String.Format("SELECT * ");

            if (whereClause != null  && whereClause.Count > 0)
                FinalSelectQuery = string.Format("{0} FROM [{1}] ({2}) WHERE {3}", FinalSelectQuery, databaseFunctionName, Parameters.ToString(), WhereStatement.ToString());
            else
                FinalSelectQuery = string.Format("{0} FROM [{1}] ({2})", FinalSelectQuery, databaseFunctionName, Parameters.ToString());

            if(groupByColumnsList != null && groupByColumnsList.Count > 0)
                FinalSelectQuery = string.Format("{0} GROUP BY {1} ", FinalSelectQuery, GroupByFields.ToString());
            else
                FinalSelectQuery = string.Format("{0}", FinalSelectQuery);


            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(FinalSelectQuery, conn);

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
                Type valueType = pair.Value.GetType();

                if (valueType == typeof(DateTime) && (DateTime)pair.Value == DateTime.MinValue)
                    continue;
                else
                    fields.Append("[" + pair.Key + "],");

                if (valueType == typeof(int) || valueType == typeof(Double))
                    values.Append(pair.Value + ",");
                else if (valueType == typeof(DateTime) && (DateTime)pair.Value == DateTime.MinValue)
                    continue;
                else
                {
                    
                    values.Append("'" + pair.Value.ToString().Replace("'","`") + "'" + ",");
                }
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
        public bool UPDATE(string tableName, Dictionary<string, object> columnsValues,string idFieldName, Int64 ID) 
        {
            StringBuilder fieldsValues = new StringBuilder();

            foreach (KeyValuePair<string, object> pair in columnsValues)
            {

                Type valueType = pair.Value.GetType();

                if (valueType == typeof(int) || valueType == typeof(Double))
                    fieldsValues.Append("[" + pair.Key + "]=" + pair.Value + ",");
                else if (valueType == typeof(DateTime) && (DateTime)pair.Value == DateTime.MinValue)
                    continue;
                else
                    fieldsValues.Append("[" + pair.Key + "]=" + "'" + pair.Value + "'" + ",");
            }

            fieldsValues.Remove(fieldsValues.Length - 1, 1);

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
                if (pair.Value == null)
                {
                    fieldsValues.Append("[" + pair.Key + "]=null,");
                }
                else if (pair.Value is int || pair.Value is Double)
                {
                    fieldsValues.Append("[" + pair.Key + "]=" + pair.Value + ",");
                }
                else if (pair.Value is DateTime && ((DateTime)pair.Value == DateTime.MinValue))
                {
                    continue;
                }
                else
                {
                    fieldsValues.Append("[" + pair.Key + "]=" + "'" + pair.Value.ToString().Replace("'", "`") + "'" + ",");
                }
            }
            
            fieldsValues.Remove(fieldsValues.Length - 1, 1);

            foreach (KeyValuePair<string, object> pair in wherePart)
            {
                Type valueType = pair.Value.GetType();

                if (valueType == typeof(DateTime) && ((DateTime)pair.Value == DateTime.MinValue))
                {
                    // DO NOTHING
                }
                else if (valueType == typeof(int) || valueType == typeof(Double))
                {
                    whereStatement.Append("[" + pair.Key + "]=" + pair.Value + " AND ");
                }
                else if (valueType == typeof(DateTime) && ((DateTime)pair.Value == DateTime.MinValue))
                {
                    continue;
                }
                else
                {
                    whereStatement.Append("[" + pair.Key + "]='" + pair.Value.ToString().Replace("'", "`") + "' AND ");
                }

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

        public bool CREATE_RATES_TABLE(string tablename) 
        {
            string createTableQuery = string.Format(
                "CREATE TABLE [dbo].[{0}] "+
                    "(" +
                    " [country_code_dialing_prefix] [bigint] NOT NULL," +
                    " [rate] [decimal](18, 4) NOT NULL," +
                    " CONSTRAINT [{0}] PRIMARY KEY NONCLUSTERED " +
                    " ([country_code_dialing_prefix] ASC )" + 
                    " WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]" ,
                tablename);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(createTableQuery.ToString(), conn);

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

        public bool CREATE(string tableName,Dictionary<string,string> columns) 
        {


            StringBuilder createTableQuery = new StringBuilder();
            createTableQuery.Append("CREATE TABLE ");
            createTableQuery.Append(tableName);
            createTableQuery.Append(" ( ");

            foreach(KeyValuePair<string,string> keyValue in columns)
            {
                createTableQuery.Append(keyValue.Key);
                createTableQuery.Append(" ");
                createTableQuery.Append(keyValue.Value);
                createTableQuery.Append(", ");
            }

            createTableQuery.Length -= 2;   //Remove trailing ", "
            createTableQuery.Append(")");

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(createTableQuery.ToString(), conn);

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