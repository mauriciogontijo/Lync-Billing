using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lync_Billing.DB
{
    public class Pools
    {
        public int PoolID { set; get; }
        public string PoolName { set; get; }

        public int INSERT()
        {
            return 0;
        }

        public List<Pools> SELECT()
        {
            List<Pools> pools = new List<Pools>();

            return pools;
        }

        public void UPDATE()
        {

        }

        public void DELETE()
        {

        }
    }
}