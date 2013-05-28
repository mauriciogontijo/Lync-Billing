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
    public partial class GridTest3 : System.Web.UI.Page
    {
        Dictionary<string, object> wherePart = new Dictionary<string, object>();
        List<string> columns = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string SipAccount = "AAlhour@ccc.gr"; //((UserSession)Session.Contents["UserData"]).SipAccount;
            /*Dictionary<string, object> wherePart = new Dictionary<string, object>();
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
            UserPhoneCallsHistoryGrid.ColumnModel.Columns[0].Renderer.Handler = "return Ext.util.Format.date(value, 'Y-m-d');";

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

            UserPhoneCallsHistoryPH.Controls.Add(UserPhoneCallsHistoryGrid);*/

            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            List<string> columns = new List<string>();

            wherePart.Add("SourceUserUri", SipAccount);
            wherePart.Add("marker_CallTypeID", 1);

            columns.Add("SessionIdTime");
            columns.Add("DestinationNumberUri");
            columns.Add("Duration");
            columns.Add("marker_CallToCountry");

            PhoneCallsHistoryStore.DataSource = PhoneCall.GetPhoneCalls(columns, wherePart, 5);
            PhoneCallsHistoryStore.DataBind();
        }

        [DirectMethod]
        public static string GetSummaryData()
        {
            string SipAccount = "AAlhour@ccc.gr"; //((UserSession)Session.Contents["UserData"]).SipAccount;

            UsersCallsSummary userSummary = new UsersCallsSummary();
            userSummary = UsersCallsSummary.GetUsersCallsSummary(SipAccount, DateTime.Now.AddYears(-1), DateTime.Now);

            return ComponentLoader.ToConfig(new List<AbstractComponent>() {
                new Ext.Net.Panel { 
                    Title="Personal Calls Overview",
                    Icon = Icon.Phone,
                    Html = String.Format(
                        "<div class='block-body wauto m5'><p>" + 
                        "<p>You have a total of {0} {1} phone calls, they add up to almost {2} minutes.</p>" +
                        "<p>The net calculated cost is {3} euros.</p></div>",
                        userSummary.PersonalCallsCount, "personal", userSummary.PersonalCallsDuration/=60, userSummary.PersonalCallsCost)
                },

                new Ext.Net.Panel { 
                    Title="Business Calls Overview",
                    Icon = Icon.Phone,
                    Html = String.Format(
                        "<div class='block-body wauto m5'>" + 
                        "<p>You have a total of {0} {1} phone calls, they add up to almost {2} minutes.</p>" + 
                        "<p>The net calculated cost is {3} euros.</p></div>",
                        userSummary.BusinessCallsCount, "business", userSummary.BusinessCallsDuration/=60, userSummary.BusinessCallsCost)
                },

                new Ext.Net.Panel { 
                    Title="Unmarked Calls Overview",
                    Icon = Icon.Phone,
                    Html = String.Format(
                        "<div class='block-body wauto m5'>" + 
                        "<p>You have a total of {0} {1} phone calls, they add up to almost {2} minutes.</p>" + 
                        "<p>The net calculated cost is {3} euros.</p></div>",
                        userSummary.UnmarkedCallsCount, "unmarked", userSummary.UnmarkedCallsDuartion/=60, userSummary.UnmarkedCallsCost)
                }
            });
        }

        /*protected string format_summary_html_contents(int total_calls, int total_cost, int total_duration, string phone_calls_type) 
        {
            //convert the total duration from seconds to minutes.
            total_duration /= 60;

            return String.Format(
                "You have a total of {0} {1}, they add up to almost {2} minutes. The net calculated cost is {3} euros.",
                total_calls, phone_calls_type, total_duration, total_cost
            );
        }*/
    }
}