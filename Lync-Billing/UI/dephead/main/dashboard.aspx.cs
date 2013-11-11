using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Lync_Billing.DB;
using Lync_Billing.Libs;
using Lync_Billing.DB.Summaries;

namespace Lync_Billing.ui.dephead.main
{
    public partial class dashboard : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        private UserSession session;
        private List<Department> UserDepartments;
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

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;


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
            UserDepartments = DepartmentHead.GetDepartmentsForHead(sipAccount);

            //By default the filter combobox is not read only
            FilterDepartments.ReadOnly = false;

            if (UserDepartments.Count > 0)
            {
                //Handle the FireSelect event
                if (alwaysFireSelect == true)
                {
                    FilterDepartments.SetValueAndFireSelect(UserDepartments.First().DepartmentName);
                    
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
            //in case there are no longer any departments for this user to monitor.
            else
            {
                FilterDepartments.Disabled = true;
            }
        }

        protected void DrawStatisticsForDepartment(object sender, DirectEventArgs e)
        {
            string departmentName = string.Empty;
            DB.Site site;
            int siteID;

            MailStatistics departmentMailStatisticsData;

            if (FilterDepartments.SelectedItem != null && !string.IsNullOrEmpty(FilterDepartments.SelectedItem.Value))
            {
                departmentName = FilterDepartments.SelectedItem.Value.ToString();
                siteID = Convert.ToInt32((from dep in UserDepartments where dep.DepartmentName == departmentName select dep.SiteID).First());
                site = DB.Site.getSite(siteID);

                if (site != null && !string.IsNullOrEmpty(site.SiteName))
                {
                    // Get Top Countries
                    TopDestinationCountriesStore.DataSource = TopCountries.GetTopDestinationsForDepartment(site.SiteName, departmentName, 5);
                    TopDestinationCountriesStore.DataBind();

                    // Get Department Phonecalls Summaries (for all year's month)
                    DepartmentCallsPerMonthChart.GetStore().DataSource = DepartmentCallsSummary.GetPhoneCallsStatisticsForDepartment(site.SiteName, departmentName, DateTime.Now.Year);
                    DepartmentCallsPerMonthChart.GetStore().DataBind();

                    // Get Department Mail Statistics
                    // Write the Department Mail Statistics to the publicly-available varialbe: departmentMailStatisticsData
                    departmentMailStatisticsData = MailStatistics.GetMailStatistics(site.SiteName, departmentName, DateTime.Now);

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