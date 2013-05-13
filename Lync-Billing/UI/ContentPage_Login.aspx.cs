using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using Lync_Billing.Libs;

namespace Lync_Billing.UI
{
    public partial class ContentPage_Login : System.Web.UI.Page
    {
        public AdLib authinticator = new AdLib();
        protected void Page_Load(object sender, EventArgs e)
        {
            local_ip_address.Value = HttpContext.Current.Request.UserHostAddress;
        }

        protected void Signin(object sender, EventArgs e)
        {
            bool authStatus = authinticator.AuthenticateUser(email.Text, password.Text);

        }


    }
}