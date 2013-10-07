﻿using Lync_Billing.Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
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
            DataTable dt = new DataTable();
            //DateTime previousMonth = DateTime.Now.AddMonths(-1).AddDays(-(DateTime.Today.Day - 1));
            MailStatistics userMailStats = new MailStatistics();

            Statistics statsLib = new Statistics();
            dt = statsLib.GET_MAIL_STATISTICS(sipAccount, date);
            
            foreach (DataRow row in dt.Rows)
            {
                userMailStats.EmailAddress = (row[dt.Columns["EmailAddress"]]).ToString();
                userMailStats.ReceivedCount = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["RecievedCount"]]));
                userMailStats.ReceivedSize = (Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["RecievedSize"]])) / 1024) / 1024; //convert Bytes to MB
                userMailStats.SentCount = Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["SentCount"]]));
                userMailStats.SentSize = (Convert.ToInt32(ReturnZeroIfNull(row[dt.Columns["SentSize"]])) / 1024) / 1024; //convert Bytes to MB
            }

            return userMailStats;
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