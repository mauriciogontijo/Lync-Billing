using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

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
        private static Dictionary<string, List<Rates>> ratesTables = Rates.GetAllGatewaysRates();


        public static PhoneCall ApplyCallRate(PhoneCall thisCall) 
        {
            string srcCallType = string.Empty;
            string dstCallType = string.Empty;

            decimal duration = Convert.ToDecimal(0);
            decimal callCostPerMin = Convert.ToDecimal(0);
            
          

            //Set SourceNumberDialing Prefix
            thisCall.marker_CallFrom = GetDialingPrefixFromNumber(FixNumberType(thisCall.SourceNumberUri),out srcCallType);

            //Set DestinationNumber Dialing Prefix
            thisCall.marker_CallTo = GetDialingPrefixFromNumber(FixNumberType(thisCall.DestinationNumberUri),out dstCallType);

            //Set Destination Country Name
            thisCall.Marker_CallToCountry = numberingPlan.Find(item => item.DialingPrefix == thisCall.marker_CallTo).ThreeDigitsCountryCode ?? "N/A";

            //Get Duration in Minutes
            duration = Math.Round(thisCall.Duration/60,3);

            //Get Rates Table for the 

            foreach (KeyValuePair<string, List<Rates>> keyValue in ratesTables) 
            {
                if (keyValue.Key.Contains(thisCall.ToGateway)) 
                {
                    Rates callRate = ((List<Rates>)keyValue.Value).Where(item => item.CountryCode == thisCall.Marker_CallToCountry).First();

                    if (callRate != null)
                    {
                        if (dstCallType == "gsm")
                        {
                            callCostPerMin = callRate.MobileLineRate;
                            break;
                        }
                        else
                        {
                            callCostPerMin = callRate.FixedLineRate;
                            break;
                        }
                    }
                    else 
                    {
                        continue;
                    }
                }
            }

            if (callCostPerMin != 0)
                thisCall.Marker_CallCost = duration * callCostPerMin;
            else
                thisCall.Marker_CallCost = 0;

            return thisCall;
        }


        private static long GetDialingPrefixFromNumber(long phoneNumber,out string callType) 
        {
            while (phoneNumber > 0) 
            {
                var number = numberingPlan.Find(item => item.DialingPrefix == phoneNumber);

                if (number != null)
                {
                    
                    callType =number.TypeOfService;
                    return number.DialingPrefix;
                }
                else
                {
                    phoneNumber = phoneNumber / 10;
                    continue;
                }
            }
            
            callType = null;
            
            return phoneNumber;
        }


        private static long FixNumberType(string number) 
        {
            long longNumber = 0;
            number = number.Trim('+');
            long.TryParse(number, out longNumber); ;
            
            return longNumber;
        }

    }
}