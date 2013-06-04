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

namespace Lync_Billing.UI.user
{
    public partial class manage_address_book : System.Web.UI.Page
    {
        Dictionary<string, object> wherePart = new Dictionary<string, object>();
        List<string> columns = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (Session.Contents["UserData"] == null)
            {
                Response.Redirect("~/UI/session/login.aspx");
            }
        }

        protected void AssignBusiness(object sender, DirectEventArgs e)
        {
            
        }

        protected void AssignPersonal(object sender, DirectEventArgs e)
        {
            
        }

        protected void PhoneCallsStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            
        }

        protected void PhoneCallsStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            this.PhoneCallsStore.DataSource = PhoneCall.GetPhoneCalls(columns, wherePart, 0);
            this.PhoneCallsStore.DataBind();
        }

        protected void PhoneCallsStore_Load(object sender, EventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            wherePart.Add("SourceUserUri", userSession.SipAccount);
            wherePart.Add("marker_CallTypeID", 1);
            wherePart.Add("ac_IsInvoiced", "NO");

            columns.Add("SessionIdTime");
            columns.Add("SessionIdSeq");
            columns.Add("ResponseTime");
            columns.Add("SessionEndTime");
            columns.Add("marker_CallToCountry");
            columns.Add("DestinationNumberUri");
            columns.Add("Duration");
            columns.Add("marker_CallCost");
            columns.Add("ui_CallType");
            columns.Add("ui_MarkedOn");

            PhoneCallsStore.DataSource = PhoneCall.GetPhoneCalls(columns, wherePart, 0);
            PhoneCallsStore.DataBind();
        }
    }
}