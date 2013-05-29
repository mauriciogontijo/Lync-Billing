using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Lync_Billing.DB;
using Ext.Net;
using System.Web.SessionState;
using Lync_Billing.Libs;

namespace Lync_Billing.UI.user
{
    public partial class dashboard : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (Session.Contents["UserData"] == null || HttpContext.Current.Session.Count == 0)
            {
                Response.Redirect("~/UI/login.aspx");
            }

            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            List<string> columns = new List<string>();

            wherePart.Add("SourceUserUri", SipAccount);
            wherePart.Add("marker_CallTypeID", 1);

            columns.Add("SessionIdTime");
            columns.Add("DestinationNumberUri");
            columns.Add("Duration");
            columns.Add("marker_CallToCountry");

            PhoneCallsHistoryStore.DataSource = PhoneCall.GetPhoneCalls(columns, wherePart, 5);
            PhoneCallsHistoryStore.DataBind();


            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
            DurationCostChartStore.DataSource = UsersCallsSummary.GetUsersCallsSummary(userSession.SipAccount, DateTime.Now.Year, 1, 12);
            DurationCostChartStore.DataBind();

            TOPDestinationNumbersStore.DataSource = TopDestinations.GetTopDestinations(SipAccount);
            TOPDestinationNumbersStore.DataBind();
        }

        [DirectMethod]
        public static string GetSummaryData()
        {
            if (HttpContext.Current.Session.Contents["UserData"] != null)
            {
                string SipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).SipAccount;
                
                UsersCallsSummary userSummary = new UsersCallsSummary();
                
                userSummary = UsersCallsSummary.GetUsersCallsSummary(SipAccount, DateTime.Now.AddMonths(-3), DateTime.Now);

                List<AbstractComponent> components = new List<AbstractComponent>();

                Ext.Net.Panel personalPanel = new Ext.Net.Panel()
                {
                    Title = "Personal",
                    Icon = Icon.Phone,
                    Html = String.Format(
                        "<div class='block-body wauto m15 p5'><p>" +
                        "<p class='line-height-1-7 mb15'>In the <span class='red-font'>last 3 months</span>, you have made a total of <span class='red-font'>{0} phone calls</span>, and they all add up to a total duration of almost <span class='red-font'>{1} minutes</span>.</p>" +
                        "<p class='line-height-1-7 mb10'>The net calculated <span class='red-font'>cost is {2} euros</span>.</p></div>",
                        userSummary.PersonalCallsCount, userSummary.PersonalCallsDuration / 60, userSummary.PersonalCallsCost)
                };

                Ext.Net.Panel businessPanel = new Ext.Net.Panel()
                {
                    Title = "Business",
                    Icon = Icon.Phone,
                    Html = String.Format(
                        "<div class='block-body wauto m15 p5'><p>" +
                        "<p class='line-height-1-7 mb15'>In the <span class='red-font'>last 3 months</span>, you have made a total of <span class='red-font'>{0} phone calls</span>, and they all add up to a total duration of almost <span class='red-font'>{1} minutes</span>.</p>" +
                        "<p class='line-height-1-7 mb10'>The net calculated <span class='red-font'>cost is {2} euros</span>.</p></div>",
                        userSummary.BusinessCallsCount, userSummary.BusinessCallsDuration / 60, userSummary.BusinessCallsCost)
                };

                Ext.Net.Panel unmarkedPanel = new Ext.Net.Panel()
                {

                    Title = "Unmarked",
                    Icon = Icon.Phone,
                    Html = String.Format(
                        "<div class='block-body wauto m15 p5'><p>" +
                        "<p class='line-height-1-7 mb15'>In the <span class='red-font'>last 3 months</span>, you have made a total of <span class='red-font'>{0} phone calls</span>, and they all add up to a total duration of almost <span class='red-font'>{1} minutes</span>.</p>" +
                        "<p class='line-height-1-7 mb10'>The net calculated <span class='red-font'>cost is {2} euros</span>.</p></div>",
                        userSummary.UnmarkedCallsCount, userSummary.UnmarkedCallsDuartion / 60, userSummary.UnmarkedCallsCost)
                };


                components.Add(personalPanel);
                components.Add(businessPanel);
                components.Add(unmarkedPanel);

                return ComponentLoader.ToConfig(components);
            }
            else
                return null;
        }

        public List<UsersCallsSummaryChartData> getChartData() 
        {
            string SipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;
            return UsersCallsSummaryChartData.GetUsersCallsSummary(((UserSession)Session.Contents["UserData"]).SipAccount, DateTime.Now.AddMonths(-3), DateTime.Now);
            
        }

        protected void PhoneCallsCostChartStore_Load(object sender, EventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
                
            //PhoneCallsCostChartStore.DataSource = getChartData();
            //PhoneCallsCostChartStore.DataBind();
        }

        protected void PhoneCallsDuartionChartStore_Load(object sender, EventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            PhoneCallsDuartionChartStore.DataSource = getChartData();
            PhoneCallsDuartionChartStore.DataBind();
        }

        protected void DurationCostChartStore_Load(object sender, EventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
            DurationCostChartStore.DataSource = UsersCallsSummary.GetUsersCallsSummary(userSession.SipAccount, DateTime.Now.Year, 1, 12);
            DurationCostChartStore.DataBind();

        }
    }
}