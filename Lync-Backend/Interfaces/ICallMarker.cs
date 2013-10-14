using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lync_Backend.Interfaces
{
    interface ICallMarker
    {
        void MarkCalls(string tableName);
        void MarkExclusion(string tableName);
        void ApplyRates(string tableName);
        void ResetPhoneCallsAttributes(string tableName);
    }
}
