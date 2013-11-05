using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lync_Backend.Helpers
{
    public class Misc
    {
        public static string ConvertDate(DateTime datetTime)
        {
            if (datetTime != DateTime.MinValue || datetTime != null)
                return datetTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            else
                return null;
        }
      
        /***
         * This converts a PhoneCall object to a dictionary.
         */
        public static Dictionary<string, object> ConvertPhoneCallToDictionary(PhoneCalls phoneCall)
        {
            Dictionary<string, object> phoneCallDict = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(phoneCall.SessionIdTime))
                phoneCallDict.Add("SessionIdTime", phoneCall.SessionIdTime);

            if (phoneCall.SessionIdSeq != null)
                phoneCallDict.Add("SessionIdSeq", phoneCall.SessionIdSeq);

            if (!string.IsNullOrEmpty(phoneCall.ResponseTime))
                phoneCallDict.Add("ResponseTime", phoneCall.ResponseTime);

            if (!string.IsNullOrEmpty(phoneCall.SessionEndTime))
                phoneCallDict.Add("SessionEndTime", phoneCall.SessionEndTime);

            if (!string.IsNullOrEmpty(phoneCall.SourceUserUri))
                phoneCallDict.Add("SourceUserUri", phoneCall.SourceUserUri);

            if (!string.IsNullOrEmpty(phoneCall.SourceNumberUri))
                phoneCallDict.Add("SourceNumberUri", phoneCall.SourceNumberUri);

            if (!string.IsNullOrEmpty(phoneCall.DestinationNumberUri))
                phoneCallDict.Add("DestinationNumberUri", phoneCall.DestinationNumberUri);

            if (!string.IsNullOrEmpty(phoneCall.FromMediationServer))
                phoneCallDict.Add("FromMediationServer", phoneCall.FromMediationServer);

            if (!string.IsNullOrEmpty(phoneCall.ToMediationServer))
                phoneCallDict.Add("ToMediationServer", phoneCall.ToMediationServer);

            if (!string.IsNullOrEmpty(phoneCall.FromGateway))
                phoneCallDict.Add("FromGateway", phoneCall.FromGateway);

            if (!string.IsNullOrEmpty(phoneCall.ToGateway))
                phoneCallDict.Add("ToGateway", phoneCall.ToGateway);

            if (!string.IsNullOrEmpty(phoneCall.SourceUserEdgeServer))
                phoneCallDict.Add("SourceUserEdgeServer", phoneCall.SourceUserEdgeServer);

            if (!string.IsNullOrEmpty(phoneCall.DestinationUserEdgeServer))
                phoneCallDict.Add("DestinationUserEdgeServer", phoneCall.DestinationUserEdgeServer);

            if (!string.IsNullOrEmpty(phoneCall.ServerFQDN))
                phoneCallDict.Add("ServerFQDN", phoneCall.ServerFQDN);

            if (!string.IsNullOrEmpty(phoneCall.PoolFQDN))
                phoneCallDict.Add("PoolFQDN", phoneCall.PoolFQDN);

            if (!string.IsNullOrEmpty(phoneCall.Marker_CallToCountry))
                phoneCallDict.Add("marker_CallToCountry", phoneCall.Marker_CallToCountry);

            if (!string.IsNullOrEmpty(phoneCall.Marker_CallType))
                phoneCallDict.Add("marker_CallType", phoneCall.Marker_CallType);

            phoneCallDict.Add("marker_CallTypeID", phoneCall.Marker_CallTypeID);
            phoneCallDict.Add("marker_CallCost", phoneCall.Marker_CallCost);
            phoneCallDict.Add("marker_CallFrom", phoneCall.Marker_CallFrom);
            phoneCallDict.Add("marker_CallTo", phoneCall.Marker_CallTo);
            phoneCallDict.Add("Duration", phoneCall.Duration);

            return phoneCallDict;
        }

        /***
         * This is used in the CallMarker classes, to fill the a dictionary version of the phonecall object from an OleDatabase DataReader.
         */
        public static Dictionary<string, object> FillDictionaryFromOleDataReader(OleDbDataReader dataReader)
        {
            string column = string.Empty;
            Dictionary<string, object> phoneCallDictionary = new Dictionary<string, object>();

            column = Enums.GetDescription(Enums.PhoneCalls.SessionIdTime);
            phoneCallDictionary.Add(column, Misc.ConvertDate((DateTime)dataReader[column]));

            column = Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq);
            phoneCallDictionary.Add(column, (int)dataReader[column]);

            column = Enums.GetDescription(Enums.PhoneCalls.ResponseTime);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, Misc.ConvertDate((DateTime)dataReader[column]));

            column = Enums.GetDescription(Enums.PhoneCalls.SessionEndTime);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, dataReader[column].ToString());

            column = Enums.GetDescription(Enums.PhoneCalls.SourceUserUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, dataReader[column].ToString());

            column = Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, dataReader[column].ToString());

            column = Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, dataReader[column].ToString());

            column = Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, dataReader[column].ToString());

            column = Enums.GetDescription(Enums.PhoneCalls.FromMediationServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, dataReader[column].ToString());

            column = Enums.GetDescription(Enums.PhoneCalls.ToMediationServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, dataReader[column].ToString());

            column = Enums.GetDescription(Enums.PhoneCalls.FromGateway);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, dataReader[column].ToString());

            column = Enums.GetDescription(Enums.PhoneCalls.ToGateway);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, dataReader[column].ToString());

            column = Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, dataReader[column].ToString());

            column = Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, dataReader[column].ToString());

            column = Enums.GetDescription(Enums.PhoneCalls.ServerFQDN);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, dataReader[column].ToString());

            column = Enums.GetDescription(Enums.PhoneCalls.PoolFQDN);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, dataReader[column].ToString());

            column = Enums.GetDescription(Enums.PhoneCalls.Duration);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, Convert.ToDecimal(dataReader[column]));

            column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, dataReader[column].ToString());
            
            column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, Convert.ToInt32(dataReader[column]));

            column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallType);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCallDictionary.Add(column, dataReader[column].ToString());

            
            return phoneCallDictionary;
        }
        
        /***
         * This is used in the CallMarker classes, to fill the PhoneCall objects from the database reader.
         */
        public static PhoneCalls FillPhoneCallFromOleDataReader(OleDbDataReader dataReader)
        {
            string column = string.Empty;
            PhoneCalls phoneCall = new PhoneCalls();


            //Start filling the PhoneCall object
            phoneCall.SessionIdTime = Misc.ConvertDate((DateTime)dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionIdTime)]);
            phoneCall.SessionIdSeq = (int)dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq)];

            column = Enums.GetDescription(Enums.PhoneCalls.ResponseTime);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.ResponseTime = Misc.ConvertDate((DateTime)dataReader[column]);

            column = Enums.GetDescription(Enums.PhoneCalls.SessionEndTime);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.SessionEndTime = Misc.ConvertDate((DateTime)dataReader[column]);

            column = Enums.GetDescription(Enums.PhoneCalls.SourceUserUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.SourceUserUri = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.SourceNumberUri = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.DestinationUserUri = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.DestinationNumberUri = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.FromMediationServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.FromMediationServer = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.ToMediationServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.ToMediationServer = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.FromGateway);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.FromGateway = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.ToGateway);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.ToGateway = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.SourceUserEdgeServer = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.DestinationUserEdgeServer = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.ServerFQDN);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.ServerFQDN = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.PoolFQDN);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.PoolFQDN = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.Duration);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.Duration = Convert.ToDecimal(dataReader[column]);

            column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallFrom);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.Marker_CallFrom = Convert.ToInt64(dataReader[column]);

            column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallTo);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.Marker_CallTo = Convert.ToInt64(dataReader[column]);

            column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.Marker_CallToCountry = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.Marker_CallTypeID = Convert.ToInt32(dataReader[column]);

            column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.Marker_CallCost = Convert.ToInt32(dataReader[column]);

            column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallType);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.Marker_CallType = dataReader[column].ToString();


            //Return teh filled object
            return phoneCall;
        }


        public static bool IsValidEmail(string emailAddress)
        {
            string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            return Regex.IsMatch(emailAddress, pattern);
        }

        public static bool IsIMEmail(string emailAddress) 
        {
            if (emailAddress.EndsWith("hotmail.com") ||
                emailAddress.EndsWith("yahoo.com") ||
                emailAddress.EndsWith("gmail.com") ||
                emailAddress.EndsWith("google.com"))
            {
                return true;
            }
            else { return false; }
        }
    }
}
