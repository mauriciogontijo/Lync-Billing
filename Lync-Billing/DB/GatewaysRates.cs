using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class GatewaysRates
    {
        public DBLib DBRoutines = new DBLib();

        public int GatewaysRatesID { set; get; }
        public int GatewayID { set; get; }
        public string RatesTableName { set; get; }
        public DateTime StartingDate { set; get; }
        public DateTime EndingDate { set; get; }
        public string ProviderName { set; get; }
        public string CurrencyCode { set; get; }

        public List<GatewaysRates> GetGatewaysRates(List<string> columns, Dictionary<string, object> wherePart, bool allFields, int limits)
        {
            GatewaysRates gatewayRate;
            DataTable dt = new DataTable();
            List<GatewaysRates> gatewayRates = new List<GatewaysRates>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.UsersRoles.TableName), columns, wherePart, limits, allFields);

            foreach (DataRow row in dt.Rows)
            {
                gatewayRate = new GatewaysRates();

                foreach (DataColumn column in dt.Columns)
                {

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.GatewaysRatesID))
                        gatewayRate.GatewaysRatesID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.GatewayID))
                        gatewayRate.GatewayID = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.RatesTableName))
                        gatewayRate.RatesTableName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.StartingDate))
                        gatewayRate.StartingDate = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.EndingDate))
                        gatewayRate.EndingDate = (DateTime)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.ProviderName))
                        gatewayRate.ProviderName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysRates.CurrencyCode))
                        gatewayRate.CurrencyCode = (string)row[column.ColumnName];

                }
                gatewayRates.Add(gatewayRate);
            }
            return gatewayRates;
        }

        public int InsertGatewaysRates(List<GatewaysRates> gatewayRates)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;

            foreach (GatewaysRates gatewaysRates in gatewayRates)
            {

                //Set Part
                if (gatewaysRates.GatewayID != null)
                    columnsValues.Add(Enums.GetDescription(Enums.GatewaysRates.GatewayID), gatewaysRates.GatewayID);

                if (gatewaysRates.RatesTableName != null)
                    columnsValues.Add(Enums.GetDescription(Enums.GatewaysRates.RatesTableName), gatewaysRates.RatesTableName);

                if (gatewaysRates.StartingDate != null)
                    columnsValues.Add(Enums.GetDescription(Enums.GatewaysRates.StartingDate), gatewaysRates.StartingDate);

                if (gatewaysRates.EndingDate != null)
                    columnsValues.Add(Enums.GetDescription(Enums.GatewaysRates.EndingDate), gatewaysRates.EndingDate);

                if (gatewaysRates.ProviderName != null)
                    columnsValues.Add(Enums.GetDescription(Enums.GatewaysRates.ProviderName), gatewaysRates.ProviderName);

                if (gatewaysRates.CurrencyCode != null)
                    columnsValues.Add(Enums.GetDescription(Enums.GatewaysRates.CurrencyCode), gatewaysRates.CurrencyCode);

                //Execute Insert
                rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.GatewaysRates.TableName), columnsValues);
            }
            return rowID;
        }

        public bool UpdateGatewaysRates(List<GatewaysRates> gatewayRates,int ID)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            foreach (GatewaysRates gatewaysRates in gatewayRates)
            {

                //Set Part
                if (gatewaysRates.GatewayID != null)
                    setPart.Add(Enums.GetDescription(Enums.GatewaysRates.GatewayID), gatewaysRates.GatewayID);

                if (gatewaysRates.RatesTableName != null)
                    setPart.Add(Enums.GetDescription(Enums.GatewaysRates.RatesTableName), gatewaysRates.RatesTableName);

                if (gatewaysRates.StartingDate != null)
                    setPart.Add(Enums.GetDescription(Enums.GatewaysRates.StartingDate), gatewaysRates.StartingDate);

                if (gatewaysRates.EndingDate != null)
                    setPart.Add(Enums.GetDescription(Enums.GatewaysRates.EndingDate), gatewaysRates.EndingDate);

                if (gatewaysRates.ProviderName != null)
                    setPart.Add(Enums.GetDescription(Enums.GatewaysRates.ProviderName), gatewaysRates.ProviderName);

                if (gatewaysRates.CurrencyCode != null)
                    setPart.Add(Enums.GetDescription(Enums.GatewaysRates.CurrencyCode), gatewaysRates.CurrencyCode);

                //Execute Update
                status = DBRoutines.UPDATE(Enums.GetDescription(Enums.UsersRoles.TableName), setPart, Enums.GetDescription(Enums.GatewaysRates.GatewaysRatesID), ID);

                if (status == false)
                {
                    //throw error message
                }
            }
            return true;
        }

        public bool DeleteFromGatewaysRates(List<GatewaysRates> ratesPerGateways)
        {
            bool status = false;


            return status;
        }

    }
}