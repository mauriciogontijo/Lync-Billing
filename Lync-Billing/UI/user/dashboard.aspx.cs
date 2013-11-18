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

using Lync_Billing.DB;
using Lync_Billing.Libs;
using Lync_Billing.DB.Summaries;
using Lync_Billing.DB.Statistics;


namespace Lync_Billing.ui.user
{
    public partial class dashboard : System.Web.UI.Page
    {
        UserSession session;
        private string sipAccount = string.Empty;
        private string normalUserRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        private string userDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee);
        
        public int unmarkedCallsCount = 0;
        UserCallsSummary UserSummary = new UserCallsSummary();
        List<UserCallsSummary> UserSummaryList = new List<UserCallsSummary>();

        public Dictionary<string, PhoneBook> phoneBookEntries;
        public List<TopDestinationNumbers> TopDestinationNumbersList;
        public List<TopDestinationCountries> TopDestinationCountriesList;

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
                session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
                if (session.ActiveRoleName != normalUserRoleName && session.ActiveRoleName != userDelegeeRoleName)
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

            //Get this user's mail statistics
            userMailStatistics = MailStatistics.GetMailStatistics(sipAccount, DateTime.Now);

            //Configure the welcome ext-js toggled welcome-message.
            //Misc.Message("Welcome","Welcome " + current_session.PrimaryDisplayName,"info");
        }


        protected void DurationCostChartStore_Load(object sender, EventArgs e)
        {
            DurationCostChartStore.DataSource = UserCallsSummary.GetUsersCallsSummary(sipAccount, DateTime.Now.Year, 1, 12);
            DurationCostChartStore.DataBind();
        }


        protected void TopDestinationNumbersStore_Load(object sender, EventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            TopDestinationNumbersList = TopDestinationNumbers.GetTopDestinationNumbers(sipAccount, 5);

            foreach (TopDestinationNumbers destination in TopDestinationNumbersList)
            {
                //if (GetUserNameBySip(destination.PhoneNumber) != string.Empty)
                //{
                //    destination.UserName = GetUserNameBySip(destination.PhoneNumber);
                //    continue;
                //}

                if (phoneBookEntries.ContainsKey(destination.PhoneNumber))
                {
                    string temporaryName = phoneBookEntries[destination.PhoneNumber].Name;
                    destination.UserName = (!string.IsNullOrEmpty(temporaryName)) ? temporaryName : "N/A";
                }
                else
                {
                    destination.UserName = "N/A";
                }
            }

            TopDestinationNumbersStore.DataSource = TopDestinationNumbersList;
            TopDestinationNumbersStore.DataBind();
        }

        protected void TopDestinationCountriesStore_Load(object sender, EventArgs e)
        {
            TopDestinationCountriesList = TopDestinationCountries.GetTopDestinationNumbersForUser(sipAccount, 5);
            TopDestinationCountriesStore.DataSource = TopDestinationCountriesList;
            
            TopDestinationCountriesStore.DataBind();
        }
        
        protected int getUnmarkedCallsCount()
        {
            wherePart = new Dictionary<string, object>
            {
                { Enums.GetDescription(Enums.PhoneCalls.UI_CallType), null }
            };

            phoneCalls = PhoneCall.GetPhoneCalls(sipAccount,null).ToList();

            return phoneCalls.Count;
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