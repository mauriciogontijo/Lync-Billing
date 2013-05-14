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
    public partial class User_Inner : System.Web.UI.Page
    {
        GridPanel UserPhoneCallsHistoryGrid;
        protected void Page_Load(object sender, EventArgs e)
        {
            PlaceHolder1.Visible = false;
            UserPhoneCallsHistoryPH.Visible = false;
        }

        protected void view_call_history_Click(object sender, EventArgs e)
        {
            UserPhoneCallsHistoryPH.Visible = true;
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            List<string> columns = new List<string>();

            wherePart.Add("SourceUserUri", SipAccount);
            columns.Add("SessionIdTime");
            columns.Add("marker_CallToCountry");
            columns.Add("DestinationNumberUri");
            columns.Add("Duration");
            columns.Add("marker_CallCost");
            columns.Add("ui_IsPersonal");
            columns.Add("ui_MarkedOn");
            columns.Add("ui_IsInvoiced");

            UserPhoneCallsHistoryGrid = Grids.PhoneCallsGrid(columns, wherePart, 0);
            Ext.Net.TextField textField = new TextField();
            textField.ID = "SessionTime";
            UserPhoneCallsHistoryGrid.ColumnModel.Columns[0].Renderer.Handler = "return Ext.util.Format.date(value, 'Y-m-d');";

            UserPhoneCallsHistoryGrid.ID = "PhoneCallsGrid";
            UserPhoneCallsHistoryGrid.Layout = "Table";
            UserPhoneCallsHistoryGrid.Width = 745;
            UserPhoneCallsHistoryGrid.Height = 400;
            UserPhoneCallsHistoryGrid.Scroll = ScrollMode.Both;

            UserPhoneCallsHistoryGrid.ColumnModel.Columns[0].Text = "Date";
            UserPhoneCallsHistoryGrid.ColumnModel.Columns[0].Width = 80;

            UserPhoneCallsHistoryGrid.ColumnModel.Columns[1].Text = "Country Code";
            UserPhoneCallsHistoryGrid.ColumnModel.Columns[1].Width = 90;

            UserPhoneCallsHistoryGrid.ColumnModel.Columns[2].Text = "Destination";
            UserPhoneCallsHistoryGrid.ColumnModel.Columns[2].Width = 130;

            UserPhoneCallsHistoryGrid.ColumnModel.Columns[3].Text = "Duration";
            UserPhoneCallsHistoryGrid.ColumnModel.Columns[3].Width = 70;

            UserPhoneCallsHistoryGrid.ColumnModel.Columns[4].Text = "Cost";
            UserPhoneCallsHistoryGrid.ColumnModel.Columns[4].Width = 70;

            UserPhoneCallsHistoryGrid.ColumnModel.Columns[5].Text = "Type";
            UserPhoneCallsHistoryGrid.ColumnModel.Columns[5].Width = 100;

            UserPhoneCallsHistoryGrid.ColumnModel.Columns[6].Text = "Updated On";
            UserPhoneCallsHistoryGrid.ColumnModel.Columns[6].Width = 100;

            UserPhoneCallsHistoryGrid.ColumnModel.Columns[7].Text = "Billing Status";
            UserPhoneCallsHistoryGrid.ColumnModel.Columns[7].Width = 100;

            UserPhoneCallsHistoryGrid.Header = true;
            UserPhoneCallsHistoryGrid.Title = "Calls History";
            UserPhoneCallsHistoryGrid.AutoScroll = true;
            
            Ext.Net.GridView gridview = new Ext.Net.GridView();
            gridview.AutoScroll = true;

            UserPhoneCallsHistoryGrid.View.Add(gridview);

            //LiveSearchGridPanel searchPanel = new LiveSearchGridPanel();
            //searchPanel.Listeners.Search.Handler = "if(count>0){#{statusbar}.setStatus({text: count + ' matche(s) found.', iconCls: 'x-status-valid'});}";
            //searchPanel.Listeners.RegExpError.Handler = "#{statusbar}.setStatus({text: message, iconCls: 'x-status-error'});";
            //searchPanel.Listeners.BeforeSearch.Handler = "#{statusbar}.setStatus({text: 'Nothing Found', iconCls: ''});";

            //LiveSearchToolbar searchToolBar = new LiveSearchToolbar();
            //StatusBar statusbar = new StatusBar();
            //statusbar.ID = "statusBar";

            //UserPhoneCallsHistoryGrid.Plugins.Add(searchPanel);
            //UserPhoneCallsHistoryGrid.TopBar.Add(searchToolBar);

            PagingToolbar pagingToolBar = new PagingToolbar();
            //pagingToolBar.DisplayMsg = "Phone Calls";

            UserPhoneCallsHistoryGrid.BottomBar.Add(pagingToolBar);
            //UserPhoneCallsHistoryGrid.BottomBar.Add(statusbar);

            UserPhoneCallsHistoryPH.Controls.Add(UserPhoneCallsHistoryGrid);
                
        }

        protected void TEST1_Click(object sender, EventArgs e)
        {
            UserPhoneCallsHistoryPH.Visible = false;
            PlaceHolder1.Visible = true;
        }
      
    }
}