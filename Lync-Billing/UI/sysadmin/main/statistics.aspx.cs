using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Globalization;
using Ext.Net;
using Lync_Billing.DB;
using Lync_Billing.Libs;

namespace Lync_Billing.ui.sysadmin.main
{
    public partial class statistics : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SystemAdmin);
       
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

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;

            List<GatewaysUsage> gatewaysUsage = new List<GatewaysUsage>();
            YearSelectorComboBox.GetStore().DataSource = GatewaysUsage.GetYears();
            YearSelectorComboBox.GetStore().DataBind();

            Bind();
        }

        protected List<GatewaysUsage> GetGatewaysUsageChartData(int year, int fromMonth, int toMonth)
        {
            List<GatewaysUsage> tmpData = new List<GatewaysUsage>();
            tmpData.AddRange(GatewaysUsage.SetGatewaysUsagePercentagesPerCallsCount(year, fromMonth, toMonth).AsEnumerable<GatewaysUsage>());
            
            return tmpData;
        }

        public List<object> RadarData
        {
            get
            {
                return new List<object> 
                { 
                    new { Name = "Cost %", Data = 100 },
                    new { Name = "Duration %", Data = 100 },
                    new { Name = "Calls %", Data = 100 }
                };
            }
        }

        protected void QuarterSelection(object sender, DirectEventArgs e)
        {
            int qaurter = Convert.ToInt32(QuarterSelectorComboBox.SelectedItem.Value);

            int year = Convert.ToInt32(YearSelectorComboBox.SelectedItem.Value);

            switch (qaurter)
            {
                case 1:
                    GatewaysDataStore.DataSource = GetGatewaysUsageChartData(year, 1, 3);
                    break;
                case 2:
                    GatewaysDataStore.DataSource = GetGatewaysUsageChartData(year, 4, 6);
                    break;
                case 3:
                    GatewaysDataStore.DataSource = GetGatewaysUsageChartData(year, 7, 9);
                    break;
                case 4:
                    GatewaysDataStore.DataSource = GetGatewaysUsageChartData(year, 10, 12);
                    break;
                case 5:
                    GatewaysDataStore.DataSource = GetGatewaysUsageChartData(year, 1, 12);
                    break;
            }
            GatewaysDataStore.DataBind();
        }

        protected void AllYearsButton_DirectClick(object sender, DirectEventArgs e)
        {

        }

        public void Bind()
        {
            if (YearSelectorComboBox.SelectedItem.Value == null && QuarterSelectorComboBox.SelectedItem.Value == null)
            {
                GatewaysDataStore.DataSource = GetGatewaysUsageChartData(DateTime.Now.Year, 1, 12);
                GatewaysDataStore.DataBind();
            }
        }
        
    }
}