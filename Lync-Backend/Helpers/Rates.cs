using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.OleDb;
using Lync_Backend.Libs;
using System.Configuration;


namespace Lync_Backend.Helpers
{
    class Rates
    {
        public int RateID { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public decimal FixedLineRate { get; set; }
        public decimal MobileLineRate { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static string GetRatesTableName(string gatewayName, DateTime startingDate)
        {
            StringBuilder RatesTableName = new StringBuilder();
            RatesTableName.Append("Rates_" + gatewayName + "_" + startingDate.Date.ToString("yyyymmdd"));

            return RatesTableName.ToString();
        }

        private static List<Rates> GetRates(string ratesTableName)
        {
            List<Rates> rates = new List<Rates>();
            DataTable dt = new DataTable();
            Rates rate;

            string selectQuery = Misc.CREATE_GET_RATES_PER_GATEWAY_QUERY(ratesTableName);

            dt = DBRoutines.SELECT(selectQuery, ConfigurationManager.ConnectionStrings["LyncConnectionString"].ConnectionString);

            foreach (DataRow row in dt.Rows)
            {
                rate = new Rates();

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

        public static Dictionary<int, List<Rates>> GetAllGatewaysRates() 
        {
            Dictionary<int, List<Rates>> allRates = new Dictionary<int, List<Rates>>();
            List<Rates> ratesPerGateway;

            //Get Entire GatewaysRates to be able to get all the rates  
            List<GatewaysRates> gatewayRates = GatewaysRates.GetGatewaysRates();

            if (gatewayRates.Count > 0) 
            {
                foreach (GatewaysRates GatewayRateTable in gatewayRates) 
                {
                    // Check RateTable Exists and Rates ending time is not null or set : to get uptodate rates table
                    if(GatewayRateTable.RatesTableName != null && 
                        (GatewayRateTable.EndingDate != DateTime.MinValue ||
                        GatewayRateTable.EndingDate != null ))
                    {
                        ratesPerGateway = new List<Rates>();
                        ratesPerGateway = GetRates(GatewayRateTable.RatesTableName);
                    }
                    else
                    {
                        continue;
                    }
                    
                    allRates.Add(GatewayRateTable.GatewayID, ratesPerGateway);
                }
            }

            return allRates;
        }
        
    }
}
