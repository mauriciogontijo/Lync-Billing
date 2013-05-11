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
            Viewport viewport = new Viewport();
            viewport.ID = "viewport";
            viewport.Layout ="Border";
            viewport.SetSize("100%", "100%");

            panel.Region = Region.Center;
            panel.SetSize("50%","50%");
            panel.Layout = "Border";
            panel.Frame = true;

            grid.ID = "PhoneCallsGrid";
            grid.Layout = "Fit";
            grid.SetSize(500, 300);
            
            Ext.Net.ComboBox markingStatus = new ComboBox();
            markingStatus.ID = "markingStatus";

            markingStatus.FieldLabel = "MarkingStatus";
            markingStatus.DisplayField = "MarkingStatus";
            markingStatus.ValueField = "MarkingValue";
            markingStatus.Store.Add(DBStores.PhoneCallMarkingStore());
            markingStatus.QueryMode = DataLoadMode.Local;
            markingStatus.TypeAhead = true;
            markingStatus.TriggerAction = TriggerAction.All;
            
            Toolbar toolbar = new Toolbar();
            toolbar.ID = "Toolbar";

            toolbar.Add(markingStatus);
            grid.TopBar.Add(toolbar);

            panel.Add(grid);
            viewport.Add(panel);


            form1.Controls.Add(viewport);
            
        }
     }
}