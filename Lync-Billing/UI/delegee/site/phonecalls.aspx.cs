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

using Lync_Billing.Backend;
using Lync_Billing.Libs;
using Lync_Billing.Backend.Roles;

namespace Lync_Billing.ui.delegee.site
{
    public partial class phonecalls : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string siteDelegeeRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SiteDelegee);

        private List<SitesDepartments> SiteDelegeeDepartments = new List<SitesDepartments>();
        private StoreReadDataEventArgs e;
        private string xmldoc = string.Empty;


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
                BindDepartmentsForThisUser(true);
            else
                BindDepartmentsForThisUser(false);
        }

        private void BindDepartmentsForThisUser(bool alwaysFireSelect = false)
        {
            session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            //If the current user is a system-developer, give him access to all the AllDepartments, otherwise grant him access to his/her own managed department
            if (SiteDelegeeDepartments == null || SiteDelegeeDepartments.Count == 0)
            {
                SiteDelegeeDepartments = SitesDepartments.GetDepartmentsInSite(session.DelegeeAccount.DelegeeSiteAccount.SiteID); 
            }


            //By default the filter combobox is not read only
            DepartmentsListComboBox.ReadOnly = false;

            if (SiteDelegeeDepartments.Count > 0)
            {
                //Bind all the Data and return to the view
                DepartmentsListComboBox.GetStore().DataSource = SiteDelegeeDepartments;
                DepartmentsListComboBox.GetStore().DataBind();
            }
            //in case there are no longer any AllDepartments for this user to monitor.
            else
            {
                DepartmentsListComboBox.Disabled = true;
            }
        }

        protected void PhoneCallsStore_Load(object sender, EventArgs e)
        {
            ManagePhoneCallsGrid.GetStore().DataSource = session.GetUserSessionPhoneCalls();
            ManagePhoneCallsGrid.GetStore().DataBind();
        }

        protected void PhoneCallsStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            this.e = e;
            PhoneCallsStore.DataBind();
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            session.DelegeeAccount.DelegeeUserPhonecallsPerPage = PhoneCallsStore.JsonData;
        }

        [DirectMethod]
        protected void PhoneCallsTypeFilter(object sender, DirectEventArgs e)
        {
            //Clear the filter
            ManagePhoneCallsGrid.GetStore().ClearFilter();

            //Get the phonecalls from the session and bind a filtered version of the list to the store
            var sessionPhoneCalls = session.GetUserSessionPhoneCalls();

            if (FilterTypeComboBox.SelectedItem.Value == "Everything")
            {
                PhoneCallsStore.LoadData(sessionPhoneCalls);
            }
            else if (FilterTypeComboBox.SelectedItem.Value == "Assigned")
            {
                sessionPhoneCalls = sessionPhoneCalls.Where(phoneCall => phoneCall.UI_AssignedByUser == sipAccount && !string.IsNullOrEmpty(phoneCall.UI_AssignedToUser)).ToList();
                PhoneCallsStore.LoadData(sessionPhoneCalls);
            }
            else if (FilterTypeComboBox.SelectedItem.Value == "Unassigned")
            {
                sessionPhoneCalls = sessionPhoneCalls.Where(phoneCall => string.IsNullOrEmpty(phoneCall.UI_AssignedByUser) && string.IsNullOrEmpty(phoneCall.UI_AssignedToUser)).ToList();
                PhoneCallsStore.LoadData(sessionPhoneCalls);
            }

            // APPROACH 2 - Should work for server side pagination.
            //if (FilterTypeComboBox.SelectedItem.Value == "Everything")
            //{
            //    PhoneCallsStore.Filter("UI_AssignedByUser", null);
            //}
            //else if (FilterTypeComboBox.SelectedItem.Value == "Assigned")
            //{
            //    PhoneCallsStore.Filter("UI_AssignedByUser", "assigned");
            //}
            //else if (FilterTypeComboBox.SelectedItem.Value == "Unassigned")
            //{
            //    PhoneCallsStore.Filter("UI_AssignedByUser", "unassigned");
            //}

            //PhoneCallsStore.LoadPage(1);
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

        //Object Data Source Filter
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
            if (filter != null && filter.Value == "assigned")
                filteredPhoneCalls = userSessionPhoneCalls.Where(phoneCall => phoneCall.UI_AssignedByUser == sipAccount && !string.IsNullOrEmpty(phoneCall.UI_AssignedToUser)).AsQueryable();
            else if (filter != null && filter.Value == "unassigned")
                filteredPhoneCalls = userSessionPhoneCalls.Where(phoneCall => string.IsNullOrEmpty(phoneCall.UI_AssignedByUser) && string.IsNullOrEmpty(phoneCall.UI_AssignedToUser)).AsQueryable();
            else
                filteredPhoneCalls = userSessionPhoneCalls.AsQueryable();


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
        protected void ShowUserHelpPanel(object sender, DirectEventArgs e)
        {
            this.UserHelpPanel.Show();
        }

        [DirectMethod]
        protected void AssignSelectedPhoneCallsToDepartment(object sender, DirectEventArgs e)
        {
            PhoneCall sessionPhoneCallRecord;
            List<PhoneCall> submittedPhoneCalls;
            List<PhoneCall> departmentPhoneCalls;
            string userSessionPhoneCallsPerPageJson = string.Empty;

            string json = string.Empty;
           
            int selectedDepartmentId;
            SitesDepartments selectedDepartment;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //Get the session and sip account of the current user
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

            //Get user phonecalls from the session
            //Handle user delegee mode and normal user mode
            departmentPhoneCalls = session.GetUserSessionPhoneCalls();

            json = e.ExtraParams["Values"];
            selectedDepartmentId = Convert.ToInt32(e.ExtraParams["SelectedDepartment"]);
            selectedDepartment = SitesDepartments.GetSiteDepartment(selectedDepartmentId);

            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);
            userSessionPhoneCallsPerPageJson = json;

            foreach (PhoneCall phoneCall in submittedPhoneCalls)
            {
                sessionPhoneCallRecord = departmentPhoneCalls.Where(o => o.SessionIdTime == phoneCall.SessionIdTime).First();

                sessionPhoneCallRecord.UI_AssignedByUser = sipAccount;
                sessionPhoneCallRecord.UI_AssignedToUser = selectedDepartment.SiteName + "-" + selectedDepartment.DepartmentName;
                sessionPhoneCallRecord.UI_AssignedOn = DateTime.Now;

                PhoneCall.UpdatePhoneCall(sessionPhoneCallRecord);

                ModelProxy model = PhoneCallsStore.Find(Enums.GetDescription(Enums.PhoneCalls.SessionIdTime), sessionPhoneCallRecord.SessionIdTime.ToString());
                model.Set(sessionPhoneCallRecord);
                model.Commit();

                PhoneCallsStore.CommitChanges();
            }

            PhoneCallsAllocationToolsMenu.Hide();
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            PhoneCallsStore.LoadPage(1);

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            session.DelegeeAccount.DelegeeUserPhonecalls = departmentPhoneCalls;
        }

    }
}