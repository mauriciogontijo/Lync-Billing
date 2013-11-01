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

                ((AbIdDatabaseImporter)instance).ImportGatewaysAndPools();
                ((AbIdDatabaseImporter)instance).ImportPhoneCalls();

                string tableName = ((Interfaces.AbIdDatabaseImporter)instance).PhoneCallsTableName;

                Interfaces.ICallMarker callsMarker = new CallMarker();
                callsMarker.MarkCalls(tableName);
            }

            
            
            //Interfaces.IDatabaseImporter lync2013 = new Lync2013();
            //lync2013.ImportPhoneCalls();

            //Interfaces.IDatabaseImporter lync2013 = new Lync2013();
            //lync2013.ImportGatewaysAndPools();

           

            //Interfaces.ICallMarker callsMarker = new CallMarker_Lync2013();
            //callsMarker.ApplyRates();

        }
    }
}
