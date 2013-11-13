using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Lync_Billing.Libs;
using System.Data;
using System.Data.OleDb;

namespace Lync_Billing.DB
{
    public class Rate
    {
        public int RateID { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public decimal FixedLineRate { get; set; }
        public decimal MobileLineRate { get; set; }

        private static  DBLib DBRoutines = new DBLib();

        //THIS FUNCTION IS USED BY "RATES_PER_GATEWAY" AND "RATESTABLE_VIEW_PER_GATEWAY" PROCEDURES
        private static OleDbConnection DBInitializeConnection(string connectionString)
        {
            return new OleDbConnection(connectionString);
        }


        public static string GetRatesTableName(string gatewayName,DateTime startingDate) 
        {
            StringBuilder RatesTableName = new StringBuilder();
            RatesTableName.Append("Rates_" + gatewayName + "_" + startingDate.Date.ToString("yyyymmdd"));
           
            return RatesTableName.ToString();
        }

        //public static List<Rate> GetRates(List<string> columns, Dictionary<string, object> wherePart, int limits, string ratesTableName)
        public static List<Rate> GetRates(string ratesTableName)
        {
            List<Rate> rates = new List<Rate>();
            DataTable dt = new DataTable();
            Rate rate;

            dt = DB.Rate.RATES_PER_GATEWAY(ratesTableName);

            foreach (DataRow row in dt.Rows)
            {
                rate = new Rate();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Rates.RateID) && row[column.ColumnName] != System.DBNull.Value)
                        rate.RateID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Rates.CountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        rate.CountryCode = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Rates.CountryName) && row[column.ColumnName] != System.DBNull.Value)
                        rate.CountryName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Rates.FixedlineRate) && row[column.ColumnName] != System.DBNull.Value)
                        rate.FixedLineRate = (decimal)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Rates.MobileLineRate) && row[column.ColumnName] != System.DBNull.Value)
                        rate.MobileLineRate = (decimal)row[column.ColumnName];
                }
                rates.Add(rate);
            }

            return rates;
            
        }

        public static int InsertRate(Rate rate,string tableName)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if ((rate.CountryCode).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Rates.CountryCode), rate.CountryCode);

            if ((rate.CountryName).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Rates.CountryName), rate.CountryName);

            if ((rate.FixedLineRate).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Rates.FixedlineRate), rate.FixedLineRate);

            if ((rate.MobileLineRate).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.Rates.MobileLineRate), rate.MobileLineRate);

            //Execute Insert
            rowID = DBRoutines.INSERT(tableName , columnsValues, Enums.GetDescription(Enums.Rates.RateID));

            return rowID;
        }

        public static bool UpdatetRate(Rate rate, string ratesTableName)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part
            if ((rate.CountryName).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Rates.CountryName), rate.CountryName);

            if ((rate.CountryCode).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Rates.CountryCode), rate.CountryCode);

            if ((rate.FixedLineRate).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Rates.FixedlineRate), rate.FixedLineRate);

            if ((rate.MobileLineRate).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.Rates.MobileLineRate), rate.MobileLineRate);

            //Execute Update
            status = DBRoutines.UPDATE(
                ratesTableName,
                setPart,
                Enums.GetDescription(Enums.Rates.RateID),
                rate.RateID);

            return status;
        }

        public static bool DeleteRate(Rate rate,string ratesTableName)
        {
             bool status = false;

            status = DBRoutines.DELETE(
                ratesTableName, 
                Enums.GetDescription(Enums.Rates.RateID),
                rate.RateID);

            return status;
        }

        public static bool CreateRatesTable(string tablename) 
        {
            return DBRoutines.CREATE_RATES_TABLE(tablename);
        }

        
        /*********** DATABASE FUNCTIONS WITH STATIC SQL QUERIES *************/

        public static DataTable RATES_PER_GATEWAY(string RatesTableName)
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


            OleDbConnection conn = DBInitializeConnection(DBLib.ConnectionString_Lync);
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

        public static DataTable RATESTABLE_VIEW_PER_GATEWAY(string RatesTableName, string conditionField = "NA", object conditionValue = null)
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;
            string whereStatement = string.Empty;

            if (conditionField != "NA" && conditionValue != null)
            {
                if (conditionField == "Dialing_prefix")
                    whereStatement = string.Format("WHERE {0} = {1}", conditionField, conditionValue);
                else
                    whereStatement = string.Format("WHERE {0} = '{1}'", conditionField, conditionValue);
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

            OleDbConnection conn = DBInitializeConnection(DBLib.ConnectionString_Lync);
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
    }
}