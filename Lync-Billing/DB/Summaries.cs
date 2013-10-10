using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Lync_Billing.Libs;

namespace Lync_Billing.DB
{
    public class UsersCallsSummaryChartData 
    {
        private static DBLib DBRoutines = new DBLib();
        

        private static Dictionary<string, object> wherePart;
        private static List<string> columns;

        public string Name { get;set;}
        public int TotalCalls { get; set; }
        public int TotalDuration { get; set; }
        public int TotalCost { get; set; }
       
        public static List<UsersCallsSummaryChartData> GetUsersCallsSummary(string sipAccount, DateTime startingDate, DateTime endingDate) 
        {
            wherePart = new Dictionary<string, object>();
            columns = new List<string>();

            DataTable dt = new DataTable();
            UsersCallsSummaryChartData userSummary;
            List<UsersCallsSummaryChartData> chartList = new List<UsersCallsSummaryChartData>();

            wherePart.Add("SourceUserUri", sipAccount);
            wherePart.Add("startingDate", startingDate);
            wherePart.Add("endingDate", endingDate);

              dt = DBRoutines.SELECT_USER_STATISTICS(Enums.GetDescription(Enums.PhoneCalls.TableName), wherePart);

            
            foreach (DataRow row in dt.Rows)
            {
                userSummary = new UsersCallsSummaryChartData();
                if (row[dt.Columns["PhoneCallType"]] != System.DBNull.Value && row[dt.Columns["PhoneCallType"]].ToString() == "Business")
                {
                    userSummary.Name = "Business";

                    if (row[dt.Columns["ui_CallType"]] != System.DBNull.Value)
                        userSummary.TotalCalls = Convert.ToInt32(row[dt.Columns["ui_CallType"]]);
                    else
                        userSummary.TotalCalls = 0;

                    if (row[dt.Columns["TotalDuration"]] != System.DBNull.Value)
                        userSummary.TotalDuration = Convert.ToInt32(row[dt.Columns["TotalDuration"]]);
                    else
                        userSummary.TotalDuration = 0;

                    if (row[dt.Columns["TotalCost"]] != System.DBNull.Value)
                        userSummary.TotalCost = Convert.ToInt32(row[dt.Columns["TotalCost"]]);
                    else
                        userSummary.TotalCost = 0;
                }

                else if (row[dt.Columns["PhoneCallType"]] != System.DBNull.Value && row[dt.Columns["PhoneCallType"]].ToString() == "Personal")
                {
                    userSummary.Name = "Personal";

                    if (row[dt.Columns["ui_CallType"]] != System.DBNull.Value)
                        userSummary.TotalCalls = Convert.ToInt32(row[dt.Columns["ui_CallType"]]);
                    else
                        userSummary.TotalCalls = 0;

                    if (row[dt.Columns["TotalDuration"]] != System.DBNull.Value)
                        userSummary.TotalDuration = Convert.ToInt32(row[dt.Columns["TotalDuration"]]);
                    else
                        userSummary.TotalDuration = 0;

                    if (row[dt.Columns["TotalCost"]] != System.DBNull.Value)
                        userSummary.TotalCost = Convert.ToInt32(row[dt.Columns["TotalCost"]]);
                    else
                        userSummary.TotalCost = 0;
                   
                }

                else if (row[dt.Columns["PhoneCallType"]] != System.DBNull.Value && row[dt.Columns["PhoneCallType"]].ToString() == "Disputed")
                {
                    userSummary.Name = "Disputed";

                    if (row[dt.Columns["ui_CallType"]] != System.DBNull.Value)
                        userSummary.TotalCalls = Convert.ToInt32(row[dt.Columns["ui_CallType"]]);
                    else
                        userSummary.TotalCalls = 0;

                    if (row[dt.Columns["TotalDuration"]] != System.DBNull.Value)
                        userSummary.TotalDuration = Convert.ToInt32(row[dt.Columns["TotalDuration"]]);
                    else
                        userSummary.TotalDuration = 0;

                    if (row[dt.Columns["TotalCost"]] != System.DBNull.Value)
                        userSummary.TotalCost = Convert.ToInt32(row[dt.Columns["TotalCost"]]);
                    else
                        userSummary.TotalCost = 0;

                }

                else if (row[dt.Columns["PhoneCallType"]] == System.DBNull.Value)
                {
                    userSummary.Name = "Unmarked"; 
                    if (row[dt.Columns["ui_CallType"]] != System.DBNull.Value)
                        userSummary.TotalCalls = Convert.ToInt32(row[dt.Columns["ui_CallType"]]);
                    else
                        userSummary.TotalCalls = 0;

                    if (row[dt.Columns["TotalDuration"]] != System.DBNull.Value)
                        userSummary.TotalDuration = Convert.ToInt32(row[dt.Columns["TotalDuration"]]);
                    else
                        userSummary.TotalDuration = 0;

                    if (row[dt.Columns["TotalCost"]] != System.DBNull.Value)
                        userSummary.TotalCost = Convert.ToInt32(row[dt.Columns["TotalCost"]]);
                    else
                        userSummary.TotalCost = 0;
                  
                }
                chartList.Add(userSummary);
            }
            return chartList;
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
            columns = new List<string>();

            DataTable dt = new DataTable();
            UserCallsSummary userSummary = new UserCallsSummary();

            dt = StatRoutines.USER_STATS(sipAccount, Year, fromMonth, toMonth);

            foreach (DataRow row in dt.Rows)
            {
                int year = Convert.ToInt32(row[dt.Columns["Year"]]);
                int month = Convert.ToInt32(row[dt.Columns["Month"]]);

                userSummary.MonthDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

                userSummary.BusinessCallsDuration = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["BusinessDuration"]]));
                userSummary.BusinessCallsCount = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["BusinessCallsCount"]]));
                userSummary.BusinessCallsCost = Convert.ToDecimal(ReturnZeroIfNull(row[dt.Columns["BusinessCost"]]));
                userSummary.PersonalCallsDuration = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["PersonalDuration"]]));
                userSummary.PersonalCallsCount = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["PersonalCallsCount"]]));
                userSummary.PersonalCallsCost = Convert.ToDecimal(ReturnZeroIfNull(row[dt.Columns["PersonalCost"]]));
                userSummary.UnmarkedCallsDuration = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["UnMarkedDuration"]]));
                userSummary.UnmarkedCallsCount = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["UnMarkedCallsCount"]]));
                userSummary.UnmarkedCallsCost = Convert.ToDecimal(ReturnZeroIfNull(row[dt.Columns["UnMarkedCost"]]));
                //userSummary.Month = mfi.GetAbbreviatedMonthName(month);
                userSummary.Year = year;
                userSummary.Month = month;

                userSummary.Duration = userSummary.PersonalCallsDuration / 60;
            }

            return userSummary;
        }

        public static List<UserCallsSummary> GetUsersCallsSummary(string sipAccount, int Year, int fromMonth, int toMonth)
        {
            columns = new List<string>();

            DataTable dt = new DataTable();
            UserCallsSummary userSummary;
            List<UserCallsSummary> chartList = new List<UserCallsSummary>();

            dt = StatRoutines.USER_STATS(sipAccount, Year, fromMonth, toMonth);

            foreach (DataRow row in dt.Rows)
            {

                int year = Convert.ToInt32(row[dt.Columns["Year"]]);
                int month = Convert.ToInt32(row[dt.Columns["Month"]]);
              
                
                userSummary = new UserCallsSummary();

                userSummary.MonthDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

                userSummary.BusinessCallsDuration = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["BusinessDuration"]]));
                userSummary.BusinessCallsCount = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["BusinessCallsCount"]]));
                userSummary.BusinessCallsCost = Convert.ToDecimal(ReturnZeroIfNull(row[dt.Columns["BusinessCost"]]));
                userSummary.PersonalCallsDuration = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["PersonalDuration"]]));
                userSummary.PersonalCallsCount = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["PersonalCallsCount"]]));
                userSummary.PersonalCallsCost = Convert.ToDecimal(ReturnZeroIfNull(row[dt.Columns["PersonalCost"]]));
                userSummary.UnmarkedCallsDuration = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["UnMarkedDuration"]]));
                userSummary.UnmarkedCallsCount = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["UnMarkedCallsCount"]]));
                userSummary.UnmarkedCallsCost = Convert.ToDecimal(ReturnZeroIfNull(row[dt.Columns["UnMarkedCost"]]));
                //userSummary.Month = mfi.GetAbbreviatedMonthName(month);
                userSummary.Year = year;
                userSummary.Month = month;
                
                userSummary.Duration = userSummary.PersonalCallsDuration / 60;

                chartList.Add(userSummary);
            }
            return chartList;
        }

        public static List<UserCallsSummary> GetUsersCallsSummary(DateTime startingDate, DateTime endingDate,string siteName) 
        {
            DataTable dt = new DataTable();
            UserCallsSummary userSummary;
            List<UserCallsSummary> usersSummaryList = new List<UserCallsSummary>();

            dt = StatRoutines.USERS_STATS(startingDate,endingDate, siteName);

            foreach (DataRow row in dt.Rows)
            {
                userSummary = new UserCallsSummary();

                userSummary.BusinessCallsDuration = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["BusinessDuration"]]));
                userSummary.BusinessCallsCount = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["BusinessCallsCount"]]));
                userSummary.BusinessCallsCost = Convert.ToDecimal(ReturnZeroIfNull(row[dt.Columns["BusinessCost"]]));
                userSummary.PersonalCallsDuration = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["PersonalDuration"]]));
                userSummary.PersonalCallsCount = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["PersonalCallsCount"]]));
                userSummary.PersonalCallsCost = Convert.ToDecimal(ReturnZeroIfNull(row[dt.Columns["PersonalCost"]]));
                userSummary.UnmarkedCallsDuration = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["UnMarkedDuration"]]));
                userSummary.UnmarkedCallsCount = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["UnMarkedCallsCount"]]));
                userSummary.UnmarkedCallsCost = Convert.ToDecimal(ReturnZeroIfNull(row[dt.Columns["UnMarkedCost"]]));
                userSummary.SipAccount = Convert.ToString(ReturnEmptyIfNull(row[dt.Columns["SourceUserUri"]]));
                userSummary.FullName = Convert.ToString(ReturnEmptyIfNull(row[dt.Columns[Enums.GetDescription(Enums.Users.AD_DisplayName)]]));
                userSummary.EmployeeID = Convert.ToString(ReturnEmptyIfNull(row[dt.Columns[Enums.GetDescription(Enums.Users.AD_UserID)]]));
                userSummary.SiteName = Convert.ToString(ReturnEmptyIfNull(row[dt.Columns[Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName)]]));

                userSummary.Duration = userSummary.PersonalCallsDuration / 60;

                usersSummaryList.Add(userSummary);
            }
            return usersSummaryList;
        }

        public static Dictionary<string, UserCallsSummary> GetUsersCallsSummary(List<string> sipAccounts, DateTime startingDate, DateTime endingDate, string siteName)
        {
            DataTable dt = new DataTable();
            
            List<string> columns = new List<string>()
            {
                "SUM(BusinessDuration) AS BusinessDuration",
                "SUM(PersonalDuration) AS PersonalDuration",
                "SUM(UnMarkedDuration) AS UnMarkedDuration"
            };

            List<string> sipAccount = new List<string>();

            UserCallsSummary userSummary;
            Dictionary<string, UserCallsSummary> usersSummaryList = new Dictionary<string, UserCallsSummary>();

            dt = StatRoutines.DISTINCT_USERS_STATS(startingDate, endingDate, siteName, columns);

            foreach (DataRow row in dt.Rows)
            {
                userSummary = new UserCallsSummary();

                if (dt.Columns.Contains("BusinessCallsCount"))
                    userSummary.BusinessCallsCount = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["BusinessCallsCount"]]));
                
                if(dt.Columns.Contains("PersonalCallsCount"))
                    userSummary.PersonalCallsCount = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["PersonalCallsCount"]]));

                if(dt.Columns.Contains("UnMarkedCallsCount"))
                    userSummary.UnmarkedCallsCount = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["UnMarkedCallsCount"]]));

                if (dt.Columns.Contains("BusinessDuration"))
                    userSummary.BusinessCallsDuration = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["BusinessDuration"]]));

                if (dt.Columns.Contains("BusinessCost"))
                    userSummary.BusinessCallsCost = Convert.ToDecimal(ReturnZeroIfNull(row[dt.Columns["BusinessCost"]]));

                if (dt.Columns.Contains("PersonalDuration"))
                    userSummary.PersonalCallsDuration = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["PersonalDuration"]]));

                if (dt.Columns.Contains("PersonalCost"))
                    userSummary.PersonalCallsCost = Convert.ToDecimal(ReturnZeroIfNull(row[dt.Columns["PersonalCost"]]));

                if (dt.Columns.Contains("UnMarkedDuration"))
                    userSummary.UnmarkedCallsDuration = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["UnMarkedDuration"]]));

                if (dt.Columns.Contains("UnMarkedCost"))
                    userSummary.UnmarkedCallsCost = Convert.ToDecimal(ReturnZeroIfNull(row[dt.Columns["UnMarkedCost"]]));

                if (dt.Columns.Contains("SourceUserUri"))
                    userSummary.SipAccount = Convert.ToString(ReturnEmptyIfNull(row[dt.Columns["SourceUserUri"]]));

                if (dt.Columns.Contains(Enums.GetDescription(Enums.Users.AD_DisplayName)))
                    userSummary.FullName = Convert.ToString(ReturnEmptyIfNull(row[dt.Columns[Enums.GetDescription(Enums.Users.AD_DisplayName)]]));

                if (dt.Columns.Contains(Enums.GetDescription(Enums.Users.AD_UserID)))
                    userSummary.EmployeeID = Convert.ToString(ReturnEmptyIfNull(row[dt.Columns[Enums.GetDescription(Enums.Users.AD_UserID)]]));

                if (dt.Columns.Contains(Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName)))
                    userSummary.SiteName = Convert.ToString(ReturnEmptyIfNull(row[dt.Columns[Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName)]]));

                userSummary.Duration = userSummary.PersonalCallsDuration / 60;

                usersSummaryList.Add(userSummary.SipAccount,userSummary);
            }
            return usersSummaryList;
        }

        public static void ExportUsersCallsSummaryToPDF(DateTime startingDate, DateTime endingDate, string siteName, Dictionary<string, Dictionary<string, object>> UsersCollection, HttpResponse response, out Document document, Dictionary<string, string> headers)
        {
            DataTable dt = new DataTable();
            List<string> columns = new List<string>(); //this is used to be passed tothe DISNTINCT_USERS_STATS function, as an empty list param
            List<string> columnSchema = new List<string>() { "AD_UserID", "SourceUserUri", "AD_DisplayName", "BusinessCost", "PersonalCost", "UnMarkedCost" }; //This is passed to the PdfLib
            int[] widths = new int[] { 4, 6, 7, 4, 4, 4 };
            
            headers.Add("comments", "* Please note that the terms: Business, Personal and Unallocated Calls Costs were abbreviated as Bus. Cost, Per. Cost and Unac. Cost respectively in the following report's columns-headers.");

            //dt = StatRoutines.DISTINCT_USERS_STATS_SUMMARY(startingDate, endingDate, UsersCollection.Keys.ToList(), columns);
            dt = StatRoutines.DISTINCT_USERS_STATS(startingDate, endingDate, siteName, UsersCollection.Keys.ToList(), columns);

            Dictionary<string, object> totals;

            //Try to compute totals, if an error occurs which is the case of an empty "dt", set the totals dictionary to zeros
            
            try
            {
                totals = new Dictionary<string, object>()
                {
                    {"PersonalCost", Decimal.Round(Convert.ToDecimal(ReturnZeroIfNull(dt.Compute("Sum(PersonalCost)", "PersonalCost > 0"))), 2)},
                    {"BusinessCost", Decimal.Round(Convert.ToDecimal(ReturnZeroIfNull(dt.Compute("Sum(BusinessCost)", "BusinessCost > 0"))), 2)},
                    {"UnMarkedCost", Decimal.Round(Convert.ToDecimal(ReturnZeroIfNull(dt.Compute("Sum(UnMarkedCost)", "UnMarkedCost > 0"))), 2)}
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

            document = PDFLib.CreateAccountingSummaryReport(response, dt, totals, headers, columnSchema, widths);
        }

        public static void ExportUsersCallsDetailedToPDF(DateTime startingDate, DateTime endingDate, string siteName, Dictionary<string, Dictionary<string, object>> UsersCollection, HttpResponse response, out Document document, Dictionary<string, string> headers)
        {
            DataTable dt = new DataTable();

            //PDF Document related variables;
            List<string> columns = new List<string>()
            { 
                "SourceUserUri", 
                "ResponseTime", 
                "marker_CallToCountry", 
                "DestinationNumberUri", 
                "Duration", 
                "marker_CallCost", 
                "ui_CallType"
            };

            //This is passed to the PdfLib
            List<string> pdfColumnSchema = new List<string>()
            {
                "ResponseTime",
                "marker_CallToCountry",
                "DestinationNumberUri",
                "Duration",
                "marker_CallCost",
                "ui_CallType"
            };

            int[] widths = new int[] { 7, 4, 6, 4, 3, 4 };

            //The PDF report body contents
            dt = StatRoutines.DISTINCT_USERS_STATS_DETAILED(startingDate, endingDate, UsersCollection.Keys.ToList(), columns);
            Dictionary<string, UserCallsSummary> UsersSummaires = UserCallsSummary.GetUsersCallsSummary(UsersCollection.Keys.ToList(), startingDate, endingDate, siteName);

            //Get a closed instance of the document which contains all the formatted data
            document = PDFLib.CreateAccountingDetailedReport(response, dt, widths, pdfColumnSchema, headers, "SourceUserUri", UsersCollection, UsersSummaires);
        }


        private static object ReturnZeroIfNull(object value) 
        {
            if (value == System.DBNull.Value)
                return 0;
            else
                return value;
        }


        private static object ReturnEmptyIfNull(object value) 
        {
            if (value == System.DBNull.Value)
                return string.Empty;
            else
                return value;
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

        public static List<DepartmentCallsSummary> GetPhoneCallsStatisticsForDepartment(string departmentName, string siteName, int year)
        {
            DataTable dt = new DataTable();
            DepartmentCallsSummary departmentSummary;
            List<DepartmentCallsSummary> ListOfDepartmentCallsSummaries = new List<DepartmentCallsSummary>();

            List<object> functionParameters = new List<object>();
            functionParameters.Add(departmentName);
            functionParameters.Add(siteName);

            dt = DBRoutines.SELECT_FROM_FUNCTION("fnc_Chargable_Calls_By_Site_Department", functionParameters, null);

            foreach (DataRow row in dt.Rows)
            {
                departmentSummary = new DepartmentCallsSummary();

                if (dt.Columns.Contains("BusinessCallsCount"))
                    departmentSummary.BusinessCallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns["BusinessCallsCount"]]));

                if (dt.Columns.Contains("PersonalCallsCount"))
                    departmentSummary.PersonalCallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns["PersonalCallsCount"]]));

                if (dt.Columns.Contains("UnMarkedCallsCount"))
                    departmentSummary.UnmarkedCallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns["UnMarkedCallsCount"]]));

                if (dt.Columns.Contains("BusinessDuration"))
                    departmentSummary.BusinessCallsDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns["BusinessDuration"]]));

                if (dt.Columns.Contains("BusinessCost"))
                    departmentSummary.BusinessCallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns["BusinessCost"]]));

                if (dt.Columns.Contains("PersonalDuration"))
                    departmentSummary.PersonalCallsDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns["PersonalDuration"]]));

                if (dt.Columns.Contains("PersonalCost"))
                    departmentSummary.PersonalCallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns["PersonalCost"]]));

                if (dt.Columns.Contains("UnMarkedDuration"))
                    departmentSummary.UnmarkedCallsDuration = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns["UnMarkedDuration"]]));

                if (dt.Columns.Contains("UnMarkedCost"))
                    departmentSummary.UnmarkedCallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[dt.Columns["UnMarkedCost"]]));

                if (dt.Columns.Contains(Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName)))
                    departmentSummary.SiteName = Convert.ToString(Misc.ReturnEmptyIfNull(row[dt.Columns[Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName)]]));

                if (dt.Columns.Contains("Month"))
                    departmentSummary.Month = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns["Month"]]));

                if (dt.Columns.Contains("Year"))
                    departmentSummary.Year = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns["Year"]]));

                if (dt.Columns.Contains("Date"))
                    departmentSummary.MonthDate = Convert.ToDateTime(row[dt.Columns["Date"]]);

                ListOfDepartmentCallsSummaries.Add(departmentSummary);
            }

            return ListOfDepartmentCallsSummaries.Where(summary => summary.Year == year).ToList<DepartmentCallsSummary>();
        }
    }
}