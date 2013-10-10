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


namespace Lync_Billing.ui.user
{
    public partial class dashboard : System.Web.UI.Page
    {
        public int unmarkedCallsCount = 0;
        UsersCallsSummary UserSummary = new UsersCallsSummary();
        List<UsersCallsSummary> UserSummaryList = new List<UsersCallsSummary>();

        private string sipAccount = string.Empty;

        public Dictionary<string, PhoneBook> phoneBookEntries;
        public List<TopDestinations> topDestinations;
        public List<TopCountries> topCountries;

        public Dictionary<string, object> wherePart = new Dictionary<string, object>();
        public List<string> columns = new List<string>();
        public List<PhoneCall> phoneCalls;
        public MailStatistics userMailStatistics;

        //This actually takes a copy of the current session for some uses on the frontend.
        public UserSession current_session { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
           //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/user/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                UserSession session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
                if (session.ActiveRoleName != "user" && session.ActiveRoleName != "delegee")
                {
                    string url = @"~/ui/session/authenticate.aspx?access=" + session.ActiveRoleName;
                    Response.Redirect(url);
                }
            }


            //Copy the current session to the instance variable, this is needed to make it more easier to access the current session from the front-end.
            current_session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            //initialize the local copy of the current user's PrimarySipAccount
            sipAccount = current_session.EffectiveSipAccount;

            //Initialize the unmarked calls counter - this is being used in the frontend.
            unmarkedCallsCount = getUnmarkedCallsCount();

            //Initialize the Address Book data.
            phoneBookEntries = PhoneBook.GetAddressBook(sipAccount);

            //Get the phone calls chart data.
            DurationCostChartStore.DataSource = UsersCallsSummary.GetUsersCallsSummary(sipAccount, DateTime.Now.Year, 1, 12);
            DurationCostChartStore.DataBind();

            //Get this user's mail statistics
            userMailStatistics = MailStatistics.GetMailStatistics(sipAccount, DateTime.Now);

            //Configure the welcome ext-js toggled welcome-message.
            //Misc.Message("Welcome","Welcome " + current_session.PrimaryDisplayName,"info");
        }
        
        public List<UsersCallsSummaryChartData> getChartData(string typeOfSummary = "")
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime fromDate = DateTime.ParseExact(DateTime.Now.Year.ToString() + "-01-01", "yyyy-mm-dd", provider);
            List<UsersCallsSummaryChartData> chartData = UsersCallsSummaryChartData.GetUsersCallsSummary(sipAccount, fromDate, DateTime.Now);

            return chartData;
        }

        protected void DurationCostChartStore_Load(object sender, EventArgs e)
        {
            DurationCostChartStore.DataSource = UsersCallsSummary.GetUsersCallsSummary(sipAccount, DateTime.Now.Year, 1, 12);
            DurationCostChartStore.DataBind();
        }

        protected void TopDestinationNumbersStore_Load(object sender, EventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
            
            topDestinations = TopDestinations.GetTopDestinations(sipAccount);

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
            topCountries = TopCountries.GetTopDestinationsForUser(sipAccount);
            TopDestinationCountriesStore.DataSource = topCountries;
            
            TopDestinationCountriesStore.DataBind();
        }

        protected int getUnmarkedCallsCount()
        {
            wherePart.Add("SourceUserUri", sipAccount);
            wherePart.Add("marker_CallTypeID", 1);
            wherePart.Add("ui_CallType", null);
            wherePart.Add("Exclude", 0);
            //wherePart.Add("ac_IsInvoiced", "NO");

            phoneCalls = PhoneCall.GetPhoneCalls(columns, wherePart, 0).Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null).ToList();

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
            ADUserInfo userInfo = adRoutines.GetUserAttributes(sipAccount);

            if (userInfo != null && userInfo.DisplayName != null)
                return userInfo.DisplayName;
            else
                return string.Empty;
        }
    }

}