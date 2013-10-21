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

        private static List<DIDs> dids = DIDs.GetDIDs();
        
        private static List<CallsTypes> callTypes = CallsTypes.GetCallTypes();
       
        private static List<string> ListOfUserUrisExceptions = PhoneCallsExceptions.GetUsersUris();
        private static List<string> ListOfUserNumbersExceptions = PhoneCallsExceptions.GetUsersNumbers();

        
        public static PhoneCall SetCallType(PhoneCall thisCall) 
        {
            string srcCountry = string.Empty;
            string dstCountry = string.Empty;
            string srcCallType = string.Empty;
            string dstCallType = string.Empty;

           

            if (!string.IsNullOrEmpty(thisCall.DestinationNumberUri) && thisCall.DestinationNumberUri.StartsWith("+3069") )
            {
                string x = string.Empty;
            }

            if (!string.IsNullOrEmpty(thisCall.DestinationNumberUri) &&  thisCall.DestinationNumberUri.StartsWith("80")) 
            {
                string x = string.Empty;
            }

            //Set SourceNumberDialing Prefix
            thisCall.marker_CallFrom = GetDialingPrefixFromNumber(FixNumberType(thisCall.SourceNumberUri),out srcCallType);

            //Set DestinationNumber Dialing Prefix
            thisCall.marker_CallTo = GetDialingPrefixFromNumber(FixNumberType(thisCall.DestinationNumberUri), out dstCallType);

            //Set Source Country
            var findSrcCountry = numberingPlan.Find(item => item.DialingPrefix == thisCall.marker_CallFrom);
            srcCountry = (findSrcCountry != null) ? findSrcCountry.ThreeDigitsCountryCode : "N/A";

            //Set Destination Country
            var findDstCountry = numberingPlan.Find(item => item.DialingPrefix == thisCall.marker_CallTo);
            dstCountry = (findDstCountry != null) ? findDstCountry.ThreeDigitsCountryCode : "N/A";

            thisCall.Marker_CallToCountry = dstCountry;

            //Incoming Call
            if (string.IsNullOrEmpty(thisCall.SourceUserUri) || !Misc.IsValidEmail(thisCall.SourceUserUri))
            {
                thisCall.marker_CallType = "INCOMING-CALL";
                thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                return thisCall;
            } 

            //Voice Mail
            if (thisCall.SourceUserUri == thisCall.DestinationUserUri || thisCall.SourceNumberUri == thisCall.DestinationNumberUri)
            {
                thisCall.marker_CallType = "VOICE-MAIL";
                thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                return thisCall;
            }

            //CHECK if the Source AND destination is Lync Client
            string srcDIDdsc = string.Empty, dstDIDdsc = string.Empty;

            MatchDID(thisCall.SourceNumberUri, out srcDIDdsc);
            MatchDID(thisCall.DestinationNumberUri, out dstDIDdsc);

            if (!string.IsNullOrEmpty(srcDIDdsc) && !string.IsNullOrEmpty(dstDIDdsc))
            {
                if (dstDIDdsc == "TOLL-FREE") 
                {
                    thisCall.marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "TOLL-FREE").id;

                    return thisCall;
                }
                else if (dstDIDdsc == "PUSH-TO-TALK-UAE")
                {
                    thisCall.marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "PUSH-TO-TALK").id;

                    return thisCall;
                }
                else
                {
                    thisCall.marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "SITE-TO-SITE").id;

                    return thisCall;
                }
            }

            //FAIL SAFE for LYNC TO LYNC CALLS
            if (string.IsNullOrEmpty(thisCall.FromGateway) && string.IsNullOrEmpty(thisCall.ToGateway) && string.IsNullOrEmpty(thisCall.FromMediationServer) && string.IsNullOrEmpty(thisCall.ToMediationServer))
            {
                thisCall.marker_CallType = "LYNC-TO-LYNC";
                thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                return thisCall;
            }


            // MARK NATIONAL INTERNATIONAL FIXED/MOBILE
            if (!string.IsNullOrEmpty(thisCall.ToGateway) && !string.IsNullOrEmpty(thisCall.DestinationNumberUri) && thisCall.DestinationNumberUri.StartsWith("+"))
            {
                //HANDLE THE PHONECALLS-EXCEPTIONS HERE
                if (ListOfUserNumbersExceptions.Contains(thisCall.DestinationNumberUri) || ListOfUserUrisExceptions.Contains(thisCall.SourceUserUri))
                {
                    thisCall.marker_CallType = "EXCLUDED";
                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                    return thisCall;
                }

                // CHECK FOR THE SOURCE AND DESTINATION COUNTRIES
                if (srcCountry == dstCountry)
                {
                    if (dstCallType == "fixedline")
                    {
                        thisCall.marker_CallType = "NATIONAL-FIXEDLINE";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                        return thisCall;
                    }
                    else if (dstCallType == "gsm")
                    {
                        thisCall.marker_CallType = "NATIONAL-MOBILE";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                        return thisCall;
                    }
                    else
                    {
                        thisCall.marker_CallType = "NATIONAL-FIXEDLINE";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                        return thisCall;
                    }
                }

                else
                {
                    if (dstCallType == "fixedline")
                    {
                        thisCall.marker_CallType = "INTERNATIONAL-FIXEDLINE";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                        return thisCall;
                    }
                    else if (dstCallType == "gsm")
                    {
                        thisCall.marker_CallType = "INTERNATIONAL-MOBILE";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                        return thisCall;
                    }
                    else
                    {
                        thisCall.marker_CallType = "INTERNATIONAL-FIXEDLINE";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                        return thisCall;
                    }
                }
            }

            //LYNC 2013
            if (Misc.IsValidEmail(thisCall.SourceUserUri) && !string.IsNullOrEmpty(thisCall.DestinationNumberUri)) 
            {
                if (!string.IsNullOrEmpty(dstDIDdsc))
                {
                    if (dstDIDdsc == "TOLL-FREE")
                    {
                        thisCall.marker_CallType = dstDIDdsc;
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "TOLL-FREE").id;

                        return thisCall;
                    }
                    else if (dstDIDdsc == "PUSH-TO-TALK")
                    {
                        thisCall.marker_CallType = dstDIDdsc;
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "PUSH-TO-TALK").id;

                        return thisCall;
                    }
                    else
                    {
                        thisCall.marker_CallType = "TO-" + dstDIDdsc;
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "SITE-TO-SITE").id;

                        return thisCall;
                    }
                }

                if (dstCallType == "fixedline")
                {
                    thisCall.marker_CallType = "FIXEDLINE";
                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                    return thisCall;
                }
                else if (dstCallType == "gsm")
                {
                    thisCall.marker_CallType = "MOBILE";
                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                    return thisCall;
                }
                else
                {
                    thisCall.marker_CallType = "FIXEDLINE";
                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                    return thisCall;
                }
            }

            //Lync2013

            if( !string.IsNullOrEmpty(thisCall.SourceUserUri))

            thisCall.marker_CallType = "N/A";
            thisCall.Marker_CallTypeID = 0;

            return thisCall;
        }   

        private static bool MatchDID(string phoneNumber, out string site)
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

        private static long GetDialingPrefixFromNumber(string phoneNumber, out string callType)
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