﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using Lync_Billing.Libs;


namespace Lync_Billing.DB.Statistics
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
            List<string> columnsList = new List<string>();
            Dictionary<string, object> whereClause = new Dictionary<string, object>();

            DateTime startingDate, endingDate;

            MailStatistics userMailStats = new MailStatistics();

            if (date == null || date == DateTime.MinValue)
            {
                //Both starting date and ending date respectively point to the beginning and ending of this current month.
                startingDate = DateTime.Now.AddDays(-(DateTime.Today.Day - 1));
                endingDate = startingDate.AddMonths(1).AddDays(-1);
            }
            else
            {
                //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
                DateTime specificDate = (DateTime)date;
                startingDate = specificDate.AddDays(-(specificDate.Day - 1));
                endingDate = startingDate.AddMonths(1).AddDays(-1);
            }

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.MailStatistics.TableName), columnsList, whereClause, 0);
            

            foreach (DataRow row in dt.Rows)
            {
                columnName = Enums.GetDescription(Enums.MailStatistics.EmailAddress);
                userMailStats.EmailAddress = (row[dt.Columns[columnName]]).ToString();

                columnName = Enums.GetDescription(Enums.MailStatistics.ReceivedCount);
                userMailStats.ReceivedCount = Convert.ToInt64(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.MailStatistics.ReceivedSize);
                userMailStats.ReceivedSize = (Convert.ToInt64(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]])) / 1024) / 1024; //convert Bytes to MB

                columnName = Enums.GetDescription(Enums.MailStatistics.SentCount);
                userMailStats.SentCount = Convert.ToInt64(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.MailStatistics.SentSize);
                userMailStats.SentSize = (Convert.ToInt64(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]])) / 1024) / 1024; //convert Bytes to MB
            }

            return userMailStats;
        }

        public static MailStatistics GetMailStatistics(string siteName, string departmentName, DateTime date)
        {
            DataTable dt = new DataTable();
            string columnName = string.Empty;
            MailStatistics departmentTotalMailStats = new MailStatistics();
            DateTime startingDate, endingDate;

            if (date == null || date == DateTime.MinValue)
            {
                //Both starting date and ending date respectively point to the beginning and ending of this current month.
                startingDate = DateTime.Now.AddDays(-(DateTime.Today.Day - 1));
                endingDate = startingDate.AddMonths(1).AddDays(-1);
            }
            else
            {
                //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
                DateTime specificDate = (DateTime)date;
                startingDate = specificDate.AddDays(-(specificDate.Day - 1));
                endingDate = startingDate.AddMonths(1).AddDays(-1);
            }

            //Initialize the select parameters for the database function
            List<object> selectParameters = new List<object>();
            
            selectParameters.Add(siteName);
            selectParameters.Add(departmentName);
            selectParameters.Add(startingDate);
            selectParameters.Add(endingDate);
            

            dt = DBRoutines.SELECT_FROM_FUNCTION("Get_MailStatistics_PerSiteDepartment", selectParameters, null);

            foreach (DataRow row in dt.Rows)
            {
                columnName = Enums.GetDescription(Enums.MailStatistics.ReceivedCount);
                departmentTotalMailStats.ReceivedCount = Convert.ToInt64(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.MailStatistics.ReceivedSize);
                departmentTotalMailStats.ReceivedSize = (Convert.ToInt64(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]])) / 1024) / 1024; //convert Bytes to MB

                columnName = Enums.GetDescription(Enums.MailStatistics.SentCount);
                departmentTotalMailStats.SentCount = Convert.ToInt64(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.MailStatistics.SentSize);
                departmentTotalMailStats.SentSize = (Convert.ToInt64(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]])) / 1024) / 1024; //convert Bytes to MB
            }

            return departmentTotalMailStats;
        }
        
    }
}