using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;
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
                if (row[dt.Columns["PhoneCallType"]] != System.DBNull.Value && row[dt.Columns["PhoneCallType"]].ToString() == "NO")
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

                else if (row[dt.Columns["PhoneCallType"]] != System.DBNull.Value && row[dt.Columns["PhoneCallType"]].ToString() == "YES")
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

        private static Dictionary<string, object> wherePart;
        private static List<string> columns;

        public int BusinessCallsCount { get; set; }
        public decimal BusinessCallsCost { get; set; }
        public int BusinessCallsDuration { get; set; }
        public int PersonalCallsCount { get; set; }
        public int PersonalCallsDuration { get; set; }
        public decimal PersonalCallsCost { get; set; }
        public int UnmarkedCallsCount { get; set; }
        public int UnmarkedCallsDuartion { get; set; }
        public decimal UnmarkedCallsCost { get; set; }
        public int NumberOfDisputedCalls { get; set; }

        public DateTime MonthDate { set; get; }
        public decimal Duration { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }

        public static UsersCallsSummary GetUsersCallsSummary(string sipAccount, DateTime startingDate, DateTime endingDate)
        {
            wherePart = new Dictionary<string, object>();
            columns = new List<string>();

            DataTable dt = new DataTable();
            UsersCallsSummary userSummary = new UsersCallsSummary();

            wherePart.Add("SourceUserUri", sipAccount);
            wherePart.Add("startingDate", startingDate);
            wherePart.Add("endingDate", endingDate);

            dt = DBRoutines.SELECT_USER_STATISTICS(Enums.GetDescription(Enums.PhoneCalls.TableName), wherePart);

            
            foreach (DataRow row in dt.Rows)
            {
                if (row[dt.Columns["PhoneCallType"]] != System.DBNull.Value && row[dt.Columns["PhoneCallType"]].ToString() == "Business")
                {
                    if (row[dt.Columns["ui_CallType"]] != System.DBNull.Value)
                        userSummary.BusinessCallsCount = Convert.ToInt32(row[dt.Columns["ui_CallType"]]);
                    else
                        userSummary.BusinessCallsCount = 0;

                    if (row[dt.Columns["TotalCost"]] != System.DBNull.Value)
                        userSummary.BusinessCallsCost = Convert.ToInt32(row[dt.Columns["TotalCost"]]);
                    else
                        userSummary.BusinessCallsCost = 0;

                    if (row[dt.Columns["TotalDuration"]] != System.DBNull.Value)
                        userSummary.BusinessCallsDuration = Convert.ToInt32(row[dt.Columns["TotalDuration"]]);
                    else
                        userSummary.BusinessCallsDuration = 0;
                }

                else if (row[dt.Columns["PhoneCallType"]] != System.DBNull.Value && row[dt.Columns["PhoneCallType"]].ToString() == "Personal")
                {
                    if (row[dt.Columns["ui_CallType"]] != System.DBNull.Value)
                        userSummary.PersonalCallsCount = Convert.ToInt32(row[dt.Columns["ui_CallType"]]);
                    else
                        userSummary.PersonalCallsCount = 0;

                    if (row[dt.Columns["TotalCost"]] != System.DBNull.Value)
                        userSummary.PersonalCallsCost = Convert.ToInt32(row[dt.Columns["TotalCost"]]);
                    else
                        userSummary.PersonalCallsCost = 0;

                    if (row[dt.Columns["TotalDuration"]] != System.DBNull.Value)
                        userSummary.PersonalCallsDuration = Convert.ToInt32(row[dt.Columns["TotalDuration"]]);
                    else
                        userSummary.PersonalCallsDuration = 0;
                }

                else if (row[dt.Columns["PhoneCallType"]] == System.DBNull.Value)
                {
                    if (row[dt.Columns["ui_CallType"]] != System.DBNull.Value)
                        userSummary.UnmarkedCallsCount = Convert.ToInt32(row[dt.Columns["ui_CallType"]]);
                    else
                        userSummary.UnmarkedCallsCount = 0;

                    if (row[dt.Columns["TotalCost"]] != System.DBNull.Value)
                        userSummary.UnmarkedCallsCost = Convert.ToInt32(row[dt.Columns["TotalCost"]]);
                    else
                        userSummary.UnmarkedCallsCost = 0;

                    if (row[dt.Columns["TotalDuration"]] != System.DBNull.Value)
                        userSummary.UnmarkedCallsDuartion = Convert.ToInt32( row[dt.Columns["TotalDuration"]]);
                    else
                        userSummary.UnmarkedCallsDuartion = 0;
                }
            }
            return userSummary;
        }

        public static List<UsersCallsSummary> GetUsersCallsSummary(string sipAccount, int Year, int fromMonth, int toMonth)
        {
            columns = new List<string>();

            DataTable dt = new DataTable();
            UsersCallsSummary userSummary;
            List<UsersCallsSummary> chartList = new List<UsersCallsSummary>();

            //System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();

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
                userSummary.UnmarkedCallsDuartion = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["UnMarkedDuration"]]));
                userSummary.UnmarkedCallsCount = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["UnMarkedCallsCount"]]));
                userSummary.UnmarkedCallsCost = Convert.ToDecimal(ReturnZeroIfNull(row[dt.Columns["UnMarkedCost"]]));
                //userSummary.Month = mfi.GetAbbreviatedMonthName(month);
                userSummary.Year = year;
                userSummary.Month = month;
                
                userSummary.Duration = userSummary.PersonalCallsDuration / 60;

                //if (month == previousMonth + 1)
                //    previousMonth = month;
                //else
                //{
                //    UsersCallsSummary glueSummary = new UsersCallsSummary();
                //    glueSummary.BusinessCallsDuration = 0;
                //    glueSummary.BusinessCallsCount = 0;
                //    glueSummary.BusinessCallsCost = 0;
                //    glueSummary.PersonalCallsDuration = 0;
                //    glueSummary.PersonalCallsCount = 0;
                //    glueSummary.PersonalCallsCost = 0;
                //    glueSummary.UnmarkedCallsDuartion = 0;
                //    glueSummary.UnmarkedCallsCount = 0;
                //    glueSummary.UnmarkedCallsCost = 0;
                //    glueSummary.Month = mfi.GetAbbreviatedMonthName(month);
                //    if (previousMonth == 12)
                //        glueSummary.Year = year + 1;
                //    else
                //        glueSummary.Year = year;

                //}
                chartList.Add(userSummary);
            }
            return chartList;
        }

        private static object ReturnZeroIfNull(object value) 
        {
            if (value == System.DBNull.Value)
                return 0;
            else
                return value;
        }
        
    }
}