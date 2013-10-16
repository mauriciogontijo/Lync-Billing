using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Interfaces;
using Lync_Backend.Libs;
using Lync_Backend.Helpers;
using System.Data.OleDb;
using System.Data;

namespace Lync_Backend.Implementation
{
    class Lync2013 : AbIdDatabaseImporter
    {
       private static DBLib DBRoutines = new DBLib();
        private static Dictionary<string, MonitoringServersInfo> monInfo = new Dictionary<string, MonitoringServersInfo>();
        
        private OleDbConnection sourceDBConnector;
        
        public Lync2013()
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

        public override string PhoneCallsTableName
        {
            get { return "PhoneCalls2013"; }
        }

        public override string PoolsTableName
        {
            get { return "Pools2013"; }
        }

        public override string GatewaysTableName
        {
            get { return "Gateways2013"; }
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

            string FirstTimeImportDate = string.Empty;

            string LastPhoneCallDate = string.Empty;
            string LAST_PHONECALL_DATE_QUERY = string.Empty;
            
            string column = string.Empty;

            string SQL = string.Empty;
            string WHERE_STATEMENT = string.Empty;
            string SELECT_STATEMENT = string.Empty;
            string ORDER_BY = string.Empty;
            
            LAST_PHONECALL_DATE_QUERY = "SELECT MAX(SessionIdTime) as SessionIdTime FROM VoipDetails";

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
                FirstTimeImportDate = Misc.ConvertDate(DateTime.Now);

                WHERE_STATEMENT = string.Format(
                    " WHERE " +
                    "Users_1.UserUri IS NOT NULL AND " +
                    "SessionDetails.ResponseCode = 200 AND " +
                    "SessionDetails.MediaTypes = 16 AND " +
                    "VoipDetails.SessionIdTime < '{0}'", FirstTimeImportDate
                );
            }

            ORDER_BY = " ORDER BY VoipDetails.SessionIdTime DESC ";

            SQL = SELECT_STATEMENT + WHERE_STATEMENT + ORDER_BY;


            /***
             * Open the database connection to execute the following commands
             */
            sourceDBConnector.Open();


            /***
             * Firstly, get the Last Phonecall Date
             * By executing the LAST_PHONECALL_DATE_QUERY
             * If the FirstTimeImportDate is initialized then assign it to the LastPhoneCallDate
             */
            if (!string.IsNullOrEmpty(FirstTimeImportDate))
            {
                LastPhoneCallDate = FirstTimeImportDate;
            }
            else
            {
                command = new OleDbCommand(LAST_PHONECALL_DATE_QUERY, sourceDBConnector);
                command.CommandTimeout = 10000;

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    LastPhoneCallDate = Misc.ConvertDate((DateTime)dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionIdTime)]);
                }
            }


            /***
             * Secondly, import the Phonecalls from the source database
             * By executing the long SQL query.
             */
            command = new OleDbCommand(SQL, sourceDBConnector);
            command.CommandTimeout = 10000;

            int phoneCallsCounter = 0;

            dataReader = command.ExecuteReader();

            while(dataReader.Read())
            {
                column = string.Empty;
                
                phoneCall = new Dictionary<string,object>();

                phoneCall.Add("SessionIdTime", Misc.ConvertDate((DateTime)dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionIdTime)]));
                phoneCall.Add("SessionIdSeq", (int)dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq)]);


                column = Enums.GetDescription(Enums.PhoneCalls.ResponseTime);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add(column, Misc.ConvertDate((DateTime)dataReader[Enums.GetDescription(Enums.PhoneCalls.ResponseTime)]));
                else
                    phoneCall.Add(column, DBNull.Value);

                
                column = Enums.GetDescription(Enums.PhoneCalls.SessionEndTime);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add(column, Misc.ConvertDate((DateTime)dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionEndTime)]));
                else
                    phoneCall.Add(column, DBNull.Value);

                
                column = Enums.GetDescription(Enums.PhoneCalls.SourceUserUri);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add(column, dataReader[Enums.GetDescription(Enums.PhoneCalls.SourceUserUri)].ToString());
                else
                    phoneCall.Add(column, DBNull.Value);

                column = Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add(column, dataReader[Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri)].ToString());
                else
                    phoneCall.Add(column, DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add(column, dataReader[Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri)].ToString());
                else
                    phoneCall.Add(column, DBNull.Value);

                column = Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add(column, dataReader[Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri)].ToString());
                else
                    phoneCall.Add(column, DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.FromMediationServer);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add(column, dataReader[Enums.GetDescription(Enums.PhoneCalls.FromMediationServer)].ToString());
                else
                    phoneCall.Add(column, DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.ToMediationServer);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add(column, dataReader[Enums.GetDescription(Enums.PhoneCalls.ToMediationServer)].ToString());
                else
                    phoneCall.Add(column, DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.FromGateway);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add(column, dataReader[Enums.GetDescription(Enums.PhoneCalls.FromGateway)].ToString());
                else
                    phoneCall.Add(column, DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.ToGateway);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add(column, dataReader[Enums.GetDescription(Enums.PhoneCalls.ToGateway)].ToString());
                else
                    phoneCall.Add(column, DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add(column, dataReader[Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer)].ToString());
                else
                    phoneCall.Add(column, DBNull.Value);
                

                column = Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add(column, dataReader[Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer)].ToString());
                else
                    phoneCall.Add(column, DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.ServerFQDN);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add(column, dataReader[Enums.GetDescription(Enums.PhoneCalls.ServerFQDN)].ToString());
                else
                    phoneCall.Add(column, DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.PoolFQDN);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add(column, dataReader[Enums.GetDescription(Enums.PhoneCalls.PoolFQDN)].ToString());
                else
                    phoneCall.Add(column, DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.Duration);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add(column, dataReader[Enums.GetDescription(Enums.PhoneCalls.Duration)].ToString());
                else
                    phoneCall.Add(column, Convert.ToDecimal(0));


                // Insert the phonecall to designated PhoneCalls table
                // Break out of the loop if the phonecall exists
                //try
                //{
                    DBRoutines.INSERT(PhoneCallsTableName, phoneCall);
                //}
                //catch (Exception e)
                //{
                //    break;
                //}


                //Update Calls Import Status for this class every 10,000 records:
                if (phoneCallsCounter % 10000 == 0)
                {
                    CallsImportStatus.SetCallsImportStatus(
                        this.GetType().Name,
                        phoneCall[Enums.GetDescription(Enums.PhoneCalls.SessionIdTime)].ToString()
                    );
                }

                phoneCallsCounter += 1;
            }


            /***
             * Thirdly, write the LastPhonecallDate to the CallsImportStatus table.
             */
            CallsImportStatus.SetCallsImportStatus(this.GetType().Name, LastPhoneCallDate);

            sourceDBConnector.Close();
        }


        override public void ImportGateways()
        {
            OleDbCommand command;
            OleDbDataReader dataReader;
            DataTable dt;

            Dictionary<string, object> newGateway;
            List<string> existingGateways = new List<string>();

            string column = string.Empty;
            string SQL = string.Empty;
            string WHERE_STATEMENT = string.Empty;
            string SELECT_STATEMENT = string.Empty;

            SELECT_STATEMENT = String.Format(
                "SELECT [GatewayId], [Gateway] " +
                "FROM [dbo].[Gateways]"
            );

            SQL = SELECT_STATEMENT;


            /***
             * Get all the existing gateways, to avoid inserting duplicates
             */
            dt = DBRoutines.SELECT(GatewaysTableName, (new List<string>()), (new Dictionary<string, object>()), 0);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row[Enums.GetDescription(Enums.Gateways.GatewayName)] != DBNull.Value)
                    {
                        existingGateways.Add(row[Enums.GetDescription(Enums.Gateways.GatewayName)].ToString());
                    }
                }
            }


            /***
             * Setup the query which gets the gateways from the source database.
             */
            command = new OleDbCommand(SQL, sourceDBConnector);
            command.CommandTimeout = 10000;

            sourceDBConnector.Open();

            dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                column = string.Empty;
                newGateway = new Dictionary<string, object>();

                column = Enums.GetDescription(Enums.Gateways.GatewayId);
                newGateway.Add(column, (dataReader[column]).ToString());

                column = Enums.GetDescription(Enums.Gateways.GatewayName);
                newGateway.Add(column, (dataReader[column]).ToString());

                //Insert the newGateway to designated Gateways table if it doesn't exist already
                if (!existingGateways.Contains(newGateway[Enums.GetDescription(Enums.Gateways.GatewayName)]))
                {
                    DBRoutines.INSERT(GatewaysTableName, newGateway);
                }
            }

            sourceDBConnector.Close();
        }


        override public void ImportPools()
        {
            OleDbCommand command;
            OleDbDataReader dataReader;
            DataTable dt;

            Dictionary<string, object> newPool;
            List<string> existingPools = new List<string>();

            string column = string.Empty;
            string SQL = string.Empty;
            string WHERE_STATEMENT = string.Empty;
            string SELECT_STATEMENT = string.Empty;

            SELECT_STATEMENT = String.Format(
                "SELECT [PoolId], [PoolFQDN] " +
                "FROM [dbo].[Pools]"
            );


            /***
             * Get all the existing pools, to avoid inserting duplicates
             */
            dt = DBRoutines.SELECT(PoolsTableName, (new List<string>()), (new Dictionary<string, object>()), 0);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row[Enums.GetDescription(Enums.Pools.PoolFQDN)] != DBNull.Value)
                    {
                        existingPools.Add(row[Enums.GetDescription(Enums.Pools.PoolFQDN)].ToString());
                    }
                }
            }


            /***
             * Setup the query which gets the pools from the source database.
             */
            SQL = SELECT_STATEMENT;

            command = new OleDbCommand(SQL, sourceDBConnector);
            command.CommandTimeout = 10000;

            sourceDBConnector.Open();

            dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                column = string.Empty;
                newPool = new Dictionary<string, object>();

                column = Enums.GetDescription(Enums.Pools.PoolId);
                newPool.Add(column, (dataReader[column]).ToString());

                column = Enums.GetDescription(Enums.Pools.PoolFQDN);
                newPool.Add(column, (dataReader[column]).ToString());

                //Insert the newPool to designated Pools table if it doesn't exist already
                if (!existingPools.Contains(newPool[Enums.GetDescription(Enums.Pools.PoolFQDN)]))
                {
                    DBRoutines.INSERT(PoolsTableName, newPool);
                }
            }

            sourceDBConnector.Close();
        }
    }
}
