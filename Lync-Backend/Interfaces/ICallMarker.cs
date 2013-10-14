using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lync_Backend.Interfaces
{
    interface ICallMarker
    {
        public void MarkCalls(string tableName);
        public void MarkExclusion(string tableName);
        public void ApplyRates(string tableName);
        public void ResetPhoneCallsAttributes(string tableName);
    }
}
