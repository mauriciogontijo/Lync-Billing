using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class DialingPrefixsRates
    {
        public Int64 RateID { set; get; }
        public Int64 DialingPrefix { set; get; }
        public decimal CountryRate { set; get; }
        public string CountryName { get; set; }
        public string TwoDigitsCountryCode { get; set; }
        public string ThreeDigitsCountryCode { get; set; }
        public string City { get; set; }
        public string Provider { get; set; }
        public string TypeOfService { get; set; }

        private static DBLib DBRoutines = new DBLib();
        private static Statistics statRoutines = new Statistics();

        public static List<DialingPrefixsRates> GetRates(string ratesTableName)
        {
            List<DialingPrefixsRates> rates = new List<DialingPrefixsRates>();
            DataTable dt = new DataTable();
            DialingPrefixsRates rate;

            dt = statRoutines.RATESTABLE_VIEW_PER_GATEWAY(ratesTableName);

            foreach (DataRow row in dt.Rows)
            {
                rate = new DialingPrefixsRates();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.RateID) && row[column.ColumnName] != System.DBNull.Value)
                        rate.RateID = (Int64)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.DialingPrefix) && row[column.ColumnName] != System.DBNull.Value)
                        rate.DialingPrefix = (Int64)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.Rate) && row[column.ColumnName] != System.DBNull.Value)
                        rate.CountryRate = (decimal)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.CountryName) && row[column.ColumnName] != System.DBNull.Value)
                        rate.CountryName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.TwoDigitsCountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        rate.TwoDigitsCountryCode = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.ThreeDigitsCountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        rate.ThreeDigitsCountryCode = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.City) && row[column.ColumnName] != System.DBNull.Value)
                        rate.City = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.Provider) && row[column.ColumnName] != System.DBNull.Value)
                        rate.Provider = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.TypeOfService) && row[column.ColumnName] != System.DBNull.Value)
                        rate.TypeOfService = (string)row[column.ColumnName];
                }
                rates.Add(rate);
            }
            return rates;
        }

        public static List<DialingPrefixsRates> GetRates(string ratesTableName, Int64 dialingPrefix)
        {
            List<DialingPrefixsRates> rates = new List<DialingPrefixsRates>();
            DataTable dt = new DataTable();
            DialingPrefixsRates rate;

            dt = statRoutines.RATESTABLE_VIEW_PER_GATEWAY(ratesTableName,Enums.GetDescription(Enums.ActualRates.DialingPrefix),dialingPrefix);

            foreach (DataRow row in dt.Rows)
            {
                rate = new DialingPrefixsRates();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.RateID) && row[column.ColumnName] != System.DBNull.Value)
                        rate.RateID = (Int64)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.DialingPrefix) && row[column.ColumnName] != System.DBNull.Value)
                        rate.DialingPrefix = (Int64)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.Rate) && row[column.ColumnName] != System.DBNull.Value)
                        rate.CountryRate = (decimal)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.RateID) && row[column.ColumnName] != System.DBNull.Value)
                        rate.RateID = (Int64)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.DialingPrefix) && row[column.ColumnName] != System.DBNull.Value)
                        rate.DialingPrefix = (Int64)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.Rate) && row[column.ColumnName] != System.DBNull.Value)
                        rate.CountryRate = (decimal)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.CountryName) && row[column.ColumnName] != System.DBNull.Value)
                        rate.CountryName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.TwoDigitsCountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        rate.TwoDigitsCountryCode = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.ThreeDigitsCountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        rate.ThreeDigitsCountryCode = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.City) && row[column.ColumnName] != System.DBNull.Value)
                        rate.City = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.Provider) && row[column.ColumnName] != System.DBNull.Value)
                        rate.Provider = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.TypeOfService) && row[column.ColumnName] != System.DBNull.Value)
                        rate.TypeOfService = (string)row[column.ColumnName];
                }
                rates.Add(rate);
            }
            return rates;
        }

        public static List<DialingPrefixsRates> GetRates(string ratesTableName, string countryCode)
        {
            List<DialingPrefixsRates> rates = new List<DialingPrefixsRates>();
            DataTable dt = new DataTable();
            DialingPrefixsRates rate;

            dt = statRoutines.RATESTABLE_VIEW_PER_GATEWAY(ratesTableName, Enums.GetDescription(Enums.ActualRates.ThreeDigitsCountryCode), countryCode);

            foreach (DataRow row in dt.Rows)
            {
                rate = new DialingPrefixsRates();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.RateID) && row[column.ColumnName] != System.DBNull.Value)
                        rate.RateID = (Int64)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.DialingPrefix) && row[column.ColumnName] != System.DBNull.Value)
                        rate.DialingPrefix = (Int64)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.Rate) && row[column.ColumnName] != System.DBNull.Value)
                        rate.CountryRate = (decimal)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.RateID) && row[column.ColumnName] != System.DBNull.Value)
                        rate.RateID = (Int64)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.DialingPrefix) && row[column.ColumnName] != System.DBNull.Value)
                        rate.DialingPrefix = (Int64)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.Rate) && row[column.ColumnName] != System.DBNull.Value)
                        rate.CountryRate = (decimal)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.CountryName) && row[column.ColumnName] != System.DBNull.Value)
                        rate.CountryName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.TwoDigitsCountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        rate.TwoDigitsCountryCode = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.ThreeDigitsCountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        rate.ThreeDigitsCountryCode = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.City) && row[column.ColumnName] != System.DBNull.Value)
                        rate.City = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.Provider) && row[column.ColumnName] != System.DBNull.Value)
                        rate.Provider = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.ActualRates.TypeOfService) && row[column.ColumnName] != System.DBNull.Value)
                        rate.TypeOfService = (string)row[column.ColumnName];
                }
                rates.Add(rate);
            }
            return rates;
        }

        public static int InsertRate(string ratesTableName, DialingPrefixsRates dialingPrefixRate) 
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            //Set Part
            if ((dialingPrefixRate.DialingPrefix).ToString() != null)
                columnsValues.Add("country_code_dialing_prefix", dialingPrefixRate.DialingPrefix);

            if ((dialingPrefixRate.CountryRate).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.ActualRates.Rate), dialingPrefixRate.CountryRate);

            //Execute Insert
            rowID = DBRoutines.INSERT(ratesTableName, columnsValues, Enums.GetDescription(Enums.ActualRates.RateID));

            return rowID;
        }

        public static bool UpdatetRate(string ratesTableName, DialingPrefixsRates dialingPrefixRate)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part
            if ((dialingPrefixRate.DialingPrefix).ToString() != null)
                setPart.Add("country_code_dialing_prefix", dialingPrefixRate.DialingPrefix);

            if ((dialingPrefixRate.CountryRate).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.ActualRates.Rate), dialingPrefixRate.CountryRate);
           

            //Execute Update
            status = DBRoutines.UPDATE(
                ratesTableName,
                setPart,
                Enums.GetDescription(Enums.ActualRates.RateID),
                dialingPrefixRate.RateID);

            return status;
        }
    }
}