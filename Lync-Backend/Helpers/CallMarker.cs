using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lync_Backend.Helpers
{
    public class CallMarker
    {
        public static void MarkCalls(DateTime? optionalFrom = null, DateTime? optionalTo = null, string gateway = null)
        {
            DateTime from = optionalFrom != null ? optionalFrom.Value : DateTime.MinValue;
            DateTime to = optionalTo != null ? optionalTo.Value : DateTime.MaxValue;

            if (string.IsNullOrEmpty(gateway))
            {

            }
            else
            {

            }
        }
    }
}
