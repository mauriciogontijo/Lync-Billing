using Ext.Net;
using Lync_Billing.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lync_Billing.ui.dephead.users
{
    public partial class phonecalls : System.Web.UI.Page
    {
        private Dictionary<string, object> wherePart;
        private List<string> columns;
        private string sipAccount = string.Empty;
        private UserSession session;
        private List<Department> departments;

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

                if (session.ActiveRoleName != "dephead")
                {
                    Response.Redirect("~/ui/session/authenticate.aspx?access=dephead");
                }
            }

            sipAccount = ((UserSession)HttpContext.Current.Session.Contents["UserData"]).EffectiveSipAccount;

            BindDepartmentsForThisUser();
        }


        private void BindDepartmentsForThisUser()
        {
            departments = DepartmentHead.GetDepartmentsForHead(sipAccount);

            if (departments.Count == 1)
            {
                FilterDepartments.SetValueAndFireSelect(departments.First().DepartmentName);
                FilterDepartments.ReadOnly = true;
            }
            else
            {
                FilterDepartments.ReadOnly = false;
                FilterDepartments.GetStore().DataSource = departments;
                FilterDepartments.GetStore().DataBind();
            }
        }


        private List<Users> GetUsers(string departmentName)
        {
            //List<Users> users = new List<Users>();
            List<string> columns = new List<string>();
            Dictionary<string, object> whereClause = new Dictionary<string, object>() 
            { 
                { "AD_Department", departmentName } 
            };

            return Users.GetUsers(columns, whereClause, 0);
        }


        protected void GetUsersPerDepartment(object sender, DirectEventArgs e)
        {
            FilterUsersByDepartment.Clear();
            FilterUsersByDepartment.ReadOnly = false;

            if (FilterDepartments.SelectedItem != null && !string.IsNullOrEmpty(FilterDepartments.SelectedItem.Value))
            {
                List<Users> users = GetUsers(FilterDepartments.SelectedItem.Value);

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
                    List<PhoneCall> phoneCalls = GetUserPhoneCalls(FilterUsersByDepartment.SelectedItem.Value);

                    ViewPhoneCallsGrid.GetStore().DataSource = phoneCalls;
                    ViewPhoneCallsGrid.GetStore().DataBind();

                    FilterPhoneCallsByType.Disabled = false;
                    FilterPhoneCallsByType.ReadOnly = false;
                }
            }
        }

        private List<PhoneCall> GetUserPhoneCalls(string userSipAccount)
        {
            wherePart = new Dictionary<string, object>();
            columns = new List<string>();

            wherePart.Add("SourceUserUri", userSipAccount);
            //wherePart.Add("ac_IsInvoiced", "NO");
            wherePart.Add("marker_CallTypeID", 1);
            wherePart.Add("Exclude", false);

            columns.Add("SourceUserUri");
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

            return PhoneCall.GetPhoneCalls(columns, wherePart, 0).Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null).ToList();
        }

        protected void PhoneCallsHistoryFilter(object sender, DirectEventArgs e)
        {
            PhoneCallsStore.ClearFilter();

            if (FilterPhoneCallsByType.SelectedItem.Value != "Unmarked")
                PhoneCallsStore.Filter("UI_CallType", FilterPhoneCallsByType.SelectedItem.Value);

            PhoneCallsStore.LoadPage(1);
        }

    }
}