using Ext.Net;
using Lync_Billing.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lync_Billing.ui.dephead.main
{
    public partial class statistics : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        private UserSession session;
        private List<Department> UserDepartments;

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/dephead/main/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                if (session.ActiveRoleName != "dephead")
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=dephead");
                }
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;

            BindDepartmentsForThisUser();
        }

        private void BindDepartmentsForThisUser()
        {
            UserDepartments = DepartmentHead.GetDepartmentsForHead(sipAccount);

            if (UserDepartments.Count == 1)
            {
                FilterDepartments.SetValueAndFireSelect(UserDepartments.First().DepartmentName);
                FilterDepartments.ReadOnly = true;
            }
            else
            {
                FilterDepartments.ReadOnly = false;
                FilterDepartments.GetStore().DataSource = UserDepartments;
                FilterDepartments.GetStore().DataBind();
            }
        }

        protected void DrawStatisticsForDepartment(object sender, DirectEventArgs e)
        {
            string departmentName = string.Empty;
            DB.Site site;
            int siteID;

            List<TopCountries> topCountries;

            if (FilterDepartments.SelectedItem != null && !string.IsNullOrEmpty(FilterDepartments.SelectedItem.Value))
            {
                departmentName = FilterDepartments.SelectedItem.Value.ToString();
                siteID = Convert.ToInt32((from dep in UserDepartments where dep.DepartmentName == departmentName select dep.SiteID).First());
                site = DB.Site.getSite(siteID);

                if (site != null && !string.IsNullOrEmpty(site.SiteName))
                {
                    topCountries = TopCountries.GetTopDestinationsForDepartment(departmentName, site.SiteName);

                    TopDestinationCountriesStore.DataSource = topCountries;
                    TopDestinationCountriesStore.DataBind();
                }

                DepartmentCallsPerMonthChartPanel.Visible = true;
                TopDestinationCountriesPanel.Visible = true;
            }
        }
    }
}