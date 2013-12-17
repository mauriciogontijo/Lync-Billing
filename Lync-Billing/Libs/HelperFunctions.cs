using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using Ext.Net;

namespace Lync_Billing.Libs
{
    public class HelperFunctions
    {
        public static void Message(string title, string msg, string type, int hideDelay = 15000, bool isPinned = false, int width = 250, int height = 150)
        {
            NotificationConfig notificationConfig = new NotificationConfig();

            notificationConfig.Title = title;
            notificationConfig.Html = msg;
            
            //Hiding Delay in mlseconds
            notificationConfig.HideDelay = hideDelay;
            
            //Height and Width
            notificationConfig.Width = width;
            notificationConfig.Height = height;

            //Type
            if (type == "success")
                notificationConfig.Icon = Icon.Accept;
            else if (type == "info")
                notificationConfig.Icon = Icon.Information;
            else if (type == "warning")
                notificationConfig.Icon = Icon.AsteriskYellow;
            else if (type == "error")
                notificationConfig.Icon = Icon.Error;
            else if (type == "help")
                notificationConfig.Icon = Icon.Help;

            //Pinning
            if (isPinned)
            {
                notificationConfig.ShowPin = true;
                notificationConfig.Pinned = true;
                notificationConfig.PinEvent = "click";
            }

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

        public static bool GetResolvedConnecionIPAddress(string serverNameOrURL, out string resolvedIPAddress)
        {
            bool isResolved = false;
            IPHostEntry hostEntry = null;
            IPAddress resolvIP = null;
            try
            {
                if (!IPAddress.TryParse(serverNameOrURL, out resolvIP))
                {
                    hostEntry = Dns.GetHostEntry(serverNameOrURL);

                    if (hostEntry != null && hostEntry.AddressList != null
                                 && hostEntry.AddressList.Length > 0)
                    {
                        if (hostEntry.AddressList.Length == 1)
                        {
                            resolvIP = hostEntry.AddressList[0];
                            isResolved = true;
                        }
                        else
                        {
                            foreach (IPAddress var in hostEntry.AddressList)
                            {
                                if (var.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    resolvIP = var;
                                    isResolved = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    isResolved = true;
                }
            }
            catch (Exception ex)
            {
                isResolved = false;
                resolvIP = null;
            }
            finally
            {
                resolvedIPAddress = resolvIP.ToString();
            }

            return isResolved;
        }

        public static string SerializeObject<T>(T source)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var sw = new System.IO.StringWriter())
            using (var writer = new XmlTextWriter(sw))
            {
                serializer.Serialize(writer, source);
                return sw.ToString();
            }
        }

        public static T DeSerializeObject<T>(string xml)
        {
            using (System.IO.StringReader sr = new System.IO.StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(sr);
            }
        }

        public static object ReturnZeroIfNull(object value)
        {
            if (value == System.DBNull.Value)
                return 0;
            else
                return value;
        }


        public static object ReturnEmptyIfNull(object value)
        {
            if (value == System.DBNull.Value)
                return string.Empty;
            else
                return value;
        }


        //This function formats the display-name of a user,
        //and removes unnecessary extra information.
        public static string FormatUserDisplayName(string displayName = null, string defaultValue = "tBill User", bool returnNameIfExists = false, bool returnAddressPartIfExists = false)
        {
            //Get the first part of the User's Display Name if s/he has a name like this: "firstname lastname (extra text)"
            //removes the "(extra text)" part
            if (!string.IsNullOrEmpty(displayName))
            {
                if (returnNameIfExists == true)
                    return Regex.Replace(displayName, @"\ \(\w{1,}\)", "");
                else
                    return (displayName.Split(' '))[0];
            }
            else
            {
                if (returnAddressPartIfExists == true)
                {
                    var emailParts = defaultValue.Split('@');
                    return emailParts[0];
                }
                else
                    return defaultValue;
            }
        }


        public static string FormatUserTelephoneNumber(string telephoneNumber)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(telephoneNumber))
            {
                result = telephoneNumber.Replace("tel:", "");
            }

            return result;
        }

        public static string ConvertDate(DateTime datetTime)
        {
            if (datetTime != DateTime.MinValue || datetTime != null)
                return datetTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            else
                return null;
        }
      

    }

}