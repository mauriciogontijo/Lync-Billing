using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Lync_Billing.Libs;


namespace Lync_Billing.Backend.Statistics
{
    public class TopDestinationCountries
    {
        private static DBLib DBRoutines = new DBLib();

        public string CountryName { private set; get; }
        public int CallsCount { private set; get; }
        public decimal CallsCost { private set; get; }
        public decimal CallsDuration { private set; get; }


        public static List<TopDestinationCountries> GetTopDestinationCountriesForUser(string sipAccount, int limit, DateTime? startingDate = null, DateTime? endingDate = null)
        {
            DataTable dt = new DataTable();
            string columnName = string.Empty;
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_DestinationCountries_ForUser);

            TopDestinationCountries topCountry;
            List<TopDestinationCountries> TopDestinationCountries = new List<TopDestinationCountries>();
            DateTime fromDate, toDate;

            if (startingDate == null || endingDate == null)
            {
                //Both starting date and ending date respectively point to the beginning and ending of this current month.
                //FromDate = DateTime.Now.AddDays(-(DateTime.Today.Day - 1));
                
                //fromDate = new DateTime(DateTime.Now.Year, 1, 1);
                //toDate = fromDate.AddYears(1).AddDays(-1);

                fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                toDate = DateTime.Now;
            }
            else
            {
                //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
                fromDate = (DateTime)startingDate;
                toDate = (DateTime)endingDate;
            }

            //Initialize the database function parameters and then send the query to the database.
            List<object> parameters = new List<object>();
            parameters.Add(sipAccount);
            parameters.Add(fromDate);
            parameters.Add(toDate);
            parameters.Add(limit);

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, parameters, null);


            foreach (DataRow row in dt.Rows)
            {
                topCountry = new TopDestinationCountries();

                //Exclude countries with no names!
                columnName = Enums.GetDescription(Enums.TopDestinationCountries.Country);
                if (row[columnName] == DBNull.Value)
                    continue;

                //Process countries!
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.Country))
                        topCountry.CountryName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.CallsCount))
                        topCountry.CallsCount = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.CallsDuration))
                        topCountry.CallsDuration = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.CallsCost))
                            topCountry.CallsCost = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));
                }

                TopDestinationCountries.Add(topCountry);
            }


            //This handles the case in which the date is at the beginning of the year, and no data was recorded yet.
            if (dt.Rows.Count == 0)
            {
                topCountry = new TopDestinationCountries();
                topCountry.CountryName = "N/A";

                TopDestinationCountries.Add(topCountry);
            }


            return TopDestinationCountries;
        }

        public static List<TopDestinationCountries> GetTopDestinationCountriesForDepartment(string siteName, string departmentName, int limit, DateTime? startingDate = null, DateTime? endingDate = null)
        {
            DataTable dt = new DataTable();
            string columnName = string.Empty;
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_DestinationCountries_ForSiteDepartment);

            TopDestinationCountries topCountry;
            List<TopDestinationCountries> TopDestinationCountries = new List<TopDestinationCountries>();
            DateTime fromDate, toDate;

            if (startingDate == null || endingDate == null)
            {
                //Both starting date and ending date respectively point to the beginning and ending of this current month.
                //FromDate = DateTime.Now.AddDays(-(DateTime.Today.Day - 1));
                
                //fromDate = new DateTime(DateTime.Now.Year, 1, 1);
                //toDate = fromDate.AddYears(1).AddDays(-1);

                fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                toDate = DateTime.Now;
            }
            else
            {
                //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
                fromDate = (DateTime)startingDate;
                toDate = (DateTime)endingDate;
            }

            //Initialize the database function and then query the database
            List<object> parameters = new List<object>();
            parameters.Add(siteName);
            parameters.Add(departmentName);
            parameters.Add(fromDate);
            parameters.Add(toDate);
            parameters.Add(limit);

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, parameters, null);


            foreach (DataRow row in dt.Rows)
            {
                topCountry = new TopDestinationCountries();

                //Exclude countries with no names!
                columnName = Enums.GetDescription(Enums.TopDestinationCountries.Country);
                if (row[columnName] == DBNull.Value)
                    continue;

                //Process countries!
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.Country))
                        topCountry.CountryName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.CallsCount))
                        topCountry.CallsCount = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.CallsDuration))
                        topCountry.CallsDuration = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.CallsCost))
                        topCountry.CallsCost = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[column.ColumnName]));
                }

                TopDestinationCountries.Add(topCountry);
            }


            //This handles the case in which the date is at the beginning of the year, and no data was recorded yet.
            if (dt.Rows.Count == 0)
            {
                topCountry = new TopDestinationCountries();
                topCountry.CountryName = "N/A";

                TopDestinationCountries.Add(topCountry);
            }


            return TopDestinationCountries;
        }
    }

}