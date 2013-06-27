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
    public partial class manage : System.Web.UI.Page
    {
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
                UserSession session = new UserSession();
                session = (UserSession)Session.Contents["UserData"];

                if ((!session.IsDeveloper || !session.IsAdmin) && session.PrimarySipAccount != session.EffectiveSipAccount)
                {
                    Response.Redirect("~/ui/user/dashboard.aspx");
                }
            }

            GatewaysComboBox.GetStore().DataSource = Gateway.GetGateways().OrderBy(item => item.GatewayName );
            GatewaysComboBox.GetStore().DataBind();

            PoolComboBox.GetStore().DataSource = Pool.GetPools().OrderBy(item => item.PoolFQDN);
            PoolComboBox.GetStore().DataBind();

            SitesComboBox.GetStore().DataSource = DB.Site.GetSites().OrderBy(item => item.SiteName);
            SitesComboBox.GetStore().DataBind();
        }

        public List<Site> GetSites() 
        {
            return DB.Site.GetSites();
        }

        public List<Pool> GetPools() 
        {
            return Pool.GetPools();
        }

        public List<Gateway> GetGateways() 
        {
           return Gateway.GetGateways();
        }

        public string CreateRatesTableName(string gatewayName) 
        {
            return string.Format("Rates_{0}_{1}_{2}_{3}", gatewayName, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        protected void GetGateaysDetails(object sender, DirectEventArgs e)
        {
            GatewayRatesLable.Text = GatewayRatesLable.Text  + GatewaysComboBox.SelectedItem.Text ;

        }
    }
}