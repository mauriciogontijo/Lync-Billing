using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lync_Billing
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session.Contents["UserData"] != null)
            {
                Response.Redirect("~/ui/user/dashboard.aspx");
            }
            else
            {
                Response.Redirect("~/ui/session/login.aspx");
            }
        }
    }
}