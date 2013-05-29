﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lync_Billing.UI.session
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();

            Response.Redirect("~/UI/session/login.aspx");
        }
    }
}