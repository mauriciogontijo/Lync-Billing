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
        public enum PDF
        {
            [Description("Session Time")]
            SessionIdTime,
            [Description("Session Seq")]
            SessionIdSeq,
            [Description("Response Time")]
            ResponseTime,
            [Description("Session End Time")]
            SessionEndTime,
            [Description("Email Address")]
            SourceUserUri,
            [Description("Telephone No")]
            SourceNumberUri,
            [Description("Destination No")]
            DestinationNumberUri,
            [Description("Duration")]
            Duration,
            [Description("marker_CallToCountry")]
            Marker_CallToCountry,
            [Description("Marker Call Type")]
            Marker_CallType,
            [Description("Cost")]
            Marker_CallCost,
            [Description("Marked On")]
            UI_MarkedOn,
            [Description("Updated By")]
            UI_UpdatedByUser,
            [Description("Call Type")]
            UI_CallType,
            [Description("Dispute Status")]
            AC_DisputeStatus,
            [Description("Dispute Resolved On")]
            AC_DisputeResolvedOn,
            [Description("Is Invoiced")]
            AC_IsInvoiced,
            [Description("Invoice Date")]
            AC_InvoiceDate

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