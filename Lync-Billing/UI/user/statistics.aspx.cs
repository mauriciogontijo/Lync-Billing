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
using Lync_Billing.Backend;
using Lync_Billing.Backend.Summaries;
using Lync_Billing.Libs;

namespace Lync_Billing.ui.user
{
    public partial class statistics : System.Web.UI.Page
    {
        //This is used for the statistics filter
        private const string STATISTICS_FILTER_CUSTOM_YEAR_NAME = "One Year Ago from Today";

        UserSession session;
        private string sipAccount = string.Empty;
        private string normalUserRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        private string userDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee);

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/user/statistics.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
                if (session.ActiveRoleName != normalUserRoleName && session.ActiveRoleName != userDelegeeRoleName)
                {
                    string url = @"~/ui/session/authenticate.aspx?access=" + session.ActiveRoleName;
                    Response.Redirect(url);
                }
            }

            sipAccount = session.GetEffectiveSipAccount();
        }

        private List<UsersCallsSummaryChartData> getChartData()
        {
            DateTime fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
            DateTime toDate = DateTime.Now;

            return UsersCallsSummaryChartData.GetUsersCallsSummary(sipAccount, fromDate, toDate);
        }

        protected void PhoneCallsDuartionChartStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                PhoneCallsDuartionChartStore.DataSource = getChartData();
                PhoneCallsDuartionChartStore.DataBind();
            }
        }

        protected void PhoneCallsCostChartStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                PhoneCallsCostChartStore.DataSource = getChartData();
                PhoneCallsCostChartStore.DataBind();
            }
        }

        protected void DurationCostChartStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                DateTime fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                DateTime toDate = DateTime.Now;

                DurationCostChartStore.DataSource = UserCallsSummary.GetUsersCallsSummary(sipAccount, fromDate, toDate);
                DurationCostChartStore.DataBind();
            }
        }


        protected void CustomizeStats_YearStore_Load(object sender, EventArgs e)
        {
            //Get years from the database
            List<UserCallsSummaryYears> Years = UserCallsSummary.GetUserCallsSummaryYears(sipAccount);

            //Add a custom year criteria
            UserCallsSummaryYears CustomYear = new UserCallsSummaryYears() { YearName = STATISTICS_FILTER_CUSTOM_YEAR_NAME };
            Years.Add(CustomYear);

            //Bind the data
            CustomizeStats_Years.GetStore().DataSource = Years;
            CustomizeStats_Years.GetStore().DataBind();
        }


        protected void CustomizeStats_Years_Select(object sender, DirectEventArgs e)
        {
            if (CustomizeStats_Years.SelectedItem.Index > -1)
            {
                string selectedValue = Convert.ToString(CustomizeStats_Years.SelectedItem.Value);

                if (selectedValue == STATISTICS_FILTER_CUSTOM_YEAR_NAME)
                {
                    CustomizeStats_Quarters.Disabled = true;
                }
                else
                {
                    CustomizeStats_Quarters.Disabled = false;
                }
            }
        }


        protected void SubmitCustomizeStatisticsBtn_Click(object sender, DirectEventArgs e)
        {
            //Submitted from the view
            string filterYear = string.Empty;
            int filterQuater;

            //For DateTime handling
            int fromMonth, toMonth;
            DateTime startingDate, endingDate;

            string yearTitleString = string.Empty;
            string quarterTitleString = string.Empty;

            if (CustomizeStats_Years.SelectedItem.Index > -1 && CustomizeStats_Quarters.SelectedItem.Index > -1)
            {
                filterYear = Convert.ToString(CustomizeStats_Years.SelectedItem.Value);
                filterQuater = Convert.ToInt32(CustomizeStats_Quarters.SelectedItem.Value);

                //Handle the year
                if (filterYear == STATISTICS_FILTER_CUSTOM_YEAR_NAME)
                {
                    startingDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                    endingDate = DateTime.Now;

                    yearTitleString = STATISTICS_FILTER_CUSTOM_YEAR_NAME;
                }
                //Double check the year format - i.e. 2012, 2013, 2014
                else if (filterYear.Length == 4)
                {
                    //Handle the fromMonth and toMonth
                    switch (filterQuater)
                    {
                        case 1:
                            fromMonth = 1;
                            toMonth = 3;
                            quarterTitleString = "(1st Quarter)";
                            break;

                        case 2:
                            fromMonth = 4;
                            toMonth = 6;
                            quarterTitleString = "(2nd Quarter)";
                            break;

                        case 3:
                            fromMonth = 7;
                            toMonth = 9;
                            quarterTitleString = "(3rd Quarter)";
                            break;

                        case 4:
                            fromMonth = 10;
                            toMonth = 12;
                            quarterTitleString = "(4th Quarter)";
                            break;

                        case 5:
                            fromMonth = 1;
                            toMonth = 12;
                            quarterTitleString = "(All Quarters)";
                            break;

                        default:
                            fromMonth = 1;
                            toMonth = 12;
                            quarterTitleString = "(All Quarters)";
                            break;
                    }

                    startingDate = new DateTime(Convert.ToInt32(filterYear), fromMonth, 1);
                    endingDate = new DateTime(Convert.ToInt32(filterYear), toMonth, 1);

                    yearTitleString = filterYear.ToString();
                }
                //Fail safe - one year from now
                else
                {
                    startingDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                    endingDate = DateTime.Now;

                    yearTitleString = "One Year Ago from Today";
                }


                //Re-bind DurationCostChart to match the filter dates criteria
                DurationCostChart.GetStore().LoadData(UserCallsSummary.GetUsersCallsSummary(sipAccount, startingDate, endingDate));
                DurationCostChartPanel.Title = String.Format("Business/Personal Calls - {0} {1}", yearTitleString, quarterTitleString);

                //Re-bind PhoneCallsDuartionChart to match the filter dates criteria
                PhoneCallsDuartionChart.GetStore().LoadData(
                    UsersCallsSummaryChartData.GetUsersCallsSummary(sipAccount, startingDate, endingDate)
                );
                PhoneCallsDuartionChartPanel.Title = String.Format("Calls Duration - {0} {1}", yearTitleString, quarterTitleString); ;

                //Re-bind PhoneCallsCostChart to match the filter dates criteria
                PhoneCallsCostChart.GetStore().LoadData(getChartData());
                PhoneCallsCostChartPanel.Title = String.Format("Calls Costs - {0} {1}", yearTitleString, quarterTitleString);

            }//End-if

        }//End-Function
    }
}