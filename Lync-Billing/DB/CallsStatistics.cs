using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class GatewaysUsage
    {
        public string GatewayName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime Date { get; set; }
        
        public long NumberOfOutgoingCalls { get; set; }
        public long TotalDuration { get; set; }
        public decimal TotalCost { set; get; }

        public decimal NumberOfOutgoingCallsPercentage { get; set; }
        public decimal TotalDurationPercentage { get; set; }
        public decimal TotalCostPercentage { get; set; }

        public static List<GatewaysUsage> GetGatewaysUsage (int year,int fromMonth, int toMonth)
        {
            Statistics DBRoutines = new Statistics();

            DataTable dt = new DataTable();

            List<GatewaysUsage> gatewaysUsage = new List<GatewaysUsage>();

            GatewaysUsage gatewayUsage;

            dt = DBRoutines.GET_GATEWAYS_USAGE(year, fromMonth, toMonth);

            foreach (DataRow row in dt.Rows)
            {
                gatewayUsage = new GatewaysUsage();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == "ToGateway" && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.GatewayName = (string)row[column.ColumnName];

                     if (column.ColumnName == "Month" && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.Month = (int)row[column.ColumnName];

                    if (column.ColumnName == "Year" && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.Year = (int)row[column.ColumnName];

                    if (column.ColumnName == "NumberOfOutgoingCalls" && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.NumberOfOutgoingCalls = Convert.ToInt64(row[column.ColumnName]);

                    if (column.ColumnName == "TotalDuration" && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.TotalDuration = Convert.ToInt64((decimal)row[column.ColumnName]);
                   

                    if (column.ColumnName == "TotalCost" && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.TotalCost = (decimal)row[column.ColumnName];
                   
                }

                gatewayUsage.Date = 
                    new DateTime(
                        gatewayUsage.Year, 
                        gatewayUsage.Month, 
                        DateTime.DaysInMonth(gatewayUsage.Year,gatewayUsage.Month));
                gatewaysUsage.Add(gatewayUsage);
            }
            return gatewaysUsage;
          }


        public static List<GatewaysUsage> GetGatewaysStatisticsResults(List<GatewaysUsage> gatewaysUsage) 
        {
            var gatewaysUsageData =
              (
                  from data in gatewaysUsage.AsEnumerable()

                  group data by new { data.GatewayName, data.Year } into res

                  select new GatewaysUsage
                  {
                      GatewayName = res.Key.GatewayName,
                      Year = res.Key.Year,
                      NumberOfOutgoingCalls = res.Sum(x => x.NumberOfOutgoingCalls),
                      TotalDuration = res.Sum(x => x.TotalDuration),
                      TotalCost = res.Sum(x => x.TotalCost),
                  }
              ).Where(e => e.NumberOfOutgoingCalls > 200).ToList();

            return gatewaysUsageData;
        }

        public static List<GatewaysUsage> SetGatewaysUsagePercentagesPerCallsCount(List<GatewaysUsage> gatewaysUsage) 
        {
            long totalOutGoingCallsCount = 0;
            long totalDurationCount = 0;
            decimal totalCostCount = 0;

            var gatewaysUsageData =
            (
                from data in gatewaysUsage.AsEnumerable()

                group data by new { data.GatewayName, data.Year} into res

                select new GatewaysUsage
                {
                    GatewayName = res.Key.GatewayName,
                    Year = res.Key.Year,

                    NumberOfOutgoingCalls = res.Sum(x => x.NumberOfOutgoingCalls),
                    TotalDuration = res.Sum(x => x.TotalDuration),
                    TotalCost = res.Sum(x => x.TotalCost)
                }
            ).Where(e => e.NumberOfOutgoingCalls > 200).ToList(); 

            foreach (GatewaysUsage gatewayUsage in gatewaysUsageData) 
            {
                totalCostCount += gatewayUsage.TotalCost;
                totalOutGoingCallsCount += gatewayUsage.NumberOfOutgoingCalls;
                totalDurationCount += gatewayUsage.TotalDuration;
            }


            foreach (GatewaysUsage gatewayUsage in gatewaysUsageData) 
            {
                if (gatewayUsage.NumberOfOutgoingCalls.ToString() != null && gatewayUsage.NumberOfOutgoingCalls > 0)
                    gatewayUsage.NumberOfOutgoingCallsPercentage = (gatewayUsage.NumberOfOutgoingCalls * 100 )/ totalOutGoingCallsCount;
                else
                    gatewayUsage.NumberOfOutgoingCallsPercentage = 0;

                if (gatewayUsage.NumberOfOutgoingCalls.ToString() != null && gatewayUsage.NumberOfOutgoingCalls > 0)
                    gatewayUsage.TotalCostPercentage = Math.Round((gatewayUsage.TotalCost * 100 )/ totalCostCount,3);
                else
                    gatewayUsage.TotalCostPercentage = 0;

                if (gatewayUsage.NumberOfOutgoingCalls.ToString() != null && gatewayUsage.NumberOfOutgoingCalls > 0)
                    gatewayUsage.TotalDurationPercentage = Math.Round((gatewayUsage.TotalDuration) * 100 / totalCostCount,3);
                else
                    gatewayUsage.TotalDurationPercentage = 0;
            }

            return gatewaysUsageData; 
        }

        public static List<int> GetYears() 
        {
            Statistics DBRoutines = new Statistics();

            DataTable dt = new DataTable();

            List<int> years = new List<int>();

            foreach (DataRow row in dt.Rows)
            {
                if( row["Year"] != null )
                    years.Add((int)row["Year"]);
            }
            return years;
        }
    }


    public class TopCountries
    {
        public string CountryName { private set; get; }
        public decimal TotalDuration { private set; get; }
        public decimal TotalCost { private set; get; }

        public static List<TopCountries> GetTopDestinations(string sipAccount)
        {
            DBLib DBRoutines = new DBLib();
            DataTable dt = new DataTable();

            List<TopCountries> topCountries = new List<TopCountries>();

            List<object> parameters = new List<object>();

            parameters.Add(sipAccount);

            TopCountries topCountry;

            dt = DBRoutines.SELECT_FROM_FUNCTION("fnc_GetTop5DestinationCountriesByCost", parameters, null);

            foreach (DataRow row in dt.Rows)
            {
                topCountry = new TopCountries();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == "Country_Name")
                        topCountry.CountryName = (string)row[column.ColumnName];

                    if (column.ColumnName == "TotalDuration")
                        topCountry.TotalDuration = (decimal)row[column.ColumnName];

                    if (column.ColumnName == "TotalCost")
                    {
                        if (row[column.ColumnName] != System.DBNull.Value)
                            topCountry.TotalCost = (decimal)row[column.ColumnName];
                        else
                            topCountry.TotalCost = 0;
                    }
                }
                topCountries.Add(topCountry);
            }

            return topCountries;
        }
    }

    public class TopDestinations 
    {
        public string PhoneNumber { private set; get; }
        public string UserName { set; get; }
        public long NumberOfPhoneCalls { private set; get; }

        public static List<TopDestinations> GetTopDestinations(string sipAccount)
        {
            DBLib DBRoutines = new DBLib();
            DataTable dt = new DataTable();

            List<TopDestinations> topDestinations = new List<TopDestinations>();
            
            List<object> parameters = new List<object>();

            parameters.Add(sipAccount);

            TopDestinations topDestination;

            dt = DBRoutines.SELECT_FROM_FUNCTION("fnc_GetTop5DestinationNumbersByCount", parameters, null);

            foreach (DataRow row in dt.Rows)
            {
                topDestination = new TopDestinations();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == "PhoneNumber")
                    {
                        if (row[column.ColumnName] == System.DBNull.Value )
                            topDestination.PhoneNumber = "UNKNOWN";
                        else
                            topDestination.PhoneNumber = (string)row[column.ColumnName];
                    }

                    if (column.ColumnName == "NumberOfPhoneCalls")
                        topDestination.NumberOfPhoneCalls = (int)row[column.ColumnName];
                }
                topDestinations.Add(topDestination);
            }

            return topDestinations;
        }
    }
}
