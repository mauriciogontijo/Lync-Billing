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

namespace Lync_Billing.UI.accounting
{
    public partial class manage_disputes : System.Web.UI.Page
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
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            wherePart.Add("SourceUserUri", userSession.SipAccount);
            wherePart.Add("marker_CallTypeID", 1);
            wherePart.Add("ui_IsInvoiced", "NO");
            wherePart.Add("ui_Dispute", "YES");

            columns.Add("SessionIdTime");
            columns.Add("SessionIdSeq");
            columns.Add("ResponseTime");
            columns.Add("SessionEndTime");
            columns.Add("marker_CallToCountry");
            columns.Add("DestinationNumberUri");
            columns.Add("Duration");
            columns.Add("marker_CallCost");
            columns.Add("ac_DisputeStatus");
            columns.Add("ui_MarkedOn");

            DisputesStore.DataSource = PhoneCall.GetPhoneCalls(columns, wherePart, 0);
            DisputesStore.DataBind();
        }

        protected void DisputesStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            XmlNode xml = e.Xml;

            this.Response.Clear();
            this.Response.ContentType = "application/vnd.ms-excel";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
            XslCompiledTransform xtExcel = new XslCompiledTransform();
            xtExcel.Load(Server.MapPath("~/Resources/Excel.xsl"));
            xtExcel.Transform(xml, null, Response.OutputStream);
        }

        protected void DisputesStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            this.DisputesStore.DataSource = PhoneCall.GetPhoneCalls(columns, wherePart, 0);
            this.ManageDisputesGrid.DataBind();
        }

        protected void AcceptDispute(object sender, DirectEventArgs e) 
        {
            RowSelectionModel sm = this.ManageDisputesGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                phoneCall.AC_DisputeStatus = "ACCEPTED";
                phoneCall.AC_ResolvedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManageDisputesGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                ManageDisputesGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
            //ManagePhoneCallsGrid.GetStore().CommitChanges();
            ManageDisputesGrid.GetSelectionModel().DeselectAll();
        }

        protected void RejectDispute(object sender, DirectEventArgs e) 
        {
            RowSelectionModel sm = this.ManageDisputesGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                phoneCall.AC_DisputeStatus = "REJECTED";
                phoneCall.AC_ResolvedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManageDisputesGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                ManageDisputesGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
            //ManagePhoneCallsGrid.GetStore().CommitChanges();
            ManageDisputesGrid.GetSelectionModel().DeselectAll();
        }
    }
}