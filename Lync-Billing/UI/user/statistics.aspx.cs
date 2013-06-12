using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Globalization;
using Ext.Net;
using Lync_Billing.DB;
using Lync_Billing.Libs;

namespace Lync_Billing.UI.user
{
    public partial class statistics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/UI/user/statistics.aspx";
                string url = @"~/UI/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }

            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

            PhoneCallsDuartionChartStore.DataSource = getChartData(SipAccount);
            PhoneCallsDuartionChartStore.DataBind();

            PhoneCallsCostChartStore.DataSource = getChartData(SipAccount);
            PhoneCallsCostChartStore.DataBind();

            DurationCostChartStore.DataSource = UsersCallsSummary.GetUsersCallsSummary(SipAccount, DateTime.Now.Year, 1, 12);
            DurationCostChartStore.DataBind();

            PhoneCallsDuartionChartPanel.Title = "Calls Duration Report for " + DateTime.Now.Year;
            PhoneCallsCostChartPanel.Title = "Calls Costs Report for " + DateTime.Now.Year;
        }

        public List<UsersCallsSummaryChartData> getChartData(string SipAccount = "")
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime toDate = DateTime.ParseExact(DateTime.Now.Year.ToString() + "-01-01", "yyyy-mm-dd", provider);

            return UsersCallsSummaryChartData.GetUsersCallsSummary(((UserSession)Session.Contents["UserData"]).SipAccount, toDate, DateTime.Now);
        }

        protected void PhoneCallsDuartionChartStore_Load(object sender, EventArgs e)
        {
            PhoneCallsDuartionChartStore.DataSource = getChartData();
            PhoneCallsDuartionChartStore.DataBind();
        }

        protected void PhoneCallsCostChartStore_Load(object sender, EventArgs e)
        {
            PhoneCallsCostChartStore.DataSource = getChartData();
            PhoneCallsCostChartStore.DataBind();
        }

        //DurationCostChartStore.DataSource = UsersCallsSummary.GetUsersCallsSummary(SipAccount, DateTime.Now.Year, 1, 12);
    }
}