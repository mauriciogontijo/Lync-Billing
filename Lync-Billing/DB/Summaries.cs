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

namespace Lync_Billing.DB
{
    public class UsersCallsSummaryChartData 
    {
        private static DBLib DBRoutines = new DBLib();
        private static Statistics StatsRoutines = new Statistics();

        private static Dictionary<string, object> wherePart;

        public string Name { get;set;}
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

    public class UserCallsSummary
    {
        private static DBLib DBRoutines = new DBLib();
        private static Statistics StatRoutines = new Statistics();

        //private static Dictionary<string, object> wherePart;
        private static List<string> columns;

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

        public DateTime MonthDate { set; get; }
        public decimal Duration { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }


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
                userSummary.MonthDate = new DateTime(summaryYear, summaryMonth, DateTime.DaysInMonth(summaryYear, summaryMonth));

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

            return userSummary;
        }


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
                userSummary.MonthDate = new DateTime(summaryYear, summaryMonth, DateTime.DaysInMonth(summaryYear, summaryMonth));

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

            return userSummary;
        }


        public static List<UserCallsSummary> GetUsersCallsSummary(string sipAccount, int Year, int fromMonth, int toMonth)
        {
            DataTable dt = new DataTable();
            UserCallsSummary userSummary;
            List<UserCallsSummary> chartList = new List<UserCallsSummary>();

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
                if (summaryYear != Year || summaryMonth < fromMonth || summaryMonth > toMonth)
                    continue;

                //Start processing the summary
                userSummary = new UserCallsSummary();

                userSummary.Year = summaryYear;
                userSummary.Month = summaryMonth;
                userSummary.MonthDate = new DateTime(summaryYear, summaryMonth, DateTime.DaysInMonth(summaryYear, summaryMonth));

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


        //IN PROGRESS: REFACTOR USING NEW DATABASE FUNCTIONS
        public static List<UserCallsSummary> GetUsersCallsSummary(DateTime startingDate, DateTime endingDate,string siteName) 
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

                userSummary.BusinessCallsDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration)]]));
                userSummary.BusinessCallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount)]]));
                userSummary.BusinessCallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost)]]));
                userSummary.PersonalCallsDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration)]]));
                userSummary.PersonalCallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount)]]));
                userSummary.PersonalCallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost)]]));
                userSummary.UnmarkedCallsDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration)]]));
                userSummary.UnmarkedCallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount)]]));
                userSummary.UnmarkedCallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost)]]));
                userSummary.SipAccount = Convert.ToString(Misc.ReturnEmptyIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.SipAccount)]]));
                userSummary.FullName = Convert.ToString(Misc.ReturnEmptyIfNull(row[dt.Columns[Enums.GetDescription(Enums.Users.AD_DisplayName)]]));
                userSummary.EmployeeID = Convert.ToString(Misc.ReturnEmptyIfNull(row[dt.Columns[Enums.GetDescription(Enums.Users.AD_UserID)]]));
                userSummary.SiteName = Convert.ToString(Misc.ReturnEmptyIfNull(row[dt.Columns[Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName)]]));

                userSummary.Duration = (userSummary.PersonalCallsDuration / 60);

                usersSummaryList.Add(userSummary);
            }
            return usersSummaryList;
        }


        //IN PROGRESS: REFACTOR USING NEW DATABASE FUNCTIONS
        public static Dictionary<string, UserCallsSummary> GetUsersCallsSummary(List<string> sipAccountsList, DateTime startingDate, DateTime endingDate, string siteName)
        {
            DataTable dt = new DataTable();
            string columnName = string.Empty;

            Users user;
            UserCallsSummary userSummary;

            Dictionary<string, UserCallsSummary> usersSummaryList = new Dictionary<string, UserCallsSummary>();
            
            foreach (string sipAccount in sipAccountsList)
            {
                user = Users.GetUser(sipAccount);
                userSummary = GetUserCallsSummary(sipAccount, startingDate, endingDate);
                
                userSummary.SipAccount = sipAccount;
                userSummary.FullName = user.FullName;
                userSummary.EmployeeID = user.EmployeeID.ToString();
                userSummary.SiteName = user.SiteName;
                userSummary.Duration = (userSummary.PersonalCallsDuration / 60);

                //Add it to the list
                usersSummaryList.Add(userSummary.SipAccount, userSummary);
            }

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
            dt = StatRoutines.DISTINCT_USERS_STATS(startingDate, endingDate, siteName, UsersCollection.Keys.ToList(), columns);

            
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
            dt = StatRoutines.DISTINCT_USERS_STATS_DETAILED(startingDate, endingDate, UsersCollection.Keys.ToList(), columns);

            //Get the collection of users' summaries.
            Dictionary<string, UserCallsSummary> UsersSummaires = UserCallsSummary.GetUsersCallsSummary(UsersCollection.Keys.ToList(), startingDate, endingDate, siteName);

            //Get a closed instance of the document which contains all the formatted data
            document = PDFLib.CreateAccountingDetailedReport(response, dt, pdfColumnsWidths, pdfColumnsSchema, headers, "SourceUserUri", UsersCollection, UsersSummaires);
        }
        
    }

    public class DepartmentCallsSummary
    {
        public string SiteName { get; set; }
        public string DepartmentName { get; set; }

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

        public DateTime MonthDate { set; get; }
        public int Year { get; set; }
        public int Month { get; set; }

        private static DBLib DBRoutines = new DBLib();


        //TO DO: REFACTOR USING NEW DATABASE FUNCTIONS
        public static List<DepartmentCallsSummary> GetPhoneCallsStatisticsForDepartment(string departmentName, string siteName, int year)
        {
            DataTable dt = new DataTable();
            string columnName = string.Empty;
            DepartmentCallsSummary departmentSummary;
            List<DepartmentCallsSummary> ListOfDepartmentCallsSummaries = new List<DepartmentCallsSummary>();

            List<object> functionParameters = new List<object>();
            functionParameters.Add(departmentName);
            functionParameters.Add(siteName);

            dt = DBRoutines.SELECT_FROM_FUNCTION("fnc_Chargable_Calls_By_Site_Department", functionParameters, null);

            foreach (DataRow row in dt.Rows)
            {
                departmentSummary = new DepartmentCallsSummary();

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.BusinessCallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.BusinessCallsDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.BusinessCallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));


                columnName = Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.PersonalCallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.PersonalCallsDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.PersonalCallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));


                columnName = Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.UnmarkedCallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.UnmarkedCallsDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.UnmarkedCallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));


                columnName = Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.SiteName = Convert.ToString(Misc.ReturnEmptyIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.Month);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.Month = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.Year);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.Year = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.MonthDate);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.MonthDate = Convert.ToDateTime(row[dt.Columns[columnName]]);

                ListOfDepartmentCallsSummaries.Add(departmentSummary);
            }

            return ListOfDepartmentCallsSummaries.Where(summary => summary.Year == year).ToList<DepartmentCallsSummary>();
        }
    }
}