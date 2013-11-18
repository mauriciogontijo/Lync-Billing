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
using System.Reflection;

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
            CallMarker callsMarker = new CallMarker();

            PhoneCalls phoneCallObj;
            Dictionary<string, object> phoneCallDic;

            int dataRowCounter = 0;
            string lastMarkedPhoneCallDate = string.Empty;
            string column = string.Empty;
            string LAST_IMPORTED_PHONECALL_DATE = string.Empty;

            Type type = Type.GetType("Lync_Backend.Implementation." + PhoneCallsTableName);

            //Class Loader
            ConstructorInfo cinfo = type.GetConstructor(new Type[] { });
            object instance = cinfo.Invoke(new object[] { });

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

            if (!string.IsNullOrEmpty(LAST_IMPORTED_PHONECALL_DATE))
                Console.WriteLine("Importing PhoneCalls from " + PhoneCallsTableName + "since " + LAST_IMPORTED_PHONECALL_DATE);
            else
                Console.WriteLine("Importing PhoneCalls from " + PhoneCallsTableName + "since the begining");

            while (dataReader.Read())
            {
                lastMarkedPhoneCallDate = string.Empty;

                phoneCallObj = Misc.FillPhoneCallFromOleDataReader(dataReader);
                
                ((Interfaces.IPhoneCalls)instance).SetCallType(phoneCallObj);
                ((Interfaces.IPhoneCalls)instance).ApplyRate(phoneCallObj);

                if (!string.IsNullOrEmpty(phoneCallObj.ReferredBy))
                    phoneCallObj.ChargingParty = phoneCallObj.ReferredBy;
                else
                    phoneCallObj.ChargingParty = phoneCallObj.SourceUserUri;
               
                lastMarkedPhoneCallDate = phoneCallObj.SessionIdTime;

                phoneCallDic = Misc.ConvertPhoneCallToDictionary(phoneCallObj);

                try
                {
                    DBRoutines.INSERT(PhoneCallsTableName, phoneCallDic);

                    //Update the CallMarkerStatus table fro this PhoneCall table.
                    if (dataRowCounter % 10000 == 0)
                    {
                        callsMarker.UpdateCallMarkerStatus(PhoneCallsTableName, "Marking", lastMarkedPhoneCallDate,  DestinationDBConnector);
                        callsMarker.UpdateCallMarkerStatus(PhoneCallsTableName, "ApplyingRates", lastMarkedPhoneCallDate, DestinationDBConnector);
                    }

                    dataRowCounter += 1;
                }
                catch (Exception e)
                {
                    break;
                }
            }

            callsMarker.UpdateCallMarkerStatus(PhoneCallsTableName, "Marking", lastMarkedPhoneCallDate, DestinationDBConnector);
            callsMarker.UpdateCallMarkerStatus(PhoneCallsTableName, "ApplyingRates", lastMarkedPhoneCallDate, DestinationDBConnector);

            sourceDBConnector.Close();

            Console.WriteLine("Finish importing Calls from " + PhoneCallsTableName);
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
