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

        private List<Site> allSites = new List<Site>();
        private List<Department> allDepartments = new List<Department>();
        private List<Users> allUsers = new List<Users>();
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
            
            FilterDelegatesBySite.GetStore().DataSource = allSites;
            FilterDelegatesBySite.GetStore().DataBind();
            
            NewDelegee_SitesList.GetStore().DataSource = allSites;
            NewDelegee_SitesList.GetStore().DataBind();

            NewDelegee_DepartmentsList.GetStore().DataSource = allDepartments;
            NewDelegee_DepartmentsList.GetStore().DataBind();
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

            string json = string.Empty;
            ChangeRecords<DelegateRole> toBeUpdated;

            json = e.ExtraParams["Values"];

            if (!string.IsNullOrEmpty(json))
            {
                toBeUpdated = new StoreDataHandler(json).BatchObjectData<DelegateRole>();

                //if (toBeUpdated.Updated.Count > 0)
                //{

                //    foreach (DelegateRole userDelgate in toBeUpdated.Updated)
                //    {
                //        DelegateRole.UpadeDelegate(userDelgate);
                //        ManageDelegatesStore.GetById(userDelgate.ID).Commit();
                //    }
                //}

                if (toBeUpdated.Deleted.Count > 0)
                {
                    foreach (DelegateRole userDelgate in toBeUpdated.Deleted)
                    {
                        DelegateRole.DeleteDelegate(userDelgate);
                        ManageDelegatesStore.GetById(userDelgate.ID).Commit();
                    }
                }
            }
        }

        protected void RejectChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            ManageDelegatesGrid.GetStore().RejectChanges();
        }


        protected void ShowAddDelegeePanel(object sender, DirectEventArgs e)
        {
            this.AddNewDelegeeWindowPanel.Show();
        }

        protected void DelegeeTypeMenu_Selected(object sender, DirectEventArgs e)
        {
            if (NewDelegee_DelegeeTypeCombobox.SelectedItem.Index != -1 && NewDelegee_DelegeeTypeCombobox.SelectedItem.Value != null)
            {
                var selected = Convert.ToInt32(NewDelegee_DelegeeTypeCombobox.SelectedItem.Value);

                if (selected == DelegateRole.DepartmentDelegeeTypeID)
                {
                    NewDelegee_DepartmentsList.Hidden = false;
                    NewDelegee_SitesList.Hidden = true;
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

        
        protected void AddNewDelegeeButton_Click(object sender, DirectEventArgs e)
        {
            DelegateRole newDelegee;

            int selectedType;
            string userSipAccount;
            string delegeeSipAccount;
            int delegeeSiteID;
            int delegeeDepartmentID;

            string statusMessage = string.Empty;

            if (NewDelegee_DelegeeTypeCombobox.SelectedItem.Index != -1 && NewDelegee_UserSipAccount.SelectedItem.Index != -1 && NewDelegee_DelegeeSipAccount.SelectedItem.Index != -1)
            {
                newDelegee = new DelegateRole();

                selectedType = Convert.ToInt32(NewDelegee_DelegeeTypeCombobox.SelectedItem.Value);
                userSipAccount = NewDelegee_UserSipAccount.SelectedItem.Value.ToString();
                delegeeSipAccount = NewDelegee_DelegeeSipAccount.SelectedItem.Value.ToString();

                var delegates = DelegateRole.GetDelegees(delegeeSipAccount);
                var userAccount = Users.GetUser(userSipAccount);
                var delegeeAccount = Users.GetUser(delegeeSipAccount);

                //Check if the users exist in our system
                if (userAccount == null || delegeeAccount == null)
                {
                    statusMessage = "Either one of the SipAccounts or both of them don't exist!";
                }
                //Check for duplicates
                else if (delegates.Find(item => item.SipAccount == userSipAccount) != null)
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
                        newDelegee.DelegeeAccount = delegeeSipAccount;
                        newDelegee.Description = Enums.GetDescription(Enums.DelegateTypes.UserDelegeeTypeDescription);

                        DelegateRole.AddDelegate(newDelegee);

                        ManageDelegatesStore.Add(newDelegee);

                        this.AddNewDelegeeWindowPanel.Hide();
                    }

                    else if (selectedType == DelegateRole.DepartmentDelegeeTypeID)
                    {
                        if (NewDelegee_DepartmentsList.SelectedItem.Index != -1)
                        {
                            delegeeDepartmentID = Convert.ToInt32(NewDelegee_DepartmentsList.SelectedItem.Value);

                            newDelegee.DelegeeType = DelegateRole.DepartmentDelegeeTypeID;
                            newDelegee.SipAccount = userSipAccount;
                            newDelegee.DelegeeAccount = delegeeSipAccount;
                            newDelegee.DepartmentID = delegeeDepartmentID;
                            newDelegee.Description = Enums.GetDescription(Enums.DelegateTypes.DepartemntDelegeeTypeDescription);

                            DelegateRole.AddDelegate(newDelegee);

                            ManageDelegatesStore.Add(newDelegee);

                            this.AddNewDelegeeWindowPanel.Hide();
                        }
                        else
                        {
                            statusMessage = "Please select a department!";
                        }
                    }

                    else if (selectedType == DelegateRole.SiteDelegeeTypeID)
                    {
                        if (NewDelegee_SitesList.SelectedItem.Index != -1)
                        {
                            delegeeSiteID = Convert.ToInt32(NewDelegee_SitesList.SelectedItem.Value);

                            newDelegee.DelegeeType = DelegateRole.SiteDelegeeTypeID;
                            newDelegee.SipAccount = userSipAccount;
                            newDelegee.DelegeeAccount = delegeeSipAccount;
                            newDelegee.SiteID = delegeeSiteID;
                            newDelegee.Description = Enums.GetDescription(Enums.DelegateTypes.SiteDelegeeTypeDescription);

                            DelegateRole.AddDelegate(newDelegee);

                            ManageDelegatesStore.Add(newDelegee);

                            this.AddNewDelegeeWindowPanel.Hide();
                        }
                        else
                        {
                            statusMessage = "Please select a site!";
                        }
                    }
                }//End else
            }
            else
            {
                statusMessage = "Please provide the required information!";
            }

            this.NewDelegee_StatusMessage.Text = statusMessage;
            //this.NewDelegee_StatusMessage = Icon.Error;
        }


        protected void CancelNewDelegeeButton_Click(object sender, DirectEventArgs e)
        {
            this.AddNewDelegeeWindowPanel.Hide();
        }


        protected void NewDelegee_UserSipAccount_BeforeQuery(object sender, DirectEventArgs e)
        {
            string searchTerm = string.Empty;
            List<Users> matchedUsers;

            if (NewDelegee_UserSipAccount.Value != null && NewDelegee_UserSipAccount.Value.ToString().Length > 3)
            {
                searchTerm = NewDelegee_UserSipAccount.Value.ToString();
                
                matchedUsers = Users.SearchForUsers(searchTerm);

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

                matchedUsers = Users.SearchForUsers(searchTerm);

                NewDelegee_DelegeeSipAccount.GetStore().LoadData(matchedUsers);
            }
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

    }

}