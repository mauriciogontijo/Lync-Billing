using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Interfaces;

namespace Lync_Backend.Helpers
{
    class PhoneCalls2010 : PhoneCalls,IPhoneCalls
    {
        public PhoneCalls SetCallType(PhoneCalls thisCall)
        {
            string srcCountry = string.Empty;
            string dstCountry = string.Empty;
            string srcCallType = string.Empty;
            string dstCallType = string.Empty;
            string srcDIDdsc = string.Empty; 
            string dstDIDdsc = string.Empty;

            if (thisCall.SessionIdTime == "2013-05-31 05:18:50.653")
            {
                string x = string.Empty;
            }

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

            MatchDID(thisCall.SourceNumberUri, out srcDIDdsc);
            MatchDID(thisCall.DestinationNumberUri, out dstDIDdsc);

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
           
            //if ((!string.IsNullOrEmpty(srcDIDdsc) && !string.IsNullOrEmpty(dstDIDdsc)))
            //{
            //    if (dstDIDdsc == "TOLL-FREE")
            //    {
            //        thisCall.marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
            //        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "TOLL-FREE").id;

            //        return thisCall;
            //    }
            //    else if (dstDIDdsc == "PUSH-TO-TALK-UAE")
            //    {
            //        thisCall.marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
            //        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "PUSH-TO-TALK").id;

            //        return thisCall;
            //    }
            //    else
            //    {
            //        thisCall.marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
            //        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "SITE-TO-SITE").id;

            //        return thisCall;
            //    }
            //}

             if (!string.IsNullOrEmpty(dstDIDdsc))
             {
                //TODO:  IF source number uri is null check if the user site could be resolved from activedirectoryUsers table
                //       IF yes put the source site from activedirectoryUsers table instead of the soyrce did site
               
                if (dstDIDdsc == "TOLL-FREE")
                {
                    if(!string.IsNullOrEmpty(srcDIDdsc))
                        thisCall.marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                    else
                        thisCall.marker_CallType = "UNKNOWN-TO-" + dstDIDdsc;
                    
                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "TOLL-FREE").id;

                    return thisCall;
                }
                else if (dstDIDdsc == "PUSH-TO-TALK-UAE")
                {
                    if (!string.IsNullOrEmpty(srcDIDdsc))
                        thisCall.marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                    else
                        thisCall.marker_CallType = "UNKNOWN-TO-" + dstDIDdsc;

                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "PUSH-TO-TALK").id;

                    return thisCall;
                }
                else
                {
                    if (!string.IsNullOrEmpty(srcDIDdsc))
                        thisCall.marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                    else
                        thisCall.marker_CallType = "UNKNOWN-TO-" + dstDIDdsc;

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
            //if (!string.IsNullOrEmpty(thisCall.ToGateway) && 
            //    !string.IsNullOrEmpty(thisCall.DestinationNumberUri) && 
            //    thisCall.DestinationNumberUri.StartsWith("+"))
            //{
            //    //HANDLE THE PHONECALLS-EXCEPTIONS HERE
            //    if (ListOfUserNumbersExceptions.Contains(thisCall.DestinationNumberUri) || ListOfUserUrisExceptions.Contains(thisCall.SourceUserUri))
            //    {
            //        thisCall.marker_CallType = "EXCLUDED";
            //        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

            //        return thisCall;
            //    }

            //    // CHECK FOR THE SOURCE AND DESTINATION COUNTRIES
            //    if (srcCountry == dstCountry)
            //    {
            //        if (!string.IsNullOrEmpty(dstDIDdsc))
            //        {
            //            thisCall.marker_CallType = "TO-" + dstDIDdsc;
            //            thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "SITE-TO-SITE").id;

            //            return thisCall;
            //        }

            //        if (dstCallType == "fixedline")
            //        {
            //            thisCall.marker_CallType = "NATIONAL-FIXEDLINE";
            //            thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

            //            return thisCall;
            //        }
            //        else if (dstCallType == "gsm")
            //        {
            //            thisCall.marker_CallType = "NATIONAL-MOBILE";
            //            thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

            //            return thisCall;
            //        }
            //        else
            //        {
            //            thisCall.marker_CallType = "NATIONAL-FIXEDLINE";
            //            thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

            //            return thisCall;
            //        }
            //    }

            //    else
            //    {
            //        if (!string.IsNullOrEmpty(dstDIDdsc))
            //        {
            //            thisCall.marker_CallType = "TO-" + dstDIDdsc;
            //            thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "SITE-TO-SITE").id;

            //            return thisCall;
            //        }

            //        if (dstCallType == "fixedline")
            //        {
            //            thisCall.marker_CallType = "INTERNATIONAL-FIXEDLINE";
            //            thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

            //            return thisCall;
            //        }
            //        else if (dstCallType == "gsm")
            //        {
            //            thisCall.marker_CallType = "INTERNATIONAL-MOBILE";
            //            thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

            //            return thisCall;
            //        }
            //        else
            //        {
            //            thisCall.marker_CallType = "INTERNATIONAL-FIXEDLINE";
            //            thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;

            //            return thisCall;
            //        }
            //    }
            //}

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
            

           
            // Handle the sourceNumber uri sip account and the destination uri is a sip account also 
            // For the calls which doesnt have source number uri or destination number uri to bable to identify which site
            if (!string.IsNullOrEmpty(thisCall.SourceUserUri) && 
                !string.IsNullOrEmpty(thisCall.DestinationUserUri) && 
                Misc.IsValidEmail(thisCall.DestinationUserUri)) 
            {
                if (Misc.IsIMEmail(thisCall.DestinationUserUri))
                {
                    thisCall.marker_CallType = "LYNC-TO-IM";
                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;
                }
                else
                {
                    thisCall.marker_CallType = "LYNC-TO-LYNC";
                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.marker_CallType).id;
                }

                return thisCall;
            }

            thisCall.marker_CallType = "N/A";
            thisCall.Marker_CallTypeID = 0;

            return thisCall;

        }

    }
}
