using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static string CREATE_LAST_IMPORTED_PHONECALL_DATE_QUERY() 
        {
            return string.Format("SELECT MAX(SessionIdTime) as SessionIdTime FROM PhoneCalls2010");
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


        public static Dictionary<string, object> ConvertPhoneCallToDictionary(PhoneCall phoneCall)
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

            if (!string.IsNullOrEmpty(phoneCall.marker_CallType))
                phoneCallDict.Add("marker_CallType", phoneCall.marker_CallType);

            if (phoneCall.Marker_CallTypeID != null)
                phoneCallDict.Add("marker_CallTypeID", phoneCall.Marker_CallTypeID);

            if (phoneCall.Marker_CallCost != null)
                phoneCallDict.Add("marker_CallCost", phoneCall.Marker_CallCost);

            if (phoneCall.marker_CallFrom != null)
                phoneCallDict.Add("marker_CallFrom", phoneCall.marker_CallFrom);

            if (phoneCall.marker_CallTo != null)
                phoneCallDict.Add("marker_CallTo", phoneCall.marker_CallTo);

            if (phoneCall.Duration != null)
                phoneCallDict.Add("Duration", phoneCall.Duration);
            

            return phoneCallDict;
        }
    }
}
