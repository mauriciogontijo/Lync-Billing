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
            wherePart.Add("startingDate", startingDate);
            wherePart.Add("endingDate", endingDate);

            DBRoutines.SELECT_USER_STATISTICS(Enums.GetDescription(Enums.PhoneCalls.TableName), wherePart);
        }

    }
}