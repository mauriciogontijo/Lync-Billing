using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
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

    public class UsersCallsSummary
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

        public static List<UsersCallsSummary> GetUsersCallsSummary(string sipAccount, int Year, int fromMonth, int toMonth)
        {
            columns = new List<string>();

            DataTable dt = new DataTable();
            UsersCallsSummary userSummary;
            List<UsersCallsSummary> chartList = new List<UsersCallsSummary>();

            dt = StatRoutines.USER_STATS(sipAccount, Year, fromMonth, toMonth);

            foreach (DataRow row in dt.Rows)
            {

                int year = Convert.ToInt32(row[dt.Columns["Year"]]);
                int month = Convert.ToInt32(row[dt.Columns["Month"]]);
              
                
                userSummary = new UsersCallsSummary();

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

        public static List<UsersCallsSummary> GetUsersCallsSummary(DateTime startingDate, DateTime endingDate,string siteName) 
        {
            DataTable dt = new DataTable();
            UsersCallsSummary userSummary;
            List<UsersCallsSummary> usersSummaryList = new List<UsersCallsSummary>();

            dt = StatRoutines.USERS_STATS(startingDate,endingDate, siteName);

            foreach (DataRow row in dt.Rows)
            {
                userSummary = new UsersCallsSummary();

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

        public static void ExportUsersCallsSummaryToPDF(DateTime startingDate, DateTime endingDate, string siteName, HttpResponse response, out Document document, Dictionary<string, string> headers)
        {
            DataTable dt = new DataTable();
            List<string> columns = new List<string>(); //this is used to be passed tothe DISNTINCT_USERS_STATS function, as an empty list param
            List<string> columnSchema = new List<string>() { "AD_UserID", "SourceUserUri", "AD_DisplayName", "BusinessCost", "PersonalCost", "UnMarkedCost" }; //This is passed to the PdfLib
            int[] widths = new int[] { 4, 6, 7, 4, 4, 4 };
            
            headers.Add("comments", "* Please note that the terms: Business, Personal and Unallocated Calls Costs were abbreviated as Bus. Cost, Per. Cost and Unac. Cost respectively in the following report's columns-headers.");

            dt = StatRoutines.DISTINCT_USERS_STATS(startingDate, endingDate, siteName, columns);

            Dictionary<string, object> totals;

            //Try to compute totals, if an error occurs which is the case of an empty "dt", set the totals dictionary to zeros
            try
            {
                totals = new Dictionary<string, object>()
                {
                    {"PersonalCost", Decimal.Round(Convert.ToDecimal(dt.Compute("Sum(PersonalCost)", "PersonalCost > 0")), 2)},
                    {"BusinessCost", Decimal.Round(Convert.ToDecimal(dt.Compute("Sum(BusinessCost)", "BusinessCost > 0")), 2)},
                    {"UnMarkedCost", Decimal.Round(Convert.ToDecimal(dt.Compute("Sum(UnMarkedCost)", "UnMarkedCost > 0")), 2)}
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

            document = PDFLib.InitializePDFDocument(response);
            PdfPTable pdfContentsTable = PDFLib.InitializePDFTable(dt.Columns.Count, widths);
            PDFLib.AddPDFHeader(ref document, headers);
            PDFLib.AddPDFTableContents(ref document, ref pdfContentsTable, dt, columnSchema);
            PDFLib.AddPDFTableTotalsRow(ref document, totals, dt, widths);
            PDFLib.ClosePDFDocument(ref document);
        }

        public static void ExportUsersCallsDetailedToPDF(DateTime startingDate, DateTime endingDate, List<string> SipAccountsList, HttpResponse response, out Document document, Dictionary<string, string> headers)
        {
            DataTable dt = new DataTable();
            List<string> columns = new List<string>() { "SourceUserUri", "ResponseTime", "marker_CallToCountry", "DestinationNumberUri", "Duration", "marker_CallCost", "ui_CallType" };
            List<string> pdfColumnSchema = new List<string>() { "ResponseTime", "marker_CallToCountry", "DestinationNumberUri", "Duration", "marker_CallCost", "ui_CallType" }; //This is passed to the PdfLib
            int[] widths = new int[] { 6, 4, 6, 4, 4, 4 };

            headers.Add("comments", "* Please note that the terms: Business, Personal and Unallocated Calls Costs were abbreviated as Bus. Cost, Per. Cost and Unac. Cost respectively in the following report's columns-headers.");

            dt = StatRoutines.DISTINCT_USERS_STATS_DETAILED(startingDate, endingDate, SipAccountsList, columns);

            Dictionary<string, object> totals;

            //Try to compute totals, if an error occurs which is the case of an empty "dt", set the totals dictionary to zeros
            try
            {
                totals = new Dictionary<string, object>()
                {
                    {"PersonalCost", Decimal.Round(Convert.ToDecimal(dt.Compute("Sum(PersonalCost)", "PersonalCost > 0")), 2)},
                    {"BusinessCost", Decimal.Round(Convert.ToDecimal(dt.Compute("Sum(BusinessCost)", "BusinessCost > 0")), 2)},
                    {"UnMarkedCost", Decimal.Round(Convert.ToDecimal(dt.Compute("Sum(UnMarkedCost)", "UnMarkedCost > 0")), 2)}
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

            document = PDFLib.InitializePDFDocument(response);
            //PdfPTable pdfContentsTable = PDFLib.InitializePDFTable(dt.Columns.Count, widths);
            PDFLib.AddPDFHeader(ref document, headers);
            PDFLib.AddCombinedPDFTablesContents(ref document, dt, widths, "SourceUserUri", SipAccountsList, pdfColumnSchema);
            PDFLib.AddPDFTableTotalsRow(ref document, totals, dt, widths);
            PDFLib.ClosePDFDocument(ref document);
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
}