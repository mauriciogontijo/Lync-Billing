using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Lync_Billing.Libs;
using Lync_Billing.ConfigurationSections;
using System.Configuration;

namespace Lync_Billing.DB.Summaries
{
    public class UsersCallsSummaryChartData
    {
        private static DBLib DBRoutines = new DBLib();
        private static Statistics StatsRoutines = new Statistics();

        private static Dictionary<string, object> wherePart;

        public string Name { get; set; }
        public int TotalCalls { get; set; }
        public int TotalDuration { get; set; }
        public int TotalCost { get; set; }


        public static List<UsersCallsSummaryChartData> GetUsersCallsSummary(string sipAccount, DateTime startingDate, DateTime endingDate)
        {
            string columnName = string.Empty;
            wherePart = new Dictionary<string, object>();
            List<object> functionParams = new List<object>();
            int summaryYear, summaryMonth;

            DataTable dt = new DataTable();
            UsersCallsSummaryChartData userSummary;
            List<UsersCallsSummaryChartData> chartList = new List<UsersCallsSummaryChartData>();

            //Specify the sipaccount for the database function
            functionParams.Add(sipAccount);

            //dt = DBRoutines.SELECT_USER_STATISTICS(Enums.GetDescription(Enums.PhoneCalls.TableName), wherePart);
            dt = DBRoutines.SELECT_FROM_FUNCTION("Get_CallsSummary_ForUser", functionParams, wherePart);

            foreach (DataRow row in dt.Rows)
            {
                summaryYear = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Year)]]));
                summaryMonth = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Month)]]));

                //Skip this summary-row if it's out of the range of the given date periods
                if ((summaryYear < startingDate.Year && summaryMonth < startingDate.Month) ||
                    (summaryYear > endingDate.Year && summaryMonth > endingDate.Month))
                    continue;

                //Start processing the BUSINESS CALLS Summary First
                userSummary = new UsersCallsSummaryChartData();
                userSummary.Name = "Business";

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount);
                userSummary.TotalCalls = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration);
                userSummary.TotalDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost);
                userSummary.TotalCost = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                chartList = AddOrUpdateListOfSummaries(chartList, userSummary);


                //Process the PERSONAL CALLS Summary
                userSummary = new UsersCallsSummaryChartData();
                userSummary.Name = "Personal";

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount);
                userSummary.TotalCalls = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration);
                userSummary.TotalDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost);
                userSummary.TotalCost = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                chartList = AddOrUpdateListOfSummaries(chartList, userSummary);


                //Process the UNMARKED CALLS Summary
                userSummary = new UsersCallsSummaryChartData();
                userSummary.Name = "Unmarked";

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount);
                userSummary.TotalCalls = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration);
                userSummary.TotalDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost);
                userSummary.TotalCost = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                chartList = AddOrUpdateListOfSummaries(chartList, userSummary);
            }

            //Return the the summaries that have a total of phone calls greater than 0
            return chartList.Where(summary => summary.TotalCalls > 0).ToList<UsersCallsSummaryChartData>();
        }


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

    }

}