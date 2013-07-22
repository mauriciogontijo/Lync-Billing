using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;

namespace Lync_Billing.DB
{
    public static class PDFDefinitions
    {
        public static Dictionary<string, string> Definitions = new Dictionary<string, string>()
        {
            {"SessionIdTime", "Session Time"},
            {"SessionIdSeq", "Session Seq"},
            {"ResponseTime", "Response Time"},
            {"SessionEndTime", "Session End Time"},
            {"SourceUserUri", "Email Address"},
            {"SourceNumberUri", "Telephone No"},
            {"DestinationNumberUri", "Destination No"},
            {"Duration", "Duration"},
            {"Marker_CallToCountry", "marker_CallToCountry"},
            {"Marker_CallType", "Marker Call Type"},
            {"Marker_CallCost", "Cost"},
            {"UI_MarkedOn", "Marked On"},
            {"UI_UpdatedByUser", "Updated By"},
            {"UI_CallType", "Call Type"},
            {"AC_DisputeStatus", "Dispute Status"},
            {"AC_DisputeResolvedOn", "Dispute Resolved On"},
            {"AC_IsInvoiced", "Is Invoiced"},
            {"AC_InvoiceDate", "Invoice Date"}
        };

        public static string GetDescription(string value)
        {
            if (!string.IsNullOrEmpty(value) && Definitions.Keys.Contains(value))
            {
                return Definitions[value];
            }
            else
            {
                return value;
            }
        }

        //public static string GetDescription(Enum value)
        //{
        //    FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

        //    DescriptionAttribute[] descAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

        //    if (descAttributes != null && descAttributes.Length > 0)
        //        return descAttributes[0].Description;
        //    else
        //        return value.ToString();
        //}

        //public static T GetDescription<T>(string description)
        //{
        //    MemberInfo[] fis = typeof(T).GetFields();

        //    foreach (var fi in fis)
        //    {
        //        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        //        if (attributes != null && attributes.Length > 0 && attributes[0].Description == description)
        //            return (T)Enum.Parse(typeof(T), fi.Name);
        //    }

        //    throw new Exception("Not found");
        //}


    }
}