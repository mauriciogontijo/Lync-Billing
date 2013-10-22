using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Lync_Backend.Libs;

namespace Lync_Backend.Helpers
{
    class GatewaysRates
    {
        private static DBLib DBRoutines = new DBLib();

        public int GatewaysRatesID { set; get; }
        public int GatewayID { set; get; }
        public string RatesTableName { set; get; }
        public DateTime StartingDate { set; get; }
        public DateTime EndingDate { set; get; }
        public string ProviderName { set; get; }
        public string CurrencyCode { set; get; }


        public static List<GatewaysRates> GetGatewaysRates()
        {
            GatewaysRates GatewaysRates;
            DataTable dt = new DataTable();
            List<GatewaysRates> GatewaysRatess = new List<GatewaysRates>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.GatewaysRates.TableName));

            foreach (DataRow row in dt.Rows)
            {
                GatewaysRates = new GatewaysRates();

                foreach (DataColumn column in dt.Columns)
                {

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.GatewaysRatesID) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.GatewaysRatesID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.GatewayID) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.RatesTableName) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.RatesTableName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.StartingDate) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.StartingDate = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.EndingDate) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.EndingDate = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.ProviderName) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.ProviderName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.CurrencyCode) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.CurrencyCode = (string)row[column.ColumnName];

                }
                GatewaysRatess.Add(GatewaysRates);
            }
            return GatewaysRatess;
        }

        public static List<GatewaysRates> GetGatewaysRates(int gatewayID)
        {
            GatewaysRates GatewaysRates;
            DataTable dt = new DataTable();
            List<GatewaysRates> GatewaysRatess = new List<GatewaysRates>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.GatewaysRates.TableName),
                                   Enums.GetDescription(Enums.GatewaysRates.GatewayID), gatewayID);

            foreach (DataRow row in dt.Rows)
            {
                GatewaysRates = new GatewaysRates();

                foreach (DataColumn column in dt.Columns)
                {

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.GatewaysRatesID) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.GatewaysRatesID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.GatewayID) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.RatesTableName) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.RatesTableName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.StartingDate) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.StartingDate = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.EndingDate) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.EndingDate = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.ProviderName) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.ProviderName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.CurrencyCode) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.CurrencyCode = (string)row[column.ColumnName];

                }
                GatewaysRatess.Add(GatewaysRates);
            }
            return GatewaysRatess;

        }

        public static List<GatewaysRates> GetGatewaysRates(List<string> columns, Dictionary<string, object> wherePart, int limits)
        {
            GatewaysRates GatewaysRates;
            DataTable dt = new DataTable();
            List<GatewaysRates> GatewaysRatesList = new List<GatewaysRates>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.GatewaysRates.TableName), columns, wherePart, limits);

            foreach (DataRow row in dt.Rows)
            {
                GatewaysRates = new GatewaysRates();

                foreach (DataColumn column in dt.Columns)
                {

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.GatewaysRatesID) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.GatewaysRatesID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.GatewayID) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.RatesTableName) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.RatesTableName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.StartingDate) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.StartingDate = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.EndingDate) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.EndingDate = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.ProviderName) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.ProviderName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.CurrencyCode) && row[column.ColumnName] != System.DBNull.Value)
                        GatewaysRates.CurrencyCode = (string)row[column.ColumnName];

                }
                GatewaysRatesList.Add(GatewaysRates);
            }
            return GatewaysRatesList;
        }

    }
}
