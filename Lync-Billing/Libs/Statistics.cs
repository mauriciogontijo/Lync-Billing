using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Text;
using Lync_Billing.DB;

namespace Lync_Billing.Libs
{
    public class Statistics
    {
        //private static void ConvertDateToYearMonth(DateTime date, out int year, out int month)
        //{
        //    year = date.YearNumber;
        //    month = date.Month;
        //}

        public static string ConnectionString_Lync = ConfigurationManager.ConnectionStrings["LyncConnectionString"].ConnectionString.ToString();

        private OleDbConnection DBInitializeConnection(string connectionString) 
        {
            return new OleDbConnection(connectionString);
        }

        //public DataTable USER_STATS(string SipAccount, int YearNumber, int startingMonth, int endingMonth)
        // {
        //     DataTable dt = new DataTable();
        //     OleDbDataReader dr;
        //     string selectQuery = string.Empty;


        //     selectQuery = string.Format("SELECT * FROM [dbo].[fnc_Chargable_Calls_By_User] ('{0}') WHERE YearNumber={1} AND [Month] BETWEEN {2} AND {3}",
        //         SipAccount,YearNumber,startingMonth,endingMonth);

        //     OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
        //     OleDbCommand comm = new OleDbCommand(selectQuery, conn);

        //     try
        //     {
        //         conn.Open();
        //         dr = comm.ExecuteReader();
        //         dt.Load(dr);
        //     }
        //     catch (Exception ex)
        //     {
        //         System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
        //         throw argEx;
        //     }
        //     finally { conn.Close(); }

        //     return dt;

        // }

        //public DataTable USERS_STATS(DateTime startingDate, DateTime endingDate, string siteName)
        //{
        //    DataTable dt = new DataTable();
        //    OleDbDataReader dr;
        //    string selectQuery = string.Empty;

        //    if (startingDate != endingDate)
        //    {

        //        selectQuery = string.Format("SELECT * FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{0}') WHERE Date BETWEEN '{1}' AND '{2}'",
        //        siteName, startingDate, endingDate);
        //    }
        //    else if (startingDate.YearNumber == endingDate.YearNumber && startingDate.Month == endingDate.Month)
        //    {
        //        selectQuery = string.Format("SELECT * FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{0}') WHERE YearNumber={1} AND Month={2}",
        //       siteName, startingDate.YearNumber, startingDate.Month);
        //    }

        //    OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
        //    OleDbCommand comm = new OleDbCommand(selectQuery, conn);

        //    try
        //    {
        //        conn.Open();
        //        dr = comm.ExecuteReader();
        //        dt.Load(dr);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
        //        throw argEx;
        //    }
        //    finally { conn.Close(); }

        //    return dt;
        //}

        //public DataTable USERS_STATS(DateTime startingDate, DateTime endingDate, string siteName, List<string> columns)
        //{
        //    DataTable dt = new DataTable();
        //    OleDbDataReader dr;
        //    StringBuilder selectedfields = new StringBuilder();

        //    if (columns != null)
        //    {
        //        if (columns.Count != 0)
        //        {
        //            foreach (string fieldName in columns)
        //            {
        //                selectedfields.Append(fieldName + ",");
        //            }
        //            selectedfields.Remove(selectedfields.Length - 1, 1);
        //        }
        //        else
        //            selectedfields.Append("*");
        //    }
        //    else
        //    {
        //        selectedfields.Append("*");
        //    }

        //    string selectQuery = string.Empty;

        //    if (startingDate != endingDate)
        //    {
        //        selectQuery = string.Format("SELECT {0} FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{1}') WHERE Date BETWEEN '{2}' AND '{3}'",
        //            selectedfields.ToString(), siteName, startingDate, endingDate);
        //    }
        //    else if (startingDate.YearNumber == endingDate.YearNumber && startingDate.Month == endingDate.Month)
        //    {
        //        selectQuery = string.Format("SELECT {0} FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{1}') WHERE YearNumber={1} AND Month={2}",
        //            selectedfields.ToString(), siteName, startingDate.YearNumber, startingDate.Month);
        //    }

        //    OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
        //    OleDbCommand comm = new OleDbCommand(selectQuery, conn);

        //    try
        //    {
        //        conn.Open();
        //        dr = comm.ExecuteReader();
        //        dt.Load(dr);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
        //        throw argEx;
        //    }
        //    finally { conn.Close(); }

        //    return dt;
        //}

        //public DataTable USERS_STATS(List<string> sipAccounts, DateTime startingDate, DateTime endingDate,string siteName) 
        //{
        //    DataTable dt = new DataTable();
        //    OleDbDataReader dr;
        //    string selectQuery = string.Empty;

        //    string subSelect = string.Empty;

        //    foreach (string sipAccount in sipAccounts) 
        //    {
        //        subSelect += "'" +sipAccounts + "',";
        //    }

        //    subSelect.Remove(subSelect.Length -1 , 1);

        //    if (startingDate != endingDate)
        //    {

        //        selectQuery = string.Format("SELECT * FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{0}') WHERE SourceUserUri in ({1}) AND Date BETWEEN '{2}' AND '{3}'",
        //        siteName, subSelect, startingDate, endingDate);
        //    }
        //    else if (startingDate.YearNumber == endingDate.YearNumber && startingDate.Month == endingDate.Month)
        //    {
        //        selectQuery = string.Format("SELECT * FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{0}') WHERE SourceUserUri in ({1}) AND YearNumber={2} AND Month={3}",
        //       siteName, subSelect, startingDate.YearNumber, startingDate.Month);
        //    }

        //    OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
        //    OleDbCommand comm = new OleDbCommand(selectQuery, conn);

        //    try
        //    {
        //        conn.Open();
        //        dr = comm.ExecuteReader();
        //        dt.Load(dr);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
        //        throw argEx;
        //    }
        //    finally { conn.Close(); }

        //    return dt;

        //}

        //public DataTable DISTINCT_USERS_STATS(DateTime startingDate, DateTime endingDate, string siteName, List<string> columns = null)
        //{
        //    DataTable dt = new DataTable();
        //    OleDbDataReader dr;
        //    string selectQuery = string.Empty;
        //    StringBuilder selectedFields = new StringBuilder();

        //    List<string> defaultSelectedFields = new List<string>()
        //    { 
        //        "SourceUserUri", 
        //        "EmployeeID", 
        //        "DisplayName", 
        //        "SUM(BusinessCost) AS BusinessCost", 
        //        "SUM(PersonalCost) AS PersonalCost", 
        //        "SUM(UnMarkedCost) AS UnMarkedCost" 
        //    };
            
        //    string groupBy = "SourceUserUri, EmployeeID, DisplayName";

        //    //First, initialize the selectedFields with the defaultSelectedFields
        //    foreach(string column in defaultSelectedFields)
        //    {
        //        selectedFields.Append(column + ",");
        //    }


        //    //Add the passed list of extra columns to the selectedFields
        //    if (columns != null)
        //    {
        //        if (columns.Count > 0)
        //        {
        //            foreach (string col in columns)
        //            {
        //                if (!defaultSelectedFields.Contains(col))
        //                {
        //                    selectedFields.Append(col + ",");
        //                }
        //            }

        //            selectedFields.Remove(selectedFields.Length - 1, 1);
        //        }
        //        else
        //        {
        //            selectedFields.Remove(selectedFields.Length - 1, 1);
        //        }
        //    }


        //    if (startingDate != endingDate)
        //    {
        //        selectQuery = string.Format("SELECT {0} FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{1}') WHERE Date BETWEEN '{2}' AND '{3}' Group by {4}",
        //            selectedFields.ToString(), siteName, startingDate, endingDate, groupBy);
        //    }
        //    else if (startingDate.YearNumber == endingDate.YearNumber && startingDate.Month == endingDate.Month)
        //    {
        //        selectQuery = string.Format("SELECT {0} FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{1}') WHERE YearNumber={1} AND Month={2} Group by {3}",
        //            selectedFields.ToString(), siteName, startingDate.YearNumber, startingDate.Month, groupBy);
        //    }

        //    OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
        //    OleDbCommand comm = new OleDbCommand(selectQuery, conn);

        //    try
        //    {
        //        conn.Open();
        //        dr = comm.ExecuteReader();
        //        dt.Load(dr);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
        //        throw argEx;
        //    }
        //    finally { conn.Close(); }

        //    return dt;
        //}

        //public DataTable DISTINCT_USERS_STATS(DateTime startingDate, DateTime endingDate, string siteName, List<string> SipAccountsList, List<string> columns = null)
        //{
        //    DataTable dt = new DataTable();
        //    OleDbDataReader dr;
        //    string selectQuery = string.Empty;
        //    StringBuilder selectedFields = new StringBuilder();
        //    StringBuilder sipAccountsWhereStatement = new StringBuilder();

        //    List<string> defaultSelectedFields = new List<string>()
        //    { 
        //        "SourceUserUri", 
        //        "EmployeeID", 
        //        "DisplayName", 
        //        "SUM(BusinessCost) AS BusinessCost", 
        //        "SUM(PersonalCost) AS PersonalCost", 
        //        "SUM(UnMarkedCost) AS UnMarkedCost" 
        //    };

        //    string groupBy = "SourceUserUri, EmployeeID, DisplayName";

        //    //First, initialize the selectedFields with the defaultSelectedFields
        //    foreach (string column in defaultSelectedFields)
        //    {
        //        selectedFields.Append(column + ",");
        //    }
            
        //    //Add the passed list of extra columns to the selectedFields
        //    if (columns != null)
        //    {
        //        if (columns.Count > 0)
        //        {
        //            foreach (string col in columns)
        //            {
        //                if (!defaultSelectedFields.Contains(col))
        //                {
        //                    selectedFields.Append(col + ",");
        //                }
        //            }

        //            selectedFields.Remove(selectedFields.Length - 1, 1);
        //        }
        //        else
        //        {
        //            selectedFields.Remove(selectedFields.Length - 1, 1);
        //        }
        //    }

        //    //Format the lis of SipAccounts to be included in the sql statement below
        //    if (SipAccountsList != null && SipAccountsList.Count > 0)
        //    {
        //        foreach (string SipAccount in SipAccountsList)
        //        {
        //            sipAccountsWhereStatement.Append("'" + SipAccount + "'" + ",");
        //        }
        //    }
        //    sipAccountsWhereStatement.Remove(sipAccountsWhereStatement.Length - 1, 1);

        //    if (startingDate != endingDate)
        //    {
        //        selectQuery = string.Format(
        //            "SELECT {0} FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{1}') " +
        //            "WHERE SourceUserUri in ({2}) " + 
        //            "AND Date BETWEEN '{3}' AND '{4}' " + 
        //            "Group by {5}",
        //            selectedFields.ToString(), siteName, sipAccountsWhereStatement.ToString(), startingDate, endingDate, groupBy);
        //    }
        //    else if (startingDate.YearNumber == endingDate.YearNumber && startingDate.Month == endingDate.Month)
        //    {
        //        selectQuery = string.Format(
        //            "SELECT {0} FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{1}') " +
        //            "WHERE SourceUserUri in ({2}) " + 
        //            "AND YearNumber={3} AND Month={4} Group by {5}",
        //            selectedFields.ToString(), siteName, sipAccountsWhereStatement.ToString(), startingDate.YearNumber, startingDate.Month, groupBy);
        //    }

        //    OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
        //    OleDbCommand comm = new OleDbCommand(selectQuery, conn);

        //    try
        //    {
        //        conn.Open();
        //        dr = comm.ExecuteReader();
        //        dt.Load(dr);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
        //        throw argEx;
        //    }
        //    finally { conn.Close(); }

        //    return dt;
        //}
        
        //public DataTable DISTINCT_USERS_STATS_DETAILED(DateTime startingDate, DateTime endingDate, List<string> SipAccountsList, List<string> columns)
        //{
        //    DataTable dt = new DataTable();
        //    OleDbDataReader dr;
        //    string selectQuery = string.Empty;
        //    StringBuilder selectedFields = new StringBuilder();
        //    StringBuilder sipAccountsWhereStatement = new StringBuilder();

        //    //Add the passed list of extra columns to the selectedFields
        //    if (columns != null && columns.Count > 0)
        //    {
        //        foreach (string col in columns)
        //        {
        //            selectedFields.Append(col + ",");
        //        }

        //        //this is required for the order-procedure!
        //        if (!columns.Contains("SourceUserUri"))
        //        {
        //            selectedFields.Append("SourceUserUri" + ",");
        //        }
                
        //        selectedFields.Remove(selectedFields.Length - 1, 1);
        //    }
        //    else
        //    {
        //        selectedFields.Append("*");
        //    }

        //    //Format the lis of SipAccounts to be included in the sql statement below
        //    if(SipAccountsList != null && SipAccountsList.Count > 0)
        //    {
        //        foreach (string SipAccount in SipAccountsList)
        //        {
        //            sipAccountsWhereStatement.Append("'" + SipAccount + "'" + ",");
        //        }
        //    }
        //    sipAccountsWhereStatement.Remove(sipAccountsWhereStatement.Length - 1, 1);

        //    //construct the sql query
        //    selectQuery = string.Format(
        //        "SELECT {0} FROM [dbo].[PhoneCalls] WHERE SourceUserUri in ({1}) " +
        //        "AND marker_CallTypeID = 1 " +
        //        "AND Exclude = 0 " + 
        //        "AND ResponseTime BETWEEN '{2}' AND '{3}' " + 
        //        "ORDER BY SourceUserUri ASC",
        //        selectedFields.ToString(), sipAccountsWhereStatement.ToString(), startingDate, endingDate);

        //    OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
        //    OleDbCommand comm = new OleDbCommand(selectQuery, conn);

        //    try
        //    {
        //        conn.Open();
        //        dr = comm.ExecuteReader();
        //        dt.Load(dr);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
        //        throw argEx;
        //    }
        //    finally { conn.Close(); }

        //    return dt;
        //}

        

    }
}