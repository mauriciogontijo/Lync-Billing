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
        public string PhoneNumber { private set; get; }
        public string UserName { set; get; }
        public long CallsCount { private set; get; }
        public decimal CallsCost { private set; get; }
        public decimal CallsDuration { private set; get; }


        public static List<TopDestinationNumbers> GetTopDestinationNumbers(string sipAccount, int limit)
        {
            DBLib DBRoutines = new DBLib();
            DataTable dt = new DataTable();

            TopDestinationNumbers topDestination;
            List<TopDestinationNumbers> TopDestinationNumbers = new List<TopDestinationNumbers>();
            
            List<object> parameters = new List<object>();
            parameters.Add(sipAccount);
            parameters.Add(limit);

            dt = DBRoutines.SELECT_FROM_FUNCTION("Get_DestinationsNumbers_ForUser", parameters, null);

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
                        topDestination.CallsCount = (int)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationNumbers.CallsDuration))
                        topDestination.CallsDuration = (decimal)row[column.ColumnName];

                    if (column.ColumnName == Enums.GetDescription(Enums.TopDestinationNumbers.CallsCost))
                        topDestination.CallsCost = (decimal)row[column.ColumnName];
                }

                TopDestinationNumbers.Add(topDestination);
            }

            return TopDestinationNumbers;
        }
    }
}