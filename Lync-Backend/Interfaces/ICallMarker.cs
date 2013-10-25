using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lync_Backend.Interfaces
{
    interface ICallMarker
    {
        void MarkCalls(string tablename);
        void MarkExclusion(string tablename);
        void ApplyRates(string tablename);
        void ResetPhoneCallsAttributes(string tablename);
    }
}
