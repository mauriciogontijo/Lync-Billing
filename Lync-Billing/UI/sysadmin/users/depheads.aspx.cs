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
    public partial class depheads : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SystemAdmin);

        private List<Users> allUsers = new List<Users>();
        private List<Site> allSites = new List<Site>();
        private List<SitesDepartments> allDepartments = new List<SitesDepartments>();
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
            allDepartments = SitesDepartments.GetSitesDepartments();
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


        protected void ShowAddDepartmentHeadWindowPanel(object sender, DirectEventArgs e)
        {
            this.AddNewDepartmentHeadWindowPanel.Show();
        }

        protected void CancelNewDepartmentHeadButton_Click(object sender, DirectEventArgs e)
        {
            this.AddNewDepartmentHeadWindowPanel.Hide();
        }

        protected void AddNewDepartmentHeadWindowPanel_BeforeHide(object sender, DirectEventArgs e)
        {
            NewDepartmentHead_UserSipAccount.Clear();
            NewDepartmentHead_SitesList.Value = null;
            NewDepartmentHead_DepartmentsList.Value = null;
            NewDepartmentHead_StatusMessage.Text = string.Empty;
        }

        protected void NewDepartmentHead_UserSipAccount_BeforeQuery(object sender, DirectEventArgs e)
        {
            string searchTerm = string.Empty;
            List<Users> matchedUsers;

            if (NewDepartmentHead_UserSipAccount.Value != null && NewDepartmentHead_UserSipAccount.Value.ToString().Length > 3)
            {
                searchTerm = NewDepartmentHead_UserSipAccount.Value.ToString();

                matchedUsers = Users.SearchForUsers(searchTerm);

                NewDepartmentHead_UserSipAccount.GetStore().DataSource = matchedUsers;
                NewDepartmentHead_UserSipAccount.GetStore().LoadData(matchedUsers);
            }
        }

        protected void NewDepartmentHead_SitesListStore_Load(object sender, EventArgs e)
        {
            NewDepartmentHead_SitesList.GetStore().DataSource = allSites;
            NewDepartmentHead_SitesList.GetStore().LoadData(allSites);
        }

        protected void NewDepartmentHead_SitesList_Selected(object sender, EventArgs e)
        {
            int siteID = Convert.ToInt32(NewDepartmentHead_SitesList.SelectedItem.Value);
            var selectedSiteDepartments = allDepartments.Where(department => department.SiteID == siteID).ToList<SitesDepartments>();

            NewDepartmentHead_DepartmentsList.ClearValue();
            NewDepartmentHead_DepartmentsList.Clear();

            NewDepartmentHead_DepartmentsList.GetStore().DataSource = selectedSiteDepartments;
            NewDepartmentHead_DepartmentsList.GetStore().LoadData(selectedSiteDepartments);
        }

        protected void SaveChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.NormalUserInfo.SipAccount;

            bool statusFlag = false;
            string successMessage = string.Empty;
            string errorMessage = string.Empty;

            string json = string.Empty;
            ChangeRecords<DepartmentHeadRole> storeShangedData;

            json = e.ExtraParams["Values"];

            if (!string.IsNullOrEmpty(json))
            {
                storeShangedData = new StoreDataHandler(json).BatchObjectData<DepartmentHeadRole>();

                //Delete existent DepartmentHeads
                if (storeShangedData.Deleted.Count > 0)
                {
                    foreach (DepartmentHeadRole departmentHead in storeShangedData.Deleted)
                    {
                        statusFlag = DepartmentHeadRole.DeleteDepartmentHead(departmentHead);
                    }

                    if (statusFlag == true)
                    {
                        successMessage = "Department Head(s) were deleted successfully, changes were saved.";
                        HelperFunctions.Message("Delete Departmen tHeads", successMessage, "success", hideDelay: 10000, width: 200, height: 100);

                    }
                    else
                    {
                        errorMessage = "Some Department Heads were NOT deleted, please try again!";
                        HelperFunctions.Message("Delete Department Heads", errorMessage, "error", hideDelay: 10000, width: 200, height: 100);
                    }
                }
            }
        }

        protected void AddNewDepartmentHeadButton_Click(object sender, DirectEventArgs e)
        {
            DepartmentHeadRole newDepartmentHead;

            int SiteID = 0;
            int DepartmentID = 0;
            string SiteName = string.Empty;
            string DepartmentName = string.Empty;
            string DepartmentHeadSipAccount = string.Empty;

            string statusMessage = string.Empty;
            string successStatusMessage = string.Empty;

            if (NewDepartmentHead_UserSipAccount.SelectedItem.Index != -1 && NewDepartmentHead_SitesList.SelectedItem.Index != -1 && NewDepartmentHead_DepartmentsList.SelectedItem.Index != -1)
            {
                newDepartmentHead = new DepartmentHeadRole();

                DepartmentHeadSipAccount = NewDepartmentHead_UserSipAccount.SelectedItem.Value.ToString();
                var DepartmentHeadUserAccount = Users.GetUser(DepartmentHeadSipAccount);
                
                //Check for duplicates
                if (allDepartmenHeads.Find(item => item.SipAccount == DepartmentHeadSipAccount) != null)
                {
                    statusMessage = "Cannot add duplicate Department Heads!";
                }
                //This DepartmentHead record doesn't exist, add it.
                else
                {
                    SiteName = NewDepartmentHead_SitesList.SelectedItem.Text;
                    DepartmentName = NewDepartmentHead_DepartmentsList.SelectedItem.Text;
                    SiteID = Convert.ToInt32(NewDepartmentHead_SitesList.SelectedItem.Value);
                    DepartmentID = Convert.ToInt32(NewDepartmentHead_DepartmentsList.SelectedItem.Value);

                    newDepartmentHead.SiteID = SiteID;
                    newDepartmentHead.SiteName = SiteName;
                    newDepartmentHead.DepartmentID = DepartmentID;
                    newDepartmentHead.DepartmentName = DepartmentName;
                    newDepartmentHead.SipAccount = DepartmentHeadSipAccount;
                    newDepartmentHead.DepartmentHeadName = HelperFunctions.FormatUserDisplayName(DepartmentHeadUserAccount.FullName, DepartmentHeadUserAccount.SipAccount, returnNameIfExists: true, returnAddressPartIfExists: true);

                    //Insert the DepartmentHead to the database
                    DepartmentHeadRole.AddDepartmentHead(newDepartmentHead);

                    //Close the window
                    this.AddNewDepartmentHeadWindowPanel.Hide();

                    //Add the DepartmentHead record to the store and apply the filter
                    ManageDepartmentHeadsGrid.GetStore().Add(newDepartmentHead);

                    //Apply the filter on SiteID if the Site was already selected from the FilterDepartmentHeadsBySite combobox
                    if (FilterDepartmentHeadsBySite.SelectedItem.Index != -1)
                    {
                        ManageDepartmentHeadsGrid.GetStore().Filter("SiteID", FilterDepartmentHeadsBySite.SelectedItem.Value);
                    }

                    successStatusMessage = "Department Head was added successfully, select their respective Site from the menu for more information.";
                }
            }
            else
            {
                statusMessage = "Please provide all the required information!";
            }

            this.NewDepartmentHead_StatusMessage.Text = statusMessage;

            if (!string.IsNullOrEmpty(successStatusMessage))
                HelperFunctions.Message("New Department Head", successStatusMessage, "success", hideDelay: 10000, width: 200, height: 100);
        }

    }

}