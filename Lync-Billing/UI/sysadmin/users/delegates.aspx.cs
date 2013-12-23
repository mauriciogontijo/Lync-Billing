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
    public partial class delegates : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SystemAdmin);

        private List<Users> allUsers = new List<Users>();
        private List<Site> allSites = new List<Site>();
        private List<Department> allDepartments = new List<Department>();
        private List<DelegateRole> allDelegees = new List<DelegateRole>();
        private Dictionary<string, int> delegeeTypes = new Dictionary<string, int>();

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
            allDelegees = DelegateRole.GetDelegees();
        }

        protected void GetDelegates(object sender, DirectEventArgs e)
        {
            List<DelegateRole> selectedSitesDelegees;

            if (FilterDelegatesBySite.SelectedItem.Index != -1)
            {
                int siteID = Convert.ToInt32(FilterDelegatesBySite.SelectedItem.Value);
                
                List<string> usersPersite = GetUsersPerSite(siteID);

                selectedSitesDelegees = allDelegees.Where(item => siteID == item.SiteID || usersPersite.Contains(item.SipAccount) || usersPersite.Contains(item.DelegeeSipAccount)).ToList();

                ManageDelegatesGrid.GetStore().ClearFilter();
                ManageDelegatesGrid.GetStore().DataSource = selectedSitesDelegees;
                ManageDelegatesGrid.GetStore().DataBind();
            }
        }

        private List<string> GetUsersPerSite(int siteID)
        {
            List<Users> users = new List<Users>();
            List<string> usersList = new List<string>();

            var siteObject = allSites.Find(site => site.SiteID == siteID);

            users = User.GetUsers(siteObject.SiteName);

            if (users.Count > 0)
            {
                foreach (Users user in users)
                {
                    usersList.Add(user.SipAccount);
                }
            }

            return usersList;
        }


        protected void ShowAddDelegeePanel(object sender, DirectEventArgs e)
        {
            this.AddNewDelegeeWindowPanel.Show();
        }
        
        protected void CancelNewDelegeeButton_Click(object sender, DirectEventArgs e)
        {
            this.AddNewDelegeeWindowPanel.Hide();
        }

        protected void AddNewDelegeeWindowPanel_BeforeHide(object sender, DirectEventArgs e)
        {
            NewDelegee_DelegeeTypeCombobox.Select(0);
            NewDelegee_UserSipAccount.Clear();
            NewDelegee_DelegeeSipAccount.Clear();
            NewDelegee_DepartmentsList.Value = null;
            NewDelegee_SitesList.Value = null;
            NewDelegee_DepartmentsList.Hidden = true;
            NewDelegee_SitesList.Hidden = true;
            NewDelegee_StatusMessage.Text = string.Empty;
        }


        protected void DelegeeTypeMenu_Selected(object sender, DirectEventArgs e)
        {
            if (NewDelegee_DelegeeTypeCombobox.SelectedItem.Index != -1 && NewDelegee_DelegeeTypeCombobox.SelectedItem.Value != null)
            {
                var selected = Convert.ToInt32(NewDelegee_DelegeeTypeCombobox.SelectedItem.Value);

                if (selected == DelegateRole.DepartmentDelegeeTypeID)
                {
                    NewDelegee_DepartmentsList.Hidden = false;
                    NewDelegee_SitesList.Hidden = false;
                }
                else if (selected == DelegateRole.SiteDelegeeTypeID)
                {
                    NewDelegee_SitesList.Hidden = false;
                    NewDelegee_DepartmentsList.Hidden = true;
                }
                else
                {
                    NewDelegee_SitesList.Hidden = true;
                    NewDelegee_DepartmentsList.Hidden = true;
                }
            }
        }


        protected void NewDelegee_UserSipAccount_BeforeQuery(object sender, DirectEventArgs e)
        {
            string searchTerm = string.Empty;
            List<Users> matchedUsers;

            if (NewDelegee_UserSipAccount.Value != null && NewDelegee_UserSipAccount.Value.ToString().Length > 3)
            {
                searchTerm = NewDelegee_UserSipAccount.Value.ToString();
                
                matchedUsers = User.SearchForUsers(searchTerm);

                NewDelegee_UserSipAccount.GetStore().DataSource = matchedUsers;
                NewDelegee_UserSipAccount.GetStore().LoadData(matchedUsers);
            }
        }


        protected void NewDelegee_DelegeeSipAccount_BeforeQuery(object sender, DirectEventArgs e)
        {
            string searchTerm = string.Empty;
            List<Users> matchedUsers;

            if (NewDelegee_DelegeeSipAccount.Value != null && NewDelegee_DelegeeSipAccount.Value.ToString().Length > 3)
            {
                searchTerm = NewDelegee_DelegeeSipAccount.Value.ToString();

                matchedUsers = User.SearchForUsers(searchTerm);

                NewDelegee_DelegeeSipAccount.GetStore().DataSource = matchedUsers;
                NewDelegee_DelegeeSipAccount.GetStore().LoadData(matchedUsers);
            }
        }

        protected void SitesListStore_Load(object sender, EventArgs e)
        {
            //NewDelegee_SitesList.GetStore().DataSource = allSites;
            //NewDelegee_SitesList.GetStore().DataBind();
            NewDelegee_SitesList.GetStore().DataSource = allSites;
            NewDelegee_SitesList.GetStore().LoadData(allSites);
        }

        protected void DelegatesSitesStore_Load(object sender, EventArgs e)
        {
            //FilterDelegatesBySite.GetStore().DataSource = allSites;
            //FilterDelegatesBySite.GetStore().DataBind();
            FilterDelegatesBySite.GetStore().DataSource = allSites;
            FilterDelegatesBySite.GetStore().LoadData(allSites);
        }

        protected void NewDelegee_SitesList_Selected(object sender, EventArgs e)
        {
            int siteID = Convert.ToInt32(NewDelegee_SitesList.SelectedItem.Value);
            var selectedSiteDepartments = allDepartments.Where(department => department.SiteID == siteID).ToList<Department>();

            NewDelegee_DepartmentsList.ClearValue();
            NewDelegee_DepartmentsList.Clear();

            NewDelegee_DepartmentsList.GetStore().DataSource = selectedSiteDepartments;
            NewDelegee_DepartmentsList.GetStore().LoadData(selectedSiteDepartments);
        }


        protected void SaveChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.NormalUserInfo.SipAccount;

            bool statusFlag = false;
            string successMessage = string.Empty;
            string errorMessage = string.Empty;

            string json = string.Empty;
            ChangeRecords<DelegateRole> storeShangedData;

            json = e.ExtraParams["Values"];

            if (!string.IsNullOrEmpty(json))
            {
                storeShangedData = new StoreDataHandler(json).BatchObjectData<DelegateRole>();

                //Delete existent delegees
                if (storeShangedData.Deleted.Count > 0)
                {
                    foreach (DelegateRole userDelgate in storeShangedData.Deleted)
                    {
                        statusFlag = DelegateRole.DeleteDelegate(userDelgate);
                    }

                    if (statusFlag == true)
                    {
                        successMessage = "Delegee(s) were deleted successfully, changes were saved.";
                        HelperFunctions.Message("Delete Delegees", successMessage, "success", hideDelay: 10000, width: 200, height: 100);

                    }
                    else
                    {
                        errorMessage = "Some delegees weren't deleted, please try again!";
                        HelperFunctions.Message("Delete Delegees", errorMessage, "error", hideDelay: 10000, width: 200, height: 100);
                    }
                }
            }
        }


        protected void AddNewDelegeeButton_Click(object sender, DirectEventArgs e)
        {
            DelegateRole newDelegee;

            int selectedType = 0;
            string userSipAccount = string.Empty;
            string delegeeSipAccount = string.Empty;
            int delegeeSiteID = 0;
            int delegeeDepartmentID = 0;

            string statusMessage = string.Empty;
            string successStatusMessage = string.Empty;

            if (NewDelegee_DelegeeTypeCombobox.SelectedItem.Index != -1 && NewDelegee_UserSipAccount.SelectedItem.Index != -1 && NewDelegee_DelegeeSipAccount.SelectedItem.Index != -1)
            {
                newDelegee = new DelegateRole();

                selectedType = Convert.ToInt32(NewDelegee_DelegeeTypeCombobox.SelectedItem.Value);
                userSipAccount = NewDelegee_UserSipAccount.SelectedItem.Value.ToString();
                delegeeSipAccount = NewDelegee_DelegeeSipAccount.SelectedItem.Value.ToString();

                var delegates = DelegateRole.GetDelegees(delegeeSipAccount);
                var userAccount = User.GetUser(userSipAccount);
                var delegeeAccount = User.GetUser(delegeeSipAccount);

                //Check for duplicates
                if (delegates.Find(item => item.SipAccount == userSipAccount) != null)
                {
                    statusMessage = "Cannot add duplicate delegees!";
                }
                //This delegee record doesn't exist, add it.
                else
                {
                    if (selectedType == DelegateRole.UserDelegeeTypeID)
                    {
                        newDelegee.DelegeeType = DelegateRole.UserDelegeeTypeID;
                        newDelegee.SipAccount = userSipAccount;
                        newDelegee.DelegeeSipAccount = delegeeSipAccount;
                        newDelegee.Description = Enums.GetDescription(Enums.DelegateTypes.UserDelegeeTypeDescription);

                        //Insert the delegee to the database
                        DelegateRole.AddDelegate(newDelegee);

                        //Close the window
                        this.AddNewDelegeeWindowPanel.Hide();

                        //FilterDelegatesBySite.SetValueAndFireSelect(delegeeSiteID);
                        ManageDelegatesGrid.GetStore().Add(newDelegee);
                        
                        //Add the delegee record to the store and apply the filter
                        //Apply the filter on SiteID if the Site was already selected from the FilterDelegatesBySite combobox
                        if (FilterDelegatesBySite.SelectedItem.Index != -1)
                        {
                            ManageDelegatesGrid.GetStore().Filter("SiteID", FilterDelegatesBySite.SelectedItem.Value);
                        }

                        successStatusMessage = "Delegee was added successfully, select their respective Site from the menu for more information.";
                    }

                    else if (selectedType == DelegateRole.DepartmentDelegeeTypeID)
                    {
                        if (NewDelegee_SitesList.SelectedItem.Index != -1 && NewDelegee_DepartmentsList.SelectedItem.Index != -1)
                        {
                            delegeeSiteID = Convert.ToInt32(NewDelegee_SitesList.SelectedItem.Value);
                            delegeeDepartmentID = Convert.ToInt32(NewDelegee_DepartmentsList.SelectedItem.Value);

                            newDelegee.DelegeeType = DelegateRole.DepartmentDelegeeTypeID;
                            newDelegee.SipAccount = userSipAccount;
                            newDelegee.DelegeeSipAccount = delegeeSipAccount;
                            newDelegee.SiteID = delegeeSiteID;
                            newDelegee.DelegeeSiteName = ((Site)allSites.Find(site => site.SiteID == delegeeSiteID)).SiteName;
                            newDelegee.DepartmentID = delegeeDepartmentID;
                            newDelegee.DelegeeDepartmentName = ((Department)allDepartments.Find(department => department.DepartmentID == delegeeDepartmentID)).DepartmentName;
                            newDelegee.Description = Enums.GetDescription(Enums.DelegateTypes.DepartemntDelegeeTypeDescription);

                            //Insert the delegee to the database
                            DelegateRole.AddDelegate(newDelegee);

                            //Close the window
                            this.AddNewDelegeeWindowPanel.Hide();

                            //Add the delegee record to the store and apply the filter
                            //FilterDelegatesBySite.SetValueAndFireSelect(delegeeSiteID);
                            ManageDelegatesGrid.GetStore().Add(newDelegee);

                            //Apply the filter on SiteID if the Site was already selected from the FilterDelegatesBySite combobox
                            if (FilterDelegatesBySite.SelectedItem.Index != -1)
                            {
                                ManageDelegatesGrid.GetStore().Filter("SiteID", FilterDelegatesBySite.SelectedItem.Value);
                            }

                            successStatusMessage = "Delegee was added successfully, select their respective Site from the menu for more information.";
                        }
                        else
                        {
                            statusMessage = "Please select a Site and a Department!";
                        }
                    }

                    else if (selectedType == DelegateRole.SiteDelegeeTypeID)
                    {
                        if (NewDelegee_SitesList.SelectedItem.Index != -1)
                        {
                            delegeeSiteID = Convert.ToInt32(NewDelegee_SitesList.SelectedItem.Value);

                            newDelegee.DelegeeType = DelegateRole.SiteDelegeeTypeID;
                            newDelegee.SipAccount = userSipAccount;
                            newDelegee.DelegeeSipAccount = delegeeSipAccount;
                            newDelegee.SiteID = delegeeSiteID;
                            newDelegee.DelegeeSiteName = ((Site)allSites.Find(site => site.SiteID == delegeeSiteID)).SiteName;
                            newDelegee.Description = Enums.GetDescription(Enums.DelegateTypes.SiteDelegeeTypeDescription);

                            //Insert the delegee to the database
                            DelegateRole.AddDelegate(newDelegee);

                            //Close the window
                            this.AddNewDelegeeWindowPanel.Hide();

                            //Add the delegee record to the store and apply the filter
                            //FilterDelegatesBySite.SetValueAndFireSelect(delegeeSiteID);
                            ManageDelegatesGrid.GetStore().Add(newDelegee);

                            //Apply the filter on SiteID if the Site was already selected from the FilterDelegatesBySite combobox
                            if (FilterDelegatesBySite.SelectedItem.Index != -1)
                            {
                                ManageDelegatesGrid.GetStore().Filter("SiteID", FilterDelegatesBySite.SelectedItem.Value);
                            }

                            successStatusMessage = "Delegee was added successfully, select their respective Site from the menu for more information.";
                        }
                        else
                        {
                            statusMessage = "Please select a Site!";
                        }
                    }
                }//End else
            }
            else
            {
                statusMessage = "Please provide the required information!";
            }

            this.NewDelegee_StatusMessage.Text = statusMessage;

            if (!string.IsNullOrEmpty(successStatusMessage))
                HelperFunctions.Message("Add New Delegee", successStatusMessage, "success", hideDelay: 10000, width: 200, height: 100);
        }

    }

}