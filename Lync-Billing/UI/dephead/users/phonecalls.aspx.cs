using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;

using Lync_Billing.Backend;
using Lync_Billing.Backend.Roles;

namespace Lync_Billing.ui.dephead.users
{
    public partial class phonecalls : System.Web.UI.Page
    {
        private UserSession session;
        private string sipAccount = string.Empty;
        private string allowedRoleName = Enums.GetDescription(Enums.ActiveRoleNames.DepartmentHead);

        private List<Site> UserSites;
        private List<SitesDepartments> UserSitesDepartments;


        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string redirect_to = @"~/ui/dephead/main/dashboard.aspx";
                string url = @"~/ui/session/login.aspx?redirect_to=" + redirect_to;
                Response.Redirect(url);
            }
            else
            {
                session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

                if (session.ActiveRoleName != allowedRoleName)
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=dephead");
                }
            }

            sipAccount = session.NormalUserInfo.SipAccount;
        }

        private void InitUserSitesAndDepartments()
        {
            //Get UserSites
            if (UserSites == null)
            {
                if (session.IsDeveloper)
                    UserSites = SitesDepartments.AllSites;
                else
                    UserSites = Backend.Site.GetUserRoleSites(session.SystemRoles, allowedRoleName);
            }

            //Get UserSitesDepartments
            if (UserSitesDepartments == null)
            {
                if (session.IsDeveloper)
                    UserSitesDepartments = SitesDepartments.GetAllSitesDepartments();
                else
                    UserSitesDepartments = DepartmentHeadRole.GetSiteDepartmentsForHead(sipAccount);
            }
        }


        private List<Users> GetUsers(string departmentName, string siteName)
        {
            //List<Users> users = new List<Users>();
            List<string> columns = new List<string>();
            Dictionary<string, object> whereClause = new Dictionary<string, object>() 
            { 
                { Enums.GetDescription(Enums.Users.SiteName), siteName },
                { Enums.GetDescription(Enums.Users.Department), departmentName }
            };

            return Users.GetUsers(columns, whereClause, 0);
        }


        protected void FilterSitesComboBoxStore_Load(object sender, EventArgs e)
        {
            InitUserSitesAndDepartments();

            FilterSitesComboBox.GetStore().DataSource = UserSites;
            FilterSitesComboBox.GetStore().DataBind();
        }


        protected void FilterDepartmentsBySite_Selected(object sender, DirectEventArgs e)
        {
            //Clear the Departments Filter
            FilterDepartments.Clear();
            FilterDepartments.Disabled = true;

            //Clear the Users filter
            FilterUsersByDepartment.Clear();
            FilterUsersByDepartment.Disabled = true;

            //Clear the phonecalls grid
            ViewPhoneCallsGrid.GetStore().DataSource = new List<PhoneCall>();
            ViewPhoneCallsGrid.DataBind();

            if (FilterSitesComboBox.SelectedItem.Index > -1)
            {
                InitUserSitesAndDepartments();

                FilterDepartments.Disabled = false;

                int siteID = Convert.ToInt32(FilterSitesComboBox.SelectedItem.Value);
                var siteDepartments = UserSitesDepartments.Where(department => department.SiteID == siteID).ToList<SitesDepartments>();

                FilterDepartments.GetStore().DataSource = siteDepartments;
                FilterDepartments.GetStore().DataBind();
            }
        }


        protected void GetUsersPerDepartment(object sender, DirectEventArgs e)
        {
            //Clear the list of user
            FilterUsersByDepartment.Clear();
            FilterUsersByDepartment.ReadOnly = false;

            //Clear the phonecalls grid
            ViewPhoneCallsGrid.GetStore().DataSource = new List<PhoneCall>();
            ViewPhoneCallsGrid.DataBind();

            //Begin fetching the users
            int siteDepartmentID;
            SitesDepartments department;
            
            if (FilterDepartments.SelectedItem.Index > -1)
            {
                siteDepartmentID = Convert.ToInt32(FilterDepartments.SelectedItem.Value);
                department = UserSitesDepartments.Find(dep => dep.ID == siteDepartmentID);

                List<Users> users = GetUsers(department.DepartmentName, department.SiteName);

                FilterUsersByDepartment.Disabled = false;
                FilterUsersByDepartment.GetStore().DataSource = users;
                FilterUsersByDepartment.GetStore().DataBind();

                if (users.Count == 1)
                {
                    FilterUsersByDepartment.SetValueAndFireSelect(users.First().SipAccount);
                    FilterUsersByDepartment.ReadOnly = true;
                }
                else
                {
                    FilterUsersByDepartment.ReadOnly = false;
                }

            }

        }


        protected void GetPhoneCallsForUser(object sender, DirectEventArgs e)
        {
            if (FilterDepartments.SelectedItem != null && !string.IsNullOrEmpty(FilterDepartments.SelectedItem.Value))
            {
                if(FilterUsersByDepartment.SelectedItem != null && !string.IsNullOrEmpty(FilterUsersByDepartment.SelectedItem.Value))
                {
                    string userSipAccount = FilterUsersByDepartment.SelectedItem.Value;
                    List<PhoneCall> phoneCalls = GetUserPhoneCalls(userSipAccount);

                    ViewPhoneCallsGrid.GetStore().DataSource = phoneCalls;
                    ViewPhoneCallsGrid.GetStore().DataBind();
                }
            }
        }


        private List<PhoneCall> GetUserPhoneCalls(string userSipAccount)
        {
            List<PhoneCall> ListOfPhoneCalls = PhoneCall.GetPhoneCalls(userSipAccount)
                                .Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null)
                                .ToList();

            List<PhoneCall> businessCalls = ListOfPhoneCalls.Where(call => call.UI_CallType == "Business").ToList();

            List<PhoneCall> otherCalls = (from phonecall in ListOfPhoneCalls 
                              where phonecall.UI_CallType != "Business" 
                              select phonecall
                              ).Select<PhoneCall, PhoneCall>(e => new PhoneCall{
                                  SourceUserUri = e.SourceUserUri,
                                  SessionIdTime = e.SessionIdTime,
                                  Marker_CallToCountry = e.Marker_CallToCountry,
                                  DestinationNumberUri = "***************",
                                  Duration = e.Duration,
                                  Marker_CallCost = e.Marker_CallCost,
                                  UI_CallType = e.UI_CallType ?? "Unallocated"
                              }).ToList();

            return businessCalls.Concat(otherCalls).ToList<PhoneCall>();
        }

    }

}