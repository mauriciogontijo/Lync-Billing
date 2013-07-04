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

namespace Lync_Billing.ui.admin.gateways
{
    public partial class rates : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;

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

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;
        }

        protected void UpdateEdited_DirectEvent(object sender, DirectEventArgs e)
        {
            UserSession userSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

            sipAccount = userSession.EffectiveSipAccount;
            string json = e.ExtraParams["Values"];

            //List<UsersDelegates> recordsToUpate = new List<UsersDelegates>();

            //ChangeRecords<UsersDelegates> toBeUpdated = new StoreDataHandler(e.ExtraParams["Values"]).BatchObjectData<UsersDelegates>();

            //if (toBeUpdated.Updated.Count > 0)
            //{

            //    foreach (UsersDelegates userDelgate in toBeUpdated.Updated)
            //    {
            //        UsersDelegates.UpadeDelegate(userDelgate);
            //        ManageRatesStore.GetById(userDelgate.ID).Commit();
            //    }
            //}

            //if (toBeUpdated.Deleted.Count > 0)
            //{
            //    foreach (UsersDelegates userDelgate in toBeUpdated.Deleted)
            //    {
            //        UsersDelegates.DeleteDelegate(userDelgate);
            //        ManageRatesStore.GetById(userDelgate.ID).Commit();
            //    }
            //}
        }

        protected void RejectChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            ManageRatesGrid.GetStore().RejectChanges();
        }

        protected void GetGateways(object sender, DirectEventArgs e)
        {

        }
    }
}