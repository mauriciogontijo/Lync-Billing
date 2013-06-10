using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net;

namespace Lync_Billing.DB
{
    public class Misc
    {
        public static void Message(string title, string msg, string type)
        {
            NotificationConfig notificationConfig = new NotificationConfig();

            notificationConfig.Title = title;
            notificationConfig.Html = msg;

            if (type == "success") 
                notificationConfig.Icon = Icon.Accept;
            else if(type == "info")
                notificationConfig.Icon = Icon.Information;
            else if (type == "warning")
            {

                notificationConfig.Icon = Icon.AsteriskYellow;
            }
            else if (type == "error")
            {
                notificationConfig.Icon = Icon.Error;
            }



                        

            Notification.Show(notificationConfig);
        }
    }
}