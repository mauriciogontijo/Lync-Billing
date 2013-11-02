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

        public static string CREATE_READ_PHONE_CALLS_QUERY(string TABLE_NAME, string TIMESTAMP = null) 
        {
            string SQL = string.Empty;
            string WHERE_STATEMENT = string.Empty;
            string SELECT_STATEMENT = string.Empty;
            string ORDER_BY = string.Empty;

            SELECT_STATEMENT = String.Format("SELECT * FROM [{0}] ",TABLE_NAME);


            if (TIMESTAMP != null)
            {
                WHERE_STATEMENT = String.Format(" WHERE SessionIdTime > '{0}'", TIMESTAMP);
            }


            ORDER_BY = " ORDER BY SessionIdTime ASC ";

            return SELECT_STATEMENT + WHERE_STATEMENT + ORDER_BY;
        }

        public static string CREATE_IMPORT_PHONE_CALLS_QUERY(string LAST_IMPORTED_PHONECALL_DATE = null) 
        {
            string SQL = string.Empty;
            string WHERE_STATEMENT = string.Empty;
            string SELECT_STATEMENT = string.Empty;
            string ORDER_BY = string.Empty;

            SELECT_STATEMENT = String.Format(
               "SELECT " +
                        "VoipDetails.SessionIdTime, " +
                        "VoipDetails.SessionIdSeq, " +
                        "Users_1.UserUri AS SourceUserUri, " +
                        "Users_2.UserUri AS DestinationUserUri, " +
                        "Phones.PhoneUri AS SourceNumberUri, " +
                        "Phones_1.PhoneUri AS DestinationNumberUri, " +
                        "MediationServers.MediationServer AS FromMediationServer, " +
                        "MediationServers_1.MediationServer AS ToMediationServer, " +
                        "Gateways.Gateway AS FromGateway, " +
                        "Gateways_1.Gateway AS ToGateway, " +
                        "EdgeServers.EdgeServer AS SourceUserEdgeServer, " +
                        "EdgeServers_1.EdgeServer AS DestinationUserEdgeServer, " +
                        "Servers.ServerFQDN, " +
                        "Pools.PoolFQDN, " +
                        "SessionDetails.ResponseTime, " +
                        "SessionDetails.SessionEndTime, " +
                        "CONVERT(decimal(8, 0), " +
                        "DATEDIFF(second, SessionDetails.ResponseTime,  SessionDetails.SessionEndTime)) AS Duration " +
               "FROM     SessionDetails " +
                        "LEFT OUTER JOIN Servers ON SessionDetails.ServerId = Servers.ServerId " +
                        "LEFT OUTER JOIN Pools ON SessionDetails.PoolId = Pools.PoolId " +
                        "LEFT OUTER JOIN SIPResponseMetaData ON SessionDetails.ResponseCode = SIPResponseMetaData.ResponseCode " +
                        "LEFT OUTER JOIN EdgeServers AS EdgeServers_1 ON SessionDetails.User2EdgeServerId = EdgeServers_1.EdgeServerId " +
                        "LEFT OUTER JOIN EdgeServers ON SessionDetails.User1EdgeServerId = EdgeServers.EdgeServerId " +
                        "LEFT OUTER JOIN Users AS Users_2 ON SessionDetails.User2Id = Users_2.UserId " +
                        "LEFT OUTER JOIN Users AS Users_1 ON SessionDetails.User1Id = Users_1.UserId " +
                        "RIGHT OUTER JOIN VoipDetails " +
                        "LEFT OUTER JOIN Gateways AS Gateways_1 ON VoipDetails.ToGatewayId = Gateways_1.GatewayId " +
                        "LEFT OUTER JOIN Gateways ON VoipDetails.FromGatewayId = Gateways.GatewayId " +
                        "LEFT OUTER JOIN MediationServers AS MediationServers_1 ON VoipDetails.ToMediationServerId = MediationServers_1.MediationServerId " +
                        "LEFT OUTER JOIN MediationServers ON VoipDetails.FromMediationServerId = MediationServers.MediationServerId " +
                        "LEFT OUTER JOIN Phones AS Phones_1 ON VoipDetails.ConnectedNumberId = Phones_1.PhoneId " +
                        "LEFT OUTER JOIN Phones ON VoipDetails.FromNumberId = Phones.PhoneId ON SessionDetails.SessionIdTime = VoipDetails.SessionIdTime AND " +
                        "SessionDetails.SessionIdSeq = VoipDetails.SessionIdSeq "
           );

            if (LAST_IMPORTED_PHONECALL_DATE != null)
            {
                WHERE_STATEMENT = String.Format(
                    " WHERE " +
                        "Users_1.UserUri IS NOT NULL AND " +
                        "Users_1.UserUri NOT LIKE '%;phone%' AND " +
                        "Users_1.UserUri NOT LIKE '%;user%' AND " +
                        "Users_1.UserUri NOT LIKE '+%@%' AND " +
                        "SessionDetails.ResponseCode = 200 AND " +
                        "SessionDetails.MediaTypes = 16 AND " +
                        "VoipDetails.SessionIdTime > '{0}'", LAST_IMPORTED_PHONECALL_DATE
                );
            }
            else
            {
                WHERE_STATEMENT = string.Format(
                    " WHERE " +
                    "Users_1.UserUri IS NOT NULL AND " +
                    "Users_1.UserUri NOT LIKE '%;phone%' AND "+
                    "Users_1.UserUri NOT LIKE '%;user%' AND " +
                    "Users_1.UserUri NOT LIKE '+%@%' AND " +
                    "SessionDetails.ResponseCode = 200 AND " +
                    "SessionDetails.MediaTypes = 16 "
                );
            }

            ORDER_BY = " ORDER BY VoipDetails.SessionIdTime ASC ";

            return  SELECT_STATEMENT + WHERE_STATEMENT + ORDER_BY;
        }
        
        public static string CREATE_IMPORT_GATEWAYS_QUERY()
        {
            return String.Format("SELECT [GatewayId], [Gateway] FROM [dbo].[Gateways]");
        }

        public static string CREATE_IMPORT_POOLS_QUERY()
        {
            return string.Format("SELECT [PoolId], [PoolFQDN] FROM [dbo].[Pools]");
        }

        public static string CREATE_LAST_IMPORTED_PHONECALL_DATE_QUERY(string tableName) 
        {
            return string.Format("SELECT MAX(SessionIdTime) as SessionIdTime FROM {0}", tableName);
        }

        public static string CREATE_GET_RATES_PER_GATEWAY_QUERY(string RatesTableName) 
        {
            return string.Format(
                "select " +
                   "Country_Name, " +
                   "Two_Digits_country_code, " +
                   "Three_Digits_Country_Code, " +
                   "max(CASE WHEN Type_Of_Service <> 'gsm'  then rate END ) Fixedline, " +
                   "max(CASE WHEN Type_Of_Service='gsm'then rate END) GSM " +

               "from " +
               "( " +
                   "SELECT	DISTINCT " +
                       "numberingplan.Country_Name, " +
                       "numberingplan.Two_Digits_country_code, " +
                       "numberingplan.Three_Digits_Country_Code, " +
                       "numberingplan.Type_Of_Service, " +
                       "fixedrate.rate as rate " +

                   "FROM  " +
                       "dbo.NumberingPlan as numberingplan " +

                   "LEFT JOIN " +
                       "dbo.[{0}]  as fixedrate ON " +
                           "numberingplan.Dialing_prefix = fixedrate.country_code_dialing_prefix) src " +

               "GROUP BY Country_Name,Two_Digits_country_code,Three_Digits_Country_Code ", RatesTableName); 
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
