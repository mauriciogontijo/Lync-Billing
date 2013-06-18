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

        protected void ManageMarkedCallsStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            this.e = e;
            this.ManageMarkedCallsStore.DataBind();
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
            userSession.MarkedPhoneCallsPerPage = ManageMarkedCallsStore.JsonData;
        }

        protected void ManageUnmarkedCallsStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            this.e = e;
            this.ManageUnmarkedCallsStore.DataBind();
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
            userSession.UnmarkedPhoneCallsPerPage = ManageUnmarkedCallsStore.JsonData;
        }

        protected void getMarkedPhoneCalls(bool force=false)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            if (userSession.MarkedPhoneCalls == null || userSession.MarkedPhoneCalls.Count == 0 || force == true)
            {
                wherePart.Add("SourceUserUri", userSession.SipAccount);
                wherePart.Add("marker_CallTypeID", 1);
                wherePart.Add("ac_IsInvoiced", "NO");
                wherePart.Add("ui_CallType", "!null");

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

                userSession.MarkedPhoneCalls = PhoneCall.GetPhoneCalls(columns, wherePart, 0);
            }
        }

        protected void getUnmarkedPhoneCalls(bool force = false)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            if (userSession.UnmarkedPhoneCalls == null || userSession.UnmarkedPhoneCalls.Count == 0 || force == true)
            {
                wherePart.Add("SourceUserUri", userSession.SipAccount);
                wherePart.Add("marker_CallTypeID", 1);
                wherePart.Add("ui_CallType",null);
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

                userSession.UnmarkedPhoneCalls = PhoneCall.GetPhoneCalls(columns, wherePart, 0);
            }
        }


        protected void MarkedPhoneCallsDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["start"] = this.e.Start;
            e.InputParameters["limit"] = this.e.Limit;
            e.InputParameters["sort"] = this.e.Sort[0];
        }

        protected void MarkedPhoneCallsDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            (this.ManageMarkedCallsStore.Proxy[0] as PageProxy).Total = (int)e.OutputParameters["count"];
        }

        protected void UnmarkedPhoneCallsDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["start"] = this.e.Start;
            e.InputParameters["limit"] = this.e.Limit;
            e.InputParameters["sort"] = this.e.Sort[0];
        }

        protected void UnmarkedPhoneCallsDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            (this.ManageUnmarkedCallsStore.Proxy[0] as PageProxy).Total = (int)e.OutputParameters["count"];
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

        protected void AssignBusiness(object sender, DirectEventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            RowSelectionModel sm = this.ManageUnmarkedCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];
            
            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);
            JsonSerializerSettings settings = new JsonSerializerSettings();

            settings.NullValueHandling = NullValueHandling.Ignore;

            perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(userSession.UnmarkedPhoneCallsPerPage, settings);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                phoneCall.UI_CallType = "Business";
                phoneCall.UI_MarkedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManageUnmarkedCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                ManageUnmarkedCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
            ManageUnmarkedCallsGrid.GetSelectionModel().DeselectAll();
        }

        protected void AssignPersonal(object sender, DirectEventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            RowSelectionModel sm = this.ManageUnmarkedCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];
            

            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);
            perPagePhoneCalls = serializer.Deserialize<List<PhoneCall>>(userSession.UnmarkedPhoneCallsPerPage);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                phoneCall.UI_CallType = "Personal";
                phoneCall.UI_MarkedOn = DateTime.Now;
                phoneCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;
                PhoneCall.UpdatePhoneCall(phoneCall);

                ManageUnmarkedCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                ManageUnmarkedCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();  
            }
            ManageUnmarkedCallsGrid.GetSelectionModel().DeselectAll();
        }

        protected void AssignAlwaysBusiness(object sender, DirectEventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            RowSelectionModel sm = this.ManageUnmarkedCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];


            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            if (e.ExtraParams["store"] == "marked")
            {
                perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(userSession.MarkedPhoneCallsPerPage, settings);
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

                    var matchedDestinationCalls = userSession.MarkedPhoneCalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);

                    foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                    {
                        matchedDestinationCall.UI_CallType = "Business";
                        matchedDestinationCall.UI_MarkedOn = DateTime.Now;
                        matchedDestinationCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;

                        PhoneCall.UpdatePhoneCall(matchedDestinationCall);

                        if (perPagePhoneCalls.Find(x => x.SessionIdTime == matchedDestinationCall.SessionIdTime) != null)
                        {
                            ManageMarkedCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Set(matchedDestinationCall);
                            ManageMarkedCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Commit();
                        }
                    }
                }
                ManageMarkedCallsGrid.GetSelectionModel().DeselectAll();
                getMarkedPhoneCalls(true);

                //Add To User PhoneBook Store
                PhoneBook.AddPhoneBookEntries(phoneBookEntries);
            }
        }

        protected void AssignAlwaysPersonal(object sender, DirectEventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            RowSelectionModel sm = this.ManageUnmarkedCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];


            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            if (e.ExtraParams["store"] == "marked")
            {
                perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(userSession.MarkedPhoneCallsPerPage, settings);
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

                    var matchedDestinationCalls = userSession.MarkedPhoneCalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);

                    foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                    {
                        matchedDestinationCall.UI_CallType = "Personal";
                        matchedDestinationCall.UI_MarkedOn = DateTime.Now;
                        matchedDestinationCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;

                        PhoneCall.UpdatePhoneCall(matchedDestinationCall);

                        if (perPagePhoneCalls.Find(x => x.SessionIdTime == matchedDestinationCall.SessionIdTime) != null)
                        {
                            ManageMarkedCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Set(matchedDestinationCall);
                            ManageMarkedCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Commit();
                        }
                    }
                }
                ManageMarkedCallsGrid.GetSelectionModel().DeselectAll();
                getMarkedPhoneCalls(true);

                //Add To User PhoneBook Store
                PhoneBook.AddPhoneBookEntries(phoneBookEntries);
            }
        }

        protected void AssignDispute(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = this.ManageUnmarkedCallsGrid.GetSelectionModel() as RowSelectionModel;

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

                ManageUnmarkedCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Set(phoneCall);
                ManageUnmarkedCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString()).Commit();
            }
            ManageUnmarkedCallsGrid.GetSelectionModel().DeselectAll();
        }

        protected void AssignAllBusiness(object sender, DirectEventArgs e)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            RowSelectionModel sm = this.ManageUnmarkedCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];

            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(userSession.UnmarkedPhoneCallsPerPage, settings);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                var matchedDestinationCalls = userSession.UnmarkedPhoneCalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);

                foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                {
                    matchedDestinationCall.UI_CallType = "Business";
                    matchedDestinationCall.UI_MarkedOn = DateTime.Now;
                    matchedDestinationCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;

                    PhoneCall.UpdatePhoneCall(matchedDestinationCall);

                    if (perPagePhoneCalls.Find(x => x.SessionIdTime == matchedDestinationCall.SessionIdTime) != null)
                    {
                        ManageUnmarkedCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Set(matchedDestinationCall);
                        ManageUnmarkedCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Commit();
                    }
                }
            }
            ManageUnmarkedCallsGrid.GetSelectionModel().DeselectAll();
            getMarkedPhoneCalls(true);
        }

        protected void AssignAllPersonal(object sender, DirectEventArgs e) 
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);

            RowSelectionModel sm = this.ManageUnmarkedCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];

            List<PhoneCall> phoneCalls = new List<PhoneCall>();
            List<PhoneCall> perPagePhoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            perPagePhoneCalls = JsonConvert.DeserializeObject<List<PhoneCall>>(userSession.UnmarkedPhoneCallsPerPage, settings);

            foreach (PhoneCall phoneCall in phoneCalls)
            {
                var matchedDestinationCalls = userSession.UnmarkedPhoneCalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri);

                foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                {
                    matchedDestinationCall.UI_CallType = "Personal";
                    matchedDestinationCall.UI_MarkedOn = DateTime.Now;
                    matchedDestinationCall.UI_UpdatedByUser = ((UserSession)Session.Contents["UserData"]).SipAccount;

                    PhoneCall.UpdatePhoneCall(matchedDestinationCall);

                    if (perPagePhoneCalls.Find(x => x.SessionIdTime == matchedDestinationCall.SessionIdTime) != null)
                    {
                        ManageUnmarkedCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Set(matchedDestinationCall);
                        ManageUnmarkedCallsStore.Find("SessionIdTime", matchedDestinationCall.SessionIdTime.ToString()).Commit();
                    }
                }
            }
            ManageUnmarkedCallsGrid.GetSelectionModel().DeselectAll();
            getMarkedPhoneCalls(true);
        }

        public List<PhoneCall> GetMarkedPhoneCallsFilter(int start, int limit, DataSorter sort, out int count)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
            getMarkedPhoneCalls();

            IQueryable<PhoneCall> result = userSession.MarkedPhoneCalls.Select(e => e).AsQueryable();

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
            count = userSession.MarkedPhoneCalls.Count();

            return result.ToList();
        }

        public List<PhoneCall> GetUnmarkedPhoneCallsFilter(int start, int limit, DataSorter sort, out int count)
        {
            UserSession userSession = ((UserSession)Session.Contents["UserData"]);
            getUnmarkedPhoneCalls();

            IQueryable<PhoneCall> result = userSession.UnmarkedPhoneCalls.Select(e => e).AsQueryable();

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
            count = userSession.UnmarkedPhoneCalls.Count();

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