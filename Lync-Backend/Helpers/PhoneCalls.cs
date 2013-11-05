using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;

namespace Lync_Backend.Helpers
{
    public class PhoneCalls
    {
        public string SessionIdTime { set; get; }
        public int SessionIdSeq { get; set; }
        public string ResponseTime { set; get; }
        public string SessionEndTime { set; get; }
        public string SourceUserUri { set; get; }
        public string SourceNumberUri { set; get; }
        public string DestinationNumberUri { set; get; }
        public string DestinationUserUri { get; set; }
        public string FromMediationServer { set; get; }
        public string ToMediationServer { set; get; }
        public string FromGateway { set; get; }
        public string ToGateway { set; get; }
        public string SourceUserEdgeServer { set; get; }
        public string DestinationUserEdgeServer { set; get; }
        public string ServerFQDN { set; get; }
        public string PoolFQDN { set; get; }
       
        public long Marker_CallFrom { set; get; }
        public long Marker_CallTo { set; get; }
        public string Marker_CallToCountry { set; get; }
        public int Marker_CallTypeID { set; get; }
        public string Marker_CallType { set; get; }
       
        public decimal Duration { set; get; }
        public decimal Marker_CallCost { set; get; }

        //User UI update Fields
        public string UI_UpdatedByUser { set; get; }
        public DateTime UI_MarkedOn { set; get; }
        public string UI_CallType { set; get; }
        public string AC_DisputeStatus { set; get; }
        public DateTime AC_DisputeResolvedOn { set; get; }
        public string AC_IsInvoiced { set; get; }
        public DateTime AC_InvoiceDate { set; get; }

        protected static List<NumberingPlan> numberingPlan = NumberingPlan.GetNumberingPlan();

        protected static List<DIDs> dids = DIDs.GetDIDs();

        protected static List<CallsTypes> callTypes = CallsTypes.GetCallTypes();

        protected static List<string> ListOfUserUrisExceptions = PhoneCallsExceptions.GetUsersUris();
        protected static List<string> ListOfUserNumbersExceptions = PhoneCallsExceptions.GetUsersNumbers();

        protected bool MatchDID(string phoneNumber, out string site)
        {
            if (string.IsNullOrEmpty(phoneNumber)) 
            {
                site = string.Empty;
                return false;
            }

            foreach (DIDs didEntry in dids)
            {
                string did = didEntry.did;

                if (Regex.IsMatch(phoneNumber.Trim('+'), @"^"+did))
                {
                    site = didEntry.description;
                    return true;
                }
                else
                {
                    continue;
                }
            }

            site = string.Empty;
            return false;
        }

        protected long GetDialingPrefixFromNumber(string phoneNumber, out string callType)
        {
            long numberToParse = 0;
            
            if (phoneNumber.Length >= 9 ) 
            {
                long.TryParse(phoneNumber, out numberToParse);

                while (numberToParse > 0)
                {
                    var number = numberingPlan.Find(item => item.DialingPrefix == numberToParse);

                    if (number != null)
                    {

                        callType = number.TypeOfService;
                        return number.DialingPrefix;
                    }
                    else
                    {
                        numberToParse = numberToParse / 10;
                        continue;
                    }
                }
            }

            callType = "N/A";
          
            long.TryParse(phoneNumber, out numberToParse);
            return 0;
        }

        protected string FixNumberType(string number)
        {

            if (string.IsNullOrEmpty(number))
                return "N/A";

            if (number.Contains(";")) 
            {
                number = number.Split(';')[0].ToString();
            }

            number = number.Trim('+');


            return number;
        }

        public static void ApplyRates(DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null)
        {
            DateTime from = optionalFrom != null ? optionalFrom.Value : DateTime.MinValue;
            DateTime to = optionalTo != null ? optionalTo.Value : DateTime.MaxValue;

            if (string.IsNullOrEmpty(gateway))
            {
                
            }
            else 
            {

            }
        }

        public static void MarkCalls(DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null)
        {
            DateTime from = optionalFrom != null ? optionalFrom.Value : DateTime.MinValue;
            DateTime to = optionalTo != null ? optionalTo.Value : DateTime.MaxValue;

            if (string.IsNullOrEmpty(gateway))
            {

            }
            else
            {

            }
        }

    }
}