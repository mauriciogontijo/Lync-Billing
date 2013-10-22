using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lync_Backend.Interfaces
{
    interface ICallMarker
    {
        void MarkCalls();
        void MarkExclusion();
        void ApplyRates();
        void ResetPhoneCallsAttributes();
    }
}
