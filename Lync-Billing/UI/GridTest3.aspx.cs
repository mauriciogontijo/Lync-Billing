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
        UsersCallsSummary userSummary;

        protected void Page_Load(object sender, EventArgs e)
        {
            userSummary = new UsersCallsSummary();
            string SipAccount = "SGhaida@ccc.gr"; //((UserSession)Session.Contents["UserData"]).SipAccount;

            this.UserCallSummary.Text = UsersCallsSummary.GetUsersCallsSummary(SipAccount, DateTime.Now.AddYears(-1), DateTime.Now).ToString();
        }
    }
}