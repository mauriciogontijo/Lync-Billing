using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;

namespace Lync_Backend.Helpers
{
    public class PhoneCall
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
        public string Marker_CallToCountry { set; get; }
        
        public string marker_CallType { set; get; }
        public int Marker_CallTypeID { set; get; }
        public long marker_CallFrom { set; get; }
        public long marker_CallTo { set; get; }

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

        private static List<NumberingPlan> numberingPlan = NumberingPlan.GetNumberingPlan();
        private static Dictionary<int, List<Rates>> ratesTables = Rates.GetAllGatewaysRates();
        private static string gatewayTable; 

        private static List<DIDs> dids = DIDs.GetDIDs();
        private static List<CallsTypes> callTypes = CallsTypes.GetCallTypes();

       
        public static PhoneCall SetCallType(PhoneCall thisCall) 
        {
            string srcCountry = string.Empty;
            string dstCountry = string.Empty;
            string srcCallType = string.Empty;
            string dstCallType = string.Empty;

            //Set SourceNumberDialing Prefix
            thisCall.marker_CallFrom = GetDialingPrefixFromNumber(FixNumberType(thisCall.SourceNumberUri),out srcCallType);

            //if DestinationNumberUri is not valid.
            if (string.IsNullOrEmpty(thisCall.DestinationNumberUri) && !string.IsNullOrEmpty(thisCall.DestinationUserUri) && !string.IsNullOrEmpty(thisCall.SourceUserUri))
            {
                thisCall.marker_CallType = "lync-to-lync";
                thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                return thisCall;
            }

            //Set DestinationNumber Dialing Prefix
            thisCall.marker_CallTo = GetDialingPrefixFromNumber(FixNumberType(thisCall.DestinationNumberUri),out dstCallType);


            var findSrcCountry = numberingPlan.Find(item => item.DialingPrefix == thisCall.marker_CallFrom);
            srcCountry = (findSrcCountry != null) ? findSrcCountry.ThreeDigitsCountryCode : "N/A";

           

            var findDstCountry = numberingPlan.Find(item => item.DialingPrefix == thisCall.marker_CallTo);
            dstCountry = (findDstCountry != null) ? findDstCountry.ThreeDigitsCountryCode : "N/A";

            thisCall.Marker_CallToCountry = dstCountry;

            // MARK NATIONAL INTERNATIONAL FIXED/MOBILE
            if (string.IsNullOrEmpty(thisCall.DestinationUserUri) || !Misc.IsValidEmail(thisCall.DestinationUserUri))
            {
                if (srcCountry == dstCountry)
                {
                    if (dstCallType == "fixedline")
                    {
                        thisCall.marker_CallType = "national-fixedline";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                        return thisCall;
                    }
                    else if (dstCallType == "gsm")
                    {
                        thisCall.marker_CallType = "national-mobile";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                        return thisCall;
                    }
                    else 
                    {
                        thisCall.marker_CallType = "national-fixedline";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                        return thisCall;
                    }
                }
                else
                {
                    if (dstCallType == "fixedline")
                    {
                        thisCall.marker_CallType = "international-fixedline";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                        return thisCall;
                    }
                    else if (dstCallType == "gsm")
                    {
                        thisCall.marker_CallType = "international-mobile";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                        return thisCall;
                    }
                }
            }

            return ApplyExceptions(thisCall);
        }

        public static PhoneCall ApplyExceptions(PhoneCall thisCall) 
        {
            //Incoming Call
            if (string.IsNullOrEmpty(thisCall.SourceUserUri) || !Misc.IsValidEmail(thisCall.SourceUserUri))
            {
                thisCall.marker_CallType = "incoming-call";
                thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                return thisCall;
            }

            //Voice mail
            if (thisCall.SourceNumberUri == thisCall.DestinationNumberUri)
            {
                thisCall.marker_CallType = "voice-mail";
                thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                return thisCall;
            }

            //Toll Free

            if (thisCall.DestinationNumberUri.StartsWith("+800") || thisCall.DestinationNumberUri.StartsWith("800"))
            {
                thisCall.marker_CallType = "toll-free";
                thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                return thisCall;
            }


            //CHECK if the Source AND destination is Lync Client
            string srcDIDdsc = string.Empty, dstDIDdsc = string.Empty;

            MatchDID(thisCall.SourceNumberUri, out srcDIDdsc);
            MatchDID(thisCall.DestinationNumberUri, out dstDIDdsc);

            if (srcDIDdsc == dstDIDdsc)
            {
                thisCall.marker_CallType = srcDIDdsc + "-to-" + dstDIDdsc;
                thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "site-to-site").id;

                return thisCall;
            }
            else 
            {
                //Cross Site Call
                thisCall.marker_CallType = srcDIDdsc + "-to-" + dstDIDdsc;
                thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "site-to-site").id;
            }

            //FAIL SAFE for SITE TO SITE CALLS
            if (string.IsNullOrEmpty(thisCall.FromGateway) && string.IsNullOrEmpty(thisCall.ToGateway) && string.IsNullOrEmpty(thisCall.FromMediationServer) && string.IsNullOrEmpty(thisCall.ToMediationServer)) 
            {
                thisCall.marker_CallType = "site-to-site";
                thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                return thisCall;
            }


            return thisCall;
        }

        private static bool MatchDID(string phoneNumber, out string site)
        {
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

        private static long GetDialingPrefixFromNumber(string phoneNumber, out string callType)
        {
            long numberToParse = 0;
            
            if (phoneNumber.Length > 0 && phoneNumber.Length != 6 && phoneNumber.Length != 7) 
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

            callType = null;
          
            long.TryParse(phoneNumber, out numberToParse);
            return numberToParse;
        }

        private static string FixNumberType(string number)
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

    }
}