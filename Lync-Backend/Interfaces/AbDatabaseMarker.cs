using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lync_Backend.Interfaces
{
    abstract class AbDatabaseMarker : ICallMarker
    {
        
        public abstract void MarkCalls(string tablename);
        public abstract void MarkExclusion(string tablename);

        public abstract void ApplyRates(string tablename);
        public abstract void ResetPhoneCallsAttributes(string tablename);
    }
}
