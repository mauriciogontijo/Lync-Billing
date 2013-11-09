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

namespace Lync_Billing.ui.accounting.main
{
    public partial class disputed : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SiteAccountant);
        private Dictionary<string, object> wherePart = new Dictionary<string, object>();
        private List<string> columns = new List<string>();
        private List<string> accountantSitesNames = new List<string>();
        

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/accounting/main/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                session = (UserSession)Session.Contents["UserData"];

                if (session.ActiveRoleName != allowedRoleName)
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=accounting");
                }
            }

            sipAccount = session.EffectiveSipAccount;
        }

        protected void DisputedCallsStore_Load(object sender, EventArgs e)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

            List<PhoneCall> usersCalls = new List<PhoneCall>();
            List<PhoneCall> accountantView = new List<PhoneCall>();
            accountantSitesNames = GetAccountantSiteName(session.EffectiveSipAccount);

            usersCalls = PhoneCall.GetDisputedPhoneCalls(columns, wherePart, 0).Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null).ToList();

            foreach (PhoneCall phoneCall in usersCalls) 
            {
                if (accountantSitesNames.Contains(GetSipAccountSite(phoneCall.SourceUserUri)))
                    accountantView.Add(phoneCall);
            }

            DisputedCallsStore.DataSource = accountantView;
            DisputedCallsStore.DataBind();
        }

        protected void DisputedCallsStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            XmlNode xml = e.Xml;

            this.Response.Clear();
            this.Response.ContentType = "application/vnd.ms-excel";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
            XslCompiledTransform xtExcel = new XslCompiledTransform();
            xtExcel.Load(Server.MapPath("~/Resources/Excel.xsl"));
            xtExcel.Transform(xml, null, Response.OutputStream);
        }

        protected void DisputedCallsStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            string siteNameKey = Enums.GetDescription(Enums.Users.AD_PhysicalDeliveryOfficeName);
            string sipAccountKey = Enums.GetDescription(Enums.PhoneCalls.SourceUserUri);
            accountantSitesNames = GetAccountantSiteName(session.EffectiveSipAccount);
            List<string> usersInSites = new List<string>();

            foreach (string siteName in accountantSitesNames)
            {
                usersInSites.AddRange(Users.GetUsers(siteName).Select(user => user.SipAccount));
            }

            this.DisputedCallsStore.DataSource = PhoneCall.GetDisputedPhoneCalls(usersInSites, wherePart, 0);
            this.ManageDisputedCallsGrid.DataBind();
        }

        protected void AcceptDispute(object sender, DirectEventArgs e) 
        {
            RowSelectionModel sm = this.ManageDisputedCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                phoneCall.AC_DisputeStatus = "Accepted";
                phoneCall.AC_DisputeResolvedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).PrimarySipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManageDisputedCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                ManageDisputedCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
            //ManagePhoneCallsGrid.GetStore().CommitChanges();
            ManageDisputedCallsGrid.GetStore().Reload();
            ManageDisputedCallsGrid.GetSelectionModel().DeselectAll();
        }

        protected void RejectDispute(object sender, DirectEventArgs e) 
        {
            RowSelectionModel sm = this.ManageDisputedCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                phoneCall.AC_DisputeStatus = "Rejected";
                phoneCall.AC_DisputeResolvedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).PrimarySipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManageDisputedCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                ManageDisputedCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
            //ManagePhoneCallsGrid.GetStore().CommitChanges();
            ManageDisputedCallsGrid.GetStore().Reload();
            ManageDisputedCallsGrid.GetSelectionModel().DeselectAll();
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
                if ((role.IsSiteAccountant() || role.IsDeveloper())) 
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