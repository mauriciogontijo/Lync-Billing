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