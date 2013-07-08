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
        private string sipAccount = string.Empty;

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
                UserSession session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
                if (session.ActiveRoleName != "user" && session.ActiveRoleName != "delegee")
                {
                    string url = @"~/ui/session/authenticate.aspx?access=" + session.ActiveRoleName;
                    Response.Redirect(url);
                }
            }

            FilterRatesByGateway.GetStore().DataSource = GetGateways();
            FilterRatesByGateway.GetStore().DataBind();
        }

        public List<Site> getSites() 
        {
            List<Site> sites = new List<DB.Site>();
            return sites = DB.Site.GetSites();
        }

        public List<Gateway> GetGateways()
        {
            UserSession session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
           
            List<Gateway> gateways = new List<Gateway>();
            List<Gateway> filteredGateways = new List<Gateway>();

            gateways = Gateway.GetGateways();

            //GetSite ID
            int siteID = getSites().First(item => item.SiteName == session.SiteName).SiteID;
            
            //Get Related Gateways for that specific site
            List<GatewayDetail> gatewaysDetails = GatewayDetail.GetGatewaysDetails().Where(item => item.SiteID == siteID).ToList();

            foreach (int id in gatewaysDetails.Select(item => item.GatewayID)) 
            {
                filteredGateways.Add(gateways.First(item => item.GatewayId == id));
            }

            return filteredGateways;
        }

        protected void GetRates(object sender, DirectEventArgs e)
        {

        }
    }
}