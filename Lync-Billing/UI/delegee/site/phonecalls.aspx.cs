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

using Lync_Billing.DB;
using Lync_Billing.Libs;
using Lync_Billing.DB.Roles;

namespace Lync_Billing.ui.delegee.site
{
    public partial class phonecalls : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string normalUserRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        private string siteDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SiteDelegee);

        private List<Department> SiteDelegeeDepartments = new List<Department>();

        private List<PhoneCall> AutoMarkedPhoneCalls = new List<PhoneCall>();
        private string pageData = string.Empty;
        private StoreReadDataEventArgs e;

        string xmldoc = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                //Don't enable the redirection process because the site name will be missing!
                //string redirect_to = @"~/ui/delegee/site.aspx";
                //string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;

                string url = @"~/ui/session/login.aspx";
                Response.Redirect(url);
            }
            else
            {
                session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                if (session.ActiveRoleName != siteDelegeeRoleName)
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=" + siteDelegeeRoleName);
                }
            }

            //sipAccount = GetEffectiveSipAccount();
            sipAccount = session.DelegeeAccount.DelegeeUserAccount.SipAccount;

            /***
             * Thie following solves the issue of infinitely looping like: Page_Load--->BindDepartmentsForThisUser--->DrawStatisticsForDepartment
             * This would happen due to a chain-reaction of triggering two DirectEvents from the aspx page
             * The chain consist of two DirectEvents: OnDepartmentSelect X---FIRE---> DrawStatisticsForDepartment which fires Page_Load and then it goes again for ever.
             * */
            if (!Ext.Net.X.IsAjaxRequest)
            {
                BindDepartmentsForThisUser(true);
            }
            else
            {
                BindDepartmentsForThisUser(false);
            }
        }

        //Get the user sipaccount.
        private string GetEffectiveSipAccount()
        {
            string userSipAccount = string.Empty;
            session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            //if the user is a user-delegee return the delegate sipaccount.
            if (session.ActiveRoleName == siteDelegeeRoleName)
            {
                userSipAccount = session.DelegeeAccount.DelegeeUserAccount.SipAccount;
            }
            else
            {
                userSipAccount = session.NormalUserInfo.SipAccount;
            }

            return userSipAccount;
        }

        //Get the user session phonecalls
        //Handle normal user mode and user delegee mode
        private List<PhoneCall> GetUserSessionPhoneCalls(bool force = false)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.DelegeeAccount.DelegeeUserAccount.SipAccount;

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

        private void BindDepartmentsForThisUser(bool alwaysFireSelect = false)
        {
            session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            //If the current user is a system-developer, give him access to all the departments, otherwise grant him access to his/her own managed department
            if (session.IsDeveloper)
                SiteDelegeeDepartments = Department.GetAllDepartments();
            else
                SiteDelegeeDepartments = Department.GetDepartmentsInSite(session.DelegeeAccount.DelegeeSiteAccount.SiteID);


            //By default the filter combobox is not read only
            DepartmentsListComboBox.ReadOnly = false;

            if (SiteDelegeeDepartments.Count > 0)
            {
                //Handle the FireSelect event
                if (alwaysFireSelect == true)
                {
                    DepartmentsListComboBox.SetValueAndFireSelect(SiteDelegeeDepartments.First().DepartmentID);

                    //Handle the ReadOnly Property
                    if (SiteDelegeeDepartments.Count == 1)
                    {
                        DepartmentsListComboBox.ReadOnly = true;
                    }
                }

                //Bind all the Data and return to the view
                DepartmentsListComboBox.GetStore().DataSource = SiteDelegeeDepartments;
                DepartmentsListComboBox.GetStore().DataBind();
            }
            //in case there are no longer any departments for this user to monitor.
            else
            {
                DepartmentsListComboBox.Disabled = true;
            }
        }

        protected void getPhoneCalls(bool force = false)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            //sipAccount = session.EffectiveSipAccount;
           

            if (session.Phonecalls == null || session.Phonecalls.Count == 0 || force == true)
            {
                session.Phonecalls = PhoneCall.GetPhoneCalls(sipAccount).Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null).ToList();
                xmldoc = HelperFunctions.SerializeObject<List<PhoneCall>>(session.Phonecalls);
            }
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
            if (filter == null)
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

        [DirectMethod]
        protected void PhoneCallsTypeFilter(object sender, DirectEventArgs e)
        {
            PhoneCallsStore.ClearFilter();

            if (FilterTypeComboBox.SelectedItem.Value != "Unassigned")
            {
                PhoneCallsStore.Filter(Enums.GetDescription(Enums.PhoneCalls.UI_AssignedByUser), sipAccount);
                PhoneBookNameEditorTextbox.ReadOnly = true;
            }
            else
            {
                PhoneBookNameEditorTextbox.ReadOnly = false;
            }

            PhoneCallsStore.LoadPage(1);
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

        protected void PhoneCallsStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            this.e = e;
            PhoneCallsStore.DataBind();
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            //session.PhonecallsPerPage = PhoneCallsStore.JsonData;
            session.DelegeeAccount.DelegeeUserPhonecallsPerPage = PhoneCallsStore.JsonData;
        }

        [DirectMethod]
        protected void ShowUserHelpPanel(object sender, DirectEventArgs e)
        {
            this.UserHelpPanel.Show();
        }

        [DirectMethod]
        protected void AssignToUser(object sender, DirectEventArgs e)
        {

        }

        [DirectMethod]
        protected void AssignToDepartment(object sender, DirectEventArgs e)
        {

        }

        [DirectMethod]
        protected void RejectChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            ManagePhoneCallsGrid.GetStore().RejectChanges();
        }

    }
}