using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Lync_Billing.Backend;
using Lync_Billing.Libs;
using Lync_Billing.Backend.Roles;
using Lync_Billing.Backend.Summaries;
using Lync_Billing.Backend.Statistics;


namespace Lync_Billing.ui.dephead.main
{
    public partial class dashboard : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.DepartmentHead);

        private List<Site> UserSites;
        private List<SitesDepartments> UserSitesDepartments;


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

                if (session.ActiveRoleName != allowedRoleName)
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=dephead");
                }
            }

            sipAccount = session.NormalUserInfo.SipAccount;

            //Set the year number in the charts header's title
            string currentYear = DateTime.Now.Year.ToString();
            TopDestinationCountriesPanel.Title = "Most Called Country in " + currentYear;
            DepartmentCallsPerMonthChartPanel.Title = "Phonecalls Distribution for " + currentYear;
        }


        private void InitUserSitesAndDepartments()
        {
            //Get UserSites
            if (UserSites == null)
            {
                if (session.IsDeveloper)
                    UserSites = SitesDepartments.AllSites;
                else
                    UserSites = Backend.Site.GetUserRoleSites(session.SystemRoles, allowedRoleName);
            }

            //Get UserSitesDepartments
            if (UserSitesDepartments == null)
            {
                if(session.IsDeveloper)
                    UserSitesDepartments = SitesDepartments.GetSitesDepartments();
                else
                    UserSitesDepartments = DepartmentHeadRole.GetSiteDepartmentsForHead(sipAccount);
            }
        }


        protected void FilterSitesComboBoxStore_Load(object sender, EventArgs e)
        {
            InitUserSitesAndDepartments();

            FilterSitesComboBox.GetStore().DataSource = UserSites;
            FilterSitesComboBox.GetStore().DataBind();
        }


        protected void FilterDepartmentsBySite_Selected(object sender, DirectEventArgs e)
        {
            FilterDepartments.Clear();
            FilterDepartments.Disabled = true;

            if (FilterSitesComboBox.SelectedItem.Index > -1)
            {
                InitUserSitesAndDepartments();

                FilterDepartments.Disabled = false;

                int siteID = Convert.ToInt32(FilterSitesComboBox.SelectedItem.Value);
                var siteDepartments = UserSitesDepartments.Where(department => department.SiteID == siteID).ToList<SitesDepartments>();

                FilterDepartments.GetStore().DataSource = siteDepartments;
                FilterDepartments.GetStore().DataBind();
            }
        }


        protected void DrawStatisticsForDepartment(object sender, DirectEventArgs e)
        {
            int departmentID;
            SitesDepartments department;
            
            MailStatistics departmentMailStatisticsData;

            if (FilterDepartments.SelectedItem != null && !string.IsNullOrEmpty(FilterDepartments.SelectedItem.Value))
            {
                departmentID = Convert.ToInt32(FilterDepartments.SelectedItem.Value);
                department = SitesDepartments.GetSiteDepartment(departmentID);

                //siteID = Convert.ToInt32((from dep in UserDepartments where dep.DepartmentName == departmentName select dep.SiteID).First());
                //site = Backend.Site.GetSite(siteID);

                if (department != null && !string.IsNullOrEmpty(department.SiteName))
                {
                    // Get Top Country
                    TopDestinationCountriesStore.DataSource = TopDestinationCountries.GetTopDestinationCountriesForDepartment(department.SiteName, department.DepartmentName, 5);
                    TopDestinationCountriesStore.DataBind();

                    // Get Department Phonecalls Summaries (for all year's month)
                    DepartmentCallsPerMonthChart.GetStore().DataSource = DepartmentCallsSummary.GetPhoneCallsStatisticsForDepartment(department.SiteName, department.DepartmentName, DateTime.Now.Year);
                    DepartmentCallsPerMonthChart.GetStore().DataBind();

                    // Get Department Mail Statistics
                    // Write the Department Mail Statistics to the publicly-available varialbe: departmentMailStatisticsData
                    departmentMailStatisticsData = MailStatistics.GetMailStatistics(department.SiteName, department.DepartmentName, DateTime.Now);

                    Ext.Net.Panel htmlContainer = new Ext.Net.Panel
                    {
                        Header = false,
                        Border = false,
                        BodyPadding = 5,
                        Html = string.Format(
                            "<div class='p10 pt15 font-14'>" +
                            "<h2 class='mb10'>Received:</h2>" + 
                            "<p class='mb5'>Total number: <span class='bold red-color'>{0}</span></p>" + 
                            "<p class='mb5'>Total size: <span class='bold red-color'>{1} (in MB)</span></p>" +
                            "<div class='clear h25'></div>" +
                            "<h2 class='mb10'>Sent:</h2>" + 
                            "<p class='mb5'>Total number: <span class='bold blue-color'>{2}</span></p>" + 
                            "<p class='mb5'>Total size: <span class='bold blue-color'>{3} (in MB)</span></p>" + 
                            "</div>",
                            departmentMailStatisticsData.ReceivedCount, 
                            departmentMailStatisticsData.ReceivedSize, 
                            departmentMailStatisticsData.SentCount, 
                            departmentMailStatisticsData.SentSize
                        )
                    };

                    DepartmentMailStatistics.RemoveAll();
                    htmlContainer.AddTo(this.DepartmentMailStatistics);
                }
            }
        }

    }

}