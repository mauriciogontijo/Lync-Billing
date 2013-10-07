using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class MailStatistics
    {
        public int ID { get; set; }
        public string EmailAddress { get; set; }
        public long ReceivedCount { get; set; }
        public long ReceivedSize { get; set; }
        public long SentCount { get; set; }
        public long SentSize { get; set; }

        public static List<MailStatistics> GetMailStatistics()
        {
            return null;
        }
    }
}