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
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.SiteDelegee);

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

                if (session.ActiveRoleName != allowedRoleName)
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=" + allowedRoleName);
                }
            }

            //sipAccount = session.EffectiveSipAccount;
            sipAccount = session.NormalUserInfo.SipAccount;

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

        private void BindDepartmentsForThisUser(bool alwaysFireSelect = false)
        {
            session = (UserSession)HttpContext.Current.Session.Contents["UserData"];
            sipAccount = session.NormalUserInfo.SipAccount;

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
            sipAccount = session.NormalUserInfo.SipAccount;

            if (session.Phonecalls == null || session.Phonecalls.Count == 0 || force == true)
            {
                session.Phonecalls = PhoneCall.GetPhoneCalls(sipAccount).Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null).ToList();
                xmldoc = HelperFunctions.SerializeObject<List<PhoneCall>>(session.Phonecalls);
            }
        }

        public List<PhoneCall> GetPhoneCallsFilter(int start, int limit, DataSorter sort, out int count, DataFilter filter)
        {
            session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            getPhoneCalls();

            IEnumerable<PhoneCall> result;

            if (filter == null)
                result = session.Phonecalls.Where(phoneCall => phoneCall.UI_CallType == null).AsQueryable();
            else
                result = session.Phonecalls.Where(phoneCall => phoneCall.UI_CallType == filter.Value).AsQueryable();

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
            session.PhonecallsPerPage = PhoneCallsStore.JsonData;
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