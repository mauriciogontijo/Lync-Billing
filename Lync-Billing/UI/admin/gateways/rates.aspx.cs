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
        }

        protected void UpdateEdited_DirectEvent(object sender, DirectEventArgs e)
        {
            UserSession userSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

            sipAccount = userSession.EffectiveSipAccount;
            string json = e.ExtraParams["Values"];

            //List<UsersDelegates> recordsToUpate = new List<UsersDelegates>();

            //ChangeRecords<UsersDelegates> toBeUpdated = new StoreDataHandler(e.ExtraParams["Values"]).BatchObjectData<UsersDelegates>();

            //if (toBeUpdated.Updated.Count > 0)
            //{

            //    foreach (UsersDelegates userDelgate in toBeUpdated.Updated)
            //    {
            //        UsersDelegates.UpadeDelegate(userDelgate);
            //        ManageRatesStore.GetById(userDelgate.ID).Commit();
            //    }
            //}

            //if (toBeUpdated.Deleted.Count > 0)
            //{
            //    foreach (UsersDelegates userDelgate in toBeUpdated.Deleted)
            //    {
            //        UsersDelegates.DeleteDelegate(userDelgate);
            //        ManageRatesStore.GetById(userDelgate.ID).Commit();
            //    }
            //}
        }

        protected void RejectChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            ManageRatesGrid.GetStore().RejectChanges();
        }

        protected void GetGateways(object sender, DirectEventArgs e)
        {

        }

        public List<Site> getSites()
        {
            List<Site> sites = new List<DB.Site>();
            return sites = DB.Site.GetSites();
        }

        public List<Gateway> GetGateways()
        {
            UserSession session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

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

        public string GetRatesTableName(int gatewayID)
        {
            if (gateways.Count < 1)
                gateways = GetGateways();

            string rateTable = string.Empty;

            return rateTable = GatewayRate.GetGatewaysRates(gatewayID).First(item => item.EndingDate == DateTime.MinValue).RatesTableName;

        }


        protected void GetRates(object sender, DirectEventArgs e)
        {
            List<GatewayRate> gatewayRates = new List<GatewayRate>();

            string ratesTableName = GetRatesTableName(Convert.ToInt32(FilterRatesByGateway.SelectedItem.Value));
            //Clear Store
            ManageRatesGrid.GetStore().RemoveAll();

            //Fill Store
            ManageRatesGrid.GetStore().DataSource = Rate.GetRates(ratesTableName);
            ManageRatesGrid.GetStore().DataBind();

        }
    }
}