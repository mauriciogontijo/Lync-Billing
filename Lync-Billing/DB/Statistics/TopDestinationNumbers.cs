using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Lync_Billing.Libs;


namespace Lync_Billing.DB.Statistics
{
    public class TopDestinationNumbers
    {
        private static DBLib DBRoutines = new DBLib();

        public string PhoneNumber { private set; get; }
        public string UserName { set; get; }
        public long CallsCount { private set; get; }
        public decimal CallsCost { private set; get; }
        public decimal CallsDuration { private set; get; }


        public static List<TopDestinationNumbers> GetTopDestinationNumbers(string sipAccount, int limit, DateTime? startingDate = null, DateTime? endingDate = null)
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_DestinationsNumbers_ForUser);

            TopDestinationNumbers topDestination;
            List<TopDestinationNumbers> TopDestinationNumbers = new List<TopDestinationNumbers>();
            DateTime fromDate, toDate;

            if (startingDate == null || endingDate == null)
            {
                //Both starting date and ending date respectively point to the beginning and ending of this current month.
                //FromDate = DateTime.Now.AddDays(-(DateTime.Today.Day - 1));
                fromDate = new DateTime(DateTime.Now.Year, 1, 1);
                toDate = fromDate.AddYears(1).AddDays(-1);
            }
            else
            {
                //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
                fromDate = (DateTime)startingDate;
                toDate = (DateTime)endingDate;
            }
            
            //Initialize the function parameters and then query the database
            List<object> parameters = new List<object>();
            parameters.Add(sipAccount);
            parameters.Add(fromDate);
            parameters.Add(toDate);
            parameters.Add(limit);

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, parameters, null);


            foreach (DataRow row in dt.Rows)
            {
                topDestination = new TopDestinationNumbers();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationNumbers.PhoneNumber))
                    {
                        if (row[column.ColumnName] == System.DBNull.Value )
                            topDestination.PhoneNumber = "UNKNOWN";
                        else
                            topDestination.PhoneNumber = (string)row[column.ColumnName];
                    }

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationNumbers.CallsCount))
                        topDestination.CallsCount = Convert.ToInt32(Misc.ReturnZeroIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationNumbers.CallsDuration))
                        topDestination.CallsDuration = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[column.ColumnName]));

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationNumbers.CallsCost))
                        topDestination.CallsCost = Convert.ToDecimal(Misc.ReturnZeroIfNull(row[column.ColumnName]));
                }

                TopDestinationNumbers.Add(topDestination);
            }

            return TopDestinationNumbers;
        }
    }
}