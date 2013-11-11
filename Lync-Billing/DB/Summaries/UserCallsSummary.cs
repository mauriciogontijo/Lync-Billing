using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Lync_Billing.Libs;
using Lync_Billing.ConfigurationSections;
using System.Configuration;

namespace Lync_Billing.DB.Summaries
{
    public class UserCallsSummary
    {
        private static DBLib DBRoutines = new DBLib();
        private static Statistics StatsRoutines = new Statistics();

        public string EmployeeID { get; set; }
        public string FullName { get; set; }
        public string SipAccount { get; set; }
        public string SiteName { get; set; }

        public int BusinessCallsCount { get; set; }
        public decimal BusinessCallsCost { get; set; }
        public int BusinessCallsDuration { get; set; }
        public int PersonalCallsCount { get; set; }
        public int PersonalCallsDuration { get; set; }
        public decimal PersonalCallsCost { get; set; }
        public int UnmarkedCallsCount { get; set; }
        public int UnmarkedCallsDuration { get; set; }
        public decimal UnmarkedCallsCost { get; set; }
        public int NumberOfDisputedCalls { get; set; }

        public DateTime Date { set; get; }
        public decimal Duration { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }


        /// <summary>
        /// Given a SipAccount address, a year's number, a starting month's number, and an ending month's number return sum object of all the phone calls summaries between the months.
        /// </summary>
        /// <param name="siteName">The user's SipAccount address</param>
        /// <param name="year">The year of the phone calls summary</param>
        /// <param name="fromMonth">The starting month of the phone calls summary</param>
        /// <param name="endMonth">The ending month of the phone calls summary</param>
        /// <returns>A UserCallsSummary sum object of all summaries between the months</returns>
        public static UserCallsSummary GetUserCallsSummary(string sipAccount, int Year, int fromMonth, int toMonth)
        {
            List<object> functionParams = new List<object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            DataTable dt = new DataTable();
            UserCallsSummary userSummary = new UserCallsSummary();
            int summaryYear, summaryMonth;

            functionParams.Add(sipAccount);

            dt = DBRoutines.SELECT_FROM_FUNCTION("Get_CallsSummary_ForUser", functionParams, wherePart);

            foreach (DataRow row in dt.Rows)
            {
                summaryYear = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Year)]]));
                summaryMonth = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Month)]]));

                //Skip this summary-row if it's out of the range of the given date periods
                if (summaryYear != Year || summaryMonth < fromMonth || summaryMonth > toMonth)
                    continue;

                //Start processing the summary
                userSummary = new UserCallsSummary();

                userSummary.Year = summaryYear;
                userSummary.Month = summaryMonth;
                userSummary.Date = new DateTime(summaryYear, summaryMonth, DateTime.DaysInMonth(summaryYear, summaryMonth));

                userSummary.BusinessCallsDuration += Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration)]]));
                userSummary.BusinessCallsCount += Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount)]]));
                userSummary.BusinessCallsCost += Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost)]]));
                userSummary.PersonalCallsDuration += Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration)]]));
                userSummary.PersonalCallsCount += Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount)]]));
                userSummary.PersonalCallsCost += Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost)]]));
                userSummary.UnmarkedCallsDuration += Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration)]]));
                userSummary.UnmarkedCallsCount += Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount)]]));
                userSummary.UnmarkedCallsCost += Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost)]]));

                userSummary.Duration += (userSummary.PersonalCallsDuration / 60);
            }

            //Get the remaining user information
            Users userInfo = Users.GetUser(sipAccount);

            userSummary.EmployeeID = userInfo.EmployeeID.ToString();
            userSummary.SipAccount = userInfo.SipAccount;
            userSummary.FullName = userInfo.FullName;
            userSummary.SiteName = userInfo.SiteName;

            return userSummary;
        }


        /// <summary>
        /// Given a SipAccount address, a starting date, and an ending date return a UserCallsSummary sum object of all the phone calls summaries between the dates.
        /// </summary>
        /// <param name="siteName">The user's SipAccount address</param>
        /// <param name="year">The year of the phone calls summary</param>
        /// <param name="startingDate">The starting date of phone calls summary</param>
        /// <param name="endingDate">The ending date of the phone calls summary</param>
        /// <returns>A UserCallsSummary sum object of all summaries between the dates</returns>
        public static UserCallsSummary GetUserCallsSummary(string sipAccount, DateTime startDate, DateTime endDate)
        {
            List<object> functionParams = new List<object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            DataTable dt = new DataTable();
            UserCallsSummary userSummary = new UserCallsSummary();
            int summaryYear, summaryMonth;

            functionParams.Add(sipAccount);

            dt = DBRoutines.SELECT_FROM_FUNCTION("Get_CallsSummary_ForUser", functionParams, wherePart);

            foreach (DataRow row in dt.Rows)
            {
                summaryYear = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Year)]]));
                summaryMonth = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Month)]]));

                //Skip this summary-row if it's out of the range of the given date periods
                if ((summaryYear < startDate.Year && summaryMonth < startDate.Month) ||
                    (summaryYear > endDate.Year && summaryMonth > endDate.Month))
                    continue;

                //Start processing the summary
                userSummary = new UserCallsSummary();

                userSummary.Year = summaryYear;
                userSummary.Month = summaryMonth;
                userSummary.Date = new DateTime(summaryYear, summaryMonth, DateTime.DaysInMonth(summaryYear, summaryMonth));

                userSummary.BusinessCallsDuration += Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration)]]));
                userSummary.BusinessCallsCount += Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount)]]));
                userSummary.BusinessCallsCost += Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost)]]));
                userSummary.PersonalCallsDuration += Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration)]]));
                userSummary.PersonalCallsCount += Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount)]]));
                userSummary.PersonalCallsCost += Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost)]]));
                userSummary.UnmarkedCallsDuration += Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration)]]));
                userSummary.UnmarkedCallsCount += Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount)]]));
                userSummary.UnmarkedCallsCost += Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost)]]));

                userSummary.Duration += (userSummary.PersonalCallsDuration / 60);
            }

            //Get the remaining user information
            Users userInfo = Users.GetUser(sipAccount);

            userSummary.EmployeeID = userInfo.EmployeeID.ToString();
            userSummary.SipAccount = userInfo.SipAccount;
            userSummary.FullName = userInfo.FullName;
            userSummary.SiteName = userInfo.SiteName;

            return userSummary;
        }


        /// <summary>
        /// Given a SipAccount address, a year's number, a starting month's number, and an ending month's number return the list of this user's summaries with respect to months.
        /// </summary>
        /// <param name="siteName">The user's SipAccount address</param>
        /// <param name="year">The year of the phone calls summary</param>
        /// <param name="fromMonth">The starting month of the phone calls summary</param>
        /// <param name="endMonth">The ending month of the phone calls summary</param>
        /// <returns>A list of UserCallsSummary for that user</returns>
        public static List<UserCallsSummary> GetUsersCallsSummary(string sipAccount, int year, int fromMonth, int toMonth)
        {
            DataTable dt = new DataTable();
            UserCallsSummary userSummary;
            List<UserCallsSummary> chartList = new List<UserCallsSummary>();

            //Get the remaining user information
            Users userInfo = Users.GetUser(sipAccount);

            List<object> functionParams = new List<object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            int summaryYear, summaryMonth;

            functionParams.Add(sipAccount);

            dt = DBRoutines.SELECT_FROM_FUNCTION("Get_CallsSummary_ForUser", functionParams, wherePart);

            foreach (DataRow row in dt.Rows)
            {
                summaryYear = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Year)]]));
                summaryMonth = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Month)]]));

                //Skip this summary-row if it's out of the range of the given date periods
                if (summaryYear != year || summaryMonth < fromMonth || summaryMonth > toMonth)
                    continue;

                //Start processing the summary
                userSummary = new UserCallsSummary();

                userSummary.Year = summaryYear;
                userSummary.Month = summaryMonth;
                userSummary.Date = new DateTime(summaryYear, summaryMonth, DateTime.DaysInMonth(summaryYear, summaryMonth));

                userSummary.EmployeeID = userInfo.EmployeeID.ToString();
                userSummary.SipAccount = userInfo.SipAccount;
                userSummary.FullName = userInfo.FullName;
                userSummary.SiteName = userInfo.SiteName;

                userSummary.BusinessCallsDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration)]]));
                userSummary.BusinessCallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount)]]));
                userSummary.BusinessCallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost)]]));
                userSummary.PersonalCallsDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration)]]));
                userSummary.PersonalCallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount)]]));
                userSummary.PersonalCallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost)]]));
                userSummary.UnmarkedCallsDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration)]]));
                userSummary.UnmarkedCallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount)]]));
                userSummary.UnmarkedCallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost)]]));

                userSummary.Duration = (userSummary.PersonalCallsDuration / 60);


                //Add it to the list.
                chartList.Add(userSummary);
            }

            return chartList;
        }


        /// <summary>
        /// Given a Site's name, a starting date, and an end date, return the list of all users' summaries who belong to that Site
        /// </summary>
        /// <param name="siteName">The Site's name</param>
        /// <param name="startingDate">The starting date of phone calls summary</param>
        /// <param name="endingDate">The ending date of the phone calls summary</param>
        /// <returns>A list of UserCallsSummary</returns>
        public static List<UserCallsSummary> GetUsersCallsSummary(string siteName, DateTime startingDate, DateTime endingDate)
        {
            DataTable dt = new DataTable();
            UserCallsSummary userSummary;
            List<UserCallsSummary> usersSummaryList = new List<UserCallsSummary>();

            List<object> functionParams = new List<object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            int summaryYear, summaryMonth;

            //Add the siteName to the functionParams
            functionParams.Add(siteName);

            dt = DBRoutines.SELECT_FROM_FUNCTION("Get_CallsSummary_ForUsers_PerSite", functionParams, wherePart);

            foreach (DataRow row in dt.Rows)
            {
                summaryYear = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Year)]]));
                summaryMonth = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Month)]]));

                //Skip this summary-row if it's out of the range of the given date periods
                if (summaryYear < startingDate.Year || summaryYear > endingDate.Year || summaryMonth < startingDate.Month || summaryMonth > endingDate.Month)
                    continue;

                userSummary = new UserCallsSummary();

                userSummary.SipAccount = Convert.ToString(Misc.ReturnEmptyIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.SipAccount)]]));
                userSummary.FullName = Convert.ToString(Misc.ReturnEmptyIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.DisplayName)]]));
                userSummary.EmployeeID = Convert.ToString(Misc.ReturnEmptyIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.EmployeeID)]]));
                userSummary.SiteName = Convert.ToString(Misc.ReturnEmptyIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.SiteName)]]));

                userSummary.Duration = (userSummary.PersonalCallsDuration / 60);

                userSummary.BusinessCallsDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration)]]));
                userSummary.BusinessCallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount)]]));
                userSummary.BusinessCallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost)]]));
                userSummary.PersonalCallsDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration)]]));
                userSummary.PersonalCallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount)]]));
                userSummary.PersonalCallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost)]]));
                userSummary.UnmarkedCallsDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration)]]));
                userSummary.UnmarkedCallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount)]]));
                userSummary.UnmarkedCallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost)]]));

                usersSummaryList.Add(userSummary);
            }

            return usersSummaryList;
        }


        /// <summary>
        /// Given a Site's name, a list of specific users sipaccounts addresses, a starting date, and an end date, return the list of all of these users summaries - who belong to that same Site.
        /// </summary>
        /// <param name="siteName">The Site's name</param>
        /// <param name="sipAccountsList">The list of users' SipAccount addresses</param>
        /// <param name="startingDate">The starting date of phone calls summary</param>
        /// <param name="endingDate">The ending date of the phone calls summary</param>
        /// <returns>A list of UserCallsSummary</returns>
        public static Dictionary<string, UserCallsSummary> GetUsersCallsSummary(string siteName, List<string> sipAccountsList, DateTime startingDate, DateTime endingDate)
        {
            DataTable dt = new DataTable();
            string columnName = string.Empty;

            List<UserCallsSummary> ListOfUsersSummaries = GetUsersCallsSummary(siteName, startingDate, endingDate);

            Dictionary<string, UserCallsSummary> usersSummaryList = ListOfUsersSummaries
                .Where(summary => sipAccountsList.Contains(summary.SipAccount))
                .ToDictionary(summary => summary.SipAccount);

            return usersSummaryList;
        }


        //TO DO: REFACTOR USING NEW DATABASE FUNCTIONS
        public static void ExportUsersCallsSummaryToPDF(DateTime startingDate, DateTime endingDate, string siteName, Dictionary<string, Dictionary<string, object>> UsersCollection, HttpResponse response, out Document document, Dictionary<string, string> headers)
        {
            //THE PDF REPORT PROPERTIES
            PDFReportsPropertiesSection section = ((PDFReportsPropertiesSection)ConfigurationManager.GetSection(PDFReportsPropertiesSection.ConfigurationSectionName));
            PDFReportPropertiesElement pdfReportProperties = section.GetReportProperties("AccountingSummary");

            //Database related
            DataTable dt = new DataTable();
            Dictionary<string, object> totals;
            List<string> columnsList = new List<string>();

            //These two are passed to the PdfLib
            int[] pdfColumnsWidths = new int[] { };
            List<string> pdfColumnsSchema = new List<string>();

            if (pdfReportProperties != null)
            {
                pdfColumnsWidths = pdfReportProperties.ColumnsWidths();
                pdfColumnsSchema = pdfReportProperties.ColumnsNames();
            }

            headers.Add("comments", "* Please note that the columns headers below - Business, Personal and Unallocated - refer to the Costs of the phonecalls based on their type.");


            //Get the report content from the database
            dt = StatsRoutines.DISTINCT_USERS_STATS(startingDate, endingDate, siteName, UsersCollection.Keys.ToList(), columnsList);


            //Try to compute totals, if an error occurs which is the case of an empty "dt", set the totals dictionary to zeros
            try
            {
                totals = new Dictionary<string, object>()
                {
                    {"PersonalCost", Decimal.Round(Convert.ToDecimal(Misc.ReturnZeroIfNull(dt.Compute("Sum(PersonalCost)", "PersonalCost > 0"))), 2)},
                    {"BusinessCost", Decimal.Round(Convert.ToDecimal(Misc.ReturnZeroIfNull(dt.Compute("Sum(BusinessCost)", "BusinessCost > 0"))), 2)},
                    {"UnMarkedCost", Decimal.Round(Convert.ToDecimal(Misc.ReturnZeroIfNull(dt.Compute("Sum(UnMarkedCost)", "UnMarkedCost > 0"))), 2)}
                };
            }
            catch (Exception e)
            {
                totals = new Dictionary<string, object>()
                {
                    {"PersonalCost", 0.00},
                    {"BusinessCost", 0.00},
                    {"UnMarkedCost", 0.00}
                };
            }

            document = PDFLib.CreateAccountingSummaryReport(response, dt, totals, headers, pdfColumnsSchema, pdfColumnsWidths);
        }


        //TO DO: REFACTOR USING NEW DATABASE FUNCTIONS
        public static void ExportUsersCallsDetailedToPDF(DateTime startingDate, DateTime endingDate, string siteName, Dictionary<string, Dictionary<string, object>> UsersCollection, HttpResponse response, out Document document, Dictionary<string, string> headers)
        {
            //THE PDF REPORT PROPERTIES
            PDFReportsPropertiesSection section = ((PDFReportsPropertiesSection)ConfigurationManager.GetSection(PDFReportsPropertiesSection.ConfigurationSectionName));
            PDFReportPropertiesElement pdfReportProperties = section.GetReportProperties("AccountingDetailed");

            //Database query related
            DataTable dt;
            List<string> columns;

            //This is passed to the PdfLib
            List<string> pdfColumnsSchema = new List<string>();
            int[] pdfColumnsWidths = new int[] { };

            if (pdfReportProperties != null)
            {
                pdfColumnsWidths = pdfReportProperties.ColumnsWidths();
                pdfColumnsSchema = pdfReportProperties.ColumnsNames();
            }

            //Database query related
            columns = new List<string>()
            { 
                "SourceUserUri", 
                "ResponseTime", 
                "marker_CallToCountry", 
                "DestinationNumberUri", 
                "Duration", 
                "marker_CallCost", 
                "ui_CallType"
            };

            //The PDF report body contents
            dt = StatsRoutines.DISTINCT_USERS_STATS_DETAILED(startingDate, endingDate, UsersCollection.Keys.ToList(), columns);

            //Get the collection of users' summaries.
            Dictionary<string, UserCallsSummary> UsersSummaires = UserCallsSummary.GetUsersCallsSummary(siteName, UsersCollection.Keys.ToList(), startingDate, endingDate);

            //Get a closed instance of the document which contains all the formatted data
            document = PDFLib.CreateAccountingDetailedReport(response, dt, pdfColumnsWidths, pdfColumnsSchema, headers, "SourceUserUri", UsersCollection, UsersSummaires);
        }

    }

}