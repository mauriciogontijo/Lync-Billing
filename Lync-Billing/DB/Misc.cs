using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net;

namespace Lync_Billing.DB
{
    public class Misc
    {
        public static void Message(string msg)
        {
            Notification.Show(new NotificationConfig
            {
                Title = "Title",
                Icon = Icon.Information,
                Html = msg
            });
        }
    }
}