using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Xsl;

using Ext.Net;
using Lync_Billing.DB;

namespace Lync_Billing.ui.delegee.department
{
    public partial class phonecalls : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string delegatedDepartmentName = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.DepartmentDelegee);

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                //Don't enable the redirection process because the site name will be missing!
                //string redirect_to = @"~/ui/delegee/department.aspx";
                //string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;

                string url = @"~/ui/session/login.aspx";
                Response.Redirect(url);
            }
            else
            {
                session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                if (session.ActiveRoleName != allowedRoleName)
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=" + allowedRoleName);
                }
            }

            sipAccount = session.NormalUserInfo.SipAccount;
            //delegatedDepartmentName = session.EffectiveDelegatedDepartmentName;
        }
    }
}