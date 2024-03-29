﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using Lync_Billing.Libs;


namespace Lync_Billing.Backend.Statistics
{
    public class MailStatistics
    {
        public int ID { get; set; }
        public string EmailAddress { get; set; }
        public long ReceivedCount { get; set; }
        public long ReceivedSize { get; set; }
        public long SentCount { get; set; }
        public long SentSize { get; set; }
        
        private static DBLib DBRoutines = new DBLib();

        public static MailStatistics GetMailStatistics(string sipAccount, DateTime date)
        {
            DataTable dt;
            string columnName = string.Empty;
            List<string> columnsList;
            Dictionary<string, object> whereClause;

            string startingDate, endingDate;
            MailStatistics userMailStats = new MailStatistics();


            if (date == null || date == DateTime.MinValue)
            {
                //Both starting date and ending date respectively point to the beginning and ending of this current month.
                var tempStartingDate = DateTime.Now.AddDays(-(DateTime.Today.Day - 1));

                //example:
                //starting date: 2013-12-01
                //ending date: 2014-01-01
                startingDate = SpecialDateTime.ConvertDate(tempStartingDate, excludeHoursAndMinutes: true);
                endingDate = SpecialDateTime.ConvertDate(tempStartingDate.AddMonths(1), excludeHoursAndMinutes: true);
            }
            else
            {
                //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
                DateTime specificDate = (DateTime)date;
                var tempStartingDate = specificDate.AddDays(-(specificDate.Day - 1));

                //example:
                //starting date: 2013-12-01
                //ending date: 2014-01-01
                startingDate = SpecialDateTime.ConvertDate(tempStartingDate, excludeHoursAndMinutes: true);
                endingDate = SpecialDateTime.ConvertDate(tempStartingDate.AddMonths(1), excludeHoursAndMinutes: true);
            }


            //Initialize the database query parameters
            columnsList = new List<string>();
            whereClause = new Dictionary<string, object>
            {
                { Enums.GetDescription(Enums.MailStatistics.EmailAddress), sipAccount },
                { Enums.GetDescription(Enums.MailStatistics.Timestamp), String.Format("BETWEEN '{0}' AND '{1}'", startingDate, endingDate) }
            };

            //Get the data from the database
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.MailStatistics.TableName), columnsList, whereClause, 1);
            

            foreach (DataRow row in dt.Rows)
            {
                columnName = Enums.GetDescription(Enums.MailStatistics.EmailAddress);
                userMailStats.EmailAddress = (row[dt.Columns[columnName]]).ToString();

                columnName = Enums.GetDescription(Enums.MailStatistics.ReceivedCount);
                userMailStats.ReceivedCount = Convert.ToInt64(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.MailStatistics.ReceivedSize);
                userMailStats.ReceivedSize = (Convert.ToInt64(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]])) / 1024) / 1024; //convert Bytes to MB

                columnName = Enums.GetDescription(Enums.MailStatistics.SentCount);
                userMailStats.SentCount = Convert.ToInt64(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.MailStatistics.SentSize);
                userMailStats.SentSize = (Convert.ToInt64(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]])) / 1024) / 1024; //convert Bytes to MB
            }

            return userMailStats;
        }

        public static MailStatistics GetMailStatistics(string siteName, string departmentName, DateTime date)
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_MailStatistics_PerSiteDepartment);
            string columnName = string.Empty;

            MailStatistics departmentTotalMailStats = new MailStatistics();
            string startingDate, endingDate;


            if (date == null || date == DateTime.MinValue)
            {
                //Both starting date and ending date respectively point to the beginning and ending of this current month.
                var tempStartingDate = DateTime.Now.AddDays(-(DateTime.Today.Day - 1));

                //example:
                //starting date: 2013-12-01
                //ending date: 2014-01-01
                startingDate = SpecialDateTime.ConvertDate(tempStartingDate, excludeHoursAndMinutes: true);
                endingDate = SpecialDateTime.ConvertDate(tempStartingDate.AddMonths(1), excludeHoursAndMinutes: true);
            }
            else
            {
                //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
                DateTime specificDate = (DateTime)date;
                var tempStartingDate = specificDate.AddDays(-(specificDate.Day - 1));

                //example:
                //starting date: 2013-12-01
                //ending date: 2014-01-01
                startingDate = SpecialDateTime.ConvertDate(tempStartingDate, excludeHoursAndMinutes: true);
                endingDate = SpecialDateTime.ConvertDate(tempStartingDate.AddMonths(1), excludeHoursAndMinutes: true);
            }


            //Initialize the select parameters for the database function
            List<object> selectParameters = new List<object>();
            
            selectParameters.Add(siteName);
            selectParameters.Add(departmentName);
            selectParameters.Add(startingDate);
            selectParameters.Add(endingDate);


            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, selectParameters, null);

            foreach (DataRow row in dt.Rows)
            {
                columnName = Enums.GetDescription(Enums.MailStatistics.ReceivedCount);
                departmentTotalMailStats.ReceivedCount = Convert.ToInt64(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.MailStatistics.ReceivedSize);
                departmentTotalMailStats.ReceivedSize = (Convert.ToInt64(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]])) / 1024) / 1024; //convert Bytes to MB

                columnName = Enums.GetDescription(Enums.MailStatistics.SentCount);
                departmentTotalMailStats.SentCount = Convert.ToInt64(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.MailStatistics.SentSize);
                departmentTotalMailStats.SentSize = (Convert.ToInt64(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]])) / 1024) / 1024; //convert Bytes to MB
            }

            return departmentTotalMailStats;
        }
        
    }
}