using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Helpers;

namespace Lync_Backend.Helpers
{
    class SQLs
    {

        public static string CREATE_PHONECALLS_TABLE_QUERY(string tableName) 
        {
            return string.Format
                (
                   "CREATE TABLE [dbo].[{0}](" +
	                    "[SessionIdTime] [datetime] NOT NULL," +
	                    "[SessionIdSeq] [int] NOT NULL," +
	                    "[SourceUserUri] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[DestinationUserUri] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[SourceNumberUri] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[DestinationNumberUri] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[FromMediationServer] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[ToMediationServer] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[FromGateway] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[ToGateway] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[SourceUserEdgeServer] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[DestinationUserEdgeServer] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[ResponseTime] [datetime] NULL," +
	                    "[SessionEndTime] [datetime] NULL," +
	                    "[ServerFQDN] [nvarchar](257) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[PoolFQDN] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[OnBehalf] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[ReferredBy] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[ChargingParty] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[Duration] [numeric](8, 0) NULL," +
	                    "[marker_CallFrom] [bigint] NULL," +
	                    "[marker_CallTo] [bigint] NULL," +
	                    "[marker_CallToCountry] [varchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[marker_CallCost] [money] NULL," +
	                    "[marker_CallTypeID] [bigint] NULL," +
	                    "[marker_CallType] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[marker_TimeStamp] [datetime] NULL," +
	                    "[ui_MarkedOn] [datetime] NULL," +
	                    "[ui_UpdatedByUser] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[ui_AssignedByUser] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[ui_AssignedOn] [datetime] NULL," +
	                    "[ui_CallType] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[ac_DisputeStatus] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[ac_DisputeResolvedOn] [datetime] NULL," +
	                    "[ac_IsInvoiced] [nvarchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL," +
	                    "[ac_InvoiceDate] [datetime] NULL," +
	                    "[Exclude] [bit] NULL," +
                    "CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED " +
                    "(" +
	                    "[SessionIdTime] ASC," +
	                    "[SessionIdSeq] ASC" +
                    ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
                    ") ON [PRIMARY]",tableName );
        }

        public static string CREATE_READ_PHONE_CALLS_QUERY(string TABLE_NAME, string SessionIdTime = null)
        {
            string SQL = string.Empty;
            string WHERE_STATEMENT = string.Empty;
            string SELECT_STATEMENT = string.Empty;
            string ORDER_BY = string.Empty;

            SELECT_STATEMENT = String.Format("SELECT * FROM [{0}] ", TABLE_NAME);

            if (SessionIdTime != null)
            {
                WHERE_STATEMENT = String.Format("WHERE " + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + " > '{0}' ", SessionIdTime);
            }

            ORDER_BY = "ORDER BY " + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + " ASC ";

            return SELECT_STATEMENT + WHERE_STATEMENT + ORDER_BY;
        }

        public static string CREATE_READ_PHONE_CALLS_QUERY(string TABLE_NAME, string gateway, DateTime? optionalFrom = null, DateTime? optionalTo = null)
        {
            string SQL = string.Empty;
            string WHERE_STATEMENT = string.Empty;
            string SELECT_STATEMENT = string.Empty;
            string ORDER_BY = string.Empty;

            DateTime from = optionalFrom != null ? optionalFrom.Value : DateTime.MinValue;
            DateTime to = optionalTo != null ? optionalTo.Value : DateTime.MaxValue;

            SELECT_STATEMENT = String.Format("SELECT * FROM [{0}] ", TABLE_NAME);

            WHERE_STATEMENT = string.Format(" WHERE " + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + " BETWEEN '{0}' AND '{1}' ", Misc.ConvertDate(from), Misc.ConvertDate(to));

            if (!string.IsNullOrEmpty(gateway))
            {
                WHERE_STATEMENT += String.Format(" AND " + Enums.GetDescription(Enums.PhoneCalls.ToGateway) + " " + "> '{0}' ", gateway);
            }

            ORDER_BY = " ORDER BY " + Enums.GetDescription(Enums.PhoneCalls.SessionIdTime) + " ASC ";

            return SELECT_STATEMENT + WHERE_STATEMENT + ORDER_BY;
        }

        public static string CREATE_IMPORT_PHONE_CALLS_QUERY_LYNC_2010(string LAST_IMPORTED_PHONECALL_DATE = null)
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
                        "OnBehalf.UserUri AS OnBehalf," +
		                "ReferredBy.UserUri AS ReferredBy," +
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
                        "LEFT OUTER JOIN Users AS OnBehalf ON SessionDetails.OnBehalfOfId = OnBehalf.UserId " +
                        "LEFT OUTER JOIN Users AS ReferredBy ON SessionDetails.ReferredById = ReferredBy.UserId " +
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
                    "Users_1.UserUri NOT LIKE '%;phone%' AND " +
                    "Users_1.UserUri NOT LIKE '%;user%' AND " +
                    "Users_1.UserUri NOT LIKE '+%@%' AND " +
                    "SessionDetails.ResponseCode = 200 AND " +
                    "SessionDetails.MediaTypes = 16 "
                );
            }

            ORDER_BY = " ORDER BY VoipDetails.SessionIdTime ASC ";

            return SELECT_STATEMENT + WHERE_STATEMENT + ORDER_BY;
        }

        public static string CREATE_IMPORT_PHONE_CALLS_QUERY_LYNC_2013(string LAST_IMPORTED_PHONECALL_DATE = null) 
        {
            string SQL = string.Empty;
            string WHERE_STATEMENT = string.Empty;
            string SELECT_STATEMENT = string.Empty;
            string ORDER_BY = string.Empty;

            SELECT_STATEMENT = String.Format
            (
                "SELECT  " +
	                "VoipDetails.SessionIdTime,   " +
	                "VoipDetails.SessionIdSeq, " +
	                "Users_1.UserUri AS SourceUserUri,  " +
	                "Phones_1.PhoneUri AS SourceNumberUri,   " +
	                "Users_2.UserUri AS DestinationUserUri,   " +
	                "Phones_2.PhoneUri AS DestinationNumberUri,   " +
	                "REPLACE(UserCalleeURI.URI,'sip:','') AS CalleeURI, " +
	                "MediationServers.MediationServer AS FromMediationServer,   " +
	                "MediationServers_1.MediationServer AS ToMediationServer,   " +
	                "Gateways.Gateway AS FromGateway,   " +
	                "Gateways_1.Gateway AS ToGateway,   " +
	                "EdgeServers.EdgeServer AS SourceUserEdgeServer,   " +
	                "EdgeServers_1.EdgeServer AS DestinationUserEdgeServer,   " +
	                "Servers.ServerFQDN,   " +
	                "Pools.PoolFQDN,   " +
	                "SessionDetails.ResponseTime,   " +
	                "SessionDetails.SessionEndTime,   " +
	                "OnBehalf.UserUri AS OnBehalf,  " +
                    "ReferredBy.UserUri AS ReferredBy, " +
	                "CONVERT(decimal(8, 0), DATEDIFF(second, SessionDetails.ResponseTime,  SessionDetails.SessionEndTime)) AS Duration    " +
                "FROM DialogsView " +
                "RIGHT OUTER JOIN VoipDetails as VoipDetails on  " +
	                "VoipDetails.SessionIdTime = DialogsView.LsCDRSessionIdTime and  " +
	                "VoipDetails.SessionIdSeq = DialogsView.LsCDRSessionIdSeq " +
                "LEFT OUTER JOIN SessionDetails as SessionDetails on  " +
	                "SessionDetails.SessionIdTime = DialogsView.LsCDRSessionIdTime and  " +
	                "VoipDetails.SessionIdSeq = DialogsView.LsCDRSessionIdSeq " +
                "LEFT OUTER JOIN [QoEMetrics].[dbo].[Session] as QoEMetricsSession on  " +
	                "QoEMetricsSession.ConferenceDateTime =  DialogsView.QoEMetricsSessionIdTime and  " +
	                "QoEMetricsSession.SessionSeq =  DialogsView.QoEMetricsSessionIdSeq " +
                "LEFT OUTER JOIN Servers ON  " +
	                "SessionDetails.ServerId = Servers.ServerId   " +
                "LEFT OUTER JOIN Pools ON  " +
	                "SessionDetails.PoolId = Pools.PoolId   " +
                "LEFT OUTER JOIN SIPResponseMetaData ON  " +
	                "SessionDetails.ResponseCode = SIPResponseMetaData.ResponseCode   " +
                "LEFT OUTER JOIN EdgeServers AS EdgeServers_1 ON  " +
	                "SessionDetails.User2EdgeServerId = EdgeServers_1.EdgeServerId   " +
                "LEFT OUTER JOIN EdgeServers ON  " +
	                "SessionDetails.User1EdgeServerId = EdgeServers.EdgeServerId   " +
                "LEFT OUTER JOIN Users AS Users_2 ON  " +
	                "SessionDetails.User2Id = Users_2.UserId   " +
                "LEFT OUTER JOIN Users AS Users_1 ON  " +
	                "SessionDetails.User1Id = Users_1.UserId   " +
                "LEFT OUTER JOIN Users AS OnBehalf ON  " +
	                "SessionDetails.OnBehalfOfId = OnBehalf.UserId    " +
                "LEFT OUTER JOIN Users AS ReferredBy ON   " +
                    "SessionDetails.ReferredById = ReferredBy.UserId  " +
                "LEFT OUTER JOIN Gateways AS Gateways_1 ON  " +
	                "VoipDetails.ToGatewayId = Gateways_1.GatewayId   " +
                "LEFT OUTER JOIN Gateways ON  " +
	                "VoipDetails.FromGatewayId = Gateways.GatewayId   " +
                "LEFT OUTER JOIN MediationServers AS MediationServers_1 ON  " +
	                "VoipDetails.ToMediationServerId = MediationServers_1.MediationServerId   " +
                "LEFT OUTER JOIN MediationServers ON  " +
	                "VoipDetails.FromMediationServerId = MediationServers.MediationServerId   " +
                "LEFT OUTER JOIN Phones AS Phones_1 ON  " +
	                "VoipDetails.FromNumberId = Phones_1.PhoneId    " +
                "LEFT OUTER JOIN Phones AS Phones_2 ON  " +
	                "VoipDetails.ConnectedNumberId = Phones_2.PhoneId   " +
                "LEFT OUTER JOIN [QoEMetrics].[dbo].[User] AS UserCalleeURI on  " +
	                "QoEMetricsSession.CalleeURI = UserCalleeURI.UserKey "
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
                    "Users_1.UserUri NOT LIKE '%;phone%' AND " +
                    "Users_1.UserUri NOT LIKE '%;user%' AND " +
                    "Users_1.UserUri NOT LIKE '+%@%' AND " +
                    "SessionDetails.ResponseCode = 200 AND " +
                    "SessionDetails.MediaTypes = 16 "
                );
            }

            ORDER_BY = " ORDER BY VoipDetails.SessionIdTime ASC ";

            return SELECT_STATEMENT + WHERE_STATEMENT + ORDER_BY;
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

        public static string CREATE_VALIDATE_DB_OBJECT_QUERY(string tableName) 
        {
            return string.Format("select OBJECT_ID('{0}')", tableName);
        }

    }
}
