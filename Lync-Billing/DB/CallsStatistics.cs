using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.DB
{
    public class CallsStatistics
    {
    }


    public class TopCountries
    {
        public string CountryName { private set; get; }
        public long TotalDuration { private set; get; }
        public decimal TotalCost { private set; get; }

        public static List<TopCountries> GetTopDestinations(string sipAccount)
        {
            DBLib DBRoutines = new DBLib();
            DataTable dt = new DataTable();

            List<TopCountries> topCountries = new List<TopCountries>();

            List<object> parameters = new List<object>();

            parameters.Add(sipAccount);

            TopCountries topCountry;

            dt = DBRoutines.SELECT_FROM_FUNCTION("fnc_GetTop5DestinationCountriesByCost", parameters, null);

            foreach (DataRow row in dt.Rows)
            {
                topCountry = new TopCountries();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == "Country_Name")
                        topCountry.CountryName = (string)row[column.ColumnName];

                    if (column.ColumnName == "TotalDuration")
                        topCountry.TotalDuration = (int)row[column.ColumnName];

                    if (column.ColumnName == "TotalCost")
                        topCountry.TotalCost = (decimal)row[column.ColumnName];
                }
                topCountries.Add(topCountry);
            }

            return topCountries;
        }
    }

    public class TopDestinations 
    {
        public string PhoneNumber { private set; get; }
        public string Internal { private set; get; }
        public long NumberOfPhoneCalls { private set; get; }

        public static List<TopDestinations> GetTopDestinations(string sipAccount)
        {
            DBLib DBRoutines = new DBLib();
            DataTable dt = new DataTable();

            List<TopDestinations> topDestinations = new List<TopDestinations>();
            
            List<object> parameters = new List<object>();

            parameters.Add(sipAccount);

            TopDestinations topDestination;

            dt = DBRoutines.SELECT_FROM_FUNCTION("fnc_GetTop5DestinationNumbersByCount", parameters, null);

            foreach (DataRow row in dt.Rows)
            {
                topDestination = new TopDestinations();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == "PhoneNumber")
                        topDestination.PhoneNumber = (string)row[column.ColumnName];

                    if (column.ColumnName == "Internal")
                        topDestination.Internal = (string)row[column.ColumnName];

                    if (column.ColumnName == "NumberOfPhoneCalls")
                        topDestination.NumberOfPhoneCalls = (int)row[column.ColumnName];
                }
                topDestinations.Add(topDestination);
            }

            return topDestinations;
        }
    }
}
