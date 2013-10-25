using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lync_Backend.Helpers
{
    class PhoneCalls2013 : PhoneCalls
    {
        public PhoneCalls SetCallType(PhoneCalls thisCall)
        {

            string srcCountry = string.Empty;
            string dstCountry = string.Empty;
            string srcCallType = string.Empty;
            string dstCallType = string.Empty;
            string srcDIDdsc = string.Empty;
            string dstDIDdsc = string.Empty;

            //Set SourceNumberDialing Prefix
            thisCall.marker_CallFrom = GetDialingPrefixFromNumber(FixNumberType(thisCall.SourceNumberUri), out srcCallType);

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

            MatchDID(thisCall.SourceNumberUri, out srcDIDdsc);
            MatchDID(thisCall.DestinationNumberUri, out dstDIDdsc);

            //Check if the call is lync to lync or site to site or lync call accros the site
            if ((!string.IsNullOrEmpty(srcDIDdsc) && !string.IsNullOrEmpty(dstDIDdsc)))
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

            //Check if the call is went through gateway which means national or international call
            if (!string.IsNullOrEmpty(thisCall.SourceUserUri) &&
                !string.IsNullOrEmpty(thisCall.ToGateway) &&
                !string.IsNullOrEmpty(thisCall.DestinationNumberUri) &&
                thisCall.DestinationNumberUri.StartsWith("+"))
            {

                //HANDLE THE PHONECALLS-EXCEPTIONS HERE
                if (ListOfUserNumbersExceptions.Contains(thisCall.DestinationNumberUri) || ListOfUserUrisExceptions.Contains(thisCall.SourceUserUri))
                {
                    thisCall.marker_CallType = "EXCLUDED";
                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

                    return thisCall;
                }

                if (srcCountry == dstCountry)
                {
                    if (!string.IsNullOrEmpty(dstDIDdsc))
                    {
                        if (!string.IsNullOrEmpty(srcDIDdsc))
                            thisCall.marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                        else
                            thisCall.marker_CallType = "TO-" + dstDIDdsc;

                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "SITE-TO-SITE").id;

                        return thisCall;
                    }

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
                    if (!string.IsNullOrEmpty(dstDIDdsc))
                    {
                        if (!string.IsNullOrEmpty(srcDIDdsc))
                            thisCall.marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                        else
                            thisCall.marker_CallType = "TO-" + dstDIDdsc;

                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "SITE-TO-SITE").id;

                        return thisCall;
                    }

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

            if (!string.IsNullOrEmpty(thisCall.SourceUserUri))
            {
                thisCall.marker_CallType = "N/A";
                thisCall.Marker_CallTypeID = 0;

                return thisCall;
            }

            thisCall.marker_CallType = "N/A";
            thisCall.Marker_CallTypeID = 0;

            return thisCall;
        }

    }
}
