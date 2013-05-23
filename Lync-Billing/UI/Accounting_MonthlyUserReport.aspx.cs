using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using Ext.Net;
using System.Web.Script.Serialization;
using Lync_Billing.DB;
using System.Xml;
using System.Xml.Xsl;

namespace Lync_Billing.UI
{
    public partial class Accounting_MonthlyUserReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (Session.Contents["UserData"] == null)
            {
                Response.Redirect("~/UI/Login.aspx");
            }
            else
            {
                bool status = new Boolean();
                status = false;

                UserSession session = new UserSession();
                session = (UserSession)Session.Contents["UserData"];

                foreach (UserRole role in session.Roles)
                {
                    if (role.RoleID == 7 || role.RoleID == 1)
                        status = true;
                }

                if (status == false)
                {
                    Response.Redirect("~/UI/Login.aspx");
                }
            }
        }
    }
}