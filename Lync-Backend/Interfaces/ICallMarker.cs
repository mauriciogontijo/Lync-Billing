﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lync_Backend.Interfaces
{
    interface ICallMarker
    {
        void MarkCalls(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null);
        void MarkExclusion(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null);
        void ApplyRates(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null);
        void ResetPhoneCallsAttributes(string tablename, DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null);
    }
}
