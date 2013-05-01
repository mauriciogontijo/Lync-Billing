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

        public int InsertGatewaysRates(List<GatewaysRates> ratesPerGateways)
        {


            return 0;
        }

        public bool UpdateGatewaysRates(List<GatewaysRates> ratesPerGateways)
        {
            bool status = false;


            return status;
        }

        public bool DeleteFromGatewaysRates(List<GatewaysRates> ratesPerGateways)
        {
            bool status = false;


            return status;
        }

    }
}