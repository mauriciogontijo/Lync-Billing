using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using Newtonsoft.Json;

using Lync_Billing.Backend;
using Lync_Billing.Backend.Roles;

namespace Lync_Billing.ui.sysadmin.users
{
    public partial class depheads : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SystemAdmin);

        private List<Users> allUsers = new List<Users>();
        private List<Site> allSites = new List<Site>();
        private List<Department> allDepartments = new List<Department>();
        private List<DepartmentHeadRole> allDepartmenHeads = new List<DepartmentHeadRole>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/sysadmin/main/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                if (session.ActiveRoleName != allowedRoleName)
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=sysadmin");
                }
            }

            sipAccount = session.NormalUserInfo.SipAccount;

            allSites = Backend.Site.GetAllSites();
            allDepartments = Backend.Department.GetAllDepartments();
            allDepartmenHeads = DepartmentHeadRole.GetDepartmentHeads();
        }


        private List<string> GetUsersPerSite(int siteID)
        {
            List<Users> users = new List<Users>();
            List<string> usersList = new List<string>();

            var siteObject = allSites.Find(site => site.SiteID == siteID);

            users = Users.GetUsers(siteObject.SiteName);

            if (users.Count > 0)
            {
                foreach (Users user in users)
                {
                    usersList.Add(user.SipAccount);
                }
            }

            return usersList;
        }


        protected void GetDepartmentHeads(object sender, DirectEventArgs e)
        {
            List<DepartmentHeadRole> selectedSiteDepartmentHeads;

            if (FilterDepartmentHeadsBySite.SelectedItem.Index != -1)
            {
                int siteID = Convert.ToInt32(FilterDepartmentHeadsBySite.SelectedItem.Value);

                List<string> usersPersite = GetUsersPerSite(siteID);

                selectedSiteDepartmentHeads = allDepartmenHeads.Where(item => usersPersite.Contains(item.SipAccount)).ToList();

                ManageDepartmentHeadsGrid.GetStore().ClearFilter();
                ManageDepartmentHeadsGrid.GetStore().DataSource = selectedSiteDepartmentHeads;
                ManageDepartmentHeadsGrid.GetStore().DataBind();
            }
        }

        protected void DepartmentHeadsSitesStore_Load(object sender, EventArgs e)
        {
            FilterDepartmentHeadsBySite.GetStore().DataSource = allSites;
            FilterDepartmentHeadsBySite.GetStore().LoadData(allSites);
        }
    }
}