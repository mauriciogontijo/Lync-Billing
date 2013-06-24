using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Text;

namespace Lync_Billing.Libs
{
    public class Statistics
    {
        public static string ConnectionString_Lync = ConfigurationManager.ConnectionStrings["LyncConnectionString"].ConnectionString.ToString();

         private OleDbConnection DBInitializeConnection(string connectionString) 
        {
            return new OleDbConnection(connectionString);
        }

        public DataTable USER_STATS(string SipAccount, int Year, int startingMonth, int endingMonth)
         {
             DataTable dt = new DataTable();
             OleDbDataReader dr;
             string selectQuery = string.Empty;


             selectQuery = string.Format("SELECT * FROM [dbo].[fnc_Chargable_Calls_By_User] ('{0}') WHERE Year={1} AND [Month] BETWEEN {2} AND {3}",
                 SipAccount,Year,startingMonth,endingMonth);

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

        public DataTable USERS_STATS(DateTime startingDate, int startingYear,int StartingMonth,int endingYear,int endingMonth,string siteName) 
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;

            selectQuery = string.Format("SELECT * FROM [dbo].[fnc_Chargable_Calls_By_User] ('{0}') WHERE (Year BETWEEN {1} AND {2}) AND (Month BETWEEN {3} AND {4})",
                siteName, startingYear, endingYear, StartingMonth, endingMonth);

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

        private static void ConvertDateToYearMonth(DateTime date,out int year, out int month) 
         {
             year = date.Year;
             month = date.Month;
         }

    }
}