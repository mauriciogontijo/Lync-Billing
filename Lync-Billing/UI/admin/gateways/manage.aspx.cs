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

        List<GatewayRate> gatewayRates = new List<GatewayRate>();
        List<GatewayDetail> gatewayDetails = new List<GatewayDetail>();

        List<Site> sites = new List<Site>();
        List<Pool> pools = new List<Pool>();


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

            GatewaysComboBox.GetStore().DataSource = Gateway.GetGateways().OrderBy(item => item.GatewayName);
            GatewaysComboBox.GetStore().DataBind();

            pools = Pool.GetPools().OrderBy(item => item.PoolFQDN).ToList();
            PoolComboBox.GetStore().DataSource = pools;
            PoolComboBox.GetStore().DataBind();

            sites = DB.Site.GetSites().OrderBy(item => item.SiteName).ToList();
            SitesComboBox.GetStore().DataSource = sites;
            SitesComboBox.GetStore().DataBind();

            if (GatewaysComboBox.SelectedItem.Index != -1)
            {
                gatewayDetails = GatewayDetail.GetGatewaysDetails(Convert.ToInt32(GatewaysComboBox.SelectedItem.Value));
                gatewayRates = GatewayRate.GetGatewaysRates(Convert.ToInt32(GatewaysComboBox.SelectedItem.Value));

            }
        }

        public string CreateRatesTableName(string gatewayName)
        {
            return string.Format("Rates_{0}_{1}_{2}_{3}", gatewayName, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        protected void GetDetails(object sender, DirectEventArgs e)
        {
            GatewayRatesLable.Text = GatewayRatesLable.Text + GatewaysComboBox.SelectedItem.Text;

            gatewayRates = GatewayRate.GetGatewaysRates(Convert.ToInt32(GatewaysComboBox.SelectedItem.Value));

            gatewayDetails = GatewayDetail.GetGatewaysDetails(Convert.ToInt32(GatewaysComboBox.SelectedItem.Value));

            ClearFields();

            if (gatewayRates.Count == 1)
            {
                if (gatewayRates[0].StartingDate != null)
                    StartingDate.SelectedDate = gatewayRates[0].StartingDate;

                if (gatewayRates[0].EndingDate != null)
                    EndingDate.SelectedDate = gatewayRates[0].EndingDate;

                if (gatewayRates[0].ProviderName != null)
                    ProviderName.Text = gatewayRates[0].ProviderName;

                if (gatewayRates[0].CurrencyCode != null)
                    CurrencyCode.Text = gatewayRates[0].CurrencyCode;
            }


            if (gatewayDetails.Count == 1)
            {
                if (gatewayDetails[0].SiteID != 0)
                {
                    Site selectedSite = new DB.Site();
                    selectedSite = sites.Single(item => item.SiteID == gatewayDetails[0].SiteID);
                    SitesComboBox.Select(selectedSite.SiteName);
                }

                if (gatewayDetails[0].PoolID != 0)
                    PoolComboBox.Select(pools.Single(item => item.PoolID == gatewayDetails[0].PoolID).PoolFQDN);

                if (gatewayDetails[0].Description != null)
                    GatewayDescription.Text = gatewayDetails[0].Description;
            }
        }

        public void ClearFields()
        {
            StartingDate.SelectedDate = DateTime.MinValue;
            EndingDate.SelectedDate = DateTime.MinValue;
            ProviderName.Text = string.Empty;
            CurrencyCode.Text = string.Empty;
            SitesComboBox.Select(-1);
            PoolComboBox.Select(-1);
            GatewayDescription.Text = string.Empty;
        }

        protected void SaveGatewayButton_DirectClick(object sender, DirectEventArgs e)
        {
            string RatesTableName = string.Format("Rates_{0}_{1}_{2}_{3}", GatewaysComboBox.SelectedItem.Text, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            if (SitesComboBox.SelectedItem != null && PoolComboBox.SelectedItem != null && GatewayDescription.Text != string.Empty)
            {
                GatewayDetail gatewayDetail = new GatewayDetail();

                if (gatewayDetails.Count == 0)
                {
                    gatewayDetail.GatewayID = Convert.ToInt32(GatewaysComboBox.SelectedItem.Value);
                    gatewayDetail.SiteID = Convert.ToInt32(SitesComboBox.SelectedItem.Value);
                    gatewayDetail.PoolID = Convert.ToInt32(PoolComboBox.SelectedItem.Value);
                    gatewayDetail.Description = GatewayDescription.Text;

                    int GatewayDetailsID = GatewayDetail.InsertGatewayDetails(gatewayDetail);
                    gatewayDetails.Add(gatewayDetail);
                }
                else
                {
                    if (gatewayDetails[0].SiteID != Convert.ToInt32(SitesComboBox.SelectedItem.Value) ||
                        gatewayDetails[0].PoolID != Convert.ToInt32(PoolComboBox.SelectedItem.Value) ||
                        gatewayDetails[0].Description != GatewayDescription.Text)
                    {
                        gatewayDetail.GatewayID = Convert.ToInt32(GatewaysComboBox.SelectedItem.Value);
                        gatewayDetail.SiteID = Convert.ToInt32(SitesComboBox.SelectedItem.Value);
                        gatewayDetail.PoolID = Convert.ToInt32(PoolComboBox.SelectedItem.Value);
                        gatewayDetail.Description = GatewayDescription.Text;

                        GatewayDetail.UpdateGatewayDetails(gatewayDetail);
                    }
                }
            }

            //if (StartingDate.SelectedValue != null && ProviderName.Text != null && CurrencyCode.Text != null)
            //{
            //    GatewayRate gatewayRate = new GatewayRate();

            //    if (gatewayRates.Count == 0)
            //    {
            //        gatewayRate.GatewayID = Convert.ToInt32(GatewaysComboBox.SelectedItem.Value);
            //        gatewayRate.CurrencyCode = CurrencyCode.Text;
            //        gatewayRate.ProviderName = ProviderName.Text;
            //        gatewayRate.StartingDate = StartingDate.SelectedDate;
            //        gatewayRate.RatesTableName = RatesTableName;

            //        int GatewaysRatesID = GatewayRate.InsertGatewayRate(gatewayRate);
            //        gatewayRates.Add(gatewayRate);
            //    }
            //    else if (gatewayRates.Count == 1)
            //    {
            //        GatewayRate PreviousGatewayRate = new GatewayRate();

            //        if (EndingDate.SelectedValue != null && gatewayRates[0].EndingDate != null)
            //        {
            //            PreviousGatewayRate.GatewayID = Convert.ToInt32(gatewayRates[0].GatewaysRatesID);
            //            PreviousGatewayRate.EndingDate = EndingDate.SelectedDate;

            //            GatewayRate.UpdateGatewayRate(PreviousGatewayRate);
            //        }

            //        gatewayRate.CurrencyCode = CurrencyCode.Text;
            //        gatewayRate.ProviderName = ProviderName.Text;
            //        gatewayRate.StartingDate = StartingDate.SelectedDate;
            //        gatewayRate.RatesTableName = RatesTableName;

            //        int GatewaysRatesID = GatewayRate.InsertGatewayRate(gatewayRate);
            //    }
            //    else 
            //    {
            //        //TODO : Get the Latest Record without ending Date and Update the ending Date
            //    }
            //}
        }
    }
}