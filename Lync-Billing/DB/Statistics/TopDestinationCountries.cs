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
        public string CountryName { private set; get; }
        public int CallsCount { private set; get; }
        public decimal CallsCost { private set; get; }
        public decimal CallsDuration { private set; get; }


        public static List<TopDestinationCountries> GetTopDestinationNumbersForUser(string sipAccount, int limit)
        {
            DBLib DBRoutines = new DBLib();
            DataTable dt = new DataTable();

            TopDestinationCountries topCountry;
            List<TopDestinationCountries> TopDestinationCountries = new List<TopDestinationCountries>();

            List<object> parameters = new List<object>();
            parameters.Add(sipAccount);
            parameters.Add(limit);

            dt = DBRoutines.SELECT_FROM_FUNCTION("Get_DestinationCountries_ForUser", parameters, null);

            foreach (DataRow row in dt.Rows)
            {
                topCountry = new TopDestinationCountries();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationNumbers.CountryName))
                        topCountry.CountryName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationNumbers.CallsCount))
                        topCountry.CallsDuration = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationNumbers.CallsDuration))
                        topCountry.CallsDuration = (decimal)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationNumbers.CallsCost))
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
            DBLib DBRoutines = new DBLib();
            DataTable dt = new DataTable();

            TopDestinationCountries topCountry;
            List<TopDestinationCountries> TopDestinationCountries = new List<TopDestinationCountries>();

            List<object> parameters = new List<object>();
            parameters.Add(siteName);
            parameters.Add(departmentName);
            parameters.Add(limit);


            dt = DBRoutines.SELECT_FROM_FUNCTION("Get_DestinationCountries_ForSiteDepartment", parameters, null);

            foreach (DataRow row in dt.Rows)
            {
                topCountry = new TopDestinationCountries();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationNumbers.CountryName))
                        topCountry.CountryName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationNumbers.CallsCount))
                        topCountry.CallsDuration = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationNumbers.CallsDuration))
                        topCountry.CallsDuration = (decimal)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationNumbers.CallsCost))
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