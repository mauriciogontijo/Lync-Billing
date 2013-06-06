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
using System.Globalization;

namespace Lync_Billing.UI.user
{
    public partial class dashboard : System.Web.UI.Page
    {
        public int unmarked_calls_count = 0;
        public string sipAccount = string.Empty;

        public Dictionary<string, PhoneBook> phoneBookEntries;
        public List<TopDestinations> topDestinations;
        public List<TopCountries> topCountries;

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (Session.Contents["UserData"] == null || HttpContext.Current.Session.Count == 0)
            {
                Response.Redirect("~/UI/session/login.aspx");
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).SipAccount;

            unmarked_calls_count = getUnmarkedCallsCount();

            DurationCostChartStore.DataSource = UsersCallsSummary.GetUsersCallsSummary(sipAccount, DateTime.Now.Year, 1, 12);
            DurationCostChartStore.DataBind();

            phoneBookEntries = PhoneBook.GetAddressBook(sipAccount);
        }

        [DirectMethod]
        public static string GetSummaryData()
        {
            if (HttpContext.Current.Session.Contents["UserData"] != null)
            {
                List<AbstractComponent> components = new List<AbstractComponent>();
                UsersCallsSummary userSummary = new UsersCallsSummary();
                string SipAccount = string.Empty;
                
                SipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).SipAccount;
                userSummary = UsersCallsSummary.GetUsersCallsSummary(SipAccount, DateTime.Now.AddMonths(-3), DateTime.Now);

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
            {
                return null;
            }
        }

        public List<UsersCallsSummaryChartData> getChartData(string typeOfSummary = "")
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime fromDate = DateTime.ParseExact(DateTime.Now.Year.ToString() + "-01-01", "yyyy-mm-dd", provider);

            return UsersCallsSummaryChartData.GetUsersCallsSummary(((UserSession)Session.Contents["UserData"]).SipAccount, fromDate, DateTime.Now);
        }

        protected void DurationCostChartStore_Load(object sender, EventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
            DurationCostChartStore.DataSource = UsersCallsSummary.GetUsersCallsSummary(userSession.SipAccount, DateTime.Now.Year, 1, 12);
            DurationCostChartStore.DataBind();

        }

        protected void TopDestinationNumbersStore_Load(object sender, EventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
           
            topDestinations = TopDestinations.GetTopDestinations(userSession.SipAccount);
            List<TopDestinations> topDestinationsStore = new List<TopDestinations>();

            foreach (TopDestinations destination in topDestinations) 
            {
                if (GetUserNameBySip(destination.PhoneNumber) != string.Empty)
                {
                    destination.UserName = GetUserNameBySip(destination.PhoneNumber);
                    topDestinationsStore.Add(destination);
                    continue;
                }

                if (GetUserNameByNumber(destination.PhoneNumber) != string.Empty)
                {
                    destination.UserName = GetUserNameByNumber(destination.PhoneNumber);
                    topDestinationsStore.Add(destination);
                    continue;
                }

                destination.UserName = "NA";
                topDestinationsStore.Add(destination);
                
            }

            TopDestinationNumbersStore.DataSource = topDestinationsStore;
            TopDestinationNumbersStore.DataBind();
        }

        protected void TopDestinationCountriesStore_Load(object sender, EventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
            
            topCountries = TopCountries.GetTopDestinations(userSession.SipAccount);
            TopDestinationCountriesStore.DataSource = topCountries;
            
            TopDestinationCountriesStore.DataBind();
        }

        protected int getUnmarkedCallsCount()
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime fromDate = DateTime.ParseExact(DateTime.Now.Year.ToString() + "-01-01", "yyyy-mm-dd", provider);

            return UsersCallsSummary.GetUsersCallsSummary(((UserSession)Session.Contents["UserData"]).SipAccount, fromDate, DateTime.Now).UnmarkedCallsCount;
        }

        private string GetUserNameByNumber(string phoneNumber) 
        {
            if (phoneBookEntries.ContainsKey(phoneNumber))
                return phoneBookEntries[phoneNumber].Name;
            else
                return string.Empty;
        }
     
        private string GetUserNameBySip(string sipAccount) 
        {
            AdLib adRoutines = new AdLib();
            string DisplayName = adRoutines.getUserAttributes(sipAccount).DisplayName;
            
            if (DisplayName != null)
                return DisplayName;
            else
                return string.Empty;
        }
    }
}