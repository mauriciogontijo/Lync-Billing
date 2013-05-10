using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

namespace Lync_Billing.UI
{
    public partial class ContentPage_Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            local_ip_address.Value = HttpContext.Current.Request.Headers.GetValues("Host")[0];
        }
    }
}