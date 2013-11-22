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
using Lync_Billing.Libs;

namespace Lync_Billing.ui.user
{
    public partial class rates : System.Web.UI.Page
    {
        UserSession session;
        private string sipAccount = string.Empty;
        private string normalUserRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        private string userDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee);

        private List<Gateway> gateways = new List<Gateway>();
        private List<Gateway> filteredGateways = new List<Gateway>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/user/rates.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
                if (session.ActiveRoleName != normalUserRoleName && session.ActiveRoleName != userDelegeeRoleName)
                {
                    string url = @"~/ui/session/authenticate.aspx?access=" + session.ActiveRoleName;
                    Response.Redirect(url);
                }
            }

            List<Gateway> gateways = GetGateways();

            FilterRatesByGateway.GetStore().DataSource = gateways;
            FilterRatesByGateway.GetStore().DataBind();

            if (gateways.Count == 1) 
            {
                FilterRatesByGateway.SetValueAndFireSelect(gateways[0].GatewayId);
                FilterRatesByGateway.ReadOnly = true;
            }
        }

        public List<Site> getSites() 
        {
            List<Site> sites = new List<DB.Site>();
            return sites = DB.Site.GetAllSites();
        }

        public List<Gateway> GetGateways()
        {
            UserSession session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

            gateways = Gateway.GetGateways();

            //GetSite ID
            DB.Site site = getSites().First(item => item.SiteName == session.SiteName);
            
            int siteID = site.SiteID;
            string siteName = site.SiteName;
            
            //Get Related Gateways for that specific site
            List<GatewayDetail> gatewaysDetails = GatewayDetail.GetGatewaysDetails().Where(item => item.SiteID == siteID).ToList();

            Gateway customGateway;

            foreach (int id in gatewaysDetails.Select(item => item.GatewayID)) 
            {

                customGateway = new Gateway();
                customGateway.GatewayId = id;
                customGateway.GatewayName = GetProviderName(id);
                //filteredGateways.Add(gateways.First(item => item.GatewayId == id));

                filteredGateways.Add(customGateway);
            }

            return filteredGateways;
        }


        public string GetProviderName(int gatewayID) 
        {
            if (gateways.Count < 1)
                gateways = GetGateways();

            return GatewayRate.GetGatewaysRates(gatewayID).First(item => item.EndingDate == DateTime.MinValue).ProviderName;
        }

        public string GetRatesTableName(int gatewayID) 
        {
            if (gateways.Count < 1)
                gateways = GetGateways();

            return GatewayRate.GetGatewaysRates(gatewayID).First(item => item.EndingDate == DateTime.MinValue).RatesTableName;

        }


        protected void GetRates(object sender, DirectEventArgs e)
        {
            List<GatewayRate> gatewayRates = new List<GatewayRate>();

            string ratesTableName = GetRatesTableName(Convert.ToInt32(FilterRatesByGateway.SelectedItem.Value));
            //Clear Store
            ViewRatesGrid.GetStore().RemoveAll();
            
            //Fill Store
            ViewRatesGrid.GetStore().DataSource = Rate.GetRates(ratesTableName);
            ViewRatesGrid.GetStore().DataBind();
            
        }
    }
}