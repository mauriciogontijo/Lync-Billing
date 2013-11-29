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

using Lync_Billing.Backend;
using Lync_Billing.Backend.Roles;

namespace Lync_Billing.ui.admin.gateways
{
    public partial class rates : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SiteAdmin);
        
        private List<Gateway> gateways = new List<Gateway>();
        private List<Gateway> filteredGateways = new List<Gateway>();
        

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/admin/main/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                if (session.ActiveRoleName != allowedRoleName)
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=admin");
                }
            }

            sipAccount = session.NormalUserInfo.SipAccount;

            SitesStore.DataSource = Backend.Site.GetUserRoleSites(session.SystemRoles, Enums.GetDescription(Enums.ValidRoles.IsSiteAdmin));
            SitesStore.DataBind();
        }
        
        protected void UpdateEdited_DirectEvent(object sender, DirectEventArgs e)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.NormalUserInfo.SipAccount;

            string json = e.ExtraParams["Values"];

            List<DelegateRole> recordsToUpate = new List<DelegateRole>();

            ChangeRecords<Rate> toBeUpdated = new StoreDataHandler(e.ExtraParams["Values"]).BatchObjectData<Rate>();

            if (toBeUpdated.Updated.Count > 0)
            {

                string ratesTableName = GetRatesTableName(Convert.ToInt32(FilterGatewaysBySite.SelectedItem.Value));

                foreach (Rate countryRate in toBeUpdated.Updated)
                {
                    List<DialingPrefixsRates> dialingPrefixsRates = DialingPrefixsRates.GetRates(ratesTableName, countryRate.CountryCode);

                    //The Rates Table doesnt have this country codes
                    if (dialingPrefixsRates.Where(item => item.RateID == 0).Count() == dialingPrefixsRates.Count()) 
                    {
                        foreach (DialingPrefixsRates dialingPrefixRate in dialingPrefixsRates) 
                        {
                            if (dialingPrefixRate.TypeOfService == "gsm")
                                dialingPrefixRate.CountryRate = countryRate.MobileLineRate;
                            else
                                dialingPrefixRate.CountryRate = countryRate.FixedLineRate;

                            int rateID = DialingPrefixsRates.InsertRate(ratesTableName, dialingPrefixRate);
                        }

                        ManageRatesStore.Find("CountryCode", countryRate.CountryCode).Commit();
                    }
                    else if (dialingPrefixsRates.Where(item => item.RateID != 0).Count() == dialingPrefixsRates.Count())
                    {
                        //the Rates Table has all the country codes and we need to check if we need to update them or not 
                        //List<NumberingPlan> countryNumberingPlan = NumberingPlan.GetNumberingPlan(countryRate.CountryCode);

                        foreach (DialingPrefixsRates entity in dialingPrefixsRates)
                        {
                            bool status = false;

                            if (entity.TypeOfService == "gsm" && entity.CountryRate != countryRate.MobileLineRate)
                            {
                                status = true;
                                entity.CountryRate = countryRate.MobileLineRate;
                            }
                            else if (entity.TypeOfService != "gsm" && entity.CountryRate != countryRate.MobileLineRate)
                            {
                                status = true;
                                entity.CountryRate = countryRate.FixedLineRate;
                            }
                            else 
                            {
                                status = false;
                            }

                            if (status == true)
                                DialingPrefixsRates.UpdatetRate(ratesTableName, entity);
                            else
                                continue;
                        }

                          ManageRatesStore.Find("CountryCode", countryRate.CountryCode).Commit();
                    }
                    else 
                    {
                        // Some needs to be updated and some needs to be inserted

                        foreach (DialingPrefixsRates dialingPrefixRate in dialingPrefixsRates)
                        {
                            if (dialingPrefixRate.RateID == 0)
                            {
                                if (dialingPrefixRate.TypeOfService == "gsm")
                                    dialingPrefixRate.CountryRate = countryRate.MobileLineRate;
                                else
                                    dialingPrefixRate.CountryRate = countryRate.FixedLineRate;

                                int rateID = DialingPrefixsRates.InsertRate(ratesTableName, dialingPrefixRate);
                            }
                            else if (dialingPrefixRate.RateID != 0)
                            {
                                bool status = false;

                                if (dialingPrefixRate.TypeOfService == "gsm" && dialingPrefixRate.CountryRate != countryRate.MobileLineRate)
                                {
                                    status = true;
                                    dialingPrefixRate.CountryRate = countryRate.MobileLineRate;
                                }
                                else if (dialingPrefixRate.TypeOfService != "gsm" && dialingPrefixRate.CountryRate != countryRate.MobileLineRate)
                                {
                                    status = true;
                                    dialingPrefixRate.CountryRate = countryRate.FixedLineRate;
                                }
                                else
                                {
                                    status = false;
                                }

                                if (status == true)
                                    DialingPrefixsRates.UpdatetRate(ratesTableName, dialingPrefixRate);
                                else
                                    continue;
                            }
                            else 
                            {
                                // Catcher should be implemented 
                            }
                            
                        }

                        ManageRatesStore.Find("CountryCode", countryRate.CountryCode).Commit();

                    }
                }
            }

            if (toBeUpdated.Deleted.Count > 0)
            {
                
            }
        }

        protected void RejectChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            ManageRatesGrid.GetStore().RejectChanges();
        }

        public List<Site> GetAdminSites()
        {
            UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            List<Site> sites = new List<Site>();
            List<SystemRole> userRoles = session.SystemRoles;

            foreach (SystemRole role in userRoles)
            {
                Backend.Site tmpSite = new Backend.Site();

                if (role.SiteID != 0 && (role.IsSiteAdmin() || role.IsDeveloper()))
                {
                    tmpSite.SiteID = role.SiteID;
                    sites.Add(tmpSite);
                }
            }

            List<Site> tmpSites = Backend.Site.GetAllSites();

            foreach (Backend.Site site in sites)
            {
                Backend.Site tmpSite = new Backend.Site();

                tmpSite = tmpSites.First(e => e.SiteID == site.SiteID);

                site.SiteName = tmpSite.SiteName;
                site.CountryCode = tmpSite.CountryCode;
            }

            return sites;
        }

        protected void GetGatewaysForSite(object sender, DirectEventArgs e)
        {
            FilterRatesByGateway.Clear();
            FilterRatesByGateway.ReadOnly = false;

            if (FilterGatewaysBySite.SelectedItem != null && !string.IsNullOrEmpty(FilterGatewaysBySite.SelectedItem.Value))
            {
                List<Gateway> gateways = GetGateways(Convert.ToInt32(FilterGatewaysBySite.SelectedItem.Value));

                FilterRatesByGateway.Disabled = false;
                FilterRatesByGateway.GetStore().DataSource = gateways;
                FilterRatesByGateway.GetStore().DataBind();

                if (gateways.Count == 1)
                {
                    FilterRatesByGateway.SetValueAndFireSelect(gateways[0].GatewayId);
                    FilterRatesByGateway.ReadOnly = true;
                }
                else
                {
                    FilterRatesByGateway.ReadOnly = false;
                }
            }
        }

        public List<Gateway> GetGateways(int siteID)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

            gateways = Gateway.GetGateways();

            //GetSite ID
            //int siteID = GetAdminSites().First(item => item.SiteName == session.SiteName).SiteID;

            //Get Related Gateways for that specific site
            List<GatewayDetail> gatewaysDetails = GatewayDetail.GetGatewaysDetails().Where(item => item.SiteID == siteID).ToList();

            foreach (int id in gatewaysDetails.Select(item => item.GatewayID))
            {
                filteredGateways.Add(gateways.First(item => item.GatewayId == id));
            }

            return filteredGateways;
        }

        public string GetRatesTableName(int siteID)
        {
            if (gateways.Count < 1)
                gateways = GetGateways(siteID);

            string rateTable = string.Empty;

            if (FilterRatesByGateway.SelectedItem.Index == -1)
            {
                return rateTable;
            }
            else
            {
                int gatewayID = Convert.ToInt32(FilterRatesByGateway.SelectedItem.Value);
                rateTable = GatewayRate.GetGatewaysRates(gatewayID).First(item => item.EndingDate == DateTime.MinValue).RatesTableName;
                return rateTable;
            }

        }

        protected void GetRates(object sender, DirectEventArgs e)
        {
            List<GatewayRate> gatewayRates = new List<GatewayRate>();

            string ratesTableName = GetRatesTableName(
                Convert.ToInt32(FilterGatewaysBySite.SelectedItem.Value)
            );

            //Clear Store
            ManageRatesGrid.GetStore().RemoveAll();


            //Fill Store
            if (!string.IsNullOrEmpty(ratesTableName))
            {
                ManageRatesGrid.GetStore().DataSource = Rate.GetRates(ratesTableName);
                ManageRatesGrid.GetStore().DataBind();
            }

        }
    }
}