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
using Lync_Billing.Backend;
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

        private string xmldoc = string.Empty;


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

            sipAccount = session.GetEffectiveSipAccount();

            //Handle user delegee mode and normal user mode
            if (session.ActiveRoleName == userDelegeeRoleName)
                session.DelegeeAccount.DelegeeUserAddressbook = PhoneBook.GetAddressBook(sipAccount);
            else
                session.Addressbook = PhoneBook.GetAddressBook(sipAccount);
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
                //HelperFunctions.Message("Users Help", "To allocate your phonecalls please [Right Click] on the grid and choose your preferred action.", "help", isPinned: true, width: 220, height: 120);
                //HelperFunctions.Message("Users Help", "You can select multiple phonecalls by pressing the [Ctrl] button.", "help", isPinned: true, width: 220, height: 120);

                HelperFunctions.Message(title, notificationMessage, type, hideDelay: 10000, width: 250, height: 100);
            }
        }

        protected void DepartmentPhoneCallsStore_Load(object sender, EventArgs e)
        {
            //string SiteDepartment = session.NormalUserInfo.SiteName + "_" + session.NormalUserInfo.Department;
            string SiteDepartment =
                (session.ActiveRoleName == userDelegeeRoleName) ?
                session.DelegeeAccount.DelegeeUserAccount.SiteName + "-" + session.DelegeeAccount.DelegeeUserAccount.Department :
                session.NormalUserInfo.SiteName + "-" + session.NormalUserInfo.Department;

            DepartmentPhoneCallsGrid.GetStore().DataSource = PhoneCall.GetPhoneCalls(SiteDepartment);
            DepartmentPhoneCallsGrid.GetStore().DataBind();
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
            userSessionPhoneCalls = session.GetUserSessionPhoneCalls();


            //Begin the filtering process
            if(filter == null)
                filteredPhoneCalls = userSessionPhoneCalls.Where(phoneCall => string.IsNullOrEmpty(phoneCall.UI_CallType)).AsQueryable();
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
            //sipAccount = session.GetEffectiveSipAccount();

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
            //sipAccount = session.GetEffectiveSipAccount();

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
            sipAccount = session.GetEffectiveSipAccount();

            //Get user phonecalls from the session
            //Handle user delegee mode and normal user mode
            userSessionPhoneCalls = session.GetUserSessionPhoneCalls();
            
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
            session.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls, null, null);
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
            sipAccount = session.GetEffectiveSipAccount();

            //Get user phonecalls from the session
            //Handle user delegee mode and normal user mode
            userSessionPhoneCalls = session.GetUserSessionPhoneCalls();

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
            session.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls, null, userSessionPhoneCallsPerPageJson);
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
            sipAccount = session.GetEffectiveSipAccount();


            //Get user phonecalls from the session
            //Handle user delegee mode and normal user mode
            userSessionPhoneCalls = session.GetUserSessionPhoneCalls();


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
            session.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls, null, userSessionPhoneCallsPerPageJson);
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
            List<PhoneCall> matchedDestinationCalls;
            List<PhoneBook> newOrUpdatedPhoneBookEntries = new List<PhoneBook>();

            //These would refer to either the the user's or the delegee's
            string userSessionPhoneCallsPerPageJson = string.Empty;
            List<PhoneCall> userSessionPhoneCalls = new List<PhoneCall>();
            Dictionary<string, PhoneBook> userSessionAddressBook = new Dictionary<string, PhoneBook>();
            
            //Get user session and effective sip account
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.GetEffectiveSipAccount();


            //Get user phoneCalls, addressbook, and phoneCallsPerPage;
            //Handle user delegee mode and normal user mode
            session.FetchSessionPhonecallsAndAddressbookData(out userSessionPhoneCalls, out userSessionAddressBook, out userSessionPhoneCallsPerPageJson);

            
            //Get the submitted grid data
            json = e.ExtraParams["Values"];
            settings.NullValueHandling = NullValueHandling.Ignore;
            
            selectiomModel = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);
            phoneCallsPerPage = JsonConvert.DeserializeObject<List<PhoneCall>>(userSessionPhoneCallsPerPageJson, settings);
            //userSessionPhoneCallsPerPageJson = json;

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

                matchedDestinationCalls = userSessionPhoneCalls.Where(
                    o => o.DestinationNumberUri == phoneCall.DestinationNumberUri && (string.IsNullOrEmpty(o.UI_CallType) || o.UI_CallType == "Business")
                ).ToList();
                

                foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                {
                    //if (matchedDestinationCall.UI_CallType == "Personal")
                    //    continue;

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

            //Add To Users Addressbook Store
            PhoneBook.AddOrUpdatePhoneBookEntries(sipAccount, newOrUpdatedPhoneBookEntries);

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            session.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls, userSessionAddressBook, userSessionPhoneCallsPerPageJson);
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
            sipAccount = session.GetEffectiveSipAccount();


            //Get user phoneCalls, addressbook, and phoneCallsPerPage;
            //Handle user delegee mode and normal user mode
            session.FetchSessionPhonecallsAndAddressbookData(out userSessionPhoneCalls, out userSessionAddressBook, out userSessionPhoneCallsPerPageJson);


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


                matchedDestinationCalls = userSessionPhoneCalls.Where(
                    o => o.DestinationNumberUri == phoneCall.DestinationNumberUri && (string.IsNullOrEmpty(o.UI_CallType) || o.UI_CallType == "Personal")
                ).ToList();

                foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                {
                    //if (matchedDestinationCall.UI_CallType == "Business")
                    //    continue;

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

            //Add To Users Addressbook Store
            PhoneBook.AddOrUpdatePhoneBookEntries(sipAccount, newOrUpdatedPhoneBookEntries);

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            session.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls, userSessionAddressBook, userSessionPhoneCallsPerPageJson);
        }

        protected void MoveToDepartmnent(object sender, DirectEventArgs e) 
        {
            PhoneCall sessionPhoneCallRecord;
            List<PhoneCall> submittedPhoneCalls;
            List<PhoneCall> userSessionPhoneCalls;
            string userSessionPhoneCallsPerPageJson = string.Empty;
            string userSiteDepartment = string.Empty;

            string json = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //Get the session and sip account of the current user
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.GetEffectiveSipAccount();

            //Get user phonecalls from the session
            //Handle user delegee mode and normal user mode
            userSessionPhoneCalls = session.GetUserSessionPhoneCalls();

            json = e.ExtraParams["Values"];
            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);
            userSessionPhoneCallsPerPageJson = json;


            foreach (PhoneCall phoneCall in submittedPhoneCalls)
            {
                sessionPhoneCallRecord = userSessionPhoneCalls.Where(o => o.SessionIdTime == phoneCall.SessionIdTime).First();

                if (sessionPhoneCallRecord.UI_AssignedToUser == sipAccount && !string.IsNullOrEmpty(sessionPhoneCallRecord.UI_AssignedByUser))
                {
                    userSiteDepartment = 
                        (session.ActiveRoleName == userDelegeeRoleName) ?
                        session.DelegeeAccount.DelegeeUserAccount.SiteName + "-" + session.DelegeeAccount.DelegeeUserAccount.Department :
                        session.NormalUserInfo.SiteName + "-" + session.NormalUserInfo.Department;  

                    sessionPhoneCallRecord.UI_AssignedToUser = userSiteDepartment;

                    PhoneCall.UpdatePhoneCall(sessionPhoneCallRecord, FORCE_RESET_UI_CallType: true);

                    ModelProxy model = PhoneCallsStore.Find(Enums.GetDescription(Enums.PhoneCalls.SessionIdTime), sessionPhoneCallRecord.SessionIdTime.ToString());
                    model.Set(sessionPhoneCallRecord);
                    model.Commit();

                    //Remove it from the PhoneCallsStore
                    PhoneCallsStore.Remove(model);

                    //Add it to the Department's PhoneCalls Store
                    DepartmentPhoneCallsStore.Add(phoneCall);

                    //Remove from the user own session PhoneCalls list.
                    userSessionPhoneCalls.Remove(sessionPhoneCallRecord);
                }
                else
                {
                    continue;
                }
            }

            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            PhoneCallsStore.LoadPage(1);

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            session.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls, null, null);
        }

        [DirectMethod]
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

                if (FilterTypeComboBox.SelectedItem.Value == "Personal")
                {
                    AllocatePhonecallsAsPersonal.Disabled = true;
                    AllocateDestinationsAsAlwaysPersonal.Disabled = true;

                    AllocatePhonecallsAsDispute.Disabled = false;
                    AllocatePhonecallsAsBusiness.Disabled = false;
                    AllocateDestinationsAsAlwaysBusiness.Disabled = false;
                }

                if (FilterTypeComboBox.SelectedItem.Value == "Business")
                {
                    AllocatePhonecallsAsBusiness.Disabled = true;
                    AllocateDestinationsAsAlwaysBusiness.Disabled = true;

                    AllocatePhonecallsAsDispute.Disabled = false;
                    AllocatePhonecallsAsPersonal.Disabled = false;
                    AllocateDestinationsAsAlwaysPersonal.Disabled = false;
                }

                if (FilterTypeComboBox.SelectedItem.Value == "Disputed")
                {
                    AllocatePhonecallsAsDispute.Disabled = true;

                    AllocatePhonecallsAsBusiness.Disabled = false;
                    AllocatePhonecallsAsPersonal.Disabled = false;
                    AllocateDestinationsAsAlwaysBusiness.Disabled = false;
                    AllocateDestinationsAsAlwaysPersonal.Disabled = false;
                }
            }
            else
            {
                PhoneBookNameEditorTextbox.ReadOnly = false;

                AllocatePhonecallsAsDispute.Disabled = false;
                AllocatePhonecallsAsBusiness.Disabled = false;
                AllocatePhonecallsAsPersonal.Disabled = false;

                AllocateDestinationsAsAlwaysPersonal.Disabled = false;
                AllocateDestinationsAsAlwaysBusiness.Disabled = false;
            }

            PhoneCallsStore.LoadPage(1);
        }

        [DirectMethod]
        protected void ShowUserHelpPanel(object sender, DirectEventArgs e)
        {
            this.UserHelpPanel.Show();
        }

        [DirectMethod]
        protected void AssignSelectedPhonecallsToMe_DirectEvent(object sender, DirectEventArgs e)
        {
            List<PhoneCall> submittedPhoneCalls;
            List<PhoneCall> userSessionPhoneCalls;
            string userSessionPhoneCallsPerPageJson = string.Empty;

            string json = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //Get the session and sip account of the current user
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.GetEffectiveSipAccount();

            //Get user phonecalls from the session
            //Handle user delegee mode and normal user mode
            userSessionPhoneCalls = session.GetUserSessionPhoneCalls();

            json = e.ExtraParams["Values"];
            //userSessionPhoneCallsPerPageJson = json;
            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);


            foreach (PhoneCall phoneCall in submittedPhoneCalls)
            {
                //sessionPhoneCallRecord = userSessionPhoneCalls.Where(o => o.SessionIdTime == phoneCall.SessionIdTime).First();

                //Assign the call to this user
                phoneCall.UI_AssignedToUser = sipAccount;

                //Update this phonecall in the database
                PhoneCall.UpdatePhoneCall(phoneCall, FORCE_RESET_UI_CallType: true);

                //Commit the changes to the grid and it's store
                ModelProxy model = DepartmentPhoneCallsStore.Find(Enums.GetDescription(Enums.PhoneCalls.SessionIdTime), phoneCall.SessionIdTime.ToString());
                model.SetDirty();
                model.Commit();

                //Remove from the Department's PhoneCalls Store
                DepartmentPhoneCallsStore.Remove(model);

                //Add it to the phonecalls store
                PhoneCallsStore.Add(phoneCall);

                //Add this new phonecall to the user session
                userSessionPhoneCalls.Add(phoneCall);
            }

            //Reload the department phonecalls grid
            DepartmentPhoneCallsGrid.GetSelectionModel().DeselectAll();
            
            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            session.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls, null, null);
        }

        protected void PhoneCallsGridSelectDirectEvents(object sender, DirectEventArgs e)
        {
            string json = string.Empty;
            List<PhoneCall> submittedPhoneCalls;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();


            json = e.ExtraParams["Values"];
            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);
            
            var result = submittedPhoneCalls.Where(item => !string.IsNullOrEmpty(item.UI_AssignedByUser)).ToList();

            if (submittedPhoneCalls.Count > 0 && submittedPhoneCalls.Count == result.Count)
                MoveToDepartmnet.Disabled = false;
            else
                MoveToDepartmnet.Disabled = true;

        }

    }
}