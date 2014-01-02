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
using Lync_Billing.Backend;
using Lync_Billing.Backend.Summaries;
using Lync_Billing.Libs;

namespace Lync_Billing.ui.user
{
    public partial class statistics : System.Web.UI.Page
    {
        UserSession session;
        private string sipAccount = string.Empty;
        private string normalUserRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        private string userDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee);

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/user/statistics.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
                if (session.ActiveRoleName != normalUserRoleName && session.ActiveRoleName != userDelegeeRoleName)
                {
                    string url = @"~/ui/session/authenticate.aspx?access=" + session.ActiveRoleName;
                    Response.Redirect(url);
                }
            }

            sipAccount = session.GetEffectiveSipAccount();

            PhoneCallsDuartionChartStore.DataSource = getChartData();
            PhoneCallsDuartionChartStore.DataBind();

            PhoneCallsCostChartStore.DataSource = getChartData();
            PhoneCallsCostChartStore.DataBind();

            DurationCostChartStore.DataSource = UserCallsSummary.GetUsersCallsSummary(sipAccount, DateTime.Now.Year, 1, 12);
            DurationCostChartStore.DataBind();

            //Set the year number in the charts header's title
            string currentYear = DateTime.Now.Year.ToString();
            DurationCostChartPanel.Title = "Business/Personal Calls Report for " + currentYear;
            PhoneCallsCostChartPanel.Title = "Calls Costs Report for " + currentYear;
            PhoneCallsDuartionChartPanel.Title = "Calls Duration Report for " + currentYear;
        }

        public List<UsersCallsSummaryChartData> getChartData()
        {
            UserSession userSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.GetEffectiveSipAccount();

            return UsersCallsSummaryChartData.GetUsersCallsSummary(sipAccount, DateTime.Now.Year, 1, 12);
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
    }
}