using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Lync_Billing.Backend;

namespace Lync_Billing.Libs
{
    public class SpecialDateTime
    {
        private DBLib DBRoutines = new DBLib();

        
        public static string ConvertDate(DateTime datetTime)
        {
            if (datetTime != DateTime.MinValue || datetTime != null)
                return datetTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            else
                return null;
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