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
    public partial class User_ManagePhoneCalls : System.Web.UI.Page
    {
        Dictionary<string, object> wherePart = new Dictionary<string, object>();
        List<string> columns = new List<string>();
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void AssignBusiness(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                phoneCall.UI_IsPersonal = "NO";
                phoneCall.UI_MarkedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                //ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
            ManagePhoneCallsGrid.GetStore().CommitChanges();
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
        }

        protected void AssignPersonal(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                phoneCall.UI_IsPersonal = "YES";
                phoneCall.UI_MarkedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                
            }
            ManagePhoneCallsGrid.GetStore().CommitChanges();
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
        }

        protected void AssignDispute(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                phoneCall.UI_Dispute = "YES";
                phoneCall.UI_MarkedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
            }
            ManagePhoneCallsGrid.GetStore().CommitChanges();
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
        }

        protected void PhoneCallsStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            XmlNode xml = e.Xml;

            this.Response.Clear();
            this.Response.ContentType = "application/vnd.ms-excel";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
            XslCompiledTransform xtExcel = new XslCompiledTransform();
            xtExcel.Load(Server.MapPath("~/Resources/Excel.xsl"));
            xtExcel.Transform(xml, null, Response.OutputStream);

            this.Response.End();
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
            wherePart.Add("ui_IsInvoiced", "NO");

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

            PhoneCallsStore.DataSource = PhoneCall.GetPhoneCalls(columns, wherePart, 0);
            PhoneCallsStore.DataBind();
        }

    }
}