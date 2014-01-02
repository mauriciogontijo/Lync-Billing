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
using System.Collections;

namespace Lync_Billing.Backend.Summaries
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
        


        /// <summary>
        /// Given a Site's name, a Department's name, and a year number, return the list of all of department summaries with respect to months.
        /// </summary>
        /// <param name="siteName">The Site's name</param>
        /// <param name="departmentName">The Department's name</param>
        /// <param name="year">The year number of the phone calls summaries</param>
        /// <returns>A list of UserCallsSummary with respect to the months</returns>
        public static List<DepartmentCallsSummary> GetPhoneCallsStatisticsForDepartment(string siteName, string departmentName, DateTime? startingDate = null, DateTime? endingDate = null)
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_CallsSummary_ForSiteDepartment);

            string columnName = string.Empty;
            Dictionary<string, object> whereClause = new Dictionary<string, object>();
            List<object> functionParameters = new List<object>();

            DepartmentCallsSummary departmentSummary;
            List<DepartmentCallsSummary> ListOfDepartmentCallsSummaries = new List<DepartmentCallsSummary>();

            DateTime fromDate, toDate;

            if (startingDate == null || endingDate == null)
            {
                fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                toDate = DateTime.Now;
            }
            else
            {
                //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
                fromDate = (DateTime)startingDate;
                toDate = (DateTime)endingDate;
            }

            //Add the function parameters in a specific order, conceptually: bigger to smaller.
            whereClause.Add(Enums.GetDescription(Enums.PhoneCallSummary.Date), String.Format("BETWEEN '{0}' AND '{1}'", fromDate, toDate));
            functionParameters.Add(siteName);
            functionParameters.Add(departmentName);

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionParameters, null);


            foreach (DataRow row in dt.Rows)
            {
                departmentSummary = new DepartmentCallsSummary();

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.BusinessCallsCount = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.BusinessCallsDuration = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.BusinessCallsCost = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));


                columnName = Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.PersonalCallsCount = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.PersonalCallsDuration = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.PersonalCallsCost = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));


                columnName = Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.UnmarkedCallsCount = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.UnmarkedCallsDuration = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.UnmarkedCallsCost = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));


                columnName = Enums.GetDescription(Enums.PhoneCallSummary.SiteName);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.SiteName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.Month);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.Month = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.Year);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.Year = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[columnName]]));

                columnName = Enums.GetDescription(Enums.PhoneCallSummary.Date);
                if (dt.Columns.Contains(columnName))
                    departmentSummary.MonthDate = Convert.ToDateTime(row[dt.Columns[columnName]]);

                ListOfDepartmentCallsSummaries.Add(departmentSummary);
            }

            //Return only the summaries for specified Year.
            DepartmentComparer comparer = new DepartmentComparer();
            ListOfDepartmentCallsSummaries.Sort(comparer);

            return ListOfDepartmentCallsSummaries;
        }

    }


    public class DepartmentComparer : IComparer<DepartmentCallsSummary>
    {
        public int Compare(DepartmentCallsSummary x, DepartmentCallsSummary y)
        {
            //less than
            if (x.Year < y.Year)
                return -1;
            //equals
            else if (x.Year == y.Year)
            {
                if (x.Month < y.Month)
                    return -1;
                else if (x.Month == y.Month)
                    return 0;
                else
                    return 1;
            }
            //greater than
            else
                return 1;
        }
    }

}