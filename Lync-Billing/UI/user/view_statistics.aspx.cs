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
using Lync_Billing.Libs;

namespace Lync_Billing.UI.user
{
    public partial class view_statistics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (Session.Contents["UserData"] == null || HttpContext.Current.Session.Count == 0)
            {
                Response.Redirect("~/UI/session/login.aspx");
            }

            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

            //PhoneCallsDuartionChartStore.DataSource = getChartData(SipAccount);
            //PhoneCallsDuartionChartStore.DataBind();

            //DurationCostChartStore.DataSource = UsersCallsSummary.GetUsersCallsSummary(SipAccount, DateTime.Now.Year, 1, 12);
            //DurationCostChartStore.DataBind();
        }

        public List<UsersCallsSummaryChartData> getChartData(string SipAccount = "")
        {
            return UsersCallsSummaryChartData.GetUsersCallsSummary(((UserSession)Session.Contents["UserData"]).SipAccount, DateTime.Now.AddMonths(-3), DateTime.Now);
        }

        protected void PhoneCallsDuartionChartStore_Load(object sender, EventArgs e)
        {
            PhoneCallsDuartionChartStore.DataSource = getChartData();
            PhoneCallsDuartionChartStore.DataBind();
        }

        protected void DurationCostChartStore_Load(object sender, EventArgs e)
        {
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

            DurationCostChartStore.DataSource = UsersCallsSummary.GetUsersCallsSummary(SipAccount, DateTime.Now.Year, 1, 12);
            DurationCostChartStore.DataBind();

        }
    }
}