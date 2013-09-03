using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lync_Billing.DB;
using Ext.Net;
using Newtonsoft.Json;

namespace Lync_Billing.ui.sysadmin.users
{
    public partial class roles : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        private UserSession session;

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/sysadmin/main/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
                //Response.Redirect("~/ui/session/login.aspx");
            }
            else
            {
                session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                if (session.ActiveRoleName != "sysadmin")
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=sysadmin");
                }
            }

            sipAccount = session.EffectiveSipAccount;
            FilterDelegatesBySite.GetStore().DataSource = DB.Site.GetUserRoleSites(session.Roles, Enums.GetDescription(Enums.ValidRoles.IsSystemAdmin));
            FilterDelegatesBySite.GetStore().DataBind();
        }

    }
}