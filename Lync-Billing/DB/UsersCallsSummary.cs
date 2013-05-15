using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lync_Billing.Libs;
namespace Lync_Billing.DB
{
    public class UsersCallsSummary
    {

        private static 

        int BusinessCallsCount { get; set; }
        int PersonalCallsCount { get; set; }
        int BusinessCallsDuration { get; set; }
        int PersonalCallsDuration { get; set; }
        int BusinessCallsCost { get; set; }
        int PersonalCallsCost { get; set; }
        int UnMarkedCalls { get; set; }
        int NumberOfDisputedCalls { get; set; }

    }
}