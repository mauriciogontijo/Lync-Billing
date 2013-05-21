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

                    if (row[dt.Columns["ui_IsPersonal"]] != System.DBNull.Value)
                        userSummary.TotalCalls = Convert.ToInt32(row[dt.Columns["ui_IsPersonal"]]);
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

                    if (row[dt.Columns["ui_IsPersonal"]] != System.DBNull.Value)
                        userSummary.TotalCalls = Convert.ToInt32(row[dt.Columns["ui_IsPersonal"]]);
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
                    if (row[dt.Columns["ui_IsPersonal"]] != System.DBNull.Value)
                        userSummary.TotalCalls = Convert.ToInt32(row[dt.Columns["ui_IsPersonal"]]);
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
                if (row[dt.Columns["PhoneCallType"]] != System.DBNull.Value && row[dt.Columns["PhoneCallType"]].ToString() == "NO")
                {
                    if (row[dt.Columns["ui_IsPersonal"]] != System.DBNull.Value)
                        userSummary.BusinessCallsCount = Convert.ToInt32(row[dt.Columns["ui_IsPersonal"]]);
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

                else if (row[dt.Columns["PhoneCallType"]] != System.DBNull.Value && row[dt.Columns["PhoneCallType"]].ToString() == "YES")
                {
                    if (row[dt.Columns["ui_IsPersonal"]] != System.DBNull.Value)
                        userSummary.PersonalCallsCount = Convert.ToInt32(row[dt.Columns["ui_IsPersonal"]]);
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
                    if (row[dt.Columns["ui_IsPersonal"]] != System.DBNull.Value)
                        userSummary.UnmarkedCallsCount = Convert.ToInt32(row[dt.Columns["ui_IsPersonal"]]);
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

            dt = StatRoutines.USER_STATS(sipAccount, Year, fromMonth, toMonth);

            foreach (DataRow row in dt.Rows)
            {
                userSummary = new UsersCallsSummary();

                userSummary.Month = Convert.ToInt32(row[dt.Columns["Month"]]);
                userSummary.Year = Convert.ToInt32(row[dt.Columns["Year"]]);
                
                if (row[dt.Columns["BusinessDuration"]] != System.DBNull.Value)
                    userSummary.BusinessCallsDuration = 0;
                else
                    userSummary.BusinessCallsDuration = Convert.ToInt32(row[dt.Columns["BusinessDuration"]]);

                if (row[dt.Columns["BusinessCallsCount"]] != System.DBNull.Value)
                    userSummary.BusinessCallsCount = 0;
                else
                    userSummary.BusinessCallsCount = Convert.ToInt32(row[dt.Columns["BusinessCallsCount"]]);

                if (row[dt.Columns["BusinessCost"]] != System.DBNull.Value)
                    userSummary.BusinessCallsCost = 0;
                else
                    userSummary.BusinessCallsCost = Convert.ToDecimal(row[dt.Columns["BusinessCost"]]);

                if (row[dt.Columns["PersonalDuartion"]] != System.DBNull.Value)
                    userSummary.PersonalCallsDuration = 0;
                else
                    userSummary.PersonalCallsDuration = Convert.ToInt32(row[dt.Columns["PersonalDuartion"]]);

                if (row[dt.Columns["PersonalCallsCount"]] != System.DBNull.Value)
                    userSummary.PersonalCallsCount = 0;
                else
                    userSummary.PersonalCallsCount = Convert.ToInt32(row[dt.Columns["PersonalCallsCount"]]);

                if (row[dt.Columns["PersonalCost"]] != System.DBNull.Value)
                    userSummary.PersonalCallsCost = 0;
                else
                    userSummary.PersonalCallsCost = Convert.ToDecimal(row[dt.Columns["PersonalCost"]]);

                if (row[dt.Columns["UnMarkedDuartion"]] != System.DBNull.Value)
                    userSummary.UnmarkedCallsDuartion = 0;
                else
                    userSummary.UnmarkedCallsDuartion = Convert.ToInt32(row[dt.Columns["UnMarkedDuartion"]]);

                if (row[dt.Columns["UnMarkedCallsCount"]] != System.DBNull.Value)
                    userSummary.UnmarkedCallsCount = 0;
                else
                    userSummary.UnmarkedCallsCount = Convert.ToInt32(row[dt.Columns["UnMarkedCallsCount"]]);

                if (row[dt.Columns["UnMarkedCost"]] != System.DBNull.Value)
                    userSummary.UnmarkedCallsCost = 0;
                else
                    userSummary.UnmarkedCallsCost = Convert.ToDecimal(row[dt.Columns["UnMarkedCost"]]);

                chartList.Add(userSummary);
            }

            return chartList;
        }
        
    }
}