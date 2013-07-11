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
        Int64 RateID { set; get; }
        Int64 DialingPrefix { set; get; }
        decimal Rate { set; get; }

        private static DBLib DBRoutines = new DBLib();

        public static List<DialingPrefixsRates> GetRates(string ratesTableName)
        {
            List<DialingPrefixsRates> rates = new List<DialingPrefixsRates>();
            DataTable dt = new DataTable();
            DialingPrefixsRates rate;

            dt = DBRoutines.SELECT(ratesTableName);

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
                        rate.Rate = (decimal)row[column.ColumnName];
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

            dt = dt = DBRoutines.SELECT(ratesTableName, Enums.GetDescription(Enums.ActualRates.DialingPrefix), dialingPrefix);

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
                        rate.Rate = (decimal)row[column.ColumnName];
                }
                rates.Add(rate);
            }
            return rates;
        }
    }
}