using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;
using Microsoft.Build.Tasks;
using Ext.Net;
using Lync_Billing.Backend;
using Lync_Billing.Libs;
using Lync_Billing.Backend.Summaries;


namespace Lync_Billing.ui.user
{
    public partial class bills : System.Web.UI.Page
    {
        UserSession session;
        private string sipAccount = string.Empty;
        private string normalUserRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        private string userDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee);

        Dictionary<string, object> wherePart = new Dictionary<string, object>();
        List<string> columns = new List<string>();


        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/user/bills.aspx";
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
        }


        protected void BillsStore_Load(object sender, EventArgs e)
        {
            sipAccount = session.GetEffectiveSipAccount();

            List<UserCallsSummary> UserSummariesList = new List<UserCallsSummary>();
            List<UserCallsSummary> BillsList = new List<UserCallsSummary>();

            DateTime fromDate = new DateTime(DateTime.Now.Year - 1, 1, 1);
            DateTime toDate = DateTime.Now;

            UserSummariesList = UserCallsSummary.GetUsersCallsSummary(sipAccount, fromDate, toDate);

            foreach (UserCallsSummary summary in UserSummariesList)
            {
                BillsList.Add(summary);
            }

            BillsStore.DataSource = BillsList;
            BillsStore.DataBind();
        }

    }

}