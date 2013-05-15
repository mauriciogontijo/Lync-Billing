using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
namespace Lync_Billing.DB
{
    public class UsersCallsSummary
    {
        private static DBLib DBRoutines = new DBLib();
        private static Dictionary<string, object> wherePart;
        private static List<string> columns;

        int BusinessCallsCount { get; set; }
        int BusinessCallsCost { get; set; }
        int BusinessCallsDuration { get; set; }
        int PersonalCallsCount { get; set; }
        int PersonalCallsDuration { get; set; }
        int PersonalCallsCost { get; set; }
        int UnmarkedCallsCount { get; set; }
        int UnmarkedCallsDuartion { get; set; }
        int UnmarkedCallsCost { get; set; }

        int NumberOfDisputedCalls { get; set; }

        public static UsersCallsSummary(string sipAccount, DateTime startingDate, DateTime endingDate)
        {
            wherePart = new Dictionary<string, object>();
            columns = new List<string>();

            wherePart.Add("SourceUserUri", sipAccount);
            wherePart.Add("marker_CallTypeID", 1);

            columns.Add("SessionIdTime");
            columns.Add("marker_CallToCountry");
            columns.Add("DestinationNumberUri");
            columns.Add("Duration");
            columns.Add("marker_CallCost");
            columns.Add("ui_IsPersonal");
            columns.Add("ui_MarkedOn");
            columns.Add("ui_IsInvoiced");

            if (startingDate != null && endingDate != null)
            {

            }
            else
            {
            }
        }

    }
}