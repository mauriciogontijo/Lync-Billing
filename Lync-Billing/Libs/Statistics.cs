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

        public DataTable USERS_STATS(DateTime startingDate,DateTime endingDate,string siteName) 
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;

            if (startingDate != endingDate)
            {

                selectQuery = string.Format("SELECT * FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{0}') WHERE Date BETWEEN '{1}' AND '{2}'",
                siteName, startingDate, endingDate);
            }
            else if(startingDate.Year == endingDate.Year && startingDate.Month == endingDate.Month)
            {
                selectQuery = string.Format("SELECT * FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{0}') WHERE Year={1} AND Month={2}",
               siteName, startingDate.Year, startingDate.Month);
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

        public DataTable RATES_PER_GATEWAY(string RatesTableName)
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;

            selectQuery = string.Format(
                 "select " + 
		            "Country_Name, " + 
		            "Two_Digits_country_code, " + 
		            "Three_Digits_Country_Code, " +
		            "max(CASE WHEN Type_Of_Service='fixedline' then rate END) Fixedline, " +
		            "max(CASE WHEN Type_Of_Service='gsm' then rate END) GSM " +

                "from " +
                "( " +
	                "SELECT	DISTINCT " +
		                "numberingplan.Country_Name, " +
		                "numberingplan.Two_Digits_country_code, " +
		                "numberingplan.Three_Digits_Country_Code, " +
		                "numberingplan.Type_Of_Service, " +
		                "fixedrate.rate as rate " +

	                "FROM  " +
		                "dbo.NumberingPlan as numberingplan " +
		
	                "LEFT JOIN " +
		                "dbo.[Rates_10.1.1.5_2013_04_02]  as fixedrate ON " +
			                "numberingplan.Dialing_prefix = fixedrate.country_code_dialing_prefix " +
	                "WHERE " + 
		                "numberingplan.Type_Of_Service='gsm' or " +
		                "numberingplan.Type_Of_Service='fixedline' " +
                ") src " +

                "GROUP BY Country_Name,Two_Digits_country_code,Three_Digits_Country_Code ", RatesTableName); 


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