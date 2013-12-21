using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
using System.Data;

namespace Lync_Billing.Backend
{
    public class NumberingPlan
    {
        public Int64 DialingPrefix { get; set; }
        public string CountryName { get; set; }
        public string TwoDigitsCountryCode { get; set; }
        public string ThreeDigitsCountryCode { get; set; }
        public string City { get; set; }
        public string Provider { get; set; }
        public string TypeOfService { get; set; }

        private static DBLib DBRoutines = new DBLib();



        public static List<NumberingPlan> GetNumberingPlan() 
        {
            List<NumberingPlan> numberingPlan = new List<NumberingPlan>();
            DataTable dt = new DataTable();

            NumberingPlan numberingPlanRow;

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.NumberingPlan.TableName));

            foreach (DataRow row in dt.Rows)
            {
                numberingPlanRow = new NumberingPlan();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.DialingPrefix) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.DialingPrefix = (Int64)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.CountryName) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.CountryName = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.TwoDigitsCountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.TwoDigitsCountryCode = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.ThreeDigitsCountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.ThreeDigitsCountryCode = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.City) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.City = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.Provider) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.Provider = (string)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.TypeOfService) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.TypeOfService = (string)row[column.ColumnName];

                }

                numberingPlan.Add(numberingPlanRow);
            }

            return numberingPlan;
        }


        public static List<NumberingPlan> GetNumberingPlan(Int64 dilaingPrefix)
        {
            List<NumberingPlan> numberingPlan = new List<NumberingPlan>();
            DataTable dt = new DataTable();

            NumberingPlan numberingPlanRow;

           
            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.NumberingPlan.TableName), Enums.GetDescription(Enums.NumberingPlan.DialingPrefix), dilaingPrefix);

            foreach (DataRow row in dt.Rows)
            {
                numberingPlanRow = new NumberingPlan();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.DialingPrefix) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.DialingPrefix = (Int64)row[column.ColumnName];

                    else if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.CountryName) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.CountryName = (string)row[column.ColumnName];

                    else if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.TwoDigitsCountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.TwoDigitsCountryCode = (string)row[column.ColumnName];

                    else if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.ThreeDigitsCountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.ThreeDigitsCountryCode = (string)row[column.ColumnName];

                    else if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.City) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.City = (string)row[column.ColumnName];

                    else if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.Provider) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.Provider = (string)row[column.ColumnName];

                    else if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.TypeOfService) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.TypeOfService = (string)row[column.ColumnName];

                }

                numberingPlan.Add(numberingPlanRow);
            }

            return numberingPlan;
        }


        public static List<NumberingPlan> GetNumberingPlan(string threeDigitsCountryCode)
        {
            List<NumberingPlan> numberingPlan = new List<NumberingPlan>();
            DataTable dt = new DataTable();

            NumberingPlan numberingPlanRow;


            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.NumberingPlan.TableName), Enums.GetDescription(Enums.NumberingPlan.ThreeDigitsCountryCode), threeDigitsCountryCode);

            foreach (DataRow row in dt.Rows)
            {
                numberingPlanRow = new NumberingPlan();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.DialingPrefix) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.DialingPrefix = (Int64)row[column.ColumnName];

                    else if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.CountryName) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.CountryName = (string)row[column.ColumnName];

                    else if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.TwoDigitsCountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.TwoDigitsCountryCode = (string)row[column.ColumnName];

                    else if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.ThreeDigitsCountryCode) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.ThreeDigitsCountryCode = (string)row[column.ColumnName];

                    else if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.City) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.City = (string)row[column.ColumnName];

                    else if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.Provider) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.Provider = (string)row[column.ColumnName];

                    else if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.TypeOfService) && row[column.ColumnName] != System.DBNull.Value)
                        numberingPlanRow.TypeOfService = (string)row[column.ColumnName];
                }

                numberingPlan.Add(numberingPlanRow);
            }

            return numberingPlan;
        }


        public static string GetCountryName(string TwoCharactersCountryCode = null, string ThreeCharactersCountryCode = null)
        {
            DataTable dt = new DataTable();
            string countryName = string.Empty;

            List<string> columns = new List<string>();
            Dictionary<string, object> whrerPart = new Dictionary<string, object>();

            columns.Add(Enums.GetDescription(Enums.NumberingPlan.CountryName));
            
            if(!string.IsNullOrEmpty(TwoCharactersCountryCode) && string.IsNullOrEmpty(ThreeCharactersCountryCode))
            {
                whrerPart.Add(Enums.GetDescription(Enums.NumberingPlan.TwoDigitsCountryCode), TwoCharactersCountryCode);
            }
            else if(string.IsNullOrEmpty(TwoCharactersCountryCode) && !string.IsNullOrEmpty(ThreeCharactersCountryCode))
            {
                whrerPart.Add(Enums.GetDescription(Enums.NumberingPlan.ThreeDigitsCountryCode), ThreeCharactersCountryCode);
            }


            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.NumberingPlan.TableName), columns, whrerPart, 1);


            if(dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                var columnName = Enums.GetDescription(Enums.NumberingPlan.CountryName);
                countryName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[columnName]));
            }
            
            return countryName;
        }


        public static List<NumberingPlan> GetAllCountries()
        {
            DataTable dt = new DataTable();
            NumberingPlan country;
            List<NumberingPlan> allCountries = new List<NumberingPlan>();

            List<string> columns = new List<string>()
            {
                String.Format("DISTINCT({0})", Enums.GetDescription(Enums.NumberingPlan.ThreeDigitsCountryCode)),
                Enums.GetDescription(Enums.NumberingPlan.TwoDigitsCountryCode),
                Enums.GetDescription(Enums.NumberingPlan.CountryName)
            };

            Dictionary<string, object> whereClause = new Dictionary<string, object>();

            dt = DBRoutines.SELECT(Enums.GetDescription(Enums.NumberingPlan.TableName), columns, whereClause, 0);


            foreach (DataRow row in dt.Rows)
            {
                country = new NumberingPlan();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.CountryName))
                        country.CountryName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.TwoDigitsCountryCode))
                        country.TwoDigitsCountryCode = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));

                    else if (column.ColumnName == Enums.GetDescription(Enums.NumberingPlan.ThreeDigitsCountryCode))
                        country.ThreeDigitsCountryCode = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[column.ColumnName]));
                }

                allCountries.Add(country);
            }

            return allCountries;
        }
    }
}