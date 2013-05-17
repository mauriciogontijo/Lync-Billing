using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Lync_Billing.DB;
using Ext.Net;
using System.Web.SessionState;

namespace Lync_Billing.UI
{
    public partial class User_Dashboard : System.Web.UI.Page
    {
        GridPanel UserPhoneCallsHistoryGrid;

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

            UserPhoneCallsHistoryGrid = Grids.PhoneCallsGrid(columns, wherePart, 7);
            Ext.Net.TextField textField = new TextField();
            textField.ID = "SessionTime";
            UserPhoneCallsHistoryGrid.ColumnModel.Columns[0].Renderer.Handler = "return Ext.util.Format.date(value, 'Y m d');";

            UserPhoneCallsHistoryGrid.ID = "PhoneCallsGrid";
            UserPhoneCallsHistoryGrid.Layout = "Table";
            UserPhoneCallsHistoryGrid.Width = 465;
            UserPhoneCallsHistoryGrid.Height = 240;
            UserPhoneCallsHistoryGrid.ColumnModel.Columns[0].Text = "Date";
            UserPhoneCallsHistoryGrid.ColumnModel.Columns[1].Text = "Destination";
            UserPhoneCallsHistoryGrid.ColumnModel.Columns[1].Width = 165;

            UserPhoneCallsHistoryGrid.ColumnModel.Columns[3].Text = "Cost";
            UserPhoneCallsHistoryGrid.Header = true;
            UserPhoneCallsHistoryGrid.Title = "Calls History";

            UserPhoneCallsHistoryPH.Controls.Add(UserPhoneCallsHistoryGrid);


            /*
             * Configuration of the User Summary Panel
             */
            /*UsersCallsSummary userSummary = new UsersCallsSummary();
            userSummary = UsersCallsSummary.GetUsersCallsSummary(SipAccount, DateTime.Now.AddYears(-1), DateTime.Now);
            ComponentLoader.Config ComponentLoaderConfig = new ComponentLoader.Config();
            ComponentLoaderConfig = ComponentLoader.ToConfig();
            ComponentLoader UserPhoneCallsSummaryLoader = new ComponentLoader(ComponentLoaderConfig);
            this.UserPhoneCallsSummary.LoadContent(ComponentLoader.ToConfig(
                //......
            );*/
        }

        [DirectMethod]
        public static string GetCallsSummaryData()
        {
            List<AbstractComponent> comp = new List<AbstractComponent>();

            string SipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).SipAccount;

            UsersCallsSummary userSummary = new UsersCallsSummary();
            userSummary = UsersCallsSummary.GetUsersCallsSummary(SipAccount, DateTime.Now.AddYears(-1), DateTime.Now);

            Ext.Net.Panel personalCallsPanel = new Ext.Net.Panel();
            personalCallsPanel.Title = "Personal Calls Overview";
            personalCallsPanel.Icon = Icon.Phone;
            personalCallsPanel.Html= string.Format
                (
                "<div class='block-body wauto m5'><p>" + 
                        "<p>You have a total of {0} {1} phone calls, they add up to almost {2} minutes.</p>" +
                        "<p>The net calculated cost is {3} euros.</p></div>",
                        userSummary.PersonalCallsCount, "personal", userSummary.PersonalCallsDuration/=60, userSummary.PersonalCallsCost);

            Ext.Net.Panel businessCallsPanel = new Ext.Net.Panel();
            businessCallsPanel.Title = "Business Calls Overview";
            businessCallsPanel.Icon = Icon.Phone;
            businessCallsPanel.Html = string.Format
                (
                "<div class='block-body wauto m5'>" +
                        "<p>You have a total of {0} {1} phone calls, they add up to almost {2} minutes.</p>" +
                        "<p>The net calculated cost is {3} euros.</p></div>",
                        userSummary.BusinessCallsCount, "business", userSummary.BusinessCallsDuration /= 60, userSummary.BusinessCallsCost);

            Ext.Net.Panel unMarkedCallsPanel = new Ext.Net.Panel();
            unMarkedCallsPanel.Title = "Unmarked Calls Overview";
            unMarkedCallsPanel.Icon = Icon.Phone;
            unMarkedCallsPanel.Html = string.Format
                (
               "<div class='block-body wauto m5'>" +
                        "<p>You have a total of {0} {1} phone calls, they add up to almost {2} minutes.</p>" +
                        "<p>The net calculated cost is {3} euros.</p></div>",
                        userSummary.UnmarkedCallsCount, "unmarked", userSummary.UnmarkedCallsDuartion /= 60, userSummary.UnmarkedCallsCost);

            comp.Add(personalCallsPanel);
            comp.Add(businessCallsPanel);
            comp.Add(unMarkedCallsPanel);

            return ComponentLoader.ToConfig(comp);
        }
    }
}