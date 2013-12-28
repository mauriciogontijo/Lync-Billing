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
        private List<SitesDepartments> UserDepartments;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.DepartmentHead);

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

            /***
             * Thie following solves the issue of infinitely looping like: Page_Load--->BindDepartmentsForThisUser--->DrawStatisticsForDepartment
             * This would happen due to a chain-reaction of triggering two DirectEvents from the aspx page
             * The chain consist of two DirectEvents: OnDepartmentSelect X---FIRE---> DrawStatisticsForDepartment which fires Page_Load and then it goes again for ever.
             * */
            if (!Ext.Net.X.IsAjaxRequest)
            {
                BindDepartmentsForThisUser(true);
            }
            else
            {
                BindDepartmentsForThisUser(false);
            }
        }

        private void BindDepartmentsForThisUser(bool alwaysFireSelect = false)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.NormalUserInfo.SipAccount;

            //If the current user is a system-developer, give him access to all the AllDepartments, otherwise grant him access to his/her own managed department
            if (UserDepartments == null || UserDepartments.Count == 0)
            {
                if (session.IsDeveloper)
                    UserDepartments = SitesDepartments.GetAllSitesDepartments();
                else
                    UserDepartments = DepartmentHeadRole.GetSiteDepartmentsForHead(sipAccount);
            }


            //By default the filter combobox is not read only
            FilterDepartments.ReadOnly = false;

            if (UserDepartments.Count > 0)
            {
                //Handle the FireSelect event
                if (alwaysFireSelect == true)
                {
                    FilterDepartments.SetValueAndFireSelect(UserDepartments.First().DepartmentID);
                    
                    //Handle the ReadOnly Property
                    if (UserDepartments.Count == 1)
                    {
                        FilterDepartments.ReadOnly = true;
                    }
                }

                //Bind all the Data and return to the view
                FilterDepartments.GetStore().DataSource = UserDepartments;
                FilterDepartments.GetStore().DataBind();
            }
            //in case there are no longer any AllDepartments for this user to monitor.
            else
            {
                FilterDepartments.Disabled = true;
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