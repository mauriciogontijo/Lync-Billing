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

        public DataTable USERS_STATS(DateTime startingDate, DateTime endingDate, string siteName)
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;

            if (startingDate != endingDate)
            {

                selectQuery = string.Format("SELECT * FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{0}') WHERE Date BETWEEN '{1}' AND '{2}'",
                siteName, startingDate, endingDate);
            }
            else if (startingDate.Year == endingDate.Year && startingDate.Month == endingDate.Month)
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

        public DataTable USERS_STATS(DateTime startingDate, DateTime endingDate, string siteName, List<string> columns)
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            StringBuilder selectedfields = new StringBuilder();

            if (columns != null)
            {
                if (columns.Count != 0)
                {
                    foreach (string fieldName in columns)
                    {
                        selectedfields.Append(fieldName + ",");
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

            string selectQuery = string.Empty;

            if (startingDate != endingDate)
            {
                selectQuery = string.Format("SELECT {0} FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{1}') WHERE Date BETWEEN '{2}' AND '{3}'",
                    selectedfields.ToString(), siteName, startingDate, endingDate);
            }
            else if (startingDate.Year == endingDate.Year && startingDate.Month == endingDate.Month)
            {
                selectQuery = string.Format("SELECT {0} FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{1}') WHERE Year={1} AND Month={2}",
                    selectedfields.ToString(), siteName, startingDate.Year, startingDate.Month);
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

        public DataTable DISTINCT_USERS_STATS(DateTime startingDate, DateTime endingDate, string siteName)
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectFields = "SourceUserUri, AD_UserID, AD_DisplayName, SUM(BusinessCost) AS BusinessCost, SUM(PersonalCost) AS PersonalCost, SUM(UnMarkedCost) AS UnMarkedCost";
            string groupBy = "SourceUserUri, AD_UserID, AD_DisplayName";

            string selectQuery = string.Empty;

            if (startingDate != endingDate)
            {
                selectQuery = string.Format("SELECT {0} FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{1}') WHERE Date BETWEEN '{2}' AND '{3}' Group by {4}",
                    selectFields, siteName, startingDate, endingDate, groupBy);
            }
            else if (startingDate.Year == endingDate.Year && startingDate.Month == endingDate.Month)
            {
                selectQuery = string.Format("SELECT {0} FROM [dbo].[fnc_Chargable_Calls_By_Site] ('{1}') WHERE Year={1} AND Month={2} Group by {3}",
                    selectFields, siteName, startingDate.Year, startingDate.Month, groupBy);
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
		            "max(CASE WHEN Type_Of_Service <> 'gsm'  then rate END ) Fixedline, " +
                    "max(CASE WHEN Type_Of_Service='gsm'then rate END) GSM " +

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
		                "dbo.[{0}]  as fixedrate ON " +
			                "numberingplan.Dialing_prefix = fixedrate.country_code_dialing_prefix " +
                    //"-- WHERE " +
                    //    "-- numberingplan.Type_Of_Service='gsm' or " +
                    //    "-- numberingplan.Type_Of_Service='fixedline' " +
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
                //throw argEx;
            }
            finally { conn.Close(); }

            return dt;

        }

        public DataTable RATESTABLE_VIEW_PER_GATEWAY(string RatesTableName,string conditionField="NA", object conditionValue=null) 
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;
            string whereStatement = string.Empty;

            if (conditionField != "NA" && conditionValue != null)
            {
                if (conditionField == "Dialing_prefix")
                    whereStatement = string.Format("WHERE {0} = {1}",conditionField,conditionValue);
                else
                    whereStatement = string.Format("WHERE {0} = '{1}'",conditionField,conditionValue);
            }

            selectQuery = string.Format
                       ("SELECT " +
                           "Rate_ID, " +
                           "Dialing_prefix, " +
                           "Country_Name, " +
                           "Two_Digits_country_code, " +
                           "Three_Digits_Country_Code, " +
                           "City, " +
                           "Provider, " +
                           "Type_Of_Service, " +
                           "rate " +
                        "FROM " +
                           "NumberingPlan LEFT OUTER JOIN " +
                               "[{0}] ON " +
                                   "Dialing_prefix = country_code_dialing_prefix {1}"
                       , RatesTableName, whereStatement);

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
                //throw argEx;
            }
            finally { conn.Close(); }

            return dt;
        }

        public DataTable GET_GATEWAYS_USAGE(int year, int fromMonth, int toMonth) 
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;

            selectQuery = string.Format("SELECT * FROM [dbo].[vw_Getways_Statistics] WHERE Year={0} AND Month BETWEEN {1} AND {2}", year, fromMonth, toMonth);

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

        public DataTable GET_GATEWAYS_YEARS_OF_USAGE() 
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;

            selectQuery = string.Format("SELECT DISTINCT Year FROM [dbo].[vw_Getways_Statistics]");

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