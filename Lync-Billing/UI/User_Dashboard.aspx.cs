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
        }


        [DirectMethod]
        public static string GetSummaryData()
        {
            string SipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).SipAccount;

            UsersCallsSummary userSummary = new UsersCallsSummary();
            userSummary = UsersCallsSummary.GetUsersCallsSummary(SipAccount, DateTime.Now.AddYears(-1), DateTime.Now);
            
            return ComponentLoader.ToConfig(new List<AbstractComponent>() {
                new Ext.Net.Panel { 
                    Title="Personal Calls Overview",
                    Icon = Icon.Phone,
                    Html = String.Format(
                        "<div class='block-body wauto m15 p5'><p>" + 
                        "<p class='line-height-1-7 mb15'>You have a total of <span class='bold'>{0} {1}</span> phone calls, and they all add up to a total duration of almost <span class='bold'>{2} minutes</span>.</p>" +
                        "<p class='line-height-1-7 mb10'>The net calculated <span class='bold'>cost is {3} euros</span>.</p></div>",
                        userSummary.PersonalCallsCount, "personal", userSummary.PersonalCallsDuration/=60, userSummary.PersonalCallsCost)
                },

                new Ext.Net.Panel { 
                    Title="Business Calls Overview",
                    Icon = Icon.Phone,
                    Html = String.Format(
                        "<div class='block-body wauto m15 p5'><p>" + 
                        "<p class='line-height-1-7 mb15'>You have a total of <span class='bold'>{0} {1}</span> phone calls, and they all add up to a total duration of almost <span class='bold'>{2} minutes</span>.</p>" +
                        "<p class='line-height-1-7 mb10'>The net calculated <span class='bold'>cost is {3} euros</span>.</p></div>",
                        userSummary.BusinessCallsCount, "business", userSummary.BusinessCallsDuration/=60, userSummary.BusinessCallsCost)
                },

                new Ext.Net.Panel { 
                    Title="Unmarked Calls Overview",
                    Icon = Icon.Phone,
                    Html = String.Format(
                        "<div class='block-body wauto m15 p5'><p>" + 
                        "<p class='line-height-1-7 mb15'>You have a total of <span class='bold'>{0} {1}</span> phone calls, and they all add up to a total duration of almost <span class='bold'>{2} minutes</span>.</p>" +
                        "<p class='line-height-1-7 mb10'>The net calculated <span class='bold'>cost is {3} euros</span>.</p></div>",
                        userSummary.UnmarkedCallsCount, "unmarked", userSummary.UnmarkedCallsDuartion/=60, userSummary.UnmarkedCallsCost)
                }
            });
        }
    }
}