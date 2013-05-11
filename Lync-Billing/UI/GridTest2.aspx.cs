using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lync_Billing.DB;
using Ext.Net;

namespace Lync_Billing.UI
{
    public partial class GridTest2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sipAccount = "SGhaida@ccc.gr";
            Dictionary<string,object> wherePart = new Dictionary<string,object>();
            List<string> columns = new List<string>();

            wherePart.Add("SourceUserUri",sipAccount);
            columns.Add("SessionIdTime");
            columns.Add("ResponseTime");
            columns.Add("DestinationNumberUri");
            columns.Add("Duration");
            columns.Add("PoolFQDN");

           //Store store = DBStores.PhoneCallsStore(columns,wherePart,10);

            Ext.Net.Panel panel = new Ext.Net.Panel();
            panel.ID = "GridPanel";

            GridPanel grid = Grids.PhoneCallsGrid(columns, wherePart, 10);
        
            form1.Controls.Add(grid);
            
        }
     }
}