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


namespace Lync_Billing.ui.sysadmin.manage
{
    public partial class departments : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SystemAdmin);

        private List<Site> allSites;
        private List<SitesDepartments> allDepartments;


        protected void Page_Load(object sender, EventArgs e)
        {
            // !!!! 
            //  REDIRECTION 
            // !!!
            Response.Redirect("~/ui/sysadmin/main/dashboard.aspx");

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
            allDepartments = SitesDepartments.GetSitesDepartments();
        }

        protected void FilterDepartmentsBySiteStore_Load(object sender, EventArgs e)
        {
            FilterDepartmentsBySite.GetStore().DataSource = allSites;
            NewDepartment_SitesList.GetStore().LoadData(allSites);
        }

        protected void GetDepartmentsPerSite(object sender, DirectEventArgs e)
        {
            if(FilterDepartmentsBySite.SelectedItem.Index > -1)
            {
                int selectedSiteID = Convert.ToInt32(FilterDepartmentsBySite.SelectedItem.Value);

                var filteredDepartmentsList = allDepartments.Where(department => department.SiteID == selectedSiteID).ToList<SitesDepartments>();

                ManageDepartmentsGrid.GetStore().ClearFilter();
                ManageDepartmentsGrid.GetStore().DataSource = filteredDepartmentsList;
                ManageDepartmentsGrid.GetStore().LoadData(filteredDepartmentsList);
            }
        }

        protected void NewDepartment_SitesListStore_Load(object sender, EventArgs e)
        {
            NewDepartment_SitesList.GetStore().DataSource = allSites;
            NewDepartment_SitesList.GetStore().LoadData(allSites);
        }

        protected void ShowAddNewDepartmentWindowPanel(object sender, DirectEventArgs e)
        {
            this.AddNewDepartmentWindowPanel.Show();
        }

        protected void CancelNewDepartmentButton_Click(object sender, DirectEventArgs e)
        {
            this.AddNewDepartmentWindowPanel.Hide();
        }

        protected void AddNewDepartmentWindowPanel_BeforeHide(object sender, DirectEventArgs e)
        {
            NewDepartment_SitesList.Value = null;
            NewDepartment_DepartmentName.Text = null;
            NewDepartment_Description.Text = null;
        }

        protected void SaveChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.NormalUserInfo.SipAccount;

            bool statusFlag = false;
            string messageType = "error";
            string notificationMessage = string.Empty;

            string json = string.Empty;
            ChangeRecords<SitesDepartments> storeShangedData;

            json = e.ExtraParams["Values"];

            if (!string.IsNullOrEmpty(json))
            {
                storeShangedData = new StoreDataHandler(json).BatchObjectData<SitesDepartments>();

                //Update existent departments
                if (storeShangedData.Updated.Count > 0)
                {
                    foreach (SitesDepartments storeDepartmentObject in storeShangedData.Updated)
                    {
                        var originalDepartmentObject = allDepartments.Find(department => department.DepartmentID == storeDepartmentObject.DepartmentID);

                        //Check for duplicate AllDepartments names
                        //If the department name was changed
                        if (storeDepartmentObject.DepartmentName != originalDepartmentObject.DepartmentName)
                        {
                            if (allDepartments.Find(department => department.SiteID == storeDepartmentObject.SiteID && department.DepartmentName == storeDepartmentObject.DepartmentName) != null)
                            {
                                ManageDepartmentsGrid.GetStore().RejectChanges();

                                messageType = "error";
                                notificationMessage = String.Format("[{0}] was not changed to [{1}] due to a duplicate Departments Names. Please enter a valide Department Name.", originalDepartmentObject.DepartmentName, storeDepartmentObject.DepartmentName);

                                break;
                            }
                        }

                        //statusFlag = SitesDepartments.UpdateDepartment(storeDepartmentObject, FORCE_RESET_DESCRIPTION: resetDescriptionFlag);
                        
                        if (statusFlag == false)
                        {
                            ManageDepartmentsGrid.GetStore().Filter("SiteID", storeDepartmentObject.SiteID.ToString());

                            messageType = "error";
                            notificationMessage = String.Format("The department(s) were NOT updated successfully. An error has occured. Please try again.");
                        }
                        else
                        {
                            ManageDepartmentsGrid.GetStore().GetById(storeDepartmentObject.DepartmentID).Commit();
                            ManageDepartmentsGrid.GetStore().Filter("SiteID", storeDepartmentObject.SiteID.ToString());

                            messageType = "success";
                            notificationMessage = "Department(s) were updated successfully, changes were saved.";
                        }
                    }//end for-each

                    HelperFunctions.Message("Update Departments", notificationMessage, messageType, hideDelay: 10000, width: 200, height: 120);
                }//end inner-if
            }//end outer-if
        }


        protected void AddNewDepartmentButton_Click(object sender, DirectEventArgs e)
        {
            int SiteID;
            string DepartmentName = string.Empty;
            string Description = string.Empty;

            SitesDepartments NewDepartment;

            string statusMessage = string.Empty;
            string successStatusMessage = string.Empty;

            if (NewDepartment_SitesList.SelectedItem.Index > -1 && !string.IsNullOrEmpty(NewDepartment_DepartmentName.Text))
            {
                SiteID = Convert.ToInt32(NewDepartment_SitesList.SelectedItem.Value);
                DepartmentName = NewDepartment_DepartmentName.Text;

                if (!string.IsNullOrEmpty(NewDepartment_Description.Text))
                    Description = NewDepartment_Description.Text;

                //Check for duplicates
                if (allDepartments.Find(department => department.SiteID == SiteID && department.DepartmentName == DepartmentName) != null)
                {
                    statusMessage = "Cannot add duplicate Departments.";
                }
                else
                {
                    NewDepartment = new SitesDepartments();

                    NewDepartment.SiteID = SiteID;
                    NewDepartment.SiteName = ((Backend.Site)allSites.Find(site => site.SiteID == SiteID)).SiteName;
                    NewDepartment.DepartmentName = DepartmentName;

                    //Insert the New Department to the database
                    //SitesDepartments.AddDepartment(NewDepartment);

                    //Close the window
                    this.AddNewDepartmentWindowPanel.Hide();

                    //Add the New Department record to the store and apply the filter
                    ManageDepartmentsGrid.GetStore().Add(NewDepartment);
                    ManageDepartmentsGrid.GetStore().Reload();

                    if (FilterDepartmentsBySite.SelectedItem.Index > -1)
                    {
                        ManageDepartmentsGrid.GetStore().Filter("SiteID", FilterDepartmentsBySite.SelectedItem.Value);
                    }

                    successStatusMessage = String.Format("The department was added successfully. {0} ({1})", NewDepartment.SiteName, NewDepartment.DepartmentName);
                }
            }
            else
            {
                statusMessage = "Please provide all the required information!";
            }

            this.NewDepartment_StatusMessage.Text = statusMessage;

            if (!string.IsNullOrEmpty(successStatusMessage))
                HelperFunctions.Message("New Department", successStatusMessage, "success", hideDelay: 10000, width: 200, height: 100);
        }

    }

}