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
            if (! Ext.Net.X.IsAjaxRequest)
            {
                InitUserSitesAndDepartments();

                FilterSitesComboBox.GetStore().DataSource = UserSites;
                FilterSitesComboBox.GetStore().DataBind();
            }
        }

        protected void FilterDepartmentsBySite_Selected(object sender, DirectEventArgs e)
        {
            FilterDepartments.Clear();
            FilterDepartments.Disabled = true;
            AdvancedFilterBtn.Disabled = true;

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

        protected void CustomizeStats_YearStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                //Get years from the database
                List<SpecialDateTime> Years = UserCallsSummary.GetUserCallsSummaryYears(sipAccount);

                //Add a custom year criteria
                SpecialDateTime CustomYear = SpecialDateTime.Get_OneYearAgoFromToday();

                Years.Reverse();        //i.e. 2015, 2014, 2013
                Years.Add(CustomYear);  //2015, 2014, 2013, "ONEYEARAGO..."
                Years.Reverse();        //"ONEYEARAGO...", 2013, 2014, 2015

                //Bind the data
                CustomizeStats_Years.GetStore().DataSource = Years;
                CustomizeStats_Years.GetStore().DataBind();
            }
        }

        protected void CustomizeStats_QuartersStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                var allQuarters = SpecialDateTime.GetQuartersOfTheYear();

                CustomizeStats_Quarters.GetStore().DataSource = allQuarters;
                CustomizeStats_Quarters.GetStore().DataBind();
            }
        }

        protected void CustomizeStats_Years_Select(object sender, DirectEventArgs e)
        {
            if (CustomizeStats_Years.SelectedItem.Index > -1)
            {
                int selectedValue = Convert.ToInt32(CustomizeStats_Years.SelectedItem.Value);

                if (selectedValue == Convert.ToInt32(Enums.GetValue(Enums.SpecialDateTime.OneYearAgoFromToday)))
                {
                    CustomizeStats_Quarters.Hide();
                }
                else
                {
                    CustomizeStats_Quarters.Show();
                }
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
                    //Enable the AdvancedFilter button
                    AdvancedFilterBtn.Disabled = false;

                    // Get Top Country
                    TopDestinationCountriesStore.DataSource = TopDestinationCountries.GetTopDestinationCountriesForDepartment(department.SiteName, department.DepartmentName, 5);
                    TopDestinationCountriesStore.DataBind();

                    // Get Department Phonecalls Summaries (for all year's month)
                    DepartmentCallsPerMonthChart.GetStore().DataSource = DepartmentCallsSummary.GetPhoneCallsStatisticsForDepartment(department.SiteName, department.DepartmentName); ;
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

        protected void AdvancedFilterBtn_Click(object sender, DirectEventArgs e)
        {
            CustomizeStatisticsWindow.Show();
        }


        protected void CancelCustomizeStatsBtn_Click(object sender, DirectEventArgs e)
        {
            CustomizeStatisticsWindow.Hide();
        }


        protected void SubmitCustomizeStatisticsBtn_Click(object sender, DirectEventArgs e)
        {
            //Submitted from the view
            int filterYear, filterQuater;
            
            //For DateTime handling
            DateTime startingDate, endingDate;
            string titleText = string.Empty;

            //Site and Department
            int siteID, departmentID;
            SitesDepartments department;

            if (FilterSitesComboBox.SelectedItem.Index > -1 && FilterDepartments.SelectedItem.Index > -1)
            {
                //Get site and department data
                siteID = Convert.ToInt32(FilterSitesComboBox.SelectedItem.Value);
                departmentID = Convert.ToInt32(FilterDepartments.SelectedItem.Value);
                department = SitesDepartments.GetSiteDepartment(departmentID);


                if (CustomizeStats_Years.SelectedItem.Index > -1 && CustomizeStats_Quarters.SelectedItem.Index > -1)
                {
                    filterYear = Convert.ToInt32(CustomizeStats_Years.SelectedItem.Value);
                    filterQuater = Convert.ToInt32(CustomizeStats_Quarters.SelectedItem.Value);

                    //Construct the Date Range
                    titleText = SpecialDateTime.ConstructDateRange(filterYear, filterQuater, out startingDate, out endingDate);

                    // Get Top Country
                    var countriesData = TopDestinationCountries.GetTopDestinationCountriesForDepartment(department.SiteName, department.DepartmentName, 5, startingDate, endingDate);
                    if (countriesData.Count > 0)
                        TopDestinationCountriesChart.GetStore().LoadData(countriesData);
                    else
                        TopDestinationCountriesChart.GetStore().RemoveAll();
                    TopDestinationCountriesPanel.Title = String.Format("Most Called Countries - {0}", titleText);

                    // Get Department Phonecalls Summaries (for all year's month)
                    DepartmentCallsPerMonthChart.GetStore().DataSource = DepartmentCallsSummary.GetPhoneCallsStatisticsForDepartment(department.SiteName, department.DepartmentName, startingDate, endingDate);
                    DepartmentCallsPerMonthChart.GetStore().DataBind();
                    DepartmentCallsPerMonthChartPanel.Title = String.Format("Phonecalls Distribution - {0}", titleText);

                    //Hide the window
                    CustomizeStatisticsWindow.Hide();

                }//End-inner-if

            }//End-outer-if

        }//End-Function

    }

}