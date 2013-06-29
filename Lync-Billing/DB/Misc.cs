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
            notificationConfig.HideDelay = 5000;

            if (type == "success") 
                notificationConfig.Icon = Icon.Accept;
            else if(type == "info")
                notificationConfig.Icon = Icon.Information;
            else if (type == "warning")
                notificationConfig.Icon = Icon.AsteriskYellow;
            else if (type == "error")
                notificationConfig.Icon = Icon.Error;

            Notification.Show(notificationConfig);
        }

        public static string ConvertSecondsToReadable(int secondsParam) 
        {
            int hours = Convert.ToInt32(Math.Floor((double)(secondsParam / 3600)));
            int minutes = Convert.ToInt32(Math.Floor((double)(secondsParam - (hours * 3600)) / 60));
            int seconds = secondsParam - (hours * 3600) - (minutes * 60);

            string hours_str = hours.ToString();
            string mins_str = minutes.ToString();
            string secs_str = seconds.ToString();

            if (hours < 10)
            {
                hours_str = "0" + hours_str;
            }

            if (minutes < 10)
            {
                mins_str = "0" + mins_str;
            }
            if (seconds < 10)
            {
                secs_str = "0" + secs_str;
            }

            return hours_str + ':' + mins_str + ':' + secs_str;
        }
    }

    
}