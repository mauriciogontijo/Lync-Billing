using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using Lync_Billing.Libs;
using Lync_Billing.DB;

namespace Lync_Billing.ui.session
{
    public partial class authenticate : System.Web.UI.Page
    {
        public AdLib authinticator = new AdLib();

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session != null && HttpContext.Current.Session.Contents["UserData"] != null)
            {
                Response.Redirect("~/ui/user/dashboard.aspx");
            }

            //Check if a redirect_to value has been passed and validate it's link
            if (Request.QueryString["redirect_to"] != null && Request.QueryString["redirect_to"] != string.Empty)
            {
                //This statement validates that the link must contain the application root path and the page extension at the end of it
                if (Request.QueryString["redirect_to"].Contains(@"~/ui/") && Request.QueryString["redirect_to"].Contains(@".aspx"))
                {
                    this.redirect_to_url.Value = Request.QueryString["redirect_to"];
                }
                else
                {
                    this.redirect_to_url.Value = string.Empty;
                }
            }
        }

        protected void authenticate_user(object sender, EventArgs e)
        {
            
        }//END OF FUNCTION
    }
}