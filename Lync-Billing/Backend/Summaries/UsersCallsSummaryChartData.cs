using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Lync_Billing.Libs;
using Lync_Billing.ConfigurationSections;
using System.Configuration;

namespace Lync_Billing.Backend.Summaries
{
    public class UsersCallsSummaryChartData
    {
        private static DBLib DBRoutines = new DBLib();

        public string Name { get; set; }
        public int TotalCalls { get; set; }
        public int TotalDuration { get; set; }
        public int TotalCost { get; set; }


        private static List<UsersCallsSummaryChartData> AddOrUpdateListOfSummaries(List<UsersCallsSummaryChartData> chartsSummariesList, UsersCallsSummaryChartData newSummary)
        {
            //If there is already a summary with the same name in the list, then just add it's values to this currently computed summary (userSummary)
            //This happens due to multiple phonecalls tables
            var existingSummary = chartsSummariesList.SingleOrDefault(summary => summary.Name == newSummary.Name);
            if (existingSummary != null)
            {
                //Get the existing summary's index
                int summaryIndex = chartsSummariesList.IndexOf(existingSummary);

                //Compute an updated summary
                newSummary.TotalCalls += existingSummary.TotalCalls;
                newSummary.TotalCost += existingSummary.TotalCost;
                newSummary.TotalDuration += existingSummary.TotalDuration;

                //Replace the old summary with the newly updated version of it.
                chartsSummariesList[summaryIndex] = newSummary;
            }
            else
            {
                chartsSummariesList.Add(newSummary);
            }

            return chartsSummariesList;
        }


        public static List<UsersCallsSummaryChartData> GetUsersCallsSummary(string sipAccount, int Year, int FromMonth, int ToMonth)
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_CallsSummary_ForUser);

            string columnName = string.Empty;
            List<object> functionParams = new List<object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            
            UsersCallsSummaryChartData userSummary;
            List<UsersCallsSummaryChartData> chartList = new List<UsersCallsSummaryChartData>();

            //Specify the sipaccount for the database function
            wherePart.Add(Enums.GetDescription(Enums.PhoneCallSummary.Year), Year);
            wherePart.Add(Enums.GetDescription(Enums.PhoneCallSummary.Month), String.Format("BETWEEN '{0}' AND '{1}'", FromMonth, ToMonth));
            functionParams.Add(sipAccount);

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionParams, wherePart);


            foreach (DataRow row in dt.Rows)
            {
                //Start processing the BUSINESS CALLS Summary First
                userSummary = new UsersCallsSummaryChartData();
                userSummary.Name = "Business";

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount);
                userSummary.TotalCalls = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration);
                userSummary.TotalDuration = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost);
                userSummary.TotalCost = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                //Add or Update this summary in the local chartList variable
                chartList = AddOrUpdateListOfSummaries(chartList, userSummary);


                //Process the PERSONAL CALLS Summary
                userSummary = new UsersCallsSummaryChartData();
                userSummary.Name = "Personal";

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount);
                userSummary.TotalCalls = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration);
                userSummary.TotalDuration = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost);
                userSummary.TotalCost = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                //Add or Update this summary in the local chartList variable
                chartList = AddOrUpdateListOfSummaries(chartList, userSummary);


                //Process the UNMARKED CALLS Summary
                userSummary = new UsersCallsSummaryChartData();
                userSummary.Name = "Unmarked";

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount);
                userSummary.TotalCalls = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration);
                userSummary.TotalDuration = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost);
                userSummary.TotalCost = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                //Add or Update this summary in the local chartList variable
                chartList = AddOrUpdateListOfSummaries(chartList, userSummary);
            }

            
            //This handles the case in which the date is at the beginning of the year, and no data was recorded yet.
            if (dt.Rows.Count == 0)
            {
                var businessSummary = new UsersCallsSummaryChartData();
                businessSummary.Name = "Business";

                var personalSummary = new UsersCallsSummaryChartData();
                personalSummary.Name = "Personal";

                var unmarkedSummary = new UsersCallsSummaryChartData();
                unmarkedSummary.Name = "Unmarked";

                chartList.Add(businessSummary);
                chartList.Add(personalSummary);
                chartList.Add(unmarkedSummary);
            }


            //Return the the summaries that have a total of phone calls greater than 0
            return chartList.Where(summary => summary.TotalCalls > 0).ToList<UsersCallsSummaryChartData>();
        }


        public static List<UsersCallsSummaryChartData> GetUsersCallsSummary(string sipAccount, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_CallsSummary_ForUser);

            string columnName = string.Empty;
            List<object> functionParams = new List<object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            UsersCallsSummaryChartData userSummary;
            List<UsersCallsSummaryChartData> chartList = new List<UsersCallsSummaryChartData>();

            //Specify the sipaccount for the database function
            wherePart.Add(Enums.GetDescription(Enums.PhoneCallSummary.Date), String.Format("BETWEEN '{0}' AND '{1}'", fromDate, toDate));
            functionParams.Add(sipAccount);

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionParams, wherePart);


            foreach (DataRow row in dt.Rows)
            {
                //Start processing the BUSINESS CALLS Summary First
                userSummary = new UsersCallsSummaryChartData();
                userSummary.Name = "Business";

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount);
                userSummary.TotalCalls = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration);
                userSummary.TotalDuration = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost);
                userSummary.TotalCost = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                //Add or Update this summary in the local chartList variable
                chartList = AddOrUpdateListOfSummaries(chartList, userSummary);


                //Process the PERSONAL CALLS Summary
                userSummary = new UsersCallsSummaryChartData();
                userSummary.Name = "Personal";

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount);
                userSummary.TotalCalls = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration);
                userSummary.TotalDuration = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost);
                userSummary.TotalCost = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                //Add or Update this summary in the local chartList variable
                chartList = AddOrUpdateListOfSummaries(chartList, userSummary);


                //Process the UNMARKED CALLS Summary
                userSummary = new UsersCallsSummaryChartData();
                userSummary.Name = "Unmarked";

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount);
                userSummary.TotalCalls = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration);
                userSummary.TotalDuration = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost);
                userSummary.TotalCost = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                //Add or Update this summary in the local chartList variable
                chartList = AddOrUpdateListOfSummaries(chartList, userSummary);
            }


            //This handles the case in which the date is at the beginning of the year, and no data was recorded yet.
            if (dt.Rows.Count == 0)
            {
                var businessSummary = new UsersCallsSummaryChartData();
                businessSummary.Name = "Business";

                var personalSummary = new UsersCallsSummaryChartData();
                personalSummary.Name = "Personal";

                var unmarkedSummary = new UsersCallsSummaryChartData();
                unmarkedSummary.Name = "Unmarked";

                chartList.Add(businessSummary);
                chartList.Add(personalSummary);
                chartList.Add(unmarkedSummary);
            }


            //Return the the summaries that have a total of phone calls greater than 0
            return chartList.Where(summary => summary.TotalCalls > 0).ToList<UsersCallsSummaryChartData>();
        }

    }

}