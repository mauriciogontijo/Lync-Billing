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
        private UserSession session;
        private string sipAccount = string.Empty;
        private string normalUserRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        private string userDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee);
        
        //public variables made available for the view
        public int unmarkedCallsCount = 0;
        public string DisplayName = string.Empty;

        public Dictionary<string, PhoneBook> phoneBookEntries;
        public List<PhoneCall> phoneCalls;
        public MailStatistics userMailStatistics;
        public List<TopDestinationNumbers> TopDestinationNumbersList;
        public List<TopDestinationCountries> TopDestinationCountriesList;

        public Dictionary<string, object> wherePart = new Dictionary<string, object>();
        public List<string> columns = new List<string>();

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
            sipAccount = GetEffectiveSipAccount();

            //Initialize the unmarked calls counter - this is being used in the frontend.
            unmarkedCallsCount = getUnmarkedCallsCount();

            //Get the display name for this user
            DisplayName = GetEffectiveDisplayName();

            //Initialize the Address Book data.
            phoneBookEntries = PhoneBook.GetAddressBook(sipAccount);

            //Get this user's mail statistics
            userMailStatistics = MailStatistics.GetMailStatistics(sipAccount, DateTime.Now);

            //Set the year number in the charts header's title
            string currentYear = DateTime.Now.Year.ToString();
            CallsCostsChartPanel.Title = "Calls Cost Chart for " + currentYear;
            TopDestinationNumbersGrid.Title = "Most Called Numbers in " + currentYear;
            TopDestinationCountriesPanel.Title = "Most Called Countries in " + currentYear;
        }


        //Get the user sipaccount.
        private string GetEffectiveSipAccount()
        {
            string userSipAccount = string.Empty;
            session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            //If the user is a normal one, just return the normal user sipaccount.
            if (session.ActiveRoleName == normalUserRoleName)
            {
                userSipAccount = session.NormalUserInfo.SipAccount;
            }
            //if the user is a user-delegee return the delegate sipaccount.
            else if (session.ActiveRoleName == userDelegeeRoleName)
            {
                userSipAccount = session.DelegeeAccount.DelegeeUserAccount.SipAccount;
            }

            return userSipAccount;
        }


        //Get the user displayname.
        private string GetEffectiveDisplayName()
        {
            string userDisplayName = string.Empty;
            session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            //If the user is a normal one, just return the normal user sipaccount.
            if (session.ActiveRoleName == normalUserRoleName)
            {
                userDisplayName = session.NormalUserInfo.DisplayName;
            }
            //if the user is a user-delegee return the delegate sipaccount.
            else if (session.ActiveRoleName == userDelegeeRoleName)
            {
                userDisplayName = session.DelegeeAccount.DelegeeUserAccount.DisplayName;
            }

            return userDisplayName;
        }


        protected void CallsCostsChartStore_Load(object sender, EventArgs e)
        {
            CallsCostsChartStore.DataSource = UserCallsSummary.GetUsersCallsSummary(sipAccount, DateTime.Now.Year, 1, 12);
            CallsCostsChartStore.DataBind();
        }


        protected void TopDestinationNumbersStore_Load(object sender, EventArgs e)
        {
            sipAccount = GetEffectiveSipAccount();

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
            TopDestinationCountriesList = TopDestinationCountries.GetTopDestinationCountriesForUser(sipAccount, 5);
            TopDestinationCountriesStore.DataSource = TopDestinationCountriesList;
            
            TopDestinationCountriesStore.DataBind();
        }
        
        protected int getUnmarkedCallsCount()
        {
            sipAccount = GetEffectiveSipAccount();
            
            wherePart = new Dictionary<string, object>
            {
                { Enums.GetDescription(Enums.PhoneCalls.UI_CallType), null }
            };

            return PhoneCall.GetPhoneCalls(sipAccount, wherePart).ToList().Count;
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