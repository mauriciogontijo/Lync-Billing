﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Ext.Net;

using Lync_Billing.Backend;
using Lync_Billing.Libs;
using Lync_Billing.Backend.Summaries;
using Lync_Billing.Backend.Statistics;


namespace Lync_Billing.ui.user
{
    public partial class dashboard : System.Web.UI.Page
    {
        //This is used for the statistics filter
        private const string STATISTICS_FILTER_CUSTOM_YEAR_NAME = "One Year Ago from Today";

        //Instance Variables
        private UserSession session;
        private string sipAccount = string.Empty;
        private string normalUserRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        private string userDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee);
        
        //public variables made available for the view
        public int unmarkedCallsCount = 0;
        public string DisplayName = string.Empty;

        //This is used as a flag to fire AutoMarkPhoneCallsFromAddressBook directly after login only!
        public static bool IsThisTheFirstTimeAfterLogin = false;

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
            sipAccount = session.GetEffectiveSipAccount();

            //Get the display name for this user
            DisplayName = session.GetEffectiveDisplayName();

            //Initialize the Address Book data.
            phoneBookEntries = PhoneBook.GetAddressBook(sipAccount);

            //Get this user's mail statistics
            userMailStatistics = MailStatistics.GetMailStatistics(sipAccount, DateTime.Now);

            //Initialize the unmarked calls counter - this is being used in the frontend.
            //unmarkedCallsCount = getUnmarkedCallsCount();
            unmarkedCallsCount = AutoMarkPhoneCallsFromAddressBook();

            //SHA1 hash = SHA1.Create();
            //ASCIIEncoding x = new ASCIIEncoding();
            //string source = (session.NormalUserInfo.SipAccount + session.NormalUserInfo.Department + session.NormalUserInfo.SiteName);
            //string gethash = HttpUtility.UrlEncode(Convert.ToBase64String(hash.ComputeHash(x.GetBytes(source))));
        }

        //Automark phonecalls from addressbook
        //This returns the number of phonecalls which weren't marked
        private int AutoMarkPhoneCallsFromAddressBook()
        {
            List<PhoneCall> userSessionPhoneCalls;
            Dictionary<string, PhoneBook> userSessionAddressbook;
            string userSessionPhoneCallsPerPage;
            PhoneBook addressBookEntry;
            int numberOfRemainingUnmarked;

            //Get the unmarked calls from the user session phonecalls container
            session.FetchSessionPhonecallsAndAddressbookData(out userSessionPhoneCalls, out userSessionAddressbook, out userSessionPhoneCallsPerPage);

            numberOfRemainingUnmarked = userSessionPhoneCalls.Where(phoneCall => string.IsNullOrEmpty(phoneCall.UI_CallType)).ToList().Count;

            if (IsThisTheFirstTimeAfterLogin == false)
            {
                //If the user has no addressbook contacts, skip the auto marking process
                if (userSessionAddressbook.Keys.Count > 0)
                {
                    foreach (var phoneCall in userSessionPhoneCalls.Where(phoneCall => string.IsNullOrEmpty(phoneCall.UI_CallType)))
                    {
                        if (userSessionAddressbook.Keys.Contains(phoneCall.DestinationNumberUri))
                        {
                            addressBookEntry = (PhoneBook)userSessionAddressbook[phoneCall.DestinationNumberUri];

                            if (!string.IsNullOrEmpty(addressBookEntry.Type))
                            {
                                phoneCall.UI_CallType = addressBookEntry.Type;
                                phoneCall.UI_UpdatedByUser = sipAccount;
                                phoneCall.UI_MarkedOn = DateTime.Now;

                                PhoneCall.UpdatePhoneCall(phoneCall);

                                numberOfRemainingUnmarked = numberOfRemainingUnmarked - 1;
                            }
                        }
                    }
                }

                session.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls, userSessionAddressbook, null);

                IsThisTheFirstTimeAfterLogin = true;
            }

            return numberOfRemainingUnmarked;
        }


        private List<TopDestinationNumbers> FilterDestinationNumbersNames(List<TopDestinationNumbers> DetinationNumbersList)
        {
            foreach (TopDestinationNumbers destination in DetinationNumbersList)
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

            return DetinationNumbersList;
        }


        protected int getUnmarkedCallsCount()
        {
            sipAccount = session.GetEffectiveSipAccount();

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


        protected void CallsCostsChartStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                DateTime fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                DateTime toDate = DateTime.Now;

                CallsCostsChartStore.DataSource = UserCallsSummary.GetUsersCallsSummary(sipAccount, fromDate, toDate);
                CallsCostsChartStore.DataBind();
            }
        }


        protected void TopDestinationNumbersStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                TopDestinationNumbersList = FilterDestinationNumbersNames(TopDestinationNumbers.GetTopDestinationNumbers(sipAccount, 5));

                TopDestinationNumbersStore.DataSource = TopDestinationNumbersList;
                TopDestinationNumbersStore.DataBind();
            }
        }


        protected void TopDestinationCountriesStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                TopDestinationCountriesList = TopDestinationCountries.GetTopDestinationCountriesForUser(sipAccount, 5);

                TopDestinationCountriesStore.DataSource = TopDestinationCountriesList;
                TopDestinationCountriesStore.DataBind();
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
            //CustomizeStats_Year.GetStore().LoadData(years);
        }


        protected void CustomizeStats_Years_Select(object sender, DirectEventArgs e)
        {
            if (CustomizeStats_Years.SelectedItem.Index > -1)
            {
                string selectedValue = Convert.ToString(CustomizeStats_Years.SelectedItem.Value);

                if (selectedValue == STATISTICS_FILTER_CUSTOM_YEAR_NAME)
                {
                    CustomizeStats_Quarters.Hide();
                }
                else
                {
                    CustomizeStats_Quarters.Show();
                }
            }
        }


        protected void ManageStatisticsBtn_Click(object sender, DirectEventArgs e)
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
                if(filterYear == STATISTICS_FILTER_CUSTOM_YEAR_NAME)
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


                //Re-bind TopDestinationCountries to match the filter dates criteria
                var topDestinationCountries_DATA = TopDestinationCountries.GetTopDestinationCountriesForUser(sipAccount, 5, startingDate, endingDate);
                
                if (topDestinationCountries_DATA.Count > 0) 
                    TopDestinationCountriesChart.GetStore().LoadData(topDestinationCountries_DATA);
                else
                    TopDestinationCountriesChart.GetStore().RemoveAll();

                TopDestinationCountriesPanel.Title = String.Format("Most Called Countries - {0} {1}", yearTitleString, quarterTitleString);


                //Re-bind TopDestinationNumbers to match the filter dates criteria
                TopDestinationNumbersGrid.GetStore().LoadData(
                    FilterDestinationNumbersNames(
                        TopDestinationNumbers.GetTopDestinationNumbers(sipAccount, 5, startingDate, endingDate)
                    )
                );

                TopDestinationNumbersGrid.Title = String.Format("Most Called Numbers - {0} {1}", yearTitleString, quarterTitleString);


                //Re-bind CallsCosts to match the filter dates criteria
                CallsCostsChart.GetStore().LoadData(
                    UserCallsSummary.GetUsersCallsSummary(sipAccount, startingDate, endingDate)
                );

                CallsCostsChartPanel.Title = String.Format("Calls Cost Chart - {0} {1}", yearTitleString, quarterTitleString);

                //Hide the window
                CustomizeStatisticsWindow.Hide();

            }//End-if

        }//End-Function

    }//End-class

}