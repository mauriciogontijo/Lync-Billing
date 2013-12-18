﻿using System;
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
    public partial class sysroles : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SystemAdmin);

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

            FilterUsersRolesBySite.GetStore().DataSource = Backend.Site.GetUserRoleSites(session.SystemRoles, Enums.GetDescription(Enums.ValidRoles.IsSystemAdmin));
            FilterUsersRolesBySite.GetStore().DataBind();
        }


        //PRIVATE METHOD USED INSIDE THE CLASS
        private List<SystemRole> getUsersRolesPerSite(string siteName)
        {
            List<string> DbSiteColumns = new List<string>();
            Dictionary<string, object> DbSiteWherePart = new Dictionary<string, object>()
            {
                {"SiteName", siteName},
            };
            
            List<Backend.Site> sitesResults = Backend.Site.GetAllSites(DbSiteColumns, DbSiteWherePart, 0);
            Backend.Site site = sitesResults.First();


            List<string> columns = new List<string>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>()
            {
                {"SiteID", site.SiteID},
            };
            
            List<SystemRole> userRolesPerSite = SystemRole.GetSystemRoles(columns, wherePart, 0);

            return userRolesPerSite;
        }


        //DIRECT EVENT CALLED BY THE FilterUsersRolesPerSite DROPDOWN MENU
        protected void GetUsersRolesPerSite(object sender, DirectEventArgs e)
        {
            if (FilterUsersRolesBySite.SelectedItem != null)
            {
                string siteName = FilterUsersRolesBySite.SelectedItem.Value;

                ManageUsersRolesGrid.GetStore().DataSource = getUsersRolesPerSite(siteName);
                ManageUsersRolesGrid.GetStore().DataBind();
            }
        }
    }
}