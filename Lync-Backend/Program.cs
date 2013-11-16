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
using Lync_Backend.Interfaces;

namespace Lync_Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, MonitoringServersInfo> monServersInfo = new Dictionary<string, MonitoringServersInfo>();

            monServersInfo = MonitoringServersInfo.GetMonitoringServersInfo();

            foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monServersInfo)
            {
                Type type = Type.GetType("Lync_Backend.Implementation." + keyValue.Key);

                string fqdn = typeof(AbIdDatabaseImporter).AssemblyQualifiedName;

                //FQN  for Lync2010: Lync_Backend.Implementation.Lync2010, Lync-Backend, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
                object instance = Activator.CreateInstance(type);

                //((AbIdDatabaseImporter)instance).ImportGatewaysAndPools();
                ((AbIdDatabaseImporter)instance).ImportPhoneCalls();

                //string tableName = ((Interfaces.AbIdDatabaseImporter)instance).PhoneCallsTableName;

                //Interfaces.ICallMarker callsMarker = new CallMarker();
                //callsMarker.MarkCalls(tableName);
                //callsMarker.ApplyRates(tableName);

            }


            //Interfaces.ICallMarker callsMarker = new CallMarker();
            //callsMarker.MarkCalls("PhoneCalls2010");
            //callsMarker.ApplyRates("PhoneCalls2010");


            //CreateDBStatistics.Get_ChargeableCalls_ForUser();
            //CreateDBStatistics.Get_ChargeableCalls_ForSite();

            //CreateDBStatistics.Get_CallsSummary_ForUser();
            //CreateDBStatistics.Get_CallsSummary_ForUsers_PerSite();
            //CreateDBStatistics.Get_CallsSummary_ForUsers_PerSite_PDF();
            //CreateDBStatistics.Get_CallsSummary_ForSiteDepartment();


            //CreateDBStatistics.Get_DestinationCountries_ForUser();
            //CreateDBStatistics.Get_DestinationCountries_ForSite();
            //CreateDBStatistics.Get_DestinationCountries_ForSiteDepartment();

            //CreateDBStatistics.Get_DestinationsNumbers_ForUser();
            //CreateDBStatistics.Get_DestinationsNumbers_ForSite();
            //CreateDBStatistics.Get_DestinationsNumbers_ForSiteDepartment();

            //CreateDBStatistics.Get_GatewaySummary_PerUser();
            //CreateDBStatistics.Get_GatewaySummary_PerSite();
            //CreateDBStatistics.Get_GatewaySummary_PerSiteDepartment();
            //CreateDBStatistics.Get_GatewaySummary_ForAll_Sites();

            //CreateDBStatistics.Get_MailStatistics_ForUsers_PerSiteDepartment();
            //CreateDBStatistics.Get_MailStatistics_PerSiteDepartment();
            //ICallMarker callsMarker = new CallMarker();
            //callsMarker.ApplyRates("PhoneCalls2013");
            //callsMarker.ApplyRates("PhoneCalls2010");

        }
    }
}
