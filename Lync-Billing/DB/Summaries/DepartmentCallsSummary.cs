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

namespace Lync_Billing.DB.Summaries
{
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