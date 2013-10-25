using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using Lync_Backend.Libs;

namespace Lync_Backend.Helpers
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
        
    }
}
