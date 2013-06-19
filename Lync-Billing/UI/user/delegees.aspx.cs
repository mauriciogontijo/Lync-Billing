﻿using System;
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
using Lync_Billing.Libs;

namespace Lync_Billing.ui.user
{
    public partial class delegees : System.Web.UI.Page
    {
        Dictionary<string, object> wherePart = new Dictionary<string, object>();
        List<string> columns = new List<string>();
        List<UsersDelegates> delegates = new List<UsersDelegates>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/user/manage_delegates.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            //but if the user is actually logged in we only need to check if he is marked as a delegate user
            else
            {
                UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];
                if (session.IsDelegate == true)
                {
                    delegates = UsersDelegates.GetSipAccounts(session.SipAccount);
                }
                else
                {
                    //We redirect the users to the User Dashboard page if they have requested the Manage Delegates page without being marked as delegates themselves
                    Response.Redirect("~/ui/session/login.aspx");
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

            DelegatedUsersStore.DataSource = delegates;
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
            string DelegatedAccount = UsersDelegates.GetDelegateAccount(DelegatedUsersComboBox.SelectedItem.Value).DelegeeAccount;

            if (DelegatedUsersComboBox.SelectedItem.Value != null && DelegatedAccount == userSession.SipAccount)
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