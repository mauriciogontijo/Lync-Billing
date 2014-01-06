using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Lync_Billing.Libs;


namespace Lync_Billing.Backend.Statistics
{
    public class GatewaysStatistics
    {
        private static DBLib DBRoutines = new DBLib();

        public string GatewayName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime Date { get; set; }

        public decimal CallsCount { get; set; }
        public decimal CallsDuration { get; set; }
        public decimal CallsCost { set; get; }

        public decimal CallsCountPercentage { get; set; }
        public decimal CallsCostPercentage { get; set; }
        public decimal CallsDurationPercentage { get; set; }

        public static List<GatewaysStatistics> GetGatewaysUsage(int year, int fromMonth, int toMonth)
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_GatewaySummary_ForAll_Sites);

            Dictionary<string, object> whereClause;
            List<object> functionParameters;

            GatewaysStatistics gatewayUsage;
            List<GatewaysStatistics> gatewaysUsageList = new List<GatewaysStatistics>();

            //Ge the gateways summaries from the database.
            functionParameters = new List<object>();
            whereClause = new Dictionary<string, object>
            {
                { Enums.GetDescription(Enums.GatewaysSummary.Year), year },
                { Enums.GetDescription(Enums.GatewaysSummary.Month), String.Format("BETWEEN '{0}' AND '{1}'", fromMonth, toMonth) }
            };

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionParameters, whereClause);


            foreach (DataRow row in dt.Rows)
            {
                gatewayUsage = new GatewaysStatistics();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysSummary.ToGateway) && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.GatewayName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysSummary.Year) && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.Year = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysSummary.Month) && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.Month = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysSummary.CallsCount) && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.CallsCount = Convert.ToInt64(row[column.ColumnName]);

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysSummary.CallsDuration) && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.CallsDuration = Convert.ToInt64((decimal)row[column.ColumnName]);

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysSummary.CallsCost) && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.CallsCost = (decimal)row[column.ColumnName];
                }

                gatewayUsage.Date =
                    new DateTime(
                        gatewayUsage.Year,
                        gatewayUsage.Month,
                        DateTime.DaysInMonth(gatewayUsage.Year, gatewayUsage.Month));
                gatewaysUsageList.Add(gatewayUsage);
            }
            return gatewaysUsageList;
        }

        public static List<GatewaysStatistics> GetGatewaysStatisticsResults(List<GatewaysStatistics> gatewaysUsage)
        {
            var gatewaysUsageData =
              (
                  from data in gatewaysUsage.AsEnumerable()

                  group data by new { data.GatewayName, data.Year } into res

                  select new GatewaysStatistics
                  {
                      GatewayName = res.Key.GatewayName,
                      Year = res.Key.Year,
                      CallsCount = res.Sum(x => x.CallsCount),
                      CallsDuration = res.Sum(x => x.CallsDuration),
                      CallsCost = res.Sum(x => x.CallsCost),
                  }
              ).Where(e => e.CallsCount > 200).ToList();

            return gatewaysUsageData;
        }

        public static List<GatewaysStatistics> SetGatewaysUsagePercentagesPerCallsCount(int year, int fromMonth, int toMonth)
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_GatewaySummary_ForAll_Sites);

            Dictionary<string, object> whereClause;
            List<object> functionParameters;

            GatewaysStatistics gatewayUsage;
            List<GatewaysStatistics> gatewaysUsageList = new List<GatewaysStatistics>();

            decimal totalOutGoingCallsCount = 0;
            decimal totalDurationCount = 0;
            decimal totalCostCount = 0;


            //Ge the gateways summaries from the database.
            functionParameters = new List<object>();
            whereClause = new Dictionary<string, object>
            {
                { Enums.GetDescription(Enums.GatewaysSummary.Year), year },
                { Enums.GetDescription(Enums.GatewaysSummary.Month), String.Format("BETWEEN '{0}' AND '{1}'", fromMonth, toMonth) }
            };

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionParameters, whereClause);


            foreach (DataRow row in dt.Rows)
            {
                gatewayUsage = new GatewaysStatistics();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysSummary.ToGateway) && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.GatewayName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysSummary.Year) && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.Year = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysSummary.Month) && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.Month = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysSummary.CallsCount) && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.CallsCount = Convert.ToInt64(row[column.ColumnName]);

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysSummary.CallsDuration) && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.CallsDuration = Convert.ToInt64((decimal)row[column.ColumnName]);

                    if (column.ColumnName == Enums.GetDescription(Enums.GatewaysSummary.CallsCost) && row[column.ColumnName] != DBNull.Value)
                        gatewayUsage.CallsCost = (decimal)row[column.ColumnName];
                }

                gatewayUsage.Date =
                    new DateTime(
                        gatewayUsage.Year,
                        gatewayUsage.Month,
                        DateTime.DaysInMonth(gatewayUsage.Year, gatewayUsage.Month));
                gatewaysUsageList.Add(gatewayUsage);
            }


            //Map all teh records for each gateway into a total-sum-one!
            var gatewaysUsageData = (
                from data in gatewaysUsageList.AsEnumerable()
                group data by new { data.GatewayName, data.Year } into res
                select new GatewaysStatistics
                {
                    GatewayName = res.Key.GatewayName,
                    Year = res.Key.Year,
                    CallsCount = res.Sum(x => x.CallsCount),
                    CallsDuration = res.Sum(x => x.CallsDuration),
                    CallsCost = res.Sum(x => x.CallsCost)
                }
            ).Where(e => e.CallsCount > 200).ToList();


            //Calculate Totals
            foreach (GatewaysStatistics tmpgatewayUsage in gatewaysUsageData)
            {
                totalCostCount += tmpgatewayUsage.CallsCost;
                totalOutGoingCallsCount += tmpgatewayUsage.CallsCount;
                totalDurationCount += tmpgatewayUsage.CallsDuration;
            }

            string resolvedGatewayAddress = string.Empty;


            //Calculate percentages
            foreach (GatewaysStatistics tmpgatewayUsage in gatewaysUsageData)
            {
                if (HelperFunctions.GetResolvedConnecionIPAddress(tmpgatewayUsage.GatewayName, out resolvedGatewayAddress) == true)
                    tmpgatewayUsage.GatewayName = resolvedGatewayAddress;

                if (tmpgatewayUsage.CallsCount.ToString() != null && tmpgatewayUsage.CallsCount > 0)
                    tmpgatewayUsage.CallsCountPercentage = Math.Round((tmpgatewayUsage.CallsCount * 100 / totalOutGoingCallsCount), 2);
                else
                    tmpgatewayUsage.CallsCountPercentage = 0;

                if (tmpgatewayUsage.CallsCost.ToString() != null && tmpgatewayUsage.CallsCost > 0)
                    tmpgatewayUsage.CallsCostPercentage = Math.Round((tmpgatewayUsage.CallsCost * 100) / totalCostCount, 2);
                else
                    tmpgatewayUsage.CallsCostPercentage = 0;

                if (tmpgatewayUsage.CallsDuration.ToString() != null && tmpgatewayUsage.CallsDuration > 0)
                    tmpgatewayUsage.CallsDurationPercentage = Math.Round((tmpgatewayUsage.CallsDuration * 100 / totalDurationCount), 2);
                else
                    tmpgatewayUsage.CallsDurationPercentage = 0;
            }

            return gatewaysUsageData;

        }

        public static List<GatewaysStatistics> SetGatewaysUsagePercentagesPerCallsCount(List<GatewaysStatistics> gatewaysUsage)
        {
            decimal totalOutGoingCallsCount = 0;
            decimal totalDurationCount = 0;
            decimal totalCostCount = 0;

            var gatewaysUsageData = (
                from data in gatewaysUsage.AsEnumerable()
                group data by new { data.GatewayName, data.Year } into res
                select new GatewaysStatistics
                {
                    GatewayName = res.Key.GatewayName,
                    Year = res.Key.Year,
                    CallsCount = res.Sum(x => x.CallsCount),
                    CallsDuration = res.Sum(x => x.CallsDuration),
                    CallsCost = res.Sum(x => x.CallsCost)
                }
            ).Where(e => e.CallsCount > 200).ToList();


            //Calculate totals
            foreach (GatewaysStatistics gatewayUsage in gatewaysUsageData)
            {
                totalCostCount += gatewayUsage.CallsCost;
                totalOutGoingCallsCount += gatewayUsage.CallsCount;
                totalDurationCount += gatewayUsage.CallsDuration;
            }

            string resolvedGatewayAddress = string.Empty;


            //Calcualte percentages
            foreach (GatewaysStatistics gatewayUsage in gatewaysUsageData)
            {
                if (HelperFunctions.GetResolvedConnecionIPAddress(gatewayUsage.GatewayName, out resolvedGatewayAddress) == true)
                    gatewayUsage.GatewayName = resolvedGatewayAddress;

                if (gatewayUsage.CallsCount.ToString() != null && gatewayUsage.CallsCount > 0)
                    gatewayUsage.CallsCountPercentage = Math.Round((gatewayUsage.CallsCount * 100 / totalOutGoingCallsCount), 2);
                else
                    gatewayUsage.CallsCountPercentage = 0;

                if (gatewayUsage.CallsCost.ToString() != null && gatewayUsage.CallsCost > 0)
                    gatewayUsage.CallsCostPercentage = Math.Round((gatewayUsage.CallsCost * 100) / totalCostCount, 2);
                else
                    gatewayUsage.CallsCostPercentage = 0;

                if (gatewayUsage.CallsDuration.ToString() != null && gatewayUsage.CallsDuration > 0)
                    gatewayUsage.CallsDurationPercentage = Math.Round((gatewayUsage.CallsDuration * 100 / totalDurationCount), 2);
                else
                    gatewayUsage.CallsDurationPercentage = 0;
            }

            return gatewaysUsageData;
        }

        public static List<SpecialDateTime> GetYears()
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_GatewaySummary_ForAll_Sites);

            List<object> functionParameters;
            List<string> columnsList;
            Dictionary<string, object> whereClause;

            SpecialDateTime Year;
            List<SpecialDateTime> YearsList = new List<SpecialDateTime>();

            //Ge the gateways summaries from the database.
            functionParameters = new List<object>();
            whereClause = new Dictionary<string, object>();
            columnsList = new List<string>() { String.Format("DISTINCT {0}", Enums.GetDescription(Enums.GatewaysSummary.Year)) };

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionParameters, whereClause, selectColumnsList: columnsList);

            foreach (DataRow row in dt.Rows)
            {
                Year = new SpecialDateTime();

                if (row[Enums.GetDescription(Enums.GatewaysSummary.Year)] != DBNull.Value)
                {
                    Year.YearAsNumber = Convert.ToInt32(row[Enums.GetDescription(Enums.GatewaysSummary.Year)]);
                    Year.YearAsText = Year.YearAsText.ToString();
                    YearsList.Add(Year);
                }
            }
            return YearsList;
        }

    }

}