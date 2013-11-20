using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Lync_Billing.Libs;


namespace Lync_Billing.DB.Statistics
{
    public class TopDestinationCountries
    {
        private static DBLib DBRoutines = new DBLib();

        public string CountryName { private set; get; }
        public int CallsCount { private set; get; }
        public decimal CallsCost { private set; get; }
        public decimal CallsDuration { private set; get; }


        public static List<TopDestinationCountries> GetTopDestinationCountriesForUser(string sipAccount, int limit)
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_DestinationCountries_ForUser);

            TopDestinationCountries topCountry;
            List<TopDestinationCountries> TopDestinationCountries = new List<TopDestinationCountries>();

            //Initialize the database function parameters and then send the query to the database.
            List<object> parameters = new List<object>();
            parameters.Add(sipAccount);
            parameters.Add(limit);

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, parameters, null);


            foreach (DataRow row in dt.Rows)
            {
                topCountry = new TopDestinationCountries();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.Country))
                        topCountry.CountryName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.CallsCount))
                        topCountry.CallsCount = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.CallsDuration))
                        topCountry.CallsDuration = (decimal)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.CallsCost))
                    {
                        if (row[column.ColumnName] != System.DBNull.Value)
                            topCountry.CallsCost = (decimal)row[column.ColumnName];
                        else
                            topCountry.CallsCost = 0;
                    }
                }
                TopDestinationCountries.Add(topCountry);
            }

            return TopDestinationCountries;
        }

        public static List<TopDestinationCountries> GetTopDestinationNumbersForDepartment(string siteName, string departmentName, int limit)
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_DestinationCountries_ForSiteDepartment);

            TopDestinationCountries topCountry;
            List<TopDestinationCountries> TopDestinationCountries = new List<TopDestinationCountries>();

            //Initialize the database function and then query the database
            List<object> parameters = new List<object>();
            parameters.Add(siteName);
            parameters.Add(departmentName);
            parameters.Add(limit);

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, parameters, null);


            foreach (DataRow row in dt.Rows)
            {
                topCountry = new TopDestinationCountries();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.Country))
                        topCountry.CountryName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.CallsCount))
                        topCountry.CallsDuration = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.CallsDuration))
                        topCountry.CallsDuration = (decimal)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationCountries.CallsCost))
                    {
                        if (row[column.ColumnName] != System.DBNull.Value)
                            topCountry.CallsCost = (decimal)row[column.ColumnName];
                        else
                            topCountry.CallsCost = 0;
                    }
                }
                TopDestinationCountries.Add(topCountry);
            }

            return TopDestinationCountries;
        }
    }

}