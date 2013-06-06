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
    public partial class manage_delegates : System.Web.UI.Page
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

                if (!session.IsDelegate && !session.IsDeveloper)
                {
                    Response.Redirect("~/UI/User_Dashboard.aspx");
                }
            }
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
                phoneCall.UI_CallType = "Business";
                phoneCall.UI_MarkedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
            //ManagePhoneCallsGrid.GetStore().CommitChanges();
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
                phoneCall.UI_CallType = "Personal";
                phoneCall.UI_MarkedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
            //ManagePhoneCallsGrid.GetStore().CommitChanges();
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
                phoneCall.UI_CallType = "Dispute";
                phoneCall.UI_MarkedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
            //ManagePhoneCallsGrid.GetStore().CommitChanges();
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

        protected void DelegatedUsersStore_Load(object sender, EventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            DelegatedUsersStore.DataSource = UsersDelegates.GetSipAccounts(userSession.SipAccount);
            DelegatedUsersStore.DataBind();
            ResourceManager res = new ResourceManager();
        }

        protected void GetDelegatedUserPhoneCalls()
        {

        }

        protected void GetDelegatedUserCallsButton_DirectClick(object sender, DirectEventArgs e)
        {
            PhoneCallsStore.RemoveAll();
            PhoneCallsStore.Dispose();
            
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            if (DelegatedUsersComboBox.SelectedItem.Value != null &&
                UsersDelegates.GetDelegateAccount(userSession.SipAccount).SipAccount == DelegatedUsersComboBox.SelectedItem.Value)
            {
                UsersDelegates userDelegate = new UsersDelegates();
                userDelegate = UsersDelegates.GetDelegateAccount(userSession.SipAccount);

                string SipAccount = DelegatedUsersComboBox.SelectedItem.Value.ToString();

                wherePart.Add("SourceUserUri", SipAccount);
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

                GetDelegatedUserCallsButton.Disabled = true;
            }
        }
    
        public void OnComboboxSelection_Change(object sender, DirectEventArgs e)
        {
           

            PhoneCallsStore.RemoveAll();
            PhoneCallsStore.Dispose();
        }
    }
}