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

namespace Lync_Billing.ui.accounting.main
{
    public partial class dashboard : System.Web.UI.Page
    {
        public UserSession currentSession { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/accounting/main/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
                //Response.Redirect("~/ui/session/login.aspx");
            }
            else
            {
                //Initialize the local cope of the current user's session
                currentSession = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                if (currentSession.ActiveRoleName != "accounting")
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=accounting");
                }

                //if ((currentSession.IsDeveloper || currentSession.IsAccountant))
                //{
                //    if (currentSession.PrimarySipAccount == currentSession.EffectiveSipAccount)
                //    {
                //        if (currentSession.ActiveRoleName != "accounting")
                //        {
                //            Response.Redirect("~/ui/session/authenticate.aspx?access=accounting");
                //        }
                //    }
                //    else
                //    {
                //        Response.Redirect("~/ui/user/dashboard.aspx");
                //    }
                //}
                //else
                //{
                //    Response.Redirect("~/ui/user/dashboard.aspx");
                //}

                //if ((!current_session.IsDeveloper || !current_session.IsAccountant) && current_session.PrimarySipAccount != current_session.EffectiveSipAccount && current_session.ActiveRoleName != "accounting")
                //{
                //    Response.Redirect("~/ui/user/dashboard.aspx");
                //}
            }
        }
    }
}