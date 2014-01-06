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


        protected void CustomizeStats_QuartersStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                CustomizeStats_Quarters.GetStore().DataSource = SpecialDateTime.GetQuartersOfTheYear();
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
            int filterYear, filterQuater;

            //For DateTime handling
            DateTime startingDate, endingDate;
            string titleText = string.Empty;


            if (CustomizeStats_Years.SelectedItem.Index > -1 && CustomizeStats_Quarters.SelectedItem.Index > -1)
            {
                filterYear = Convert.ToInt32(CustomizeStats_Years.SelectedItem.Value);
                filterQuater = Convert.ToInt32(CustomizeStats_Quarters.SelectedItem.Value);


                //Construct the Date Range
                titleText = SpecialDateTime.ConstructDateRange(filterYear, filterQuater, out startingDate, out endingDate);


                //Re-bind DurationCostChart to match the filter dates criteria
                DurationCostChart.GetStore().LoadData(UserCallsSummary.GetUsersCallsSummary(sipAccount, startingDate, endingDate));
                DurationCostChartPanel.Title = String.Format("Business/Personal Calls - {0}", titleText);

                //Re-bind PhoneCallsDuartionChart to match the filter dates criteria
                PhoneCallsDuartionChart.GetStore().LoadData(UsersCallsSummaryChartData.GetUsersCallsSummary(sipAccount, startingDate, endingDate));
                PhoneCallsDuartionChartPanel.Title = String.Format("Calls Duration - {0}", titleText);

                //Re-bind PhoneCallsCostChart to match the filter dates criteria
                PhoneCallsCostChart.GetStore().LoadData(getChartData());
                PhoneCallsCostChartPanel.Title = String.Format("Calls Costs - {0}", titleText);

            }//End-if

        }//End-Function
    }
}