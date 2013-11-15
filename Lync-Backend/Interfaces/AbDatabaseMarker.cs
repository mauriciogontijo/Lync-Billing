using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Helpers;

namespace Lync_Backend.Interfaces
{
    abstract class AbDatabaseMarker : ICallMarker
    {

        public abstract void MarkCalls(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null);
        public abstract void MarkCalls(string tableName, ref PhoneCalls phoneCall,ref Type type);

        public abstract void MarkExclusion(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null);
        public abstract void MarkExclusion(string tableName, ref  PhoneCalls phoneCall);

        public abstract void ApplyRates(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null);
        public abstract void ApplyRates(string tableName, ref PhoneCalls phoneCall);

        public abstract void ResetPhoneCallsAttributes(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null);
        public abstract void ResetPhoneCallsAttributes(string tableName, ref  PhoneCalls phoneCall);
    }
}
