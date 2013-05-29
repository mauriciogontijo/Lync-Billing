using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class Announcements
    {
        int ID { get; set; }
        string Announcement { get; set; }
        string Role { get; set; }
        DateTime AnnouncementDate { get; set; }

        public static string GetAnnouncemnet(string role)
        {

            return null;
        }
    
    }

    
}