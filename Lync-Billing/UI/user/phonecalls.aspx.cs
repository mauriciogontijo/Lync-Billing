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
using Ext.Net;
using Lync_Billing.DB;
using Lync_Billing.Libs;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Lync_Billing.ui.user
{
    public partial class phonecalls : System.Web.UI.Page
    {
        /// <summary>
        /// This is working with single Grid without tabed panels
        /// </summary>

        private Dictionary<string, object> wherePart = new Dictionary<string, object>();
        private List<string> columns = new List<string>();
        private List<PhoneCall> AutoMarkedPhoneCalls = new List<PhoneCall>();

        private StoreReadDataEventArgs e;

        private string sipAccount = string.Empty;
        private string pageData = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/user/phonecalls.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).SipAccount;
            ((UserSession)HttpContext.Current.Session.Contents["UserData"]).phoneBook = PhoneBook.GetAddressBook(sipAccount);
        }

        protected void getPhoneCalls(bool force = false)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            if (userSession.PhoneCalls == null || userSession.PhoneCalls.Count == 0 || force == true)
            {
                wherePart.Add("SourceUserUri", userSession.SipAccount);
                wherePart.Add("marker_CallTypeID", 1);
                wherePart.Add("ac_IsInvoiced", "NO");

                columns.Add("SessionIdTime");
                columns.Add("SessionIdSeq");
                columns.Add("ResponseTime");
                columns.Add("SessionEndTime");
                columns.Add("marker_CallToCountry");
                columns.Add("DestinationNumberUri");
                columns.Add("Duration");
                columns.Add("marker_CallCost");
                columns.Add("ui_CallType");
                columns.Add("ui_MarkedOn");

                userSession.PhoneCalls = PhoneCall.GetPhoneCalls(columns, wherePart, 0);
            }
        }

        protected void PhoneCallsDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["start"] = this.e.Start;
            e.InputParameters["limit"] = this.e.Limit;
            e.InputParameters["sort"] = this.e.Sort[0];
        }

        protected void PhoneCallsDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            (this.PhoneCallsStore.Proxy[0] as PageProxy).Total = (int)e.OutputParameters["count"];
        }

        protected void PhoneCallsStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            XmlNode xml = e.Xml;

            this.Response.Clear();
            this.Response.ContentType = "application/vnd.ms-excel";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
            XslCompiledTransform xtExcel = new XslCompiledTransform();
            xtExcel.Load(Server.MapPath("~/Resources/Excel.xsl"));
            xtExcel.Transform(xml, null, Response.OutputStream);

            this.Response.End();
        }

        protected void PhoneCallsStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            this.e = e;
            this.PhoneCallsStore.DataBind();
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
            userSession.PhoneCallsPerPage = PhoneCallsStore.JsonData;

        }

        protected void AssignBusiness(object sender, DirectEventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];

            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);
            JsonSerializerSettings settings = new JsonSerializerSettings();

            settings.NullValueHandling = NullValueHandling.Ignore;

            perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(userSession.PhoneCallsPerPage, settings);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                phoneCall.UI_CallType = "Business";
                phoneCall.UI_MarkedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
        }

        protected void AssignPersonal(object sender, DirectEventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];


            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);
            perPagePhoneCalls = serializer.Deserialize<List<PhoneCall>>(userSession.PhoneCallsPerPage);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                phoneCall.UI_CallType = "Personal";
                phoneCall.UI_MarkedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
        }

        protected void AssignAlwaysBusiness(object sender, DirectEventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];

            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(userSession.PhoneCallsPerPage, settings);
            //perPagePhoneCalls = serializer.Deserialize<List<PhoneCall>>(userSession.PhoneCallsPerPage);

            PhoneBook phoneBookEntry;

            List<PhoneBook> phoneBookEntries = new List<PhoneBook>();

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                //Ceare Phonebook Entry
                phoneBookEntry = new PhoneBook();

                //Check if this entry Already exists 
                if (!userSession.phoneBook.ContainsKey(phoneCall.DestinationNumberUri))
                {
                    phoneBookEntry.DestinationCountry = phoneCall.Marker_CallToCountry;
                    phoneBookEntry.DestinationNumber = phoneCall.DestinationNumberUri;
                    phoneBookEntry.SipAccount = userSession.SipAccount;
                    phoneBookEntry.Type = "Business";

                    //Add Phonebook entry to Session and to the list which will be written to database 
                    userSession.phoneBook.Add(phoneCall.DestinationNumberUri, phoneBookEntry);
                    phoneBookEntries.Add(phoneBookEntry);
                }

                var matchedDestinationCalls = userSession.PhoneCalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);

                foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                {
                    matchedDestinationCall.UI_CallType = "Business";
                    matchedDestinationCall.UI_MarkedOn = DateTime.Now;
                    matchedDestinationCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;

                    PhoneCall.UpdatePhoneCall(matchedDestinationCall);

                    if (perPagePhoneCalls.Find(x => x.SessionIdTime == matchedDestinationCall.SessionIdTime) != null)
                    {
                        PhoneCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Set(matchedDestinationCall);
                        PhoneCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Commit();
                    }
                }
            }
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            getPhoneCalls(true);

            //Add To User PhoneBook Store
            PhoneBook.AddPhoneBookEntries(phoneBookEntries);
        }

        protected void AssignAlwaysPersonal(object sender, DirectEventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];


            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(userSession.PhoneCallsPerPage, settings);
            //perPagePhoneCalls = serializer.Deserialize<List<PhoneCall>>(userSession.PhoneCallsPerPage);

            PhoneBook phoneBookEntry;

            List<PhoneBook> phoneBookEntries = new List<PhoneBook>();

            foreach (PhoneCall phoneCall in phoneCalls)
            {

                //Ceare Phonebook Entry
                phoneBookEntry = new PhoneBook();

                //Check if this entry Already exists 
                if (!userSession.phoneBook.ContainsKey(phoneCall.DestinationNumberUri))
                {
                    phoneBookEntry.DestinationCountry = phoneCall.Marker_CallToCountry;
                    phoneBookEntry.DestinationNumber = phoneCall.DestinationNumberUri;
                    phoneBookEntry.SipAccount = userSession.SipAccount;
                    phoneBookEntry.Type = "Personal";

                    //Add Phonebook entry to Session and to the list which will be written to database 
                    userSession.phoneBook.Add(phoneCall.DestinationNumberUri, phoneBookEntry);
                    phoneBookEntries.Add(phoneBookEntry);
                }

                var matchedDestinationCalls = userSession.PhoneCalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);

                foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                {
                    matchedDestinationCall.UI_CallType = "Personal";
                    matchedDestinationCall.UI_MarkedOn = DateTime.Now;
                    matchedDestinationCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;

                    PhoneCall.UpdatePhoneCall(matchedDestinationCall);

                    if (perPagePhoneCalls.Find(x => x.SessionIdTime == matchedDestinationCall.SessionIdTime) != null)
                    {
                        PhoneCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Set(matchedDestinationCall);
                        PhoneCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Commit();
                    }
                }
            }
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            getPhoneCalls(true);

            //Add To User PhoneBook Store
            PhoneBook.AddPhoneBookEntries(phoneBookEntries);
        }

        protected void AssignDispute(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                phoneCall.UI_CallType = "Dispute";
                phoneCall.UI_MarkedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                ManagePhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
        }

        protected void AssignAllBusiness(object sender, DirectEventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];

            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(userSession.PhoneCallsPerPage, settings);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                var matchedDestinationCalls = userSession.PhoneCalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);

                foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                {
                    matchedDestinationCall.UI_CallType = "Business";
                    matchedDestinationCall.UI_MarkedOn = DateTime.Now;
                    matchedDestinationCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;

                    PhoneCall.UpdatePhoneCall(matchedDestinationCall);

                    if (perPagePhoneCalls.Find(x => x.SessionIdTime == matchedDestinationCall.SessionIdTime) != null)
                    {
                        PhoneCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Set(matchedDestinationCall);
                        PhoneCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Commit();
                    }
                }
            }
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            getPhoneCalls(true);
        }

        protected void AssignAllPersonal(object sender, DirectEventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            RowSelectionModel sm = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];

            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(userSession.PhoneCallsPerPage, settings);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                var matchedDestinationCalls = userSession.PhoneCalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);

                foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                {
                    matchedDestinationCall.UI_CallType = "Personal";
                    matchedDestinationCall.UI_MarkedOn = DateTime.Now;
                    matchedDestinationCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;

                    PhoneCall.UpdatePhoneCall(matchedDestinationCall);

                    if (perPagePhoneCalls.Find(x => x.SessionIdTime == matchedDestinationCall.SessionIdTime) != null)
                    {
                        PhoneCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Set(matchedDestinationCall);
                        PhoneCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Commit();
                    }
                }
            }
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            getPhoneCalls(true);
        }

        public List<PhoneCall> GetPhoneCallsFilter(int start, int limit, DataSorter sort, out int count)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
            getPhoneCalls();

            IQueryable<PhoneCall> result = userSession.PhoneCalls.Select(e => e).AsQueryable();

            if (sort != null)
            {
                ParameterExpression param = Expression.Parameter(typeof(PhoneCall), "e");

                Expression<Func<PhoneCall, object>> sortExpression = Expression.Lambda<Func<PhoneCall, object>>(Expression.Property(param, sort.Property), param);
                if (sort.Direction == Ext.Net.SortDirection.DESC)
                    result = result.OrderByDescending(sortExpression);
                else
                    result = result.OrderBy(sortExpression);
            }

            if (start >= 0 && limit > 0)
                result = result.Skip(start).Take(limit);

            PhoneBook phoneBookentry;

            foreach (PhoneCall phoneCall in result)
            {
                phoneBookentry = new PhoneBook();
                phoneBookentry = GetUserNameByNumber(phoneCall.DestinationNumberUri);

                if (phoneBookentry != null)
                {
                    phoneCall.PhoneBookName = phoneBookentry.Name;

                    if (phoneCall.UI_CallType == null)
                    {
                        phoneCall.UI_CallType = phoneBookentry.Type;
                        phoneCall.UI_MarkedOn = DateTime.Now;

                        AutoMarkedPhoneCalls.Add(phoneCall);
                    }
                }
                else
                {
                    phoneCall.PhoneBookName = "N/A";
                }
            }
            count = userSession.PhoneCalls.Count();

            return result.ToList();
        }

        private PhoneBook GetUserNameByNumber(string phoneNumber)
        {
            UserSession userSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            if (userSession.phoneBook.Count > 0)
            {
                if (userSession.phoneBook.ContainsKey(phoneNumber))
                    return userSession.phoneBook[phoneNumber];
                else
                    return null;
            }
            else
                return null;
        }
    }
}