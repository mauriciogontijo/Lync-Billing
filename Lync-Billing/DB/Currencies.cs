using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class Currencies
    {
        public string CountryName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyISOName { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static List<Currencies> GetCurrencies()
        {
            Currencies currency;
            DataTable dt = new DataTable();
            List<Currencies> currencies = new List<Currencies>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Currencies.TableName));

            foreach (DataRow row in dt.Rows)
            {
                currency = new Currencies();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.Currencies.CountryName) && row[column.ColumnName] != System.DBNull.Value)
                        currency.CountryName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Currencies.CurrencyName) && row[column.ColumnName] != System.DBNull.Value)
                        currency.CurrencyName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.Currencies.CurrencyISOName) && row[column.ColumnName] != System.DBNull.Value)
                        currency.CurrencyISOName = (string)row[column.ColumnName];
                }
                currencies.Add(currency);
            }
            return currencies;
        }
    }
}