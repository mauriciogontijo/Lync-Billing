using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lync_Billing.UI
{
    public partial class FilterExample : System.Web.UI.Page
    {
        GridPanel grid;
        protected void Page_Load(object sender, EventArgs e)
        {

            string sipAccount = "SGhaida@ccc.gr";
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            List<string> columns = new List<string>();

            wherePart.Add("SourceUserUri", SipAccount);
            columns.Add("SessionIdTime");
            columns.Add("DestinationNumberUri");
            columns.Add("Duration");
            columns.Add("marker_CallCost");

            Ext.Net.Panel panel = new Ext.Net.Panel();
            panel.ID = "GridPanel";

            grid = Grids.PhoneCallsGrid(columns, wherePart, 7);
            Ext.Net.TextField textField = new TextField();
            textField.ID = "SessionTime";
            grid.ColumnModel.Columns[0].Renderer.Handler = "return Ext.util.Format.date(value, 'Y-m-d');";

            grid.ID = "PhoneCallsGrid";
            grid.Layout = "Table";
            grid.Width = 465;
            grid.ColumnModel.Columns[0].Text = "Date";
            grid.ColumnModel.Columns[1].Text = "Destination";
            grid.ColumnModel.Columns[1].Width = 165;

            grid.ColumnModel.Columns[3].Text = "Cost";
            grid.Header = true;
            grid.Title = "Calls History";

            ph.Controls.Add(grid);

        }
    }
}