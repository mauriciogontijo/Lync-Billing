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
    public partial class delegates : System.Web.UI.Page
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

            FilterDelegatesBySite.GetStore().DataSource = Backend.Site.GetUserRoleSites(session.SystemRoles, Enums.GetDescription(Enums.ValidRoles.IsSystemAdmin));
            FilterDelegatesBySite.GetStore().DataBind();

        }

        protected void GetDelegates(object sender, DirectEventArgs e)
        {
            if (FilterDelegatesBySite.SelectedItem != null)
            {
                string site = FilterDelegatesBySite.SelectedItem.Value;

                List<DelegateRole> usersDelgates = new List<DelegateRole>();
                List<DelegateRole> tmpUsersDelegates = new List<DelegateRole>();
                List<string> usersPersite = new List<string>();

                usersPersite = GetUsersPerSite(site);

                usersDelgates = DelegateRole.GetDelegees();

                tmpUsersDelegates = usersDelgates.Where(item => usersPersite.Contains(item.SipAccount)).ToList();

                ManageDelegatesGrid.GetStore().DataSource = tmpUsersDelegates;
                ManageDelegatesGrid.GetStore().DataBind();
            }
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
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.NormalUserInfo.SipAccount;

            string json = e.ExtraParams["Values"];

            List<DelegateRole> recordsToUpate = new List<DelegateRole>();

            ChangeRecords<DelegateRole> toBeUpdated = new StoreDataHandler(e.ExtraParams["Values"]).BatchObjectData<DelegateRole>();

            if (toBeUpdated.Updated.Count > 0)
            {

                foreach (DelegateRole userDelgate in toBeUpdated.Updated)
                {
                    DelegateRole.UpadeDelegate(userDelgate);
                    ManageDelegatesStore.GetById(userDelgate.ID).Commit();
                }
            }

            if (toBeUpdated.Deleted.Count > 0)
            {
                foreach (DelegateRole userDelgate in toBeUpdated.Deleted)
                {
                    DelegateRole.DeleteDelegate(userDelgate);
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