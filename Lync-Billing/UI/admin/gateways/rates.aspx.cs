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
using Lync_Billing.DB;

namespace Lync_Billing.ui.admin.gateways
{
    public partial class rates : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
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
                //Response.Redirect("~/ui/session/login.aspx");
            }
            else
            {
                UserSession session = new UserSession();
                session = (UserSession)Session.Contents["UserData"];

                if (session.ActiveRoleName != "admin")
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=admin");
                }
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;

            SitesStore.DataSource = GetAdminSites();
            SitesStore.DataBind();
        }

        protected void UpdateEdited_DirectEvent(object sender, DirectEventArgs e)
        {
            UserSession userSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

            sipAccount = userSession.EffectiveSipAccount;
            string json = e.ExtraParams["Values"];

            List<UsersDelegates> recordsToUpate = new List<UsersDelegates>();

            ChangeRecords<Rate> toBeUpdated = new StoreDataHandler(e.ExtraParams["Values"]).BatchObjectData<Rate>();

            if (toBeUpdated.Updated.Count > 0)
            {

                string ratesTableName = GetRatesTableName(Convert.ToInt32(FilterGatewaysBySite.SelectedItem.Value));

                foreach (Rate countryRate in toBeUpdated.Updated)
                {
                    List<DialingPrefixsRates> dialingPrefixsRates = DialingPrefixsRates.GetRates(ratesTableName, countryRate.CountryCode);

                    if (dialingPrefixsRates.Count == 0) 
                    {
                        List<NumberingPlan> countryDialingPrefixes = NumberingPlan.GetNumberingPlan(countryRate.CountryCode);
                    }
                    else
                    {
                        List<NumberingPlan> countryNumberingPlan = NumberingPlan.GetNumberingPlan(countryRate.CountryCode);

                        //Numbering Plan table has the same count of entries for that country with dialing Prefixes view
                        // Which means that we are going to check record by record and update if not updated 
                        if (countryNumberingPlan.Count == dialingPrefixsRates.Count)
                        {
                            DialingPrefixsRates dialingPrefixRate;
                            foreach (DialingPrefixsRates entity in dialingPrefixsRates)
                            {
                                dialingPrefixRate = new DialingPrefixsRates();

                                dialingPrefixRate.DialingPrefix = entity.DialingPrefix;
                                dialingPrefixRate.RateID = entity.RateID;

                                if (entity.TypeOfService == "gsm")
                                    dialingPrefixRate.CountryRate = countryRate.MobileLineRate;
                                else
                                    dialingPrefixRate.CountryRate = countryRate.FixedLineRate;

                                DialingPrefixsRates.UpdatetRate(ratesTableName, dialingPrefixRate);
                            }
                        }
                        else
                        {
                            // they are not the same and we need to update or insert based on what we have from the orginal numbering plan table
                                //
                                //if(countryNumberingPlan.Where(item => item.))
                        }
                    }
                    //ManageRatesStore.GetById(countryRate.RateID).Commit();
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
            UserSession session = (UserSession)Session.Contents["UserData"];

            List<Site> sites = new List<Site>();
            Site site;

            List<UserRole> userRoles = session.Roles;

            foreach (UserRole role in userRoles)
            {
                if (role.RoleID == 5 || role.RoleID == 1)
                {
                    site = new DB.Site();
                    site = DB.Site.getSite(role.SiteID);

                    sites.Add(site);
                }
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
            UserSession session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

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