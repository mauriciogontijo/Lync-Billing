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
        }


        protected void ManageSitesGridStore_Load(object sender, EventArgs e)
        {
            ManageSitesGrid.GetStore().DataSource = allSites;
            ManageSitesGrid.GetStore().DataBind();
        }


        protected void Editor_CountryNameComboboxStore_Load(object sender, EventArgs e)
        {
            Editor_CountryNameCombobox.GetStore().DataSource = allCountries;
            Editor_CountryNameCombobox.GetStore().LoadData(allCountries);
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

                        //Validate the Country Name
                        //If the submitted country name doesn't exist in the database, exit and display error message
                        if (countryInfo == null)
                        {
                            messageType = "error";
                            notificationMessage = String.Format("[{0}] was NOT updated successfully due to invalid Country Name. Please try again.", storeSiteObject.SiteName);

                            break;
                        }


                        //Everything is ok, proceed with updating the site
                        storeSiteObject.CountryCode = countryInfo.TwoDigitsCountryCode.ToLower();
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
    }
}