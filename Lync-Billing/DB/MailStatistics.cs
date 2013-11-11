using Lync_Billing.Libs;
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
            string columnName = string.Empty;
            //DateTime previousMonth = DateTime.Now.AddMonths(-1).AddDays(-(DateTime.Today.Day - 1));
            MailStatistics userMailStats = new MailStatistics();

            Statistics statsLib = new Statistics();
            dt = statsLib.GET_MAIL_STATISTICS(sipAccount, date);
            
            foreach (DataRow row in dt.Rows)
            {
                columnName = Enums.GetDescription(Enums.MailStatistics.EmailAddress);
                userMailStats.EmailAddress = (row[dt.Columns[columnName]]).ToString();

                columnName = Enums.GetDescription(Enums.MailStatistics.ReceivedCount);
                userMailStats.ReceivedCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.MailStatistics.ReceivedSize);
                userMailStats.ReceivedSize = (Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]])) / 1024) / 1024; //convert Bytes to MB

                columnName = Enums.GetDescription(Enums.MailStatistics.SentCount);
                userMailStats.SentCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.MailStatistics.SentSize);
                userMailStats.SentSize = (Convert.ToInt32(Misc.ReturnZeroIfNull(row[dt.Columns[columnName]])) / 1024) / 1024; //convert Bytes to MB
            }

            return userMailStats;
        }

        public static MailStatistics GetMailStatistics(string siteName, string departmentName, DateTime date)
        {
            DataTable dt = new DataTable();
            string columnName = string.Empty;
            MailStatistics departmentTotalMailStats = new MailStatistics();
            DateTime startOfThisMonth = DateTime.Now.AddDays(-(DateTime.Today.Day - 1));
            DateTime endOfThisMonth = startOfThisMonth.AddMonths(1).AddDays(-1);
            
            //Initialize the select parameters for the database function
            List<object> selectParameters = new List<object>();
            selectParameters.Add(startOfThisMonth);
            selectParameters.Add(endOfThisMonth);
            selectParameters.Add(departmentName);
            selectParameters.Add(siteName);

            dt = DBRoutines.SELECT_FROM_FUNCTION("fnc_GetMailStatistics_PerDepartment", selectParameters, null);

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