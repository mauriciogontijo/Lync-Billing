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

        public int InsertGatewayRate(GatewaysRates gatewayRate)
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
            rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.GatewaysRates.TableName), columnsValues);

            return rowID;
        }

        public bool UpdateGatewayRate(GatewaysRates gatewayRate)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            //Where Part
            setPart.Add(Enums.GetDescription(Enums.GatewaysRates.GatewaysRatesID), gatewayRate.GatewaysRatesID);

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
            status = DBRoutines.UPDATE(Enums.GetDescription(Enums.UsersRoles.TableName), setPart, wherePart);

            if (status == false)
            {
                //throw error message
            }
            return true;
        }

        public bool DeleteGatewayRate(List<GatewaysRates> ratesPerGateways)
        {
            bool status = false;


            return status;
        }

    }
}