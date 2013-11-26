using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Interfaces;
using Lync_Backend.Helpers;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Lync_Backend.Implementation
{
    class PhoneCalls2013 : PhoneCalls,IPhoneCalls
    {
        public PhoneCalls2013() { }
        /***
            * List of Chargeable Phonecalls Types include:
            * 1 = LOCAL PHONECALL
            * 2 = NATIONAL-FIXEDLINE
            * 3 = NATIONAL-MOBILE
            * 4 = INTERNATIONAL-FIXEDLINE
            * 5 = INTERNATIONAL-MOBILE
            * 21 = FIXEDLINE
            * 22 = MOBILE
            */
        private static BillableCallTypesSection section = (BillableCallTypesSection)ConfigurationManager.GetSection("BillableCallTypesSection");

        //Get Billibale types from App.config
        private static  List<int> ListofChargeableCallTypes = section.BillableTypesList;
        private static List<int> ListOfFixedLinesIDs = section.FixedlinesIdsList;
        private static List<int> ListOfMobileLinesIDs = section.MobileLinesIdsList;

        //Get Gateways for that Marker
        private static List<Gateways> ListofGateways = Gateways.GetGateways();

        //Get Gateway IDs from Gateways
        private List<string> ListofGatewaysNames = ListofGateways.Select(item => item.GatewayName).ToList<string>();

        //Get Rates for those Gateways for that marker
        private Dictionary<int, List<Rates>> ratesPerGatway = Rates.GetAllGatewaysRatesList();

        public PhoneCalls SetCallType(PhoneCalls thisCall)
        {

            string srcCountry = string.Empty;
            string dstCountry = string.Empty;
            string srcCallType = string.Empty;
            string dstCallType = string.Empty;
            string srcDIDdsc = string.Empty;
            string dstDIDdsc = string.Empty;

            //Set SourceNumberDialing Prefix
            thisCall.Marker_CallFrom = GetDialingPrefixFromNumber(FixNumberType(thisCall.SourceNumberUri), out srcCallType);

            //Set DestinationNumber Dialing Prefix
            thisCall.Marker_CallTo = GetDialingPrefixFromNumber(FixNumberType(thisCall.DestinationNumberUri), out dstCallType);

            //Set Source Country
            var findSrcCountry = numberingPlan.Find(item => item.DialingPrefix == thisCall.Marker_CallFrom);
            srcCountry = (findSrcCountry != null) ? findSrcCountry.ThreeDigitsCountryCode : "N/A";

            //Set Destination Country
            var findDstCountry = numberingPlan.Find(item => item.DialingPrefix == thisCall.Marker_CallTo);
            dstCountry = (findDstCountry != null) ? findDstCountry.ThreeDigitsCountryCode : "N/A";

            thisCall.Marker_CallToCountry = dstCountry;


            //Test Case
            if (thisCall.SessionIdTime == "2013-10-03 10:17:57.240" || thisCall.SessionIdTime == "2013-10-03 11:18:58.650" || thisCall.SessionIdTime == "2013-10-22 06:51:25.777" || thisCall.SessionIdTime == "2013-10-23 12:52:04.993") 
            {
                string x= string.Empty;
            }

            //Incoming Call
            if (string.IsNullOrEmpty(thisCall.SourceUserUri) || !Misc.IsValidEmail(thisCall.SourceUserUri))
            {
                thisCall.Marker_CallType = "INCOMING-CALL";
                thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.Marker_CallType).id;

                return thisCall;
            }

            //Voice Mail
            if (thisCall.SourceUserUri == thisCall.DestinationUserUri || thisCall.SourceNumberUri == thisCall.DestinationNumberUri)
            {
                thisCall.Marker_CallType = "VOICE-MAIL";
                thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.Marker_CallType).id;

                return thisCall;
            }

            //CHECK if the Source AND destination is Lync Client

            MatchDID(thisCall.SourceNumberUri, out srcDIDdsc);
            MatchDID(thisCall.DestinationNumberUri, out dstDIDdsc);

            //Check if the call is lync to lync or site to site or lync call accros the site
            if (!string.IsNullOrEmpty(dstDIDdsc))
            {
                //TODO:  IF source number uri is null check if the user site could be resolved from activedirectoryUsers table
                //       IF yes put the source site from activedirectoryUsers table instead of the soyrce did site
               
                if (dstDIDdsc == "TOLL-FREE")
                {
                    if(!string.IsNullOrEmpty(srcDIDdsc))
                        thisCall.Marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                    else
                        thisCall.Marker_CallType = "UNKNOWN-TO-" + dstDIDdsc;
                    
                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "TOLL-FREE").id;

                    return thisCall;
                }
                else if (dstDIDdsc == "PUSH-TO-TALK-UAE")
                {
                    if (!string.IsNullOrEmpty(srcDIDdsc))
                        thisCall.Marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                    else
                        thisCall.Marker_CallType = "UNKNOWN-TO-" + dstDIDdsc;

                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "PUSH-TO-TALK").id;

                    return thisCall;
                }
                else
                {
                    if (!string.IsNullOrEmpty(srcDIDdsc))
                        thisCall.Marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                    else
                        thisCall.Marker_CallType = "UNKNOWN-TO-" + dstDIDdsc;

                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "SITE-TO-SITE").id;

                    return thisCall;
                }
            }

            //FAIL SAFE for LYNC TO LYNC CALLS
            if (string.IsNullOrEmpty(thisCall.FromGateway) && string.IsNullOrEmpty(thisCall.ToGateway) && string.IsNullOrEmpty(thisCall.FromMediationServer) && string.IsNullOrEmpty(thisCall.ToMediationServer))
            {
                thisCall.Marker_CallType = "LYNC-TO-LYNC";
                thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.Marker_CallType).id;

                return thisCall;
            }

            //Check if the call is went through gateway which means national or international call
            // To Gateway or to mediation server should be set to be able to be able to consider this call as external
            // There is a bug some calls went ththrough pst but the gateways is null (NADER TO INVISTAGATE) : we could apply default rates for those calls
            if (!string.IsNullOrEmpty(thisCall.SourceUserUri) &&
                (!string.IsNullOrEmpty(thisCall.ToGateway) || !string.IsNullOrEmpty(thisCall.ToMediationServer)) &&
                !string.IsNullOrEmpty(thisCall.DestinationNumberUri) &&
                thisCall.DestinationNumberUri.StartsWith("+"))
            {

                //HANDLE THE PHONECALLS-EXCEPTIONS HERE
                if (ListOfUserNumbersExceptions.Contains(thisCall.DestinationNumberUri) || ListOfUserUrisExceptions.Contains(thisCall.SourceUserUri))
                {
                    thisCall.Marker_CallType = "EXCLUDED";
                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.Marker_CallType).id;

                    return thisCall;
                }

                if (srcCountry == dstCountry)
                {
                    if (!string.IsNullOrEmpty(dstDIDdsc))
                    {
                        if (!string.IsNullOrEmpty(srcDIDdsc))
                            thisCall.Marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                        else
                            thisCall.Marker_CallType = "TO-" + dstDIDdsc;

                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "SITE-TO-SITE").id;

                        return thisCall;
                    }

                    if (dstCallType == "fixedline")
                    {
                        thisCall.Marker_CallType = "NATIONAL-FIXEDLINE";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.Marker_CallType).id;

                        if (thisCall.CalleeURI == thisCall.DestinationNumberUri || thisCall.CalleeURI == null)
                        {
                            return thisCall;
                        }
                        else if (Regex.IsMatch(thisCall.CalleeURI, @"\d{1,}@\w{1,}.*"))
                        {
                            if (Regex.Replace(thisCall.CalleeURI, @"@\w{1,}.*", "") == thisCall.DestinationNumberUri)
                            {
                                return thisCall;
                            }
                            else
                            {
                                thisCall.ChargingParty = thisCall.CalleeURI;
                                return thisCall;
                            }
                        }
                        return thisCall;
                    }
                    else if (dstCallType == "gsm")
                    {
                        thisCall.Marker_CallType = "NATIONAL-MOBILE";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.Marker_CallType).id;

                        if (thisCall.CalleeURI == thisCall.DestinationNumberUri || thisCall.CalleeURI == null)
                        {
                            return thisCall;
                        }
                        else if (Regex.IsMatch(thisCall.CalleeURI, @"\d{1,}@\w{1,}.*"))
                        {
                            if (Regex.Replace(thisCall.CalleeURI, @"@\w{1,}.*", "") == thisCall.DestinationNumberUri)
                            {
                                return thisCall;
                            }
                            else
                            {
                                thisCall.ChargingParty = thisCall.CalleeURI;
                                return thisCall;
                            }
                        }
                        return thisCall;
                    }
                    else
                    {
                        thisCall.Marker_CallType = "NATIONAL-FIXEDLINE";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.Marker_CallType).id;

                        if (thisCall.CalleeURI == thisCall.DestinationNumberUri || thisCall.CalleeURI == null)
                        {
                            return thisCall;
                        }
                        else if (Regex.IsMatch(thisCall.CalleeURI, @"\d{1,}@\w{1,}.*"))
                        {
                            if (Regex.Replace(thisCall.CalleeURI, @"@\w{1,}.*", "") == thisCall.DestinationNumberUri)
                            {
                                return thisCall;
                            }
                            else
                            {
                                thisCall.ChargingParty = thisCall.CalleeURI;
                                return thisCall;
                            }
                        }
                        return thisCall;
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(dstDIDdsc))
                    {
                        if (!string.IsNullOrEmpty(srcDIDdsc))
                            thisCall.Marker_CallType = srcDIDdsc + "-TO-" + dstDIDdsc;
                        else
                            thisCall.Marker_CallType = "TO-" + dstDIDdsc;

                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == "SITE-TO-SITE").id;

                        return thisCall;
                    }

                    if (dstCallType == "fixedline")
                    {
                        thisCall.Marker_CallType = "INTERNATIONAL-FIXEDLINE";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.Marker_CallType).id;

                        if (thisCall.CalleeURI == thisCall.DestinationNumberUri || thisCall.CalleeURI == null)
                        {
                            return thisCall;
                        }
                        else if (Regex.IsMatch(thisCall.CalleeURI, @"\d{1,}@\w{1,}.*"))
                        {
                            if (Regex.Replace(thisCall.CalleeURI, @"@\w{1,}.*", "") == thisCall.DestinationNumberUri)
                            {
                                return thisCall;
                            }
                            else
                            {
                                thisCall.ChargingParty = thisCall.CalleeURI;
                                return thisCall;
                            }
                        }
                        return thisCall;
                    }
                    else if (dstCallType == "gsm")
                    {
                        thisCall.Marker_CallType = "INTERNATIONAL-MOBILE";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.Marker_CallType).id;

                        if (thisCall.CalleeURI == thisCall.DestinationNumberUri || thisCall.CalleeURI == null)
                        {
                            return thisCall;
                        }
                        else if (Regex.IsMatch(thisCall.CalleeURI, @"\d{1,}@\w{1,}.*"))
                        {
                            if (Regex.Replace(thisCall.CalleeURI, @"@\w{1,}.*", "") == thisCall.DestinationNumberUri)
                            {
                                return thisCall;
                            }
                            else
                            {
                                thisCall.ChargingParty = thisCall.CalleeURI;
                                return thisCall;
                            }
                        }
                        return thisCall;
                    }
                    else
                    {
                        thisCall.Marker_CallType = "INTERNATIONAL-FIXEDLINE";
                        thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.Marker_CallType).id;

                        if (thisCall.CalleeURI == thisCall.DestinationNumberUri || thisCall.CalleeURI == null)
                        {
                            return thisCall;
                        }
                        else if (Regex.IsMatch(thisCall.CalleeURI, @"\d{1,}@\w{1,}.*"))
                        {
                            if (Regex.Replace(thisCall.CalleeURI, @"@\w{1,}.*", "") == thisCall.DestinationNumberUri)
                            {
                                return thisCall;
                            }
                            else
                            {
                                thisCall.ChargingParty = thisCall.CalleeURI;
                                return thisCall;
                            }
                        }
                        return thisCall;
                    }
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
                    thisCall.Marker_CallType = "LYNC-TO-IM";
                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.Marker_CallType).id;
                }
                else
                {
                    thisCall.Marker_CallType = "LYNC-TO-LYNC";
                    thisCall.Marker_CallTypeID = callTypes.Find(type => type.CallType == thisCall.Marker_CallType).id;
                }

                return thisCall;
            }

            thisCall.Marker_CallType = "N/A";
            thisCall.Marker_CallTypeID = 0;

            return thisCall;
        }

        public PhoneCalls ApplyRate(PhoneCalls thisCall)
        {

            string srcDIDdsc = string.Empty;
            bool status = false;

            MatchDID(thisCall.SourceNumberUri, out srcDIDdsc);

            status = CallsExceptions.ApplyMOAExceptions(ref thisCall, srcDIDdsc);

            if (status == true)
                return thisCall;

            if (!string.IsNullOrEmpty(thisCall.ToGateway))
            {
                // Check if we can apply the rates for this phone-call
                var gateway = ListofGateways.Find(g => g.GatewayName == thisCall.ToGateway.ToString());
                var rates = (from keyValuePair in ratesPerGatway where keyValuePair.Key == gateway.GatewayId select keyValuePair.Value).SingleOrDefault<List<Rates>>() ?? (new List<Rates>());

                if (rates.Count > 0 && ListofChargeableCallTypes.Contains(thisCall.Marker_CallTypeID))
                {
                    //Apply the rate for this phone call
                    var rate = (from r in rates
                                where r.CountryCode == thisCall.Marker_CallToCountry
                                select r).SingleOrDefault<Rates>();

                    //if the call is of type national/international MOBILE then apply the Mobile-Rate, otherwise apply the Fixedline-Rate
                    if (rate != null)
                    {
                        if (ListOfFixedLinesIDs.Contains(thisCall.Marker_CallTypeID))
                        {
                            thisCall.Marker_CallCost = Math.Ceiling(Convert.ToDecimal(thisCall.Duration) / 60) * rate.FixedLineRate;
                        }
                        else if (ListOfMobileLinesIDs.Contains(thisCall.Marker_CallTypeID))
                        {
                            thisCall.Marker_CallCost = Math.Ceiling(Convert.ToDecimal(thisCall.Duration) / 60) * rate.MobileLineRate;
                        }
                    }

                }
            }
            return thisCall;
        }
    }
}
