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
using System.Data;

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

        override public string PhoneCallsTableName { get; set; }

        override public string PoolsTableName
        {
            get { return "Pools"; }
        }

        override public string GatewaysTableName
        {
            get { return "Gateways"; }
        }

        override public string ConstructConnectionString()
        {
            monInfo = MonitoringServersInfo.GetMonitoringServersInfo();
            var info = monInfo.Where(item => item.Key == this.GetType().Name).Select(item => (MonitoringServersInfo)item.Value).First() as MonitoringServersInfo;

            PhoneCallsTableName = info.PhoneCallsTable;

            return MonitoringServersInfo.CreateConnectionString(info);
        }

        override public void ImportPhoneCalls()
        {
          
            OleDbDataReader dataReader;
            OleDbConnection DestinationDBConnector = new OleDbConnection(ConfigurationManager.ConnectionStrings["LyncConnectionString"].ConnectionString);

            Dictionary<string, object> phoneCall;

            string LAST_IMPORTED_PHONECALL_DATE = string.Empty;

            string column = string.Empty;

            //OPEN CONNECTIONS
            sourceDBConnector.Open();
            DestinationDBConnector.Open();

            dataReader = DBRoutines.EXECUTEREADER(SQLs.CREATE_LAST_IMPORTED_PHONECALL_DATE_QUERY(PhoneCallsTableName), DestinationDBConnector);

            if (dataReader.Read() && !dataReader.IsDBNull(0))
                LAST_IMPORTED_PHONECALL_DATE = Misc.ConvertDate((DateTime)dataReader[Enums.GetDescription(Enums.PhoneCalls.SessionIdTime)]);
            else
                LAST_IMPORTED_PHONECALL_DATE = null;


            //Construct CREATE_IMPORT_PHONE_CALLS_QUERY
            string SQL = SQLs.CREATE_IMPORT_PHONE_CALLS_QUERY(LAST_IMPORTED_PHONECALL_DATE);
            
            dataReader = DBRoutines.EXECUTEREADER(SQL, sourceDBConnector);

            while (dataReader.Read())
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

                column = Enums.GetDescription(Enums.PhoneCalls.OnBehalf);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("OnBehalf", dataReader[Enums.GetDescription(Enums.PhoneCalls.OnBehalf)].ToString());
                else
                    phoneCall.Add("OnBehalf", DBNull.Value);

                column = Enums.GetDescription(Enums.PhoneCalls.ReferredBy);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("ReferedBy", dataReader[Enums.GetDescription(Enums.PhoneCalls.ReferredBy)].ToString());
                else
                    phoneCall.Add("ReferedBy", DBNull.Value);


                column = Enums.GetDescription(Enums.PhoneCalls.Duration);
                if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                    phoneCall.Add("Duration", dataReader[Enums.GetDescription(Enums.PhoneCalls.Duration)].ToString());
                else
                    phoneCall.Add("Duration", Convert.ToDecimal(0));

               

                try
                {
                    DBRoutines.INSERT(PhoneCallsTableName, phoneCall);
                }
                catch (Exception e)
                {
                    break;
                }
            }

            sourceDBConnector.Close();
        }


        public override void ImportGatewaysAndPools()
        {
            OleDbDataReader dataReader;
            DataTable dt;

            string column = string.Empty;

            // Varialbes for handling importing Gateways.
            Dictionary<string, object> newGateway;
            List<string> existingGateways = new List<string>();

            // Varialbes for handling importing Pools.
            Dictionary<string, object> newPool;
            List<string> existingPools = new List<string>();


            /***
             * Get all the existing gateways, to avoid inserting duplicates
             */
            dt = new DataTable();
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
             * Get all the existing pools, to avoid inserting duplicates
             */
            dt.Clear();
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
             * Open a connection to the database
             */
            sourceDBConnector.Open();


            /**
             * Start by importing the Gateways.
             */
            dataReader = DBRoutines.EXECUTEREADER(SQLs.CREATE_IMPORT_GATEWAYS_QUERY(), sourceDBConnector);

            while (dataReader.Read())
            {
                column = string.Empty;
                newGateway = new Dictionary<string, object>();

                //column = Enums.GetDescription(Enums.Gateways.GatewayId);
                //newGateway.Add(column, (dataReader[column]).ToString());

                column = Enums.GetDescription(Enums.Gateways.GatewayName);
                newGateway.Add(column, (dataReader[column]).ToString());

                //Insert the newGateway to designated Gateways table if it doesn't exist already
                if (!existingGateways.Contains(newGateway[Enums.GetDescription(Enums.Gateways.GatewayName)]))
                {
                    DBRoutines.INSERT(GatewaysTableName, newGateway);
                }
            }


            /**
             * Procede by importing the Pools.
             */
            dataReader = DBRoutines.EXECUTEREADER(SQLs.CREATE_IMPORT_POOLS_QUERY(), sourceDBConnector);

            while (dataReader.Read())
            {
                column = string.Empty;
                newPool = new Dictionary<string, object>();

                //column = Enums.GetDescription(Enums.Pools.PoolId);
                //newPool.Add(column, (dataReader[column]).ToString());

                column = Enums.GetDescription(Enums.Pools.PoolFQDN);
                newPool.Add(column, (dataReader[column]).ToString());

                //Insert the newPool to designated Pools table if it doesn't exist already
                if (!existingPools.Contains(newPool[Enums.GetDescription(Enums.Pools.PoolFQDN)]))
                {
                    DBRoutines.INSERT(PoolsTableName, newPool);
                }
            }


            /***
             * Close the database connection
             */
            sourceDBConnector.Close();
        }

    }
}
