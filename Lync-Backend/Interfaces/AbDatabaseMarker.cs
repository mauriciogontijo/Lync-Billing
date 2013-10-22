using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lync_Backend.Interfaces
{
    abstract class AbDatabaseMarker : ICallMarker
    {
        public abstract string PhoneCallsTableName { get; }
        public abstract string GatewaysTableName { get; }

        public abstract void MarkCalls();
        public abstract void MarkExclusion();

        public abstract void ApplyRates();
        public abstract void ResetPhoneCallsAttributes();
    }
}
