using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;

namespace Lync_Billing.DB
{
    
    public class Gateways
    {

        public int GatewayID { set; get; }
        public string GatewayName { set; get; }

        public int INSERT()
        {
            return 0;
        }

        public List<Gateways> SELECT()
        {
            List<Gateways> gateways = new List<Gateways>();

            return gateways;
        }

        public void UPDATE()
        {

        }

        public void DELETE()
        {

        }
    }

   
}