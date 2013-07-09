using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lync_Billing.DB;
using Ext.Net;
using Newtonsoft.Json;

namespace Lync_Billing.ui.admin.users
{
    public partial class delegates : System.Web.UI.Page
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
            FilterDelegatesBySite.GetStore().DataSource = GetAdminSiteName();
            FilterDelegatesBySite.GetStore().DataBind();
           
        }

        protected void GetDelegates(object sender, DirectEventArgs e)
        {
            if (FilterDelegatesBySite.SelectedItem != null)
            {
                string site = FilterDelegatesBySite.SelectedItem.Value;

                List<UsersDelegates> usersDelgates = new List<UsersDelegates>();
                List<UsersDelegates> tmpUsersDelegates = new List<UsersDelegates>();
                List<string> usersPersite = new List<string>();

                usersPersite = GetUsersPerSite(site);

                usersDelgates = UsersDelegates.GetDelgatees();

                tmpUsersDelegates = usersDelgates.Where(item => usersPersite.Contains(item.SipAccount)).ToList();

                ManageDelegatesGrid.GetStore().DataSource = tmpUsersDelegates;
                ManageDelegatesGrid.GetStore().DataBind();
            }
        }
         
        public string GetSiteName(int siteID)
        {
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            wherePart.Add("SiteID", siteID);

            List<Site> sites = DB.Site.GetSites(null, wherePart, 0);

            return sites[0].SiteName;
        }

        public List<Site> GetAdminSiteName()
        {
            UserSession session = (UserSession)Session.Contents["UserData"];
            List<Site> sites = new List<Site>();
            Site site;

            List<UserRole> userRoles = session.Roles;

            foreach (UserRole role in userRoles)
            {
                if (role.RoleID == 5 || role.RoleID == 1)
                {
                    site = new DB.Site();
                    site.SiteID = role.SiteID;
                    site.SiteName = GetSiteName(role.SiteID);

                    sites.Add(site);
                }
            }
            return sites;
        }

        public string GetSipAccount(string employeeID)
        {
            Dictionary<string, object> whereStatement = new Dictionary<string, object>();
            List<string> fields = new List<string>();
            List<Users> users = new List<Users>();

            whereStatement.Add("UserID", employeeID);
            fields.Add("SipAccount");

            users = Users.GetUsers(fields, whereStatement, 0);
            return users[0].SipAccount;
        }

        public string GetSipAccountSite(string employeeID)
        {
            Dictionary<string, object> whereStatement = new Dictionary<string, object>();
            // List<string> fields = new List<string>();
            List<Users> users = new List<Users>();

            whereStatement.Add("UserID", employeeID);

            users = Users.GetUsers(null, whereStatement, 0);
            return users[0].SiteName;
        }

        public List<string> GetUsersPerSite(string siteName) 
        {
            List<Users> users = new List<Users>();
            List<string> usersList = new List<string>();
            
            users = Users.GetUsers(siteName);

            if (users.Count > 0)
            {
                foreach (Users user in users) 
                {
                    usersList.Add(user.SipAccount);
                }
            }
            return usersList;
        }

        protected void UpdateEdited_DirectEvent(object sender, DirectEventArgs e)
        {
            UserSession userSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

            sipAccount = userSession.EffectiveSipAccount;
            string json = e.ExtraParams["Values"];

            List<UsersDelegates> recordsToUpate = new List<UsersDelegates>();

            ChangeRecords<UsersDelegates> toBeUpdated = new StoreDataHandler(e.ExtraParams["Values"]).BatchObjectData<UsersDelegates>();

            if (toBeUpdated.Updated.Count > 0)
            {

                foreach (UsersDelegates userDelgate in toBeUpdated.Updated)
                {
                    UsersDelegates.UpadeDelegate(userDelgate);
                    ManageDelegatesStore.GetById(userDelgate.ID).Commit();
                }
            }
            
            if (toBeUpdated.Deleted.Count > 0) 
            {
                foreach (UsersDelegates userDelgate in toBeUpdated.Deleted)
                {
                    UsersDelegates.DeleteDelegate(userDelgate);
                    ManageDelegatesStore.GetById(userDelgate.ID).Commit();
                }
            }
        }

        protected void RejectChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            ManageDelegatesGrid.GetStore().RejectChanges();
        }
    }
}