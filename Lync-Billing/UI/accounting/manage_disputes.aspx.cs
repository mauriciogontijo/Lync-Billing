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

            List<PhoneCall> usersCalls = new List<PhoneCall>();
            List<PhoneCall> accountantView = new List<PhoneCall>();
            List<string> sites = GetAccountantSiteName(userSession.SipAccount);

            wherePart.Add("marker_CallTypeID", 1);
            wherePart.Add("ac_IsInvoiced", "NO");
            wherePart.Add("ui_CallType", "Dispute");

            columns.Add("SessionIdTime");
            columns.Add("SessionIdSeq");
            columns.Add("ResponseTime");
            columns.Add("SourceUserUri");
            columns.Add("marker_CallToCountry");
            columns.Add("DestinationNumberUri");
            columns.Add("Duration");
            columns.Add("marker_CallCost");
            columns.Add("ac_DisputeStatus");
            columns.Add("ui_MarkedOn");

            usersCalls = PhoneCall.GetPhoneCalls(columns, wherePart, 0);

            foreach (PhoneCall phoneCall in usersCalls) 
            {
                if (sites.Contains(GetSipAccountSite(phoneCall.SourceUserUri)))
                    accountantView.Add(phoneCall);
            }

            DisputesStore.DataSource = accountantView;
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
                phoneCall.AC_DisputeStatus = "Accepted";
                phoneCall.AC_DisputeResolvedOn = DateTime.Now;
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
                phoneCall.AC_DisputeStatus = "Rejected";
                phoneCall.AC_DisputeResolvedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManageDisputesGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                ManageDisputesGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
            //ManagePhoneCallsGrid.GetStore().CommitChanges();
            ManageDisputesGrid.GetSelectionModel().DeselectAll();
        }

        public string GetSiteName(int siteID)
        {
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            wherePart.Add("SiteID", siteID);

            List<Site> sites = DB.Site.GetSites(null, wherePart, 0);

            return sites[0].SiteName;
        }

        public List<string> GetAccountantSiteName(string sipAccount)
        {
            List<string> accountantSites = new List<string>();

            UserSession session = (UserSession)Session.Contents["UserData"];

            List<UserRole> userRoles = session.Roles;

            foreach (UserRole role in userRoles)
            {
                if ((role.RoleID == 7 || role.RoleID == 1 )) 
                {
                    accountantSites.Add(GetSiteName(role.SiteID));
                }
                    
            }
            return accountantSites;
        }

        public string GetSipAccountSite(string sipAccount)
        {
            Dictionary<string, object> whereStatement = new Dictionary<string, object>();
            // List<string> fields = new List<string>();
            List<Users> users = new List<Users>();

            whereStatement.Add("SipAccount", sipAccount);


            users = Users.GetUsers(null, whereStatement, 0);
            return users[0].SiteName;
        }
      
    }
}