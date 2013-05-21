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
        public static string ConnectionString_Lync = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString();

         private OleDbConnection DBInitializeConnection(string connectionString) 
        {
            return new OleDbConnection(connectionString);
        }

         public static DataTable USER_SUMMARY(string SipAccount, int Year, int startingMonth, int endingMonth)
         {
             DataTable dt = new DataTable();
             OleDbDataReader dr;
             string selectQuery = string.Empty;


             selectQuery = string.Format("SELECT * FROM [dbo].[fnc_Chargable_Calls_By_User] ('{0}') WHERE Year={1} AND [Month] BETWEEN {2} AND {3}",
                 SipAccount,Year,startingMonth,endingMonth);




         }

         private static void ConvertDateToYearMonth(DateTime date,out int year, out int month) 
         {
             year = date.Year;
             month = date.Month;
         }

    }
}