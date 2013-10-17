using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Libs;

namespace Lync_Backend.Helpers
{
    class DIDs
    {
        public int id { get; set; }
        public string did { get; set; }
        public string description { get; set; }

        private static DBLib DBRoutines = new DBLib();



    }
}
