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
using Lync_Billing.Libs;

namespace Lync_Billing.UI.user
{
    public partial class phonecalls : System.Web.UI.Page
    {
        public Dictionary<string, object> wherePart = new Dictionary<string, object>();
        public Dictionary<string, PhoneBook> phoneBookEntries;
        public List<string> columns = new List<string>();
        public List<PhoneCall> phoneCalls;
        List<PhoneCall> AutoMarkedPhoneCalls = new List<PhoneCall>();

        string sipAccount = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/UI/user/phonecalls.aspx";
                string url = @"~/UI/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).SipAccount;
            phoneBookEntries = PhoneBook.GetAddressBook(sipAccount);
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

        protected void PhoneCallsStore_Load(object sender, EventArgs e)
        {


            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            ManagePhoneCallsGrid.Loader.LoadMask.ShowMask = true;

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

            phoneCalls = PhoneCall.GetPhoneCalls(columns, wherePart, 0);

            PhoneBook phoneBookentry;

            PhoneCallsStore.DataSource = phoneCalls;

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                phoneBookentry = new PhoneBook();
                phoneBookentry = GetUserNameByNumber(phoneCall.DestinationNumberUri);

                if (phoneBookentry != null)
                {
                    phoneCall.PhoneBookName = phoneBookentry.Name;

                    if (phoneCall.UI_CallType == null)
                    {
                        phoneCall.UI_CallType = phoneBookentry.Type;
                        phoneCall.UI_MarkedOn = DateTime.Now;
                        AutoMarkedPhoneCalls.Add(phoneCall);
                    }
                }
                else
                {
                    phoneCall.PhoneBookName = "N/A";
                }

            }
            PhoneCallsStore.DataBind();

            if (AutoMarkedPhoneCalls.Count > 0)
            {
                foreach (PhoneCall autoMarkedPhonecall in AutoMarkedPhoneCalls)
                {
                    PhoneCall.UpdatePhoneCall(autoMarkedPhonecall);
                }
            }
            ManagePhoneCallsGrid.SetLoading(false);
        }
      
        private PhoneBook GetUserNameByNumber(string phoneNumber)
        {
            if (phoneBookEntries.ContainsKey(phoneNumber))
                return phoneBookEntries[phoneNumber];
            else
                return null;
        }
    }
}