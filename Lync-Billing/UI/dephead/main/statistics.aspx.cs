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

            List<TopCountries> topCountries;

            if (FilterDepartments.SelectedItem != null && !string.IsNullOrEmpty(FilterDepartments.SelectedItem.Value))
            {
                MailStatistics departmentMailStatisticsData;

                departmentName = FilterDepartments.SelectedItem.Value.ToString();
                siteID = Convert.ToInt32((from dep in UserDepartments where dep.DepartmentName == departmentName select dep.SiteID).First());
                site = DB.Site.getSite(siteID);

                if (site != null && !string.IsNullOrEmpty(site.SiteName))
                {
                    topCountries = TopCountries.GetTopDestinationsForDepartment(departmentName, site.SiteName);

                    TopDestinationCountriesStore.DataSource = topCountries;
                    TopDestinationCountriesStore.DataBind();

                    //Write the Department Mail Statistics to the publicly-available varialbe: departmentMailStatisticsData
                    departmentMailStatisticsData = MailStatistics.GetMailStatistics(departmentName, site.SiteName, DateTime.Now);

                    Ext.Net.Panel htmlContainer = new Ext.Net.Panel
                    {
                        Header = false,
                        Border = false,
                        BodyPadding = 5,
                        Html = string.Format(
                            "<div class='p10 pt15 font-14'>" +
                            "<h2 class='mb10'>Received Emails:</h2>" + 
                            "<p class='mb5'>Total number: <span class='bold red-color'>{0}</span></p>" + 
                            "<p class='mb5'>Total emails size: <span class='bold red-color'>{1} (in MB)</span></p>" +
                            "<div class='clear h25'></div>" +
                            "<h2 class='mb10'>Sent Emails:</h2>" + 
                            "<p class='mb5'>Total number: <span class='bold blue-color'>{2}</span></p>" + 
                            "<p class='mb5'>Total emails size: <span class='bold blue-color'>{3} (in MB)</span></p>" + 
                            "</div>",
                            departmentMailStatisticsData.ReceivedCount, 
                            departmentMailStatisticsData.ReceivedSize, 
                            departmentMailStatisticsData.SentCount, 
                            departmentMailStatisticsData.SentSize
                        )
                    };

                    //DepartmentMailStatistics.Body = "";
                    htmlContainer.AddTo(this.DepartmentMailStatistics);
                }
            }
        }
    }
}