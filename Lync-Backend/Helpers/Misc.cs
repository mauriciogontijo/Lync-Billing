using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lync_Backend.Helpers
{
    public class Misc
    {
        public static string ConvertDate(DateTime datetTime)
        {
            return datetTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
    }
}
