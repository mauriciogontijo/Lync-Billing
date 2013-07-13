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

namespace Lync_Billing.ui.admin.main
{
    public partial class statistics : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;

        public string[] COLORS = new string[] { "rgb(47, 162, 223)", "rgb(60, 133, 46)", "rgb(234, 102, 17)", "rgb(154, 176, 213)", "rgb(186, 10, 25)", "rgb(40, 40, 40)" };

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

            List<GatewaysUsage> gatewaysUsage = new List<GatewaysUsage>();
            gatewaysUsage = GetGatewaysUsageChartData(2013, 1, 12);
            GatewaysDataStore.DataSource = gatewaysUsage;
            GatewaysDataStore.DataBind();
        }

        protected List<GatewaysUsage> GetGatewaysUsageChartData(int year, int fromMonth, int toMonth)
        {
            List<GatewaysUsage> tmpData = new List<GatewaysUsage>();

            tmpData.AddRange(GatewaysUsage.GetGatewaysUsage(year, fromMonth, toMonth).AsEnumerable<GatewaysUsage>());

            var gatewaysUsageData = GatewaysUsage.GetGatewaysStatisticsResults(tmpData);

            return gatewaysUsageData;
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
    }
}