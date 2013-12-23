using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Lync_Billing.Libs;

namespace Lync_Billing.Backend
{
    public class Countries
    {
        public string CountryName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyISOName { get; set; }
        public string CountryCodeISO2 { get; set; }
        public string CountryCodeISO3 { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static List<Countries> GetCurrencies()
        {
            Countries country;
            DataTable dt = new DataTable();
            List<Countries> currencies = new List<Countries>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Countries.TableName));

            foreach (DataRow row in dt.Rows)
            {
                country = new Countries();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Countries.CountryName) && row[column.ColumnName] != System.DBNull.Value)
                        country.CountryName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Countries.CurrencyName) && row[column.ColumnName] != System.DBNull.Value)
                        country.CurrencyName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Countries.CurrencyISOName) && row[column.ColumnName] != System.DBNull.Value)
                        country.CurrencyISOName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Countries.CountryCodeISO2) && row[column.ColumnName] != System.DBNull.Value)
                        country.CountryCodeISO2 = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Countries.CountryCodeISO3) && row[column.ColumnName] != System.DBNull.Value)
                        country.CountryCodeISO3 = (string)row[column.ColumnName];
                }
                currencies.Add(country);
            }
            return currencies;
        }
    }
}