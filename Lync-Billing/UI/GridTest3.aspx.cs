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
        }

        [DirectMethod]
        public static string Items()
        {
            string SipAccount = "AAlhour@ccc.gr"; //((UserSession)Session.Contents["UserData"]).SipAccount;
            UsersCallsSummary userSummary = new UsersCallsSummary();
            userSummary = UsersCallsSummary.GetUsersCallsSummary(SipAccount, DateTime.Now.AddYears(-1), DateTime.Now);
            return ComponentLoader.ToConfig(new List<AbstractComponent>() {
                new Ext.Net.Panel { 
                    Title="Business Calls Overview",
                    Icon = Icon.Phone,
                    Html = "Total Calls: " + userSummary.BusinessCallsCount + "<br />" +
                        "Total Duration: " + userSummary.BusinessCallsDuration + "<br />" +
                        "Total Cost: " + userSummary.BusinessCallsCost + "<br />"
                },

                new Ext.Net.Panel { 
                    Title="Business Calls Overview",
                    Icon = Icon.Phone,
                    Html = "Total Calls: " + userSummary.BusinessCallsCount + "<br />" +
                        "Total Duration: " + userSummary.BusinessCallsDuration + "<br />" +
                        "Total Cost: " + userSummary.BusinessCallsCost + "<br />"
                },

                new Ext.Net.Panel { 
                    Title="Business Calls Overview",
                    Icon = Icon.Phone,
                    Html = "Total Calls: " + userSummary.BusinessCallsCount + "<br />" +
                        "Total Duration: " + userSummary.BusinessCallsDuration + "<br />" +
                        "Total Cost: " + userSummary.BusinessCallsCost + "<br />"
                }
            });
        }
    }
}