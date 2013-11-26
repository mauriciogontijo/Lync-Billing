using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend;
using Lync_Backend.Helpers;
using Lync_Backend.Libs;
using Lync_Backend.Implementation;
using Lync_Backend.Interfaces;
using System.Configuration;
using System.Data.OleDb;

using System.Runtime.InteropServices;namespace Lync_Backend
{
    class Program
    {
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_HIDE = 0;

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {

            //IntPtr winHandle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            //ShowWindow(winHandle, SW_HIDE);

            Dictionary<string, MonitoringServersInfo> monServersInfo = new Dictionary<string, MonitoringServersInfo>();

            monServersInfo = MonitoringServersInfo.GetMonitoringServersInfo();

            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monServersInfo)
            {
                Type type = Type.GetType("Lync_Backend.Implementation." + keyValue.Key);

                //Check if there is an existing phonecalls table 
                using (OleDbConnection sourceDBConnector = new OleDbConnection(ConfigurationManager.ConnectionStrings["LyncConnectionString"].ConnectionString))
                {
                    OleDbCommand comm = new OleDbCommand(SQLs.CREATE_VALIDATE_DB_OBJECT_QUERY(keyValue.Value.PhoneCallsTable), sourceDBConnector);

                    try
                    {
                        sourceDBConnector.Open();

                        string result = comm.ExecuteScalar().ToString();

                        //Define the query Type
                        if (string.IsNullOrEmpty(result))
                        {
                            //PhoneCalls Table does not Exists, so we need to create it 
                            comm.CommandText = SQLs.CREATE_PHONECALLS_TABLE_QUERY(keyValue.Value.PhoneCallsTable);
                            comm.ExecuteNonQuery();
                        }

                    }
                    catch (Exception ex)
                    {
                        string x = string.Empty;
                    }
                }
                string fqdn = typeof(AbIdDatabaseImporter).AssemblyQualifiedName;

                //FQN  for Lync2010: Lync_Backend.Implementation.Lync2010, Lync-Backend, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
                object instance = Activator.CreateInstance(type);

                ((AbIdDatabaseImporter)instance).ImportGatewaysAndPools();
                ((AbIdDatabaseImporter)instance).ImportPhoneCalls();
            }

            //string tableName = ((Interfaces.AbIdDatabaseImporter)instance).PhoneCallsTableName;

            //Interfaces.ICallMarker callsMarker = new CallMarker();
            //callsMarker.MarkCalls(tableName);
            //callsMarker.ApplyRates(tableName);


            //Interfaces.ICallMarker callsMarker = new CallMarker();
            //callsMarker.MarkCalls("PhoneCalls2010");
            //callsMarker.ApplyRates("PhoneCalls2010");

            Console.WriteLine("Creating DB Functions");

            CreateDBStatistics.Get_ChargeableCalls_ForUser();
            CreateDBStatistics.Get_ChargeableCalls_ForSite();

            CreateDBStatistics.Get_CallsSummary_ForUser();
            CreateDBStatistics.Get_CallsSummary_ForUsers_PerSite();
            CreateDBStatistics.Get_CallsSummary_ForUsers_PerSite_PDF();
            CreateDBStatistics.Get_CallsSummary_ForSiteDepartment();


            CreateDBStatistics.Get_DestinationCountries_ForUser();
            CreateDBStatistics.Get_DestinationCountries_ForSite();
            CreateDBStatistics.Get_DestinationCountries_ForSiteDepartment();

            CreateDBStatistics.Get_DestinationsNumbers_ForUser();
            CreateDBStatistics.Get_DestinationsNumbers_ForSite();
            CreateDBStatistics.Get_DestinationsNumbers_ForSiteDepartment();

            CreateDBStatistics.Get_GatewaySummary_PerUser();
            CreateDBStatistics.Get_GatewaySummary_PerSite();
            CreateDBStatistics.Get_GatewaySummary_PerSiteDepartment();
            CreateDBStatistics.Get_GatewaySummary_ForAll_Sites();

            CreateDBStatistics.Get_MailStatistics_ForUsers_PerSiteDepartment();
            CreateDBStatistics.Get_MailStatistics_PerSiteDepartment();
        }
    }
}
