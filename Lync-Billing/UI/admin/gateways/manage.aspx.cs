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
        List<Currencies> currencies = new List<Currencies>();

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

            currencies = Currencies.GetCurrencies();
            CurrenciesCodesCombobox.GetStore().DataSource = currencies;
            CurrenciesCodesCombobox.GetStore().DataBind();

            if (GatewaysComboBox.SelectedItem.Index != -1)
            {
                gatewayDetails = GatewayDetail.GetGatewaysDetails(Convert.ToInt32(GatewaysComboBox.SelectedItem.Value));
                gatewayRates = GatewayRate.GetGatewaysRates(Convert.ToInt32(GatewaysComboBox.SelectedItem.Value));

                if (gatewayRates.Count > 0)
                    StartingDate.ReadOnly = true;
                else
                    StartingDate.ReadOnly = false;

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

            if (gatewayRates.Count >= 1)
            {
                GatewayRate tmpGatewayRate = gatewayRates.Single(item => item.EndingDate == null || item.EndingDate == DateTime.MinValue);

                if (gatewayRates[0].StartingDate != null)
                    StartingDate.SelectedDate = tmpGatewayRate.StartingDate;

                if (gatewayRates[0].EndingDate != null)
                    EndingDate.SelectedDate = tmpGatewayRate.EndingDate;

                if (gatewayRates[0].ProviderName != null)
                    ProviderName.Text = tmpGatewayRate.ProviderName;

                if (gatewayRates[0].CurrencyCode != null)
                {
                    Currencies selectedCurrency = currencies.First(item => item.CurrencyISOName == tmpGatewayRate.CurrencyCode);
                    CurrenciesCodesCombobox.SetValue(selectedCurrency.CurrencyISOName);
                }
            }


            if (gatewayDetails.Count == 1)
            {
                if (gatewayDetails[0].SiteID != 0)
                {
                    Site selectedSite = new DB.Site();
                    selectedSite = sites.Single(item => item.SiteID == gatewayDetails[0].SiteID);
                    SitesComboBox.SetValue(selectedSite.SiteID);
                }

                if (gatewayDetails[0].PoolID != 0)
                {
                    Pool selectedPool = pools.Single(item => item.PoolID == gatewayDetails[0].PoolID);
                    PoolComboBox.SetValue(selectedPool.PoolID);
                }

                if (gatewayDetails[0].Description != null)
                    GatewayDescription.Text = gatewayDetails[0].Description;
            }
        }

        public void ClearFields()
        {
            StartingDate.SelectedDate = DateTime.MinValue;
            EndingDate.SelectedDate = DateTime.MinValue;
            ProviderName.Text = string.Empty;
            CurrenciesCodesCombobox.Select(-1);
            SitesComboBox.Select(-1);
            PoolComboBox.Select(-1);
            GatewayDescription.Text = string.Empty;
        }

        protected void SaveGatewayButton_DirectClick(object sender, DirectEventArgs e)
        {
            string RatesTableName = string.Format("Rates_{0}_{1}_{2}_{3}", GatewaysComboBox.SelectedItem.Text, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            ///
            ///GatewaysDetails Block
            ///
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


            ///
            /// GatewaysRates Block
            /// 
            if (StartingDate.SelectedValue != null && ProviderName.Text != null && CurrenciesCodesCombobox.SelectedItem != null)
            {
                GatewayRate gatewayRate = new GatewayRate();

                if (gatewayRates.Count == 0)
                {
                    gatewayRate.GatewayID = Convert.ToInt32(GatewaysComboBox.SelectedItem.Value);
                    gatewayRate.CurrencyCode = CurrenciesCodesCombobox.SelectedItem.Value;
                    gatewayRate.ProviderName = ProviderName.Text;
                    gatewayRate.StartingDate = StartingDate.SelectedDate;
                    gatewayRate.RatesTableName = RatesTableName;

                    int GatewaysRatesID = GatewayRate.InsertGatewayRate(gatewayRate);
                }
                else 
                {
                    GatewayRate tmpGatewayRate = gatewayRates.Single(item => item.EndingDate == null || item.EndingDate == DateTime.MinValue);

                    if (gatewayRates[0].ProviderName != ProviderName.Text ||
                        gatewayRates[0].CurrencyCode != CurrenciesCodesCombobox.SelectedItem.Value) 
                    {
                        gatewayRate.CurrencyCode = CurrenciesCodesCombobox.SelectedItem.Value;
                        gatewayRate.ProviderName = ProviderName.Text;

                        GatewayRate.UpdateGatewayRate(gatewayRate);
                    }
                    else
                    {
                        tmpGatewayRate.EndingDate = DateTime.Now;
                        GatewayRate.UpdateGatewayRate(tmpGatewayRate);

                        gatewayRate.CurrencyCode = CurrenciesCodesCombobox.SelectedItem.Value;
                        gatewayRate.ProviderName = ProviderName.Text;
                        gatewayRate.StartingDate = StartingDate.SelectedDate;
                        gatewayRate.RatesTableName = RatesTableName;

                        int GatewaysRatesID = GatewayRate.InsertGatewayRate(gatewayRate);
                    }
                }
            }
        }
    }
}