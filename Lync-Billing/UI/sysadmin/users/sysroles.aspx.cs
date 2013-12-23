using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using Newtonsoft.Json;

using Lync_Billing.Libs;
using Lync_Billing.Backend;
using Lync_Billing.Backend.Roles;

namespace Lync_Billing.ui.sysadmin.users
{
    public partial class sysroles : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SystemAdmin);

        private List<Site> allSites = new List<Site>();
        private List<SystemRole> allSystemRoles = new List<SystemRole>();

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

            //Get all Sites
            allSites = Backend.Site.GetAllSites();

            //Get all Site Admins and Site Accountants Roles
            Dictionary<string, object> conditions = new Dictionary<string, object>{
                { Enums.GetDescription(Enums.SystemRoles.RoleID), String.Format("{0},{1}", SystemRole.SiteAdminRoleID, SystemRole.SiteAdminRoleID) }
            };

            allSystemRoles = SystemRole.GetSystemRoles(wherePart: conditions);
        }


        //DIRECT EVENT CALLED BY THE FilterSystemRolesPerSite DROPDOWN MENU
        protected void GetSystemRolesPerSite(object sender, DirectEventArgs e)
        {
            if (FilterSystemRolesBySite.SelectedItem.Index != -1)
            {
                int siteID = Convert.ToInt32(FilterSystemRolesBySite.SelectedItem.Value);

                ManageSystemRolesGrid.GetStore().DataSource = allSystemRoles.Where(role => role.SiteID == siteID).ToList<SystemRole>();
                ManageSystemRolesGrid.GetStore().DataBind();
            }
        }


        protected void FilterSystemRolesBySiteStore_Load(object sender, EventArgs e)
        {
            //FilterSystemRolesBySite.GetStore().DataSource = Backend.Site.GetUserRoleSites(session.SystemRoles, Enums.GetDescription(Enums.ValidRoles.IsSystemAdmin));
            FilterSystemRolesBySite.GetStore().DataSource = allSites;
            FilterSystemRolesBySite.GetStore().DataBind();
        }


        protected void NewSystemRole_UserSipAccount_BeforeQuery(object sender, DirectEventArgs e)
        {
            string searchTerm = string.Empty;
            List<User> matchedUsers;

            if (NewSystemRole_UserSipAccount.Value != null && NewSystemRole_UserSipAccount.Value.ToString().Length > 3)
            {
                searchTerm = NewSystemRole_UserSipAccount.Value.ToString();

                matchedUsers = User.SearchForUsers(searchTerm);

                NewSystemRole_UserSipAccount.GetStore().DataSource = matchedUsers;
                NewSystemRole_UserSipAccount.GetStore().LoadData(matchedUsers);
            }
        }


        protected void SitesListStore_Load(object sender, EventArgs e)
        {
            //NewSystemRole_SitesList.GetStore().DataSource = allSites;
            //NewSystemRole_SitesList.GetStore().DataBind();
            NewSystemRole_SitesList.GetStore().DataSource = allSites;
            NewSystemRole_SitesList.GetStore().LoadData(allSites);
        }


        protected void ShowAddSystemRoleWindowPanel(object sender, DirectEventArgs e)
        {
            this.AddNewSystemRoleWindowPanel.Show();
        }

        protected void CancelNewSystemRoleButton_Click(object sender, DirectEventArgs e)
        {
            this.AddNewSystemRoleWindowPanel.Hide();
        }

        protected void AddNewSystemRoleWindowPanel_BeforeHide(object sender, DirectEventArgs e)
        {
            NewSystemRole_RoleTypeCombobox.Select(0);
            NewSystemRole_UserSipAccount.Clear();
            NewSystemRole_SitesList.Value = null;
            NewSystemRole_StatusMessage.Text = string.Empty;
        }


        protected void SaveChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.NormalUserInfo.SipAccount;

            bool statusFlag = false;
            string successMessage = string.Empty;
            string errorMessage = string.Empty;

            string json = string.Empty;
            ChangeRecords<SystemRole> storeShangedData;

            json = e.ExtraParams["Values"];

            if (!string.IsNullOrEmpty(json))
            {
                storeShangedData = new StoreDataHandler(json).BatchObjectData<SystemRole>();

                //Delete existent delegees
                if (storeShangedData.Deleted.Count > 0)
                {
                    foreach (SystemRole systemRole in storeShangedData.Deleted)
                    {
                        statusFlag = SystemRole.DeleteSystemRole(systemRole);
                    }

                    if (statusFlag == true)
                    {
                        successMessage = "System Role(s) were deleted successfully, changes were saved.";
                        HelperFunctions.Message("Delete System Roles", successMessage, "success", hideDelay: 10000, width: 200, height: 100);

                    }
                    else
                    {
                        errorMessage = "Some System Roles were NOT deleted, please try again!";
                        HelperFunctions.Message("Delete System Roles", errorMessage, "error", hideDelay: 10000, width: 200, height: 100);
                    }
                }
            }
        }


        protected void AddNewSystemRoleButton_Click(object sender, DirectEventArgs e)
        {
            SystemRole NewSystemRole;

            int siteID = 0;
            int roleTypeID = 0;
            string userSipAccount = string.Empty;
            
            string statusMessage = string.Empty;
            string successStatusMessage = string.Empty;

            if (NewSystemRole_RoleTypeCombobox.SelectedItem.Index != -1 && NewSystemRole_UserSipAccount.SelectedItem.Index != -1 && NewSystemRole_SitesList.SelectedItem.Index != -1)
            {
                NewSystemRole = new SystemRole();

                roleTypeID = Convert.ToInt32(NewSystemRole_RoleTypeCombobox.SelectedItem.Value);
                userSipAccount = NewSystemRole_UserSipAccount.SelectedItem.Value.ToString();

                var userAccount = User.GetUser(userSipAccount);
                
                //Check for duplicates
                if (allSystemRoles.Find(item => item.SipAccount == userSipAccount && item.RoleID == roleTypeID) != null)
                {
                    statusMessage = "Cannot add duplicate delegees!";
                }
                //This system role record doesn't exist, add it.
                else
                {
                    siteID = Convert.ToInt32(NewSystemRole_SitesList.SelectedItem.Value);

                    NewSystemRole.RoleID = roleTypeID;
                    NewSystemRole.SipAccount = userSipAccount;
                    NewSystemRole.RoleOwnerName = HelperFunctions.FormatUserDisplayName(userAccount.FullName, userAccount.SipAccount, returnNameIfExists: true, returnAddressPartIfExists: true);
                    NewSystemRole.SiteID = siteID;
                    NewSystemRole.SiteName = ((Site)allSites.Find(site => site.SiteID == siteID)).SiteName;
                    NewSystemRole.RoleDescription = SystemRole.GetRoleDescription(NewSystemRole.RoleID);
                    NewSystemRole.Description = String.Format("{0} - {1}", NewSystemRole.SiteName, NewSystemRole.RoleDescription);

                    //Insert the delegee to the database
                    SystemRole.AddSystemRole(NewSystemRole);

                    //Close the window
                    this.AddNewSystemRoleWindowPanel.Hide();

                    //Add the delegee record to the store and apply the filter
                    //FilterDelegatesBySite.SetValueAndFireSelect(delegeeSiteID);
                    ManageSystemRolesGrid.GetStore().Add(NewSystemRole);

                    //Apply the filter on SiteID if the Site was already selected from the FilterDelegatesBySite combobox
                    if (FilterSystemRolesBySite.SelectedItem.Index != -1)
                    {
                        ManageSystemRolesGrid.GetStore().Filter("SiteID", FilterSystemRolesBySite.SelectedItem.Value);
                    }

                    successStatusMessage = "System Role was added successfully, select their respective Site from the menu for more information.";
                }//End else
            }
            else
            {
                statusMessage = "Please provide the required information!";
            }

            this.NewSystemRole_StatusMessage.Text = statusMessage;

            if (!string.IsNullOrEmpty(successStatusMessage))
                HelperFunctions.Message("Add New System Role", successStatusMessage, "success", hideDelay: 10000, width: 200, height: 100);
        }
        
    }//End class
}