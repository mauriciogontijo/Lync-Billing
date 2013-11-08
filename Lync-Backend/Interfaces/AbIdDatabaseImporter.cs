using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lync_Backend.Interfaces
{
    public abstract class AbIdDatabaseImporter :IDatabaseImporter
    {
        public abstract string PhoneCallsTableName { get; set; }
        public abstract string PoolsTableName { get; }
        public abstract string GatewaysTableName { get; }

        public abstract string ConstructConnectionString();
        public abstract void ImportPhoneCalls();
        public abstract void ImportGatewaysAndPools();
        
    }
}
