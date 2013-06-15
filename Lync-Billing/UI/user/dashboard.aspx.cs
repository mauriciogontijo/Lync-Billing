using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Globalization;
using Lync_Billing.DB;
using Ext.Net;
using Lync_Billing.Libs;


namespace Lync_Billing.UI.user
{
    public partial class dashboard : System.Web.UI.Page
    {
        public int unmarked_calls_count = 0;
        public string sipAccount = string.Empty;

        public Dictionary<string, PhoneBook> phoneBookEntries;
        public List<TopDestinations> topDestinations;
        public List<TopCountries> topCountries;

        public Dictionary<string, object> wherePart = new Dictionary<string, object>();
        public List<string> columns = new List<string>();
        public List<PhoneCall> phoneCalls;

        protected void Page_Load(object sender, EventArgs e)
        {
           //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/UI/user/dashboard.aspx";
                string url = @"~/UI/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).SipAccount;

            unmarked_calls_count = getUnmarkedCallsCount();

            DurationCostChartStore.DataSource = UsersCallsSummary.GetUsersCallsSummary(sipAccount, DateTime.Now.Year, 1, 12);
            DurationCostChartStore.DataBind();

            phoneBookEntries = PhoneBook.GetAddressBook(sipAccount);

            Misc.Message("Welcome","Welcome " + ((UserSession)HttpContext.Current.Session.Contents["UserData"]).DisplayName,"info");
            
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
                    Title = "Personal Calls",
                    Icon = Icon.Phone,
                    Html = String.Format(
                        "<div class='block-body wauto m15 p5'><p>" +
                        "<p class='line-height-1-7 mb15'>In the <span class='red-font'>last 3 months</span>, you have made a total of <span class='red-font'>{0} phone calls</span>, and they all add up to a total duration of almost <span class='red-font'>{1} minutes</span>.</p>" +
                        "<p class='line-height-1-7 mb10'>The net calculated <span class='red-font'>cost is {2} euros</span>.</p></div>",
                        userSummary.PersonalCallsCount, userSummary.PersonalCallsDuration / 60, userSummary.PersonalCallsCost)
                };

                Ext.Net.Panel businessPanel = new Ext.Net.Panel()
                {
                    Title = "Business Calls",
                    Icon = Icon.Phone,
                    Html = String.Format(
                        "<div class='block-body wauto m15 p5'><p>" +
                        "<p class='line-height-1-7 mb15'>In the <span class='red-font'>last 3 months</span>, you have made a total of <span class='red-font'>{0} phone calls</span>, and they all add up to a total duration of almost <span class='red-font'>{1} minutes</span>.</p>" +
                        "<p class='line-height-1-7 mb10'>The net calculated <span class='red-font'>cost is {2} euros</span>.</p></div>",
                        userSummary.BusinessCallsCount, userSummary.BusinessCallsDuration / 60, userSummary.BusinessCallsCost)
                };

                Ext.Net.Panel unmarkedPanel = new Ext.Net.Panel()
                {

                    Title = "Unmarked Calls",
                    Icon = Icon.Phone,
                    Html = String.Format(
                        "<div class='block-body wauto m15 p5'><p>" +
                        "<p class='line-height-1-7 mb15'>In the <span class='red-font'>last 3 months</span>, you have made a total of <span class='red-font'>{0} phone calls</span>, and they all add up to a total duration of almost <span class='red-font'>{1} minutes</span>.</p>" +
                        "<p class='line-height-1-7 mb10'>The net calculated <span class='red-font'>cost is {2} euros</span>.</p></div>",
                        userSummary.UnmarkedCallsCount, userSummary.UnmarkedCallsDuartion / 60, userSummary.UnmarkedCallsCost)
                };

                components.Add(unmarkedPanel);
                components.Add(personalPanel);
                components.Add(businessPanel);
                                  

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
            List<UsersCallsSummaryChartData> chartData = UsersCallsSummaryChartData.GetUsersCallsSummary(((UserSession)Session.Contents["UserData"]).SipAccount, fromDate, DateTime.Now);

            return chartData;
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

            foreach (TopDestinations destination in topDestinations)
            {
                if (GetUserNameBySip(destination.PhoneNumber) != string.Empty)
                {
                    destination.UserName = GetUserNameBySip(destination.PhoneNumber);
                    continue;
                }

                if (GetUserNameByNumber(destination.PhoneNumber) != string.Empty)
                {
                    destination.UserName = GetUserNameByNumber(destination.PhoneNumber);
                    continue;
                }
                destination.UserName = "N/A";
            }

            TopDestinationNumbersStore.DataSource = topDestinations;
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
            sipAccount = ((UserSession)Session.Contents["UserData"]).SipAccount;

            wherePart.Add("SourceUserUri", sipAccount);
            wherePart.Add("marker_CallTypeID", 1);
            wherePart.Add("ui_CallType", null);
            wherePart.Add("ac_IsInvoiced", "NO");

            phoneCalls = PhoneCall.GetPhoneCalls(columns, wherePart, 0);

            return phoneCalls.Count;
        }

        private string GetUserNameByNumber(string phoneNumber) 
        {
            if (phoneBookEntries.ContainsKey(phoneNumber))
                return phoneBookEntries[phoneNumber].Name;
            else 
            {
                AdLib adRoutines = new AdLib();
                string DisplayName = adRoutines.getUsersAttributesFromPhone(phoneNumber).DisplayName;

                if (DisplayName != null)
                    return DisplayName;
                else
                    return string.Empty;
            }
                
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