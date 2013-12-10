using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Helpers;

namespace Lync_Backend.Interfaces
{
    interface IPhoneCalls
    {
        PhoneCalls SetCallType(PhoneCalls thisCall);

        PhoneCalls ApplyRate(PhoneCalls thisCall);

        PhoneCalls ApplyExclusions(PhoneCalls thisCalls);
    }
}
