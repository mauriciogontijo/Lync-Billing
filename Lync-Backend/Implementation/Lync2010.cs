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
    class Lync2010 : AbIdDatabaseImporter 
    {

        private static DBLib DBRoutines = new DBLib();
        private static Dictionary<string, MonitoringServersInfo> monInfo = new Dictionary<string, MonitoringServersInfo>();
        
        private OleDbConnection sourceDBConnector;
        
        public Lync2010()
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

        override public string ConstructConnectionString()
        {
            monInfo = MonitoringServersInfo.GetMonitoringServersInfo();
            var info = monInfo.Where(item => item.Key == this.GetType().Name).Select(item => (MonitoringServersInfo)item.Value).First() as MonitoringServersInfo;

            return MonitoringServersInfo.CreateConnectionString(info);
        }

        override public void ImportPhoneCalls()
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

            CallsImportStatus lastImportStat = CallsImportStatus.GetCallsImportStatus(PhoneCallsTableName);

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
            command.CommandTimeout = 10000;

            sourceDBConnector.Open();



            dataReader = command.ExecuteReader();

            while(dataReader.Read())
            {
                column = string.Empty;
                
                phoneCall = new Dictionary<string,object>();

                phoneCall.Add("SessionIdTime", Misc.ConvertDate((DateTime)dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionIdTime)]));
                phoneCall.Add("SessionIdSeq", (int)dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq)]);


                column = Enums.GetDescription(Enums.PhoneCalls.ResponseTime);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("ResponseTime", Misc.ConvertDate((DateTime)dataReader[Enums.GetDescription(Enums.PhoneCalls.ResponseTime)]));
                else
                    phoneCall.Add("ResponseTime", DBNull.Value);

                
                column = Enums.GetDescription(Enums.PhoneCalls.SessionEndTime);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("SessionEndTime", Misc.ConvertDate((DateTime)dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionEndTime)]));
                else
                    phoneCall.Add("SessionEndTime", DBNull.Value);

                
                column = Enums.GetDescription(Enums.PhoneCalls.SourceUserUri);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("SourceUserUri", dataReader[Enums.GetDescription(Enums.PhoneCalls.SourceUserUri)].ToString());
                else
                    phoneCall.Add("SourceUserUri", DBNull.Value);

                column = Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("SourceNumberUri", dataReader[Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri)].ToString());
                else
                    phoneCall.Add("SourceNumberUri", DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("DestinationUserUri", dataReader[Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri)].ToString());
                else
                    phoneCall.Add("DestinationUserUri", DBNull.Value);

                column = Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("DestinationNumberUri", dataReader[Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri)].ToString());
                else
                    phoneCall.Add("DestinationNumberUri", DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.FromMediationServer);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("FromMediationServer", dataReader[Enums.GetDescription(Enums.PhoneCalls.FromMediationServer)].ToString());
                else
                    phoneCall.Add("FromMediationServer", DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.ToMediationServer);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("ToMediationServer", dataReader[Enums.GetDescription(Enums.PhoneCalls.ToMediationServer)].ToString());
                else
                    phoneCall.Add("ToMediationServer", DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.FromGateway);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("FromGateway", dataReader[Enums.GetDescription(Enums.PhoneCalls.FromGateway)].ToString());
                else
                    phoneCall.Add("FromGateway", DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.ToGateway);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("ToGateway", dataReader[Enums.GetDescription(Enums.PhoneCalls.ToGateway)].ToString());
                else
                    phoneCall.Add("ToGateway", DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("SourceUserEdgeServer", dataReader[Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer)].ToString());
                else
                    phoneCall.Add("SourceUserEdgeServer", DBNull.Value);
                

                column = Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("DestinationUserEdgeServer", dataReader[Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer)].ToString());
                else
                    phoneCall.Add("DestinationUserEdgeServer", DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.ServerFQDN);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("ServerFQDN", dataReader[Enums.GetDescription(Enums.PhoneCalls.ServerFQDN)].ToString());
                else
                    phoneCall.Add("ServerFQDN", DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.PoolFQDN);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("PoolFQDN", dataReader[Enums.GetDescription(Enums.PhoneCalls.PoolFQDN)].ToString());
                else
                    phoneCall.Add("PoolFQDN", DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.Duration);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("Duration", dataReader[Enums.GetDescription(Enums.PhoneCalls.Duration)].ToString());
                else
                    phoneCall.Add("Duration", Convert.ToDecimal(0));


                //Insert the phonecall to designated PhoneCalls table
                DBRoutines.INSERT(this.GetType().Name,phoneCall);
                
            }
        }

        override public void ImportGateways()
        {
            OleDbCommand command;
            OleDbDataReader dataReader;

            string SQL = string.Empty;
        }

        override public void ImportPools()
        {
            OleDbCommand command;
            OleDbDataReader dataReader;

            string SQL = string.Empty;
        }

        public override string PhoneCallsTableName
        {
            get { return "PhoneCalls2010"; }
        }

        public override string PoolsTableName
        {
            get { return "Pools"; }
        }

        public override string GatewaysTableName
        {
            get { return "Gateways"; }
        }
    }
}
