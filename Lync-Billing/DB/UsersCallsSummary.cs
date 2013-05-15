using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;
namespace Lync_Billing.DB
{
    public class UsersCallsSummary
    {
        private static DBLib DBRoutines = new DBLib();
        private static Dictionary<string, object> wherePart;
        private static List<string> columns;

        int BusinessCallsCount { get; set; }
        int BusinessCallsCost { get; set; }
        int BusinessCallsDuration { get; set; }
        int PersonalCallsCount { get; set; }
        int PersonalCallsDuration { get; set; }
        int PersonalCallsCost { get; set; }
        int UnmarkedCallsCount { get; set; }
        int UnmarkedCallsDuartion { get; set; }
        int UnmarkedCallsCost { get; set; }

        int NumberOfDisputedCalls { get; set; }

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
                if(row[dt.Columns["PhoneCallType"]].ToString() == "NO")
                {
                    if (row[dt.Columns["ui_IsPersonal"]] != System.DBNull.Value)
                        userSummary.BusinessCallsCount = (int)row[dt.Columns["ui_IsPersonal"]];
                    else
                        userSummary.BusinessCallsCount = 0;

                    if (row[dt.Columns["TotalCost"]] != System.DBNull.Value)
                        userSummary.BusinessCallsCost = (int)row[dt.Columns["TotalCost"]];
                    else
                        userSummary.BusinessCallsCost = 0;

                    if (row[dt.Columns["TotalDuration"]] != System.DBNull.Value)
                        userSummary.BusinessCallsDuration = (int)row[dt.Columns["TotalDuration"]];
                    else
                        userSummary.BusinessCallsDuration = 0;
                }

                else if (row[dt.Columns["PhoneCallType"]].ToString() == "YES")
                {
                    if (row[dt.Columns["ui_IsPersonal"]] != System.DBNull.Value)
                        userSummary.PersonalCallsCount = (int)row[dt.Columns["ui_IsPersonal"]];
                    else
                        userSummary.PersonalCallsCount = 0;

                    if (row[dt.Columns["TotalCost"]] != System.DBNull.Value)
                        userSummary.PersonalCallsCost = (int)row[dt.Columns["TotalCost"]];
                    else
                        userSummary.PersonalCallsCost = 0;

                    if (row[dt.Columns["TotalDuration"]] != System.DBNull.Value)
                        userSummary.PersonalCallsDuration = (int)row[dt.Columns["TotalDuration"]];
                    else
                        userSummary.PersonalCallsDuration = 0;
                }

                else if (row[dt.Columns["PhoneCallType"]] == System.DBNull.Value)
                {
                    if (row[dt.Columns["ui_IsPersonal"]] != System.DBNull.Value)
                        userSummary.UnmarkedCallsCount = (int)row[dt.Columns["ui_IsPersonal"]];
                    else
                        userSummary.UnmarkedCallsCount = 0;

                    if (row[dt.Columns["TotalCost"]] != System.DBNull.Value)
                        userSummary.UnmarkedCallsCost = (int)row[dt.Columns["TotalCost"]];
                    else
                        userSummary.UnmarkedCallsCost = 0;

                    if (row[dt.Columns["TotalDuration"]] != System.DBNull.Value)
                        userSummary.UnmarkedCallsDuartion = (int)row[dt.Columns["TotalDuration"]];
                    else
                        userSummary.UnmarkedCallsDuartion = 0;
                }
            }
            return userSummary;
        }

    }
}