﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Xsl;
using Ext.Net;
using Lync_Billing.DB;
using Lync_Billing.Libs;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.Xml.Serialization;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;


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

            sipAccount = session.EffectiveSipAccount;
            ((UserSession)HttpContext.Current.Session.Contents["UserData"]).PhoneBook = PhoneBook.GetAddressBook(sipAccount);
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

        protected void getPhoneCalls(bool force = false)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.EffectiveSipAccount;

            if (session.PhoneCalls == null || session.PhoneCalls.Count == 0 || force == true)
            {
                var userPhoneCalls = PhoneCall.GetPhoneCalls(sipAccount).Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null);
                
                foreach (var phoneCall in userPhoneCalls)
                {
                    if (session.PhoneBook.ContainsKey(phoneCall.DestinationNumberUri))
                    {
                        phoneCall.PhoneBookName = ((PhoneBook)session.PhoneBook[phoneCall.DestinationNumberUri]).Name;
                    }
                }

                session.PhoneCalls = userPhoneCalls.ToList();

                xmldoc = HelperFunctions.SerializeObject<List<PhoneCall>>(session.PhoneCalls);
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
            string format = this.FormatType.Value.ToString();

            XmlNode xml = e.Xml;

            this.Response.Clear();

            switch (format)
            {
                case "xls":
                    this.Response.Clear();
                    this.Response.ContentType = "application/vnd.ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
                    XslCompiledTransform xtExcel = new XslCompiledTransform();
                    xtExcel.Load(Server.MapPath("~/Resources/Excel.xsl"));
                    xtExcel.Transform(xml, null, Response.OutputStream);

                    break;

                case "pdf":
                    session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
                    sipAccount = session.EffectiveSipAccount;

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=TestPage.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    
                    Dictionary<string, string> headers = new Dictionary<string,string>()
                    {
                        {"title", session.EffectiveDisplayName.ToString() + "(#" + session.EmployeeID.ToString() + ")" },
                        {"subTitle", "The List of Uninvoiced Phone Calls"}
                    };

                    Document doc = new Document();
                    //PhoneCall.ExportUserPhoneCalls(sipAccount, Response, out doc, headers);
                    
                    Response.Write(doc);

                    break;
            }

            this.Response.End();
        }

        protected void PhoneCallsStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            this.e = e;
            PhoneCallsStore.DataBind();
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            session.PhoneCallsPerPage = PhoneCallsStore.JsonData;
        }

        public List<PhoneCall> GetPhoneCallsFilter(int start, int limit, DataSorter sort, out int count, DataFilter filter)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            getPhoneCalls();

            IEnumerable<PhoneCall> result;
 
            if(filter == null)
                result = session.PhoneCalls.Where(phoneCall => phoneCall.UI_CallType == null).AsQueryable();
            else
                result = session.PhoneCalls.Where(phoneCall => phoneCall.UI_CallType == filter.Value).AsQueryable();
         
            if (sort != null)
            {
                ParameterExpression param = Expression.Parameter(typeof(PhoneCall), "e");

                //Expression<Func<PhoneCall, object>> sortExpression = Expression.Lambda<Func<PhoneCall, object>>(Expression.Property(param, sort.Property), param);

               var p = Expression.Parameter(typeof(PhoneCall));
               var sortExpression = Expression.Lambda<Func<PhoneCall, object>>(Expression.TypeAs(Expression.Property(p, sort.Property), typeof(object)), p).Compile();

                if (sort.Direction == Ext.Net.SortDirection.DESC)
                    result = result.OrderByDescending(sortExpression);
                else
                    result = result.OrderBy(sortExpression);
            }

            int resultCount = result.Count();

            if (start >= 0 && limit > 0)
                result = result.Skip(start).Take(limit);

            count = resultCount;
            
            return result.ToList();
        }

        protected void AssignAllPersonal(object sender, DirectEventArgs e)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.EffectiveSipAccount;

            RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];

            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(session.PhoneCallsPerPage, settings);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                var matchedDestinationCalls = session.PhoneCalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);

                foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                {
                    if (matchedDestinationCall.UI_CallType == "Personal")
                        break;

                    matchedDestinationCall.UI_CallType = "Personal";
                    
                    matchedDestinationCall.UI_MarkedOn = DateTime.Now;
                    matchedDestinationCall.UI_UpdatedByUser = sipAccount;

                    PhoneCall.UpdatePhoneCall(matchedDestinationCall);
                }
            }
            
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            PhoneCallsStore.LoadPage(1);
        }

        protected void AssignAllBusiness(object sender, DirectEventArgs e)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.EffectiveSipAccount;

            RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];

            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(session.PhoneCallsPerPage, settings);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                var matchedDestinationCalls = session.PhoneCalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);

                foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                {
                    if (matchedDestinationCall.UI_CallType == "Business")
                        break;

                    matchedDestinationCall.UI_CallType = "Business";
                    matchedDestinationCall.UI_MarkedOn = DateTime.Now;
                    matchedDestinationCall.UI_UpdatedByUser = sipAccount;

                    PhoneCall.UpdatePhoneCall(matchedDestinationCall);
                }
            }
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            PhoneCallsStore.LoadPage(1);
        }

        protected void AssignPersonal(object sender, DirectEventArgs e) 
        {
            //Get the session and sip account of the current user
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.EffectiveSipAccount;

            //Initialize data cotnainers
            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string json = e.ExtraParams["Values"];
            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                PhoneCall sessionPhoneCallRecord = session.PhoneCalls.Where(o => o.SessionIdTime == phoneCall.SessionIdTime).First();

                sessionPhoneCallRecord.UI_CallType = "Personal";
                sessionPhoneCallRecord.UI_MarkedOn = DateTime.Now;
                sessionPhoneCallRecord.UI_UpdatedByUser = sipAccount;

                PhoneCall.UpdatePhoneCall(sessionPhoneCallRecord);

                ModelProxy model = PhoneCallsStore.Find("SessionIdTime", sessionPhoneCallRecord.SessionIdTime.ToString());
                model.Set(sessionPhoneCallRecord);
                model.Commit();
            }

            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            PhoneCallsStore.LoadPage(1);
        }

        protected void AssignBusiness(object sender, DirectEventArgs e) 
        {
            //Get the session and sip account of the current user
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.EffectiveSipAccount;

            //Initialize data cotnainers
            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            
            string json = e.ExtraParams["Values"];
            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                PhoneCall sessionPhoneCallRecord = session.PhoneCalls.Where(o => o.SessionIdTime == phoneCall.SessionIdTime).First();

                sessionPhoneCallRecord.UI_CallType = "Business";
                sessionPhoneCallRecord.UI_MarkedOn = DateTime.Now;
                sessionPhoneCallRecord.UI_UpdatedByUser = sipAccount;

                PhoneCall.UpdatePhoneCall(sessionPhoneCallRecord);

                ModelProxy model = PhoneCallsStore.Find("SessionIdTime", sessionPhoneCallRecord.SessionIdTime.ToString());
                model.Set(sessionPhoneCallRecord);
                model.Commit();
            }

            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            PhoneCallsStore.LoadPage(1);
        }

        protected void AssignDispute(object sender, DirectEventArgs e)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;

            RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];

            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(session.PhoneCallsPerPage, settings);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                PhoneCall matchedDestinationCalls = session.PhoneCalls.Where(o => o.SessionIdTime == phoneCall.SessionIdTime).First();


                matchedDestinationCalls.UI_CallType = "Disputed";
                matchedDestinationCalls.UI_MarkedOn = DateTime.Now;
                matchedDestinationCalls.UI_UpdatedByUser = sipAccount;

                PhoneCall.UpdatePhoneCall(matchedDestinationCalls);

                ModelProxy model = PhoneCallsStore.Find("SessionIdTime", matchedDestinationCalls.SessionIdTime.ToString());
                model.Set(matchedDestinationCalls);
                model.Commit();

            }

            PhoneCallsAllocationToolsMenu.Hide();
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            PhoneCallsStore.LoadPage(1);
        }

        protected void AssignAlwaysPersonal(object sender, DirectEventArgs e)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.EffectiveSipAccount;

            RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];

            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(session.PhoneCallsPerPage, settings);
            //perPagePhoneCalls = serializer.Deserialize<List<PhoneCall>>(session.PhoneCallsPerPage);

            PhoneBook phoneBookEntry;

            List<PhoneBook> phoneBookEntries = new List<PhoneBook>();

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                //Ceare Phonebook Entry
                phoneBookEntry = new PhoneBook();

                //Check if this entry Already exists by either destination number and destination name (in case it's edited)
                bool found =    session.PhoneBook.ContainsKey(phoneCall.DestinationNumberUri) &&
                                (session.PhoneBook.Values.Select(phoneBookContact => phoneBookContact.Name == phoneCall.PhoneBookName) == null ? false : true);

                if (!found)
                {
                    phoneBookEntry.Name = phoneCall.PhoneBookName ?? string.Empty;
                    phoneBookEntry.DestinationCountry = phoneCall.Marker_CallToCountry;
                    phoneBookEntry.DestinationNumber = phoneCall.DestinationNumberUri;
                    phoneBookEntry.SipAccount = session.EffectiveSipAccount;
                    phoneBookEntry.Type = "Personal";

                    //Add Phonebook entry to Session and to the list which will be written to database 
                    if (session.PhoneBook.ContainsKey(phoneCall.DestinationNumberUri))
                        session.PhoneBook[phoneCall.DestinationNumberUri] = phoneBookEntry;
                    else
                        session.PhoneBook.Add(phoneCall.DestinationNumberUri, phoneBookEntry);

                    phoneBookEntries.Add(phoneBookEntry);
                }

                var matchedDestinationCalls = session.PhoneCalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);

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

            //Add To User PhoneBook Store
            PhoneBook.AddOrUpdatePhoneBookEntries(sipAccount, phoneBookEntries);
        }

        protected void AssignAlwaysBusiness(object sender, DirectEventArgs e)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.EffectiveSipAccount;

            RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];

            List<PhoneCall> phoneCalls = new List<PhoneCall>();                 
            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(session.PhoneCallsPerPage, settings);
            //perPagePhoneCalls = serializer.Deserialize<List<PhoneCall>>(session.PhoneCallsPerPage);

            PhoneBook phoneBookEntry;

            List<PhoneBook> phoneBookEntries = new List<PhoneBook>();

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                //Ceare Phonebook Entry
                phoneBookEntry = new PhoneBook();

                //Check if this entry Already exists by either destination number and destination name (in case it's edited)
                bool found = session.PhoneBook.ContainsKey(phoneCall.DestinationNumberUri) &&
                                (session.PhoneBook.Values.Select(phoneBookContact => phoneBookContact.Name == phoneCall.PhoneBookName) == null ? false : true);

                if (!found)
                {
                    phoneBookEntry.Name = phoneCall.PhoneBookName ?? string.Empty;
                    phoneBookEntry.DestinationCountry = phoneCall.Marker_CallToCountry;
                    phoneBookEntry.DestinationNumber = phoneCall.DestinationNumberUri;
                    phoneBookEntry.SipAccount = session.EffectiveSipAccount;
                    phoneBookEntry.Type = "Business";

                    //Add Phonebook entry to Session and to the list which will be written to database 
                    if (session.PhoneBook.ContainsKey(phoneCall.DestinationNumberUri))
                        session.PhoneBook[phoneCall.DestinationNumberUri] = phoneBookEntry;
                    else
                        session.PhoneBook.Add(phoneCall.DestinationNumberUri, phoneBookEntry);

                    phoneBookEntries.Add(phoneBookEntry);
                }

                var matchedDestinationCalls = session.PhoneCalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);

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

            //Add To User PhoneBook Store
            PhoneBook.AddOrUpdatePhoneBookEntries(sipAccount, phoneBookEntries);
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

            //Ext.Net.Panel multipleSelection = new Ext.Net.Panel("How do I select multiple Phonecalls?");
            //multipleSelection.Html = String.Format("<p class='font-12 p5'>You can select multiple phonecalls by pressing the [Ctrl] button.</p>");

            //Ext.Net.Panel callsAllocation = new Ext.Net.Panel("How do I allocate my Phonecalls?");
            //callsAllocation.Html = String.Format("<p class='font-12 p5'>You can allocate your phonecalls by [Right Clicking] on the selected phonecalls and choosing your preferred action.</p>");

            //******* APPROACH 1 ********/
            //UserHelpPanel.Items.Add(multipleSelection);
            //UserHelpPanel.Items.Add(callsAllocation);
            //this.UserHelpPanel.Show();

            /****** APPROACH 2 *******/
            //Window UserHelpWindow = new Window();

            //UserHelpWindow.Items.Add(multipleSelection);
            //UserHelpWindow.Items.Add(callsAllocation);

            //UserHelpWindow.ID = "UserHelpWindowID";
            //UserHelpWindow.Title = "Accordion Window";
            //UserHelpWindow.Width = Unit.Pixel(350);
            //UserHelpWindow.Height = Unit.Pixel(450);
            //UserHelpWindow.Maximizable = false;
            //UserHelpWindow.Icon = Icon.Help;
            //UserHelpWindow.BodyBorder = 0;
            //UserHelpWindow.Layout = "Accordion";
            //UserHelpWindow.Visible = true;
            //this.UserHelpPanelPlaceholder.Controls.Add(UserHelpWindow);
        }

    }
}