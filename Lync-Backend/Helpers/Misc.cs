using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lync_Backend.Libs;

namespace Lync_Backend.Helpers
{
    public class Misc
    {

        private static AdLib adRoutines = new AdLib();

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
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.SessionIdTime), phoneCall.SessionIdTime);

            if (phoneCall.SessionIdSeq != null)
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq), phoneCall.SessionIdSeq);

            if (!string.IsNullOrEmpty(phoneCall.ResponseTime))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.ResponseTime), phoneCall.ResponseTime);

            if (!string.IsNullOrEmpty(phoneCall.SessionEndTime))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.SessionEndTime), phoneCall.SessionEndTime);

            if (!string.IsNullOrEmpty(phoneCall.SourceUserUri))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.SourceUserUri), phoneCall.SourceUserUri);

            if (!string.IsNullOrEmpty(phoneCall.SourceNumberUri))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri), phoneCall.SourceNumberUri);

            if (!string.IsNullOrEmpty(phoneCall.DestinationNumberUri))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri), phoneCall.DestinationNumberUri);

            if (!string.IsNullOrEmpty(phoneCall.FromMediationServer))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.FromMediationServer), phoneCall.FromMediationServer);

            if (!string.IsNullOrEmpty(phoneCall.ToMediationServer))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.ToMediationServer), phoneCall.ToMediationServer);

            if (!string.IsNullOrEmpty(phoneCall.FromGateway))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.FromGateway), phoneCall.FromGateway);

            if (!string.IsNullOrEmpty(phoneCall.ToGateway))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.ToGateway), phoneCall.ToGateway);

            if (!string.IsNullOrEmpty(phoneCall.SourceUserEdgeServer))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer), phoneCall.SourceUserEdgeServer);

            if (!string.IsNullOrEmpty(phoneCall.DestinationUserEdgeServer))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer), phoneCall.DestinationUserEdgeServer);

            if (!string.IsNullOrEmpty(phoneCall.ServerFQDN))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.ServerFQDN), phoneCall.ServerFQDN);

            if (!string.IsNullOrEmpty(phoneCall.PoolFQDN))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.PoolFQDN), phoneCall.PoolFQDN);

            if (!string.IsNullOrEmpty(phoneCall.OnBehalf))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.OnBehalf), phoneCall.OnBehalf);

            if (!string.IsNullOrEmpty(phoneCall.ReferredBy))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.ReferredBy), phoneCall.ReferredBy);

            if(!string.IsNullOrEmpty(phoneCall.CalleeURI))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.CalleeURI), phoneCall.CalleeURI);

            if (!string.IsNullOrEmpty(phoneCall.ChargingParty))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.ChargingParty), phoneCall.ChargingParty);

            if (!string.IsNullOrEmpty(phoneCall.Marker_CallToCountry))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry), phoneCall.Marker_CallToCountry);

            if (!string.IsNullOrEmpty(phoneCall.Marker_CallType))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.Marker_CallType), phoneCall.Marker_CallType);

            phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID), phoneCall.Marker_CallTypeID);
            phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost), phoneCall.Marker_CallCost);
            phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.Marker_CallFrom), phoneCall.Marker_CallFrom);
            phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.Marker_CallTo), phoneCall.Marker_CallTo);
            phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.Duration), phoneCall.Duration);

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
            
            column = Enums.GetDescription(Enums.PhoneCalls.OnBehalf);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.OnBehalf = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.ReferredBy);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.ReferredBy = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.CalleeURI);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.CalleeURI = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.ChargingParty);
            if (ValidateColumnName(ref dataReader,ref column) == true && (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.ChargingParty = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.Duration);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.Duration = Convert.ToDecimal(dataReader[column]);

            column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallFrom);
            if (ValidateColumnName(ref dataReader, ref column) == true && (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.Marker_CallFrom = Convert.ToInt64(dataReader[column]);

            column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallTo);
            if (ValidateColumnName(ref dataReader, ref column) == true && ( dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.Marker_CallTo = Convert.ToInt64(dataReader[column]);

            column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry);
            if (ValidateColumnName(ref dataReader, ref column) == true && ( dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.Marker_CallToCountry = dataReader[column].ToString();

            column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallTypeID);
            if (ValidateColumnName(ref dataReader, ref column) == true && ( dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.Marker_CallTypeID = Convert.ToInt32(dataReader[column]);

            column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost);
            if (ValidateColumnName(ref dataReader, ref column) == true && ( dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.Marker_CallCost = Convert.ToInt32(dataReader[column]);

            column = Enums.GetDescription(Enums.PhoneCalls.Marker_CallType);
            if (ValidateColumnName(ref dataReader, ref column) == true && ( dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.Marker_CallType = dataReader[column].ToString();


            //Return teh filled object
            return phoneCall;
        }

        private static bool ValidateColumnName(ref OleDbDataReader dataReader,ref string columnName) 
        {
            try
            {
                if (dataReader.GetOrdinal(columnName) >= 0)
                    return true;
                else
                    return false;
            }
            catch(Exception ex) 
            {
                return false;
            }
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

        public static string NormalizePhoneNumber(string phoneNumber) 
        {
            string number = string.Empty;
            
            if (phoneNumber.StartsWith("+"))
            {
                number = Regex.Replace(phoneNumber, @"@\w{1,}.*", "");
                
                var userInfo = adRoutines.getUsersAttributesFromPhone(number);
               
                number = (userInfo != null && userInfo.SipAccount != null) ? userInfo.SipAccount.Replace("sip:","") : number;

            }
            else
                number = phoneNumber;

            return number;
        }
    }

}
