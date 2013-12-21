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

using Lync_Billing.Libs;
using Lync_Billing.Backend;


namespace Lync_Billing.ui.sysadmin.manage
{
    public partial class sites : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SystemAdmin);

        List<Backend.Site> allSites = new List<Backend.Site>();
        List<NumberingPlan> allCountries = new List<NumberingPlan>();


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
            allCountries = NumberingPlan.GetAllCountries();

            
            /***
             * IMPORTANT
             * Uncomment to assign Three Characters Countries Codes to all Sites
             */
            //foreach (var site in allSites)
            //{
            //    site.CountryCode = ((NumberingPlan)allCountries.Find(country => country.TwoDigitsCountryCode.ToLower() == site.CountryCode.ToLower())).ThreeDigitsCountryCode;
            //    Backend.Site.UpdateSite(site);
            //}
        }


        protected void ManageSitesGridStore_Load(object sender, EventArgs e)
        {
            ManageSitesGrid.GetStore().DataSource = allSites;
            ManageSitesGrid.GetStore().DataBind();
        }


        protected void ManageSites_RowEditing_BeforeEdit(object sender, DirectEventArgs e)
        {
            Editor_CountryNameCombobox.GetStore().DataSource = allCountries;
            Editor_CountryNameCombobox.GetStore().LoadData(allCountries);
        }

        protected void NewSite_CountryListStore_Load(object sender, EventArgs e)
        {
            NewSite_CountryList.GetStore().DataSource = allCountries;
            NewSite_CountryList.GetStore().LoadData(allCountries);
        }


        protected void ShowAddSitePanel(object sender, DirectEventArgs e)
        {
            this.AddNewSiteWindowPanel.Show();
        }


        protected void CancelNewSiteButton_Click(object sender, DirectEventArgs e)
        {
            this.AddNewSiteWindowPanel.Hide();
        }


        protected void AddNewSiteWindowPanel_BeforeHide(object sender, DirectEventArgs e)
        {
            NewSite_SiteName.Text = null;
            NewSite_CountryList.Value = null;
            NewSite_Description.Text = null;
            NewSite_StatusMessage.Text = null;
        }


        protected void SaveChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.NormalUserInfo.SipAccount;

            bool statusFlag = false;
            string messageType = "error";
            string notificationMessage = string.Empty;

            string json = string.Empty;
            ChangeRecords<Backend.Site> storeShangedData;

            json = e.ExtraParams["Values"];

            if (!string.IsNullOrEmpty(json))
            {
                storeShangedData = new StoreDataHandler(json).BatchObjectData<Backend.Site>();

                //Delete existent delegees
                if (storeShangedData.Updated.Count > 0)
                {
                    foreach (Backend.Site storeSiteObject in storeShangedData.Updated)
                    {
                        var originalSiteObject = (Site)allSites.Find(siteRecord => siteRecord.SiteID == storeSiteObject.SiteID);
                        var countryInfo = (NumberingPlan)allCountries.Find(country => country.CountryName == storeSiteObject.CountryName);

                        //Validate the Country Name
                        //If the submitted country name doesn't exist in the database, exit and display error message
                        if (countryInfo == null)
                        {
                            messageType = "error";
                            notificationMessage = String.Format("[{0}] was NOT updated successfully due to invalid Country Name. Please try again.", storeSiteObject.SiteName);

                            break;
                        }

                        //Check for duplicate site name
                        //If the site name was changed
                        if (storeSiteObject.SiteName != originalSiteObject.SiteName)
                        {
                            //If the changed (submitted) SiteName alread exists in the system, exit and display error message
                            if (allSites.Find(site => site.SiteName.ToLower() == storeSiteObject.SiteName.ToLower()) != null)
                            {
                                messageType = "error";
                                notificationMessage = String.Format("[{0}] was not changed to [{1}] due to a duplicate Site Name. Please enter a valide Site Name.", originalSiteObject.SiteName, storeSiteObject.SiteName);

                                break;
                            }
                        }

                        //Everything is ok, proceed with updating the site
                        storeSiteObject.CountryCode = countryInfo.TwoDigitsCountryCode.ToUpper();
                        statusFlag = Backend.Site.UpdateSite(storeSiteObject);

                        //If an error has occured during the Database Update, display error message
                        if (statusFlag == false)
                        {
                            messageType = "error";
                            notificationMessage = String.Format("The site(s) were NOT updated successfully. An error has occured. Please try again.");
                        }
                        else
                        {
                            messageType = "success";
                            notificationMessage = "Site(s) were updated successfully, changes were saved.";

                            ManageSitesGrid.GetStore().GetById(storeSiteObject.SiteID).Commit();
                            ManageSitesGrid.GetStore().Reload();
                        }
                    }

                    HelperFunctions.Message("Update Sites", notificationMessage, messageType, hideDelay: 10000, width: 200, height: 120);
                }//End if
            }//End if
        }


        protected void AddNewSiteButton_Click(object sender, DirectEventArgs e)
        {
            Backend.Site NewSite;

            string SiteName = string.Empty;
            string CountryName = string.Empty;
            string Description = string.Empty;
            Backend.Site SiteInfo;

            string statusMessage = string.Empty;
            string successStatusMessage = string.Empty;

            if (!string.IsNullOrEmpty(NewSite_SiteName.Text) && NewSite_CountryList.SelectedItem.Index != -1)
            {
                NewSite = new Backend.Site();

                SiteName = NewSite_SiteName.Text.ToString();
                CountryName = NewSite_CountryList.SelectedItem.Value.ToString();

                //Check for duplicates
                if (allSites.Find(site => site.SiteName == SiteName) != null)
                {
                    statusMessage = "Cannot add duplicate Sites - with the same name!";
                }
                //This Site record doesn't exist, add it.
                else
                {
                    SiteName = NewSite_SiteName.Text.ToString();
                    CountryName = NewSite_CountryList.SelectedItem.Value.ToString();
                    
                    if(!string.IsNullOrEmpty(NewSite_Description.Text))
                        Description = NewSite_Description.Text.ToString();

                    NewSite.SiteName = SiteName;
                    NewSite.CountryName = CountryName;
                    NewSite.CountryCode = ((NumberingPlan)allCountries.Find(country => country.CountryName == CountryName)).TwoDigitsCountryCode;
                    NewSite.Description = Description;

                    //Insert the DepartmentHead to the database
                    Backend.Site.AddSite(NewSite);

                    //Close the window
                    this.AddNewSiteWindowPanel.Hide();

                    //Add the DepartmentHead record to the store and apply the filter
                    ManageSitesGrid.GetStore().Add(NewSite);
                    ManageSitesGrid.GetStore().Reload();

                    successStatusMessage = String.Format("The site was added successfully. {0} ({1})", NewSite.SiteName, NewSite.CountryCode);
                }
            }
            else
            {
                statusMessage = "Please provide all the required information!";
            }

            this.NewSite_StatusMessage.Text = statusMessage;

            if (!string.IsNullOrEmpty(successStatusMessage))
                HelperFunctions.Message("New Site", successStatusMessage, "success", hideDelay: 10000, width: 200, height: 100);
        }

    }

}