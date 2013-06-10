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

            NotificationAlignConfig AlignCfg = new NotificationAlignConfig
            {
                ElementAnchor = AnchorPoint.TopRight,
                TargetAnchor = AnchorPoint.TopRight,
                OffsetX = -5,
                OffsetY = 35
            };

            notificationConfig.Title = title;
            notificationConfig.Html = msg;

            notificationConfig.AlignCfg = AlignCfg;

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