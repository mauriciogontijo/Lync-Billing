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
    public partial class User_Home : System.Web.UI.Page
    {
        GridPanel UserPhoneCallsSummaryGrid;
        private static string SipAccount { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
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

            UserPhoneCallsSummaryGrid = Grids.PhoneCallsGrid(columns, wherePart, 7);
            Ext.Net.TextField textField = new TextField();
            textField.ID = "SessionTime";
            UserPhoneCallsSummaryGrid.ColumnModel.Columns[0].Renderer.Handler = "return Ext.util.Format.date(value, 'Y-m-d');";

            UserPhoneCallsSummaryGrid.ID = "PhoneCallsGrid";
            UserPhoneCallsSummaryGrid.Layout = "Table";
            UserPhoneCallsSummaryGrid.Width = 465;
            UserPhoneCallsSummaryGrid.ColumnModel.Columns[0].Text = "Date";
            UserPhoneCallsSummaryGrid.ColumnModel.Columns[1].Text = "Destination";
            UserPhoneCallsSummaryGrid.ColumnModel.Columns[1].Width = 165;

            UserPhoneCallsSummaryGrid.ColumnModel.Columns[3].Text = "Cost";
            UserPhoneCallsSummaryGrid.Header = true;
            UserPhoneCallsSummaryGrid.Title = "Calls History";

            UserPhoneCallsSummaryPH.Controls.Add(UserPhoneCallsSummaryGrid);
        }
    }
}