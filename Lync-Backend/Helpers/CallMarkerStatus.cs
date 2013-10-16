using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lync_Backend.Helpers
{
    class CallMarkerStatus
    {
        public int MarkerId { get; set; }
        public string PhoneCallsTable { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
