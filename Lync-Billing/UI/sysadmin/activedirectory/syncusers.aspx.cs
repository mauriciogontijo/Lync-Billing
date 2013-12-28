using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;

using Lync_Billing.Backend;

namespace Lync_Billing.ui.sysadmin.activedirectory
{
    public partial class syncusers : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SystemAdmin);


        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            //if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            //{
            //    string redirect_to = @"~/ui/sysadmin/main/dashboard.aspx";
            //    string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
            //    Response.Redirect(url);
            //}
            //else
            //{
            //    session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            //    if (session.ActiveRoleName != allowedRoleName)
            //    {
            //        Response.Redirect("~/ui/session/authenticate.aspx?access=sysadmin");
            //    }
            //}

            //sipAccount = session.NormalUserInfo.SipAccount;
        }


        protected void SyncUsersButton_Click(object sender, DirectEventArgs e)
        {
            Users.SyncWithAD();
        }

        protected void SyncDepartmentsButton_Click(object sender, DirectEventArgs e)
        {
            Department.SyncDepartments();
        }

        protected void SyncSitesButton_Click(object sender, DirectEventArgs e)
        {
            //to do
        }
    }
}