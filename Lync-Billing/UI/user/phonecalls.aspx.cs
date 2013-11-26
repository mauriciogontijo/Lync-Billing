using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Xsl;
using System.Linq.Expressions;
using System.Xml.Serialization;
using Ext.Net;
using Newtonsoft.Json;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Lync_Billing.DB;
using Lync_Billing.Libs;

namespace Lync_Billing.ui.user
{
    public partial class phonecalls : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string normalUserRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        private string userDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee);

        private List<PhoneCall> AutoMarkedPhoneCalls = new List<PhoneCall>();
        private string pageData = string.Empty;
        private StoreReadDataEventArgs e;

        string xmldoc = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/user/phonecalls.aspx";
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

            sipAccount = GetEffectiveSipAccount();

            //Handle user delegee mode and normal user mode
            if (session.ActiveRoleName == userDelegeeRoleName)
                session.DelegeeAccount.DelegeeUserAddressbook = PhoneBook.GetAddressBook(sipAccount);
            else
                session.Addressbook = PhoneBook.GetAddressBook(sipAccount);
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


        private void ShowInfoMessages()
        {
            string title = "Did You Know?";
            string type = "help";
            string notificationMessage = "You can click on the [HELP] button at the Top-Right corner of the grid whenever you want any help.";
            string fullMessage = String.Format(
                "<div class='text-left'>" + 
                    "<p>1. You can select multiple phonecalls by pressing the [Ctrl] button.</p>" + 
                    "<br />" + 
                    "<p>2. You can allocate your phonecalls by [Right Clicking] on the grid and choosing your preferred action.</p>" + 
                    "<br />" +
                    "<p>3. If you [Right Click] on the grid, you can either mark some selected phonecall(s), or you can mark the destinations of these phonecalls, which will result in adding these destintions to your phonebook and from that moment on, any phonecall to these destinations will be marked automatically.</p>" + 
                    "<br />" +
                    "<p>4. You can add Contact Name to a phonecall destination by [Double Clicking] on the \"Contact Name\" field and then filling the text box, please note that this works for the Unallocated phonecalls.</p>" + 
                "</div>"
            );

            if (!Ext.Net.X.IsAjaxRequest)
            {
                //HelperFunctions.Message("User Help", "To allocate your phonecalls please [Right Click] on the grid and choose your preferred action.", "help", isPinned: true, width: 220, height: 120);
                //HelperFunctions.Message("User Help", "You can select multiple phonecalls by pressing the [Ctrl] button.", "help", isPinned: true, width: 220, height: 120);

                HelperFunctions.Message(title, notificationMessage, type, hideDelay: 10000, width: 250, height: 100);
            }
        }


        //Get the user session phonecalls
        //Handle normal user mode and user delegee mode
        private List<PhoneCall> GetUserSessionPhoneCalls(bool force = false)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = GetEffectiveSipAccount();

            //User Delegee Mode
            if (session.ActiveRoleName == userDelegeeRoleName)
            {
                if (session.DelegeeAccount.DelegeeUserPhonecalls == null || session.DelegeeAccount.DelegeeUserPhonecalls.Count == 0 || force == true)
                {
                    var userPhoneCalls = PhoneCall.GetPhoneCalls(sipAccount).Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null);
                    var addressbook = session.DelegeeAccount.DelegeeUserAddressbook;

                    if (addressbook == null || addressbook.Count == 0)
                        addressbook = PhoneBook.GetAddressBook(sipAccount);

                    foreach (var phoneCall in userPhoneCalls)
                    {
                        if (addressbook.ContainsKey(phoneCall.DestinationNumberUri))
                        {
                            phoneCall.PhoneBookName = ((PhoneBook)addressbook[phoneCall.DestinationNumberUri]).Name;
                        }
                    }

                    session.DelegeeAccount.DelegeeUserPhonecalls = userPhoneCalls.ToList();

                    xmldoc = HelperFunctions.SerializeObject<List<PhoneCall>>(session.DelegeeAccount.DelegeeUserPhonecalls);
                }

                return session.DelegeeAccount.DelegeeUserPhonecalls;
            }
            //Normal User Mode
            else
            {
                if (session.Phonecalls == null || session.Phonecalls.Count == 0 || force == true)
                {
                    var userPhoneCalls = PhoneCall.GetPhoneCalls(sipAccount).Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null);
                    var addressbook = session.Addressbook;

                    if (addressbook == null || addressbook.Count == 0)
                        addressbook = PhoneBook.GetAddressBook(sipAccount);

                    foreach (var phoneCall in userPhoneCalls)
                    {
                        if (addressbook.ContainsKey(phoneCall.DestinationNumberUri))
                        {
                            phoneCall.PhoneBookName = ((PhoneBook)session.Addressbook[phoneCall.DestinationNumberUri]).Name;
                        }
                    }

                    session.Phonecalls = userPhoneCalls.ToList();

                    xmldoc = HelperFunctions.SerializeObject<List<PhoneCall>>(session.Phonecalls);
                }

                return session.Phonecalls;
            }
        }


        //Get the user session phonecalls data, such as: PhoneCalls list, AddressBook and PhoneCallsPerPage JSON String
        //Handle normal user mode and user delegee mode
        private void GetUserSessionPhoneCallsData(out List<PhoneCall> userSessionPhoneCalls, out Dictionary<string, PhoneBook> userSessionAddressBook, out string userSessionPhoneCallsPerPageJson)
        {
            session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            //Initialize the passed varaibles
            userSessionPhoneCalls = new List<PhoneCall>();
            userSessionAddressBook = new Dictionary<string, PhoneBook>();
            userSessionPhoneCallsPerPageJson = string.Empty;

            //This part is off-loaded to another procedure due to size of code
            userSessionPhoneCalls = GetUserSessionPhoneCalls();

            if (session.ActiveRoleName == userDelegeeRoleName)
            {
                userSessionAddressBook = session.DelegeeAccount.DelegeeUserAddressbook;
                userSessionPhoneCallsPerPageJson = session.DelegeeAccount.DelegeeUserPhonecallsPerPage;
            }
            else
            {
                userSessionPhoneCalls = session.Phonecalls;
                userSessionPhoneCallsPerPageJson = session.PhonecallsPerPage;
            }

        }


        //Pass any of the three variables to this function and it will assign it's data to the respective UserSession container
        //The functions respectively stand for the the list of user phonecalls, his/her addressbook contacts, and the phonecalls grid json string
        //This handles the normal user mode and the user delegee mode.
        private void SetUserSessionPhoneCallsData(List<PhoneCall> userSessionPhoneCalls, Dictionary<string, PhoneBook> userSessionAddressBook, string userSessionPhoneCallsPerPageJson)
        {
            if (session.ActiveRoleName == userDelegeeRoleName)
            {
                if (userSessionPhoneCalls != null && userSessionPhoneCalls.Count > 0)
                    session.DelegeeAccount.DelegeeUserPhonecalls = userSessionPhoneCalls;

                if (userSessionAddressBook != null && userSessionAddressBook.Count > 0)
                    session.DelegeeAccount.DelegeeUserAddressbook = userSessionAddressBook;

                if (!string.IsNullOrEmpty(userSessionPhoneCallsPerPageJson))
                    session.DelegeeAccount.DelegeeUserPhonecallsPerPage = userSessionPhoneCallsPerPageJson;
            }
            else
            {
                if (userSessionPhoneCalls != null && userSessionPhoneCalls.Count > 0)
                    session.Phonecalls = userSessionPhoneCalls;

                if (userSessionAddressBook != null && userSessionAddressBook.Count > 0)
                    session.Addressbook = userSessionAddressBook;

                if (!string.IsNullOrEmpty(userSessionPhoneCallsPerPageJson))
                    session.PhonecallsPerPage = userSessionPhoneCallsPerPageJson;
            }
        }


        protected void PhoneCallsDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (this.e.Start != -1)
                e.InputParameters["start"] = this.e.Start;
            else
                e.InputParameters["start"] = 0;

            if (this.e.Limit != -1)
                e.InputParameters["limit"] = this.e.Limit;
            else
                e.InputParameters["limit"] = 25;

            if (!string.IsNullOrEmpty(this.e.Parameters["sort"]))
                e.InputParameters["sort"] = this.e.Sort[0];
            else
                e.InputParameters["sort"] = null;

            if (!string.IsNullOrEmpty(this.e.Parameters["filter"]))
                e.InputParameters["filter"] = this.e.Filter[0];
            else
                e.InputParameters["filter"] = null;
        }

        protected void PhoneCallsDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            (this.PhoneCallsStore.Proxy[0] as PageProxy).Total = (int)e.OutputParameters["count"];
        }

        protected void PhoneCallsStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            
        }

        protected void PhoneCallsStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            this.e = e;
            PhoneCallsStore.DataBind();
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

            //Handle user delegee mode and normal use mode
            if (session.ActiveRoleName == userDelegeeRoleName)
                session.DelegeeAccount.DelegeeUserPhonecallsPerPage = PhoneCallsStore.JsonData;
            else
                session.PhonecallsPerPage = PhoneCallsStore.JsonData;
        }

        public List<PhoneCall> GetPhoneCallsFilter(int start, int limit, DataSorter sort, out int count, DataFilter filter)
        {
            List<PhoneCall> userSessionPhoneCalls;
            IEnumerable<PhoneCall> filteredPhoneCalls;
            int filteredPhoneCallsCount;

            //Get use session and user phonecalls list.
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            
            //Get user session phonecalls; handle normal user mode and delegee mode
            userSessionPhoneCalls = GetUserSessionPhoneCalls();


            //Begin the filtering process
            if(filter == null)
                filteredPhoneCalls = userSessionPhoneCalls.Where(phoneCall => phoneCall.UI_CallType == null).AsQueryable();
            else
                filteredPhoneCalls = userSessionPhoneCalls.Where(phoneCall => phoneCall.UI_CallType == filter.Value).AsQueryable();
         

            //Begin sorting process
            if (sort != null)
            {
                ParameterExpression param = Expression.Parameter(typeof(PhoneCall), "e");
                var p = Expression.Parameter(typeof(PhoneCall));
                var sortExpression = Expression.Lambda<Func<PhoneCall, object>>(Expression.TypeAs(Expression.Property(p, sort.Property), typeof(object)), p).Compile();

                if (sort.Direction == Ext.Net.SortDirection.DESC)
                    filteredPhoneCalls = filteredPhoneCalls.OrderByDescending(sortExpression);
                else
                    filteredPhoneCalls = filteredPhoneCalls.OrderBy(sortExpression);
            }

            filteredPhoneCallsCount = filteredPhoneCalls.Count();

            if (start >= 0 && limit > 0)
                filteredPhoneCalls = filteredPhoneCalls.Skip(start).Take(limit);

            count = filteredPhoneCallsCount;
            
            return filteredPhoneCalls.ToList();
        }

        protected void AssignAllPersonal(object sender, DirectEventArgs e)
        {
            //session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            //sipAccount = GetEffectiveSipAccount();

            //RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            //string json = e.ExtraParams["Values"];

            //List<PhoneCall> phoneCalls = new List<PhoneCall>();
            //List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();

            //JavaScriptSerializer serializer = new JavaScriptSerializer();

            //phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.NullValueHandling = NullValueHandling.Ignore;

            //perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(session.PhonecallsPerPage, settings);

            //foreach (PhoneCall phoneCall in phoneCalls)
            //{
            //    var matchedDestinationCalls = session.Phonecalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);

            //    foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
            //    {
            //        if (matchedDestinationCall.UI_CallType == "Personal")
            //            break;

            //        matchedDestinationCall.UI_CallType = "Personal";
                    
            //        matchedDestinationCall.UI_MarkedOn = DateTime.Now;
            //        matchedDestinationCall.UI_UpdatedByUser = sipAccount;

            //        PhoneCall.UpdatePhoneCall(matchedDestinationCall);
            //    }
            //}
            
            //ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            //PhoneCallsStore.LoadPage(1);
        }

        protected void AssignAllBusiness(object sender, DirectEventArgs e)
        {
            //session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            //sipAccount = GetEffectiveSipAccount();

            //RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            //string json = e.ExtraParams["Values"];

            //List<PhoneCall> phoneCalls = new List<PhoneCall>();
            //List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();

            //JavaScriptSerializer serializer = new JavaScriptSerializer();

            //phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.NullValueHandling = NullValueHandling.Ignore;

            //perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(session.PhonecallsPerPage, settings);

            //foreach (PhoneCall phoneCall in phoneCalls)
            //{
            //    var matchedDestinationCalls = session.Phonecalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);

            //    foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
            //    {
            //        if (matchedDestinationCall.UI_CallType == "Business")
            //            break;

            //        matchedDestinationCall.UI_CallType = "Business";
            //        matchedDestinationCall.UI_MarkedOn = DateTime.Now;
            //        matchedDestinationCall.UI_UpdatedByUser = sipAccount;

            //        PhoneCall.UpdatePhoneCall(matchedDestinationCall);
            //    }
            //}
            //ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            //PhoneCallsStore.LoadPage(1);
        }

        protected void AssignPersonal(object sender, DirectEventArgs e) 
        {
            PhoneCall sessionPhoneCallRecord;
            List<PhoneCall> submittedPhoneCalls;
            List<PhoneCall> userSessionPhoneCalls;
            string userSessionPhoneCallsPerPageJson = string.Empty;

            string json = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //Get the session and sip account of the current user
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = GetEffectiveSipAccount();

            //Get user phonecalls from the session
            //Handle user delegee mode and normal user mode
            userSessionPhoneCalls = GetUserSessionPhoneCalls();
            
            json = e.ExtraParams["Values"];
            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);
            userSessionPhoneCallsPerPageJson = json;


            foreach (PhoneCall phoneCall in submittedPhoneCalls)
            {
                sessionPhoneCallRecord = userSessionPhoneCalls.Where(o => o.SessionIdTime == phoneCall.SessionIdTime).First();

                sessionPhoneCallRecord.UI_CallType = "Personal";
                sessionPhoneCallRecord.UI_MarkedOn = DateTime.Now;
                sessionPhoneCallRecord.UI_UpdatedByUser = sipAccount;

                PhoneCall.UpdatePhoneCall(sessionPhoneCallRecord);

                ModelProxy model = PhoneCallsStore.Find(Enums.GetDescription(Enums.PhoneCalls.SessionIdTime), sessionPhoneCallRecord.SessionIdTime.ToString());
                model.Set(sessionPhoneCallRecord);
                model.Commit();
            }

            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            PhoneCallsStore.LoadPage(1);

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            SetUserSessionPhoneCallsData(userSessionPhoneCalls, null, null);
        }

        protected void AssignBusiness(object sender, DirectEventArgs e) 
        {
            PhoneCall sessionPhoneCallRecord;
            List<PhoneCall> submittedPhoneCalls;
            List<PhoneCall> userSessionPhoneCalls;
            string userSessionPhoneCallsPerPageJson = string.Empty;

            string json = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //Get the session and sip account of the current user
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = GetEffectiveSipAccount();

            //Get user phonecalls from the session
            //Handle user delegee mode and normal user mode
            userSessionPhoneCalls = GetUserSessionPhoneCalls();

            json = e.ExtraParams["Values"];
            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);
            userSessionPhoneCallsPerPageJson = json;

            foreach (PhoneCall phoneCall in submittedPhoneCalls)
            {
                sessionPhoneCallRecord = userSessionPhoneCalls.Where(o => o.SessionIdTime == phoneCall.SessionIdTime).First();

                sessionPhoneCallRecord.UI_CallType = "Business";
                sessionPhoneCallRecord.UI_MarkedOn = DateTime.Now;
                sessionPhoneCallRecord.UI_UpdatedByUser = sipAccount;

                PhoneCall.UpdatePhoneCall(sessionPhoneCallRecord);

                ModelProxy model = PhoneCallsStore.Find(Enums.GetDescription(Enums.PhoneCalls.SessionIdTime), sessionPhoneCallRecord.SessionIdTime.ToString());
                model.Set(sessionPhoneCallRecord);
                model.Commit();
            }

            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            PhoneCallsStore.LoadPage(1);

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            SetUserSessionPhoneCallsData(userSessionPhoneCalls, null, userSessionPhoneCallsPerPageJson);
        }

        protected void AssignDispute(object sender, DirectEventArgs e)
        {
            List<PhoneCall> submittedPhoneCalls;
            List<PhoneCall> phoneCallsPerPage;
            List<PhoneCall> userSessionPhoneCalls;
            string userSessionPhoneCallsPerPageJson = string.Empty;

            string json = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //Get the session and sip account of the current user
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = GetEffectiveSipAccount();


            //Get user phonecalls from the session
            //Handle user delegee mode and normal user mode
            userSessionPhoneCalls = GetUserSessionPhoneCalls();


            json = e.ExtraParams["Values"];
            settings.NullValueHandling = NullValueHandling.Ignore;

            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);
            phoneCallsPerPage = JsonConvert.DeserializeObject<List<PhoneCall>>(session.PhonecallsPerPage, settings);
            userSessionPhoneCallsPerPageJson = json;


            foreach (PhoneCall phoneCall in submittedPhoneCalls)
            {
                PhoneCall matchedDestinationCalls = userSessionPhoneCalls.Where(o => o.SessionIdTime == phoneCall.SessionIdTime).First();
                
                matchedDestinationCalls.UI_CallType = "Disputed";
                matchedDestinationCalls.UI_MarkedOn = DateTime.Now;
                matchedDestinationCalls.UI_UpdatedByUser = sipAccount;

                PhoneCall.UpdatePhoneCall(matchedDestinationCalls);

                ModelProxy model = PhoneCallsStore.Find(Enums.GetDescription(Enums.PhoneCalls.SessionIdTime), matchedDestinationCalls.SessionIdTime.ToString());
                model.Set(matchedDestinationCalls);
                model.Commit();

            }

            PhoneCallsAllocationToolsMenu.Hide();
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            PhoneCallsStore.LoadPage(1);

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            SetUserSessionPhoneCallsData(userSessionPhoneCalls, null, userSessionPhoneCallsPerPageJson);
        }

        protected void AssignAlwaysPersonal(object sender, DirectEventArgs e)
        {
            string json = string.Empty;
            RowSelectionModel selectiomModel;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //These are used for querying the filtering the submitted phonecalls and their destinations
            PhoneBook phoneBookEntry;
            List<PhoneCall> submittedPhoneCalls;
            List<PhoneCall> phoneCallsPerPage;
            IEnumerable<PhoneCall> matchedDestinationCalls;
            List<PhoneBook> newOrUpdatedPhoneBookEntries = new List<PhoneBook>();

            //These would refer to either the the user's or the delegee's
            string userSessionPhoneCallsPerPageJson = string.Empty;
            List<PhoneCall> userSessionPhoneCalls = new List<PhoneCall>();
            Dictionary<string, PhoneBook> userSessionAddressBook = new Dictionary<string, PhoneBook>();
            
            //Get user session and effective sip account
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = GetEffectiveSipAccount();


            //Get user phoneCalls, addressbook, and phoneCallsPerPage;
            //Handle user delegee mode and normal user mode
            GetUserSessionPhoneCallsData(out userSessionPhoneCalls, out userSessionAddressBook, out userSessionPhoneCallsPerPageJson);

            
            //Get the submitted grid data
            json = e.ExtraParams["Values"];
            settings.NullValueHandling = NullValueHandling.Ignore;
            
            selectiomModel = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);
            phoneCallsPerPage = JsonConvert.DeserializeObject<List<PhoneCall>>(session.PhonecallsPerPage, settings);
            userSessionPhoneCallsPerPageJson = json;

            //Start allocating the submitted phone calls
            foreach (PhoneCall phoneCall in submittedPhoneCalls)
            {
                //Create a Phonebook Entry
                phoneBookEntry = new PhoneBook();

                //Check if this entry Already exists by either destination number and destination name (in case it's edited)
                bool found = userSessionAddressBook.ContainsKey(phoneCall.DestinationNumberUri) &&
                             (userSessionAddressBook.Values.Select(phoneBookContact => phoneBookContact.Name == phoneCall.PhoneBookName) == null ? false : true);

                if (!found)
                {
                    phoneBookEntry.Name = phoneCall.PhoneBookName ?? string.Empty;
                    phoneBookEntry.DestinationCountry = phoneCall.Marker_CallToCountry;
                    phoneBookEntry.DestinationNumber = phoneCall.DestinationNumberUri;
                    phoneBookEntry.SipAccount = sipAccount;
                    phoneBookEntry.Type = "Personal";

                    //Add Phonebook entry to Session and to the list which will be written to database 
                    if (userSessionAddressBook.ContainsKey(phoneCall.DestinationNumberUri))
                        userSessionAddressBook[phoneCall.DestinationNumberUri] = phoneBookEntry;
                    else
                        userSessionAddressBook.Add(phoneCall.DestinationNumberUri, phoneBookEntry);

                    newOrUpdatedPhoneBookEntries.Add(phoneBookEntry);
                }

                matchedDestinationCalls = userSessionPhoneCalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);
                

                foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                {
                    if (matchedDestinationCall.UI_CallType == "Personal")
                        break;

                    matchedDestinationCall.UI_CallType = "Personal";
                    matchedDestinationCall.UI_MarkedOn = DateTime.Now;
                    matchedDestinationCall.UI_UpdatedByUser = sipAccount;
                    matchedDestinationCall.PhoneBookName = phoneCall.PhoneBookName ?? string.Empty;

                    PhoneCall.UpdatePhoneCall(matchedDestinationCall);
                }
            }

            PhoneCallsAllocationToolsMenu.Hide();
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            PhoneCallsStore.LoadPage(1);

            //Add To User Addressbook Store
            PhoneBook.AddOrUpdatePhoneBookEntries(sipAccount, newOrUpdatedPhoneBookEntries);

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            SetUserSessionPhoneCallsData(userSessionPhoneCalls, userSessionAddressBook, userSessionPhoneCallsPerPageJson);
        }

        protected void AssignAlwaysBusiness(object sender, DirectEventArgs e)
        {
            string json = string.Empty;
            RowSelectionModel selectiomModel;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //These are used for querying the filtering the submitted phonecalls and their destinations
            PhoneBook phoneBookEntry;
            List<PhoneCall> submittedPhoneCalls;
            List<PhoneCall> phoneCallsPerPage;
            IEnumerable<PhoneCall> matchedDestinationCalls;
            List<PhoneBook> newOrUpdatedPhoneBookEntries = new List<PhoneBook>();

            //These would refer to either the the user's or the delegee's
            string userSessionPhoneCallsPerPageJson = string.Empty;
            List<PhoneCall> userSessionPhoneCalls = new List<PhoneCall>();
            Dictionary<string, PhoneBook> userSessionAddressBook = new Dictionary<string, PhoneBook>();

            //Get user session and effective sip account
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = GetEffectiveSipAccount();


            //Get user phoneCalls, addressbook, and phoneCallsPerPage;
            //Handle user delegee mode and normal user mode
            GetUserSessionPhoneCallsData(out userSessionPhoneCalls, out userSessionAddressBook, out userSessionPhoneCallsPerPageJson);


            //Get the submitted grid data
            json = e.ExtraParams["Values"];
            settings.NullValueHandling = NullValueHandling.Ignore;

            selectiomModel = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);
            phoneCallsPerPage = JsonConvert.DeserializeObject<List<PhoneCall>>(session.PhonecallsPerPage, settings);
            userSessionPhoneCallsPerPageJson = json;

            
            //Start allocating the submitted phone calls
            foreach (PhoneCall phoneCall in submittedPhoneCalls)
            {
                //Create a Phonebook Entry
                phoneBookEntry = new PhoneBook();

                //Check if this entry Already exists by either destination number and destination name (in case it's edited)
                bool found = userSessionAddressBook.ContainsKey(phoneCall.DestinationNumberUri) &&
                             (userSessionAddressBook.Values.Select(phoneBookContact => phoneBookContact.Name == phoneCall.PhoneBookName) == null ? false : true);
                
                if (!found)
                {
                    phoneBookEntry.Name = phoneCall.PhoneBookName ?? string.Empty;
                    phoneBookEntry.DestinationCountry = phoneCall.Marker_CallToCountry;
                    phoneBookEntry.DestinationNumber = phoneCall.DestinationNumberUri;
                    phoneBookEntry.SipAccount = sipAccount;
                    phoneBookEntry.Type = "Business";

                    //Add Phonebook entry to Session and to the list which will be written to database 
                    if (userSessionAddressBook.ContainsKey(phoneCall.DestinationNumberUri))
                        userSessionAddressBook[phoneCall.DestinationNumberUri] = phoneBookEntry;
                    else
                        userSessionAddressBook.Add(phoneCall.DestinationNumberUri, phoneBookEntry);

                    newOrUpdatedPhoneBookEntries.Add(phoneBookEntry);
                }

                matchedDestinationCalls = session.Phonecalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);

                foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                {
                    if (matchedDestinationCall.UI_CallType == "Business")
                        break;

                    matchedDestinationCall.UI_CallType = "Business";
                    matchedDestinationCall.UI_MarkedOn = DateTime.Now;
                    matchedDestinationCall.UI_UpdatedByUser = sipAccount;
                    matchedDestinationCall.PhoneBookName = phoneCall.PhoneBookName ?? string.Empty;

                    PhoneCall.UpdatePhoneCall(matchedDestinationCall);
                }
            }

            PhoneCallsAllocationToolsMenu.Hide();
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            PhoneCallsStore.LoadPage(1);

            //Add To User Addressbook Store
            PhoneBook.AddOrUpdatePhoneBookEntries(sipAccount, newOrUpdatedPhoneBookEntries);

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            SetUserSessionPhoneCallsData(userSessionPhoneCalls, userSessionAddressBook, userSessionPhoneCallsPerPageJson);
        }



        protected void RejectChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            ManagePhoneCallsGrid.GetStore().RejectChanges();
        }

        [DirectMethod]
        protected void PhoneCallsTypeFilter(object sender, DirectEventArgs e)
        {
            PhoneCallsStore.ClearFilter();

            if (FilterTypeComboBox.SelectedItem.Value != "Unmarked")
            {
                PhoneCallsStore.Filter("UI_CallType", FilterTypeComboBox.SelectedItem.Value);
                PhoneBookNameEditorTextbox.ReadOnly = true;
            }
            else
            {
                PhoneBookNameEditorTextbox.ReadOnly = false;
            }

            PhoneCallsStore.LoadPage(1);
        }

        [DirectMethod]
        protected void ShowUserHelpPanel(object sender, DirectEventArgs e)
        {
            this.UserHelpPanel.Show();
        }

    }
}