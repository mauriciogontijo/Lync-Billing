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
        private List<PhoneCall> GetDepartmentPhoneCalls(bool force = false)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = session.DelegeeAccount.DelegeeUserAccount.SipAccount;

            if (session.DelegeeAccount.DelegeeUserPhonecalls == null || session.DelegeeAccount.DelegeeUserPhonecalls.Count == 0 || force == true)
            {
                var userPhoneCalls = PhoneCall.GetPhoneCalls(sipAccount).Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null);

                session.DelegeeAccount.DelegeeUserPhonecalls = userPhoneCalls.ToList();

                xmldoc = HelperFunctions.SerializeObject<List<PhoneCall>>(session.DelegeeAccount.DelegeeUserPhonecalls);
            }

            return session.DelegeeAccount.DelegeeUserPhonecalls;
        }

        private void BindDepartmentsForThisUser(bool alwaysFireSelect = false)
        {
            session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            //If the current user is a system-developer, give him access to all the departments, otherwise grant him access to his/her own managed department
            if (SiteDelegeeDepartments == null || SiteDelegeeDepartments.Count == 0)
            {
                SiteDelegeeDepartments = Department.GetDepartmentsInSite(session.DelegeeAccount.DelegeeSiteAccount.SiteID);
            }


            //By default the filter combobox is not read only
            DepartmentsListComboBox.ReadOnly = false;

            if (SiteDelegeeDepartments.Count > 0)
            {
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


        public List<PhoneCall> GetPhoneCallsFilter(int start, int limit, DataSorter sort, out int count, DataFilter filter)
        {
            List<PhoneCall> userSessionPhoneCalls;
            IEnumerable<PhoneCall> filteredPhoneCalls;
            int filteredPhoneCallsCount;

            //Get use session and user phonecalls list.
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

            //Get user session phonecalls; handle normal user mode and delegee mode
            userSessionPhoneCalls = GetDepartmentPhoneCalls();


            //Begin the filtering process

            if (filter == null || filter.Property == Enums.GetDescription(Enums.PhoneCalls.UI_AssignedByUser))
            {
                filteredPhoneCalls = userSessionPhoneCalls.Where(phoneCall => phoneCall.ChargingParty == sipAccount).AsQueryable();
            }
            else
            {
                filteredPhoneCalls = userSessionPhoneCalls.Where(phoneCall => phoneCall.UI_AssignedByUser != sipAccount).AsQueryable();
            }
          

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

            if (FilterTypeComboBox.SelectedItem.Value == "Unassigned")
            {
                PhoneCallsStore.Filter("UI_AssignedByUser", null);
            }
            else 
            {
                PhoneCallsStore.Filter("UI_AssignedByUser", sipAccount);
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
        protected void RejectChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            ManagePhoneCallsGrid.GetStore().RejectChanges();
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
            Department selectedDepartment;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //Get the session and sip account of the current user
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

            //Get user phonecalls from the session
            //Handle user delegee mode and normal user mode
            departmentPhoneCalls = GetDepartmentPhoneCalls();

            json = e.ExtraParams["Values"];
            selectedDepartmentId = Convert.ToInt32(e.ExtraParams["selectedDepartment"]);
            selectedDepartment = Department.GetDepartment(selectedDepartmentId);

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
                
            }

            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            PhoneCallsStore.LoadPage(1);

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            session.DelegeeAccount.DelegeeUserPhonecalls = departmentPhoneCalls;
        }

    }
}