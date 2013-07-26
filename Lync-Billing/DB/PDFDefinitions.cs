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
            {"ResponseTime", "Date"},
            {"SessionEndTime", "Session End Time"},
            {"SourceUserUri", "Email Address"},
            {"SourceNumberUri", "Telephone No"},
            //{"DestinationNumberUri", "Destination Number"},
            {"DestinationNumberUri", "Destination"},
            {"Duration", "Duration"},
            {"marker_CallToCountry", "Country"},
            {"marker_CallType", "Marker Call Type"},
            {"marker_CallCost", "Cost"},
            {"ui_MarkedOn", "Marked On"},
            {"ui_UpdatedByUser", "Updated By"},
            {"ui_CallType", "Call Type"},
            {"ac_DisputeStatus", "Dispute Status"},
            {"ac_DisputeResolvedOn", "Dispute Resolved On"},
            {"ac_IsInvoiced", "Is Invoiced"},
            {"ac_InvoiceDate", "Invoice Date"},
            {"AD_UserID", "Group No."},
            {"AD_DisplayName", "Name"},
            {"BusinessCost", "Bus. Cost"},
            {"PersonalCost", "Per. Cost"},
            {"UnMarkedCost", "Unac. Cost"}
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

    }
}