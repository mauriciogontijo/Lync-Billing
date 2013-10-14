using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using Lync_Backend.Interfaces;
using Lync_Backend.Helpers;
using System.Configuration;
using Lync_Backend.Libs;

namespace Lync_Backend.Implementation
{
    class PhoneCalls2010 : IDatabaseImporter 
    {

        private static DBLib DBRoutines = new DBLib();
        private static Dictionary<string, MonitoringServersInfo> monInfo = new Dictionary<string, MonitoringServersInfo>();
        
        private OleDbConnection sourceDBConnector;
        
        public PhoneCalls2010()
        {
            try
            {
                sourceDBConnector = new OleDbConnection(ConstructConnectionString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ConstructConnectionString()
        {
            monInfo = MonitoringServersInfo.GetMonitoringServersInfo();
            var info = monInfo.Where(item => item.Key == this.GetType().Name).Select(item => (MonitoringServersInfo)item.Value).First() as MonitoringServersInfo;

            return MonitoringServersInfo.CreateConnectionString(info);
        }

        public void ImportPhoneCalls()
        {
            OleDbCommand command;
            OleDbDataReader dataReader;

            Dictionary<string, object> phoneCall;

            string SQL = string.Empty;
            string WHERE_STATEMENT = string.Empty;
            string SELECT_STATEMENT = string.Empty;

            string column = string.Empty;
            
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

            CallsImportStatus lastImportStat = CallsImportStatus.GetCallsImportStatus(this.GetType().Name);

            if (lastImportStat != null)
            {
                WHERE_STATEMENT = String.Format(
                    " WHERE " +
                        "Users_1.UserUri IS NOT NULL AND " +
                        "SessionDetails.ResponseCode = 200 AND " +
                        "SessionDetails.MediaTypes = 16 AND " +
                        "VoipDetails.SessionIdTime > '{0}'", lastImportStat.Timestamp
                );
            }
            else 
            {
                WHERE_STATEMENT = String.Format(
                    " WHERE " +
                        "Users_1.UserUri IS NOT NULL AND " +
                        "SessionDetails.ResponseCode = 200 AND " +
                        "SessionDetails.MediaTypes = 16"
                );
            }

            SQL = SELECT_STATEMENT + WHERE_STATEMENT;

            command = new OleDbCommand(SQL, sourceDBConnector);

            sourceDBConnector.Open();

            dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            while(dataReader.Read())
            {
                column = string.Empty;
                
                phoneCall = new Dictionary<string,object>();
               
                phoneCall.Add("SessionIdTime", dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionIdTime)].ToString());
                phoneCall.Add("SessionIdSeq", (int)dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq)]);
                phoneCall.Add("ResponseTime", dataReader[Enums.GetDescription(Enums.PhoneCalls.ResponseTime)].ToString());
                phoneCall.Add("SessionEndTime", dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionEndTime)].ToString());

                //Handle null values
                column = Enums.GetDescription(Enums.PhoneCalls.SourceUserUri);
                phoneCall.Add("SourceUserUri", dataReader[column]);

                column = Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri);
                phoneCall.Add("DestinationNumberUri", dataReader[column]);

                column = Enums.GetDescription(Enums.PhoneCalls.FromMediationServer);
                phoneCall.Add("FromMediationServer", dataReader[column]);

                column = Enums.GetDescription(Enums.PhoneCalls.ToMediationServer);
                phoneCall.Add("ToMediationServer", dataReader[column]);

                column = Enums.GetDescription(Enums.PhoneCalls.FromGateway);
                phoneCall.Add("FromGateway", dataReader[column]);

                column = Enums.GetDescription(Enums.PhoneCalls.ToGateway);
                phoneCall.Add("ToGateway", dataReader[column]);

                column = Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer);
                phoneCall.Add("SourceUserEdgeServer", dataReader[column]);

                column = Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer);
                phoneCall.Add("DestinationUserEdgeServer", dataReader[column]);

                column = Enums.GetDescription(Enums.PhoneCalls.ServerFQDN);
                phoneCall.Add("ServerFQDN", dataReader[column]);

                column = Enums.GetDescription(Enums.PhoneCalls.PoolFQDN);
                phoneCall.Add("PoolFQDN", dataReader[column]);

                column = Enums.GetDescription(Enums.PhoneCalls.Duration);
                phoneCall.Add("Duration", dataReader[column]);


                //Insert the phonecall to designated PhoneCalls table

                DBRoutines.INSERT(this.GetType().Name,phoneCall,Enums.GetDescription(Enums.PhoneCalls.SessionIdTime));
                
            }
        }

        public void ImportGateways()
        {
            throw new NotImplementedException();
        }

        public void ImportPools()
        {
            throw new NotImplementedException();
        }
    

    }
}
