﻿using System;
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
        
        public decimal NumberOfOutgoingCalls { get; set; }
        public decimal TotalDuration { get; set; }
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

        public static List<GatewaysUsage> SetGatewaysUsagePercentagesPerCallsCount(int year, int fromMonth, int toMonth) 
        {
            Statistics DBRoutines = new Statistics();

            DataTable dt = new DataTable();

            List<GatewaysUsage> gatewaysUsage = new List<GatewaysUsage>();

            GatewaysUsage gatewayUsage;

            decimal totalOutGoingCallsCount = 0;
            decimal totalDurationCount = 0;
            decimal totalCostCount = 0;

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
                        DateTime.DaysInMonth(gatewayUsage.Year, gatewayUsage.Month));
                gatewaysUsage.Add(gatewayUsage);
            }

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
                   TotalCost = res.Sum(x => x.TotalCost)
               }
           ).Where(e => e.NumberOfOutgoingCalls > 200).ToList();

            //Calculate Totals

            foreach (GatewaysUsage tmpgatewayUsage in gatewaysUsageData)
            {
                totalCostCount += tmpgatewayUsage.TotalCost;
                totalOutGoingCallsCount += tmpgatewayUsage.NumberOfOutgoingCalls;
                totalDurationCount += tmpgatewayUsage.TotalDuration;
            }

            string resolvedGatewayAddress = string.Empty;

            foreach (GatewaysUsage tmpgatewayUsage in gatewaysUsageData)
            {
                if (Misc.GetResolvedConnecionIPAddress(tmpgatewayUsage.GatewayName, out resolvedGatewayAddress) == true)
                    tmpgatewayUsage.GatewayName = resolvedGatewayAddress;

                if (tmpgatewayUsage.NumberOfOutgoingCalls.ToString() != null && tmpgatewayUsage.NumberOfOutgoingCalls > 0)
                    tmpgatewayUsage.NumberOfOutgoingCallsPercentage = Math.Round((tmpgatewayUsage.NumberOfOutgoingCalls * 100 / totalOutGoingCallsCount), 2);
                else
                    tmpgatewayUsage.NumberOfOutgoingCallsPercentage = 0;

                if (tmpgatewayUsage.TotalCost.ToString() != null && tmpgatewayUsage.TotalCost > 0)
                    tmpgatewayUsage.TotalCostPercentage = Math.Round((tmpgatewayUsage.TotalCost * 100) / totalCostCount, 2);
                else
                    tmpgatewayUsage.TotalCostPercentage = 0;

                if (tmpgatewayUsage.TotalDuration.ToString() != null && tmpgatewayUsage.TotalDuration > 0)
                    tmpgatewayUsage.TotalDurationPercentage = Math.Round((tmpgatewayUsage.TotalDuration * 100 / totalDurationCount), 2);
                else
                    tmpgatewayUsage.TotalDurationPercentage = 0;
            }

            return gatewaysUsageData; 

        }
       
        public static List<GatewaysUsage> SetGatewaysUsagePercentagesPerCallsCount(List<GatewaysUsage> gatewaysUsage) 
        {
            decimal totalOutGoingCallsCount = 0;
            decimal totalDurationCount = 0;
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

            string resolvedGatewayAddress = string.Empty;
            
            foreach (GatewaysUsage gatewayUsage in gatewaysUsageData) 
            {
                if (Misc.GetResolvedConnecionIPAddress(gatewayUsage.GatewayName, out resolvedGatewayAddress) == true)
                    gatewayUsage.GatewayName = resolvedGatewayAddress;

                if (gatewayUsage.NumberOfOutgoingCalls.ToString() != null && gatewayUsage.NumberOfOutgoingCalls > 0)
                    gatewayUsage.NumberOfOutgoingCallsPercentage =  Math.Round((gatewayUsage.NumberOfOutgoingCalls * 100 / totalOutGoingCallsCount),2);
                else
                    gatewayUsage.NumberOfOutgoingCallsPercentage = 0;

                if (gatewayUsage.TotalCost.ToString() != null && gatewayUsage.TotalCost > 0)
                    gatewayUsage.TotalCostPercentage = Math.Round((gatewayUsage.TotalCost * 100 )/ totalCostCount,2);
                else
                    gatewayUsage.TotalCostPercentage = 0;

                if (gatewayUsage.TotalDuration.ToString() != null && gatewayUsage.TotalDuration > 0)
                    gatewayUsage.TotalDurationPercentage = Math.Round((gatewayUsage.TotalDuration * 100 / totalDurationCount),2);
                else
                    gatewayUsage.TotalDurationPercentage = 0;
            }

            return gatewaysUsageData; 
        }

        public static List<Years> GetYears() 
        {
            Statistics DBRoutines = new Statistics();

            DataTable dt = new DataTable();

            List<Years> years = new List<Years>();
            Years year; 

            dt = DBRoutines.GET_GATEWAYS_YEARS_OF_USAGE();
            
            foreach (DataRow row in dt.Rows)
            {
                year = new Years();

                if( row["Year"] != null )
                {
                    year.Year = (int)row["Year"];
                    years.Add(year);
                }
            }
            return years;
        }
    }


    public class TopCountries
    {
        public string CountryName { private set; get; }
        public decimal TotalDuration { private set; get; }
        public decimal TotalCost { private set; get; }

        public static List<TopCountries> GetTopDestinationsForUser(string sipAccount)
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

        public static List<TopCountries> GetTopDestinationsForDepartment(string departmentName, string siteName)
        {
            DBLib DBRoutines = new DBLib();
            DataTable dt = new DataTable();

            List<TopCountries> topCountries = new List<TopCountries>();

            List<object> parameters = new List<object>();
            parameters.Add(departmentName);
            parameters.Add(siteName);

            TopCountries topCountry;
            dt = DBRoutines.SELECT_FROM_FUNCTION("fnc_GetTop5DestinationCountriesByCostPerDepartment", parameters, null);

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

    public class Years 
    {
        public int Year { get; set; }
    }
       
}
