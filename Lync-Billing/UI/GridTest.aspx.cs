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

namespace Lync_Billing.UI
{
    public partial class GridTest : System.Web.UI.Page
    {

        Dictionary<string, object> wherePart = new Dictionary<string, object>();
        List<string> columns = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string SipAccount = "SGhaida@ccc.gr"; //((UserSession)Session.Contents["UserData"]).SipAccount;


            wherePart.Add("SourceUserUri", SipAccount);
            wherePart.Add("marker_CallTypeID", 1);
            wherePart.Add("ui_IsInvoiced", "PENDING");

            columns.Add("SessionIdTime");
            columns.Add("SessionIdSeq");
            columns.Add("ResponseTime");
            columns.Add("SessionEndTime");
            columns.Add("marker_CallToCountry");
            columns.Add("DestinationNumberUri");
            columns.Add("Duration");
            columns.Add("marker_CallCost");
            columns.Add("ui_IsPersonal");
            columns.Add("ui_MarkedOn");

            PhoneCallStore.DataSource = PhoneCall.GetPhoneCalls(columns, wherePart, 0);
            PhoneCallStore.DataBind();
        }

        protected void AssignBusiness(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = this.PhoneCallsHistoryGrid.GetSelectionModel() as RowSelectionModel;
            
            string json = e.ExtraParams["Values"];
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in phoneCalls) 
            {
                phoneCall.UI_IsPersonal="NO";
                phoneCall.UI_MarkedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = "SGhaida@ccc.gr";
                //phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);
                
                PhoneCallsHistoryGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                PhoneCallsHistoryGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
        }

        protected void AssignPersonal(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = this.PhoneCallsHistoryGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                phoneCall.UI_IsPersonal = "YES";
                phoneCall.UI_MarkedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = "SGhaida@ccc.gr";
                //phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                PhoneCallsHistoryGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                PhoneCallsHistoryGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
        }

        protected void AssignDispute(object sender, DirectEventArgs e)
        {

        }
    }
}
