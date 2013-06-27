using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class GatewayRate
    {
        private static DBLib DBRoutines = new DBLib();

        public int GatewaysRatesID { set; get; }
        public int GatewayID { set; get; }
        public string RatesTableName { set; get; }
        public DateTime StartingDate { set; get; }
        public DateTime EndingDate { set; get; }
        public string ProviderName { set; get; }
        public string CurrencyCode { set; get; }


        public static List<GatewayRate> GetGatewaysRates() 
        {
            GatewayRate gatewayRate;
            DataTable dt = new DataTable();
            List<GatewayRate> gatewayRates = new List<GatewayRate>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.GatewaysRates.TableName));

            foreach (DataRow row in dt.Rows)
            {
                gatewayRate = new GatewayRate();

                foreach (DataColumn column in dt.Columns)
                {

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.GatewaysRatesID) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.GatewaysRatesID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.GatewayID) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.RatesTableName) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.RatesTableName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.StartingDate) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.StartingDate = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.EndingDate) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.EndingDate = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.ProviderName) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.ProviderName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.CurrencyCode) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.CurrencyCode = (string)row[column.ColumnName];

                }
                gatewayRates.Add(gatewayRate);
            }
            return gatewayRates;
        } 
        
        public static List<GatewayRate> GetGatewaysRates(int gatewayID) 
        {
            GatewayRate gatewayRate;
            DataTable dt = new DataTable();
            List<GatewayRate> gatewayRates = new List<GatewayRate>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.GatewaysRates.TableName), 
                                   Enums.GetDescription(Enums.GatewaysRates.GatewayID),gatewayID);

            foreach (DataRow row in dt.Rows)
            {
                gatewayRate = new GatewayRate();

                foreach (DataColumn column in dt.Columns)
                {

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.GatewaysRatesID) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.GatewaysRatesID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.GatewayID) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.RatesTableName) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.RatesTableName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.StartingDate) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.StartingDate = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.EndingDate) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.EndingDate = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.ProviderName) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.ProviderName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.CurrencyCode) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.CurrencyCode = (string)row[column.ColumnName];

                }
                gatewayRates.Add(gatewayRate);
            }
            return gatewayRates;

        }

        public static List<GatewayRate> GetGatewaysRates(List<string> columns, Dictionary<string, object> wherePart, int limits)
        {
            GatewayRate gatewayRate;
            DataTable dt = new DataTable();
            List<GatewayRate> gatewayRates = new List<GatewayRate>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.GatewaysRates.TableName), columns, wherePart, limits);

            foreach (DataRow row in dt.Rows)
            {
                gatewayRate = new GatewayRate();

                foreach (DataColumn column in dt.Columns)
                {

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.GatewaysRatesID) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.GatewaysRatesID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.GatewayID) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.RatesTableName) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.RatesTableName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.StartingDate) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.StartingDate = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.EndingDate) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.EndingDate = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.ProviderName) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.ProviderName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.CurrencyCode) && row[column.ColumnName] != System.DBNull.Value)
                        gatewayRate.CurrencyCode = (string)row[column.ColumnName];

                }
                gatewayRates.Add(gatewayRate);
            }
            return gatewayRates;
        }

        public static int InsertGatewayRate(GatewayRate gatewayRate)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

                //Set Part
            if ((gatewayRate.GatewayID).ToString() != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysRates.GatewayID), gatewayRate.GatewayID);

            if (gatewayRate.RatesTableName != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysRates.RatesTableName), gatewayRate.RatesTableName);

            if (gatewayRate.StartingDate != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysRates.StartingDate), gatewayRate.StartingDate);

            if (gatewayRate.EndingDate != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysRates.EndingDate), gatewayRate.EndingDate);

            if (gatewayRate.ProviderName != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysRates.ProviderName), gatewayRate.ProviderName);

            if (gatewayRate.CurrencyCode != null)
                columnsValues.Add(Enums.GetDescription(Enums.GatewaysRates.CurrencyCode), gatewayRate.CurrencyCode);

            //Execute Insert
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.GatewaysRates.TableName), columnsValues, Enums.GetDescription(Enums.GatewaysRates.GatewaysRatesID));

            return rowID;
        }

        public static bool UpdateGatewayRate(GatewayRate gatewayRate)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            //Set Part

            if ((gatewayRate.GatewayID).ToString() != null)
                setPart.Add(Enums.GetDescription(Enums.GatewaysRates.GatewayID), gatewayRate.GatewayID);

            if (gatewayRate.RatesTableName != null)
                setPart.Add(Enums.GetDescription(Enums.GatewaysRates.RatesTableName), gatewayRate.RatesTableName);

            if (gatewayRate.StartingDate != null)
                setPart.Add(Enums.GetDescription(Enums.GatewaysRates.StartingDate), gatewayRate.StartingDate);

            if (gatewayRate.EndingDate != null)
                setPart.Add(Enums.GetDescription(Enums.GatewaysRates.EndingDate), gatewayRate.EndingDate);

            if (gatewayRate.ProviderName != null)
                setPart.Add(Enums.GetDescription(Enums.GatewaysRates.ProviderName), gatewayRate.ProviderName);

            if (gatewayRate.CurrencyCode != null)
                setPart.Add(Enums.GetDescription(Enums.GatewaysRates.CurrencyCode), gatewayRate.CurrencyCode);

            //Execute Update
            status = DBRoutines.UPDATE(
                Enums.GetDescription(Enums.GatewaysRates.TableName),
                setPart, Enums.GetDescription(Enums.GatewaysRates.GatewaysRatesID),
                gatewayRate.GatewaysRatesID);

            if (status == false)
            {
                //throw error message
            }
            return true;
        }

    }
}