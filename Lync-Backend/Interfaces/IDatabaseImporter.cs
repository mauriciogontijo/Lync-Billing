﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Helpers;

namespace Lync_Backend.Interfaces
{
    interface IDatabaseImporter
    {
        public void ImportPhoneCalls(MonitoringServersInfo info);
        public void ImportGateways(MonitoringServersInfo info);
        public void ImportPools(MonitoringServersInfo info);
    }
}
