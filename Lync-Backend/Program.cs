﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend;
using Lync_Backend.Helpers;
using Lync_Backend.Libs;
using Lync_Backend.Implementation;

namespace Lync_Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, MonitoringServersInfo> monServersInfo = new Dictionary<string, MonitoringServersInfo>();

            monServersInfo = MonitoringServersInfo.GetMonitoringServersInfo();

            //foreach (KeyValuePair<string, MonitoringServersInfo> keyValue in monServersInfo)
            //{
            //    Type type = Type.GetType("Lync_Backend.Implementation." + keyValue.Key);

            //    string fqdn = typeof(Interfaces.IDatabaseImporter).AssemblyQualifiedName;

            //    //FQN  for Lync2010: Lync_Backend.Implementation.Lync2010, Lync-Backend, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
            //    object instance = Activator.CreateInstance(type);

            //    ((Interfaces.IDatabaseImporter)instance).ImportPools();
            //    ((Interfaces.IDatabaseImporter)instance).ImportGateways();
            //    ((Interfaces.IDatabaseImporter)instance).ImportPhoneCalls();
            //}

            //Interfaces.IDatabaseImporter lync2010 = new Lync2010();
            //lync2010.ImportPhoneCalls();

            //Interfaces.ICallMarker callsMarker = new CallMarker_Lync2010();
            //callsMarker.ApplyRates("PhoneCalls2010");

            Interfaces.IDatabaseImporter lync2013 = new Lync2013();
            lync2013.ImportPhoneCalls();

            Interfaces.ICallMarker callsMarker = new CallMarker_Lync2013();
            callsMarker.MarkCalls("PhoneCalls2013");
        }
    }
}
