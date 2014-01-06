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
                notificationConfig.Icon = Icon.Exclamation;
            else if (type == "help")
                notificationConfig.Icon = Icon.Help;

            //Pinning
            if (isPinned)
            {
                notificationConfig.ShowPin = true;
                notificationConfig.Pinned = true;
                notificationConfig.PinEvent = "click";
            }

            notificationConfig.BodyStyle = "background-color: #f9f9f9;";

            Notification.Show(notificationConfig);
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

        public static object ReturnFalseIfNull(object value)
        {
            if (value == System.DBNull.Value)
                return false;
            else
                return value;
        }

        public static object ReturnDateTimeMinIfNull(object value)
        {
            if (value == System.DBNull.Value)
                return DateTime.MinValue;
            else
                return value;
        }


        //This function formats the display-name of a user,
        //and removes unnecessary extra information.
        public static string FormatUserDisplayName(string displayName = null, string defaultValue = "tBill Users", bool returnNameIfExists = false, bool returnAddressPartIfExists = false)
        {
            //Get the first part of the Users's Display Name if s/he has a name like this: "firstname lastname (extra text)"
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

    }

}