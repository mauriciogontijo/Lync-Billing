﻿using System;
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

namespace Lync_Billing.UI.accounting
{
    public partial class manage_disputes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (Session.Contents["UserData"] == null)
            {
                Response.Redirect("~/UI/session/login.aspx");
            }
            else
            {
                UserSession session = new UserSession();
                session = (UserSession)Session.Contents["UserData"];

                if (!session.IsDeveloper && !session.IsAccountant)
                {
                    Response.Redirect("~/UI/user/dashboard.aspx");
                }
            }
        }

        protected void DisputesStore_Load(object sender, EventArgs e)
        {

        }

        protected void DisputesStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {

        }

        protected void DisputesStore_ReadData(object sender, StoreReadDataEventArgs e)
        {

        }

        protected void AcceptDispute(object sender, DirectEventArgs e) { }

        protected void RejectDispute(object sender, DirectEventArgs e) { }
    }
}