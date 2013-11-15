using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Helpers;

namespace Lync_Backend.Interfaces
{
    interface ICallMarker
    {
        void MarkCalls(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null);
        void MarkCalls(ref PhoneCalls phoneCall);
        
        void MarkExclusion(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null);
        void MarkExclusion(ref PhoneCalls phoneCall);
        
        void ApplyRates(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null);
        void ApplyRates(ref PhoneCalls phoneCall);

        void ResetPhoneCallsAttributes(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null);
        void ResetPhoneCallsAttributes(ref PhoneCalls phoneCall);
    }
}
