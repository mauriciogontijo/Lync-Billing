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

namespace Lync_Billing.ui.admin.main
{
    public partial class configuration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/admin/main/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
                //Response.Redirect("~/ui/session/login.aspx");
            }
            else
            {
                UserSession session = new UserSession();
                session = (UserSession)Session.Contents["UserData"];

                if (session.ActiveRoleName != "admin")
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=admin");
                }
            }
        }

        protected void AppConfigStore_ReadData(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            //AppConfigGrid.GetStore().DataSource = Persistence.
        }

        protected void AppConfigStore_Load(object sender, EventArgs e)
        {

        }

        protected void UpdateEdited_DirectEvent(object sender, DirectEventArgs e)
        {

        }

        protected void DeleteSelected_DirectEvent(object sender, DirectEventArgs e)
        {

        }

        [DirectMethod]
        public void DoConfirm()
        {
            // Manually configure Handler...
            //Msg.Confirm("Message", "Confirm?", "if (buttonId == 'yes') { CompanyX.DoYes(); } else { CompanyX.DoNo(); }").Show();

            // Configure individualock Buttons using a ButtonsConfig...
            X.Msg.Confirm(
                "Message",
                "Confirm?", 
                new MessageBoxButtonsConfig {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "CompanyX.DoYes()",
                        Text = "Yes"
                    },
                    No = new MessageBoxButtonConfig
                    {
                        Handler = "CompanyX.DoNo()",
                        Text = "No!"
                    }
            }).Show();
        }

        [DirectMethod]
        public void DoYes()
        {
            //Your code goes here on YES
        }

        [DirectMethod]
        public void DoNo()
        {
            //Your code goes here on NO
        }
    }
}